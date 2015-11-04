//Leonova Anna
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Filters
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap bm = (Bitmap)pictureBox1.Image;
                Blur(bm);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap bm = (Bitmap)pictureBox1.Image;
                FilterSobel(bm);
            }
        }

        private void FilterSobel(Bitmap bm)
        {            
            int w = bm.Width;
            int h = bm.Height;
            double ind = 0;
            int[,] gx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] gy = new int[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };

            int[,] allPixR = new int[w, h];
            int[,] allPixG = new int[w, h];
            int[,] allPixB = new int[w, h];

            int limit = 128 * 128;

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    allPixR[i, j] = bm.GetPixel(i, j).R;
                    allPixG[i, j] = bm.GetPixel(i, j).G;
                    allPixB[i, j] = bm.GetPixel(i, j).B;
                }
            }

            int new_rx, new_ry;
            int new_gx, new_gy;
            int new_bx, new_by;
            int rc, gc, bc;
            for (int i = 1; i < bm.Width - 1; i++)
            {
                for (int j = 1; j < bm.Height - 1; j++)
                {
                    new_rx = 0;
                    new_ry = 0;
                    new_gx = 0;
                    new_gy = 0;
                    new_bx = 0;
                    new_by = 0;
                    rc = 0;
                    gc = 0;
                    bc = 0;

                    for (int wi = -1; wi < 2; wi++)
                    {
                        for (int hw = -1; hw < 2; hw++)
                        {
                            ind = (i * 1.0 / (bm.Width - 2));
                            progressBar1.Value = (int)(ind * 100);
                            Application.DoEvents();

                            rc = allPixR[i + hw, j + wi];
                            new_rx += gx[wi + 1, hw + 1] * rc;
                            new_ry += gy[wi + 1, hw + 1] * rc;

                            gc = allPixG[i + hw, j + wi];
                            new_gx += gx[wi + 1, hw + 1] * gc;
                            new_gy += gy[wi + 1, hw + 1] * gc;

                            bc = allPixB[i + hw, j + wi];
                            new_bx += gx[wi + 1, hw + 1] * bc;
                            new_by += gy[wi + 1, hw + 1] * bc;
                        }
                    }
                    if (new_rx * new_rx + new_ry * new_ry > limit || new_gx * new_gx + new_gy * new_gy > limit || new_bx * new_bx + new_by * new_by > limit)
                        bm.SetPixel(i, j, Color.Transparent);

                    else
                        bm.SetPixel(i, j, Color.Black);
                }
            }
            progressBar1.Value = 0;
            pictureBox1.Refresh();
        }

        private void Blur(Bitmap bm)
        {
            int w = bm.Width;
            int h = bm.Height;
            double ind = 0;

            // horizontal blur
            for (int i = 1; i < w - 1; i++)
            {
                ind = (i * 0.5 / w);
                progressBar1.Value = (int)(ind * 100);
                for (int j = 0; j < h; j++)
                {
                    Application.DoEvents();
                    Color c1 = bm.GetPixel(i - 1, j);
                    Color c2 = bm.GetPixel(i, j);
                    Color c3 = bm.GetPixel(i + 1, j);

                    byte gR = (byte)((c1.R + c2.R + c3.R) / 3);
                    byte gG = (byte)((c1.G + c2.G + c3.G) / 3);
                    byte gB = (byte)((c1.B + c2.B + c3.B) / 3);

                    Color cBlured = Color.FromArgb(gR, gG, gB);
                    bm.SetPixel(i, j, cBlured);
                }
            }

            // vertical blur
            for (int i = 0; i < w; i++)
            {
                ind = 0.5 + i * 0.5 / (w - 1);
                progressBar1.Value = (int)(ind * 100);
                for (int j = 1; j < h - 1; j++)
                {
                    Application.DoEvents();
                    Color c1 = bm.GetPixel(i, j - 1);
                    Color c2 = bm.GetPixel(i, j);
                    Color c3 = bm.GetPixel(i, j + 1);

                    byte gR = (byte)((c1.R + c2.R + c3.R) / 3);
                    byte gG = (byte)((c1.G + c2.G + c3.G) / 3);
                    byte gB = (byte)((c1.B + c2.B + c3.B) / 3);

                    Color cBlured = Color.FromArgb(gR, gG, gB);
                    bm.SetPixel(i, j, cBlured);
                }
            }
            progressBar1.Value = 0;
            pictureBox1.Refresh();
        }
    }
}
