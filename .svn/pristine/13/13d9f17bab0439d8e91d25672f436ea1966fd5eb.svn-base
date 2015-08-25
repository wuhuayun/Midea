using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Vehicle.Entities;

namespace MideaAscm.Services.Vehicle
{
    public class AscmUnloadingPointMapService
    {
        private static AscmUnloadingPointMapService ascmUnloadingPointMapServices;
        public static AscmUnloadingPointMapService GetInstance()
        {
            if (ascmUnloadingPointMapServices == null)
                ascmUnloadingPointMapServices = new AscmUnloadingPointMapService();
            return ascmUnloadingPointMapServices;
        }

        public AscmUnloadingPointMap Get(int id)
        {
            AscmUnloadingPointMap ascmUnloadingPointMap = null;
            try
            {
                ascmUnloadingPointMap = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmUnloadingPointMap>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmUnloadingPointMap)", ex);
                throw ex;
            }
            return ascmUnloadingPointMap;
        }
        public List<AscmUnloadingPointMap> GetList(string sql, bool isSetWarehouse = false)
        {
            List<AscmUnloadingPointMap> list = null;
            try
            {
                IList<AscmUnloadingPointMap> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPointMap>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUnloadingPointMap>(ilist);
                    if (isSetWarehouse)
                        SetWarehouse(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmUnloadingPointMap)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmUnloadingPointMap> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther, bool isSetWarehouse = true)
        {
            List<AscmUnloadingPointMap> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmUnloadingPointMap ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " name like '%" + queryWord.Trim() + "%'";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmUnloadingPointMap> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPointMap>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPointMap>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUnloadingPointMap>(ilist);
                    if (isSetWarehouse)
                        SetWarehouse(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmUnloadingPointMap)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmUnloadingPointMap> listAscmUnloadingPointMap)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmUnloadingPointMap);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmUnloadingPointMap)", ex);
                throw ex;
            }
        }
        public void Save(AscmUnloadingPointMap ascmUnloadingPointMap)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmUnloadingPointMap);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmUnloadingPointMap)", ex);
                throw ex;
            }
        }
        public void Update(AscmUnloadingPointMap ascmUnloadingPointMap)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmUnloadingPointMap>(ascmUnloadingPointMap);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmUnloadingPointMap)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmUnloadingPointMap)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmUnloadingPointMap ascmUnloadingPointMap = Get(id);
                Delete(ascmUnloadingPointMap);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmUnloadingPointMap ascmUnloadingPointMap)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmUnloadingPointMap>(ascmUnloadingPointMap);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmUnloadingPointMap)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmUnloadingPointMap> listAscmUnloadingPointMap)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmUnloadingPointMap);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmUnloadingPointMap)", ex);
                throw ex;
            }
        }

        private void SetWarehouse(List<AscmUnloadingPointMap> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmUnloadingPointMap ascmUnloadingPointMap in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'" + ascmUnloadingPointMap.warehouseId + "'";
                }
                string sql = "from AscmWarehouse where id in (" + ids + ")";
                IList<AscmWarehouse> ilistAscmWarehouse = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarehouse>(sql);
                if (ilistAscmWarehouse != null && ilistAscmWarehouse.Count > 0)
                {
                    List<AscmWarehouse> listAscmWarehouse = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarehouse>(ilistAscmWarehouse);
                    foreach (AscmUnloadingPointMap ascmUnloadingPointMap in list)
                    {
                        ascmUnloadingPointMap.ascmWarehouse = listAscmWarehouse.Find(e => e.id == ascmUnloadingPointMap.warehouseId);
                    }
                }
            }
        }
    }
}
