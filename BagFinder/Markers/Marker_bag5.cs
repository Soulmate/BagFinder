using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BagFinder.Main;

namespace BagFinder.Markers
{
    internal class MarkerBag5 : Marker
    {
        public PointF[] Points = new PointF[5];
        public int? F1, F2;

        public MarkerBag5()
        {
            TypeText = "bag5";
            HpList = new List<HandlePoint>();
            for (var i = 0; i <= 4; i++)
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
            return (num < 2) ? F1 : F2;
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
            if (num < 2)
            {
                if (F1 != f)
                {
                    F1 = f;
                    if (F1.HasValue && F2.HasValue)
                        F2 = Math.Max(F1.Value, F2.Value); //не даст сдвинуть меньший кадр больше большего
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

        public MarkerBag5(string s) : this()
        {
            char[] sep = { '\t' };
            var ss = s.Split(sep, StringSplitOptions.None);
            if (ss.Length != 14)
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
        public override int? FrameEnd   => F2 ?? F1;
        
        public override void Paint(Graphics g, CoordnateTransfer ct, int frameNum)
        {
            //ОСНОВНОЕ если попадаем кадром на маркер
            if (frameNum >= F1 && frameNum <= F2)
            {
                var pen1 = new Pen(Program.ProgramSettings.MarkerColors["bag5_pen1"]); //основной цвет
                var pen2 = new Pen(Program.ProgramSettings.MarkerColors["bag5_pen2"]);//цвет выделения текущего кадра (кружки и основы если кадр совпадает)     
                                                                                      //если выбран    
                pen1.Width = pen1.Width = Program.Record.MarkersList.SelectionIsSelected(this) ? 2 : 1;
                pen2.Width = pen2.Width = Program.Record.MarkersList.SelectionIsSelected(this) ? 2 : 1;

                var p11Wc = ct.Ic2Wcf(Points[0]);
                var p12Wc = ct.Ic2Wcf(Points[1]);
                var p21Wc = ct.Ic2Wcf(Points[2]);
                var p22Wc = ct.Ic2Wcf(Points[3]);
                var p23Wc = ct.Ic2Wcf(Points[4]);

                if (!p11Wc.IsEmpty && !p12Wc.IsEmpty) //начало
                {
                    var p = (frameNum == F1) ? pen2 : pen1;
                    g.DrawLine(p, p11Wc, p12Wc);
                }
                if (!p21Wc.IsEmpty && !p22Wc.IsEmpty) //конец
                {
                    var p = (frameNum == F2) ? pen2 : pen1;
                    g.DrawLine(p, p21Wc, p22Wc);
                }
                if (!p23Wc.IsEmpty) //крестик на пленке
                {
                    g.DrawLine(pen1, p23Wc.X, p23Wc.Y - 6, p23Wc.X, p23Wc.Y + 6);
                    g.DrawLine(pen1, p23Wc.X - 6, p23Wc.Y, p23Wc.X + 6, p23Wc.Y);
                }
                if (!p21Wc.IsEmpty && !p22Wc.IsEmpty && !p23Wc.IsEmpty) //дуга пленки
                {
                    var p = (frameNum == F2) ? pen2 : pen1;
                    g.DrawCurve(p, new[] { p21Wc, p23Wc, p22Wc }, (float)0.8);
                }
                if (!p11Wc.IsEmpty && !p12Wc.IsEmpty && !p21Wc.IsEmpty && !p22Wc.IsEmpty) // стороны и кружки на них
                {
                    g.DrawLine(pen1, p11Wc, p21Wc);
                    g.DrawLine(pen1, p12Wc, p22Wc);
                    if (F2.Value == F1.Value) //все 4 кружка рисуем
                    {
                        int x, y;
                        x = (int)(p11Wc.X); y = (int)(p11Wc.Y); g.DrawEllipse(pen2, x - 3, y - 3, 6, 6);
                        x = (int)(p12Wc.X); y = (int)(p12Wc.Y); g.DrawEllipse(pen2, x - 3, y - 3, 6, 6);
                        x = (int)(p21Wc.X); y = (int)(p21Wc.Y); g.DrawEllipse(pen2, x - 3, y - 3, 6, 6);
                        x = (int)(p22Wc.X); y = (int)(p22Wc.Y); g.DrawEllipse(pen2, x - 3, y - 3, 6, 6);
                    }
                    else
                    {
                        var part = (F2.Value == F1.Value) ? 0 : (frameNum - F1.Value) / (double)(F2.Value - F1.Value);
                        if (part > 0 && part < 1)
                        {
                            int x, y;
                            x = (int)(p11Wc.X + (p21Wc.X - p11Wc.X) * part);
                            y = (int)(p11Wc.Y + (p21Wc.Y - p11Wc.Y) * part);
                            g.DrawEllipse(pen2, x - 3, y - 3, 6, 6);
                            x = (int)(p12Wc.X + (p22Wc.X - p12Wc.X) * part);
                            y = (int)(p12Wc.Y + (p22Wc.Y - p12Wc.Y) * part);
                            g.DrawEllipse(pen2, x - 3, y - 3, 6, 6);
                        }
                    }
                }

            }
            else if (F1.HasValue && !F2.HasValue) //при создании
            {
                var p11Wc = ct.Ic2Wcf(Points[0]);
                var p12Wc = ct.Ic2Wcf(Points[1]);
                if (!p11Wc.IsEmpty && !p12Wc.IsEmpty) //начало
                {
                    var p = new Pen(Program.ProgramSettings.MarkerColors["bag5_pen1"]); //основной цвет
                    g.DrawLine(p, p11Wc, p12Wc);
                }
            }
            else if (F1.HasValue && F2.HasValue &&
                     !Points[0].IsEmpty && !Points[1].IsEmpty && !Points[2].IsEmpty && !Points[3].IsEmpty && Points[4].IsEmpty) //при создании, осталась только последняя точка
            {
                var p11Wc = ct.Ic2Wcf(Points[0]);
                var p12Wc = ct.Ic2Wcf(Points[1]);
                var p21Wc = ct.Ic2Wcf(Points[2]);
                var p22Wc = ct.Ic2Wcf(Points[3]);
                var p = new Pen(Program.ProgramSettings.MarkerColors["bag5_pen1"]); //основной цвет
                g.DrawLine(p, p11Wc, p12Wc);
                g.DrawLine(p, p21Wc, p22Wc);
            }
            else
            {
                var penPhantom = GetPhantomPen(frameNum);
                if (penPhantom != null)
                {
                    var p11Wc = ct.Ic2Wcf(Points[0]);
                    var p12Wc = ct.Ic2Wcf(Points[1]);
                    var p21Wc = ct.Ic2Wcf(Points[2]);
                    var p22Wc = ct.Ic2Wcf(Points[3]);
                    var p23Wc = ct.Ic2Wcf(Points[4]);
                    g.DrawLine(penPhantom, p11Wc, p12Wc);
                    g.DrawLine(penPhantom, p21Wc, p22Wc);
                    g.DrawLine(penPhantom, p11Wc, p21Wc);
                    g.DrawLine(penPhantom, p12Wc, p22Wc);
                    g.DrawCurve(penPhantom, new[] { p21Wc, p23Wc, p22Wc }, (float)0.8);
                }
            }
        }

        public override bool AllPointsDefined()
        {
            return Points.All(p => !p.IsEmpty);
        }
    }
}
//TODO! рисовать линию если только первый кадр определен всегда