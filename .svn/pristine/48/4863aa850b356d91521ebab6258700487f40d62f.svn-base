using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>货位</summary>
    public class AscmWarelocation
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
        /// <summary>编码</summary>
        public virtual string docNumber { get; set; }
        ///<summary>绑定rfid</summary>
        public virtual string rfid { get; set; }
        /// <summary>厂房ID</summary>
        public virtual int buildingId { get; set; }
        /// <summary>厂房区域</summary>
        public virtual string buildingArea { get; set; }
        /// <summary>子库ID</summary>
        public virtual string warehouseId { get; set; }
        /// <summary>物料大类编码(取物料前四位编码)</summary>
        public virtual string categoryCode { get; set; }
        /// <summary>功能属性</summary>
        public virtual string funcProperty { get; set; }
        /// <summary>货位类型</summary>
        public virtual int type { get; set; }
        /// <summary>货架号</summary>
        public virtual string shelfNo { get; set; }
        /// <summary>层</summary>
        public virtual int layer { get; set; }
        /// <summary>货位号</summary>
        public virtual string No { get; set; }
        /// <summary>行</summary>
        public virtual string floorRow { get; set; }
        /// <summary>列</summary>
        public virtual string floorColumn { get; set; }
        /// <summary>上限</summary>
        public virtual int upperLimit { get; set; }
        /// <summary>下限</summary>
        public virtual int lowerLimit { get; set; }
        /// <summary>描述</summary>
        public virtual string description { get; set; }
        /// <summary>仓管员Id</summary>
        public virtual string warehouseUserId { get; set; }

        public AscmWarelocation() { }
        public AscmWarelocation(AscmWarelocation ascmWarelocation, decimal totalNumber)
        {
            this.id = ascmWarelocation.id;
            this.organizationId = ascmWarelocation.organizationId;
            this.createUser = ascmWarelocation.createUser;
            this.createTime = ascmWarelocation.createTime;
            this.modifyUser = ascmWarelocation.modifyUser;
            this.modifyTime = ascmWarelocation.modifyTime;
            this.docNumber = ascmWarelocation.docNumber;
            this.buildingId = ascmWarelocation.buildingId;
            this.warehouseId = ascmWarelocation.warehouseId;
            this.categoryCode = ascmWarelocation.categoryCode;
            this.type = ascmWarelocation.type;
            this.shelfNo = ascmWarelocation.shelfNo;
            this.layer = ascmWarelocation.layer;
            this.No = ascmWarelocation.No;
            this.floorRow = ascmWarelocation.floorRow;
            this.floorColumn = ascmWarelocation.floorColumn;
            this.upperLimit = ascmWarelocation.upperLimit;
            this.lowerLimit = ascmWarelocation.lowerLimit;
            this.description = ascmWarelocation.description;
            this.warehouseUserId = ascmWarelocation.warehouseUserId;

            this.totalNumber = totalNumber;
        }
        public AscmWarelocation(AscmWarelocation ascmWarelocation, decimal totalNumber, string warehouseDes)
        {
            this.id = ascmWarelocation.id;
            this.organizationId = ascmWarelocation.organizationId;
            this.createUser = ascmWarelocation.createUser;
            this.createTime = ascmWarelocation.createTime;
            this.modifyUser = ascmWarelocation.modifyUser;
            this.modifyTime = ascmWarelocation.modifyTime;
            this.docNumber = ascmWarelocation.docNumber;
            this.buildingId = ascmWarelocation.buildingId;
            this.warehouseId = ascmWarelocation.warehouseId;
            this.categoryCode = ascmWarelocation.categoryCode;
            this.type = ascmWarelocation.type;
            this.shelfNo = ascmWarelocation.shelfNo;
            this.layer = ascmWarelocation.layer;
            this.No = ascmWarelocation.No;
            this.floorRow = ascmWarelocation.floorRow;
            this.floorColumn = ascmWarelocation.floorColumn;
            this.upperLimit = ascmWarelocation.upperLimit;
            this.lowerLimit = ascmWarelocation.lowerLimit;
            this.description = ascmWarelocation.description;
            this.warehouseUserId = ascmWarelocation.warehouseUserId;

            this.totalNumber = totalNumber;
            this.warehouseDes = warehouseDes;
        }
        public AscmWarelocation(AscmWarelocation ascmWarelocation, int batchId, decimal assignQuantity)
        {
            this.id = ascmWarelocation.id;
            this.organizationId = ascmWarelocation.organizationId;
            this.createUser = ascmWarelocation.createUser;
            this.createTime = ascmWarelocation.createTime;
            this.modifyUser = ascmWarelocation.modifyUser;
            this.modifyTime = ascmWarelocation.modifyTime;
            this.docNumber = ascmWarelocation.docNumber;
            this.buildingId = ascmWarelocation.buildingId;
            this.warehouseId = ascmWarelocation.warehouseId;
            this.categoryCode = ascmWarelocation.categoryCode;
            this.type = ascmWarelocation.type;
            this.shelfNo = ascmWarelocation.shelfNo;
            this.layer = ascmWarelocation.layer;
            this.No = ascmWarelocation.No;
            this.floorRow = ascmWarelocation.floorRow;
            this.floorColumn = ascmWarelocation.floorColumn;
            this.upperLimit = ascmWarelocation.upperLimit;
            this.lowerLimit = ascmWarelocation.lowerLimit;
            this.description = ascmWarelocation.description;
            this.warehouseUserId = ascmWarelocation.warehouseUserId;

            this.batchId = batchId;
            this.assignQuantity = assignQuantity;
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
        public string warehouseUserName { get; set; }
        public string typeCn { get { return TypeDefine.DisplayText(type); } }
        public string funcPropertyCn { get { return FuncPropertyDefine.DisplayText(funcProperty); } }
        public AscmWorkshopBuilding ascmWorkshopBuilding { get; set; }
        public string buildingName { get { if (ascmWorkshopBuilding != null) return ascmWorkshopBuilding.name; return ""; } }
        public AscmWarehouse ascmWarehouse { get; set; }
        public string warehouseName { get { if (ascmWarehouse != null) return ascmWarehouse.description; return ""; } }
        public string warehouseDes { get; set; }

        /// <summary>货位存放物料的列表</summary>
        public List<AscmLocationMaterialLink> listAscmLocationMaterialLink { get; set; }

        public decimal assignQuantity { get; set; }
        public int batchId { get; set; }
        public string batchDocNumber { get; set; }
        public int materialId { get; set; }
        public string materialDocNumber { get; set; }
        /// <summary>存放货物数量</summary>
        public decimal totalNumber { get; set; }

        public class FuncPropertyDefine
        {
            /// <summary>物料区</summary>
            public const string materialArea = "MATERIALAREA";
            /// <summary>备料区</summary>
            public const string preparationArea  = "PREPARATIONAREA";
            /// <summary>不合格品</summary>
            public const string disqualifiedProduct = "DISQUALIFIEDPRODUCT";
            /// <summary>其它</summary>
            public const string otherArea = "OTHERAREA";

            public static string DisplayText(string funcProperty)
            {
                switch (funcProperty)
                {
                    case materialArea: return "物料区";
                    case preparationArea: return "备料区";
                    case disqualifiedProduct: return "不合格品";
                    case otherArea: return "其它";
                    default: return funcProperty;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(materialArea);
                list.Add(preparationArea);
                list.Add(disqualifiedProduct);
                list.Add(otherArea);
                return list;
            }
        }
        public class TypeDefine
        {
            public const int shelf = 1;
            public const int highShelf = 2;
            public const int floor = 3;

            public static string DisplayText(int type)
            {
                switch (type)
                {
                    case shelf: return "货架";
                    case highShelf: return "高位货架";
                    case floor: return "地面(区域)";
                    default: return type.ToString();
                }
            }
            public static List<int> GetList()
            {
                List<int> list = new List<int>();
                list.Add(shelf);
                list.Add(highShelf);
                list.Add(floor);
                return list;
            }
        }
    }
}
