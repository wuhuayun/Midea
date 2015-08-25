using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Warehouse.Entities
{
    public class AscmWmsLogisticsMainLog
    {
        ///<summary>日志主表ID</summary>
        public virtual int id { get; set; }
        ///<summary>创建时间</summary>
        public virtual string createTime { get; set; }
        ///<summary>更新时间</summary>
        public virtual string modifyTime { get; set; }
        ///<summary>返回代码</summary>
        public virtual int returnCode { get; set; }
        ///<summary>返回信息</summary>
        public virtual string returnMessage { get; set; }

        public class ReturnCodeDefine
        {
            public const int error = -1;
            public const int correct = 0;
        }
    }
}
