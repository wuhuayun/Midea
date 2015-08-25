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

namespace MideaAscm.Services.Warehouse
{
    public class AscmWmsMtlReturnMainService
    {
        private static AscmWmsMtlReturnMainService ascmWmsMtlReturnMainServices;
        public static AscmWmsMtlReturnMainService GetInstance()
        {
            if (ascmWmsMtlReturnMainServices == null)
                ascmWmsMtlReturnMainServices = new AscmWmsMtlReturnMainService();
            return ascmWmsMtlReturnMainServices;
        }

        public AscmWmsMtlReturnMain Get(int id)
        {
            AscmWmsMtlReturnMain ascmWmsMtlReturnMain = null;
            try
            {
                ascmWmsMtlReturnMain = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsMtlReturnMain>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsMtlReturnMain)", ex);
                throw ex;
            }
            return ascmWmsMtlReturnMain;
        }
        public List<AscmWmsMtlReturnMain> GetList(string sql)
        {
            List<AscmWmsMtlReturnMain> list = null;
            try
            {
                IList<AscmWmsMtlReturnMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMtlReturnMain>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsMtlReturnMain>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsMtlReturnMain)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsMtlReturnMain> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmWmsMtlReturnMain> list = null;
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

                string sql = "from AscmWmsMtlReturnMain ";
                
                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }
                IList<AscmWmsMtlReturnMain> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMtlReturnMain>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMtlReturnMain>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsMtlReturnMain>(ilist);
                    SetTransactionReason(list);
                    SetWipEntities(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsMtlReturnMain)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmWmsMtlReturnMain> listAscmWmsMtlReturnMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsMtlReturnMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsMtlReturnMain)", ex);
                throw ex;
            }
        }
        public void Save(AscmWmsMtlReturnMain ascmWmsMtlReturnMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsMtlReturnMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsMtlReturnMain)", ex);
                throw ex;
            }
        }
        public void Update(AscmWmsMtlReturnMain ascmWmsMtlReturnMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWmsMtlReturnMain>(ascmWmsMtlReturnMain);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWmsMtlReturnMain)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmWmsMtlReturnMain)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmWmsMtlReturnMain ascmWmsMtlReturnMain = Get(id);
                Delete(ascmWmsMtlReturnMain);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmWmsMtlReturnMain ascmWmsMtlReturnMain)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWmsMtlReturnMain>(ascmWmsMtlReturnMain);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsMtlReturnMain)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmWmsMtlReturnMain> listAscmWmsMtlReturnMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsMtlReturnMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsMtlReturnMain)", ex);
                throw ex;
            }
        }

        private void SetTransactionReason(List<AscmWmsMtlReturnMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsMtlReturnMain ascmWmsMtlReturnMain in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsMtlReturnMain.reasonId;
                }
                if (!string.IsNullOrEmpty(ids))
                {
                    string sql = "from AscmMtlTransactionReasons where id in (" + ids + ")";
                    IList<AscmMtlTransactionReasons> ilistAscmMtlTransactionReasons = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMtlTransactionReasons>(sql);
                    if (ilistAscmMtlTransactionReasons != null && ilistAscmMtlTransactionReasons.Count > 0)
                    {
                        List<AscmMtlTransactionReasons> listAscmMtlTransactionReasons = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMtlTransactionReasons>(ilistAscmMtlTransactionReasons);
                        foreach (AscmWmsMtlReturnMain ascmWmsMtlReturnMain in list)
                        {
                            ascmWmsMtlReturnMain.ascmMtlTransactionReasons = listAscmMtlTransactionReasons.Find(e => e.reasonId == ascmWmsMtlReturnMain.reasonId);
                        }
                    }
                }
            }
        }
        private void SetWipEntities(List<AscmWmsMtlReturnMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsMtlReturnMain ascmWmsMtlReturnMain in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsMtlReturnMain.wipEntityId;
                }
                if (!string.IsNullOrEmpty(ids))
                {
                    string sql = "from AscmWipEntities where wipEntityId in (" + ids + ")";
                    IList<AscmWipEntities> ilistAscmWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                    if (ilistAscmWipEntities != null && ilistAscmWipEntities.Count > 0)
                    {
                        List<AscmWipEntities> listAscmWipEntities = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilistAscmWipEntities);
                        foreach (AscmWmsMtlReturnMain ascmWmsMtlReturnMain in list)
                        {
                            ascmWmsMtlReturnMain.ascmWipEntities = listAscmWipEntities.Find(e => e.wipEntityId == ascmWmsMtlReturnMain.wipEntityId);
                        }
                    }
                }
            }
        }
    }
}
