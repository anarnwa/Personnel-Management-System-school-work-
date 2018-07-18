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
using System.IO;

namespace 人事管理系统.人事专用
{
    public partial class 人事选择进入界面 : Form
    {
        public 人事选择进入界面()
        {
            InitializeComponent();
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

        private void 人事选择进入界面_Load(object sender, EventArgs e)
        {
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
            this.Resize += new EventHandler(Form1_Resize);
            窗口.人事选择进入界面 = this;
        }

        private void 人事选择进入界面_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(窗口.请假信息!=null)
            {
                窗口.请假信息.Close();
            }
            if (窗口.编辑公告 != null)
            {
                窗口.编辑公告.Close();
            }
            if (窗口.更新详细信息 != null)
            {
                窗口.更新详细信息.Close();
            }
            if (窗口.新建部门 != null)
            {
                窗口.新建部门.Close();
            }
            if (窗口.收到错误反馈 != null)
            {
                窗口.收到错误反馈.Close();
            }
            if (窗口.录入 != null)
            {
                窗口.录入.Close();
            }
            if (窗口.删除 != null)
            {
                窗口.删除.Close();
            }
            if (窗口.查询员工个人信息 != null)
            {
                窗口.查询员工个人信息.Close();
            }
            窗口.人事选择进入界面 = null;
            窗口.员工自助查询.Show();
            if (窗口.GetValue("默认大小") == "1")
            {
                窗口.员工自助查询.WindowState = FormWindowState.Normal;
            }
            else if (窗口.GetValue("默认大小") == "2")
            {
                窗口.员工自助查询.WindowState = FormWindowState.Maximized;
            }
            else
            {
                窗口.员工自助查询.WindowState = this.WindowState;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (窗口.查询员工个人信息 == null)
            {
                查询员工个人信息 查询员工个人信息 = new 查询员工个人信息();
                查询员工个人信息.Show();
                if (窗口.GetValue("默认大小") == "1")
                {
                    查询员工个人信息.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    查询员工个人信息.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    查询员工个人信息.WindowState = this.WindowState;
                }
            }
            else
            {
                if (窗口.GetValue("默认大小") == "1")
                {
                    窗口.查询员工个人信息.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    窗口.查询员工个人信息.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    窗口.查询员工个人信息.WindowState = this.WindowState;
                }
                窗口.查询员工个人信息.Focus();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (窗口.更新详细信息 == null)
            {
                更新.更新详细信息 更新详细信息 = new 更新.更新详细信息();
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
            else
            {
                if (窗口.GetValue("默认大小") == "1")
                {
                    窗口.更新详细信息.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    窗口.更新详细信息.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    窗口.更新详细信息.WindowState = this.WindowState;
                }
                窗口.更新详细信息.Focus();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (窗口.收到错误反馈 == null)
            {
                人事专用.收到错误反馈 收到错误反馈 = new 收到错误反馈();
                收到错误反馈.Show();
                if (窗口.GetValue("默认大小") == "1")
                {
                    收到错误反馈.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    收到错误反馈.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    收到错误反馈.WindowState = this.WindowState;
                }
            }
            else
            {
                if (窗口.GetValue("默认大小") == "1")
                {
                    窗口.收到错误反馈.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    窗口.收到错误反馈.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    窗口.收到错误反馈.WindowState = this.WindowState;
                }
                窗口.收到错误反馈.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (窗口.录入 == null)
            {
                录入 录入 = new 录入();
                录入.Show();
                if (窗口.GetValue("默认大小") == "1")
                {
                    录入.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    录入.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    录入.WindowState = this.WindowState;
                }
            }
            else
            {
                if (窗口.GetValue("默认大小") == "1")
                {
                    窗口.录入.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    窗口.录入.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    窗口.录入.WindowState = this.WindowState;
                }
                窗口.录入.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (窗口.删除 == null)
            {
                删除 删除 = new 删除();
                删除.Show();
                if (窗口.GetValue("默认大小") == "1")
                {
                    删除.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    删除.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    删除.WindowState = this.WindowState;
                }
            }
            else
            {
                if (窗口.GetValue("默认大小") == "1")
                {
                    窗口.删除.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    窗口.删除.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    窗口.删除.WindowState = this.WindowState;
                }
                窗口.删除.Focus();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (窗口.新建部门 == null)
            {
                新建部门 新建部门 = new 新建部门();
                新建部门.Show();
                if (窗口.GetValue("默认大小") == "1")
                {
                    新建部门.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    新建部门.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    新建部门.WindowState = this.WindowState;
                }
            }
            else
            {
                if (窗口.GetValue("默认大小") == "1")
                {
                    窗口.新建部门.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    窗口.新建部门.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    窗口.新建部门.WindowState = this.WindowState;
                }
                窗口.新建部门.Focus();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (窗口.编辑公告 == null)
            {
                等待 等待 = new 等待();
                等待.Show();
                编辑公告 编辑公告 = new 编辑公告();
                编辑公告.Show();
                if (窗口.GetValue("默认大小") == "1")
                {
                    编辑公告.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    编辑公告.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    编辑公告.WindowState = this.WindowState;
                }
            }
            else
            {
                if (窗口.GetValue("默认大小") == "1")
                {
                    窗口.编辑公告.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    窗口.编辑公告.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    窗口.编辑公告.WindowState = this.WindowState;
                }
                窗口.编辑公告.Focus();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (窗口.请假信息 == null)
            {
                请假信息 请假信息 = new 请假信息();
                请假信息.Show();
                if (窗口.GetValue("默认大小") == "1")
                {
                    请假信息.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    请假信息.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    请假信息.WindowState = this.WindowState;
                }
            }
            else
            {
                if (窗口.GetValue("默认大小") == "1")
                {
                    窗口.请假信息.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    窗口.请假信息.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    窗口.请假信息.WindowState = this.WindowState;
                }
                窗口.请假信息.Focus();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "delete from 请假 where 是否通知=true";
                string sql1 = "delete from 错误报告 where 是否通知=true";
                OleDbCommand cmd = new OleDbCommand(sql, 窗口.conn);
                OleDbCommand sm = new OleDbCommand(sql1, 窗口.conn);
                int re = sm.ExecuteNonQuery();
                int res = cmd.ExecuteNonQuery();
                if (res >= 0 && re >= 0)
                {
                    MessageBox.Show("删除成功");
                }
                else
                {
                    MessageBox.Show("删除失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
                this.Close();
            }
        }

        private void 人事选择进入界面_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
            {
                button7_Click(null, null);
            }
        }

        private void 人事选择进入界面_Enter(object sender, EventArgs e)
        {
        }
    }
}
