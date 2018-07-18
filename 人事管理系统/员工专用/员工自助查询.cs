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
    public partial class 员工自助查询 : Form
    {
        string a = "";
        public 员工自助查询()
        {
            //读取工号并注册窗体
            a = 窗口.ID;
            窗口.员工自助查询 = this;
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //返回登录界面
            if (MessageBox.Show("是否确认注销？", "注销", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Hide();
                this.Close();
                窗口.leave_count = 0;
                窗口.error_count = 0;
                窗口.登录.Show();
                if (this.WindowState != FormWindowState.Minimized)
                {
                    if (窗口.GetValue("默认大小") == "1")
                    {
                        窗口.登录.WindowState = FormWindowState.Normal;
                    }
                    else if (窗口.GetValue("默认大小") == "2")
                    {
                        窗口.登录.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        窗口.登录.WindowState = this.WindowState;
                    }
                }
                else
                {
                    if (窗口.GetValue("默认大小") == "2")
                    {
                        窗口.登录.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        窗口.登录.WindowState = this.WindowState;
                    }
                }
                窗口.登录.Focus();
            }
            else
            {
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
        private void 员工自助查询_Load(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            z = this.Left;
            s = this.Top;
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
            this.Resize += new EventHandler(Form1_Resize);
            //注册时间控件用于查询请假和错误报告有没有被处理
            Timer Timer = new Timer();
            Timer.Interval = 1000;
            Timer.Tick += new EventHandler(timer1_Tick);
            Timer.Start();
            Timer Timer2 = new Timer();
            Timer2.Interval = 1000;
            Timer2.Tick += new EventHandler(timer2_Tick);
            Timer Timer3 = new Timer();
            Timer3.Interval = 1000;
            Timer3.Tick += new EventHandler(timer3_Tick);
            Timer3.Start();
            try
            {
                //读取基本信息
                string sqlstr = "SELECT 姓名,基本工资,奖金,奖励和处分信息,人员信息.可休假期,部门名称,照片位置,部门名称,职位,联系电话 from 人员信息,职位信息 where 工号 ='" + a + "' and 人员信息.职位编号 = 职位信息.职位编号";
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlstr, 窗口.conn);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                label1.Text = dt.Rows[0][0].ToString();
                label2.Text = a;
                label8.Text = dt.Rows[0][1].ToString();
                label9.Text = dt.Rows[0][2].ToString();
                label16.Text = dt.Rows[0][7].ToString();
                label17.Text = dt.Rows[0][8].ToString();
                //如果有照片 则载入照片
                if (dt.Rows[0][6].ToString() != "")
                {
                    //如果服务器文件存在
                    if (File.Exists(窗口.ip + 窗口.add + dt.Rows[0][6].ToString()))
                    {//且本地无此文件
                        if (!File.Exists(Path.Combine(@"", Path.GetFileName(窗口.ip + 窗口.add + dt.Rows[0][6].ToString()))))
                        { //就复制
                            string destPath = Path.Combine(@"", Path.GetFileName(窗口.ip + 窗口.add + dt.Rows[0][6].ToString()));
                            System.IO.File.Copy(窗口.ip + 窗口.add + dt.Rows[0][6].ToString(), destPath);
                            pictureBox2.Load(destPath);
                        }
                        else
                        {
                            string destPath = Path.Combine(@"", Path.GetFileName(窗口.ip + 窗口.add + dt.Rows[0][6].ToString()));
                            pictureBox2.Load(destPath);
                        }
                    }
                    else
                    {
                        MessageBox.Show("照片被删除 请联系人事更新");
                        try
                        {
                            string sql10 = "update 人员信息 set 照片位置='' where 工号='" + a + "';";
                            OleDbCommand cmd = new OleDbCommand(sql10, 窗口.conn);
                            cmd.ExecuteNonQuery();
                        }
                        catch
                        {

                        }
                        
                    }
                }
                if (dt.Rows[0][9].ToString() == "")
                    label18.Text = "无信息";
                else
                    label18.Text = dt.Rows[0][9].ToString();
                label12.Text = dt.Rows[0][4].ToString() + "天";
                //如果是人事部的  就显示人事管理的按钮S
                if (dt.Rows[0][5].ToString() == "人事部")
                {
                    button6.Visible = true;
                }
                else
                {
                    contextMenuStrip1.Items[4].Visible = false;
                    contextMenuStrip1.Items[4].Enabled = false;
                }
                label7.Text = ((int)dt.Rows[0][1] + (int)dt.Rows[0][2]).ToString();
                //查询是否有未处理的请假信息
                string sql = "SELECT 请假条编号 from 请假 where 请假人工号='" + a + "' and 是否通知 = false";
                OleDbDataAdapter od = new OleDbDataAdapter(sql, 窗口.conn);
                DataTable d = new DataTable();
                od.Fill(d);
                if (d.Rows.Count > 0)
                {
                    //如果有 就记录
                    窗口.leave_count = 1;
                }
                //是否有未处理的错误报告
                OleDbCommand sql2 = 窗口.conn.CreateCommand();
                sql2.CommandText = "SELECT 错误内容,编号 from 错误报告 where 申请人工号='" + a + "'";
                OleDbDataReader read = sql2.ExecuteReader();
                while (true)
                {
                    if (read.Read())
                    {
                        //有则记录
                        窗口.error_count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (窗口.修改密码 != null)
            {
            }
            else
            {
                修改密码 修改密码 = new 修改密码();
            }
            窗口.修改密码.Show();
            if (this.WindowState != FormWindowState.Minimized)
            {
                if (窗口.GetValue("默认大小") == "1")
                {
                    窗口.修改密码.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    窗口.修改密码.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    窗口.修改密码.WindowState = this.WindowState;
                }
            }
            else
            {
                if (窗口.GetValue("默认大小") == "2")
                {
                    窗口.修改密码.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    窗口.修改密码.WindowState = FormWindowState.Normal;
                }
            }
            窗口.修改密码.Focus();
            this.Hide();
        }

        private void 员工自助查询_FormClosed(object sender, FormClosedEventArgs e)
        {
            窗口.员工自助查询 = null;
            timer3.Stop();
            timer3.Dispose();
            this.Dispose();

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (窗口.报告错误 == null)
            {
                员工专用.报告错误 报告错误 = new 员工专用.报告错误();
            }
            else
            {
                窗口.报告错误.Focus();
            }
            窗口.报告错误.Show();
            if (this.WindowState != FormWindowState.Minimized)
            {
                if (窗口.GetValue("默认大小") == "1")
                {
                    窗口.报告错误.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    窗口.报告错误.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    窗口.报告错误.WindowState = this.WindowState;
                }
            }
            else
            {
                if (窗口.GetValue("默认大小") == "2")
                {
                    窗口.报告错误.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    窗口.报告错误.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void 员工自助查询_Shown(object sender, EventArgs e)
        {
            button4.Focus();
            if (窗口.等待 != null)
            {
                窗口.等待.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DateTime Date_time = DateTime.Today;
            //获取当前系统日期
            DateTime Add_Date_time = DateTime.Today.AddDays(1);//在原有日期上加一
            OleDbCommand sql1 = 窗口.conn.CreateCommand();
            sql1.CommandText = "SELECT 日期 FROM 出勤记录 WHERE 工号 ='" + 窗口.ID + "'";
            OleDbDataReader rea1 = sql1.ExecuteReader();
            if (rea1.Read())
            {

                if (rea1.GetDateTime(0) <= Date_time)
                {
                    OleDbCommand sql3 = 窗口.conn.CreateCommand();
                    sql3.CommandText = "UPDATE 出勤记录 SET 日期 = '" + Add_Date_time.ToString() + "',出勤次数=出勤次数+1 WHERE 工号='" + 窗口.ID + "';";
                    OleDbDataReader rea5 = sql3.ExecuteReader();
                    MessageBox.Show("打卡成功！");
                }
                else
                {
                    MessageBox.Show("请勿重复打卡");
                }
            }
            else
            {
                OleDbCommand sql7 = 窗口.conn.CreateCommand();
                sql7.CommandText = " insert into 出勤记录(工号, 出勤次数,日期) values('" + 窗口.ID + "', 0,'" + Date_time + "') ";
                OleDbDataReader rea5 = sql7.ExecuteReader();
                button4_Click(null, null);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (窗口.leave_count == 0)
            {
                //没有正在处理的请假时 才能请假
                if (窗口.请假 == null)
                {
                    请假 请假 = new 请假();
                }
                else
                {
                    窗口.请假.Focus();
                }
                窗口.请假.Show();
                if (this.WindowState != FormWindowState.Minimized)
                {
                    if (窗口.GetValue("默认大小") == "1")
                    {
                        窗口.请假.WindowState = FormWindowState.Normal;
                    }
                    else if (窗口.GetValue("默认大小") == "2")
                    {
                        窗口.请假.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        窗口.请假.WindowState = this.WindowState;
                    }
                }
                else
                {
                    if (窗口.GetValue("默认大小") == "2")
                    {
                        窗口.请假.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        窗口.请假.WindowState = FormWindowState.Normal;
                    }
                }
            }
            else
            {
                MessageBox.Show("您有正在处理的请假请求！");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(窗口.修改密码!=null)
            {
                窗口.修改密码.Close();
            }
            if(窗口.请假!=null)
            {
                窗口.请假.Close();
            }
            if(窗口.报告错误!=null)
            {
                窗口.报告错误.Close();
            }
            人事专用.人事选择进入界面 人事选择进入界面 = new 人事专用.人事选择进入界面();
            人事选择进入界面.Show();
            if (this.WindowState != FormWindowState.Minimized)
            {
                if (窗口.GetValue("默认大小") == "1")
                {
                    人事选择进入界面.WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    人事选择进入界面.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    人事选择进入界面.WindowState = this.WindowState;
                }
            }
            else
            {
                if (窗口.GetValue("默认大小") == "2")
                {
                    人事选择进入界面.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    人事选择进入界面.WindowState =FormWindowState.Normal;
                }
            }
            人事选择进入界面.Focus();
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if ((notifyIcon1.Visible) && 窗口.leave_count == 1)
            {
                //如果有在处理的请假信息  就查询
                try
                {
                    OleDbCommand sql1 = 窗口.conn.CreateCommand();
                    sql1.CommandText = "SELECT 请假时间,请假条编号,是否通过,结束时间 from 请假 where 请假人工号='" + a + "' and 是否查看=true and 是否通知 = false";
                    OleDbDataReader rea = sql1.ExecuteReader();
                    if (rea.Read())
                    {
                        string sql = "update 请假 set 是否通知=true where 请假条编号 =" + rea.GetValue(1);
                        OleDbCommand cmd = new OleDbCommand(sql, 窗口.conn);
                        int res = cmd.ExecuteNonQuery();
                        窗口.leave_count = 0;
                        if (rea.GetBoolean(2))
                            MessageBox.Show("你的" + rea.GetDateTime(0).ToString("yyyy/MM/dd") + "至" + rea.GetDateTime(3).ToString("yyyy/MM/dd") + "的请假已通过");
                        else
                            MessageBox.Show("你的" + rea.GetDateTime(0).ToString("yyyy/MM/dd") + "至" + rea.GetDateTime(3).ToString("yyyy/MM/dd") + "的请假未通过");

                    }
                }
                catch
                {
                    this.Hide();
                    this.Close();
                    if (窗口.登录 != null)
                        窗口.登录.Show();
                }
            }
            if ((notifyIcon1.Visible) && 窗口.error_count > 0)
            { //如果有在处理的错误报告 就查询
                try
                {
                    OleDbCommand sql2 = 窗口.conn.CreateCommand();
                    sql2.CommandText = "SELECT 错误内容,编号 from 错误报告 where 申请人工号='" + a + "' and 是否处理=true and 是否通知 = false";
                    OleDbDataReader read = sql2.ExecuteReader();
                    if (read.Read())
                    {
                        string sql = "update 错误报告 set 是否通知=true where 编号 =" + read.GetValue(1);
                        OleDbCommand cmd = new OleDbCommand(sql, 窗口.conn);
                        int res = cmd.ExecuteNonQuery();
                        if (res > 0)
                        {
                            MessageBox.Show("你提交的错误：\n" + read.GetString(0) + "\n已处理\n如修改过照片 请重新登录以更新");
                            窗口.error_count--;
                        }
                    }
                }
                catch
                {
                    this.Hide();
                    this.Close();
                    if (窗口.登录 != null)
                    {
                        窗口.登录.Show();
                    }
                }
            }
        }

        private void 员工自助查询_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                button5_Click(null, null);
            }
        }

        private void 员工自助查询_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Visible==true)
            {
                if (窗口.GetValue("默认不关闭") != "0")
                {
                    e.Cancel = true;    //取消"关闭窗口"事件
                    this.Hide();
                    return;
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (窗口.人事选择进入界面 == null)
            {
                this.Show();
                if (窗口.GetValue("默认大小") == "1")
                {
                    WindowState = FormWindowState.Normal;
                }
                else if (窗口.GetValue("默认大小") == "2")
                {
                    WindowState = FormWindowState.Maximized;
                }
                else
                {

                }
                this.Focus();
            }
            else
            {
                窗口.人事选择进入界面.Show();
                窗口.人事选择进入界面.WindowState =this.WindowState;
                窗口.人事选择进入界面.Focus();
            }
        }

        private void 注销_Click(object sender, EventArgs e)
        {
            button5_Click(null, null);
        }

        private void 更改密码_Click(object sender, EventArgs e)
        {
            button1_Click(null, null);
        }
        private void 报告错误_Click(object sender, EventArgs e)
        {
            Button2_Click(null, null);
        }

        private void 请假_Click(object sender, EventArgs e)
        {
            button3_Click(null, null);
        }

        private void 退出_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认退出程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // 关闭所有的线程
                this.Dispose();
                this.Hide();
                this.Close();
                Application.Exit();
            }
        }

        private void 人事管理_Click(object sender, EventArgs e)
        {
            button6_Click(null, null);
        }
        //每秒判断一次是否存在文件 x 
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                //删除x文件
                DirectoryInfo di = new DirectoryInfo(Application.StartupPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    string exname = fi.Name;//得到文件名
                    if (exname == "x")
                    {
                        File.Delete(Application.StartupPath + "\\" + fi.Name);//删除当前文件
                        this.Show();
                        this.WindowState = FormWindowState.Normal;
                        this.Focus();
                    }
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 设置_Click(object sender, EventArgs e)
        {
            if (窗口.设置 == null)
            {
                设置 设置 = new 设置();
            }
            窗口.设置.Show();
            窗口.设置.WindowState = FormWindowState.Normal;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (!File.Exists(窗口.ip + 窗口.add + "\\已登录\\" + 窗口.ID))
            {
                this.Hide();
                this.Close();
                ////////////
                //补上被删除的文件  防止timer3重复触发
                if (!File.Exists(窗口.ip + 窗口.add + "\\已登录\\" + 窗口.ID))
                {
                    FileStream fs = new FileStream(窗口.ip + 窗口.add + "\\已登录\\" + 窗口.ID, FileMode.OpenOrCreate);
                    fs.Close();
                }
                else
                {
                }
                ////////////
                MessageBox.Show("该账号已在其余地区登陆\n若非本人操作\n请及时修改密码", "已下线", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                窗口.登录.Show();
                if (this.WindowState != FormWindowState.Minimized)
                {
                    if (窗口.GetValue("默认大小") == "1")
                    {
                        窗口.登录.WindowState = FormWindowState.Normal;
                    }
                    else if (窗口.GetValue("默认大小") == "2")
                    {
                        窗口.登录.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        窗口.登录.WindowState = this.WindowState;
                    }
                }
                else
                {
                    if (窗口.GetValue("默认大小") == "2")
                    {
                        窗口.登录.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        窗口.登录.WindowState = this.WindowState;
                    }
                }
                窗口.登录.Focus();
            }
        }

        private void 员工自助查询_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible==true)
            {
                timer2.Stop();  //当此窗口可见时   暂定计时器防止占用过多资源
            }
            else
            {
                timer2.Start(); //此窗口隐藏时  启用计时器
            }
            this.Left = z;
            this.Top = s;
            try
            {
                //读取基本信息
                string sqlstr = "SELECT 姓名,基本工资,奖金,奖励和处分信息,人员信息.可休假期,部门名称,照片位置,部门名称,职位,联系电话 from 人员信息,职位信息 where 工号 ='" + a + "' and 人员信息.职位编号 = 职位信息.职位编号";
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlstr, 窗口.conn);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                label1.Text = dt.Rows[0][0].ToString();
                label2.Text = a;
                label8.Text = dt.Rows[0][1].ToString();
                label9.Text = dt.Rows[0][2].ToString();
                label16.Text = dt.Rows[0][7].ToString();
                label17.Text = dt.Rows[0][8].ToString();
                //如果有照片 则重新载入照片
                if (dt.Rows[0][6].ToString() != "")
                {
                    //如果服务器文件存在
                    if (File.Exists(窗口.ip + 窗口.add + dt.Rows[0][6].ToString()))
                    {//且本地无此文件
                        if (!File.Exists(Path.Combine(@"", Path.GetFileName(窗口.ip + 窗口.add + dt.Rows[0][6].ToString()))))
                        { //就复制
                            string destPath = Path.Combine(@"", Path.GetFileName(窗口.ip + 窗口.add + dt.Rows[0][6].ToString()));
                            System.IO.File.Copy(窗口.ip + 窗口.add + dt.Rows[0][6].ToString(), destPath);
                            pictureBox2.Load(destPath);
                        }
                    }
                    else
                    {
                        MessageBox.Show("照片被删除 请联系人事更新");
                        try
                        {
                            string sql10 = "update 人员信息 set 照片位置='' where 工号='" + a + "';";
                            OleDbCommand cmd = new OleDbCommand(sql10, 窗口.conn);
                            cmd.ExecuteNonQuery();
                        }
                        catch
                        {

                        }
                    }
                }
                if (dt.Rows[0][9].ToString() == "")
                    label18.Text = "无信息";
                else
                    label18.Text = dt.Rows[0][9].ToString();
                label12.Text = dt.Rows[0][4].ToString() + "天";
                //如果是人事部的  就显示人事管理的按钮
                if (dt.Rows[0][5].ToString() == "人事部")
                {
                    button6.Visible = true;
                    contextMenuStrip1.Items[4].Visible = true;
                    contextMenuStrip1.Items[4].Enabled = true;
                }
                else
                {
                    button6.Visible = false;
                    contextMenuStrip1.Items[4].Visible = false;
                    contextMenuStrip1.Items[4].Enabled = false;
                }
                label7.Text = ((int)dt.Rows[0][1] + (int)dt.Rows[0][2]).ToString();
                //查询是否有未处理的请假信息
                string sql = "SELECT 请假条编号 from 请假 where 请假人工号='" + a + "' and 是否通知 = false";
                OleDbDataAdapter od = new OleDbDataAdapter(sql, 窗口.conn);
                DataTable d = new DataTable();
                od.Fill(d);
                if (d.Rows.Count > 0)
                {
                    //如果有 就记录
                    窗口.leave_count = 1;
                }
                //是否有未处理的错误报告
                OleDbCommand sql2 = 窗口.conn.CreateCommand();
                sql2.CommandText = "SELECT 错误内容,编号 from 错误报告 where 申请人工号='" + a + "'";
                OleDbDataReader read = sql2.ExecuteReader();
                while (true)
                {
                    if (read.Read())
                    {
                        //有则记录
                        窗口.error_count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }
    }
}