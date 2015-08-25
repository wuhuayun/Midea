using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.GetMaterialManage.Entities
{
    /// <summary>
    /// 定义物料类别状态（备料形式）
    /// </summary>
    public class MtlCategoryStatusDefine
    {
        /// <summary>有库存</summary>
        public const string inStock = "INSTOCK";
        /// <summary>须备料</summary>
        public const string preStock = "PRESTOCK";
        /// <summary>须配料</summary>
        public const string mixStock = "MIXSTOCK";

        public static string DisplayText(string value)
        {
            if (value == inStock)
                return "有库存";
            else if (value == preStock)
                return "须备料";
            else if (value == mixStock)
                return "须配料";
            return value;
        }

        public static List<string> GetList()
        {
            List<string> list = new List<string>();
            list.Add(inStock);
            list.Add(preStock);
            list.Add(mixStock);
            return list;
        }
    }
}
