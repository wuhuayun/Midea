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
    public class AscmWmsStockTransDetailService
    {
        private static AscmWmsStockTransDetailService ascmWmsStockTransDetailServices;
        public static AscmWmsStockTransDetailService GetInstance()
        {
            if (ascmWmsStockTransDetailServices == null)
                ascmWmsStockTransDetailServices = new AscmWmsStockTransDetailService();
            return ascmWmsStockTransDetailServices;
        }

        public int GetMaxId()
        {
            int maxId = 0;
            try
            {
                maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsStockTransDetail");
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsStockTransDetail MaxId)", ex);
                throw ex;
            }
            return maxId;
        }
        public AscmWmsStockTransDetail Get(int id)
        {
            AscmWmsStockTransDetail ascmWmsStockTransDetail = null;
            try
            {
                ascmWmsStockTransDetail = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsStockTransDetail>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsStockTransDetail)", ex);
                throw ex;
            }
            return ascmWmsStockTransDetail;
        }
        public List<AscmWmsStockTransDetail> GetList(string sql)
        {
            List<AscmWmsStockTransDetail> list = null;
            try
            {
                IList<AscmWmsStockTransDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsStockTransDetail>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsStockTransDetail>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsStockTransDetail)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsStockTransDetail> GetList(string sortName, string sortOrder, int? mainId, string queryWord, string whereOther)
        {
            return GetList(null, sortName, sortOrder, mainId, queryWord, whereOther);
        }
        public List<AscmWmsStockTransDetail> GetList(YnPage ynPage, string sortName, string sortOrder, int? mainId, string queryWord, string whereOther)
        {
            List<AscmWmsStockTransDetail> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                string sql = "from AscmWmsStockTransDetail ";
                
                string where = "", whereQueryWord = "";
                if (mainId.HasValue)
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, " mainId=" + mainId);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }
                IList<AscmWmsStockTransDetail> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsStockTransDetail>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsStockTransDetail>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsStockTransDetail>(ilist);
                    SetWarelocation(list);
                    SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsStockTransDetail)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsStockTransDetail> GetListByMainId(int mainId)
        {
            return GetList("", "", mainId, "", "");
        }

        public void Save(List<AscmWmsStockTransDetail> listAscmWmsStockTransDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsStockTransDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsStockTransDetail)", ex);
                throw ex;
            }
        }
        public void Save(AscmWmsStockTransDetail ascmWmsStockTransDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsStockTransDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsStockTransDetail)", ex);
                throw ex;
            }
        }
        public void Update(AscmWmsStockTransDetail ascmWmsStockTransDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWmsStockTransDetail>(ascmWmsStockTransDetail);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWmsStockTransDetail)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmWmsStockTransDetail)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmWmsStockTransDetail ascmWmsStockTransDetail = Get(id);
                Delete(ascmWmsStockTransDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmWmsStockTransDetail ascmWmsStockTransDetail)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWmsStockTransDetail>(ascmWmsStockTransDetail);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsStockTransDetail)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmWmsStockTransDetail> listAscmWmsStockTransDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsStockTransDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsStockTransDetail)", ex);
                throw ex;
            }
        }

        private void SetWarelocation(List<AscmWmsStockTransDetail> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsStockTransDetail ascmWmsStockTransDetail in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsStockTransDetail.fromWarelocationId + "," + ascmWmsStockTransDetail.toWarelocationId;
                }
                string sql = "from AscmWarelocation where id in (" + ids + ")";
                IList<AscmWarelocation> ilistAscmWarelocation = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarelocation>(sql);
                if (ilistAscmWarelocation != null && ilistAscmWarelocation.Count > 0)
                {
                    List<AscmWarelocation> listAscmWarelocation = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarelocation>(ilistAscmWarelocation);
                    foreach (AscmWmsStockTransDetail ascmWmsStockTransDetail in list)
                    {
                        ascmWmsStockTransDetail.fromWarelocation = listAscmWarelocation.Find(e => e.id == ascmWmsStockTransDetail.fromWarelocationId);
                        ascmWmsStockTransDetail.toWarelocation = listAscmWarelocation.Find(e => e.id == ascmWmsStockTransDetail.toWarelocationId);
                    }
                }
            }
        }
        private void SetMaterial(List<AscmWmsStockTransDetail> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsStockTransDetail ascmWmsStockTransDetail in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsStockTransDetail.materialId;
                }
                string sql = "from AscmMaterialItem where id in (" + ids + ")";
                IList<AscmMaterialItem> ilistAscmMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                if (ilistAscmMaterialItem != null && ilistAscmMaterialItem.Count > 0)
                {
                    List<AscmMaterialItem> listAscmMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistAscmMaterialItem);
                    foreach (AscmWmsStockTransDetail ascmWmsStockTransDetail in list)
                    {
                        ascmWmsStockTransDetail.ascmMaterialItem = listAscmMaterialItem.Find(e => e.id == ascmWmsStockTransDetail.materialId);
                    }
                }
            }
        }
    }
}
