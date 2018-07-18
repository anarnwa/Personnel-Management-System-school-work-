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
    public partial class 设置 : Form
    {
        public 设置()
        {
            InitializeComponent();
            窗口.设置 = this;
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

        private void 设置_VisibleChanged(object sender, EventArgs e)
        {
            this.Left = z;
            this.Top = s;
        }

        private void 设置_FormClosed(object sender, FormClosedEventArgs e)
        {
            窗口.设置 = null;
        }

        private void 设置_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape||e.KeyCode==Keys.Enter)
            {
                this.Close();
            }
        }
        private bool x = false;
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox3.Checked==true)
            {
                if (x)
                {
                    if (MessageBox.Show("允许多开可能会出现一系列问题\n别说我没有警告你哦", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                    {
                        窗口.SetValue("允许多开", "1");
                    }
                    else
                    {
                        checkBox3.Checked = false;
                    }
                }
                else
                {
                    窗口.SetValue("允许多开", "1");
                }
            }
            else
            {
                窗口.SetValue("允许多开", "0");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text=="默认")
            {
                窗口.SetValue("默认大小", "0");
            }
            if (comboBox1.Text == "正常窗口")
            {
                窗口.SetValue("默认大小", "1");
            }
            if (comboBox1.Text == "最大化窗口")
            {
                窗口.SetValue("默认大小", "2");
            }
        }

        private void 设置_Shown(object sender, EventArgs e)
        {
            if(窗口.GetValue("默认大小")=="1")
            {
                this.WindowState = FormWindowState.Normal;
            }
            else if (窗口.GetValue("默认大小") == "2")
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.Text=="退出程序")
            {
                窗口.SetValue("默认不关闭", "0");
            }
            else
            {
                窗口.SetValue("默认不关闭", "1");
            }
        }

        private void 设置_Load(object sender, EventArgs e)
        {
            z = this.Left;
            s = this.Top;
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
            this.Resize += new EventHandler(Form1_Resize);
            if (窗口.GetValue("允许多开") == "1")
            {
                checkBox3.Checked = true;
            }
            x = true;
            if (窗口.GetValue("默认大小") == "1")
            {
                comboBox1.Text = "正常窗口";
            }
            else if (窗口.GetValue("默认大小") == "2")
            {
                comboBox1.Text = "最大化窗口";
            }
            else
            {
                comboBox1.Text = "默认";
            }
            if(窗口.GetValue("默认不关闭")=="0")
            {
                comboBox2.Text = "退出程序";
            }
            else
            {
                comboBox2.Text = "最小化到系统栏";
            }
        }
    }
}
