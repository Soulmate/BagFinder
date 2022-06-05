using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BagFinder.Main;

namespace BagFinder.Markers
{
    internal class MarkerLine : Marker
    {
        public PointF[] Points = new PointF[2];
        public int? F1, F2;

        public MarkerLine()
        {
            TypeText = "line";
            HpList = new List<HandlePoint>();
            for (var i = 0; i <= 1; i++)
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
            return (num < 1) ? F1 : F2;
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
            if (num < 1)
            {
                if (F1 != f)
                {
                    F1 = f;
                    if (F1.HasValue && F2.HasValue)
                        F2 = Math.Max(F1.Value, F2.Value); //не даст сдвинуть меньший кадр больше большего //TODO а почему бы и нет
                    OnChanged(new MakerIsChangedArgs(true, false));
                }
            }
            else
            {
                if (F2 != f)
                {
                    F2 = f;
                    if (F1.HasValue && F2.HasValue)
                        F1 = Math.Min(F1.Value, F2.Value);
                    OnChanged(new MakerIsChangedArgs(true, false));
                }
            }
        }

        public MarkerLine(string s) : this()
        {
            char[] sep = { '\t' };
            var ss = s.Split(sep, StringSplitOptions.None);
            if (ss.Length != 8)
                throw new Exception($"Ошибка чтения строки файлма маркеров: {s}");

            Comment = ss[1];
            F1 = ss[2].ToNullableInt();
            F2 = ss[3].ToNullableInt();
            for (var pointsI = 0; pointsI < Points.Length; pointsI++)
                Points[pointsI] = new PointF(float.Parse(ss[4 + pointsI * 2]), float.Parse(ss[4 + pointsI * 2 + 1]));
        }

        public override string ConvertToString()
        {
            var s = $"{TypeText}\t{Comment}\t{F1}\t{F2}";
            return Points.Aggregate(s, (current, t) => current + $"\t{t.X}\t{t.Y}");
        }

        public override int? FrameStart => F1 ?? F2;
        public override int? FrameEnd => F2 ?? F1;
        
        public override void Paint(Graphics g, CoordnateTransfer ct, int frameNum)
        {
            const int crossSize = 7;

            //ОСНОВНОЕ если попадаем кадром на маркер
            if (frameNum >= F1 && frameNum <= F2)
            {
                //TODO вынести куданибудь, чтобы не пересоздавать каждый раз
                var pen1 = new Pen(Program.ProgramSettings.MarkerColors["line_pen1"]); //основной цвет 
                var pen2 = new Pen(Program.ProgramSettings.MarkerColors["line_pen2"]);//цвет выделения текущего кадра (кружки и основы если кадр совпадает)     

                pen1.Width = pen1.Width = Program.Record.MarkersList.SelectionIsSelected(this) ? 2 : 1; //если выбран

                var p11Wc = ct.Ic2Wcf(Points[0]);
                var p21Wc = ct.Ic2Wcf(Points[1]);
                if (!p11Wc.IsEmpty)//начало
                {
                    var p = (frameNum == F1) ? pen2 : pen1;
                    g.DrawLine(p, p11Wc.X, p11Wc.Y - crossSize, p11Wc.X, p11Wc.Y + crossSize);
                    g.DrawLine(p, p11Wc.X - crossSize, p11Wc.Y, p11Wc.X + crossSize, p11Wc.Y);
                }
                if (!p21Wc.IsEmpty)//конец
                {
                    var p = (frameNum == F2) ? pen2 : pen1;
                    g.DrawLine(p, p21Wc.X, p21Wc.Y - crossSize, p21Wc.X, p21Wc.Y + crossSize);
                    g.DrawLine(p, p21Wc.X - crossSize, p21Wc.Y, p21Wc.X + crossSize, p21Wc.Y);
                }
                if (!p11Wc.IsEmpty && !p21Wc.IsEmpty)
                {
                    g.DrawLine(pen1, p11Wc, p21Wc);
                    var part = (frameNum - F1.Value) / (double)(F2.Value - F1.Value);
                    if (part >= 0 && part <= 1)
                    {
                        int x, y;
                        x = (int)(p11Wc.X + (p21Wc.X - p11Wc.X) * part);
                        y = (int)(p11Wc.Y + (p21Wc.Y - p11Wc.Y) * part);
                        g.DrawEllipse(pen2, x - 3, y - 3, 6, 6);
                    }
                }
            }
            // призраки
            else
            {
                var penPhantom = GetPhantomPen(frameNum);
                if (penPhantom != null)
                {
                    var p11Wc = ct.Ic2Wcf(Points[0]);
                    var p21Wc = ct.Ic2Wcf(Points[1]);
                    g.DrawLine(penPhantom, p11Wc.X, p11Wc.Y - crossSize, p11Wc.X, p11Wc.Y + crossSize);
                    g.DrawLine(penPhantom, p11Wc.X - crossSize, p11Wc.Y, p11Wc.X + crossSize, p11Wc.Y);
                    g.DrawLine(penPhantom, p21Wc.X, p21Wc.Y - crossSize, p21Wc.X, p21Wc.Y + crossSize);
                    g.DrawLine(penPhantom, p21Wc.X - crossSize, p21Wc.Y, p21Wc.X + crossSize, p21Wc.Y);
                    g.DrawLine(penPhantom, p11Wc, p21Wc);
                }
            }
        }

        public override bool AllPointsDefined()
        {
            return Points.All(p => !p.IsEmpty);
        }
    }
}
