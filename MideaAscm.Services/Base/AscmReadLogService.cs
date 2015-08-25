using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Vehicle.Entities;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;

namespace MideaAscm.Services.Base
{
    /// <summary>
    /// RFID读头读取日志
    /// </summary>
    public class AscmReadLogService
    {
        private static AscmReadLogService ascmReadLogService;
        public static AscmReadLogService GetInstance()
        {
            //return ascmDoorServices ?? new AscmDoorService();
            if (ascmReadLogService == null)
                ascmReadLogService = new AscmReadLogService();
            return ascmReadLogService;
        }
        /// <summary>
        /// 根据读头ID获取相关信息 
        /// </summary>
        /// <param name="ReadHeadIds"></param>
        /// <returns></returns>
        public List<object> GetAscmReadingHeadLog(string ReadHeadIds)
        {
            List<object> list = null;
            string endTime = "";
            string readTime = System.Web.Configuration.WebConfigurationManager.AppSettings["ReadTime"];
            if (string.IsNullOrEmpty(readTime))
            {
                endTime = DateTime.Now.AddSeconds(-140).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                int seconds = Convert.ToInt32(readTime);
                endTime = DateTime.Now.AddSeconds(-seconds).ToString("yyyy-MM-dd HH:mm:ss");
            }
            string startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string hql = "select distinct log.rfid from AscmReadingHeadLog log where log.readingHeadId in (" + ReadHeadIds + ") and log.processed=0";
            IList<object> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<object>(hql);
            if (ilist.Count > 0)
            {
                list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<object>(ilist);
            }
            return list;

        }

        /// <summary>
        /// 根据读头ID获取相关信息 
        /// </summary>
        /// <param name="ReadHeadIds"></param>
        /// <returns></returns>
        public List<object> GetAscmReadingHeadLog(string ReadHeadIds, string str_seconds)
        {

            List<object> list = null;
            string endTime = "";
            try
            {
                int seconds = Convert.ToInt32(str_seconds);
                endTime = DateTime.Now.AddSeconds(-seconds).ToString("yyyy-MM-dd HH:mm:ss");
                string startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string hql = "select distinct log.rfid from AscmReadingHeadLog log where log.readingHeadId in (" + ReadHeadIds + ") and log.rfid  in  (select rfid  from AscmContainer where supplierId=" + str_seconds + ") and  log.processed=0";
                IList<object> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<object>(hql);
                if (ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<object>(ilist);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 根据读头ID获取相关信息 
        /// </summary>
        /// <param name="ReadHeadIds"></param>
        /// <returns></returns>
        public void DeleteAscmReadingHeadLog(string ReadHeadIds, string str_seconds)
        {

            try
            {
                string hql = "from AscmReadingHeadLog where readingHeadId=" + ReadHeadIds + " and  processed=false and (rfid in (" + str_seconds + ") )";
                List<AscmReadingHeadLog> list = null;
                IList<AscmReadingHeadLog> ilist=  YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmReadingHeadLog>(hql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmReadingHeadLog>(ilist);
                    foreach (AscmReadingHeadLog log in list)
                    {
                        log.processed = true;
                    }
                }
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(list);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmReadingHeadLog)", ex);
                throw ex;
            }

        }





    }
}
