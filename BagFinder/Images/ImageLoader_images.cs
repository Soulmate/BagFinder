using System;
using System.Drawing;
using Emgu.CV;

namespace BagFinder.Images
{
    internal sealed class ImageLoaderImages : ImageLoader
    {      
        public override int ImCount => _imCount;
        public override Size ImSize => _imSize;

        public override string ImagePath(int num)
        {
            return _pathFiles[num];
        }

        private readonly int _imCount;
        private Size _imSize;
        private readonly string[] _pathFiles;
        
        public ImageLoaderImages(string[] pathFiles)
        {
            _pathFiles = pathFiles;
            _imCount = pathFiles.Length; 
            _imSize = GetImage_Bitmap(0).Size;
        }        
        public override Bitmap GetImage_Bitmap(int num)
        {
            var imNum = Math.Max(0, Math.Min(_imCount - 1, num));
            var b = (Bitmap)Image.FromFile(_pathFiles[imNum]);
            var imSizeCurrent = b.Size;
            if (_imSize != new Size(0, 0) && _imSize != imSizeCurrent) //если не первый раз вызываем, и в этот раз размер не тот
            {
                _imSize = imSizeCurrent;
                throw new Exception("Size is different");
            }
            return b;
        }
        public override Mat GetImage_Mat(int num)
        {
            var imNum = Math.Max(0, Math.Min(_imCount - 1, num));
            var m = new Mat(_pathFiles[imNum]);
            return m;
        }   
    }
}