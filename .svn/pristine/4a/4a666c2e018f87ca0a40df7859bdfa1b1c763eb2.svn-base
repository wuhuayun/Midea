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
    public class AscmUnloadingPointMapLinkService
    {
        private static AscmUnloadingPointMapLinkService ascmUnloadingPointMapLinkServices;
        public static AscmUnloadingPointMapLinkService GetInstance()
        {
            if (ascmUnloadingPointMapLinkServices == null)
                ascmUnloadingPointMapLinkServices = new AscmUnloadingPointMapLinkService();
            return ascmUnloadingPointMapLinkServices;
        }

        public AscmUnloadingPointMapLink Get(int id)
        {
            AscmUnloadingPointMapLink ascmUnloadingPointMapLink = null;
            try
            {
                ascmUnloadingPointMapLink = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmUnloadingPointMapLink>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmUnloadingPointMapLink)", ex);
                throw ex;
            }
            return ascmUnloadingPointMapLink;
        }
        public List<AscmUnloadingPointMapLink> GetList(string sql, bool isSetUnloadingPoint = false)
        {
            List<AscmUnloadingPointMapLink> list = null;
            try
            {
                IList<AscmUnloadingPointMapLink> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPointMapLink>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUnloadingPointMapLink>(ilist);
                    if (isSetUnloadingPoint)
                        SetUnloadingPoint(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmUnloadingPointMapLink)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmUnloadingPointMapLink> GetList(int mapId)
        {
            List<AscmUnloadingPointMapLink> list = null;
            try
            {
                string sql = "from AscmUnloadingPointMapLink ";

                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, " mapId=" + mapId);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmUnloadingPointMapLink> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPointMapLink>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUnloadingPointMapLink>(ilist);
                    SetUnloadingPoint(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmUnloadingPointMapLink)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmUnloadingPointMapLink> listAscmUnloadingPointMapLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmUnloadingPointMapLink);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmUnloadingPointMapLink)", ex);
                throw ex;
            }
        }
        public void Save(AscmUnloadingPointMapLink ascmUnloadingPointMapLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmUnloadingPointMapLink);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmUnloadingPointMapLink)", ex);
                throw ex;
            }
        }
        public void Update(AscmUnloadingPointMapLink ascmUnloadingPointMapLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmUnloadingPointMapLink>(ascmUnloadingPointMapLink);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmUnloadingPointMapLink)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmUnloadingPointMapLink)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmUnloadingPointMapLink ascmUnloadingPointMapLink = Get(id);
                Delete(ascmUnloadingPointMapLink);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmUnloadingPointMapLink ascmUnloadingPointMapLink)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmUnloadingPointMapLink>(ascmUnloadingPointMapLink);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmUnloadingPointMapLink)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmUnloadingPointMapLink> listAscmUnloadingPointMapLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmUnloadingPointMapLink);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmUnloadingPointMapLink)", ex);
                throw ex;
            }
        }

        private void SetUnloadingPoint(List<AscmUnloadingPointMapLink> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmUnloadingPointMapLink ascmUnloadingPointMapLink in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmUnloadingPointMapLink.pointId;
                }
                string sql = "from AscmUnloadingPoint where id in (" + ids + ")";
                IList<AscmUnloadingPoint> ilistAscmUnloadingPoint = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPoint>(sql);
                if (ilistAscmUnloadingPoint != null && ilistAscmUnloadingPoint.Count > 0)
                {
                    List<AscmUnloadingPoint> listAscmUnloadingPoint = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUnloadingPoint>(ilistAscmUnloadingPoint);
                    foreach (AscmUnloadingPointMapLink ascmUnloadingPointMapLink in list)
                    {
                        ascmUnloadingPointMapLink.ascmUnloadingPoint = listAscmUnloadingPoint.Find(e => e.id == ascmUnloadingPointMapLink.pointId);
                    }
                }
            }
        }
    }
}
