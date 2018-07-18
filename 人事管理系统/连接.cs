using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Configuration;

namespace 人事管理系统
{
    public partial class 连接 : Form
    {
        public 连接()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsIP(textBox1.Text)) //如果文本框里的内容是个IP地址  即符合IP地址的格式
            {
                窗口.ip = "\\\\" + textBox1.Text.Trim();   //将IP地址存入变量                
                try
                {
                    窗口.SetValue("ip", textBox1.Text);
                }
                finally
                {
                    //不管有没有成功 都打开登录窗口
                    等待 等待 = new 等待();
                    等待.Show();
                    登录 登录 = new 登录();
                    登录.Show();
                    if (窗口.GetValue("默认大小") == "1")
                    {
                        登录.WindowState = FormWindowState.Normal;
                    }
                    else if (窗口.GetValue("默认大小") == "2")
                    {
                        登录.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        登录.WindowState = this.WindowState;
                    }
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("IP地址格式错误",null,MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
        private int z, s;
        private void 连接_Load(object sender, EventArgs e)
        {
            z = this.Left;
            s = this.Top;
            //定义一个timer控件
            Timer Timer = new Timer();
            Timer.Interval = 1000;
            Timer.Tick += new EventHandler(timer1_Tick);
            Timer.Start();
            //注册窗体
            窗口.连接 = this;
            try
            {
                textBox1.Text = 窗口.GetValue("ip");
                //如果写了IP地址  就自动按下button1
                if(textBox1.Text.Trim()!="")
                {
                    button1_Click(null, null);
                }
            }
            catch(Exception ex)
            {
                 MessageBox.Show(ex.Message,null,MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            } 
        }

        private void 连接_VisibleChanged(object sender, EventArgs e)
        {
            this.Left = z;
            this.Top = s;
            //如果登录窗口存在且被显示则此窗口隐藏
            if (窗口.登录!=null && 窗口.登录.Visible==true&&this.Visible==true)
            {
                this.Hide();
            }
            //当此窗口隐藏时  清空textbox1
            if(this.Visible==false)
            {
                textBox1.Text = "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //时间控件 每秒一次   当登录窗口不存在时 显示此窗口
            if(this.Visible==false&&窗口.登录==null)
            this.Show();
        }

        private void 连接_KeyDown(object sender, KeyEventArgs e)
        {
            //按键动作
            switch (e.KeyCode)
            {
                case Keys.Enter: button1_Click(null, null); break;
                case Keys.Escape: Application.Exit(); break;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private static bool IsIP(string ip)
        {
            //判断是否为IP
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //当在textbox1 中输入时
            if (textBox1.Text != "")
            {
                //如果不是第一位 则不能连续输入..
                if (textBox1.Text.Substring(textBox1.Text.Length - 1, 1) == "." && !char.IsNumber(e.KeyChar) && e.KeyChar != 8)
                {
                    e.Handled = true;
                }
            }
            else if(!char.IsNumber(e.KeyChar) && e.KeyChar != 8)
            {
                //否则 不能输入.
                e.Handled = true;
            }
            if (!char.IsNumber(e.KeyChar)&&e.KeyChar!=46&&e.KeyChar!=8)
            {
                //不管是哪一位 只能输入数字  .   和控制字符 如回车 删除
                e.Handled = true;
            }             
        }
    }
}
