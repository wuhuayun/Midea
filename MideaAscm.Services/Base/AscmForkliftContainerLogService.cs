using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Services.Warehouse;

namespace MideaAscm.Services.Base
{
    public class AscmForkliftContainerLogService
    {
        private static AscmForkliftContainerLogService ascmForkliftContainerLogServices;
        public static AscmForkliftContainerLogService GetInstance()
        {
            //return ascmForkliftContainerLogServices ?? new AscmForkliftContainerLogService();
            if (ascmForkliftContainerLogServices == null)
                ascmForkliftContainerLogServices = new AscmForkliftContainerLogService();
            return ascmForkliftContainerLogServices;
        }
        public AscmForkliftContainerLog Get(int id)
        {
            AscmForkliftContainerLog ascmForkliftContainerLog = null;
            try
            {
                ascmForkliftContainerLog = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmForkliftContainerLog>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmForkliftContainerLog)", ex);
                throw ex;
            }
            return ascmForkliftContainerLog;
        }
        public List<AscmForkliftContainerLog> GetList(string sql)
        {
            List<AscmForkliftContainerLog> list = null;
            try
            {
                IList<AscmForkliftContainerLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmForkliftContainerLog>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmForkliftContainerLog>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmForkliftContainerLog)", ex);
                throw ex;
            }
            return list;
        }
        public void AddForkliftContainerLog(string forkliftRfid, AscmReadingHead ascmReadingHead, AscmForklift ascmForklift, List<string> listRfid, string sessionKey)
        {
            try
            {
                if (listRfid.Count > 0)
                {

                    DateTime dtServer = DateTime.Now;
                    int times = 0;
                    //该叉车最后一次读取
                    YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                    ynPage.SetPageSize(1);
                    string sql = "from AscmForkliftContainerLog where passDate='" + dtServer.ToString("yyyy-MM-dd") + "' and forkliftId=" + ascmForklift.id + "";
                    IList<AscmForkliftContainerLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmForkliftContainerLog>(sql + " order by times desc", sql, ynPage, sessionKey);
                    if (ilist != null && ilist.Count > 0)
                    {
                        AscmForkliftContainerLog ascmForkliftContainerLog = ilist[0];
                        DateTime dtCreateTime = Convert.ToDateTime(ascmForkliftContainerLog.createTime);
                        TimeSpan ts = dtServer.Subtract(dtCreateTime);

                        if (ascmForkliftContainerLog.readingHeadId == ascmReadingHead.id)
                        {
                            if (ts.TotalMinutes < 10)//10分钟同一叉车不重复计入
                                return;
                            times = ascmForkliftContainerLog.times;
                        }
                    }
                    //开始加入
                    times++;
                    string maxIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("AscmForkliftContainerLog", "", "", 10, listRfid.Count, sessionKey);
                    long maxId = Convert.ToInt64(maxIdKey);
                    List<AscmForkliftContainerLog> listAscmForkliftContainerLog = new List<AscmForkliftContainerLog>();
                    foreach (string rfid in listRfid)
                    {
                        AscmForkliftContainerLog ascmForkliftContainerLog = new AscmForkliftContainerLog();
                        ascmForkliftContainerLog.id = ++maxId;
                        ascmForkliftContainerLog.forkliftId = ascmForklift.id;
                        ascmForkliftContainerLog.forkliftIdRfidId = forkliftRfid;
                        ascmForkliftContainerLog.containerRfidId = rfid;
                        ascmForkliftContainerLog.createTime = dtServer.ToString("yyyy-MM-dd HH:mm");
                        ascmForkliftContainerLog.passDate = dtServer.ToString("yyyy-MM-dd");
                        ascmForkliftContainerLog.times = times;
                        ascmForkliftContainerLog.readingHeadId = ascmReadingHead.id;
                        ascmForkliftContainerLog.readingHeadIp = ascmReadingHead.ip;
                        ascmForkliftContainerLog.status = "";
                        listAscmForkliftContainerLog.Add(ascmForkliftContainerLog);
                    }
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession(sessionKey).BeginTransaction())
                    {
                        try
                        {
                            if (listAscmForkliftContainerLog.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmForkliftContainerLog, sessionKey);


                            tx.Commit();//正确执行提交
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            throw ex;
                        }
                    }
                    //建立领料员单次领料与领料单的关联
                    AscmWmsMtlRequisitionMainService.GetInstance().WmsStoreIssueCheck(forkliftRfid, times, dtServer.ToString("yyyy-MM-dd HH:mm"), sessionKey);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("记录日志失败(Find AscmForkliftContainerLog)", ex);
                throw ex;
            }
        }
    }
}
