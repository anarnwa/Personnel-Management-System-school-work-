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

namespace 人事管理系统.人事专用
{
    public partial class 编辑公告 : Form
    {
        public 编辑公告()
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
        private void 编辑公告_Load(object sender, EventArgs e)
        {
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
            this.Resize += new EventHandler(Form1_Resize);
            窗口.编辑公告 = this;
            try
            {
                byte[] byData = new byte[16000];
                char[] charData = new char[16000];
                FileStream fs = new FileStream(窗口.ip + 窗口.add + "\\公告.txt", FileMode.Open);
                fs.Seek(0, SeekOrigin.Begin);
                fs.Read(byData, 0, 16000);
                Decoder d = Encoding.Default.GetDecoder();
                d.GetChars(byData, 0, byData.Length, charData, 0);
                fs.Close();
                StringBuilder s = new StringBuilder();
                for (int x = 0; x < charData.Length; x++)
                {
                    s.Append(charData[x]);
                }
                textBox1.Text = s.ToString();
                 textBox1.Text = 窗口.DecryptDES(textBox1.Text, "99594703"); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
            finally
            {
                if(窗口.等待!=null)
                {
                    窗口.等待.Close();
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream file = new FileStream(窗口.ip + 窗口.add + "\\公告.txt", FileMode.Create);
                byte[] data = System.Text.Encoding.Default.GetBytes(窗口.EncryptDES(textBox1.Text,"99594703"));
                file.Write(data, 0, data.Length);
                file.Flush();
                file.Close();
                MessageBox.Show("成功");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void 编辑公告_DragEnter(object sender, DragEventArgs e)
        {
            string pathname = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (Path.GetExtension(pathname) == ".txt" || Path.GetExtension(pathname) == ".TXT")  //判断文件类型，只接受txt文件
            {
                try
                {
                    byte[] byData = new byte[1000];
                    char[] charData = new char[1000];
                    FileStream fs = new FileStream(pathname, FileMode.Open);
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Read(byData, 0, 1000);
                    Decoder d = Encoding.Default.GetDecoder();
                    d.GetChars(byData, 0, byData.Length, charData, 0);
                    string s = "";
                    for (int x = 0; x < charData.Length; x++)
                    {
                        s = s + charData[x];
                    }
                    fs.Close();
                    textBox1.Text = s;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }
            }
        }

        private void 编辑公告_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void 编辑公告_FormClosed(object sender, FormClosedEventArgs e)
        {
            窗口.编辑公告 = null;
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
