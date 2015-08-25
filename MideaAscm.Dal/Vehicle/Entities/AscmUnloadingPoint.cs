using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Vehicle.Entities
{
    /// <summary>卸货点</summary>
    public class AscmUnloadingPoint
    {
        /// <summary>标识</summary>
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
        /// <summary>名称</summary>
        public virtual string name { get; set; }
        /// <summary>编号</summary>
        public virtual string sn { get; set; }
        /// <summary>子库标识</summary>
        public virtual string warehouseId { get; set; }
        /// <summary>方向</summary>
        public virtual string direction { get; set; }
        /// <summary>描述</summary>
        public virtual string description { get; set; }
        /// <summary>位置</summary>
        public virtual string location { get; set; }
        /// <summary>状态</summary>
        public virtual string status { get; set; }
        /// <summary>IP地址</summary>
        public virtual string ip { get; set; }
        /// <summary>是否启用</summary>
        public virtual bool enabled { get; set; }
        /// <summary>备注</summary>
        public virtual string memo { get; set; }
        /// <summary>卸货点控制器Id</summary>
        public virtual int controllerId { get; set; }
        /// <summary>卸货点控制器地址</summary>
        public virtual int controllerAddress { get; set; }

        //辅助属性
        public MideaAscm.Dal.Base.Entities.AscmWarehouse ascmWarehouse { get; set; }
        public string warehouseDescription { get { if (ascmWarehouse != null) return ascmWarehouse.description; return ""; } }
        public AscmUnloadingPointController ascmUnloadingPointController { get; set; }
        public string controllerName { get { if (ascmUnloadingPointController != null) return ascmUnloadingPointController.name; return ""; } }
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
        public string statusCn { get { return StatusDefine.DisplayText(status); } }
        public string enabledCn { get { return enabled ? "是" : "否"; } }
        public class StatusDefine
        {
            /// <summary>空闲</summary>
            public const string idle = "IDLE";
            /// <summary>使用中</summary>
            public const string inUse = "INUSE";
            /// <summary>预订</summary>
            public const string reserve = "RESERVE";
            /// <summary>故障</summary>
            public const string breakdown = "BREAKDOWN";

            public static string DisplayText(string status)
            {
                switch (status)
                {
                    case idle: return "空闲";
                    case inUse: return "使用中";
                    case reserve: return "预订";
                    case breakdown: return "故障";
                    default: return status;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(idle);
                list.Add(inUse);
                list.Add(reserve);
                list.Add(breakdown);
                return list;
            }
        }
    }
}
