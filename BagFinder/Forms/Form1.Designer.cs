namespace BagFinder.Forms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bagMarkerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTIFFPCOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveMarkersListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadMarkersListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.saveImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomFitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullscreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markerBrowserToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.timeLineToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownN = new System.Windows.Forms.NumericUpDown();
            this.colorDialog2 = new System.Windows.Forms.ColorDialog();
            this.timeLine1 = new BagFinder.Forms.TimeLine();
            this.toolSet_bar1 = new BagFinder.Forms.ToolSetBar();
            this.markerListViewer1 = new BagFinder.Forms.MarkerListViewer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownN)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlText;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 56);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(425, 306);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            this.pictureBox1.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
            this.pictureBox1.Resize += new System.EventHandler(this.pictureBox1_Resize);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem,
            this.imageToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(425, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bagMarkerToolStripMenuItem,
            this.loadFileToolStripMenuItem,
            this.openTIFFPCOToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveMarkersListToolStripMenuItem,
            this.loadMarkersListToolStripMenuItem,
            this.toolStripSeparator2,
            this.settingsToolStripMenuItem,
            this.recordSettingsToolStripMenuItem,
            this.toolStripSeparator3,
            this.saveImagesToolStripMenuItem});
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.createToolStripMenuItem.Text = "File";
            // 
            // bagMarkerToolStripMenuItem
            // 
            this.bagMarkerToolStripMenuItem.Name = "bagMarkerToolStripMenuItem";
            this.bagMarkerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.bagMarkerToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.bagMarkerToolStripMenuItem.Text = "Open folder with images";
            this.bagMarkerToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
            // 
            // loadFileToolStripMenuItem
            // 
            this.loadFileToolStripMenuItem.Name = "loadFileToolStripMenuItem";
            this.loadFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.loadFileToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.loadFileToolStripMenuItem.Text = "Open video file";
            this.loadFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // openTIFFPCOToolStripMenuItem
            // 
            this.openTIFFPCOToolStripMenuItem.Name = "openTIFFPCOToolStripMenuItem";
            this.openTIFFPCOToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.openTIFFPCOToolStripMenuItem.Text = "Open 16 bit TIFF file";
            this.openTIFFPCOToolStripMenuItem.Click += new System.EventHandler(this.openTIFFPCOToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(244, 6);
            // 
            // saveMarkersListToolStripMenuItem
            // 
            this.saveMarkersListToolStripMenuItem.Name = "saveMarkersListToolStripMenuItem";
            this.saveMarkersListToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveMarkersListToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.saveMarkersListToolStripMenuItem.Text = "SaveMarkersList";
            this.saveMarkersListToolStripMenuItem.Click += new System.EventHandler(this.saveMarkersListToolStripMenuItem_Click);
            // 
            // loadMarkersListToolStripMenuItem
            // 
            this.loadMarkersListToolStripMenuItem.Name = "loadMarkersListToolStripMenuItem";
            this.loadMarkersListToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadMarkersListToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.loadMarkersListToolStripMenuItem.Text = "LoadMarkersList";
            this.loadMarkersListToolStripMenuItem.Click += new System.EventHandler(this.loadMarkersListToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(244, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // recordSettingsToolStripMenuItem
            // 
            this.recordSettingsToolStripMenuItem.Name = "recordSettingsToolStripMenuItem";
            this.recordSettingsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.recordSettingsToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.recordSettingsToolStripMenuItem.Text = "Record settings";
            this.recordSettingsToolStripMenuItem.Click += new System.EventHandler(this.recordSettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(244, 6);
            // 
            // saveImagesToolStripMenuItem
            // 
            this.saveImagesToolStripMenuItem.Name = "saveImagesToolStripMenuItem";
            this.saveImagesToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.saveImagesToolStripMenuItem.Text = "Save images";
            this.saveImagesToolStripMenuItem.Click += new System.EventHandler(this.saveImagesToolStripMenuItem_Click);
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetViewToolStripMenuItem,
            this.zoomFitToolStripMenuItem,
            this.panelsToolStripMenuItem});
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.imageToolStripMenuItem.Text = "View";
            // 
            // resetViewToolStripMenuItem
            // 
            this.resetViewToolStripMenuItem.Name = "resetViewToolStripMenuItem";
            this.resetViewToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.resetViewToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.resetViewToolStripMenuItem.Text = "Reset view";
            this.resetViewToolStripMenuItem.Click += new System.EventHandler(this.resetViewToolStripMenuItem_Click);
            // 
            // zoomFitToolStripMenuItem
            // 
            this.zoomFitToolStripMenuItem.Name = "zoomFitToolStripMenuItem";
            this.zoomFitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.zoomFitToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.zoomFitToolStripMenuItem.Text = "Zoom fit";
            this.zoomFitToolStripMenuItem.Click += new System.EventHandler(this.zoomFitToolStripMenuItem_Click);
            // 
            // panelsToolStripMenuItem
            // 
            this.panelsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullscreenToolStripMenuItem,
            this.toolBarToolStripMenuItem,
            this.markerBrowserToolStripMenuItem1,
            this.timeLineToolStripMenuItem1,
            this.statusBarToolStripMenuItem});
            this.panelsToolStripMenuItem.Name = "panelsToolStripMenuItem";
            this.panelsToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.panelsToolStripMenuItem.Text = "Panels";
            // 
            // fullscreenToolStripMenuItem
            // 
            this.fullscreenToolStripMenuItem.Name = "fullscreenToolStripMenuItem";
            this.fullscreenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.fullscreenToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.fullscreenToolStripMenuItem.Text = "Show/hide all";
            this.fullscreenToolStripMenuItem.Click += new System.EventHandler(this.fullscreenToolStripMenuItem_Click);
            // 
            // toolBarToolStripMenuItem
            // 
            this.toolBarToolStripMenuItem.Name = "toolBarToolStripMenuItem";
            this.toolBarToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.toolBarToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.toolBarToolStripMenuItem.Text = "Tool bar";
            this.toolBarToolStripMenuItem.Click += new System.EventHandler(this.showhide_toolbar);
            // 
            // markerBrowserToolStripMenuItem1
            // 
            this.markerBrowserToolStripMenuItem1.Name = "markerBrowserToolStripMenuItem1";
            this.markerBrowserToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.markerBrowserToolStripMenuItem1.Size = new System.Drawing.Size(175, 22);
            this.markerBrowserToolStripMenuItem1.Text = "Marker browser";
            this.markerBrowserToolStripMenuItem1.Click += new System.EventHandler(this.showhide_markerBrowser);
            // 
            // timeLineToolStripMenuItem1
            // 
            this.timeLineToolStripMenuItem1.Name = "timeLineToolStripMenuItem1";
            this.timeLineToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.timeLineToolStripMenuItem1.Size = new System.Drawing.Size(175, 22);
            this.timeLineToolStripMenuItem1.Text = "Time line";
            this.timeLineToolStripMenuItem1.Click += new System.EventHandler(this.showhide_timeline);
            // 
            // statusBarToolStripMenuItem
            // 
            this.statusBarToolStripMenuItem.Name = "statusBarToolStripMenuItem";
            this.statusBarToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.statusBarToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.statusBarToolStripMenuItem.Text = "Status bar";
            this.statusBarToolStripMenuItem.Click += new System.EventHandler(this.showhide_statusbar);
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(106, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Frame:";
            // 
            // numericUpDownN
            // 
            this.numericUpDownN.Location = new System.Drawing.Point(151, 3);
            this.numericUpDownN.Name = "numericUpDownN";
            this.numericUpDownN.Size = new System.Drawing.Size(57, 20);
            this.numericUpDownN.TabIndex = 10;
            this.numericUpDownN.ValueChanged += new System.EventHandler(this.numericUpDownN_ValueChanged);
            // 
            // timeLine1
            // 
            this.timeLine1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.timeLine1.Location = new System.Drawing.Point(0, 362);
            this.timeLine1.Name = "timeLine1";
            this.timeLine1.Size = new System.Drawing.Size(425, 58);
            this.timeLine1.TabIndex = 22;
            // 
            // toolSet_bar1
            // 
            this.toolSet_bar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolSet_bar1.Location = new System.Drawing.Point(0, 24);
            this.toolSet_bar1.Name = "toolSet_bar1";
            this.toolSet_bar1.Size = new System.Drawing.Size(425, 32);
            this.toolSet_bar1.TabIndex = 19;
            // 
            // markerListViewer1
            // 
            this.markerListViewer1.Dock = System.Windows.Forms.DockStyle.Right;
            this.markerListViewer1.Location = new System.Drawing.Point(425, 0);
            this.markerListViewer1.Name = "markerListViewer1";
            this.markerListViewer1.Size = new System.Drawing.Size(307, 420);
            this.markerListViewer1.TabIndex = 20;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 420);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(732, 22);
            this.statusStrip1.TabIndex = 23;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(78, 17);
            this.toolStripStatusLabel1.Text = "BagFinder 3.0";
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 442);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.timeLine1);
            this.Controls.Add(this.toolSet_bar1);
            this.Controls.Add(this.numericUpDownN);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.markerListViewer1);
            this.Controls.Add(this.statusStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "BagFinder";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownN)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bagMarkerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetViewToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem saveMarkersListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadMarkersListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomFitToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownN;
        private System.Windows.Forms.ToolStripMenuItem loadFileToolStripMenuItem;
        private System.Windows.Forms.ColorDialog colorDialog2;
        private System.Windows.Forms.ToolStripMenuItem openTIFFPCOToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recordSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private ToolSetBar toolSet_bar1;
        private MarkerListViewer markerListViewer1;
        private TimeLine timeLine1;
        private System.Windows.Forms.ToolStripMenuItem panelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem timeLineToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem markerBrowserToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolBarToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem statusBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fullscreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem saveImagesToolStripMenuItem;
    }
}

