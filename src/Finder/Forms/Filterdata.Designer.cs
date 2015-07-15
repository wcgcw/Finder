namespace Finder.Forms
{
    partial class Filterdata
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label2 = new System.Windows.Forms.Label();
            this.searchTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pidlist = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.weblist = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cmsSearch = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cms = new System.Windows.Forms.ToolStripMenuItem();
            this.dataView = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.shenglist = new System.Windows.Forms.ComboBox();
            this.shilist = new System.Windows.Forms.ComboBox();
            this.xianlist = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.kidlist = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.kwlist = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblMainFocus = new System.Windows.Forms.Label();
            this.lblFocus = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.RichTextBox();
            this.lblCancelFocus = new System.Windows.Forms.Label();
            this.lblCancelTop = new System.Windows.Forms.Label();
            this.lblSetTop = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.cmsSearch.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(3, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "查找内容：";
            // 
            // searchTxt
            // 
            this.searchTxt.Location = new System.Drawing.Point(58, 17);
            this.searchTxt.Name = "searchTxt";
            this.searchTxt.Size = new System.Drawing.Size(188, 21);
            this.searchTxt.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(3, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "网站类别：";
            // 
            // pidlist
            // 
            this.pidlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pidlist.FormattingEnabled = true;
            this.pidlist.Location = new System.Drawing.Point(58, 69);
            this.pidlist.Name = "pidlist";
            this.pidlist.Size = new System.Drawing.Size(121, 20);
            this.pidlist.TabIndex = 12;
            this.pidlist.Tag = "9999";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(616, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "选择网站：";
            // 
            // weblist
            // 
            this.weblist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.weblist.FormattingEnabled = true;
            this.weblist.Items.AddRange(new object[] {
            "网易新闻",
            "新浪财经",
            "腾讯专题"});
            this.weblist.Location = new System.Drawing.Point(671, 69);
            this.weblist.Name = "weblist";
            this.weblist.Size = new System.Drawing.Size(121, 20);
            this.weblist.TabIndex = 14;
            this.weblist.Tag = "9999";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(624, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 22);
            this.button1.TabIndex = 17;
            this.button1.Tag = "9999";
            this.button1.Text = "查找";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(779, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 22);
            this.button2.TabIndex = 18;
            this.button2.Tag = "9999";
            this.button2.Text = "清除当前结果";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.Location = new System.Drawing.Point(671, 15);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(102, 22);
            this.button3.TabIndex = 19;
            this.button3.Tag = "9999";
            this.button3.Text = "导出查找结果";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(9, 3, 9, 3);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeight = 32;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.ContextMenuStrip = this.cmsSearch;
            this.dataGridView1.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.CornflowerBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dataGridView1.Location = new System.Drawing.Point(3, 123);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.RowTemplate.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(995, 314);
            this.dataGridView1.TabIndex = 23;
            this.dataGridView1.Tag = "";
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // cmsSearch
            // 
            this.cmsSearch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cms});
            this.cmsSearch.Name = "cmsSearch";
            this.cmsSearch.Size = new System.Drawing.Size(140, 26);
            // 
            // cms
            // 
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(139, 22);
            this.cms.Text = "留存证据(&S)";
            this.cms.Click += new System.EventHandler(this.cms_Click);
            // 
            // dataView
            // 
            this.dataView.BackColor = System.Drawing.Color.White;
            this.dataView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataView.Location = new System.Drawing.Point(3, 487);
            this.dataView.Name = "dataView";
            this.dataView.ReadOnly = true;
            this.dataView.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.dataView.Size = new System.Drawing.Size(995, 214);
            this.dataView.TabIndex = 25;
            this.dataView.Tag = "9999";
            this.dataView.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(184, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "网站区域：";
            // 
            // shenglist
            // 
            this.shenglist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shenglist.FormattingEnabled = true;
            this.shenglist.Items.AddRange(new object[] {
            "全部",
            "微博",
            "博客",
            "论坛",
            "其他"});
            this.shenglist.Location = new System.Drawing.Point(239, 69);
            this.shenglist.Name = "shenglist";
            this.shenglist.Size = new System.Drawing.Size(121, 20);
            this.shenglist.TabIndex = 27;
            this.shenglist.Tag = "9999";
            this.shenglist.SelectedIndexChanged += new System.EventHandler(this.shenglist_SelectedIndexChanged);
            // 
            // shilist
            // 
            this.shilist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shilist.FormattingEnabled = true;
            this.shilist.Items.AddRange(new object[] {
            "全部",
            "微博",
            "博客",
            "论坛",
            "其他"});
            this.shilist.Location = new System.Drawing.Point(365, 69);
            this.shilist.Name = "shilist";
            this.shilist.Size = new System.Drawing.Size(121, 20);
            this.shilist.TabIndex = 28;
            this.shilist.Tag = "9999";
            this.shilist.SelectedIndexChanged += new System.EventHandler(this.shilist_SelectedIndexChanged);
            // 
            // xianlist
            // 
            this.xianlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.xianlist.FormattingEnabled = true;
            this.xianlist.Items.AddRange(new object[] {
            "全部",
            "微博",
            "博客",
            "论坛",
            "其他"});
            this.xianlist.Location = new System.Drawing.Point(491, 69);
            this.xianlist.Name = "xianlist";
            this.xianlist.Size = new System.Drawing.Size(121, 20);
            this.xianlist.TabIndex = 29;
            this.xianlist.Tag = "9999";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(252, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 30;
            this.label5.Text = "抓取时间：";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(306, 17);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(139, 21);
            this.dateTimePicker1.TabIndex = 31;
            this.dateTimePicker1.Tag = "9999";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(473, 17);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(139, 21);
            this.dateTimePicker2.TabIndex = 33;
            this.dateTimePicker2.Tag = "9999";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(450, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 32;
            this.label6.Text = "至";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(3, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 34;
            this.label7.Text = "事件类别：";
            // 
            // kidlist
            // 
            this.kidlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kidlist.FormattingEnabled = true;
            this.kidlist.Items.AddRange(new object[] {
            "常规舆情",
            "敏感舆情",
            "重点舆情",
            "突发舆情",
            "全部"});
            this.kidlist.Location = new System.Drawing.Point(58, 45);
            this.kidlist.Name = "kidlist";
            this.kidlist.Size = new System.Drawing.Size(121, 20);
            this.kidlist.TabIndex = 35;
            this.kidlist.Tag = "9999";
            this.kidlist.SelectedIndexChanged += new System.EventHandler(this.kidlist_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(184, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 36;
            this.label8.Text = "事件名称：";
            // 
            // kwlist
            // 
            this.kwlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kwlist.FormattingEnabled = true;
            this.kwlist.Items.AddRange(new object[] {
            "全部"});
            this.kwlist.Location = new System.Drawing.Point(239, 45);
            this.kwlist.Name = "kwlist";
            this.kwlist.Size = new System.Drawing.Size(121, 20);
            this.kwlist.TabIndex = 37;
            this.kwlist.Tag = "9999";
            this.kwlist.SelectedIndexChanged += new System.EventHandler(this.kwlist_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataView, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1001, 704);
            this.tableLayoutPanel1.TabIndex = 39;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblCount);
            this.panel1.Controls.Add(this.kwlist);
            this.panel1.Controls.Add(this.dateTimePicker1);
            this.panel1.Controls.Add(this.searchTxt);
            this.panel1.Controls.Add(this.pidlist);
            this.panel1.Controls.Add(this.shenglist);
            this.panel1.Controls.Add(this.weblist);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.kidlist);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.dateTimePicker2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.xianlist);
            this.panel1.Controls.Add(this.shilist);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(995, 114);
            this.panel1.TabIndex = 26;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblMainFocus);
            this.panel2.Controls.Add(this.lblFocus);
            this.panel2.Controls.Add(this.txtTitle);
            this.panel2.Controls.Add(this.lblCancelFocus);
            this.panel2.Controls.Add(this.lblCancelTop);
            this.panel2.Controls.Add(this.lblSetTop);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 443);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(995, 38);
            this.panel2.TabIndex = 27;
            // 
            // lblMainFocus
            // 
            this.lblMainFocus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMainFocus.AutoSize = true;
            this.lblMainFocus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMainFocus.ForeColor = System.Drawing.Color.Red;
            this.lblMainFocus.Location = new System.Drawing.Point(853, 11);
            this.lblMainFocus.Name = "lblMainFocus";
            this.lblMainFocus.Size = new System.Drawing.Size(59, 12);
            this.lblMainFocus.TabIndex = 0;
            this.lblMainFocus.Text = "+重点关注";
            this.lblMainFocus.Click += new System.EventHandler(this.lblMainFocus_Click);
            // 
            // lblFocus
            // 
            this.lblFocus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFocus.AutoSize = true;
            this.lblFocus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblFocus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblFocus.Location = new System.Drawing.Point(812, 11);
            this.lblFocus.Name = "lblFocus";
            this.lblFocus.Size = new System.Drawing.Size(35, 12);
            this.lblFocus.TabIndex = 0;
            this.lblFocus.Text = "+关注";
            this.lblFocus.Click += new System.EventHandler(this.lblFocus_Click);
            // 
            // txtTitle
            // 
            this.txtTitle.BackColor = System.Drawing.Color.White;
            this.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTitle.Location = new System.Drawing.Point(5, 3);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.ReadOnly = true;
            this.txtTitle.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtTitle.Size = new System.Drawing.Size(676, 30);
            this.txtTitle.TabIndex = 25;
            this.txtTitle.Tag = "9999";
            this.txtTitle.Text = "";
            // 
            // lblCancelFocus
            // 
            this.lblCancelFocus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCancelFocus.AutoSize = true;
            this.lblCancelFocus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblCancelFocus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblCancelFocus.Location = new System.Drawing.Point(918, 11);
            this.lblCancelFocus.Name = "lblCancelFocus";
            this.lblCancelFocus.Size = new System.Drawing.Size(53, 12);
            this.lblCancelFocus.TabIndex = 0;
            this.lblCancelFocus.Text = "取消关注";
            this.lblCancelFocus.Click += new System.EventHandler(this.lblCancelFocus_Click);
            // 
            // lblCancelTop
            // 
            this.lblCancelTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCancelTop.AutoSize = true;
            this.lblCancelTop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblCancelTop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblCancelTop.Location = new System.Drawing.Point(739, 11);
            this.lblCancelTop.Name = "lblCancelTop";
            this.lblCancelTop.Size = new System.Drawing.Size(53, 12);
            this.lblCancelTop.TabIndex = 0;
            this.lblCancelTop.Text = "取消置顶";
            this.lblCancelTop.Click += new System.EventHandler(this.lblCancelTop_Click);
            // 
            // lblSetTop
            // 
            this.lblSetTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSetTop.AutoSize = true;
            this.lblSetTop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblSetTop.ForeColor = System.Drawing.Color.Blue;
            this.lblSetTop.Location = new System.Drawing.Point(695, 11);
            this.lblSetTop.Name = "lblSetTop";
            this.lblSetTop.Size = new System.Drawing.Size(35, 12);
            this.lblSetTop.TabIndex = 0;
            this.lblSetTop.Text = "+置顶";
            this.lblSetTop.Click += new System.EventHandler(this.lblSetTop_Click);
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(861, 72);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(131, 12);
            this.lblCount.TabIndex = 38;
            this.lblCount.Text = "共计检索到XXXXX条结果";
            // 
            // Filterdata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1001, 704);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Filterdata";
            this.Text = "Filterdata";
            this.Load += new System.EventHandler(this.Filterdata_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.cmsSearch.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox searchTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox pidlist;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox weblist;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.RichTextBox dataView;
        private System.Windows.Forms.ContextMenuStrip cmsSearch;
        private System.Windows.Forms.ToolStripMenuItem cms;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox shenglist;
        private System.Windows.Forms.ComboBox shilist;
        private System.Windows.Forms.ComboBox xianlist;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox kidlist;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox kwlist;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblMainFocus;
        private System.Windows.Forms.Label lblFocus;
        private System.Windows.Forms.Label lblCancelFocus;
        private System.Windows.Forms.Label lblCancelTop;
        private System.Windows.Forms.Label lblSetTop;
        private System.Windows.Forms.RichTextBox txtTitle;
        private System.Windows.Forms.Label lblCount;
    }
}