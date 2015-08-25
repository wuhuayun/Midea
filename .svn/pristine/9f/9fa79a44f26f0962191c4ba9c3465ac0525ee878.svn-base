using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.SupplierPreparation.Entities;

namespace MideaAscm.Dal.ContainerManage.Entities
{
    public class AscmCheckContainerInfo
    {
        /// <summary>记录编号</summary>
        public virtual int id { get; set; }
        /// <summary>盘点ID</summary>
        public virtual int checkId { get; set; }
        /// <summary>供应商ID</summary>
        public virtual int supplierId { get; set; }
        /// <summary>容器ID</summary>
        public virtual string containerId { get; set; }
        /// <summary>创建人</summary>
        public virtual string createUser { get; set; }
        /// <summary>创建时间</summary>
        public virtual string createTime { get; set; }
        /// <summary>状态</summary>
        public virtual string status { get; set; }
        /// <summary>备注</summary>
        public virtual string tip { get; set; }

        //辅助信息
        public virtual AscmCheck ascmCheck { get; set; }
        public virtual AscmSupplier ascmSupplier { get; set; }
        public virtual AscmContainer ascmContainer { get; set; }
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _status { get { return StatusDefine.DisplayText(status); } }

        #region 状态定义
        public class StatusDefine
        {
            /// <summary>丢失</summary>
            public const string lost = "LOST";
            /// <summary>找回</summary>
            public const string find = "FIND";

            public static string DisplayText(string value)
            {
                if (value == lost)
                    return "丢失";
                else if (value == find)
                    return "找回";
                return value;
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(lost);
                list.Add(find);
                return list;
            }
        }
        #endregion
    }
}
