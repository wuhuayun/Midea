using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.FromErp.Entities
{
    ///<summary>领料单头表</summary>
    public class AscmCuxWipReleaseHeaders
    {
        ///<summary>id</summary>
        public virtual int releaseHeaderId { get; set; }
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
        ///<summary>作业</summary>
        public virtual int wipEntityId { get; set; }
        ///<summary>编号</summary>
        public virtual string releaseNumber { get; set; }
        ///<summary>单据类型:ISSUE的是领料单 RETURN这个为退料单</summary>
        public virtual string releaseType { get; set; }
        ///<summary>?</summary>
        public virtual string releaseStatus { get; set; }
        ///<summary>?</summary>
        public virtual string releaseDate { get; set; }
        
        public AscmCuxWipReleaseHeaders()
        {
        }
        public AscmCuxWipReleaseHeaders(AscmCuxWipReleaseHeaders ascmCuxWipReleaseHeaders)//, long detailCount, long totalNumber
        {
            this.releaseHeaderId = ascmCuxWipReleaseHeaders.releaseHeaderId;
            this.organizationId = ascmCuxWipReleaseHeaders.organizationId;
            this.createUser = ascmCuxWipReleaseHeaders.createUser;
            this.createTime = ascmCuxWipReleaseHeaders.createTime;
            this.modifyUser = ascmCuxWipReleaseHeaders.modifyUser;
            this.modifyTime = ascmCuxWipReleaseHeaders.modifyTime;
            this.wipEntityId = ascmCuxWipReleaseHeaders.wipEntityId;
            this.releaseNumber = ascmCuxWipReleaseHeaders.releaseNumber;
            this.releaseType = ascmCuxWipReleaseHeaders.releaseType;
            this.releaseStatus = ascmCuxWipReleaseHeaders.releaseStatus;
            this.releaseDate = ascmCuxWipReleaseHeaders.releaseDate;

            //this.detailCount = detailCount;
            //this.totalNumber = totalNumber;
        }
        public AscmCuxWipReleaseHeaders GetOwner()
        {
            return (AscmCuxWipReleaseHeaders)this.MemberwiseClone();
        }
        ///<summary>?</summary>
        public virtual string releaseTypeCn 
        {
            get 
            {
                if (releaseType == "RETURN")
                    return "";
                else if (releaseType == "ISSUE")
                    return "";
                return "";
            }
        }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }

        public long detailCount { get; set; }
        
        public virtual AscmWipEntities ascmWipEntities { get; set; }
        public virtual string wipEntitiesName { get { if (ascmWipEntities != null && ascmWipEntities.name != null) return ascmWipEntities.name.Trim(); return ""; } }

        public virtual AscmWipDiscreteJobs ascmWipDiscreteJobs { get; set; }
        public virtual string ascmWipDiscreteJobsScheduledStartDate { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs._scheduledStartDate; return ""; } }
        public virtual string ascmWipDiscreteJobsBomRevisionDate { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs._bomRevisionDate; return ""; } }
        public virtual string ascmWipDiscreteJobsDescription { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.description; return ""; } }
        public virtual string ascmWipDiscreteJobsJobType_Cn { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.jobType_Cn; return ""; } }
        public virtual string ascmWipDiscreteJobsStatusType_Cn { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.statusType_Cn; return ""; } }
        public virtual string ascmWipDiscreteJobsWipSupplyType_Cn { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.wipSupplyType_Cn; return ""; } }
        public virtual string ascmWipDiscreteJobsAscmWipScheduleGroupsName { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.ascmWipScheduleGroupsName; return ""; } }
        public virtual string ascmWipDiscreteJobs_ascmMaterialItem_DocNumber { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.ascmMaterialItem_DocNumber; return ""; } }
        public virtual string ascmWipDiscreteJobs_ascmMaterialItem_Description { get { if (ascmWipDiscreteJobs != null && ascmWipDiscreteJobs.ascmMaterialItem != null) return ascmWipDiscreteJobs.ascmMaterialItem.description; return ""; } }
        public virtual decimal ascmWipDiscreteJobsNetQuantity { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.netQuantity; return 0; } }

        /*
         *  2014/5/13 by chenyao
         */
        /// <summary>作业号</summary>
        public string wipEntityName { get; set; }
        /// <summary>作业日期</summary>
        public string scheduledStartDate { get; set; }
        public string scheduledStartDateFt { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(scheduledStartDate) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(scheduledStartDate)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        /// <summary>供应类型</summary>
        public int wipSupplyType { get; set; }
        public string wipSupplyTypeCn { get { return AscmMaterialItem.WipSupplyTypeDefine.DisplayText(wipSupplyType); } }
        ///<summary>计划组，车间</summary>
        public int scheduleGroupId { get; set; }
        public string scheduleGroupName { get; set; }
        ///<summary>计划数量</summary>
        public decimal netQuantity { get; set; }
        /// <summary>作业状态</summary>
        public int statusType { get; set; }
        public string statusTypeCn { get { return AscmWipDiscreteJobs.StatusTypeDefine.DisplayText(statusType); } }
        /// <summary>作业说明</summary>
        public string description { get; set; }
        /// <summary>装配件编码</summary>
        public string primaryItemDocNumber { get; set; }
    }
}
