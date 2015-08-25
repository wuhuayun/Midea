using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>手工单明细</summary>
    public class AscmWmsIncManAccDetail
    {
        /// <summary>ID</summary>
        public virtual int incManAccDetailId { get; set; }
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
        ///<summary>表头Id</summary>
        public virtual int incManAccMainId { get; set; }
        ///<summary>物料Id</summary>
        public virtual int materialId { get; set; }
        ///<summary>要求到货时间</summary>
        public virtual string requestDeliveryDate { get; set; }
        ///<summary>送货数量</summary>
        public virtual int deliveryQuantity { get; set; }
        ///<summary>接收数量</summary>
        public virtual int receivedQuantity { get; set; }
        ///<summary>货位ID</summary>
        public virtual int warelocationId { get; set; }
		/// <summary>上传返回代码</summary>
		public virtual string returnCode { get; set; }
		/// <summary>上传返回消息</summary>
		public virtual string returnMessage { get; set; }

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
        public virtual string requestDeliveryDateShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(requestDeliveryDate) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(requestDeliveryDate)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual AscmMaterialItem ascmMaterialItem { get; set; }
        public virtual string materialDocNumber 
        { 
            get { 
                if (ascmMaterialItem != null && ascmMaterialItem.docNumber != null) 
                    return ascmMaterialItem.docNumber.Trim(); 
                return "";
            }
            set 
            {
                if (ascmMaterialItem == null) 
                {
                    ascmMaterialItem = new AscmMaterialItem();
                }

                ascmMaterialItem.docNumber = value;
            }
        }

        public virtual string materialName 
        { 
            get 
            { 
                if (ascmMaterialItem != null && ascmMaterialItem.description != null) 
                    return ascmMaterialItem.description.Trim(); 
                return ""; 
            }
            set
            {
                if (ascmMaterialItem == null)
                {
                    ascmMaterialItem = new AscmMaterialItem();
                }

                ascmMaterialItem.description = value;
            }
        }

        public virtual string materialUnit 
        { 
            get 
            { 
                if (ascmMaterialItem != null && ascmMaterialItem.unit != null) 
                    return ascmMaterialItem.unit.Trim(); 
                return ""; 
            }
            set
            {
                if (ascmMaterialItem == null)
                {
                    ascmMaterialItem = new AscmMaterialItem();
                }

                ascmMaterialItem.unit = value;
            }
        }

        public virtual AscmWarelocation ascmWarelocation { get; set; }
        
        public virtual string warelocationdocNumber 
        { 
            get 
            { 
                if (ascmWarelocation != null && ascmWarelocation.docNumber != null) 
                    return ascmWarelocation.docNumber.Trim(); 
                return ""; 
            }
            set
            {
                //if (ascmWmsIncManAccMain == null)
                //{
                //    ascmWmsIncManAccMain = new AscmWmsIncManAccMain();
                //}

                //ascmWmsIncManAccMain.docNumber = value;

                //用于货位编码
                if (ascmWarelocation == null)
                {
                    ascmWarelocation = new AscmWarelocation();
                }

                ascmWarelocation.docNumber = value;
            }
        }
        
        public virtual AscmWmsIncManAccMain ascmWmsIncManAccMain { get; set; }
        
        public string docNumber 
        { 
            get 
            { 
                if (ascmWmsIncManAccMain != null && ascmWmsIncManAccMain.docNumber != null) 
                    return ascmWmsIncManAccMain.docNumber.Trim(); 
                return ""; 
            }
            set
            {
                //if (ascmWmsIncManAccMain == null)
                //{
                //    ascmWmsIncManAccMain = new AscmWmsIncManAccMain();
                //}

                //ascmWmsIncManAccMain.docNumber = value;

                //用于货位编码
                if (ascmWarelocation == null)
                {
                    ascmWarelocation = new AscmWarelocation();
                }

                ascmWarelocation.docNumber = value;
            }
        }
        
        public string supperWarehouse { get { if (ascmWmsIncManAccMain != null && ascmWmsIncManAccMain.supperWarehouse != null) return ascmWmsIncManAccMain.supperWarehouse.Trim(); return ""; } }
        public string warehouseId { get { if (ascmWmsIncManAccMain != null && ascmWmsIncManAccMain.warehouseId != null) return ascmWmsIncManAccMain.warehouseId.Trim(); return ""; } }
        public string supplierAddressVendorSiteCode { get { if (ascmWmsIncManAccMain != null && ascmWmsIncManAccMain.supplierAddressVendorSiteCode != null) return ascmWmsIncManAccMain.supplierAddressVendorSiteCode.Trim(); return ""; } }
        public virtual string supplierName { get { if (ascmWmsIncManAccMain != null && ascmWmsIncManAccMain.supplierName != null) return ascmWmsIncManAccMain.supplierName.Trim(); return ""; } }
    }
}