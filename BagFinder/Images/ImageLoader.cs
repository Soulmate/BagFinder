using System;
using System.Drawing;
using Emgu.CV;

namespace BagFinder.Images
{
    internal abstract class ImageLoader: IDisposable
    {
        public abstract Mat GetImage_Mat(int num);
        public abstract Bitmap GetImage_Bitmap(int num);
        public abstract string ImagePath(int num);
        public abstract int ImCount { get; }
        public abstract Size ImSize { get; }
        public virtual void Dispose() { }
    }
}
