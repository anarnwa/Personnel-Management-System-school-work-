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

namespace 人事管理系统.员工专用
{
    public partial class 报告错误 : Form
    {
        string a = 窗口.ID;
        public 报告错误()
        {           
            InitializeComponent();
            窗口.报告错误 = this;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                //文本框不为空
                string sql = "insert into 错误报告 (申请人工号,错误内容) values ('" + a + "','" + textBox1.Text + "');";
                try
                {
                    //提交
                    OleDbCommand cmd = new OleDbCommand(sql, 窗口.conn);
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        MessageBox.Show("错误提交成功！");
                        //已提交的错误计数+1
                        窗口.error_count++;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("提交失败");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message );
                    this.Close();
                }
            }
            else
                MessageBox.Show("内容不能为空");
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
        private void 报告错误_Load(object sender, EventArgs e)
        {
            z = Left;
            s = Top;
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
            this.Resize += new EventHandler(Form1_Resize);
            //获取提交者的信息
            string sqlstr = "SELECT 姓名 FROM 人员信息 WHERE 工号 = '" + a + "'";
            try
            {
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlstr, 窗口.conn);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                label2.Text = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
                this.Close();
            }
        }

        private void 报告错误_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
            if (e.KeyCode == Keys.Escape)
            {
                button2_Click(null, null);
            }
        }

        private void 报告错误_VisibleChanged(object sender, EventArgs e)
        {
            Top = s;
            Left = z;
        }

        private void 报告错误_FormClosed(object sender, FormClosedEventArgs e)
        {
            窗口.报告错误 = null;
            if (窗口.员工自助查询.Visible == true)
            {
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
            }
        }
    }
}
