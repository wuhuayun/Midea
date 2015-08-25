using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Warehouse.Entities
{
    [Serializable]
    public class AscmWhTeamUser 
    {
        /// <summary>组别用户联合主键</summary>
        public virtual AscmWhTeamUserPK pk { get; set; }

        ///<summary>是否为负责人</summary>
        public virtual bool isLeader { get; set; }

        ///<summary>库存组织ID</summary>
        public virtual int organizationId { get; set; }

        ///<summary>创建人</summary>
        public virtual string createUser { get; set; }

        ///<summary>创建时间</summary>
        public virtual string createTime { get; set; }

        ///<summary>最后更新人</summary>
        public virtual string modifyUser { get; set; }

        ///<summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }

        public AscmWhTeamUser GetOwner()
        {
            return (AscmWhTeamUser)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }

        public int M_TeamId 
        {
            get 
            {
                if (pk == null) pk = new AscmWhTeamUserPK();

                return pk.teamId;
            }
            set 
            {
                if (pk == null) pk = new AscmWhTeamUserPK();

                pk.teamId = value;
            }
        }

        public string M_UserId
        {
            get
            {
                if (pk == null) pk = new AscmWhTeamUserPK();

                return pk.userId;
            }
            set
            {
                if (pk == null) pk = new AscmWhTeamUserPK();

                pk.userId = value;
            }
        }
    }
}
