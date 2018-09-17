using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap b_image;
        Bitmap black_white1;
        Bitmap black_white2;

        private void pictureBox1_Click(object sender, EventArgs e)
        {      	
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"; 
            if (open_dialog.ShowDialog() == DialogResult.OK) 
            {
              try
              {
                  b_image = new Bitmap(open_dialog.FileName);
                  pictureBox1.Image = b_image;
                  pictureBox1.Invalidate();
             }
             catch
             {
                  DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл",
                     "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null) 
            {
                black_white1 = new Bitmap(b_image.Width, b_image.Height);
                black_white2 = new Bitmap(b_image.Width, b_image.Height);

                for (int j = 0; j < b_image.Height; j++)
                    for (int i = 0; i < b_image.Width; i++)
                    {
                        UInt32 pixel = (UInt32)(b_image.GetPixel(i, j).ToArgb());

                        float R = (float)((pixel & 0x00FF0000) >> 16); 
                        float G = (float)((pixel & 0x0000FF00) >> 8); 
                        float B = (float)(pixel & 0x000000FF); 
                        
                        if (radioButton1.Checked)
                        {
                            R = G = B = (R + G + B) / 3.0f;
                            UInt32 newPixel = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | ((UInt32)B);
                            black_white1.SetPixel(i, j, Color.FromArgb((int)newPixel));
                        }       
                        else
                        {
                            R = G = B = (float)(0.2123 * (double)R + 0.7152 * (double)G + 0.0722 * (double)B) / 3.0f;
                            UInt32 newPixel = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | ((UInt32)B);
                            black_white2.SetPixel(i, j, Color.FromArgb((int)newPixel));
                        }
                     
                        
                    }
                if (radioButton1.Checked)
                 pictureBox2.Image = black_white1;
                else
                    pictureBox3.Image = black_white2;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap sub_image = new Bitmap(b_image.Width, b_image.Height);

            for (int j = 0; j < b_image.Height; j++)
                    for (int i = 0; i < b_image.Width; i++)
                    {
                        Color pixel1 = black_white1.GetPixel(i, j);
                        Color pixel2 = black_white2.GetPixel(i, j); 
                        UInt32 sub_pixel = (UInt32)Math.Max(pixel1.R - pixel2.R + pixel1.G - pixel2.G + pixel1.B - pixel2.B,
                                                    pixel2.R - pixel1.R + pixel2.G - pixel1.G + pixel2.B - pixel1.B);
                        sub_image.SetPixel(i, j, Color.FromArgb((int)sub_pixel));
                    }
            pictureBox1.Image = sub_image;
        }
    }
}
