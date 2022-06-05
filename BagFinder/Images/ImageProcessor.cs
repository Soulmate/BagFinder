using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace BagFinder.Images
{
    internal class ImageProcessor
    {
        public ImageProcessor(int bittness) //TODO костыль
        {
            Bittness = bittness;
            switch (bittness)
            {
                case 8:
                    LevelMin = 0;
                    LevelMax = 255;
                    break;
                case 16:
                    LevelMin = 0;
                    LevelMax = 65536;
                    break;
                default:
                    throw new Exception($"Unnown bitness: {bittness}");
            }
        }
        public int Level1
        {
            get => _level1;
            set => _level1 = Math.Max(Math.Min(value, LevelMax), LevelMin);
        }
        public int Level2
        {
            get => _level2;
            set => _level2 = Math.Max(Math.Min(value, LevelMax), LevelMin);
        }
        public bool Rotate;
        public bool Invert;
        public bool Dolevels;
        public int LevelMax, LevelMin;
        public int Bittness;
        private int _level1, _level2;


        public Bitmap ProcessToBitmap(Mat initialImage)
        {
            Bitmap result;
            using (var processedImage = Process(initialImage))
                result = new Bitmap(processedImage.Bitmap);
            return result;
        }

        public Mat Process(Mat initialImage)
        {            
            if (!Rotate && !Invert && !Dolevels)
            {
                return initialImage;
            }
            
            if (Bittness == 8)
            {
                var im = initialImage.ToImage<Gray, Byte>();
                if (Rotate) //через флип и транспонз, поворот вправо на 90 градусов
                {
                    using (var imTemp = new Image<Gray, Byte>(initialImage.Width, initialImage.Height))
                    {
                        CvInvoke.Transpose(im, imTemp);
                        CvInvoke.Flip(imTemp, im, FlipType.Vertical);
                    }
                }
                if (Invert)
                {
                    im = im.Not();
                }
                var processedImage = im.Mat;
                if (Dolevels)
                {
                    var adjMap = new Mat(processedImage.Rows, processedImage.Cols, DepthType.Cv8U, 4);
                    var scale = 255.0 / (Level2 - Level1);
                    processedImage.ConvertTo(adjMap, DepthType.Cv8U, scale, -Level1 * scale);
                    //Image<Gray, Single> img4 = img1.Convert<Single>(delegate (Byte b) { return (Single)Math.cos(b * b / 255.0); });
                    processedImage = adjMap;
                }

                return processedImage;
            }
            else
            {
                var im = initialImage.ToImage<Gray, UInt16>();
                if (Rotate) //через флип и транспонз, поворот вправо на 90 градусов
                {
                    using (var imTemp = new Image<Gray, UInt16>(initialImage.Width, initialImage.Height))
                    {
                        CvInvoke.Transpose(im, imTemp);
                        CvInvoke.Flip(imTemp, im, FlipType.Horizontal);
                    }
                }
                if (Invert)
                {
                    im = im.Not();
                }
                var processedImage = im.Mat;
                if (!Dolevels) return processedImage;
                var adjMap = new Mat(processedImage.Rows, processedImage.Cols, DepthType.Cv8U, 4);                    
                var scale = 255.0 / (_level2 - _level1);
                processedImage.ConvertTo(adjMap, DepthType.Cv8U, scale, -_level1 * scale);
                //Image<Gray, Single> img4 = img1.Convert<Single>(delegate (Byte b) { return (Single)Math.cos(b * b / 255.0); });
                processedImage = adjMap;
                return processedImage;
            }                        
        }
        public void AutoLevels(Mat image)
        {
            CvInvoke.MinMaxIdx(image, out var min, out var max, null, null);
            Level1 = (int)min;
            Level2 = (int)max;
        }
    }
}
