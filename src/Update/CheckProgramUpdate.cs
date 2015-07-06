using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Threading;
using System.Diagnostics;

namespace Update
{
    public class CheckProgramUpdate
    {
        private UpdateInfo uInfo;

        #region 事件及代理
        public delegate void ShowUpdateInfoEventHandler(UpdateInfo updateInfo);
        public ShowUpdateInfoEventHandler sueh;
        public event ShowUpdateInfoEventHandler ShowUpdate
        {
            add { sueh += new ShowUpdateInfoEventHandler(value); }
            remove { sueh -= new ShowUpdateInfoEventHandler(value); }
        }

        public delegate void UpdateProgressVal(int rowId,int val,bool isLast);
        public UpdateProgressVal ups;
        public event UpdateProgressVal UpdateProgress
        {
            add { ups += new UpdateProgressVal(value); }
            remove { ups -= new UpdateProgressVal(value); }
        }
        #endregion

        //更新文件的地址
        private string checkUpdateUrl = "http://www.shangwukaocha.cn/finder/";

        /// <summary>
        /// 检查是否有更新
        /// </summary>
        public void check()
        {
            HttpWebRequest webreq = (HttpWebRequest)WebRequest.Create(checkUpdateUrl + "update.xml");
            webreq.Method = "GET";
            webreq.BeginGetResponse(new AsyncCallback(EndGetCheckResponse), webreq);
        }
        /// <summary>
        /// 检查是否有更新的返回及处理
        /// </summary>
        /// <param name="result"></param>
        private void EndGetCheckResponse(IAsyncResult result)
        {
            UpdateInfo ui = new UpdateInfo();
            ui.UpdateFilesInfo = new List<UpdateFileInfo>();
            try
            {
                //结束异步请求，获取结果
                HttpWebRequest webRequest = (HttpWebRequest)result.AsyncState;
                WebResponse webResponse = webRequest.EndGetResponse(result);
                //把输出结果转化为Person对象
                Stream stream = webResponse.GetResponseStream();
                StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                string upInfo = sr.ReadToEnd();

                //开始解析更新文件的xml
                if (upInfo.Trim().Length > 0)
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(upInfo);

                    XmlNode root = xmldoc.SelectSingleNode("//files");
                    if (root.HasChildNodes)
                    {
                        ui.FilesTotalSize = int.Parse(root.Attributes["totalsize"].Value);
                        ui.FilesTotalSizeUnit = root.Attributes["unit"].Value;

                        XmlNodeList xnl = root.ChildNodes;
                        foreach (XmlNode xn in xnl)
                        {
                            UpdateFileInfo ufi = new UpdateFileInfo();
                            string ufileVersion = xn.Attributes["version"].Value;
                            string[] ufileVerInfo = ufileVersion.Split(new char[] { '.' });
                            FileVersionInfo fileVer = FileVersionInfo.GetVersionInfo(xn.Attributes["name"].Value);
                            string oldFileVersion = fileVer.FileVersion;
                            string[] oldFileVerInfo = oldFileVersion.Split(new char[] { '.' });
                            //以下开始比较文件版本号。如果更新文件中的文件版本号大于本地的文件版本号，则下载更新，否则，不下载此文件更新
                            if (int.Parse(ufileVerInfo[0]) > int.Parse(oldFileVerInfo[0]) || int.Parse(ufileVerInfo[1]) > int.Parse(oldFileVerInfo[1]) || int.Parse(ufileVerInfo[2]) > int.Parse(oldFileVerInfo[2]) || int.Parse(ufileVerInfo[3]) > int.Parse(oldFileVerInfo[3]))
                            {
                                ufi.FileName = xn.Attributes["name"].Value;
                                ufi.FileSize = int.Parse(xn.Attributes["size"].Value);
                                ufi.FileVersion = xn.Attributes["version"].Value;
                                ui.UpdateFilesInfo.Add(ufi);
                            }
                        }
                        if (ui.UpdateFilesInfo.Count == 0)
                        {
                            ui.CheckResult = "您目前的版本已是最新";
                        }
                    }
                    else
                    {
                        ui.CheckResult = "您目前的版本已是最新";
                    }
                }
                else
                {
                    ui.CheckResult = "您目前的版本已是最新";
                }
            }
            catch (Exception e)
            {
                ui.CheckResult = "更新检查无法完成，请联系供应商";
            }
            this.uInfo = ui;
            if (sueh != null) sueh(ui);
        }
        public void update(object tmpDirObj)
        {
            string tmpDir = tmpDirObj.ToString();
            tmpDir = tmpDir += "/tmp/";
            if (!Directory.Exists(tmpDir))
            {
                Directory.CreateDirectory(tmpDir);
            }
            for (int i = 0; i < uInfo.UpdateFilesInfo.Count; i++)
            {
                UpdateFileInfo ufi = uInfo.UpdateFilesInfo[i];
                DownloadFile(i, this.checkUpdateUrl + ufi.FileName, tmpDir + ufi.FileName, i == uInfo.UpdateFilesInfo.Count - 1 ? true : false);
            }
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="index">要更新的行数</param>
        /// <param name="URL">要下载的文件地址</param>
        /// <param name="saveFile">保存文件的地址</param>
        /// <param name="isLast">是否最后一个文件</param>
        public void DownloadFile(int index,string URL, string saveFile,bool isLast)
        {
            int percent = 0;
            try
            {
                HttpWebRequest Myrq = (HttpWebRequest)WebRequest.Create(URL);
                Myrq.Proxy = null;
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();

                long totalBytes = myrp.ContentLength;
                System.IO.Stream st = myrp.GetResponseStream();
                System.IO.Stream so = new System.IO.FileStream(saveFile, System.IO.FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    System.Windows.Forms.Application.DoEvents();
                    so.Write(by, 0, osize);
                    osize = st.Read(by, 0, (int)by.Length);

                    percent = (int)(totalDownloadedByte / (float)totalBytes * 100);
                    if (ups != null) ups(index, percent,isLast);
                    System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则将因为循环执行太快而来不及显示信息
                }
                so.Close();
                st.Close();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
