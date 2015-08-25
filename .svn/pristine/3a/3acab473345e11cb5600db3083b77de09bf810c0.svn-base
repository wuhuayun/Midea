using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>物料数量更改日志</summary>
    public class AscmLocationMaterialLinkLog
    {
        /// <summary>日志ID</summary>
        public int id { get; set; }
        ///<summary>库存组织ID</summary>
        public virtual int organizationId { get; set; }
        ///<summary>最后更新人</summary>
        public virtual string modifyUser { get; set; }
        ///<summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }
        ///<summary>货位ID</summary>
        public virtual int warelocationId { get; set; }
        /// <summary>物料ID</summary>
        public virtual int materialId { get; set; }
        ///<summary>更改前数量</summary>
        public virtual decimal oldQuantity { get; set; }
        ///<summary>更改后数量</summary>
        public virtual decimal newQuantity { get; set; }
    }
}
