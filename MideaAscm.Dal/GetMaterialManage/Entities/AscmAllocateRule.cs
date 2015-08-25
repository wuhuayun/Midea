using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using YnFrame.Dal.Entities;

 
namespace MideaAscm.Dal.GetMaterialManage.Entities
{
    public class AscmAllocateRule
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
        /// <summary>领料员</summary>
        public virtual string workerName { get; set; }
        /// <summary>总装排产员</summary>
        public virtual string zRankerName { get; set; }
        /// <summary>电装排产员</summary>
        public virtual string dRankerName { get; set; }
        /// <summary>
        /// [须配料(子库|子库|...)(子库:物料编码%物料编码%...|子库:物料编码%物料编码%...)]&[须备料(子库|子库|...)(子库:物料编码%物料编码%...|子库:物料编码%物料编码%...)]&[(特殊子库|特殊子库|...)(子库:物料编码%物料编码%...|子库:物料编码%物料编码%...)]
        /// </summary>
        public virtual string ruleCode { get; set; }
        /// <summary>手动任务</summary>
        public virtual string other { get; set; }
        /// <summary>备注1</summary>
        public virtual string tip1 { get; set; }
        /// <summary>备注2</summary>
        public virtual string tip2 { get; set; }
        /// <summary>任务数</summary>
        public virtual int taskCount { get; set; }
        /// <summary>车间信息</summary>
        public virtual int logisticsClassId { get; set; }
        
        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _other { get { return OtherDefine.DisplayText(other); } }
        public virtual YnUser ynUserWorker { get; set; }
        public virtual string worker { get { if (ynUserWorker != null) return ynUserWorker.userName; return ""; } }
        public virtual YnUser ynUserZRanker { get; set; }
        public virtual string zRanker { get { if (ynUserZRanker != null) return ynUserZRanker.userName; return ""; } }
        public virtual YnUser ynUserDRanker { get; set; }
        public virtual string dRanker { get { if (ynUserDRanker != null) return ynUserDRanker.userName; return ""; } }
        public virtual AscmLogisticsClassInfo ascmLogisticsClassInfo { get; set; }
        public virtual string logisticsClass { get { if (ascmLogisticsClassInfo != null) return ascmLogisticsClassInfo.logisticsClass; return ""; } }
        public virtual string logisticsClassName { get; set; }
        public virtual int count { get { return taskCount; } }

        //领料员
        public virtual AscmUserInfo ascmUserInfoWorker { get; set; }
        public string userWorker { get { if (ascmUserInfoWorker != null) return ascmUserInfoWorker.userName; return workerName; } }
        
        //总装排产员
        public virtual AscmUserInfo ascmUserInfoZRanker { get; set; }
        public virtual string userZRanker { get { if (ascmUserInfoZRanker != null) return ascmUserInfoZRanker.userName; return zRankerName; } }

        //电装排产员
        public virtual AscmUserInfo ascmUserInfoDRanker { get; set; }
        public virtual string userDRanker { get { if (ascmUserInfoDRanker != null) return ascmUserInfoDRanker.userName; return dRankerName; } }

        #region 手动任务定义
        public class OtherDefine
        {
            /// <summary>附件</summary>
            public const string fujian = "FUJIAN";
            /// <summary>散件</summary>
            public const string sanjian = "SANJIAN";
            /// <summary>两器</summary>
            public const string liangqi = "LIANGQI";
            /// <summary>铜管</summary>
            public const string tongguan = "TONGGUAN";
            /// <summary>铝箔</summary>
            public const string lvbo = "LVBO";
            /// <summary>配管</summary>
            public const string peiguan = "PEIGUAN";
            /// <summary>其他</summary>
            public const string qita = "QITA";

            public static string DisplayText(string value)
            {
                if (value == fujian)
                    return "附件";
                else if (value == sanjian)
                    return "散件";
                else if (value == liangqi)
                    return "两器";
                else if (value == tongguan)
                    return "铜管";
                else if (value == lvbo)
                    return "铝箔";
                else if (value == peiguan)
                    return "配管";
                else if (value == qita)
                    return "其他";
                return value;
            }

            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(fujian);
                list.Add(sanjian);
                list.Add(liangqi);
                list.Add(tongguan);
                list.Add(lvbo);
                list.Add(peiguan);
                list.Add(qita);
                return list;
            }
        }
        #endregion
    }
}
