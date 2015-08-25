using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Dal.SupplierPreparation.Entities;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>供应商退货主表</summary>
    public class AscmWmsBackInvoiceMain
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
        /// <summary>退货单号:从MES申请</summary>
        public virtual string docNumber { get; set; }
        /// <summary>手工退货单号（仓管员手工输入）</summary>
        public virtual string manualDocNumber { get; set; }
        ///<summary>退货原因</summary>
        public virtual int reasonId { get; set; }
        ///<summary>默认仓库</summary>
        public virtual string warehouseId { get; set; }
        ///<summary>送货合单ID</summary>
        public virtual int batSumMainId { get; set; }
        ///<summary>供应商Id</summary>
        public virtual int supplierId { get; set; }
        ///<summary>状态：待审核、完成</summary>
        public virtual string status { get; set; }
        ///<summary>备注</summary>
        public virtual string memo { get; set; }
        ///<summary>责任人</summary>
        public virtual string responsiblePerson { get; set; }
        ///<summary>状态</summary>
        public virtual string accountStatus { get; set; }
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
        public string statusCn { get { return StatusDefine.DisplayText(status); } }
        public AscmDeliBatSumMain ascmDeliBatSumMain { get; set; }
        public string batSumMainDocNumber { get { if (ascmDeliBatSumMain != null) return ascmDeliBatSumMain.docNumber; return ""; } }
        public AscmSupplier ascmSupplier { get; set; }
        public string supplierName { get { if (ascmSupplier != null && ascmSupplier.name != null) return ascmSupplier.name.Trim(); return ""; } }
        public string supplierDocNumber { get { if (ascmSupplier != null && ascmSupplier.docNumber != null) return ascmSupplier.docNumber.Trim(); return ""; } }
        public AscmMtlTransactionReasons ascmMtlTransactionReasons { get; set; }
        public string reasonName { get { if (ascmMtlTransactionReasons != null) return ascmMtlTransactionReasons.reasonName; return ""; } }

        public class StatusDefine
        {
            /// <summary>待审核</summary>
            public const string unVerify = "UNVERIFY";
            /// <summary>完成</summary>
            public const string success = "SUCCESS";

            public static string DisplayText(string status)
            {
                switch (status)
                {
                    case unVerify: return "待审核";
                    case success: return "完成";
                    default: return status;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(unVerify);
                list.Add(success);
                return list;
            }
        }
    }
}
