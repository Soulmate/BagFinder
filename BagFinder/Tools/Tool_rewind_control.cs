using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BagFinder.Main;

namespace BagFinder.Tools
{
    internal class ToolRewindControl : Tool
    {
        private bool _qGoToFrame = false;
        private Point _qGoToFrameInitialCursorPos;
        private int _qGoToFrameF0;

        public ToolRewindControl(ToolSet ownerToolSet) : base(ownerToolSet)
        {
            Text = "Rewind control";
        }

        public override bool MouseWheel(MouseEventArgs e)
        {
            if (_qGoToFrame)
            {
                Program.ProgramSettings.MouseRewindSens *= 1 + (double)Math.Sign(e.Delta) / 5;
                return true;
            }

            var record = Program.Record;
            if (record == null)
            {
                return false;
            }

            var d = e.Delta;

            if (Control.ModifierKeys == Keys.None)
            {
                Program.Rewinder.ImNum += -Math.Sign(d) * Program.ProgramSettings.WheelRewindFrameStep;
            }
            else if (Control.ModifierKeys == Keys.Shift)
            {
                if (d < 0)
                    Program.ProgramSettings.PlayStep = 0.5 * Program.ProgramSettings.PlayStep;
                else
                    Program.ProgramSettings.PlayStep = 2.0 * Program.ProgramSettings.PlayStep;
            }

            return false;
        }

        public override bool KeyDown(KeyEventArgs e)
        {

            var r = Program.Rewinder;

            if (e.KeyCode == Keys.Space)
            {
                if (e.Control)
                    r.PlayingForward = false;
                else
                    r.PlayingForward = true;
                r.Playing = !r.Playing;
            }

            if (e.Control && e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                if (r.PlayingForward == true)
                    r.Playing = !r.Playing;
                else
                    r.Playing = true;
                r.PlayingForward = true;
            }

            if (e.Control && e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                if (r.PlayingForward == false)
                    r.Playing = !r.Playing;
                else
                    r.Playing = true;
                r.PlayingForward = false;
            }

            //скорость просмотра:
            if (e.Control && e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                if (Program.ProgramSettings.PlayTimerInterval > 1)
                {
                    Program.ProgramSettings.PlayTimerInterval = (int)Math.Round(0.8 * (double)Program.ProgramSettings.PlayTimerInterval - 1);
                    Program.ProgramSettings.OnProgramSettingsChanged(this);
                }
                else
                    Program.ProgramSettings.PlayStep = 1.2 * Program.ProgramSettings.PlayStep + 1;
            }

            if (e.Control && e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                if (Program.ProgramSettings.PlayStep <= 1) //если уперлись в минимальное значение
                {
                    Program.ProgramSettings.PlayTimerInterval = (int)Math.Round(1.2 * (double)Program.ProgramSettings.PlayTimerInterval + 1);
                    Program.ProgramSettings.OnProgramSettingsChanged(this);
                }
                else
                    Program.ProgramSettings.PlayStep = 0.8 * Program.ProgramSettings.PlayStep - 1;
            }

            // перемотка
            if (e.KeyData == Keys.Q)
            {
                _qGoToFrame = true;
                _qGoToFrameInitialCursorPos = Cursor.Position;
                _qGoToFrameF0 = r.ImNum;
            }

            return false;
        }

        public override bool KeyUp(KeyEventArgs e)
        {
            // перемотка
            if (e.KeyData == Keys.Q)
            {
                _qGoToFrame = false;
            }

            return false;
        }

        public override bool MouseMove(MouseEventArgs e)
        {
            if (_qGoToFrame)
            {
                var pos = Cursor.Position;
                if (Program.ViewerImage.Pb.PointToClient(pos).X > 0.95 * (Program.ViewerImage.Pb.Width))
                {
                    //pictureBox1.MouseMove -= Form1_MouseMove_GoToFrame;
                    Cursor.Position = new Point(pos.X - 200, pos.Y);
                    //pictureBox1.MouseMove += Form1_MouseMove_GoToFrame;
                    _qGoToFrameInitialCursorPos = Cursor.Position;
                    _qGoToFrameF0 = Program.Rewinder.ImNum;
                    return true;
                }

                if (Program.ViewerImage.Pb.PointToClient(pos).X < 0.05 * Program.ViewerImage.Pb.Width)
                {
                    //pictureBox1.MouseMove -= Form1_MouseMove_GoToFrame;
                    Cursor.Position = new Point(pos.X + 200, pos.Y);
                    //pictureBox1.MouseMove += Form1_MouseMove_GoToFrame;
                    _qGoToFrameInitialCursorPos = Cursor.Position;
                    _qGoToFrameF0 = Program.Rewinder.ImNum;
                    return true;
                }

                var delta = e.X - Program.ViewerImage.Pb.PointToClient(_qGoToFrameInitialCursorPos).X;
                Program.Rewinder.ImNum =
                    (int)Math.Round(_qGoToFrameF0 + delta * (double)Program.ProgramSettings.MouseRewindSens);


                //int delta = e.X - pictureBox1.PointToClient(q_GoToFrame_initial_cursor_pos).X;
                //pictureBox1.MouseMove -= Form1_MouseMove_GoToFrame;
                //Cursor.Position = q_GoToFrame_initial_cursor_pos;
                //pictureBox1.MouseMove += Form1_MouseMove_GoToFrame;
                //GoToFrame((int)Math.Round(record.il.ImNum + delta * (double)numericUpDownPlayStep.Value / 10.0));
            }

            return false;
        }
    }
}