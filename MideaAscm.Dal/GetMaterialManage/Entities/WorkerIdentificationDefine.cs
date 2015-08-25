using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.GetMaterialManage.Entities
{
    /// <summary>
    /// 物流模块工作人员角色定义
    /// </summary>
    public class WorkerIdentificationDefine
    {
        /// <summary>领料员</summary>
        public const int getMaterialWorker = 1;
        /// <summary>排产员</summary>
        public const int rankProductWorker = 2;

        public static string DisplayText(int value)
        {
            if (value == rankProductWorker)
                return "排产员";
            else if (value == getMaterialWorker)
                return "领料员";
            return value.ToString();
        }
        public static List<int> GetList()
        {
            List<int> list = new List<int>();
            list.Add(getMaterialWorker);
            list.Add(rankProductWorker);
            return list;
        }
    }
}
