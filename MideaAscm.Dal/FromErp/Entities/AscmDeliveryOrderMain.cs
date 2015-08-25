using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.FromErp.Entities
{
    public class AscmDeliveryOrderMain
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
        ///<summary>作业ID</summary>
        public virtual int wipEntityId { get; set; }
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
        ///<summary>注释</summary>
        public virtual string comments { get; set; }
        ///<summary>出货子库</summary>
        public virtual string supperWarehouse { get; set; }
        ///<summary>批送货单Id</summary>
        public virtual int batchId { get; set; }
        ///<summary>批条码</summary>
        public virtual string batchBarCode { get; set; }
        
        public AscmDeliveryOrderMain()
        {
        }
        public AscmDeliveryOrderMain(AscmDeliveryOrderMain ascmDeliveryOrderMain, string wipEntityName, decimal totalNumber)
        {
            this.id = ascmDeliveryOrderMain.id;
            this.organizationId = ascmDeliveryOrderMain.organizationId;
            this.createUser = ascmDeliveryOrderMain.createUser;
            this.createTime = ascmDeliveryOrderMain.createTime;
            this.modifyUser = ascmDeliveryOrderMain.modifyUser;
            this.modifyTime = ascmDeliveryOrderMain.modifyTime;
            this.docNumber = ascmDeliveryOrderMain.docNumber;
            this.barCode = ascmDeliveryOrderMain.barCode;
            this.wipEntityId = ascmDeliveryOrderMain.wipEntityId;
            this.supplierId = ascmDeliveryOrderMain.supplierId;
            this.warehouseId = ascmDeliveryOrderMain.warehouseId;
            this.materialId = ascmDeliveryOrderMain.materialId;
            this.status = ascmDeliveryOrderMain.status;
            this.deliveryTime = ascmDeliveryOrderMain.deliveryTime;
            this.comments = ascmDeliveryOrderMain.comments;
            this.supperWarehouse = ascmDeliveryOrderMain.supperWarehouse;
            this.batchId = ascmDeliveryOrderMain.batchId;
            this.batchBarCode = ascmDeliveryOrderMain.batchBarCode;

            this.wipEntity = wipEntityName;
            this.totalNumber = totalNumber;
        }
        public AscmDeliveryOrderMain(AscmDeliveryOrderMain ascmDeliveryOrderMain, long detailCount, long totalNumber)
        {
            this.id = ascmDeliveryOrderMain.id;
            this.organizationId = ascmDeliveryOrderMain.organizationId;
            this.createUser = ascmDeliveryOrderMain.createUser;
            this.createTime = ascmDeliveryOrderMain.createTime;
            this.modifyUser = ascmDeliveryOrderMain.modifyUser;
            this.modifyTime = ascmDeliveryOrderMain.modifyTime;
            this.docNumber = ascmDeliveryOrderMain.docNumber;
            this.barCode = ascmDeliveryOrderMain.barCode;
            this.wipEntityId = ascmDeliveryOrderMain.wipEntityId;
            this.supplierId = ascmDeliveryOrderMain.supplierId;
            this.warehouseId = ascmDeliveryOrderMain.warehouseId;
            this.materialId = ascmDeliveryOrderMain.materialId;
            this.status = ascmDeliveryOrderMain.status;
            this.deliveryTime = ascmDeliveryOrderMain.deliveryTime;
            this.comments = ascmDeliveryOrderMain.comments;
            this.supperWarehouse = ascmDeliveryOrderMain.supperWarehouse;
            this.batchId = ascmDeliveryOrderMain.batchId;
            this.batchBarCode = ascmDeliveryOrderMain.batchBarCode;

            this.detailCount = detailCount;
            this.totalNumber = totalNumber;
        }
        public AscmDeliveryOrderMain(int id, int wipEntityId, int batchId, decimal totalNumber)
        {
            this.id = id;
            this.wipEntityId = wipEntityId;
            this.batchId = batchId;
            this.totalNumber = totalNumber;
        }
        public AscmDeliveryOrderMain(int id, int wipEntityId, string wipEntityName, int wipEntityStatus)
        {
            this.id = id;
            this.wipEntityId = wipEntityId;
            this.wipEntity = wipEntityName;
            this.wipEntityStatus = wipEntityStatus;
        }


        public AscmDeliveryOrderMain GetOwner()
        {
            return (AscmDeliveryOrderMain)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _deliveryTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(deliveryTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(deliveryTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual AscmSupplier ascmSupplier { get; set; }
        public virtual string supplierName { get { if (ascmSupplier != null && ascmSupplier.name!=null) return ascmSupplier.name.Trim(); return ""; } }
        public virtual string supplierDocNumber { get { if (ascmSupplier != null && ascmSupplier.docNumber != null) return ascmSupplier.docNumber.Trim(); return ""; } }
        public virtual AscmDeliveryOrderBatch ascmDeliveryOrderBatch { get; set; }
        public virtual string batchDocNumber { get { if (ascmDeliveryOrderBatch != null && ascmDeliveryOrderBatch.docNumber != null) return ascmDeliveryOrderBatch.docNumber.Trim(); return ""; } }
        public virtual string batchBarCodeShow 
        {
            get 
            {
                if (string.IsNullOrEmpty(batchBarCode))
                {
                    if (ascmDeliveryOrderBatch != null && ascmDeliveryOrderBatch.barCode != null) return ascmDeliveryOrderBatch.barCode.Trim(); return "";
                }
                return batchBarCode;
            } 
        }
        public virtual string statusCn { get { return StatusDefine.DisplayText(status); } }
        public virtual AscmWipEntities ascmWipEntities { get; set; }
        public virtual string wipEntityName { get { if (ascmWipEntities != null) return ascmWipEntities.name; return ""; } }

        public string wipEntity { get; set; }
        public int wipEntityStatus { get; set; }

        /// <summary>总数量(货品总数量)</summary>
        public virtual decimal totalNumber { get; set; }
        public virtual long detailCount { get; set; }
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

    }
}
