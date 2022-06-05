//всегда отсортрован во времени


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BagFinder.Main;

namespace BagFinder.Markers
{
    internal class MarkerTrack : Marker
    {
        public class PF : IComparable<PF>
        {
            public PointF P;
            public int F;

            public int CompareTo(PF other)
            {
                return this.F - other.F;
            }
        }

        public List<PF> PF_arr = new List<PF>();

        public MarkerTrack()
        {
            TypeText = "track";
            HpList = new List<HandlePoint>();
            /*for (var i = 0; i <= 4; i++)
            {
                HpList.Add(new HandlePoint(this, i));
            }*/
        }
        public override PointF GetP(int num)
        {
            return PF_arr[num].P;
        }

        public override int? GetF(int num)
        {
            return PF_arr[num].F;
        }
        public override void SetP(int num, PointF p)
        {
            if (PF_arr[num].P != p)
            {
                PF_arr[num].P = p;
                OnChanged(new MakerIsChangedArgs(false, true));
            }
        }

        public override void SetF(int num, int? f)
        {
            if (PF_arr[num].F != f && f.HasValue)
            {
                PF_arr[num].F = f.Value;
                //сортируем
                PF_arr.Sort();
                OnChanged(new MakerIsChangedArgs(true, false));
            }
        }

        public void AddPF(int f, PointF p)
        {
            PF_arr.Add(
                new PF
                {
                    F = f,
                    P = p
                }
                );
            PF_arr.Sort();
            HpList.Add(new HandlePoint(this, HpList.Count));
            OnChanged(new MakerIsChangedArgs(true, true));
        }

        public void RemovePF(int num)
        {
            PF_arr.RemoveAt(num);
            OnChanged(new MakerIsChangedArgs(true, true));
            HpList.RemoveAt(HpList.Count-1);
        }

        public MarkerTrack(string s) : this()
        {
            char[] sep = { '\t' };
            var ss = s.Split(sep, StringSplitOptions.None);
            if ( ss.Length < 5 )
                throw new Exception($"Ошибка чтения строки файлма маркеров: {s}");

            Comment = ss[1];
            for (var pointsI = 2; pointsI < ss.Length; pointsI += 3)
            {
                var s_f  = ss[pointsI];
                var s_px = ss[pointsI + 1];
                var s_py = ss[pointsI + 2];
                s_f  = s_f.Replace('.', ',');
                s_px = s_px.Replace('.', ',');
                s_py = s_py.Replace('.', ',');
                PF_arr.Add(
                new PF
                {
                    F = int.Parse(s_f),
                    P = new PointF(float.Parse(s_px), float.Parse(s_py))
                }
                );                
            }
            for (int i = 0; i< PF_arr.Count; i++)
                HpList.Add(new HandlePoint(this, i));            
            PF_arr.Sort();
        }

        public override string ConvertToString()
        {
            var s = $"{TypeText}\t{Comment}";
            return PF_arr.Aggregate(s, (current, t) => current + $"\t{t.F}\t{t.P.X}\t{t.P.Y}");
        }

        public override int? FrameStart
        {
            get
            {
                if (PF_arr.Count == 0)
                    return null;
                else
                    return PF_arr[0].F;                
            }
        }
        public override int? FrameEnd
        {
            get
            {
                if (PF_arr.Count == 0)
                    return null;
                else
                    return PF_arr[ PF_arr.Count - 1 ].F;
            }
        }

        public override void Paint(Graphics g, CoordnateTransfer ct, int frameNum)
        {
            //ОСНОВНОЕ если попадаем кадром на маркер
            if (frameNum >= FrameStart && frameNum <= FrameEnd)
            {
                var pen1 = new Pen(Program.ProgramSettings.MarkerColors["track_pen1"]); //основной цвет
                var pen2 = new Pen(Program.ProgramSettings.MarkerColors["track_pen2"]); //цвет выделения текущего кадра
                                                                                        //если выбран    
                pen1.Width = pen1.Width = Program.Record.MarkersList.SelectionIsSelected(this) ? 2 : 1;
                pen2.Width = pen2.Width = Program.Record.MarkersList.SelectionIsSelected(this) ? 2 : 1;

                for (int i = 0; i < PF_arr.Count - 2; i++)
                {
                    var p1 = ct.Ic2Wcf(PF_arr[i].P);
                    var p2 = ct.Ic2Wcf(PF_arr[i + 1].P);
                    var f1 = PF_arr[i].F;
                    var f2 = PF_arr[i + 1].F;

                    g.DrawLine(pen1, p1, p2);

                    if (frameNum == f1)
                    {
                        g.DrawLine(pen1, p1.X, p1.Y - 6, p1.X, p1.Y + 6);
                        g.DrawLine(pen1, p1.X - 6, p1.Y, p1.X + 6, p1.Y);
                    }
                    if (frameNum == f2)
                    {
                        g.DrawLine(pen1, p2.X, p2.Y - 6, p2.X, p2.Y + 6);
                        g.DrawLine(pen1, p2.X - 6, p2.Y, p2.X + 6, p2.Y);
                    }
                    if (frameNum > f1 && frameNum < f2)
                    {
                        var part = (frameNum - f1) / (double)(f2 - f1);                        
                        int x = (int)(p1.X + (p2.X - p1.X) * part);
                        int y = (int)(p1.Y + (p2.Y - p1.Y) * part);
                        g.DrawEllipse(pen1, x - 3, y - 3, 6, 6);
                        //g.DrawLine(pen1, x, y - 3, x, y + 3);
                        //g.DrawLine(pen1, x - 3, y, x + 3, y);
                    }
                }
            }

            else
            {
                var penPhantom = GetPhantomPen(frameNum);
                if (penPhantom != null)
                {
                    for (int i = 0; i < PF_arr.Count - 2; i++)
                    {
                        var p1 = ct.Ic2Wcf(PF_arr[i].P);
                        var p2 = ct.Ic2Wcf(PF_arr[i + 1].P);
                        g.DrawLine(penPhantom, p1, p2);
                    }                    
                }
            }
        }

        public override bool AllPointsDefined()
        {
            return true;
            //return Points.All(p => !p.IsEmpty);
        }
    }
}
//TODO! рисовать линию если только первый кадр определен всегда