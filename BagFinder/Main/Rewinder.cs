using System;
using System.Windows.Forms;

namespace BagFinder.Main
{
    internal class Rewinder
    {
        public event EventHandler FrameChanged;

        public Timer Timer;

        public bool Playing
        {
            get => _playing;
            set
            {
                if (Timer != null && value != _playing)
                {
                    _playing = value;
                    Timer.Enabled = value;
                }
            }
        }
        private bool _playing;
        public bool PlayingForward = true;

        private System.Diagnostics.Stopwatch _watch = System.Diagnostics.Stopwatch.StartNew();
        public void Play_timer_tick()
        {
            _watch.Stop();
            //Console.WriteLine("tick {0} ms", watch.ElapsedMilliseconds);
            _watch = System.Diagnostics.Stopwatch.StartNew();            

            int n;
            if (PlayingForward)
                n = ImNum + (int)Math.Round(Program.ProgramSettings.PlayStep);
            else
                n = ImNum - (int)Math.Round(Program.ProgramSettings.PlayStep);

            if (n >= 0 && n < ImCount)
                ImNum = n;
            else
                Playing = false;
        }

        private int _imNum;
        public int ImNum
        {
            get => _imNum;
            set
            {
                if (Program.Record != null)
                {
                    var n = Math.Max(0, Math.Min(Program.Record.Il.ImCount - 1, value));

                    if (_imNum != n)
                    {
                        _imNum = n;

                        Program.ViewerImage.Invalidate();
                        Program.ViewerTimeLine.Invalidate();

                        FrameChanged?.Invoke(this, new EventArgs());
                    }
                }
            }
        }
        public int ImCount => Program.Record != null ? Program.Record.Il.ImCount : 0;
    }
}
