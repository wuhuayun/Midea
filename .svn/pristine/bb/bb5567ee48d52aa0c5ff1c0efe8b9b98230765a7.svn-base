using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.FromErp.Entities;
using Oracle.DataAccess.Client;
using System.Data;

namespace MideaAscm.Services.FromErp
{
    public class AscmCuxWipReleaseHeadersService
    {
        private static AscmCuxWipReleaseHeadersService ascmDeliveryOrderMainServices;
        public static AscmCuxWipReleaseHeadersService GetInstance()
        {
            //return ascmDeliveryOrderMainServices ?? new AscmCuxWipReleaseHeadersService();
            if (ascmDeliveryOrderMainServices == null)
                ascmDeliveryOrderMainServices = new AscmCuxWipReleaseHeadersService();
            return ascmDeliveryOrderMainServices;
        }
        public AscmCuxWipReleaseHeaders Get(int id)
        {
            AscmCuxWipReleaseHeaders ascmCuxWipReleaseHeaders = null;
            try
            {
                ascmCuxWipReleaseHeaders = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmCuxWipReleaseHeaders>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmCuxWipReleaseHeaders)", ex);
                throw ex;
            }
            return ascmCuxWipReleaseHeaders;
        }
        public List<AscmCuxWipReleaseHeaders> GetList(string sql, bool isSetWipEntities = false)
        {
            List<AscmCuxWipReleaseHeaders> list = null;
            try
            {
                IList<AscmCuxWipReleaseHeaders> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmCuxWipReleaseHeaders>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmCuxWipReleaseHeaders>(ilist);
                    if (isSetWipEntities)
                        SetWipEntities(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmCuxWipReleaseHeaders)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmCuxWipReleaseHeaders> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther, string whereWipDiscreteJobs)
        {
            List<AscmCuxWipReleaseHeaders> list = null;
            try
            {
                string whereWipEntityId="";
                if (!string.IsNullOrEmpty(whereWipDiscreteJobs))
                {
                    //List<AscmWipDiscreteJobs> listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList(ynPage, "", "", queryWord, whereWipDiscreteJobs);
                    List<AscmWipDiscreteJobs> listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList(null, "", "", "", whereWipDiscreteJobs,false,false,false,false);
                    int iCount = listWipDiscreteJobs.Count();
                    string ids = string.Empty;
                    for(int i=0;i<iCount;i++)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += listWipDiscreteJobs[i].wipEntityId;
                        if((i+1)%1000==0||i+1==iCount)
                        {
                            if (!string.IsNullOrEmpty(ids))
                            {
                                if (!string.IsNullOrEmpty(whereWipEntityId))
                                    whereWipEntityId += " or ";
                                whereWipEntityId += "wipEntityId in ( " + ids + ")";
                            }
                            ids = string.Empty;
                        }
                    }
                    if(string.IsNullOrEmpty(whereWipEntityId))
                        whereWipEntityId = "wipEntityId in ( -1)";
                    //foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in listWipDiscreteJobs)
                    //{
                    //    if (whereWipEntityId != "")
                    //        whereWipEntityId += ",";
                    //    whereWipEntityId += ascmWipDiscreteJobs.wipEntityId;
                    //}
                    //if (!string.IsNullOrEmpty(whereWipEntityId))
                    //    whereWipEntityId = "wipEntityId in ( " + whereWipEntityId + ")";
                    //else
                    //    whereWipEntityId = "wipEntityId in ( -1)";
                }
                string sort = " order by releaseHeaderId ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                //sort = ""; //不能在里面加order ，否则效率非常低
                string sql = "from AscmCuxWipReleaseHeaders";
                //string detailCount = "select count(*) from AscmCuxWipReleaseLines where releaseHeaderId= a.releaseHeaderId";
                //string totalNumber = "select sum(deliveryQuantity) from AscmDeliveryOrderDetail where mainId= a.id";
                //string sql1 = "select new AscmCuxWipReleaseHeaders(a,(" + detailCount + "),(" + totalNumber + ")) from AscmCuxWipReleaseHeaders a ";
                string sql1 = "select new AscmCuxWipReleaseHeaders(a) from AscmCuxWipReleaseHeaders a ";//,(" + detailCount + ")

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                    whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereWipEntityId);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    sql1 += " where " + where;
                }
                //IList<AscmCuxWipReleaseHeaders> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmCuxWipReleaseHeaders>(sql + sort);
                IList<AscmCuxWipReleaseHeaders> ilist = null;
                if(ynPage!=null)
                   ilist= YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmCuxWipReleaseHeaders>(sql1 + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmCuxWipReleaseHeaders>(sql1 + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmCuxWipReleaseHeaders>(ilist);
                    SetWipEntities(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmCuxWipReleaseHeaders)", ex);
                throw ex;
            }
            return list;
        }
        public void Update(AscmCuxWipReleaseHeaders ascmDeliveryNotify)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmCuxWipReleaseHeaders>(ascmDeliveryNotify);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmCuxWipReleaseHeaders)", ex);
                    throw ex;
                }
            }
        }
        public void SetWipEntities(List<AscmCuxWipReleaseHeaders> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmCuxWipReleaseHeaders ascmCuxWipReleaseHeaders in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmCuxWipReleaseHeaders.wipEntityId + "";
                }
                string sql = "from AscmWipEntities where wipEntityId in (" + ids + ")";
                IList<AscmWipEntities> ilistAscmWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                sql = "from AscmWipDiscreteJobs where wipEntityId in (" + ids + ")";
                IList<AscmWipDiscreteJobs> ilistAscmWipDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sql);
                List<AscmWipEntities> listAscmWipEntities = null;
                List<AscmWipDiscreteJobs> listAscmWipDiscreteJobs = null;
                if (ilistAscmWipEntities != null && ilistAscmWipEntities.Count > 0)
                    listAscmWipEntities = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilistAscmWipEntities);
                if (ilistAscmWipDiscreteJobs != null && ilistAscmWipDiscreteJobs.Count > 0)
                {
                    listAscmWipDiscreteJobs = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobs>(ilistAscmWipDiscreteJobs);
                    AscmWipDiscreteJobsService.GetInstance().SetLookupValues(listAscmWipDiscreteJobs);
                    AscmWipDiscreteJobsService.GetInstance().SetScheduleGroups(listAscmWipDiscreteJobs);
                    AscmWipDiscreteJobsService.GetInstance().SetMaterialItem(listAscmWipDiscreteJobs);
                }
                foreach (AscmCuxWipReleaseHeaders ascmCuxWipReleaseHeaders in list)
                {
                    if (listAscmWipEntities != null)
                        ascmCuxWipReleaseHeaders.ascmWipEntities = listAscmWipEntities.Find(e => e.wipEntityId == ascmCuxWipReleaseHeaders.wipEntityId);
                    if (listAscmWipDiscreteJobs != null)
                        ascmCuxWipReleaseHeaders.ascmWipDiscreteJobs = listAscmWipDiscreteJobs.Find(e => e.wipEntityId == ascmCuxWipReleaseHeaders.wipEntityId);
                }
            }
        }

        /*
         *  2014/5/12 by chenyao
         */
        public List<AscmCuxWipReleaseHeaders> GetList(YnPage ynPage, string releaseNumber, string startScheduledStartDate, string endScheduledStartDate, int? statusType)
        {
            List<AscmCuxWipReleaseHeaders> list = null;
            string sql = "select {0} from cux_wip_release_headers wrh,wip_discrete_jobs wdj,wip_entities we,mtl_system_items_b msib";
            string where = string.Empty;            
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wrh.wip_entity_id = wdj.wip_entity_id");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wdj.wip_entity_id = we.wip_entity_id");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wdj.primary_item_id = msib.inventory_item_id");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wdj.organization_id = 775");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wrh.organization_id = 775");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wrh.release_type = 'ISSUE'");
            if (!string.IsNullOrWhiteSpace(releaseNumber))
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wrh.release_number = '" + releaseNumber.Trim() + "'");
            string whereStartScheduledStartDate = "", whereEndScheduledStartDate = "";
            DateTime dtStartScheduledStartDate, dtEndScheduledStartDate;
            if (!string.IsNullOrEmpty(startScheduledStartDate) && DateTime.TryParse(startScheduledStartDate, out dtStartScheduledStartDate))
            {
                whereStartScheduledStartDate = "wdj.scheduled_start_date >= to_date('" + dtStartScheduledStartDate.ToString("yyyy-MM-dd 00:00") + "', 'yyyy-mm-dd hh24:mi')";
                whereEndScheduledStartDate = "wdj.scheduled_start_date < to_date('" + dtStartScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00") + "', 'yyyy-mm-dd hh24:mi')";
            }
            if (!string.IsNullOrEmpty(endScheduledStartDate) && DateTime.TryParse(endScheduledStartDate, out dtEndScheduledStartDate))
                whereEndScheduledStartDate = "wdj.scheduled_start_date < to_date('" + dtEndScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00") + "', 'yyyy-mm-dd hh24:mi')";
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereStartScheduledStartDate);
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereEndScheduledStartDate);
            if (statusType.HasValue)
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wdj.status_type = " + statusType.Value);
            sql += " where " + where;
            
            string selectCount = string.Format(sql, "count(1)");

            sql = string.Format(sql, "wrh.release_header_id,wrh.release_number,wrh.wip_entity_id,we.wip_entity_name,wdj.scheduled_start_date,wdj.wip_supply_type,wdj.schedule_group_id,wdj.net_quantity,wdj.status_type,wdj.description,msib.segment1");
            sql += " order by wrh.release_header_id";

            OracleParameter[] commandParameters = new OracleParameter[] {
                            new OracleParameter {
                                ParameterName = "i_sql",
                                OracleDbType = OracleDbType.Varchar2,
                                Size = 2000,
                                Value = sql,
                                Direction = ParameterDirection.Input
                            },
                            new OracleParameter {
                                ParameterName = "i_sql_count",
                                OracleDbType = OracleDbType.Varchar2,
                                Size = 2000,
                                Value = selectCount,
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

            try
            {
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
                    list = new List<AscmCuxWipReleaseHeaders>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        AscmCuxWipReleaseHeaders cuxWipReleaseHeaders = new AscmCuxWipReleaseHeaders();
                        cuxWipReleaseHeaders.releaseHeaderId = Convert.ToInt32(dr["release_header_id"]);
                        cuxWipReleaseHeaders.releaseNumber = dr["release_number"].ToString();
                        cuxWipReleaseHeaders.wipEntityId = Convert.ToInt32(dr["wip_entity_id"]);
                        cuxWipReleaseHeaders.wipEntityName = dr["wip_entity_name"].ToString();
                        cuxWipReleaseHeaders.scheduledStartDate = dr["scheduled_start_date"].ToString();
                        cuxWipReleaseHeaders.wipSupplyType = Convert.ToInt32(dr["wip_supply_type"]);
                        cuxWipReleaseHeaders.scheduleGroupId = Convert.ToInt32(dr["schedule_group_id"]);
                        cuxWipReleaseHeaders.netQuantity = Convert.ToDecimal(dr["net_quantity"]);
                        cuxWipReleaseHeaders.statusType = Convert.ToInt32(dr["status_type"]);
                        cuxWipReleaseHeaders.description = dr["description"].ToString();
                        cuxWipReleaseHeaders.primaryItemDocNumber = dr["segment1"].ToString();
                        list.Add(cuxWipReleaseHeaders);
                    }
                }

                int totalCount = 0;
                int.TryParse(commandParameters[4].Value.ToString(), out totalCount);
                ynPage.SetRecordCount(totalCount);

                int pageCount = 0;
                int.TryParse(commandParameters[5].Value.ToString(), out pageCount);
                ynPage.pageCount = pageCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public void SetScheduleGroups(List<AscmCuxWipReleaseHeaders> list)
        {
            if (list != null && list.Count > 0)
            {
                string sql = "from AscmWipScheduleGroups where scheduleGroupId in (" + string.Join(",", list.Select(P => P.scheduleGroupId).Distinct()) + ")";
                IList<AscmWipScheduleGroups> ilistAscmWipScheduleGroups = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipScheduleGroups>(sql);
                if (ilistAscmWipScheduleGroups != null && ilistAscmWipScheduleGroups.Count > 0)
                {
                    List<AscmWipScheduleGroups> listAscmWipScheduleGroups = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipScheduleGroups>(ilistAscmWipScheduleGroups);
                    foreach (AscmCuxWipReleaseHeaders cuxWipReleaseHeaders in list)
                    {
                        AscmWipScheduleGroups wipScheduleGroups = listAscmWipScheduleGroups.Find(e => e.scheduleGroupId == cuxWipReleaseHeaders.scheduleGroupId);
                        if (wipScheduleGroups != null)
                            cuxWipReleaseHeaders.scheduleGroupName = wipScheduleGroups.scheduleGroupName;
                    }
                }
            }
        }
    }
}
