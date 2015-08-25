using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Base.Entities
{
    public class AscmEmployeeCar
    {
        ///<summary>小车id</summary>
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
        ///<summary>车牌号</summary>
        public virtual string plateNumber { get; set; }
        ///<summary>型号</summary>
        public virtual string spec { get; set; }
        ///<summary>颜色</summary>
        public virtual string color { get; set; }
        ///<summary>座位数量</summary>
        public virtual int seatCount { get; set; }
        ///<summary>rfid</summary>
        public virtual string rfid { get; set; }
        ///<summary>描述</summary>
        public virtual string memo { get; set; }
        ///<summary>状态</summary>
        public virtual string status { get; set; }
        ///<summary>免检</summary>
        public virtual bool exemption { get; set; }


        ///<summary>员工id</summary>
        public virtual int employeeId { get; set; }
        ///<summary>编号</summary>
        public virtual string employeeDocNumber { get; set; }
        ///<summary>姓名</summary>
        public virtual string employeeName { get; set; }
        ///<summary>性别</summary>
        public virtual string employeeSex { get; set; }
        ///<summary>身份证号</summary>
        public virtual string employeeIdNumber { get; set; }
        ///<summary>办公室电话</summary>
        public virtual string employeeOfficeTel { get; set; }
        ///<summary>移动电话</summary>
        public virtual string employeeMobileTel { get; set; }
        ///<summary>员工级别</summary>
        public virtual string employeeLevel{ get; set; }


        public AscmEmployeeCar GetOwner()
        {
            return (AscmEmployeeCar)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string createTime_ { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string modifyTime_ { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        /////<summary>员工</summary>
        //public virtual AscmEmployee ascmEmployee { get; set; }
        //public virtual string _employeeDocNumber { get { if (ascmEmployee != null) return ascmEmployee.docNumber; return ""; } }
        //public virtual string _employeeName { get { if (ascmEmployee != null) return ascmEmployee.name; return ""; } }

        public virtual string rfid1 
        {
            get 
            {
                if (rfid != null && rfid.Trim().Length == 10)
                {
                    foreach (string s1 in MideaAscm.Dal.Base.Entities.AscmRfid.Pre24G.GetList())
                    {
                        if (rfid.Trim().StartsWith(s1))
                        {
                            return s1;
                        }

                    }
                }
                return ""; 
            } 
        }
        public virtual string rfid2
        {
            get
            {
                if (rfid != null && rfid.Trim().Length == 10)
                {
                    foreach (string s1 in MideaAscm.Dal.Base.Entities.AscmRfid.Pre24G.GetList())
                    {
                        if (rfid.Trim().StartsWith(s1))
                        {
                            return rfid.Trim().Substring(s1.Length);
                        }

                    }
                }
                return "";
            }
        }
        public class EmployeeLevelDefine
        {
            public const string M1 = "M1";
            public const string M2 = "M2";
            public const string M3 = "M3";
            public const string M4 = "M4";
            public const string M5 = "M5";
            public const string M6 = "M6";
            public const string M7 = "M7";
            public const string P1 = "P1";
            public const string P2 = "P2";
            public const string P3 = "P3";
            public const string P4 = "P4";
            public const string P5 = "P5";
            public const string O1 = "O1";
            public const string O2 = "O2";
            public const string O3 = "O3";
            public const string O4 = "O4";
            public const string O5 = "O5";

            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(M1);
                list.Add(M2);
                list.Add(M3);
                list.Add(M4);
                list.Add(M5);
                list.Add(M6);
                list.Add(M7);
                list.Add(P1);
                list.Add(P2);
                list.Add(P3);
                list.Add(P4);
                list.Add(P5);
                list.Add(O1);
                list.Add(O2);
                list.Add(O3);
                list.Add(O4);
                list.Add(O5);
                return list;
            }
        }
    }
}
