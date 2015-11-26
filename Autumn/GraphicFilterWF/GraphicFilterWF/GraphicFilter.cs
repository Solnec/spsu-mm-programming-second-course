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
using Timer = System.Threading.Timer;

namespace GraphicFilterWF
{
    public partial class GraphicFilter : Form
    {
        FilterModel model = new FilterModel();
        private Timer _progress;
        public GraphicFilter()
        {
            InitializeComponent();
            cmbFilters.DataSource = model.FilterList;
            model.Filter = (FilterModel.Filters)cmbFilters.SelectedIndex;
            model.EndOfApply += ShowImage;

        }

        private void cmbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            model.Filter = (FilterModel.Filters)cmbFilters.SelectedIndex;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
            model.InPath = openFileDialog.FileName;
            model.Load();
            pictureBox.Image = model.OldImage();
        }

        private Thread _t;
        private void btnApply_Click(object sender, EventArgs e)
        {
            if (_progress != null)
                _progress.Dispose();
            model.Update();
            progressBar.Value = 0;
            progressBar.Maximum = model.Size;
            if (_t != null && _t.IsAlive)
            {
                _t.Abort();
            }
            _t = new Thread(model.Apply);
            _t.Start();
            _progress = new Timer(ShowProgress, null, 10, 20);
        }

        private void ShowImage()
        {
            pictureBox.Image = model.NewImage();
            _progress.Dispose();
        }

        private void ShowProgress(object state)
        {
            this.Invoke(new ThreadStart(delegate { progressBar.Value = model.Progress(); }));
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
