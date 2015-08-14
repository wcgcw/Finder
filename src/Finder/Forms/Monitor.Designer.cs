namespace Finder.Forms
{
    partial class Monitor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmWeb = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ssmWeb = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkAllWeb = new System.Windows.Forms.CheckBox();
            this.chkCustom = new System.Windows.Forms.CheckBox();
            this.chkWeixin = new System.Windows.Forms.CheckBox();
            this.chkTieba = new System.Windows.Forms.CheckBox();
            this.chkWeibo = new System.Windows.Forms.CheckBox();
            this.chkBlog = new System.Windows.Forms.CheckBox();
            this.chkBBS = new System.Windows.Forms.CheckBox();
            this.chkMedia = new System.Windows.Forms.CheckBox();
            this.kwlist = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.kidlist = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dvView = new Finder.UserControles.DoubleBufferDataGridView();
            this.cmWeb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvView)).BeginInit();
            this.SuspendLayout();
            // 
            // cmWeb
            // 
            this.cmWeb.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssmWeb});
            this.cmWeb.Name = "cmWeb";
            this.cmWeb.Size = new System.Drawing.Size(140, 26);
            this.cmWeb.Tag = "9999";
            // 
            // ssmWeb
            // 
            this.ssmWeb.Name = "ssmWeb";
            this.ssmWeb.Size = new System.Drawing.Size(139, 22);
            this.ssmWeb.Tag = "9999";
            this.ssmWeb.Text = "留存证据(&S)";
            this.ssmWeb.Click += new System.EventHandler(this.ssmLCZJ_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.Color.Black;
            this.pictureBox2.Location = new System.Drawing.Point(1167, -30);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(48, 43);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Tag = "9999";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox3.BackColor = System.Drawing.Color.Black;
            this.pictureBox3.Location = new System.Drawing.Point(1173, -30);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(48, 43);
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Tag = "9999";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox4.BackColor = System.Drawing.Color.Black;
            this.pictureBox4.Location = new System.Drawing.Point(791, -30);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(48, 43);
            this.pictureBox4.TabIndex = 2;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Tag = "9999";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.Tag = "9999";
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "button_blue_play.png");
            this.imageList1.Images.SetKeyName(1, "button_grey_play.png");
            this.imageList1.Images.SetKeyName(2, "button_blue_stop.png");
            this.imageList1.Images.SetKeyName(3, "button_grey_stop.png");
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::Finder.Properties.Resources.loading;
            this.pictureBox1.Location = new System.Drawing.Point(813, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Tag = "9999";
            this.pictureBox1.Visible = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox6.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox6.BackgroundImage = global::Finder.Properties.Resources.button_blue_play;
            this.pictureBox6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox6.Location = new System.Drawing.Point(851, 17);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(34, 32);
            this.pictureBox6.TabIndex = 6;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.Tag = "9999";
            this.pictureBox6.Click += new System.EventHandler(this.pictureBox6_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 119F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(900, 620);
            this.tableLayoutPanel1.TabIndex = 30;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkAllWeb);
            this.panel1.Controls.Add(this.chkCustom);
            this.panel1.Controls.Add(this.chkWeixin);
            this.panel1.Controls.Add(this.chkTieba);
            this.panel1.Controls.Add(this.chkWeibo);
            this.panel1.Controls.Add(this.chkBlog);
            this.panel1.Controls.Add(this.chkBBS);
            this.panel1.Controls.Add(this.chkMedia);
            this.panel1.Controls.Add(this.kwlist);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.kidlist);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.pictureBox6);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(894, 113);
            this.panel1.TabIndex = 0;
            // 
            // chkAllWeb
            // 
            this.chkAllWeb.AutoSize = true;
            this.chkAllWeb.Checked = true;
            this.chkAllWeb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllWeb.Location = new System.Drawing.Point(14, 65);
            this.chkAllWeb.Name = "chkAllWeb";
            this.chkAllWeb.Size = new System.Drawing.Size(48, 16);
            this.chkAllWeb.TabIndex = 88;
            this.chkAllWeb.Tag = "9999";
            this.chkAllWeb.Text = "全网";
            this.chkAllWeb.UseVisualStyleBackColor = true;
            // 
            // chkCustom
            // 
            this.chkCustom.AutoSize = true;
            this.chkCustom.Checked = true;
            this.chkCustom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCustom.Location = new System.Drawing.Point(416, 65);
            this.chkCustom.Name = "chkCustom";
            this.chkCustom.Size = new System.Drawing.Size(48, 16);
            this.chkCustom.TabIndex = 86;
            this.chkCustom.Tag = "9999";
            this.chkCustom.Text = "定制";
            this.chkCustom.UseVisualStyleBackColor = true;
            // 
            // chkWeixin
            // 
            this.chkWeixin.AutoSize = true;
            this.chkWeixin.Checked = true;
            this.chkWeixin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWeixin.Location = new System.Drawing.Point(200, 65);
            this.chkWeixin.Name = "chkWeixin";
            this.chkWeixin.Size = new System.Drawing.Size(48, 16);
            this.chkWeixin.TabIndex = 87;
            this.chkWeixin.Tag = "9999";
            this.chkWeixin.Text = "微信";
            this.chkWeixin.UseVisualStyleBackColor = true;
            // 
            // chkTieba
            // 
            this.chkTieba.AutoSize = true;
            this.chkTieba.Checked = true;
            this.chkTieba.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTieba.Location = new System.Drawing.Point(254, 65);
            this.chkTieba.Name = "chkTieba";
            this.chkTieba.Size = new System.Drawing.Size(48, 16);
            this.chkTieba.TabIndex = 85;
            this.chkTieba.Tag = "9999";
            this.chkTieba.Text = "贴吧";
            this.chkTieba.UseVisualStyleBackColor = true;
            // 
            // chkWeibo
            // 
            this.chkWeibo.AutoSize = true;
            this.chkWeibo.Checked = true;
            this.chkWeibo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWeibo.Location = new System.Drawing.Point(149, 65);
            this.chkWeibo.Name = "chkWeibo";
            this.chkWeibo.Size = new System.Drawing.Size(48, 16);
            this.chkWeibo.TabIndex = 84;
            this.chkWeibo.Tag = "9999";
            this.chkWeibo.Text = "微博";
            this.chkWeibo.UseVisualStyleBackColor = true;
            // 
            // chkBlog
            // 
            this.chkBlog.AutoSize = true;
            this.chkBlog.Checked = true;
            this.chkBlog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBlog.Location = new System.Drawing.Point(362, 65);
            this.chkBlog.Name = "chkBlog";
            this.chkBlog.Size = new System.Drawing.Size(48, 16);
            this.chkBlog.TabIndex = 83;
            this.chkBlog.Tag = "9999";
            this.chkBlog.Text = "博客";
            this.chkBlog.UseVisualStyleBackColor = true;
            // 
            // chkBBS
            // 
            this.chkBBS.AutoSize = true;
            this.chkBBS.Checked = true;
            this.chkBBS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBBS.Location = new System.Drawing.Point(308, 65);
            this.chkBBS.Name = "chkBBS";
            this.chkBBS.Size = new System.Drawing.Size(48, 16);
            this.chkBBS.TabIndex = 82;
            this.chkBBS.Tag = "9999";
            this.chkBBS.Text = "论坛";
            this.chkBBS.UseVisualStyleBackColor = true;
            // 
            // chkMedia
            // 
            this.chkMedia.AutoSize = true;
            this.chkMedia.Checked = true;
            this.chkMedia.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMedia.Location = new System.Drawing.Point(71, 65);
            this.chkMedia.Name = "chkMedia";
            this.chkMedia.Size = new System.Drawing.Size(72, 16);
            this.chkMedia.TabIndex = 81;
            this.chkMedia.Tag = "9999";
            this.chkMedia.Text = "主流媒体";
            this.chkMedia.UseVisualStyleBackColor = true;
            // 
            // kwlist
            // 
            this.kwlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kwlist.FormattingEnabled = true;
            this.kwlist.Items.AddRange(new object[] {
            "全部"});
            this.kwlist.Location = new System.Drawing.Point(257, 17);
            this.kwlist.Name = "kwlist";
            this.kwlist.Size = new System.Drawing.Size(121, 20);
            this.kwlist.TabIndex = 41;
            this.kwlist.Tag = "9999";
            this.kwlist.SelectedIndexChanged += new System.EventHandler(this.kwlist_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(198, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 40;
            this.label8.Text = "事件名称：";
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
            this.kidlist.Location = new System.Drawing.Point(71, 17);
            this.kidlist.Name = "kidlist";
            this.kidlist.Size = new System.Drawing.Size(121, 20);
            this.kidlist.TabIndex = 39;
            this.kidlist.Tag = "9999";
            this.kidlist.SelectedIndexChanged += new System.EventHandler(this.kidlist_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(12, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 38;
            this.label7.Text = "事件类别：";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dvView);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 122);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(894, 495);
            this.panel2.TabIndex = 1;
            // 
            // dvView
            // 
            this.dvView.AllowUserToAddRows = false;
            this.dvView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(9, 3, 9, 3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dvView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dvView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dvView.BackgroundColor = System.Drawing.Color.White;
            this.dvView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(9, 3, 9, 3);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.PaleTurquoise;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.dvView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dvView.ColumnHeadersHeight = 32;
            this.dvView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dvView.ContextMenuStrip = this.cmWeb;
            this.dvView.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(9, 3, 9, 3);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dvView.DefaultCellStyle = dataGridViewCellStyle3;
            this.dvView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvView.EnableHeadersVisualStyles = false;
            this.dvView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dvView.Location = new System.Drawing.Point(0, 0);
            this.dvView.MultiSelect = false;
            this.dvView.Name = "dvView";
            this.dvView.ReadOnly = true;
            this.dvView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dvView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dvView.RowHeadersVisible = false;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(9, 3, 9, 3);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dvView.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dvView.RowTemplate.Height = 24;
            this.dvView.RowTemplate.ReadOnly = true;
            this.dvView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dvView.Size = new System.Drawing.Size(894, 495);
            this.dvView.TabIndex = 29;
            this.dvView.Tag = "9999";
            this.dvView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
            this.dvView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvView_CellContentDoubleClick);
            this.dvView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
            // 
            // Monitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 620);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Monitor";
            this.Text = "Monitoring";
            this.Load += new System.EventHandler(this.Monitoring_Load);
            this.cmWeb.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dvView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmWeb;
        private System.Windows.Forms.ToolStripMenuItem ssmWeb;
        private System.Windows.Forms.PictureBox pictureBox3;
        //private System.Windows.Forms.DataGridView dvWeb;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ImageList imageList1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private UserControles.DoubleBufferDataGridView dvView;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox kwlist;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox kidlist;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkAllWeb;
        private System.Windows.Forms.CheckBox chkCustom;
        private System.Windows.Forms.CheckBox chkWeixin;
        private System.Windows.Forms.CheckBox chkTieba;
        private System.Windows.Forms.CheckBox chkWeibo;
        private System.Windows.Forms.CheckBox chkBlog;
        private System.Windows.Forms.CheckBox chkBBS;
        private System.Windows.Forms.CheckBox chkMedia;
    }
}