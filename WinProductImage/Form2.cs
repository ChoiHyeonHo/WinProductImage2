using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinProductImage
{
    public partial class Form2 : Form
    {
        int productID;

        public Form2(int pid, string pname)
        {
            InitializeComponent();

            productID = pid;
            textBox1.Text = pname;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Images Files(*.jpg;*.gif;*.jpeg;*.png;*.bmp)|*.jpg;*.gif;*.jpeg;*.png;*.bmp";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = dlg.FileName;

                pictureBox1.Image = Image.FromFile(dlg.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Tag = dlg.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //이미지경로를 DB에 저장
            //로컬에서 선택한 이미지경로를 그대로 저장하면 안된다.
            //로컬에 이미지경로는 로컬PC에서만의 경로이므로, DB저장을 하고, 조회할때 다른 PC의 경로는 없을 수 있다.
            //실행파일의 상대경로로 파일을 복사해두고, 그 복사된 파일의 경로를 DB에 저장해야한다.

            try
            {
                string sPath = $"productImage/{productID}/";
                string localfile = pictureBox1.Tag.ToString();
                string sExt = localfile.Substring(localfile.LastIndexOf("."));
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + sExt;
                string destFileName = Path.Combine(sPath, newFileName);

                DirectoryInfo dir = new DirectoryInfo(sPath);
                if (!dir.Exists)
                {
                    dir.Create();
                }

                File.Copy(localfile, destFileName, true);

                ProductDB db = new ProductDB();
                bool bResult = db.AddProductImage(productID, destFileName);
                if (bResult)
                {
                    MessageBox.Show("이미지 추가가 완료되었습니다.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("이미지 저장에 실패하였습니다. 다시 시도하여 주십시오.");
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //BLOB 타입으로 이미지를 저장
            //이미지를 byte[] 로 변환해서, byte[]을 DB에 저장
            //장점:이미지자체가 DB에 저장되므로, 어디서든, 어떤환경에서든 사용
            //단점:이미지를 byte[] 변환할때, DB사이즈가 커진다
            //보안에 중요한 이미지 (서명, 개인정보 포함)

            //선택한 파일경로로부터 FileStream => BinaryReader
            try
            {
                FileStream fs = new FileStream(pictureBox1.Tag.ToString(), FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] imageData = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();

                ProductDB db = new ProductDB();
                bool bResult = db.AddProductImage(productID, imageData);
                if (bResult)
                {
                    MessageBox.Show("이미지 추가가 완료되었습니다.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("이미지 저장에 실패하였습니다. 다시 시도하여 주십시오.");
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
