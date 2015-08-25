using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Vehicle.Entities
{
    public class AscmUnloadingPointLog
    {
        ///<summary>关键字</summary>
        public virtual int id { get; set; }
        ///<summary>卸货点</summary>
        public virtual int unloadingPointId { get; set; }
        ///<summary>创建时间</summary>
        public virtual string createTime { get; set; }
        ///<summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }
        ///<summary>卸货点名称</summary>
        public virtual string unloadingPointName { get; set; }
        ///<summary>卸货点编号</summary>
        public virtual string unloadingPointSn { get; set; }
        ///<summary>卸货点状态</summary>
        public virtual string unloadingPointStatus { get; set; }

        //辅助属性
        public virtual string createTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public string unloadingPointStatusCn { get { return AscmUnloadingPoint.StatusDefine.DisplayText(unloadingPointStatus); } }
    }
}
