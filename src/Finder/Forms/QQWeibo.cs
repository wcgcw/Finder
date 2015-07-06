using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Finder
{
    public partial class QQWeibo : Form
    {
        util.XmlUtil xmlUtil = new util.XmlUtil();
        string appKey = "";
        string appSecret = "";
        string accessToken;// = System.Configuration.ConfigurationSettings.AppSettings["QQWbAccessToken"];
        string openId;// = System.Configuration.ConfigurationSettings.AppSettings["QQWbOpenId"];
        string callBackUrl;// = System.Configuration.ConfigurationSettings.AppSettings["QQWbCallbackUrl"];
        string openKey;// = System.Configuration.ConfigurationSettings.AppSettings["QQWbOpenKey"];
        string RefreshToken;// = System.Configuration.ConfigurationSettings.AppSettings["QQWbRefreshToken"];
        string expire_in;// = System.Configuration.ConfigurationSettings.AppSettings["QQWbExpire_in"];
        string fetchDateStr;// = System.Configuration.ConfigurationSettings.AppSettings["QQWbFetchAccessTokenDate"];
        string currentUrl;
        public QQWeibo()
        {
            InitializeComponent();
            appKey = "801426797";
            appSecret = "0e9382190a5f764b57e09c15cda96e09";
            accessToken = xmlUtil.GetValue("QQWbAccessToken");
            openId = xmlUtil.GetValue("QQWbOpenId");
            callBackUrl = xmlUtil.GetValue("QQWbCallbackUrl");
            openKey = xmlUtil.GetValue("QQWbOpenKey");
            RefreshToken = xmlUtil.GetValue("QQWbRefreshToken");
            expire_in = xmlUtil.GetValue("QQWbExpire_in");
            fetchDateStr = xmlUtil.GetValue("QQWbFetchAccessTokenDate");
        }

        private void QQWeibo_Load(object sender, EventArgs e)
        {
            webBrowser1.IsAccessible = true;
            if (!string.IsNullOrEmpty(appKey) && !string.IsNullOrEmpty(appSecret) && !string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(openKey) && !string.IsNullOrEmpty(RefreshToken) && !string.IsNullOrEmpty(expire_in) && !string.IsNullOrEmpty(fetchDateStr))
            {
                webBrowser1.Url = new Uri(callBackUrl);
            }
            else
            {
                webBrowser1.Url = new Uri("https://open.t.qq.com/cgi-bin/oauth2/authorize?client_id="+appKey+"&response_type=token&redirect_uri="+callBackUrl);
            }
        }

        private void QQWeibo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (currentUrl != null)
            {
                string[] urlpars = currentUrl.Split(new char[] { '#', '&', '=' });
                if (urlpars.Length > 11)
                {
                    accessToken = urlpars[2];
                    openId = urlpars[6];
                    openKey = urlpars[8];
                    RefreshToken = urlpars[10];
                    expire_in = urlpars[4];
                    xmlUtil.SetValue("QQWbAccessToken", accessToken);
                    xmlUtil.SetValue("QQWbOpenId", openId);
                    xmlUtil.SetValue("QQWbOpenKey", openKey);
                    xmlUtil.SetValue("QQWbRefreshToken", RefreshToken);
                    xmlUtil.SetValue("QQWbExpire_in", expire_in);
                    xmlUtil.SetValue("QQWbFetchAccessTokenDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else if (!string.IsNullOrEmpty(appKey) && !string.IsNullOrEmpty(appSecret) && !string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(openKey) && !string.IsNullOrEmpty(RefreshToken))
                {

                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string url = e.Url.AbsoluteUri;
            if (url != null)
            {
                if(url.StartsWith(callBackUrl)){
                    currentUrl = url;
                }
            }
        }
    }
}
