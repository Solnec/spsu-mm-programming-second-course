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

        Filters filter = new Filters();
        AutoResetEvent autoreset = new AutoResetEvent(false);

        public FormFilters()
        {
            InitializeComponent();
        }

        private void LoadImage(string AddressRead)
        {
            if (AddressRead == "")
            {
                MessageBox.Show("ERROR :( PLease, try again!!!");
                return;
            }

            filter.Image = filter.ReadImage(AddressRead);
            if (filter.Image == null)
            {
                MessageBox.Show("Ошибка при считывании картинки :(");
                return;
            }
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Load(AddressRead);
        }

        private void Load_Button_Click(object sender, EventArgs e)
        {
            autoreset.Reset();
            progressBar1.Value = newValue = 0;
            FileDialog.Filter = "Изображения |*.bmp";
            FileDialog.FileName = "";
            FileDialog.ShowDialog();

            AddressRead = FileDialog.FileName;

            Thread load = new Thread(delegate() { LoadImage(AddressRead); });
            load.Start();
            load.IsBackground = true;
        }

        private void Filter_Button_Click(object sender, EventArgs e)
        {
            autoreset.Reset();
            saveFile.Filter = "Изображения |*.bmp";
            saveFile.FileName = null;
            progressBar1.Value = newValue = filter.Progress = 0;
            int index = FiltersList.SelectedIndex + 1;

            saveFile.ShowDialog();
            AddressWrite = saveFile.InitialDirectory + saveFile.FileName;
            int Max = progressBar1.Maximum;

            if ((index < 0) || (filter.Image == null) || (AddressRead == saveFile.InitialDirectory + saveFile.FileName))
            {
                MessageBox.Show("ERROR :( PLease, try again!!!");
                return;
            }

            thread = new Thread(delegate() { filter.Filter(index, AddressWrite, autoreset); });
            thread.Start();
            thread.IsBackground = true;

            thread1 = new Thread(delegate() { Process(Max); });
            thread1.Start();
            thread.IsBackground = true;
        }

        private void FiltersList_SelectedIndexChanged(object sender, EventArgs e)
        {
            progressBar1.Value = newValue = filter.Progress = 0;
        }

        private void Process(int Max)
        {
            do
            {
                newValue = Max * filter.Progress / (filter.Image.Height * 2);

                autoreset.WaitOne();

                if (this.InvokeRequired)
                {
                    this.Invoke(new Proc(delegate() { this.progressBar1.Value = newValue; }));
                }
                else
                    this.progressBar1.Value = newValue;

                autoreset.Set();
            }
            while ((filter.IsAlive) || (newValue < Max));

            autoreset.WaitOne();

            if (filter.err)
            {
                MessageBox.Show("Ошибка записи");
                return;
            }

            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Proc(delegate() { this.pictureBox.Load(AddressWrite); }));
                }
                else
                    this.pictureBox.Load(AddressWrite);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка вывода:(");
                return;
            }

        }
    }
}
