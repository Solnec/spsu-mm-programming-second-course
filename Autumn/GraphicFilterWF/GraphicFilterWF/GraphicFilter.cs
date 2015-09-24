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

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
            model.InPath = openFileDialog.FileName;
            pictureBox.ImageLocation = model.InPath;
            pictureBox.Load();
            Thread t = new Thread(model.Load);
            t.Start();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            model.Apply();
            model.OutPath = model.Filter.ToString() + ".bmp";
            model.Save();
            pictureBox.ImageLocation = model.OutPath;
            pictureBox.Load();
        }
    }
}
