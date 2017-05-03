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
        string ShufflingAlgorithm; // The used algorithm in shuffling the training data set
        Bitmap BrowsedImage;//The Browsed Image through Browse Button
        
        public Form1()
        {
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e)
        {
            Loader.RunTheDataLoader(this);
        }

    }
}
