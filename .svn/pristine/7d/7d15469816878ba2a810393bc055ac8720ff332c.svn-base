using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.FromErp.Entities
{
    ///<summary>作业信息_现有量</summary>
    public class AscmMtlOnhandQuantitiesDetail
    {
        ///<summary>id</summary>
        public virtual int id { get; set; }
        ///<summary>inventoryItemId</summary>
        public virtual int inventoryItemId { get; set; }
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
        ///<summary>子库</summary>
        public virtual string subinventoryCode { get; set; }
        ///<summary>数量</summary>
        public virtual decimal transactionQuantity { get; set; }

        public AscmMtlOnhandQuantitiesDetail GetOwner()
        {
            return (AscmMtlOnhandQuantitiesDetail)this.MemberwiseClone();
        }
        public AscmMtlOnhandQuantitiesDetail()
        {
        }
        public AscmMtlOnhandQuantitiesDetail(int inventoryItemId, decimal transactionQuantity)
        {
            this.inventoryItemId = inventoryItemId;
            this.transactionQuantity = transactionQuantity;

        }
        public AscmMtlOnhandQuantitiesDetail(int inventoryItemId, string subinventoryCode, decimal transactionQuantity)
        {
            this.inventoryItemId = inventoryItemId;
            this.subinventoryCode = subinventoryCode;
            this.transactionQuantity = transactionQuantity;
        }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual AscmMaterialItem ascmMaterialItem { get; set; }
        public virtual string ascmMateiralItem_Docnumber { get { if (ascmMaterialItem != null) return ascmMaterialItem.docNumber; return ""; } }
        public virtual string ascmMaterialItem_Description { get { if (ascmMaterialItem != null) return ascmMaterialItem.description; return ""; } }
        public virtual string ascmMaterialItem_Unit { get { if (ascmMaterialItem != null) return ascmMaterialItem.unit; return ""; } }

    }
}
