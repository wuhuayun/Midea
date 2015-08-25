using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.SupplierPreparation.Entities
{
    ///<summary>司机送货备料</summary>
    public class AscmDriverDelivery
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
        ///<summary>>司机SN</summary>
        public virtual string driverSn { get; set; }
        ///<summary>合单Id</summary>
        public virtual int batSumMainId { get; set; }
        ///<summary>批送货单Id</summary>
        public virtual int deliveryOrderBatchId { get; set; }
        ///<summary>送货单Id</summary>
        public virtual int deliveryOrderId { get; set; }
        ///<summary>送货单物料Id</summary>
        public virtual int materialId { get; set; }
        ///<summary>数量</summary>
        public virtual decimal quantity { get; set; }
        ///<summary>状态</summary>
        public virtual string status { get; set; }
        ///<summary>备注</summary>
        public virtual string memo { get; set; }

        //辅助属性
        public virtual AscmMaterialItem ascmMaterialItem { get; set; }
        public virtual string materialDocNumber { get { if (ascmMaterialItem != null && ascmMaterialItem.docNumber != null) return ascmMaterialItem.docNumber.Trim(); return ""; } }
        public virtual string materialName { get { if (ascmMaterialItem != null && ascmMaterialItem.description != null) return ascmMaterialItem.description.Trim(); return ""; } }

    }
}
