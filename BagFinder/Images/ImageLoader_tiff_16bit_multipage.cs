using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace BagFinder.Images
{
    internal class ImageLoaderTiff16BitMultipage : ImageLoader
    {   
        public override int ImCount { get; }
        public override Size ImSize { get; }
        public override string ImagePath(int num)
        {
            //imNum_fileNum * framesInFile + imNum_frameNum;
            var imNum = Math.Max(0, Math.Min(ImCount - 1, num));
            var imNumFileNum = Math.DivRem(imNum, _framesInFile, out var imNumFrameNum);

            return $"{_tiffLoaderList[imNumFileNum].FileName} frame {imNumFrameNum}";
        }

        private readonly int _framesInFile;
        private readonly List<TiffLoader16Bit> _tiffLoaderList = new List<TiffLoader16Bit>();
        private readonly List<int> _imCountList = new List<int>();
        

        public ImageLoaderTiff16BitMultipage(string pathFirtsFile)
        {
            var finfo = new FileInfo(pathFirtsFile);
            if (finfo.Extension != ".tif")
                throw new Exception("Not tiff file");
            var fileNameBase = Path.GetFileNameWithoutExtension(finfo.Name);
            if (fileNameBase.Contains('@'))
                fileNameBase = fileNameBase.Substring(0, fileNameBase.LastIndexOf('@'));
            var filePaths = Directory.GetFiles(finfo.Directory.FullName, fileNameBase + "*.tif", SearchOption.TopDirectoryOnly);
            if (filePaths.Length == 0)
                throw new Exception("No files");
            Array.Sort(filePaths); //(x,y) => String.Compare(x.Name, y.Name)
            foreach (var fi in filePaths)
            {
                var tl = new TiffLoader16Bit(fi);
                _tiffLoaderList.Add(tl); 
                _imCountList.Add(tl.Count);
            }            
            ImSize = new Size(_tiffLoaderList[0].Width, _tiffLoaderList[0].Height);

            //проверка что в каждом файле кроме полседнего одинаковое количество кадров
            if (_imCountList.Count > 1)            
                if (_imCountList.GetRange(0, _imCountList.Count - 1).Any(x => x != _imCountList.First()))
                    throw new Exception("different frame numbers");
            _framesInFile = _imCountList.First();
            ImCount = _imCountList.Sum();            
        }        
        public override Bitmap GetImage_Bitmap(int num) //тут приходится делать конвесию цветов по автоматическому рейнжду
        {
            var imNum = Math.Max(0, Math.Min(ImCount - 1, num));
            var imNumFileNum = Math.DivRem(imNum, _framesInFile, out var imNumFrameNum);
            var m = _tiffLoaderList[imNumFileNum].GetImage_mat(imNumFrameNum);
            //double min = 1000;
            //double max = 4000;
            CvInvoke.MinMaxIdx(m, out var min, out var max, null, null);
            var adjMap = new Mat(m.Rows, m.Cols, DepthType.Cv8U, 4);
            var scale = 255.0 / (max - min);
            m.ConvertTo(adjMap, DepthType.Cv8U, scale, -min * scale);
            return adjMap.Bitmap;
        }
        public override Mat GetImage_Mat(int num)
        {
            var imNum = Math.Max(0, Math.Min(ImCount - 1, num));
            var imNumFileNum = Math.DivRem(imNum, _framesInFile, out var imNumFrameNum);
            var m = _tiffLoaderList[imNumFileNum].GetImage_mat(imNumFrameNum);
            return m;
        }
    }
}