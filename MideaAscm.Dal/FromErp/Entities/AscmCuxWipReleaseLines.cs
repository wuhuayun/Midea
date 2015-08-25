using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.FromErp.Entities
{
    ///<summary>领料单line表</summary>
    public class AscmCuxWipReleaseLines
    {
        ///<summary>id</summary>
        public virtual int releaseLineId  { get; set; }
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
        ///<summary>标题头</summary>
        public virtual int releaseHeaderId { get; set; }
        ///<summary>line数量</summary>
        public virtual int lineNumber { get; set; }
        ///<summary>作业</summary>
        public virtual int wipEntityId { get; set; }
        ///<summary>物料</summary>
        public virtual int inventoryItemId { get; set; }
        ///<summary>?</summary>
        public virtual int operationSeqNum { get; set; }
        ///<summary>子库</summary>
        public virtual string subInventory { get; set; }
        ///<summary>打印数量，计划数量</summary>
        public virtual int printQuantity { get; set; }

        public AscmCuxWipReleaseLines GetOwner()
        {
            return (AscmCuxWipReleaseLines)this.MemberwiseClone();
        }
        ///<summary>?</summary>

        //辅助信息

        public AscmMaterialItem ascmMaterialItem { get; set; }
        public string materialDocNumber { get { if (ascmMaterialItem != null && ascmMaterialItem.docNumber != null) return ascmMaterialItem.docNumber.Trim(); return ""; } }
        public string materialName { get { if (ascmMaterialItem != null && ascmMaterialItem.description != null) return ascmMaterialItem.description.Trim(); return ""; } }
        public string materialUnit { get { if (ascmMaterialItem != null && ascmMaterialItem.unit != null) return ascmMaterialItem.unit.Trim(); return ""; } }

        public AscmWipRequirementOperations ascmWipRequirementOperations { get; set; }
        public string ascmWipRequirementOperations_wipSupplyTypeCn { get { if (ascmWipRequirementOperations != null) return ascmWipRequirementOperations.wipSupplyTypeCn; return ""; } }

        public AscmCuxWipReleaseHeaders ascmCuxWipReleaseHeaders { get; set; }
        public string ascmCuxWipReleaseHeaders_releaseNumber { get { if (ascmCuxWipReleaseHeaders != null) return ascmCuxWipReleaseHeaders.releaseNumber; return ""; } }

        ///<summary>未发数</summary>
        //使用方法：Select decode（columnname，值1,翻译值1,值2,翻译值2,...值n,翻译值n,缺省值）
        //在 Oracle/PLSQL,  sign 函数返回一个值来说明数字的符号，
        //语法如下sign( number )
        //If number < 0, then sign returns -1
        //If number = 0, then sign returns 0
        //If number > 0, then sign returns 1
        /*DECODE(WRH.RELEASE_TYPE,
              'RETURN',
              WRO.QUANTITY_ISSUED,
              DECODE(SIGN(WRO.REQUIRED_QUANTITY),
                     -1 * SIGN(WRO.QUANTITY_ISSUED),
                     (WRO.REQUIRED_QUANTITY - WRO.QUANTITY_ISSUED),
                     DECODE(SIGN(ABS(WRO.REQUIRED_QUANTITY) -
                                 ABS(WRO.QUANTITY_ISSUED)),
                            -1,
                            NULL,
                            (WRO.REQUIRED_QUANTITY - WRO.QUANTITY_ISSUED)))) QUANTITY_AV
         */
        public virtual decimal QUANTITY_AV { get; set; }
        ///<summary>现有量</summary>
        /*现有量 
            select sum(transaction_quantity)
            from mtl_onhand_quantities
           where inventory_item_id = :inventory_item_id
             and organization_id = :organization_id
             and subinventory_code = :subinventory;*/
        //mtl_onhand_quantities:INV.MTL_ONHAND_QUANTITIES_DETAIL
        public virtual decimal transaction_quantity { get; set; }
        ///<summary>接收中</summary>
        /*SELECT SUM(to_org_primary_quantity)
	        FROM rcv_supply   rs
	        WHERE rs.item_id  = :inventory_item_id
          AND rs.to_organization_id = :organization_id;*/
        //PO.RCV_SUPPLY 
        public virtual decimal to_org_primary_quantity { get; set; }

        //public virtual string wipSupplyTypeCn
        //{ 
        //    get 
        //    {
        //        if (ascmWipRequirementOperations != null)
        //        {
        //            if (ascmWipRequirementOperations.wipSupplyType == 1) 
        //                return "推式"; 
        //            else if (ascmWipRequirementOperations.wipSupplyType == 2) 
        //                return "拉式";
        //        }
        //        return "";
        //    }
        //}
    }
}
