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
    public class AscmWmsMtlReturnDetailService
    {
        private static AscmWmsMtlReturnDetailService ascmWmsMtlReturnDetailServices;
        public static AscmWmsMtlReturnDetailService GetInstance()
        {
            if (ascmWmsMtlReturnDetailServices == null)
                ascmWmsMtlReturnDetailServices = new AscmWmsMtlReturnDetailService();
            return ascmWmsMtlReturnDetailServices;
        }

        public AscmWmsMtlReturnDetail Get(int id)
        {
            AscmWmsMtlReturnDetail ascmWmsMtlReturnDetail = null;
            try
            {
                ascmWmsMtlReturnDetail = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsMtlReturnDetail>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsMtlReturnDetail)", ex);
                throw ex;
            }
            return ascmWmsMtlReturnDetail;
        }
        public List<AscmWmsMtlReturnDetail> GetList(string sql)
        {
            List<AscmWmsMtlReturnDetail> list = null;
            try
            {
                IList<AscmWmsMtlReturnDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMtlReturnDetail>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsMtlReturnDetail>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsMtlReturnDetail)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsMtlReturnDetail> GetList(string sortName, string sortOrder, int? mainId, string queryWord, string whereOther)
        {
            return GetList(null, sortName, sortOrder, mainId, queryWord, whereOther);
        }
        public List<AscmWmsMtlReturnDetail> GetList(YnPage ynPage, string sortName, string sortOrder, int? mainId, string queryWord, string whereOther)
        {
            List<AscmWmsMtlReturnDetail> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                string sql = "from AscmWmsMtlReturnDetail ";
                
                string where = "", whereQueryWord = "";
                if (mainId.HasValue)
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, " mainId=" + mainId);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }
                IList<AscmWmsMtlReturnDetail> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMtlReturnDetail>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMtlReturnDetail>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsMtlReturnDetail>(ilist);
                    SetWarelocation(list);
                    SetMaterial(list);
                    SetAscmWarehouse(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsMtlReturnDetail)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsMtlReturnDetail> GetListByMainId(int mainId)
        {
            return GetList("", "", mainId, "", "");
        }

        public void Save(List<AscmWmsMtlReturnDetail> listAscmWmsMtlReturnDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsMtlReturnDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsMtlReturnDetail)", ex);
                throw ex;
            }
        }
        public void Save(AscmWmsMtlReturnDetail ascmWmsMtlReturnDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsMtlReturnDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsMtlReturnDetail)", ex);
                throw ex;
            }
        }
        public void Update(AscmWmsMtlReturnDetail ascmWmsMtlReturnDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWmsMtlReturnDetail>(ascmWmsMtlReturnDetail);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWmsMtlReturnDetail)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmWmsMtlReturnDetail)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmWmsMtlReturnDetail ascmWmsMtlReturnDetail = Get(id);
                Delete(ascmWmsMtlReturnDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmWmsMtlReturnDetail ascmWmsMtlReturnDetail)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWmsMtlReturnDetail>(ascmWmsMtlReturnDetail);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsMtlReturnDetail)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmWmsMtlReturnDetail> listAscmWmsMtlReturnDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsMtlReturnDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsMtlReturnDetail)", ex);
                throw ex;
            }
        }

        private void SetAscmWarehouse(List<AscmWmsMtlReturnDetail> list)
        {
            if (list != null && list.Count > 0)
            {
                StringBuilder ids = new StringBuilder();
                foreach (AscmWmsMtlReturnDetail ascmWmsMtlReturnDetail in list)
                {
                    if (ids.Length > 0)
                    {
                        ids.Append(',');
                    }
                    ids.Append(ascmWmsMtlReturnDetail.warehouseId);
                }

                string sql = "from AscmWarehouse where id in (" + ids + ")";
                IList<AscmWarehouse> listAscmWarehouse = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarehouse>(sql);

                if (listAscmWarehouse != null && listAscmWarehouse.Count > 0)
                {
                    foreach (AscmWmsMtlReturnDetail ascmWmsMtlReturnDetail in list)
                    {
                        var ascmWarehouse = listAscmWarehouse.FirstOrDefault(e => e.id == ascmWmsMtlReturnDetail.warehouseId);
                        ascmWmsMtlReturnDetail.warehouseName = ascmWarehouse != null ? ascmWarehouse.name : "";
                    }
                }
            }
        }

        private void SetWarelocation(List<AscmWmsMtlReturnDetail> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsMtlReturnDetail ascmWmsMtlReturnDetail in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsMtlReturnDetail.warelocationId;
                }
                string sql = "from AscmWarelocation where id in (" + ids + ")";
                IList<AscmWarelocation> ilistAscmWarelocation = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarelocation>(sql);
                if (ilistAscmWarelocation != null && ilistAscmWarelocation.Count > 0)
                {
                    List<AscmWarelocation> listAscmWarelocation = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarelocation>(ilistAscmWarelocation);
                    foreach (AscmWmsMtlReturnDetail ascmWmsMtlReturnDetail in list)
                    {
                        ascmWmsMtlReturnDetail.ascmWarelocation = listAscmWarelocation.Find(e => e.id == ascmWmsMtlReturnDetail.warelocationId);
                    }
                }
            }
        }
        private void SetMaterial(List<AscmWmsMtlReturnDetail> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsMtlReturnDetail ascmWmsMtlReturnDetail in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsMtlReturnDetail.materialId;
                }
                string sql = "from AscmMaterialItem where id in (" + ids + ")";
                IList<AscmMaterialItem> ilistAscmMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                if (ilistAscmMaterialItem != null && ilistAscmMaterialItem.Count > 0)
                {
                    List<AscmMaterialItem> listAscmMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistAscmMaterialItem);
                    foreach (AscmWmsMtlReturnDetail ascmWmsMtlReturnDetail in list)
                    {
                        ascmWmsMtlReturnDetail.ascmMaterialItem = listAscmMaterialItem.Find(e => e.id == ascmWmsMtlReturnDetail.materialId);
                    }
                }
            }
        }
    }
}
