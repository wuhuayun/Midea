﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.GetMaterialManage.Entities
{
    public class AscmRequirementList
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
        /// <summary>任务号</summary>
        public virtual int taskId { get; set; }
        /// <summary>物料编码</summary>
        public virtual string materialCode { get; set; }
        /// <summary>物料描述</summary>
        public virtual string materialDesc { get; set; }
        /// <summary>单位</summary>
        public virtual string unit { get; set; }
        /// <summary>需求数</summary>
        public virtual int rcount { get; set; }
        /// <summary>已发数</summary>
        public virtual int scount { get; set; }
        /// <summary>现有数</summary>
        public virtual int ncount { get; set; }
        /// <summary>备注</summary>
        public virtual string tip { get; set; }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual AscmGetMaterialTask ascmGetMaterialTask { get; set; }

    }
}
