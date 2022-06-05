using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BagFinder.Main;

namespace BagFinder.Forms
{
    public partial class FormSettings : Form
    {
        private ProgramSettings _tempSettings;
        private bool _suspendEvents;

        public FormSettings()
        {
            InitializeComponent();
            UpdateValues();
        }

        private void UpdateValues()
        {
            _suspendEvents = true; //чтобы не вызывались события //TODO костыль

            _tempSettings = Program.ProgramSettings.ShallowCopy();
            numericUpDown_ScaleCircle.Value = Program.ProgramSettings.ScaleCircle;
            numericUpDown_Pahntom_frameSpan.Value = Program.ProgramSettings.PhantomFrames;
            numericUpDown_Alt_Fill.Value = Program.ProgramSettings.CrossAlpha;
            numericUpDown_tlScale.Value = (decimal)Program.ProgramSettings.TlScale;
            numericUpDownTimerInterval.Value = Program.ProgramSettings.PlayTimerInterval;
            numericUpDownPlayStep.Value = (decimal)Program.ProgramSettings.PlayStep;
            numericUpDownMouseRewindSens.Value = (decimal)Program.ProgramSettings.MouseRewindSens;
            numericUpDownwheelRewindFrameStep.Value = Program.ProgramSettings.WheelRewindFrameStep;
            numericUpDown_zoom_wheel_speed.Value = (decimal)Program.ProgramSettings.ZoomWheelSpeed;
            numericUpDown_cache_imnum.Value = Program.ProgramSettings.ImageChacherMaximumNumberOfImages;
            numericUpDown_cache_center.Value = Program.ProgramSettings.ImageChacherCenterShift;
            numericUpDown__handle_size.Value = Program.ProgramSettings.HandleRectSize;
            numericUpDown_grid_x.Value = Program.ProgramSettings.GridX;
            numericUpDown_grid_y.Value = Program.ProgramSettings.GridY;

            checkBox_chache_useplaystep.Checked = Program.ProgramSettings.ImageChacherUsePlayStep;

            //Таблица комментов
            dataGridView1.Rows.Clear();
            var dc = Program.ProgramSettings.DefaultComments;
            var markerTypeList = new List<string>(dc.Keys);
            foreach (var markerType in markerTypeList)
            {
                var markerComs = dc[markerType];
                dataGridView1.Rows.Add(markerType, markerComs[0], markerComs[1], markerComs[2], markerComs[3]);
            }
            dataGridView1.CellEndEdit += DataGridView1_CellEndEdit1;

            //таблица цветов
            dataGridView2.Rows.Clear();
            var mc = Program.ProgramSettings.MarkerColors;
            foreach (var markerText in mc.Keys)
            {
                dataGridView2.Rows.Add(markerText, mc[markerText].Name);
                dataGridView2.Rows[dataGridView2.Rows.Count-1].Cells[1].Style.BackColor = mc[markerText];
            }            
            dataGridView2.CellClick += DataGridView2_CellClick;

            _suspendEvents = false;
        }

        private void DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var mc = Program.ProgramSettings.MarkerColors;
            var markerText = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
            colorDialog1.Color = mc[markerText];
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                mc[markerText] = colorDialog1.Color;
                UpdateValues();
                RaiseChangeEvent();                
            }
        }

        private void DataGridView1_CellEndEdit1(object sender, DataGridViewCellEventArgs e)
        {
            var dc = Program.ProgramSettings.DefaultComments;
            var row = dataGridView1.Rows[e.RowIndex];
            dc[row.Cells[0].Value.ToString()] = new[] { row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString(), row.Cells[4].Value.ToString() };
        }
        
        

        private void RaiseChangeEvent()
        {
            Program.ProgramSettings.OnProgramSettingsChanged(this);
        }

        private void some_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!_suspendEvents)
            {
                Program.ProgramSettings.ScaleCircle = (int)numericUpDown_ScaleCircle.Value;
                Program.ProgramSettings.PhantomFrames = (int)numericUpDown_Pahntom_frameSpan.Value;
                Program.ProgramSettings.CrossAlpha = (int)numericUpDown_Alt_Fill.Value;
                Program.ProgramSettings.TlScale = (double)numericUpDown_tlScale.Value;
                Program.ProgramSettings.PlayTimerInterval = (int)numericUpDownTimerInterval.Value;
                Program.ProgramSettings.PlayStep = (int)numericUpDownPlayStep.Value;
                Program.ProgramSettings.MouseRewindSens = (double)numericUpDownMouseRewindSens.Value;
                Program.ProgramSettings.WheelRewindFrameStep = (int)numericUpDownwheelRewindFrameStep.Value;
                Program.ProgramSettings.ZoomWheelSpeed = (double)numericUpDown_zoom_wheel_speed.Value;
                Program.ProgramSettings.ImageChacherMaximumNumberOfImages = (int)numericUpDown_cache_imnum.Value;
                Program.ProgramSettings.ImageChacherCenterShift = (int)numericUpDown_cache_center.Value;                
                Program.ProgramSettings.ImageChacherUsePlayStep = checkBox_chache_useplaystep.Checked;
                Program.ProgramSettings.HandleRectSize = (int)numericUpDown__handle_size.Value;
                Program.ProgramSettings.GridX = (int)numericUpDown_grid_x.Value;
                Program.ProgramSettings.GridY = (int)numericUpDown_grid_y.Value;
                RaiseChangeEvent();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Program.ProgramSettings = _tempSettings; //откат изменений
            RaiseChangeEvent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Program.ProgramSettings.Save(ProgramSettings.SettingsSavePath); //собственно все и так сохраняется
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            Program.ProgramSettings = new ProgramSettings(); //откат изменений
            UpdateValues();
            RaiseChangeEvent();
        }

        private void button_clear_cache_Click(object sender, EventArgs e)
        {
            Program.Record?.Ic.Clear();
            Program.ViewerTimeLine.Invalidate();
        }

        private void button_fit_timeline_Click(object sender, EventArgs e)
        {
            Program.ViewerTimeLine.Fit();
            UpdateValues();
        }
        


        private void buttonPhantom_color_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = Program.ProgramSettings.PhantomColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Program.ProgramSettings.PhantomColor = colorDialog1.Color;
                RaiseChangeEvent();
            }
        }
        private void buttonBackgorund_color_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = Program.ProgramSettings.BgColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Program.ProgramSettings.BgColor = colorDialog1.Color;
                RaiseChangeEvent();
            }
        }
        private void button__handle_color_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = Program.ProgramSettings.HandleRectColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Program.ProgramSettings.HandleRectColor = colorDialog1.Color;
                RaiseChangeEvent();
            }
        }

        private void button_ScaleCircleColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = Program.ProgramSettings.ScaleCircleColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Program.ProgramSettings.ScaleCircleColor = colorDialog1.Color;
                RaiseChangeEvent();
            }
        }
    }
}
