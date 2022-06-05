using System.Drawing;
using System.Windows.Forms;
using BagFinder.Main;

namespace BagFinder.Tools
{
    internal class ToolPan : Tool
    {
        private Point _mPoint = new Point();
        private bool _panning = false;
        public ToolPan(ToolSet ownerToolSet) : base(ownerToolSet)
        {            
            Text = "pan";
        }
        public override bool MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                _panning = true;
                _mPoint = e.Location;
            }
            return false;
        }
        public override bool MouseMove(MouseEventArgs e)
        {
            if (_panning)
            {
                var mDispl = Size.Subtract((Size)e.Location, (Size)_mPoint);
                Program.ViewerImage.Ct.Ico.X = Program.ViewerImage.Ct.Ico.X + e.X - _mPoint.X;
                Program.ViewerImage.Ct.Ico.Y = Program.ViewerImage.Ct.Ico.Y + e.Y - _mPoint.Y;
                _mPoint = e.Location;
                Program.ViewerImage.Invalidate();
            }
            return false;
        }
        public override bool MouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                _panning = false;
            }
            return false;
        }
    }
}
