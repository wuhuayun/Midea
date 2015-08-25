using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>退料明细表</summary>
    public class AscmWmsMtlReturnDetail
    {
        /// <summary>ID</summary>
        public virtual int id { get; set; }
        /// <summary>创建人</summary>
        public virtual string createUser { get; set; }
        /// <summary>创建时间</summary>
        public virtual string createTime { get; set; }
        /// <summary>最后更新人</summary>
        public virtual string modifyUser { get; set; }
        /// <summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }
        /// <summary>主表ID</summary>
        public virtual int mainId { get; set; }
        /// <summary>物料ID</summary>
        public virtual int materialId { get; set; }
        /// <summary>子库ID（领料单退料需在明细中指定子库）</summary>
        public virtual string warehouseId { get; set; }
        /// <summary>货位ID</summary>
        public virtual int warelocationId { get; set; }
        /// <summary>退料数量</summary>
        public virtual decimal quantity { get; set; }
        ///<summary>需求数量</summary>
        public virtual decimal requiredQuantity { get; set; }
        ///<summary>发料数量</summary>
        public virtual decimal quantityIssued { get; set; }

        //辅助信息
        public AscmMaterialItem ascmMaterialItem { get; set; }
        public string materialDocNumber { get { if (ascmMaterialItem != null) return ascmMaterialItem.docNumber; return string.Empty; } }
        public string materialDescription { get { if (ascmMaterialItem != null) return ascmMaterialItem.description; return string.Empty; } }
        public string materialUnit { get { if (ascmMaterialItem != null) return ascmMaterialItem.unit; return string.Empty; } }
        public AscmWarelocation ascmWarelocation { get; set; }
        public string locationDocNumber { get { if (ascmWarelocation != null) return ascmWarelocation.docNumber; return string.Empty; } }
        public string warehouseIdName { get { return warehouseId; } }
        public string warehouseName { get; set; }
        ///<summary>差异数量=需求数量-发料数量</summary>
        public decimal quantityDifference { get { return requiredQuantity - quantityIssued; } }
        public string warelocationDoc { get; set; }
        public int wipEntityId { get; set; }
        //发料计划数
        public decimal printQuantity { get; set; }
    }
}
