using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Finder.util
{
    /// <summary>
    /// 因为该类调用了word的dll。所以，在使用此类前，一定要先安装office。
    /// 安装完成office以后才能添加Microsoft.Office.Interop.Word的引用。
    ///使用方法
    ///        WordUtil wu = new WordUtil("d:\\a.doc");
    ///        wu.read();
    ///        string[] replace = { "word", "12345" };
    ///        string[] replaceWith ={ "替换后的word", "替换后的12345" };
    ///        wu.replace(replace, replaceWith);
    ///        wu.print();
    ///        wu.close();
    /// </summary>
    class WordUtil
    {

        //private Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
        //private Microsoft.Office.Interop.Word.Document doc;
        //object Nothing = System.Reflection.Missing.Value;
        //private object sourceDocPath;

        //#region "set方法"
        //public void setSourceDocPath(object _sourceDocPath)
        //{
        //    this.sourceDocPath = _sourceDocPath;
        //}
        //#endregion

        //public WordUtil(object _sourceDocPath)
        //{
        //    this.sourceDocPath = _sourceDocPath;
        //}
        ///// <summary>
        ///// 读取word文件，这个应该是第一步要进行的，其他的后续操作都要用到此方法返回的类。
        ///// </summary>
        ///// <returns></returns>
        //public void read()
        //{
        //    object objDocType = WdDocumentType.wdTypeDocument;
        //    object type = WdBreakType.wdSectionBreakContinuous;

        //    object readOnly = true;
        //    object isVisible = true;

        //    object docPath = this.sourceDocPath;

        //    //doc = wordApp.Documents.Add(ref docPath, ref Nothing, ref Nothing, ref isVisible);
        //    doc = wordApp.Documents.Open(ref docPath, ref Nothing, ref readOnly, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref isVisible, ref Nothing, ref Nothing, ref Nothing, ref Nothing);

        //}
        ///// <summary>
        ///// 对word文档内的内容进行替换。两个数组长度要一致。
        ///// 如果第一个数组长，则超出的用空字符串替换。
        ///// 如果第二个数组长，则只使用与第一数组长度相同的数据。
        ///// </summary>
        ///// <param name="replace">要被替换的内容数组</param>
        ///// <param name="replaceWith">替换的内容数组</param>
        ///// <returns></returns>
        //public void replace(string[] replace,string[] replaceWith){
        //    object format = WdSaveFormat.wdFormatDocument;
        //    object readOnly = false;
        //    object isVisible = false;

        //    object _FindText, _ReplaceWith, _Replace;
        //    object MissingValue = Type.Missing;

        //    for (int i = 0; i < replace.Length; i++)
        //    {
        //        string rep = replace[i];
        //        _FindText = rep;
        //        doc.Content.Find.Text = rep;

        //        string repWith = replaceWith[i];
        //        _ReplaceWith = repWith;

        //        //wdReplaceAll - 替换找到的所有项。
        //        //wdReplaceNone - 不替换找到的任何项。
        //        //wdReplaceOne - 替换找到的第一项。 
        //        _Replace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;
        //        //删除格式。
        //        doc.Content.Find.ClearFormatting();
        //        doc.Content.Find.Execute(ref _FindText, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref _ReplaceWith, ref _Replace, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue);
        //    }
        //}
        //public void saveAs(object savePath)
        //{
        //    try
        //    {
        //        this.doc.SaveAs(ref savePath, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
        //        MessageBox.Show("保存路径为："+savePath.ToString(),"成功");
        //    }
        //    catch (Exception error)
        //    {
        //        MessageBox.Show(error.Message);
        //    }

        //}
        ///// <summary>
        ///// 打印word文档
        ///// </summary>
        ///// <param name="doc"></param>
        //public void print()
        //{
        //    object background = false;
        //    if (this.doc != null)
        //    {
        //        this.doc.PrintOut(ref background, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
        //    }
        //}
        ///// <summary>
        ///// 打印预览文档
        ///// </summary>
        ///// <param name="doc"></param>
        //public void printPreview()
        //{
        //    if (this.doc != null)
        //    {
        //        this.wordApp.PrintPreview = !this.wordApp.PrintPreview;
        //        this.wordApp.Visible = true;
        //        this.doc.ActiveWindow.View.Type = WdViewType.wdPrintPreview;
        //        this.doc.PrintPreview();
        //    }
        //}
        ///// <summary>
        ///// 关闭word应用
        ///// </summary>
        //public void close()
        //{
        //    //object saveOptions = Microsoft.Office.Interop.Word.WdSaveOptions.wdSaveChanges;
        //    object saveOptions = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;

        //    if (doc != null)
        //    {
        //        try
        //        {
        //            doc.Close(ref saveOptions, ref Nothing, ref Nothing);
        //        }
        //        catch (COMException ex)
        //        {
        //            //MessageBox.Show(ex.ErrorCode.ToString());
        //        }
        //    }
        //    if (wordApp != null)
        //    {
        //        wordApp.Quit(ref Nothing, ref Nothing, ref Nothing);
        //    }
        //}

        public void creatWord(object savePath, string title, List<string> p, string time, string username)
        {
            try
            {
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document doc;
                object Nothing = System.Reflection.Missing.Value;
                object oMissing = System.Reflection.Missing.Value;
                wordApp.Visible = true;
                doc = wordApp.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                Microsoft.Office.Interop.Word.Paragraph pa = doc.Content.Paragraphs.Add(ref Nothing);                
                pa.Range.Text = title;
                pa.Range.Font.Bold = 2;
                pa.Range.Font.Size = 18;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                pa.Range.InsertParagraphAfter();

                for (int i = 0, l = p.Count; i < l; i++)
                {
                    if (p[i].Length > 0)
                    {
                        string[] s = p[i].Split(new string[] { "<?p>" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0, k = s.Length; j < k; j++)
                        {
                            Microsoft.Office.Interop.Word.Paragraph pt = doc.Content.Paragraphs.Add(ref Nothing);
                            pt.Range.Text = s[j];
                            if (j == 0)
                            {
                                pt.Range.Font.Bold = 2;
                                pt.Range.Font.Size = 16;
                            }
                            else
                            {
                                pt.Range.Font.Bold = 0;
                                pt.Range.Font.Size = 14;
                            }
                            pt.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                            pt.Range.InsertParagraphAfter();
                        }
                    }
                }

                Microsoft.Office.Interop.Word.Paragraph paend = doc.Content.Paragraphs.Add(ref Nothing);
                paend.Range.Text = time;
                paend.Range.Font.Size = 14;
                paend.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                paend.Range.InsertParagraphAfter();

                Microsoft.Office.Interop.Word.Paragraph paend1 = doc.Content.Paragraphs.Add(ref Nothing);
                paend1.Range.Text = "报告者：" + username;
                paend1.Range.Font.Size = 14;
                paend1.Range.Font.Bold = 2;
                paend1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                paend1.Range.InsertParagraphAfter();
                //try
                //{
                //    doc.SaveAs(ref savePath, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                //    MessageBox.Show("保存路径为：" + savePath.ToString(), "成功");
                //}
                //catch (Exception error)
                //{
                //    MessageBox.Show(error.Message);
                //}
            }
            catch (Exception error)
            {
                //MessageBox.Show("无法加载 Word Office 文档组件，详细内容：" + error.Message);
                MessageBox.Show("无法加载 Word Office 文档组件，请尝试重新安装 Word Office 软件！");
            }
        }

        public void CreateWord(object savePath, string title, List<string> p, string time, string username)
        {
            try
            {
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document doc;
                object Nothing = System.Reflection.Missing.Value;
                object oMissing = System.Reflection.Missing.Value;
                wordApp.Visible = true;
                doc = wordApp.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                Microsoft.Office.Interop.Word.Paragraph pa = doc.Content.Paragraphs.Add(ref Nothing);
                //标题 红色 初号字
                pa.Range.Text = "网络舆情监测报告";
                //pa.Range.Font.Bold = 2;
                pa.Range.Font.Size = 42;
                pa.Range.Font.ColorIndex = WdColorIndex.wdRed;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                pa.Range.InsertParagraphAfter();

                pa.Range.Text = "";
                pa.Range.Font.Size = 14;
                pa.Range.Font.ColorIndex = WdColorIndex.wdRed;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                pa.Range.InsertParagraphAfter();

                //报告主题：黑色 宋体 14号字
                pa.Range.Text = "报告主题：" + title;
                pa.Range.Font.Size = 14;
                pa.Range.Font.ColorIndex = WdColorIndex.wdBlack;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                pa.Range.InsertParagraphAfter();

                //发布者：黑色 宋体 14号字
                pa.Range.Text = "发布者：" + username;
                pa.Range.Font.Size = 14;
                pa.Range.Font.ColorIndex = WdColorIndex.wdBlack;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                pa.Range.InsertParagraphAfter();

                //发布日期：黑色 宋体 14号字
                pa.Range.Text = "发布日期：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                pa.Range.Font.Size = 14;
                pa.Range.Font.ColorIndex = WdColorIndex.wdBlack;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                pa.Range.InsertParagraphAfter();

                pa.Range.Text = "";
                pa.Range.Font.Size = 12;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                pa.Range.InsertParagraphAfter();

                //报告内容：黑色 宋体 12号字
                pa.Range.Text = "报告内容：";
                pa.Range.Font.Size = 12;
                pa.Range.Font.ColorIndex = WdColorIndex.wdBlack;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                pa.Range.InsertParagraphAfter();

                pa.Range.Text = "";
                pa.Range.Font.Size = 12;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                pa.Range.InsertParagraphAfter();

                pa.Range.Text = "";
                pa.Range.Font.Size = 12;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                pa.Range.InsertParagraphAfter();

                pa.Range.Text = "";
                pa.Range.Font.Size = 12;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                pa.Range.InsertParagraphAfter();

                pa.Range.Text = "";
                pa.Range.Font.Size = 12;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                pa.Range.InsertParagraphAfter();

                //数据分析
                pa.Range.Text = "数据分析";
                pa.Range.Font.Size = 12;
                pa.Range.Font.ColorIndex = WdColorIndex.wdBlack;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                pa.Range.InsertParagraphAfter();

                for (int i = 0, l = p.Count; i < l; i++)
                {
                    if (p[i].Length > 0)
                    {
                        string[] s = p[i].Split(new string[] { "<?p>" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0, k = s.Length; j < k; j++)
                        {
                            Microsoft.Office.Interop.Word.Paragraph pt = doc.Content.Paragraphs.Add(ref Nothing);
                            pt.Range.Text = s[j];
                            if (j == 0)
                            {
                                pt.Range.Font.Bold = 2;
                                pt.Range.Font.Size = 12;
                            }
                            else
                            {
                                pt.Range.Font.Bold = 0;
                                pt.Range.Font.Size = 12;
                            }
                            pt.Space15();
                            pt.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                            pt.Range.InsertParagraphAfter();
                        }
                    }
                }

                Microsoft.Office.Interop.Word.Paragraph paend = doc.Content.Paragraphs.Add(ref Nothing);
                paend.Range.Text = time;
                paend.Range.Font.Size = 14;
                paend.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                paend.Range.InsertParagraphAfter();

                Microsoft.Office.Interop.Word.Paragraph paend1 = doc.Content.Paragraphs.Add(ref Nothing);
                paend1.Range.Text = "发布者：" + username;
                paend1.Range.Font.Size = 14;
                paend1.Range.Font.Bold = 2;
                paend1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                paend1.Range.InsertParagraphAfter();

                //try
                //{
                //    doc.SaveAs(ref savePath, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                //    MessageBox.Show("保存路径为：" + savePath.ToString(), "成功");
                //}
                //catch (Exception error)
                //{
                //    MessageBox.Show(error.Message);
                //}
            }
            catch (Exception error)
            {
                //MessageBox.Show("无法加载 Word Office 文档组件，详细内容：" + error.Message);
                MessageBox.Show("无法加载 Word Office 文档组件，请尝试重新安装 Word Office 软件！");
            }
        }

        //public void PrintDialog(string strFileName,string printerName)   
        //{   
        //    if (File.Exists(strFileName))   
        //    {   
        //        activeWordApp();   
        //        WordApp.Visible = false;   
        //        object oPrintFile = (object)strFileName;   
        //        try  
        //        {   
        //            while (isOpened)   
        //            {   
        //                System.Threading.Thread.Sleep(500);   
        //            }   
        //            WordDoc = WordApp.Documents.Add(ref oPrintFile, ref missing, ref missing, ref missing);   
        //            isOpened = true;   
        //            WordDoc.Activate();   

        //            //WordDoc.PrintPreview();   
        //            if (printerName != null)   
        //            {   
        //                WordApp.ActivePrinter = printerName;   
        //            }   
        //            WordDoc.PrintOut();   
        //            this.Quit();   
        //        }   
        //        catch (Exception Ex)   
        //        {   
        //            Quit();   
        //            isOpened = false;   
        //            throw new Exception(Ex.Message);   
        //        }   
        //    }   
        //}   

    }
}
