using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Warehouse.Entities
{
    public class AscmWmsLogisticsDetailLog
    {
        ///<summary>日志明细表ID</summary>
        public virtual int id { get; set; }
        ///<summary>日志主表ID</summary>
        public virtual int mainId { get; set; }
        ///<summary>作业号ID</summary>
        public virtual int wipEntityId { get; set; }
        ///<summary>物料ID</summary>
        public virtual int materialId { get; set; }
        ///<summary>领料数量</summary>
        public virtual decimal quantity { get; set; }
        ///<summary>备料单字符串</summary>
        public virtual string preparationString { get; set; }
        ///<summary>领料员ID</summary>
        public virtual string workerId { get; set; }

        public AscmWmsLogisticsDetailLog() { }
        public AscmWmsLogisticsDetailLog(int wipEntityId, int materialId, decimal quantity) 
        {
            this.wipEntityId = wipEntityId;
            this.materialId = materialId;
            this.quantity = quantity;
        }
    }
}
