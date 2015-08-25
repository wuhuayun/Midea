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
    public class AscmUnloadingPointControllerService
    {
        private static AscmUnloadingPointControllerService ascmUnloadingPointControllerServices;
        public static AscmUnloadingPointControllerService GetInstance()
        {
            if (ascmUnloadingPointControllerServices == null)
                ascmUnloadingPointControllerServices = new AscmUnloadingPointControllerService();
            return ascmUnloadingPointControllerServices;
        }

        public AscmUnloadingPointController Get(int id)
        {
            AscmUnloadingPointController ascmUnloadingPointController = null;
            try
            {
                ascmUnloadingPointController = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmUnloadingPointController>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmUnloadingPointController)", ex);
                throw ex;
            }
            return ascmUnloadingPointController;
        }
        public List<AscmUnloadingPointController> GetList(string sql)
        {
            List<AscmUnloadingPointController> list = null;
            try
            {
                IList<AscmUnloadingPointController> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPointController>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUnloadingPointController>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmUnloadingPointController)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmUnloadingPointController> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmUnloadingPointController> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmUnloadingPointController ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " (name like '%" + queryWord.Trim() + "%') or (ip like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmUnloadingPointController> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPointController>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPointController>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUnloadingPointController>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmUnloadingPointController)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmUnloadingPointController> listAscmUnloadingPointController)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmUnloadingPointController);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmUnloadingPointController)", ex);
                throw ex;
            }
        }
        public void Save(AscmUnloadingPointController ascmUnloadingPointController)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmUnloadingPointController);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmUnloadingPointController)", ex);
                throw ex;
            }
        }
        public void Update(AscmUnloadingPointController ascmUnloadingPointController)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmUnloadingPointController>(ascmUnloadingPointController);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmUnloadingPointController)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmUnloadingPointController)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmUnloadingPointController ascmUnloadingPointController = Get(id);
                Delete(ascmUnloadingPointController);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmUnloadingPointController ascmUnloadingPointController)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmUnloadingPointController>(ascmUnloadingPointController);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmUnloadingPointController)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmUnloadingPointController> listAscmUnloadingPointController)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmUnloadingPointController);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmUnloadingPointController)", ex);
                throw ex;
            }
        }
    }
}
