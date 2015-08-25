using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>分配货位</summary>
    public class AscmAssignWarelocation
    {
        /// <summary>ID</summary>
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
        ///<summary>批送货单标识</summary>
        public virtual int batchId { get; set; }
        ///<summary>送货单批条码号</summary>
        public virtual string batchBarCode { get; set; }
        ///<summary>批送货单号</summary>
        public virtual string batchDocNumber { get; set; }
        ///<summary>货位ID</summary>
        public virtual int warelocationId { get; set; }
        ///<summary>分配数量</summary>
        public virtual decimal assignQuantity { get; set; }
        ///<summary>物料ID</summary>
        public virtual int materialId { get; set; }

        public AscmAssignWarelocation() { }
        public AscmAssignWarelocation(AscmAssignWarelocation assignWarelocation, string locationDocNumber) 
        {
            GetOwner(assignWarelocation);

            this.locationDocNumber = locationDocNumber;
        }
        public AscmAssignWarelocation(AscmAssignWarelocation assignWarelocation, string materialDocNumber, string materialDescription, string warehouseId, string locationDocNumber)
        {
            GetOwner(assignWarelocation);

            this.materialDocNumber = materialDocNumber;
            this.materialDescription = materialDescription;
            this.warehouseId = warehouseId;
            this.locationDocNumber = locationDocNumber;
        }
        public void GetOwner(AscmAssignWarelocation assignWarelocation)
        {
            this.id = assignWarelocation.id;
            this.organizationId = assignWarelocation.organizationId;
            this.createUser = assignWarelocation.createUser;
            this.createTime = assignWarelocation.createTime;
            this.modifyUser = assignWarelocation.modifyUser;
            this.modifyTime = assignWarelocation.modifyTime;
            this.batchId = assignWarelocation.batchId;
            this.batchBarCode = assignWarelocation.batchBarCode;
            this.batchDocNumber = assignWarelocation.batchDocNumber;
            this.warelocationId = assignWarelocation.warelocationId;
            this.assignQuantity = assignWarelocation.assignQuantity;
        }

        //辅助信息
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
        /// <summary>货位编码</summary>
        public string locationDocNumber { get; set; }
        /// <summary>物料大类</summary>
        public string categoryCode { get; set; }
        /// <summary>物料编码</summary>
        public string materialDocNumber { get; set; }
        /// <summary>货位已存数量</summary>
        public decimal quantity { get; set; }

        /// <summary>物料描述</summary>
        public string materialDescription { get; set; }
        /// <summary>子库</summary>
        public string warehouseId { get; set; }

        /// <summary>供应商简称</summary>
        public string supplierShortName { get; set; }
    }
}
