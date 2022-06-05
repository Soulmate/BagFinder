using BagFinder.Forms;

namespace BagFinder.Main
{
    internal class ViewerTimeLine
    {
        public TimeLine TimeLine;

        public void Invalidate()
        {
            TimeLine?.Invalidate();
        }
        
        public void Reset()
        {
            TimeLine?.Reset();
        }

        internal void Fit()
        {
            if (Program.Record == null) return;
            double imCount = Program.Record.Il.ImCount;
            Program.ProgramSettings.TlScale = (imCount / TimeLine.Width).Clamp(0.001, 10000);
            Program.ProgramSettings.OnProgramSettingsChanged(this);
        }
    }
}
