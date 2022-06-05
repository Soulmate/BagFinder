using System;
using System.Collections.Generic;
using System.Drawing;
using BagFinder.Main;

namespace BagFinder.Markers
{
    internal class Marker_area_brush : Marker
    {
        public Bitmap b;
        public int? F;

        public Marker_area_brush()
        {
            TypeText = "area_brush";
            HpList = new List<HandlePoint>();
            var size = Program.Record.ImSize;
            b = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        public override PointF GetP(int num)
        {
            return new PointF(0,0); // вообще не нужна эта точка
        }

        public override int? GetF(int num)
        {
            return F;
        }
        public override void SetP(int num, PointF p)
        {            
        }

        public override void SetF(int num, int? f)
        {
            if (F != f)
            {
                F = f;
                OnChanged(new MakerIsChangedArgs(true, false));
            }
        }

        public Marker_area_brush(string s) // комент ТФБ номер кадра ТАБ путь к файлу с маской (что добавить к папке)
        {            
            char[] sep = { '\t' };
            var ss = s.Split(sep, StringSplitOptions.None);
            if (ss.Length != 4)
                throw new Exception($"Ошибка чтения строки файлма маркеров: {s}");
            Comment = ss[1];
            F = ss[2].ToNullableInt();
            string im_path = ss[3];
            im_path = $"{Program.ProgramSettings.RecordPath}{im_path}";

            // решение с отпускающей загрузкой битмапов из https://stackoverflow.com/questions/4803935/free-file-locked-by-new-bitmapfilepath/8701748#8701748
            using (var bmpTemp = new Bitmap(im_path))
            {
                b = new Bitmap(bmpTemp);
            }

            TypeText = "area_brush";
            HpList = new List<HandlePoint>();
        }
        
        public override string ConvertToString()
        {
            var s = $"{TypeText}\t{Comment}\t{F}\t - area_brush images\\{F}.png";
            return s;
        }
                
        public override int? FrameStart => F;
        public override int? FrameEnd => F;
        
        public override void Paint(Graphics g, CoordnateTransfer ct, int frameNum)
        {
            //ОСНОВНОЕ если попадаем кадром на маркер
            if (frameNum == F)
            {
                g.DrawImage(b, Program.ViewerImage.Ct.GetImageRectangle());
            }

            // призраки
            /*
            else
            {
                var penPhantom = GetPhantomPen(frameNum);
                if (penPhantom != null)
                {
                    var p11Wc = ct.Ic2Wcf(P);
                    g.DrawLine(penPhantom, p11Wc.X, p11Wc.Y - crossSize, p11Wc.X, p11Wc.Y + crossSize);
                    g.DrawLine(penPhantom, p11Wc.X - crossSize, p11Wc.Y, p11Wc.X + crossSize, p11Wc.Y);
                }
            }
            */
        }

        public override bool AllPointsDefined()
        {
            return true;
        }

        internal void SaveBitmap()
        {
            string im_path = $" - area_brush images\\{F}.png";
            im_path = $"{Program.ProgramSettings.RecordPath}{im_path}";
            string dir_path = System.IO.Path.GetDirectoryName(im_path);
            if (!System.IO.Directory.Exists(dir_path))
                System.IO.Directory.CreateDirectory(dir_path);
            if (System.IO.File.Exists(im_path))
            { 
                try
                {
                    System.IO.File.Delete(im_path);
                }
                catch (Exception e)
                {
                    Program.ViewerInfo.BottomText = $"Saving error:{e.Message}";
                }            
            }
            try
            { 
                b?.Save(im_path);
                Program.ViewerInfo.BottomText = $"Saving {im_path}";
            }
            catch (Exception e)
            {
                Program.ViewerInfo.BottomText = $"Saving error:{e.Message}";
            }
        }
    }
}
