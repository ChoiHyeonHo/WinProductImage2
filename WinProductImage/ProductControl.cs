using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinProductImage
{
    public partial class ProductControl : UserControl
    {
        public delegate void AddCartEventHandler(object sender, AddCartEventAgrs e);
        public event AddCartEventHandler AddCart; //이벤트정의

        //public event EventHandler AddCart; //이벤트정의

        int productID;
        public int ProductID 
        {
            get { return productID; } 
            set { productID = value; }
        }
        public string ProdName
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }
        public int ProductPrice
        {
            get { return int.Parse(lblPrice.Text.Replace("원", "")); }
            set { lblPrice.Text = $"{value}원"; }
        }
        public string ProductImage
        {
            get { return pictureBox1.ImageLocation; }
            set { pictureBox1.ImageLocation = value; }
        }

        public ProductControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(lblName.Text);

            if (AddCart != null) //등록된 이벤트 핸들러가 있는지 체크
            {
                AddCartEventAgrs args = new AddCartEventAgrs();
                args.ProductID = productID;
                args.ProdName = ProdName;
                args.ProductPrice = ProductPrice;

                AddCart(this, args); //이벤트 발생
            }
        }
    }

    public class AddCartEventAgrs : EventArgs
    {
        int productID, productPrice;
        string productName;
        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }
        public string ProdName
        {
            get { return productName; }
            set { productName = value; }
        }
        public int ProductPrice
        {
            get { return productPrice; }
            set { productPrice = value; }
        }
    }
}
