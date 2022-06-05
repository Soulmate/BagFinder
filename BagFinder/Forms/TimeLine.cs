using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using BagFinder.Main;
using BagFinder.Markers;

namespace BagFinder.Forms
{
    public partial class TimeLine : UserControl
    {
        private double _tlm = 1; //масштаб таймлайна (кадров/пиксель)
        private double _tlo; //начало координат таймлайна (кадров)

        public TimeLine()
        {
            InitializeComponent();
        }

        public void Rewinder_FrameChanged()
        {
            //показать на таймлайне нынешний кадр //TODO
            var framePos = (float) ConvertFrameToPx(Program.Rewinder.ImNum); //меточка
            if (framePos < pictureBox1.Width * -0.01) //если меточка за пределами
            {
                var newTlo = ConvertPxToFrame(framePos - pictureBox1.Width * 0.02);
                _tlo = Math.Max(0, newTlo);
            }

            if (framePos > pictureBox1.Width * 1.01) //если меточка за пределами
            {
                var newTlo = ConvertPxToFrame(framePos - pictureBox1.Width * 0.98);
                _tlo = Math.Max(0, newTlo);
            }
        }

        public void Reset()
        {
            _tlo = 0;
            UpdateScrollbarParameters();
            pictureBox1.Invalidate();
        }

        private double ConvertPxToFrame(double p) //из координатной систмы пикчабокса в tl
        {
            return p * _tlm + _tlo;
        }

        private double ConvertFrameToPx(double f) //из координатной систмы tl в координатную систему пикчабокса
        {
            return (f - _tlo) / _tlm;
        }

        private void TlSetFrame(double value)
        {
            Program.Rewinder.ImNum = (int) Math.Round(ConvertPxToFrame(value));
            Program.ViewerImage.Invalidate();
            Program.ViewerTimeLine.Invalidate();
        }

        public new void Invalidate()
        {
            base.Invalidate();
            pictureBox1.Invalidate();
        }

        #region Обработка событий
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            var watch = Stopwatch.StartNew();

            var record = Program.Record;
            if (record == null) return;
            var g = e.Graphics;

            var framePos = (float)ConvertFrameToPx(Program.Rewinder.ImNum);
            g.DrawLine(Pens.Red, framePos, 20, framePos, 70);

            var maxP = (float) ConvertFrameToPx(Program.Rewinder.ImCount);
            g.FillRectangle(Brushes.Gray, maxP, 0, 2000, 2000);

            var tlDisplaySize = pictureBox1.Size.Width;
            var leftFrame = (int) _tlo;
            var rightFrame = (int) (_tlo + tlDisplaySize * _tlm);
            int min_px_distance = 30;
            var frameStep10Pow = (int) (Math.Floor(Math.Log10( min_px_distance * _tlm )) + 1);
            var frameStep = Math.Pow(10, frameStep10Pow);
            var leftFrameFrac = Math.Floor(leftFrame / frameStep) * frameStep;
            var rightFrameFrac = (Math.Floor(rightFrame / frameStep) + 1) * frameStep;
            for (var frameTick = leftFrameFrac; frameTick <= rightFrameFrac; frameTick += frameStep)
            {
                //Console.WriteLine("{0}", frameTick);
                var p = (float) ConvertFrameToPx(frameTick);
                g.DrawLine(Pens.Black, p, 0, p, 5);
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center
                };
                g.DrawString(frameTick.ToString(CultureInfo.CurrentCulture), new Font("Arial", 6), Brushes.Black, p, 5,
                    sf);
            }

            foreach (var m in record.MarkersList)
            {
                int verticalPos;
                Pen pen;
                if (m is MarkerBag3)
                {
                    verticalPos = 17;
                    pen = Pens.BlueViolet;
                }
                else if (m is MarkerBag5)
                {
                    verticalPos = 20;
                    pen = Pens.BlueViolet;
                }
                else if (m is MarkerPoint)
                {
                    verticalPos = 23;
                    pen = Pens.Orange;
                }
                else if (m is MarkerLine)
                {
                    verticalPos = 26;
                    pen = Pens.Orange;
                }
                else if (m is MarkerCross)
                {
                    verticalPos = 29;
                    pen = Pens.Black;
                }
                else
                {
                    verticalPos = 15;
                    pen = Pens.Green;
                }

                if (m.FrameStart.HasValue)
                {
                    var p = (float) ConvertFrameToPx(m.FrameStart.Value);
                    g.DrawLine(pen, p, verticalPos - 2, p, verticalPos + 2);
                }

                if (m.FrameEnd.HasValue)
                {
                    var p = (float) ConvertFrameToPx(m.FrameEnd.Value);
                    g.DrawLine(pen, p, verticalPos - 2, p, verticalPos + 2);
                }

                if (m.FrameStart.HasValue && m.FrameEnd.HasValue)
                {
                    var p1 = (float) ConvertFrameToPx(m.FrameStart.Value);
                    var p2 = (float) ConvertFrameToPx(m.FrameEnd.Value);
                    g.DrawLine(pen, p1, verticalPos, p2, verticalPos);
                }
            }

            //закешированные кадры
            foreach (var frame in record.Ic.Frames)
            {
                var verticalPos = 33;
                var p1 = (float) ConvertFrameToPx(frame);
                var p2 = (float) ConvertFrameToPx(frame + 1);
                g.DrawRectangle(Pens.Gray, p1, verticalPos, p2 - p1, 1);
            }

            watch.Stop();
            //Console.WriteLine("timeline {0} ms", watch.ElapsedMilliseconds);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (Program.Record == null) return;
            if (e.Button == MouseButtons.Left)
            {
                TlSetFrame(e.X);
                pictureBox1.MouseMove += pictureBox1_MouseMove;
                pictureBox1.MouseUp += pictureBox1_MouseUp;
            }

            if (e.Button == MouseButtons.Right) Program.Rewinder.Playing = !Program.Rewinder.Playing;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X >= 0 && e.X < pictureBox1.Width)
                TlSetFrame(e.X);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.MouseMove -= pictureBox1_MouseMove;
        }

        private void hScrollBar1_Scroll_1(object sender, ScrollEventArgs e)
        {
            _tlo = hScrollBar1.Value;
            pictureBox1.Invalidate();
        }

        public void Program_ProgramSettings_changed()
        {
            //новый масштаб
            var tlmNew = Program.ProgramSettings.TlScale;
            
            //пересчитаем tlo, чтобы позиция кадра сохранилась по возможности: значение (frame - _tlo) / _tlm сохраняется
            var frame = Program.Rewinder.ImNum;
            var framePx = ConvertFrameToPx(frame);
            var tloNew = frame - framePx * tlmNew;
            _tlo = tloNew.Clamp(0, MaxTlo);
            _tlm = tlmNew;
            UpdateScrollbarParameters();
            pictureBox1.Invalidate();
        }

        private int MaxTlo => (int) Math.Max(0, Math.Round(Program.Rewinder.ImCount - pictureBox1.Size.Width* _tlm));

        private void UpdateScrollbarParameters()
        {
            var max = MaxTlo;
            if (max <= 0)
                hScrollBar1.Visible = false;
            else
            {
                hScrollBar1.Visible = true;
                hScrollBar1.Maximum = max;
                hScrollBar1.Value = (int)Math.Round(_tlo);
            }
            
        }

        private int? _prevWidth;
        private void TimeLine_Resize(object sender, EventArgs e)
        {
            if (Program.Rewinder == null) return;
            if (Program.Form1?.WindowState != FormWindowState.Minimized && (_prevWidth == null || _prevWidth != pictureBox1.Size.Width))
            {
                _prevWidth = pictureBox1.Size.Width;

                var frame = Program.Rewinder.ImNum;
                var framePx = ConvertFrameToPx(frame);
                if (framePx > pictureBox1.Size.Width * 0.9)
                {
                    _tlo = (frame - (pictureBox1.Size.Width * 0.9) * _tlm).Clamp(0, MaxTlo);
                    UpdateScrollbarParameters();
                    pictureBox1.Invalidate();
                }
            }
        }
        #endregion


    }
}