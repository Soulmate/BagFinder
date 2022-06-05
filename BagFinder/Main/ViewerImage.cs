using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace BagFinder.Main
{
    internal class ViewerImage
    {
        public CoordnateTransfer Ct = new CoordnateTransfer();

        public PictureBox Pb;
        
        public void Invalidate()
        {
            Pb?.Invalidate();
        }

        public void Reset()
        {
            Ct.Reset();
            Pb?.Invalidate();
        }
        public void ZoomFit()
        {
            if (Pb == null) return;
            Ct.ZoomFit(Pb.Size);
            Pb.Invalidate();
        }

        public void Paint(Graphics g)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor; //значительно ускоряет работу приложения

            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                var record = Program.Record;
                if (record == null) { return; }
                
                var b = record.GetFrame_bitmap(Program.Rewinder.ImNum);
                if (b != null)
                {                    
                    g.DrawImage(b, Ct.GetImageRectangle());
                }
                
                foreach (var marker in record.MarkersList)
                {
                    marker.Paint(g, Ct, Program.Rewinder.ImNum);
                }

                Program.ToolSet.Pb_Paint(g);

                //масштаб
                var scalePos = new PointF(Pb.Width / 2, Pb.Height - 10);

                var scale = Program.Record.RecordSettings.Scale;
                var scaleBarSize = Pb.Width * 0.25; //примерный размер линейки при показе с учетом масштабирования
                var scaleBarSizeMM = scaleBarSize * scale / Ct.Icm; //примерный размер масштабной линейки в мм
                var scaleBarSizeMMR = Program.RoundToNearestDividedBy10(scaleBarSizeMM); //округленный размер масштабной линейки в мм
                var scaleBarSizeR_im = scaleBarSizeMMR / scale;                          //размер масштабной линейки в пикселях на изображении при показе 1 к 1
                var scaleBarSizeR    = scaleBarSizeR_im * Ct.Icm;                          //размер масштабной линейки в пикселях на изображении при показе с учетом масштабирования

                g.DrawLine(Pens.Black,
                    (float)(scalePos.X - scaleBarSizeR / 2.0), (float)scalePos.Y,
                    (float)(scalePos.X + scaleBarSizeR / 2.0), (float)scalePos.Y);
                g.DrawLine(Pens.Black,
                    (float)(scalePos.X - scaleBarSizeR / 2.0), (float)scalePos.Y + 3,
                    (float)(scalePos.X - scaleBarSizeR / 2.0), (float)scalePos.Y - 3);
                g.DrawLine(Pens.Black,
                    (float)(scalePos.X + scaleBarSizeR / 2.0), (float)scalePos.Y + 3,
                    (float)(scalePos.X + scaleBarSizeR / 2.0), (float)scalePos.Y - 3);
                g.DrawString($"{scaleBarSizeMMR} mm",
                    new Font("Arial", 10),
                    Brushes.Black,
                    scalePos, 
                    new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Far
                    }
                    );

                //время
                var timePos = new PointF(Pb.Width - 10, Pb.Height - 10);
                
                var time = (double)( Program.Rewinder.ImNum - Program.Record?.RecordSettings.FrameTimeZero ) / Program.Record.RecordSettings.Fps;
                int numOfDecimals = (int)Math.Round(Math.Log10(Program.Record.RecordSettings.Fps)) + 2; //сколько знаков после запятой отображать
                string timeStr;
                if (numOfDecimals >= 3) 
                    timeStr = string.Format(new NumberFormatInfo() { NumberDecimalDigits = numOfDecimals - 3 }, "{0:F} ms", time * 1000);
                else if (numOfDecimals >= 0)
                    timeStr = string.Format(new NumberFormatInfo() { NumberDecimalDigits = numOfDecimals }, "{0:F} s", time);
                else
                    timeStr = $"{time} s";
                g.DrawString(timeStr,
                    new Font("Arial", 10),
                    Brushes.Black,
                    timePos,
                    new StringFormat
                    {
                        Alignment = StringAlignment.Far,
                        LineAlignment = StringAlignment.Far
                    }
                );
                
                //сетка:
                for (int grid_i = 1; grid_i < Program.ProgramSettings.GridX; grid_i++)
                {
                    float x = (float)Program.Record.Il.ImSize.Width / Program.ProgramSettings.GridX * grid_i;
                    float y1 = 0;
                    float y2 = Program.Record.Il.ImSize.Height;
                    g.DrawLine(Pens.Yellow, Ct.Ic2Wcf(new PointF(x, y1)), Ct.Ic2Wcf(new PointF(x, y2)));
                }

                for (int grid_i = 1; grid_i < Program.ProgramSettings.GridY; grid_i++)
                {
                    float y = (float)Program.Record.Il.ImSize.Height / Program.ProgramSettings.GridY * grid_i;
                    float x1 = 0;
                    float x2 = Program.Record.Il.ImSize.Width;
                    g.DrawLine(Pens.Yellow, Ct.Ic2Wcf(new PointF(x1, y)), Ct.Ic2Wcf(new PointF(x2, y)));
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            
            watch.Stop();
            //Console.WriteLine("image {0} ms", watch.ElapsedMilliseconds);
        }

        public void SaveBitmap(string fileName)
        {
            Bitmap b = new Bitmap(Pb.Size.Width, Pb.Size.Height);
            Paint(Graphics.FromImage(b));
            b.Save(fileName);
        }
    }
}




