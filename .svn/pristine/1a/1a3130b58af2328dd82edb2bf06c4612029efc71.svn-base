using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Dal.SupplierPreparation.Entities
{
    /// <summary>送货合单明细与送货单明细关联</summary>
    public class AscmDeliBatOrderLink
    {
        ///<summary>送货单Line表ID</summary>
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
        ///<summary>送货批ID</summary>
        public virtual int batchId { get; set; }
        ///<summary>送货单批条码号</summary>
        public virtual string batchBarCode { get; set; }
        ///<summary>送货单批号</summary>
        public virtual string batchDocNumber { get; set; }
        ///<summary>收货子库</summary>
        public virtual string warehouseId { get; set; }
        ///<summary>送货单头ID</summary>
        public virtual int mainId { get; set; }
        ///<summary>送货数量</summary>
        public virtual decimal deliveryQuantity { get; set; }
        ///<summary>接收数量</summary>
        public virtual decimal receivedQuantity { get; set; }
        ///<summary>送货单批注释</summary>
        public virtual string batchComments { get; set; }
        ///<summary>作业ID</summary>
        public virtual int wipEntityId { get; set; }
        ///<summary>物料Id</summary>
        public virtual int materialId { get; set; }
		/// <summary>上传返回代码</summary>
		public virtual string returnCode { get; set; }
		/// <summary>上传返回消息</summary>
		public virtual string returnMessage { get; set; }
		
		public string uploadStatusCn
		{
			get
			{
				if (string.IsNullOrEmpty(returnCode)) return "";
				if (returnCode == "0") return "成功！";

				return "失败！";
			}
		}

		/// <summary>上传日期</summary>
		public virtual string uploadTime { get; set; }

		//上传日期
		public string uploadTimeShow
		{
			get
			{
				if (YnBaseClass2.Helper.DateHelper.GetDateTime(uploadTime) != null)
					return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(uploadTime)).ToString("yyyy-MM-dd");
				return "";
			}
		}

        public AscmDeliBatOrderLink() { }
        public AscmDeliBatOrderLink(AscmDeliBatOrderLink deliBatOrderLink, string wipEntityName, int wipEntityStatus, string materialDocNumber, string materialName, string materialUnit) 
        {
            this.id = deliBatOrderLink.id;
            this.organizationId = deliBatOrderLink.organizationId;
            this.createUser = deliBatOrderLink.createUser;
            this.createTime = deliBatOrderLink.createTime;
            this.modifyUser = deliBatOrderLink.modifyUser;
            this.modifyTime = deliBatOrderLink.modifyTime;
            this.batchId = deliBatOrderLink.batchId;
            this.batchBarCode = deliBatOrderLink.batchBarCode;
            this.batchDocNumber = deliBatOrderLink.batchDocNumber;
            this.warehouseId = deliBatOrderLink.warehouseId;
            this.mainId = deliBatOrderLink.mainId;
            this.deliveryQuantity = deliBatOrderLink.deliveryQuantity;
            this.receivedQuantity = deliBatOrderLink.receivedQuantity;
            this.batchComments = deliBatOrderLink.batchComments;
            this.wipEntityId = deliBatOrderLink.wipEntityId;
            this.materialId = deliBatOrderLink.materialId;

            this.wipEntityName = wipEntityName;
            this.wipEntityStatus = wipEntityStatus;
            this._materialDocNumber = materialDocNumber;
            this._materialName = materialName;
            this._materialUnit = materialUnit;
        }

        //辅助信息
        public AscmDeliveryOrderDetail ascmDeliveryOrderDetail { get; set; }

        private string _materialDocNumber;
        public string materialDocNumber { get { if (ascmDeliveryOrderDetail != null && ascmDeliveryOrderDetail.ascmMaterialItem != null) return ascmDeliveryOrderDetail.ascmMaterialItem.docNumber; return _materialDocNumber; } }
        private string _materialName;
        public string materialName { get { if (ascmDeliveryOrderDetail != null && ascmDeliveryOrderDetail.ascmMaterialItem != null) return ascmDeliveryOrderDetail.ascmMaterialItem.description; return _materialName; } }
        private string _materialUnit;
        public string materialUnit { get { if (ascmDeliveryOrderDetail != null && ascmDeliveryOrderDetail.ascmMaterialItem != null) return ascmDeliveryOrderDetail.ascmMaterialItem.unit; return _materialUnit; } }
        
        public string ascmDeliveryNotifyDetail_needTime { get { if (ascmDeliveryOrderDetail != null && ascmDeliveryOrderDetail.ascmDeliveryNotifyDetail != null) return ascmDeliveryOrderDetail.ascmDeliveryNotifyDetail.ascmDeliveryNotifyMain_needTime; return ""; } }

        public string mainDocNumber { get { if (ascmDeliveryOrderDetail != null && ascmDeliveryOrderDetail.ascmDeliveryOrderMain != null) return ascmDeliveryOrderDetail.ascmDeliveryOrderMain.docNumber; return ""; } }
        public string mainDeliveryTime { get { if (ascmDeliveryOrderDetail != null && ascmDeliveryOrderDetail.ascmDeliveryOrderMain != null) return ascmDeliveryOrderDetail.ascmDeliveryOrderMain._deliveryTime; return ""; } }
        public string supplierDocNumber { get { if (ascmDeliveryOrderDetail != null && ascmDeliveryOrderDetail.ascmDeliveryOrderMain != null) return ascmDeliveryOrderDetail.ascmDeliveryOrderMain.supplierDocNumber; return ""; } }
        public string supplierName { get { if (ascmDeliveryOrderDetail != null && ascmDeliveryOrderDetail.ascmDeliveryOrderMain != null) return ascmDeliveryOrderDetail.ascmDeliveryOrderMain.supplierName; return ""; } }
        public string mainStatusCn { get { if (ascmDeliveryOrderDetail != null && ascmDeliveryOrderDetail.ascmDeliveryOrderMain != null) return ascmDeliveryOrderDetail.ascmDeliveryOrderMain.statusCn; return ""; } }

        /// <summary>作业号</summary>
        public string wipEntityName { get; set; }
        /// <summary>作业状态</summary>
        public int wipEntityStatus { get; set; }
        public string wipEntityStatusCn { get { if (wipEntityStatus == 0) return ""; return AscmWipDiscreteJobs.StatusTypeDefine.DisplayText(wipEntityStatus); } }
	}
}
