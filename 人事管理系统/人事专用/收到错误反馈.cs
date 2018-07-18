using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace 人事管理系统.人事专用
{
    public partial class 收到错误反馈 : Form
    {
        object a = "";
        string b = "";
        public 收到错误反馈()
        {
            InitializeComponent();
        }
        public void load()
        {
            收到错误反馈_Load(null, null);
        }
        private float X;
        private float Y;
        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    setTag(con);
            }
        }
        private void setControls(float newx, float newy, Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });
                float a = Convert.ToSingle(mytag[0]) * newx;
                con.Width = (int)a;
                a = Convert.ToSingle(mytag[1]) * newy;
                con.Height = (int)(a);
                a = Convert.ToSingle(mytag[2]) * newx;
                con.Left = (int)(a);
                a = Convert.ToSingle(mytag[3]) * newy;
                con.Top = (int)(a);
                Single currentSize = Convert.ToSingle(mytag[4]) * Math.Min(newx, newy);
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                {
                    setControls(newx, newy, con);
                }
            }
        }
        void Form1_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X;
            float newy = this.Height / Y;
            setControls(newx, newy, this);
        }
        private int z, s;
        private void 收到错误反馈_Load(object sender, EventArgs e)
        {
            z = Left;
            s = Top;
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
            this.Resize += new EventHandler(Form1_Resize);
            窗口.收到错误反馈 = this;
            try
            {
                //获取信息
                OleDbCommand sql1 = 窗口.conn.CreateCommand();
                sql1.CommandText = "SELECT 姓名,错误内容,编号,申请人工号 from 错误报告,人员信息 where 申请人工号=工号 and 是否处理=false";
                OleDbDataReader rea = sql1.ExecuteReader();
                if (rea.Read())
                {
                    label1.Text = rea.GetString(0);
                    textBox1.Text = rea.GetString(1);
                    a = rea.GetValue(2);
                    b = rea.GetString(3);
                }
                else
                {
                    MessageBox.Show("没有未处理的信息");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void 返回_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //去处理   调用更新信息 带参数的构造函数
            更新.更新详细信息 更新详细信息 = new 更新.更新详细信息(b,a);
            更新详细信息.Show();
            if (窗口.GetValue("默认大小") == "1")
            {
                更新详细信息.WindowState = FormWindowState.Normal;
            }
            else if (窗口.GetValue("默认大小") == "2")
            {
                更新详细信息.WindowState = FormWindowState.Maximized;
            }
            else
            {
                更新详细信息.WindowState = this.WindowState;
            }
        }

        private void 收到错误反馈_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
            {
                返回_Click(null, null);
            }
            if(e.KeyCode==Keys.Enter)
            {
                button1_Click(null, null);
            }
        }

        private void 收到错误反馈_VisibleChanged(object sender, EventArgs e)
        {
            Top = s;
            Left = z;
        }

        private void 收到错误反馈_FormClosed(object sender, FormClosedEventArgs e)
        {
            窗口.收到错误反馈 = null;
            if (窗口.GetValue("默认大小") == "1")
            {
                窗口.人事选择进入界面.WindowState = FormWindowState.Normal;
            }
            else if (窗口.GetValue("默认大小") == "2")
            {
                窗口.人事选择进入界面.WindowState = FormWindowState.Maximized;
            }
            else
            {
                窗口.人事选择进入界面.WindowState = this.WindowState;
            }
            窗口.人事选择进入界面.Focus();
        }
    }
}
