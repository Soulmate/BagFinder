using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BagFinder.Main;

namespace BagFinder.Markers
{
    internal class MarkerBag3 : Marker
    {
        public PointF[] Points = new PointF[3];
        public int? F;

        public MarkerBag3()
        {
            TypeText = "bag3";
            HpList = new List<HandlePoint>();            
            for(var i = 0; i <= 2; i++)
            {                
                HpList.Add( new HandlePoint(this, i) );
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
                OnChanged(new MakerIsChangedArgs(false,true));
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
        
        public MarkerBag3(string s) : this()
        {
            char[] sep = { '\t' };
            var ss = s.Split(sep, StringSplitOptions.None);
            if (ss.Length != 9)
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
                var pen1 = new Pen(Program.ProgramSettings.MarkerColors["bag3_pen1"]); //основной цвет
                var pen2 = new Pen(Program.ProgramSettings.MarkerColors["bag3_pen2"]);//цвет выделения текущего кадра (кружки и основы если кадр совпадает)     

                //если выбран    
                pen1.Width = pen1.Width = Program.Record.MarkersList.SelectionIsSelected(this) ? 2 : 1;
                pen2.Width = pen2.Width = Program.Record.MarkersList.SelectionIsSelected(this) ? 2 : 1;

                var p21Wc = ct.Ic2Wcf(Points[0]);
                var p22Wc = ct.Ic2Wcf(Points[1]);
                var p23Wc = ct.Ic2Wcf(Points[2]);

                if (!p21Wc.IsEmpty && !p22Wc.IsEmpty) //конец
                {
                    var p = pen2;
                    g.DrawLine(p, p21Wc, p22Wc);
                }
                if (!p23Wc.IsEmpty) //крестик на пленке
                {
                    g.DrawLine(pen1, p23Wc.X, p23Wc.Y - 6, p23Wc.X, p23Wc.Y + 6);
                    g.DrawLine(pen1, p23Wc.X - 6, p23Wc.Y, p23Wc.X + 6, p23Wc.Y);
                }
                if (!p21Wc.IsEmpty && !p22Wc.IsEmpty && !p23Wc.IsEmpty) //дуга пленки
                {
                    var p = pen2;
                    g.DrawCurve(p, new[] { p21Wc, p23Wc, p22Wc }, (float)0.8);
                }
                if (!p21Wc.IsEmpty && !p22Wc.IsEmpty) // стороны и кружки на них
                {
                    int x, y;
                    x = (int)(p21Wc.X); y = (int)(p21Wc.Y); g.DrawEllipse(pen2, x - 3, y - 3, 6, 6);
                    x = (int)(p22Wc.X); y = (int)(p22Wc.Y); g.DrawEllipse(pen2, x - 3, y - 3, 6, 6);
                }
            }

            // призраки            
            else
            {
                var penPhantom = GetPhantomPen(frameNum);
                if (penPhantom != null)
                {                    
                    var p21Wc = ct.Ic2Wcf(Points[0]);
                    var p22Wc = ct.Ic2Wcf(Points[1]);
                    var p23Wc = ct.Ic2Wcf(Points[2]);
                    g.DrawLine(penPhantom, p21Wc, p22Wc);
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
