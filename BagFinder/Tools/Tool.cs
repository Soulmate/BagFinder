using System;
using System.Windows.Forms;

namespace BagFinder.Tools
{
    internal abstract class Tool
    {
        public virtual bool Paint(System.Drawing.Graphics g) { return false; }  //возвращаемое значение - надо ли прервать опрос инструментов
        public virtual bool MouseDown(MouseEventArgs e) { return false; }
        public virtual bool MouseMove(MouseEventArgs e) { return false; }
        public virtual bool MouseUp(MouseEventArgs e) { return false; }
        public virtual bool MouseWheel(MouseEventArgs e) { return false; }
        public virtual bool KeyDown(KeyEventArgs e) { return false; }
        public virtual bool KeyUp(KeyEventArgs e) { return false; }

        public ToolSet OwnerToolSet { get; }
        
        public string Text;

        protected Tool(ToolSet ownerToolSet)
        {
            OwnerToolSet = ownerToolSet;
        }
    }
}
