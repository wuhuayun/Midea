using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.ContainerManage.Entities
{
    public class AscmStoreInOut
    {
        /// <summary>记录编号</summary>
        public virtual int id { get; set; }
        /// <summary>创建人</summary>
        public virtual string createUser { get; set; }
        /// <summary>创建时间</summary>
        public virtual string createTime { get; set; }
        /// <summary>容器ID</summary>
        public virtual string containerId { get; set; }
        /// <summary>供应商ID</summary>
        public virtual int supplierId { get; set; }
        /// <summary>方向</summary>
        public virtual string direction { get; set; }
        /// <summary>EPCID</summary>
        public virtual string epcId { get; set; }
        /// <summary>出入库时间</summary>
        public virtual string readTime { get; set; }
        /// <summary>干系人</summary>
        public virtual string stakeHolders { get; set; }
        /// <summary>状态</summary>
        public virtual string status { get; set; }
        /// <summary>备注</summary>
        public virtual string tip { get; set; }
        /// <summary> 单号</summary>
        public virtual string docNumber { get; set; }

        //辅助信息
        public virtual string createTimeCN { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public AscmContainer ascmContainer { get; set; }
        public AscmSupplier ascmSupplier { get; set; }
        public virtual string queryStartTime { get; set; }
        public virtual string queryEndTime { get; set; }
        public virtual string supplierNameCN { get { return (ascmSupplier==null)?string.Empty:ascmSupplier.name; } }
        public virtual string specCN { get { return (ascmContainer == null) ? string.Empty : ascmContainer.spec; } }
        public virtual string directionCN { get { return DirectionDefine.DisplayText(direction); } }
        public virtual string readTimeCN { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(readTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(readTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string statusCN { get { return StatusDefine.DisplayText(status); } }
        public virtual string supplierName { get; set; }
        public virtual string _direction { get { return DirectionDefine.DisplayText(direction); } }
        public virtual string _readTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(readTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(readTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _status { get { return StatusDefine.DisplayText(status); } }
        public virtual double extendedTime { get; set; }//超期时间
        public virtual int warnTime { get; set; }//预警时间

        //2013年07月17添加  覃小华
        public virtual string supplierDoc { get; set; }  //供应商名称
        public virtual int specCount { get; set; }  //相同规格数量
        public virtual string specName { get; set; } //规格名称


        #region 方向定义
        public class DirectionDefine
        {
            /// <summary>入库</summary>
            public const string storeIn = "STOREIN";
            /// <summary>出库</summary>
            public const string storeOut = "STOREOUT";

            public static string DisplayText(string value)
            {
                if (value == storeIn)
                    return "入库";
                else if (value == storeOut)
                    return "出库";
                return value;
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(storeIn);
                list.Add(storeOut);
                return list;
            }
        }
        #endregion

        #region 状态定义
        public class StatusDefine
        {
            /// <summary>入库中</summary>
            public const string inStoring = "INSTORING";
            /// <summary>已入库</summary>
            public const string inStored = "INSTORED";
            /// <summary>出库中</summary>
            public const string outStoring = "OUTSTORING";
            /// <summary>已出库</summary>
            public const string outStored = "OUTSTORED";

            public static string DisplayText(string value)
            {
                if (value == inStoring)
                    return "入库中";
                else if (value == inStored)
                    return "已入库";
                else if (value == outStoring)
                    return "出库中";
                else if (value == outStored)
                    return "已出库";
                return value;
            }

            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(inStoring);
                list.Add(inStored);
                list.Add(outStoring);
                list.Add(outStored);
                return list;
            }
        }
        #endregion

        public AscmStoreInOut()
        { 
        }

        public  AscmStoreInOut(AscmStoreInOut ascmStore, int warnTime, string supplierName, double extendedTime )
        {
            this.id = ascmStore.id;
            this.createUser = ascmStore.createUser;
            this.createTime = ascmStore.containerId;
            this.containerId = ascmStore.containerId;
            this.supplierId = ascmStore.supplierId;
            this.direction = ascmStore.direction;
            this.epcId = ascmStore.epcId;
            this.readTime = ascmStore.readTime;
            this.stakeHolders = ascmStore.stakeHolders;
            this.status = ascmStore.status;
            this.tip = ascmStore.tip;
            this.supplierName = supplierName;
            this.extendedTime = extendedTime;
           // this.ascmContainer = ascmStore.ascmContainer;
           // this.ascmSupplier = ascmStore.ascmSupplier;
            this.warnTime = warnTime;
        }
    }
}
