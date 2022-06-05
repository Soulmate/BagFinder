namespace BagFinder.Forms
{
    partial class FormRecordSettings
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
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown_recFPS = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_recScale = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.checkBoxRotate = new System.Windows.Forms.CheckBox();
            this.nl1 = new System.Windows.Forms.NumericUpDown();
            this.nl2 = new System.Windows.Forms.NumericUpDown();
            this.checkBoxInvert = new System.Windows.Forms.CheckBox();
            this.checkBoxDolevels = new System.Windows.Forms.CheckBox();
            this.buttonAutolevels = new System.Windows.Forms.Button();
            this.numericUpDown_FrameTimeZero = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.button_SetZeroFrame = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_recFPS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_recScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FrameTimeZero)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "Scale (mm/px)";
            // 
            // numericUpDown_recFPS
            // 
            this.numericUpDown_recFPS.DecimalPlaces = 3;
            this.numericUpDown_recFPS.Location = new System.Drawing.Point(115, 38);
            this.numericUpDown_recFPS.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown_recFPS.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numericUpDown_recFPS.Name = "numericUpDown_recFPS";
            this.numericUpDown_recFPS.Size = new System.Drawing.Size(118, 20);
            this.numericUpDown_recFPS.TabIndex = 30;
            this.numericUpDown_recFPS.Value = new decimal(new int[] {
            4500,
            0,
            0,
            0});
            this.numericUpDown_recFPS.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // numericUpDown_recScale
            // 
            this.numericUpDown_recScale.DecimalPlaces = 5;
            this.numericUpDown_recScale.Location = new System.Drawing.Point(115, 13);
            this.numericUpDown_recScale.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown_recScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            393216});
            this.numericUpDown_recScale.Name = "numericUpDown_recScale";
            this.numericUpDown_recScale.Size = new System.Drawing.Size(118, 20);
            this.numericUpDown_recScale.TabIndex = 31;
            this.numericUpDown_recScale.Value = new decimal(new int[] {
            22,
            0,
            0,
            196608});
            this.numericUpDown_recScale.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Fps (Hz)";
            // 
            // buttonDefault
            // 
            this.buttonDefault.Location = new System.Drawing.Point(94, 338);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(75, 23);
            this.buttonDefault.TabIndex = 33;
            this.buttonDefault.Text = "Default";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click_1);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(13, 338);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 34;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click_1);
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(175, 338);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 35;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // checkBoxRotate
            // 
            this.checkBoxRotate.AutoSize = true;
            this.checkBoxRotate.Location = new System.Drawing.Point(20, 101);
            this.checkBoxRotate.Name = "checkBoxRotate";
            this.checkBoxRotate.Size = new System.Drawing.Size(125, 17);
            this.checkBoxRotate.TabIndex = 36;
            this.checkBoxRotate.Text = "Rotate image 90 deg";
            this.checkBoxRotate.UseVisualStyleBackColor = true;
            this.checkBoxRotate.CheckedChanged += new System.EventHandler(this.ValueChanged);
            // 
            // nl1
            // 
            this.nl1.Location = new System.Drawing.Point(94, 124);
            this.nl1.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nl1.Name = "nl1";
            this.nl1.Size = new System.Drawing.Size(64, 20);
            this.nl1.TabIndex = 30;
            this.nl1.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // nl2
            // 
            this.nl2.Location = new System.Drawing.Point(164, 124);
            this.nl2.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nl2.Name = "nl2";
            this.nl2.Size = new System.Drawing.Size(64, 20);
            this.nl2.TabIndex = 30;
            this.nl2.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // checkBoxInvert
            // 
            this.checkBoxInvert.AutoSize = true;
            this.checkBoxInvert.Location = new System.Drawing.Point(20, 150);
            this.checkBoxInvert.Name = "checkBoxInvert";
            this.checkBoxInvert.Size = new System.Drawing.Size(84, 17);
            this.checkBoxInvert.TabIndex = 36;
            this.checkBoxInvert.Text = "Invert image";
            this.checkBoxInvert.UseVisualStyleBackColor = true;
            this.checkBoxInvert.CheckedChanged += new System.EventHandler(this.ValueChanged);
            // 
            // checkBoxDolevels
            // 
            this.checkBoxDolevels.AutoSize = true;
            this.checkBoxDolevels.Location = new System.Drawing.Point(20, 124);
            this.checkBoxDolevels.Name = "checkBoxDolevels";
            this.checkBoxDolevels.Size = new System.Drawing.Size(70, 17);
            this.checkBoxDolevels.TabIndex = 36;
            this.checkBoxDolevels.Text = "Do levels";
            this.checkBoxDolevels.UseVisualStyleBackColor = true;
            this.checkBoxDolevels.CheckedChanged += new System.EventHandler(this.ValueChanged);
            // 
            // buttonAutolevels
            // 
            this.buttonAutolevels.Location = new System.Drawing.Point(234, 121);
            this.buttonAutolevels.Name = "buttonAutolevels";
            this.buttonAutolevels.Size = new System.Drawing.Size(42, 23);
            this.buttonAutolevels.TabIndex = 37;
            this.buttonAutolevels.Text = "auto";
            this.buttonAutolevels.UseVisualStyleBackColor = true;
            this.buttonAutolevels.Click += new System.EventHandler(this.buttonAutolevels_Click);
            // 
            // numericUpDown_FrameTimeZero
            // 
            this.numericUpDown_FrameTimeZero.Location = new System.Drawing.Point(115, 64);
            this.numericUpDown_FrameTimeZero.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown_FrameTimeZero.Minimum = new decimal(new int[] {
            100000000,
            0,
            0,
            -2147483648});
            this.numericUpDown_FrameTimeZero.Name = "numericUpDown_FrameTimeZero";
            this.numericUpDown_FrameTimeZero.Size = new System.Drawing.Size(118, 20);
            this.numericUpDown_FrameTimeZero.TabIndex = 30;
            this.numericUpDown_FrameTimeZero.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Frame time zero";
            // 
            // button_SetZeroFrame
            // 
            this.button_SetZeroFrame.Location = new System.Drawing.Point(239, 61);
            this.button_SetZeroFrame.Name = "button_SetZeroFrame";
            this.button_SetZeroFrame.Size = new System.Drawing.Size(36, 23);
            this.button_SetZeroFrame.TabIndex = 38;
            this.button_SetZeroFrame.Text = "Set";
            this.button_SetZeroFrame.UseVisualStyleBackColor = true;
            this.button_SetZeroFrame.Click += new System.EventHandler(this.button_SetZeroFrame_Click);
            // 
            // FormRecordSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 373);
            this.Controls.Add(this.button_SetZeroFrame);
            this.Controls.Add(this.buttonAutolevels);
            this.Controls.Add(this.checkBoxDolevels);
            this.Controls.Add(this.checkBoxInvert);
            this.Controls.Add(this.checkBoxRotate);
            this.Controls.Add(this.buttonDefault);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nl2);
            this.Controls.Add(this.nl1);
            this.Controls.Add(this.numericUpDown_FrameTimeZero);
            this.Controls.Add(this.numericUpDown_recFPS);
            this.Controls.Add(this.numericUpDown_recScale);
            this.Name = "FormRecordSettings";
            this.Text = "FormRecordSettings";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_recFPS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_recScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FrameTimeZero)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown_recFPS;
        private System.Windows.Forms.NumericUpDown numericUpDown_recScale;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonDefault;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.CheckBox checkBoxRotate;
        private System.Windows.Forms.NumericUpDown nl1;
        private System.Windows.Forms.NumericUpDown nl2;
        private System.Windows.Forms.CheckBox checkBoxInvert;
        private System.Windows.Forms.CheckBox checkBoxDolevels;
        private System.Windows.Forms.Button buttonAutolevels;
        private System.Windows.Forms.NumericUpDown numericUpDown_FrameTimeZero;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_SetZeroFrame;
    }
}