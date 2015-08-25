using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Base.Entities
{
    public class AscmForkliftContainerLog
    {
        /// <summary>id</summary>
        public virtual long id { get; set; }
        ///<summary>叉车id</summary>
        public virtual int forkliftId { get; set; }
        /// <summary>叉车标签号</summary>
        public virtual string forkliftIdRfidId { get; set; }
        /// <summary>容器标签号</summary>
        public virtual string containerRfidId { get; set; }
        ///<summary>创建时间</summary>
        public virtual string createTime { get; set; }
        ///<summary>日期</summary>
        public virtual string passDate { get; set; }
        /// <summary>次数</summary>
        public virtual int times { get; set; }
        ///<summary>读头id</summary>
        public virtual int readingHeadId { get; set; }
        ///<summary>读头ip</summary>
        public virtual string readingHeadIp { get; set; }
        ///<summary>状态</summary>
        public virtual string status { get; set; }

        public AscmForkliftContainerLog GetOwner()
        {
            return (AscmForkliftContainerLog)this.MemberwiseClone();
        }
    }
}