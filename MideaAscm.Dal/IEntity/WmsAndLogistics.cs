using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.IEntity
{
    public class WmsAndLogistics
    {
        ///<summary>作业号ID</summary>
        public virtual int wipEntityId { get; set; }
        ///<summary>物料ID</summary>
        public virtual int materialId { get; set; }
        ///<summary>子库</summary>
        public virtual string warehouseId { get; set; }
        ///<summary>备料数量（领料数量）</summary>
        public virtual decimal quantity { get; set; }
        ///<summary>备料单字符串</summary>
        public virtual string preparationString { get; set; }
        ///<summary>领料员ID</summary>
        public virtual string workerId { get; set; }
    }
}
