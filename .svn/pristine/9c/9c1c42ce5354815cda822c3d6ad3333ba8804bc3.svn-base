using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Base.Entities
{
    public class AscmEmployee
    {
        ///<summary>员工id</summary>
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
        ///<summary>部门ID</summary>
        public virtual int departmentId { get; set; }
        ///<summary>姓名</summary>
        public virtual string name { get; set; }
        ///<summary>性别</summary>
        public virtual string sex { get; set; }
        ///<summary>身份证号</summary>
        public virtual string idNumber { get; set; }
        ///<summary>邮箱</summary>
        public virtual string email { get; set; }
        ///<summary>办公室电话</summary>
        public virtual string officeTel { get; set; }
        ///<summary>移动电话</summary>
        public virtual string mobileTel { get; set; }
        ///<summary>开始时间</summary>
        public virtual string startTime { get; set; }
        ///<summary>生日</summary>
        public virtual string birth { get; set; }
        ///<summary>描述</summary>
        public virtual string memo { get; set; }
        ///<summary>状态</summary>
        public virtual string status { get; set; }


        public AscmEmployee GetOwner()
        {
            return (AscmEmployee)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _startTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(startTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(startTime)).ToString("yyyy-MM-dd"); return ""; } }
        public virtual string _birth { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(birth) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(birth)).ToString("yyyy-MM-dd"); return ""; } }
        ///<summary>部门</summary>
        public virtual YnFrame.Dal.Entities.YnDepartment ynDepartment { get; set; }
        public virtual string departmentName { get { if (ynDepartment != null) return ynDepartment.name; return ""; } }
        public bool select { get; set; }

    }
}
