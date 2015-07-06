using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataBaseServer;

namespace Finder.Forms
{
    public partial class Keyword : Form
    {
        DataBaseServer.SQLitecommand cmd = new DataBaseServer.SQLitecommand();

        public Keyword()
        {
            InitializeComponent();
        }

        private bool checkHasData(string dataName, string dataValue) 
        {
            string sql = "select count(0) from keywords where " + dataName + "='" + dataValue + "'";
            int result = Convert.ToInt32(cmd.GetOne(sql));
            if (result.Equals(1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        private bool AddKeyWord(string kw, int meal, int mual, int has) 
        {
            if(checkHasData("keyword",kw))
            {
                MessageBox.Show("关键词 ：" + kw + " 已存在！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
           
           string sql = "insert into keywords (keyword,messagealarm,musicalarm,has) values ('"+kw+"',"+meal+","+mual+","+has+")";
           cmd.ExecuteNonQuery(sql);
           return true;
        }

        private string GetDataViewSelectedUids()
        {
            int row = dataGridView1.SelectedRows.Count;
            string[] uids = new string[row];
            while (row > 0)
            {
                uids[--row] = ((DataRowView)dataGridView1.SelectedRows[row].DataBoundItem).Row["uid"].ToString();
            }
            return string.Join(",", uids);
        }

        private void UpdataKeyWord(string dataName, string dataValue, string instr)
        {
            string sql = "update keywords set "+dataName+" = '"+dataValue +"' where uid in ("+instr+") and has=1";
            cmd.ExecuteNonQuery(sql);
            GetKeyWordData();
        }

        private void GetKeyWordData()
        {
            DataTable dt = new DataTable();
            //dt = cmd.GetTabel("select uid,keyword as 关键字,case messagealarm when 0 then '不报警' else '报警' end as 短信报警 ,case musicalarm when 0 then '不报警' else '报警' end as 声音报警 ,case has when 0 then '不包含' else '包含' end as 是否包含 from keywords order by uid desc");
            dt = cmd.GetTabel("select uid,keyword as 关键字,case messagealarm when 0 then '不报警' else '报警' end as 短信报警 ,case musicalarm when 0 then '不报警' else '报警' end as 声音报警 from keywords where has=1 order by uid desc");
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Visible = false;
        }

        private void ClearForm() {
            hasKeyWord.Text = "";
            //noHasKeyWord.Text = "";
            messageAlarm.Checked = false;
            musicAlarm.Checked = false;
        }

        private void Keyword_Load(object sender, EventArgs e)
        {
            ClearForm();
            GetKeyWordData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (hasKeyWord.Text.Equals("") && noHasKeyWord.Text.Equals(""))
            //{
            //    MessageBox.Show("请至少填写一个关键词","提示：");
            //    hasKeyWord.Focus();
            //    return;
            //}
            //if (!hasKeyWord.Text.Equals("")) 
            //{
            //    AddKeyWord(hasKeyWord.Text,messageAlarm.Checked?1:0,musicAlarm.Checked?1:0,1);
            //}
            //if (!noHasKeyWord.Text.Equals("")) {
            //    AddKeyWord(noHasKeyWord.Text, 0, 0, 0);
            //}
            if (hasKeyWord.Text.Equals(""))
            {
                MessageBox.Show("请填写关键词！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                hasKeyWord.Focus();
                return;
            }
            AddKeyWord(hasKeyWord.Text, messageAlarm.Checked ? 1 : 0, musicAlarm.Checked ? 1 : 0, 1);
            ClearForm();
            GetKeyWordData();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount <= 5)
            {
                MessageBox.Show("关键词需至少保留五个！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int row = dataGridView1.SelectedRows.Count;
            if (MessageBox.Show("确认删除选中的" + row.ToString() + "条记录吗？", "请确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "delete from keywords where uid in ("+GetDataViewSelectedUids()+")";
                cmd.ExecuteNonQuery(sql);
                GetKeyWordData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdataKeyWord("messagealarm", "1", GetDataViewSelectedUids());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UpdataKeyWord("messagealarm", "0", GetDataViewSelectedUids());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdataKeyWord("musicalarm", "1", GetDataViewSelectedUids());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UpdataKeyWord("musicalarm", "0", GetDataViewSelectedUids());
        }
    }
}
