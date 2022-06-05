using System;
using System.Drawing;

namespace BagFinder.Main
{
    internal class CoordnateTransfer
    {
        /// <summary>
        /// преобразования
        /// </summary>
        //пикчабокс = картинка * icm + ico;
        public Point Ico = new Point(0, 0); // image origin position
        public double Icm = 1; //magnify image ratio

        public PointF Ic2Wcf(PointF p)  //из координатной систмы картинки в координатную систему пикчабокса
        {
            if (p.IsEmpty)
                return new Point();
            if (Program.Record.RecordSettings.Rotate)
            {
                var tempx = p.X;
                p.X = p.Y;
                p.Y = -tempx + Program.Record.Il.ImSize.Width; //TODO до преобразования?!
            }
            var x = (p.X * (float)Icm + Ico.X);
            var y = (p.Y * (float)Icm + Ico.Y);
            return new PointF(x, y);
        }
        public PointF Wc2Ic(PointF p) //из координатной систмы пикчабокса в координатную систему картинки 
        {
            if (p.IsEmpty)
                return new PointF();
            var x = (p.X - Ico.X) / Icm;
            var y = (p.Y - Ico.Y) / Icm;
            if (Program.Record.RecordSettings.Rotate)
            {
                var tempx = x;
                x = -y + Program.Record.Il.ImSize.Width;
                y = tempx;
            }
            return new PointF((float)x, (float)y);
        }
        public Rectangle GetImageRectangle()
        {
            return new Rectangle(Ico.X, Ico.Y,
                (int)Math.Round(Program.Record.ImSize.Width * Icm),
                (int)Math.Round(Program.Record.ImSize.Height * Icm));
        }
        public void ZoomFit(Size size)
        {
            Ico = new Point(0, 0);
            Icm = Math.Min((double)size.Width / Program.Record.ImSize.Width, (double)size.Height / Program.Record.ImSize.Height);
        }
        public void ZoomToCorners(ref PointF zoomP1, ref PointF zoomP2, Size size) //зум по координатам углов, возвращает коориданты углов после зума
        {
            //левый верхний и правый нижний
            var p1 = new PointF(Math.Min(zoomP1.X, zoomP2.X), Math.Min(zoomP1.Y, zoomP2.Y));
            var p2 = new PointF(Math.Max(zoomP1.X, zoomP2.X), Math.Max(zoomP1.Y, zoomP2.Y));
            var imP1 = Wc2Ic(p1);
            var imP2 = Wc2Ic(p2);
            double newW = Math.Max(10, imP2.X - imP1.X);
            double newH = Math.Max(10, imP2.Y - imP1.Y);
            Icm = Math.Min(size.Width / newW, size.Height / newH);
            Ico = new Point((int)(-(float)Icm * imP1.X), (int)(-(float)Icm * imP1.Y));

            //чтобы оставить прямоугольник:
            zoomP1 = Ic2Wcf(imP1);
            zoomP2 = Ic2Wcf(imP2);
        }
        public void Reset()
        {
            Ico = new Point(0, 0); // image origin position
            Icm = 1; //magnify image ratio
        }
    }
}
