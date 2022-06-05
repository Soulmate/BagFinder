using System;
using System.Drawing;
using System.Windows.Forms;
using BagFinder.Main;

namespace BagFinder.Tools
{
    internal class ToolShowPos : Tool
    {
        public float ScaleCircleRadius = 10;
        private PointF _p;
        private bool _showPos = false;
        private string _tempText;
        public ToolShowPos(ToolSet ownerToolSet) : base(ownerToolSet)
        {            
            Text = "sp";
        }
        public override bool KeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.P)
            {
                _p = Program.ViewerImage.Ct.Wc2Ic(Program.ViewerImage.Pb.PointToClient(Cursor.Position));
                _tempText = Program.ViewerInfo.BottomText;
                Program.ViewerInfo.BottomText = $"X: {_p.X}, Y: {_p.Y}";                
                _showPos = true;
                Program.ViewerImage.Invalidate();
            }
            return false;
        }
        public override bool KeyUp(KeyEventArgs e)
        {
            if (e.KeyData == Keys.P)
            {
                Program.ViewerInfo.BottomText = _tempText;
                _showPos = false;
                Program.ViewerImage.Invalidate();
            }
            return false;
        }
        public override bool MouseMove(MouseEventArgs e)
        {
            if (_showPos)
            {
                _p = Program.ViewerImage.Ct.Wc2Ic(e.Location);
                Program.ViewerInfo.BottomText = $"X: {_p.X}, Y: {_p.Y}";
                Program.ViewerImage.Invalidate();
            }
            return false;
        }
        public override bool Paint(Graphics g)
        {
            if (_showPos)
            {
                var center = Program.ViewerImage.Ct.Ic2Wcf(_p);
                g.DrawLine(Pens.White,
                    center.X - 25, center.Y + 1,
                    center.X + 25, center.Y + 1);
                g.DrawLine(Pens.White,
                    center.X + 1, center.Y - 25,
                    center.X + 1, center.Y + 25);
                g.DrawLine(Pens.Black,
                    center.X - 25, center.Y,
                    center.X + 25, center.Y);
                g.DrawLine(Pens.Black,
                    center.X, center.Y - 25,
                    center.X, center.Y + 25);
                
            }
            return false;
        }
    }
}
