using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BagFinder.Forms
{
    public partial class FormSaveImages : Form
    {
        public FormSaveImages()
        {
            InitializeComponent();
        }

        private void FormSaveImages_Load(object sender, EventArgs e)
        {
            numericUpDown1.Minimum = 0;
            numericUpDown1.Maximum = BagFinder.Main.Program.Rewinder.ImCount - 1;
            numericUpDown2.Minimum = 0;
            numericUpDown2.Maximum = BagFinder.Main.Program.Rewinder.ImCount - 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = saveFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int frameNum = (int) numericUpDown1.Value; frameNum < (int) numericUpDown2.Value; frameNum++)
            {
                BagFinder.Main.Program.Rewinder.ImNum = frameNum;
                var fileName = textBox1.Text;
                BagFinder.Main.Program.ViewerImage.SaveBitmap( 
                    $@"{Path.GetDirectoryName(fileName)}\{Path.GetFileNameWithoutExtension(fileName)}{frameNum:D8}{Path.GetExtension(fileName)}" 
                    );
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = BagFinder.Main.Program.Rewinder.ImNum;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            numericUpDown2.Value = BagFinder.Main.Program.Rewinder.ImNum;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BagFinder.Main.Program.Rewinder.ImNum = (int)numericUpDown1.Value;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            BagFinder.Main.Program.Rewinder.ImNum = (int)numericUpDown2.Value;
        }
    }
}
