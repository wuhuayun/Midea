using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.GetMaterialManage.Entities;
using YnFrame.Dal.Entities;
namespace MideaAscm.Dal.Base.Entities
{
    public class AscmUserInfo : YnFrame.Dal.Entities.YnUser
    {
        //Member
        public virtual string logisticsClass { get; set; }

        public AscmUserInfo()
        { 
        
        }
        public AscmUserInfo(AscmUserInfo ascmUserInfo)
        {
            this.departmentId = ascmUserInfo.departmentId;
            //this.departmentPositionList = ascmUserInfo.departmentPositionList;
            this.description = ascmUserInfo.description;
            this.email = ascmUserInfo.email;
            this.employeeId = ascmUserInfo.employeeId;
            this.extExpandId = ascmUserInfo.extExpandId;
            this.extExpandType = ascmUserInfo.extExpandType;
            this.firstSpell = ascmUserInfo.firstSpell;
            this.id = ascmUserInfo.id;
            this.isAccountLocked = ascmUserInfo.isAccountLocked;
            this.lastLoginDate = ascmUserInfo.lastLoginDate;
            this.lastLoginIp = ascmUserInfo.lastLoginIp;
            this.mobilePhone = ascmUserInfo.mobilePhone;
            this.mobileTel = ascmUserInfo.mobileTel;
            this.officePhone = ascmUserInfo.officePhone;
            this.officeTel = ascmUserInfo.officeTel;
            this.organizationId = ascmUserInfo.organizationId;
            this.password = ascmUserInfo.password;
            this.roleId = ascmUserInfo.roleId;
            //this.roleList = ascmUserInfo.roleList;
            this.select = ascmUserInfo.select;
            this.sex = ascmUserInfo.sex;
            this.sign = ascmUserInfo.sign;
            this.siteAdmin = ascmUserInfo.siteAdmin;
            this.sortNo = ascmUserInfo.sortNo;
            this.spell = ascmUserInfo.spell;
            //this.supplierId = ascmUserInfo.supplierId;
            //this.supplierName = ascmUserInfo.supplierName;
            this.userId = ascmUserInfo.userId;
            this.userName = ascmUserInfo.userName;
            this.userName1 = ascmUserInfo.userName1;
            //this.userName2 = ascmUserInfo.userName2;
            //this.ynUnit = ascmUserInfo.ynUnit;
            this.logisticsClass = ascmUserInfo.logisticsClass;
        }

        //辅助信息
        public virtual AscmEmployee ascmEmployee { get; set; }
        public virtual string employeeName { get { if (ascmEmployee != null && ascmEmployee.name != null) return ascmEmployee.name.Trim(); return ""; } }

        public virtual AscmSupplier ascmSupplier { get; set; }
        public virtual int? supplierId { get { if (ascmSupplier != null) return ascmSupplier.id; return null; } }
        public virtual string supplierName { get { if (ascmSupplier != null && ascmSupplier.name != null) return ascmSupplier.name.Trim(); return ""; } }
        public virtual AscmLogisticsClassInfo ascmLogisticsClassInfo { get; set; }
        public virtual string logisticsClassName { get { if (ascmLogisticsClassInfo != null) return ascmLogisticsClassInfo.logisticsName; return ""; } }
        public bool isLeader { get; set; }
        
       
    }
}
