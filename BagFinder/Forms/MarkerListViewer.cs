using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BagFinder.Main;
using BagFinder.Markers;

namespace BagFinder.Forms
{
    internal partial class MarkerListViewer : UserControl
    {
        //TODO  не сохраняются иногда комменты

        private MarkerList _ml;
        private Rewinder _rewinder;

        public MarkerListViewer()
        {
            InitializeComponent();
        }
        public void BindToStuff(MarkerList ml, Rewinder rewinder)
        {
            _rewinder = rewinder;
            _ml = ml;
            rewinder.FrameChanged += Rewinder_FrameChanged;
            ml.MakerListIsChanged += Ml_MakerListIsChanged;
            ml.SomeMakerIsChanged += Ml_SomeMakerIsChanged;
            ml.SelectionIsChanged += Ml_SelectionIsChanged;
            UpdateMB_all();
        }

        private void Rewinder_FrameChanged(object sender, EventArgs e)
        {
            if (!_rewinder.Playing)
                HightlightByFrame(); //TODO костыль, чтобы не тормозило
        }

        private void Ml_SelectionIsChanged(object sender, EventArgs e)
        {
            var me = (MarkerList.SelectionIsChangedArgs)e;
            if (me.ChangeType == MarkerList.SelectionIsChangedArgs.SelectionIsChangedTypeEnum.AddOne &&
                _ml.SelectedMarkers.Count == 1)
                dataGridView1.FirstDisplayedScrollingRowIndex = _ml.IndexOf(me.Marker);
            HighlightSeleted();
        }

        private void HighlightSeleted()
        {
            for (var i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow r = dataGridView1.Rows[i];
                r.DefaultCellStyle.BackColor = _ml.SelectionIsSelected(i) ? Color.Tomato : default(Color);
            }
        }

        private void HightlightByFrame() //подсветить маркеры на текущем фрейме
        {           
            var n = _rewinder.ImNum;
            foreach (DataGridViewRow r in dataGridView1.Rows)
                r.DefaultCellStyle.ForeColor = Color.Black;
            foreach (var i in _ml.FindIndexAtFrame(n))
                dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;                
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.BeginEdit(false);
            dataGridView1.ClearSelection();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || (e.ColumnIndex != 0 && e.ColumnIndex != 1)) return;
            if (Control.ModifierKeys == Keys.None)
                _ml.SelectionSelectOnly(e.RowIndex);
            else if (Control.ModifierKeys == Keys.Control)
                _ml.SelectionToggle(e.RowIndex);
            //else if (Control.ModifierKeys == Keys.Shift && _ml.SelectedMarkers.Count > 0) //TODO

                var frame = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToNullableInt(); //TODO может сразу инт?
            if (frame.HasValue)
                _rewinder.ImNum = frame.Value;
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var rn = e.RowIndex;
            var cn = e.ColumnIndex;
            if (cn == 3)
            {
                if (Program.Record != null && Program.Record.MarkersList != null && rn < Program.Record.MarkersList.Count)
                {
                    var s = e.FormattedValue.ToString();
                    Program.Record.MarkersList[rn].Comment = s;
                }
            }
        }
        
        

        private void Ml_MakerListIsChanged(object sender, EventArgs e)
        {
            var me = (MarkerList.MakerListIsChangedArgs)e;
            switch (me.ChangeType)
            {
                case MarkerList.MakerListIsChangedArgs.ChangeTypeEnum.AddOne:
                    var m = me.Marker;
                    dataGridView1.Rows.Add(m.FrameStart.ToString(), m.FrameEnd.ToString(), m.TypeText, m.Comment);
                    break;
                case MarkerList.MakerListIsChangedArgs.ChangeTypeEnum.RemoveOne:
                    //dataGridView1.Rows.RemoveAt(_ml.IndexOf(me.Marker)); //TODO чтобы нигде индексы не использовались
                    UpdateMB_all();
                    break;
                case MarkerList.MakerListIsChangedArgs.ChangeTypeEnum.Massive:
                    UpdateMB_all();
                    break;
            }
            HightlightByFrame();
        }

        private void Ml_SomeMakerIsChanged(object sender, EventArgs e)
        {
            var mce = (Marker.MakerIsChangedArgs)e;
            if (!mce.FramesChanged && !mce.CommentChanged) return;
            var m = (Marker)sender;
            var rownum = _ml.IndexOf(m);
            if (rownum < 0) return;
            dataGridView1.Rows[rownum].Cells[0].Value = m.FrameStart.ToString();
            dataGridView1.Rows[rownum].Cells[1].Value = m.FrameEnd.ToString();
            dataGridView1.Rows[rownum].Cells[3].Value = m.Comment;
        }

        private void UpdateMB_all()
        {
            var strList = _ml.Select(m => new[] { m.FrameStart.ToString(), m.FrameEnd.ToString(), m.TypeText, m.Comment });
            dataGridView1.Rows.Clear();
            foreach (object[] s in strList)
                dataGridView1.Rows.Add(s);
            HighlightSeleted();
            HightlightByFrame();
        }

        public void ForceEndEdit()
        {
            dataGridView1.EndEdit();
            Validate();
        }
        public bool IsCurrentCellInEditMode => dataGridView1.IsCurrentCellInEditMode;

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            _ml.Sort();
        }

        private void toolStripButton_clear_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Точно удалить все маркеры?", "Внимание",
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                _ml.Clear();
        }

        private void toolStripButton_delete_selected_Click(object sender, EventArgs e)
        {
            _ml.RemoveSelected();
        }
    }
}
