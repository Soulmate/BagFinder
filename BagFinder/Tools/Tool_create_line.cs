using System.Windows.Forms;
using BagFinder.Main;
using BagFinder.Markers;

namespace BagFinder.Tools
{
    internal class ToolCreateLine : Tool
    {
        private MarkerLine _marker;

        public ToolCreateLine(ToolSet ownerToolSet) : base(ownerToolSet)
        {
            Text = "Line";
        }
        public override bool MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _marker = new MarkerLine();                
                Program.Record.MarkersList.Add(_marker);
                _marker.Comment = Program.ToolSet.GetDefaultMarkerComment(_marker.TypeText);

                var hp = _marker.HpList[0];
                hp.P = Program.ViewerImage.Ct.Wc2Ic(e.Location);
                hp.F = Program.Rewinder.ImNum;
                OwnerToolSet.ToolEditMarkers.StartEdit(_marker.HpList[1]);
                return true;
            }
            return false;
        }
       
        public override bool KeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                CancelCreating();
            }
            return false;
        }

        private void CancelCreating()
        {
            Program.Record.MarkersList.Remove(_marker);
            Program.ViewerImage.Invalidate();
        }
    }
}
