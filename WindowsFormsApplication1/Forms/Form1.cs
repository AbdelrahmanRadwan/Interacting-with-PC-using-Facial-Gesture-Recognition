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
        Data_Loader Loader;
        Bitmap BrowsedImage;//The Brosed Image through Browse Button
        
        public Form1()
        {
            InitializeComponent();
            MinimizeBox = false;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Loader = new Data_Loader();

        }

        private void BrowseButton_Clicked(object sender, EventArgs e)
        {
            //Clear Image
            pictureBox1.Image = null;
            ////////////////////////////////////////////////////////////
            //Browse
            OpenFileDialog BrowseImageDialog = new OpenFileDialog();
            if (BrowseImageDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    BrowsedImage = PGMUtil.ToBitmap(BrowseImageDialog.FileName);
                    pictureBox1.Image = BrowsedImage;
                }
                catch
                {
                    MessageBox.Show("Ops!\n Something went wrong!");
                }
            }

        }
    }
}
