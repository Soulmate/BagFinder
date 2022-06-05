using System.Windows.Forms;

namespace BagFinder.Main
{
    internal class ViewerInfo
    {
        public string BottomText
        {
            get
            {
                if (ToolStripStatusLabel != null)
                    return ToolStripStatusLabel.Text;
                else
                    return "";
            }
            set
            {
                if (ToolStripStatusLabel != null)
                    if (ToolStripStatusLabel.Text != value)
                    {
                        ToolStripStatusLabel.Text = value;
                        Invalidate();                        
                    }
            }
        }

        public ToolStripStatusLabel ToolStripStatusLabel;
        public StatusStrip StatusStrip;

        public void Invalidate()
        {
            if (ToolStripStatusLabel != null)
            {
                StatusStrip.Invalidate();
                StatusStrip.Update();
            }
        }
    }
}
