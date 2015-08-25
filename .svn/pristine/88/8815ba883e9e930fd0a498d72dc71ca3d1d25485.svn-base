using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Warehouse.Entities;

namespace MideaAscm.Services.Warehouse
{
    public class AscmWorkshopBuildingService
    {
        private static AscmWorkshopBuildingService ascmWorkshopBuildingServices;
        public static AscmWorkshopBuildingService GetInstance()
        {
            if (ascmWorkshopBuildingServices == null)
                ascmWorkshopBuildingServices = new AscmWorkshopBuildingService();
            return ascmWorkshopBuildingServices;
        }

        public AscmWorkshopBuilding Get(int id)
        {
            AscmWorkshopBuilding ascmWorkshopBuilding = null;
            try
            {
                ascmWorkshopBuilding = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWorkshopBuilding>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWorkshopBuilding)", ex);
                throw ex;
            }
            return ascmWorkshopBuilding;
        }
        public List<AscmWorkshopBuilding> GetList(string sql)
        {
            List<AscmWorkshopBuilding> list = null;
            try
            {
                IList<AscmWorkshopBuilding> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWorkshopBuilding>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWorkshopBuilding>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWorkshopBuilding)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWorkshopBuilding> GetList(string sortName, string sortOrder)
        {
            List<AscmWorkshopBuilding> list = null;
            try
            {
                string sort = " order by sortNo ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmWorkshopBuilding ";

                IList<AscmWorkshopBuilding> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWorkshopBuilding>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWorkshopBuilding>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWorkshopBuilding)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmWorkshopBuilding> listAscmWorkshopBuilding)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWorkshopBuilding);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWorkshopBuilding)", ex);
                throw ex;
            }
        }
        public void Save(AscmWorkshopBuilding ascmWorkshopBuilding)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWorkshopBuilding);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWorkshopBuilding)", ex);
                throw ex;
            }
        }
        public void Update(AscmWorkshopBuilding ascmWorkshopBuilding)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWorkshopBuilding>(ascmWorkshopBuilding);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWorkshopBuilding)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmWorkshopBuilding)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmWorkshopBuilding ascmWorkshopBuilding = Get(id);
                Delete(ascmWorkshopBuilding);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmWorkshopBuilding ascmWorkshopBuilding)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWorkshopBuilding>(ascmWorkshopBuilding);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWorkshopBuilding)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmWorkshopBuilding> listAscmWorkshopBuilding)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWorkshopBuilding);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWorkshopBuilding)", ex);
                throw ex;
            }
        }

        #region 应用
        /// <summary>获取厂房列表</summary>
        public List<AscmWorkshopBuilding> GetAllList()
        {
            string sql = "from AscmWorkshopBuilding";
            return GetList(sql);
        }

        #endregion
    }
}
