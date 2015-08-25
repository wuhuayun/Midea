using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>供应商退货明细表</summary>
    public class AscmWmsBackInvoiceDetail
    {
        ///<summary>ID</summary>
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
        ///<summary>主表ID</summary>
        public virtual int mainId { get; set; }
        ///<summary>送货批ID</summary>
        public virtual int batchId { get; set; }
        ///<summary>物料Id</summary>
        public virtual int materialId { get; set; }
        ///<summary>送货数量</summary>
        public virtual decimal deliveryQuantity { get; set; }
        ///<summary>退货数量</summary>
        public virtual decimal returnQuantity { get; set; }
        ///<summary>货位</summary>
        public virtual int warelocationId { get; set; }
        ///<summary>编号（批单退货batchId!=0,docNumber批条码；手工单退货batchId=0，docNumber手工单号）</summary>
        public virtual string docNumber { get; set; }

        //辅助信息
        public string createTimeShow
        {
            get
            {
                if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null)
                    return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm");
                return "";
            }
        }
        public string modifyTimeShow
        {
            get
            {
                if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null)
                    return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm");
                return "";
            }
        }
        public AscmDeliveryOrderBatch ascmDeliveryOrderBatch { get; set; }
        ///<summary>批送货单编号</summary>
        public string batchDocNumber { get { if (ascmDeliveryOrderBatch != null) return ascmDeliveryOrderBatch.docNumber; return ""; } }
        ///<summary>批送货单条码</summary>
        public string batchBarCode { get { if (ascmDeliveryOrderBatch != null) return ascmDeliveryOrderBatch.barCode; return ""; } }
        ///<summary>生成日期</summary>
        public string batchCreateTime { get { if (ascmDeliveryOrderBatch != null) return ascmDeliveryOrderBatch.createTimeShow; return ""; } }
        ///<summary>收货子库</summary>
        public string batchWarehouseId { get { if (ascmDeliveryOrderBatch != null) return ascmDeliveryOrderBatch.warehouseId; return ""; } }
        public AscmMaterialItem ascmMaterialItem { get; set; }
        ///<summary>物料编码</summary>
        public string materialDocNumber { get { if (ascmMaterialItem != null) return ascmMaterialItem.docNumber; return ""; } }
        ///<summary>物料描述</summary>
        public string materialDescription { get { if (ascmMaterialItem != null) return ascmMaterialItem.description; return ""; } }
        ///<summary>物料单位</summary>
        public string materialUnit { get { if (ascmMaterialItem != null) return ascmMaterialItem.unit; return ""; } }
        ///<summary>货位编码</summary>
        public string warelocationDoc { get; set; }
        //public string batchBarCodeNum { get; set; }
        public string warehouseId { get; set; }
        ///<summary>退货数量和</summary>
        public decimal returnQuantityTotal { get; set; }
    }
}
