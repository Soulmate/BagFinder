using System;
using System.Collections.Generic;
using System.Drawing;
using BagFinder.Main;

namespace BagFinder.Markers
{
    internal class MarkerPoint : Marker
    {
        public PointF P;
        public int? F;

        public MarkerPoint()
        {
            TypeText = "point";
            HpList = new List<HandlePoint> {new HandlePoint(this, 1)};
        }
        public override PointF GetP(int num)
        {
            return P;
        }

        public override int? GetF(int num)
        {
            return F;
        }
        public override void SetP(int num, PointF p)
        {
            if (P != p)
            {
                P = p;
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

        public MarkerPoint(string s) : this()
        {
            char[] sep = { '\t' };
            var ss = s.Split(sep, StringSplitOptions.None);
            if (ss.Length != 5)
                throw new Exception($"Ошибка чтения строки файлма маркеров: {s}");
            Comment = ss[1];
            F = ss[2].ToNullableInt();
            P = new PointF(float.Parse(ss[3]), float.Parse(ss[4]));            
        }
        
        public override string ConvertToString()
        {
            var s = $"{TypeText}\t{Comment}\t{F}\t{P.X}\t{P.Y}";
            return s;
        }
                
        public override int? FrameStart => F;
        public override int? FrameEnd => F;
        
        public override void Paint(Graphics g, CoordnateTransfer ct, int frameNum)
        {
            const int crossSize = 7;

            //ОСНОВНОЕ если попадаем кадром на маркер
            if (frameNum == F)
            {

                var pen1 = new Pen(Program.ProgramSettings.MarkerColors["point_pen"]); //основной цвет
                pen1.Width = pen1.Width = Program.Record.MarkersList.SelectionIsSelected(this) ? 2 : 1; //если выбран
                var p11Wc = ct.Ic2Wcf(P);
                //основные линии
                g.DrawLine(pen1, p11Wc.X, p11Wc.Y - crossSize, p11Wc.X, p11Wc.Y + crossSize);
                g.DrawLine(pen1, p11Wc.X - crossSize, p11Wc.Y, p11Wc.X + crossSize, p11Wc.Y);
            }

            // призраки
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
        }

        public override bool AllPointsDefined()
        {
            return !P.IsEmpty;
        }
    }
}
