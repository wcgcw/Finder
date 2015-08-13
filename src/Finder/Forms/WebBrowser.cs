using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Finder.Forms
{
    public partial class WebBrowser : Form
    {
        public WebBrowser()
        {
            InitializeComponent();
        }

        public string _title { get; set; }
        public string _link { get; set; }
        public WebBrowser(string title, string link)
        {
            InitializeComponent();

            this._title = title;
            this._link = link;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WebBrowser_Load(object sender, EventArgs e)
        {
            label2.Text = "";
            if (!string.IsNullOrEmpty(this._title))
            {
                label2.Text = this._title;
            }

            if (!string.IsNullOrEmpty(this._link))
            {
                webBrowser1.Navigate(this._link);
            }
        }    

    }
}
