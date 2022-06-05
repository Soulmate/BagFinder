using System;
using System.Drawing;
using System.Windows.Forms;
using BagFinder.Main;

namespace BagFinder.Tools
{
    internal class ToolZoomRect : Tool
    {
        private bool _creatingRect = false;
        private PointF _zoomP1, _zoomP2;

        public ToolZoomRect(ToolSet ownerToolSet) : base(ownerToolSet)
        {
            Text = "zoom";
        }

        public override bool Paint(Graphics g)
        {
            if (_creatingRect)
            {
                var p1 = new PointF(Math.Min(_zoomP1.X, _zoomP2.X), Math.Min(_zoomP1.Y, _zoomP2.Y));
                var p2 = new PointF(Math.Max(_zoomP1.X, _zoomP2.X), Math.Max(_zoomP1.Y, _zoomP2.Y));
                g.DrawRectangle(Pens.White, p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
            }

            return false;
        }

        public override bool MouseDown(MouseEventArgs e)
        {
            _creatingRect = true; //начинаем создавать рамку зума    
            _zoomP1 = e.Location;
            _zoomP2 = e.Location;

            return false;
        }

        public override bool MouseMove(MouseEventArgs e)
        {
            if (_creatingRect)
            {
                _zoomP2 = e.Location;
                Program.ViewerImage.Invalidate();
            }

            return false;
        }

        public override bool MouseUp(MouseEventArgs e)
        {
            if (_creatingRect)
            {
                _zoomP2 = e.Location;
                Program.ViewerImage.Ct.ZoomToCorners(ref _zoomP1, ref _zoomP2, Program.ViewerImage.Pb.Size);
                _creatingRect = false;
                Program.ViewerImage.Invalidate();
            }

            return false;
        }

        public override bool KeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                _creatingRect = false;
                Program.ViewerImage.Invalidate();
            }

            return false;
        }
    }
}