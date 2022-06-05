using System;
using System.Drawing;
using System.IO;
using BagFinder.Images;
using BagFinder.Markers;

namespace BagFinder.Main
{
    internal class Record : IDisposable
    {
        public ImageLoader Il;
        public ImageProcessor Ip;
        public ImageCacher Ic;
        public MarkerList MarkersList;
        public RecordSettings RecordSettings;
        private readonly string _settingsPath;
        
        public enum RecordType { ImagesFolder, VideoFile, Tiff16BitMultipage, Empty };

        public Record(string recordPath, RecordType recordType) //если что-то не получится, надо отлавливать эксепшены
        {            
            _settingsPath = recordPath + ".xml";
            try { RecordSettings = RecordSettings.Load(_settingsPath); }
            catch { RecordSettings = new RecordSettings(); }

            RecordSettings.ImageProcessorSettingsChanged += ImageProcessorSettings_changed;

            MarkersList = new MarkerList
            {
                FilePath = recordPath + ".dat"
            };
            if (MarkersList.FileExists)
                MarkersList.Load(); 
            

            switch (recordType)
            {
                case RecordType.ImagesFolder:
                    string[] pathFiles;
                    var pathFilesJpg = Directory.GetFiles(recordPath, "*.jpg");
                    var pathFilesTif = Directory.GetFiles(recordPath, "*.tif");
                    var pathFilesPng = Directory.GetFiles(recordPath, "*.png");
                    var pathFilesBmp = Directory.GetFiles(recordPath, "*.bmp");
                    if (pathFilesJpg.Length != 0)
                        pathFiles = pathFilesJpg;
                    else if (pathFilesTif.Length != 0)
                        pathFiles = pathFilesTif;
                    else if (pathFilesPng.Length != 0)
                        pathFiles = pathFilesPng;
                    else if (pathFilesBmp.Length != 0)
                        pathFiles = pathFilesBmp;
                    else
                        throw new Exception("Empty folder");
                    Il = new ImageLoaderImages(pathFiles);
                    Ip = new ImageProcessor(8);
                    break;
                case RecordType.VideoFile:
                    Il = new ImageLoaderAvi(recordPath);
                    Ip = new ImageProcessor(8);
                    break;
                case RecordType.Tiff16BitMultipage:
                    Il = new ImageLoaderTiff16BitMultipage(recordPath);
                    Ip = new ImageProcessor(16);
                    break;
                case RecordType.Empty:
                    break;
            }
            Ic = new ImageCacher();
            ImageProcessorSettings_changed(this, new EventArgs());
        }
        private void ImageProcessorSettings_changed(object sender, EventArgs e)
        {
            Ip.Rotate = RecordSettings.Rotate;
            Ip.Invert = RecordSettings.Invert;
            Ip.Dolevels = RecordSettings.Dolevels;
            Ip.Level1 = RecordSettings.Level1;
            Ip.Level2 = RecordSettings.Level2;

            Ic.Clear();
        }

        public void SaveSettings()
        { RecordSettings.Save(_settingsPath); }
        public void Dispose()
        {
            Il?.Dispose();
        }
        public static RecordType GetRecordTypeFromPath(string recordPath)
        {
            try
            {
                var attr = File.GetAttributes(recordPath);
                //detect whether its a directory or file
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    return RecordType.ImagesFolder;
                else
                {
                    if (Path.GetExtension(recordPath) == ".tif")
                        return RecordType.Tiff16BitMultipage;
                    else
                        return RecordType.VideoFile;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RecordType.Empty;
            }
            
        }
        public Bitmap GetFrame_bitmap(int num)
        {
            if (Ic.HasImage(num))
                return Ic.GetImage(num);
            else
            {
                Bitmap b;
                using (var im = Il.GetImage_Mat(num))
                    b = Ip.ProcessToBitmap(im);
                Ic.CacheImage(num, b);
                _imSize = b.Size;
                return b;
            }
        }
        public void AutoLevels()
        {
            Ip.AutoLevels(Il.GetImage_Mat(Program.Rewinder.ImNum));
        }
        public Size ImSize => _imSize;
        private Size _imSize;
    }
}
