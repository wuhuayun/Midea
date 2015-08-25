using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YnFrame.Dal.Entities;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.GetMaterialManage.Entities
{
    public class AscmLogisticsClassInfo
    {
        /// <summary>记录编号</summary>
        public virtual int id { get; set; }
        /// <summary>创建人</summary>
        public virtual string createUser { get; set; }
        /// <summary>创建时间</summary>
        public virtual string createTime { get; set; }
        /// <summary>最后更新人</summary>
        public virtual string modifyUser { get; set; }
        /// <summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }
        /// <summary>归属主体 {英文}</summary>
        public virtual string logisticsClass { get; set; }
        /// <summary>所属组长</summary>
        public virtual string groupLeader { get; set; }
        /// <summary>所属班长</summary>
        public virtual string monitorLeader { get; set; }
        /// <summary>归属主体 {中文}</summary>
        public virtual string logisticsName { get; set; }

        //辅助信息

        //public virtual YnUser groupUser { get; set; }
        //public virtual YnUser monitorUser { get; set; }
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string logisticsClassName { get; set; }

        //组长
        public virtual AscmUserInfo groupUser { get; set; }
        public virtual string _groupLeader { get { if (groupUser != null) return groupUser.userName; return groupLeader; } }

        //班长
        public virtual AscmUserInfo monitorUser { get; set; }
        public virtual string _monitorLeader { get { if (monitorUser != null) return monitorUser.userName; return monitorLeader; } }
        
    }
}
