using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using BagFinder.Main;

namespace BagFinder.Markers
{
    internal class MarkerCross : Marker
    {
        public PointF[] Points = new PointF[4];
        public int? F;

        public MarkerCross()
        {
            TypeText = "cross";
            HpList = new List<HandlePoint>();
            for (var i = 0; i <= 3; i++)
            {
                HpList.Add(new HandlePoint(this, i));
            }
        }
        public override PointF GetP(int num)
        {
            return Points[num];
        }

        public override int? GetF(int num)
        {
            return F;
        }
        public override void SetP(int num, PointF p)
        {
            if (Points[num] != p)
            {
                Points[num] = p;
                OnChanged(new MakerIsChangedArgs(false, true));
            }
        }

        public override void SetF(int num, int? f)
        {
            if (F != f)
            {
                F = f;
                OnChanged(new MakerIsChangedArgs(true, false));
            }
        }

        public MarkerCross(string s) : this()
        {
            char[] sep = { '\t' };
            var ss = s.Split(sep, StringSplitOptions.None);
            if (ss.Length != 11)
                throw new Exception($"Ошибка чтения строки файлма маркеров: {s}");

            Comment = ss[1];
            F = ss[2].ToNullableInt();
            for (var pointsI = 0; pointsI < Points.Length; pointsI++)
                Points[pointsI] = new PointF(float.Parse(ss[3 + pointsI * 2]), float.Parse(ss[3 + pointsI * 2 + 1]));
        }

        public override string ConvertToString()
        {
            var s = $"{TypeText}\t{Comment}\t{F}";
            return Points.Aggregate(s, (current, t) => current + $"\t{t.X}\t{t.Y}");
        }

        public override int? FrameStart => F;

        public override int? FrameEnd => F;
        
        public override void Paint(Graphics g, CoordnateTransfer ct, int frameNum)
        {
            //ОСНОВНОЕ если попадаем кадром на маркер
            if (frameNum == F)
            {               
                var pen1 = new Pen(Program.ProgramSettings.MarkerColors["cross_pen"]); //основной цвет
                Brush brush = new SolidBrush(Color.FromArgb(Program.ProgramSettings.CrossAlpha, Program.ProgramSettings.MarkerColors["cross_fill"])); //кисть заливки

                //если выбран    
                pen1.Width = Program.Record.MarkersList.SelectionIsSelected(this) ? 2 : 1;

                var p11Wc = ct.Ic2Wcf(Points[0]);
                var p12Wc = ct.Ic2Wcf(Points[1]);
                var p21Wc = ct.Ic2Wcf(Points[2]);
                var p22Wc = ct.Ic2Wcf(Points[3]);

                //основные линии
                if (!p11Wc.IsEmpty && !p12Wc.IsEmpty)
                    g.DrawLine(pen1, p11Wc.X, p11Wc.Y, p12Wc.X, p12Wc.Y);
                if (!p21Wc.IsEmpty && !p22Wc.IsEmpty)
                    g.DrawLine(pen1, p21Wc.X, p21Wc.Y, p22Wc.X, p22Wc.Y);

                //кривуля
                if (!p11Wc.IsEmpty && !p12Wc.IsEmpty && !p21Wc.IsEmpty && !p22Wc.IsEmpty)
                {
                    g.DrawClosedCurve(pen1,
                        new[] { p11Wc, p21Wc, p12Wc, p22Wc },
                        (float)0.8,
                        FillMode.Winding);

                    g.FillClosedCurve(brush,
                    new[] { p11Wc, p21Wc, p12Wc, p22Wc },
                    FillMode.Winding,
                    (float)0.8);
                }
            }

            // призраки
            else
            {
                var penPhantom = GetPhantomPen(frameNum);
                if (penPhantom != null)
                {
                    var p11Wc = ct.Ic2Wcf(Points[0]);
                    var p12Wc = ct.Ic2Wcf(Points[1]);
                    var p21Wc = ct.Ic2Wcf(Points[2]);
                    var p22Wc = ct.Ic2Wcf(Points[3]);
                    //основные линии
                    if (!p11Wc.IsEmpty && !p12Wc.IsEmpty)
                        g.DrawLine(penPhantom, p11Wc.X, p11Wc.Y, p12Wc.X, p12Wc.Y);
                    if (!p21Wc.IsEmpty && !p22Wc.IsEmpty)
                        g.DrawLine(penPhantom, p21Wc.X, p21Wc.Y, p22Wc.X, p22Wc.Y);

                    //кривуля
                    if (!p11Wc.IsEmpty && !p12Wc.IsEmpty && !p21Wc.IsEmpty && !p22Wc.IsEmpty)
                    {
                        g.DrawClosedCurve(penPhantom,
                            new[] { p11Wc, p21Wc, p12Wc, p22Wc },
                            (float)0.8,
                            FillMode.Winding);
                    }
                }
            }

            
            //double rad = Math.Sqrt(
            //    Math.Pow(Math.Abs(p11WC.X - p12WC.X), 2) +
            //    Math.Pow(Math.Abs(p11WC.Y - p12WC.Y), 2));

            

        }

        public override bool AllPointsDefined()
        {
            return Points.All(p => !p.IsEmpty);
        }
    }
}
