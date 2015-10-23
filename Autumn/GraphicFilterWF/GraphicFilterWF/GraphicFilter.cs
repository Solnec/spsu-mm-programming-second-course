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

namespace GraphicFilterWF
{
    public partial class GraphicFilter : Form
    {
        FilterModel model = new FilterModel();
        public GraphicFilter()
        {
            InitializeComponent();
            cmbFilters.DataSource = model.FilterList;
            model.Filter = (FilterModel.Filters)cmbFilters.SelectedIndex;
        }

        private void cmbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            model.Filter = (FilterModel.Filters)cmbFilters.SelectedIndex;
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
            model.InPath = openFileDialog.FileName;
            model.Load();
            pictureBox.Image = model.OldImage();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            progressBar.Maximum = model.Size;
            progressBar.Value = 0;
            Thread t = new Thread(model.Apply);
            t.Start();
            Thread t2 = new Thread(ShowProgress);
            t2.Start();
        }

        private void ShowImage()
        {
            pictureBox.Image = model.NewImage();
        }

        private void ShowProgress()
        {
            model.Mut.WaitOne();
            while (progressBar.Value != progressBar.Maximum)
            {
                this.Invoke(new ThreadStart(delegate { progressBar.Value = model.Progress(); }));
                Thread.Sleep(0);
            }
            ShowImage();
            model.Mut.ReleaseMutex();
        }

        private void bntSave_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
            model.OutPath = saveFileDialog.FileName;
            model.Save();
            model.OutPath = model.InPath;
            pictureBox.Image = model.OldImage();
        }
    }
}
