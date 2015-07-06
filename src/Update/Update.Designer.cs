namespace Update
{
    partial class Update
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Update));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lb_checkstate = new System.Windows.Forms.ToolStripStatusLabel();
            this.pb_check = new System.Windows.Forms.ToolStripProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_checkresult = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_beginupdate = new System.Windows.Forms.Button();
            this.pb_min = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pb_close = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_close)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // skinEngine1
            // 
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = null;
            this.skinEngine1.SkinStreamMain = ((System.IO.Stream)(resources.GetObject("skinEngine1.SkinStreamMain")));
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lb_checkstate,
            this.pb_check});
            this.statusStrip1.Location = new System.Drawing.Point(0, 446);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(570, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Tag = "";
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lb_checkstate
            // 
            this.lb_checkstate.BackColor = System.Drawing.Color.Transparent;
            this.lb_checkstate.ForeColor = System.Drawing.Color.Black;
            this.lb_checkstate.Name = "lb_checkstate";
            this.lb_checkstate.Size = new System.Drawing.Size(41, 17);
            this.lb_checkstate.Tag = "9999";
            this.lb_checkstate.Text = "状态：";
            // 
            // pb_check
            // 
            this.pb_check.Name = "pb_check";
            this.pb_check.Size = new System.Drawing.Size(100, 16);
            this.pb_check.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(191, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 16);
            this.label1.TabIndex = 1;
            this.label1.Tag = "9999";
            this.label1.Text = "正义东方舆情分析系统更新";
            // 
            // lb_checkresult
            // 
            this.lb_checkresult.AutoSize = true;
            this.lb_checkresult.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_checkresult.ForeColor = System.Drawing.Color.White;
            this.lb_checkresult.Location = new System.Drawing.Point(192, 234);
            this.lb_checkresult.Name = "lb_checkresult";
            this.lb_checkresult.Size = new System.Drawing.Size(0, 16);
            this.lb_checkresult.TabIndex = 3;
            this.lb_checkresult.Tag = "9999";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.dataGridView1.Enabled = false;
            this.dataGridView1.Location = new System.Drawing.Point(13, 209);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(545, 176);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.Visible = false;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "文件名称";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "文件版本";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "文件大小";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "已完成更新";
            this.Column4.Name = "Column4";
            // 
            // btn_beginupdate
            // 
            this.btn_beginupdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_beginupdate.Location = new System.Drawing.Point(242, 409);
            this.btn_beginupdate.Name = "btn_beginupdate";
            this.btn_beginupdate.Size = new System.Drawing.Size(89, 23);
            this.btn_beginupdate.TabIndex = 5;
            this.btn_beginupdate.Text = "开始下载更新";
            this.btn_beginupdate.UseVisualStyleBackColor = true;
            this.btn_beginupdate.Click += new System.EventHandler(this.btn_beginupdate_Click);
            // 
            // pb_min
            // 
            this.pb_min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_min.Image = global::Update.Properties.Resources.m;
            this.pb_min.Location = new System.Drawing.Point(503, 1);
            this.pb_min.Name = "pb_min";
            this.pb_min.Size = new System.Drawing.Size(28, 20);
            this.pb_min.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pb_min.TabIndex = 6;
            this.pb_min.TabStop = false;
            this.pb_min.Tag = "9999";
            this.pb_min.Click += new System.EventHandler(this.pb_min_Click);
            this.pb_min.MouseEnter += new System.EventHandler(this.pb_min_MouseEnter);
            this.pb_min.MouseLeave += new System.EventHandler(this.pb_min_MouseLeave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Update.Properties.Resources.loading;
            this.pictureBox1.Location = new System.Drawing.Point(390, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // pb_close
            // 
            this.pb_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(34)))), ((int)(((byte)(17)))));
            this.pb_close.Image = global::Update.Properties.Resources.x;
            this.pb_close.Location = new System.Drawing.Point(537, 1);
            this.pb_close.Name = "pb_close";
            this.pb_close.Size = new System.Drawing.Size(28, 20);
            this.pb_close.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pb_close.TabIndex = 6;
            this.pb_close.TabStop = false;
            this.pb_close.Tag = "9999";
            this.pb_close.Click += new System.EventHandler(this.pb_close_Click);
            this.pb_close.MouseEnter += new System.EventHandler(this.pb_close_MouseEnter);
            this.pb_close.MouseLeave += new System.EventHandler(this.pb_close_MouseLeave);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Update.Properties.Resources.Update;
            this.pictureBox2.Location = new System.Drawing.Point(222, 58);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(128, 128);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // Update
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(570, 468);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pb_close);
            this.Controls.Add(this.pb_min);
            this.Controls.Add(this.btn_beginupdate);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lb_checkresult);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Update";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自动更新";
            this.Load += new System.EventHandler(this.Update_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_close)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sunisoft.IrisSkin.SkinEngine skinEngine1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lb_checkstate;
        private System.Windows.Forms.ToolStripProgressBar pb_check;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lb_checkresult;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_beginupdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.PictureBox pb_min;
        private System.Windows.Forms.PictureBox pb_close;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}