using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>退料主表</summary>
    public class AscmWmsMtlReturnMain
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
        /// <summary>单号:从MES申请</summary>
        public virtual string docNumber { get; set; }
        ///<summary>ERP退料单ID</summary>
        public virtual int releaseHeaderId { get; set; }
        ///<summary>作业ID</summary>
        public virtual int wipEntityId { get; set; }
        /// <summary>子库ID（作业退料需在主表中指定子库）</summary>
        public virtual string warehouseId { get; set; }
        /// <summary>退料区域</summary>
        public virtual string returnArea { get; set; }
        ///<summary>退货原因</summary>
        public virtual int reasonId { get; set; }
        /// <summary>备注</summary>
        public virtual string memo { get; set; }
        /// <summary>单据类型：ERP退料单退料、作业退料</summary>
        public virtual string billType { get; set; }
		/// <summary>上传返回代码</summary>
		public virtual string returnCode { get; set; }
		/// <summary>上传返回消息</summary>
		public virtual string returnMessage { get; set; }

		//上传状态
		public string uploadStatusCn
		{
			get
			{
				if (string.IsNullOrEmpty(returnCode)) return "";
				if (returnCode == "0") return "成功！";

				return "失败！";
			}
		}

		/// <summary>上传日期</summary>
		public virtual string uploadTime { get; set; }

		//上传日期
		public string uploadTimeShow
		{
			get
			{
				if (YnBaseClass2.Helper.DateHelper.GetDateTime(uploadTime) != null)
					return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(uploadTime)).ToString("yyyy-MM-dd");
				return "";
			}
		}

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
        public string returnAreaCn { get { return ReturnAreaDefine.DisplayText(returnArea); } }
        public string billTypeCn { get { return BillTypeDefine.DisplayText(billType); } }
        public AscmWipEntities ascmWipEntities { get; set; }
        public string wipEntityName
        {
            get
            {
                if (ascmWipEntities != null)
                    return ascmWipEntities.name;
                return string.Empty;
            }
        }
        public string wipEntitiesName { get; set; }
        public AscmMtlTransactionReasons ascmMtlTransactionReasons { get; set; }
        public string reasonName { get { if (ascmMtlTransactionReasons != null) return ascmMtlTransactionReasons.reasonName; return ""; } }
        public AscmCuxWipReleaseHeaders ascmCuxWipReleaseHeaders { get; set; }
        public string releaseNumber { get { if (ascmCuxWipReleaseHeaders != null) return ascmCuxWipReleaseHeaders.releaseNumber; return ""; } }
        public string releaseStatus { get { if (ascmCuxWipReleaseHeaders != null) return ascmCuxWipReleaseHeaders.releaseStatus; return ""; } }

        /// <summary>退料区域</summary>
        public class ReturnAreaDefine
        {
            /// <summary>产线区</summary>
            public const string onWip = "OnWip";
            /// <summary>配套区</summary>
            public const string stock = "Stock";
            /// <summary>暂存区</summary>
            public const string buffer = "Buffer";

            public static string DisplayText(string returnArea)
            {
                switch (returnArea)
                {
                    case onWip: return "产线区";
                    case stock: return "配套区";
                    case buffer: return "暂存区";
                    default: return returnArea;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(onWip);
                list.Add(stock);
                list.Add(buffer);
                return list;
            }
        }
        /// <summary>退料单据类型</summary>
        public class BillTypeDefine
        {
            /// <summary>ERP退料单</summary>
            public const string erpReturnBill = "ERPRETURNBILL";
            /// <summary>作业</summary>
            public const string wipEntity = "WIPENTITY";

            public static string DisplayText(string billType)
            {
                switch (billType)
                {
                    case erpReturnBill: return "ERP退料单";
                    case wipEntity: return "作业";
                    default: return billType;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(erpReturnBill);
                list.Add(wipEntity);
                return list;
            }
        }
    }
}
