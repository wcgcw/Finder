using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Finder.UserControles
{
    public partial class UC_Menu : UserControl
    {
        [Category("Appearance"), Browsable(true), Description("要显示的文字")]
        public string Txt
        {
            set { label1.Text = value; }
            get { return label1.Text; }
        }
        [Category("Appearance"),Browsable(true),Description("要显示的照片")]
        public Image PicSource
        {
            set { pictureBox1.Image = value; }
            get { return pictureBox1.Image; }
        }
        [Category("Appearance"), Browsable(true), Description("字体颜色")]
        public Color FontColor
        {
            set { label1.ForeColor = value; }
            get { return label1.ForeColor; }
        }
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler UC_Click;

        private Color bc;

        public UC_Menu()
        {
            InitializeComponent();
        }

        private void UC_Menu_Click(object sender, EventArgs e)
        {
            if (UC_Click != null) UC_Click(this, e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (UC_Click != null) UC_Click(this, e);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (UC_Click != null) UC_Click(this, e);
        }
    }
}
