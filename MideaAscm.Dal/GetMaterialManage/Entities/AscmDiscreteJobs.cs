using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.FromErp.Entities;
using YnFrame.Dal.Entities;

namespace MideaAscm.Dal.GetMaterialManage.Entities
{
    /// <summary>
    /// 排产单
    /// </summary>
    public class AscmDiscreteJobs
    {
        /// <summary>记录编号</summary>
        public virtual int id { get; set; }
        /// <summary>标识号</summary>
        public virtual int identificationId { get; set; }
        /// <summary>创建人</summary>
        public virtual string createUser { get; set; }
        /// <summary>创建时间</summary>
        public virtual string createTime { get; set; }
        /// <summary>最后更新人</summary>
        public virtual string modifyUser { get; set; }
        /// <summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }
        /// <summary>作业号</summary>
        public virtual string jobId { get; set; }
        /// <summary>作业日期</summary>
        public virtual string jobDate { get; set; }
        /// <summary>装配件</summary>
        public virtual string jobInfoId { get; set; }
        /// <summary>装配件描述</summary>
        public virtual string jobDesc { get; set; }
        /// <summary>数量</summary>
        public virtual int count { get; set; }
        ///<summary>角色名称 吴华允于2015年7月28日添加</summary>
        public virtual string personName { get; set; }
        /// <summary>生产线</summary>
        public virtual string productLine { get; set; }
        /// <summary>顺序</summary>
        public virtual string sequence { get; set; }
        /// <summary>备注</summary>
        public virtual string tip { get; set; }
        /// <summary>上线时间</summary>
        public virtual string onlineTime { get; set; }
        /// <summary>第几次上传</summary>
        public virtual int which { get; set; }
        /// <summary>上传时间</summary>
        public virtual string time { get; set; }
        /// <summary>所属排产员</summary>
        public virtual string workerId { get; set; }
        /// <summary>状态</summary>
        public virtual int status { get; set; }
        /// <summary>备注1</summary>
        public virtual string tip1 { get; set; }
        /// <summary>备注2</summary>
        public virtual string tip2 { get; set; }
        /// <summary>id</summary>
        public virtual int wipEntityId { get; set; }

        //辅助信息
        public virtual AscmWipEntities ascmWipEntities { get; set; }
        public virtual int? wipEntitiesId
        {
            get
            {
                if (ascmWipEntities != null)
                    return ascmWipEntities.wipEntityId;
                return null;
            }
        }
        public List<AscmWipRequirementOperations> listAscmWipRequirementOperations { get; set; }
        public List<AscmWipRequirementOperations> listAscmWipBom { get; set; }

        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _time { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(time) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(time)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual YnUser ynUser { get; set; }
        public virtual string rankerName { get { if (ynUser != null) return ynUser.userName; return ""; } }
        public virtual string _status { get { return StatusDefine.DisplayText(status); } }
        public virtual string inputPath { get; set; }
        public virtual string fileLoad { get; set; }
        public virtual string lineAndSequence { get { return LineAndSequenceAppend.DisplayText(productLine, sequence); } }
        public virtual string sIdentificationId { get { return IdentificationIdDefine.DisplayText(identificationId); } }
        public virtual string detail { get; set; }

        #region 状态定义
        public class StatusDefine
        {
            /// <summary>正常</summary>
            public const int defaultvalue = 1;
            /// <summary>异常</summary>
            public const int othervalue = 0;
            /// <summary>已生成</summary>
            public const int planvalue = 2;

            public static string DisplayText(int value)
            {
                if (value == defaultvalue)
                    return "正常";
                else if (value == othervalue)
                    return "异常";
                else if (value == planvalue)
                    return "已生成";
                return value.ToString();
            }

            public static List<int> GetList()
            {
                List<int> list = new List<int>();
                list.Add(defaultvalue);
                list.Add(othervalue);
                list.Add(planvalue);
                return list;
            }
        }
        #endregion

        #region 标示符定义
        public class IdentificationIdDefine
        {
            /// <summary>总装</summary>
            public const int zz = 1;
            /// <summary>电装</summary>
            public const int dz = 2;

            public static string DisplayText(int value)
            {
                if (value == zz)
                    return "总装";
                else if (value == dz)
                    return "电装";
                return value.ToString();
            }

            public static List<int> GetList()
            {
                List<int> list = new List<int>();
                list.Add(zz);
                list.Add(dz);
                return list;
            }
        }
        #endregion

        #region 生产线及顺序拼接
        public class LineAndSequenceAppend
        {
            /// <summary>
            /// 显示  生产线(顺序)
            /// </summary>
            /// <param name="l">生产线</param>
            /// <param name="s">顺序</param>
            /// <returns></returns>
            public static string DisplayText(string l, string s)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(l);
                sb.Append("(");
                sb.Append(s);
                sb.Append(")");

                return sb.ToString();
            }
        }
        #endregion
    }
}
