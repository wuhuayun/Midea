using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.GetMaterialManage.Entities;
using NHibernate;
using MideaAscm.Dal;
using YnBaseDal;
using System.Collections;
using YnFrame.Dal.Entities;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Services.Base;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Services.FromErp;
using MideaAscm.Dal.IEntity;
using MideaAscm.Services.IEntity;

namespace MideaAscm.Services.GetMaterialManage
{
    public class AscmGetMaterialTaskService
    {
        private static AscmGetMaterialTaskService service;
        public static AscmGetMaterialTaskService GetInstance()
        {
            if (service == null)
                service = new AscmGetMaterialTaskService();
            return service;
        }

        public AscmGetMaterialTask Get(int id)
        {
            AscmGetMaterialTask ascmGetMaterialTask = null;
            try
            {
                ascmGetMaterialTask = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmGetMaterialTask>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmGetMaterialTask)", ex);
                throw ex;
            }
            return ascmGetMaterialTask;
        }

        public List<AscmGetMaterialTask> GetList(string sql, bool isSetRanker, bool isSetWorker, bool isSetRelatedTask, bool isSetRelatedJob)
        {
            List<AscmGetMaterialTask> list = null;
            try
            {
                IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                    if (isSetRanker)
                        SetRanker(list);
                    if (isSetWorker)
                        SetWorker(list);
                    if (isSetRelatedTask)
                        SetRelatedTask(list);
                    if (isSetRelatedJob)
                        SetRelatedJob(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmGetMaterialTask)", ex);
                throw ex;
            }
            return list;
        }

        public List<AscmGetMaterialTask> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmGetMaterialTask> list = null;
            try
            {
                string sort = " order by logisticsClass,status,IdentificationId,taskTime,warehouserId,mtlCategoryStatus,productLine ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortName))
                        sort += sortOrder.Trim();
                }
                string sql = " from AscmGetMaterialTask";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " (jobId like '%" + queryWord.Trim() + "%')";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmGetMaterialTask> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql + sort);

                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmGetMaterialTask)", ex);
                throw ex;
            }
            return list;
        }

        // 领料通知@2014-03-25
        public string GetNotifierMessageList(string userName)
        {
            StringBuilder sb = new StringBuilder();
            string hql = "from AscmGetMaterialTask";
            string hql_Param = "select taskId from AscmWipRequirementOperations where taskId > 0 and wmsPreparationQuantity > 0";

            try
            {
                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
                string userLogistisClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName);

                string where = "", whereQueryWord = "";

                whereQueryWord = "status not in ('FINISH', 'CLOSE')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);


                if (!string.IsNullOrEmpty(userLogistisClass) && (userRole.IndexOf("班长") > -1 || userRole.IndexOf("组长") > -1))
                {
                    whereQueryWord = "logisticsClass in (" + userLogistisClass + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else if (!string.IsNullOrEmpty(userLogistisClass) && userRole == "领料员")
                {
                    whereQueryWord = "workerId = '" + userName + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                whereQueryWord = "id in ({0})";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(where))
                    hql += " where " + where + " order by uploadDate,taskTime,warehouserId,taskId";

                hql = string.Format(hql, hql_Param);

                IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(hql);
                if (ilist != null && ilist.Count > 0)
                {
                    List <AscmGetMaterialTask> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                    SumQuantity(list);
                    list = list.OrderByDescending(e => e.totalWmsPreparationQuantity > 0).ToList();
                    int count = 0;
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                    {
                        if (count < 2)
                        {
                            if (!string.IsNullOrEmpty(sb.ToString()))
                                sb.Append(",");
                            string str = "[" + ascmGetMaterialTask.id + "]" + ascmGetMaterialTask.taskIdCn + "[" + ascmGetMaterialTask.uploadDate + "]";
                            sb.Append(str);
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("获取领料通知数据(Get AscmGetMaterialTask)", ex);
                throw ex;
            }

            return sb.ToString();
        }

        // 手动任务管理@2014-03-25
        public List<AscmGetMaterialTask> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther, string queryStartTime, string queryEndTime, string queryStatus)
        {
            List<AscmGetMaterialTask> list = null;

            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortName))
                        sort += sortOrder.Trim();
                }
                string sql = " from AscmGetMaterialTask";

                string where = "", whereQueryWord = "";
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

                if (!string.IsNullOrEmpty(queryStatus))
                {
                    whereQueryWord = "status = '" + queryStatus + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                whereQueryWord = "taskId like '%L%'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                if (!string.IsNullOrEmpty(sort))
                    sql += sort;

                IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql, sql, ynPage);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                    SetRelatedMark(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmGetMaterialTask)", ex);
                throw ex;
            }

            return list;
        }

        public void Save(List<AscmGetMaterialTask> listAscmGetMaterialTask)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmGetMaterialTask);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmGetMaterialTask)", ex);
                throw ex;
            }
        }

        public void Save(AscmGetMaterialTask ascmGetMaterialTask)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmGetMaterialTask);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmGetMaterialTask)", ex);
                throw ex;
            }
        }

        public void Update(AscmGetMaterialTask ascmGetMaterialTask)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmGetMaterialTask>(ascmGetMaterialTask);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmGetMaterialTask)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmGetMaterialTask)", ex);
                throw ex;
            }
        }

        public void Update(List<AscmGetMaterialTask> listAscmGetMaterialTask)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmGetMaterialTask);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("更新失败(Update AscmGetMaterialTask)", ex);
                    throw ex;
                }
            }
        }

        public void Delete(int id)
        {
            try
            {
                AscmGetMaterialTask ascmGetMaterialTask = Get(id);
                Delete(ascmGetMaterialTask);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(AscmGetMaterialTask ascmGetMaterialTask)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmGetMaterialTask>(ascmGetMaterialTask);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmGetMaterialTask)", ex);
                throw ex;
            }
        }

        public void Delete(List<AscmGetMaterialTask> listAscmGetMaterialTask)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmGetMaterialTask);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmGetMaterialTask)", ex);
            }
        }

        /// <summary>
        /// 设置排产员
        /// </summary>
        /// <param name="list"></param>
        public void SetRanker(List<AscmGetMaterialTask> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'" + ascmGetMaterialTask.rankerId + "'";
                }
                string sql = "from YnUser";
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "userId");
                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    IList<YnUser> ilistYnUser = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.Find<YnUser>(sql);
                    if (ilistYnUser != null && ilistYnUser.Count > 0)
                    {
                        List<YnUser> listYnUser = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnUser>(ilistYnUser);
                        foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                        {
                            ascmGetMaterialTask.ascmRanker = listYnUser.Find(e => e.userId == ascmGetMaterialTask.rankerId);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置责任人
        /// </summary>
        /// <param name="list"></param>
        public void SetWorker(List<AscmGetMaterialTask> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'" + ascmGetMaterialTask.workerId + "'";
                }
                string sql = "from YnUser";
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "userId");
                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    IList<YnUser> ilistYnUser = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.Find<YnUser>(sql);
                    if (ilistYnUser != null && ilistYnUser.Count > 0)
                    {
                        List<YnUser> listYnUser = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnUser>(ilistYnUser);
                        foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                        {
                            if (!string.IsNullOrEmpty(ascmGetMaterialTask.workerId))
                                ascmGetMaterialTask.ascmWorker = listYnUser.Find(e => e.userId == ascmGetMaterialTask.workerId);
                        }
                    }
                }
            }
        }

        public void SetRelatedMark(List<AscmGetMaterialTask> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.relatedMark))
                        ids += ascmGetMaterialTask.relatedMark;
                }
                string sql = "from AscmMarkTaskLog";
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    IList<AscmMarkTaskLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMarkTaskLog>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmMarkTaskLog> listAscmMarkTaskLog = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMarkTaskLog>(ilist);
                        foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                        {
                            if (!string.IsNullOrEmpty(ascmGetMaterialTask.relatedMark))
                                ascmGetMaterialTask.listAscmMarkTaskLog = listAscmMarkTaskLog.FindAll(e => ascmGetMaterialTask.relatedMark.IndexOf(e.id.ToString()) > -1);
                        }
                    }
                }
            }
        }

        public void SetRelatedJob(List<AscmGetMaterialTask> list)
        {
            if (list != null && list.Count > 0)
            {
                SetRelatedMark(list);
                string ids = string.Empty;
                foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                {
                    if (ascmGetMaterialTask.listAscmMarkTaskLog != null && ascmGetMaterialTask.listAscmMarkTaskLog.Count > 0)
                    {
                        foreach (AscmMarkTaskLog ascmMarkTaskLog in ascmGetMaterialTask.listAscmMarkTaskLog)
                        {
                            if (!string.IsNullOrEmpty(ids))
                                ids += ",";
                            ids += ascmMarkTaskLog.wipEntityId;
                        }
                    }
                }
                string sql = "from AscmWipEntities";
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "wipEntityId");

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;

                    IList<AscmWipEntities> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmWipEntities> listAscmWipEntities = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilist);
                        foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                        {
                            if (ascmGetMaterialTask.listAscmMarkTaskLog != null && ascmGetMaterialTask.listAscmMarkTaskLog.Count > 0)
                            {
                                List<AscmWipEntities> listWipEntities = new List<AscmWipEntities>();
                                foreach (AscmMarkTaskLog ascmMarkTaskLog in ascmGetMaterialTask.listAscmMarkTaskLog)
                                {
                                    AscmWipEntities ascmWipEntities = listAscmWipEntities.Find(e => e.wipEntityId == ascmMarkTaskLog.wipEntityId);
                                    listWipEntities.Add(ascmWipEntities);
                                }
                                ascmGetMaterialTask.listAscmWipEntities = listWipEntities;
                            }
                        }
                    }
                }
            }
        }

        public void SetRelatedTask(List<AscmGetMaterialTask> list)
        {
            if (list != null && list.Count > 0)
            {
                SetRelatedMark(list);
                string ids = string.Empty;
                foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                {
                    if (ascmGetMaterialTask.listAscmGetMaterialTask != null && ascmGetMaterialTask.listAscmGetMaterialTask.Count > 0)
                    {
                        foreach (AscmMarkTaskLog ascmMarkTaskLog in ascmGetMaterialTask.listAscmMarkTaskLog)
                        {
                            if (!string.IsNullOrEmpty(ids))
                                ids += ",";
                            ids += ascmMarkTaskLog.taskId;
                        }
                    }
                }
                string sql = "from AscmGetMaterialTask";
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;

                    IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmGetMaterialTask> listAscmGetMaterialTask = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                        foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                        {
                            if (ascmGetMaterialTask.listAscmMarkTaskLog != null && ascmGetMaterialTask.listAscmMarkTaskLog.Count > 0)
                            {
                                List<AscmGetMaterialTask> listGetMaterialTask = new List<AscmGetMaterialTask>();
                                foreach (AscmMarkTaskLog ascmMarkTaskLog in ascmGetMaterialTask.listAscmMarkTaskLog)
                                {
                                    AscmGetMaterialTask getMaterialTask = listAscmGetMaterialTask.Find(e => e.id == ascmMarkTaskLog.taskId);
                                    listGetMaterialTask.Add(getMaterialTask);
                                }
                                ascmGetMaterialTask.listAscmGetMaterialTask = listGetMaterialTask;
                            }
                        }
                    }
                }
            }
        }

        public void SetLogisticsClassName(List<AscmGetMaterialTask> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                {
                    if (!string.IsNullOrEmpty(ids) && !string.IsNullOrEmpty(ascmGetMaterialTask.logisticsClass))
                        ids += ",";
                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.logisticsClass))
                        ids += "'" + ascmGetMaterialTask.logisticsClass + "'";
                }

                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "logisticsClass");
                string sql = "from AscmLogisticsClassInfo";

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmLogisticsClassInfo> ilistAscmLogisticsClassInfo = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLogisticsClassInfo>(sql);
                if (ilistAscmLogisticsClassInfo != null && ilistAscmLogisticsClassInfo.Count > 0)
                {
                    List<AscmLogisticsClassInfo> listAscmLogisticsClassInfo = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmLogisticsClassInfo>(ilistAscmLogisticsClassInfo);
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                    {
                        if (!string.IsNullOrEmpty(ascmGetMaterialTask.logisticsClass))
                            ascmGetMaterialTask.ascmLogisticsClassInfo = listAscmLogisticsClassInfo.Find(e => e.logisticsClass == ascmGetMaterialTask.logisticsClass);
                    }
                }
            }
        }

        public void SetWarehousePlace(List<AscmGetMaterialTask> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                {
                    if (!string.IsNullOrEmpty(ids) && !string.IsNullOrEmpty(ascmGetMaterialTask.warehouserId))
                        ids += ",";
                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.warehouserId))
                        ids += "'" + ascmGetMaterialTask.warehouserId + "'";
                }

                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");
                string sql = "from AscmWarehouse";

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmWarehouse> ilistAscmWarehouse = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarehouse>(sql);
                if (ilistAscmWarehouse != null && ilistAscmWarehouse.Count > 0)
                {
                    List<AscmWarehouse> listAscmWarehouse = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarehouse>(ilistAscmWarehouse);
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                    {
                        if (!string.IsNullOrEmpty(ascmGetMaterialTask.warehouserId))
                            ascmGetMaterialTask.ascmWarehouse = listAscmWarehouse.Find(e => e.id == ascmGetMaterialTask.warehouserId);
                    }
                }
            }
        }

        public void SumQuantity(List<AscmGetMaterialTask> list)
        {
            if (list != null && list.Count > 0)
            {
                
                foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                {
                    string sql = "from AscmWipRequirementOperations";
                    string where = "", whereQueryWord = "";
                    List<AscmWipRequirementOperations> listBom = null;
                    string taskWord = AscmCommonHelperService.GetInstance().GetConfigTaskWords(1);
                    if (ascmGetMaterialTask.taskId.Substring(0,1) != taskWord)
                    {
                        whereQueryWord = "taskId = " + ascmGetMaterialTask.id;
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        if (!string.IsNullOrEmpty(where))
                            sql += " where " + where;
                        IList<AscmWipRequirementOperations> ilistBom = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql);
                        if (ilistBom != null && ilistBom.Count > 0)
                        {
                            listBom = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilistBom);
                            decimal sumRequiredQuantity = 0;
                            decimal sumGetMaterialQuantity = 0;
                            decimal sumPreparationQuantity = 0;
                            foreach (AscmWipRequirementOperations ascmWipRequirementOperations in listBom)
                            {
                                sumRequiredQuantity += ascmWipRequirementOperations.requiredQuantity;
                                sumGetMaterialQuantity += ascmWipRequirementOperations.getMaterialQuantity;
                                sumPreparationQuantity += ascmWipRequirementOperations.wmsPreparationQuantity;
                            }
                            ascmGetMaterialTask.totalRequiredQuantity = sumRequiredQuantity;
                            ascmGetMaterialTask.totalGetMaterialQuantity = sumGetMaterialQuantity;
                            ascmGetMaterialTask.totalWmsPreparationQuantity = sumPreparationQuantity;
                        }
                    }
                }
            }
        }

        public void SetUsedTime(List<AscmGetMaterialTask> list)
        {
            if (list.Count > 0 && list != null)
            {
                foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                {
                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.starTime) && !string.IsNullOrEmpty(ascmGetMaterialTask.endTime))
                    {
                        DateTime starTime = DateTime.Parse(ascmGetMaterialTask.starTime);
                        DateTime endTime = DateTime.Parse(ascmGetMaterialTask.endTime);
                        ascmGetMaterialTask.strUsedTime = endTime.Subtract(starTime).TotalMinutes.ToString() + " 分钟";
                    }
                }
            }
        }

        // 判断是否包含符合条件的任务@2014-3-25
        public int IsContainsMeetTheConditionTask(string warehouseId, int inventoryItemId, int wipEntityId, ref string createTime)
        {
            int taskId = 0;
            AscmMaterialItem ascmMaterialItem = AscmMaterialItemService.GetInstance().Get(inventoryItemId);
            if (ascmMaterialItem == null)
                return taskId;

            AscmWipEntities ascmWipEntities = AscmWipEntitiesService.GetInstance().Get(wipEntityId);
            if (ascmWipEntities == null)
                return taskId;

            List<AscmDiscreteJobs> listAscmDiscreteJobs = AscmDiscreteJobsService.GetInstance().GetList("from AscmDiscreteJobs where jobId = '" + ascmWipEntities.name + "'", true);
            if (listAscmDiscreteJobs == null || listAscmDiscreteJobs.Count == 0)
                return taskId;

            DateTime dateRequired = Convert.ToDateTime(listAscmDiscreteJobs[0].createTime);
            string productLine = listAscmDiscreteJobs[0].productLine;
            int identificationId = listAscmDiscreteJobs[0].identificationId;

            string startTime = dateRequired.ToString("yyyy-MM-dd") + " 00:00";
            string endTime = dateRequired.AddDays(1).ToString("yyyy-MM-dd") + " 00:00";

            IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>("from AscmGetMaterialTask where createTime >= '" + startTime + "' and createTime < '" + endTime + "' and status in ('NOTEXECUTE', 'NOTALLOCATE')");
            if (ilist != null && ilist.Count > 0)
            {
                List<AscmGetMaterialTask> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                
                foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                {
                    if (ascmGetMaterialTask.warehouserId == warehouseId && ascmGetMaterialTask.productLine == productLine && ascmGetMaterialTask.IdentificationId == identificationId)
                    {
                        if (!string.IsNullOrEmpty(ascmGetMaterialTask.mtlCategoryStatus))
                        {
                            if (identificationId == 1)
                            {
                                if (ascmGetMaterialTask.mtlCategoryStatus == ascmMaterialItem.zMtlCategoryStatus)
                                {
                                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.materialDocNumber))
                                    {
                                        if (ascmGetMaterialTask.materialDocNumber == ascmMaterialItem.docNumber.Substring(0,ascmGetMaterialTask.materialDocNumber.Length))
                                        {
                                            taskId = ascmGetMaterialTask.id;
                                        }
                                    }
                                    else
                                    {
                                        taskId = ascmGetMaterialTask.id;
                                    }
                                }
                            }
                            else if (identificationId == 2)
                            {
                                if (ascmGetMaterialTask.mtlCategoryStatus == ascmMaterialItem.dMtlCategoryStatus)
                                {
                                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.materialDocNumber))
                                    {
                                        if (ascmGetMaterialTask.materialDocNumber == ascmMaterialItem.docNumber.Substring(0, ascmGetMaterialTask.materialDocNumber.Length))
                                        {
                                            taskId = ascmGetMaterialTask.id;
                                        }
                                    }
                                    else
                                    {
                                        taskId = ascmGetMaterialTask.id;
                                    }
                                }
                            }
                        }
                        else
                        {
                            taskId = ascmGetMaterialTask.id;
                        }
                    }
                }
            }

            return taskId;
        }

        // 批量领料(支持按作业领料)@2014-4-16
        /// <summary>
        /// 修改于20150325
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="releaseHeaderIds"></param>
        /// <returns></returns>
        public bool BatchGetMaterialTask(string userName, string releaseHeaderIds)
        {
            bool result = false;
            List<AscmWipRequirementOperations> list = new List<AscmWipRequirementOperations>();
            List<WmsAndLogistics> InterfaceList = new List<WmsAndLogistics>();

            try
            {
                string task_ids = string.Empty;
                string[] taskArray = null;
                if (!string.IsNullOrEmpty(releaseHeaderIds))
                {
                    taskArray = releaseHeaderIds.Split(',');
                    foreach (string item in taskArray)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            
                            if (item.IndexOf("[") == -1)
                            {
                                if (item.IndexOf(":") > -1)
                                {
                                    if (list.Count > 0)
                                    {
                                           foreach(AscmWipRequirementOperations ascmWipRequirementOperations  in AscmWipRequirementOperationsService.GetInstance().GetList("", "", "", "", "", item.Split(':')[0], "", item.Split(':')[1]))
                                           {
                                               if(list.Select(p=>p.id).Contains(ascmWipRequirementOperations.id))
                                               {
                                                   continue;
                                               }
                                               else
                                               {

                                                   list.Add(ascmWipRequirementOperations);
                                               }

                                           }
                                    }
                                }
                                else 
                                {
                                    if (list.Count > 0)
                                    {
                                         foreach (AscmWipRequirementOperations ascmWipRequirementOperations in AscmWipRequirementOperationsService.GetInstance().GetList("", "", "", "", "", item, "", ""))
                                        {
                                            if (list.Select(p => p.id).Contains(ascmWipRequirementOperations.id))
                                            {
                                                continue;
                                            }
                                            else
                                            {

                                                list.Add(ascmWipRequirementOperations);
                                            }

                                        }
                                    } 
                                }
                               // task_ids += item;
                                
                                //task_ids += item.Substring(0, item.IndexOf("["));
                            }
                            else
                            {
                                if (!list.Select(p => p.id).Contains(int.Parse(item.Substring(item.IndexOf("[") + 1, item.IndexOf("]") - (item.IndexOf("[") + 1)))))
                                {
                                    if (!string.IsNullOrEmpty(task_ids))
                                        task_ids += ",";
                                    task_ids += item.Substring(item.IndexOf("[") + 1, item.IndexOf("]") - (item.IndexOf("[") + 1));
                                }
                                task_ids += item;
                            }
                        }
                    }
                }
             
                //判断任务是否在执行中
               // bool isExecutable = false;
                if (!string.IsNullOrEmpty(task_ids))
                {
                    IList<AscmWipRequirementOperations> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>("from AscmWipRequirementOperations where id in (" + task_ids + ")");
                    if (ilist != null && ilist.Count > 0)
                    {
                        list.AddRange(ilist);
                    }
                    //string sql = "select count(*) from AscmGetMaterialTask";
                    //string where = "", whereQueryWord = "";

                    //whereQueryWord = "status != '" + AscmGetMaterialTask.StatusDefine.execute + "'";
                    //where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    //whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(task_ids, "id");
                    //where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    //if (!string.IsNullOrEmpty(where))
                    //    sql += " where " + where;
                    //object obj_taskCount = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(sql);
                    //if (obj_taskCount == null)
                    //    throw new Exception("查询任务失败(Find AscmGetMaterialTask)");
                    //if (int.Parse(obj_taskCount.ToString()) == 0)
                    //    isExecutable = true;
                }

                ////任务未在执行中
                //if (!isExecutable)
                //    throw new Exception("选择的所有任务未在执行中！");

                ////领料操作
                bool isCompletion = false;
                //if (isExecutable)
                //{
                //    foreach (string item in taskArray)
                //    {
                //        if (!string.IsNullOrEmpty(item))
                //        {
                //            string sql = "from AscmWipRequirementOperations a, AscmGetMaterialTask b where ";
                //            string where = "", whereQueryWord = "";

                //            if (item.IndexOf("[") > -1)
                //            {
                //                int taskId = int.Parse(item.Substring(0, item.IndexOf("[")));
                //                int wipEntityId = int.Parse(item.Substring(item.IndexOf("[") + 1, item.IndexOf("]") - (item.IndexOf("[") + 1)));

                //                whereQueryWord = "taskId = " + taskId;
                //                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                //                whereQueryWord = "wipEntityId = " + wipEntityId;
                //                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                //                if (!string.IsNullOrEmpty(where))
                //                    sql += " where " + where;

                //                CommonGetMaterialMethod(sql, userName, list, InterfaceList);
                //            }
                //            else
                //            {
                //                int taskId = int.Parse(item);

                //                whereQueryWord = "taskId = " + taskId;
                //                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                //                if (!string.IsNullOrEmpty(where))
                //                    sql += " where " + where;

                //                CommonGetMaterialMethod(sql, userName, list, InterfaceList);
                //            }
                //        }
                //    }
              
                    foreach (AscmWipRequirementOperations ascmWipRequirementOperations in list)
                    {
                        if (ascmWipRequirementOperations.wmsPreparationQuantity > 0)
                        {
                            WmsAndLogistics wmsAndLogistics = new WmsAndLogistics();
                            wmsAndLogistics.wipEntityId = ascmWipRequirementOperations.wipEntityId;
                            wmsAndLogistics.materialId = ascmWipRequirementOperations.inventoryItemId;
                            wmsAndLogistics.quantity = ascmWipRequirementOperations.wmsPreparationQuantity;
                            if (!string.IsNullOrEmpty(ascmWipRequirementOperations.wmsPreparationString))
                            {
                                if (ascmWipRequirementOperations.wmsPreparationString.IndexOf(',') > -1)
                                {
                                    string[] strarr = ascmWipRequirementOperations.wmsPreparationString.Split(',');
                                    foreach (string str in strarr)
                                    {
                                        if (!string.IsNullOrEmpty(str))
                                        {
                                            string temp = str.Substring(0, str.IndexOf('['));
                                            if (!string.IsNullOrEmpty(wmsAndLogistics.preparationString))
                                                wmsAndLogistics.preparationString += ",";
                                            wmsAndLogistics.preparationString += temp;
                                        }
                                    }
                                }
                                else
                                {
                                    string temp = ascmWipRequirementOperations.wmsPreparationString.Substring(0, ascmWipRequirementOperations.wmsPreparationString.IndexOf('['));
                                    wmsAndLogistics.preparationString += temp;
                                }
                            }
                            wmsAndLogistics.workerId = userName;
                            InterfaceList.Add(wmsAndLogistics);

                            ascmWipRequirementOperations.getMaterialQuantity += ascmWipRequirementOperations.wmsPreparationQuantity;
                            if (!string.IsNullOrEmpty(ascmWipRequirementOperations.getMaterialString))
                                ascmWipRequirementOperations.getMaterialString += ",";
                            ascmWipRequirementOperations.getMaterialString += ascmWipRequirementOperations.wmsPreparationQuantity.ToString();
                            ascmWipRequirementOperations.wmsPreparationQuantity = 0;
                            ascmWipRequirementOperations.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmWipRequirementOperations.modifyUser = userName;
                           // list.Add(ascmWipRequirementOperations);
                        }
                    }
                    int d=  list.Select(p => p.id).Distinct().Count();
                    if (list.Count > 0 && InterfaceList.Count > 0)
                    {
                        WmsAndLogisticsService.GetInstance().DoMaterialRequisition(InterfaceList);
                        AscmWipRequirementOperationsService.GetInstance().Update(list);
                        isCompletion = true;
                    }
                

                //调用接口未成功时
                if (!isCompletion)
                    throw new Exception("选择的领料任务未备料！");

                //记录时间节点及自动完成任务
                if (isCompletion)
                {
                    string sql = "from AscmGetMaterialTask id  in (select distinct taskId  from AscmWipRequirementOperations  where id in ("+task_ids+"))";
                   // string where = "", whereQueryWord = "";

                    //whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(task_ids, "id");
                    //where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    //if (!string.IsNullOrEmpty(where))
                    //    sql += " where " + where;
                    IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmGetMaterialTask> listTask = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                        AscmGetMaterialTaskService.GetInstance().SumQuantity(listTask);
                        foreach (AscmGetMaterialTask ascmGetMaterialTask in listTask)
                        {
                            if (!string.IsNullOrEmpty(ascmGetMaterialTask.errorTime))
                                ascmGetMaterialTask.errorTime += ",";
                            ascmGetMaterialTask.errorTime += DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.modifyUser = userName;


                            if (ascmGetMaterialTask.totalQuantityGetMaterialDiff == 0)
                            {
                                ascmGetMaterialTask.status = AscmGetMaterialTask.StatusDefine.finish;
                                if (string.IsNullOrEmpty(ascmGetMaterialTask.endTime))
                                {
                                    ascmGetMaterialTask.endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                                }

                            }
                            else 
                            {
                                if (string.IsNullOrEmpty(ascmGetMaterialTask.starTime))
                                {
                                    ascmGetMaterialTask.starTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                                }

                            }

                        }
                        AscmGetMaterialTaskService.GetInstance().Update(listTask);
                    }
                    result = true;
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error(ex.Message, ex);
                throw ex;
            }

            return result;
        }

        // 公用领料方法@2014-4-16
        public void CommonGetMaterialMethod(string sql, string userName, List<AscmWipRequirementOperations> list, List<WmsAndLogistics> InterfaceList)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                IList<AscmWipRequirementOperations> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql);
                if (ilist != null && ilist.Count > 0)
                {
                    List<AscmWipRequirementOperations> newlist = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilist);
                    foreach (AscmWipRequirementOperations ascmWipRequirementOperations in newlist)
                    {
                        if (ascmWipRequirementOperations.wmsPreparationQuantity > 0)
                        {
                            WmsAndLogistics wmsAndLogistics = new WmsAndLogistics();
                            wmsAndLogistics.wipEntityId = ascmWipRequirementOperations.wipEntityId;
                            wmsAndLogistics.materialId = ascmWipRequirementOperations.inventoryItemId;
                            wmsAndLogistics.quantity = ascmWipRequirementOperations.wmsPreparationQuantity;
                            if (!string.IsNullOrEmpty(ascmWipRequirementOperations.wmsPreparationString))
                            {
                                if (ascmWipRequirementOperations.wmsPreparationString.IndexOf(',') > -1)
                                {
                                    string[] strarr = ascmWipRequirementOperations.wmsPreparationString.Split(',');
                                    foreach (string str in strarr)
                                    {
                                        if (!string.IsNullOrEmpty(str))
                                        {
                                            string temp = str.Substring(0, str.IndexOf('['));
                                            if (!string.IsNullOrEmpty(wmsAndLogistics.preparationString))
                                                wmsAndLogistics.preparationString += ",";
                                            wmsAndLogistics.preparationString += temp;
                                        }
                                    }
                                }
                                else
                                {
                                    string temp = ascmWipRequirementOperations.wmsPreparationString.Substring(0, ascmWipRequirementOperations.wmsPreparationString.IndexOf('['));
                                    wmsAndLogistics.preparationString += temp;
                                }
                            }
                            wmsAndLogistics.workerId = userName;
                            InterfaceList.Add(wmsAndLogistics);

                            ascmWipRequirementOperations.getMaterialQuantity += ascmWipRequirementOperations.wmsPreparationQuantity;
                            if (!string.IsNullOrEmpty(ascmWipRequirementOperations.getMaterialString))
                                ascmWipRequirementOperations.getMaterialString += ",";
                            ascmWipRequirementOperations.getMaterialString += ascmWipRequirementOperations.wmsPreparationQuantity.ToString();
                            ascmWipRequirementOperations.wmsPreparationQuantity = 0;
                            ascmWipRequirementOperations.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmWipRequirementOperations.modifyUser = userName;
                            list.Add(ascmWipRequirementOperations);
                        }
                    }
                }
            }
        }

        // 领料任务监控列表@2014-4-18
        public List<AscmGetMaterialTask> GetMonitorTaskList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string userName, string queryStatus, string queryLine, string queryType, string queryStartDate, string queryEndDate, string taskString, string queryWarehouse, string queryFormat, string queryPerson, string queryStartJobDate, string queryEndJobDate, string queryWipEntity)
        {
            List<AscmGetMaterialTask> list = null;

            try
            {
                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
                string userLogisticsClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName);

                string sql = "from AscmGetMaterialTask";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(userLogisticsClass) && (userRole.IndexOf("物流班长") > -1 || userRole.IndexOf("物流组长") > -1))
                {
                    whereQueryWord = "logisticsClass = '" + userLogisticsClass + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    //string ids_userId = AscmAllocateRuleService.GetInstance().GetLogisticsRankerName(userLogisticsClass, userName);
                    //if (!string.IsNullOrEmpty(ids_userId))
                    //    whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids_userId, "rankerId");
                    //where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else if (!string.IsNullOrEmpty(userLogisticsClass) && userRole == "领料员")
                {
                    whereQueryWord = "workerId = '" + userName + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(userLogisticsClass) && userRole.IndexOf("排产员") > -1)
                {
                    whereQueryWord = "rankerId = '" + userName + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(taskString))
                {
                    whereQueryWord = "status in ('" + AscmGetMaterialTask.StatusDefine.execute + "' , '" + AscmGetMaterialTask.StatusDefine.notExecute + "')";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    if (taskString == "ALL")
                    {
                        List<AscmWipRequirementOperations> listWipRequireOperat = AscmWipRequirementOperationsService.GetInstance().GetList("from AscmWipRequirementOperations where taskId > 0 and wmsPreparationQuantity > 0 order by wmsPreparationQuantity desc");
                        if (listWipRequireOperat != null && listWipRequireOperat.Count > 0)
                        {
                            string ids = string.Empty;
                            foreach (AscmWipRequirementOperations ascmWipRequirementOperations in listWipRequireOperat)
                            {
                                if (!string.IsNullOrEmpty(ids))
                                    ids += ",";
                                ids += ascmWipRequirementOperations.taskId;
                            }

                            whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        }
                    }
                    else
                    {
                        string[] taskArray = taskString.Split(',');
                        string ids = string.Empty;
                        foreach (string str in taskArray)
                        {
                            if (!string.IsNullOrEmpty(ids))
                                ids += ",";
                            ids += str.Substring(str.IndexOf('[') + 1, str.IndexOf(']') - 1);
                        }

                        whereQueryWord = "id in (" + ids + ")";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(queryStartDate) && string.IsNullOrEmpty(queryEndDate) && string.IsNullOrEmpty(queryStartJobDate) && string.IsNullOrEmpty(queryEndJobDate) && string.IsNullOrEmpty(queryWipEntity))
                        throw new Exception("请选择作业起止日期或任务起止日期或作业号！");

                    string hql = "from AscmWipRequirementOperations where wipEntityId = {0}";
                    if (!string.IsNullOrEmpty(queryWipEntity))
                    {
                        hql = string.Format(hql, queryWipEntity);
                        IList<AscmWipRequirementOperations> ilistAscmWipRequirementOperation = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(hql);
                        if (ilistAscmWipRequirementOperation != null && ilistAscmWipRequirementOperation.Count > 0)
                        {
                            List<AscmWipRequirementOperations> listAscmWipRequirementOperation = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilistAscmWipRequirementOperation);
                            string ids = string.Empty;
                            var taskIds = listAscmWipRequirementOperation.Select(P => P.taskId).Distinct();
                            for (int i = 0; i < taskIds.Count(); i++)
                            {
                                if (!string.IsNullOrEmpty(ids) && taskIds.ElementAt(i).HasValue)
                                    ids += ",";
                                ids += taskIds.ElementAt(i);
                            }

                            whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        }
                    }

                    if (!string.IsNullOrEmpty(queryStartDate) && !string.IsNullOrEmpty(queryEndDate))
                    {
                        queryStartDate = queryStartDate + " 00:00:00";
                        queryEndDate = queryEndDate + " 23:59:59";
                        whereQueryWord = "createTime >= '" + queryStartDate + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        whereQueryWord = "createTime <= '" + queryEndDate + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                    else if (!string.IsNullOrEmpty(queryStartDate) && string.IsNullOrEmpty(queryEndDate))
                    {
                        whereQueryWord = "createTime like '" + queryStartDate + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                    else if (string.IsNullOrEmpty(queryStartDate) && !string.IsNullOrEmpty(queryEndDate))
                    {
                        whereQueryWord = "createTime like '" + queryEndDate + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryStartJobDate) && !string.IsNullOrEmpty(queryEndJobDate))
                    {
                        queryStartJobDate = queryStartJobDate + " 00:00:00";
                        queryEndJobDate = queryEndJobDate + " 23:59:59";
                        whereQueryWord = "dateReleased >= '" + queryStartJobDate + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        whereQueryWord = "dateReleased <= '" + queryEndJobDate + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                    else if (!string.IsNullOrEmpty(queryStartJobDate) && string.IsNullOrEmpty(queryEndJobDate))
                    {
                        whereQueryWord = "dateReleased like '" + queryStartJobDate + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                    else if (string.IsNullOrEmpty(queryStartJobDate) && !string.IsNullOrEmpty(queryEndJobDate))
                    {
                        whereQueryWord = "dateReleased like '" + queryEndJobDate + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryStatus))
                    {
                        whereQueryWord = "status = '" + queryStatus + "'";
                    }
                    else
                    {
                        whereQueryWord = "status in ('" + AscmGetMaterialTask.StatusDefine.execute + "','" + AscmGetMaterialTask.StatusDefine.notExecute + "')";
                    }
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    if (!string.IsNullOrEmpty(queryLine))
                    {
                        whereQueryWord = "productline like '" + queryLine + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryType))
                    {
                        whereQueryWord = "IdentificationId =" + queryType;
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryStartDate) && !string.IsNullOrEmpty(queryEndDate))
                    {
                        queryStartDate = queryStartDate + " 00:00:00";
                        queryEndDate = queryEndDate + " 23:59:59";
                        whereQueryWord = "createTime >= '" + queryStartDate + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        whereQueryWord = "createTime <= '" + queryEndDate + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryFormat))
                    {
                        if (queryFormat == "SPECWAREHOUSE")
                        {
                            whereQueryWord = "mtlCategoryStatus is null and taskId like 'T%'";
                        }
                        else if (queryFormat == "TEMPTASK")
                        {
                            whereQueryWord = "mtlCategoryStatus is null and taskId like 'L%'";
                        }
                        else
                        {
                            whereQueryWord = "mtlCategoryStatus = '" + queryFormat + "'";
                        }
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryWarehouse))
                    {
                        string warehouseString = queryWarehouse.Substring(0, 4).ToUpper();
                        whereQueryWord = "warehouserId like '" + warehouseString + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryPerson))
                    {
                        whereQueryWord = "workerId = '" + queryPerson + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                }

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where + " order by status,IdentificationId,taskTime,warehouserId,mtlCategoryStatus,productLine";
                IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                    SetRanker(list);
                    SetWorker(list);
                    SetWarehousePlace(list);
                    SumQuantity(list);
                    //list = list.OrderBy(e => e.totalQuantityPreparationDiff).ToList<AscmGetMaterialTask>().OrderByDescending(e => e.statusInt).ToList<AscmGetMaterialTask>();
                    list = list.OrderByDescending(e => e.statusInt).ToList<AscmGetMaterialTask>();
                    //foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                    //{
                    //    if (ascmGetMaterialTask.mtlCategoryStatus == MtlCategoryStatusDefine.mixStock)
                    //    {
                    //        ascmGetMaterialTask.totalGetMaterialQuantity = 0;
                    //        ascmGetMaterialTask.totalRequiredQuantity = 0;
                    //        ascmGetMaterialTask.totalWmsPreparationQuantity = 0;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error(ex.Message, ex);
                throw ex;
            }

            return list;
        }


        // 领料任务监控列表@2014-4-18
        /// <summary>
        /// 覃小华修改于 2015年3月15
        /// </summary>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="queryWord"></param>
        /// <param name="userName"></param>
        /// <param name="queryStatus"></param>
        /// <param name="queryLine"></param>
        /// <param name="queryType"></param>
        /// <param name="queryStartDate"></param>
        /// <param name="queryEndDate"></param>
        /// <param name="taskString"></param>
        /// <param name="queryWarehouse"></param>
        /// <param name="queryFormat"></param>
        /// <param name="queryPerson"></param>
        /// <param name="queryStartJobDate"></param>
        /// <param name="queryEndJobDate"></param>
        /// <param name="queryWipEntity"></param>
        /// <returns></returns>
        public   string GetMonitorTaskList(string sortName, string sortOrder, string queryWord, string userName, string queryStatus, string queryLine, string queryType, string queryStartDate, string queryEndDate, string taskString, string queryWarehouse, string queryFormat, string queryPerson, string queryStartJobDate, string queryEndJobDate, string queryWipEntity)
        {
            string sql = "select distinct id from AscmGetMaterialTask";
          //  string sqlCount = "select distinct id from ASCM_GETMATERIAL_TASK";

            try
            {
                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
                string userLogisticsClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName);

              
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(userLogisticsClass) && (userRole.IndexOf("物流班长") > -1 || userRole.IndexOf("物流组长") > -1))
                {
                    whereQueryWord = "logisticsClass = '" + userLogisticsClass + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    //string ids_userId = AscmAllocateRuleService.GetInstance().GetLogisticsRankerName(userLogisticsClass, userName);
                    //if (!string.IsNullOrEmpty(ids_userId))
                    //    whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids_userId, "rankerId");
                    //where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else if (!string.IsNullOrEmpty(userLogisticsClass) && userRole == "领料员")
                {
                    whereQueryWord = "workerId = '" + userName + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(userLogisticsClass) && userRole.IndexOf("排产员") > -1)
                {
                    whereQueryWord = "rankerId = '" + userName + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(taskString))
                {
                    whereQueryWord = "status in ('" + AscmGetMaterialTask.StatusDefine.execute + "' , '" + AscmGetMaterialTask.StatusDefine.notExecute + "')";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    if (taskString == "ALL")
                    {
                        List<AscmWipRequirementOperations> listWipRequireOperat = AscmWipRequirementOperationsService.GetInstance().GetList("from AscmWipRequirementOperations where taskId > 0 and wmsPreparationQuantity > 0 order by wmsPreparationQuantity desc");
                        if (listWipRequireOperat != null && listWipRequireOperat.Count > 0)
                        {
                            string ids = string.Empty;
                            foreach (AscmWipRequirementOperations ascmWipRequirementOperations in listWipRequireOperat)
                            {
                                if (!string.IsNullOrEmpty(ids))
                                    ids += ",";
                                ids += ascmWipRequirementOperations.taskId;
                            }

                            whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        }
                    }
                    else
                    {
                        string[] taskArray = taskString.Split(',');
                        string ids = string.Empty;
                        foreach (string str in taskArray)
                        {
                            if (!string.IsNullOrEmpty(ids))
                                ids += ",";
                            ids += str.Substring(str.IndexOf('[') + 1, str.IndexOf(']') - 1);
                        }

                        whereQueryWord = "id in (" + ids + ")";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(queryStartDate) && string.IsNullOrEmpty(queryEndDate) && string.IsNullOrEmpty(queryStartJobDate) && string.IsNullOrEmpty(queryEndJobDate) && string.IsNullOrEmpty(queryWipEntity))
                        throw new Exception("请选择作业起止日期或任务起止日期或作业号！");

                    string hql = "from AscmWipRequirementOperations where wipEntityId = {0}";
                    if (!string.IsNullOrEmpty(queryWipEntity))
                    {
                        hql = string.Format(hql, queryWipEntity);
                        IList<AscmWipRequirementOperations> ilistAscmWipRequirementOperation = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(hql);
                        if (ilistAscmWipRequirementOperation != null && ilistAscmWipRequirementOperation.Count > 0)
                        {
                            List<AscmWipRequirementOperations> listAscmWipRequirementOperation = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilistAscmWipRequirementOperation);
                            string ids = string.Empty;
                            var taskIds = listAscmWipRequirementOperation.Select(P => P.taskId).Distinct();
                            for (int i = 0; i < taskIds.Count(); i++)
                            {
                                if (!string.IsNullOrEmpty(ids) && taskIds.ElementAt(i).HasValue)
                                    ids += ",";
                                ids += taskIds.ElementAt(i);
                            }

                            whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        }
                    }

                    if (!string.IsNullOrEmpty(queryStartDate) && !string.IsNullOrEmpty(queryEndDate))
                    {
                        queryStartDate = queryStartDate + " 00:00:00";
                        queryEndDate = queryEndDate + " 23:59:59";
                        whereQueryWord = "createTime >= '" + queryStartDate + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        whereQueryWord = "createTime <= '" + queryEndDate + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                    else if (!string.IsNullOrEmpty(queryStartDate) && string.IsNullOrEmpty(queryEndDate))
                    {
                        whereQueryWord = "createTime like '" + queryStartDate + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                    else if (string.IsNullOrEmpty(queryStartDate) && !string.IsNullOrEmpty(queryEndDate))
                    {
                        whereQueryWord = "createTime like '" + queryEndDate + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryStartJobDate) && !string.IsNullOrEmpty(queryEndJobDate))
                    {
                        queryStartJobDate = queryStartJobDate + " 00:00:00";
                        queryEndJobDate = queryEndJobDate + " 23:59:59";
                        whereQueryWord = "dateReleased >= '" + queryStartJobDate + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        whereQueryWord = "dateReleased <= '" + queryEndJobDate + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                    else if (!string.IsNullOrEmpty(queryStartJobDate) && string.IsNullOrEmpty(queryEndJobDate))
                    {
                        whereQueryWord = "dateReleased like '" + queryStartJobDate + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                    else if (string.IsNullOrEmpty(queryStartJobDate) && !string.IsNullOrEmpty(queryEndJobDate))
                    {
                        whereQueryWord = "dateReleased like '" + queryEndJobDate + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryStatus))
                    {
                        whereQueryWord = "status = '" + queryStatus + "'";
                    }
                    else
                    {
                        whereQueryWord = "status in ('" + AscmGetMaterialTask.StatusDefine.execute + "','" + AscmGetMaterialTask.StatusDefine.notExecute + "')";
                    }
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    if (!string.IsNullOrEmpty(queryLine))
                    {
                        whereQueryWord = "productline like '" + queryLine + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryType))
                    {
                        whereQueryWord = "IdentificationId =" + queryType;
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryStartDate) && !string.IsNullOrEmpty(queryEndDate))
                    {
                        queryStartDate = queryStartDate + " 00:00:00";
                        queryEndDate = queryEndDate + " 23:59:59";
                        whereQueryWord = "createTime >= '" + queryStartDate + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        whereQueryWord = "createTime <= '" + queryEndDate + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryFormat))
                    {
                        if (queryFormat == "SPECWAREHOUSE")
                        {
                            whereQueryWord = "mtlCategoryStatus is null and taskId like 'T%'";
                        }
                        else if (queryFormat == "TEMPTASK")
                        {
                            whereQueryWord = "mtlCategoryStatus is null and taskId like 'L%'";
                        }
                        else
                        {
                            whereQueryWord = "mtlCategoryStatus = '" + queryFormat + "'";
                        }
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryWarehouse))
                    {
                        string warehouseString = queryWarehouse.Substring(0, 4).ToUpper();
                        whereQueryWord = "warehouserId like '" + warehouseString + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryPerson))
                    {
                        whereQueryWord = "workerId = '" + queryPerson + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                }

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                        //+ " order by status,IdentificationId,taskTime,warehouserId,mtlCategoryStatus,productLine";
               // return sql;
                //IList<object> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<object>(sql);
                //if (ilist != null && ilist.Count > 0)
                //{
                //    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<object>(ilist);
                //    //SetRanker(list);
                //    //SetWorker(list);
                //    //SetWarehousePlace(list);
                //    //SumQuantity(list);
                //    //list = list.OrderBy(e => e.totalQuantityPreparationDiff).ToList<AscmGetMaterialTask>().OrderByDescending(e => e.statusInt).ToList<AscmGetMaterialTask>();
                //    //list = list.OrderByDescending(e => e.statusInt).ToList<AscmGetMaterialTask>();
                //    //foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                //    //{
                //    //    if (ascmGetMaterialTask.mtlCategoryStatus == MtlCategoryStatusDefine.mixStock)
                //    //    {
                //    //        ascmGetMaterialTask.totalGetMaterialQuantity = 0;
                //    //        ascmGetMaterialTask.totalRequiredQuantity = 0;
                //    //        ascmGetMaterialTask.totalWmsPreparationQuantity = 0;
                //    //    }
                //    //}
                //}
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error(ex.Message, ex);
                throw ex;
            }

            return sql;
        }


    }
}
