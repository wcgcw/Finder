using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Finder.util;


namespace Finder.Forms
{
    public partial class Datachart : Form
    {
        public Datachart()
        {
            InitializeComponent();
        }

        DataBaseServer.SQLitecommand cmd = new DataBaseServer.SQLitecommand();
        
        DataTable kwdt = new DataTable();

        DataTable webdt = new DataTable();

        private void tab0_Load()
        {
            int day = trackBar0.Value;
            label0.Text = "最近" + day + "天的数据分析：";

            string time = DateTime.Now.AddDays(0 - day).ToString("yyyy-MM-dd HH:mm:ss");
            string sql = @"select count(1),a.keyWords from releaseinfo a  left join keywords b on a.keywords=b.[KeyWord]
                            where b.[Name] is not null and a.kid=0 and a.collectdate > '" + time + "' GROUP BY a.keyWords";
            //kwdt = cmd.GetTabel("SELECT count(1),keyWords FROM releaseinfo where kid=0 and collectdate > '" + time + "' GROUP BY keyWords");
            kwdt = cmd.GetTabel(sql);
            sql = @"select count(1),a.webaddress from releaseinfo a  left join keywords b on a.keywords=b.[KeyWord]
                            where b.[Name] is not null and  a.kid=0 and a.collectdate > '" + time + "' GROUP BY a.webaddress";
            //webdt = cmd.GetTabel("SELECT count(1),webaddress FROM releaseinfo where kid=0 and collectdate > '" + time + "' GROUP BY webaddress");
            webdt = cmd.GetTabel(sql);

            int l = kwdt.Rows.Count;
            string[] x = new string[l];
            int[] y = new int[l];
            for (int i = 0; i < l; i++)
            {
                x[i] = kwdt.Rows[i][1].ToString();
                y[i] = Convert.ToInt32(kwdt.Rows[i][0]);
            }

            chart10.Series[0].Points.DataBindXY(x, y);

            l = webdt.Rows.Count;
            string[] webx = new string[l];
            int[] weby = new int[l];

            for (int i = 0; i < l; i++)
            {
                webx[i] = webdt.Rows[i][1].ToString();
                weby[i] = Convert.ToInt32(webdt.Rows[i][0]);
            }

            chart20.Series["Series"].Points.DataBindXY(webx, weby);
        }

        private void tab1_Load()
        {
            int day = trackBar1.Value;
            label1.Text = "最近" + day + "天的数据分析：";

            string time = DateTime.Now.AddDays(0 - day).ToString("yyyy-MM-dd HH:mm:ss");
            string sql = @"select count(1),a.keyWords from releaseinfo a  left join keywords b on a.keywords=b.[KeyWord]
                            where b.[Name] is not null and  a.kid=1 and a.collectdate > '" + time + "' GROUP BY a.keyWords";
            //kwdt = cmd.GetTabel("SELECT count(1),keyWords FROM releaseinfo where kid=1 and collectdate > '" + time + "' GROUP BY keyWords");
            kwdt = cmd.GetTabel(sql);
            sql = @"select count(1),a.webaddress from releaseinfo a  left join keywords b on a.keywords=b.[KeyWord]
                            where b.[Name] is not null and  a.kid=1 and a.collectdate > '" + time + "' GROUP BY a.webaddress";
            //webdt = cmd.GetTabel("SELECT count(1),webaddress FROM releaseinfo where kid=1 and collectdate > '" + time + "' GROUP BY webaddress");
            webdt = cmd.GetTabel(sql);

            int l = kwdt.Rows.Count;
            string[] x = new string[l];
            int[] y = new int[l];
            for (int i = 0; i < l; i++)
            {
                x[i]=kwdt.Rows[i][1].ToString();
                y[i]=Convert.ToInt32(kwdt.Rows[i][0]);
            }
            
            chart11.Series[0].Points.DataBindXY(x, y);

            l = webdt.Rows.Count;
            string[] webx = new string[l];
            int[] weby = new int[l];

            for (int i = 0; i < l; i++)
            {
                webx[i] = webdt.Rows[i][1].ToString();
                weby[i] = Convert.ToInt32(webdt.Rows[i][0]);
            }

            chart21.Series["Series"].Points.DataBindXY(webx, weby);
        }

        private void tab2_Load()
        {
            int day = trackBar2.Value;
            label2.Text = "最近" + day + "天的数据分析：";
            string time = DateTime.Now.AddDays(0 - day).ToString("yyyy-MM-dd HH:mm:ss");
            string sql = @"select count(1),a.keyWords from releaseinfo a  left join keywords b on a.keywords=b.[KeyWord]
                            where b.[Name] is not null and  a.kid=2 and a.collectdate > '" + time + "' GROUP BY a.keyWords";
            //kwdt = cmd.GetTabel("SELECT count(1),keyWords FROM releaseinfo where kid=2 and collectdate > '" + time + "' GROUP BY keyWords");
            kwdt = cmd.GetTabel(sql);
            sql = @"select count(1),a.webaddress from releaseinfo a  left join keywords b on a.keywords=b.[KeyWord]
                            where b.[Name] is not null and  a.kid=2 and a.collectdate > '" + time + "' GROUP BY a.webaddress";
            //webdt = cmd.GetTabel("SELECT count(1),webaddress FROM releaseinfo where kid=2 and collectdate > '" + time + "' GROUP BY webaddress");
            webdt = cmd.GetTabel(sql);

            int l = kwdt.Rows.Count;
            string[] x = new string[l];
            int[] y = new int[l];
            for (int i = 0; i < l; i++)
            {
                x[i] = kwdt.Rows[i][1].ToString();
                y[i] = Convert.ToInt32(kwdt.Rows[i][0]);
            }

            chart12.Series["Series"].Points.DataBindXY(x, y);

            l = webdt.Rows.Count;
            string[] webx = new string[l];
            int[] weby = new int[l];

            for (int i = 0; i < l; i++)
            {
                webx[i] = webdt.Rows[i][1].ToString();
                weby[i] = Convert.ToInt32(webdt.Rows[i][0]);
            }

            chart22.Series["Series"].Points.DataBindXY(webx, weby);
        }

        private void tab3_Load()
        {
            int day = trackBar3.Value;
            label3.Text = "最近" + day + "天的数据分析：";
            string time = DateTime.Now.AddDays(0 - day).ToString("yyyy-MM-dd HH:mm:ss");
            string sql = @"select count(1),a.keyWords from releaseinfo a  left join keywords b on a.keywords=b.[KeyWord]
                            where b.[Name] is not null and  a.kid=3 and a.collectdate > '" + time + "' GROUP BY a.keyWords";
            //kwdt = cmd.GetTabel("SELECT count(1),keyWords FROM releaseinfo where kid=3 and collectdate > '" + time + "' GROUP BY keyWords");
            kwdt = cmd.GetTabel(sql);
            sql = @"select count(1),a.webaddress from releaseinfo a  left join keywords b on a.keywords=b.[KeyWord]
                            where b.[Name] is not null and  a.kid=3 and a.collectdate > '" + time + "' GROUP BY a.webaddress";
            //webdt = cmd.GetTabel("SELECT count(1),webaddress FROM releaseinfo where kid=3 and collectdate > '" + time + "' GROUP BY webaddress");
            webdt = cmd.GetTabel(sql);

            int l = kwdt.Rows.Count;
            string[] x = new string[l];
            int[] y = new int[l];
            for (int i = 0; i < l; i++)
            {
                x[i] = kwdt.Rows[i][1].ToString();
                y[i] = Convert.ToInt32(kwdt.Rows[i][0]);
            }

            chart13.Series["Series"].Points.DataBindXY(x, y);

            l = webdt.Rows.Count;
            string[] webx = new string[l];
            int[] weby = new int[l];

            for (int i = 0; i < l; i++)
            {
                webx[i] = webdt.Rows[i][1].ToString();
                weby[i] = Convert.ToInt32(webdt.Rows[i][0]);
            }

            chart23.Series["Series"].Points.DataBindXY(webx, weby);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    tab0_Load();
                    break;
                case 1:
                    tab1_Load();
                    break;
                case 2:
                    tab2_Load();
                    break;
                case 3:
                    tab3_Load();
                    break;
            }

        }

        private void Datachart_Load(object sender, EventArgs e)
        {
            
            string softVer = GlobalPars.GloPars.ContainsKey("SoftVer") ? GlobalPars.GloPars["SoftVer"].ToString() : "1";
            if (softVer.Equals("1"))
            {
                tabControl1.TabPages.Remove(tabPage1);
                tabControl1.TabPages.Remove(tabPage2);
                tabControl1.TabPages.Remove(tabPage3);
            }
            tab0_Load();
        }

        private void trackBar0_ValueChanged(object sender, EventArgs e)
        {
            tab0_Load();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            tab1_Load();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            tab2_Load();
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            tab3_Load();
        }

        private void allKwChart_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            HitTestResult htr = chart10.HitTest(e.X, e.Y);
            int time = 0;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    time = trackBar0.Value;
                    htr = chart10.HitTest(e.X, e.Y);
                    break;
                case 1:
                    time = trackBar1.Value;
                    htr = chart11.HitTest(e.X, e.Y);
                    break;
                case 2:
                    time = trackBar2.Value;
                    htr = chart12.HitTest(e.X, e.Y);
                    break;
                case 3:
                    time = trackBar3.Value;
                    htr = chart13.HitTest(e.X, e.Y);
                    break;
            }
            if (htr.PointIndex < 0)
            {
                return;
            }
            string keyword = "";
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    keyword = chart10.Series["Series"].Points[htr.PointIndex].AxisLabel;
                    break;
                case 1:
                    keyword = chart11.Series["Series"].Points[htr.PointIndex].AxisLabel;
                    break;
                case 2:
                    keyword = chart12.Series["Series"].Points[htr.PointIndex].AxisLabel;
                    break;
                case 3:
                    keyword = chart13.Series["Series"].Points[htr.PointIndex].AxisLabel;
                    break;
            }
            ChartDetailData cdd = new ChartDetailData(tabControl1.SelectedIndex, keyword, "全网", time, "U");
            cdd.ShowDialog();
        }

        private void allWebChart_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            HitTestResult htr = chart10.HitTest(e.X, e.Y);
            int time = 0;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    time = trackBar0.Value;
                    htr = chart20.HitTest(e.X, e.Y);
                    break;
                case 1:
                    time = trackBar1.Value;
                    htr = chart21.HitTest(e.X, e.Y);
                    break;
                case 2:
                    time = trackBar2.Value;
                    htr = chart22.HitTest(e.X, e.Y);
                    break;
                case 3:
                    time = trackBar3.Value;
                    htr = chart23.HitTest(e.X, e.Y);
                    break;
            }
            if (htr.PointIndex < 0)
            {
                return;
            }
            string webaddress = "";
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    webaddress = chart20.Series["Series"].Points[htr.PointIndex].AxisLabel;
                    break;
                case 1:
                    webaddress = chart21.Series["Series"].Points[htr.PointIndex].AxisLabel;
                    break;
                case 2:
                    webaddress = chart22.Series["Series"].Points[htr.PointIndex].AxisLabel;
                    break;
                case 3:
                    webaddress = chart23.Series["Series"].Points[htr.PointIndex].AxisLabel;
                    break;
            }
            ChartDetailData cdd = new ChartDetailData(tabControl1.SelectedIndex, webaddress, "全网", time, "D");
            cdd.ShowDialog();
        }
    }
}
