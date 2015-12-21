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
        delegate void Proc();

        public void TLoad()
        {
            Proc proc1 = delegate() { progressbar.Value = 0; };
            if (this.InvokeRequired)
            {
                this.Invoke(proc1);
            }
            else
            {
                progressbar.Value = 0;
            }
            string path = choiceofimage.FileName;
            picturesbox.Image = Image.FromFile(path);
            picturesbox.SizeMode = PictureBoxSizeMode.StretchImage;
            Interlocked.Exchange(ref flag, 0);
        }

        public void TFilter()
        {
            Interlocked.Exchange(ref flag, 1);
            string path = choiceofimage.FileName;
            Bitmap bmp = new Bitmap(path);
            int width = bmp.Width;
            int height = bmp.Height;

            Bitmap greybmp = new Bitmap(bmp);
            Proc proc2 = delegate() { progressbar.Maximum = height - 1; };
           
            if (this.InvokeRequired)
            {               
                this.Invoke(proc2);
            }
            else
            {
                progressbar.Maximum = height - 1;
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
                    Proc proc3 = delegate() { progressbar.Value = 0; };
                    if (this.InvokeRequired)
                    {
                        this.Invoke(proc3);
                    }
                    else
                    {
                        progressbar.Value = 0;
                    }
                    return;
                }

                else
                {
                    try
                    {
                        Proc proc4 = delegate() { progressbar.Value = i; };
                        if (this.InvokeRequired)
                        {
                            this.Invoke(proc4);
                        }
                        else
                        {
                            progressbar.Value = i;
                        }
                    }
                    catch(Exception)
                    {
                        progressbar.Value = i;
                    }
                }
            }
            if (flag == 1)
                picturesbox.Image = greybmp;
            return;
        }




        public Thread loadthread;
        private void loadbutton_Click(object sender, EventArgs e)
        {
            choiceofimage.ShowDialog();
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

