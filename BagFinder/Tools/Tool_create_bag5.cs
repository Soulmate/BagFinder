using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BagFinder.Main;
using BagFinder.Markers;

namespace BagFinder.Tools
{
    internal class ToolCreateBag5 : Tool
    {
        private int _creatingStep = 0;
        private MarkerBag5 _marker;

        public ToolCreateBag5(ToolSet ownerToolSet) : base(ownerToolSet)
        {
            Text = "Bag5";
        }

        public override bool MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_marker != null && !Program.Record.MarkersList.Contains(_marker))
                    //если маркер создали на предыдущем этапе, но его уже успели удалить
                    CancelCreating();
                switch (_creatingStep)
                {
                    case 0:
                    {
                        _marker = new MarkerBag5();
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
                        _creatingStep = 2;
                        OwnerToolSet.ToolEditMarkers.StartEdit(_marker.HpList[3]);
                    }
                        break;
                    case 2:
                    {
                        var hp = _marker.HpList[4];
                        hp.P = Program.ViewerImage.Ct.Wc2Ic(e.Location);
                        hp.F = Program.Rewinder.ImNum;
                        _creatingStep = 0;
                        OwnerToolSet.ToolEditMarkers.StartEdit(_marker.HpList[4]);
                        _marker = null; //TODO надо ли это делать?
                    }
                        break;
                }

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