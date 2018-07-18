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

namespace 人事管理系统
{
    public partial class 修改密码 : Form
    {
        string a;
        public 修改密码()
        {
            a = 窗口.ID;
            InitializeComponent();
            窗口.修改密码 = this;
        }

        private void button3_Click(object sender, EventArgs e)
        {
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
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {//都不为空才处理
                if (textBox2.Text == textBox3.Text)
                {
                    //新密码和确认密码都相同
                    try
                    {
                        //尝试读取正确的密码并比较
                        OleDbCommand cmd = 窗口.conn.CreateCommand();
                        cmd.CommandText = " select * from 人员信息 where 工号='" + a + "'and 密码='" + textBox1.Text + "'";
                        OleDbDataReader reader = cmd.ExecuteReader();
                        if (!reader.Read())
                        {
                            MessageBox.Show("密码错误");
                        }
                        else
                        {
                            //更新
                            string sql = "update 人员信息 set 密码='" + textBox2.Text + "' where 工号='" + a + "';";
                            OleDbCommand cm = new OleDbCommand(sql, 窗口.conn);
                            int res = cm.ExecuteNonQuery();
                            if (res > 0)
                            {
                                MessageBox.Show("修改成功");
                                窗口.SetValue("密码", 窗口.EncryptDES(textBox2.Text,"74110968"));
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
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("修改失败");
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message );
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("新密码与确认密码不匹配");
                }

            }
            else
            {
                MessageBox.Show("请填写空白处");
            }
        }

        private void 修改密码_FormClosed(object sender, FormClosedEventArgs e)
        {
            窗口.修改密码 = null;
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
        private void 修改密码_Load(object sender, EventArgs e)
        {
            z = this.Left;
            s = this.Top;
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
            this.Resize += new EventHandler(Form1_Resize);
            try
            {
                //读取人员信息
                string sqlstr = "SELECT 姓名 FROM 人员信息 WHERE 工号 = '" + a + "'";
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlstr, 窗口.conn);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                label5.Text = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
                this.Close();
            }
        }

        private void 修改密码_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
            if (e.KeyCode == Keys.Escape)
            {
                button3_Click(null, null);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsNumber(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsNumber(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void 修改密码_VisibleChanged(object sender, EventArgs e)
        {
            this.Left = z;
            this.Top = s;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsNumber(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }
    }
}
