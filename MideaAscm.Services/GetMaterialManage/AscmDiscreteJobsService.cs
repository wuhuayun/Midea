using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using MideaAscm.Dal;
using YnBaseDal;
using MideaAscm.Dal.GetMaterialManage.Entities;
using System.Collections;
using YnFrame.Dal.Entities;
using MideaAscm.Services.Base;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Services.GetMaterialManage
{
    public class AscmDiscreteJobsService
    {
        private static AscmDiscreteJobsService service;
        public static AscmDiscreteJobsService GetInstance()
        {
            if (service == null)
                service = new AscmDiscreteJobsService();
            return service;
        }

        public AscmDiscreteJobs Get(int id)
        {
            AscmDiscreteJobs ascmDiscreteJobs = null;
            try
            {
                ascmDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDiscreteJobs>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDiscreteJobs)", ex);
                throw ex;
            }
            return ascmDiscreteJobs;
        }

        public List<AscmDiscreteJobs> GetList(string sql, bool isSetRanker)
        {
            List<AscmDiscreteJobs> list = null;
            try
            {
                IList<AscmDiscreteJobs> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDiscreteJobs>(ilist);
                    if (isSetRanker)
                        SetRanker(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDiscreteJobs)", ex);
                throw ex;
            }
            return list;
        }

        //public List<AscmDiscreteJobs> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        //{
        //    List<AscmDiscreteJobs> list = null;
        //    try
        //    {
        //        string sort = " order by id ";
        //        if (!string.IsNullOrEmpty(sortName))
        //        {
        //            sort = " order by " + sortName.Trim() + " ";
        //            if (!string.IsNullOrEmpty(sortOrder))
        //                sort += sortOrder.Trim();
        //        }
        //        string sql = " from AscmDiscreteJobs ";

        //        string where = "", whereQueryWord = "";
        //        if (!string.IsNullOrEmpty(queryWord))
        //        {
        //            //whereQueryWord = " (jobId like '%" + queryWord.Trim() + "%')";
        //            whereQueryWord = " workerId = '" + queryWord + "'";
        //        }
        //        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
        //        if (!string.IsNullOrEmpty(whereOther))
        //        {
        //            whereOther = "time like '%" + whereOther +"%'";
        //        }
        //        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

        //        if (!string.IsNullOrEmpty(where))
        //            sql += " where " + where + sortOrder;
        //        IList<AscmDiscreteJobs> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(sql,sql,ynPage);
        //        if (ilist != null)
        //        {
        //            list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDiscreteJobs>(ilist);
        //            SetRanker(list);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDiscreteJobs)", ex);
        //        throw ex;
        //    }
        //    return list;
        //}

        public List<AscmDiscreteJobs> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther, bool isSetRanker = true)
        {
            List<AscmDiscreteJobs> list = null;

            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                string sql = " from AscmDiscreteJobs ";
                string where = "", whereQueryWord = "";

                if (!string.IsNullOrEmpty(queryWord))
                {
                    queryWord = queryWord.ToUpper().Trim();
                    whereQueryWord = "jobId like '" +  queryWord + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmDiscreteJobs> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(sql + sort);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDiscreteJobs>(ilist);
                    if (isSetRanker)
                        SetRanker(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDiscreteJobs)", ex);
                throw ex;
            }

            return list;
        }

        //public List<AscmDiscreteJobs> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord)
        //{
        //    List<AscmDiscreteJobs> list = null;
        //    try
        //    {
        //        string sort = "";
        //        if (!string.IsNullOrEmpty(sortName))
        //        {
        //            sort = "order by " + sortName.Trim() + " " ;
        //            if (!string.IsNullOrEmpty(sortOrder))
        //                sort += sortOrder.Trim();
        //        }

        //        string sql = "from AscmDiscreteJobs";
        //        IList<AscmDiscreteJobs> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(sql + sort, sql, ynPage);
        //        if (ilist != null)
        //        {
        //            list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDiscreteJobs>(ilist);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDiscreteJobs)", ex);
        //        throw ex;
        //    }
        //    return list;
        //}

        public List<AscmDiscreteJobs> GetList(YnPage ynpage, string sortName, string sortOrder, string queryWord, string queryDate, string queryType, string queryRanker, string userLogistisClass, string userRole, string userName)
        {
            List<AscmDiscreteJobs> list = null;
            try
            {
                string sort = "", whereQueryWord = "", where = "";
                string sql = "from AscmDiscreteJobs";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = "order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                if (!string.IsNullOrEmpty(queryDate))
                {
                    whereQueryWord = "createTime like '%" + queryDate + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryType))
                {
                    whereQueryWord = "identificationId = " + queryType;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(userLogistisClass))
                {
                    IList<AscmLogisticsClassInfo> ilistAscmLogisticsClassInfo = AscmLogisticsClassInfoService.GetInstance().GetList("from AscmLogisticsClassInfo where logisticsClass in (" + userLogistisClass + ")", false, false);
                    string ids = string.Empty;
                    if (ilistAscmLogisticsClassInfo != null && ilistAscmLogisticsClassInfo.Count > 0)
                    {
                        foreach (AscmLogisticsClassInfo ascmlogisticsClassInfo in ilistAscmLogisticsClassInfo)
                        {
                            if (!string.IsNullOrEmpty(ids))
                                ids += ",";
                            ids += ascmlogisticsClassInfo.id;
                        }
                    }

                    string newCondition = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "logisticsClassId");
                    if (!string.IsNullOrEmpty(newCondition))
                    {
                        string newsql = "from AscmAllocateRule where " + newCondition;
                        IList<AscmAllocateRule> ilistAscmAllocateRule = AscmAllocateRuleService.GetInstance().GetList(newsql, false, false, false);
                        ids = "";
                        if (ilistAscmAllocateRule != null && ilistAscmAllocateRule.Count > 0)
                        {
                            foreach (AscmAllocateRule ascmAllocateRule in ilistAscmAllocateRule)
                            {
                                if (!string.IsNullOrEmpty(ids) && (!string.IsNullOrEmpty(ascmAllocateRule.zRankerName) || !string.IsNullOrEmpty(ascmAllocateRule.dRankerName)))
                                    ids += ",";
                                if (!string.IsNullOrEmpty(ascmAllocateRule.zRankerName))
                                    ids += "'" + ascmAllocateRule.zRankerName + "'";
                                if (!string.IsNullOrEmpty(ids) && !string.IsNullOrEmpty(ascmAllocateRule.dRankerName))
                                    ids += ",";
                                if (!string.IsNullOrEmpty(ascmAllocateRule.dRankerName))
                                    ids += "'" + ascmAllocateRule.dRankerName + "'";
                            }
                        }

                        if (string.IsNullOrEmpty(queryRanker))
                        {
                            whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "workerId");
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        }
                        else
                        {
                            if (ids.IndexOf(queryRanker) > -1)
                            {
                                whereQueryWord = "workerId like '%" + queryRanker + "%'";
                                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                            }
                            else
                            {
                                whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "workerId");
                                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                            }
                        }
                    }
                }
                else
                {
                    if (userRole == "总装排产员" || userRole == "电装排产员")
                    {
                        whereQueryWord = "workerId = '" + userName + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                    else if (userRole == "领料员")
                    {
                        string newsql = "from AscmAllocateRule where workerName = '" + userName + "'";
                        string ids = string.Empty;
                        IList<AscmAllocateRule> ilistAscmAllocateRule = AscmAllocateRuleService.GetInstance().GetList(newsql, false, false, false);
                        if (ilistAscmAllocateRule != null && ilistAscmAllocateRule.Count > 0)
                        {
                            foreach (AscmAllocateRule ascmAllocateRule in ilistAscmAllocateRule)
                            {
                                if (!string.IsNullOrEmpty(ids))
                                    ids += ",";
                                if (!string.IsNullOrEmpty(ascmAllocateRule.zRankerName))
                                    ids += "'" + ascmAllocateRule.zRankerName + "'";
                                if (!string.IsNullOrEmpty(ascmAllocateRule.dRankerName))
                                    ids += "'" + ascmAllocateRule.dRankerName + "'";
                            }
                        }

                        whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "workerId");
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                }

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where + " order by workerId,workerId,productLine,sequence,id";
                    IList<AscmDiscreteJobs> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(sql, sql, ynpage);
                    if (ilist != null && ilist.Count > 0)
                    {
                        list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDiscreteJobs>(ilist);
                        SetRanker(list);
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDiscreteJobs)", ex);
                throw ex;
            }

            return list;
        }

        public List<AscmDiscreteJobs> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string queryStartTime, string queryEndTime, string queryType, string queryRanker, string userName)
        {
            List<AscmDiscreteJobs> list = null;

            try
            {
                string sort = "", whereQueryWord = "", where = "", where_Param = "";
                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
                string userLogisticsName = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName, userRole);

                string hql = "from AscmDiscreteJobs";
                string hql_ParamZ = "select zRankerName from AscmAllocateRule";
                string hql_ParamD = "select dRankerName from AscmAllocateRule";

                string sql = "select * from ASCM_DISCRETE_JOBS";

                if (userRole == "领料员")
                {
                    whereQueryWord = "workerName = '" + userName + "'";
                    where_Param = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where_Param, whereQueryWord);
                }
                else if (userRole == "总装排产员" || userRole == "电装排产员")
                {
                    whereQueryWord = "workerId = '" + userName + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else
                {
                    if (!string.IsNullOrEmpty(userLogisticsName))
                    {
                        IList<AscmLogisticsClassInfo> ilistAscmLogisticsClassInfo = AscmLogisticsClassInfoService.GetInstance().GetList("from AscmLogisticsClassInfo where logisticsClass in (" + userLogisticsName + ")", false, false);
                        string ids = string.Empty;
                        if (ilistAscmLogisticsClassInfo != null && ilistAscmLogisticsClassInfo.Count > 0)
                        {
                            foreach (AscmLogisticsClassInfo ascmlogisticsClassInfo in ilistAscmLogisticsClassInfo)
                            {
                                if (!string.IsNullOrEmpty(ids))
                                    ids += ",";
                                ids += ascmlogisticsClassInfo.id;
                            }
                        }

                        whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "logisticsClassId");
                        where_Param = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where_Param, whereQueryWord);
                    }
                }

                if (!string.IsNullOrEmpty(where_Param))
                {
                    string newCondition = string.Empty;
                    hql_ParamZ += " where " + where_Param;
                    hql_ParamZ = "workerId in (" + hql_ParamZ + ")";
                    newCondition = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(newCondition, hql_ParamZ);

                    hql_ParamD += " where " + where_Param;
                    hql_ParamD = "workerId in (" + hql_ParamD + ")";
                    newCondition = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(newCondition, hql_ParamD);
                    where += "(" + newCondition + ")";
                }

                if (!string.IsNullOrEmpty(queryType))
                {
                    whereQueryWord = "identificationId = " + queryType;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryStartTime) && !string.IsNullOrEmpty(queryEndTime))
                {
                    queryStartTime = queryStartTime + " 00:00:00";
                    queryEndTime = queryEndTime + " 23:59:59";

                    whereQueryWord = "createTime > '" + queryStartTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    whereQueryWord = "createTime <= '" + queryEndTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else if (!string.IsNullOrEmpty(queryStartTime) && string.IsNullOrEmpty(queryEndTime))
                {
                    whereQueryWord = "createTime like '" + queryStartTime + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(queryStartTime) && !string.IsNullOrEmpty(queryEndTime))
                {
                    whereQueryWord = "createTime like '" + queryEndTime + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryRanker))
                {
                    whereQueryWord = "workerId = '" + queryRanker + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(where))
                {
                    hql += " where " + where;
                    sql += " where " + where;
                }

                sql = sql.Replace("AscmAllocateRule", "ASCM_ALLOCATE_RULE");

                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObjectBySQL("select count(*) from (" + sql + ")");
                int count = 0;
                int.TryParse(object1.ToString(), out count);
                IList<AscmDiscreteJobs> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(hql, count, ynPage);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDiscreteJobs>(ilist);
                    SetRanker(list);
                    list = list.OrderBy(e => e.workerId).OrderBy(e => e.productLine).OrderBy(e => e.sequence).OrderBy(e => e.identificationId).OrderBy(e => e.id).ToList();
                }
                
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDiscreteJobs)", ex);
                throw ex;
            }

            return list;
        }

        /// <summary>
        /// 关联作业@2014-04-22
        /// </summary>
        /// <param name="ynPage"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="queryWord"></param>
        /// <param name="queryStartTime"></param>
        /// <param name="queryEndTime"></param>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public List<AscmDiscreteJobs> GetRelatedDiscreteJobs(YnPage ynPage, string sortName, string sortOrder, string queryWord, string queryStartTime, string queryEndTime, int materialId)
        {
            List<AscmDiscreteJobs> list = null;

            try
            {
                //string hql_ParamData = "from AscmWipRequirementOperations where wipentityId in (select t1.wipentityId from AscmWipEntities t1, AscmDiscreteJobs t2 where t1.name = t2.jobId and t2.createTime > '{0}' and t2.createTime <= '{1}') and inventoryItemId = {2}";
                string hql_ParamData = "select new AscmWipRequirementOperations(t1.wipEntityId) from AscmWipRequirementOperations t1, AscmWipEntities t2, AscmDiscreteJobs t3 where t2.name = t3.jobId and t1.wipEntityId = t2.wipEntityId and t3.jobDate >= '{0}' and t3.jobDate <= '{1}' and t1.inventoryItemId = {2}";
                //string hql_ParamData = "select t1.wipEntityId from AscmWipRequirementOperations t1, AscmWipEntities t2, AscmDiscreteJobs t3 where t2.name = t3.jobId and t1.wipentityId = t2.wipentityId and t3.jobDate > '{0}' and t3.jobDate <= '{1}' and t1.inventoryItemId = {2}";
                string hql = "from AscmDiscreteJobs where jobId in ({0})";
                string hql_Param = "select name from AscmWipEntities";

                hql_ParamData = string.Format(hql_ParamData, queryStartTime, queryEndTime, materialId);

                IList <AscmWipRequirementOperations> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(hql_ParamData);
                if (ilist != null && ilist.Count > 0)
                {
                    List<AscmWipRequirementOperations> newlist = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilist);
                    var wipEntityIds = newlist.Select(P => P.wipEntityId).Distinct();
                    var count = wipEntityIds.Count();
                    string ids = string.Empty;
                    for (int i = 0; i < count; i++)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += wipEntityIds.ElementAt(i);
                    }
                    
                    string where = "", whereQueryWord = "";
                    whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "wipentityId");
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    if (!string.IsNullOrEmpty(where))
                    {
                        hql_Param += " where " + where;
                        hql = string.Format(hql, hql_Param);
                    }

                    IList<AscmDiscreteJobs> ilistAscmDiscreteJobs = null;
                    if (ynPage != null)
                        ilistAscmDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(hql, hql, ynPage);
                    else
                        ilistAscmDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(hql);
                    if (ilistAscmDiscreteJobs != null && ilistAscmDiscreteJobs.Count > 0)
                    {
                        list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDiscreteJobs>(ilistAscmDiscreteJobs);
                    }
                }
                
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDiscreteJobs)", ex);
                throw ex;
            }

            return list;
        }

        public void Save(List<AscmDiscreteJobs> listAscmDiscreteJobs)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmDiscreteJobs);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDiscreteJobs)", ex);
                throw ex;
            }
        }

        public void Save(AscmDiscreteJobs ascmDiscreteJobs)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmDiscreteJobs);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDiscreteJobs)", ex);
                throw ex;
            }
        }

        public void Update(List<AscmDiscreteJobs> listAscmDiscreteJobs)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmDiscreteJobs);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("更新失败(Update listAscmDiscreteJobs)", ex);
                throw ex;
            }
        }

        public void Update(AscmDiscreteJobs ascmDiscreteJobs)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmDiscreteJobs>(ascmDiscreteJobs);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmDiscreteJobs)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmDiscreteJobs)", ex);
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                AscmDiscreteJobs ascmDiscreteJobs = Get(id);
                Delete(ascmDiscreteJobs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(AscmDiscreteJobs ascmDiscreteJobs)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmDiscreteJobs>(ascmDiscreteJobs);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmDiscreteJobs)", ex);
                throw ex;
            }
        }

        public void Delete(List<AscmDiscreteJobs> listAscmDiscreteJobs)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmDiscreteJobs);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmDiscreteJobs)", ex);
            }
        }



        public void SetRanker(List<AscmDiscreteJobs> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'" + ascmDiscreteJobs.workerId + "'";
                }
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "userId");
                if (!string.IsNullOrEmpty(where))
                {
                    string sql = "from YnUser where " + where;
                    IList<YnUser> ilist = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.Find<YnUser>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<YnUser> listYnUser = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnUser>(ilist);
                        foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                        {
                            ascmDiscreteJobs.ynUser = listYnUser.Find(e => e.userId == ascmDiscreteJobs.workerId);
                        }
                    }
                }
            }
        }

        public void SetWipEntities(List<AscmDiscreteJobs> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'" + ascmDiscreteJobs.jobId + "'";
                }
                string sql = "from AscmWipEntities";
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "name");

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmWipEntities> ilistAscmWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                if (ilistAscmWipEntities != null && ilistAscmWipEntities.Count > 0)
                {
                    List<AscmWipEntities> listAscmWipEntities = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilistAscmWipEntities);
                    foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                    {
                        ascmDiscreteJobs.ascmWipEntities = listAscmWipEntities.Find(e => e.name == ascmDiscreteJobs.jobId);
                    }
                }
            }
        }

        public List<string> GetLineList()
        {
            List<string> list = new List<string>();
            try
            {
                string sql = "select distinct productline from ascm_discrete_jobs";
                IList ilist = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);

                foreach (object[] obj in ilist)
                {
                    string str = obj[0].ToString();
                    list.Add(str);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("获取生产线(Get Line)", ex);
                throw ex;
            }
            
            return list;
        }
    }
}
