using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Finder.util;
using System.Threading;
using System.Diagnostics;
using Finder.Entites;
using System.Net;
using System.IO;
using System.Xml;
using Update;
using Finder.Entities;
using Finder.Forms;
using NetDimension.Weibo;
using Finder.UserControles;

namespace Finder
{
    public partial class MainForm : Form
    {
        #region 属性
        //定时6小时读取天报情况
        private System.Timers.Timer weatherT = new System.Timers.Timer(1000 * 60 * 60 * 6);
        //定时3小时检查一次更新
        private System.Timers.Timer updateT = new System.Timers.Timer(1000 * 60 * 60 * 24);
        //定时1小时更新客户端在线状态并获取软件是否已过服务期（使用一个url做这两项工作）。如果已过服务期，给出用户提示，并停止监控。
        private System.Timers.Timer tongjiT = new System.Timers.Timer(1000 * 60 * 60);
        //定时每24小时，删除一次数据库里的过时内容。
        private System.Timers.Timer delExpiredDbData = new System.Timers.Timer(1000 * 60 * 60 * 24);

        //定义鼠标拖拽私有域
        private System.Drawing.Point mousePoint;

        //机器码
        private string machineCode = "";
        //狗ID
        private string dogId = "";

        //官网，以下的所有用到url的地方，如更新、登记客户端都需要用到此地址。
        string remoteUrl = "http://www.shangwukaocha.cn/finder/";

        Forms.SystemSetting ss;
        Forms.Filterdata fd;
        Forms.Weekreport wp;
        Forms.Datachart dc;
        Forms.Monitor monit;
        Forms.Home home;
        Forms.Changgui cg;
        Forms.Mingan mingan;
        Forms.Zhongdian zd;
        Forms.Tufa tufa;
        Forms.Kongzhi kz;
        UC_Menu um;
        #endregion

        #region 初始化
        public MainForm()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();

            if (!Comm.IsNetworkConnectd())
            {
                MessageBox.Show("当前计算机没有联网，程序将退出！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Environment.Exit(0);
            }
            
        }
        /// <summary>
        /// 生成缓存数据
        /// </summary>
        private void GenCacheData()
        {
            //以下缓存报警信息
            DataBaseServer.MySqlCmd dbobj = new DataBaseServer.MySqlCmd();
            string sql = "select Id,EvidenceImgSavePath from systemset";
            DataTable dt = dbobj.GetTabel(sql);
            SystemSet ss = new SystemSet();
            if (dt != null && dt.Rows.Count > 0)
            {
                ss.Id = dt.Rows[0]["Id"].ToString();
                string path = dt.Rows[0]["EvidenceImgSavePath"].ToString();                
                if (!Path.IsPathRooted(path))
                {
                    path = Path.GetFullPath(path);
                }
                ss.EvidenceImgSavePath = path;
            }
            else
            {
                //向数据库插入一条数据
                string path = Path.Combine(Directory.GetCurrentDirectory(), "EvidenceImgSavePath");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                sql = "insert into systemset values('1', 'EvidenceImgSavePath')";
                if (dbobj.ExecuteNonQueryInt(sql) > 0)
                {
                    ss.Id = "1";
                    ss.EvidenceImgSavePath = path;
                }
            }
            GlobalPars.GloPars.Add("systemset", ss);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            //UserInfo ui = (UserInfo)GlobalPars.GloPars["UserInfo"];
            //lb_username.Text = ui.UName;

            //读取超级狗里的数据文件，确定用户ID。
            //dogId = Comm.GetDogFile(1);
            //if (string.IsNullOrEmpty(dogId))
            //{
            //    if (MessageBox.Show("读取加密狗信息失败，请检查加密狗！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.OK)
            //    {
            //        System.Environment.Exit(0);
            //    }
            //}
            //else
            //{
            //    GlobalPars.GloPars.Add("userID", dogId);
            //}
            //获取软件版本（1、基础版。2、高级版。3、专业版）
            string softVer = "3";
            if (string.IsNullOrWhiteSpace(softVer) || (!softVer.Equals("1")) && (!softVer.Equals("2")) && (!softVer.Equals("3")))
            {
                if (MessageBox.Show("读取软件信息错误，请联系软件发布商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.OK)
                {
                    System.Environment.Exit(0);
                }
            }
            GlobalPars.GloPars.Add("SoftVer", softVer);
            GenCacheData();

            string SoftVer = !GlobalPars.GloPars.ContainsKey("SoftVer") ? "2" : GlobalPars.GloPars["SoftVer"].ToString();
            if (!SoftVer.Equals("1"))
            {
                updateT.AutoReset = true;
                updateT.Elapsed += new System.Timers.ElapsedEventHandler(updateT_Elapsed);
                updateT.Enabled = true;
            }

            tongjiT.AutoReset = true;
            tongjiT.Elapsed += new System.Timers.ElapsedEventHandler(tongjiT_Elapsed);
            tongjiT.Enabled = true;

            delExpiredDbData.AutoReset = true;
            delExpiredDbData.Elapsed +=new System.Timers.ElapsedEventHandler(delExpiredDbData_Elapsed);
            delExpiredDbData.Enabled = true;

            //2016.4.19 服务网站已经不可用了，暂停检查
            //检测试用，且第一次打开程序，要向服务端提交客户端在线数
            //Thread trailT = new Thread(new ThreadStart(CheckTrail));
            //trailT.IsBackground = true;
            //trailT.Start();

            //报警
            Alert alert = new Alert(1000 * 60);
            alert.Start();
            
            //生成所有Forms
            AddAllFormsInControl();
            //默认显示监控界面。
            um = uC_Menu1;
            uC_Menu9_UC_Click(null, null);
            //使窗体可以使用透明色。
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);

            if (SoftVer.Equals("1"))
            {
                uC_Menu10.Enabled = false;
                uC_Menu11.Enabled = false;
                uC_Menu12.Enabled = false;
            }

        }
        private void AddAllFormsInControl()
        {
            ss = new Forms.SystemSetting();
            ss.TopLevel = false;
            ss.Dock = DockStyle.Fill;

            fd = new Forms.Filterdata();
            fd.TopLevel = false;
            fd.Dock = DockStyle.Fill;

            wp = new Forms.Weekreport();
            wp.TopLevel = false;
            wp.Dock = DockStyle.Fill;

            dc = new Forms.Datachart();
            dc.TopLevel = false;
            dc.Dock = DockStyle.Fill;

            monit = new Forms.Monitor();
            monit.TopLevel = false;
            monit.Dock = DockStyle.Fill;

            home = new Forms.Home();
            home.TopLevel = false;
            home.Dock = DockStyle.Fill;

            kz = new Forms.Kongzhi();
            kz.TopLevel = false;
            kz.Dock = DockStyle.Fill;

            panel_main.Controls.Add(home);
            //if (!Comm.IsServiced())
            //{
            //    MessageBox.Show("软件出现未知错误，无法运行，请联系软件提供商。程序将退出！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    System.Environment.Exit(0);
            //}
        }
        #endregion

        #region 检查更新
        private void updateT_Elapsed(object sender, EventArgs e)
        {
            CheckProgramUpdate u = new CheckProgramUpdate();
            u.ShowUpdate += new CheckProgramUpdate.ShowUpdateInfoEventHandler(ShowUpdateInfo);
            u.check();
        }
        private void ShowUpdateInfo(UpdateInfo ui)
        {
            //如果有更新，则提示用户
            if (ui.FilesTotalSize != 0 && String.IsNullOrEmpty(ui.CheckResult))
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    if (MessageBox.Show("发现软件有更新版本，是否下载更新？", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                    {
                        Process.Start("Update.exe");
                    }
                }));
            }
        }
        #endregion

        #region 统计客户端和检查试用期
        private void tongjiT_Elapsed(object sender, EventArgs e)
        {
            //2016.4.19 服务网站已经不能用了，暂停检查
            //CheckTrail();
        }
        /// <summary>
        /// 检测试用是否到期
        /// </summary>
        private void CheckTrail()
        {
            try
            {
                machineCode = machineCode == "" ? Comm.GetMachineCode() : machineCode;
                //此url有两个功能。一个是用来统计在线客户端。另一个是用来返回软件是否已经过期
                //string url = remoteUrl + "trail.php?mc=" + Comm.GetMachineCode() + "&m=i";
                string url = remoteUrl + "trail.php?mc=" + dogId + "&m=i";
                HttpWebRequest webreq = (HttpWebRequest)WebRequest.Create(url);
                webreq.Method = "GET";
                WebResponse res = webreq.GetResponse();
                Stream stream = res.GetResponseStream();
                StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                string r = sr.ReadToEnd();
                r = r.Replace("\t", "").Replace("\n", "").Replace("\r", "");
                //如果返回的值为1，则表明软件服务期已过
                if (Comm.isNumeric(r) && int.Parse(r) == 1)
                {
                    this.BeginInvoke(new MethodInvoker(delegate()
                    {
                        //停止监控
                        Program.ProClose = true;
                        Program.isBeyondDate = true;
                        //提示用户软件到期
                        MessageBox.Show("软件服务期已到，抓取服务已不可使用，请联系软件供应商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    Program.ProClose = false;
                }
            }
            catch (Exception ex) { }
        }
        #endregion

        #region 删除数据库过时数据
        private void delExpiredDbData_Elapsed(object sender, EventArgs e)
        {
            string SoftVer = !GlobalPars.GloPars.ContainsKey("SoftVer") ? "1" : GlobalPars.GloPars["SoftVer"].ToString();
            if (SoftVer.Equals("3"))
            {
                string sql = "delete from ReleaseInfo where datetime(CollectDate) < datetime('now','-60 day');";
                DataBaseServer.MySqlCmd dbobj = new DataBaseServer.MySqlCmd();
                dbobj.ExecuteNonQuery(sql);
            }
            else if (SoftVer.Equals("2"))
            {
                string sql = "delete from ReleaseInfo where datetime(CollectDate) < datetime('now','-45 day');";
                DataBaseServer.MySqlCmd dbobj = new DataBaseServer.MySqlCmd();
                dbobj.ExecuteNonQuery(sql);
            }
            else
            {
                string sql = "delete from ReleaseInfo where datetime(CollectDate) < datetime('now','-30 day');";
                DataBaseServer.MySqlCmd dbobj = new DataBaseServer.MySqlCmd();
                dbobj.ExecuteNonQuery(sql);
            }

            
        }
        #endregion

        #region 控件事件
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Comm.VisiteUrl(remoteUrl + "updatehistory.html");
        }

        public void uC_Menu7_Click(object sender, EventArgs e)
        {
            panel_main.Controls.RemoveAt(0);
            panel_main.Controls.Add(ss);
            um.BackgroundImage = Properties.Resources.a;
            um.FontColor = Color.FromArgb(204, 204, 204);
            uC_Menu7.BackgroundImage = Properties.Resources.a1;
            uC_Menu7.FontColor = Color.White;
            um = uC_Menu7;
            ss.Show();
        }

        private void lb_CheckUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("update.exe");
        }

        private void pb_min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pb_close_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确认要退出系统？", "注意", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
            {
                try
                {
                    Thread t = new Thread(new ThreadStart(closeWin));
                    t.IsBackground = true;
                    t.Start();
                }
                catch (Exception e1)
                {
                }
                this.Close();
            }
        }
        private void closeWin()
        {
            //往服务端发送信息，通知服务器此客户端已关闭，更新在线客户端数量
            try
            {
                string url = remoteUrl + "trail.php?mc=" + dogId + "&m=o";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.Timeout = 3000;
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                byte[] data = encoding.GetBytes(url);
                req.ContentLength = data.Length;
                Stream outstream = req.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Flush();
                outstream.Close();
            }
            catch (Exception e)
            {
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

            if (this.WindowState == FormWindowState.Maximized)
            {
                pictureBox2.Image = Properties.Resources.s1;
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                pictureBox2.Image = Properties.Resources.s;
                this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void pb_close_MouseEnter(object sender, EventArgs e)
        {
            pb_close.BackColor = Color.FromArgb(255, 102, 0);
        }

        private void pb_close_MouseLeave(object sender, EventArgs e)
        {
            pb_close.BackColor = Color.FromArgb(136, 34, 17);
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.FromArgb(112, 205, 225);
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.Transparent;
        }

        private void pb_min_MouseEnter(object sender, EventArgs e)
        {
            pb_min.BackColor = Color.FromArgb(112, 205, 225);
        }

        private void pb_min_MouseLeave(object sender, EventArgs e)
        {
            pb_min.BackColor = Color.Transparent;
        }

        private void MainForm_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                pictureBox2.Image = Properties.Resources.s1;
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                pictureBox2.Image = Properties.Resources.s;
                this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) 
            { 
                 this.Visible = false;
                 this.miniIcon.Visible = true;
            }
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.Visible = true;
                this.miniIcon.Visible = false;
            }
        }

        private void miniIcon_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = true;
                this.ShowInTaskbar = true;  //显示在系统任务栏 
                this.WindowState = FormWindowState.Normal;  //还原窗体 
                this.miniIcon.Visible = false;  //托盘图标隐藏 
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mousePoint.X = e.X;
                this.mousePoint.Y = e.Y;
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Top = Control.MousePosition.Y - this.mousePoint.Y;
                this.Left = Control.MousePosition.X - this.mousePoint.X;
            }
        }

        public void uC_Menu10_UC_Click(object sender, EventArgs e)
        {
            mingan = new Forms.Mingan();
            mingan.TopLevel = false;
            mingan.Dock = DockStyle.Fill;
            panel_main.Controls.RemoveAt(0);
            panel_main.Controls.Add(mingan);
            um.BackgroundImage = Properties.Resources.a;
            um.FontColor = Color.FromArgb(204, 204, 204);
            uC_Menu10.BackgroundImage = Properties.Resources.a1;
            uC_Menu10.FontColor = Color.White;
            um = uC_Menu10;
            mingan.Tag = sender;
            mingan.Show();
        }

        public void uC_Menu11_UC_Click(object sender, EventArgs e)
        {
            zd = new Forms.Zhongdian();
            zd.TopLevel = false;
            zd.Dock = DockStyle.Fill;
            panel_main.Controls.RemoveAt(0);
            panel_main.Controls.Add(zd);
            um.BackgroundImage = Properties.Resources.a;
            um.FontColor = Color.FromArgb(204, 204, 204);
            uC_Menu11.BackgroundImage = Properties.Resources.a1;
            uC_Menu11.FontColor = Color.White;
            um = uC_Menu11;
            zd.Tag = sender;
            zd.Show();
        }

        public void uC_Menu12_UC_Click(object sender, EventArgs e)
        {
            tufa = new Forms.Tufa();
            tufa.TopLevel = false;
            tufa.Dock = DockStyle.Fill;
            panel_main.Controls.RemoveAt(0);
            panel_main.Controls.Add(tufa);
            um.BackgroundImage = Properties.Resources.a;
            um.FontColor = Color.FromArgb(204, 204, 204);
            uC_Menu12.BackgroundImage = Properties.Resources.a1;
            uC_Menu12.FontColor = Color.White;
            um = uC_Menu12;
            tufa.Tag = sender;
            tufa.Show();
        }

        public void uC_Menu13_UC_Click(object sender, EventArgs e)
        {
            panel_main.Controls.RemoveAt(0);
            panel_main.Controls.Add(kz);
            um.BackgroundImage = Properties.Resources.a;
            um.FontColor = Color.FromArgb(204, 204, 204);
            uC_Menu13.BackgroundImage = Properties.Resources.a1;
            uC_Menu13.FontColor = Color.White;
            um = uC_Menu13;
            kz.Show();
        }

        public void uC_Menu6_UC_Click(object sender, EventArgs e)
        {
            panel_main.Controls.RemoveAt(0);
            panel_main.Controls.Add(fd);
            um.BackgroundImage = Properties.Resources.a;
            um.FontColor = Color.FromArgb(204, 204, 204);
            uC_Menu6.BackgroundImage = Properties.Resources.a1;
            uC_Menu6.FontColor = Color.White;
            um = uC_Menu6;
            fd.Show();
        }

        public void uC_Menu2_UC_Click(object sender, EventArgs e)
        {
            panel_main.Controls.RemoveAt(0);
            panel_main.Controls.Add(wp);
            um.BackgroundImage = Properties.Resources.a;
            um.FontColor = Color.FromArgb(204, 204, 204);
            uC_Menu2.BackgroundImage = Properties.Resources.a1;
            uC_Menu2.FontColor = Color.White;
            um = uC_Menu2;
            wp.Show();
        }
        private void uC_Menu9_UC_Click(object sender, EventArgs e)
        {
            panel_main.Controls.RemoveAt(0);
            panel_main.Controls.Add(home);
            um.BackgroundImage = Properties.Resources.a;
            um.FontColor = Color.FromArgb(204, 204, 204);
            uC_Menu9.BackgroundImage = Properties.Resources.a1;
            uC_Menu9.FontColor = Color.White;
            um = uC_Menu9;

            if (Comm.isChangedEvents)
            {
                home.readData();
                Comm.isChangedEvents = false;
            }

            home.Show();
        }
        public void uC_Menu8_UC_Click(object sender, EventArgs e)
        {
            panel_main.Controls.RemoveAt(0);
            panel_main.Controls.Add(dc);
            um.BackgroundImage = Properties.Resources.a;
            um.FontColor = Color.FromArgb(204, 204, 204);
            uC_Menu8.BackgroundImage = Properties.Resources.a1;
            uC_Menu8.FontColor = Color.White;
            um = uC_Menu8;
            dc.Show();
        }
        public void uC_Menu1_UC_Click(object sender, EventArgs e)
        {
            panel_main.Controls.RemoveAt(0);
            panel_main.Controls.Add(monit);
            um.BackgroundImage = Properties.Resources.a;
            um.FontColor = Color.FromArgb(204, 204, 204);
            uC_Menu1.BackgroundImage = Properties.Resources.a1;
            uC_Menu1.FontColor = Color.White;
            um = uC_Menu1;
            monit.Show();
        }
        public void uC_Menu3_UC_Click_1(object sender, EventArgs e)
        {
            cg = new Forms.Changgui();
            cg.TopLevel = false;
            cg.Dock = DockStyle.Fill;
            panel_main.Controls.RemoveAt(0);
            panel_main.Controls.Add(cg);
            um.BackgroundImage = Properties.Resources.a;
            um.FontColor = Color.FromArgb(204, 204, 204);
            uC_Menu3.BackgroundImage = Properties.Resources.a1;
            uC_Menu3.FontColor = Color.White;
            um = uC_Menu3;
            cg.Tag = sender;       
            cg.Show();
        }
        private void llb_website_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Comm.VisiteUrl("http://www.shangwukaocha.cn/finder/");
        }
        private void llb_contactus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Comm.VisiteUrl("http://wpa.qq.com/msgrd?v=3&uin=2979798279&site=qq&menu=yes");
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            //if (panel_main.Controls.Count > 0)
            //{
            //    Form currentF = (Form)panel_main.Controls[0];
            //    if (currentF.GetType() == monit.GetType())
            //    {
            //        monit.Report();
            //    }
            //}
            //else
            //{
            //    panel_main.Controls.Add(monit);
            //    monit.Show();
            //}
        }


        private void panel_main_ControlAdded(object sender, ControlEventArgs e)
        {
            //if (panel_main.Controls.Count > 0)
            //{
            //    Form currentF = (Form)panel_main.Controls[0];
            //    if (currentF.GetType() == monit.GetType())
            //    {
            //        monit.Report();
            //    }
            //}
            //else
            //{
            //    panel_main.Controls.Add(monit);
            //    monit.Show();
            //}
        }


        #endregion

        #region 窗体可拖拽
        private bool m_isMouseDown = false;
        private Point m_mousePos = new Point();
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            m_mousePos = Cursor.Position;
            m_isMouseDown = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            m_isMouseDown = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (m_isMouseDown)
            {
                Point tempPos = Cursor.Position;
                this.Location = new Point(Location.X + (tempPos.X - m_mousePos.X), Location.Y + (tempPos.Y - m_mousePos.Y));
                m_mousePos = Cursor.Position;
            }
        }
        #endregion

    }
}