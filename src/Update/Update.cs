using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace Update
{

    /// <summary>
    /// 更新程序代码
    /// </summary>
    public partial class Update : Form
    {
        string SoftVer;
        CheckProgramUpdate u = new CheckProgramUpdate();
        public Update()
        {
            InitializeComponent();            
        }
        /// <summary>
        /// 获取超级狗中的数据文件内容
        /// </summary>
        /// <param name="FileId">数据文件ID</param>
        /// <returns></returns>
        private string GetDogFile(int FileId)
        {
            SuperDog.Dog dog = new SuperDog.Dog(SuperDog.DogFeature.Default);
            SuperDog.DogStatus dogSta = dog.Login(DogVendorCode.Code);
            if (dogSta == SuperDog.DogStatus.StatusOk)
            {
                //读取第一个数据文件（即用户名)
                SuperDog.DogFile df = dog.GetFile(FileId);
                if (df.IsLoggedIn())
                {
                    int size = 0;
                    dogSta = df.FileSize(ref size);
                    if (dogSta == SuperDog.DogStatus.StatusOk)
                    {
                        byte[] bytes = new byte[size];
                        dogSta = df.Read(bytes, 0, bytes.Length);
                        if (SuperDog.DogStatus.StatusOk == dogSta)
                        {
                            string aaa = Encoding.ASCII.GetString(bytes);
                            if (dog.IsLoggedIn()) { dog.Logout(); }
                            return aaa;
                        }
                    }
                }
            }
            if (dog.IsLoggedIn()) { dog.Logout(); }
            return "";
        }
        private void Update_Load(object sender, EventArgs e)
        {
            //获取软件版本（1、基础版。2、高级版。3、专业版）
            SoftVer = string.IsNullOrWhiteSpace(GetDogFile(2))?"1":GetDogFile(2);
            if (!SoftVer.Equals("1"))
            {
                lb_checkstate.Text = "正在检查更新：";
                u.ShowUpdate += new CheckProgramUpdate.ShowUpdateInfoEventHandler(ShowCheckInfo);
                u.check();
            }
            else
            {
                lb_checkresult.Text = "当前软件版本不允许更新";
                lb_checkstate.Text = "当前软件版本不允许更新";
                pictureBox1.Visible = false;
                dataGridView1.Visible = false;
                btn_beginupdate.Visible = false;
            }
            
        }
        private void ShowCheckInfo(UpdateInfo ui)
        {
            if (ui.FilesTotalSize == 0 || ui.CheckResult.Equals("您目前的版本已是最新") || ui.CheckResult.Equals("更新检查无法完成，请联系软件发布商"))
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    lb_checkresult.Text = ui.CheckResult;
                    lb_checkstate.Text = "检查更新完成";
                    pictureBox1.Visible = false;
                    dataGridView1.Visible = false;
                    btn_beginupdate.Visible = false;
                }));
            }
            else
            {
                this.BeginInvoke(new EventHandler(ShowUpdateInfo), ui);
            }
        }
        private void ShowUpdateInfo(object sender,EventArgs args)
        {
            pictureBox1.Visible = false;
            lb_checkresult.Visible = false;
            UpdateInfo ui = (UpdateInfo)sender;
            foreach (UpdateFileInfo ufi in ui.UpdateFilesInfo)
            {
                dataGridView1.Rows.Add(new object[] { ufi.FileName, ufi.FileVersion, ufi.FileSize + ui.FilesTotalSizeUnit, "0%" });
            }
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[0].Selected = false;
                lb_checkstate.Text = "检查更新完成。本次更新大小共" + ui.FilesTotalSize + ui.FilesTotalSizeUnit;
                dataGridView1.Visible = true;
                btn_beginupdate.Visible = true;
            }
        }

        private void btn_beginupdate_Click(object sender, EventArgs e)
        {
            u.UpdateProgress -= new CheckProgramUpdate.UpdateProgressVal(u_UpdateProgress);
            u.UpdateProgress +=new CheckProgramUpdate.UpdateProgressVal(u_UpdateProgress);
            Thread t = new Thread(new ParameterizedThreadStart(u.update));
            t.Start(System.AppDomain.CurrentDomain.BaseDirectory);
        }
        private void u_UpdateProgress(int rowId,int percent,bool isLast)
        {
            this.BeginInvoke(new MethodInvoker(delegate() {
                dataGridView1.Rows[rowId].Cells[3].Value = percent + "%";
                if (isLast && percent == 100)
                {
                    if (MessageBox.Show("已全部更新完成，点击确定，重新启动程序", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                    {
                        RestartProgram();
                    }
                }
            }));
        }
        /// <summary>
        /// 替换主程序并重启
        /// </summary>
        private void RestartProgram()
        {
            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("Finder");
            foreach (System.Diagnostics.Process p in process)
            {
                p.Kill();
            }
            string[] files = Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory + "/tmp/");
            foreach (string file in files)
            {
                string targetFile = System.AppDomain.CurrentDomain.BaseDirectory + file.Substring(file.LastIndexOf('/')+1);
                if (File.Exists(targetFile))
                {
                    File.Delete(targetFile);
                }
                File.Move(file, targetFile);
            }
            Directory.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "/tmp/",true);
            Application.Exit();
            Process.Start("Finder.exe");
        }

        private void pb_close_MouseEnter(object sender, EventArgs e)
        {
            pb_close.BackColor = Color.FromArgb(255, 102, 0);
        }

        private void pb_close_MouseLeave(object sender, EventArgs e)
        {
            pb_close.BackColor = Color.FromArgb(136, 34, 17);
        }

        private void pb_min_MouseEnter(object sender, EventArgs e)
        {
            pb_min.BackColor = Color.FromArgb(112, 205, 225);
        }

        private void pb_min_MouseLeave(object sender, EventArgs e)
        {
            pb_min.BackColor = Color.Transparent;
        }

        private void pb_min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pb_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
