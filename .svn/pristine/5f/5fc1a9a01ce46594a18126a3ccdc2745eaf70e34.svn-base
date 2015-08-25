using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.SupplierPreparation.Entities
{
    ///<summary>标准容器</summary>
    public class AscmContainer
    {
        ///<summary>容器编号</summary>
        public virtual string sn { get; set; }
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
        ///<summary>规格</summary>
        public virtual int specId { get; set; }
        ///<summary>绑定rfid</summary>
        public virtual string rfid { get; set; }
        ///<summary>供应商id</summary>
        public virtual int supplierId { get; set; }
        ///<summary>地点(读头ID)</summary>
        public virtual string place { get; set; }
        ///<summary>状态</summary>
        public virtual int? status { get; set; }
        ///<summary>描述</summary>
        public virtual string description { get; set; }
        ///<summary>入厂时间</summary>添加于2013/07/15
        public virtual string storeInTime { get; set; }
        /// <summary>是否盘点</summary>
        public virtual int isCheck { get; set; }

        //辅助属性
        public AscmContainerSpec containerSpec { get; set; }
        public string spec
        {
            get
            {
                if (containerSpec != null)
                    return containerSpec.spec;
                return "";
            }
        }
        public Base.Entities.AscmSupplier supplier { get; set; }
        public string supplierName
        {
            get
            {
                if (supplier != null)
                    return supplier.name;
                return "";
            }
        }
        public string statusCn
        {
            get
            {
                if (status.HasValue)
                {
                    return StatusDefine.DisplayText(status.Value);
                }
                else
                {
                    return "";
                }
            }
        }
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
        public string startSn { get; set; }
        public string endSn { get; set; }

        public class StatusDefine
        {
            /// <summary>标箱作废</summary>
            public static readonly int InvalidContainer = 7;
            /// <summary>标签作废</summary>
            public static readonly int InvalidRfid = 6;
            /// <summary>已入库</summary>
            public static readonly int StoreIn = 5;
            /// <summary>已出库</summary>
            public static readonly int StoreOut = 4;
            /// <summary>未使用</summary>
            public static readonly int unuse = 3;
            /// <summary>备料</summary>
            public static readonly int preparation = 2;
            /// <summary>作废</summary>
            public static readonly int invalid = 1;
            /// </summary>已丢失<summary>
            public static readonly int losted = 0;

            public static string DisplayText(int status)
            {
                switch (status)
                {

                    case 7: return "标箱作废";
                    case 6: return "标签作废";
                    case 5: return "已入库";
                    case 4: return "已出库";
                    case 3: return "未使用";
                    case 2: return "备料";
                    case 1: return "作废";
                    case 0: return "已丢失";
                    default: return status.ToString();
                }
            }
            public static List<int> GetList()
            {
                List<int> list = new List<int>();
                list.Add(unuse);
                list.Add(preparation);
                list.Add(invalid);
                return list;
            }
            public static List<int> GetListII()
            {

                List<int> list = new List<int>();
                list.Add(unuse);
                list.Add(preparation);
                list.Add(invalid);
                list.Add(StoreIn);
                list.Add(StoreOut);
                list.Add(InvalidRfid);
                list.Add(losted);
                list.Add(InvalidContainer);
                return list;
            }
        }

        public virtual string SpecCount { get; set; }  //相同规格的数量
        public virtual string SpecName { get; set; }   //规格名称
        public virtual string deadline { get; set; }//截止时间
        public virtual double extendedTime { get; set; }//超期时间
        public virtual string _supplierName { get; set; } //供应商名称
        public virtual AscmReadingHead ascmReadingHead { get; set; }
        public virtual string ReadingHeadAddress { get { if (ascmReadingHead != null) return ascmReadingHead.address; return ""; } }


        public AscmContainer()
        {

        }
        public AscmContainer(AscmContainer ascmContainer, string supplierName, string deadline, double extendedTime)
        {
            this.sn = ascmContainer.sn;
            this.organizationId = ascmContainer.organizationId;
            this.createUser = ascmContainer.createUser;
            this.createTime = ascmContainer.createTime;
            this.modifyUser = ascmContainer.modifyUser;
            this.modifyTime = ascmContainer.modifyTime;
            this.rfid = ascmContainer.rfid;
            this.specId = ascmContainer.specId;
            this.supplierId = ascmContainer.supplierId;
            this.place = ascmContainer.place;
            this.status = ascmContainer.status;
            this.description = ascmContainer.description;
            this.storeInTime = ascmContainer.storeInTime;
            this.isCheck = ascmContainer.isCheck;
            this.containerSpec = ascmContainer.containerSpec;
            this.deadline = deadline;
            this.startSn = ascmContainer.startSn;
            this.endSn = ascmContainer.endSn;
            this.supplier = ascmContainer.supplier;
            this.extendedTime = extendedTime;
            this.SpecName = ascmContainer.SpecName;
            this.SpecCount = ascmContainer.SpecCount;
            this._supplierName = supplierName;
        }

    }
}
