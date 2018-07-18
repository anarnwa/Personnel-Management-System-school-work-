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
using System.Runtime.InteropServices;

namespace 人事管理系统
{
    public partial class 登录 : Form
    {
        public 登录()
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
        private int z, s;
        private void 登录_Load(object sender, EventArgs e)
        {
            z = this.Left;
            s = this.Top;
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
            this.Resize += new EventHandler(Form1_Resize);
            textBox1.Text = 窗口.GetValue("用户名");
            textBox2.Text = 窗口.DecryptDES(窗口.GetValue("密码"),"74110968");
            if(窗口.GetValue("记住密码")=="1")
            {
                checkBox1.Checked = true;
            }
            //连接字符串
            if(窗口.GetValue("add")!="")
            {
                窗口.add = 窗口.GetValue("add");
            }
            else
            {
                窗口.add = "\\共享文件夹";
                窗口.SetValue("add", "\\共享文件夹");
            }
            try
            {
                if (!Directory.Exists(窗口.ip + 窗口.add +"\\照片"))
                {
                    Directory.CreateDirectory(窗口.ip + 窗口.add + "\\照片");
                }
            }
            catch
            {

            }
            try
            {
                if (!Directory.Exists(窗口.ip + 窗口.add + "\\已登录"))
                {
                    Directory.CreateDirectory(窗口.ip + 窗口.add + "\\已登录");
                }
            }
            catch
            {

            }
            窗口.conn.ConnectionString = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + 窗口.ip + 窗口.add + "\\人事管理系统.mdb;Persist Security Info=False;Jet OLEDB:Database Password=123456";
            窗口.登录 = this;
            try
            {
                //尝试打开连接
                窗口.conn.Open();
            }
            catch (Exception)
            {
                button2_Click(null, null);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //焦点复位
            textBox1.Focus();
            try
            {
                //删除所有 .jpg和.png格式的文件
                DirectoryInfo di = new DirectoryInfo(Application.StartupPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    string exname = fi.Name.Substring(fi.Name.LastIndexOf(".") + 1);//得到后缀名   
                    //判断当前文件后缀名是否与给定后缀名一样
                    if (exname == "jpg"||exname=="png" || exname == "JPG" || exname == "PNG")
                    {
                        File.Delete(Application.StartupPath + "\\" + fi.Name);//删除当前文件
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,null,MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            try
            {
                //查询密码 工号是否正确
                OleDbCommand cmd = 窗口.conn.CreateCommand();
                cmd.CommandText = " select * from 人员信息 where 工号='" + textBox1.Text.Trim() + "'and 密码='" + textBox2.Text.Trim() + "'";
                OleDbDataReader reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    MessageBox.Show("用户名或密码错误", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox2.Text = "";
                    textBox2.Focus();
                }
                else
                {
                    窗口.ID = textBox1.Text.Trim();  //正确则将工号写入全局变量备用
                    if(!File.Exists(窗口.ip+窗口.add+"\\已登录\\"+窗口.ID))
                    {
                        FileStream fs = new FileStream(窗口.ip + 窗口.add + "\\已登录\\" + 窗口.ID, FileMode.OpenOrCreate);
                        fs.Close();
                    }
                    else
                    {
                        File.Delete(窗口.ip + 窗口.add + "\\已登录\\" + 窗口.ID);
                        System.Threading.Thread.Sleep(1000);
                        FileStream fs = new FileStream(窗口.ip + 窗口.add + "\\已登录\\" + 窗口.ID, FileMode.OpenOrCreate);
                        fs.Close();
                    }
                    if (checkBox1.Checked == true)
                    {
                        窗口.SetValue("用户名", textBox1.Text);
                        窗口.SetValue("密码", 窗口.EncryptDES((textBox2.Text),"74110968"));
                        窗口.SetValue("记住密码", "1");
                        
                    }
                    else
                    {
                        窗口.SetValue("用户名", "");
                        窗口.SetValue("密码", "");
                        窗口.SetValue("记住密码", "0");
                        
                    }
                    if(checkBox2.Checked==true)
                    {
                        窗口.SetValue("自动登录", "1");
                    }
                    else
                    {
                        窗口.SetValue("自动登录", "0");
                    }
                    if (textBox2.Text.Trim() == "123456")
                    { 
                        //是默认密码则要求修改密码
                        MessageBox.Show("密码为默认密码，请修改！", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        员工自助查询 A = new 员工自助查询();
                        修改密码 修改密码 = new 修改密码();
                        this.Hide();
                        修改密码.Show();
                        if (窗口.GetValue("默认大小") == "1")
                        {
                            修改密码.WindowState = FormWindowState.Normal;
                        }
                        else if (窗口.GetValue("默认大小") == "2")
                        {
                            修改密码.WindowState = FormWindowState.Maximized;
                        }
                        else
                        {
                            修改密码.WindowState = this.WindowState;
                        }
                    }
                    else
                    {
                        员工自助查询 A = new 员工自助查询();
                        this.Hide();
                        等待 等待 = new 等待();
                        等待.Show();
                        A.Show();
                        if (窗口.GetValue("默认大小") == "1")
                        {
                            A.WindowState = FormWindowState.Normal;
                        }
                        else if (窗口.GetValue("默认大小") == "2")
                        {
                            A.WindowState = FormWindowState.Maximized;
                        }
                        else
                        {
                            A.WindowState = this.WindowState;
                        }

                    }                   
                }
            }
            catch
            {
            }

        }

        private void 登录_KeyDown(object sender, KeyEventArgs e)
        {
            //按键动作
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }
        private void 登录_VisibleChanged(object sender, EventArgs e)
        {
            this.Left = z;
            this.Top = s;
            //尝试读取公告文件
            if (this.Visible == true)
            {
                this.Focus();
                try
                {
                    byte[] byData = new byte[16000];
                    char[] charData = new char[16000];
                    FileStream fs = new FileStream(窗口.ip + 窗口.add + "\\公告.txt", FileMode.OpenOrCreate);
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Read(byData, 0, 16000);
                    Decoder d = Encoding.Default.GetDecoder();
                    d.GetChars(byData, 0, byData.Length, charData, 0);
                    fs.Close();
                    StringBuilder s=new StringBuilder() ;
                    for (int x = 0; x < charData.Length; x++)
                    {
                        s.Append(charData[x]);
                    }
                    textBox3.Text = s.ToString();
                    textBox3.Text = 窗口.DecryptDES(textBox3.Text, "99594703");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    button2_Click(null, null);
                }
                finally
                {
                    //关闭等待界面
                    if (窗口.等待 != null)
                    {
                        窗口.等待.Close();
                    }
                }
            }
            textBox1.Text = 窗口.GetValue("用户名");
            textBox2.Text = 窗口.DecryptDES((窗口.GetValue("密码")),"74110968");
        }

        private void 登录_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                窗口.SetValue("用户名", textBox1.Text);
                窗口.SetValue("密码", 窗口.EncryptDES((textBox2.Text),"74110968"));
                窗口.SetValue("记住密码", "1");

            }
            else
            {
                窗口.SetValue("用户名", "");
                窗口.SetValue("密码", "");
                窗口.SetValue("记住密码", "0");

            }
            if (checkBox2.Checked == true)
            {
                窗口.SetValue("自动登录", "1");
            }
            else
            {
                窗口.SetValue("自动登录", "0");
            }
            try
            {
                //当登录窗口被关闭时 删除.jpg和.png文件
                DirectoryInfo di = new DirectoryInfo(Application.StartupPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    string exname = fi.Name.Substring(fi.Name.LastIndexOf(".") + 1);//得到后缀名
                                                                                    //判断当前文件后缀名是否与给定后缀名一样
                    if (exname == "jpg"||exname=="png"|| exname == "JPG" || exname == "PNG")
                    {
                        File.Delete(Application.StartupPath+ "\\" + fi.Name);//删除当前文件
                    }
                }
            }
            catch
            {
            }
            if (this.Visible)
            {
                Application.Exit();
            }
            else
            {
                try
                {
                    窗口.conn.Close();
                }
                catch (Exception)
                {

                }
                finally
                {
                    窗口.登录 = null;
                    窗口.连接.Show();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //删除ip.txt文件并关闭此窗口   此时  连接窗口会尝试跳出
            窗口.SetValue("ip", "");
            this.Hide();
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //限制工号文本框只能输入数字或控制字符
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
                checkBox2.Checked = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
                checkBox1.Checked = true;
        }

        private void 登录_Shown(object sender, EventArgs e)
        {
            if (窗口.GetValue("自动登录") == "1")
            {
                checkBox2.Checked = true;
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    button1_Click(null, null);
                }
            }
        }

        private void checkBox2_MouseEnter(object sender, EventArgs e)
        {
            ToolTip p = new ToolTip();
            p.ShowAlways = true;
            p.SetToolTip(checkBox2, "不推荐，将无法看到公告");
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar)&& !char.IsNumber(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }
    }
}
