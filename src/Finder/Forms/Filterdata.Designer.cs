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
            this.chkPrecise = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.cmsSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(-1, 7);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "查找内容：";
            // 
            // searchTxt
            // 
            this.searchTxt.Location = new System.Drawing.Point(72, 2);
            this.searchTxt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.searchTxt.Name = "searchTxt";
            this.searchTxt.Size = new System.Drawing.Size(250, 25);
            this.searchTxt.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(-1, 71);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "网站类别：";
            // 
            // pidlist
            // 
            this.pidlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pidlist.FormattingEnabled = true;
            this.pidlist.Location = new System.Drawing.Point(72, 68);
            this.pidlist.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pidlist.Name = "pidlist";
            this.pidlist.Size = new System.Drawing.Size(160, 23);
            this.pidlist.TabIndex = 12;
            this.pidlist.Tag = "9999";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(816, 71);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
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
            this.weblist.Location = new System.Drawing.Point(889, 68);
            this.weblist.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.weblist.Name = "weblist";
            this.weblist.Size = new System.Drawing.Size(160, 23);
            this.weblist.TabIndex = 14;
            this.weblist.Tag = "9999";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(827, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 28);
            this.button1.TabIndex = 17;
            this.button1.Tag = "9999";
            this.button1.Text = "查找";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(1033, 0);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(136, 28);
            this.button2.TabIndex = 18;
            this.button2.Tag = "9999";
            this.button2.Text = "清除当前结果";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.Location = new System.Drawing.Point(889, 0);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(136, 28);
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
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(9, 3, 9, 3);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.PaleTurquoise;
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
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dataGridView1.Location = new System.Drawing.Point(2, 99);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.RowTemplate.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1333, 371);
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
            this.cmsSearch.Size = new System.Drawing.Size(158, 28);
            // 
            // cms
            // 
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(157, 24);
            this.cms.Text = "留存证据(&S)";
            this.cms.Click += new System.EventHandler(this.cms_Click);
            // 
            // dataView
            // 
            this.dataView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataView.BackColor = System.Drawing.Color.White;
            this.dataView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataView.Location = new System.Drawing.Point(0, 478);
            this.dataView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataView.Name = "dataView";
            this.dataView.ReadOnly = true;
            this.dataView.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.dataView.Size = new System.Drawing.Size(1335, 399);
            this.dataView.TabIndex = 25;
            this.dataView.Tag = "9999";
            this.dataView.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(240, 71);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
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
            this.shenglist.Location = new System.Drawing.Point(312, 68);
            this.shenglist.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.shenglist.Name = "shenglist";
            this.shenglist.Size = new System.Drawing.Size(160, 23);
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
            this.shilist.Location = new System.Drawing.Point(480, 68);
            this.shilist.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.shilist.Name = "shilist";
            this.shilist.Size = new System.Drawing.Size(160, 23);
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
            this.xianlist.Location = new System.Drawing.Point(648, 68);
            this.xianlist.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xianlist.Name = "xianlist";
            this.xianlist.Size = new System.Drawing.Size(160, 23);
            this.xianlist.TabIndex = 29;
            this.xianlist.Tag = "9999";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(330, 7);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 30;
            this.label5.Text = "抓取时间：";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(403, 2);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(184, 25);
            this.dateTimePicker1.TabIndex = 31;
            this.dateTimePicker1.Tag = "9999";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(625, 2);
            this.dateTimePicker2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(184, 25);
            this.dateTimePicker2.TabIndex = 33;
            this.dateTimePicker2.Tag = "9999";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(595, 7);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 15);
            this.label6.TabIndex = 32;
            this.label6.Text = "至";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(-1, 40);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 15);
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
            this.kidlist.Location = new System.Drawing.Point(72, 37);
            this.kidlist.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.kidlist.Name = "kidlist";
            this.kidlist.Size = new System.Drawing.Size(160, 23);
            this.kidlist.TabIndex = 35;
            this.kidlist.Tag = "9999";
            this.kidlist.SelectedIndexChanged += new System.EventHandler(this.kidlist_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(240, 40);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 15);
            this.label8.TabIndex = 36;
            this.label8.Text = "事件名称：";
            // 
            // kwlist
            // 
            this.kwlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kwlist.FormattingEnabled = true;
            this.kwlist.Items.AddRange(new object[] {
            "全部"});
            this.kwlist.Location = new System.Drawing.Point(312, 37);
            this.kwlist.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.kwlist.Name = "kwlist";
            this.kwlist.Size = new System.Drawing.Size(160, 23);
            this.kwlist.TabIndex = 37;
            this.kwlist.Tag = "9999";
            this.kwlist.SelectedIndexChanged += new System.EventHandler(this.kwlist_SelectedIndexChanged);
            // 
            // chkPrecise
            // 
            this.chkPrecise.AutoSize = true;
            this.chkPrecise.Location = new System.Drawing.Point(480, 39);
            this.chkPrecise.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkPrecise.Name = "chkPrecise";
            this.chkPrecise.Size = new System.Drawing.Size(89, 19);
            this.chkPrecise.TabIndex = 38;
            this.chkPrecise.Text = "精确匹配";
            this.chkPrecise.UseVisualStyleBackColor = true;
            // 
            // Filterdata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1335, 880);
            this.Controls.Add(this.chkPrecise);
            this.Controls.Add(this.kwlist);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.kidlist);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.xianlist);
            this.Controls.Add(this.shilist);
            this.Controls.Add(this.shenglist);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dataView);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.weblist);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pidlist);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchTxt);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Filterdata";
            this.Text = "Filterdata";
            this.Load += new System.EventHandler(this.Filterdata_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.cmsSearch.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.CheckBox chkPrecise;
    }
}