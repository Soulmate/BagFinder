using System;
using System.Windows.Forms;
using BagFinder.Main;
using BagFinder.Tools;

namespace BagFinder.Forms
{
    internal partial class Form1 : Form
    {
        private ProgramSettings _programSettings;
        private Rewinder _rewinder;
        private ViewerImage _viewerImage;
        private ViewerTimeLine _viewerTimeLine;

        public Form1()
        {
            InitializeComponent();
            KeyPreview = true; // чотбы форма видела нажатие клавиш
            //todo Надо заблокировать все лишние элементы управления

            markerListViewer1.Enabled = false;
            timeLine1.Enabled = false;
            pictureBox1.Enabled = false;
            toolSet_bar1.Enabled = false;
        }

        public void BindToToStuff(
            ToolSet toolSet, ProgramSettings programSettings, Rewinder rewinder,
            ViewerImage viewerImage, ViewerTimeLine viewerTimeLine, ViewerInfo viewerInfo)
        {
            toolSet_bar1.BindToToolSet(Program.ToolSet);
            KeyDown += toolSet.Form1_KeyDown;
            KeyUp += toolSet.Form1_KeyUp;
            pictureBox1.MouseDown += toolSet.Pb_MouseDown;
            pictureBox1.MouseMove += toolSet.Pb_MouseMove;
            pictureBox1.MouseUp += toolSet.Pb_MouseUp;
            pictureBox1.MouseWheel += toolSet.Pb_MouseWheel;

            _programSettings = programSettings;
            programSettings.ProgramSettingsChanged += Program_MySettings_changed;

            _rewinder = rewinder;
            rewinder.FrameChanged += Rewinder_FrameChanged;
            rewinder.Timer = timer1;

            _viewerImage = viewerImage;
            viewerImage.Pb = pictureBox1;

            _viewerTimeLine = viewerTimeLine;
            viewerTimeLine.TimeLine = timeLine1;

            viewerInfo.ToolStripStatusLabel = toolStripStatusLabel1;
            viewerInfo.StatusStrip = statusStrip1;

            //вручную вызываем ивенты
            Program_MySettings_changed(this, new EventArgs());
            Rewinder_FrameChanged(this, new EventArgs());

            //чтобы шоткеи работали корректно и не перехватывались при работе с комментами
            pictureBox1.GotFocus += (s1, e1) => toolSet.CaptureKeys = true;
            pictureBox1.LostFocus += (s1, e1) => toolSet.CaptureKeys = false;
            timeLine1.GotFocus += (s1, e1) => toolSet.CaptureKeys = true;
            timeLine1.LostFocus += (s1, e1) => toolSet.CaptureKeys = false;
        }

        public void OnOpenRecord()
        {
            if (Program.ProgramSettings.RecordType == Record.RecordType.ImagesFolder)
                _dialogFolder.SelectedPath = Program.ProgramSettings.RecordPath;
            else
                _dialogFile.FileName = Program.ProgramSettings.RecordPath;

            markerListViewer1.Enabled = true;
            timeLine1.Enabled = true;
            pictureBox1.Enabled = true;
            toolSet_bar1.Enabled = true;

            numericUpDownN.Minimum = 0;
            numericUpDownN.Maximum = _rewinder.ImCount - 1;
            numericUpDownN.Value = 0;

            Text = _programSettings.RecordPath;

            markerListViewer1.BindToStuff(Program.Record.MarkersList, _rewinder);
        }

        private void Rewinder_FrameChanged(object sender, EventArgs e)
        {
            if (numericUpDownN.Value != _rewinder.ImNum)
                numericUpDownN.Value = _rewinder.ImNum;
            timeLine1.Rewinder_FrameChanged();
        }

        private void Program_MySettings_changed(object sender, EventArgs e)
        {
            timer1.Interval = _programSettings.PlayTimerInterval;
            timeLine1.Program_ProgramSettings_changed();
            pictureBox1.BackColor = _programSettings.BgColor;
        }

        // открытие закрытие  
        private readonly FolderBrowserDialog _dialogFolder = new FolderBrowserDialog();

        private readonly OpenFileDialog _dialogFile = new OpenFileDialog();

        //CommonOpenFileDialog dialog_folder = new CommonOpenFileDialog();
        //CommonOpenFileDialog dialog_file = new CommonOpenFileDialog();
        //открытие через меню
        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_dialogFolder.ShowDialog() == DialogResult.OK)
                try
                {
                    Program.OpenRecord(_dialogFolder.SelectedPath, Record.RecordType.ImagesFolder);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Record oppening error: {ex.Message}");
                }
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_dialogFile.ShowDialog() == DialogResult.OK)
                try
                {
                    Program.OpenRecord(_dialogFile.FileName, Record.RecordType.VideoFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Record oppening error: {ex.Message}");
                }
        }

        private void openTIFFPCOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_dialogFile.ShowDialog() == DialogResult.OK)
                try
                {
                    Program.OpenRecord(_dialogFile.FileName, Record.RecordType.Tiff16BitMultipage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Record oppening error: {ex.Message}");
                }
        }

        //открытие через перетаскивание файла        
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[]) e.Data.GetData(DataFormats.FileDrop); //TODO возможность перетащить несколько файлов и рассмотреть их как запись
            try
            {
                Program.OpenRecord(files[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Record oppening error: {ex.Message}");
            }
        }






        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Program.ViewerImage.Paint(e.Graphics);
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.Focus();
            //Console.WriteLine("Focus");
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            //Console.WriteLine("Lost focus");
            Focus();
        }

        private void resetViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _viewerImage.Reset();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _rewinder.Play_timer_tick();
        }

        private void saveMarkersListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            markerListViewer1.ForceEndEdit();
            Program.ViewerInfo.BottomText = $"Saving...";
            Program.Record.MarkersList.Save();
            Program.ViewerInfo.BottomText = $"Saving done";
        }

        private void loadMarkersListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Program.Record.MarkersList.FileExists)
                MessageBox.Show("No marker file");

            var result = MessageBox.Show("Точно загрузить?", "Внимание",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;
            if (Program.Record.MarkersList.TryLoad())
            {
                _viewerImage.Invalidate();
                _viewerTimeLine.Invalidate();
                MessageBox.Show("Loaded markers: " + Program.Record.MarkersList.Count.ToString());
            }
            else
            {
                MessageBox.Show($"File read error: {Program.Record.MarkersList.FileReadException.Message}");
            }
        }

        private void zoomFitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _viewerImage.ZoomFit();
        }

        private void numericUpDownN_ValueChanged(object sender, EventArgs e)
        {
            _rewinder.ImNum = (int) numericUpDownN.Value;
        }

        private FormSettings _formSettings;

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_formSettings == null || _formSettings.IsDisposed) _formSettings = new FormSettings {TopMost = true};
            _formSettings.Show();
        }

        private FormRecordSettings _formRecordSettings;

        private void recordSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_formRecordSettings == null || _formRecordSettings.IsDisposed)
                _formRecordSettings = new Forms.FormRecordSettings {TopMost = true};
            _formRecordSettings.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Program.OpenRecord();
        }




        //SHOW HIDE
        private void showhide_timeline(object sender, EventArgs e)
        {
            timeLine1.Visible = !timeLine1.Visible;
        }

        private void showhide_markerBrowser(object sender, EventArgs e)
        {
            markerListViewer1.Visible = !markerListViewer1.Visible;
        }

        private void showhide_toolbar(object sender, EventArgs e)
        {
            toolSet_bar1.Visible = !toolSet_bar1.Visible;
        }

        private void showhide_statusbar(object sender, EventArgs e)
        {
            statusStrip1.Visible = !statusStrip1.Visible;
        }

        private bool _fullscreen;

        private void fullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fullscreen = !_fullscreen;
            if (_fullscreen)
            {
                markerListViewer1.Visible = false;
                timeLine1.Visible = false;
                menuStrip1.Visible = false;
                toolSet_bar1.Visible = false;
                statusStrip1.Visible = false;
            }
            else
            {
                markerListViewer1.Visible = true;
                timeLine1.Visible = true;
                menuStrip1.Visible = true;
                toolSet_bar1.Visible = true;
                statusStrip1.Visible = true;
            }
        }

        private void Form1_Resize(object sender, EventArgs e) //чтобы при сворачивании окна закрывались дополнительные окна
        {
            if (WindowState == FormWindowState.Minimized)
            {
                _formSettings?.Close();
                _formRecordSettings?.Close();
            }
        }

        private void saveImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormSaveImages().Show();
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }
    }
}