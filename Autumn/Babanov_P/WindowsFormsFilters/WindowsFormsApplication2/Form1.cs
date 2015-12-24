using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace WinFormsFilters
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Picture WorkingPicture;
        int n = 0;
        AutoResetEvent ResetEvent = new AutoResetEvent(false);
        private void OpenButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog Opener = new OpenFileDialog();
            Opener.Multiselect = false;
            Opener.Filter = "bmp files (*.bmp)|*.bmp";
            Opener.ShowDialog();
            string path = Opener.FileName;
            if((path == "") || (path == null))
                return;
            FileStream f_input = new FileStream(path, FileMode.Open, FileAccess.Read);
            f_input.Seek(18, SeekOrigin.Begin);
            Byte[] width = new Byte[4];
            Byte[] height = new Byte[4];
            f_input.Read(width, 0, 4);
            f_input.Read(height, 0, 4);
            f_input.Seek(0, SeekOrigin.Begin);
            Image img = Image.FromStream(f_input);
            f_input.Close();
            MainImage.Image = img;
            WorkingPicture = new Picture(BitConverter.ToInt32(width, 0), BitConverter.ToInt32(height, 0), ResetEvent);
            WorkingPicture.Load(path);
            Progress.Value = 0;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if(SwitchFilter.SelectedIndex == -1)
            {
                return;
            }
            Progress.Minimum = 0;
            Progress.Maximum = WorkingPicture.Height;
            Thread Worker = new Thread(Handler);
            Worker.Start(SwitchFilter.SelectedIndex);
            object[] param = { ResetEvent };
            Progress.Invoke(new Action<AutoResetEvent>(UpdaterProgress), param);
        }
        private void UpdaterProgress(AutoResetEvent Event)
        {
            while (Progress.Value != Progress.Maximum)
            {
                Event.WaitOne();
                Progress.PerformStep();
            }
        }
        private void Handler(object o)
        {
            switch (Convert.ToInt32(o))
            {
                case 0:
                    {
                        WorkingPicture.Mean3();
                        break;
                    }
                case 1:
                    {
                        WorkingPicture.Mean5();
                        break;
                    }
                case 2:
                    {
                        WorkingPicture.Gauss();
                        break;
                    }
                case 3:
                    {
                        WorkingPicture.Grayscale();
                        break;
                    }
                case 4:
                    {
                        WorkingPicture.SobelOnX();
                        break;
                    }
                case 5:
                    {
                        WorkingPicture.SobelOnY();
                        break;
                    }
            }
            WorkingPicture.Save(n);
            FileStream f_input = new FileStream("D:\\NewImage" + n.ToString() + ".bmp", FileMode.Open, FileAccess.Read);
            Image img = Image.FromStream(f_input);
            f_input.Close();
            MainImage.Image = img;
            n++;
        }
    }
}
