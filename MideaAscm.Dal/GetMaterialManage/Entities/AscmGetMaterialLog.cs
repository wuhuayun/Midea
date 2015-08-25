using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.GetMaterialManage.Entities
{
    public class AscmGetMaterialLog
    {
        /// <summary>记录编号</summary>
        public virtual int id { get; set; }
        /// <summary>创建人</summary>
        public virtual string createUser { get; set; }
        /// <summary>创建时间</summary>
        public virtual string createTime { get; set; }
        /// <summary>最后更新人</summary>
        public virtual string modifyUser { get; set; }
        /// <summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }
        ///<summary>作业号ID</summary>
        public virtual int wipEntityId { get; set; }
        ///<summary>物料ID</summary>
        public virtual int materialId { get; set; }
        ///<summary>子库</summary>
        public virtual string warehouseId { get; set; }
        ///<summary>备料数量（领料数量）</summary>
        public virtual decimal quantity { get; set; }
        ///<summary>备料单字符串</summary>
        public virtual string preparationString { get; set; }
        ///<summary>领料员ID</summary>
        public virtual string workerId { get; set; }
        /// <summary>状态</summary>
        public virtual int status { get; set; }
        /// <summary>备注</summary>
        public virtual string tip { get; set; }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }

        public class StatusDefine
        {
            public const int normalStatus = 0;

            public const int abnormalSatus = 1;

            public static string DisplayText(int value)
            {
                if (value == normalStatus)
                    return "正常";
                else if (value == abnormalSatus)
                    return "异常";
                else
                    return value.ToString();
            }

            public static List<int> GetList()
            {
                List<int> list = new List<int>();
                list.Add(normalStatus);
                list.Add(abnormalSatus);

                return list;
            }
        }
    }
}
