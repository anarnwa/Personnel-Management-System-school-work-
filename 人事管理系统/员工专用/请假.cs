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
    public partial class 请假 : Form
    {
        string a = "";
        public 请假()
        {
            a = 窗口.ID;
            InitializeComponent();
            窗口.请假 = this;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() != "")
            {
                //理由不为空
                if (dateTimePicker1.Value >= DateTime.Now.Date)
                {
                    //开始时间大于当前时间
                    if (dateTimePicker2.Value>dateTimePicker1.Value)
                    {
                        //结束时间大于开始时间
                        try
                        {
                            //插入
                            string sql = "insert into 请假 (请假人工号,请假理由,请假时间,结束时间) values ('" + a + "','" + textBox2.Text + "','" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "','" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "');";
                            OleDbCommand cmd = new OleDbCommand(sql, 窗口.conn);
                            int res = cmd.ExecuteNonQuery();
                            if (res > 0)
                            {
                                //置变量为1   即不能继续请假
                                窗口.leave_count = 1;
                                MessageBox.Show("申请成功！ 请等候通知！");
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("申请失败");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message );
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("结束日期必须大于开始日期");
                    }
                }
                else
                    MessageBox.Show("开始时间必须大于当前时间");
            }
            else
                MessageBox.Show("理由不能为空！");
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
        private int z, s;
        private void 请假_Load(object sender, EventArgs e)
        {
            z = Left;
            s = Top;
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
            this.Resize += new EventHandler(Form1_Resize);
            //读取当前日期并让结束日期自动变为当前时间+1
            dateTimePicker2.Value = DateTime.Now.AddDays(1);
            OleDbCommand sql = 窗口.conn.CreateCommand();
            sql.CommandText = "SELECT 姓名 FROM 人员信息 WHERE 工号 = '" + a + "'";
            try
            { 
                OleDbDataReader rea = sql.ExecuteReader();
                rea.Read();
                label3.Text = rea.GetString(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
                this.Close();
            }
        }

        private void 请假_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:button2_Click(null, null); break;
                case Keys.Escape:button1_Click(null, null);break;
            }
        }

        private void 请假_VisibleChanged(object sender, EventArgs e)
        {
            Top = s;
            Left = z;
        }

        private void 请假_FormClosed(object sender, FormClosedEventArgs e)
        {
            窗口.请假 = null;
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
