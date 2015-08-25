using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Base.Entities
{
    public class AscmReadingHeadLog
    {
        public virtual int id { get; set; }
        ///<summary>创建时间</summary>
        public virtual string createTime { get; set; }
        ///<summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }
        ///<summary>读头</summary>
        public virtual int readingHeadId { get; set; }
        ///<summary>rfid标签号</summary>
        public virtual string sn { get; set; }
        ///<summary>rfid</summary>
        public virtual string rfid { get; set; }
        ///<summary>已处理</summary>
        public virtual bool processed { get; set; }

        //辅助信息
        public virtual string createTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public AscmReadingHead ascmReadingHead { get; set; }
        public string readingHeadIp { get { if (ascmReadingHead != null) return ascmReadingHead.ip; return ""; } }
        public int readingHeadPort { get { if (ascmReadingHead != null) return ascmReadingHead.port; return 0; } }
        public virtual string processedName { get{if(processed) return "是";return"否";}}
    }
}
