﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Dal.Warehouse.Entities;

namespace MideaAscm.Dal.FromErp.Entities
{
    ///<summary>作业表</summary>
    public class AscmWipDiscreteJobs
    {
        ///<summary>id</summary>
        public virtual int wipEntityId { get; set; }
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
        ///<summary>?</summary>
        public virtual int requestId { get; set; }
        ///<summary>?</summary>
        public virtual int programId { get; set; }
        ///<summary>作业状态</summary>
        public virtual int statusType { get; set; }
        ///<summary>装配件</summary>
        public virtual int primaryItemId { get; set; }
        ///<summary>?</summary>
        public virtual int jobType { get; set; }
        ///<summary>?</summary>
        public virtual int wipSupplyType { get; set; }
        ///<summary>描述</summary>
        public virtual string description { get; set; }
        ///<summary>作业类型</summary>
        public virtual string classCode { get; set; }
        ///<summary>?</summary>
        public virtual decimal materialAccount { get; set; }
        ///<summary>计划开始时间</summary>
        public virtual string scheduledStartDate { get; set; }
        ///<summary>计划完成时间</summary>
        public virtual string scheduledCompletionDate { get; set; }
        ///<summary>?</summary>
        public virtual string bomRevisionDate { get; set; }
        ///<summary>?</summary>
        public virtual string routingRevisionDate { get; set; }
        ///<summary>计划组，车间</summary>
        public virtual int scheduleGroupId { get; set; }
        ///<summary>生产线:ATTRIBUTE6</summary>
        public virtual string productionLine { get; set; }
        ///<summary>计划数量</summary>
        public virtual decimal netQuantity { get; set; }
        ///<summary>作业开始数量</summary>
        public virtual decimal startQuantity { get; set; }
        ///<summary>作业完成数量</summary>
        public virtual decimal quantityCompleted { get; set; }
        ///<summary>作业发放日期</summary>
        public virtual string dateReleased { get; set; }
        ///<summary>作业关闭日期</summary>
        public virtual string dateClosed { get; set; }
        ///<summary>绑定容器标签</summary>
        public virtual string tempBindContainSn { get; set; }
        ///<summary>备料状态</summary>
        public virtual string ascmStatus { get; set; }

        public AscmWipDiscreteJobs GetOwner()
        {
            return (AscmWipDiscreteJobs)this.MemberwiseClone();
        }
        public AscmWipDiscreteJobs()
        { 
            
        }

        public AscmWipDiscreteJobs(int wipEntityId, string mtlCategoryStatus)
        {
            this.wipEntityId = wipEntityId;
            this.mtlCategoryStatus = mtlCategoryStatus;
          //  this.ascmMaterialItem_description = primaryItemDescription;
        }
        public AscmWipDiscreteJobs(int wipEntityId, string primaryItemDocNumber, string primaryItemDescription)
        {
            this.wipEntityId = wipEntityId;
            this.ascmMaterialItem_docnumber = primaryItemDocNumber;
            this.ascmMaterialItem_description = primaryItemDescription;
        }
        public AscmWipDiscreteJobs(int wipEntityId, string scheduledStartDate, decimal netQuantity, string wipEntityName)
        {
            this.wipEntityId = wipEntityId;
            this.scheduledStartDate = scheduledStartDate;
            this.netQuantity = netQuantity;
            this.wipEntityName = wipEntityName;
        }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _scheduledStartDate { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(scheduledStartDate) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(scheduledStartDate)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _bomRevisionDate { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(bomRevisionDate) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(bomRevisionDate)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string scheduledCompletionDateShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(scheduledCompletionDate) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(scheduledCompletionDate)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string dateReleasedShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(dateReleased) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(dateReleased)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string dateClosedShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(dateClosed) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(dateClosed)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string operate { get; set; }
        public virtual string mtlCategoryStatus { get; set; }  //物料类别状态  2015年3月15日添加  覃小华 用于领料监控
        public virtual string leaderId { get; set; }
        public virtual string leaderName { get; set; }
        public virtual string subStatus { get; set; }
		public int statusId { get; set; }
		public bool isStop { get; set; }
        public virtual string ascmWorker_name { get; set; }   //责任人2015年3月15日添加  覃小华 用于领料监控
        public virtual string wipEntityIdentification { get; set; }   //用于标示任务 用于前台idfile 不然获取GETCHECK有问题2015年3月15日添加  覃小华 用于领料监控

        //？
        public virtual AscmFndLookupValues ascmFndLookupValues_jobType { get; set; }
        public virtual string jobType_Cn { get { if (ascmFndLookupValues_jobType != null) return ascmFndLookupValues_jobType.meaning; return ""; } }
        //作业状态
        public virtual AscmFndLookupValues ascmFndLookupValues_statusType { get; set; }
        public virtual string statusType_Cn { get { if (ascmFndLookupValues_statusType != null) return ascmFndLookupValues_statusType.meaning; return ""; } }
        //供应类型
        public virtual AscmFndLookupValues ascmFndLookupValues_wipSupplyType { get; set; }
        public virtual string wipSupplyType_Cn { get { if (ascmFndLookupValues_wipSupplyType != null) return ascmFndLookupValues_wipSupplyType.meaning; return ""; } }
        //车间
        public virtual AscmWipScheduleGroups ascmWipScheduleGroups { get; set; }
        public virtual string ascmWipScheduleGroupsName { get { if (ascmWipScheduleGroups != null) return ascmWipScheduleGroups.scheduleGroupName; return ""; } }
        //装配件
        public virtual AscmMaterialItem ascmMaterialItem { get; set; }
        public string ascmMaterialItem_docnumber;
        public string ascmMaterialItem_DocNumber {
            get { if (ascmMaterialItem != null) return ascmMaterialItem.docNumber; return ascmMaterialItem_docnumber; }
            set { ascmMaterialItem_docnumber = value; }
        }
        public virtual string ascmMaterialItem_Name { get { if (ascmMaterialItem != null) return ascmMaterialItem.name; return ""; } }
        public string ascmMaterialItem_description;
        public string ascmMaterialItem_Description {
            get { if (ascmMaterialItem != null) return ascmMaterialItem.description; return ascmMaterialItem_description; }
            set { ascmMaterialItem_description = value; }
        }
        //作业
        public virtual AscmWipEntities ascmWipEntities { get; set; }
        public string wipEntityName { get; set; }
        public string ascmWipEntities_Name { get { if (ascmWipEntities != null) return ascmWipEntities.name; return ""; }}
        public string ascmWipentities_Description { get { if (ascmWipEntities != null) return ascmWipEntities.description; return ""; } }
        //上传作业
        public virtual AscmDiscreteJobs ascmDiscreteJobs { get; set; }
        public string ascmDiscreteJobs_line { get { if (ascmDiscreteJobs != null) return ascmDiscreteJobs.lineAndSequence; return ""; } }
        public string sIdentificationId { get { if (ascmDiscreteJobs != null) return ascmDiscreteJobs.sIdentificationId; return ""; } }
        public string jobDate { get { if (ascmDiscreteJobs != null) return ascmDiscreteJobs.jobDate; return ""; } }
        public string jobInfoId { get { if (ascmDiscreteJobs != null) return ascmDiscreteJobs.jobInfoId; return ""; } }
        public string jobDesc { get { if (ascmDiscreteJobs != null) return ascmDiscreteJobs.jobDesc; return ""; } }
        public string onlineTime { get { if (ascmDiscreteJobs != null) return ascmDiscreteJobs.onlineTime; return ""; } }
        public string workerId { get { if (ascmDiscreteJobs != null) return ascmDiscreteJobs.workerId; return ""; } }
        public string rankerName { get { if (ascmDiscreteJobs != null) return ascmDiscreteJobs.rankerName; return ""; } }
        public string uploadDate { get { if (ascmDiscreteJobs != null) return ascmDiscreteJobs._time; return ""; } }
        /// <summary>
        /// 覃小华2015年3月16日
        /// </summary>
        public virtual string taskWipState { get { if (totalRequiredQuantity - totalGetMaterialQuantity > 0) return "未完成"; return "已完成"; } }
        /// <summary>
        /// 覃小华2015年3月16日
        /// 作业开始时间
        /// </summary>
        public virtual string taskStarTime { get; set; }
        /// <summary>
        /// 覃小华2015年3月16日
        /// 作业结束时间
        /// </summary>
        public virtual string taskEndTime { get;set;}

        //总数量
		public string status_Type { get; set; }
        public virtual decimal listWipRequireOperat_Total { get; set; }
        public virtual decimal listWipRequireOperat_GetTotal { get; set; }
        //统计数量
        public virtual decimal totalRequiredQuantity { get; set; }
        public virtual decimal totalGetMaterialQuantity { get; set; }
        public virtual decimal totalPreparationQuantity { get; set; }
        public virtual decimal totalSumQuantity { get { return totalGetMaterialQuantity + totalPreparationQuantity; } }//总备料数+总领料数

        //作业备料监控
        /// <summary>缺料情况：1、有库存，“欠料”2、无库存，“缺料”</summary>
        public string jobLackOfMaterial { get; set; }
        /// <summary>已配料人员</summary>
        public string jobCompoundedPerson { get; set; }
        /// <summary>容器数</summary>
        public decimal containerQuantity { get; set; }
        /// <summary>校验数</summary>
        public decimal checkQuantity { get; set; }
        /// <summary>备料状态</summary>
        public string ascmStatusCn { get { return AscmStatusDefine.DisplayText(ascmStatus); } }
        /// <summary>备料状态序号</summary>
        public int ascmStatusSn { get { return AscmStatusDefine.GetSortNo(ascmStatus); } }

        /// <summary>备料明细状态</summary>
        public string subStatusCn
        {
            get
            {
                if (!string.IsNullOrEmpty(subStatus)) 
                {
                    return AscmStatusDefine.DisplayText(subStatus);
                }

                return AscmStatusDefine.DisplayText(ascmStatus);
            }
        }
        
        /// <summary>作业状态</summary>
        public string statusTypeCn { get { return StatusTypeDefine.DisplayText(statusType); } }
        /// <summary>供应类型</summary>
        public string wipSupplyTypeCn { get { return AscmMaterialItem.WipSupplyTypeDefine.DisplayText(wipSupplyType); } }
        /// <summary>是否排产（如果排产，生产线取排产数据）</summary>
        public bool isScheduled { get; set; }
        public virtual string _mtlCategoryStatus { get { return MtlCategoryStatusDefine.DisplayText(mtlCategoryStatus); } }
        /// <summary>标记</summary>
        public virtual string markJobOperate { get; set; }
        public virtual AscmMarkTaskLog ascmMarkTaskLog { get; set; }

        /// <summary>作业日期</summary>
        public virtual string scheduledStartDateCn { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(scheduledStartDate) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(scheduledStartDate)).ToString("yyyy-MM-dd"); return ""; } }
        public virtual string scheduledStartDateSimple { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(scheduledStartDate) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(scheduledCompletionDateShow)).ToString("MM-dd"); return "";} }

        public class AscmStatusDefine
        {
            /// <summary>未打单</summary>
            public const string unPrint = "4";
            /// <summary>待备料</summary>
            public const string unPrepare = "3";
            /// <summary>备料中</summary>
            public const string preparing = "2";
            /// <summary>停止</summary>
            public const string unPick = "1";
            /// <summary>已领料</summary>
            public const string picked = "5";

            public static string DisplayText(string status)
            {
                switch (status)
                {
                    
                    case unPrepare: return "待备料";
                    case preparing: return "备料中";
                    case unPick: return "停止";
                    case picked: return "已领料";
                    case unPrint:
                    case "":
                    case null: return "未打单";
                    default: return status;
                }
            }
            public static int GetSortNo(string status)
            {
                switch (status)
                {
                    
                    case unPrepare: return 3;
                    case preparing: return 2;
                    case unPick: return 1;
                    case picked: return 5;
                    case unPrint:
                    case "":
                    case null: return 4;
                    default: return 0;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(unPrint);
                list.Add(unPrepare);
                list.Add(preparing);
                list.Add(unPick);
                list.Add(picked);
               
                return list;
            }
        }
        public class StatusTypeDefine
        {
            /// <summary>未发放</summary>
            public const int wff = 1;
            /// <summary>已发放</summary>
            public const int yff = 3;
            /// <summary>完成</summary>
            public const int wc = 4;
            /// <summary>完成 - 不计费</summary>
            public const int wcbjf = 5;
            /// <summary>暂挂</summary>
            public const int zg = 6;
            /// <summary>已取消</summary>
            public const int yqx = 7;
            /// <summary>装入待定清单</summary>
            public const int zrddqd = 8;
            /// <summary>无法装入清单</summary>
            public const int wfzrqd = 9;
            /// <summary>装入待定工艺路线</summary>
            public const int zrddgylx = 10;
            /// <summary>无法装入工艺路线</summary>
            public const int wfzrgylx = 11;
            /// <summary>已关闭</summary>
            public const int ygb = 12;
            /// <summary>待定 - 成批装入</summary>
            public const int ddcpzr = 13;
            /// <summary>等待关闭</summary>
            public const int ddgb = 14;
            /// <summary>无法关闭</summary>
            public const int wfgb = 15;
            /// <summary>待定计划</summary>
            public const int ddjh = 16;
            /// <summary>拟定</summary>
            public const int nd = 17;

            public static string DisplayText(int statusType)
            {
                switch (statusType)
                {
                    case wff: return "未发放";
                    case yff: return "已发放";
                    case wc: return "完成";
                    case wcbjf: return "完成 - 不计费";
                    case zg: return "暂挂";
                    case yqx: return "已取消";
                    case zrddqd: return "装入待定清单";
                    case wfzrqd: return "无法装入清单";
                    case zrddgylx: return "装入待定工艺路线";
                    case wfzrgylx: return "无法装入工艺路线";
                    case ygb: return "已关闭";
                    case ddcpzr: return "待定 - 成批装入";
                    case ddgb: return "等待关闭";
                    case wfgb: return "无法关闭";
                    case ddjh: return "待定计划";
                    case nd: return "拟定";
                    default: return statusType.ToString();
                }
            }
            public static List<int> GetList()
            {
                List<int> list = new List<int>();
                list.Add(wff);
                list.Add(yff);
                list.Add(wc);
                list.Add(wcbjf);
                list.Add(zg);
                list.Add(yqx);
                list.Add(zrddqd);
                list.Add(wfzrqd);
                list.Add(zrddgylx);
                list.Add(wfzrgylx);
                list.Add(ygb);
                list.Add(ddcpzr);
                list.Add(ddgb);
                list.Add(wfgb);
                list.Add(ddjh);
                list.Add(nd);
                return list;
            }
        }
    }
}