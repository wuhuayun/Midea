using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>货位与物料关联</summary>
    public class AscmLocationMaterialLink
    {
        /// <summary>货位物料联合主键</summary>
        public virtual AscmLocationMaterialLinkPK pk { get; set; }
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
        /// <summary>存放货物数量</summary>
        public virtual decimal quantity { get; set; }

        public AscmLocationMaterialLink() { }
        public AscmLocationMaterialLink(AscmLocationMaterialLink locationMaterialLink,
            string materialDocNumber, string materialDescription,
            string locationDocNumber, string warehouseId)
        {
            this.pk = locationMaterialLink.pk;
            this.organizationId = locationMaterialLink.organizationId;
            this.createUser = locationMaterialLink.createUser;
            this.createTime = locationMaterialLink.createUser;
            this.modifyUser = locationMaterialLink.modifyUser;
            this.modifyTime = locationMaterialLink.modifyTime;

            this.materialDocNumber = materialDocNumber;
            this.materialDescription = materialDescription;
            this.locationDocNumber = locationDocNumber;
            this.warehouseId = warehouseId;
        }

        //辅助信息
        //public AscmMaterialItem ascmMaterialItem { get; set; }
        //public string materialDocNumber { get { if (ascmMaterialItem != null) return ascmMaterialItem.docNumber; return ""; } }
        //public string materialName { get { if (ascmMaterialItem != null) return ascmMaterialItem.description; return ""; } }
        //public AscmWarelocation ascmWarelocation { get; set; }
        //public string locationDocNumber { get { if (ascmWarelocation != null) return ascmWarelocation.docNumber; return ""; } }
        //public string warehouseId { get { if (ascmWarelocation != null) return ascmWarelocation.warehouseId; return ""; } }

        public string materialDocNumber { get; set; }
        public string materialDescription { get; set; }
        public string materialName 
        { 
            get 
            {
                return materialDescription;
            } 
        }

        public string locationDocNumber { get; set; }
        public string warehouseId { get; set; }
    }
}
