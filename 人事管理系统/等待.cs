using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 人事管理系统
{
    public partial class 等待 : Form
    {
        public 等待()
        {
            InitializeComponent();
        }

        private void 等待_Load(object sender, EventArgs e)
        {
            //注册窗体
            窗口.等待 = this;
        }

        private void 等待_FormClosed(object sender, FormClosedEventArgs e)
        {
            //注销窗体
            窗口.等待 = null;
        }
    }
}
