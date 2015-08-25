using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal;
using YnBaseDal;
using MideaAscm.Dal.Warehouse.Entities;
using NHibernate;
using MideaAscm.Dal.IEntity;

namespace MideaAscm.Services.Warehouse
{
    public class AscmWmsLogisticsMainLogService
    {
        private static AscmWmsLogisticsMainLogService service;
        public static AscmWmsLogisticsMainLogService GetInstance()
        {
            if (service == null)
                service = new AscmWmsLogisticsMainLogService();
            return service;
        }
        public AscmWmsLogisticsMainLog Get(int id)
        {
            AscmWmsLogisticsMainLog ascmWmsLogisticsMainLog = null;
            try
            {
                ascmWmsLogisticsMainLog = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsLogisticsMainLog>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsLogisticsMainLog)", ex);
                throw ex;
            }
            return ascmWmsLogisticsMainLog;
        }
        public List<AscmWmsLogisticsMainLog> GetList(string sql)
        {
            List<AscmWmsLogisticsMainLog> list = null;
            try
            {
                IList<AscmWmsLogisticsMainLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsLogisticsMainLog>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsLogisticsMainLog>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsLogisticsMainLog)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsLogisticsMainLog> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmWmsLogisticsMainLog> list = null;
            try
            {
                string sort = " order by createTime ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmWmsLogisticsMainLog ";

                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmWmsLogisticsMainLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsLogisticsMainLog>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsLogisticsMainLog>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsLogisticsMainLog)", ex);
                throw ex;
            }
            return list;
        }
        public AscmWmsLogisticsMainLog Create()
        {
            AscmWmsLogisticsMainLog ascmWmsLogisticsMainLog = new AscmWmsLogisticsMainLog();
            int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsLogisticsMainLog");
            ascmWmsLogisticsMainLog.id = ++maxId;
            ascmWmsLogisticsMainLog.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            ascmWmsLogisticsMainLog.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            ascmWmsLogisticsMainLog.returnCode = AscmWmsLogisticsMainLog.ReturnCodeDefine.correct;
            ascmWmsLogisticsMainLog.returnMessage = "领料成功";

            return ascmWmsLogisticsMainLog;
        }
        public void Save(AscmWmsLogisticsMainLog wmsLogisticsMainLog, List<WmsAndLogistics> listWmsAndLogistics)
        {
            try
            {
                int maxId_Detail = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsLogisticsDetailLog");
                List<AscmWmsLogisticsDetailLog> listWmsLogisticsDetailLog = new List<AscmWmsLogisticsDetailLog>();
                foreach (WmsAndLogistics wmsAndLogistics in listWmsAndLogistics)
                {
                    AscmWmsLogisticsDetailLog wmsLogisticsDetailLog = new AscmWmsLogisticsDetailLog();
                    wmsLogisticsDetailLog.id = ++maxId_Detail;
                    wmsLogisticsDetailLog.mainId = wmsLogisticsMainLog.id;
                    wmsLogisticsDetailLog.wipEntityId = wmsAndLogistics.wipEntityId;
                    wmsLogisticsDetailLog.materialId = wmsAndLogistics.materialId;
                    wmsLogisticsDetailLog.quantity = wmsAndLogistics.quantity;
                    wmsLogisticsDetailLog.preparationString = wmsAndLogistics.preparationString;
                    wmsLogisticsDetailLog.workerId = wmsAndLogistics.workerId;
                    listWmsLogisticsDetailLog.Add(wmsLogisticsDetailLog);
                }

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(wmsLogisticsMainLog);
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listWmsLogisticsDetailLog);

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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsLogisticsMainLog)", ex);
                throw ex;
            }
        }
        public void Update(AscmWmsLogisticsMainLog ascmWmsLogisticsMainLog)
        {
            try
            {
                AscmWmsLogisticsMainLog _ascmWmsLogisticsMainLog = Get(ascmWmsLogisticsMainLog.id);
                _ascmWmsLogisticsMainLog.returnCode = ascmWmsLogisticsMainLog.returnCode;
                _ascmWmsLogisticsMainLog.returnMessage = ascmWmsLogisticsMainLog.returnMessage;

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update(_ascmWmsLogisticsMainLog);

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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("更新失败(Update AscmWmsLogisticsMainLog)", ex);
                throw ex;
            }
        }
        public AscmWmsLogisticsDetailLog GetDetail(string where)
        {
            AscmWmsLogisticsDetailLog wmsLogisticsDetailLog = null;
            try
            {
                if (!string.IsNullOrEmpty(where))
                {
                    string hql = "from AscmWmsLogisticsDetailLog where " + where;
                    IList<AscmWmsLogisticsDetailLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.FindTop<AscmWmsLogisticsDetailLog>(hql, 1);
                    if (ilist != null)
                        wmsLogisticsDetailLog = ilist[0];
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsLogisticsDetailLog)", ex);
                throw ex;
            }
            return wmsLogisticsDetailLog;
        }
        public List<AscmWmsLogisticsDetailLog> GetAllDetail(string where)
        {
            List<AscmWmsLogisticsDetailLog> list = null;
            try
            {
                if (!string.IsNullOrEmpty(where))
                {
                    string hql = "from AscmWmsLogisticsDetailLog where " + where;
                    IList<AscmWmsLogisticsDetailLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsLogisticsDetailLog>(hql);
                    if (ilist != null)
                    {
                        list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsLogisticsDetailLog>(ilist);
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsLogisticsDetailLog)", ex);
                throw ex;
            }
            return list;
        }
        public void UpdateDetail(AscmWmsLogisticsDetailLog ascmWmsLogisticsDetailLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmWmsLogisticsDetailLog);

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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("更新失败(Update AscmWmsLogisticsDetailLog)", ex);
                throw ex;
            }
        }
    }
}
