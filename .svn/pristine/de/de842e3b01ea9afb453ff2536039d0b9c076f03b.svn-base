using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Warehouse.Entities
{
    public class AscmLocationMaterialLinkPK
    {
        ///<summary>货位ID</summary>
        public virtual int warelocationId { get; set; }
        /// <summary>物料ID</summary>
        public virtual int materialId { get; set; }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            if (other == this)
                return true;
            if (other is AscmLocationMaterialLinkPK)
            {
                AscmLocationMaterialLinkPK pk = other as AscmLocationMaterialLinkPK;
                return pk.warelocationId.Equals(warelocationId) && pk.materialId.Equals(materialId);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
