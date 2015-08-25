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
    public class AscmWmsIncManAccDetailService
    {
        private static AscmWmsIncManAccDetailService ascmWmsIncManAccDetailServices;
        public static AscmWmsIncManAccDetailService GetInstance()
        {
            if (ascmWmsIncManAccDetailServices == null)
                ascmWmsIncManAccDetailServices = new AscmWmsIncManAccDetailService();
            return ascmWmsIncManAccDetailServices;
        }

        public AscmWmsIncManAccDetail Get(int id)
        {
            AscmWmsIncManAccDetail ascmWmsIncManAccDetail = null;
            try
            {
                ascmWmsIncManAccDetail = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsIncManAccDetail>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsIncManAccDetail)", ex);
                throw ex;
            }
            return ascmWmsIncManAccDetail;
        }
        public List<AscmWmsIncManAccDetail> GetList(int mainId)
        {
            List<AscmWmsIncManAccDetail> list = null;
            try
            {
                string sql = "from AscmWmsIncManAccDetail where incManAccMainId=" + mainId;
                IList<AscmWmsIncManAccDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsIncManAccDetail>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsIncManAccDetail>(ilist);
                    SetMaterialItem(list);
                    SetWarelocation(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsIncManAccDetail)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsIncManAccDetail> GetList(string sql)
        {
            List<AscmWmsIncManAccDetail> list = null;
            try
            {
                if (string.IsNullOrEmpty(sql))
                    sql = "from AscmWmsIncManAccDetail";
                IList<AscmWmsIncManAccDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsIncManAccDetail>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsIncManAccDetail>(ilist);
                    SetMaterialItem(list);
                    SetWarelocation(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsIncManAccDetail)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsIncManAccDetail> GetList(YnPage ynPage, string sortName, string sortOrder, string whereOther)
        {
            List<AscmWmsIncManAccDetail> list = null;
            try
            {
                string sort = " order by incManAccDetailId ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                string where = "";
                string sql = " from AscmWmsIncManAccDetail";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }
                IList<AscmWmsIncManAccDetail> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsIncManAccDetail>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsIncManAccDetail>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsIncManAccDetail>(ilist);
                    SetMaterialItem(list);
                    SetWarelocation(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsIncManAccDetail)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmWmsIncManAccDetail> listAscmWmsIncManAccDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsIncManAccDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsIncManAccDetail)", ex);
                throw ex;
            }
        }
        public void Save(AscmWmsIncManAccDetail ascmWmsIncManAccDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsIncManAccDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsIncManAccDetail)", ex);
                throw ex;
            }
        }
        public void Update(AscmWmsIncManAccDetail ascmWmsIncManAccDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWmsIncManAccDetail>(ascmWmsIncManAccDetail);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWmsIncManAccDetail)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmWmsIncManAccDetail)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmWmsIncManAccDetail ascmWmsIncManAccDetail = Get(id);
                Delete(ascmWmsIncManAccDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmWmsIncManAccDetail ascmWmsIncManAccDetail)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWmsIncManAccDetail>(ascmWmsIncManAccDetail);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsIncManAccDetail)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmWmsIncManAccDetail> listAscmWmsIncManAccDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsIncManAccDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsIncManAccDetail)", ex);
                throw ex;
            }
        }
        public void SetMaterialItem(List<AscmWmsIncManAccDetail> list)
        {
            if (list != null && list.Count() > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsIncManAccDetail ascmWmsIncManAccDetial in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsIncManAccDetial.materialId;
                }
                string sql = "from AscmMaterialItem where id in (" + ids + ")";
                IList<AscmMaterialItem> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                if (ilist != null && ilist.Count() > 0)
                {
                    List<AscmMaterialItem> listAscmMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilist);
                    foreach (AscmWmsIncManAccDetail ascmWmsIncManAccDetial in list)
                    {
                        ascmWmsIncManAccDetial.ascmMaterialItem = listAscmMaterialItem.Find(item => item.id == ascmWmsIncManAccDetial.materialId);
                    }
                }
            }
        }
        public void SetWarelocation(List<AscmWmsIncManAccDetail> list)
        {
            if (list != null && list.Count() > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsIncManAccDetail ascmWmsIncManAccDetial in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsIncManAccDetial.warelocationId;
                }
                string sql = "from AscmWarelocation where id in (" + ids + ")";
                IList<AscmWarelocation> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarelocation>(sql);
                if (ilist != null && ilist.Count() > 0)
                {
                    List<AscmWarelocation> listAscmWarelocation = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarelocation>(ilist);
                    foreach (AscmWmsIncManAccDetail ascmWmsIncManAccDetial in list)
                    {
                        ascmWmsIncManAccDetial.ascmWarelocation = listAscmWarelocation.Find(item => item.id == ascmWmsIncManAccDetial.warelocationId);
                    }
                }
            }
        }
    }
}
