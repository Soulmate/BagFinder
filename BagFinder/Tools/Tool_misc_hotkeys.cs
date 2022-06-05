using System.Drawing;
using System.Windows.Forms;
using BagFinder.Main;

namespace BagFinder.Tools
{
    internal class ToolMiscHotkeys : Tool
    {
        public ToolMiscHotkeys(ToolSet ownerToolSet) : base(ownerToolSet)
        {            
            Text = "mh";
        }
        public override bool KeyDown(KeyEventArgs e)
        {
            //2019 02
            if (e.KeyData == Keys.Delete)
            {
                Program.Record.MarkersList.RemoveSelected();
                return true;
            }
            return false;
        }
        public override bool KeyUp(KeyEventArgs e)
        {
           /* if (e.KeyData == Keys.O)
            {
                Program.ViewerImage.Invalidate();
            }*/
            return false;
        }
    }
}
