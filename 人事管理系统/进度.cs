using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 人事管理系统
{
    public partial class 进度 : Form
    {
        private BackgroundWorker backgroundWorker1; //ProcessForm 窗体事件(进度条窗体)
        public 进度(BackgroundWorker backgroundWorker1,int rows)
        {
            InitializeComponent();
            this.progressBar1.Maximum = rows;
            this.backgroundWorker1 = backgroundWorker1;
            this.backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);         
        }
        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();//执行完之后，直接关闭页面
        }
        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                this.progressBar1.Value = e.ProgressPercentage;
            }
            catch
            {
                button1_Click(null, null);
            }

        }
        private void 进度_Load(object sender, EventArgs e)
        {
        }
        private void 进度_FormClosed(object sender, FormClosedEventArgs e)
        {                     
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.CancelAsync();
            this.button1.Enabled = false;
            this.Close();
        }
    }
}
