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
    public partial class Form4 : Form
    {
        CheckBox headerCheckBox = new CheckBox(); //1
        
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            CommonUtil.SetInitGridView(dataGridView1);


            DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
            col.Name = "chk";
            col.HeaderText = "";
            col.Width = 30;
            dataGridView1.Columns.Add(col); //2

            Point headerCellLocation = dataGridView1.GetCellDisplayRectangle(0, -1, true).Location;
            //헤더부분의 체크박스
            headerCheckBox.Location = new Point(headerCellLocation.X + 8, headerCellLocation.Y + 2);
            headerCheckBox.Size = new Size(18, 18);
            headerCheckBox.BackColor = Color.Transparent;
            headerCheckBox.Click += HeaderCheckBox_Click;
            dataGridView1.Controls.Add(headerCheckBox); //3

            CommonUtil.AddGridTextColumn(dataGridView1, "제품 ID", "productID", 70, true);
            CommonUtil.AddGridTextColumn(dataGridView1, "제품명", "productName", 200, true);
            CommonUtil.AddGridTextColumn(dataGridView1, "제품가격", "productPrice", 100, true);

            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.HeaderText = "관리";
            btn.Name = "Edit";
            btn.Text = "수정";
            btn.Width = 50;
            btn.DefaultCellStyle.Padding = new Padding(5, 40, 5, 40);//안쪽 여백 설정
            btn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btn); // 4번째 컬럼

            btn = new DataGridViewButtonColumn();
            btn.HeaderText = "관리";
            btn.Name = "Del";
            btn.Text = "삭제";
            btn.Width = 50;
            btn.DefaultCellStyle.Padding = new Padding(5,40,5,40);//안쪽 여백 설정
            btn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btn); // 5번째 컬럼

            DataGridViewImageColumn img = new DataGridViewImageColumn();
            //컬럼에 이미지 또는 값을 주면 모두 같은 값으로 바인딩.
            //img.Image = Image.FromFile(@"C:\Users\GDC6\Desktop\Image/홈런볼.jpg");
            img.ImageLayout = DataGridViewImageCellLayout.Stretch;
            img.DataPropertyName = "productImage";
            //BLOB인 경우 byte[]로 데이터 소스에 갖고 있기때문에 byte[] => Image 생성
            dataGridView1.Columns.Add(img); //6번째 컬럼

            DataGridViewComboBoxColumn cbo = new DataGridViewComboBoxColumn();
            cbo.HeaderText = "권한";
            cbo.Items.Add("일반회원");
            cbo.Items.Add("관리자");
            cbo.DefaultCellStyle.Padding = new Padding(0, 40, 0, 40);
            dataGridView1.Columns.Add(cbo); //7번째 컬럼


            dataGridView1.RowTemplate.Height = 100; //Data Row의 높이
            DataLoad();
        }

        private void HeaderCheckBox_Click(object sender, EventArgs e) //4
        {
            dataGridView1.EndEdit(); //현재 cell의 편집모드(포커스가 있을때)를 종료 => Commit

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["chk"];
                chk.Value = headerCheckBox.Checked;
            }
        }

        private void DataLoad() // 데이터 조회
        {
            ProductDB db = new ProductDB();
            dataGridView1.DataSource = db.GetProductList();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) //전체 선택후 하나 체크 해제시
        {
            //CellClick 이벤트는 자주 일어남. 셀 클릭하더라도 0번째가 아니면 일어나지 않게. 프로그램 부하 방지
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                //전체 0번째 CheckBox가 Check 해제된게 하나라도 있다면
                //HeaderCheckBox를 False로
                //모두 Check된 상태면 HeaderCheckBox를 true로 변경

                bool isChecked = true;
                dataGridView1.EndEdit();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["chk"].EditedFormattedValue) == false)
                    {
                        isChecked = false;
                        break;
                    }
                }
                headerCheckBox.Checked = isChecked;
            }

            else if (e.ColumnIndex == 4) //수정버튼이 4번째 컬럼
            {
                string msg = $@"수정 클릭: PID: {dataGridView1[1,e.RowIndex].Value} PName: {dataGridView1[2, e.RowIndex].Value} Price: {dataGridView1[3, e.RowIndex].Value}"; // 선택된 셀의 정보
                MessageBox.Show(msg);
            }
            else if (e.ColumnIndex == 5) //삭제버튼이 5번째 컬럼
            {
                string msg = $@"삭제 클릭: PID: {dataGridView1[1, e.RowIndex].Value} PName: {dataGridView1[2, e.RowIndex].Value} Price: {dataGridView1[3, e.RowIndex].Value}"; // 선택된 셀의 정보
                MessageBox.Show(msg);
            }
        }

        private void button1_Click(object sender, EventArgs e) //path저장
        {
            ProductDB db = new ProductDB();
            DataTable dt = db.GetProductListImage();
            dataGridView1.DataSource = dt;
            db.Dispose();
            for (int i = 0; i < dt.Rows.Count ; i++)
            {
                Image img = Image.FromFile(dt.Rows[i]["productImgFileName"].ToString()); //경로로부터 이미지 생성
                dataGridView1.Rows[i].Cells[6].Value = img;
            }
            dataGridView1.ClearSelection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ProductDB db = new ProductDB();
            DataTable dt = db.GetProductListImageBLOB();
            dataGridView1.DataSource = dt;
            db.Dispose();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //DataSource를 Set했을때 바인딩이 완료 되고, 눈에 보이기 전에 일어나는 이벤트
            // for 문으로 Loop돌면서 값 체크해서 색상 변경

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int price = Convert.ToInt32(dataGridView1[3, i].Value);
                if (price >= 2000)
                {
                    dataGridView1[2, i].Style = dataGridView1[3, i].Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                }
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)//우클릭
            {
                //마우스 다운이 일어난 셀을 선택하고, 컨텍스트 메뉴를 출력
                dataGridView1.Rows[e.RowIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1[2, e.RowIndex];//CurrentCell 선택된 셀
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void 수정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentRow.Index;

            string msg = $@"수정 클릭: PID: {dataGridView1[1, rowIndex].Value} PName: {dataGridView1[2, rowIndex].Value} Price: {dataGridView1[3, rowIndex].Value}";
            MessageBox.Show(msg);
        }

        private void 삭제ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentRow.Index;

            string msg = $@"삭제 클릭: PID: {dataGridView1[1, rowIndex].Value} PName: {dataGridView1[2, rowIndex].Value} Price: {dataGridView1[3, rowIndex].Value}";
            MessageBox.Show(msg);
        }
    }
}
