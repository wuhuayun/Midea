using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YnFrame.Dal.Entities;

namespace MideaAscm.Dal.GetMaterialManage.Entities
{
    public class WorkloadStatistics
    {
        /// <summary>用户</summary>
        public virtual string workerId { get; set; }
        /// <summary>任务数量</summary>
        public virtual string taskCount { get; set; }
        /// <summary>平均时间</summary>
        public virtual string avgTime { get; set; }

        //辅助属性
        public virtual YnUser ynUser { get; set; }
        public virtual string workerName { get { if (ynUser != null) return ynUser.userName; return ""; } }
        public virtual string operate { get; set; }//操作符
    }
}
