using System;
using System.Drawing;

namespace BagFinder.Markers
{
    internal class HandlePoint
    {
        public PointF P
        {
            get => Marker.GetP(Num);
            set => Marker.SetP(Num, value);
        }
        public int? F
        {
            get => Marker.GetF(Num);
            set => Marker.SetF(Num, value);
        }
        public int Num;
        public Marker Marker;

        public HandlePoint(Marker marker, int num)
        {
            Marker = marker;
            Num = num;
        }

        public bool PointIsInsideHandleRect(PointF p)
        {
            if (P.IsEmpty) return false;
            double minDist = BagFinder.Main.Program.ProgramSettings.HandleRectSize / 2.0;
            return (Math.Abs(P.X - p.X) < minDist && Math.Abs(P.Y - p.Y) < minDist);
        }
    }
}
