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
    public partial class 请假信息 : Form
    {
        object a = "";
        public 请假信息()
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
        private void 请假信息_Load(object sender, EventArgs e)
        {
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
            this.Resize += new EventHandler(Form1_Resize);
            窗口.请假信息 = this;
            try
            {
                //读取信息
                OleDbCommand sql1 = 窗口.conn.CreateCommand();
                sql1.CommandText = "SELECT 姓名,请假理由,请假时间,请假条编号,结束时间 from 请假,人员信息 where 请假人工号=工号 and 是否查看=false";
                OleDbDataReader rea = sql1.ExecuteReader();
                if (rea.Read())
                {
                    label4.Text = rea.GetString(0);
                    label5.Text = rea.GetString(1);
                    label6.Text = rea.GetDateTime(2).ToString("yyyy/MM/dd");
                    label9.Text = rea.GetDateTime(4).ToString("yyyy/MM/dd");
                    label10.Text = (rea.GetDateTime(4) - rea.GetDateTime(2)).ToString("dd") + "天";
                    a = rea.GetValue(3);
                }
                else
                {
                    MessageBox.Show("没有未处理的信息");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //提交结果
                string sql = "update 请假 set 是否通过 =false,是否查看= true where 请假条编号 =" + a;
                OleDbCommand cmd = new OleDbCommand(sql, 窗口.conn);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    MessageBox.Show("成功");
                }
                else
                {
                    MessageBox.Show("失败");
                }
                请假信息_Load(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //提交结果
                string sql = "update 请假 set 是否通过 = true ,是否查看= true where 请假条编号 =" + a;
                OleDbCommand cmd = new OleDbCommand(sql, 窗口.conn);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    MessageBox.Show("成功");
                }
                else
                {
                    MessageBox.Show("失败");
                }
                请假信息_Load(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 请假信息_FormClosed(object sender, FormClosedEventArgs e)
        {
            窗口.请假信息 = null;
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
