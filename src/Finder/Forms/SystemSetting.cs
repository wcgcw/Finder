using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using Finder.util;
using DataBaseServer;
using System.Data.SQLite;
using Finder.Entites;
using Finder.Entities;
using System.Collections;
using System.IO;

namespace Finder.Forms
{
    public partial class SystemSetting : Form
    {
        SQLitecommand cmd = new SQLitecommand();
        private SoundPlayer sPlayer = new SoundPlayer();

        public SystemSetting()
        {
            InitializeComponent();
            IniDgv1();
            IniDgv2();
        }

        private void SystemSetting_Load(object sender, EventArgs e)
        {
            label18.ForeColor = Color.Red;
            label2.ForeColor = Color.LightBlue;

            //填充数据
            SystemSet ss = null;
            if (GlobalPars.GloPars.ContainsKey("systemset"))
            {
                ss = (SystemSet)GlobalPars.GloPars["systemset"];
            }
            else
            {
                string sql = "select Id,EvidenceImgSavePath from systemset";
                DataTable dt = cmd.GetTabel(sql);
                ss = new SystemSet();
                ss.Id = dt.Rows[0]["Id"].ToString();
                ss.EvidenceImgSavePath = dt.Rows[0]["EvidenceImgSavePath"].ToString();
                GlobalPars.GloPars.Add("systemset", ss);
            }
            tb_choosedimgpath.Text = ss.EvidenceImgSavePath;

        }

        private void btn_updatepwd_Click(object sender, EventArgs e)
        {
            UserInfo ui = (UserInfo)GlobalPars.GloPars["UserInfo"];
            if (ui.Pword != tb_oldpwd.Text)
            {
                MessageBox.Show("您输入的旧密码不正确！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string newpwd1 = tb_newpwd1.Text;
            string newpwd2 = tb_newpwd2.Text;
            if (!newpwd1.Equals(newpwd2))
            {
                MessageBox.Show("您两次输入的密码不一致！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string sql = "UPDATE LoginUser SET PWord=@PWord WHERE uid=@Uid";
            List<SQLiteParameter> pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("@PWord", newpwd1));
            pars.Add(new SQLiteParameter("@Uid", ui.Uid));
            try
            {
                DataBaseServer.SQLitecommand dbobj = new DataBaseServer.SQLitecommand();
                if (dbobj.ExecuteNonQueryInt(sql, pars) > 0)
                {
                    MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ui.Pword = newpwd1;
                }
                else
                {
                    MessageBox.Show("修改失败，请稍后重试或联系软件提供商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("修改失败，请稍后重试或联系软件提供商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_weibo_qq_auth_Click(object sender, EventArgs e)
        {
            QQWeibo qqwb = new QQWeibo();
            qqwb.ShowDialog();
        }

        private void btn_weibo_sina_auth_Click(object sender, EventArgs e)
        {
            SinaWBOauth sinawb = new SinaWBOauth();
            sinawb.ShowDialog();
        }

        private void pb_choosepath_Click(object sender, EventArgs e)
        {
            string selectedPath = null;
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedPath = folderBrowserDialog1.SelectedPath;
                tb_choosedimgpath.Text = selectedPath;
            }
        }

        private void btn_saveimgpath_Click(object sender, EventArgs e)
        {
            if (tb_choosedimgpath.Text == null || tb_choosedimgpath.Text.Trim().Length == 0)
            {
                MessageBox.Show("请先选择证据图片保存路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string sql = "UPDATE systemset SET EvidenceImgSavePath=@EvidenceImgSavePath WHERE id=1";
            List<SQLiteParameter> pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("@EvidenceImgSavePath", tb_choosedimgpath.Text));
            try
            {
                DataBaseServer.SQLitecommand dbobj = new DataBaseServer.SQLitecommand();
                if (dbobj.ExecuteNonQueryInt(sql, pars) > 0)
                {
                    SystemSet ss = (SystemSet)GlobalPars.GloPars["systemset"];
                    ss.EvidenceImgSavePath = tb_choosedimgpath.Text;
                    MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("修改失败，请稍后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("出现异常，请稍后重试或联系软件提供商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lbsmsCX_Click(object sender, EventArgs e)
        {
            AlertDetail ad = new AlertDetail("sms");
            ad.ShowDialog();
        }

        private void lbsoundCX_Click(object sender, EventArgs e)
        {
            AlertDetail ad = new AlertDetail("sound");
            ad.ShowDialog();
        }

        private void btn_query_smscount_Click(object sender, EventArgs e)
        {
            string userID = "";
            try
            {
                userID = (string)GlobalPars.GloPars["userID"];
            }
            catch (Exception) { }
            string url = "http://www.shangwukaocha.cn/finder/users.php?userID=" + userID + "&m=q";
            string _r = Alert.postSend(url, "");
            if (_r.Contains(','))
            {
                lb_show_shengyusmscount.Text = "剩余短信数为：" + _r.Split(',')[0];
                lb_show_shengyusmscount.ForeColor = Color.Blue;
            }
        }

        private void label18_Click(object sender, EventArgs e)
        {
            string msg = "1.短信内容需小于70个字。\r\n"
                       + "2.短信内容的输入模板为：系统已经发现________________舆情信息，请您及时关注！【正义东方】\r\n"
                       + "3.因电信运营商原因，用户手机实际收到的短信，事件名称可能会出现：[事*件*1],[事-件-1]等形式。\r\n"
                       + "4.选中单元格进行数据编辑。";
            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void IniDgv1(){
            DataGridViewTextBoxCell txt_cell = new DataGridViewTextBoxCell();
            DataGridViewButtonCell btn_cell = new DataGridViewButtonCell();

            DataGridViewColumn cl_gjz = new DataGridViewColumn();
            cl_gjz.HeaderText = "事件";
            cl_gjz.CellTemplate = txt_cell;
            cl_gjz.ReadOnly = true;
            cl_gjz.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv1.Columns.Add(cl_gjz);

            DataGridViewColumn cl_jgsj = new DataGridViewColumn();
            cl_jgsj.HeaderText = "间隔时间(小时)";
            cl_jgsj.CellTemplate = txt_cell;
            cl_jgsj.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv1.Columns.Add(cl_jgsj);

            DataGridViewColumn cl_cs = new DataGridViewColumn();
            cl_cs.HeaderText = "出现次数";
            cl_cs.CellTemplate = txt_cell;
            cl_cs.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv1.Columns.Add(cl_cs);

            DataGridViewColumn cl_warncontent = new DataGridViewColumn();
            cl_warncontent.HeaderText = "短信内容";
            cl_warncontent.CellTemplate = txt_cell;
            cl_warncontent.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv1.Columns.Add(cl_warncontent);

            DataGridViewColumn cl_mobile = new DataGridViewColumn();
            cl_mobile.HeaderText = "电话号码";
            cl_mobile.CellTemplate = txt_cell;
            cl_mobile.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv1.Columns.Add(cl_mobile);

            DataGridViewColumn cl_bj = new DataGridViewColumn();
            cl_bj.HeaderText = "";
            cl_bj.CellTemplate = btn_cell;
            cl_bj.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv1.Columns.Add(cl_bj);

            DataGridViewColumn cl_cz = new DataGridViewColumn();
            cl_cz.HeaderText = "";
            cl_cz.CellTemplate = btn_cell;
            cl_cz.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv1.Columns.Add(cl_cz);

            //dgv1.CellContentClick += new DataGridViewCellEventHandler(dgv1_CellContentClick);
        }

        private void IniDgv2()
        {
            dgv2.Columns.Clear();
            DataGridViewTextBoxCell txt_cell = new DataGridViewTextBoxCell();
            DataGridViewButtonCell btn_cell = new DataGridViewButtonCell();
            DataGridViewComboBoxCell cbo_cell = new DataGridViewComboBoxCell();
            DataGridViewImageCell img_cell = new DataGridViewImageCell();
            img_cell.ImageLayout = DataGridViewImageCellLayout.Zoom;

            DataGridViewColumn cl_gjz = new DataGridViewColumn();
            cl_gjz.HeaderText = "事件";
            cl_gjz.CellTemplate = txt_cell;
            cl_gjz.ReadOnly = true;
            cl_gjz.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv2.Columns.Add(cl_gjz);

            DataGridViewColumn cl_jgsj = new DataGridViewColumn();
            cl_jgsj.HeaderText = "间隔时间(小时)";
            cl_jgsj.CellTemplate = txt_cell;
            cl_jgsj.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv2.Columns.Add(cl_jgsj);

            DataGridViewColumn cl_cs = new DataGridViewColumn();
            cl_cs.HeaderText = "出现次数";
            cl_cs.CellTemplate = txt_cell;
            cl_cs.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv2.Columns.Add(cl_cs);

            DataGridViewComboBoxColumn cl_warncontent = new DataGridViewComboBoxColumn();
            cl_warncontent.HeaderText = "选择声音";
            cl_warncontent.CellTemplate = cbo_cell;
            cl_warncontent.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv2.Columns.Add(cl_warncontent);

            DataGridViewColumn cl_playSound = new DataGridViewColumn();
            cl_playSound.HeaderText = "试听";
            cl_playSound.CellTemplate = img_cell;
            cl_playSound.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv2.Columns.Add(cl_playSound);

            DataGridViewColumn cl_bj1 = new DataGridViewColumn();
            cl_bj1.HeaderText = "";
            cl_bj1.CellTemplate = btn_cell;
            cl_bj1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv2.Columns.Add(cl_bj1);

            DataGridViewColumn cl_cz1 = new DataGridViewColumn();
            cl_cz1.HeaderText = "";
            cl_cz1.CellTemplate = btn_cell;
            cl_cz1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv2.Columns.Add(cl_cz1);

            //dgv2.CellContentClick += new DataGridViewCellEventHandler(dgv2_CellContentClick);
        }

        private void IniDgv1Data()
        {
            DataTable dt_sms = null;
            string sql_sms = "select t1.name,t2.id id,t2.IntervalHours hours," +
                             "t2.IntervalHoursTotalInfo info_ct,t2.Keyword keyword_2," +
                             "t2.WarnContent content,t2.Mobile mobile " +
                             "from (select name,MessageAlarm from KeyWords where MessageAlarm=1 group by name) t1 " +
                             "left JOIN (select * from warn where WarnWay=1) t2 on t1.name=t2.Keyword " +
                             "where t1.MessageAlarm=1 order by hours asc";
            dt_sms = cmd.GetTabel(sql_sms);

            string id, kw, IntervalHours, IntervalHoursTotalInfo, WarnContent, Mobile;
            int index = 0;
            dgv1.Rows.Clear();
            for (int i = 0; i < dt_sms.Rows.Count; i++)
            {
                id = dt_sms.Rows[i]["id"].ToString().Equals("") ? "0" : dt_sms.Rows[i]["id"].ToString();
                kw = dt_sms.Rows[i]["name"].ToString().Equals("") ? "" : dt_sms.Rows[i]["name"].ToString();
                IntervalHours = dt_sms.Rows[i]["hours"].ToString().Equals("") ? "" : dt_sms.Rows[i]["hours"].ToString();
                IntervalHoursTotalInfo = dt_sms.Rows[i]["info_ct"].ToString().Equals("") ? "" : dt_sms.Rows[i]["info_ct"].ToString();
                WarnContent = dt_sms.Rows[i]["content"].ToString().Equals("") ? "" : dt_sms.Rows[i]["content"].ToString();
                Mobile = dt_sms.Rows[i]["mobile"].ToString().Equals("") ? "" : dt_sms.Rows[i]["mobile"].ToString();

                index = dgv1.Rows.Add();

                dgv1.Rows[index].Cells[0].Value = kw;
                dgv1.Rows[index].Cells[1].Value = IntervalHours;
                dgv1.Rows[index].Cells[2].Value = IntervalHoursTotalInfo;
                dgv1.Rows[index].Cells[3].Value = WarnContent;
                dgv1.Rows[index].Cells[4].Value = Mobile;
                dgv1.Rows[index].Cells[5].Value = "保存";
                dgv1.Rows[index].Cells[5].Tag = id;
                dgv1.Rows[index].Cells[6].Value = "删除";
                dgv1.Rows[index].Cells[6].Tag = id;
                if (!id.Equals("0"))
                {
                    dgv1.Rows[index].DefaultCellStyle.BackColor = Color.LightBlue;
                }
                
            }
            dgv1.CellValueChanged += new DataGridViewCellEventHandler(dgv1_CellValueChanged);
            InitDataGridView(dgv1);
        }

        private void IniDgv2Data()
        {
            ArrayList list = new ArrayList();
            list.Add(".WAV");
            string[] sounds = GetFiles(@"warn_sound\", list);

            DataTable dt_snd = null;
            string sql_snd = "select t1.name,t2.id id,t2.IntervalHours hours," +
                             "t2.IntervalHoursTotalInfo info_ct,t2.Keyword keyword_2," +
                             "t2.WarnContent content,t2.Mobile mobile " +
                             "from (select name,MusicAlarm from KeyWords where MusicAlarm=1 group by name) t1 " +
                             "left JOIN (select * from warn where WarnWay=2) t2 on t1.name=t2.Keyword " +
                             "where t1.MusicAlarm=1 order by hours asc";
            dt_snd = cmd.GetTabel(sql_snd);

            string id, kw, IntervalHours, IntervalHoursTotalInfo, WarnContent;
            int index = 0;
            dgv2.Rows.Clear();
            for (int i = 0; i < dt_snd.Rows.Count; i++)
            {
                id = dt_snd.Rows[i]["id"].ToString().Equals("") ? "0" : dt_snd.Rows[i]["id"].ToString();
                kw = dt_snd.Rows[i]["name"].ToString().Equals("") ? "" : dt_snd.Rows[i]["name"].ToString();
                IntervalHours = dt_snd.Rows[i]["hours"].ToString().Equals("") ? "" : dt_snd.Rows[i]["hours"].ToString();
                IntervalHoursTotalInfo = dt_snd.Rows[i]["info_ct"].ToString().Equals("") ? "" : dt_snd.Rows[i]["info_ct"].ToString();
                WarnContent = dt_snd.Rows[i]["content"].ToString().Equals("") ? "" : dt_snd.Rows[i]["content"].ToString();

                index = dgv2.Rows.Add();
                dgv2.Rows[index].Cells[0].Value = kw;
                dgv2.Rows[index].Cells[1].Value = IntervalHours;
                dgv2.Rows[index].Cells[2].Value = IntervalHoursTotalInfo;
                DataGridViewComboBoxCell cell = dgv2.Rows[index].Cells[3] as DataGridViewComboBoxCell;
                if (cell != null)
                {
                    if (!WarnContent.Equals(""))
                    {
                        cell.Items.Add(WarnContent);
                        cell.Items.AddRange(sounds);
                    } else {
                        cell.Items.AddRange(sounds);
                    }
                }
                dgv2.Rows[index].Cells[3].Value = cell.Items[0].ToString();
                
                dgv2.Rows[index].Cells[4].Value = global::Finder.Properties.Resources.play;
                dgv2.Rows[index].Cells[5].Value = "保存";
                dgv2.Rows[index].Cells[5].Tag = id;
                dgv2.Rows[index].Cells[6].Value = "删除";
                dgv2.Rows[index].Cells[6].Tag = id;
                if (!id.Equals("0"))
                {
                    dgv2.Rows[index].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }
            dgv2.CellValueChanged += new DataGridViewCellEventHandler(dgv2_CellValueChanged);
            InitDataGridView(dgv2);
        }

        private void dgv1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex != -1 && e.ColumnIndex == 1)  //hours
            //{
            //    string hours = dgv1.Rows[e.RowIndex].Cells[1].Value == null ? "" : dgv1.Rows[e.RowIndex].Cells[1].Value.ToString();
            //    if (!Comm.isInteger(hours))
            //    {
            //        MessageBox.Show("输入的时间格式不正确,应为整数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        dgv1.Rows[e.RowIndex].Cells[1].Value = 0;
            //        return;
            //    }
            //}
            //if (e.RowIndex != -1 && e.ColumnIndex == 2)  //hours_info
            //{
            //    string count = dgv1.Rows[e.RowIndex].Cells[2].Value == null ? "" : dgv1.Rows[e.RowIndex].Cells[2].Value.ToString();
            //    if (!Comm.isInteger(count))
            //    {
            //        MessageBox.Show("输入的次数格式不正确,应为整数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        dgv1.Rows[e.RowIndex].Cells[2].Value = 0;
            //        return;
            //    }
            //}
            //if (e.RowIndex != -1 && e.ColumnIndex == 3)  //content
            //{
            //    string content = dgv1.Rows[e.RowIndex].Cells[3].Value == null ? "" : dgv1.Rows[e.RowIndex].Cells[3].Value.ToString();
            //    if (content.Trim().Length > 70)
            //    {
            //        MessageBox.Show("您输入的短信内容不正确。短信内容需小于70个字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        dgv1.Rows[e.RowIndex].Cells[3].Value = content.Substring(0, 64);
            //        return;
            //    }
            //}
            //if (e.RowIndex != -1 && e.ColumnIndex == 4)  //mobile
            //{
            //    string mobile = dgv1.Rows[e.RowIndex].Cells[4].Value == null ? "" : dgv1.Rows[e.RowIndex].Cells[4].Value.ToString();
            //    if (!Comm.isMobile(mobile))
            //    {
            //        MessageBox.Show("输入的手机格式不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        dgv1.Rows[e.RowIndex].Cells[4].Value = "13988888888";
            //        return;
            //    }
            //}
        }

        private void dgv2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex != -1 && e.ColumnIndex == 1)  //hours
            //{
            //    string hours = dgv2.Rows[e.RowIndex].Cells[1].Value == null ? "" : dgv2.Rows[e.RowIndex].Cells[1].Value.ToString();
            //    if (!Comm.isInteger(hours))
            //    {
            //        MessageBox.Show("输入的时间格式不正确,应为整数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        dgv2.Rows[e.RowIndex].Cells[1].Value = 0;
            //        return;
            //    }
            //}
            //if (e.RowIndex != -1 && e.ColumnIndex == 2)  //hours_info
            //{
            //    string count = dgv2.Rows[e.RowIndex].Cells[2].Value == null ? "" : dgv2.Rows[e.RowIndex].Cells[2].Value.ToString();
            //    if (!Comm.isInteger(count))
            //    {
            //        MessageBox.Show("输入的次数格式不正确,应为整数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        dgv2.Rows[e.RowIndex].Cells[2].Value = 0;
            //        return;
            //    }
            //}
            //if (e.RowIndex != -1 && e.ColumnIndex == 3)  //content
            //{
            //    string content = dgv2.Rows[e.RowIndex].Cells[3].Value == null ? "" : dgv2.Rows[e.RowIndex].Cells[3].Value.ToString();
            //    if (content.Trim().Length > 70)
            //    {
            //        MessageBox.Show("您设置的声音报警内容不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //}
        }

        public static void InitDataGridView(DataGridView dgv)
        {
            //只读属性设置 
            //dgv.ReadOnly = true;
            //尾行自动追加 
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            //行幅自动变化               
            dgv.AllowUserToResizeRows = true;
            //高度设定               
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //标头设定                 
            dgv.RowHeadersVisible = true;
            //标题行高 
            dgv.ColumnHeadersHeight = 25;
            dgv.RowTemplate.Height = 23;
            //行选择设定 
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //多行选择 
            dgv.MultiSelect = false;
            //选择状态解除 
            dgv.ClearSelection();
            //head文字居中       
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //选择状态的行的颜色 
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            //设定交替行颜色 
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dgv.RowsDefaultCellStyle.BackColor = Color.LightGray;
            //行副填充时自动调整宽度 
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgv.AutoGenerateColumns = false;
            //可否手动调整行大小 
            dgv.AllowUserToResizeRows = false;
            dgv.AutoGenerateColumns = false;
            dgv.ScrollBars = ScrollBars.Both;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        }

        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == 5)  //保存
            {
                string sql;
                string kw;
                string hours;
                string hours_info;
                string content;
                string mobile;
                int id=0;
                try
                {
                    kw = dgv1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    hours = dgv1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    hours_info = dgv1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    content = dgv1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    mobile = dgv1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    id = int.Parse(dgv1.Rows[e.RowIndex].Cells[5].Tag.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("请确认您所有的数据都填写完整！","提示",MessageBoxButtons.OK);
                    return;
                }
                if (!Comm.isInteger(hours))
                {
                    MessageBox.Show("输入的时间格式不正确,应为整数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgv1.Rows[e.RowIndex].Cells[1].Value = 0;
                    return;
                }
                if (!Comm.isInteger(hours_info))
                {
                    MessageBox.Show("输入的次数格式不正确,应为整数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgv1.Rows[e.RowIndex].Cells[2].Value = 0;
                    return;
                }
                if (content.Trim().Length > 70)
                {
                    MessageBox.Show("您输入的短信内容不正确。短信内容需小于70个字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgv1.Rows[e.RowIndex].Cells[3].Value = content.Substring(0, 64);
                    return;
                }
                if (!Comm.isMobile(mobile))
                {
                    MessageBox.Show("输入的手机格式不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgv1.Rows[e.RowIndex].Cells[4].Value = "13988888888";
                    return;
                }
                if (!id.Equals(0))
                {
                    sql = "UPDATE Warn SET IntervalHours=@IntervalHours,IntervalHoursTotalInfo=@IntervalHoursTotalInfo,"
                        + "keyword=@keyword,WarnWay=@WarnWay,WarnContent=@WarnContent,Mobile=@Mobile WHERE ID=" + id;

                    List<SQLiteParameter> pars = new List<SQLiteParameter>();
                    pars.Add(new SQLiteParameter("@IntervalHours", hours));
                    pars.Add(new SQLiteParameter("@IntervalHoursTotalInfo", hours_info));
                    pars.Add(new SQLiteParameter("@keyword", kw));
                    pars.Add(new SQLiteParameter("@WarnWay", 1));
                    pars.Add(new SQLiteParameter("@WarnContent", content));
                    pars.Add(new SQLiteParameter("@Mobile", mobile));

                    try
                    {
                        if (cmd.ExecuteNonQueryInt(sql, pars) > 0)
                        {
                            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgv1.CellValueChanged -= new DataGridViewCellEventHandler(dgv1_CellValueChanged);
                            dgv1.Rows.Clear();
                            IniDgv1Data();
                        }
                        else
                        {
                            MessageBox.Show("保存失败，请稍后重试或联系软件提供商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("保存失败，请稍后重试或联系软件提供商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    if (!mobile.Equals(""))
                    {
                        sql = "insert into warn (IntervalHours,IntervalHoursTotalInfo,Keyword,WarnWay,WarnContent,Mobile) "
                            + "values ('" + hours + "','" + hours_info + "','" + kw + "',1,'" + content + "','" + mobile + "')";

                        try
                        {
                            if (cmd.ExecuteNonQueryInt(sql) > 0)
                            {
                                MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dgv1.CellValueChanged -= new DataGridViewCellEventHandler(dgv1_CellValueChanged);
                                dgv1.Rows.Clear();
                                IniDgv1Data();
                            }
                            else
                            {
                                MessageBox.Show("保存失败，请稍后重试或联系软件提供商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("保存失败，请稍后重试或联系软件提供商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }


            }
            if (e.RowIndex != -1 && e.ColumnIndex == 6)  //删除
            {
                if (!dgv1.Rows[e.RowIndex].Cells[6].Tag.ToString().Equals("0"))
                {
                    string sql = "delete from warn where id =" + int.Parse(dgv1.Rows[e.RowIndex].Cells[6].Tag.ToString());
                    try
                    {
                        if (cmd.ExecuteNonQueryInt(sql) > 0)
                        {
                            MessageBox.Show("删除成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgv1.CellValueChanged -= new DataGridViewCellEventHandler(dgv1_CellValueChanged);
                            dgv1.Rows.Clear();
                            IniDgv1Data();
                        }
                        else
                        {
                            MessageBox.Show("删除失败，请稍后重试或联系软件提供商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("删除失败，请稍后重试或联系软件提供商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void dgv2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == 4)  //试听
            {
                if (null != dgv2.Rows[e.RowIndex].Cells[3].Value)
                {
                    sPlayer.SoundLocation = @"warn_sound\" + dgv2.Rows[e.RowIndex].Cells[3].Value.ToString();
                    sPlayer.Load();
                    sPlayer.Play();
                }
            }
            if (e.RowIndex != -1 && e.ColumnIndex == 5)  //保存
            {
                string sql = "";
                string hours;
                string hours_info;
                string kw;
                string content;
                int id=0;
                try
                {
                    if (null == dgv2.Rows[e.RowIndex].Cells[3].Value) return;
                    hours = dgv2.Rows[e.RowIndex].Cells[1].Value.ToString();
                    hours_info = dgv2.Rows[e.RowIndex].Cells[2].Value.ToString();
                    kw = dgv2.Rows[e.RowIndex].Cells[0].Value.ToString();
                    content = dgv2.Rows[e.RowIndex].Cells[3].Value.ToString();
                    id = int.Parse(dgv2.Rows[e.RowIndex].Cells[5].Tag.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("请确认您所有的数据都填写完整！", "提示", MessageBoxButtons.OK);
                    return;
                }

                if (!id.Equals(0))
                {
                    if (!Comm.isInteger(hours))
                    {
                        MessageBox.Show("输入的时间格式不正确,应为整数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgv2.Rows[e.RowIndex].Cells[1].Value = 0;
                        return;
                    }
                    if (!Comm.isInteger(hours_info))
                    {
                        MessageBox.Show("输入的次数格式不正确,应为整数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgv2.Rows[e.RowIndex].Cells[2].Value = 0;
                        return;
                    }
                    sql = "UPDATE Warn SET IntervalHours=@IntervalHours,IntervalHoursTotalInfo=@IntervalHoursTotalInfo,"
                        + "keyword=@keyword,WarnWay=@WarnWay,WarnContent=@WarnContent,Mobile=@Mobile WHERE ID=" + id;

                    List<SQLiteParameter> pars = new List<SQLiteParameter>();
                    pars.Add(new SQLiteParameter("@IntervalHours", hours));
                    pars.Add(new SQLiteParameter("@IntervalHoursTotalInfo", hours_info));
                    pars.Add(new SQLiteParameter("@keyword", kw));
                    pars.Add(new SQLiteParameter("@WarnWay", 2));
                    pars.Add(new SQLiteParameter("@WarnContent", content));
                    pars.Add(new SQLiteParameter("@Mobile", ""));

                    try
                    {
                        if (cmd.ExecuteNonQueryInt(sql, pars) > 0)
                        {
                            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgv2.CellValueChanged -= new DataGridViewCellEventHandler(dgv2_CellValueChanged);
                            dgv2.Rows.Clear();
                            IniDgv2();
                            IniDgv2Data();
                        }
                        else
                        {
                            MessageBox.Show("保存失败，请稍后重试或联系软件提供商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("保存失败，请稍后重试或联系软件提供商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    if (!content.Equals("") && !hours.Equals(0) && !hours_info.Equals(0))
                    {
                        sql = "insert into warn (IntervalHours,IntervalHoursTotalInfo,Keyword,WarnWay,WarnContent,Mobile) "
                            + "values (" + hours + "," + hours_info + ",'" + kw + "',2,'" + content + "','')";

                        try
                        {
                            if (cmd.ExecuteNonQueryInt(sql) > 0)
                            {
                                MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dgv2.CellValueChanged -= new DataGridViewCellEventHandler(dgv2_CellValueChanged);
                                dgv2.Rows.Clear();
                                IniDgv2();
                                IniDgv2Data();
                            }
                            else
                            {
                                MessageBox.Show("保存失败，请稍后重试或联系软件提供商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("保存失败，请稍后重试或联系软件提供商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }


            }
            if (e.RowIndex != -1 && e.ColumnIndex == 6)  //删除
            {
                if (!dgv2.Rows[e.RowIndex].Cells[6].Tag.ToString().Equals("0"))
                {
                    string sql = "delete from warn where id =" + int.Parse(dgv2.Rows[e.RowIndex].Cells[6].Tag.ToString());
                    try
                    {
                        if (cmd.ExecuteNonQueryInt(sql) > 0)
                        {
                            MessageBox.Show("删除成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgv2.CellValueChanged -= new DataGridViewCellEventHandler(dgv2_CellValueChanged);
                            dgv2.Rows.Clear();
                            IniDgv2Data();
                        }
                        else
                        {
                            MessageBox.Show("删除失败，请稍后重试或联系软件提供商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("删除失败，请稍后重试或联系软件提供商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        public string[] GetFiles(string strPath, ArrayList lstExtend)
        {
            List<string> listArr = new List<string>();
            try
            {
                //获取文件夹下的所有文件
                DirectoryInfo fdir = new DirectoryInfo(strPath);
                FileInfo[] file = fdir.GetFiles();

                //遍历该文件夹下的所有文件
                foreach (FileInfo f in file)
                {
                    //如果文件的扩展名包含于该ArrayList内
                    if (lstExtend.Contains(f.Extension.ToUpper()))
                    {
                        listArr.Add(f.Name.ToString());
                    }
                }
                return listArr.ToArray();
            }
            catch (Exception e)
            {
                return listArr.ToArray();
                MessageBox.Show(e.StackTrace);
            }
        }

        private void SystemSetting_Paint(object sender, PaintEventArgs e)
        {
            IniDgv1Data();
            IniDgv2Data();
        }
    }
}
