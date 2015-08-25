using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.SupplierPreparation.Entities;

namespace MideaAscm.Services.SupplierPreparation
{
    public class AscmContainerService
    {
        private static AscmContainerService ascmContainerServices;
        public static AscmContainerService GetInstance()
        {
            if (ascmContainerServices == null)
                ascmContainerServices = new AscmContainerService();
            return ascmContainerServices;
        }

        public AscmContainer Get(string sn)
        {
            AscmContainer ascmContainer = null;
            try
            {
                ascmContainer = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmContainer>(sn);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmContainer)", ex);
                throw ex;
            }
            return ascmContainer;
        }
        public List<AscmContainer> GetList(string sql, bool isSetSupplier = false, bool isSetSpec = false)
        {
            List<AscmContainer> list = null;
            try
            {
                IList<AscmContainer> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainer>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainer>(ilist);
                    if (isSetSupplier)
                        SetSupplier(list);
                    if (isSetSpec)
                        SetSpec(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmContainer)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmContainer> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmContainer> list = null;
            try
            {
                string sort = " order by sn ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmContainer ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " (sn like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmContainer> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainer>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainer>(ilist);
                    SetSupplier(list);
                    SetSpec(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmContainer)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmContainer> listAscmContainer)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmContainer);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmContainer)", ex);
                throw ex;
            }
        }
        public void Save(AscmContainer ascmContainer)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmContainer);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmContainer)", ex);
                throw ex;
            }
        }
        /// <summary>
        /// 批量修改容器信息
        /// </summary>
        /// <param name="listAscm"></param>
        public void UpdateList(List<AscmContainer> listAscm)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscm);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("批量修改失败(Update AscmContainer)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("批量修改失败(Update AscmContainer)", ex);
                throw ex;
            }

        }
        public void Update(AscmContainer ascmContainer)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmContainer>(ascmContainer);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmContainer)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmContainer)", ex);
                throw ex;
            }
        }
        public void Delete(string sn)
        {
            try
            {
                AscmContainer ascmContainer = Get(sn);
                Delete(ascmContainer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmContainer ascmContainer)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmContainer>(ascmContainer);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmContainer)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmContainer> listAscmContainer)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmContainer);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmContainer)", ex);
                throw ex;
            }
        }

        private void SetSupplier(List<AscmContainer> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmContainer ascmContainer in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmContainer.supplierId + "";
                }
                string sql = "from AscmSupplier where id in (" + ids + ")";
                IList<AscmSupplier> ilistAscmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql);
                if (ilistAscmSupplier != null && ilistAscmSupplier.Count > 0)
                {
                    List<AscmSupplier> listAscmSupplier = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(ilistAscmSupplier);
                    foreach (AscmContainer ascmContainer in list)
                    {
                        ascmContainer.supplier = listAscmSupplier.Find(e => e.id == ascmContainer.supplierId);
                    }
                }
            }
        }
        private void SetSpec(List<AscmContainer> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmContainer ascmContainer in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmContainer.specId + "";
                }
                string sql = "from AscmContainerSpec where id in (" + ids + ")";
                IList<AscmContainerSpec> ilistAscmContainerSpec = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainerSpec>(sql);
                if (ilistAscmContainerSpec != null && ilistAscmContainerSpec.Count > 0)
                {
                    List<AscmContainerSpec> listAscmContainerSpec = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainerSpec>(ilistAscmContainerSpec);
                    foreach (AscmContainer ascmContainer in list)
                    {
                        ascmContainer.containerSpec = listAscmContainerSpec.Find(e => e.id == ascmContainer.specId);
                    }
                }
            }
        }
    }
}
