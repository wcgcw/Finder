using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Finder.util
{
    /// <summary>
    /// ��Ϊ���������word��dll�����ԣ���ʹ�ô���ǰ��һ��Ҫ�Ȱ�װoffice��
    /// ��װ���office�Ժ�������Microsoft.Office.Interop.Word�����á�
    ///ʹ�÷���
    ///        WordUtil wu = new WordUtil("d:\\a.doc");
    ///        wu.read();
    ///        string[] replace = { "word", "12345" };
    ///        string[] replaceWith ={ "�滻���word", "�滻���12345" };
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

        //#region "set����"
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
        ///// ��ȡword�ļ������Ӧ���ǵ�һ��Ҫ���еģ������ĺ���������Ҫ�õ��˷������ص��ࡣ
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
        ///// ��word�ĵ��ڵ����ݽ����滻���������鳤��Ҫһ�¡�
        ///// �����һ�����鳤���򳬳����ÿ��ַ����滻��
        ///// ����ڶ������鳤����ֻʹ�����һ���鳤����ͬ�����ݡ�
        ///// </summary>
        ///// <param name="replace">Ҫ���滻����������</param>
        ///// <param name="replaceWith">�滻����������</param>
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

        //        //wdReplaceAll - �滻�ҵ��������
        //        //wdReplaceNone - ���滻�ҵ����κ��
        //        //wdReplaceOne - �滻�ҵ��ĵ�һ� 
        //        _Replace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;
        //        //ɾ����ʽ��
        //        doc.Content.Find.ClearFormatting();
        //        doc.Content.Find.Execute(ref _FindText, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref _ReplaceWith, ref _Replace, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue);
        //    }
        //}
        //public void saveAs(object savePath)
        //{
        //    try
        //    {
        //        this.doc.SaveAs(ref savePath, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
        //        MessageBox.Show("����·��Ϊ��"+savePath.ToString(),"�ɹ�");
        //    }
        //    catch (Exception error)
        //    {
        //        MessageBox.Show(error.Message);
        //    }

        //}
        ///// <summary>
        ///// ��ӡword�ĵ�
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
        ///// ��ӡԤ���ĵ�
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
        ///// �ر�wordӦ��
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
                paend1.Range.Text = "�����ߣ�" + username;
                paend1.Range.Font.Size = 14;
                paend1.Range.Font.Bold = 2;
                paend1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                paend1.Range.InsertParagraphAfter();
                //try
                //{
                //    doc.SaveAs(ref savePath, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                //    MessageBox.Show("����·��Ϊ��" + savePath.ToString(), "�ɹ�");
                //}
                //catch (Exception error)
                //{
                //    MessageBox.Show(error.Message);
                //}
            }
            catch (Exception error)
            {
                //MessageBox.Show("�޷����� Word Office �ĵ��������ϸ���ݣ�" + error.Message);
                MessageBox.Show("�޷����� Word Office �ĵ�������볢�����°�װ Word Office �����");
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
                //���� ��ɫ ������
                pa.Range.Text = "���������ⱨ��";
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

                //�������⣺��ɫ ���� 14����
                pa.Range.Text = "�������⣺" + title;
                pa.Range.Font.Size = 14;
                pa.Range.Font.ColorIndex = WdColorIndex.wdBlack;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                pa.Range.InsertParagraphAfter();

                //�����ߣ���ɫ ���� 14����
                pa.Range.Text = "�����ߣ�" + username;
                pa.Range.Font.Size = 14;
                pa.Range.Font.ColorIndex = WdColorIndex.wdBlack;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                pa.Range.InsertParagraphAfter();

                //�������ڣ���ɫ ���� 14����
                pa.Range.Text = "�������ڣ�" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                pa.Range.Font.Size = 14;
                pa.Range.Font.ColorIndex = WdColorIndex.wdBlack;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                pa.Range.InsertParagraphAfter();

                pa.Range.Text = "";
                pa.Range.Font.Size = 12;
                pa.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                pa.Range.InsertParagraphAfter();

                //�������ݣ���ɫ ���� 12����
                pa.Range.Text = "�������ݣ�";
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

                //���ݷ���
                pa.Range.Text = "���ݷ���";
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
                paend1.Range.Text = "�����ߣ�" + username;
                paend1.Range.Font.Size = 14;
                paend1.Range.Font.Bold = 2;
                paend1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                paend1.Range.InsertParagraphAfter();

                //try
                //{
                //    doc.SaveAs(ref savePath, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                //    MessageBox.Show("����·��Ϊ��" + savePath.ToString(), "�ɹ�");
                //}
                //catch (Exception error)
                //{
                //    MessageBox.Show(error.Message);
                //}
            }
            catch (Exception error)
            {
                //MessageBox.Show("�޷����� Word Office �ĵ��������ϸ���ݣ�" + error.Message);
                MessageBox.Show("�޷����� Word Office �ĵ�������볢�����°�װ Word Office �����");
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
