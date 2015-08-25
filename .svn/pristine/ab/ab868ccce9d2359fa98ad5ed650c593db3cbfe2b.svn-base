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
    public class AscmBuildingWarehouseLinkService
    {
        private static AscmBuildingWarehouseLinkService ascmBuildingWarehouseLinkServices;
        public static AscmBuildingWarehouseLinkService GetInstance()
        {
            if (ascmBuildingWarehouseLinkServices == null)
                ascmBuildingWarehouseLinkServices = new AscmBuildingWarehouseLinkService();
            return ascmBuildingWarehouseLinkServices;
        }

        public AscmBuildingWarehouseLink Get(int id)
        {
            AscmBuildingWarehouseLink ascmBuildingWarehouseLink = null;
            try
            {
                ascmBuildingWarehouseLink = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmBuildingWarehouseLink>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmBuildingWarehouseLink)", ex);
                throw ex;
            }
            return ascmBuildingWarehouseLink;
        }
        public AscmBuildingWarehouseLink Get(int buildingId, string warehouseId)
        {
            AscmBuildingWarehouseLink ascmBuildingWarehouseLink = null;
            try
            {
                string sql = "from AscmBuildingWarehouseLink where buildingId=" + buildingId + " and warehouseId='" + warehouseId + "'";
                IList<AscmBuildingWarehouseLink> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmBuildingWarehouseLink>(sql);
                if (ilist != null && ilist.Count > 0)
                {
                    ascmBuildingWarehouseLink = ilist[0];
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmBuildingWarehouseLink)", ex);
                throw ex;
            }
            return ascmBuildingWarehouseLink;
        }
        public List<AscmBuildingWarehouseLink> GetList(string sql, bool isSetWarehouse = false)
        {
            List<AscmBuildingWarehouseLink> list = null;
            try
            {
                IList<AscmBuildingWarehouseLink> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmBuildingWarehouseLink>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmBuildingWarehouseLink>(ilist);
                    if (isSetWarehouse)
                        SetWarehouse(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmBuildingWarehouseLink)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmBuildingWarehouseLink> GetList(int buildingId, bool isSetWarehouse = true)
        {
            string whereOther = "";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " buildingId=" + buildingId);
            return GetList("", "", whereOther, isSetWarehouse);
        }
        public List<AscmBuildingWarehouseLink> GetList(string sortName, string sortOrder, string whereOther, bool isSetWarehouse = true)
        {
            List<AscmBuildingWarehouseLink> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmBuildingWarehouseLink ";

                string where = "";
                if (!string.IsNullOrEmpty(whereOther))
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmBuildingWarehouseLink> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmBuildingWarehouseLink>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmBuildingWarehouseLink>(ilist);
                    if (isSetWarehouse)
                        SetWarehouse(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmBuildingWarehouseLink)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmBuildingWarehouseLink> listAscmBuildingWarehouseLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmBuildingWarehouseLink);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmBuildingWarehouseLink)", ex);
                throw ex;
            }
        }
        public void Save(AscmBuildingWarehouseLink ascmBuildingWarehouseLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmBuildingWarehouseLink);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmBuildingWarehouseLink)", ex);
                throw ex;
            }
        }
        public void Update(AscmBuildingWarehouseLink ascmBuildingWarehouseLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmBuildingWarehouseLink>(ascmBuildingWarehouseLink);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmBuildingWarehouseLink)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmBuildingWarehouseLink)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmBuildingWarehouseLink ascmBuildingWarehouseLink = Get(id);
                Delete(ascmBuildingWarehouseLink);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmBuildingWarehouseLink ascmBuildingWarehouseLink)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmBuildingWarehouseLink>(ascmBuildingWarehouseLink);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmBuildingWarehouseLink)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmBuildingWarehouseLink> listAscmBuildingWarehouseLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmBuildingWarehouseLink);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmBuildingWarehouseLink)", ex);
                throw ex;
            }
        }

        private void SetWarehouse(List<AscmBuildingWarehouseLink> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmBuildingWarehouseLink ascmBuildingWarehouseLink in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'" + ascmBuildingWarehouseLink.warehouseId + "'";
                }
                string sql = "from AscmWarehouse where id in (" + ids + ")";
                IList<AscmWarehouse> ilistAscmWarehouse = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarehouse>(sql);
                if (ilistAscmWarehouse != null && ilistAscmWarehouse.Count > 0)
                {
                    List<AscmWarehouse> listAscmWarehouse = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarehouse>(ilistAscmWarehouse);
                    foreach (AscmBuildingWarehouseLink ascmBuildingWarehouseLink in list)
                    {
                        ascmBuildingWarehouseLink.ascmWarehouse = listAscmWarehouse.Find(e => e.id == ascmBuildingWarehouseLink.warehouseId);
                    }
                }
            }
        }
    }
}
