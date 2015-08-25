using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.SupplierPreparation.Entities
{
    ///<summary>托盘</summary>
    public class AscmPallet
    {
        ///<summary>托盘编号</summary>
        public virtual string sn { get; set; }
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
        ///<summary>绑定rfid</summary>
        public virtual string rfid { get; set; }
        ///<summary>供应商id</summary>
        public virtual int supplierId { get; set; }
        ///<summary>状态</summary>
        public virtual string status { get; set; }
        ///<summary>描述</summary>
        public virtual string description { get; set; }

        //辅助属性
        public Base.Entities.AscmSupplier supplier { get; set; }
        public string supplierName
        {
            get
            {
                if (supplier != null)
                    return supplier.name;
                return "";
            }
        }
        public string statusCn
        {
            get
            {
                return StatusDefine.DisplayText(status);
            }
        }
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
        public string startSn { get; set; }
        public string endSn { get; set; }

        public class StatusDefine
        {
            /// <summary>未使用</summary>
            public static readonly string unuse = "UNUSE";
            /// <summary>备料</summary>
            public static readonly string preparation = "PREPARATION";
            /// <summary>作废</summary>
            public static readonly string invalid = "INVALID";

            public static string DisplayText(string status)
            {
                switch (status)
                {
                    case "UNUSE": return "未使用";
                    case "PREPARATION": return "备料";
                    case "INVALID": return "作废";
                    default: return status;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(unuse);
                list.Add(preparation);
                list.Add(invalid);
                return list;
            }
        }
    }
}
