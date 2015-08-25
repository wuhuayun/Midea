using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Warehouse.Entities
{
    public class AscmMesInteractiveLog
    {
        /// <summary>日志ID</summary>
        public int id { get; set; }
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
        ///<summary>单据ID</summary>
        public virtual int billId { get; set; }
        ///<summary>单据号</summary>
        public virtual string docNumber { get; set; }
        ///<summary>单据类型</summary>
        public virtual string billType { get; set; }
        ///<summary>返回代码</summary>
        public virtual string returnCode { get; set; }
        ///<summary>返回信息</summary>
        public virtual string returnMessage { get; set; }

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
        public string billTypeCn { get { return BillTypeDefine.DisplayText(billType); } }

        public class BillTypeDefine
        {
            /// <summary>到货接收_系统单</summary>
            public const string incAccSystem = "IncAccSystem";
            /// <summary>到货接收_手工单</summary>
            public const string incAccManual = "IncAccManual";
            /// <summary>供应商退货</summary>
            public const string goodsReject = "GoodsReject";
            /// <summary>子库存转移</summary>
            public const string stockTrans = "StockTrans";
            /// <summary>作业领料</summary>
            public const string mtlRequisition = "MtlRequisition";
            /// <summary>作业退料</summary>
            public const string mtlReturnManual = "MtlReturnManual";
            /// <summary>退料单退料</summary>
            public const string mtlReturnSystem = "MtlReturnSystem";

            public static string DisplayText(string billType)
            {
                switch (billType)
                {
                    case incAccSystem: return "到货接收_系统单";
                    case incAccManual: return "到货接收_手工单";
                    case goodsReject: return "供应商退货";
                    case stockTrans: return "子库存转移";
                    case mtlRequisition: return "作业领料";
                    case mtlReturnManual: return "作业退料";
                    case mtlReturnSystem: return "退料单退料";
                    default: return billType;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(incAccSystem);
                list.Add(incAccManual);
                list.Add(goodsReject);
                list.Add(stockTrans);
                list.Add(mtlRequisition);
                list.Add(mtlReturnManual);
                list.Add(mtlReturnSystem);
                return list;
            }
        }
    }
}
