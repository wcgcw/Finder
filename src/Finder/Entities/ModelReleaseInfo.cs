using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finder
{
    public class ModelReleaseInfo
    {
        private int _uid;

        /// <summary>
        /// 流水号
        /// </summary>
        public int Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }
        private int _kid;
       
        public int Kid
        {
            get { return _kid; }
            set { _kid = value; }
        }
        private string _sheng;

        public string Sheng
        {
            get { return _sheng; }
            set { _sheng = value; }
        }
        private string _shi;

        public string Shi
        {
            get { return _shi; }
            set { _shi = value; }
        }
        private string _xian;

        public string Xian
        {
            get { return _xian; }
            set { _xian = value; }
        }
        private string _Title;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private string _Contexts;

        /// <summary>
        /// 内容
        /// </summary>
        public string Contexts
        {
            get { return _Contexts; }
            set { _Contexts = value; }
        }
        private string _ReleaseDate;

        /// <summary>
        /// 发布日期
        /// </summary>
        public string ReleaseDate
        {
            get { return _ReleaseDate; }
            set { _ReleaseDate = value; }
        }
        private string _InfoSource;

        /// <summary>
        /// 信息来源
        /// </summary>
        public string InfoSource
        {
            get { return _InfoSource; }
            set { _InfoSource = value; }
        }
        private string _KeyWords;

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWords
        {
            get { return _KeyWords; }
            set { _KeyWords = value; }
        }
        private string _ReleaseName;

        /// <summary>
        /// 发布人
        /// </summary>
        public string ReleaseName
        {
            get { return _ReleaseName; }
            set { _ReleaseName = value; }
        }
        private string _CollectDate;

        /// <summary>
        /// 采集时间
        /// </summary>
        public string CollectDate
        {
            get { return _CollectDate; }
            set { _CollectDate = value; }
        }
        private string _Snapshot;

        /// <summary>
        /// 快照
        /// </summary>
        public string Snapshot
        {
            get { return _Snapshot; }
            set { _Snapshot = value; }
        }

        private string _WebName;
        /// <summary>
        /// 网站名
        /// </summary>
        public string WebName
        {
            get { return _WebName; }
            set { _WebName = value; }
        }

        private int _pid;
        /// <summary>
        /// 数据类别：0：其他，1：博客，2：论坛，3：微博
        /// </summary>
        public int Pid
        {
            get { return _pid; }
            set { _pid = value; }
        }
        private int  _part;

        /// <summary>
        /// 正负词性。0：负，1：正
        /// </summary>
        public int Part
        {
            get { return _part; }
            set { _part = value; }
        }
        private int _reposts;
        /// <summary>
        /// 此项未用
        /// </summary>
        public int Reposts
        {
            get { return _reposts; }
            set { _reposts = value; }
        }
        private int _Comments;

        /// <summary>
        /// 此项未用
        /// </summary>
        public int Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }
    }
}
