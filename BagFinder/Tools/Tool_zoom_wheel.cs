using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BagFinder.Main;

namespace BagFinder.Tools
{
    internal class ToolZoomWheel : Tool
    {
        public ToolZoomWheel(ToolSet ownerToolSet) : base(ownerToolSet)
        {         
            Text = "zw";
        }
        public override bool MouseWheel(MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                var scale = (1 + e.Delta * Program.ProgramSettings.ZoomWheelSpeed / 500.0);
                var icmNew = Program.ViewerImage.Ct.Icm * scale;
                icmNew = icmNew.Clamp(0.1, 20);
                scale = icmNew / Program.ViewerImage.Ct.Icm;
                Program.ViewerImage.Ct.Icm = icmNew;

                //сохраняем положение
                var mousePosInIcUnscaledX = e.X - Program.ViewerImage.Ct.Ico.X;
                var mousePosInIcUnscaledY = e.Y - Program.ViewerImage.Ct.Ico.Y;
                Program.ViewerImage.Ct.Ico.X = e.X - (int)(mousePosInIcUnscaledX * scale);
                Program.ViewerImage.Ct.Ico.Y = e.Y - (int)(mousePosInIcUnscaledY * scale);
                
                Program.ViewerImage.Invalidate();
                return true;
            }
            return false;
        }
    }
}
