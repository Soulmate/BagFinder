using System.Drawing;
using System.Windows.Forms;
using BagFinder.Main;
using BagFinder.Markers;

namespace BagFinder.Tools
{
    internal class ToolEditMarker : Tool
    {
        private HandlePoint _hp;
        private PointF _tempP;
        private int? _tempF;

        public bool IsEditing => _hp != null;

        public ToolEditMarker(ToolSet ownerToolSet) : base(ownerToolSet)
        {
            Text = "Edit marker";
        }

        public override bool MouseDown(MouseEventArgs e)
        {
            if (Control.ModifierKeys == (Keys.Control | Keys.Alt)) return false;
            var pos = Program.ViewerImage.Ct.Wc2Ic(e.Location);
            var frame = Program.Rewinder.ImNum;
            //ищем хэндлпоинт на этом кадре
            HandlePoint hp = Program.Record.MarkersList.GetHpOnThisFrame(pos, frame);
            //если не нашли на этом кадре ищем среди видимых
            if (hp == null)
            {
                hp = Program.Record.MarkersList.GetVisibleHp(pos, frame);
                if (hp == null || hp.F == null)
                    return false;
                if (Control.ModifierKeys == Keys.Shift)
                {
                    Program.Rewinder.ImNum = hp.F.Value;                    
                }
                return false;
            }

            if (e.Button == MouseButtons.Left)
            {
                StartEdit(hp);
                return true;
            }

            else if (e.Button == MouseButtons.Right) //может срабатывать и во время создания маркеров!
            {
                Program.Record.MarkersList.Remove(hp.Marker);
                return true;
            }

            return false;
        }

        public void StartEdit(HandlePoint hp) // для вызова из других инструментов
        {
            if (hp == null) return;
            _hp = hp;
            Program.Record.MarkersList.SelectionSelectOnly(_hp.Marker);
            _tempP = _hp.P;
            _tempF = _hp.F;
        }

        public override bool MouseMove(MouseEventArgs e)
        {
            if (_hp != null)
            {
                _hp.P = Program.ViewerImage.Ct.Wc2Ic(e.Location);
                _hp.F = Program.Rewinder.ImNum;
                Program.ViewerImage.Invalidate();
            }

            return false;
        }

        public override bool MouseUp(MouseEventArgs e)
        {
            if (_hp != null && e.Button == MouseButtons.Left)
            {
                _hp = null;
                Program.ViewerImage.Invalidate();
            }

            return false;
        }

        public override bool KeyDown(KeyEventArgs e)
        {
            if (_hp != null && e.KeyData == Keys.Escape)
            {
                _hp.P = _tempP;
                _hp.F = _tempF;
                _hp = null;
                Program.ViewerImage.Invalidate();
            }
            

            return false;
        }

        public override bool Paint(Graphics g)
        {
            var handleRectPen = new Pen(Program.ProgramSettings.HandleRectColor);
            //отобразить возможные для выбора ручки и ВЫБРАННУЮ
            foreach (var hp in Program.Record.MarkersList.GetAllHpOnFrame(Program.Rewinder.ImNum))
            {
                handleRectPen.Width = (hp == _hp) ? 2 : 1;
                var p = Program.ViewerImage.Ct.Ic2Wcf(hp.P);
                if (!p.IsEmpty)
                {
                    float hSize = Program.ProgramSettings.HandleRectSize;
                    g.DrawRectangle(handleRectPen, p.X - hSize / 2, p.Y - hSize / 2, hSize, hSize);
                }
            }

            return false;
        }
    }
}