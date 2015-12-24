using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphicFilters;
using System.IO;

namespace FormFilters
{
    public partial class Form1 : Form
    {
        public static AutoResetEvent Event = new AutoResetEvent(true);
        public static AutoResetEvent Filter = new AutoResetEvent(false);
        public Thread WriterThread;
        public Thread PrBarThread;
        public int numberOfStarts = 1;

        public Form1()
        {
            InitializeComponent();
        }

        public void Start()
        {
            BMPFile myImage = new BMPFile();

            myImage.Read(textBox1.Text);
            pictureBox1.Size = new System.Drawing.Size(myImage.biWidth, myImage.biHeight);
            pictureBox1.Image = Bitmap.FromFile(textBox1.Text);

            BMPFile newImage = null;

            progressBar1.Maximum = myImage.biHeight;

            int filt = comboBox.SelectedIndex;

            Filters choosenFilter = new Filters();

            switch (filt)
            {
                case 0:
                    progressBar1.Maximum = myImage.biHeight - 1;
                    break;
                case 1:
                    progressBar1.Maximum = myImage.biHeight - 4;
                    break;
                case 2:
                    progressBar1.Maximum = myImage.biHeight - 2;
                    break;
                case 3:
                    progressBar1.Maximum = myImage.biHeight - 4;
                    break;
                case 4:
                    progressBar1.Maximum = myImage.biHeight - 2;
                    break;
                case 5:
                    progressBar1.Maximum = myImage.biHeight - 2;
                    break;
                default:
                    break;
            }

            WriterThread = new Thread(new ThreadStart(delegate
                {
                    this.Invoke(new ThreadStart(delegate
                        {
                            switch (filt)
                            {
                                case 0:
                                    newImage = choosenFilter.Grayscale(myImage);
                                    break;
                                case 1:
                                    newImage = choosenFilter.Gauss(myImage);
                                    break;
                                case 2:
                                    newImage = choosenFilter.Average3x3(myImage);
                                    break;
                                case 3:
                                    newImage = choosenFilter.Average5x5(myImage);
                                    break;
                                case 4:
                                    newImage = choosenFilter.SobelX(myImage);
                                    break;
                                case 5:
                                    newImage = choosenFilter.SobelY(myImage);
                                    break;
                                default:
                                    break;
                            }

                            newImage.Write(newImage, textBox2.Text);

                            Image Im = Bitmap.FromFile(textBox2.Text);
                            pictureBox2.Size = new System.Drawing.Size(myImage.biWidth, myImage.biHeight);
                            pictureBox2.Image = Im;
                        }));
                }));

            //WriterThread = new Thread(new ThreadStart(delegate
            //{
            //    newImage.Write(newImage, textBox2.Text);
            //    this.Invoke(new ThreadStart(delegate
            //                {
            //                    Image Im = Bitmap.FromFile(textBox2.Text);
            //                    pictureBox2.Size = new System.Drawing.Size(myImage.biWidth, myImage.biHeight);
            //                    pictureBox2.Image = Im;
            //                }));
            //}));

            WriterThread.Start();

            PrBarThread = new Thread(new ThreadStart(delegate
                {
                    while (progressBar1.Value != progressBar1.Maximum)
                    {
                        Event.Set();
                        this.Invoke(new ThreadStart(delegate
                            {
                                progressBar1.Value = choosenFilter.CountOfIteration;
                            }));
                        Filter.Set();
                        Event.WaitOne();
                    }
                }));
            PrBarThread.Start();

            
        }

        private void Input_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpFD = new OpenFileDialog();
            OpFD.Filter = "BMPImages |*.bmp";
            OpFD.Title = "Select file";

            if (OpFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                textBox1.Text = OpFD.FileName;
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            Clear();
            textBox2.Text = AppDomain.CurrentDomain.BaseDirectory + numberOfStarts.ToString() + ".bmp";
            numberOfStarts++;
            Event.Reset();
            Filter.Reset();
            PrBarThread = null;
            WriterThread = null;

            Start();
        }

        private void Clear()
        {
            this.Invoke(new ThreadStart(delegate { progressBar1.Value = 0; }));
        }
    }
}
