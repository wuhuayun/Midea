using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.Warehouse.Entities;

namespace MideaAscm.Dal.FromErp.Entities
{
    ///<summary>作业信息</summary>
    public class AscmWipEntities
    {
        ///<summary>id</summary>
        public virtual int wipEntityId { get; set; }
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
        ///<summary>任务名称</summary>
        public virtual string name { get; set; }
        ///<summary>任务类型</summary>
        public virtual int type { get; set; }
        ///<summary>描述</summary>
        public virtual string description { get; set; }
        ///<summary>?</summary>
        public virtual int primaryItemId { get; set; }
        ///<summary>?</summary>
        public virtual int genObjectId { get; set; }


        public AscmWipEntities GetOwner()
        {
            return (AscmWipEntities)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }

        public enum WipEntityType
        {
            none = 0,
            /// <summary>内机</summary>
            withinTheMachine = 1,
            /// <summary>外机</summary>
            outsideTheMachine = 2
        }
    }
}
