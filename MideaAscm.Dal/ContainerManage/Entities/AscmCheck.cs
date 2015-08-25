using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.ContainerManage.Entities
{
    public class AscmCheck
    {
        /// <summary>记录编号</summary>
        public virtual int id { get; set; }
        /// <summary>标识号</summary>
        public virtual int identificationId { get; set; }
        /// <summary>数量</summary>
        public virtual int? count { get; set; }
        /// <summary>总数量</summary>
        public virtual int totalNumber { get; set; }
        /// <summary>创建人</summary>
        public virtual string createUser { get; set; }
        /// <summary>创建时间</summary>
        public virtual string createTime { get; set; }
        /// <summary>备注</summary>
        public virtual string tip { get; set; }

        //辅助信息
        public virtual string _identificationId { get { return IdentificationDefine.DisplayText(identificationId); } }
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }

        #region 标示符定义
        public class IdentificationDefine
        {
            /// <summary>盘盈</summary>
            public const int inventoryProfit = -1;
            /// <summary>账实相符</summary>
            public const int inventoryLosses = 0;
            /// <summary>盘亏</summary>
            public const int iinventoryDeuce = 1;

            public static string DisplayText(int value)
            {
                if (value == 1)
                    return "盘盈";
                else if (value == 0)
                    return "账实相符";
                if (value ==-1)
                    return "盘亏";
                return value.ToString();
            }

            public static List<int> GetList()
            {
                List<int> list = new List<int>();
                list.Add(inventoryProfit);
                list.Add(inventoryLosses);
                return list;
            }
        }
        #endregion
    }
}
