using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.FromErp.Entities
{
    ///<summary>作业信息_接收中</summary>
    public class AscmRcvSupply
    {
        ///<summary>id</summary>
        public virtual int id { get; set; }
        ///<summary>itemId</summary>
        public virtual int itemId { get; set; }
        ///<summary>库存组织ID</summary>
        public virtual int toOrganizationId { get; set; }
        ///<summary>创建人</summary>
        public virtual string createUser { get; set; }
        ///<summary>创建时间</summary>
        public virtual string createTime { get; set; }
        ///<summary>最后更新人</summary>
        public virtual string modifyUser { get; set; }
        ///<summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }
        ///<summary>接收中数量</summary>
        public virtual decimal toOrgPrimaryQuantity { get; set; }
        public AscmRcvSupply()
        {
        }
        public AscmRcvSupply(int itemId, decimal toOrgPrimaryQuantity)
        {
            this.itemId = itemId;
            this.toOrgPrimaryQuantity = toOrgPrimaryQuantity;
        }
        public AscmRcvSupply GetOwner()
        {
            return (AscmRcvSupply)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }

    }
}
