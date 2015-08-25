using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Base.Entities
{
    public class AscmRfid
    {
        ///<summary>id</summary>
        public virtual string id { get; set; }
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
        ///<summary>EPC</summary>
        public virtual string epcId { get; set; }
        ///<summary>类型</summary>
        public virtual string bindType { get; set; }
        ///<summary>绑定</summary>
        public virtual string bindId { get; set; }
        ///<summary>状态</summary>
        public virtual string status { get; set; }


        public AscmRfid GetOwner()
        {
            return (AscmRfid)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _bindTypeCn { get { return BindTypeDefine.DisplayText(bindType); } }
        public virtual string _bindDescription { get; set; }
        public virtual string _status { get { return StatusDefine.DisplayText(status); } }

        //RFID绑定类型
        public class BindTypeDefine
        {
            /// <summary>员工车辆</summary>
            public const string employeeCar = "EMPLOYEECAR";
            /// <summary>容器</summary>
            public const string container = "CONTAINER";
            /// <summary>托盘</summary>
            public const string pallet = "PALLET";
            /// <summary>司机</summary>
            public const string driver = "DRIVER";
            /// <summary>领料车辆</summary>
            public const string forklift = "FORKLIFT";

            public static string DisplayText(string value)
            {
                if (value == employeeCar)
                    return "员工车辆";
                else if (value == container)
                    return "容器";
                else if (value == pallet)
                    return "托盘";
                else if (value == driver)
                    return "司机";
                else if (value == forklift)
                    return "领料车辆";
                return value;
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(employeeCar);
                list.Add(container);
                list.Add(pallet);
                list.Add(driver);
                list.Add(forklift);
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
        //RFID绑定类型
        public class Pre24G
        {
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add("400977");
                list.Add("4009");
                list.Add("");
                return list;
            }
        }
    }
}
