using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.SupplierPreparation.Entities
{
    public class AscmDriver
    {
        ///<summary>标识</summary>
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
        ///<summary>司机编号</summary>
        public virtual string sn { get; set; }
        ///<summary>绑定rfid</summary>
        public virtual string rfid { get; set; }
        ///<summary>供应商id</summary>
        public virtual int supplierId { get; set; }
        ///<summary>姓名</summary>
        public virtual string name { get; set; }
        ///<summary>性别</summary>
        public virtual string sex { get; set; }
        ///<summary>身份证号</summary>
        public virtual string idNumber { get; set; }
        ///<summary>移动电话</summary>
        public virtual string mobileTel { get; set; }
        ///<summary>状态</summary>
        public virtual string status { get; set; }
        ///<summary>车牌号</summary>
        public virtual string plateNumber { get; set; }
        ///<summary>载重</summary>
        public virtual decimal load { get; set; }
        ///<summary>描述</summary>
        public virtual string description { get; set; }
        ///<summary>类型</summary>
        public virtual string type { get; set; }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
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
        public string typeCn
        {
            get
            {
                return TypeDefine.DisplayText(type);
            }
        }

        public class TypeDefine
        {
            /// <summary>自有</summary>
            public static readonly string own = "OWN";
            /// <summary>第三方</summary>
            public static readonly string thirdParty = "THIRDPARTY";

            public static string DisplayText(string type)
            {
                switch (type)
                {
                    case "OWN": return "自有";
                    case "THIRDPARTY": return "第三方";
                    default: return type;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(own);
                list.Add(thirdParty);
                return list;
            }
        }
        public virtual string rfid1
        {
            get
            {
                if (rfid != null && rfid.Trim().Length == 10)
                {
                    foreach (string s1 in MideaAscm.Dal.Base.Entities.AscmRfid.Pre24G.GetList())
                    {
                        if (rfid.Trim().StartsWith(s1))
                        {
                            return s1;
                        }

                    }
                }
                return "";
            }
        }
        public virtual string rfid2
        {
            get
            {
                if (rfid != null && rfid.Trim().Length == 10)
                {
                    foreach (string s1 in MideaAscm.Dal.Base.Entities.AscmRfid.Pre24G.GetList())
                    {
                        if (rfid.Trim().StartsWith(s1))
                        {
                            return rfid.Trim().Substring(s1.Length);
                        }

                    }
                }
                return "";
            }
        }
    }
}
