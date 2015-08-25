using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using NHibernate;

namespace MideaAscm.Services.Base
{
    public class AscmSupplierMaterialLinkService
    {
        private static AscmSupplierMaterialLinkService ascmSupplierMaterialLinkService;
        public static AscmSupplierMaterialLinkService GetInstance()
        {
            if (ascmSupplierMaterialLinkService == null)
                ascmSupplierMaterialLinkService = new AscmSupplierMaterialLinkService();
            return ascmSupplierMaterialLinkService;

        }
        public AscmSupplierMaterialLink Get(int id)
        {
            AscmSupplierMaterialLink ascmSupplierMaterialLink = null;
            try
            {
                ascmSupplierMaterialLink = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmSupplierMaterialLink>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmSupplierMaterialLink)", ex);
                throw ex;
            }
            return ascmSupplierMaterialLink;
        }
        public List<AscmSupplierMaterialLink> GetList(int supplierId)
        {
            List<AscmSupplierMaterialLink> list = null;
            try
            {
                string sort = " order by materialId ";
                string sql = "from AscmSupplierMaterialLink where supplierId=" + supplierId;

                IList<AscmSupplierMaterialLink> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplierMaterialLink>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplierMaterialLink>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmSupplierMaterialLink)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmSupplierMaterialLink> GetList(string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmSupplierMaterialLink> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmSupplierMaterialLink ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    //whereQueryWord = " docNumber like '%" + queryWord.Trim() + "%'";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }

                IList<AscmSupplierMaterialLink> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplierMaterialLink>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplierMaterialLink>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmSupplierMaterialLink)", ex);
                throw ex;
            }
            return list;
        }
        public void Update(AscmSupplierMaterialLink ascmSupplierMaterialLink)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmSupplierMaterialLink>(ascmSupplierMaterialLink);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmSupplierMaterialLink)", ex);
                    throw ex;
                }
            }
        }
        public void Update(List<AscmSupplierMaterialLink> list)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(list);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmSupplierMaterialLink)", ex);
                throw ex;
            }
        }
        public void Delete(AscmSupplierMaterialLink ascmSupplierMaterialLink)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmSupplierMaterialLink>(ascmSupplierMaterialLink);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmSupplierMaterialLink)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmSupplierMaterialLink> list)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(list);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmSupplierMaterialLink)", ex);
                throw ex;
            }
        }
        public void Save(List<AscmSupplierMaterialLink> list)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(list);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmSupplierMaterialLink)", ex);
                throw ex;
            }
        }
    }
}
