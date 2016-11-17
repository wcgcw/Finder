using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataBaseServer;
using Finder.Entites;
using Finder.util;
using System.Threading;

namespace Finder
{
    public partial class Login : Form
    {
        XmlUtil xu = new XmlUtil();
        public Login()
        {
            InitializeComponent();
            lb_mc.Text = Comm.GetMachineCode();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            string username = tb_username.Text.Trim();
            string password = tb_password.Text.Trim();
            string uid = "0";
            if (username.Length <= 0 || password.Length <= 0)
            {
                MessageBox.Show("请填写用户名和密码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = "SELECT * FROM LoginUser WHERE UName='{0}'";
            sql = string.Format(sql, username);

            try
            {
                DataBaseServer.MySqlCmd dbobj = new MySqlCmd();
                DataTable one = dbobj.GetTabel(sql);
                if (one.Rows.Count <= 0)
                {
                    MessageBox.Show("您输入的用户名和密码不正确！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                object pwd = one.Rows[0]["PWord"];
                if (!(pwd != null) || (password != pwd.ToString()))
                {
                    MessageBox.Show("您输入的用户名和密码不正确！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                uid = one.Rows[0]["uid"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("出现异常，请稍后重试或联系软件提供商！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            UserInfo ui = new UserInfo();
            ui.UName = username;
            ui.Pword = password;
            ui.Uid = uid;
            //将用户信息放入到缓存中。
            GlobalPars.GloPars.Add("UserInfo", ui);
            this.Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            tb_username.Select();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            Thread regT = new Thread(new ParameterizedThreadStart(register));
            regT.IsBackground = true;
            regT.Start(lb_mc.Text + "-" + tb_sn.Text+"-1");
        }
        private void register(object obj)
        {
            string[] regInfo = obj.ToString().Split('-');
            string result = Comm.register(regInfo[0], regInfo[1]);
            this.BeginInvoke(new MethodInvoker(delegate() {
                if (!string.IsNullOrEmpty(result) && result.Length == 16)
                {
                    xu.SetValue("rr", result.ToUpper());
                    tb_sn.Enabled = false;
                    btn_save.Enabled = false;
                    lb_regstate.Text = "已注册";
                }
                else
                {
                    lb_regstate.Text = regInfo.Length > 2 && regInfo[2].Equals("1") ? "注册失败,请检查序列号是否正确" : "未注册";
                }
            }));
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tc = (TabControl)sender;
            if (tc.SelectedIndex == 2)
            {
                string rr = xu.GetValue("rr");
                tb_sn.Text = rr;
                Thread regT = new Thread(new ParameterizedThreadStart(register));
                regT.IsBackground = true;
                regT.Start(lb_mc.Text + "-" + tb_sn.Text);
            }
        }
    }
}
