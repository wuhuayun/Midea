using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Vehicle.Entities
{
    /// <summary>卸货位地图关联卸货点</summary>
    public class AscmUnloadingPointMapLink
    {
        /// <summary>标识</summary>
        public virtual int id { get; set; }
        ///<summary>库存组织ID</summary>
        public virtual int organizationId { get; set; }
        ///<summary>创建人</summary>
        public virtual string createUser { get; set; }
        ///<summary>创建时间</summary>
        public virtual string createTime { get; set; }
        ///<summary>最后更新人</summary>
        public virtual string modifyUser { get; set; }
        ///<summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }
        /// <summary>卸货位地图ID</summary>
        public virtual int mapId { get; set; }
        /// <summary>卸货点ID</summary>
        public virtual int pointId { get; set; }
        /// <summary>Y轴</summary>
        public virtual int y { get; set; }
        /// <summary>X轴</summary>
        public virtual int x { get; set; }
        /// <summary>备注</summary>
        public virtual string memo { get; set; }

        //辅助属性
        public string _createTime
        {
            get
            {
                if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null)
                    return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm");
                return "";
            }
        }
        public string _modifyTime
        {
            get
            {
                if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null)
                    return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm");
                return "";
            }
        }
        public AscmUnloadingPoint ascmUnloadingPoint { get; set; }
        public string pointName { get { if (ascmUnloadingPoint != null) return ascmUnloadingPoint.name; return ""; } }
        public string pointStatus { get { if (ascmUnloadingPoint != null) return ascmUnloadingPoint.status; return ""; } }
    }
}
