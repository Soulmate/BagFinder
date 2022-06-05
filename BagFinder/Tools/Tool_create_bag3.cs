using System.Linq;
using System.Windows.Forms;
using BagFinder.Main;
using BagFinder.Markers;

namespace BagFinder.Tools
{
    internal class ToolCreateBag3 : Tool
    {
        private int _creatingStep = 0;
        private MarkerBag3 _marker;

        public ToolCreateBag3(ToolSet ownerToolSet) : base(ownerToolSet)
        {     
            Text = "Bag3";
        }
        public override bool MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {                
                switch (_creatingStep)
                {
                    case 0:
                        {
                            _marker = new MarkerBag3();                            
                            Program.Record.MarkersList.Add(_marker);
                            _marker.Comment = Program.ToolSet.GetDefaultMarkerComment(_marker.TypeText);

                            var hp = _marker.HpList[0];
                            hp.P = Program.ViewerImage.Ct.Wc2Ic(e.Location);
                            hp.F = Program.Rewinder.ImNum;
                            _creatingStep = 1;
                            OwnerToolSet.ToolEditMarkers.StartEdit(_marker.HpList[1]);
                        }
                        break;
                    case 1:
                        {
                            var hp = _marker.HpList[2];
                            hp.P = Program.ViewerImage.Ct.Wc2Ic(e.Location);
                            hp.F = Program.Rewinder.ImNum;
                            _creatingStep = 0;
                            OwnerToolSet.ToolEditMarkers.StartEdit(_marker.HpList[2]);
                        }
                        break;
                }
                Program.ViewerImage.Invalidate();
                return true;
            }
            return false;
        }
        public override bool KeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
                CancelCreating();
            return false;
        }        

        private void CancelCreating()
        {
            _creatingStep = 0;
            Program.Record.MarkersList.Remove(_marker);
            Program.ViewerImage.Invalidate();
        }
    }
}
