using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>仓库转移明细表</summary>
    public class AscmWmsStockTransDetail
    {
        ///<summary>ID</summary>
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
        ///<summary>主表ID</summary>
        public virtual int mainId { get; set; }
        ///<summary>物料Id</summary>
        public virtual int materialId { get; set; }
        ///<summary>来源货位</summary>
        public virtual int fromWarelocationId { get; set; }
        ///<summary>目标货位</summary>
        public virtual int toWarelocationId { get; set; }
        ///<summary>数量</summary>
        public virtual decimal quantity { get; set; }
        ///<summary>参考</summary>
        public virtual string reference { get; set; }

        //辅助信息
        public string _createTime
        {
            get
            {
                if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null)
                    return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm");
                return "";
            }
        }
        public string _modifyTime
        {
            get
            {
                if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null)
                    return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm");
                return "";
            }
        }
        public AscmWarelocation fromWarelocation { get; set; }
        public string fromWarelocationDocNumber { get { if (fromWarelocation != null) return fromWarelocation.docNumber; return ""; } }
        public AscmWarelocation toWarelocation { get; set; }
        public string toWarelocationDocNumber { get { if (toWarelocation != null) return toWarelocation.docNumber; return ""; } }
        public AscmMaterialItem ascmMaterialItem { get; set; }
        ///<summary>物料编码</summary>
        public string materialDocNumber { get { if (ascmMaterialItem != null) return ascmMaterialItem.docNumber; return ""; } }
        ///<summary>物料描述</summary>
        public string materialDescription { get { if (ascmMaterialItem != null) return ascmMaterialItem.description; return ""; } }
        ///<summary>物料单位</summary>
        public string materialUnit { get { if (ascmMaterialItem != null) return ascmMaterialItem.unit; return ""; } }
        ///<summary>来源货位物料数量</summary>
        public virtual decimal fromQuantity { get; set; }
    }
}
