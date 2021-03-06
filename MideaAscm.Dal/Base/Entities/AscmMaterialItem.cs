﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Dal.Base.Entities
{
    public class AscmMaterialItem
    {
        ///<summary>物料id</summary>
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
        ///<summary>编号</summary>
        public virtual string docNumber { get; set; }
        ///<summary>名称</summary>
        public virtual string name { get; set; }
        ///<summary>描述</summary>
        public virtual string description { get; set; }
        ///<summary>物料计量单位</summary>
        public virtual string unit { get; set; }
        ///<summary>采购员Id</summary>
        public virtual int buyerId { get; set; }
        ///<summary>总装备料形式</summary>
        public virtual string zMtlCategoryStatus { get; set; }
        ///<summary>电装备料形式</summary>
        public virtual string dMtlCategoryStatus { get; set; }
        ///<summary>供应类型</summary>
        public virtual int wipSupplyType { get; set; }
        ///<summary>子类</summary>
        public virtual int? subCategoryId { get; set; }
        /// <summary>标示符（判断物料编码是否产生大类小类）</summary>
        public virtual int isFlag { get; set; }
        /// <summary>其他备料形式{仓库使用}</summary>
        public virtual string wMtlCategoryStatus { get; set; }

        public AscmMaterialItem() { }
        public AscmMaterialItem(int id, string docNumber, string description) 
        {
            this.id = id;
            this.docNumber = docNumber;
            this.description = description;
        }
        public AscmMaterialItem GetOwner()
        {
            return (AscmMaterialItem)this.MemberwiseClone();
        }

        //辅助信息
        public virtual AscmEmployee ascmBuyerEmployee { get; set; }
        public virtual string buyerName { get { if (ascmBuyerEmployee != null && ascmBuyerEmployee.name != null) return ascmBuyerEmployee.name.Trim(); return ""; } }
        ///<summary>送货数量</summary>
        public virtual decimal deliveryQuantity { get; set; }
        public virtual string _zMtlCategoryStatus { get { return MtlCategoryStatusDefine.DisplayText(zMtlCategoryStatus); } }
        public virtual string _dMtlCategoryStatus { get { return MtlCategoryStatusDefine.DisplayText(dMtlCategoryStatus); } }
        public virtual string _wMtlCategoryStatus { get { return MtlCategoryStatusDefine.DisplayText(wMtlCategoryStatus); } }
        public virtual string wipSupplyTypeCn
        {
            get
            {
                if (wipSupplyType == 1)
                {
                    return "推式";
                }
                else if (wipSupplyType == 2)
                {
                    return "装配拉式";
                }
                else if (wipSupplyType == 3)
                {
                    return "拉式工序";
                }
                else if (wipSupplyType == 4)
                {
                    return "批量";
                }
                else if (wipSupplyType == 5)
                {
                    return "供应商";
                }
                else if (wipSupplyType == 6)
                {
                    return "虚拟件";
                }
                else if (wipSupplyType == 7)
                {
                    return "基于物料清单";
                }
                return wipSupplyType.ToString();
            }
        }
        public virtual string warehouseName { get; set; }
        public virtual string warehouseDes { get; set; }
        
        public virtual AscmWipEntities ascmWipEntities { get; set; }
        public virtual string ascmWipEntities_Name { get { if (ascmWipEntities != null) return ascmWipEntities.name; return ""; } }
        public virtual int ascmWipEntities_Id { get { if (ascmWipEntities != null) return ascmWipEntities.wipEntityId; return 0; } }
        public virtual AscmWipDiscreteJobs ascmWipDiscreteJobs { get; set; }
        public virtual decimal ascmWipDiscreteJobs_Count { get { if (ascmWipDiscreteJobs != null) return ascmWipDiscreteJobs.netQuantity; return 0; } }
        public virtual AscmMaterialSubCategory ascmMaterialSubCategory { get; set; }
        public virtual string ascmMaterialSubCategory_Code { get { if (ascmMaterialSubCategory != null) return ascmMaterialSubCategory.categoryCode; return ""; } }
        public virtual decimal quantityIssued { get; set; }//发料数量
        public virtual decimal requiredQuantity { get; set; }//需求数量
        public virtual decimal quantityDifference { get; set; }//差异数量
        public virtual decimal getMaterialQuantity { get; set; }//领料数量
        public virtual decimal quantityGetMaterialDifference { get; set; }//领料差异数量
        public virtual string sDocnumber { get; set; }//起始编码段
        public virtual string eDocnumber { get; set; }//起始编码段

        public virtual decimal totalNumber { get; set; }//总数量
        /// <summary>物料单元数</summary>
        public virtual decimal unitQuantity { get; set; }
        ///<summary>容器</summary>
        public string container { get; set; }
        ///<summary>容器+单元数字符串</summary>
        public string containerUnitQuantity { get; set; }
        ///<summary>容器单元数id</summary>
        public int containerUnitQuantityId { get; set; }

        public int warelocationId { get; set; }
        public string warelocationDoc { get; set; }
        /// <summary>物料条码</summary>
        public virtual byte[] materialBarcode { get; set; }

        #region 供应类型
        public class WipSupplyTypeDefine
        {
            /// <summary>推式</summary>
            public const int ts = 1;
            /// <summary>装配拉式</summary>
            public const int zpls = 2;
            /// <summary>拉式工序</summary>
            public const int lsgx = 3;
            /// <summary>批量</summary>
            public const int pl = 4;
            /// <summary>供应商</summary>
            public const int gys = 5;
            /// <summary>虚拟件</summary>
            public const int xnj = 6;
            /// <summary>基于物料清单</summary>
            public const int jywlqd = 7;

            public static string DisplayText(int value)
            {
                if (value == ts)
                    return "推式";
                else if (value == zpls)
                    return "装配拉式";
                else if (value == lsgx)
                    return "拉式工序";
                else if (value == pl)
                    return "批量";
                else if (value == gys)
                    return "供应商";
                else if (value == xnj)
                    return "虚拟件";
                else if (value == jywlqd)
                    return "基于物料清单";
                return value.ToString();
            }

            public static List<int> GetList()
            {
                List<int> list = new List<int>();
                list.Add(ts);
                list.Add(zpls);
                list.Add(lsgx);
                list.Add(pl);
                list.Add(gys);
                list.Add(xnj);
                list.Add(jywlqd);
                return list;
            }
        }
        #endregion
    }
    public class AscmMaterialItemComparer : IEqualityComparer<AscmMaterialItem>
    {
        public bool Equals(AscmMaterialItem x, AscmMaterialItem y)
        {
            if (x == null)
                throw new ArgumentNullException("x");
            if (y == null)
                throw new ArgumentNullException("y");
            return x.id == y.id;
        }

        public int GetHashCode(AscmMaterialItem obj)
        {
            return base.GetHashCode();
        }
    }
}
