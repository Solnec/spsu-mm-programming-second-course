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
using Timer = System.Windows.Forms.Timer;

namespace GraphicFilterWF
{
    public partial class GraphicFilter : Form
    {
        FilterModel model = new FilterModel();
        private Timer _progress = new Timer();
        private object _lock = new object();
        public GraphicFilter()
        {
            InitializeComponent();
            cmbFilters.DataSource = model.FilterList;
            model.Filter = (FilterModel.Filters)cmbFilters.SelectedIndex;
            model.EndOfApply += ShowImage;
            _progress.Tick += ShowProgress;
            _progress.Interval = 20;

        }

        private void cmbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            model.Filter = (FilterModel.Filters)cmbFilters.SelectedIndex;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.Cancel)
                return;
            _progress.Stop();
            model.Update();
            progressBar.Value = 0;
            model.InPath = openFileDialog.FileName;
            model.Stop();
            model.Load();
            pictureBox.Image = model.OldImage();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {

            model.Stop();
            _progress.Stop();
            model.Update();
            progressBar.Value = 0;
            progressBar.Maximum = model.Size;
            model.Start();
            _progress.Start();

        }

        private void ShowImage()
        {
            this.Invoke(new ThreadStart(delegate { progressBar.Value = progressBar.Maximum; }));
            pictureBox.Image = model.NewImage();
            _progress.Stop();
        }

        private void ShowProgress(object sender, EventArgs e)
        {
            progressBar.Value = model.Progress();
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
