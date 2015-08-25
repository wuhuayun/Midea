using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.GetMaterialManage.Entities;

namespace MideaAscm.Dal.FromErp.Entities
{
    ///<summary>作业BOM表</summary>
    public class AscmWipRequirementOperations
    {
        ///<summary>id</summary>
        public virtual int id { get; set; }
        ///<summary>物料ID</summary>
        public virtual int inventoryItemId { get; set; }
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
        ///<summary>作业ID</summary>
        public virtual int wipEntityId { get; set; }
        ///<summary>数量</summary>
        public virtual int operationSeqNum { get; set; }
        ///<summary>需求数量</summary>
        public virtual decimal requiredQuantity { get; set; }
        ///<summary>发料数量</summary>
        public virtual decimal quantityIssued { get; set; }
        ///<summary>每个装配件需求数量</summary>
        public virtual decimal quantityPerAssembly { get; set; }
        ///<summary>子库</summary>
        public virtual string supplySubinventory { get; set; }
        ///<summary>数量</summary>
        public virtual decimal mpsRequiredQuantity { get; set; }
        ///<summary>时间</summary>
        public virtual string mpsDateRequired { get; set; }
        ///<summary>供应类型</summary>
        public virtual int wipSupplyType { get; set; }
        ///<summary>任务号</summary>
        public virtual int? taskId { get; set; }
        ///<summary>备注</summary>
        public virtual string tip { get; set; }
        ///<summary>需求日期</summary>
        public virtual string dateRequired { get; set; }
        ///<summary>领料字符串</summary>
        public virtual string getMaterialString { get; set; }
        ///<summary>领料数量</summary>
        public virtual decimal getMaterialQuantity { get; set; }
        ///<summary>备料单字符串</summary>
        public virtual string wmsPreparationString { get; set; }
        ///<summary>备料数量</summary>
        public virtual decimal wmsPreparationQuantity { get; set; }
        //作业备料监控新增字段 2013-10-11 chenyao
        ///<summary>仓库发料数量</summary>
        public virtual decimal ascmIssuedQuantity { get; set; }
        ///<summary>仓库备料数量</summary>
        public virtual decimal ascmPreparedQuantity { get; set; }
        //改子库备料新增字段2014-04-07 yike
        ///<summary>改子库备料子库</summary>
        public virtual string wmsPreparationWarehouse { get; set; }

        public AscmWipRequirementOperations()
        {
        }
        public AscmWipRequirementOperations(int wipEntityId)
        {
            this.wipEntityId = wipEntityId;
        }
        public AscmWipRequirementOperations(AscmWipRequirementOperations ascmWipRequirementOperations)
        {
            GetOwner(ascmWipRequirementOperations);
            
            //this.id = ascmWipRequirementOperations.id;
            //this.inventoryItemId = ascmWipRequirementOperations.inventoryItemId;
            //this.organizationId = ascmWipRequirementOperations.organizationId;
            //this.createUser = ascmWipRequirementOperations.createUser;
            //this.createTime = ascmWipRequirementOperations.createTime;
            //this.modifyUser = ascmWipRequirementOperations.modifyUser;
            //this.modifyTime = ascmWipRequirementOperations.modifyTime;
            //this.wipEntityId = ascmWipRequirementOperations.wipEntityId;
            //this.operationSeqNum = ascmWipRequirementOperations.operationSeqNum;
            //this.requiredQuantity = ascmWipRequirementOperations.requiredQuantity;
            //this.quantityIssued = ascmWipRequirementOperations.quantityIssued;
            //this.quantityPerAssembly = ascmWipRequirementOperations.quantityPerAssembly;
            //this.supplySubinventory = ascmWipRequirementOperations.supplySubinventory;
            //this.mpsRequiredQuantity = ascmWipRequirementOperations.mpsRequiredQuantity;
            //this.mpsDateRequired = ascmWipRequirementOperations.mpsDateRequired;
            //this.wipSupplyType = ascmWipRequirementOperations.wipSupplyType;
            //this.dateRequired = ascmWipRequirementOperations.dateRequired;
            //this.getMaterialString = ascmWipRequirementOperations.getMaterialString;
            //this.getMaterialQuantity = ascmWipRequirementOperations.getMaterialQuantity;
            //this.wmsPreparationString = ascmWipRequirementOperations.wmsPreparationString;
            //this.wmsPreparationQuantity = ascmWipRequirementOperations.wmsPreparationQuantity;
            //this.ascmIssuedQuantity = ascmWipRequirementOperations.ascmIssuedQuantity;
            //this.ascmPreparedQuantity = ascmWipRequirementOperations.ascmPreparedQuantity;
        }
        public AscmWipRequirementOperations(int inventoryItemId, long orderCount, decimal requiredQuantity, decimal quantityIssued)
        {
            this.inventoryItemId = inventoryItemId;
            this.orderCount = (int)orderCount;
            this.requiredQuantity = requiredQuantity;
            this.quantityIssued = quantityIssued;
        }
        public AscmWipRequirementOperations(int inventoryItemId, decimal requiredQuantity, decimal quantityIssued, decimal getMaterialQuantity)
        {
            this.inventoryItemId = inventoryItemId;
            this.requiredQuantity = requiredQuantity;
            this.quantityIssued = quantityIssued;
            this.getMaterialQuantity = getMaterialQuantity;
        }
        public AscmWipRequirementOperations(int wipEntityId, int inventoryItemId, string workerId, string productLine)
        {
            this.wipEntityId = wipEntityId;
            this.inventoryItemId = inventoryItemId;
            this.workerId = workerId;
            this.productLine = productLine;
        }
        public AscmWipRequirementOperations(AscmWipRequirementOperations ascmWipRequirementOperations, string docNumber, string description)
        {
            GetOwner(ascmWipRequirementOperations);
            this.docNumber = docNumber;
            this.description = description;
        }
        public AscmWipRequirementOperations(AscmWipRequirementOperations ascmWipRequirementOperations, string docNumber, string description, string zMtlCategoryStatus, string dMtlCategoryStatus, int docWipSupplyType, string jobDate, int identificationId, string productLine, int which, string workerId, string onlineTime)
        {
            GetOwner(ascmWipRequirementOperations);
            this.docNumber = docNumber;
            this.description = description;
            this.zMtlCategoryStatus = zMtlCategoryStatus;
            this.dMtlCategoryStatus = dMtlCategoryStatus;
            this.docWipSupplyType = docWipSupplyType;
            this.jobDate = jobDate;
            this.identificationId = identificationId;
            this.productLine = productLine;
            this.which = which;
            this.workerId = workerId;
            this.onlineTime = onlineTime;
        }
        public AscmWipRequirementOperations(int wipEntityId, decimal requiredQuantity, decimal quantityIssued, decimal getMaterialQuantity, decimal wmsPreparationQuantity)
        {
            this.wipEntityId = wipEntityId;
            this.quantityIssued = quantityIssued;
            this.requiredQuantity = requiredQuantity;
            this.getMaterialQuantity = getMaterialQuantity;
            this.wmsPreparationQuantity = wmsPreparationQuantity;
        }
        public AscmWipRequirementOperations GetOwner()
        {
            return (AscmWipRequirementOperations)this.MemberwiseClone();
        }

        public void GetOwner(AscmWipRequirementOperations ascmWipRequirementOperations)
        {
            this.id = ascmWipRequirementOperations.id;
            this.inventoryItemId = ascmWipRequirementOperations.inventoryItemId;
            this.organizationId = ascmWipRequirementOperations.organizationId;
            this.createUser = ascmWipRequirementOperations.createUser;
            this.createTime = ascmWipRequirementOperations.createTime;
            this.modifyUser = ascmWipRequirementOperations.modifyUser;
            this.modifyTime = ascmWipRequirementOperations.modifyTime;
            this.wipEntityId = ascmWipRequirementOperations.wipEntityId;
            this.operationSeqNum = ascmWipRequirementOperations.operationSeqNum;
            this.requiredQuantity = ascmWipRequirementOperations.requiredQuantity;
            this.quantityIssued = ascmWipRequirementOperations.quantityIssued;
            this.quantityPerAssembly = ascmWipRequirementOperations.quantityPerAssembly;
            this.supplySubinventory = ascmWipRequirementOperations.supplySubinventory;
            this.mpsRequiredQuantity = ascmWipRequirementOperations.mpsRequiredQuantity;
            this.mpsDateRequired = ascmWipRequirementOperations.mpsDateRequired;
            this.wipSupplyType = ascmWipRequirementOperations.wipSupplyType;
            this.dateRequired = ascmWipRequirementOperations.dateRequired;
            this.getMaterialString = ascmWipRequirementOperations.getMaterialString;
            this.getMaterialQuantity = ascmWipRequirementOperations.getMaterialQuantity;
            this.wmsPreparationString = ascmWipRequirementOperations.wmsPreparationString;
            this.wmsPreparationQuantity = ascmWipRequirementOperations.wmsPreparationQuantity;
            this.ascmIssuedQuantity = ascmWipRequirementOperations.ascmIssuedQuantity;
            this.ascmPreparedQuantity = ascmWipRequirementOperations.ascmPreparedQuantity;
        }

        //辅助信息
        public virtual string wipSupplyTypeCn
        {
            get
            {
                if (wipSupplyType == 1)
                {
                    return "推式";
                }
                else if (wipSupplyType == 2)
                {
                    return "装配拉式";
                }
                else if (wipSupplyType == 3)
                {
                    return "拉式工序";
                }
                else if (wipSupplyType == 4)
                {
                    return "批量";
                }
                else if (wipSupplyType == 5)
                {
                    return "供应商";
                }
                else if (wipSupplyType == 6)
                {
                    return "虚拟件";
                }
                else if (wipSupplyType == 7)
                {
                    return "基于物料清单";
                }
                return wipSupplyType.ToString();
            }
        }
        public AscmGetMaterialTask ascmGetMaterialTask { get; set; }
        public AscmWarehouse ascmWarehouse { get; set; }
        public virtual string mpsDateRequiredStr { get { if (!string.IsNullOrEmpty(mpsDateRequired)) return Convert.ToDateTime(mpsDateRequired).ToString("yyyy-MM-dd"); return ""; } }
        //需求备料监控增加：领料员、产线
        public string workerId { get; set; }
        public string productLine { get; set; }

        //构造物料：编码、描述
        public string docNumber { get; set; }
        public string description { get; set; }
        public string zMtlCategoryStatus { get; set; }
        public string dMtlCategoryStatus { get; set; }
        public int docWipSupplyType { get; set; }
        public string jobDate { get; set; }
        public int identificationId { get; set; }
        public int which { get; set; }
        public string onlineTime { get; set; }

        //物料
        public virtual AscmMaterialItem ascmMaterialItem { get; set; }
        public virtual string ascmMaterialItem_DocNumber { get { if (ascmMaterialItem != null) return ascmMaterialItem.docNumber; return ""; }}
        public virtual string ascmMaterialItem_Description { get { if (ascmMaterialItem != null) return ascmMaterialItem.description; return ""; }}
        public virtual string ascmMaterialItem_unit { get { if (ascmMaterialItem != null) return ascmMaterialItem.unit; return ""; } }
        public virtual string ascmMaterialItem_zMtlCategoryStatus { get { if (ascmMaterialItem != null) return ascmMaterialItem.zMtlCategoryStatus; return ""; } }
        public virtual string ascmMaterialItem_dMtlCategoryStatus { get { if (ascmMaterialItem != null) return ascmMaterialItem.dMtlCategoryStatus; return ""; } }
        public virtual string ascmMaterialItem_Warehouse { get { if (ascmMaterialItem != null) return ascmMaterialItem.warehouseName; return ""; } }
        //作业
        public virtual AscmWipEntities ascmWipEntities { get; set; }
        public virtual string wipEntitiesName { get { if (ascmWipEntities != null && ascmWipEntities.name != null) return ascmWipEntities.name.Trim(); return ""; } }

        public virtual AscmWipDiscreteJobs ascmWipDiscreteJobs { get; set; }
        public virtual string ascmWipDiscreteJobsScheduledStartDate { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs._scheduledStartDate; return ""; } }
        public virtual string ascmWipDiscreteJobs_scheduledCompletionDate { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.scheduledCompletionDateShow; return ""; } }
        public virtual string ascmWipDiscreteJobsBomRevisionDate { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs._bomRevisionDate; return ""; } }
        public virtual string ascmWipDiscreteJobsDescription { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.description; return ""; } }
        public virtual string ascmWipDiscreteJobsJobType_Cn { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.jobType_Cn; return ""; } }
        public virtual string ascmWipDiscreteJobsStatusType_Cn { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.statusType_Cn; return ""; } }
        public virtual string ascmWipDiscreteJobsWipSupplyType_Cn { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.wipSupplyType_Cn; return ""; } }
        public virtual string ascmWipDiscreteJobsAscmWipScheduleGroupsName { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.ascmWipScheduleGroupsName; return ""; } }
        public virtual string ascmWipDiscreteJobs_ascmMaterialItem_DocNumber { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.ascmMaterialItem_DocNumber; return ""; } }
        public virtual string ascmWipDiscreteJobs_ascmMaterialItem_Description { get { if (ascmWipDiscreteJobs != null && ascmWipDiscreteJobs.ascmMaterialItem != null) return ascmWipDiscreteJobs.ascmMaterialItem.description; return ""; } }
        public virtual decimal ascmWipDiscreteJobsNetQuantity { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.netQuantity; return 0; } }
        public virtual string ascmWipDiscreteJobs_classCode { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.classCode; return ""; } }
        public virtual decimal ascmWipDiscreteJobs_startQuantity { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.startQuantity; return 0; } }
        public virtual decimal ascmWipDiscreteJobs_quantityCompleted { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.quantityCompleted; return 0; } }
        public virtual string ascmWipDiscreteJobs_dateReleased { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.dateReleasedShow; return ""; } }
        public virtual string ascmWipDiscreteJobs_dateClosed { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.dateClosedShow; return ""; } }
		/// <summary>组件</summary>
        public virtual string materialDocNumber { get { return (ascmMaterialItem == null) ? string.Empty : ascmMaterialItem.docNumber; } }
        /// <summary>组件说明</summary>
        public virtual string materialDescription { get { return (ascmMaterialItem == null) ? string.Empty : ascmMaterialItem.description; } }
        ///<summary>订单数</summary>
        public virtual int orderCount { get; set; }

        ///<summary>差异数量=需求数量-发料数量</summary>
        public virtual decimal quantityDifference { get { return requiredQuantity - quantityIssued; } }
        ///<summary>现有数</summary>
        public virtual decimal transactionQuantity { get; set; }
        ///<summary>接收中数量</summary>
        public virtual decimal toOrgPrimaryQuantity { get; set; }
        ///<summary>备料差异数量=需求数量-备料数量</summary>
        public virtual decimal quantityPreparationDifference { get { return requiredQuantity - wmsPreparationQuantity; } }
        ///<summary>领料差异数量=需求数量-领料数量</summary>
        public virtual decimal quantityGetMaterialDifference { get { return requiredQuantity - getMaterialQuantity; } }
    }
}
