using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Warehouse.Entities
{
    public class AscmWmsBackInvoiceLink
    {
        ///<summary>ID</summary>
        public virtual int id { get; set; }
        ///<summary>退货明细ID</summary>
        public virtual int detailId { get; set; }
        ///<summary>送货批ID</summary>
        public virtual int batchId { get; set; }
        ///<summary>送货单头ID</summary>
        public virtual int deliOrderMainId { get; set; }
        ///<summary>送货单条码</summary>
        public virtual string barCode { get; set; }
        ///<summary>送货单行表ID</summary>
        public virtual int deliOrderDetailId { get; set; }
        ///<summary>子库</summary>
        public virtual string warehouseId { get; set; }
        ///<summary>物料Id</summary>
        public virtual int materialId { get; set; }
        ///<summary>物料编码</summary>
        public virtual string materialDocNumber { get; set; }
        ///<summary>送货数量</summary>
        public virtual decimal deliveryQuantity { get; set; }
        ///<summary>退货数量</summary>
        public virtual decimal rejectQuantity { get; set; }
    }
}
