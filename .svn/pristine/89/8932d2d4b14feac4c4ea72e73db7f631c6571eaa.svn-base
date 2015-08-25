using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.GetMaterialManage.Entities
{
    public class AscmMaterialSubCategory
    {
        /// <summary>记录编号</summary>
        public virtual int id { get; set; }
        /// <summary>创建人</summary>
        public virtual string createUser { get; set; }
        /// <summary>创建时间</summary>
        public virtual string createTime { get; set; }
        /// <summary>最后更新人</summary>
        public virtual string modifyUser { get; set; }
        /// <summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }
        /// <summary>子类</summary>
        public virtual string subCategoryCode { get; set; }
        /// <summary>所属大类</summary>
        public virtual int categoryId { get; set; }
        /// <summary>物料编码子类别描述</summary>
        public virtual string description { get; set; }
        /// <summary>总装备料形式</summary>
        public virtual string zMtlCategoryStatus { get; set; }
        /// <summary>电装备料形式</summary>
        public virtual string dMtlCategoryStatus { get; set; }
        /// <summary>组合码</summary>
        public virtual string combinationCode { get; set; }
        /// <summary>备注</summary>
        public virtual string tip { get; set; }
        /// <summary>其他备料形式{仓库使用}</summary>
        public virtual string wMtlCategoryStatus { get; set; }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public AscmMaterialCategory ascmMaterialCategory { get; set; }
        public string categoryCode { get{ if (ascmMaterialCategory != null) return ascmMaterialCategory.categoryCode; return "";}}
        public virtual string _zMtlCategoryStatus { get { return MtlCategoryStatusDefine.DisplayText(zMtlCategoryStatus); } }
        public virtual string _dMtlCategoryStatus { get { return MtlCategoryStatusDefine.DisplayText(dMtlCategoryStatus); } }
        public virtual string _wMtlCategoryStatus { get { return MtlCategoryStatusDefine.DisplayText(wMtlCategoryStatus); } }
    }
}
