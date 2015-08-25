using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.Warehouse.Entities
{
    ///<summary>仓库容器备料</summary>
    public class AscmWmsContainerDelivery
    {
        ///<summary>Id</summary>
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
        ///<summary>容器SN</summary>
        public virtual string containerSn { get; set; }
        ///<summary>备料单主表Id</summary>
        public virtual int preparationMainId { get; set; }
        ///<summary>物料Id</summary>
        public virtual int materialId { get; set; }
        ///<summary>数量</summary>
        public virtual decimal quantity { get; set; }
        ///<summary>状态</summary>
        public virtual string status { get; set; }
        ///<summary>作业ID</summary>
        public virtual int wipEntityId { get; set; }

        public AscmWmsContainerDelivery() { }
        public AscmWmsContainerDelivery(AscmWmsContainerDelivery containerDelivery, string compounder, string preparationStatus)
        {
            this.id = containerDelivery.id;
            this.organizationId = containerDelivery.organizationId;
            this.createUser = containerDelivery.createUser;
            this.createTime = containerDelivery.createTime;
            this.modifyUser = containerDelivery.modifyUser;
            this.modifyTime = containerDelivery.modifyTime;
            this.containerSn = containerDelivery.containerSn;
            this.preparationMainId = containerDelivery.preparationMainId;
            this.materialId = containerDelivery.materialId;
            this.quantity = containerDelivery.quantity;
            this.status = containerDelivery.status;
            this.wipEntityId = containerDelivery.wipEntityId;

            this.compounder = compounder;
            this.preparationStatus = preparationStatus;
        }
        
        //辅助属性
        /// <summary>用于统计作业备料监控中已配料人员</summary>
        public string compounder { get; set; }
        /// <summary>备料单状态(业务要求根据备料单状态统计作业备料监控中已配料人员)</summary>
        public string preparationStatus { get; set; }
        public string wipEntityName { get; set; }
        public string materialDocNumber { get; set; }
        public string materialName { get; set; }
        /// <summary>手持端传送的备料数量</summary>
        public decimal sendQuantity { get; set; }
        public int statusSn { get { return StatusDefine.GetSortNo(status); } }

        public class StatusDefine
        {
            /// <summary></summary>
            public static readonly string none = "";
            /// <summary>出仓库</summary>
            public static readonly string outWarehouseDoor = "outWarehouseDoor";

            public static string DisplayText(string status)
            {
                switch (status)
                {
                    case "none": return "";
                    case "outWarehouseDoor": return "出仓库";
                    default: return status;
                }
            }
            public static int GetSortNo(string status)
            {
                switch (status)
                {
                    case "none": return 0;
                    case "outWarehouseDoor": return 1;
                    default: return 0;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(none);
                list.Add(outWarehouseDoor);
                return list;
            }
        }
    }
}
