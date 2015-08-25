using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.GetMaterialManage.Entities
{
    public class AscmGetMaterialCreateTask
    {
        /// <summary>任务号</summary>
        public virtual string taskId { get; set; }
        /// <summary>类型</summary>
        public virtual int IdentificationId { get; set; }
        /// <summary>生产线</summary>
        public virtual string productLine { get; set; }
        /// <summary>所属仓库</summary>
        public virtual string warehouserId { get; set; }
        /// <summary>物料编码</summary>
        public virtual string materialDocNumber { get; set; }
        /// <summary>物料编码类型</summary>
        public virtual int materialType { get; set; }
        /// <summary>物料类别状态</summary>
        public virtual string categoryStatus { get; set; }
        /// <summary>上传日期</summary>
        public virtual string uploadDate { get; set; }
        /// <summary>上线时间</summary>
        public virtual string taskTime { get; set; }
        /// <summary>仓库位置</summary>
        public virtual string warehouserPlace { get; set; }
        /// <summary>上传排产员</summary>
        public virtual string rankMan { get; set; }
        /// <summary>第几次产生的任务</summary>
        public virtual int which { get; set; }
        /// <summary>作业发放日期</summary>
        public virtual string dateReleased { get; set; }

        //辅助信息
        public virtual List<AscmDiscreteJobs> ascmDiscreteJobsList { get; set; }
        public virtual string rankManName { get; set; }
        public virtual string IdentificationIdCN { get { return AscmGetMaterialTask.IdentificationIdDefine.DisplayText(IdentificationId); } }//类型
        public virtual string taskIdCn { get { return AscmGetMaterialTask.IdentificationIdDefine.DisplayNewTaskId(IdentificationId, taskId); } }
        public virtual string materialTypeCN { get { return AscmMaterialItem.WipSupplyTypeDefine.DisplayText(materialType); } }
        public virtual string categoryStatusCN { get { return MtlCategoryStatusDefine.DisplayText(categoryStatus); } }
    }
}
