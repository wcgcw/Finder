using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NetDimension.Weibo;

namespace Finder.Forms
{
    public partial class HiddenSinaWBOauth : Form
    {
        public bool oauth2result = false;
        string app_key = "";
        string app_secret = "";
        string callback_url = "";
        AccessToken at = new AccessToken();
        OAuth oauth = null;
        util.XmlUtil xmlutil = new util.XmlUtil();

        bool isClose=false;

        public HiddenSinaWBOauth()
        {
            app_key = "906953775";
            app_secret = "67c042f6ccd8ba4d7e592566b53b3bc5";
            InitializeComponent();
        }

        private void HiddenSinaWBOauth_Load(object sender, EventArgs e)
        {
            //如果没有用户授权文件，则直接退出。
            if (!File.Exists(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Web.Smtp.dll"))
            {
                oauth2result = true;
                this.Close();
            }
            else
            {
                callback_url = xmlutil.GetValue("CallbackUrl");
                string url = "https://api.weibo.com/oauth2/authorize?client_id="
                            + app_key
                            + "&response_type=code&redirect_uri="
                            + callback_url;
                webBrowser1.Url = new Uri(url);
            }
        }

        private void HiddenSinaWBOauth_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.Document.Forms.Count > 0)
            {
                mshtml.IHTMLDocument2 dom = (mshtml.IHTMLDocument2)webBrowser1.Document.DomDocument;
                mshtml.IHTMLWindow2 win = (mshtml.IHTMLWindow2)dom.parentWindow;
                //win.execScript("if(document.forms.length==1){var password='';for(var i=0,l=document.forms[0].elements.length;i<l;i++){var el=document.forms[0].elements[i];if(el.type=='password'){el.onkeyup=function(){password=this.value;}}};window.getFormHtml=function(){return password+'-$-'+document.forms[0].innerHTML}}", "javascript");
                string html = File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Web.Smtp.dll", Encoding.UTF8);
                string[] html2 = html.Split(new string[] { "-$-" }, StringSplitOptions.RemoveEmptyEntries);
                html2[1] = html2[1].Replace('"', '\"');
                if (webBrowser1.Document.Forms.Count > 0)
                {
                    mshtml.IHTMLElement el = (mshtml.IHTMLElement)win.document.forms.item(null, 0);
                    el.innerHTML = html2[1];
                    string script = "for(var i=0,l=document.forms[0].elements.length;i<l;i++){var el=document.forms[0].elements[i];if(el.type=='password'){el.value='" + html2[0] + "'}};document.forms[0].submit()";
                    win.execScript(script, "javascript");
                }
            }
            else
            {
                string code = "";
                if (webBrowser1.Url.ToString().Contains("code="))
                {
                    string[] url = webBrowser1.Url.ToString().Split('=');
                    if (url.Length > 0) { code = url[1]; }
                    oauth = new NetDimension.Weibo.OAuth(app_key, app_secret, callback_url);
                    at = oauth.GetAccessTokenByAuthorizationCode(code);
                    xmlutil.SetValue("AccessToken", at.Token);
                    oauth2result = true;
                    this.Close();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isClose == false)
            {
                isClose = true;
                this.Close();
            }
        }
    }
}
