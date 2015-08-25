using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using YnFrame.Dal.Entities;

namespace MideaAscm.Dal.GetMaterialManage.Entities
{
    public class AscmForklift
    {
        /// <summary>记录编号</summary>
        public virtual int id { get; set; }
        /// <summary>创建人</summary>
        public virtual string createUser { get; set; }
        /// <summary>创建时间</summary>
        public virtual string createTime { get; set; }
        /// <summary>最后更新人</summary>
        public virtual string modifyUser { get; set; }
        /// <summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }
        /// <summary>资产号</summary>
        public virtual string assetsId { get; set; }
        /// <summary>标签号</summary>
        public virtual string tagId { get; set; }
        /// <summary>车辆号码</summary>
        public virtual string forkliftNumber { get; set; }
        /// <summary>车种</summary>
        public virtual string forkliftType { get; set; }
        /// <summary>车辆规格</summary>
        public virtual string forkliftSpec { get; set; }
        /// <summary>作业内容</summary>
        public virtual string workContent { get; set; }
        /// <summary>领料路线</summary>
        public virtual string forkliftWay { get; set; }
        /// <summary>允许活动范围</summary>
        public virtual string actionLimits { get; set; }
        /// <summary>归属主体</summary>
        public virtual string logisticsClass { get; set; }
        /// <summary>责任人</summary>
        public virtual string workerId { get; set; }
        /// <summary>状态</summary>
        public virtual string status { get; set; }
        /// <summary>备注</summary>
        public virtual string tip { get; set; }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public AscmRfid ascmRfid { get; set; }
        public string tag
        {
            get
            {
                return tagId;
            }
        }
        public virtual AscmReadingHead ascmReadingHead { get; set; }
        public virtual string _forkliftSpec { get { return ForkliftSpecDefine.DisplayText(forkliftSpec); } }
        public virtual string _forkliftType { get { return ForkliftTypeDefine.DisplayText(forkliftType); } }
        public virtual string logisticsClassName { get;set; }
        public virtual string _status { get { return StatusDefine.DisplayText(status); } }

        public virtual AscmUserInfo ascmUserInfo { get; set; }
        public virtual string workerName { get { if (ascmUserInfo != null) return ascmUserInfo.userName; return ""; } }


        #region 车种定义
        public class ForkliftTypeDefine
        {
            /// <summary>力之尤电瓶叉车</summary>
            public const string lzydpc = "力之尤电瓶叉车";
            /// <summary>丰田电瓶叉车</summary>
            public const string ftdpc = "丰田电瓶叉车";
            /// <summary>林德电瓶叉车</summary>
            public const string lddpc = "林德电瓶叉车";
            /// <summary>丰田柴油叉车</summary>
            public const string ftcyc = "丰田柴油叉车";
            /// <summary>丰田小牵引</summary>
            public const string tfqyc = "丰田小牵引";
            /// <summary>林德大牵引车</summary>
            public const string ldqyc = "林德大牵引车";

            public static string DisplayText(string value)
            {
                if (value == lzydpc)
                    return "力之尤电瓶叉车";
                else if (value == ftdpc)
                    return "丰田电瓶叉车";
                else if (value == lddpc)
                    return "林德电瓶叉车";
                else if (value == ftcyc)
                    return "丰田柴油叉车";
                else if (value == tfqyc)
                    return "丰田小牵引";
                else if (value == ldqyc)
                    return "林德大牵引车";
                return value;
            }

            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(lzydpc);
                list.Add(ftdpc);
                list.Add(lddpc);
                list.Add(ftcyc);
                list.Add(tfqyc);
                list.Add(ldqyc);
                return list;
            }
        }
        #endregion

        #region 车辆规格定义
        public class ForkliftSpecDefine
        {
            public const string spec1 = "1吨";
            public const string spec2 = "1.5吨";
            public const string spec3 = "2吨";
            public const string spec4 = "2.5吨";
            public const string spec5 = "3吨";

            public static string DisplayText(string value)
            {
                if (value == spec1)
                    return "1吨";
                else if (value == spec2)
                    return "1.5吨";
                else if (value == spec3)
                    return "2吨";
                else if (value == spec4)
                    return "2.5吨";
                else if (value == spec5)
                    return "3吨";
                return value;
            }

            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(spec1);
                list.Add(spec2);
                list.Add(spec3);
                list.Add(spec4);
                list.Add(spec5);
                return list;
            }
        }
        #endregion

        #region 状态定义
        public class StatusDefine
        {
            /// <summary>正常</summary>
            public const string normal = "NORMAL";
            /// <summary>维修</summary>
            public const string service = "SERVICE";
            /// <summary>报废</summary>
            public const string scrap = "SCRAP";

            public static string DisplayText(string value)
            {
                if (value == normal)
                    return "正常";
                else if (value == service)
                    return "维修";
                else if (value == scrap)
                    return "报废";
                return value;
            }

            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(normal);
                list.Add(service);
                list.Add(scrap);
                return list;
            }
        }
        #endregion
    }
}
