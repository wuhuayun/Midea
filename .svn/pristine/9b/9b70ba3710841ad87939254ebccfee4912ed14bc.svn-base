using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.FromErp.Entities
{
    public class AscmDeliveryNotifyDetail
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
        ///<summary>mainId</summary>
        public virtual int mainId { get; set; }
        ///<summary>物料Id</summary>
        public virtual int materialId { get; set; }
        ///<summary>编号</summary>
        public virtual int lineNumber { get; set; }
        ///<summary>数量</summary>
        public virtual decimal quantity { get; set; }
        ///<summary>送货数量</summary>
        public virtual decimal deliveryQuantity { get; set; }

        public AscmDeliveryNotifyDetail GetOwner()
        {
            return (AscmDeliveryNotifyDetail)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual AscmDeliveryNotifyMain ascmDeliveryNotifyMain { get; set; }
        public virtual string ascmDeliveryNotifyMain_needTime { get { if (ascmDeliveryNotifyMain != null && ascmDeliveryNotifyMain.needTimeShow != null) return ascmDeliveryNotifyMain.needTimeShow.Trim(); return ""; } }
    }
}
