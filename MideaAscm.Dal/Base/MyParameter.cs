using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Base
{
    public class MyParameter
    {
        #region 公共
        /// <summary>大门LED名称</summary>
        public static readonly int doorLedTitle = 101;
        /// <summary>卸货点预约失效时间</summary>
        public static readonly int reserveInvalid = 102;
        /// <summary>供应商到厂放行时长（0.5≤ Tc ≤  24，Tc为0.5的整数倍）</summary>
        public static readonly int supplierPassDuration = 103;
        /// <summary>员工车辆免检级别</summary>
        public static readonly int employeeCarExemptionLevel = 104;
        #endregion
    }
}
