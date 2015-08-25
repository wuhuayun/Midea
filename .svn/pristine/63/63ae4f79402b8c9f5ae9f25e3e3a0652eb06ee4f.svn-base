using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>厂房与子库关联</summary>
    public class AscmBuildingWarehouseLink
    {
        /// <summary>关键字</summary>
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
        /// <summary>子库代号(一位字符)</summary>
        public virtual string warehouseCode { get; set; }
        /// <summary>厂房ID</summary>
        public virtual int buildingId { get; set; }
        /// <summary>子库ID</summary>
        public virtual string warehouseId { get; set; }
        /// <summary>厂房区域</summary>
        public virtual string buildingArea { get; set; }

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
        public AscmWarehouse ascmWarehouse { get; set; }
        public string warehouseName { get { if (ascmWarehouse != null) return ascmWarehouse.description; return ""; } }
    }
}
