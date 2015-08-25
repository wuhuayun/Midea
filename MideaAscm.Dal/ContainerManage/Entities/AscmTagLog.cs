using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.ContainerManage.Entities
{
    /// <summary>
    /// 记录厂区内所有标签的活动日志
    /// </summary>
    public class AscmTagLog
    {
        /// <summary>记录编号</summary>
        public virtual int id { get; set; }
        /// <summary>绑定类型</summary>
        public virtual string bindType { get; set; }
        /// <summary>读头ID</summary>
        public virtual int readingHeadId { get; set; }
        /// <summary>EPCID</summary>
        public virtual string epcId { get; set; }
        /// <summary>读取时间</summary>
        public virtual string readTime { get; set; }
        /// <summary>具体地点</summary>
        public virtual string place { get; set; }
        /// <summary>描述</summary>
        public virtual string description { get; set; }
        /// <summary>创建时间</summary>
        public virtual string createTime { get; set; }
        /// <summary>状态</summary>
        public virtual string status { get; set; }
        /// <summary>备注</summary>
        public virtual string tip { get; set; }

        public AscmTagLog GetOwner()
        {
            return (AscmTagLog)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string _bindType { get { return BindTypeDefine.DisplayText(bindType); } }
        public virtual AscmReadingHead ascmReadingHead { get; set; }
        public virtual string ReadingHeadAddress { get { if (ascmReadingHead != null) return ascmReadingHead.address; return ""; } }
        public virtual string _readTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(readTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(readTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _status { get { return StatusDefine.DisplayText(status); } }

        public virtual string supplierName { get; set; }  //供应商名称
        public virtual string objectID { get; set; } //物件的ID

        #region RFID绑定类型定义
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
        #endregion

        #region 状态定义
        public class StatusDefine
        {
            /// <summary>正常</summary>
            public const string normal = "NORMAL";
            /// <summary>异常</summary>
            public const string abnormal = "ABNORMAL";

            public static string DisplayText(string value)
            {
                if (value == normal)
                    return "正常";
                else if (value == abnormal)
                    return "异常";
                return value;
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(normal);
                list.Add(abnormal);
                return list;
            }
        }
        #endregion
    }
}
