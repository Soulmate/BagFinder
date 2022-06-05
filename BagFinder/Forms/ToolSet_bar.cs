using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BagFinder.Tools;

namespace BagFinder.Forms
{
    internal partial class ToolSetBar : UserControl
    {
        public ToolSetBar()
        {
            InitializeComponent();
        }

        private void ToolSet_bar_Load(object sender, EventArgs e)
        {

        }

        private readonly Dictionary<CheckBox, Tool> _buttonToolPairs = new Dictionary<CheckBox, Tool>();
        private readonly Dictionary<Tool, CheckBox> _toolButtonPairs = new Dictionary<Tool, CheckBox>();
        private ToolSet _toolSet;

        public void BindToToolSet(ToolSet toolSet)
        {
            _toolSet = toolSet;
            _toolSet.IsSwitchedTool += ToolSet_IsSwitchedTool;
            foreach (var t in toolSet.ToolsListCanBeActived)
            {
                var cb = new CheckBox
                {
                    Text = t.Text,
                    Appearance = Appearance.Button,
                    AutoSize = true
                };
                flowLayoutPanel1.Controls.Add(cb);

                _toolButtonPairs.Add(t, cb);
                _buttonToolPairs.Add(cb, t);

                cb.Click += Cb_Click;

                var k = t.OwnerToolSet.Shortcuts.FirstOrDefault(x => x.Value == t).Key;
                if (k != default(Keys))
                    cb.Text += $@" ({k.ToString()})";
            }
        }

        private void ToolSet_IsSwitchedTool(object sender, EventArgs e)
        {
            foreach (var tool in _toolSet.ToolsListCanBeActived)
            {
                _toolButtonPairs[tool].Checked = _toolSet.IsActive(tool);
            }
            
        }

        private void Cb_Click(object sender, EventArgs e)
        {
            var cb = (CheckBox)sender;
            var t = _buttonToolPairs[cb];
            t.OwnerToolSet.ActivateOnlyOrDeactivate(t);
        }
    }
}
