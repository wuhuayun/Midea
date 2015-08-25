using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Warehouse.Entities
{
    [Serializable]
    public class AscmWhTeamUserPK
    {
        ///<summary>组别ID</summary>
        public virtual int teamId { get; set; }

        /// <summary>用户ID</summary>
        public virtual string userId { get; set; }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            if (other == this)
                return true;

            if (other is AscmWhTeamUserPK)
            {
                AscmWhTeamUserPK pk = other as AscmWhTeamUserPK;
                return pk.teamId.Equals(teamId) && (pk.userId != null && pk.userId.Equals(userId));
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
