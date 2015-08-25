using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>厂房</summary>
    public class AscmWorkshopBuilding
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
        /// <summary>序号</summary>
        public virtual int sortNo { get; set; }
        /// <summary>名称</summary>
        public virtual string name { get; set; }
        /// <summary>代号(一位字符)</summary>
        public virtual string code { get; set; }
        /// <summary>描述</summary>
        public virtual string description { get; set; }
        /// <summary>水平</summary>
        public virtual int horizontal { get; set; }
        /// <summary>垂直</summary>
        public virtual string vertical { get; set; }

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
        public string buildingArea { get; set; }
    }
}
