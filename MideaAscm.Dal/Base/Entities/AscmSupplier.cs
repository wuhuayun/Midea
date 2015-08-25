using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Base.Entities
{
    public class AscmSupplier
    {
        ///<summary>供应商id</summary>
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
        ///<summary>编号</summary>
        public virtual string docNumber { get; set; }
        ///<summary>名称</summary>
        public virtual string name { get; set; }
        ///<summary>描述</summary>
        public virtual string description { get; set; }
        ///<summary>状态</summary>
        public virtual string status { get; set; }
        ///<summary>使用</summary>
        public virtual bool enabled { get; set; }
        ///<summary>报警小时</summary>
        public virtual int? warnHours { get; set; }
        ///<summary>报警天数</summary>
        public virtual int? warnDays { get; set; }
        /// <summary>供应商到厂放行时长（0.5≤ Tc ≤  24，Tc为0.5的整数倍）</summary>
        public virtual int passDuration { get; set; }

        public AscmSupplier GetOwner()
        {
            return (AscmSupplier)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual int containerAmount { get; set; }//容器数量
        public virtual int ExceptionCount { get; set; }  //异常数量
        public virtual AscmSupplierAddress ascmSupplierAddress { get; set; }
        public virtual string supplierShortName { get { if (ascmSupplierAddress != null && ascmSupplierAddress.vendorSiteCode != null) return ascmSupplierAddress.vendorSiteCode.Trim(); return name; } }

    }
}
