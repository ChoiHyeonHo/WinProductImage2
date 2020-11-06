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
    public partial class GudiDataGridView : DataGridView
    {
        public GudiDataGridView()
        {
            InitializeComponent();

            this.AutoGenerateColumns = false; //데이터소스의 컬럼이 자동으로 바인딩, 수동으로 Colums.Add()
            this.AllowUserToAddRows = false; //제일 마지막 줄에 추가로 한 줄이 생김
            this.MultiSelect = false; // 여러개 선택되지 않게.
            //SelectedRows.Count
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //전체 행 선택

            this.DefaultCellStyle.BackColor = Color.AntiqueWhite;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
