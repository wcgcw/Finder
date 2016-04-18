namespace Finder.Forms
{
    partial class SystemSetting
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_oldpwd = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_newpwd1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_newpwd2 = new System.Windows.Forms.TextBox();
            this.btn_updatepwd = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btn_weibo_sina_auth = new System.Windows.Forms.Button();
            this.btn_weibo_qq_auth = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tb_choosedimgpath = new System.Windows.Forms.TextBox();
            this.btn_saveimgpath = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.lbsmsCX = new System.Windows.Forms.Label();
            this.lbsoundCX = new System.Windows.Forms.Label();
            this.btn_query_smscount = new System.Windows.Forms.Button();
            this.lb_show_shengyusmscount = new System.Windows.Forms.Label();
            this.dgv1 = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgv2 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pb_choosepath = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_choosepath)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(12, 379);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(233, 14);
            this.label3.TabIndex = 20;
            this.label3.Tag = "9999";
            this.label3.Text = "微博授权(点击以下图片按钮授权)";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(102)))), ((int)(((byte)(0)))));
            this.label4.Location = new System.Drawing.Point(12, 615);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 14);
            this.label4.TabIndex = 20;
            this.label4.Tag = "9999";
            this.label4.Text = "修改密码";
            this.label4.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(32, 655);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 14);
            this.label5.TabIndex = 20;
            this.label5.Tag = "9999";
            this.label5.Text = "当前密码：";
            this.label5.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.groupBox1.Location = new System.Drawing.Point(-2, 635);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1139, 2);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // tb_oldpwd
            // 
            this.tb_oldpwd.Location = new System.Drawing.Point(111, 652);
            this.tb_oldpwd.MaxLength = 20;
            this.tb_oldpwd.Name = "tb_oldpwd";
            this.tb_oldpwd.Size = new System.Drawing.Size(131, 21);
            this.tb_oldpwd.TabIndex = 11;
            this.tb_oldpwd.UseSystemPasswordChar = true;
            this.tb_oldpwd.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(269, 656);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 20;
            this.label6.Tag = "9999";
            this.label6.Text = "新密码：";
            this.label6.Visible = false;
            // 
            // tb_newpwd1
            // 
            this.tb_newpwd1.Location = new System.Drawing.Point(324, 652);
            this.tb_newpwd1.MaxLength = 20;
            this.tb_newpwd1.Name = "tb_newpwd1";
            this.tb_newpwd1.Size = new System.Drawing.Size(131, 21);
            this.tb_newpwd1.TabIndex = 12;
            this.tb_newpwd1.UseSystemPasswordChar = true;
            this.tb_newpwd1.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(488, 655);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 20;
            this.label7.Tag = "9999";
            this.label7.Text = "重复密码：";
            this.label7.Visible = false;
            // 
            // tb_newpwd2
            // 
            this.tb_newpwd2.Location = new System.Drawing.Point(556, 651);
            this.tb_newpwd2.MaxLength = 20;
            this.tb_newpwd2.Name = "tb_newpwd2";
            this.tb_newpwd2.Size = new System.Drawing.Size(131, 21);
            this.tb_newpwd2.TabIndex = 13;
            this.tb_newpwd2.UseSystemPasswordChar = true;
            this.tb_newpwd2.Visible = false;
            // 
            // btn_updatepwd
            // 
            this.btn_updatepwd.Location = new System.Drawing.Point(706, 649);
            this.btn_updatepwd.Name = "btn_updatepwd";
            this.btn_updatepwd.Size = new System.Drawing.Size(96, 23);
            this.btn_updatepwd.TabIndex = 14;
            this.btn_updatepwd.Text = "确认修改密码";
            this.btn_updatepwd.UseVisualStyleBackColor = true;
            this.btn_updatepwd.Visible = false;
            this.btn_updatepwd.Click += new System.EventHandler(this.btn_updatepwd_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.groupBox2.Location = new System.Drawing.Point(-2, 403);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1139, 2);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Visible = false;
            // 
            // btn_weibo_sina_auth
            // 
            this.btn_weibo_sina_auth.BackgroundImage = global::Finder.Properties.Resources.sinaweibo;
            this.btn_weibo_sina_auth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_weibo_sina_auth.Location = new System.Drawing.Point(285, 423);
            this.btn_weibo_sina_auth.Name = "btn_weibo_sina_auth";
            this.btn_weibo_sina_auth.Size = new System.Drawing.Size(175, 50);
            this.btn_weibo_sina_auth.TabIndex = 10;
            this.btn_weibo_sina_auth.Tag = "9999";
            this.toolTip1.SetToolTip(this.btn_weibo_sina_auth, "新浪微博授权");
            this.btn_weibo_sina_auth.UseVisualStyleBackColor = true;
            this.btn_weibo_sina_auth.Visible = false;
            this.btn_weibo_sina_auth.Click += new System.EventHandler(this.btn_weibo_sina_auth_Click);
            // 
            // btn_weibo_qq_auth
            // 
            this.btn_weibo_qq_auth.BackgroundImage = global::Finder.Properties.Resources.qqweibo;
            this.btn_weibo_qq_auth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_weibo_qq_auth.Location = new System.Drawing.Point(14, 423);
            this.btn_weibo_qq_auth.Name = "btn_weibo_qq_auth";
            this.btn_weibo_qq_auth.Size = new System.Drawing.Size(175, 50);
            this.btn_weibo_qq_auth.TabIndex = 9;
            this.btn_weibo_qq_auth.Tag = "9999";
            this.toolTip1.SetToolTip(this.btn_weibo_qq_auth, "腾讯微博授权");
            this.btn_weibo_qq_auth.UseVisualStyleBackColor = true;
            this.btn_weibo_qq_auth.Visible = false;
            this.btn_weibo_qq_auth.Click += new System.EventHandler(this.btn_weibo_qq_auth_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(12, 504);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(127, 14);
            this.label15.TabIndex = 20;
            this.label15.Tag = "9999";
            this.label15.Text = "证据图片保存路径";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.groupBox5.Location = new System.Drawing.Point(-2, 527);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1139, 2);
            this.groupBox5.TabIndex = 20;
            this.groupBox5.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(11, 553);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(175, 14);
            this.label16.TabIndex = 20;
            this.label16.Tag = "9999";
            this.label16.Text = "请选择证据图片保存路径：";
            // 
            // tb_choosedimgpath
            // 
            this.tb_choosedimgpath.Enabled = false;
            this.tb_choosedimgpath.Location = new System.Drawing.Point(189, 550);
            this.tb_choosedimgpath.Name = "tb_choosedimgpath";
            this.tb_choosedimgpath.Size = new System.Drawing.Size(350, 21);
            this.tb_choosedimgpath.TabIndex = 21;
            // 
            // btn_saveimgpath
            // 
            this.btn_saveimgpath.Location = new System.Drawing.Point(578, 548);
            this.btn_saveimgpath.Name = "btn_saveimgpath";
            this.btn_saveimgpath.Size = new System.Drawing.Size(75, 23);
            this.btn_saveimgpath.TabIndex = 22;
            this.btn_saveimgpath.Text = "保  存";
            this.btn_saveimgpath.UseVisualStyleBackColor = true;
            this.btn_saveimgpath.Click += new System.EventHandler(this.btn_saveimgpath_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "请选择留存证据的图片保存路径";
            // 
            // lbsmsCX
            // 
            this.lbsmsCX.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbsmsCX.AutoSize = true;
            this.lbsmsCX.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbsmsCX.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbsmsCX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(102)))), ((int)(((byte)(0)))));
            this.lbsmsCX.Location = new System.Drawing.Point(6, 9);
            this.lbsmsCX.Name = "lbsmsCX";
            this.lbsmsCX.Size = new System.Drawing.Size(83, 12);
            this.lbsmsCX.TabIndex = 24;
            this.lbsmsCX.Tag = "9999";
            this.lbsmsCX.Text = "报警历史查询";
            this.lbsmsCX.Click += new System.EventHandler(this.lbsmsCX_Click);
            // 
            // lbsoundCX
            // 
            this.lbsoundCX.AutoSize = true;
            this.lbsoundCX.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbsoundCX.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbsoundCX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(102)))), ((int)(((byte)(0)))));
            this.lbsoundCX.Location = new System.Drawing.Point(89, 6);
            this.lbsoundCX.Name = "lbsoundCX";
            this.lbsoundCX.Size = new System.Drawing.Size(83, 12);
            this.lbsoundCX.TabIndex = 25;
            this.lbsoundCX.Tag = "9999";
            this.lbsoundCX.Text = "报警历史查询";
            this.lbsoundCX.Click += new System.EventHandler(this.lbsoundCX_Click);
            // 
            // btn_query_smscount
            // 
            this.btn_query_smscount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_query_smscount.Location = new System.Drawing.Point(91, 3);
            this.btn_query_smscount.Name = "btn_query_smscount";
            this.btn_query_smscount.Size = new System.Drawing.Size(103, 23);
            this.btn_query_smscount.TabIndex = 27;
            this.btn_query_smscount.Text = "查询剩余短信量";
            this.btn_query_smscount.UseVisualStyleBackColor = true;
            this.btn_query_smscount.Visible = false;
            this.btn_query_smscount.Click += new System.EventHandler(this.btn_query_smscount_Click);
            // 
            // lb_show_shengyusmscount
            // 
            this.lb_show_shengyusmscount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lb_show_shengyusmscount.AutoSize = true;
            this.lb_show_shengyusmscount.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_show_shengyusmscount.ForeColor = System.Drawing.Color.Blue;
            this.lb_show_shengyusmscount.Location = new System.Drawing.Point(202, 7);
            this.lb_show_shengyusmscount.Name = "lb_show_shengyusmscount";
            this.lb_show_shengyusmscount.Size = new System.Drawing.Size(0, 14);
            this.lb_show_shengyusmscount.TabIndex = 28;
            // 
            // dgv1
            // 
            this.dgv1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv1.BackgroundColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv1.Location = new System.Drawing.Point(6, 30);
            this.dgv1.Name = "dgv1";
            this.dgv1.RowTemplate.Height = 23;
            this.dgv1.Size = new System.Drawing.Size(970, 238);
            this.dgv1.TabIndex = 30;
            this.dgv1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv1_CellContentClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.ItemSize = new System.Drawing.Size(84, 25);
            this.tabControl1.Location = new System.Drawing.Point(13, 56);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(990, 303);
            this.tabControl1.TabIndex = 31;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.dgv1);
            this.tabPage1.Controls.Add(this.label18);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(982, 270);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "短信报警设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel1.Controls.Add(this.lbsmsCX);
            this.panel1.Controls.Add(this.lb_show_shengyusmscount);
            this.panel1.Controls.Add(this.btn_query_smscount);
            this.panel1.Location = new System.Drawing.Point(626, -1);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(350, 28);
            this.panel1.TabIndex = 32;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(6, 10);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(53, 12);
            this.label18.TabIndex = 29;
            this.label18.Text = "填写说明";
            this.label18.Click += new System.EventHandler(this.label18_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Controls.Add(this.dgv2);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage2.Size = new System.Drawing.Size(982, 270);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "声音报警设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel2.Controls.Add(this.lbsoundCX);
            this.panel2.Location = new System.Drawing.Point(776, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 25);
            this.panel2.TabIndex = 27;
            // 
            // dgv2
            // 
            this.dgv2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv2.BackgroundColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dgv2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv2.Location = new System.Drawing.Point(6, 31);
            this.dgv2.Name = "dgv2";
            this.dgv2.RowTemplate.Height = 23;
            this.dgv2.Size = new System.Drawing.Size(970, 238);
            this.dgv2.TabIndex = 26;
            this.dgv2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv2_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 20;
            this.label1.Tag = "9999";
            this.label1.Text = "报警设置";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.groupBox4.Location = new System.Drawing.Point(-2, 44);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1140, 2);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            // 
            // pb_choosepath
            // 
            this.pb_choosepath.Image = global::Finder.Properties.Resources.openfolder;
            this.pb_choosepath.Location = new System.Drawing.Point(542, 551);
            this.pb_choosepath.Name = "pb_choosepath";
            this.pb_choosepath.Size = new System.Drawing.Size(19, 19);
            this.pb_choosepath.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_choosepath.TabIndex = 23;
            this.pb_choosepath.TabStop = false;
            this.pb_choosepath.Click += new System.EventHandler(this.pb_choosepath_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(190, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 12);
            this.label2.TabIndex = 32;
            this.label2.Text = "注意:蓝色行为报警条件已设置";
            // 
            // SystemSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1024, 686);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pb_choosepath);
            this.Controls.Add(this.btn_saveimgpath);
            this.Controls.Add(this.tb_choosedimgpath);
            this.Controls.Add(this.btn_weibo_sina_auth);
            this.Controls.Add(this.btn_weibo_qq_auth);
            this.Controls.Add(this.btn_updatepwd);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb_newpwd2);
            this.Controls.Add(this.tb_newpwd1);
            this.Controls.Add(this.tb_oldpwd);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SystemSetting";
            this.Text = "SystemSetting";
            this.Load += new System.EventHandler(this.SystemSetting_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SystemSetting_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_choosepath)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_oldpwd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_newpwd1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_newpwd2;
        private System.Windows.Forms.Button btn_updatepwd;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btn_weibo_qq_auth;
        private System.Windows.Forms.Button btn_weibo_sina_auth;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tb_choosedimgpath;
        private System.Windows.Forms.Button btn_saveimgpath;
        private System.Windows.Forms.PictureBox pb_choosepath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label lbsmsCX;
        private System.Windows.Forms.Label lbsoundCX;
        private System.Windows.Forms.Button btn_query_smscount;
        private System.Windows.Forms.Label lb_show_shengyusmscount;
        private System.Windows.Forms.DataGridView dgv1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgv2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label18;
    }
}