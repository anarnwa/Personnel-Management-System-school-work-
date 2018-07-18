using System;
using System.Data.OleDb;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace 人事管理系统.人事专用.更新
{
    public partial class 更新详细信息 : Form
    {
        string A = "";
        object B = "";
        private string pathname = "";
        private bool x = false;
        public 更新详细信息()
        {
            InitializeComponent();
        }
        public 更新详细信息(string a, object b)
        {
            A = a;
            B = b;
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
        private void 更新详细信息_Load(object sender, EventArgs e)
        {
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
            this.Resize += new EventHandler(Form1_Resize);
            窗口.更新详细信息 = this;
            comboBox1.Text = "男";
            comboBox2.Text = "初中";
            textBox1.Text = A;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (x)
            {
                try
                {
                    //图片不为空
                    string sql = "";
                    OleDbCommand sql2 = 窗口.conn.CreateCommand();
                    sql2.CommandText = "SELECT 照片位置 FROM 人员信息 WHERE 工号='" + textBox1.Text + "';";
                    OleDbDataReader rea = sql2.ExecuteReader();
                    if (rea.Read())
                    {
                        if (rea.GetValue(0).ToString()!="")
                        {
                            //删除服务器端图片
                            if (File.Exists(窗口.ip + 窗口.add + rea.GetValue(0).ToString()))
                            {
                                File.Delete(窗口.ip + 窗口.add + rea.GetValue(0).ToString());
                            }
                            if (pictureBox1.Image != null)
                            {
                                //存放图片到服务器
                                pictureBox1.Image.Save(窗口.ip + 窗口.add + "\\照片\\" + textBox1.Text + Path.GetExtension(pathname));
                                sql = "update 人员信息 set 照片位置='" + "\\照片\\" + textBox1.Text + Path.GetExtension(pathname) + "',姓名='" + textBox2.Text + "', 性别='" + comboBox1.Text + "' , 出生年月= '" + dateTimePicker1.Value + "' , 学历='" + comboBox2.Text + "' , 职位编号='" + textBox4.Text + "' ," +
                                " 联系电话='" + textBox6.Text + "', 工资卡号='" + textBox5.Text + "' where  工号='" + textBox1.Text + "';";
                            }
                            else
                            {
                                sql = "update 人员信息 set 照片位置='',姓名='" + textBox2.Text + "', 性别='" + comboBox1.Text + "' , 出生年月= '" + dateTimePicker1.Value + "' , 学历='" + comboBox2.Text + "' , 职位编号='" + textBox4.Text + "' ," +
                          " 联系电话='" + textBox6.Text + "', 工资卡号='" + textBox5.Text + "' where  工号='" + textBox1.Text + "';";
                            }
                        }
                        else
                        {
                            if (pictureBox1.Image != null)
                            {
                                //存放图片到服务器
                                pictureBox1.Image.Save(窗口.ip + 窗口.add + "\\照片\\" + textBox1.Text + Path.GetExtension(pathname));
                                sql = "update 人员信息 set 照片位置='" + "\\照片\\" + textBox1.Text + Path.GetExtension(pathname) + "',姓名='" + textBox2.Text + "', 性别='" + comboBox1.Text + "' , 出生年月= '" + dateTimePicker1.Value + "' , 学历='" + comboBox2.Text + "' , 职位编号='" + textBox4.Text + "' ," +
                                " 联系电话='" + textBox6.Text + "', 工资卡号='" + textBox5.Text + "' where  工号='" + textBox1.Text + "';";
                            }
                            else
                            {
                                sql = "update 人员信息 set 照片位置='',姓名='" + textBox2.Text + "', 性别='" + comboBox1.Text + "' , 出生年月= '" + dateTimePicker1.Value + "' , 学历='" + comboBox2.Text + "' , 职位编号='" + textBox4.Text + "' ," +
                          " 联系电话='" + textBox6.Text + "', 工资卡号='" + textBox5.Text + "' where  工号='" + textBox1.Text + "';";
                            }
                        }
                    }
                    //更新
                    else
                    {
                        sql = "update 人员信息 set 照片位置='',姓名='" + textBox2.Text + "', 性别='" + comboBox1.Text + "' , 出生年月= '" + dateTimePicker1.Value + "' , 学历='" + comboBox2.Text + "' , 职位编号='" + textBox4.Text + "' ," +
                            " 联系电话='" + textBox6.Text + "', 工资卡号='" + textBox5.Text + "' where  工号='" + textBox1.Text + "';";
                    }
                    OleDbCommand cmd = new OleDbCommand(sql, 窗口.conn);
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        if (A != "")
                        {
                            //如果是从查看错误信息打开的  就提交更新
                            string sql1 = "update 错误报告 set 是否处理= true where 编号 =" + B;
                            OleDbCommand cmd1 = new OleDbCommand(sql1, 窗口.conn);
                            int res1 = cmd1.ExecuteNonQuery();
                            if (res1 > 0)
                            {
                                MessageBox.Show("更新成功");
                            }
                            else
                            {
                                MessageBox.Show("失败", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("更新成功");
                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("更新失败", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }  
            }
            else
            {
                MessageBox.Show("此职位编号错误", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //获取当前文本框的内容的信息并显示
                OleDbCommand sql1 = 窗口.conn.CreateCommand();
                sql1.CommandText = "SELECT 姓名, 性别, 出生年月, 学历,人员信息.职位编号, 联系电话,工资卡号,照片位置 FROM 人员信息, 职位信息 WHERE 人员信息.职位编号=职位信息.职位编号 and 工号='" + textBox1.Text + "';";
                OleDbDataReader rea = sql1.ExecuteReader();
                if (rea.Read())
                {
                    textBox2.Text = rea.GetString(0);
                    comboBox1.Text = rea.GetString(1);
                    if (rea.GetDateTime(2).ToString("yyyy/MM/dd") == "1775/01/01")
                        label10.Text = "(无信息)";
                    dateTimePicker1.Value = rea.GetDateTime(2);
                    if (rea.GetString(3) == "0")
                        comboBox2.Text = "无信息";
                    else
                        comboBox2.Text = rea.GetString(3);
                    textBox4.Text = rea.GetString(4);
                    if (rea.GetString(5) == "0")
                        textBox6.Text = "无信息";
                    else
                        textBox6.Text = rea.GetString(5);
                    textBox5.Text = rea.GetString(6);
                    if (rea.GetValue(7).ToString() != "")
                    {
                        try
                        {
                            string destPath = Path.Combine(@"", Path.GetFileName(窗口.ip + 窗口.add + rea.GetString(7)));
                            string extension = Path.GetExtension(destPath);
                            if (File.Exists(textBox1.Text + extension))
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
                            MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                }
                else
                {
                    //如果没有读取到 就置空
                    label10.Text = "";
                    textBox2.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    comboBox1.Text = "";
                    comboBox2.Text = "";
                    pathname = "";
                    pictureBox1.Image = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //读取职位名
                OleDbCommand sql1 = 窗口.conn.CreateCommand();
                sql1.CommandText = "SELECT 职位,部门名称 from 职位信息 WHERE 职位编号='" + textBox4.Text + "';";
                OleDbDataReader rea = sql1.ExecuteReader();
                if (rea.Read())
                {
                    label11.Text = rea.GetString(1)+"    "+rea.GetString(0);
                    x = true;
                }
                else
                {
                    label11.Text = "无职位";
                    x = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void 更新详细信息_KeyDown(object sender, KeyEventArgs e)
        {
            //按键动作
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void comboBox2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //按键动作
            if (e.KeyCode == Keys.Space)
            {
                if (comboBox2.Text == "初中")
                {
                    comboBox2.Text = "高中";
                }
                else if (comboBox2.Text == "高中")
                {
                    comboBox2.Text = "本科";
                }
                else if (comboBox2.Text == "本科")
                {
                    comboBox2.Text = "大专";
                }
                else if (comboBox2.Text == "大专")
                {
                    comboBox2.Text = "硕士";
                }
                else if (comboBox2.Text == "硕士")
                {
                    comboBox2.Text = "博士";
                }
                else
                {
                    comboBox2.Text = "初中";
                }
            }
        }

        private void comboBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //按键动作
            if (e.KeyCode == Keys.Space)
            {
                if (comboBox1.Text == "男")
                {
                    comboBox1.Text = "女";
                }
                else
                    comboBox1.Text = "男";
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void 更新详细信息_FormClosed(object sender, FormClosedEventArgs e)
        {
            窗口.更新详细信息 = null;
            if (A != "")
            {
                //如果处理完了  就刷新错误反馈页面
                if (窗口.收到错误反馈 != null)
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
                    窗口.收到错误反馈.load();
                }
            }
            else
            {
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

        private void button2_Click(object sender, EventArgs e)
        {
            //上传图片
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = ".";
            file.Filter = "图片文件|*.jpg;*.png";
            file.ShowDialog();
            if (file.FileName != string.Empty)
            {
                try
                {
                    pathname = file.FileName;   //获得文件的绝对路径
                    this.pictureBox1.Load(pathname);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void 更新详细信息_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void 更新详细信息_DragEnter(object sender, DragEventArgs e)
        {
            string files = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (Path.GetExtension(files) == ".jpg" || Path.GetExtension(files) == ".png" || Path.GetExtension(files) == ".JPG" || Path.GetExtension(files) == ".PNG")  //判断文件类型，只接受jpg或png文件
            {
                this.pictureBox1.Load(((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString());
                pathname = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsPunctuation(e.KeyChar)||char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            } 
        }
    }
}
