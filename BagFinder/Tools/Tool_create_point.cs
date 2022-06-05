using System.Windows.Forms;
using BagFinder.Main;
using BagFinder.Markers;

namespace BagFinder.Tools
{
    internal class ToolCreatePoint : Tool
    {
        public ToolCreatePoint(ToolSet ownerToolSet) : base(ownerToolSet)
        {
            Text = "Point";
        }
        public override bool MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (OwnerToolSet.ToolEditMarkers.IsEditing) return false;

                var marker = new MarkerPoint();
                Program.Record.MarkersList.Add(marker);
                marker.Comment = Program.ToolSet.GetDefaultMarkerComment(marker.TypeText);
                
                var hp = marker.HpList[0];
                hp.P = Program.ViewerImage.Ct.Wc2Ic(e.Location);
                hp.F = Program.Rewinder.ImNum;
                OwnerToolSet.ToolEditMarkers.StartEdit(hp);

                return true;
            }
            return false;
        }
    }
}
