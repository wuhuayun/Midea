using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.Warehouse.Entities
{
    public class AscmWmsLocationTransfer
    {
        /// <summary>ID</summary>
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
        /// <summary>来源货位</summary>
        public virtual int fromWarelocationId { get; set; }
        /// <summary>目标货位</summary>
        public virtual int toWarelocationId { get; set; }
        /// <summary>物料ID</summary>
        public virtual int materialId { get; set; }
        /// <summary>数量</summary>
        public virtual decimal quantity { get; set; }
        /// <summary>操作方式：取、存</summary>
        public virtual string operateType { get; set; }

        //辅助信息
        public string operateTypeCn { get { return OperateTypeDefine.DisplayText(operateType); } }
        public AscmWarelocation fromWarelocation { get; set; }
        public string fromLocationDocNumber { get { if (fromWarelocation != null) return fromWarelocation.docNumber; return ""; } }
        public AscmWarelocation toWarelocation { get; set; }
        public string toLocationDocNumber { get { if (toWarelocation != null) return toWarelocation.docNumber; return ""; } }
        public AscmMaterialItem ascmMaterialItem { get; set; }
        public string materialDocNumber { get { if (ascmMaterialItem != null) return ascmMaterialItem.docNumber; return ""; } }
        public string materialName { get { if (ascmMaterialItem != null) return ascmMaterialItem.description; return ""; } }

        public class OperateTypeDefine
        {
            /// <summary>取出</summary>
            public const string takeOut = "TAKEOUT";
            /// <summary>存放</summary>
            public const string putIn = "PUTIN";

            public static string DisplayText(string operateType)
            {
                switch (operateType)
                {
                    case takeOut: return "取";
                    case putIn: return "存";
                    default: return operateType;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(takeOut);
                list.Add(putIn);
                return list;
            }
        }
    }
}
