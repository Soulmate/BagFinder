using System;
using System.Windows.Forms;
using BagFinder.Main;

namespace BagFinder.Forms
{
    public partial class FormRecordSettings : Form
    {
        private readonly RecordSettings _tempRecordSettings;
        private bool _suspendEvents;

        public FormRecordSettings()
        {
            InitializeComponent();
            _tempRecordSettings = Program.Record.RecordSettings.ShallowCopy();
            nl1.Maximum = Program.Record.Ip.LevelMax;
            nl2.Maximum = Program.Record.Ip.LevelMax;
            nl1.Minimum = Program.Record.Ip.LevelMin;
            nl2.Minimum = Program.Record.Ip.LevelMin;
            UpdateDisplayedValues();
        }
        private void UpdateDisplayedValues()
        {
            _suspendEvents = true; //чтобы не вызывались события //TODO костыль

            numericUpDown_recFPS.Value = (decimal)Program.Record.RecordSettings.Fps;
            numericUpDown_recScale.Value = (decimal)Program.Record.RecordSettings.Scale;
            numericUpDown_FrameTimeZero.Value = Program.Record.RecordSettings.FrameTimeZero;
            checkBoxRotate.Checked = Program.Record.RecordSettings.Rotate;
            checkBoxInvert.Checked = Program.Record.RecordSettings.Invert;
            checkBoxDolevels.Checked = Program.Record.RecordSettings.Dolevels;
            nl1.Value = Program.Record.RecordSettings.Level1;
            nl2.Value = Program.Record.RecordSettings.Level2;

            _suspendEvents = false;
        }
        
        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void ValueChanged(object sender, EventArgs e)
        {
            if (!_suspendEvents)
            {
                Program.Record.RecordSettings.Fps = (double)numericUpDown_recFPS.Value;
                Program.Record.RecordSettings.Scale = (double)numericUpDown_recScale.Value;
                Program.Record.RecordSettings.FrameTimeZero = (int)numericUpDown_FrameTimeZero.Value;
                Program.Record.RecordSettings.Rotate = checkBoxRotate.Checked;
                Program.Record.RecordSettings.Invert = checkBoxInvert.Checked;
                Program.Record.RecordSettings.Dolevels = checkBoxDolevels.Checked;
                Program.Record.RecordSettings.Level1 = (int)nl1.Value;
                Program.Record.RecordSettings.Level2 = (int)nl2.Value;
            }
        }

        private void buttonDefault_Click_1(object sender, EventArgs e)
        {
            Program.Record.RecordSettings = new RecordSettings(); //откат изменений
            UpdateDisplayedValues();
        }

        private void buttonCancel_Click_1(object sender, EventArgs e)
        {
            Program.Record.RecordSettings = (RecordSettings) _tempRecordSettings.Clone(); //откат изменений //оно там само вызовет нужные ивенты
        }

        private void buttonAutolevels_Click(object sender, EventArgs e)
        {
            Program.Record.AutoLevels();
            Program.Record.RecordSettings.Level1 = Program.Record.Ip.Level1;
            Program.Record.RecordSettings.Level2 = Program.Record.Ip.Level2;
            _suspendEvents = true; //чтобы не вызывались события //TODO костыль
            nl1.Value = Program.Record.RecordSettings.Level1;
            nl2.Value = Program.Record.RecordSettings.Level2;
            _suspendEvents = false;            
            Program.Record.RecordSettings.OnImageProcessorSettings_changed();
        }

        private void button_SetZeroFrame_Click(object sender, EventArgs e)
        {
            Program.Record.RecordSettings.FrameTimeZero = Program.Rewinder.ImNum;
            UpdateDisplayedValues();
        }
    }
}
