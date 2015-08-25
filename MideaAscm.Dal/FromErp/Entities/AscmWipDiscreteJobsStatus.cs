using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Dal.Warehouse.Entities;

namespace MideaAscm.Dal.FromErp.Entities
{
    ///<summary>作业表明细状态</summary>
    public class AscmWipDiscreteJobsStatus
    {
        ///<summary>id</summary>
        public virtual int id { get; set; }
        ///<summary>作业id</summary>
        public virtual int wipEntityId { get; set; }
        ///<summary>上级领导id</summary>
        public virtual string leaderId { get; set; }
        ///<summary>上级领导名称</summary>
        public virtual string leaderName { get; set; }
        ///<summary>备料状态(来自AscmStatus)</summary>
        public virtual string subStatus { get; set; }
        ///<summary>库存组织ID</summary>
        public virtual int organizationId { get; set; }
		///<summary>是否停止</summary>
		public virtual bool isStop { get; set; }

        public AscmWipDiscreteJobsStatus GetOwner()
        {
            return (AscmWipDiscreteJobsStatus)this.MemberwiseClone();
        }

        public AscmWipDiscreteJobsStatus()
        { 
            
        }
    }
}
