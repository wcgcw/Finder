using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataBaseServer;
using System.IO;

namespace Finder.Forms
{
    public partial class Partword : Form
    {
        DataBaseServer.MySqlCmd cmd = new DataBaseServer.MySqlCmd();

        public Partword()
        {
            InitializeComponent();
        }

        private void ClearForm()
        {
            pWord.Text = "";
            mWord.Text = "";
            aWordView.Checked = true;
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

        private bool checkHasData(string dataName, string dataValue)
        {
            string sql = "select count(0) from partword where " + dataName + "='" + dataValue + "'";
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

        private void UpdataPartWord(string dataName, string dataValue, string instr)
        {
            string sql = "update partword set " + dataName + " = '" + dataValue + "' where uid in (" + instr + ")";
            cmd.ExecuteNonQuery(sql);
            GetPartWordData();
        }

        private bool AddWord(string word, int part)
        {
            if (checkHasData("word", word))
            {
                MessageBox.Show("关键词 ：" + word + " 已存在！",  "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            string sql = "insert into partword (word,part) values ('" + word + "'," + part + ")";
            cmd.ExecuteNonQuery(sql);
            return true;
        }

        private void CellFormattingTable(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex != dataGridView1.NewRowIndex)
            {
                var part=dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                if (part.Equals("1"))
                {
                    e.Value = File.ReadAllBytes(@"icons\u.png");
                }
                else
                {
                    e.Value = File.ReadAllBytes(@"icons\d.png");
                }
            }
        }

        private void GetPartWordData()
        {
            DataTable dt = new DataTable();
            string sql = "select uid , word , part from partword ";
            if (pWordView.Checked)
            {
                sql += "where part=1";
            }
            if (mWordView.Checked)
            {
                sql += "where part=0";
            }

            dt = cmd.GetTabel(sql+" order by uid desc");

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pWord.Text.Equals("") && mWord.Text.Equals(""))
            {
                MessageBox.Show("请至少填写一个关键词！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pWord.Focus();
                return;
            }
            if (!pWord.Text.Equals(""))
            {
                AddWord(pWord.Text, 1);
            }
            if (!mWord.Text.Equals(""))
            {
                AddWord(mWord.Text, 0);
            }
            ClearForm();
            GetPartWordData();
        }

        private void Partword_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "uid", DataPropertyName = "uid" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "关键词", DataPropertyName = "word" });
            dataGridView1.Columns.Add(new DataGridViewImageColumn() { HeaderText = "正负词性", DataPropertyName = "part" });

            dataGridView1.CellFormatting += new DataGridViewCellFormattingEventHandler(CellFormattingTable);

            GetPartWordData();
        }

        private void aWordView_CheckedChanged(object sender, EventArgs e)
        {
            GetPartWordData();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.SelectedRows.Count;
            if (MessageBox.Show("确认删除选中的" + row.ToString() + "条记录吗？", "请确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "delete from partword where uid in (" + GetDataViewSelectedUids() + ")";
                cmd.ExecuteNonQuery(sql);
                GetPartWordData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdataPartWord("part", "1", GetDataViewSelectedUids());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UpdataPartWord("part", "0", GetDataViewSelectedUids());
        }
    }
}
