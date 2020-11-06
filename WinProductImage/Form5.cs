using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinProductImage
{
    public partial class Form5 : Form
    {
        DataTable dt;
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            ProductDB db = new ProductDB();
            dt = db.GetProductList();

            bindingSource1.DataSource = dt;
            bindingNavigator1.BindingSource = bindingSource1;
            dataGridView1.DataSource = bindingSource1;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            DataTable dt = (DataTable)dataGridView1.DataSource;
            DataTable newDt = dt;
        }
    }
}
