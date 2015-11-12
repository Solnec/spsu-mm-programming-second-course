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
using System.IO;

namespace Filters
{
    public partial class FormFilters : Form
    {
        int newValue = 0;
        Thread thread, thread1;
        string AddressWrite, AddressRead;
        delegate void Proc();
        bool IsProcess = true;

        Filters filter = new Filters();
        AutoResetEvent autoreset = new AutoResetEvent(false);

        public FormFilters()
        {
            InitializeComponent();
        }

        private void ResetThreads()
        {
            progressBar1.Value = newValue = filter.Progress = 0;
            if ((filter.IsAlive) && (thread != null) && (thread1 != null))
            {
                autoreset.Reset();
                filter.IsAlive = false;
                IsProcess = false;
            }
        }

        private void LoadImage(string AddressRead)
        {
            try
            {
                filter.Image = new Bitmap(AddressRead);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при считывании файла");
                return;
            }

            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Image = filter.Image;
        }

        private void Load_Button_Click(object sender, EventArgs e)
        {
            ResetThreads();
            
            FileDialog.Filter = "Изображения |*.bmp";
            FileDialog.FileName = "";
            FileDialog.ShowDialog();

            AddressRead = FileDialog.FileName;

            thread = new Thread(delegate() { LoadImage(AddressRead); });
            thread.Start();
            thread.IsBackground = true;
        }

        private void Filter_Button_Click(object sender, EventArgs e)
        {
            ResetThreads();
            filter.Image = new Bitmap(pictureBox.Image);

            int index = FiltersList.SelectedIndex + 1;
            if ((index == 0) || (filter.Image == null))
            {
                MessageBox.Show("ERROR! PLease, try again!!!");
                return;
            }

            int Max = progressBar1.Maximum;
            int Width = filter.Image.Width;

            thread = new Thread(delegate() { filter.Filter(index, autoreset); });
            thread.IsBackground = true;
            thread.Start();

            thread1 = new Thread(delegate() { Process(Max, Width); });
            thread1.IsBackground = true;
            thread1.Start();

            IsProcess = true;
        }

        private void Process(int Max, int Width)
        {
            newValue = filter.Progress = 0;
            try
            {
                do
                {
                    autoreset.WaitOne();
                    newValue = Max * filter.Progress / Width;

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Proc(delegate() { progressBar1.Value = newValue; }));
                    }
                    else
                        progressBar1.Value = newValue;

                    autoreset.Set();
                }
                while (filter.IsAlive);

                autoreset.WaitOne();
                if (IsProcess)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Proc(delegate() { pictureBox.Image = filter.Image; }));
                    }
                    else
                        pictureBox.Image = filter.Image;
                }
            }

            catch (Exception)
            {
                return;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if ((filter.Image == null) || (filter.IsAlive))
                return;

            saveFile.Filter = "Изображения |*.bmp";
            saveFile.FileName = null;

            saveFile.ShowDialog();
            AddressWrite = saveFile.InitialDirectory + saveFile.FileName;

            if ((AddressRead == saveFile.InitialDirectory + saveFile.FileName) || (AddressWrite == ""))
            {
                MessageBox.Show("ERROR! PLease, try again!!!");
                return;
            }

            filter.Image.Save(AddressWrite);
        }
    }
}
