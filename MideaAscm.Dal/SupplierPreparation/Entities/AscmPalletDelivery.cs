using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.SupplierPreparation.Entities
{
    ///<summary>托盘送货备料</summary>
    public class AscmPalletDelivery
    {
        ///<summary>Id</summary>
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
        ///<summary>托盘SN</summary>
        public virtual string palletSn { get; set; }
        ///<summary>合单Id</summary>
        public virtual int batSumMainId { get; set; }
        ///<summary>批送货单Id</summary>
        public virtual int deliveryOrderBatchId { get; set; }
        ///<summary>送货单Id</summary>
        public virtual int deliveryOrderId { get; set; }
        ///<summary>送货单物料Id</summary>
        public virtual int materialId { get; set; }
        ///<summary>数量</summary>
        public virtual decimal quantity { get; set; }
        ///<summary>状态</summary>
        public virtual string status { get; set; }
        ///<summary>备注</summary>
        public virtual string memo { get; set; }

        //辅助属性
        public virtual AscmMaterialItem ascmMaterialItem { get; set; }
        public virtual string materialDocNumber { get { if (ascmMaterialItem != null && ascmMaterialItem.docNumber != null) return ascmMaterialItem.docNumber.Trim(); return ""; } }
        public virtual string materialName { get { if (ascmMaterialItem != null && ascmMaterialItem.description != null) return ascmMaterialItem.description.Trim(); return ""; } }
        public class StatusDefine
        {
            /// <summary>未确认</summary>
            public static readonly string none = "none";
            /// <summary>入厂</summary>
            public static readonly string inWarehouseDoor = "inWarehouseDoor";

            public static string DisplayText(string status)
            {
                switch (status)
                {
                    case "none": return "";
                    case "inWarehouseDoor": return "入厂";
                    default: return status;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(none);
                list.Add(inWarehouseDoor);
                return list;
            }
        }
    }
}
