using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Services.FromErp;
using MideaAscm.Services.Base;

namespace MideaAscm.Services.Warehouse
{
    public class AscmWmsPreparationMainService
    {
        private static AscmWmsPreparationMainService service;
        public static AscmWmsPreparationMainService GetInstance()
        {
            if (service == null)
                service = new AscmWmsPreparationMainService();
            return service;
        }

        public AscmWmsPreparationMain Get(int id)
        {
            AscmWmsPreparationMain preparationMain = null;
            try
            {
                preparationMain = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsPreparationMain>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsPreparationMain)", ex);
                throw ex;
            }
            return preparationMain;
        }
        public List<AscmWmsPreparationMain> GetList(string sql)
        {
            List<AscmWmsPreparationMain> list = null;
            try
            {
                IList<AscmWmsPreparationMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsPreparationMain>(sql);
                if (ilist != null)
                    list = ilist.ToList();
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsPreparationMain)", ex);
                throw ex;
            }
            return list;
        }

        public List<AscmWmsPreparationMain> GetList(List<int> wipEntityIds)
        {
            if (wipEntityIds == null || wipEntityIds.Count == 0)
            {
                return null;
            }

            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendFormat(" from AscmWmsPreparationMain ");
            strSQL.AppendFormat(" where wipEntityId in ({0}) ", string.Join(",", wipEntityIds));
            List<AscmWmsPreparationMain> list = GetList(strSQL.ToString());

            return list;
        }

        public List<AscmWmsPreparationMain> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmWmsPreparationMain> list = null;
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

                string sql = "from AscmWmsPreparationMain ";

                string totalNumber = "select sum(planQuantity) from AscmWmsPreparationDetail where mainId=awpm.id";
                string containerBindNumber = "select sum(quantity) from AscmWmsContainerDelivery where preparationMainId=awpm.id";
                //string containerBindNumber = "select sum(quantity) from AscmWmsContainerDelivery where wipEntityId=awpm.wipEntityId";
                string sql1 = string.Format("select new AscmWmsPreparationMain(awpm,({0}),({1})) from AscmWmsPreparationMain awpm", totalNumber, containerBindNumber);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    sql1 += " where " + where;
                }
                IList<AscmWmsPreparationMain> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsPreparationMain>(sql1 + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsPreparationMain>(sql1 + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsPreparationMain>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsPreparationMain)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsPreparationMain> GetCurrentList(string userId)
        {
            string whereOther = "";
            //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "status='" + AscmWmsPreparationMain.StatusDefine.preparationed + "'");
            return GetList(null, "", "", "", whereOther);
        }
        public List<AscmWmsPreparationMain> GetJobMonitorList(YnPage ynPage, string sortName, string sortOrder, string whereOther)
        {
            List<AscmWmsPreparationMain> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                string sql = "from AscmWmsPreparationMain ";

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }
                IList<AscmWmsPreparationMain> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsPreparationMain>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsPreparationMain>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsPreparationMain>(ilist);
                    SetWipDiscreteJobs(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsPreparationMain)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsPreparationMain> GetListByIds(string preparationMainIds, string whereOther = "")
        {
            List<AscmWmsPreparationMain> list = null;
            if (!string.IsNullOrEmpty(preparationMainIds))
            {
                list = new List<AscmWmsPreparationMain>();
                var ieId = preparationMainIds.Split(',').Distinct();
                int count = ieId.Count();
                string ids = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ieId.ElementAt(i);
                    if ((i + 1) % 500 == 0 || (i + 1) == count)
                    {
                        if (!string.IsNullOrEmpty(ids))
                        {
                            string hql = "from AscmWmsPreparationMain";
                            string where = "";
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "id in(" + ids + ")");
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "locked=0");
                            if (!string.IsNullOrEmpty(whereOther))
                                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                            hql += " where " + where;

                            IList<AscmWmsPreparationMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsPreparationMain>(hql);
                            if (ilist != null)
                                list.AddRange(ilist);
                        }
                        ids = string.Empty;
                    }
                }
            }
            return list;
        }

        public void Save(List<AscmWmsPreparationMain> listPreparationMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listPreparationMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsPreparationMain)", ex);
                throw ex;
            }
        }
        public void Save(AscmWmsPreparationMain preparationMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(preparationMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsPreparationMain)", ex);
                throw ex;
            }
        }
        public void Update(AscmWmsPreparationMain preparationMain, bool changeWipEntityStatus = false)
        {
            try
            {
                // 2014/5/14 当备料单状态变更为“已备齐”或“备料中_已确认”时，如果作业状态处于“待领料”或“已领料”则变更为“备料中”
                AscmWipDiscreteJobs wipDiscreteJobs = null;
                AscmWipDiscreteJobsStatus jobsStatus = null;
                if (changeWipEntityStatus && (preparationMain.status == AscmWmsPreparationMain.StatusDefine.prepared || preparationMain.status == AscmWmsPreparationMain.StatusDefine.preparing))
                {
                    //旧的逻辑
                    wipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().Get(preparationMain.wipEntityId.ToString());
                    if (wipDiscreteJobs.ascmStatus == AscmWipDiscreteJobs.AscmStatusDefine.unPick || wipDiscreteJobs.ascmStatus == AscmWipDiscreteJobs.AscmStatusDefine.picked)
                        wipDiscreteJobs.ascmStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing;
                    else
                        wipDiscreteJobs = null;

                    //新的逻辑，保存到明细状态
                    jobsStatus = AscmWipDiscreteJobsService.Instance.Get(preparationMain.wipEntityId, AscmWhTeamUserService.Instance.GetLeaderId(preparationMain.createUser));
                    if (jobsStatus != null && jobsStatus.subStatus == AscmWipDiscreteJobs.AscmStatusDefine.unPick || jobsStatus.subStatus == AscmWipDiscreteJobs.AscmStatusDefine.picked)
                        jobsStatus.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing;
                    else
                        jobsStatus = null;
                }

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWmsPreparationMain>(preparationMain);

                        if (wipDiscreteJobs != null)
                            YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWipDiscreteJobs>(wipDiscreteJobs);

                        if (jobsStatus != null) 
                            YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWipDiscreteJobsStatus>(jobsStatus);

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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWmsPreparationMain)", ex);
                throw ex;
            }
        }
        public void Update(List<AscmWmsPreparationMain> listPreparationMain, bool changeWipEntityStatus = false)
        {
            try
            {
                // 2014/5/14 当备料单状态变更为“已备齐”或“备料中_已确认”时，如果作业状态处于“待领料”或“已领料”则变更为“备料中”
                List<AscmWipDiscreteJobs> listWipDiscreteJobs = null;
                List<AscmWipDiscreteJobsStatus> listStatus = null;
                if (changeWipEntityStatus)
                {
                    List<AscmWmsPreparationMain> _listPreparationMain = listPreparationMain.Where(P => P.status == AscmWmsPreparationMain.StatusDefine.prepared || P.status == AscmWmsPreparationMain.StatusDefine.preparing).ToList();
                    if (_listPreparationMain != null && _listPreparationMain.Count > 0)
                    {
                        //旧的逻辑
                        listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList(_listPreparationMain.Select(P => P.wipEntityId).ToList(), "ascmStatus in('" + AscmWipDiscreteJobs.AscmStatusDefine.unPick + "','" + AscmWipDiscreteJobs.AscmStatusDefine.picked + "')");
                        if (listWipDiscreteJobs != null && listWipDiscreteJobs.Count > 0)
                            listWipDiscreteJobs.ForEach(P => P.ascmStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing);
                        else
                            listWipDiscreteJobs = null;

                        //新的逻辑，保存到明细状态
                        StringBuilder strStatusWhere = new StringBuilder();
                        //wipEntityId
                        strStatusWhere.Append(" AND wipEntityId IN (");
                        foreach (var mainInfo in listPreparationMain)
                        {
                            strStatusWhere.Append(mainInfo.wipEntityId);
                            strStatusWhere.Append(",");
                        }
                        strStatusWhere.Remove(strStatusWhere.Length - 1, 1);
                        strStatusWhere.Append(") ");
                        //leaderId
                        var listUserId = listPreparationMain.Select(a => a.createUser).ToList();
                        var whereLeaderIds = AscmWhTeamUserService.Instance.GetLeaderIdsForWhere(listUserId);
                        if (whereLeaderIds != null) 
                        {
                            strStatusWhere.Append(" AND leaderId IN (" + whereLeaderIds + ") ");

                            listStatus = AscmWipDiscreteJobsService.Instance.GetListByStrWhere(strStatusWhere.ToString());
                            if (listStatus != null) 
                            {
                                listStatus = listStatus.Where(a => a.subStatus == AscmWipDiscreteJobs.AscmStatusDefine.unPick || a.subStatus == AscmWipDiscreteJobs.AscmStatusDefine.picked).ToList() ;

                                if (listStatus != null && listStatus.Count > 0)
                                    listStatus.ForEach(P => P.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing);
                                else
                                    listStatus = null;
                            }
                        }
                    }
                }

                YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().Clear();
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listPreparationMain);

                        if (listWipDiscreteJobs != null)
                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listWipDiscreteJobs);

                        if (listStatus != null)
                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listStatus);

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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Update AscmWmsPreparationMain)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmWmsPreparationMain preparationMain = Get(id);
                Delete(preparationMain);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmWmsPreparationMain preparationMain)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWmsPreparationMain>(preparationMain);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsPreparationMain)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmWmsPreparationMain> listPreparationMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listPreparationMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsPreparationMain)", ex);
                throw ex;
            }
        }
        public void SetWipDiscreteJobs(List<AscmWmsPreparationMain> list)
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
                                AscmWipDiscreteJobsService.GetInstance().SetWipEntities(listAscmWipDiscreteJobs);
                                AscmWipDiscreteJobsService.GetInstance().SetScheduleGroups(listAscmWipDiscreteJobs);
                                AscmWipDiscreteJobsService.GetInstance().SetMaterialItem(listAscmWipDiscreteJobs);
                                //AscmWipDiscreteJobsService.GetInstance().SetLookupValues(listAscmWipDiscreteJobs);
                                AscmWipDiscreteJobsService.GetInstance().SetScheduleProductionLine(listAscmWipDiscreteJobs);
                                foreach (AscmWmsPreparationMain preparationMain in list)
                                {
                                    AscmWipDiscreteJobs ascmWipDiscreteJobs = listAscmWipDiscreteJobs.Find(P => P.wipEntityId == preparationMain.wipEntityId);
                                    if (ascmWipDiscreteJobs != null)
                                        preparationMain.ascmWipDiscreteJobs = ascmWipDiscreteJobs;
                                }
                            }
                        }
                        ids = string.Empty;
                    }
                }
            }
        }
        public void SetDetailInfo(List<AscmWmsPreparationMain> list, bool isSetWipEntity = false, bool isSetScheduleDate = false, bool isSetScheduleGroups = false, bool isSetProductionLine = false)
        {
            if (list != null && list.Count > 0)
            {
                var selectIds = list.Select(P => P.id);
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
                            string hql = "select new AscmWmsPreparationDetail(awpd,ami.docNumber) from AscmWmsPreparationDetail awpd,AscmMaterialItem ami where awpd.materialId=ami.id and awpd.mainId in(" + ids + ")";
                            IList<AscmWmsPreparationDetail> ilistPreparationDetail = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsPreparationDetail>(hql);
                            if (ilistPreparationDetail != null && ilistPreparationDetail.Count > 0)
                            {
                                List<AscmWmsPreparationDetail> listPreparationDetail = ilistPreparationDetail.ToList();
                                if (isSetWipEntity || isSetScheduleDate || isSetProductionLine || isSetScheduleGroups)
                                    AscmWmsPreparationDetailService.GetInstance().SetWipDiscreteJobs(listPreparationDetail);
                                foreach (AscmWmsPreparationMain preparationMain in list)
                                {
                                    List<AscmWmsPreparationDetail> _listPreparationDetail = listPreparationDetail.FindAll(P => P.mainId == preparationMain.id);
                                    if (_listPreparationDetail != null)
                                    {
                                        string minWipEntity = string.Empty, maxWipEntity = string.Empty;
                                        string minScheduleDate = string.Empty, maxScheduleDate = string.Empty;
                                        string minWarehouse = string.Empty, maxWarehouse = string.Empty;
                                        string minMaterialDoc = string.Empty, maxMaterialDoc = string.Empty;
                                        bool existScheduleGroups = false, existProductionLine = false;
                                        foreach (AscmWmsPreparationDetail preparationDetail in _listPreparationDetail)
                                        {
                                            //作业段
                                            if (string.IsNullOrEmpty(minWipEntity))
                                                minWipEntity = preparationDetail.jobWipEntityName;
                                            else if (!string.IsNullOrEmpty(preparationDetail.jobWipEntityName) && minWipEntity.CompareTo(preparationDetail.jobWipEntityName) > 0)
                                                minWipEntity = preparationDetail.warehouseId;
                                            if (string.IsNullOrEmpty(maxWipEntity))
                                                maxWipEntity = preparationDetail.jobWipEntityName;
                                            else if (!string.IsNullOrEmpty(preparationDetail.jobWipEntityName) && maxWipEntity.CompareTo(preparationDetail.jobWipEntityName) < 0)
                                                maxWipEntity = preparationDetail.jobWipEntityName;
                                            //作业时间段
                                            if (string.IsNullOrEmpty(minScheduleDate))
                                                minScheduleDate = preparationDetail.jobScheduledStartDate;
                                            else if (!string.IsNullOrEmpty(preparationDetail.jobScheduledStartDate) && minScheduleDate.CompareTo(preparationDetail.jobScheduledStartDate) > 0)
                                                minScheduleDate = preparationDetail.jobScheduledStartDate;
                                            if (string.IsNullOrEmpty(maxScheduleDate))
                                                maxScheduleDate = preparationDetail.jobScheduledStartDate;
                                            else if (!string.IsNullOrEmpty(preparationDetail.jobScheduledStartDate) && maxScheduleDate.CompareTo(preparationDetail.jobScheduledStartDate) < 0)
                                                maxScheduleDate = preparationDetail.jobScheduledStartDate;
                                            //子库段
                                            if (string.IsNullOrEmpty(minWarehouse))
                                                minWarehouse = preparationDetail.warehouseId;
                                            else if (!string.IsNullOrEmpty(preparationDetail.warehouseId) && minWarehouse.CompareTo(preparationDetail.warehouseId) > 0)
                                                minWarehouse = preparationDetail.warehouseId;
                                            if (string.IsNullOrEmpty(maxWarehouse))
                                                maxWarehouse = preparationDetail.warehouseId;
                                            else if (!string.IsNullOrEmpty(preparationDetail.warehouseId) && maxWarehouse.CompareTo(preparationDetail.warehouseId) < 0)
                                                maxWarehouse = preparationDetail.warehouseId;
                                            //物料大类段
                                            if (string.IsNullOrEmpty(minMaterialDoc))
                                                minMaterialDoc = preparationDetail.materialDocNumber;
                                            else if (!string.IsNullOrEmpty(preparationDetail.materialDocNumber) && minMaterialDoc.CompareTo(preparationDetail.materialDocNumber) > 0)
                                                minMaterialDoc = preparationDetail.materialDocNumber;
                                            if (string.IsNullOrEmpty(maxMaterialDoc))
                                                maxMaterialDoc = preparationDetail.materialDocNumber;
                                            else if (!string.IsNullOrEmpty(preparationDetail.materialDocNumber) && maxMaterialDoc.CompareTo(preparationDetail.materialDocNumber) < 0)
                                                maxMaterialDoc = preparationDetail.materialDocNumber;
                                            //计划组
                                            if (isSetScheduleGroups && !existScheduleGroups)
                                            {
                                                if (string.IsNullOrEmpty(preparationMain.requireScheduleGroupsName))
                                                {
                                                    if (!string.IsNullOrEmpty(preparationDetail.jobScheduleGroupsName))
                                                        preparationMain.requireScheduleGroupsName = preparationDetail.jobScheduleGroupsName;
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(preparationDetail.jobScheduleGroupsName))
                                                    {
                                                        if (preparationMain.requireScheduleGroupsName != preparationDetail.jobScheduleGroupsName)
                                                        {
                                                            preparationMain.requireScheduleGroupsName = string.Empty;
                                                            existScheduleGroups = true;
                                                        }
                                                    }
                                                }
                                            }
                                            //产线
                                            if (isSetProductionLine && !existProductionLine)
                                            {
                                                if (string.IsNullOrEmpty(preparationMain.requireProductionLine))
                                                {
                                                    if (!string.IsNullOrEmpty(preparationDetail.jobProductionLine))
                                                        preparationMain.requireProductionLine = preparationDetail.jobProductionLine;
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(preparationDetail.jobProductionLine))
                                                    {
                                                        if (preparationMain.requireProductionLine != preparationDetail.jobProductionLine)
                                                        {
                                                            preparationMain.requireProductionLine = string.Empty;
                                                            existProductionLine = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //作业段
                                        if (!string.IsNullOrEmpty(minWipEntity))
                                            preparationMain.requireWipEntitySegment = minWipEntity;
                                        if (!string.IsNullOrEmpty(maxWipEntity) && minWipEntity != maxWipEntity)
                                        {
                                            if (!string.IsNullOrEmpty(preparationMain.requireWipEntitySegment))
                                                preparationMain.requireWipEntitySegment += "至";
                                            preparationMain.requireWipEntitySegment += maxWipEntity;
                                        }
                                        //作业时间段
                                        if (!string.IsNullOrEmpty(minScheduleDate))
                                            preparationMain.requireScheduledDateSegment = minScheduleDate;
                                        if (!string.IsNullOrEmpty(maxScheduleDate) && minScheduleDate != maxScheduleDate)
                                        {
                                            if (!string.IsNullOrEmpty(preparationMain.requireScheduledDateSegment))
                                                preparationMain.requireScheduledDateSegment += "至";
                                            preparationMain.requireScheduledDateSegment += maxScheduleDate;
                                        }
                                        //子库段
                                        if (!string.IsNullOrEmpty(minWarehouse))
                                            preparationMain.jobWarehouseSegment = minWarehouse;
                                        if (!string.IsNullOrEmpty(maxWarehouse) && minWarehouse != maxWarehouse)
                                        {
                                            if (!string.IsNullOrEmpty(preparationMain.jobWarehouseSegment))
                                                preparationMain.jobWarehouseSegment += "-";
                                            preparationMain.jobWarehouseSegment += maxWarehouse;
                                        }
                                        //物料大类段
                                        if (!string.IsNullOrEmpty(minMaterialDoc))
                                            preparationMain.jobMtlCategorySegment = minMaterialDoc.Substring(0, 4);
                                        if (!string.IsNullOrEmpty(maxMaterialDoc) && minMaterialDoc.Substring(0, 4) != maxMaterialDoc.Substring(0, 4))
                                        {
                                            if (!string.IsNullOrEmpty(preparationMain.jobMtlCategorySegment))
                                                preparationMain.jobMtlCategorySegment += "-";
                                            preparationMain.jobMtlCategorySegment += maxMaterialDoc.Substring(0, 4);
                                        }
                                    }
                                }
                            }
                        }
                        ids = string.Empty;
                    }
                }
            }
        }

        #region 业务
        /// <summary>获取手持端作业备料列表</summary>
        public List<AscmWmsPreparationMain> GetMobileWipJobPreparationList(string userId)
        {
            string hql = "select new AscmWmsPreparationMain(awpm,awe.name) from AscmWmsPreparationMain awpm,AscmWipEntities awe,AscmWipDiscreteJobs awdj";
            string where = "";
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpm.wipEntityId=awe.wipEntityId");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpm.wipEntityId=awdj.wipEntityId");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpm.pattern='" + AscmWmsPreparationMain.PatternDefine.wipJob + "'");
            //状态：待备料、备料中_未确认、备料中_已确认
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, string.Format("awpm.status in('{0}','{1}','{2}')", AscmWmsPreparationMain.StatusDefine.unPrepare, AscmWmsPreparationMain.StatusDefine.preparingUnConfirm, AscmWmsPreparationMain.StatusDefine.preparing));
            //作业日期：大于等于今天
            //where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awdj.scheduledStartDate>='" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "'");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awdj.scheduledStartDate>='2014-03-25 00:00'");

            hql += " where " + where;
            return GetList(hql);
        }
        /// <summary>获取手持端需求备料列表</summary>
        public List<AscmWmsPreparationMain> GetMobileWipRequirePreparationList(string userId)
        {
            string where = "";
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "pattern='" + AscmWmsPreparationMain.PatternDefine.wipRequire + "'");
            //状态：待备料、备料中_未确认、备料中_已确认
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, string.Format("awpm.status in('{0}','{1}','{2}')", AscmWmsPreparationMain.StatusDefine.unPrepare, AscmWmsPreparationMain.StatusDefine.preparingUnConfirm, AscmWmsPreparationMain.StatusDefine.preparing));
            string wipEntityName = "select name from AscmWipEntities where wipEntityId=(select wipEntityId from AscmWmsPreparationDetail awpd where awpd.mainId=awpm.id and rownum=1)";
            string hql = string.Format("select new AscmWmsPreparationMain(awpm,({0})) from AscmWmsPreparationMain awpm", wipEntityName);
            hql += " where " + where;
            return GetList(hql);
        }
        /// <summary>获取手持端备料单明细列表</summary>
        public List<AscmWmsPreparationDetail> GetMobileWmsPreparationDetailList(int id, string containerSn)
        {
            List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetList("select new AscmWmsPreparationDetail(awpd,ami.docNumber,ami.description) from AscmWmsPreparationDetail awpd,AscmMaterialItem ami where awpd.materialId=ami.id and awpd.mainId=" + id);
            if (listPreparationDetail != null && listPreparationDetail.Count > 0)
            {
                //获取作业物料容器备料列表(注意：对应作业物料，而不仅仅只是备料单)
                string whereOther = string.Empty;
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "quantity>0");
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "materialId in(" + string.Join(",", listPreparationDetail.Select(P => P.materialId).Distinct()) + ")");
                List<AscmWmsContainerDelivery> listContainerDelivery = AscmWmsContainerDeliveryService.GetInstance().GetListByWipEntity(listPreparationDetail.Select(P => P.wipEntityId).ToList(), whereOther);
                if (listContainerDelivery != null && listContainerDelivery.Count > 0)
                    listPreparationDetail.ForEach(P => P.listContainerDelivery = listContainerDelivery.FindAll(T => T.wipEntityId == P.wipEntityId && T.materialId == P.materialId));
                //如果查询特定容器，则从明细中移除其他备料明细
                if (!string.IsNullOrEmpty(containerSn))
                    listPreparationDetail.RemoveAll(P => P.listContainerDelivery == null || P.listContainerDelivery.Count == 0 || !P.listContainerDelivery.Exists(T => T.containerSn == containerSn));
            }
            return listPreparationDetail;
        }
        /// <summary>根据容器标签获取手持端备料单列表</summary>
        public List<AscmWmsPreparationMain> GetMobileWmsPreparationMainList(string containerSn)
        {
            List<AscmWmsPreparationMain> listPreparationMain = null;
            if (!string.IsNullOrEmpty(containerSn))
            {
                //兼容作业备料、需求备料
                string wipEntityName = "select name from AscmWipEntities where wipEntityId=(select wipEntityId from AscmWmsPreparationDetail where mainId=awpm.id and rownum=1)";
                string hql = string.Format("select new AscmWmsPreparationMain(awpm,({0})) from AscmWmsPreparationMain awpm", wipEntityName);
                string where = string.Empty;
                //排除已出库的
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "exists (select 1 from AscmWmsContainerDelivery where preparationMainId=awpm.id and containerSn='" + containerSn + "' and status is null)");
                //排除已领料的
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, string.Format("awpm.status<>'{0}'", AscmWmsPreparationMain.StatusDefine.picked));
                hql += " where " + where;
                listPreparationMain = GetList(hql);
            }
            return listPreparationMain;
        }
        /// <summary>手持端备料</summary>
        public bool DoMobilePreparation(int preparationMainId, List<AscmWmsContainerDelivery> listContainerDeliveryUpdate, ref string error)
        {
            error = string.Empty;
            AscmWmsPreparationMain preparationMain = null;
            if (preparationMainId > 0 && listContainerDeliveryUpdate != null && listContainerDeliveryUpdate.Count > 0)
                preparationMain = AscmWmsPreparationMainService.GetInstance().Get(preparationMainId);
            if (preparationMain == null)
            {
                error = "传送到服务端的数据有误！";
                return false;
            }
            //获取作业BOM
            string hql = "from AscmWipRequirementOperations";
            string where = "";
            var wipEntityIds = listContainerDeliveryUpdate.Select(P => P.wipEntityId).Distinct();
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wipEntityId in(" + string.Join(",", wipEntityIds) + ")");
            var materialIds = listContainerDeliveryUpdate.Select(P => P.materialId).Distinct();
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "inventoryItemId in(" + string.Join(",", materialIds) + ")");
            hql += " where " + where;
            List<AscmWipRequirementOperations> listBom = AscmWipRequirementOperationsService.GetInstance().GetList(hql);
            if (listBom == null || listBom.Count == 0)
            {
                error = "服务端获取作业BOM失败！";
                return false;
            }
            //获取备料单对应的容器备料表
            hql = "from AscmWmsContainerDelivery where preparationMainId=" + preparationMain.id;
            List<AscmWmsContainerDelivery> listContainerDelivery = AscmWmsContainerDeliveryService.GetInstance().GetList(hql);

            int containerDeliveryId = 0;
            List<AscmWmsContainerDelivery> listContainerDeliverySaveOrUpdate = new List<AscmWmsContainerDelivery>();
            List<AscmWipRequirementOperations> listBomUpdate = new List<AscmWipRequirementOperations>();
            AscmWmsContainerDeliveryService.GetInstance().SetMaterial(listContainerDeliveryUpdate);
            AscmWmsContainerDeliveryService.GetInstance().SetWipEntities(listContainerDeliveryUpdate);
            var result = listContainerDeliveryUpdate.GroupBy(P => P.wipEntityId);
            foreach (IGrouping<int, AscmWmsContainerDelivery> ig in result)
            {
                var result2 = ig.GroupBy(P => P.materialId);
                foreach (IGrouping<int, AscmWmsContainerDelivery> ig2 in result2)
                {
                    AscmWmsContainerDelivery firstContainerDelivery = ig2.First();
                    decimal prepareQuantity = ig2.Sum(P => P.sendQuantity);
                    //验证作业BOM备料数量
                    var bom = listBom.Find(P => P.wipEntityId == ig.Key && P.inventoryItemId == ig2.Key);
                    if (bom != null)
                    {
                        //作业中物料备料总数=本次作业物料备料数量+作业中物料已备数量
                        decimal preparationTotal = prepareQuantity + bom.ascmPreparedQuantity;
                        if (bom.requiredQuantity < preparationTotal)
                            error = "作业【" + firstContainerDelivery.wipEntityName + "】物料【" + firstContainerDelivery.materialDocNumber + "】的备料数量[" + preparationTotal + "]大于需求数量[" + bom.requiredQuantity + "]";
                    }
                    else
                        error = "作业【" + firstContainerDelivery.wipEntityName + "】物料清单中找不到物料【" + firstContainerDelivery.materialDocNumber + "】";
                    if (!string.IsNullOrEmpty(error))
                        return false;
                    bom.ascmPreparedQuantity += prepareQuantity; //更新作业BOM备料数量
                    listBomUpdate.Add(bom);
                    //容器备料
                    foreach (AscmWmsContainerDelivery containerDelivery in ig2)
                    {
                        AscmWmsContainerDelivery findContainerDelivery = listContainerDelivery.Find(P => P.wipEntityId == ig.Key && P.materialId == ig2.Key && P.containerSn == containerDelivery.containerSn);
                        if (findContainerDelivery == null)
                        {
                            findContainerDelivery = containerDelivery;
                            findContainerDelivery.id = --containerDeliveryId;
                        }
                        else
                        {
                            findContainerDelivery.modifyUser = containerDelivery.modifyUser;
                            findContainerDelivery.quantity += containerDelivery.sendQuantity;
                        }
                        listContainerDeliverySaveOrUpdate.Add(findContainerDelivery);
                    }
                }
            }
            if (containerDeliveryId < 0)
            {
                string maxIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_wms_container_delivery_id", "", "", 10, Math.Abs(containerDeliveryId));
                int maxId = Convert.ToInt32(maxIdKey);
                listContainerDeliverySaveOrUpdate.Where(P => P.id < 0).ToList().ForEach(P => P.id = maxId++);
            }
            //作业备料时要对作业状态进行更新
            List<AscmWipDiscreteJobs> listWipDiscreteJobs = null;
            AscmWipDiscreteJobsStatus jobsStatus = null;
            if (preparationMain.pattern == AscmWmsPreparationMain.PatternDefine.wipJob)
            {
                listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList(wipEntityIds.ToList(), "ascmStatus='" + AscmWipDiscreteJobs.AscmStatusDefine.unPrepare + "'");
                if (listWipDiscreteJobs != null)
                    listWipDiscreteJobs.ForEach(P => P.ascmStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing);

                //新的逻辑，保存到明细状态
                jobsStatus = AscmWipDiscreteJobsService.Instance.Get(preparationMain.wipEntityId, AscmWhTeamUserService.Instance.GetLeaderId(preparationMain.createUser));
                if (jobsStatus != null && jobsStatus.subStatus == AscmWipDiscreteJobs.AscmStatusDefine.unPrepare)
                {
                    jobsStatus.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing;
                }
                else if (jobsStatus != null && jobsStatus.subStatus == AscmWipDiscreteJobs.AscmStatusDefine.unPick)
                {
                    jobsStatus.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing;
                }
                else
                {
                    jobsStatus = null;
                }
            }
            //执行事务处理
            ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
            session.Clear();
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    if (listBomUpdate != null && listBomUpdate.Count > 0)
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listBomUpdate);
                    if (listContainerDeliverySaveOrUpdate.Count > 0)
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdateList(listContainerDeliverySaveOrUpdate);
                    if (listWipDiscreteJobs != null && listWipDiscreteJobs.Count > 0)
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listWipDiscreteJobs);
                    if (jobsStatus != null)
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWipDiscreteJobsStatus>(jobsStatus);

                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    error = ex.Message;
                }
            }
            if (!string.IsNullOrEmpty(error))
                return false;
            //更新备料单状态
            string previousStatus = preparationMain.status;
            PrepareChangeStatus(preparationMain);
            List<AscmWmsPreparationMain> listPreparationMainUpdate = new List<AscmWmsPreparationMain>();
            if (previousStatus != preparationMain.status)
                listPreparationMainUpdate.Add(preparationMain);
            //其他备料单
            string whereOther = "";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "id<>" + preparationMain.id);
            if (preparationMain.pattern == AscmWmsPreparationMain.PatternDefine.wipJob)
            {
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "pattern='" + AscmWmsPreparationMain.PatternDefine.wipJob + "'");
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "wipEntityId=" + preparationMain.wipEntityId);
            }
            else if (preparationMain.pattern == AscmWmsPreparationMain.PatternDefine.wipRequire)
            {
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "id in(select mainId from AscmWmsPreparationDetail where wipEntityId in(" + string.Join(",", wipEntityIds) + "))");
            }
            hql = "from AscmWmsPreparationMain where " + whereOther;
            List<AscmWmsPreparationMain> listPreparationMain = GetList(hql);
            if (listPreparationMain != null && listPreparationMain.Count > 0)
            {
                foreach (AscmWmsPreparationMain _preparationMain in listPreparationMain)
                {
                    //只有当其它备料单状态更新为“已备齐”时，才更新
                    PrepareChangeStatus(_preparationMain);
                    if (_preparationMain.status == AscmWmsPreparationMain.StatusDefine.prepared)
                        listPreparationMainUpdate.Add(_preparationMain);
                }
            }
            if (listPreparationMainUpdate.Count > 0)
                Update(listPreparationMainUpdate, preparationMain.pattern == AscmWmsPreparationMain.PatternDefine.wipJob);
            return true;
        }

        /// <summary>PC端备料</summary>
        public bool DoWebPreparation(AscmWmsPreparationMain preparationMain, List<AscmWmsPreparationDetail> listPreparationDetail, ref string error, bool doMerge = false)
        {
            error = string.Empty;
            if (preparationMain == null || listPreparationDetail == null || listPreparationDetail.Count == 0)
                return false;
            //获取作业BOM
            string where = "";
            var wipEntityIds = listPreparationDetail.Select(P => P.wipEntityId).Distinct();
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wipEntityId in(" + string.Join(",", wipEntityIds) + ")");
            var materialIds = listPreparationDetail.Select(P => P.materialId).Distinct();
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "inventoryItemId in(" + string.Join(",", materialIds) + ")");
            string hql = "from AscmWipRequirementOperations";
            if (!string.IsNullOrEmpty(where))
                hql += " where " + where;
            List<AscmWipRequirementOperations> listBom = AscmWipRequirementOperationsService.GetInstance().GetList(hql);
            if (listBom == null || listBom.Count == 0)
                return false;
            //获取备料单对应的容器备料表
            hql = "from AscmWmsContainerDelivery where preparationMainId=" + preparationMain.id;
            List<AscmWmsContainerDelivery> listContainerDelivery = AscmWmsContainerDeliveryService.GetInstance().GetList(hql);

            List<AscmWipRequirementOperations> listBomUpdate = new List<AscmWipRequirementOperations>();
            List<AscmWmsContainerDelivery> listContainerDeliverySaveOrUpdate = new List<AscmWmsContainerDelivery>();
            int containerDeliveryId = 0;
            foreach (AscmWmsPreparationDetail preparationDetail in listPreparationDetail)
            {
                if (preparationDetail.prepareQuantity == 0)
                    continue;
                //验证作业BOM备料数量
                var bom = listBom.Find(P => P.wipEntityId == preparationDetail.wipEntityId && P.inventoryItemId == preparationDetail.materialId);
                if (bom != null)
                {
                    //作业中物料备料总数=本次作业物料备料数量+作业中物料已备数量
                    decimal preparationTotal = preparationDetail.prepareQuantity + bom.ascmPreparedQuantity;
                    if (bom.requiredQuantity < preparationTotal)
                        error = "作业【" + preparationDetail.wipEntityName + "】物料【" + preparationDetail.materialDocNumber + "】的备料数量[" + preparationTotal + "]大于需求数量[" + bom.requiredQuantity + "]";
                }
                else
                    error = "作业【" + preparationDetail.wipEntityName + "】物料清单中找不到物料【" + preparationDetail.materialDocNumber + "】";
                if (!string.IsNullOrEmpty(error))
                    return false;
                bom.ascmPreparedQuantity += preparationDetail.prepareQuantity; //更新作业BOM备料数量
                listBomUpdate.Add(bom);
                //容器备料
                AscmWmsContainerDelivery containerDelivery = null;
                if (listContainerDelivery != null)
                    containerDelivery = listContainerDelivery.Find(P => P.wipEntityId == preparationDetail.wipEntityId && P.materialId == preparationDetail.materialId);
                if (containerDelivery == null)
                {
                    containerDelivery = new AscmWmsContainerDelivery();
                    containerDelivery.id = --containerDeliveryId;
                    containerDelivery.createUser = preparationDetail.modifyUser;
                    containerDelivery.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    containerDelivery.wipEntityId = preparationDetail.wipEntityId;
                    containerDelivery.materialId = preparationDetail.materialId;
                    containerDelivery.preparationMainId = preparationMain.id;
                }
                containerDelivery.modifyUser = preparationDetail.modifyUser;
                containerDelivery.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                containerDelivery.quantity += preparationDetail.prepareQuantity;
                listContainerDeliverySaveOrUpdate.Add(containerDelivery);
            }
            if (containerDeliveryId < 0)
            {
                string maxIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_wms_container_delivery_id", "", "", 10, Math.Abs(containerDeliveryId));
                int maxId = Convert.ToInt32(maxIdKey);
                listContainerDeliverySaveOrUpdate.Where(P => P.id < 0).ToList().ForEach(P => P.id = maxId++);
            }
            //作业备料时要对作业状态进行更新
            List<AscmWipDiscreteJobs> listWipDiscreteJobs = null;
            AscmWipDiscreteJobsStatus jobsStatus = null;
            if (preparationMain.pattern == AscmWmsPreparationMain.PatternDefine.wipJob)
            {
                //旧的逻辑
                listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList(wipEntityIds.ToList(), "ascmStatus='" + AscmWipDiscreteJobs.AscmStatusDefine.unPrepare + "'");
                if (listWipDiscreteJobs != null)
                    listWipDiscreteJobs.ForEach(P => P.ascmStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing);

                //新的逻辑，保存到明细状态
                jobsStatus = AscmWipDiscreteJobsService.Instance.Get(preparationMain.wipEntityId, AscmWhTeamUserService.Instance.GetLeaderId(preparationMain.createUser));
                if (jobsStatus != null && jobsStatus.subStatus == AscmWipDiscreteJobs.AscmStatusDefine.unPrepare)
                {
                    jobsStatus.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing;
                }
                else if (jobsStatus != null && jobsStatus.subStatus == AscmWipDiscreteJobs.AscmStatusDefine.unPick) 
                {
                    jobsStatus.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing;
                }
                else if (jobsStatus != null && jobsStatus.subStatus == AscmWipDiscreteJobs.AscmStatusDefine.picked)
                {
                    jobsStatus.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing;
                }
                else
                {
                    jobsStatus = null;
                }
            }
            //执行事务处理
            ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
            session.Clear();
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    if (doMerge)
                        listPreparationDetail.ForEach(P => session.Merge(P));
                    else
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listPreparationDetail);
                    if (listBomUpdate != null && listBomUpdate.Count > 0)
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listBomUpdate);
                    if (listContainerDeliverySaveOrUpdate.Count > 0)
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdateList(listContainerDeliverySaveOrUpdate);
                    if (listWipDiscreteJobs != null && listWipDiscreteJobs.Count > 0)
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listWipDiscreteJobs);
                    if (jobsStatus != null)
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWipDiscreteJobsStatus>(jobsStatus);

                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    error = ex.Message;
                }
            }
            if (!string.IsNullOrEmpty(error))
                return false;
            //更新备料单状态
            string previousStatus = preparationMain.status;
            PrepareChangeStatus(preparationMain);
            List<AscmWmsPreparationMain> listPreparationMainUpdate = new List<AscmWmsPreparationMain>();
            if (previousStatus != preparationMain.status)
                listPreparationMainUpdate.Add(preparationMain);
            //其他备料单
            string whereOther = "";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "id<>" + preparationMain.id);
            if (preparationMain.pattern == AscmWmsPreparationMain.PatternDefine.wipJob)
            {
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "pattern='" + AscmWmsPreparationMain.PatternDefine.wipJob + "'");
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "wipEntityId=" + preparationMain.wipEntityId);
            }
            else if (preparationMain.pattern == AscmWmsPreparationMain.PatternDefine.wipRequire)
            {
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "id in(select mainId from AscmWmsPreparationDetail where wipEntityId in(" + string.Join(",", wipEntityIds) + "))");
            }
            hql = "from AscmWmsPreparationMain where " + whereOther;
            List<AscmWmsPreparationMain> listPreparationMain = GetList(hql);
            if (listPreparationMain != null && listPreparationMain.Count > 0)
            {
                foreach (AscmWmsPreparationMain _preparationMain in listPreparationMain)
                {
                    //只有当其它备料单状态更新为“已备齐”时，才更新
                    PrepareChangeStatus(_preparationMain);
                    if (_preparationMain.status == AscmWmsPreparationMain.StatusDefine.prepared)
                        listPreparationMainUpdate.Add(_preparationMain);
                }
            }
            if (listPreparationMainUpdate.Count > 0)
                Update(listPreparationMainUpdate, preparationMain.pattern == AscmWmsPreparationMain.PatternDefine.wipJob);
            return true;
        }
        /// <summary>备料时更新备料单状态</summary>
        public void PrepareChangeStatus(AscmWmsPreparationMain preparationMain)
        {
            if (preparationMain.status == AscmWmsPreparationMain.StatusDefine.unPrepare || preparationMain.status == AscmWmsPreparationMain.StatusDefine.preparingUnConfirm || preparationMain.status == AscmWmsPreparationMain.StatusDefine.preparing)
            {
                List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetBomBindNumberList(preparationMain.id);
                if (listPreparationDetail != null && listPreparationDetail.Exists(P => P.planQuantity > P.containerBindNumber))
                    preparationMain.status = AscmWmsPreparationMain.StatusDefine.preparingUnConfirm;
                else
                    preparationMain.status = AscmWmsPreparationMain.StatusDefine.prepared;
            }
        }
        #endregion
    }
}
