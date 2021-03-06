﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Services.GetMaterialManage;
using Oracle.DataAccess.Client;
using System.Data;

namespace MideaAscm.Services.FromErp
{
    public class AscmWipDiscreteJobsService
    {
        private static AscmWipDiscreteJobsService ascmWipDiscreteJobsServices;
        public static AscmWipDiscreteJobsService GetInstance()
        {
            if (ascmWipDiscreteJobsServices == null)
                ascmWipDiscreteJobsServices = new AscmWipDiscreteJobsService();
            return ascmWipDiscreteJobsServices;
        }

        public static AscmWipDiscreteJobsService Instance
        {
            get
            {
                return GetInstance();
            }
        }

        public AscmWipDiscreteJobs Get(string id)
        {
            AscmWipDiscreteJobs ascmDeliveryNotify = null;
            try
            {
                int tempId = 0;
                if (!string.IsNullOrEmpty(id))
                {
                    int.TryParse(id, out tempId);
                }


                ascmDeliveryNotify = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWipDiscreteJobs>(tempId);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWipDiscreteJobs)", ex);
                throw ex;
            }
            return ascmDeliveryNotify;
        }
        public List<AscmWipDiscreteJobs> GetList(string sql)
        {
            List<AscmWipDiscreteJobs> list = null;
            try
            {
                IList<AscmWipDiscreteJobs> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobs>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWipDiscreteJobs)", ex);
                throw ex;
            }
            return list;
        }




        public List<AscmWipDiscreteJobs> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord,
            string whereOther, bool isSetWipEntities = true, bool isSetScheduleGroups = false, bool isSetMaterialItem = false, bool isSetLookupValues = false)
        {
            List<AscmWipDiscreteJobs> list = null;
            try
            {
                string sort = " order by wipEntityId ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmWipDiscreteJobs";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                    whereQueryWord = " (description like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }

                IList<AscmWipDiscreteJobs> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobs>(ilist);
                    if (isSetWipEntities)
                        SetWipEntities(list);
                    if (isSetScheduleGroups)
                        SetScheduleGroups(list);
                    if (isSetMaterialItem)
                        SetMaterialItem(list);
                    if (isSetLookupValues)
                        SetLookupValues(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWipDiscreteJobs)", ex);
                throw ex;
            }
            return list;
        }

        public List<AscmWipDiscreteJobs> GetListForMonitor(YnPage ynPage, string sortName, string sortOrder, string queryWord,
                                                string whereOther, bool isSetWipEntities = true, bool isSetScheduleGroups = false,
                                                bool isSetMaterialItem = false, bool isSetLookupValues = false)
        {
            List<AscmWipDiscreteJobs> list = new List<AscmWipDiscreteJobs>();
            try
            {
                StringBuilder strSQL = new StringBuilder();
                strSQL.Append(" SELECT a.*, b.LEADERID, b.LEADERNAME, b.SUBSTATUS,b.ID as STATUSID, b.ISSTOP ");
                strSQL.Append(" FROM ASCM_WIP_DISCRETE_JOBS a  ");
                strSQL.Append(" LEFT JOIN ASCM_WIP_DISCRETE_JOBS_STATUS b on a.wipentityid=b.wipentityid ");
                strSQL.Append(" LEFT JOIN ASCM_WIP_ENTITIES c on a.wipentityid=c.wipentityid ");

                StringBuilder strSQLCount = new StringBuilder();
                strSQLCount.Append(" SELECT count(1) ");
                strSQLCount.Append(" FROM ASCM_WIP_DISCRETE_JOBS a ");
                strSQLCount.Append(" LEFT JOIN ASCM_WIP_DISCRETE_JOBS_STATUS b on a.wipentityid=b.wipentityid ");

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                    whereQueryWord = " (description like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                if (!string.IsNullOrEmpty(where))
                {
                    strSQL.Append(" WHERE " + where);
                    strSQLCount.Append(" WHERE " + where);
                }

                string sort = " ORDER BY c.NAME, a.SCHEDULEDSTARTDATE ";
                strSQL.Append(sort);

                OracleParameter[] commandParameters = new OracleParameter[] {
                            new OracleParameter {
                                ParameterName = "i_sql",
                                OracleDbType = OracleDbType.Varchar2,
                                Size = 2000,
                                Value = strSQL.ToString(),
                                Direction = ParameterDirection.Input
                            },
                            new OracleParameter {
                                ParameterName = "i_sql_count",
                                OracleDbType = OracleDbType.Varchar2,
                                Size = 2000,
                                Value = strSQLCount.ToString(),
                                Direction = ParameterDirection.Input
                            },
                            new OracleParameter {
                                ParameterName = "i_pagesize",
                                OracleDbType = OracleDbType.Int32,
                                Value = ynPage.pageSize,
                                Direction = ParameterDirection.Input
                            },
                            new OracleParameter {
                                ParameterName = "i_currentpage",
                                OracleDbType = OracleDbType.Int32,
                                Value = ynPage.currentPage,
                                Direction = ParameterDirection.Input
                            },
                            new OracleParameter {
                                ParameterName = "o_totalcount",
                                OracleDbType = OracleDbType.Int32,
                                Direction = ParameterDirection.Output
                            },
                            new OracleParameter {
                                ParameterName = "o_pagecount",
                                OracleDbType = OracleDbType.Int32,
                                Direction = ParameterDirection.Output
                            },
                            new OracleParameter {
                                ParameterName = "o_cursor",
                                OracleDbType = OracleDbType.RefCursor,
                                Direction = ParameterDirection.Output
                            }
                        };

                ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                OracleCommand command = (OracleCommand)session.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "cux_ascm_interface_utl.sp_exec_page";
                Array.ForEach<OracleParameter>(commandParameters, P => command.Parameters.Add(P));
                OracleDataAdapter da = new OracleDataAdapter(command);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];

                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AscmWipDiscreteJobs entity = new AscmWipDiscreteJobs();
                        object objValue = null;
                        int intVal = 0;
                        decimal decimalVal = 0;
                        objValue = dr["WIPENTITYID"];
                        if (objValue != null && int.TryParse(objValue.ToString(), out intVal))
                        {
                            entity.wipEntityId = intVal;
                        }

                        objValue = dr["ORGANIZATIONID"];
                        if (objValue != null && int.TryParse(objValue.ToString(), out intVal))
                        {
                            entity.organizationId = intVal;
                        }

                        objValue = dr["CREATEUSER"];
                        if (objValue != null)
                        {
                            entity.createUser = objValue.ToString();
                        }

                        objValue = dr["CREATETIME"];
                        if (objValue != null)
                        {
                            entity.createTime = objValue.ToString();
                        }

                        objValue = dr["MODIFYUSER"];
                        if (objValue != null)
                        {
                            entity.modifyUser = objValue.ToString();
                        }

                        objValue = dr["MODIFYTIME"];
                        if (objValue != null)
                        {
                            entity.modifyTime = objValue.ToString();
                        }

                        objValue = dr["REQUESTID"];
                        if (objValue != null && int.TryParse(objValue.ToString(), out intVal))
                        {
                            entity.requestId = intVal;
                        }

                        objValue = dr["PROGRAMID"];
                        if (objValue != null && int.TryParse(objValue.ToString(), out intVal))
                        {
                            entity.programId = intVal;
                        }

                        objValue = dr["STATUSTYPE"];
                        if (objValue != null && int.TryParse(objValue.ToString(), out intVal))
                        {
                            entity.statusType = intVal;
                        }

                        objValue = dr["PRIMARYITEMID"];
                        if (objValue != null && int.TryParse(objValue.ToString(), out intVal))
                        {
                            entity.primaryItemId = intVal;
                        }

                        objValue = dr["JOBTYPE"];
                        if (objValue != null && int.TryParse(objValue.ToString(), out intVal))
                        {
                            entity.jobType = intVal;
                        }

                        objValue = dr["WIPSUPPLYTYPE"];
                        if (objValue != null && int.TryParse(objValue.ToString(), out intVal))
                        {
                            entity.wipSupplyType = intVal;
                        }

                        objValue = dr["DESCRIPTION"];
                        if (objValue != null)
                        {
                            entity.description = objValue.ToString();
                        }

                        objValue = dr["CLASSCODE"];
                        if (objValue != null)
                        {
                            entity.classCode = objValue.ToString();
                        }

                        objValue = dr["MATERIALACCOUNT"];
                        if (objValue != null && decimal.TryParse(objValue.ToString(), out decimalVal))
                        {
                            entity.materialAccount = decimalVal;
                        }

                        objValue = dr["SCHEDULEDSTARTDATE"];
                        if (objValue != null)
                        {
                            entity.scheduledStartDate = objValue.ToString();
                        }

                        objValue = dr["SCHEDULEDCOMPLETIONDATE"];
                        if (objValue != null)
                        {
                            entity.scheduledCompletionDate = objValue.ToString();
                        }

                        objValue = dr["BOMREVISIONDATE"];
                        if (objValue != null)
                        {
                            entity.bomRevisionDate = objValue.ToString();
                        }

                        objValue = dr["ROUTINGREVISIONDATE"];
                        if (objValue != null)
                        {
                            entity.routingRevisionDate = objValue.ToString();
                        }

                        objValue = dr["SCHEDULEGROUPID"];
                        if (objValue != null && int.TryParse(objValue.ToString(), out intVal))
                        {
                            entity.scheduleGroupId = intVal;
                        }

                        objValue = dr["PRODUCTIONLINE"];
                        if (objValue != null)
                        {
                            entity.productionLine = objValue.ToString();
                        }

                        objValue = dr["NETQUANTITY"];
                        if (objValue != null && decimal.TryParse(objValue.ToString(), out decimalVal))
                        {
                            entity.netQuantity = decimalVal;
                        }

                        objValue = dr["STARTQUANTITY"];
                        if (objValue != null && decimal.TryParse(objValue.ToString(), out decimalVal))
                        {
                            entity.startQuantity = decimalVal;
                        }

                        objValue = dr["QUANTITYCOMPLETED"];
                        if (objValue != null && decimal.TryParse(objValue.ToString(), out decimalVal))
                        {
                            entity.quantityCompleted = decimalVal;
                        }

                        objValue = dr["DATERELEASED"];
                        if (objValue != null)
                        {
                            entity.dateReleased = objValue.ToString();
                        }

                        objValue = dr["DATECLOSED"];
                        if (objValue != null)
                        {
                            entity.dateClosed = objValue.ToString();
                        }

                        objValue = dr["TEMPBINDCONTAINSN"];
                        if (objValue != null)
                        {
                            entity.tempBindContainSn = objValue.ToString();
                        }

                        objValue = dr["ASCMSTATUS"];
                        entity.ascmStatus = "";
                        if (objValue != null)
                        {
                            entity.ascmStatus = objValue.ToString();
                        }

                        objValue = dr["LEADERID"];
                        if (objValue != null)
                        {
                            entity.leaderId = objValue.ToString();
                        }

                        objValue = dr["LEADERNAME"];
                        if (objValue != null)
                        {
                            entity.leaderName = objValue.ToString();
                        }

                        objValue = dr["SUBSTATUS"];
                        entity.subStatus = "";
                        if (objValue != null)
                        {
                            entity.subStatus = objValue.ToString();
                        }

						objValue = dr["STATUSID"];
						if (objValue != null && int.TryParse(objValue.ToString(), out intVal))
						{
							entity.statusId = intVal;
						}

						objValue = dr["ISSTOP"];
						if (objValue != null && objValue.ToString() == "1")
						{
							entity.isStop = true;
						}

                        list.Add(entity);
                    }
                }

                int totalCount = 0;
                int.TryParse(commandParameters[4].Value.ToString(), out totalCount);
                ynPage.SetRecordCount(totalCount);

                int pageCount = 0;
                int.TryParse(commandParameters[5].Value.ToString(), out pageCount);
                ynPage.pageCount = pageCount;

                if (list.Count > 0)
                {
                    if (isSetWipEntities)
                        SetWipEntities(list);
                    if (isSetScheduleGroups)
                        SetScheduleGroups(list);
                    if (isSetMaterialItem)
                        SetMaterialItem(list);
                    if (isSetLookupValues)
                        SetLookupValues(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWipDiscreteJobs)", ex);
                throw ex;
            }

            return list;
        }

        public void Save(List<AscmWipDiscreteJobs> listAscmWipDiscreteJobs)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWipDiscreteJobs);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWipDiscreteJobs)", ex);
                throw ex;
            }
        }

        public void SetLookupValues(List<AscmWipDiscreteJobs> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                {
                    if (ids.IndexOf("'" + ascmWipDiscreteJobs.jobType + "'") < 0)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += "'" + ascmWipDiscreteJobs.jobType + "'";
                    }
                    if (ids.IndexOf("'" + ascmWipDiscreteJobs.statusType + "'") < 0)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += "'" + ascmWipDiscreteJobs.statusType + "'";
                    }
                    if (ids.IndexOf("'" + ascmWipDiscreteJobs.wipSupplyType + "'") < 0)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += "'" + ascmWipDiscreteJobs.wipSupplyType + "'";
                    }
                }
                string sql = "from AscmFndLookupValues where type in ('" + AscmFndLookupValues.AttributeCodeDefine.wipDiscreteJob + "','" + AscmFndLookupValues.AttributeCodeDefine.wipJobStatus + "','" + AscmFndLookupValues.AttributeCodeDefine.wipSupply + "') and code in (" + ids + ")";
                IList<AscmFndLookupValues> ilistAscmFndLookupValues = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmFndLookupValues>(sql);
                if (ilistAscmFndLookupValues != null && ilistAscmFndLookupValues.Count > 0)
                {
                    List<AscmFndLookupValues> listAscmFndLookupValues = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmFndLookupValues>(ilistAscmFndLookupValues);
                    foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                    {
                        ascmWipDiscreteJobs.ascmFndLookupValues_jobType = listAscmFndLookupValues.Find(e => e.type == AscmFndLookupValues.AttributeCodeDefine.wipDiscreteJob && e.code == ascmWipDiscreteJobs.jobType.ToString());
                        ascmWipDiscreteJobs.ascmFndLookupValues_statusType = listAscmFndLookupValues.Find(e => e.type == AscmFndLookupValues.AttributeCodeDefine.wipJobStatus && e.code == ascmWipDiscreteJobs.statusType.ToString());
                        ascmWipDiscreteJobs.ascmFndLookupValues_wipSupplyType = listAscmFndLookupValues.Find(e => e.type == AscmFndLookupValues.AttributeCodeDefine.wipSupply && e.code == ascmWipDiscreteJobs.wipSupplyType.ToString());
                    }
                }
            }
        }
        public void SetScheduleGroups(List<AscmWipDiscreteJobs> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmWipDiscreteJobs.scheduleGroupId + "";
                }
                string sql = "from AscmWipScheduleGroups where scheduleGroupId in (" + ids + ")";
                IList<AscmWipScheduleGroups> ilistAscmWipScheduleGroups = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipScheduleGroups>(sql);
                if (ilistAscmWipScheduleGroups != null && ilistAscmWipScheduleGroups.Count > 0)
                {
                    List<AscmWipScheduleGroups> listAscmWipScheduleGroups = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipScheduleGroups>(ilistAscmWipScheduleGroups);
                    foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                    {
                        ascmWipDiscreteJobs.ascmWipScheduleGroups = listAscmWipScheduleGroups.Find(e => e.scheduleGroupId == ascmWipDiscreteJobs.scheduleGroupId);
                    }
                }
            }
        }
        public void SetMaterialItem(List<AscmWipDiscreteJobs> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                {
                    if (ids.IndexOf("'" + ascmWipDiscreteJobs.primaryItemId + "'") < 0)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += "" + ascmWipDiscreteJobs.primaryItemId + "";
                    }
                }
                string sql = "from AscmMaterialItem where id in (" + ids + ")";
                IList<AscmMaterialItem> ilistAscmMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                if (ilistAscmMaterialItem != null && ilistAscmMaterialItem.Count > 0)
                {
                    List<AscmMaterialItem> listAscmMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistAscmMaterialItem);
                    foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                    {
                        ascmWipDiscreteJobs.ascmMaterialItem = listAscmMaterialItem.Find(e => e.id == ascmWipDiscreteJobs.primaryItemId);
                    }
                }
            }
        }
        public List<AscmFndLookupValues> GetStatusList()
        {
            List<AscmFndLookupValues> list = null;
            try
            {
                string sql = "from AscmFndLookupValues where type='" + AscmFndLookupValues.AttributeCodeDefine.wipJobStatus + "' order by code";
                IList<AscmFndLookupValues> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmFndLookupValues>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmFndLookupValues>(ilist);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public void SetWipEntities(List<AscmWipDiscreteJobs> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmWipDiscreteJobs.wipEntityId + "";
                }
                string sql = "from AscmWipEntities where wipEntityId in (" + ids + ")";
                IList<AscmWipEntities> ilistAscmWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                if (ilistAscmWipEntities != null && ilistAscmWipEntities.Count > 0)
                {
                    List<AscmWipEntities> listAscmWipEntities = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilistAscmWipEntities);
                    foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                    {
                        ascmWipDiscreteJobs.ascmWipEntities = listAscmWipEntities.Find(e => e.wipEntityId == ascmWipDiscreteJobs.wipEntityId);
                    }
                }
            }
        }
        /// <summary>作业产线取排产计划中的数据(前提是ascmWipEntities已赋值)</summary>
        public void SetScheduleProductionLine(List<AscmWipDiscreteJobs> list)
        {
            if (list == null || list.Count == 0)
                return;

            List<AscmDiscreteJobs> listDiscreteJobs = new List<AscmDiscreteJobs>();
            var ieWipEntityName = list.Where(P => !string.IsNullOrWhiteSpace(P.ascmWipEntities_Name)).Select(P => P.ascmWipEntities_Name).Distinct();
            var count = ieWipEntityName.Count();
            string wipEntityNames = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(wipEntityNames))
                    wipEntityNames += ",";
                wipEntityNames += "'" + ieWipEntityName.ElementAt(i) + "'";
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(wipEntityNames))
                    {
                        string hql = "from AscmDiscreteJobs where jobId in (" + wipEntityNames + ")";
                        IList<AscmDiscreteJobs> ilistDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(hql);
                        if (ilistDiscreteJobs != null && ilistDiscreteJobs.Count > 0)
                            listDiscreteJobs.AddRange(ilistDiscreteJobs);
                    }
                }
            }

            if (listDiscreteJobs.Count == 0)
                return;
            foreach (AscmWipDiscreteJobs wipDiscreteJobs in list)
            {
                AscmDiscreteJobs discreteJobs = listDiscreteJobs.Find(P => P.jobId == wipDiscreteJobs.ascmWipEntities_Name);
                if (discreteJobs != null && !string.IsNullOrWhiteSpace(discreteJobs.productLine))
                {
                    wipDiscreteJobs.isScheduled = true;
                    wipDiscreteJobs.productionLine = discreteJobs.productLine;
                }
            }
        }


        /// <summary>
        /// 设置需求数量和领料数量
        /// </summary>
        /// <param name="list"></param>
        public void SumQuantity(List<AscmWipDiscreteJobs> list)
        {
            if (list != null && list.Count > 0)
            {


                foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                {
                    string sql = "select a.wipEntityId, a.requiredQuantity, a.getMaterialQuantity, b.mtlCategoryStatus from AscmWipRequirementOperations a, AscmGetMaterialTask  b where a.taskId=b.id  and a.wipEntityId=" + ascmWipDiscreteJobs.wipEntityId + "  and " + (string.IsNullOrEmpty(ascmWipDiscreteJobs.mtlCategoryStatus) ? "b.mtlCategoryStatus is null" : " b.mtlCategoryStatus ='" + ascmWipDiscreteJobs.mtlCategoryStatus) + "'";
                    // string where = "", whereQueryWord = "";
                    // List<AscmWipRequirementOperations> listBom = null;
                    //   string taskWord = AscmCommonHelperService.GetInstance().GetConfigTaskWords(1);
                    //if (ascmWipDiscreteJobs != taskWord)
                    ////{
                    //   whereQueryWord = "taskId = " + ascmWipDiscreteJobs.wipEntityId;
                    //    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    //    if (!string.IsNullOrEmpty(where))
                    //        sql += " where " + where;
                    IList<object[]> ilistBom = YnDaoHelper.GetInstance().nHibernateHelper.Find<object[]>(sql);
                    if (ilistBom != null && ilistBom.Count > 0)
                    {
                        //listBom = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilistBom);
                        decimal sumRequiredQuantity = 0;
                        decimal sumGetMaterialQuantity = 0;
                        //  decimal sumPreparationQuantity = 0;
                        foreach (object[] ascmWipRequirementOperations in ilistBom)
                        {
                            sumRequiredQuantity += Convert.ToDecimal(ascmWipRequirementOperations[1]);
                            sumGetMaterialQuantity += Convert.ToDecimal(ascmWipRequirementOperations[2]);
                            //sumPreparationQuantity += ascmWipRequirementOperations.wmsPreparationQuantity;
                        }
                        ascmWipDiscreteJobs.totalRequiredQuantity = sumRequiredQuantity;
                        ascmWipDiscreteJobs.totalGetMaterialQuantity = sumGetMaterialQuantity;
                        //ascmWipDiscreteJobs.totalWmsPreparationQuantity = sumPreparationQuantity;
                    }
                }
            }
            //}
        }

        /// <summary>
        /// 设置需作业开始时间和结束时间
        /// </summary>
        /// <param name="list"></param>
        public void starTimeAndEndTime(List<AscmWipDiscreteJobs> list)
        {
            if (list != null && list.Count > 0)
            {


                foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                {
                    string sql = @"select min(b.starTime) from AscmGetMaterialTask  b where b.id in ( select distinct a.taskId  from AscmWipRequirementOperations a  where 
                    a.wipEntityId=" + ascmWipDiscreteJobs.wipEntityId + " and  a.taskId is not null) and " + (string.IsNullOrEmpty(ascmWipDiscreteJobs.mtlCategoryStatus) ? "b.mtlCategoryStatus is null" : " b.mtlCategoryStatus ='" + ascmWipDiscreteJobs.mtlCategoryStatus + "'");
                    object ilistBom = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(sql);
                    if (ilistBom == null)
                    {
                        continue;
                    }
                    if (ilistBom != null)
                    {
                        ascmWipDiscreteJobs.taskStarTime = ilistBom.ToString();
                    }
                    if (ascmWipDiscreteJobs.taskWipState == "已完成")
                    {
                        sql = @"select max(b.endTime) from AscmGetMaterialTask  b where b.id in ( select a.taskId  from AscmWipRequirementOperations a  where 
                       a.wipEntityId=" + ascmWipDiscreteJobs.wipEntityId + " and  a.taskId is not null ) and " + (string.IsNullOrEmpty(ascmWipDiscreteJobs.mtlCategoryStatus) ? "b.mtlCategoryStatus is null" : " b.mtlCategoryStatus ='" + ascmWipDiscreteJobs.mtlCategoryStatus + "'");
                        object endTime = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(sql);
                        ascmWipDiscreteJobs.taskStarTime = endTime == null ? "" : endTime.ToString();

                    }

                }

            }

        }
        //}

        /// <summary>
        /// 2015年3月15日 覃小华
        /// 设置作业单的责任人
        /// </summary>
        /// <param name="list"></param>
        public void SetDutyPeple(List<AscmWipDiscreteJobs> list)
        {
            //IList<object[]> ilistAscmWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<object[]>(sql);
            //if (ilistAscmWipEntities != null && ilistAscmWipEntities.Count > 0)
            //{
            //   // List<AscmWipEntities> listAscmWipEntities = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilistAscmWipEntities);
            //    ids = "";

            //    foreach (object[] ascmWipEntities in ilistAscmWipEntities)
            //    {
            //        if (!string.IsNullOrEmpty(ids))
            //            ids += ",";
            //        ids += "" + ascmWipEntities[0] + "";
            //        if (!dicTaskWipEntityId.Keys.Contains(Convert.ToInt32(ascmWipEntities[1])))
            //        {
            //            dicTaskWipEntityId.Add(Convert.ToInt32(ascmWipEntities[1]), new List<int>());
            //        }
            //        dicTaskWipEntityId[Convert.ToInt32(ascmWipEntities[1])].Add(Convert.ToInt32(ascmWipEntities[0]));
            // Dictionary<int, List<int>> dicTaskWipEntityId = new Dictionary<int, List<int>>();
            //    }
            //if (!string.IsNullOrEmpty(q.ascmWorker_name))
            //    q.ascmWorker_name += "、";
            //      q.ascmWorker_name += "" + ascmWipDiscreteJobs[2] + "";

            if (list != null && list.Count > 0)
            {

                Dictionary<Dictionary<int, string>, string> dicTaskWorkName = new Dictionary<Dictionary<int, string>, string>();
                string ids = string.Empty;
                foreach (int ascmWipDiscreteJobs in list.Select(p => p.wipEntityId).Distinct())
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmWipDiscreteJobs + "";
                }
                //  string sql = "select distinct taskId, wipEntityId from AscmWipRequirementOperations where taskId<>0  and  wipEntityId in (" + ids + ")";
                string sSql = "select distinct c.wipEntityId, a.workerId, b.userName, a.id,  c.wipEntityId, a.mtlCategoryStatus  from AscmGetMaterialTask a, AscmWipRequirementOperations c, AscmUserInfo b where  a.workerId=b.id and  a.id=c.taskId  and  c.wipEntityId in (" + ids + ")";
                IList<object[]> ilistAscmDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<object[]>(sSql);
                if (ilistAscmDiscreteJobs != null && ilistAscmDiscreteJobs.Count > 0)
                {
                    foreach (object[] ascmWipDiscreteJobs in ilistAscmDiscreteJobs)
                    {
                        var q = list.Find(g => g.wipEntityId == Convert.ToInt32(ascmWipDiscreteJobs[0]) && g.mtlCategoryStatus == (ascmWipDiscreteJobs[5] == null ? "" : ascmWipDiscreteJobs[5].ToString()));
                        if (q != null)
                        {
                            if (string.IsNullOrEmpty(q.ascmWorker_name))
                            {
                                q.ascmWorker_name += "" + ascmWipDiscreteJobs[2] + "";

                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(q.ascmWorker_name) && !q.ascmWorker_name.Contains(ascmWipDiscreteJobs[2].ToString()))
                                {
                                    q.ascmWorker_name += "、";
                                    q.ascmWorker_name += "" + ascmWipDiscreteJobs[2] + "";
                                }
                            }
                        }
                    }

                }
            }
        }

        public void SetDiscreteJobs(List<AscmWipDiscreteJobs> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmWipDiscreteJobs.wipEntityId + "";
                }
                string sql = "from AscmWipEntities where wipEntityId in (" + ids + ")";
                IList<AscmWipEntities> ilistAscmWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                if (ilistAscmWipEntities != null && ilistAscmWipEntities.Count > 0)
                {
                    List<AscmWipEntities> listAscmWipEntities = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilistAscmWipEntities);
                    ids = "";
                    foreach (AscmWipEntities ascmWipEntities in listAscmWipEntities)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += "'" + ascmWipEntities.name + "'";
                    }
                    string sSql = "from AscmDiscreteJobs where jobId in (" + ids + ")";
                    IList<AscmDiscreteJobs> ilistAscmDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(sSql);
                    if (ilistAscmDiscreteJobs != null && ilistAscmDiscreteJobs.Count > 0)
                    {
                        List<AscmDiscreteJobs> listAscmDiscreteJobs = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDiscreteJobs>(ilistAscmDiscreteJobs);
                        foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                        {
                            ascmWipDiscreteJobs.ascmWipEntities = listAscmWipEntities.Find(e => e.wipEntityId == ascmWipDiscreteJobs.wipEntityId);
                        }
                        foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                        {
                            ascmWipDiscreteJobs.ascmDiscreteJobs = listAscmDiscreteJobs.Find(e => e.jobId == ascmWipDiscreteJobs.ascmWipEntities.name);
                        }
                    }
                }
            }
        }

        public List<AscmWipDiscreteJobs> GetList(List<int> listWipEntityId, string whereOther = "")
        {
            List<AscmWipDiscreteJobs> list = null;
            if (listWipEntityId != null)
            {
                list = new List<AscmWipDiscreteJobs>();
                var wipEntityIds = listWipEntityId.Distinct();
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
                            string hql = "from AscmWipDiscreteJobs";
                            string where = "";
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wipEntityId in(" + ids + ")");
                            if (!string.IsNullOrEmpty(whereOther))
                                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                            hql += " where " + where;
                            List<AscmWipDiscreteJobs> _list = GetList(hql);
                            if (_list != null && _list.Count > 0)
                                list.AddRange(_list);
                        }
                        ids = string.Empty;
                    }
                }
            }
            return list;
        }
        public bool ChangeAscmStatus(AscmWipDiscreteJobs ascmWipDiscreteJobs, string ascmStatus)
        {
            bool result = false;
            switch (ascmStatus)
            {
                case AscmWipDiscreteJobs.AscmStatusDefine.unPrepare:
                    if (string.IsNullOrEmpty(ascmWipDiscreteJobs.ascmStatus))
                    {
                        ascmWipDiscreteJobs.ascmStatus = AscmWipDiscreteJobs.AscmStatusDefine.unPrepare;
                        result = true;
                    }
                    break;
                case AscmWipDiscreteJobs.AscmStatusDefine.preparing:
                    if (ascmWipDiscreteJobs.ascmStatus == AscmWipDiscreteJobs.AscmStatusDefine.unPrepare
                        || ascmWipDiscreteJobs.ascmStatus == AscmWipDiscreteJobs.AscmStatusDefine.unPick) //根据陈志宇要求，备料状态为“待领料”的作业，如果再次备料时，状态将更改为“备料中”
                    {
                        ascmWipDiscreteJobs.ascmStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing;
                        result = true;
                    }
                    break;
                case AscmWipDiscreteJobs.AscmStatusDefine.unPick:
                    if (ascmWipDiscreteJobs.ascmStatus == AscmWipDiscreteJobs.AscmStatusDefine.preparing)
                    {
                        ascmWipDiscreteJobs.ascmStatus = AscmWipDiscreteJobs.AscmStatusDefine.unPick;
                        result = true;
                    }
                    break;
                case AscmWipDiscreteJobs.AscmStatusDefine.picked:
                    if (ascmWipDiscreteJobs.ascmStatus == AscmWipDiscreteJobs.AscmStatusDefine.unPick)
                    {
                        ascmWipDiscreteJobs.ascmStatus = AscmWipDiscreteJobs.AscmStatusDefine.picked;
                        result = true;
                    }
                    break;
                default: break;
            }
            return result;
        }
        //W112-W412 所有“须配料”，未备完的以物料大类显示
        public void GetLackOfMaterial(List<AscmWipDiscreteJobs> listWipDiscreteJobs)
        {
            if (listWipDiscreteJobs == null || listWipDiscreteJobs.Count == 0)
                return;

            List<AscmWipRequirementOperations> listBom = new List<AscmWipRequirementOperations>();
            var ieWipEntity = listWipDiscreteJobs.Select(P => P.wipEntityId).Distinct();
            var count = ieWipEntity.Count();
            string wipEntityIds = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(wipEntityIds))
                    wipEntityIds += ",";
                wipEntityIds += ieWipEntity.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(wipEntityIds))
                    {
                        string hql = "from AscmWipRequirementOperations";
                        string where = "";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wipEntityId in(" + wipEntityIds + ")");
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "supplySubinventory>='W112材料' and supplySubinventory<='W412材料'");
                        hql += " where " + where;
                        IList<AscmWipRequirementOperations> ilistWipRequirementOperations = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(hql);
                        if (ilistWipRequirementOperations != null && ilistWipRequirementOperations.Count > 0)
                            listBom.AddRange(ilistWipRequirementOperations);
                    }
                }
            }
            if (listBom.Count == 0)
                return;
            AscmWipRequirementOperationsService.GetInstance().SetMaterial(listBom);
            List<AscmWipRequirementOperations> _listBom = listBom.FindAll(P => (P.ascmMaterialItem_dMtlCategoryStatus == MtlCategoryStatusDefine.mixStock || P.ascmMaterialItem_zMtlCategoryStatus == MtlCategoryStatusDefine.mixStock) && P.ascmPreparedQuantity < P.requiredQuantity);
            if (_listBom == null || _listBom.Count == 0)
                return;
            foreach (AscmWipDiscreteJobs wipDiscreteJobs in listWipDiscreteJobs)
            {
                List<AscmWipRequirementOperations> _listBom2 = _listBom.FindAll(P => P.wipEntityId == wipDiscreteJobs.wipEntityId);
                if (_listBom2 != null && _listBom2.Count > 0)
                    wipDiscreteJobs.jobLackOfMaterial = string.Join(",", _listBom2.Select(P => P.ascmMaterialItem_DocNumber.Substring(0, 4)).Distinct().OrderBy(P => P));
            }
        }

        //发料校验：已配料人员、统计容器数、校验数
        public void GetWmsStoreIssueCheck(List<AscmWipDiscreteJobs> listWipDiscreteJobs)
        {
            if (listWipDiscreteJobs == null || listWipDiscreteJobs.Count == 0)
                return;

            List<YnFrame.Dal.Entities.YnUser> ynUserList = null;
            List<int> listWipEntityId = listWipDiscreteJobs.Select(P => P.wipEntityId).ToList();
            List<AscmWhTeamUser> listWhTeamUser = MideaAscm.Services.Warehouse.AscmWhTeamUserService.Instance.GetList();
            listWhTeamUser = listWhTeamUser == null ? new List<AscmWhTeamUser>() : listWhTeamUser;

            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendFormat(" from YnUser ");

            StringBuilder strUserIds = new StringBuilder();
            foreach (var item in listWhTeamUser)
            {
                strUserIds.AppendFormat("'{0}',", item.M_UserId);
            }
            if (strUserIds.Length > 0)
            {
                strUserIds.Remove(strUserIds.Length - 1, 1);
                strSQL.AppendFormat(" where userId in({0}) ", strUserIds);
                ynUserList = YnFrame.Services.YnUserService.GetInstance().GetList(strSQL.ToString());
            }
            ynUserList = ynUserList ?? new List<YnFrame.Dal.Entities.YnUser>();

            List<AscmWmsPreparationMain> listPreparationMain = MideaAscm.Services.Warehouse.AscmWmsPreparationMainService.GetInstance().GetList(listWipEntityId);
            List<AscmWmsContainerDelivery> listContainerDelivery = MideaAscm.Services.Warehouse.AscmWmsContainerDeliveryService.GetInstance().GetListByWipEntity(listWipEntityId);

            if (listPreparationMain == null)
            {
                listPreparationMain = new List<AscmWmsPreparationMain>();
            }

            if (listContainerDelivery != null && listContainerDelivery.Count > 0)
            {
                foreach (AscmWipDiscreteJobs wipDiscreteJobs in listWipDiscreteJobs)
                {
                    //已配料人员是指：备料中_已确认、已备齐的人员姓名
                    List<string> teamUserIds = null;
                    if (listWhTeamUser != null && !string.IsNullOrEmpty(wipDiscreteJobs.leaderId))
                    {
                        var leaderUser = listWhTeamUser.FirstOrDefault(a => a.M_UserId == wipDiscreteJobs.leaderId);
                        if (leaderUser != null)
                        {
                            teamUserIds = listWhTeamUser.Where(a => a.M_TeamId == leaderUser.M_TeamId).Select(a => a.M_UserId).ToList();
                        }
                    }

                    string compoundedPersons = "";
                    if (teamUserIds != null && teamUserIds.Count > 0)
                    {
                        List<AscmWmsPreparationMain> jobPreMainList = listPreparationMain.Where(a => teamUserIds.Contains(a.createUser)
                            && a.wipEntityId == wipDiscreteJobs.wipEntityId
                            && (a.status == AscmWmsPreparationMain.StatusDefine.prepared || a.status == AscmWmsPreparationMain.StatusDefine.preparing
                                || a.status == AscmWmsPreparationMain.StatusDefine.preparingUnPick || a.status == AscmWmsPreparationMain.StatusDefine.preparedUnPick
                                || a.status == AscmWmsPreparationMain.StatusDefine.picked)).ToList();
                        if (jobPreMainList == null) jobPreMainList = new List<AscmWmsPreparationMain>();

                        List<string> jobUserIds = jobPreMainList.Select(a => a.createUser).ToList();
                        if (jobUserIds == null) jobUserIds = new List<string>();

                        List<string> tempUserNameList = ynUserList.Where(a => jobUserIds.Contains(a.userId)).Select(a => a.userName).ToList();
                        if (tempUserNameList != null)
                        {
                            compoundedPersons = string.Join(",", tempUserNameList);
                        }
                    }
                    if (!string.IsNullOrEmpty(compoundedPersons))
                    {
                        wipDiscreteJobs.jobCompoundedPerson = compoundedPersons;
                    }

                    //容器数、校验数
                    List<AscmWmsContainerDelivery> _listContainerDelivery = listContainerDelivery.FindAll(P => P.wipEntityId == wipDiscreteJobs.wipEntityId);
                    if (_listContainerDelivery != null && _listContainerDelivery.Count > 0)
                    {
                        _listContainerDelivery = _listContainerDelivery.FindAll(P => !string.IsNullOrEmpty(P.containerSn));
                        if (_listContainerDelivery != null && _listContainerDelivery.Count > 0)
                        {
                            //容器数
                            wipDiscreteJobs.containerQuantity = _listContainerDelivery.Select(P => P.containerSn).Distinct().Count();
                            //校验数
                            wipDiscreteJobs.checkQuantity = _listContainerDelivery.Where(P => P.status == AscmWmsContainerDelivery.StatusDefine.outWarehouseDoor).Select(P => P.containerSn).Distinct().Count();
                        }

                        //这是针对旧的数据
                        if (string.IsNullOrEmpty(wipDiscreteJobs.leaderId))
                        {
                            wipDiscreteJobs.jobCompoundedPerson = string.Join(",", _listContainerDelivery.Select(P => P.compounder).Distinct());
                        }
                    }
                }
            }
        }



        //public void GetWmsStoreIssueCheck(List<AscmWipDiscreteJobs> listWipDiscreteJobs)
        //{
        //    if (listWipDiscreteJobs == null || listWipDiscreteJobs.Count == 0)
        //        return;

        //    var listWhTeamUser = MideaAscm.Services.Warehouse.AscmWhTeamUserService.Instance.GetList();
        //    List<int> listWipEntityId = listWipDiscreteJobs.Select(P => P.wipEntityId).ToList();
        //    List<AscmWmsContainerDelivery> listContainerDelivery = MideaAscm.Services.Warehouse.AscmWmsContainerDeliveryService.GetInstance().GetListByWipEntity(listWipEntityId);
        //    if (listContainerDelivery != null && listContainerDelivery.Count > 0)
        //    {
        //        foreach (AscmWipDiscreteJobs wipDiscreteJobs in listWipDiscreteJobs)
        //        {
        //            List<AscmWmsContainerDelivery> _listContainerDelivery = listContainerDelivery.FindAll(P => P.wipEntityId == wipDiscreteJobs.wipEntityId);
        //            if (_listContainerDelivery != null && _listContainerDelivery.Count > 0)
        //            {
        //                //已配料人员
        //                //wipDiscreteJobs.leaderId
        //                List<string> teamUserIds = null;
        //                if (listWhTeamUser != null && !string.IsNullOrEmpty(wipDiscreteJobs.leaderId)) 
        //                {
        //                    var leaderUser = listWhTeamUser.FirstOrDefault(a => a.M_UserId == wipDiscreteJobs.leaderId);
        //                    if (leaderUser != null) 
        //                    {
        //                        teamUserIds = listWhTeamUser.Where(a => a.M_TeamId == leaderUser.M_TeamId).Select(a => a.M_UserId).ToList();                      
        //                    }
        //                }

        //                if(teamUserIds!=null && teamUserIds.Count>0)
        //                {
        //                    var _mylistContainerDelivery = _listContainerDelivery.Where(a => teamUserIds.Contains(a.createUser)).ToList();
        //                    if (_mylistContainerDelivery != null && _mylistContainerDelivery.Count > 0) 
        //                    {
        //                        wipDiscreteJobs.jobCompoundedPerson = string.Join(",", _mylistContainerDelivery.Select(P => P.compounder).Distinct());
        //                    }
        //                }
        //                else
        //                {
        //                    wipDiscreteJobs.jobCompoundedPerson = string.Join(",", _listContainerDelivery.Select(P => P.compounder).Distinct());
        //                    //Where(P => P.preparationStatus == AscmWmsPreparationMain.StatusDefine.prepared || P.preparationStatus == AscmWmsPreparationMain.StatusDefine.preparingUnPick || P.preparationStatus == AscmWmsPreparationMain.StatusDefine.preparedUnPick || P.preparationStatus == AscmWmsPreparationMain.StatusDefine.picked).
        //                }

        //                _listContainerDelivery = _listContainerDelivery.FindAll(P => !string.IsNullOrEmpty(P.containerSn));
        //                if (_listContainerDelivery != null && _listContainerDelivery.Count > 0)
        //                {
        //                    //容器数
        //                    wipDiscreteJobs.containerQuantity = _listContainerDelivery.Select(P => P.containerSn).Distinct().Count();
        //                    //校验数
        //                    wipDiscreteJobs.checkQuantity = _listContainerDelivery.Where(P => P.status == AscmWmsContainerDelivery.StatusDefine.outWarehouseDoor).Select(P => P.containerSn).Distinct().Count();
        //                }
        //            }
        //        }
        //    }
        //}



        /// <summary>
        /// 设置标记 id:任务号
        /// </summary>
        /// <param name="id"></param>
        /// <param name="list"></param>
        public void SetMarkTaskLog(string taskIds, List<AscmWipDiscreteJobs> list)
        {
            if (list != null && list.Count > 0)
            {
                string sql = "from AscmMarkTaskLog";
                string where = "", whereQueryWord = "";
                //string ids = string.Empty;
                //foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                //{
                //    if (!string.IsNullOrEmpty(ids))
                //        ids += ",";
                //    ids += ascmWipDiscreteJobs;
                //}
                string wipEntityids = string.Empty;
                foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                {
                    if (!string.IsNullOrEmpty(wipEntityids))
                        wipEntityids += ",";
                    wipEntityids += ascmWipDiscreteJobs.wipEntityId;
                }
                whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(wipEntityids, "wipEntityId");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(taskIds, "wipEntityId");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                whereQueryWord = "isMark = 1";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    IList<AscmMarkTaskLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMarkTaskLog>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmMarkTaskLog> listAscmMarkTaskLog = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMarkTaskLog>(ilist);
                        foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                        {
                            ascmWipDiscreteJobs.ascmMarkTaskLog = listAscmMarkTaskLog.Find(e => e.wipEntityId == ascmWipDiscreteJobs.wipEntityId);
                        }
                    }
                }
            }
        }

        //设置标记 id:任务号
        public void SetMarkTaskLog(int? id, List<AscmWipDiscreteJobs> list)
        {
            if (list != null && list.Count > 0 && id.HasValue)
            {
                string sql = "from AscmMarkTaskLog";
                string where = "", whereQueryWord = "";
                string ids = string.Empty;
                foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWipDiscreteJobs.wipEntityId;
                }
                whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "taskId");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                whereQueryWord = "wipEntityId = " + id.Value;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                whereQueryWord = "isMark = 1";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    IList<AscmMarkTaskLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMarkTaskLog>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmMarkTaskLog> listAscmMarkTaskLog = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMarkTaskLog>(ilist);
                        foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                        {
                            ascmWipDiscreteJobs.ascmMarkTaskLog = listAscmMarkTaskLog.Find(e => e.wipEntityId == ascmWipDiscreteJobs.wipEntityId);
                        }
                    }
                }
            }
        }


        //加载领料任务的作业
        public List<AscmWipDiscreteJobs> GetWipDiscreteJobsSumList(string taskIds)
        {
            List<AscmWipDiscreteJobs> list = null;
            try
            {
                string hql = "select new AscmWipRequirementOperations(a.wipEntityId, sum(a.requiredQuantity), sum(a.quantityIssued), sum(a.getMaterialQuantity), sum(a.wmsPreparationQuantity)) from AscmWipRequirementOperations a, AscmGetMaterialTask b where a.taskId =b.id and a.taskId in (" + taskIds + ")  group by  a.wipEntityId, a.requiredQuantity, a.quantityIssued, a.getMaterialQuantity, b.mtlCategoryStatus";
                IList<AscmWipRequirementOperations> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(hql);
                string ids = string.Empty;
                List<AscmWipRequirementOperations> listAscmWipRequirementOperations = null;
                if (ilist != null && ilist.Count > 0)
                {
                    listAscmWipRequirementOperations = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilist);
                    foreach (AscmWipRequirementOperations ascmWipRequirementOperations in listAscmWipRequirementOperations)
                    {
                        if (!string.IsNullOrEmpty(ids) && ascmWipRequirementOperations.wipEntityId > 0)
                            ids += ",";
                        if (ascmWipRequirementOperations.wipEntityId > 0)
                            ids += ascmWipRequirementOperations.wipEntityId;
                    }
                }

                string whereOther = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(ids))
                {
                    whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "wipEntityId");
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(whereOther))
                {
                    list = AscmWipDiscreteJobsService.GetInstance().GetList(null, "", "", "", whereOther);
                    AscmWipDiscreteJobsService.GetInstance().SetMaterialItem(list);
                    AscmWipDiscreteJobsService.GetInstance().SetDiscreteJobs(list);
                    AscmWipDiscreteJobsService.GetInstance().SetMarkTaskLog(taskIds, list);
                    AscmCommonHelperService.GetInstance().SetTotalSum(listAscmWipRequirementOperations, list);
                    list = list.OrderBy(e => e.wipEntityName).ToList<AscmWipDiscreteJobs>();
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("获取任务作业失败(Get AscmWipDiscreteJobs)", ex);
                throw ex;
            }

            return list;
        }


        //加载领料任务的作业
        /// <summary>
        /// 2015年3月15日重写
        /// 覃小华
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public List<AscmWipDiscreteJobs> GetWipDiscreteJobsSumListI(YnBaseDal.YnPage page, string taskIds)
        {
            List<AscmWipDiscreteJobs> list = new List<AscmWipDiscreteJobs>();
            try
            {
                string hql = "select distinct a.wipEntityId, b.mtlCategoryStatus from AscmWipRequirementOperations a, AscmGetMaterialTask b  where a.taskId=b.id and a.taskId in (" + taskIds + ") ";

                object rowsCount = YnDaoHelper.GetInstance().nHibernateHelper.GetObjectBySQL("select  count(*) from (SELECT  distinct  t.wipentityid, b.mtlcategorystatus FROM ASCM_WIP_REQUIRE_OPERAT T, ASCM_GETMATERIAL_TASK B WHERE T.TASKID = B.ID  AND  B.id in (" + taskIds.Replace("AscmGetMaterialTask", "ASCM_GETMATERIAL_TASK") + "))");
                IList<object[]> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<object[]>(hql, Convert.ToInt32(rowsCount), page);
                string ids = string.Empty;
                //  List<AscmWipRequirementOperations> listAscmWipRequirementOperations = null;
                if (ilist != null && ilist.Count > 0)
                {
                    foreach (object[] wipEntityId in ilist)
                    {
                        list.Add(new AscmWipDiscreteJobs(Convert.ToInt32(wipEntityId[0]), wipEntityId[1] != null ? wipEntityId[1].ToString() : ""));
                        if (!string.IsNullOrEmpty(ids) && Convert.ToInt32(wipEntityId[0]) > 0)
                            ids += ",";
                        if (Convert.ToInt32(wipEntityId[0]) > 0)
                            ids += wipEntityId[0];
                    }
                }

                string whereOther = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(ids))
                {
                    whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "wipEntityId");
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(whereOther))
                {
                    foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in AscmWipDiscreteJobsService.GetInstance().GetList(null, "", "", "", whereOther))
                    {
                        if (list.FindAll(g => g.wipEntityId == ascmWipDiscreteJobs.wipEntityId).Count > 0)
                        {
                            foreach (AscmWipDiscreteJobs ascmWipdiscreteJob in list.FindAll(g => g.wipEntityId == ascmWipDiscreteJobs.wipEntityId))
                            {
                                ascmWipdiscreteJob.organizationId = ascmWipDiscreteJobs.organizationId;
                                ascmWipdiscreteJob.createUser = ascmWipDiscreteJobs.createUser;
                                ascmWipdiscreteJob.createTime = ascmWipDiscreteJobs.createTime;
                                ascmWipdiscreteJob.modifyUser = ascmWipDiscreteJobs.modifyUser;
                                ascmWipdiscreteJob.modifyTime = ascmWipDiscreteJobs.modifyTime;
                                ascmWipdiscreteJob.requestId = ascmWipDiscreteJobs.requestId;
                                ascmWipdiscreteJob.programId = ascmWipDiscreteJobs.programId;
                                ascmWipdiscreteJob.statusType = ascmWipDiscreteJobs.statusType;
                                ascmWipdiscreteJob.primaryItemId = ascmWipDiscreteJobs.primaryItemId;
                                ascmWipdiscreteJob.jobType = ascmWipDiscreteJobs.jobType;
                                ascmWipdiscreteJob.wipSupplyType = ascmWipDiscreteJobs.wipSupplyType;
                                ascmWipdiscreteJob.description = ascmWipDiscreteJobs.description;
                                ascmWipdiscreteJob.classCode = ascmWipDiscreteJobs.classCode;
                                ascmWipdiscreteJob.materialAccount = ascmWipDiscreteJobs.materialAccount;
                                ascmWipdiscreteJob.scheduledStartDate = ascmWipDiscreteJobs.scheduledStartDate;
                                ascmWipdiscreteJob.scheduledCompletionDate = ascmWipDiscreteJobs.scheduledCompletionDate;
                                ascmWipdiscreteJob.bomRevisionDate = ascmWipDiscreteJobs.bomRevisionDate;
                                ascmWipdiscreteJob.routingRevisionDate = ascmWipDiscreteJobs.routingRevisionDate;
                                ascmWipdiscreteJob.scheduleGroupId = ascmWipDiscreteJobs.scheduleGroupId;
                                ascmWipdiscreteJob.productionLine = ascmWipDiscreteJobs.productionLine;
                                ascmWipdiscreteJob.netQuantity = ascmWipDiscreteJobs.netQuantity;
                                ascmWipdiscreteJob.startQuantity = ascmWipDiscreteJobs.startQuantity;
                                ascmWipdiscreteJob.quantityCompleted = ascmWipDiscreteJobs.quantityCompleted;
                                ascmWipdiscreteJob.dateReleased = ascmWipDiscreteJobs.dateReleased;
                                ascmWipdiscreteJob.dateClosed = ascmWipDiscreteJobs.dateClosed;
                                ascmWipdiscreteJob.tempBindContainSn = ascmWipDiscreteJobs.tempBindContainSn;
                                ascmWipdiscreteJob.ascmStatus = ascmWipDiscreteJobs.ascmStatus;

                            }
                        }
                    }
                    AscmWipDiscreteJobsService.GetInstance().SetMaterialItem(list);
                    AscmWipDiscreteJobsService.GetInstance().SetDiscreteJobs(list);
                    AscmWipDiscreteJobsService.GetInstance().SetDutyPeple(list);
                    AscmWipDiscreteJobsService.GetInstance().SetMarkTaskLog(taskIds, list);
                    AscmWipDiscreteJobsService.GetInstance().SumQuantity(list);
                    AscmWipDiscreteJobsService.GetInstance().starTimeAndEndTime(list);
                    //  AscmCommonHelperService.GetInstance().SetTotalSum(listAscmWipRequirementOperations, list);
                    list = list.OrderBy(e => e.wipEntityId).ToList<AscmWipDiscreteJobs>();
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("获取任务作业失败(Get AscmWipDiscreteJobs)", ex);
                throw ex;
            }

            return list;
        }


        /// <summary>
        /// 2015年3月15日重写
        /// </summary>
        /// <param name="ynPage"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="queryWord"></param>
        /// <param name="whereOther"></param>
        /// <param name="isSetWipEntities"></param>
        /// <param name="isSetScheduleGroups"></param>
        /// <param name="isSetMaterialItem"></param>
        /// <param name="isSetLookupValues"></param>
        /// <returns></returns>
        public List<AscmWipDiscreteJobs> GetmtlCategoryStatusList(YnPage ynPage, string sortName, string sortOrder, string queryWord,
           string whereOther)
        {
            List<AscmWipDiscreteJobs> list = null;
            try
            {
                string sort = " order by wipEntityId ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmWipDiscreteJobs";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                    whereQueryWord = " (description like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }

                IList<AscmWipDiscreteJobs> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobs>(ilist);
                    //if (isSetWipEntities)
                    //    SetWipEntities(list);
                    //if (isSetScheduleGroups)
                    //    SetScheduleGroups(list);
                    //if (isSetMaterialItem)
                    //    SetMaterialItem(list);
                    //if (isSetLookupValues)
                    //    SetLookupValues(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWipDiscreteJobs)", ex);
                throw ex;
            }
            return list;
        }

        #region JobsStatus
        public AscmWipDiscreteJobsStatus Get(int id)
        {
            AscmWipDiscreteJobsStatus ascmWipDiscreteJobsStatus = null;
            try
            {
                ascmWipDiscreteJobsStatus = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWipDiscreteJobsStatus>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error(" 查询失败 (Get AscmWipDiscreteJobsStatus) ", ex);
                throw ex;
            }
            return ascmWipDiscreteJobsStatus;
        }

        public List<AscmWipDiscreteJobsStatus> GetListByStrWhere(string strWhere)
        {
            List<AscmWipDiscreteJobsStatus> list = null;
            try
            {
                string sql = " from AscmWipDiscreteJobsStatus ";
                if (!string.IsNullOrEmpty(strWhere)) sql += " where 1=1 " + strWhere;

                IList<AscmWipDiscreteJobsStatus> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobsStatus>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobsStatus>(ilist);
                }

                if (list == null)
                {
                    list = new List<AscmWipDiscreteJobsStatus>();
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWipDiscreteJobsStatus)", ex);
                throw ex;
            }

            return list;
        }
        ////吴华允于2015年7月29日添加
        //public List<AscmWipDiscreteJobs> GetListByStrWhereJobs(string strWhere)
        //{
        //    List<AscmWipDiscreteJobs> list = null;
        //    try
        //    {
        //        string sql = " from AscmWipDiscreteJobs ";
        //        if (!string.IsNullOrEmpty(strWhere)) sql += " where 1=1 " + strWhere;

        //        IList<AscmWipDiscreteJobs> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sql);
        //        if (ilist != null)
        //        {
        //            list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobs>(ilist);
        //        }

        //        if (list == null)
        //        {
        //            list = new List<AscmWipDiscreteJobs>();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWipDiscreteJobs)", ex);
        //        throw ex;
        //    }

        //    return list;
        //}

        public AscmWipDiscreteJobsStatus Get(int wipEntityId, string leaderId)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" AND wipEntityId={0} AND leaderId='{1}' ", wipEntityId, leaderId);
            List<AscmWipDiscreteJobsStatus> list = GetListByStrWhere(strWhere.ToString());
            if (list == null || list.Count == 0)
            {
                return null;
            }

            return list[0];
        }

        public void Save(AscmWipDiscreteJobsStatus ascmWipDiscreteJobsStatus)
        {
            try
            {
                int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmWipDiscreteJobsStatus where wipEntityId=" + ascmWipDiscreteJobsStatus.wipEntityId + " AND leaderId='" + ascmWipDiscreteJobsStatus.leaderId + "'");
                if (count == 0)
                {
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            int id = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId(" select max(id) from AscmWipDiscreteJobsStatus ") + 1;
                            ascmWipDiscreteJobsStatus.id = id;
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWipDiscreteJobsStatus);
                            tx.Commit();        //正确执行提交
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();      //回滚
                            throw ex;
                        }
                    }
                }
                else
                {
                    //throw new Exception("已经存在记录！（AscmWipDiscreteJobsStatus）");
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWipDiscreteJobsStatus)", ex);
                throw ex;
            }
        }

        public void Update(List<AscmWipDiscreteJobsStatus> listStatus)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listStatus);
                        tx.Commit();        //正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();      //回滚
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWipDiscreteJobsStatus)", ex);
                throw ex;
            }
        }

        public void Update(AscmWipDiscreteJobsStatus AscmWipDiscreteJobsStatus)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWipDiscreteJobsStatus>(AscmWipDiscreteJobsStatus);
                    tx.Commit();    //正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();  //回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWipDiscreteJobsStatus)", ex);
                    throw ex;
                }
            }
        }

        public void Delete(int id)
        {
            try
            {
                AscmWipDiscreteJobsStatus AscmWipDiscreteJobsStatus = Get(id);
                Delete(AscmWipDiscreteJobsStatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(AscmWipDiscreteJobsStatus AscmWipDiscreteJobsStatus)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWipDiscreteJobsStatus>(AscmWipDiscreteJobsStatus);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWipDiscreteJobsStatus)", ex);
                throw ex;
            }
        }

        public void Delete(List<AscmWipDiscreteJobsStatus> listAscmWipDiscreteJobsStatus)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWipDiscreteJobsStatus);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWipDiscreteJobsStatus List)", ex);
                throw ex;
            }
        }
        #endregion
    }
}
