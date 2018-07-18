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
    public partial class 查询员工个人信息 : Form
    {
        public 查询员工个人信息()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void 查询员工个人信息_Load(object sender, EventArgs e)
        {
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
            this.Resize += new EventHandler(Form1_Resize);
            窗口.查询员工个人信息 = this;
        }
        public void change(string no)
        {
            textBox3.Text = no;
        }
        public void changetextbox(bool x)
        {
            textBox3.Enabled = x;
        }
        private void 查询员工个人信息_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                button1_Click(null, null);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                OleDbCommand sql1 = 窗口.conn.CreateCommand();
                sql1.CommandText = "SELECT 姓名, 性别, 出生年月, 学历, 部门名称, 职位, 联系电话,照片位置 FROM 人员信息, 职位信息 WHERE 人员信息.职位编号=职位信息.职位编号 and 工号='" + textBox3.Text + "';";
                OleDbDataReader rea = sql1.ExecuteReader();
                if (rea.Read())
                {
                    if (rea.GetValue(7).ToString() != "")
                    {
                        try
                        {
                            string destPath = Path.Combine(@"", Path.GetFileName(窗口.ip + 窗口.add + rea.GetString(7)));
                            if (File.Exists(destPath))
                            {
                                //如果文件存在 则读取
                                pictureBox1.Load(destPath);
                            }
                            else
                            {
                                //否则复制到本地
                                if (File.Exists(窗口.ip + 窗口.add + rea.GetString(7)))
                                {
                                    System.IO.File.Copy(窗口.ip + 窗口.add + rea.GetString(7), destPath);
                                    pictureBox1.Load(destPath);
                                }                               
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    if (rea.GetString(0) == "")
                    {
                        label3.Text = "无信息";
                    }
                    else
                    {
                        label3.Text = rea.GetString(0);
                    }
                    if (rea.GetString(4) == "")
                    {
                        label6.Text = "无信息";
                    }
                    else
                    {
                        label6.Text = rea.GetString(4);
                    }
                    if (rea.GetString(5) == "")
                    {
                        label8.Text = "无信息";
                    }
                    else
                    {
                        label8.Text = rea.GetString(5);
                    }
                    if (rea.GetString(6) == "0")
                    {
                        label9.Text = "无信息";
                    }
                    else
                    {
                        label9.Text = rea.GetString(6);
                    }
                    if (rea.GetString(1) == "")
                    {
                        label11.Text = "无信息";
                    }
                    else
                    {
                        label11.Text = rea.GetString(1);
                    }
                    if (rea.GetDateTime(2).ToString("yyyy/MM/dd") == "1775/01/01")
                    {
                        label13.Text = "无信息";
                    }
                    else
                    {
                        label13.Text = rea.GetDateTime(2).ToString("yyyy/MM/dd");
                    }
                    if (rea.GetString(3) == "0")
                    {
                        label15.Text = "无信息";
                    }
                    else
                    {
                        label15.Text = rea.GetString(3);
                    }
                }
                else
                {
                    label3.Text = "无信息";
                    label6.Text = "无信息";
                    label8.Text = "无信息";
                    label9.Text = "无信息";
                    label11.Text = "无信息";
                    label13.Text = "无信息";
                    label15.Text = "无信息";
                    pictureBox1.Image = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void 查询员工个人信息_FormClosed(object sender, FormClosedEventArgs e)
        {
            窗口.查询员工个人信息 = null;
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
            if(窗口.删除!=null)
            {
                窗口.删除.WindowState = this.WindowState;
                窗口.删除.Focus();
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsNumber(e.KeyChar)&&!char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
