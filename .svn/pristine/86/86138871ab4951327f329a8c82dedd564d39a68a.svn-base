using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.FromErp.Entities
{
    public class AscmAkWebUserSecAttrValues
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
        ///<summary>id</summary>
        public virtual int webUserId { get; set; }
        ///<summary>AttributeCode</summary>
        public virtual string attributeCode { get; set; }
        ///<summary>NumberValue</summary>
        public virtual int numberValue { get; set; }

        public AscmHrLocationsAll GetOwner()
        {
            return (AscmHrLocationsAll)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        
        //
        public class AttributeCodeDefine
        {
            /// <summary>供应商用户</summary>
            public const string supplierUser = "ICX_SUPPLIER_ORG_ID";
        }
    }
}
