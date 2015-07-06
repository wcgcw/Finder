using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finder.Entities
{
    class SystemSet
    {
        public string Id { set; get; }
        /// <summary>
        /// 留存证据图片的保存路径
        /// </summary>
        public string EvidenceImgSavePath { set; get; }
    }
}
