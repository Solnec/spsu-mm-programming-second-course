using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WFFilter
{
    public partial class Filter : Form
    {
        public Filter()
        {
            InitializeComponent();
        }

        public int flag = 0;
        delegate void Rds();

        public void TLoad()
        {
            Rds rds3 = delegate() { progressBar1.Value = 0; };
            if (this.InvokeRequired)
            {
                this.Invoke(rds3);
            }
            else
            {
                progressBar1.Value = 0;
            }
            string path = openFileDialog1.FileName;
            pictureBox1.Image = Image.FromFile(path);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            Interlocked.Exchange(ref flag, 0);
        }

        public void TFilter()
        {
            Interlocked.Exchange(ref flag, 1);
            string path = openFileDialog1.FileName;
            Bitmap bmp = new Bitmap(path);
            int width = bmp.Width;
            int height = bmp.Height;

            Bitmap greybmp = new Bitmap(bmp);
            Rds rds = delegate() { progressBar1.Maximum = height - 1; };
           
            if (this.InvokeRequired)
            {               
                this.Invoke(rds);
            }
            else
            {
                progressBar1.Maximum = height - 1;
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i);

                    int a = pixel.A;
                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;
                    int color = (r + g + b) / 3;
                    greybmp.SetPixel(j, i, Color.FromArgb(a, color, color, color));
                }

                if (flag == 0)
                {
                    Rds rds1 = delegate() { progressBar1.Value = 0; };
                    if (this.InvokeRequired)
                    {
                        this.Invoke(rds1);
                    }
                    else
                    {
                        progressBar1.Value = 0;
                    }
                    return;
                }

                else
                {
                    try
                    {
                        Rds rds2 = delegate() { progressBar1.Value = i; };
                        if (this.InvokeRequired)
                        {
                            this.Invoke(rds2);
                        }
                        else
                        {
                            progressBar1.Value = i;
                        }
                    }
                    catch(Exception)
                    {
                        progressBar1.Value = i;
                    }
                }
            }
            if (flag == 1)
                pictureBox1.Image = greybmp;
            return;
        }




        public Thread loadthread;
        private void loadbutton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            loadthread = new Thread(TLoad);
            loadthread.IsBackground = true;
            loadthread.Start();
        }
        public Thread filterthread;
        private void filtbutton_Click(object sender, EventArgs e)
        {
            filterthread = new Thread(TFilter);
            filterthread.IsBackground = true;
            filterthread.Start();
        }
    
    }

}

