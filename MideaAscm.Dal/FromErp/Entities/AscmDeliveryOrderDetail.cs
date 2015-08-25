using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.FromErp.Entities
{
    public class AscmDeliveryOrderDetail
    {
        ///<summary>id</summary>
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
        ///<summary>mainId</summary>
        public virtual int mainId { get; set; }
        ///<summary>物料Id</summary>
        public virtual int materialId { get; set; }
        ///<summary>通知Id</summary>
        public virtual int notifyDetailId { get; set; }
        ///<summary>编号</summary>
        public virtual int lineNumber { get; set; }
        ///<summary>送货数量</summary>
        public virtual decimal deliveryQuantity { get; set; }
        ///<summary>接收数量</summary>
        public virtual decimal receivedQuantity { get; set; }

        public AscmDeliveryOrderDetail GetOwner()
        {
            return (AscmDeliveryOrderDetail)this.MemberwiseClone();
        }
        public AscmDeliveryOrderDetail() { }
        public AscmDeliveryOrderDetail(AscmDeliveryOrderDetail deliveryOrderDetail, int batchId) 
        {
            this.id = deliveryOrderDetail.id;
            this.mainId = deliveryOrderDetail.mainId;
            this.materialId = deliveryOrderDetail.materialId;
            this.notifyDetailId = deliveryOrderDetail.notifyDetailId;
            this.lineNumber = deliveryOrderDetail.lineNumber;
            this.deliveryQuantity = deliveryOrderDetail.deliveryQuantity;
            this.receivedQuantity = deliveryOrderDetail.receivedQuantity;

            this.batchId = batchId;
        }
        public AscmDeliveryOrderDetail(AscmDeliveryOrderDetail deliveryOrderDetail, int batchId, int wipEntityId)
        {
            this.id = deliveryOrderDetail.id;
            this.mainId = deliveryOrderDetail.mainId;
            this.materialId = deliveryOrderDetail.materialId;
            this.notifyDetailId = deliveryOrderDetail.notifyDetailId;
            this.lineNumber = deliveryOrderDetail.lineNumber;
            this.deliveryQuantity = deliveryOrderDetail.deliveryQuantity;
            this.receivedQuantity = deliveryOrderDetail.receivedQuantity;

            this.batchId = batchId;
            this.wipEntityId = wipEntityId;
        }

        //辅助信息
        public virtual string createTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string modifyTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual AscmMaterialItem ascmMaterialItem { get; set; }
        public virtual string materialDocNumber { get { if (ascmMaterialItem != null && ascmMaterialItem.docNumber != null) return ascmMaterialItem.docNumber.Trim(); return ""; } }
        public virtual string materialName { get { if (ascmMaterialItem != null && ascmMaterialItem.description != null) return ascmMaterialItem.description.Trim(); return ""; } }
        public virtual string materialUnit { get { if (ascmMaterialItem != null && ascmMaterialItem.unit != null) return ascmMaterialItem.unit.Trim(); return ""; } }
        public virtual AscmDeliveryNotifyDetail ascmDeliveryNotifyDetail { get; set; }
        public virtual string ascmDeliveryNotifyDetail_needTime { get { if (ascmDeliveryNotifyDetail != null && ascmDeliveryNotifyDetail.ascmDeliveryNotifyMain_needTime != null) return ascmDeliveryNotifyDetail.ascmDeliveryNotifyMain_needTime.Trim(); return ""; } }
        
        public virtual AscmDeliveryOrderMain ascmDeliveryOrderMain { get; set; }
        public virtual string mainDocNumber { get { if (ascmDeliveryOrderMain != null && ascmDeliveryOrderMain.docNumber != null) return ascmDeliveryOrderMain.docNumber.Trim(); return ""; } }
        public virtual string mainDeliveryTime { get { if (ascmDeliveryOrderMain != null ) return ascmDeliveryOrderMain._deliveryTime; return ""; } }
        public virtual string mainBarCode { get { if (ascmDeliveryOrderMain != null && ascmDeliveryOrderMain.barCode != null) return ascmDeliveryOrderMain.barCode.Trim(); return ""; } }
        public virtual string supplierDocNumber { get { if (ascmDeliveryOrderMain != null) return ascmDeliveryOrderMain.supplierDocNumber.Trim(); return ""; } }
        public virtual string supplierName { get { if (ascmDeliveryOrderMain != null) return ascmDeliveryOrderMain.supplierName.Trim(); return ""; } }
        public virtual string warehouseId { get { if (ascmDeliveryOrderMain != null && ascmDeliveryOrderMain.warehouseId != null) return ascmDeliveryOrderMain.warehouseId.Trim(); return ""; } }
        public virtual string mainStatusCn { get { if (ascmDeliveryOrderMain != null) return ascmDeliveryOrderMain.statusCn.Trim(); return ""; } }
        public virtual string mainBatchDocNumber { get { if (ascmDeliveryOrderMain != null && ascmDeliveryOrderMain.batchDocNumber != null) return ascmDeliveryOrderMain.batchDocNumber.Trim(); return ""; } }
        public virtual string mainBatchBarCode { get { if (ascmDeliveryOrderMain != null && ascmDeliveryOrderMain.batchBarCodeShow != null) return ascmDeliveryOrderMain.batchBarCodeShow.Trim(); return ""; } }
        public virtual string wipEntityName { get { if (ascmDeliveryOrderMain != null && !string.IsNullOrEmpty(ascmDeliveryOrderMain.wipEntityName)) return ascmDeliveryOrderMain.wipEntityName; return ""; } }

        public int batchId { get; set; }
        public int wipEntityId { get; set; }
    }
}
