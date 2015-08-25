using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>领料单与备料单联合主键</summary>
    public class AscmWmsMtlReqMainLinkPK
    {
        /// <summary>领料单明细ID</summary>
        public virtual int reqMainId { get; set; }
        /// <summary>备料单明细ID</summary>
        public virtual int preMainId { get; set; }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            if (other == this)
                return true;
            if (other is AscmWmsMtlReqMainLinkPK)
            {
                AscmWmsMtlReqMainLinkPK pk = other as AscmWmsMtlReqMainLinkPK;
                return pk.reqMainId.Equals(reqMainId) && pk.preMainId.Equals(preMainId);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
