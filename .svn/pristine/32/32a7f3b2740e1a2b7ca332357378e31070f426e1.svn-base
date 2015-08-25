using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>手工单接收</summary>
    public class AscmWmsIncManAccMain
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
        ///<summary>供应商Id</summary>
        public virtual int supplierId { get; set; }
        ///<summary>供应商地址Id</summary>
        public virtual int supplierAddressId { get; set; }
        ///<summary>出货子库</summary>
        public virtual string supperWarehouse { get; set; }
        ///<summary>车牌号</summary>
        public virtual string supperPlateNumber { get; set; }
        ///<summary>联系方式</summary>
        public virtual string supperTelephone { get; set; }
        ///<summary>收货子库</summary>
        public virtual string warehouseId { get; set; }
        ///<summary>作业</summary>
        public virtual int wipEntityId { get; set; }
        /// <summary>备注</summary>
        public virtual string memo { get; set; }
        /// <summary>责任人</summary>
        public virtual string responsiblePerson { get; set; }
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
		
        public string createTimeShow
        {
            get
            {
                if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null)
                    return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm");
                return "";
            }
        }
        public string modifyTimeShow
        {
            get
            {
                if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null)
                    return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm");
                return "";
            }
        }
        public virtual AscmSupplier ascmSupplier { get; set; }
        public virtual string supplierName { get { if (ascmSupplier != null && ascmSupplier.name != null) return ascmSupplier.name.Trim(); return ""; } }
        public virtual string supplierDocNumber { get { if (ascmSupplier != null && ascmSupplier.docNumber != null) return ascmSupplier.docNumber.Trim(); return ""; } }
        public virtual AscmSupplierAddress ascmSupplierAddress { get; set; }
        public virtual string supplierAddressVendorSiteCode { get { if (ascmSupplierAddress != null && ascmSupplierAddress.vendorSiteCode != null) return ascmSupplierAddress.vendorSiteCode.Trim(); return ""; } }
        public virtual string supplierAddressVendorSiteCodeAlt { get { if (ascmSupplierAddress != null && ascmSupplierAddress.vendorSiteCodeAlt != null) return ascmSupplierAddress.vendorSiteCodeAlt.Trim(); return ""; } }
        public MideaAscm.Dal.Base.Entities.AscmWarehouse ascmWarehouse { get; set; }
        public string warehouseDescription { get { if (ascmWarehouse != null) return ascmWarehouse.description; return ""; } }
    }
}