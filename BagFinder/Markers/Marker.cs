using System;
using System.Collections.Generic;
using System.Drawing;
using BagFinder.Main;

namespace BagFinder.Markers
{
    internal abstract class Marker:IComparable
    {   
        public abstract int? FrameStart { get; }
        public abstract int? FrameEnd { get; }


        public string Comment
        {
            get => _comment;
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnChanged(new MakerIsChangedArgs(false, false, true));
                }
            }
        }

        private string _comment;

        public string TypeText;
        
        public event EventHandler Changed;
        protected void OnChanged(MakerIsChangedArgs e)
        {
            Changed?.Invoke(this, e);
            //Console.WriteLine(this.ConvertToString());
        }

        public List<HandlePoint> HpList;
        public HandlePoint GetNearestHandlePointAtThisFrame(PointF p, int frame)
        {
            return HpList.Find(hp => frame == hp.F && hp.PointIsInsideHandleRect(p));
        }
        public HandlePoint GetNearestVisibleHandlePoint(PointF p, int frame)
        {
            if (frame < FrameStart - Program.ProgramSettings.PhantomFrames ||
                frame > FrameEnd   + Program.ProgramSettings.PhantomFrames) return null;
            return HpList.Find(hp => hp.PointIsInsideHandleRect(p));
        }
        public List<HandlePoint> GetHandlePointsAtFrame(int frame)
        {
            return HpList.FindAll(hp => frame == hp.F);
        }
        //для использования через хэндлпоинт
        public abstract PointF GetP(int num);
        public abstract int? GetF(int num);
        public abstract void SetP(int num, PointF p);
        public abstract void SetF(int num, int? f);
        
        public abstract string ConvertToString();
        public static Marker CreateFromString(string s, int versionNum)
        {
            switch (versionNum)
            {
                case 1:
                    {
                        var m = new MarkerBag5();
                        char[] sep = { '\t' };
                        var numOfMarkers = 5; //число точечных маркеров в каждой маркере бэга                    
                        var ss = s.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        if (ss.Length == 2 + numOfMarkers * 2)
                        {
                            m.F1 = int.Parse(ss[0]);
                            m.F2 = int.Parse(ss[1]);
                            for (var i = 0; i < numOfMarkers; i++)
                                m.Points[i] = new PointF(float.Parse(ss[2 + i * 2]), float.Parse(ss[3 + i * 2]));
                            return m;
                        }
                        else
                            throw new Exception($"Ошибка чтения строки файлма маркеров: {s}");
                    }
                case 2:
                    {
                        char[] sep = { '\t' };
                        
                        var ss = s.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        if (ss.Length != 14)
                            throw new Exception($"Ошибка чтения строки файлма маркеров: {s}");
                        switch (ss[12])
                        {
                            case "Bag":
                                {
                                    var m = new MarkerBag5
                                    {
                                        F1 = int.Parse(ss[0]),
                                        F2 = int.Parse(ss[1])
                                    };
                                    var numOfMarkers = 5; //число точечных маркеров в каждой маркере бэга
                                    for (var i = 0; i < numOfMarkers; i++)
                                        m.Points[i] = new PointF(float.Parse(ss[2 + i * 2]), float.Parse(ss[3 + i * 2]));
                                    m.Comment = ss[ss.Length - 1];
                                    return m;
                                }
                            case "Alt":
                            {
                                var m = new MarkerCross()
                                {
                                    F = int.Parse(ss[0])
                                };
                                var numOfMarkers = 4; //число точечных маркеров в каждой маркере бэга
                                for (var i = 0; i < numOfMarkers; i++)
                                    m.Points[i] = new PointF(float.Parse(ss[2 + i * 2]), float.Parse(ss[3 + i * 2]));
                                m.Comment = ss[ss.Length - 1];
                                return m;
                            }
                            case "Shift":
                            {
                                var m = new MarkerLine()
                                {
                                    F1 = int.Parse(ss[0]),
                                    F2 = int.Parse(ss[1])
                                };
                                var numOfMarkers = 2; //число точечных маркеров в каждой маркере бэга
                                for (var i = 0; i < numOfMarkers; i++)
                                    m.Points[i] = new PointF(float.Parse(ss[2 + i * 4]), float.Parse(ss[3 + i * 4]));
                                m.Comment = ss[ss.Length - 1];
                                return m;
                            }
                            case "Control":
                            {
                                var m = new MarkerPoint()
                                {
                                    F = int.Parse(ss[0]),
                                    P = new PointF(float.Parse(ss[2]), float.Parse(ss[3])),
                                    Comment = ss[ss.Length - 1]
                                };
                                return m;
                            }                            
                            default:
                                throw new Exception(
                                    $"Ошибка чтения строки файлма маркеров (неизвестный тип маркера): {s}");
                        }
                    }
                case 3:
                    {
                        char[] sep = { '\t' };
                        var ss = s.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        if (ss.Length == 0)
                            throw new Exception($"Ошибка чтения строки файлма маркеров: {s}");
                        switch (ss[0]) //TODO можно переделать покрасивее
                        {
                            case "bag3":
                                return new MarkerBag3(s);
                            case "bag5":
                                return new MarkerBag5(s);
                            case "point":
                                return new MarkerPoint(s);
                            case "line":
                                return new MarkerLine(s);
                            case "cross":
                                return new MarkerCross(s);
                            case "area_brush":
                                return new Marker_area_brush(s);
                            case "track":
                                return new MarkerTrack(s);


                            default:
                                throw new Exception(
                                    $"Ошибка чтения строки файлма маркеров (неизвестный тип маркера): {s}");
                        }
                    }
                default:
                    throw new Exception($"Неизвестная версия файлма маркеров: {versionNum}");
            }
        }

        public abstract void Paint(Graphics g, CoordnateTransfer ct, int frameNum);

        public class MakerIsChangedArgs : EventArgs
        {
            public bool FramesChanged;
            public bool PointsChanged;
            public bool CommentChanged;
            public MakerIsChangedArgs(bool framesChanged, bool pointsChanged)
            {
                FramesChanged = framesChanged;
                PointsChanged = pointsChanged;
                CommentChanged = false;
            }
            public MakerIsChangedArgs(bool framesChanged, bool pointsChanged, bool commentChanged)
            {
                FramesChanged = framesChanged;
                PointsChanged = pointsChanged;
                CommentChanged = commentChanged;
            }
        }
        public bool IsAtFrame(int frame)
        {            
            return frame >= FrameStart && frame <= FrameEnd;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Marker m2))
                return 0;
            if (!FrameStart.HasValue || !m2.FrameStart.HasValue)
                return 0;
            if (FrameStart.Value != m2.FrameStart.Value)
                return FrameStart.Value - m2.FrameStart.Value;
            return GetHashCode() - m2.GetHashCode(); //если остальное совпадает сравнимаем положение в памяти
        }

        public Pen GetPhantomPen(int frameNum)
        {
            var phantomFrames = Program.ProgramSettings.PhantomFrames;
            if (phantomFrames != 0 &&
                frameNum >= FrameStart - phantomFrames && // f1.HasValue && f2.HasValue && сработает само при сравнении с null
                frameNum <= FrameEnd + phantomFrames &&
                AllPointsDefined()) //все точки проставлены
            {
                var a = (int) Math.Max(0, 255 - Math.Min(
                                              Math.Abs(FrameStart.Value - frameNum),
                                              Math.Abs(FrameEnd.Value - frameNum))
                                          / (double) phantomFrames * 255.0);
                return new Pen(Program.ProgramSettings.PhantomColor) { Color = Color.FromArgb(a, Program.ProgramSettings.PhantomColor) };
            }
            return null;
        }
        public abstract bool AllPointsDefined();
    }
}
