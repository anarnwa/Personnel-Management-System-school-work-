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
    public partial class 新建部门 : Form
    {
        public 新建部门()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label9.Text != "职位已存在")
            {
                //职位不存在   则插入
                if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
                {
                    //都不为空
                    string sql = "insert into 职位信息 (职位编号,部门编号,部门名称,职位,基本工资,可休假期) values ('" + textBox2.Text + textBox1.Text + "','" + textBox2.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox3.Text + "','" + textBox6.Text + "');";
                    try
                    {
                        //插入
                        OleDbCommand cmd = new OleDbCommand(sql, 窗口.conn);
                        int res = cmd.ExecuteNonQuery();
                        if (res > 0)
                        {
                            MessageBox.Show("更新成功");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("请填写空白处");
                }
            }
            else
            {
                //职位已经存在  则更新
                if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
                {
                    string sql = "update 职位信息 set 部门名称='"+textBox4.Text+"',职位='"+textBox5.Text+"',基本工资='"+textBox3.Text+"',可休假期='"+textBox6.Text+"'where 职位编号='"+textBox2.Text+textBox1.Text+"';";
                    try
                    {
                        OleDbCommand cmd = new OleDbCommand(sql, 窗口.conn);
                        int res = cmd.ExecuteNonQuery();
                        if (res > 0)
                        {
                            MessageBox.Show("更新成功");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("请填写空白处");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "000")
            {
                try
                {
                    //查询部门信息 并显示
                    string sql1 = "select 部门名称 from 职位信息 where 部门编号='" + textBox2.Text + "';";
                    OleDbCommand cmd = new OleDbCommand(sql1, 窗口.conn);
                    OleDbDataReader rea1 = cmd.ExecuteReader();
                    if (rea1.Read())
                    {
                        label8.Text = "部门已存在";
                        textBox4.Text = rea1.GetString(0);
                    }
                    else
                    {
                        label8.Text = "";
                        textBox4.Text = "";
                    }
                    button1.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Close();
                }
            }
            else
            {
                textBox4.Text = "此部门不可变动";
                button1.Enabled = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "000")
            {
                //try
                {
                    //查询职位信息并显示
                    string sql1 = "select 职位,基本工资,可休假期 from 职位信息 where 职位编号='" + textBox2.Text + textBox1.Text + "';";
                    OleDbCommand cmd = new OleDbCommand(sql1, 窗口.conn);
                    OleDbDataReader rea1 = cmd.ExecuteReader();
                    if (rea1.Read())
                    {
                        label9.Text = "职位已存在";
                        textBox5.Text = rea1.GetString(0);
                        textBox3.Text = rea1.GetInt32(1).ToString();
                        textBox6.Text = rea1.GetInt32(2).ToString();
                    }
                    else
                    {
                        label9.Text = "";
                        textBox5.Text = "";
                        textBox6.Text = "";
                        textBox3.Text = "";
                        button1.Enabled = true;
                    }
                }
             /*   catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Close();
                }  */
            }
            else
            {
                button1.Enabled = false;
            }
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
        private void 新建部门_Load(object sender, EventArgs e)
        {
            z = Left;
            s = Top;
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
            this.Resize += new EventHandler(Form1_Resize);
            窗口.新建部门 = this;
        }

        private void 新建部门_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Escape: button2_Click(null, null);break;
                case Keys.Enter: button1_Click(null, null);break;
            }
        }

        private void 新建部门_VisibleChanged(object sender, EventArgs e)
        {
            Top = s;
            Left = z;
        }

        private void 新建部门_FormClosed(object sender, FormClosedEventArgs e)
        {
            窗口.新建部门 = null;
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
