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
    public partial class GudiTextBox : TextBox
    {
        public enum TextType { PK, NotNull, Common }

        TextType textType;
        public TextType InputTextType 
        {
            get { return textType; }
            set
            {
                textType = value;
                
                switch (value)
                {
                    case TextType.PK:
                        this.BackColor = Color.AliceBlue;
                        break;
                    case TextType.NotNull:
                        this.BackColor = System.Drawing.SystemColors.Info;
                        break;
                    case TextType.Common:
                        this.BackColor = Color.White;
                        break;
                }
            }
        }
        public GudiTextBox()
        {
            InitializeComponent();
            textType = TextType.Common;
        }

        //메서드명이 On...()인 경우 이벤트 핸들러 메서드.
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
