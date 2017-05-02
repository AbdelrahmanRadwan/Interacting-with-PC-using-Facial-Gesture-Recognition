using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /*int i = 0;
        string fn = string.Empty;
        string os = "0000";*/
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string fn = openFileDialog1.FileName;
            Bitmap B = PGMUtil.ToBitmap(fn);
            this.Size = B.Size;
            pictureBox1.Image = (Image)B;
            int s = fn.LastIndexOf('\\') + 1;
            this.Text = fn.Substring(s) + " (W: " + B.Width.ToString() + ", H: " + B.Height.ToString() + ")";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Press \"Enter\" to open an image");
        }

       /* private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X > pictureBox1.Width / 2)
                i++;
            if (e.X < pictureBox1.Width / 2)
                i--;
            showimage();
        }

        private void showimage()
        {
            string si = i.ToString();
            if (si.Length == 1)
                si = "000" + si;
            else if (si.Length == 2)
                si = "00" + si;
            else if (si.Length == 3)
                si = "0" + si;
            fn = fn.Replace(os, si);
            os = si;
            
            Bitmap B = PGMUtil.ToBitmap(fn);
            pictureBox1.Image = (Image)B;
            pictureBox1.Invalidate();

            int s = fn.LastIndexOf('\\') + 1;
            this.Text = fn.Substring(s) + " (W: " + B.Width.ToString() + ", H: " + B.Height.ToString() + ")";
        }*/
    }
}
