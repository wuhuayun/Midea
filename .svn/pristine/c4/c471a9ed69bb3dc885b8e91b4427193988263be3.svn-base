using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Vehicle.Entities
{
    /// <summary>卸货位地图</summary>
    public class AscmUnloadingPointMap
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
        /// <summary>名称</summary>
        public virtual string name { get; set; }
        /// <summary>子库标识</summary>
        public virtual string warehouseId { get; set; }
        /// <summary>方向</summary>
        public virtual string direction { get; set; }
        /// <summary>地图尺寸：宽</summary>
        public virtual int imgWidth { get; set; }
        /// <summary>地图尺寸：高</summary>
        public virtual int imgHeight { get; set; }
        /// <summary>卸货点宽</summary>
        public virtual int width { get; set; }
        /// <summary>卸货点高</summary>
        public virtual int height { get; set; }
        /// <summary>地图路径</summary>
        public virtual string imgUrl { get; set; }
        ///// <summary>缩小地图路径</summary>
        //public virtual string smallImgUrl { get; set; }
        /// <summary>描述</summary>
        public virtual string description { get; set; }
        /// <summary>备注</summary>
        public virtual string memo { get; set; }

        //辅助属性
        public MideaAscm.Dal.Base.Entities.AscmWarehouse ascmWarehouse { get; set; }
        public string warehouseDescription { get { if (ascmWarehouse != null) return ascmWarehouse.description; return ""; } }
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
    }
}
