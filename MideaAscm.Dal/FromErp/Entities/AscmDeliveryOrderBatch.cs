using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.FromErp.Entities
{
    ///<summary>批送货单</summary>
    public class AscmDeliveryOrderBatch
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
        ///<summary>编号</summary>
        public virtual string docNumber { get; set; }
        ///<summary>条码</summary>
        public virtual string barCode { get; set; }
        ///<summary>供应商Id</summary>
        public virtual int supplierId { get; set; }
        ///<summary>子库</summary>
        public virtual string warehouseId { get; set; }
        ///<summary>物料Id</summary>
        public virtual int materialId { get; set; }
        ///<summary>状态</summary>
        public virtual string status { get; set; }
        ///<summary>送货时间</summary>
        public virtual string deliveryTime { get; set; }
        ///<summary>安排开始时间</summary>
        public virtual string scheduleStartTime { get; set; }
        ///<summary>注释</summary>
        public virtual string comments { get; set; }
        ///<summary>出货子库</summary>
        public virtual string supperWarehouse { get; set; }
        ///<summary>送货地点</summary>
        public virtual string wipLine { get; set; }
        ///<summary>ASCM状态</summary>
        public virtual string ascmStatus { get; set; }
        ///<summary>子库(接收时实际指定的收货子库)</summary>
        public virtual string actualWarehouseId { get; set; }
		/// <summary>上传返回代码</summary>
		public virtual string returnCode { get; set; }
		/// <summary>上传返回消息</summary>
		public virtual string returnMessage { get; set; }

		//上传状态
		public string uploadStatusCn
		{
			get
			{
				if (string.IsNullOrEmpty(returnCode)) return "";
				if (returnCode == "0") return "成功！";

				return "失败！";
			}
		}

        public AscmDeliveryOrderBatch GetOwner()
        {
            return (AscmDeliveryOrderBatch)this.MemberwiseClone();
        }
        public void GetOwner(AscmDeliveryOrderBatch ascmDeliveryOrderBatch)
        {
            this.id = ascmDeliveryOrderBatch.id;
            this.organizationId = ascmDeliveryOrderBatch.organizationId;
            this.createUser = ascmDeliveryOrderBatch.createUser;
            this.createTime = ascmDeliveryOrderBatch.createTime;
            this.modifyUser = ascmDeliveryOrderBatch.modifyUser;
            this.modifyTime = ascmDeliveryOrderBatch.modifyTime;
            this.docNumber = ascmDeliveryOrderBatch.docNumber;
            this.barCode = ascmDeliveryOrderBatch.barCode;
            this.supplierId = ascmDeliveryOrderBatch.supplierId;
            this.warehouseId = ascmDeliveryOrderBatch.warehouseId;
            this.materialId = ascmDeliveryOrderBatch.materialId;
            this.status = ascmDeliveryOrderBatch.status;
            this.deliveryTime = ascmDeliveryOrderBatch.deliveryTime;
            this.scheduleStartTime = ascmDeliveryOrderBatch.scheduleStartTime;
            this.comments = ascmDeliveryOrderBatch.comments;
            this.supperWarehouse = ascmDeliveryOrderBatch.supperWarehouse;
            this.wipLine = ascmDeliveryOrderBatch.wipLine;
            this.ascmStatus = ascmDeliveryOrderBatch.ascmStatus;
            this.actualWarehouseId = ascmDeliveryOrderBatch.actualWarehouseId;
        }
        public AscmDeliveryOrderBatch()
        {
        }
        public AscmDeliveryOrderBatch(AscmDeliveryOrderBatch ascmDeliveryOrderBatch, long detailCount, decimal totalNumber, int _materialId)
        {
            GetOwner(ascmDeliveryOrderBatch);

            this.detailCount = detailCount;
            this.totalNumber = totalNumber;
            this.materialIdTmp = _materialId;
            this.appointmentStartTime = appointmentStartTime;
            this.appointmentEndTime = appointmentEndTime;
        }
        public AscmDeliveryOrderBatch(AscmDeliveryOrderBatch ascmDeliveryOrderBatch, long detailCount, decimal totalNumber, int _materialId,
            decimal containerBindNumber, decimal palletBindNumber, decimal driverBindNumber, string deliveryStatus)
        {
            GetOwner(ascmDeliveryOrderBatch);

            this.detailCount = detailCount;
            this.totalNumber = totalNumber;
            this.materialIdTmp = _materialId;

            this.containerBindNumber = containerBindNumber;
            this.palletBindNumber = palletBindNumber;
            this.driverBindNumber = driverBindNumber;
            this.deliveryStatus = MideaAscm.Dal.SupplierPreparation.Entities.AscmDeliBatSumMain.StatusDefine.DisplayText(deliveryStatus);
        }
        public AscmDeliveryOrderBatch(AscmDeliveryOrderBatch ascmDeliveryOrderBatch, long detailCount, decimal totalNumber, int _materialId, decimal receivedQuantity)
        {
            GetOwner(ascmDeliveryOrderBatch);

            this.detailCount = detailCount;
            this.totalNumber = totalNumber;
            this.materialIdTmp = _materialId;
            this.receivedQuantity = receivedQuantity;
        }
        public AscmDeliveryOrderBatch(AscmDeliveryOrderBatch ascmDeliveryOrderBatch, int materialIdTmp)
        {
            GetOwner(ascmDeliveryOrderBatch);

            this.materialIdTmp = materialIdTmp;
        }
        public AscmDeliveryOrderBatch(AscmDeliveryOrderBatch ascmDeliveryOrderBatch, string comments)
        {
            GetOwner(ascmDeliveryOrderBatch);

            this.comments = comments;
        }
        public AscmDeliveryOrderBatch(int id, string ispShortName)
        {
            this.id = id;
            this.ispShortName = ispShortName;
        }
        public AscmDeliveryOrderBatch(AscmDeliveryOrderBatch ascmDeliveryOrderBatch, decimal receivedQuantity, decimal checkQuantity)
        {
            GetOwner(ascmDeliveryOrderBatch);

            this.receivedQuantity = receivedQuantity;
            this.checkQuantity = checkQuantity;
        }

        //辅助信息
        public virtual string createTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string modifyTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string deliveryTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(deliveryTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(deliveryTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string ascmStatusCn { get { return AscmStatusDefine.DisplayText(ascmStatus); } }
        public virtual AscmSupplier ascmSupplier { get; set; }
        public virtual string supplierName { get { if (ascmSupplier != null && ascmSupplier.name!=null) return ascmSupplier.name.Trim(); return ""; } }
        public virtual string supplierShortName { get { if (ascmSupplier != null && ascmSupplier.supplierShortName != null) return ascmSupplier.supplierShortName.Trim(); return ""; } }
        public virtual string statusCn { get { return StatusDefine.DisplayText(status); } }
        public string ispShortName { get; set; }
        public virtual AscmMaterialItem ascmMaterialItem { get; set; }
        public virtual string materialDocNumber { get { if (ascmMaterialItem != null && ascmMaterialItem.docNumber != null) return ascmMaterialItem.docNumber.Trim(); return ""; } }
        public virtual string materialName { get { if (ascmMaterialItem != null && ascmMaterialItem.description != null) return ascmMaterialItem.description.Trim(); return ""; } }
        public virtual string materialUnit { get { if (ascmMaterialItem != null && ascmMaterialItem.unit != null) return ascmMaterialItem.unit.Trim(); return ""; } }
        /// <summary>总数量(货品总数量)</summary>
        public virtual decimal totalNumber { get; set; }
        public virtual long detailCount { get; set; }
        ///<summary>物料Id</summary>
        public virtual int materialIdTmp { get; set; }
        ///<summary>容器绑定数量</summary>
        public virtual decimal containerBindNumber { get; set; }
        ///<summary>托盘绑定数量</summary>
        public virtual decimal palletBindNumber { get; set; }
        ///<summary>司机绑定数量</summary>
        public virtual decimal driverBindNumber { get; set; }
        ///<summary>送货状态</summary>
        public virtual string deliveryStatus { get; set; }
        ///<summary>到货接收数量</summary>
        public virtual decimal receivedQuantity { get; set; }
        ///<summary>以字符串形式显示分配的货位</summary>
        public virtual string assignWarelocation { get; set; }
        ///<summary>收货子库</summary>
        public virtual string receivedWarehouseId { get { if (!string.IsNullOrEmpty(actualWarehouseId)) return actualWarehouseId; return warehouseId; } }
        ///<summary>预约开始时间</summary>
        public virtual string appointmentStartTime { get; set; }
        ///<summary>预约最后时间</summary>
        public virtual string appointmentEndTime { get; set; }

        //合单接收新增辅助字段
        ///<summary>合单号</summary>
        public string deliBatSumDocNumber { get; set; }
        ///<summary>合单条码</summary>
        public string deliBatSumBarcode { get; set; }
        /// <summary>入库校验数</summary>
        public decimal checkQuantity { get; set; }

        //状态定义
        public class StatusDefine
        {
            /// <summary>已打开</summary>
            public const string open = "OPEN";
            /// <summary>已关闭</summary>
            public const string closed = "CLOSED";
            /// <summary>已取消</summary>
            public const string cancel = "CANCEL";

            public static string DisplayText(string value)
            {
                if (value == open)
                    return "已打开";
                else if (value == closed)
                    return "已关闭";
                else if (value == cancel)
                    return "已取消";

                return value;
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(open);
                list.Add(closed);
                list.Add(cancel);
                return list;
            }
            public static List<string> GetSupplierList()
            {
                List<string> list = new List<string>();
                list.Add(open);
                //list.Add(closed);
                //list.Add(cancel);
                return list;
            }
        }
        //ASCM状态定义
        public class AscmStatusDefine
        {
            /// <summary>无需求</summary>
            public const string noDemand = "NODEMAND";
            /// <summary>入仓库门</summary>
            public const string enterDoor = "enterDoor";
            /// <summary>已入库</summary>
            public const string inStorage = "INSTORAGE";
            /// <summary>已接收</summary>
            public const string received = "RECEIVED";
            /// <summary>接收失败</summary>
            public const string receiveFail = "RECEIVEFAIL";

            public static string DisplayText(string value)
            {
                switch (value)
                {
                    case noDemand: return "无需求";
                    case enterDoor: return "入仓库门";
                    case inStorage: return "已入库";
                    case received: return "已接收";
                    case receiveFail: return "接收失败";
                    default: return value;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(noDemand);
                list.Add(enterDoor);
                list.Add(inStorage);
                list.Add(received);
                list.Add(receiveFail);
                return list;
            }
        }
    }
}
