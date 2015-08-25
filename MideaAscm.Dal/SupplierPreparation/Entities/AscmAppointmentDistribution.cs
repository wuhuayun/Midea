using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Dal.SupplierPreparation.Entities
{
    ///<summary>供应商预约时间分布</summary>
    public class AscmAppointmentDistribution
    {
        ///<summary>id</summary>
        public virtual int id { get; set; }
        ///<summary>日期</summary>
        public virtual string distributionDate { get; set; }
        ///<summary>时间序列</summary>
        public virtual int timeId { get; set; }
        ///<summary>开始时间</summary>
        public virtual string startTime { get; set; }
        ///<summary>结束时间</summary>
        public virtual string endTime { get; set; }
        ///<summary>数量</summary>
        public virtual decimal count { get; set; }

        //辅助属性
     }
}
