using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace Finder.util
{
    /**//// <summary>
    /// WebSnap ：网页抓图对象
    /// </summary>
    public class WebSnap
    {
        /**//// <summary>
        /// 开始一个抓图并返回图象
        /// </summary>
        /// <param name="Url">要抓取的网页地址</param>
        /// <returns></returns>
        public static Bitmap StartSnap(string Url)
        {
            try
            {
                WebBrowser myWB = GetPage(Url);
                Bitmap returnValue = SnapWeb(myWB);
                myWB.Dispose();
                return returnValue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static WebBrowser GetPage(string Url)
        {
            WebBrowser myWB = new WebBrowser();
            myWB.ScrollBarsEnabled = false;
            myWB.ScriptErrorsSuppressed = false;
            myWB.Navigate(Url);
            long i = 0;
            while (myWB.ReadyState != WebBrowserReadyState.Complete)
            {
                i += 1;
                if (i%10 == 0)
                {
                    System.Windows.Forms.Application.DoEvents();
                    
                }
                if (i > 1000000)
                {
                     throw new Exception("连接超时!");
                }
            }
            return myWB;
        }

        private static Bitmap SnapWeb(WebBrowser wb)
        {
            HtmlDocument hd = wb.Document;
            int height = Convert.ToInt32(hd.Body.GetAttribute("scrollHeight")) + 10;
            int width = Convert.ToInt32(hd.Body.GetAttribute("scrollWidth")) + 10;
            wb.Height = height;
            wb.Width = width;
            Bitmap bmp = new Bitmap(width, height);
            Rectangle rec = new Rectangle();
            rec.Width = width;
            rec.Height = height;
            wb.DrawToBitmap(bmp, rec);
            return bmp;
        }
    }

}
