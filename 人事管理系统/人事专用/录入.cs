using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace 人事管理系统.人事专用
{
    public partial class 录入 : Form
    {

        private bool x = false;
        private bool y = true;
        private string pathname = "";
        public 录入()
        {
            InitializeComponent();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
            {
                try
                {
                    //如果其余的文本框不为空
                    if (x && y)
                    {
                        try
                        {
                            if (pictureBox1.Image != null)
                            {
                                //删除服务器端图片
                                if (File.Exists(窗口.ip + 窗口.add + "\\照片\\" + textBox2.Text + Path.GetExtension(pathname)))
                                {
                                    File.Delete(窗口.ip + 窗口.add + "\\照片\\" + textBox2.Text + Path.GetExtension(pathname));
                                }
                                //存放图片到服务器
                                pictureBox1.Image.Save(窗口.ip + 窗口.add + "\\照片\\" + textBox2.Text + Path.GetExtension(pathname));
                                //更新
                                string sql = "insert into 人员信息 (姓名,性别,工号,工资卡号,职位编号,联系电话,学历,出生年月,照片位置) values ('" + textBox1.Text + "','" + comboBox1.Text + "', '" + textBox2.Text + "','" + textBox5.Text + "','" + textBox4.Text + "','" + textBox6.Text + "','" + comboBox2.Text + "','" + dateTimePicker1.Value.ToShortDateString() + "','" + "\\照片\\" + textBox2.Text + Path.GetExtension(pathname)+"'); ";
                                OleDbCommand cmd = new OleDbCommand(sql, 窗口.conn);
                                int res = cmd.ExecuteNonQuery();
                                if (res > 0)
                                {
                                    MessageBox.Show("添加数据成功！");
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("添加失败", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                string sql = "insert into 人员信息 (姓名,性别,工号,工资卡号,职位编号,联系电话,学历,出生年月) values ('" + textBox1.Text + "','" + comboBox1.Text + "', '" + textBox2.Text + "','" + textBox5.Text + "','" + textBox4.Text + "','" + textBox6.Text + "','" + comboBox2.Text + "','" + dateTimePicker1.Value.ToShortDateString() + "'); ";
                                OleDbCommand cmd = new OleDbCommand(sql, 窗口.conn);
                                int res = cmd.ExecuteNonQuery();
                                if (res > 0)
                                {
                                    MessageBox.Show("添加数据成功！");
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("添加失败", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("请填写空白处", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        private void 录入_Load(object sender, EventArgs e)
        {
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
            this.Resize += new EventHandler(Form1_Resize);
            窗口.录入 = this;
            //初始化
            comboBox1.Text = "男";
            comboBox2.Text = "本科";
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //读取职位信息
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

        private void comboBox2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //comobox2为焦点时  按空格就自动变到下一个
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
            //类似上一个
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

        private void 录入_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void 录入_KeyDown(object sender, KeyEventArgs e)
        {
            //按回车和退出时
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //获取工号看是否冲突
                OleDbCommand sql1 = 窗口.conn.CreateCommand();
                sql1.CommandText = "SELECT * from 人员信息 WHERE 工号='" + textBox2.Text + "';";
                OleDbDataReader rea = sql1.ExecuteReader();
                if (rea.Read())
                {
                    label10.Text = "工号冲突";
                    y = false;
                    button2.Enabled = false;
                    button1.Enabled = false;
                }
                else
                {
                    y = true;
                    label10.Text = "";
                    button2.Enabled = true;
                    button1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //打开图片上传框   限制为 .jpg或png格式
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

        private void textBox1_TextChanged(object sender, EventArgs e)
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
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

        private void 录入_DragEnter(object sender, DragEventArgs e)
        {
            string files = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (Path.GetExtension(files) == ".jpg" || Path.GetExtension(files) == ".png" || Path.GetExtension(files) == ".JPG" || Path.GetExtension(files) == ".PNG")  //判断文件类型，只接受jpg或png文件
            {
                this.pictureBox1.Load(((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString());
                pathname = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            }
            else if (Path.GetExtension(files) == ".xls" || Path.GetExtension(files) == ".XLS") //表格
            {
                textBox3.Text = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            } 
        }
        /// <summary>
        /// 准备删除excel表的数据
        /// </summary>       
        private void delete(int i)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook book;
            Microsoft.Office.Interop.Excel.Worksheet sheet;
            Microsoft.Office.Interop.Excel.Range range;
            excelApp.Visible = false;   //若为true，删除瞬间可以看见 office excel界面
            try
            {
                //打开excel文件
                book = excelApp.Workbooks.Open(textBox3.Text, Missing.Value, false, Missing.Value, Missing.Value, Missing.Value, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                //获取sheet1

                sheet = (Microsoft.Office.Interop.Excel.Worksheet)book.Worksheets[1];
                //获取编辑范围
                try
                {
                    range = (Microsoft.Office.Interop.Excel.Range)sheet.Rows[i, Missing.Value];
                    //删除整行
                    range.EntireRow.Delete(Microsoft.Office.Interop.Excel.XlDeleteShiftDirection.xlShiftUp);

                }
                finally
                {
                    //保存编辑
                    book.Save();
                    //关闭book
                    book.Close();
                }
            }
            finally
            {
                //释放资源        
                excelApp.Workbooks.Close();
                excelApp.Quit();
                //直接杀死excel进程
                Process[] process = Process.GetProcessesByName("EXCEL");
                foreach (Process p in process)
                {
                    if (!p.HasExited)  // 如果程序没有关闭，结束程序
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                }
            }
        }
        //删除函数到此完成
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                if (!File.Exists(textBox3.Text))
                {
                    MessageBox.Show("文件不存在", null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                try
                {
                    OleDbConnection conn = new OleDbConnection();
                    string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + textBox3.Text + ";" + "Extended Properties=Excel 8.0;";
                    conn.ConnectionString = strConn;
                    conn.Open();
                    string strExcel = "";
                    OleDbDataAdapter myCommand = null;
                    strExcel = "select * from [sheet1$]";
                    myCommand = new OleDbDataAdapter(strExcel, strConn);
                    DataSet ds = new DataSet();
                    myCommand.Fill(ds, "table1");
                    //开始尝试写入数据
                    int rows = ds.Tables[0].Rows.Count;
                    conn.Close();
                    backgroundWorker1.WorkerSupportsCancellation = true;
                    this.backgroundWorker1.RunWorkerAsync(); // 运行 backgroundWorker 组件
                    进度 form = new 进度(this.backgroundWorker1, rows);// 显示进度条窗体
                    form.ShowDialog(this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,null,MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("路径不能为空！",null,MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //打开表格上传框   限制为 .xls格式
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = ".";
            file.Filter = "excle(*.xls)|*.xls";
            file.ShowDialog();
            if (file.FileName != string.Empty)
            {
                try
                {
                    textBox3.Text = file.FileName;   //获得文件的绝对路径
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        //选择保存路径
        private string ShowSaveFileDialog()
        {
            string localFilePath = "";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel表格（*.xls）|*.xls";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                localFilePath = sfd.FileName.ToString(); //获得文件路径 
                string fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1); //获取文件名，不带路径
            }

            return localFilePath;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //复制到本地
            string pt = ShowSaveFileDialog();
            if (pt != string.Empty)
            {
                File.Copy(窗口.ip + 窗口.add + "\\批量导入模板.xls", pt);
            }
        }

        private void 录入_FormClosed(object sender, FormClosedEventArgs e)
        {
            窗口.录入 = null;
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(char.IsNumber(e.KeyChar)||char.IsPunctuation(e.KeyChar)||e.KeyChar==32)
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==32)
            {
                e.Handled = true;
            }
        }
        public void UpdateExcel(int row, string value)
        {
            Microsoft.Office.Interop.Excel.Application xApp = new Microsoft.Office.Interop.Excel.Application(); ;
            Microsoft.Office.Interop.Excel.Workbook xBook;
            Microsoft.Office.Interop.Excel.Worksheet xSheet;
            Microsoft.Office.Interop.Excel.Range rng2;
            try
            {
                xApp.Visible = false;
                xBook = xApp.Workbooks._Open(textBox3.Text,
               Missing.Value, Missing.Value, Missing.Value, Missing.Value
               , Missing.Value, Missing.Value, Missing.Value, Missing.Value
               , Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[1];
                rng2 = (Microsoft.Office.Interop.Excel.Range)xSheet.Cells[row, 7];
                rng2.Value = value;
                xBook.Save();
                xSheet = null;
                xBook = null;
            }
            catch (Exception)
            {
                xSheet = null;
                xBook = null;
            }
            finally
            {
                //释放资源        
                xApp.Workbooks.Close();
                xApp.Quit();
                //直接杀死excel进程
                Process[] process = Process.GetProcessesByName("EXCEL");
                foreach (Process p in process)
                {
                    if (!p.HasExited)  // 如果程序没有关闭，结束程序
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                }
            }
        }
        //异步动作执行
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            OleDbConnection conn = new OleDbConnection();
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                //从.xls 中读取数据
                int x = 0;
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + textBox3.Text + ";" + "Extended Properties=Excel 8.0;";
                conn.ConnectionString = strConn;
                conn.Open();
                string strExcel = "";
                OleDbDataAdapter myCommand = null;
                strExcel = "select * from [sheet1$]";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "table1");
                conn.Close();
                //开始尝试写入数据
                int rows = ds.Tables[0].Rows.Count;
                int 成功 = 0;
                for (int i = 0; i < rows; i++)
                {
                    OleDbCommand sql1 = 窗口.conn.CreateCommand();
                    sql1.CommandText = "SELECT * from 人员信息 WHERE 工号='" + ds.Tables[0].Rows[i][0] + "';";
                    OleDbDataReader rea = sql1.ExecuteReader();
                    if (!rea.Read())
                    {
                        OleDbCommand sql2 = 窗口.conn.CreateCommand();
                        sql2.CommandText = "SELECT * from 职位信息 WHERE 职位编号='" + ds.Tables[0].Rows[i][3] + "';";
                        OleDbDataReader rea1 = sql2.ExecuteReader();
                        if (rea1.Read())
                        {
                            try
                            {
                                string sql = "insert into 人员信息 (工号,姓名,性别,职位编号,联系电话,工资卡号) values ('" + ds.Tables[0].Rows[i][0] + "','" + ds.Tables[0].Rows[i][1] + "', '" + ds.Tables[0].Rows[i][2] + "','" + ds.Tables[0].Rows[i][3] + "','" + ds.Tables[0].Rows[i][4] + "','" + ds.Tables[0].Rows[i][5] + "');";
                                OleDbCommand cmd = new OleDbCommand(sql, 窗口.conn);
                                int res = cmd.ExecuteNonQuery();
                                if (res > 0)
                                {
                                    成功++;
                                    delete(x + 2);
                                }
                            }
                            catch
                            {
                                if(ds.Tables[0].Rows[i][2].ToString()!="男"&&ds.Tables[0].Rows[i][2].ToString() != "女")
                                {
                                    x++;
                                    UpdateExcel(x + 1, "性别 格式错误");
                                }
                                else
                                {                               
                                //联系电话或工资卡号格式错误
                                x++;
                                UpdateExcel(x + 1, "联系电话或工资卡号 格式错误");
                               }
                            }
                        }
                        else   //没有这个职位
                        {
                            x++;
                            UpdateExcel(x + 1, "没有这个职位");
                        }
                    }
                    else
                    {   //工号冲突
                        x++;
                        UpdateExcel(x + 1, "工号冲突");

                    }
                    worker.WorkerReportsProgress = true;
                    worker.ReportProgress(成功 + x); //注意：这里向子窗体返回信息值，这里是两个值，一个用于进度条，一个用于进度条上限的。
                    if (worker.CancellationPending)  // 如果用户取消则跳出处理数据代码 
                    {
                        e.Cancel = true;
                        break;
                    }
                }              
                //提示信息
                int cou = rows;
                int last = cou - 成功 -x;
                System.Threading.Thread.Sleep(1000);
                MessageBox.Show("共有" + cou + "条记录\n成功" + 成功 + "条\n失败" + x + "条\n取消"+last+"条");
                if(x!=0)
                {
                    System.Diagnostics.Process.Start(textBox3.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
