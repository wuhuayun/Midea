using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>领料单明细与备料明细联合主键</summary>
    public class AscmWmsMtlReqDetailLinkPK
    {
        /// <summary>领料单明细ID</summary>
        public virtual int reqDetailId { get; set; }
        /// <summary>备料单明细ID</summary>
        public virtual int preDetailId { get; set; }

        public override bool Equals(object other)
        {
            if (other == null) 
                return false;
            if (other == this) 
                return true;
            if (other is AscmWmsMtlReqDetailLinkPK)
            {
                AscmWmsMtlReqDetailLinkPK pk = other as AscmWmsMtlReqDetailLinkPK;
                return pk.reqDetailId.Equals(reqDetailId) && pk.preDetailId.Equals(preDetailId);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
