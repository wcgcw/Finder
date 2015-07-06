namespace Finder
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel_main = new System.Windows.Forms.Panel();
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.miniIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.timer_currentDate = new System.Windows.Forms.Timer(this.components);
            this.timer_sinaoauth2 = new System.Windows.Forms.Timer(this.components);
            this.panelBG = new System.Windows.Forms.Panel();
            this.pictureBox12 = new System.Windows.Forms.PictureBox();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelLabel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lb_CheckUpdate = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pb_close = new System.Windows.Forms.PictureBox();
            this.pb_min = new System.Windows.Forms.PictureBox();
            this.uC_Menu3 = new Finder.UserControles.UC_Menu();
            this.uC_Menu8 = new Finder.UserControles.UC_Menu();
            this.uC_Menu7 = new Finder.UserControles.UC_Menu();
            this.uC_Menu2 = new Finder.UserControles.UC_Menu();
            this.uC_Menu13 = new Finder.UserControles.UC_Menu();
            this.uC_Menu12 = new Finder.UserControles.UC_Menu();
            this.uC_Menu11 = new Finder.UserControles.UC_Menu();
            this.uC_Menu10 = new Finder.UserControles.UC_Menu();
            this.uC_Menu6 = new Finder.UserControles.UC_Menu();
            this.uC_Menu9 = new Finder.UserControles.UC_Menu();
            this.uC_Menu1 = new Finder.UserControles.UC_Menu();
            this.panelBG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_close)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_min)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_main
            // 
            this.panel_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_main.BackColor = System.Drawing.Color.White;
            this.panel_main.Location = new System.Drawing.Point(15, 15);
            this.panel_main.Name = "panel_main";
            this.panel_main.Size = new System.Drawing.Size(1001, 690);
            this.panel_main.TabIndex = 1;
            this.panel_main.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.panel_main_ControlAdded);
            // 
            // skinEngine1
            // 
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = null;
            this.skinEngine1.SkinStreamMain = ((System.IO.Stream)(resources.GetObject("skinEngine1.SkinStreamMain")));
            // 
            // miniIcon
            // 
            this.miniIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("miniIcon.Icon")));
            this.miniIcon.Text = "正义东方舆情监控系统";
            this.miniIcon.Visible = true;
            this.miniIcon.Click += new System.EventHandler(this.miniIcon_Click);
            // 
            // panelBG
            // 
            this.panelBG.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBG.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(0)))), ((int)(((byte)(7)))));
            this.panelBG.Controls.Add(this.pictureBox12);
            this.panelBG.Controls.Add(this.pictureBox11);
            this.panelBG.Controls.Add(this.pictureBox10);
            this.panelBG.Controls.Add(this.pictureBox9);
            this.panelBG.Controls.Add(this.pictureBox6);
            this.panelBG.Controls.Add(this.panel_main);
            this.panelBG.Controls.Add(this.pictureBox7);
            this.panelBG.Controls.Add(this.pictureBox8);
            this.panelBG.Controls.Add(this.pictureBox5);
            this.panelBG.Location = new System.Drawing.Point(185, 29);
            this.panelBG.Name = "panelBG";
            this.panelBG.Size = new System.Drawing.Size(1031, 720);
            this.panelBG.TabIndex = 15;
            // 
            // pictureBox12
            // 
            this.pictureBox12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox12.BackgroundImage = global::Finder.Properties.Resources.mb;
            this.pictureBox12.Location = new System.Drawing.Point(15, 705);
            this.pictureBox12.Name = "pictureBox12";
            this.pictureBox12.Size = new System.Drawing.Size(1001, 15);
            this.pictureBox12.TabIndex = 18;
            this.pictureBox12.TabStop = false;
            // 
            // pictureBox11
            // 
            this.pictureBox11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox11.BackgroundImage = global::Finder.Properties.Resources.mt;
            this.pictureBox11.Location = new System.Drawing.Point(15, 0);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(1001, 15);
            this.pictureBox11.TabIndex = 16;
            this.pictureBox11.TabStop = false;
            // 
            // pictureBox10
            // 
            this.pictureBox10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox10.BackgroundImage = global::Finder.Properties.Resources.rm;
            this.pictureBox10.Location = new System.Drawing.Point(1016, 15);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(15, 690);
            this.pictureBox10.TabIndex = 17;
            this.pictureBox10.TabStop = false;
            // 
            // pictureBox9
            // 
            this.pictureBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox9.BackgroundImage = global::Finder.Properties.Resources.lm;
            this.pictureBox9.Location = new System.Drawing.Point(0, 15);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(15, 690);
            this.pictureBox9.TabIndex = 16;
            this.pictureBox9.TabStop = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox6.BackgroundImage = global::Finder.Properties.Resources.rb;
            this.pictureBox6.Location = new System.Drawing.Point(1016, 705);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(15, 15);
            this.pictureBox6.TabIndex = 12;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox7
            // 
            this.pictureBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox7.BackgroundImage = global::Finder.Properties.Resources.lb;
            this.pictureBox7.Location = new System.Drawing.Point(0, 705);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(15, 15);
            this.pictureBox7.TabIndex = 13;
            this.pictureBox7.TabStop = false;
            // 
            // pictureBox8
            // 
            this.pictureBox8.BackgroundImage = global::Finder.Properties.Resources.lt;
            this.pictureBox8.Location = new System.Drawing.Point(0, 0);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(15, 15);
            this.pictureBox8.TabIndex = 14;
            this.pictureBox8.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox5.BackgroundImage = global::Finder.Properties.Resources.rt;
            this.pictureBox5.Location = new System.Drawing.Point(1016, 0);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(15, 15);
            this.pictureBox5.TabIndex = 11;
            this.pictureBox5.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(0)))), ((int)(((byte)(7)))));
            this.panel1.BackgroundImage = global::Finder.Properties.Resources.menu_back;
            this.panel1.Controls.Add(this.uC_Menu3);
            this.panel1.Controls.Add(this.uC_Menu8);
            this.panel1.Controls.Add(this.uC_Menu7);
            this.panel1.Controls.Add(this.uC_Menu2);
            this.panel1.Controls.Add(this.uC_Menu13);
            this.panel1.Controls.Add(this.uC_Menu12);
            this.panel1.Controls.Add(this.uC_Menu11);
            this.panel1.Controls.Add(this.uC_Menu10);
            this.panel1.Controls.Add(this.uC_Menu6);
            this.panel1.Controls.Add(this.uC_Menu9);
            this.panel1.Controls.Add(this.uC_Menu1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.panelLabel);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(180, 785);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::Finder.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(123, 131);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panelLabel
            // 
            this.panelLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelLabel.BackColor = System.Drawing.Color.Transparent;
            this.panelLabel.BackgroundImage = global::Finder.Properties.Resources.bg;
            this.panelLabel.Controls.Add(this.label1);
            this.panelLabel.Controls.Add(this.label3);
            this.panelLabel.Controls.Add(this.lb_CheckUpdate);
            this.panelLabel.Controls.Add(this.label2);
            this.panelLabel.Location = new System.Drawing.Point(0, 594);
            this.panelLabel.Name = "panelLabel";
            this.panelLabel.Size = new System.Drawing.Size(450, 186);
            this.panelLabel.TabIndex = 0;
            this.panelLabel.Tag = "9999";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 14;
            this.label1.Tag = "9999";
            this.label1.Text = "正义东方舆情监控系统";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(4, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 15);
            this.label3.TabIndex = 14;
            this.label3.Tag = "9999";
            this.label3.Text = "服务电话:010-59795128";
            // 
            // lb_CheckUpdate
            // 
            this.lb_CheckUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lb_CheckUpdate.AutoSize = true;
            this.lb_CheckUpdate.BackColor = System.Drawing.Color.Transparent;
            this.lb_CheckUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lb_CheckUpdate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_CheckUpdate.LinkColor = System.Drawing.Color.White;
            this.lb_CheckUpdate.Location = new System.Drawing.Point(108, 145);
            this.lb_CheckUpdate.Name = "lb_CheckUpdate";
            this.lb_CheckUpdate.Size = new System.Drawing.Size(53, 12);
            this.lb_CheckUpdate.TabIndex = 16;
            this.lb_CheckUpdate.TabStop = true;
            this.lb_CheckUpdate.Tag = "9999";
            this.lb_CheckUpdate.Text = "检查更新";
            this.lb_CheckUpdate.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(5, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 15;
            this.label2.Tag = "9999";
            this.label2.Text = "Ver 3.0.1";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox3.Image = global::Finder.Properties.Resources.bg;
            this.pictureBox3.Location = new System.Drawing.Point(0, 594);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(950, 186);
            this.pictureBox3.TabIndex = 10;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = global::Finder.Properties.Resources.s1;
            this.pictureBox2.Location = new System.Drawing.Point(1154, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(28, 20);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Tag = "9999";
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            this.pictureBox2.MouseEnter += new System.EventHandler(this.pictureBox2_MouseEnter);
            this.pictureBox2.MouseLeave += new System.EventHandler(this.pictureBox2_MouseLeave);
            // 
            // pb_close
            // 
            this.pb_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(34)))), ((int)(((byte)(17)))));
            this.pb_close.Image = global::Finder.Properties.Resources.x;
            this.pb_close.Location = new System.Drawing.Point(1184, 3);
            this.pb_close.Name = "pb_close";
            this.pb_close.Size = new System.Drawing.Size(32, 20);
            this.pb_close.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pb_close.TabIndex = 8;
            this.pb_close.TabStop = false;
            this.pb_close.Tag = "9999";
            this.pb_close.Click += new System.EventHandler(this.pb_close_Click);
            this.pb_close.MouseEnter += new System.EventHandler(this.pb_close_MouseEnter);
            this.pb_close.MouseLeave += new System.EventHandler(this.pb_close_MouseLeave);
            // 
            // pb_min
            // 
            this.pb_min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_min.Image = global::Finder.Properties.Resources.m;
            this.pb_min.Location = new System.Drawing.Point(1124, 3);
            this.pb_min.Name = "pb_min";
            this.pb_min.Size = new System.Drawing.Size(28, 20);
            this.pb_min.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pb_min.TabIndex = 7;
            this.pb_min.TabStop = false;
            this.pb_min.Tag = "9999";
            this.pb_min.Click += new System.EventHandler(this.pb_min_Click);
            this.pb_min.MouseEnter += new System.EventHandler(this.pb_min_MouseEnter);
            this.pb_min.MouseLeave += new System.EventHandler(this.pb_min_MouseLeave);
            // 
            // uC_Menu3
            // 
            this.uC_Menu3.BackColor = System.Drawing.Color.Transparent;
            this.uC_Menu3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uC_Menu3.BackgroundImage")));
            this.uC_Menu3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uC_Menu3.Font = new System.Drawing.Font("宋体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_Menu3.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.uC_Menu3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.uC_Menu3.Location = new System.Drawing.Point(12, 342);
            this.uC_Menu3.Margin = new System.Windows.Forms.Padding(2);
            this.uC_Menu3.Name = "uC_Menu3";
            this.uC_Menu3.PicSource = global::Finder.Properties.Resources.newa;
            this.uC_Menu3.Size = new System.Drawing.Size(150, 40);
            this.uC_Menu3.TabIndex = 12;
            this.uC_Menu3.Txt = "常规舆情监控";
            this.uC_Menu3.UC_Click += new System.EventHandler(this.uC_Menu3_UC_Click_1);
            // 
            // uC_Menu8
            // 
            this.uC_Menu8.BackColor = System.Drawing.Color.Transparent;
            this.uC_Menu8.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uC_Menu8.BackgroundImage")));
            this.uC_Menu8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uC_Menu8.Font = new System.Drawing.Font("宋体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_Menu8.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.uC_Menu8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.uC_Menu8.Location = new System.Drawing.Point(12, 260);
            this.uC_Menu8.Margin = new System.Windows.Forms.Padding(2);
            this.uC_Menu8.Name = "uC_Menu8";
            this.uC_Menu8.PicSource = global::Finder.Properties.Resources.bar_chart;
            this.uC_Menu8.Size = new System.Drawing.Size(150, 40);
            this.uC_Menu8.TabIndex = 11;
            this.uC_Menu8.Txt = "舆情分析";
            this.uC_Menu8.UC_Click += new System.EventHandler(this.uC_Menu8_UC_Click);
            // 
            // uC_Menu7
            // 
            this.uC_Menu7.BackColor = System.Drawing.Color.Transparent;
            this.uC_Menu7.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uC_Menu7.BackgroundImage")));
            this.uC_Menu7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uC_Menu7.Font = new System.Drawing.Font("宋体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_Menu7.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.uC_Menu7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.uC_Menu7.Location = new System.Drawing.Point(12, 547);
            this.uC_Menu7.Margin = new System.Windows.Forms.Padding(2);
            this.uC_Menu7.Name = "uC_Menu7";
            this.uC_Menu7.PicSource = global::Finder.Properties.Resources.tools;
            this.uC_Menu7.Size = new System.Drawing.Size(150, 40);
            this.uC_Menu7.TabIndex = 6;
            this.uC_Menu7.Txt = "系统设置";
            this.uC_Menu7.UC_Click += new System.EventHandler(this.uC_Menu7_Click);
            // 
            // uC_Menu2
            // 
            this.uC_Menu2.BackColor = System.Drawing.Color.Transparent;
            this.uC_Menu2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uC_Menu2.BackgroundImage")));
            this.uC_Menu2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uC_Menu2.Font = new System.Drawing.Font("宋体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_Menu2.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.uC_Menu2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.uC_Menu2.Location = new System.Drawing.Point(12, 301);
            this.uC_Menu2.Margin = new System.Windows.Forms.Padding(2);
            this.uC_Menu2.Name = "uC_Menu2";
            this.uC_Menu2.PicSource = global::Finder.Properties.Resources.report_word;
            this.uC_Menu2.Size = new System.Drawing.Size(150, 40);
            this.uC_Menu2.TabIndex = 1;
            this.uC_Menu2.Txt = "舆情报告";
            this.uC_Menu2.UC_Click += new System.EventHandler(this.uC_Menu2_UC_Click);
            // 
            // uC_Menu13
            // 
            this.uC_Menu13.BackColor = System.Drawing.Color.Transparent;
            this.uC_Menu13.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uC_Menu13.BackgroundImage")));
            this.uC_Menu13.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uC_Menu13.Font = new System.Drawing.Font("宋体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_Menu13.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.uC_Menu13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.uC_Menu13.Location = new System.Drawing.Point(12, 506);
            this.uC_Menu13.Margin = new System.Windows.Forms.Padding(2);
            this.uC_Menu13.Name = "uC_Menu13";
            this.uC_Menu13.PicSource = global::Finder.Properties.Resources.ccon;
            this.uC_Menu13.Size = new System.Drawing.Size(150, 40);
            this.uC_Menu13.TabIndex = 3;
            this.uC_Menu13.Txt = "控制中心";
            this.uC_Menu13.UC_Click += new System.EventHandler(this.uC_Menu13_UC_Click);
            // 
            // uC_Menu12
            // 
            this.uC_Menu12.BackColor = System.Drawing.Color.Transparent;
            this.uC_Menu12.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uC_Menu12.BackgroundImage")));
            this.uC_Menu12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uC_Menu12.Font = new System.Drawing.Font("宋体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_Menu12.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.uC_Menu12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.uC_Menu12.Location = new System.Drawing.Point(12, 465);
            this.uC_Menu12.Margin = new System.Windows.Forms.Padding(2);
            this.uC_Menu12.Name = "uC_Menu12";
            this.uC_Menu12.PicSource = global::Finder.Properties.Resources.newa;
            this.uC_Menu12.Size = new System.Drawing.Size(150, 40);
            this.uC_Menu12.TabIndex = 3;
            this.uC_Menu12.Txt = "突发舆情监控";
            this.uC_Menu12.UC_Click += new System.EventHandler(this.uC_Menu12_UC_Click);
            // 
            // uC_Menu11
            // 
            this.uC_Menu11.BackColor = System.Drawing.Color.Transparent;
            this.uC_Menu11.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uC_Menu11.BackgroundImage")));
            this.uC_Menu11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uC_Menu11.Font = new System.Drawing.Font("宋体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_Menu11.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.uC_Menu11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.uC_Menu11.Location = new System.Drawing.Point(12, 424);
            this.uC_Menu11.Margin = new System.Windows.Forms.Padding(2);
            this.uC_Menu11.Name = "uC_Menu11";
            this.uC_Menu11.PicSource = global::Finder.Properties.Resources.newa;
            this.uC_Menu11.Size = new System.Drawing.Size(150, 40);
            this.uC_Menu11.TabIndex = 3;
            this.uC_Menu11.Txt = "重点舆情监控";
            this.uC_Menu11.UC_Click += new System.EventHandler(this.uC_Menu11_UC_Click);
            // 
            // uC_Menu10
            // 
            this.uC_Menu10.BackColor = System.Drawing.Color.Transparent;
            this.uC_Menu10.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uC_Menu10.BackgroundImage")));
            this.uC_Menu10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uC_Menu10.Font = new System.Drawing.Font("宋体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_Menu10.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.uC_Menu10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.uC_Menu10.Location = new System.Drawing.Point(12, 383);
            this.uC_Menu10.Margin = new System.Windows.Forms.Padding(2);
            this.uC_Menu10.Name = "uC_Menu10";
            this.uC_Menu10.PicSource = global::Finder.Properties.Resources.newa;
            this.uC_Menu10.Size = new System.Drawing.Size(150, 40);
            this.uC_Menu10.TabIndex = 3;
            this.uC_Menu10.Txt = "敏感舆情监控";
            this.uC_Menu10.UC_Click += new System.EventHandler(this.uC_Menu10_UC_Click);
            // 
            // uC_Menu6
            // 
            this.uC_Menu6.BackColor = System.Drawing.Color.Transparent;
            this.uC_Menu6.BackgroundImage = global::Finder.Properties.Resources.a;
            this.uC_Menu6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uC_Menu6.Font = new System.Drawing.Font("宋体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_Menu6.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.uC_Menu6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.uC_Menu6.Location = new System.Drawing.Point(12, 219);
            this.uC_Menu6.Margin = new System.Windows.Forms.Padding(2);
            this.uC_Menu6.Name = "uC_Menu6";
            this.uC_Menu6.PicSource = global::Finder.Properties.Resources.fel;
            this.uC_Menu6.Size = new System.Drawing.Size(150, 40);
            this.uC_Menu6.TabIndex = 5;
            this.uC_Menu6.Txt = "舆情提取";
            this.uC_Menu6.UC_Click += new System.EventHandler(this.uC_Menu6_UC_Click);
            // 
            // uC_Menu9
            // 
            this.uC_Menu9.BackColor = System.Drawing.Color.Transparent;
            this.uC_Menu9.BackgroundImage = global::Finder.Properties.Resources.a;
            this.uC_Menu9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uC_Menu9.Font = new System.Drawing.Font("宋体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_Menu9.FontColor = System.Drawing.Color.White;
            this.uC_Menu9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.uC_Menu9.Location = new System.Drawing.Point(12, 137);
            this.uC_Menu9.Margin = new System.Windows.Forms.Padding(2);
            this.uC_Menu9.Name = "uC_Menu9";
            this.uC_Menu9.PicSource = global::Finder.Properties.Resources.start;
            this.uC_Menu9.Size = new System.Drawing.Size(150, 40);
            this.uC_Menu9.TabIndex = 1;
            this.uC_Menu9.Txt = "首页";
            this.uC_Menu9.UC_Click += new System.EventHandler(this.uC_Menu9_UC_Click);
            // 
            // uC_Menu1
            // 
            this.uC_Menu1.BackColor = System.Drawing.Color.Transparent;
            this.uC_Menu1.BackgroundImage = global::Finder.Properties.Resources.a;
            this.uC_Menu1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uC_Menu1.Font = new System.Drawing.Font("宋体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_Menu1.FontColor = System.Drawing.Color.White;
            this.uC_Menu1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.uC_Menu1.Location = new System.Drawing.Point(12, 178);
            this.uC_Menu1.Margin = new System.Windows.Forms.Padding(2);
            this.uC_Menu1.Name = "uC_Menu1";
            this.uC_Menu1.PicSource = global::Finder.Properties.Resources.tel;
            this.uC_Menu1.Size = new System.Drawing.Size(150, 40);
            this.uC_Menu1.TabIndex = 1;
            this.uC_Menu1.Txt = "舆情搜索";
            this.uC_Menu1.UC_Click += new System.EventHandler(this.uC_Menu1_UC_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(0)))), ((int)(((byte)(7)))));
            this.ClientSize = new System.Drawing.Size(1221, 780);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelBG);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pb_close);
            this.Controls.Add(this.pb_min);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1100, 720);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "正义东方网络舆情监控系统";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DoubleClick += new System.EventHandler(this.MainForm_DoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.panelBG.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelLabel.ResumeLayout(false);
            this.panelLabel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_close)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_min)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private UserControles.UC_Menu uC_Menu1;
        private UserControles.UC_Menu uC_Menu7;
        private UserControles.UC_Menu uC_Menu2;
        private UserControles.UC_Menu uC_Menu6;
        private System.Windows.Forms.Panel panel_main;
        private Sunisoft.IrisSkin.SkinEngine skinEngine1;
        private System.Windows.Forms.PictureBox pb_close;
        private System.Windows.Forms.PictureBox pb_min;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.NotifyIcon miniIcon;
        private UserControles.UC_Menu uC_Menu8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer_currentDate;
        private System.Windows.Forms.Timer timer_sinaoauth2;
        private System.Windows.Forms.LinkLabel lb_CheckUpdate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.Panel panelBG;
        private System.Windows.Forms.PictureBox pictureBox12;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.PictureBox pictureBox10;
        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.Panel panelLabel;
        private UserControles.UC_Menu uC_Menu13;
        private UserControles.UC_Menu uC_Menu12;
        private UserControles.UC_Menu uC_Menu11;
        private UserControles.UC_Menu uC_Menu10;
        private UserControles.UC_Menu uC_Menu9;
        private UserControles.UC_Menu uC_Menu3;
    }
}