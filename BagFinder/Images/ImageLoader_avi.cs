using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace BagFinder.Images
{
    internal sealed class ImageLoaderAvi : ImageLoader
    {      
        public override int ImCount { get; }
        public override Size ImSize { get; }

        public override string ImagePath(int num)
        {
            return _pathFile;
        }

        private readonly VideoCapture _capture;
        private readonly string _pathFile;
        private Mat _currentFrameMat;

        public ImageLoaderAvi(string pathFile)
        {
            _pathFile = pathFile;            
            _capture = new VideoCapture(pathFile);
            ImCount = (int)_capture.GetCaptureProperty(CapProp.FrameCount); 
            ImSize = GetImage_Bitmap(0).Size;
        }        
        public override Bitmap GetImage_Bitmap(int num)
        {            
            GetImage_Mat(num);
            Bitmap b;
            if (_currentFrameMat != null)
                b = _currentFrameMat.Bitmap;
            else
                throw new Exception("Error reading avi file");            
            return b;
        }
        public override Mat GetImage_Mat(int num)
        {
            var imNum = Math.Max(0, Math.Min(ImCount - 1, num));
            _capture.SetCaptureProperty(CapProp.PosFrames, imNum);
            _currentFrameMat = _capture.QueryFrame();
            return _currentFrameMat;
        }
        public override void Dispose()
        {
            _capture.Dispose();
        }
    }
}