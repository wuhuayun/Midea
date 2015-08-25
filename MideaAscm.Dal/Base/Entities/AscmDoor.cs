using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Base.Entities
{
    public class AscmDoor
    {
        ///<summary>大门id</summary>
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
        ///<summary>名称</summary>
        public virtual string name { get; set; }
        ///<summary>方向</summary>
        public virtual string direction { get; set; }
        ///<summary>车辆类型</summary>
        public virtual string vehicleType { get; set; }
        ///<summary>使用</summary>
        public virtual bool enabled { get; set; }
        ///<summary>描述</summary>
        public virtual string description { get; set; }

        public AscmDoor GetOwner()
        {
            return (AscmDoor)this.MemberwiseClone();
        }

        //辅助信息
        public virtual string _createTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(createTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(createTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _modifyTime { get { if (YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime) != null) return ((DateTime)YnBaseClass2.Helper.DateHelper.GetDateTime(modifyTime)).ToString("yyyy-MM-dd HH:mm"); return ""; } }
        public virtual string _directionCn { get { return DirectionDefine.DisplayText(direction); } }
        public virtual string _vehicleTypeCn { get { return VehicleTypeDefine.DisplayText(vehicleType); } }

        //方向类型
        public class DirectionDefine
        {
            /// <summary>进</summary>
            public const string enterWay = "enterWay";
            /// <summary>出</summary>
            public const string outWay = "outWay";
            /// <summary>双向</summary>
            public const string twoWay = "twoWay";

            public static string DisplayText(string value)
            {
                if (value == enterWay)
                    return "进";
                else if (value == outWay)
                    return "出";
                else if (value == twoWay)
                    return "双向";

                return value;
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(enterWay);
                list.Add(outWay);
                list.Add(twoWay);
                return list;
            }
        }
        //车辆类型
        public class VehicleTypeDefine
        {
            /// <summary>货车</summary>
            public const string truck = "truck";
            /// <summary>小车</summary>
            public const string car = "car";
            /// <summary>全部</summary>
            public const string all = "all";

            public static string DisplayText(string value)
            {
                if (value == truck)
                    return "货车";
                else if (value == car)
                    return "小车";
                else if (value == all)
                    return "全部";

                return value;
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(truck);
                list.Add(car);
                list.Add(all);
                return list;
            }
        }
        //
        public int carEnterCount = 0;
        public int carOutCount = 0;
        public int truckEnterCount = 0;
        public int truckOutCount = 0;
    }
}
