using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.FromErp.Entities
{
    ///<summary>备料类型</summary>
    public class AscmFndLookupValues
    {
        ///<summary>id</summary>
        public virtual int id { get; set; }
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
        ///<summary>类型</summary>
        public virtual string type { get; set; }
        ///<summary>代码</summary>
        public virtual string code { get; set; }
        ///<summary>代码</summary>
        public virtual string meaning { get; set; }
        ///<summary>描述</summary>
        public virtual string description { get; set; }


        public AscmFndLookupValues GetOwner()
        {
            return (AscmFndLookupValues)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public class AttributeCodeDefine
        {
            /// <summary>备料类型</summary>
            public const string feedTypes = "CUX_FD_FEED_TYPES";
            /// <summary>作业</summary>
            public const string wipDiscreteJob = "WIP_DISCRETE_JOB";
            /// <summary>作业状态</summary>
            public const string wipJobStatus = "WIP_JOB_STATUS";
            /// <summary>供应类型</summary>
            public const string wipSupply = "WIP_SUPPLY";
        }
    }
}
