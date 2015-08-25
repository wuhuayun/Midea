using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>仓库领料单明细表</summary>
    public class AscmWmsMtlRequisitionDetail
    {
        /// <summary>ID</summary>
        public virtual int id { get; set; }
        ///<summary>主表ID</summary>
        public virtual int mainId { get; set; }
        /// <summary>物料ID</summary>
        public virtual int materialId { get; set; }
        /// <summary>供应子库</summary>
        public virtual string warehouseId { get; set; }
        /// <summary>货位ID</summary>
        public virtual int warelocationId { get; set; }
        /// <summary>实领数量</summary>
        public virtual decimal quantity { get; set; }
        ///<summary>返回代码</summary>
        public virtual int returnCode { get; set; }
        ///<summary>返回信息</summary>
        public virtual string returnMessage { get; set; }

        public AscmWmsMtlRequisitionDetail() { }
        public AscmWmsMtlRequisitionDetail(AscmWmsMtlRequisitionDetail ascmWmsMtlRequisitionDetail, string wipEntityName)
        {
            this.id = ascmWmsMtlRequisitionDetail.id;
            this.mainId = ascmWmsMtlRequisitionDetail.mainId;
            this.materialId = ascmWmsMtlRequisitionDetail.materialId;
            this.warehouseId = ascmWmsMtlRequisitionDetail.warehouseId;
            this.warelocationId = ascmWmsMtlRequisitionDetail.warelocationId;
            this.quantity = ascmWmsMtlRequisitionDetail.quantity;

            this.wipEntityName = wipEntityName;
        }

        //辅助信息
        public virtual AscmMaterialItem ascmMaterialItem { get; set; }
        public virtual string materialDocNumber { get { if (ascmMaterialItem != null && ascmMaterialItem.docNumber != null) return ascmMaterialItem.docNumber.Trim(); return ""; } }
        public virtual string materialName { get { if (ascmMaterialItem != null && ascmMaterialItem.description != null) return ascmMaterialItem.description.Trim(); return ""; } }
        public virtual string materialUnit { get { if (ascmMaterialItem != null && ascmMaterialItem.unit != null) return ascmMaterialItem.unit.Trim(); return ""; } }
        public virtual AscmWarelocation ascmWarelocation { get; set; }
        public virtual string warelocationdocNumber { get { if (ascmWarelocation != null && ascmWarelocation.docNumber != null) return ascmWarelocation.docNumber.Trim(); return ""; } }
        public string wipEntityName { get; set; }

        public class ReturnCodeDefine
        {
            public const int error = -1;
            public const int correct = 0;
        }
    }
}
