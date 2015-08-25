using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>物料标签</summary>
    public class AscmWmsMaterialLabel
    {
        /// <summary>标签号</summary>
        public virtual string labelNo { get; set; }
        /// <summary>标题</summary>
        public virtual string title { get; set; }
        /// <summary>子库</summary>
        public virtual string warehouseId { get; set; }
        /// <summary>作业日期</summary>
        public virtual string wipEntityDate { get; set; }
        /// <summary>作业号</summary>
        public virtual string wipEntityName { get; set; }
        /// <summary>作业数量</summary>
        public virtual decimal wipEntityQuantity { get; set; }
        /// <summary>供应商ID</summary>
        public virtual int supplierId { get; set; }
        /// <summary>供应商简称</summary>
        public virtual string supplierShortName { get; set; }
        /// <summary>数量</summary>
        public virtual decimal quantity { get; set; }
        /// <summary>入库日期</summary>
        public virtual string enterWarehouseDate { get; set; }
        /// <summary>物料ID</summary>
        public virtual int materialId { get; set; }
        /// <summary>物料编码</summary>
        public virtual string materialDocNumber { get; set; }
        /// <summary>物料描述</summary>
        public virtual string materialDescription { get; set; }
        /// <summary>检验结果</summary>
        public virtual string checkResult { get; set; }
        /// <summary>打印方式</summary>
        public virtual string printType { get; set; }

        //辅助信息
        public string _wipEntityDate { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(wipEntityDate) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(wipEntityDate)).ToString("yyyy/MM/dd"); return ""; } }
        public string _enterWarehouseDate { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(enterWarehouseDate) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(enterWarehouseDate)).ToString("yyyy/MM/dd"); return ""; } }
        public string checkResultCn { get { return CheckResult.DisplayText(checkResult); } }

		/// <summary>货位编码</summary>
		public string locationDocNumber { get; set; }

        /// <summary>检验结果</summary>
        public struct CheckResult
        {
            /// <summary>未检</summary>
            public const string unCheck = "unCheck";

            public static string DisplayText(string checkResult)
            {
                switch (checkResult)
                {
                    case unCheck: return "未检";
                    default: return checkResult;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(unCheck);
                return list;
            }
        }
        /// <summary>打印方式</summary>
        public struct PrintType
        {
            /// <summary>批单打印</summary>
            public const string batchPrint = "batchPrint";
            /// <summary>作业打印</summary>
            public const string wipEntityPrint = "wipEntityPrint";

            public static string DisplayText(string printType)
            {
                switch (printType)
                {
                    case batchPrint: return "批单打印";
                    case wipEntityPrint: return "作业打印";
                    default: return printType;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(batchPrint);
                list.Add(wipEntityPrint);
                return list;
            }
        }
    }
}
