using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.FromErp.Entities
{
    public class AscmDeliveryNotifyMain
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
        ///<summary>供应商Id</summary>
        public virtual int supplierId { get; set; }
        ///<summary>子库</summary>
        public virtual string warehouseId { get; set; }
        ///<summary>物料Id</summary>
        public virtual int materialId { get; set; }
        ///<summary>下单数量</summary>
        public virtual int releasedQuantity { get; set; }
        ///<summary>承诺数量</summary>
        public virtual int promisedQuantity { get; set; }
        ///<summary>在途数量</summary>
        public virtual int deliveryQuantity { get; set; }
        ///<summary>取消数量</summary>
        public virtual int cancelQuantity { get; set; }
        ///<summary>状态</summary>
        public virtual string status { get; set; }
        ///<summary>?下单时间</summary>
        public virtual string releasedTime { get; set; }
        ///<summary>?需求时间</summary>
        public virtual string needTime { get; set; }
        ///<summary>?接收时间</summary>
        public virtual string receiveTime { get; set; }
        ///<summary>答应时间</summary>
        public virtual string promisedTime { get; set; }
        ///<summary>批准时间</summary>
        public virtual string confirmTime { get; set; }
        ///<summary>注释</summary>
        public virtual string comments { get; set; }
        ///<summary>采购员</summary>
        public virtual int purchasingAgentId { get; set; }
        ///<summary>作业ID</summary>
        public virtual int wipEntityId { get; set; }
        ///<summary>车间ID</summary>
        public virtual int departmentId { get; set; }
        ///<summary>厂区</summary>
        public virtual int locationId { get; set; }
        ///<summary>fdSourceType</summary>
        public virtual string fdSourceType { get; set; }
        ///<summary>最早到货时间=收货标准</summary>
        public virtual string appointmentStartTime { get; set; }
        ///<summary>预约到货时间=作业时间-卸货时间-检验-发料-配送时间</summary>
        public virtual string appointmentEndTime { get; set; }
        ///<summary>预约状态</summary>
        public virtual string appointmentStatus { get; set; }
        /////<summary>预约人</summary>
        //public virtual string appointmentUser { get; set; }
        /////<summary>预约修改</summary>
        //public virtual string appointmentModifyTime { get; set; }
        ///<summary>ERP总接收数量</summary>
        public virtual int totalReceiveQuantity { get; set; }


        public AscmDeliveryNotifyMain()
        {
        }
        public AscmDeliveryNotifyMain(AscmDeliveryNotifyMain ascmDeliveryNotifyMain, long detailCount)//, long totalNumber
        {
            this.id = ascmDeliveryNotifyMain.id;
            this.organizationId = ascmDeliveryNotifyMain.organizationId;
            this.createUser = ascmDeliveryNotifyMain.createUser;
            this.createTime = ascmDeliveryNotifyMain.createTime;
            this.modifyUser = ascmDeliveryNotifyMain.modifyUser;
            this.modifyTime = ascmDeliveryNotifyMain.modifyTime;
            this.docNumber = ascmDeliveryNotifyMain.docNumber;
            this.supplierId = ascmDeliveryNotifyMain.supplierId;
            this.warehouseId = ascmDeliveryNotifyMain.warehouseId;
            this.materialId = ascmDeliveryNotifyMain.materialId;
            this.releasedQuantity = ascmDeliveryNotifyMain.releasedQuantity;
            this.promisedQuantity = ascmDeliveryNotifyMain.promisedQuantity;
            this.deliveryQuantity = ascmDeliveryNotifyMain.deliveryQuantity;
            this.cancelQuantity = ascmDeliveryNotifyMain.cancelQuantity;
            //this.containerBindNumber = ascmDeliveryNotifyMain.containerBindNumber;
            this.status = ascmDeliveryNotifyMain.status;
            this.releasedTime = ascmDeliveryNotifyMain.releasedTime;
            this.needTime = ascmDeliveryNotifyMain.needTime;
            this.promisedTime = ascmDeliveryNotifyMain.promisedTime;
            this.confirmTime = ascmDeliveryNotifyMain.confirmTime;
            this.comments = ascmDeliveryNotifyMain.comments;
            this.wipEntityId = ascmDeliveryNotifyMain.wipEntityId;
            this.departmentId = ascmDeliveryNotifyMain.departmentId;
            this.locationId = ascmDeliveryNotifyMain.locationId;
            this.fdSourceType = ascmDeliveryNotifyMain.fdSourceType;
            this.totalReceiveQuantity = ascmDeliveryNotifyMain.totalReceiveQuantity;

            this.appointmentStartTime = ascmDeliveryNotifyMain.appointmentStartTime;
            this.appointmentEndTime = ascmDeliveryNotifyMain.appointmentEndTime;
            this.appointmentStatus = ascmDeliveryNotifyMain.appointmentStatus;

            this.detailCount = detailCount;
            //this.containerBindNumber = containerBindNumber;
            //this.totalNumber = totalNumber;
            //this.containerBindQuantity = containerBindNumber;
            //this.receiveTime = receiveTime;
        }

        public AscmDeliveryNotifyMain GetOwner()
        {
            return (AscmDeliveryNotifyMain)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string createTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string modifyTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string releasedTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(releasedTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(releasedTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string needTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(needTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(needTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string receiveTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(receiveTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(receiveTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual AscmSupplier ascmSupplier { get; set; }
        public virtual AscmSupplierAddress ascmSupplierAddress { get; set; }
        public virtual string supplierDocNumber { get { if (ascmSupplier != null && ascmSupplier.docNumber != null) return ascmSupplier.docNumber.Trim(); return ""; } }
        public virtual string supplierName { get { if (ascmSupplier != null && ascmSupplier.name != null) return ascmSupplier.name.Trim(); return ""; } }
        public virtual string supplierShortName { get { if (ascmSupplier != null && ascmSupplier.supplierShortName != null) return ascmSupplier.supplierShortName.Trim(); return ""; } }
        public virtual AscmMaterialItem ascmMaterialItem { get; set; }
        public virtual string materialName { get { if (ascmMaterialItem != null && ascmMaterialItem.description != null) return ascmMaterialItem.description.Trim(); return ""; } }
        public virtual string materialDocNumber { get { if (ascmMaterialItem != null && ascmMaterialItem.docNumber != null) return ascmMaterialItem.docNumber.Trim(); return ""; } }
        public virtual string materialBuyerName{ get { if (ascmMaterialItem != null ) return ascmMaterialItem.buyerName.Trim(); return ""; } }
        public virtual string statusCn { get { return StatusDefine.DisplayText(status); } }
        //public virtual decimal containerBindNumber { get; set; }
        public virtual decimal containerBindQuantity { get; set; }
        /// <summary>总数量(货品总数量)</summary>
        //public virtual decimal totalNumber { get; set; }
        public virtual long detailCount { get; set; }
        //public virtual AscmEmployee ascmEmployee_createUser_Employee { get; set; }
        public virtual AscmUserInfo ascmUserInfo_createUser { get; set; }
        public virtual string createUserEmployeeName { get { if (ascmUserInfo_createUser != null && ascmUserInfo_createUser.employeeName != null) return ascmUserInfo_createUser.employeeName.Trim(); return ""; } }

        public virtual AscmWipEntities ascmWipEntities { get; set; }
        public virtual string wipEntitiesName { get { if (ascmWipEntities != null && ascmWipEntities.name != null) return ascmWipEntities.name.Trim(); return ""; } }
        public virtual AscmWipDiscreteJobs ascmWipDiscreteJobs { get; set; }
        public virtual string ascmWipDiscreteJobsScheduledStartTime { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs._scheduledStartDate; return ""; } }
        public virtual string ascmWipDiscreteJobsScheduledStartTimeCn { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.scheduledStartDateCn; return ""; } }

        public virtual AscmBomDepartments ascmBomDepartments { get; set; }
        public virtual string bomDepartmentsCode { get { if (ascmBomDepartments != null) return ascmBomDepartments.code; return ""; } }

        public virtual AscmHrLocationsAll ascmHrLocationsAll { get; set; }
        public virtual string ascmHrLocationsAllCode { get { if (ascmHrLocationsAll != null) return ascmHrLocationsAll.code; return ""; } }

        public virtual AscmFndLookupValues ascmFndLookupValues { get; set; }
        public virtual string ascmFndLookupValuesMeaning { get { if (ascmFndLookupValues != null) return ascmFndLookupValues.meaning; return ""; } }

        public virtual string appointmentStartTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(appointmentStartTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(appointmentStartTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string appointmentEndTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(appointmentEndTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(appointmentEndTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }

        ///<summary>在途数量</summary>
        /*IN_DELIVERY_QTY := CUX_FD_DEL_NOTIFY_QTY_UTL.GET_IN_DELIVERY_QTY
                                                                     (NOTIFY_HEADER_ID,
                                                                      ORGANIZATION_ID
                                                                  ) ;*/
        public virtual int IN_DELIVERY_QTY { get; set; }
        /*
        TOTAL_RECEIPT_QTY（总接收）     
        SUBINV_QTY（入库量）
        IN_RECEIVED_QTY（接收未检验）
        IN_INSPECTED_QTY（接收未入库）
        TOTAL_RETURN_VENDOR_QTY（总退回给供应商数量）
                 * 
                 * 
         CUX_FD_DEL_NOTIFY_QTY_UTL.get_shipment_quantities(p_notify_header_id   => :FOLDER.NOTIFY_HEADER_ID ,
                                                                 x_total_received_qty => :FOLDER.TOTAL_RECEIPT_QTY ,
                                                                 x_total_returned_qty => :FOLDER.TOTAL_RETURN_VENDOR_QTY ,
                                                                 x_in_received_qty    => :FOLDER.IN_RECEIVED_QTY ,
                                                                 x_in_inspected_qty   => :FOLDER.IN_INSPECTED_QTY ,
                                                                 x_in_subinv_qty      => :FOLDER.SUBINV_QTY
                                                                 );
 
         */
        ///<summary>总接收</summary>
        public virtual int TOTAL_RECEIPT_QTY { get; set; }
        ///<summary>入库量</summary>
        public virtual int SUBINV_QTY { get; set; }
        ///<summary>接收未检验</summary>
        public virtual int IN_RECEIVED_QTY { get; set; }
        ///<summary>接收未入库</summary>
        public virtual int IN_INSPECTED_QTY { get; set; }
        ///<summary>总退回给供应商数量</summary>
        public virtual int TOTAL_RETURN_VENDOR_QTY { get; set; }
        ///<summary>接收中</summary>
        public virtual int IN_RECEIVED { get { return IN_RECEIVED_QTY + IN_INSPECTED_QTY; } }
        //到期预警定义
        public class AlertDefine
        {
            /// <summary>超期</summary>
            public const string overdue = "OVERDUE";
            /// <summary>预警</summary>
            public const string alert = "ALERT";

            public static string DisplayText(string value)
            {
                if (value == overdue)
                    return "超期";
                if (value == alert)
                    return "预警";

                return value;
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(overdue);
                list.Add(alert);
                return list;
            }
        }
        //状态定义
        public class StatusDefine
        {
            /// <summary>已确认</summary>
            public const string confirm = "CONFIRM";
            /// <summary>完成</summary>
            public const string completed = "COMPLETED";
            /// <summary>已关闭</summary>
            public const string closed = "CLOSED";
            /// <summary>已取消</summary>
            public const string cancel = "CANCEL";
            /// <summary>已抛弃</summary>
            public const string eject = "REJECT";
            /// <summary>未确认</summary>
            public const string unConfirm = "UNCONFIRM";

            public static string DisplayText(string value)
            {
                if (value == confirm)
                    return "已确认";
                if (value == completed)
                    return "完成";
                else if (value == closed)
                    return "已关闭";
                else if (value == cancel)
                    return "已取消";
                else if (value == eject)
                    return "已抛弃";
                else if (value == unConfirm)
                    return "未确认";

                return value;
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(confirm);
                list.Add(completed);
                list.Add(closed);
                list.Add(cancel);
                list.Add(eject);
                list.Add(unConfirm);
                return list;
            }
            public static List<string> GetSupplierList()
            {
                List<string> list = new List<string>();
                list.Add(confirm);
                //list.Add(completed);
                //list.Add(closed);
                //list.Add(cancel);
                //list.Add(eject);
                list.Add(unConfirm);
                return list;
            }
        }
    }
}
