using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Update
{
    /// <summary>
    /// 更新信息类。包含更新文件的集合
    /// </summary>
    public class UpdateInfo
    {
        public int FilesTotalSize { set; get; }
        public string FilesTotalSizeUnit { set; get; }
        public string CheckResult { set; get; }
        public IList<UpdateFileInfo> UpdateFilesInfo { set; get; }
    }
    /// <summary>
    /// 单个更新文件的信息类。
    /// </summary>
    public class UpdateFileInfo
    {
        public string FileName { set; get; }
        public string FileVersion { set; get; }
        public int FileSize { set; get; }
    }
}
