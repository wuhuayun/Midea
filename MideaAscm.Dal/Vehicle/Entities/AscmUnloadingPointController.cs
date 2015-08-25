using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Vehicle.Entities
{
    public class AscmUnloadingPointController
    {
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
        ///<summary>控制器名称</summary>
        public virtual string name { get; set; }
        ///<summary>IP地址</summary>
        public virtual string ip { get; set; }
        ///<summary>端口</summary>
        public virtual int port { get; set; }
    }
}
