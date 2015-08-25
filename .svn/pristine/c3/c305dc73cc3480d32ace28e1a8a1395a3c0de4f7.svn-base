using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using YnFrame.Dal.Entities;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Dal.GetMaterialManage.Entities
{
    /// <summary>
    /// 生成的领料任务
    /// </summary>
    public class AscmGetMaterialTask
    {
        /// <summary>编号</summary>
        public virtual int id { get; set; }
        /// <summary>创建人</summary>
        public virtual string createUser { get; set; }
        /// <summary>创建时间</summary>
        public virtual string createTime { get; set; }
        /// <summary>最后更新人</summary>
        public virtual string modifyUser { get; set; }
        /// <summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }
        /// <summary>任务号</summary>
        public virtual string taskId { get; set; }
        /// <summary>生产线</summary>
        public virtual string productLine { get; set; }
        /// <summary>所属仓库</summary>
        public virtual string warehouserId { get; set; }
        /// <summary>物料类别状态</summary>
        public virtual string mtlCategoryStatus { get; set; }
        /// <summary>排产员</summary>
        public virtual string rankerId { get; set; }
        /// <summary>责任人</summary>
        public virtual string workerId { get; set; }
        /// <summary>开始时间</summary>
        public virtual string starTime { get; set; }
        /// <summary>结束时间</summary>
        public virtual string endTime { get; set; }
        /// <summary>状态</summary>
        public virtual string status { get; set; }
        /// <summary>任务内容</summary>
        public virtual string tip { get; set; }
        /// <summary>类型</summary>
        public virtual int IdentificationId { get; set; }
        /// <summary>物料编码</summary>
        public virtual string materialDocNumber { get; set; }
        /// <summary>物料编码类型</summary>
        public virtual int materialType { get; set; }
        /// <summary>上传日期</summary>
        public virtual string uploadDate { get; set; }
        /// <summary>上线时间</summary>
        public virtual string taskTime { get; set; }
        /// <summary>仓库位置</summary>
        public virtual string warehouserPlace { get; set; }
        /// <summary>异常时间</summary>
        public virtual string errorTime { get; set; }
        /// <summary>关联标记</summary>
        public virtual string relatedMark { get; set; }
        /// <summary>第几次生成的任务</summary>
        public virtual int which { get; set; }
        /// <summary>车间信息</summary>
        public virtual string logisticsClass { get; set; }
        /// <summary>作业发放日期</summary>
        public virtual string dateReleased { get; set; }

        public AscmGetMaterialTask GetOwner()
        {
            return (AscmGetMaterialTask)this.MemberwiseClone();
        }
        public AscmGetMaterialTask()
        { 
        
        }
        public AscmGetMaterialTask(string warehouserId, string tip, string productLine, string mtlCategoryStatus, string taskTime, int IdentificationId, string materialDocNumber, string relatedMark)
        {
            this.warehouserId = warehouserId;
            this.tip = tip;
            this.productLine = productLine;
            this.mtlCategoryStatus = mtlCategoryStatus;
            this.taskTime = taskTime;
            this.IdentificationId = IdentificationId;
            this.materialDocNumber = materialDocNumber;
            this.relatedMark = relatedMark;
        }
        public AscmGetMaterialTask(string workerId, string taskCount, string avgTime)
        {
            this.workerId = workerId;
            this.taskCount = taskCount;
            this.avgTime = avgTime;
        }

        //辅助信息
        public virtual List<AscmDiscreteJobs> ascmDiscreteJobsList { get; set; }
        public virtual List<AscmWipRequirementOperations> listAscmWipRequirementOperations { get; set; } //生成任务时，任务关联的BOM
        public virtual string materialTypeCN { get { return materialTypeDefine.DisplayText(materialType); } }//物料编码类型
        public virtual string IdentificationIdCN { get { return IdentificationIdDefine.DisplayText(IdentificationId); } }//类型
        public virtual string categoryStatusCN { get { return _mtlCategoryStatus; } }//物料类别状态
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _mtlCategoryStatus { get { return MtlCategoryStatusDefine.DisplayText(mtlCategoryStatus); } }
        public virtual string _starTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(starTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(starTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _endTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(endTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(endTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _status { get { return StatusDefine.DisplayText(status); } }
        public virtual string statusCN { get { return StatusDefine.DisplayText(status); } }
        public virtual string strUsedTime { get; set; }//使用时间
        public virtual string strUserName { get; set; }//用户名称
        public virtual string operate { get; set; }//操作符
        public virtual string tipCN { get { return AscmAllocateRule.OtherDefine.DisplayText(tip); } }//任务内容
        public virtual decimal totalRequiredQuantity { get; set; }//需求总数
        public virtual decimal totalWmsPreparationQuantity { get; set; }//备料数量
        public virtual decimal totalGetMaterialQuantity { get; set; }//领料总数
        ///<summary>备料差异数量=需求数量-备料数量</summary>
        public virtual decimal totalQuantityPreparationDiff { get { return totalRequiredQuantity - totalWmsPreparationQuantity; } }
        ///<summary>领料差异数量=需求数量-领料数量</summary>
        public virtual decimal totalQuantityGetMaterialDiff { get { return totalRequiredQuantity - totalGetMaterialQuantity; } }
        public virtual int statusInt { get { return StatusDefine.DisplayInt(status); } } // 把字符状态转换为数字
        public virtual string taskIdCn { get { return IdentificationIdDefine.DisplayNewTaskId(IdentificationId, taskId); } }//作业内容
        public virtual string markTaskOperate { get; set; }
        public virtual string dateReleasedCn { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(dateReleased) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(dateReleased)).ToString("yyyy-MM-dd"); return ""; } }
        //public AscmWipEntities ascmWipEntities { get; set; }
        //public virtual string relatedJob { get { if (ascmWipEntities != null) return ascmWipEntities.name; return ""; } }
        //public AscmGetMaterialTask ascmGetMaterialTask { get; set; }
        //public virtual string relatedTask { get { if (ascmGetMaterialTask != null) return ascmGetMaterialTask.taskIdCn; return ""; } }
        //public AscmMarkTaskLog ascmMarkTaskLog { get; set; }

        //仓库
        public virtual AscmWarehouse ascmWarehouse { get; set; }
        public virtual string warehousePlace { get { if (ascmWarehouse != null) return ascmWarehouse.description; return ""; } }
        //领料员
        public virtual YnUser ascmWorker { get; set; }
        public virtual string ascmWorker_name { get { if (ascmWorker != null) return ascmWorker.userName; return ""; } }
        private string workerName;
        public string WorkerName
        {
            get { if (ascmWorker != null) return ascmWorker.userName; return workerName; }
            set { workerName = value; }
        }

        //排产员
        public virtual YnUser ascmRanker { get; set; }
        public virtual string ascmRanker_name { get { if (ascmRanker != null) return ascmRanker.userName; return ""; } }

        // 标记关联的作业及任务
        public virtual List<AscmMarkTaskLog> listAscmMarkTaskLog { get; set; }
        public virtual string relatedMarkInfo { get; set; }//修改标记任务获取关联信息
        public virtual int relatedMarkId { get; set; } //添加临时任务关联标记ID
        public virtual List<AscmGetMaterialTask> listAscmGetMaterialTask { get; set; }
        public virtual List<AscmWipEntities> listAscmWipEntities { get; set; }

        //车间
        public virtual AscmLogisticsClassInfo ascmLogisticsClassInfo { get; set; }
        public virtual string logisticsClassName { get { if (ascmLogisticsClassInfo != null) return ascmLogisticsClassInfo.logisticsClassName; return ""; } }
        

        //满足的生成规则
        public virtual string ruleType { get; set; }

        //统计字段
        public virtual string taskCount { get; set; } //任务数量
        public virtual string avgTime { get; set; } //平均时间


        #region 状态定义
        public class StatusDefine
        {
            /// <summary>已关闭</summary>
            public const string close = "CLOSE";
            /// <summary>已完成</summary>
            public const string finish = "FINISH";
            /// <summary>执行中</summary>
            public const string execute = "EXECUTE";
            /// <summary>已分配，未执行</summary>
            public const string notExecute = "NOTEXECUTE";
            /// <summary>未分配</summary>
            public const string notAllocate = "NOTALLOCATE";

            public static string DisplayText(string value)
            {
                if (value == close)
                    return "已关闭";
                else if (value == finish)
                    return "已完成";
                else if (value == execute)
                    return "执行中";
                else if (value == notExecute)
                    return "已分配";
                else if (value == notAllocate)
                    return "未分配";
                return value;
            }
            public static int DisplayInt(string value)
            {
                List<string> list = GetList();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ToString() == value)
                    {
                        return i + 1;
                    }
                }
                return 0;
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(notAllocate);
                list.Add(notExecute);
                list.Add(execute);
                list.Add(finish);
                list.Add(close);

                return list;
            }
        }
        #endregion

        #region 标示符定义
        public class IdentificationIdDefine
        {
            /// <summary>临时</summary>
            public const int ls = 0;
            /// <summary>总装</summary>
            public const int zz = 1;
            /// <summary>电装</summary>
            public const int dz = 2;

            public static string DisplayText(int value)
            {
                if (value == ls)
                    return "临时";
                else if (value == zz)
                    return "总装";
                else if (value == dz)
                    return "电装";
                return string.Empty;
            }
            public static string DisplayNewTaskId(int value1, string value2)
            {
                string newTaskId = "";
                if (value1 == zz)
                {
                    newTaskId = "Z" + value2;
                }
                else if (value1 == dz)
                {
                    newTaskId = "D" + value2;
                }
                else
                {
                    newTaskId = value2;
                }
                return newTaskId;
            }
            public static List<int> GetList()
            {
                List<int> list = new List<int>();
                list.Add(zz);
                list.Add(dz);
                list.Add(ls);
                return list;
            }
        }
        #endregion

        #region 物料编码类型
        public class materialTypeDefine
        {
            /// <summary>推式</summary>
            public const int ts = 1;

            public static string DisplayText(int value)
            {
                if (value == ts)
                    return "推式";
                return string.Empty;
            }
        }
        #endregion
    }
}
