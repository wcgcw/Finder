using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Finder.Forms
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        DataBaseServer.SQLitecommand cmd = new DataBaseServer.SQLitecommand();
        private void linkLableClick(object sender, LinkLabelLinkClickedEventArgs args)
        {
            if (sender is LinkLabel)
            {
                LinkLabel ll = (LinkLabel)sender;
                MainForm mf = (MainForm)this.Parent.Parent.Parent;
                switch (ll.Tag.ToString())
                {
                    case "0":
                        mf.uC_Menu3_UC_Click_1(ll.Text, args);
                        break;
                    case "1":
                        mf.uC_Menu10_UC_Click(ll.Text, args);
                        break;
                    case "2":
                        mf.uC_Menu11_UC_Click(ll.Text, args);
                        break;
                    case "3":
                        mf.uC_Menu12_UC_Click(ll.Text, args);
                        break;
                    default:
                        break;
                }
            }
        }
        private void readData()
        {
            string keywords_k0 = "select Name from keywords where kid=0 group by name limit 10";
            string keywords_k1 = "select Name from keywords where kid=1 group by name limit 10";
            string keywords_k2 = "select Name from keywords where kid=2 group by name limit 10";
            string keywords_k3 = "select Name from keywords where kid=3 group by name limit 10";

            panel1.Controls.Clear();

            Label l_changgui = new Label();
            l_changgui.Text = "常规舆情";
            l_changgui.AutoSize = true;
            l_changgui.Font = new System.Drawing.Font(new FontFamily("宋体"), 10, FontStyle.Bold);
            l_changgui.ForeColor = Color.DimGray;
            l_changgui.Location = new Point(197, 276);
            l_changgui.BackColor = Color.Transparent;
            l_changgui.Tag = 9999;
            panel1.Controls.Add(l_changgui);

            Label l_mingan = new Label();
            l_mingan.Text = "敏感舆情";
            l_mingan.Font = new System.Drawing.Font(new FontFamily("宋体"), 10, FontStyle.Bold);
            l_mingan.ForeColor = Color.DimGray;
            l_mingan.Location = new Point(381, 276);
            l_mingan.BackColor = Color.Transparent;
            l_mingan.Tag = 9999;
            panel1.Controls.Add(l_mingan);

            Label l_zhongdian = new Label();
            l_zhongdian.Text = "重点舆情";
            l_zhongdian.Font = new System.Drawing.Font(new FontFamily("宋体"), 10, FontStyle.Bold);
            l_zhongdian.ForeColor = Color.DimGray;
            l_zhongdian.Location = new Point(571, 276);
            l_zhongdian.BackColor = Color.Transparent;
            l_zhongdian.Tag = 9999;
            panel1.Controls.Add(l_zhongdian);

            Label l_tufa = new Label();
            l_tufa.Text = "突发舆情";
            l_tufa.Font = new System.Drawing.Font(new FontFamily("宋体"), 10, FontStyle.Bold);
            l_tufa.ForeColor = Color.DimGray;
            l_tufa.Location = new Point(774, 276);
            l_tufa.BackColor = Color.Transparent;
            l_tufa.Tag = 9999;
            panel1.Controls.Add(l_tufa);

            int x = 200, y = 310;
            DataTable dt_changgui = cmd.GetTabel(keywords_k0);
            foreach (DataRow dr in dt_changgui.Rows)
            {
                LinkLabel l = new LinkLabel();
                l.LinkBehavior = LinkBehavior.NeverUnderline;
                l.Text = dr[0].ToString();
                l.Location = new Point(x, y);
                l.AutoSize = true;
                l.Font = new System.Drawing.Font(new FontFamily("宋体"), 11);
                l.Tag = "0";
                l.LinkColor = Color.DimGray;
                l.BackColor = Color.Transparent;
                l.LinkClicked +=new LinkLabelLinkClickedEventHandler(linkLableClick);
                panel1.Controls.Add(l);
                y += 25;
            }

            x = 385; y = 310;
            DataTable dt_mingan = cmd.GetTabel(keywords_k1);
            foreach (DataRow dr in dt_mingan.Rows)
            {
                LinkLabel l = new LinkLabel();
                l.LinkBehavior = LinkBehavior.NeverUnderline;
                l.Text = dr[0].ToString();
                l.Font = new System.Drawing.Font(new FontFamily("宋体"), 11);
                l.Location = new Point(x, y);
                l.Tag = "1";
                l.AutoSize = true;
                l.LinkColor = Color.DimGray;
                l.BackColor = Color.Transparent;
                l.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLableClick);
                panel1.Controls.Add(l);
                y += 25;
            }

            x = 575; y = 310;
            DataTable dt_zhongdian = cmd.GetTabel(keywords_k2);
            foreach (DataRow dr in dt_zhongdian.Rows)
            {
                LinkLabel l = new LinkLabel();
                l.LinkBehavior = LinkBehavior.NeverUnderline;
                l.Text = dr[0].ToString();
                l.Font = new System.Drawing.Font(new FontFamily("宋体"), 11);
                l.Location = new Point(x, y);
                l.Tag = "2";
                l.AutoSize = true;
                l.LinkColor = Color.DimGray;
                l.BackColor = Color.Transparent;
                l.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLableClick);
                panel1.Controls.Add(l);
                y += 25;
            }

            x = 775; y = 310;
            DataTable dt_tufa = cmd.GetTabel(keywords_k3);
            foreach (DataRow dr in dt_tufa.Rows)
            {
                LinkLabel l = new LinkLabel();
                l.LinkBehavior = LinkBehavior.NeverUnderline;
                l.Text = dr[0].ToString();
                l.Font = new System.Drawing.Font(new FontFamily("宋体"), 11);
                l.Location = new Point(x, y);
                l.Tag = "3";
                l.AutoSize = true;
                l.LinkColor = Color.DimGray;
                l.BackColor = Color.Transparent;
                l.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLableClick);
                panel1.Controls.Add(l);
                y += 25;
            }
        }
        private void Home_Load(object sender, EventArgs e)
        {
            readData();
        }

        private void Home_MouseDown(object sender, MouseEventArgs e)
        {
            readData();
        }

        //private void Home_Resize(object sender, EventArgs e)
        //{
        //    panel1.Left = (this.Width - 216-panel1.Width) / 2;
        //    panel1.Top = (this.Height-panel1.Height) / 2;
        //}

        //private void pictureBox1_Click(object sender, EventArgs e)
        //{
        //    MainForm mf = (MainForm)this.Parent.Parent.Parent;
        //    mf.uC_Menu1_UC_Click(null, null);
        //}

        //private void pictureBox2_Click(object sender, EventArgs e)
        //{
        //    MainForm mf = (MainForm)this.Parent.Parent.Parent;
        //    mf.uC_Menu6_UC_Click(null, null);
        //}

        //private void pictureBox3_Click(object sender, EventArgs e)
        //{
        //    MainForm mf = (MainForm)this.Parent.Parent.Parent;
        //    mf.uC_Menu8_UC_Click(null, null);
        //}

        //private void pictureBox6_Click(object sender, EventArgs e)
        //{
        //    MainForm mf = (MainForm)this.Parent.Parent.Parent;
        //    mf.uC_Menu2_UC_Click(null, null);
        //}

        //private void pictureBox4_Click(object sender, EventArgs e)
        //{
        //    MainForm mf = (MainForm)this.Parent.Parent.Parent;
        //    mf.uC_Menu10_UC_Click(null, null);
        //}

        //private void pictureBox9_Click(object sender, EventArgs e)
        //{
        //    MainForm mf = (MainForm)this.Parent.Parent.Parent;
        //    mf.uC_Menu11_UC_Click(null, null);
        //}

        //private void pictureBox5_Click(object sender, EventArgs e)
        //{
        //    MainForm mf = (MainForm)this.Parent.Parent.Parent;
        //    mf.uC_Menu12_UC_Click(null, null);
        //}

        //private void pictureBox7_Click(object sender, EventArgs e)
        //{
        //    MainForm mf = (MainForm)this.Parent.Parent.Parent;
        //    mf.uC_Menu13_UC_Click(null, null);
        //}

        //private void pictureBox8_Click(object sender, EventArgs e)
        //{
        //    MainForm mf = (MainForm)this.Parent.Parent.Parent;
        //    mf.uC_Menu7_Click(null, null);
        //}
    }
}
