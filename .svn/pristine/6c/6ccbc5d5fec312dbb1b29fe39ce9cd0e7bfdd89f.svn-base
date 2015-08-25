using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal;
using MideaAscm.Dal.GetMaterialManage.Entities;

namespace MideaAscm.Services.GetMaterialManage
{
    public class AscmMonitoringService
    {
        #region  统计任务量的简单类
        public class Monitoring
        {
            public string userName { get; set; }
            public string workerId { get; set; }
            public int taskNumber { get; set; }
            public double avgTime { get; set; }
            public Monitoring(string userName, string workerId, int taskNumber, double avgTime)
            {
                this.userName = userName;
                this.workerId = workerId;
                this.taskNumber = taskNumber;
                this.avgTime = avgTime;
            }
        }

        #endregion

        private static AscmMonitoringService service;
        public static AscmMonitoringService GetInstance()
        {
            if (service == null)
                service = new AscmMonitoringService();
            return service;
        }

        public List<Monitoring> GetListForMonitoring(string strWay, string strStarTime, string strEndTime)
        {
            string strHql = "";
            if (string.IsNullOrEmpty(strWay))
            {
                strHql = "select '', workerId ,count(workerId), round(avg(round(to_number(to_date(endTime,'yyyy-mm-dd hh24:mi:ss')- to_date(starTime,'yyyy-mm-dd hh24:mi:ss'))*1440))) from AscmGetMaterialTask where status='FINISH'  Group by workerId";
            }
            else
            {
                if (strWay == "wk")
                {
                    strHql = "select '', workerId ,count(workerId), round(avg(round(to_number(to_date(endTime,'yyyy-mm-dd hh24:mi:ss')- to_date(starTime,'yyyy-mm-dd hh24:mi:ss'))*1440))) from AscmGetMaterialTask  where to_date(substr(endtime, 0, 10),'yyyy-mm-dd')   between  to_date('" + System.DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "' , 'yyyy-mm-dd') and to_date('" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','yyyy-mm-dd')  and  status='FINISH'   group by workerid";
                }
                else
                {
                    strHql = "select '', workerId ,count(workerId), round(avg(round(to_number(to_date(endTime,'yyyy-mm-dd hh24:mi:ss')- to_date(starTime,'yyyy-mm-dd hh24:mi:ss'))*1440))) from AscmGetMaterialTask  where to_date(substr(endtime, 0, 10),'yyyy-mm-dd')   between  to_date('" + System.DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd") + "' , 'yyyy-mm-dd') and to_date('" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','yyyy-mm-dd') and  status='FINISH'   group by workerid";
                }
            }
            if (!string.IsNullOrEmpty(strStarTime) && !string.IsNullOrEmpty(strEndTime))
            {
                strHql = "select '', workerId ,count(workerId), round(avg(round(to_number(to_date(endTime,'yyyy-mm-dd hh24:mi:ss')- to_date(starTime,'yyyy-mm-dd hh24:mi:ss'))*1440))) from AscmGetMaterialTask where  to_date(substr(endtime, 0, 10),'yyyy-mm-dd')   between  to_date('" + strStarTime + "' , 'yyyy-mm-dd') and to_date('" + strEndTime + "','yyyy-mm-dd')  and  status='FINISH'  Group by workerId";
            }
            IList<object[]> list = YnDaoHelper.GetInstance().nHibernateHelper.Find<object[]>(strHql);
            List<Monitoring> mlist = new List<Monitoring>();
            foreach (object[] obj in list)
            {
                obj[0] = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select userName from AscmUserInfo where userId='" + obj[1].ToString() + "'");
                mlist.Add(new Monitoring(obj[0].ToString(), obj[1].ToString(), Convert.ToInt32(obj[2].ToString()), Convert.ToDouble(obj[3].ToString())));

            }
            return mlist;

        }

        public List<AscmGetMaterialTask> TaskDetailStatisticsList(string strWorkId)
        {
           
            List<AscmGetMaterialTask> list = null;
            try
            {
                string sort = " order by id ";
                string strHql = "from AscmGetMaterialTask  where workerId='" + strWorkId + "'";
                IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(strHql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                
               }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliveryOrderDetail)", ex);
                throw ex;
            }
            return list;
        }
    }
}
