using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BagFinder.Main;

namespace BagFinder.Tools
{
    internal class ToolSet
    {
        public bool CaptureKeys = true;

        private readonly List<Tool> _toolsList = new List<Tool>();
        private readonly List<Tool> _toolsListAlwaysActive;
        public readonly List<Tool> ToolsListCanBeActived;

        public ToolCreateBag3 ToolCreateBag3;
        public ToolCreateBag5 ToolCreateBag5;
        public ToolCreatePoint ToolCreatePoint;
        public ToolCreateLine ToolCreateLine;
        public ToolCreateCross ToolCreateCross;
        public ToolEditMarker ToolEditMarkers;
        public ToolZoomRect ToolZoomRect;
        public ToolZoomWheel ToolZoomWheel;
        public ToolRewindControl ToolRewindControl;
        public ToolShowScaleCircle ToolShowScaleCircle;
        public ToolPan ToolPan;
        public ToolShowPos ToolShowPos;
        public ToolMiscHotkeys ToolMiscHotkeys;
        public Tool_create_edit_area_brush Tool_create_edit_area_brush;

        public string Text;

        public Dictionary<Keys, Tool>
            Shortcuts = new Dictionary<Keys, Tool>(); //горячие клавиши и тулзы, которые они вызывают

        public ToolSet()
        {
            ToolCreateBag3 = new ToolCreateBag3(this);
            ToolCreateBag5 = new ToolCreateBag5(this);
            ToolCreatePoint = new ToolCreatePoint(this);
            ToolCreateLine = new ToolCreateLine(this);
            ToolCreateCross = new ToolCreateCross(this);
            ToolEditMarkers = new ToolEditMarker(this);
            ToolZoomRect = new ToolZoomRect(this);
            ToolZoomWheel = new ToolZoomWheel(this);
            ToolRewindControl = new ToolRewindControl(this);
            ToolShowScaleCircle = new ToolShowScaleCircle(this);
            ToolMiscHotkeys = new ToolMiscHotkeys(this);
            Tool_create_edit_area_brush = new Tool_create_edit_area_brush(this);
            ToolPan = new ToolPan(this);
            ToolShowPos = new ToolShowPos(this);

            _toolsListAlwaysActive = new List<Tool>
            {
                ToolEditMarkers, //важно, что он до, чтобы если попадем в хэндл, не срабатывало создание    
                ToolRewindControl, // TODO перед зумвил 
                ToolZoomWheel,
                ToolShowScaleCircle,
                ToolPan,
                ToolShowPos,
                ToolMiscHotkeys
            };
            ToolsListCanBeActived = new List<Tool>
            {
                ToolCreatePoint,
                ToolCreateLine,
                ToolCreateCross,
                ToolCreateBag3,
                ToolCreateBag5,
                Tool_create_edit_area_brush,
                ToolZoomRect                
            };
            
            _toolsList.AddRange(_toolsListAlwaysActive);

            Shortcuts.Add(Keys.D1, ToolCreatePoint);
            Shortcuts.Add(Keys.D2, ToolCreateLine);
            Shortcuts.Add(Keys.D3, ToolCreateCross);
            Shortcuts.Add(Keys.D4, ToolCreateBag3);
            Shortcuts.Add(Keys.D5, ToolCreateBag5);
            //Shortcuts.Add(Keys.Z, ToolZoomRect);
            Shortcuts.Add(Keys.D6, Tool_create_edit_area_brush);
        }

        #region Events

        public void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!CaptureKeys) return;

            switch (e.KeyData)
            {
                case Keys.Oemtilde:
                    DeactivateAll();
                    break;
            }

            if (Shortcuts.ContainsKey(e.KeyData))
                ActivateOnlyOrDeactivate(Shortcuts[e.KeyData]);

            foreach (var tool in _toolsList)
                if (tool.KeyDown(e))
                    break;
        }

        public void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!CaptureKeys) return;

            foreach (var tool in _toolsList)
                if (tool.KeyUp(e))
                    break;
        }

        public void Pb_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var tool in _toolsList)
                if (tool.MouseDown(e))
                    break;
        }

        public void Pb_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (var tool in _toolsList)
                if (tool.MouseMove(e))
                    break;
        }

        public void Pb_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (var tool in _toolsList)
                if (tool.MouseUp(e))
                    break;
        }

        public void Pb_MouseWheel(object sender, MouseEventArgs e)
        {
            foreach (var tool in _toolsList)
                if (tool.MouseWheel(e))
                    break;
        }

        public void Pb_Paint(Graphics g)
        {
            foreach (var tool in _toolsList)
            {
                tool.Paint(g);
            }
        }

        #endregion

        #region Activeate-Deactivate

        public bool IsActive(Tool tool)
        {
            return _toolsList.Contains(tool);
        }

        public void Activate(Tool tool)
        {
            if (!_toolsList.Contains(tool) && ToolsListCanBeActived.Contains(tool))
            {
                _toolsList.Add(tool);
                Text = tool.Text;
                On_is_switched_tool();
            }
        }

        public void Deactivate(Tool tool)
        {
            if (_toolsList.Contains(tool) && ToolsListCanBeActived.Contains(tool))
            {
                _toolsList.Remove(tool);
                Text = "";
                On_is_switched_tool();
            }
        }

        private void DeactivateAll()
        {
            if (_toolsList.RemoveAll(t => !_toolsListAlwaysActive.Contains(t)) > 0)
            {
                Text = "";
                On_is_switched_tool();
            }
        }

        public void ActivateOnly(Tool tool)
        {
            DeactivateAll();
            Activate(tool);
        }

        public void ActivateOnlyOrDeactivate(Tool tool)
        {
            if (!IsActive(tool))
            {
                ActivateOnly(tool);
            }
            else
            {
                Deactivate(tool);
            }
        }

        #endregion

        public string GetDefaultMarkerComment(string markerType)
        {
            try
            {
                var comArr = Program.ProgramSettings.DefaultComments[markerType];
                var comNum = 0;
                if (Control.ModifierKeys == Keys.Control)
                    comNum = 1;
                else if (Control.ModifierKeys == Keys.Shift)
                    comNum = 2;
                else if (Control.ModifierKeys == Keys.Alt)
                    comNum = 3;
                return comArr[comNum];
            }
            catch
            {
                return "";
            }
        }

        public event EventHandler IsSwitchedTool;

        public void On_is_switched_tool()
        {
            IsSwitchedTool?.Invoke(this, new EventArgs());
        }
    }
}