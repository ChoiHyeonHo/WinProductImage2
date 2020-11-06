using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WinProductImage
{
    class CommonUtil
    {
        public static void SetInitGridView(DataGridView dgv)
        {
            dgv.AutoGenerateColumns = true; //데이터소스의 컬럼이 자동으로 바인딩, 수동으로 Colums.Add()
            dgv.AllowUserToAddRows = false; //제일 마지막 줄에 추가로 한 줄이 생김
            dgv.MultiSelect = false; // 여러개 선택되지 않게.
            //SelectedRows.Count
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //전체 행 선택
        }

        public static void AddGridTextColumn( 
                           DataGridView dgv, 
                           string headertext,
                           string dataPropertyName,
                           int colWidth = 100,
                           bool visibility = true,
                           DataGridViewContentAlignment textalien = DataGridViewContentAlignment.MiddleLeft)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = headertext;
            col.DataPropertyName = dataPropertyName;
            col.Width = colWidth;
            col.DefaultCellStyle.Alignment = textalien;
            col.Visible = visibility;
            col.ReadOnly = true;
            dgv.Columns.Add(col);
        }


    }
}
