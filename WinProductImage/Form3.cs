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
    public partial class Form3 : Form
    {
        DataTable dtCart;

        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int r = int.Parse(textBox1.Text);
            int c = int.Parse(textBox2.Text);

            int idx = 1;

            for(int i=0; i<r; i++)
            {
                for(int k=0; k<c; k++)
                {
                    Button btn = new Button();
                    btn.Name = $"btn{idx}";
                    btn.Location = new Point((k*50) + 10, (i* 50) + 10);
                    btn.Size = new Size(25, 20);
                    btn.Text = idx.ToString();
                    btn.Click += Btn_Click;

                    panel1.Controls.Add(btn);
                    idx++;
                }
            }
            
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            MessageBox.Show(btn.Name);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int r = int.Parse(textBox1.Text);

            int idx = 1;

            for (int i = 0; i < r; i++)
            {
                for (int k = 0; k < 2; k++)
                {
                    ProductControl prod = new ProductControl();
                    prod.Name = $"prod{idx}";
                    prod.Location = new Point((k * 240) + 10, (i * 130) + 10);
                    prod.Size = new Size(224, 114);
                    
                    panel1.Controls.Add(prod);
                    idx++;
                }
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            //장바구니용 빈 테이블 생성
            InitCart();

            //DB에서 등록된 제품목록을 조회해서 바인딩
            ProductDB db = new ProductDB();
            DataTable dt = db.GetProductListImage();
            db.Dispose();

            //데이터 건수를 2열로 보여주기 위한 row수 계산
            int rowCnt = (int)Math.Ceiling(dt.Rows.Count / 2.0);

            int idx = 1;
            for (int i = 0; i < rowCnt; i++)
            {
                for (int k = 0; k < 2; k++)
                {
                    //데이터건수가 홀수인 경우를 위해서
                    if (idx > dt.Rows.Count)
                        break;

                    ProductControl prod = new ProductControl();
                    prod.Name = $"prod{idx}";
                    prod.Location = new Point((k * 240) + 10, (i * 130) + 10);
                    prod.Size = new Size(224, 114);

                    prod.ProductID = Convert.ToInt32(dt.Rows[idx - 1]["productID"]);
                    prod.ProdName = dt.Rows[idx - 1]["productName"].ToString();
                    prod.ProductPrice = Convert.ToInt32(dt.Rows[idx - 1]["productPrice"]);
                    prod.ProductImage = dt.Rows[idx - 1]["productImgFileName"].ToString();

                    prod.AddCart += Prod_AddCart;

                    panel1.Controls.Add(prod);
                    idx++;
                }
            }
        }

        private void InitCart()
        {
            dtCart = new DataTable();
            dtCart.Columns.Add(new DataColumn("pID", typeof(int)));
            dtCart.Columns.Add(new DataColumn("pName", typeof(string)));
            dtCart.Columns.Add(new DataColumn("pQty", typeof(int)));
            dtCart.Columns.Add(new DataColumn("pPrice", typeof(int)));
        }        

        private void Prod_AddCart(object sender, AddCartEventAgrs e)
        {
            DataRow dr = dtCart.NewRow();
            dr["pID"] = e.ProductID;
            dr["pName"] = e.ProdName;
            dr["pQty"] = 1;
            dr["pPrice"] = e.ProductPrice;
            dtCart.Rows.Add(dr);

            dtCart.AcceptChanges();

            dataGridView1.DataSource = dtCart;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(cartlist.Count.ToString());
        }

        //private void Prod_AddCart(object sender, EventArgs e)
        //{
        //    ProductControl ctrl = (ProductControl)sender;
        //    MessageBox.Show(ctrl.ProdName);
        //}
    }
}
