using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.Vehicle.Entities
{
    public class AscmEmpCarSwipeLog
    {
        ///<summary>id</summary>
        public virtual int id { get; set; }
        ///<summary>创建时间</summary>
        public virtual string createTime { get; set; }
        ///<summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }
        ///<summary>大门</summary>
        public virtual int doorId { get; set; }
        ///<summary>读头</summary>
        public virtual int readingHeadId { get; set; }
        ///<summary>rfid</summary>
        public virtual string rfid { get; set; }
        ///<summary>姓名</summary>
        public virtual string employeeName { get; set; }
        ///<summary>车牌号</summary>
        public virtual string plateNumber { get; set; }
        ///<summary>是否处理</summary>
        public virtual int status { get; set; }
        ///<summary>放行</summary>
        public virtual bool pass { get; set; }
        ///<summary>描述</summary>
        public virtual string description { get; set; }
        ///<summary>读头</summary>
        public virtual string readingHead { get; set; }
        ///<summary>方向</summary>
        public virtual string direction { get; set; }

        public AscmEmpCarSwipeLog GetOwner()
        {
            return (AscmEmpCarSwipeLog)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string createTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public AscmDoor ascmDoor { get; set; }
        public string doorName { get { if (ascmDoor != null) return ascmDoor.name; return ""; } }
        //方向类型
        public class DirectionDefine
        {
            /// <summary>进</summary>
            public const string enter = "小车入口";
            /// <summary>出</summary>
            public const string out1 = "小车出口";

            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(enter);
                list.Add(out1);
                return list;
            }
        }
    }
}
