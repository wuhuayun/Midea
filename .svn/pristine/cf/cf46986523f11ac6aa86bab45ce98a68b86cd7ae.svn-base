using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Services.MesInterface;
using MideaAscm.Dal.IEntity;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Services.FromErp;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Services.Base;

namespace MideaAscm.Services.Warehouse
{
    public class AscmWmsMtlRequisitionMainService
    {
        private static AscmWmsMtlRequisitionMainService ascmWmsMtlRequisitionMainService;
        public static AscmWmsMtlRequisitionMainService GetInstance()
        {
            if (ascmWmsMtlRequisitionMainService == null)
                ascmWmsMtlRequisitionMainService = new AscmWmsMtlRequisitionMainService();
            return ascmWmsMtlRequisitionMainService;
        }

        public AscmWmsMtlRequisitionMain Get(int id)
        {
            AscmWmsMtlRequisitionMain aAscmWmsMtlRequisitionMain = null;
            try
            {
                aAscmWmsMtlRequisitionMain = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsMtlRequisitionMain>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsPreparationMain)", ex);
                throw ex;
            }
            return aAscmWmsMtlRequisitionMain;
        }
        public List<AscmWmsMtlRequisitionMain> GetList(string sql)
        {
            List<AscmWmsMtlRequisitionMain> list = null;
            try
            {
                IList<AscmWmsMtlRequisitionMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMtlRequisitionMain>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsMtlRequisitionMain>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsMtlRequisitionMain)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsMtlRequisitionMain> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmWmsMtlRequisitionMain> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " docNumber like '%" + queryWord.Trim() + "%'";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                string sql = "from AscmWmsMtlRequisitionMain ";

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }
                IList<AscmWmsMtlRequisitionMain> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMtlRequisitionMain>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMtlRequisitionMain>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsMtlRequisitionMain>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsMtlRequisitionMain)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmWmsMtlRequisitionMain> listAscmWmsMtlRequisitionMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsMtlRequisitionMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsMtlRequisitionMain)", ex);
                throw ex;
            }
        }
        public void Save(AscmWmsMtlRequisitionMain ascmWmsMtlRequisitionMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsMtlRequisitionMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsMtlRequisitionMain)", ex);
                throw ex;
            }
        }
        public void Update(AscmWmsMtlRequisitionMain ascmWmsMtlRequisitionMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWmsMtlRequisitionMain>(ascmWmsMtlRequisitionMain);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWmsMtlRequisitionMain)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmWmsMtlRequisitionMain)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmWmsMtlRequisitionMain ascmWmsMtlRequisitionMain = Get(id);
                Delete(ascmWmsMtlRequisitionMain);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmWmsMtlRequisitionMain ascmWmsMtlRequisitionMain)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWmsMtlRequisitionMain>(ascmWmsMtlRequisitionMain);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsMtlRequisitionMain)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmWmsMtlRequisitionMain> listAscmWmsMtlRequisitionMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsMtlRequisitionMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsMtlRequisitionMain)", ex);
                throw ex;
            }
        }

        public void SetWipDiscreteJobs(List<AscmWmsMtlRequisitionMain> list, 
            bool isSetWipEntities = true,
            bool isSetScheduleGroups = true,
            bool isSetMaterialItem = true,
            bool isSetLookupValues = true)
        {
            if (list != null && list.Count > 0)
            {
                var wipEntityIds = list.Select(P => P.wipEntityId).Distinct();
                var count = wipEntityIds.Count();
                string ids = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += wipEntityIds.ElementAt(i);
                    if ((i + 1) % 500 == 0 || (i + 1) == count)
                    {
                        if (!string.IsNullOrEmpty(ids))
                        {
                            string sql = "from AscmWipDiscreteJobs where wipEntityId in(" + ids + ")";
                            IList<AscmWipDiscreteJobs> ilistAscmWipDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sql);
                            if (ilistAscmWipDiscreteJobs != null && ilistAscmWipDiscreteJobs.Count > 0)
                            {
                                List<AscmWipDiscreteJobs> listAscmWipDiscreteJobs = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobs>(ilistAscmWipDiscreteJobs);
                                if (isSetWipEntities)
                                {
                                    AscmWipDiscreteJobsService.GetInstance().SetWipEntities(listAscmWipDiscreteJobs);
                                    AscmWipDiscreteJobsService.GetInstance().SetScheduleProductionLine(listAscmWipDiscreteJobs);
                                }
                                if (isSetScheduleGroups)
                                    AscmWipDiscreteJobsService.GetInstance().SetScheduleGroups(listAscmWipDiscreteJobs);
                                if (isSetMaterialItem)
                                    AscmWipDiscreteJobsService.GetInstance().SetMaterialItem(listAscmWipDiscreteJobs);
                                if (isSetLookupValues)
                                    AscmWipDiscreteJobsService.GetInstance().SetLookupValues(listAscmWipDiscreteJobs);
                                foreach (AscmWmsMtlRequisitionMain wmsMtlRequisitionMain in list)
                                {
                                    AscmWipDiscreteJobs ascmWipDiscreteJobs = listAscmWipDiscreteJobs.Find(P => P.wipEntityId == wmsMtlRequisitionMain.wipEntityId);
                                    if (ascmWipDiscreteJobs != null)
                                        wmsMtlRequisitionMain.ascmWipDiscreteJobs = ascmWipDiscreteJobs;
                                }
                            }
                        }
                        ids = string.Empty;
                    }
                }
            }
        }

        public void SetPreparationDocNumbers(List<AscmWmsMtlRequisitionMain> list)
        {
            if (list != null && list.Count > 0)
            {
                var ieIds = list.Select(P => P.id).Distinct();
                var count = ieIds.Count();
                string ids = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(ids)) ids += ",";
                    ids += ieIds.ElementAt(i);
                }

                if (!string.IsNullOrEmpty(ids))
                {
                    string hql = "from AscmWmsMtlReqMainLink where reqMainLinkPK.reqMainId in(" + ids + ")";
                    IList<AscmWmsMtlReqMainLink> ilistReqMainLink = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMtlReqMainLink>(hql);
                    if (ilistReqMainLink != null && ilistReqMainLink.Count > 0)
                    {
                        List<AscmWmsMtlReqMainLink> listReqMainLink = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsMtlReqMainLink>(ilistReqMainLink);
                        
                        hql = "from AscmWmsPreparationMain where id in(" + " select reqMainLinkPK.preMainId from AscmWmsMtlReqMainLink where reqMainLinkPK.reqMainId in(" + ids + ") )";
                        IList<AscmWmsPreparationMain> ilistPreparationMain = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsPreparationMain>(hql);
                        if (ilistPreparationMain != null && ilistPreparationMain.Count > 0)
                        {
                            List<AscmWmsPreparationMain> listPreparationMain = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsPreparationMain>(ilistPreparationMain);
                            foreach (AscmWmsMtlRequisitionMain requisitionMain in list)
                            {
                                requisitionMain.preparationDocNumbers
                                    = string.Join(",", listPreparationMain.Where(P =>
                                        listReqMainLink.Where(T => T.reqMainLinkPK.reqMainId == requisitionMain.id).
                                        Select(T => T.reqMainLinkPK.preMainId).Contains(P.id)).OrderBy(p => p.docNumber).
                                        Select(P => P.docNumber));
                            }
                        }
                    }
                }
            }
        }

        public List<AscmWmsMtlReqMainLink> GetReqMainLinkList(List<AscmWmsMtlRequisitionMain> list)
        {
            List<AscmWmsMtlReqMainLink> listReqMainLink = null;
            if (list != null && list.Count > 0)
            {
                listReqMainLink = new List<AscmWmsMtlReqMainLink>();
                var selectIds = list.Select(P => P.id).Distinct();
                var count = selectIds.Count();
                string ids = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += selectIds.ElementAt(i);
                    if ((i + 1) % 500 == 0 || (i + 1) == count)
                    {
                        if (!string.IsNullOrEmpty(ids))
                        {
                            string hql = "from AscmWmsMtlReqMainLink where reqMainLinkPK.reqMainId in(" + ids + ")";
                            IList<AscmWmsMtlReqMainLink> ilistReqMainLink = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMtlReqMainLink>(hql);
                            if (ilistReqMainLink != null)
                                listReqMainLink.AddRange(ilistReqMainLink);
                        }
                        ids = string.Empty;
                    }
                }
            }
            return listReqMainLink;
        }

        #region 应用
        //发料校验
        public List<AscmWmsStoreIssueCheck> GetWmsLedStoreIssueCheck()
        {
            List<AscmWmsStoreIssueCheck> listStoreIssueCheck = new List<AscmWmsStoreIssueCheck>();
            //获取当天的领料单
            string whereOther = "";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "createTime>='" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "'");
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "createTime<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'");
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "checkOutNo>0");

            List<AscmWmsMtlRequisitionMain> listRequisitionMain = GetList(null, "", "", "", whereOther);
            if (listRequisitionMain == null || listRequisitionMain.Count == 0)
                return listStoreIssueCheck;
            SetWipDiscreteJobs(listRequisitionMain, true, true, false, false);
            //获取当天的领料明细
            List<AscmWmsMtlRequisitionDetail> listRequisitionDetail = AscmWmsMtlRequisitionDetailService.GetInstance().GetList(listRequisitionMain);
            if (listRequisitionDetail == null || listRequisitionDetail.Count == 0)
                return listStoreIssueCheck;
            //获取领料单与备料单关联
            List<AscmWmsMtlReqMainLink> listReqMainLink = GetReqMainLinkList(listRequisitionMain);
            //获取备料容器
            List<AscmWmsContainerDelivery> listContainerDelivery = AscmWmsContainerDeliveryService.GetInstance().GetList(listReqMainLink.Select(P => P.reqMainLinkPK.preMainId).ToList(), "");

            //按领料员分组
            var result = listRequisitionMain.Where(P => !string.IsNullOrEmpty(P.workerId)).GroupBy(P => P.workerId);
            foreach (IGrouping<string, AscmWmsMtlRequisitionMain> ig in result)
            {
                //按领料次数分组
                var result2 = ig.GroupBy(P => P.checkOutNo);
                foreach (IGrouping<int, AscmWmsMtlRequisitionMain> ig2 in result2)
                {
                    AscmWmsStoreIssueCheck storeIssueCheck = new AscmWmsStoreIssueCheck();
                    storeIssueCheck.workerId = ig.Key;
                    storeIssueCheck.checkTime = ig2.First().checkTime;
                    storeIssueCheck.times = ig2.Key;
                    storeIssueCheck.destination = ig2.First().jobScheduleGroupsName;
                    listStoreIssueCheck.Add(storeIssueCheck);

                    //获取应发容器
                    List<AscmWmsContainerDelivery> listContainerDeliverySub = new List<AscmWmsContainerDelivery>();
                    foreach (AscmWmsMtlRequisitionMain requisitionMain in ig2)
                    {
                        List<AscmWmsMtlRequisitionDetail> listRequisitionDetailSub = listRequisitionDetail.Where(P => P.mainId == requisitionMain.id).ToList();
                        storeIssueCheck.shouldMaterialNum += listRequisitionDetailSub.Sum(P => P.quantity);
                        //考虑相同作业、相同物料、取自不同货位，所以应按物料分组
                        var detailResult = listRequisitionDetailSub.GroupBy(P => P.materialId);
                        foreach (IGrouping<int, AscmWmsMtlRequisitionDetail> detailIg in detailResult)
                        {
                            var findContainerDelivery = listContainerDeliverySub.Where(P => P.wipEntityId == requisitionMain.wipEntityId
                                && P.materialId == detailIg.Key
                                && listReqMainLink.Where(T => T.reqMainLinkPK.reqMainId == requisitionMain.id).Select(T => T.reqMainLinkPK.preMainId).Contains(P.preparationMainId));
                            if (findContainerDelivery != null)
                            {
                                listContainerDeliverySub.AddRange(findContainerDelivery);
                            }
                        }
                    }
                    if (listContainerDeliverySub.Count > 0)
                    {
                        storeIssueCheck.shouldContainerNum = listContainerDeliverySub.Select(P => P.containerSn).Distinct().Count();

                        var findContainerDelivery = listContainerDeliverySub.Where(P => P.status == AscmWmsContainerDelivery.StatusDefine.outWarehouseDoor);
                        if (findContainerDelivery.Count() > 0)
                        {
                            storeIssueCheck.realMaterialNum = findContainerDelivery.Sum(P => P.quantity);
                            storeIssueCheck.realContainerNum = findContainerDelivery.Select(P => P.containerSn).Distinct().Count();
                        }
                    }

                    //设置产线
                    storeIssueCheck.productionLine = string.Join("、", ig2.Where(P => !string.IsNullOrEmpty(P.jobProductionLine)).Select(P => P.jobProductionLine).Distinct().OrderBy(P => P));

                    //设置发料状态
                    storeIssueCheck.status = IssueStatus.prepared;
                    //获取校验的最后时间
                    string lastCheckTime = ig2.Where(P => !string.IsNullOrEmpty(P.checkTime)).Max(P => P.checkTime);
                    if (!string.IsNullOrEmpty(lastCheckTime))
                    {
                        //暂定最后校验时间与当前时间间隔1分钟以内的设置为“正在出仓”
                        DateTime checkTime;
                        if (DateTime.TryParse(lastCheckTime, out checkTime) && DateTime.Now.Subtract(checkTime).TotalMinutes < 1)
                            storeIssueCheck.status = IssueStatus.outingOfWarehouse;
                    }
                    if (storeIssueCheck.waitContainerNum == 0 && storeIssueCheck.status != IssueStatus.outingOfWarehouse)
                        storeIssueCheck.status = IssueStatus.outedOfWarehouse;
                }
            }
            return listStoreIssueCheck;
        }
        /// <summary>
        /// 提供中间件调用，建立领料员单次领料与领料单的关联
        /// </summary>
        /// <param name="forkliftRfid">叉车RFID</param>
        /// <param name="checkoutNo">当天第几次校验</param>
        /// <param name="checkTime">校验时间</param>
        /// <param name="sessionKey"></param>
        public void WmsStoreIssueCheck(string forkliftRfid, int checkoutNo, string checkTime, string sessionKey)
        {
            if (string.IsNullOrEmpty(forkliftRfid))
                return;
            if (checkoutNo <= 0)
                return;
            try
            {
                object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select workerId from AscmForklift where tagId='" + forkliftRfid + "' and rownum=1", sessionKey);
                if (obj == null)
                    return;
                string workerId = obj.ToString();
                if (string.IsNullOrEmpty(workerId))
                    return;
                DateTime dt = DateTime.Now;
                DateTime.TryParse(checkTime, out dt);
                //获取领料员本次领料生成的领料单
                string hql = "from AscmWmsMtlRequisitionMain";
                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "workerId='" + workerId + "'");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "checkout=0");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "createTime>='" + dt.ToString("yyyy-MM-dd 00:00") + "' and createTime<'" + dt.AddDays(1).ToString("yyyy-MM-dd 00:00") + "'");
                hql += " where " + where;
                IList<AscmWmsMtlRequisitionMain> ilistRequisitionMain = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMtlRequisitionMain>(hql, sessionKey);
                if (ilistRequisitionMain == null || ilistRequisitionMain.Count == 0)
                    return;
                List<AscmWmsMtlRequisitionMain> listRequisitionMain = ilistRequisitionMain.ToList();

                ////避免硬件故障或中间件服务停止造成本次累计了前面未校验的领料单，此处作时间间隔限制
                //string maxCreateTime = listRequisitionMain.Max(P => P.createTime);
                //DateTime dtMaxCreateTime = DateTime.Now;
                //bool parseMaxCreateTime = DateTime.TryParse(maxCreateTime, out dtMaxCreateTime);

                foreach (AscmWmsMtlRequisitionMain requisitionMain in listRequisitionMain)
                {
                    ////凡比最晚生成的领料单早30分钟以上的将被排除
                    //DateTime dtCreateTime = DateTime.Now;
                    //if (parseMaxCreateTime && DateTime.TryParse(requisitionMain.createTime, out dtCreateTime) && dtMaxCreateTime.CompareTo(dtCreateTime.AddMinutes(30)) > 0)
                    //    continue;

                    requisitionMain.checkout = true;
                    requisitionMain.checkOutNo = checkoutNo;
                    requisitionMain.checkTime = dt.ToString("yyyy-MM-dd HH:mm");
                }

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession(sessionKey).BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listRequisitionMain, sessionKey);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsMtlRequisitionMain)", ex);
                throw ex;
            }
        }
        #endregion`
    } 
}
