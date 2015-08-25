using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.Vehicle.Entities
{
    public class AscmTruckSwipeLog
    {
        ///<summary>id</summary>
        public virtual int id { get; set; }
        ///<summary>大门</summary>
        public virtual int doorId { get; set; }
        ///<summary>创建时间</summary>
        public virtual string createTime { get; set; }
        ///<summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }
        ///<summary>读头</summary>
        public virtual int readingHeadId { get; set; }
        ///<summary>司机编号</summary>
        public virtual string rfid { get; set; }
        ///<summary>供应商Id</summary>
        public virtual int supplierId { get; set; }
        ///<summary>司机Id</summary>
        public virtual int driverId { get; set; }
        ///<summary>供应商名称</summary>
        public virtual string supplierName { get; set; }
        ///<summary>司机姓名</summary>
        public virtual string driverName { get; set; }
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
        ///<summary>合单ID</summary>
        public virtual int batSumMainId { get; set; }
        ///<summary>合单号</summary>
        public virtual string batSumDocNumber { get; set; }
        ///<summary>是否准时</summary>
        public virtual bool onTime { get; set; }
        ///<summary>预约开始时间</summary>
        public virtual string appointmentStartTime { get; set; }
        ///<summary>预约最后时间</summary>
        public virtual string appointmentEndTime { get; set; }

        public AscmTruckSwipeLog GetOwner()
        {
            return (AscmTruckSwipeLog)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string createTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string appointmentStartTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(appointmentStartTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(appointmentStartTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string appointmentEndTimeShow { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(appointmentEndTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(appointmentEndTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string directionCn { get { return DirectionDefine.DisplayText(direction); } }
        public AscmDoor ascmDoor { get; set; }
        public string doorName { get { if (ascmDoor != null) return ascmDoor.name; return ""; } }
        //方向类型
        public class DirectionDefine
        {
            /// <summary>进</summary>
            public const string enter = "enter";
            /// <summary>出</summary>
            public const string out1 = "out";

            public static string DisplayText(string value)
            {
                if (value == enter)
                    return "进";
                else if (value == out1)
                    return "出";

                return value;
            }
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
