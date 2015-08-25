using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.GetMaterialManage.Entities
{
    public class ImOrExMaterialDefine
    {
        /// <summary>物料编码</summary>
        public virtual string materialDocnumber { get; set; }

        public virtual string wipSupplyType { get; set; }
        /// <summary>物料描述</summary>
        public virtual string materialDescription { get; set; }
        /// <summary>总装备料形式</summary>
        public virtual string zMtlCategoryStatus { get; set; }
        /// <summary>电装备料形式</summary>
        public virtual string dMtlCategoryStatus { get; set; }
        /// <summary>其他备料形式{仓库使用}</summary>
        public virtual string wMtlCategoryStatus { get; set; }
        /// <summary>行号</summary>
        public int rowNumber { get; set; }

        public const string MaterialDocnumber = "materialDocnumber";
        public const string WipSupplyType = "wipSupplyType";
        public const string MaterialDescription = "materialDescription";
        public const string ZMtlCategoryStatus = "zMtlCategoryStatus";
        public const string DMtlCategoryStatus = "dMtlCategoryStatus";
        public const string WMtlCategoryStatus = "wMtlCategoryStatus";

        public static string DisplayText(string value)
        {
            if (value == MaterialDocnumber)
                return "物料编码";
            else if (value == WipSupplyType)
                return "供应类型";
            else if (value == MaterialDescription)
                return "物料描述";
            else if (value == ZMtlCategoryStatus)
                return "总装备料形式";
            else if (value == DMtlCategoryStatus)
                return "电装备料形式";
            else if (value == WMtlCategoryStatus)
                return "其他备料形式";

            return "";
        }

        public static List<string> GetList()
        {
            List<string> list = new List<string>();
            list.Add(MaterialDocnumber);
            list.Add(WipSupplyType);
            list.Add(MaterialDescription);
            list.Add(ZMtlCategoryStatus);
            list.Add(DMtlCategoryStatus);
            list.Add(WMtlCategoryStatus);

            return list;
        }
    }
}
