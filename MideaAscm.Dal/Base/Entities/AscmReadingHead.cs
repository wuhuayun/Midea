using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Base.Entities
{
    public class AscmReadingHead
    {
        ///<summary>id</summary>
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
        ///<summary>类型</summary>
        public virtual string bindType { get; set; }
        ///<summary>绑定</summary>
        public virtual string bindId { get; set; }
        ///<summary>ip</summary>
        public virtual string ip { get; set; }
        ///<summary>port</summary>
        public virtual int port { get; set; }
        ///<summary>状态</summary>
        public virtual string status { get; set; }
        ///<summary>地址</summary>
        public virtual string address { get; set; }

        public AscmReadingHead GetOwner()
        {
            return (AscmReadingHead)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _bindTypeCn { get { return BindTypeDefine.DisplayText(bindType); } }
        
        //RFID绑定类型
        public class BindTypeDefine
        {
            /// <summary>员工车辆</summary>
            public const string employeeCar = "EmployeeCar";
            /// <summary>供应商车辆</summary>
            public const string supplierVehicleI = "SupplierVehicleI";

            public static string DisplayText(string value)
            {
                //if (value == user)
                //    return "用户";
                if (value == employeeCar)
                    return "员工车辆";
                else if (value == supplierVehicleI)
                    return "供应商车辆";

                return value;
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(employeeCar);
                list.Add(supplierVehicleI);
                return list;
            }
        }
        //RFID绑定类型
        public class StatusDefine
        {
            /// <summary></summary>
            public const string none = "";
            /// <summary>使用中</summary>
            public const string inUse = "inUse";
            /// <summary>作废</summary>
            public const string cancel = "cancel";

            public static string DisplayText(string value)
            {
                //if (value == user)
                //    return "用户";
                if (value == inUse)
                    return "使用中";
                else if (value == cancel)
                    return "作废";

                return value;
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(inUse);
                list.Add(cancel);
                return list;
            }
        }
    }
}
