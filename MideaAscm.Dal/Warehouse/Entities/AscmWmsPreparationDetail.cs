using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>物料备料明细表</summary>
    public class AscmWmsPreparationDetail
    {
        /// <summary>ID</summary>
        public virtual int id { get; set; }
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
        /// <summary>物料ID</summary>
        public virtual int materialId { get; set; }
        /// <summary>供应子库</summary>
        public virtual string warehouseId { get; set; }
        /// <summary>供应类型</summary>
        public virtual string wipSupplyType { get; set; }
        /// <summary>货位ID</summary>
        public virtual int warelocationId { get; set; }
        /// <summary>作业计划数量</summary>
        public virtual decimal planQuantity { get; set; }
        ///// <summary>数量</summary>
        //public virtual decimal quantity { get; set; }
        /// <summary>发料数量(领料确认后更新)</summary>
        public virtual decimal issueQuantity { get; set; }
        /// <summary>作业ID</summary>
        public virtual int wipEntityId { get; set; }
        /// <summary>传递给物流领料模块的数量(备料确认后更新)</summary>
        public virtual decimal sendLogisticsQuantity { get; set; }

        public AscmWmsPreparationDetail() { }
        public void GetOwner(AscmWmsPreparationDetail preparationDetail)
        {
            this.id = preparationDetail.id;
            this.createUser = preparationDetail.createUser;
            this.createTime = preparationDetail.createTime;
            this.modifyUser = preparationDetail.modifyUser;
            this.modifyTime = preparationDetail.modifyTime;
            this.mainId = preparationDetail.mainId;
            this.materialId = preparationDetail.materialId;
            this.warehouseId = preparationDetail.warehouseId;
            this.wipSupplyType = preparationDetail.wipSupplyType;
            this.warelocationId = preparationDetail.warelocationId;
            this.planQuantity = preparationDetail.planQuantity;
            //this.quantity = preparationDetail.quantity;
            this.wipEntityId = preparationDetail.wipEntityId;
            this.issueQuantity = preparationDetail.issueQuantity;
            this.sendLogisticsQuantity = preparationDetail.sendLogisticsQuantity;
        }
        public AscmWmsPreparationDetail(AscmWmsPreparationDetail preparationDetail, string materialDocNumber)
        {
            GetOwner(preparationDetail);
            this.materialDocNumber = materialDocNumber;
        }
        public AscmWmsPreparationDetail(AscmWmsPreparationDetail preparationDetail, decimal containerBindNumber)
        {
            GetOwner(preparationDetail);
            this.containerBindNumber = containerBindNumber;
        }
        public AscmWmsPreparationDetail(AscmWmsPreparationDetail preparationDetail, string materialDocNumber, string materialName)
        {
            GetOwner(preparationDetail);
            this.materialDocNumber = materialDocNumber;
            this.materialName = materialName;
        }
        public AscmWmsPreparationDetail(AscmWmsPreparationDetail preparationDetail, string wipEntityName, decimal containerBindNumber)
        {
            GetOwner(preparationDetail);
            this.wipEntityName = wipEntityName;
            this.containerBindNumber = containerBindNumber;
        }
        public AscmWmsPreparationDetail(AscmWmsPreparationDetail preparationDetail, string locationDocNumber, string wipEntityName, decimal containerBindNumber)
        {
            GetOwner(preparationDetail);
            this.locationDocNumber = locationDocNumber;
            this.wipEntityName = wipEntityName;
            this.containerBindNumber = containerBindNumber;
        }
        public AscmWmsPreparationDetail(AscmWmsPreparationDetail preparationDetail, string materialDocNumber, string materialName, string materialUnit, string locationDocNumber, string wipEntityName, decimal containerBindNumber)
        {
            GetOwner(preparationDetail);
            this.materialDocNumber = materialDocNumber;
            this.materialName = materialName;
            this.materialUnit = materialUnit;
            this.locationDocNumber = locationDocNumber;
            this.wipEntityName = wipEntityName;
            this.containerBindNumber = containerBindNumber;
        }
        public AscmWmsPreparationDetail(int materialId, string materialDocNumber, string materialName, string materialUnit, decimal planQuantity, decimal containerBindNumber)
        {
            this.materialId = materialId;
            this.materialDocNumber = materialDocNumber;
            this.materialName = materialName;
            this.materialUnit = materialUnit;
            this.planQuantity = planQuantity;
            this.containerBindNumber = containerBindNumber;
        }
        public AscmWmsPreparationDetail(int materialId, string materialDocNumber, decimal planQuantity, decimal issueQuantity, int wipEntityId, string wipEntityName)
        {
            this.materialId = materialId;
            this.materialDocNumber = materialDocNumber;
            this.planQuantity = planQuantity;
            this.issueQuantity = issueQuantity;
            this.wipEntityId = wipEntityId;
            this.wipEntityName = wipEntityName;
        }

        //辅助信息
        /// <summary>物料编码</summary>
        public string materialDocNumber { get; set; }
        /// <summary>物料名称</summary>
        public string materialName { get; set; }
        /// <summary>物料单位</summary>
        public string materialUnit { get; set; }
        /// <summary>货位编码</summary>
        public string locationDocNumber { get; set; }
        /// <summary>作业号</summary>
        public string wipEntityName { get; set; }

        public AscmMaterialItem ascmMaterialItem { get; set; }
        /// <summary>物料名称</summary>
        public string materialDescription { get { if (ascmMaterialItem != null) return ascmMaterialItem.description; return string.Empty; } }

        public AscmWipDiscreteJobs ascmWipDiscreteJobs { get; set; }
        /// <summary>产线</summary>
        public string jobProductionLine { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.productionLine; return string.Empty; } }
        /// <summary>作业号</summary>
        public string jobWipEntityName { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.ascmWipEntities_Name; return string.Empty; } }
        /// <summary>作业计划开始时间</summary>
        public string jobScheduledStartDate
        {
            get
            {
                if (ascmWipDiscreteJobs != null)
                {
                    object objDate = YnBaseClass2.Helper.DateHelper.GetDateTime(ascmWipDiscreteJobs.scheduledStartDate);
                    if (objDate != null)
                        return ((DateTime)objDate).ToString("yyyy-MM-dd HH:mm");
                }
                return string.Empty;
            }
        }
        /// <summary>装配件编码</summary>
        public string jobPrimaryItemDoc { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.ascmMaterialItem_DocNumber; return string.Empty; } }
        /// <summary>装配件描述</summary>
        public string jobPrimaryItemDes { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.ascmMaterialItem_Description; return string.Empty; } }
        /// <summary>计划组</summary>
        public string jobScheduleGroupsName { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.ascmWipScheduleGroupsName; return string.Empty; } }

        /// <summary>库存现有量</summary>
        public decimal onhandQuantity { get; set; }
        /// <summary>已领数量</summary>
        public decimal receivedQuantity { get; set; }
        /// <summary>接收中数量</summary>
        public decimal recSupplyQuantity { get; set; }

        public decimal sumQuantity { get; set; }
        ///<summary>容器绑定数量</summary>
        public decimal containerBindNumber { get; set; }
        /// <summary>条码</summary>
        public virtual byte[] barcodeShow { get; set; }
        /// <summary>未发料数量</summary>
        public virtual decimal unIssueQuantity { get { return (planQuantity - issueQuantity); } }
        /* 支持WEB端备料 */
        /// <summary>本次备料数量</summary>
        private decimal? _prepareQuantity;
        public decimal prepareQuantity 
        {
            get { if (_prepareQuantity.HasValue) return _prepareQuantity.Value; return planQuantity - containerBindNumber; }
            set { _prepareQuantity = value; }
        }
        /// <summary>单个页面大数据显示时，提供勾选</summary>
        public bool select { get; set; }
        /// <summary>备料明细容器备料列表</summary>
        public List<AscmWmsContainerDelivery> listContainerDelivery { get; set; }
    }
}
