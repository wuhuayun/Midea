using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal;
using MideaAscm.Dal.Warehouse.Entities;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Services.Warehouse
{
    public class AscmWmsMtlRequisitionDetailService
    {
        private static AscmWmsMtlRequisitionDetailService ascmWmsMtlRequisitionDetailServices;
        public static AscmWmsMtlRequisitionDetailService GetInstance()
        {
            if (ascmWmsMtlRequisitionDetailServices == null)
                ascmWmsMtlRequisitionDetailServices = new AscmWmsMtlRequisitionDetailService();
            return ascmWmsMtlRequisitionDetailServices;
        }
        public AscmWmsMtlRequisitionDetail Get(int id)
        {
            AscmWmsMtlRequisitionDetail ascmWmsMtlRequisitionDetail = null;
            try
            {
                ascmWmsMtlRequisitionDetail = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsMtlRequisitionDetail>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsMtlRequisitionDetail)", ex);
                throw ex;
            }
            return ascmWmsMtlRequisitionDetail;
        }
        public List<AscmWmsMtlRequisitionDetail> GetList(string sql)
        {
            List<AscmWmsMtlRequisitionDetail> list = null;
            try
            {
                IList<AscmWmsMtlRequisitionDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMtlRequisitionDetail>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsMtlRequisitionDetail>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsMtlRequisitionDetail)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsMtlRequisitionDetail> GetList2(string whereOther)
        {
            List<AscmWmsMtlRequisitionDetail> list = null;
            try
            {
                string wipEntityName = "select name from AscmWipEntities where wipEntityId=(select wipEntityId from AscmWmsMtlRequisitionMain where id=a.mainId)";
                string hql = "select new AscmWmsMtlRequisitionDetail(a,(" + wipEntityName + ")) from AscmWmsMtlRequisitionDetail a ";

                if (!string.IsNullOrEmpty(whereOther))
                    hql += " where " + whereOther;

                IList<AscmWmsMtlRequisitionDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMtlRequisitionDetail>(hql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsMtlRequisitionDetail>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsMtlRequisitionDetail)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmWmsMtlRequisitionDetail> listAscmWmsMtlRequisitionDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsMtlRequisitionDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsMtlRequisitionDetail)", ex);
                throw ex;
            }
        }
        public void Save(AscmWmsMtlRequisitionDetail ascmWmsMtlRequisitionDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsMtlRequisitionDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsMtlRequisitionDetail)", ex);
                throw ex;
            }
        }

        public List<AscmWmsMtlRequisitionDetail> GetList(List<AscmWmsMtlRequisitionMain> list)
        {
            List<AscmWmsMtlRequisitionDetail> listDetail = null;
            if (list != null && list.Count > 0)
            {
                listDetail = new List<AscmWmsMtlRequisitionDetail>();
                var selectIds = list.Select(P => P.id).Distinct();
                var count = selectIds.Count();
                string ids = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += selectIds.ElementAt(i);
                    if ((i + 1) % 500 == 0 || (i + 1) == count)
                    {
                        if (!string.IsNullOrEmpty(ids))
                        {
                            string hql = "from AscmWmsMtlRequisitionDetail where mainId in(" + ids + ")";
                            List<AscmWmsMtlRequisitionDetail> _listDetail = GetList(hql);
                            if (_listDetail != null && _listDetail.Count > 0)
                            {
                                listDetail.AddRange(_listDetail);
                            }
                        }
                        ids = string.Empty;
                    }
                }
            }
            return listDetail;
        }
        public List<AscmWmsMtlRequisitionDetail> GetDetialByMainId(int mainId)
        {
            List<AscmWmsMtlRequisitionDetail> list = null;
            try
            {
                string sql = "from AscmWmsMtlRequisitionDetail where mainId=" + mainId;
                IList<AscmWmsMtlRequisitionDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMtlRequisitionDetail>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsMtlRequisitionDetail>(ilist);
                    SetWarelocation(list);
                    SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsMtlRequisitionDetail)", ex);
                throw ex;
            }
            return list;
        }
        public void SetWarelocation(List<AscmWmsMtlRequisitionDetail> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsMtlRequisitionDetail ascmWmsMtlRequisitionDetail in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsMtlRequisitionDetail.warelocationId;
                }
                string sql = "from AscmWarelocation where id in (" + ids + ")";
                IList<AscmWarelocation> ilistAscmWarelocation = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarelocation>(sql);
                if (ilistAscmWarelocation != null && ilistAscmWarelocation.Count > 0)
                {
                    List<AscmWarelocation> listAscmWarelocation = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarelocation>(ilistAscmWarelocation);
                    foreach (AscmWmsMtlRequisitionDetail ascmWmsMtlRequisitionDetail in list)
                    {
                        ascmWmsMtlRequisitionDetail.ascmWarelocation = listAscmWarelocation.Find(e => e.id == ascmWmsMtlRequisitionDetail.warelocationId);
                    }
                }
            }
        }
        public void SetMaterial(List<AscmWmsMtlRequisitionDetail> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsMtlRequisitionDetail ascmWmsMtlRequisitionDetail in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsMtlRequisitionDetail.materialId;
                }
                string sql = "from AscmMaterialItem where id in (" + ids + ")";
                IList<AscmMaterialItem> ilistAscmMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                if (ilistAscmMaterialItem != null && ilistAscmMaterialItem.Count > 0)
                {
                    List<AscmMaterialItem> listAscmMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistAscmMaterialItem);
                    foreach (AscmWmsMtlRequisitionDetail ascmWmsMtlRequisitionDetail in list)
                    {
                        ascmWmsMtlRequisitionDetail.ascmMaterialItem = listAscmMaterialItem.Find(e => e.id == ascmWmsMtlRequisitionDetail.materialId);
                    }
                }
            }
        }
    }
}
