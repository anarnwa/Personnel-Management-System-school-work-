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
    public partial class 删除 : Form
    {
        public 删除()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2_TextChanged(null, null);
            if(comboBox1.Text=="工号")
            {
                
            }
            else if(comboBox1.Text=="职位")
            {
                
            }
            else
            {
              
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != string.Empty)
            {
                try
                {
                    if (comboBox1.Text == "工号")
                    {
                        //判断删除的是什么
                        if (textBox2.Text != 窗口.ID)
                        {
                            //删除服务器端的照片
                            OleDbCommand sql2 = 窗口.conn.CreateCommand();
                            sql2.CommandText = "SELECT 照片位置 FROM 人员信息 WHERE 工号='" + textBox2.Text + "';";
                            OleDbDataReader rea = sql2.ExecuteReader();
                            if (rea.Read())
                            {
                                if (rea.GetValue(0).ToString() != "")
                                {
                                    //删除服务器端图片
                                    if (File.Exists(窗口.ip + 窗口.add + rea.GetValue(0).ToString()))
                                    {
                                        File.Delete(窗口.ip + 窗口.add + rea.GetValue(0).ToString());
                                    }
                                }
                            }
                            //删除人员信息表的信息
                            string sql1 = "delete from 人员信息 where 工号='" + textBox2.Text + "';";
                            OleDbCommand cmd1 = new OleDbCommand(sql1, 窗口.conn);
                            int res1 = cmd1.ExecuteNonQuery();
                            string sql3 = "delete from 出勤记录 where 工号='" + textBox2.Text + "';";
                            OleDbCommand cmd3 = new OleDbCommand(sql3, 窗口.conn);
                            cmd3.ExecuteNonQuery();
                            if (res1 > 0)
                            {
                                MessageBox.Show("删除成功");
                            }
                            else
                            {
                                MessageBox.Show("工号不存在");
                            }
                            comboBox1_SelectedIndexChanged(null,null);
                        }
                        else
                        {
                            MessageBox.Show("你不能删除自己");
                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "职位")
                        {
                            //职位
                            if (textBox2.Text != "000000"&&textBox2.Text!="110001")
                            {
                                //000000 默认不可删除  将被删除的职位的人员的职位全部设置为000000  待安排
                                string sql2 = "update 人员信息 set 职位编号='000000' where 职位编号='" + textBox2.Text + "';";
                                OleDbCommand cmd2 = new OleDbCommand(sql2, 窗口.conn);
                                int res2 = cmd2.ExecuteNonQuery();
                                //删除此职位
                                string sql1 = "delete from 职位信息 where 职位编号='" + textBox2.Text + "';";
                                OleDbCommand cmd1 = new OleDbCommand(sql1, 窗口.conn);
                                int res1 = cmd1.ExecuteNonQuery();
                                if (res1 > 0)
                                {
                                    MessageBox.Show("删除成功");
                                }
                                else
                                {
                                    MessageBox.Show("职位不存在");
                                }
                            }
                            else
                            {
                                MessageBox.Show("此职位不可移除");
                            }
                        }
                        else if (comboBox1.Text == "部门")
                        {
                            if (textBox2.Text != "000"&&textBox2.Text!="110")
                            {
                                //000 部门不可删除  将此外的此部门的人员职位改为000000    
                                string sql2 = "update 人员信息,职位信息 set 人员信息.职位编号='000000' where 部门编号='" + textBox2.Text + "' and 人员信息.职位编号=职位信息.职位编号";
                                OleDbCommand cmd2 = new OleDbCommand(sql2, 窗口.conn);
                                int res2 = cmd2.ExecuteNonQuery();
                                //删除部门及部门里的职位
                                string sql1 = "delete from 职位信息 where 部门编号='" + textBox2.Text + "';";
                                OleDbCommand cmd1 = new OleDbCommand(sql1, 窗口.conn);
                                int res1 = cmd1.ExecuteNonQuery();
                                if (res1 > 0)
                                {
                                    MessageBox.Show("删除成功");
                                }
                                else
                                {
                                    MessageBox.Show("部门不存在");
                                }
                            }
                            else
                            {
                                MessageBox.Show("此部门不可移除");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    textBox2.Text = "";
                    textBox2.Focus();
                }
            }
            else
            {
                MessageBox.Show("请输入数据");
                textBox2.Text = "";
                textBox2.Focus();
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

        private void 删除_Load(object sender, EventArgs e)
        {
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
            this.Resize += new EventHandler(Form1_Resize);
            窗口.删除 = this;
            comboBox1.Text = "工号";
        }

        private void comboBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (comboBox1.Text == "工号")
                    comboBox1.Text = "职位";
                else if(comboBox1.Text == "职位")
                    comboBox1.Text = "部门";
                else 
                    comboBox1.Text = "工号";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "工号")
            {
                string sql = "";
                OleDbDataAdapter myCommand = null;
                DataSet ds = null;
                sql = "select 工号,姓名 from 人员信息 where 工号 like '" + textBox2.Text + "%' ORDER BY 工号";
                myCommand = new OleDbDataAdapter(sql, 窗口.conn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                dataGridView1.DataSource = ds.Tables[0];
                if (窗口.查询员工个人信息!=null)
                {
                    窗口.查询员工个人信息.change(textBox2.Text);
                    窗口.查询员工个人信息.changetextbox(false);
                    窗口.查询员工个人信息.WindowState = this.WindowState;
                }
                OleDbCommand sql1 = 窗口.conn.CreateCommand();
                sql1.CommandText = "SELECT 姓名,职位,部门名称 FROM 人员信息,职位信息 WHERE 工号='" + textBox2.Text + "' and 人员信息.职位编号=职位信息.职位编号;";
                OleDbDataReader rea = sql1.ExecuteReader();
                if (rea.Read())
                {
                    label5.Text = rea.GetString(2)+"  "+rea.GetString(1)+"  "+rea.GetString(0);
                }
                else
                {
                    label5.Text = "";
                }
            }
            else if (comboBox1.Text == "职位")
            {
                string sql = "";
                OleDbDataAdapter myCommand = null;
                DataSet ds = null;
                sql = "select 职位编号,部门名称,职位 from 职位信息 where 职位编号 like '" + textBox2.Text + "%' ORDER BY 职位编号";
                myCommand = new OleDbDataAdapter(sql, 窗口.conn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                dataGridView1.DataSource = ds.Tables[0];
                label5.Text = "";
            }
            else
            {
                string sql = "";
                OleDbDataAdapter myCommand = null;
                DataSet ds = null;
                sql = "select DISTINCT 部门编号,部门名称 from 职位信息 where 部门编号 like '" + textBox2.Text + "%' ORDER BY 部门编号";
                myCommand = new OleDbDataAdapter(sql, 窗口.conn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                dataGridView1.DataSource = ds.Tables[0];
                label5.Text = "";
            }
        }

        private void 删除_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            if (e.KeyCode == Keys.Enter)
                button1_Click(null, null);
        }

        private void 删除_FormClosed(object sender, FormClosedEventArgs e)
        {
            窗口.删除 = null;
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
            if (窗口.查询员工个人信息 != null)
            {
                窗口.查询员工个人信息.Close();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsNumber(e.KeyChar)&&!char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (窗口.查询员工个人信息 == null)
            {
                if (comboBox1.Text == "工号")
                {
                    查询员工个人信息 查询员工个人信息 = new 查询员工个人信息();
                    查询员工个人信息.Show();
                    查询员工个人信息.WindowState = this.WindowState;
                    查询员工个人信息.change(textBox2.Text);
                    查询员工个人信息.changetextbox(false);
                }
            }
            else
            {
                窗口.查询员工个人信息.WindowState = this.WindowState;
                窗口.查询员工个人信息.Focus();
            }
        }
    }
}
