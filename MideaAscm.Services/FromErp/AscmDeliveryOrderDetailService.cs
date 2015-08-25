using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Services.FromErp
{
    public class AscmDeliveryOrderDetailService
    {
        private static AscmDeliveryOrderDetailService ascmDeliveryOrderDetailServices;
        public static AscmDeliveryOrderDetailService GetInstance()
        {
            //return ascmDeliveryOrderDetailServices ?? new AscmDeliveryOrderDetailService();
            if (ascmDeliveryOrderDetailServices == null)
                ascmDeliveryOrderDetailServices = new AscmDeliveryOrderDetailService();
            return ascmDeliveryOrderDetailServices;
        }
        public AscmDeliveryOrderDetail Get(string id)
        {
            AscmDeliveryOrderDetail ascmDeliveryOrder = null;
            try
            {
                ascmDeliveryOrder = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDeliveryOrderDetail>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDeliveryOrderDetail)", ex);
                throw ex;
            }
            return ascmDeliveryOrder;
        }
        public List<AscmDeliveryOrderDetail> GetList(string sql)
        {
            List<AscmDeliveryOrderDetail> list = null;
            try
            {
                IList<AscmDeliveryOrderDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderDetail>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderDetail>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliveryOrderDetail)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmDeliveryOrderDetail> GetList(int mainId)
        {
            List<AscmDeliveryOrderDetail> list = null;
            try
            {
                string sort = " order by id ";
                //if (!string.IsNullOrEmpty(sortName))
                //{
                //    sort = " order by " + sortName.Trim() + " ";
                //    if (!string.IsNullOrEmpty(sortOrder))
                //        sort += sortOrder.Trim();
                //}
                string sql = "from AscmDeliveryOrderDetail where mainId=" + mainId;

                string where = "", whereQueryWord = "";
                //if (!string.IsNullOrEmpty(queryWord))
                //    whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                //where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " and " + where;
                //IList<AscmDeliveryOrderDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderDetail>(sql + sort);
                IList<AscmDeliveryOrderDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderDetail>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderDetail>(ilist);
                    SetMain(list);
                    SetMaterial(list);
                    SetNotifyDetail(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliveryOrderDetail)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmDeliveryOrderDetail> GetListByBatch(List<AscmDeliveryOrderBatch> ListAscmDeliveryOrderBatch)
        {
            List<AscmDeliveryOrderDetail> list = null;
            try
            {
                string whereIds = "";
                foreach(AscmDeliveryOrderBatch ascmDeliveryOrderBatch in ListAscmDeliveryOrderBatch)
                {

                    if (!string.IsNullOrEmpty(whereIds))
                        whereIds += ",";
                    whereIds += ascmDeliveryOrderBatch.id;
                }

                string sql = "from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId in (" + whereIds + "))";

                string where = "", whereQueryWord = "";

                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(where))
                    sql += " and " + where;

                IList<AscmDeliveryOrderDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderDetail>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderDetail>(ilist);
                    SetNotifyDetail(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliveryOrderDetail)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmDeliveryOrderDetail> GetListByBatch(int batchId, bool isSetMain = true, bool isSetMaterial = true, bool isSetNotifyDetail = true)
        {
            List<AscmDeliveryOrderDetail> list = null;
            try
            {
                string sort = " order by id ";
                //string hql = "from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId =" + batchId + ")";
                string hql = "from AscmDeliveryOrderDetail d where exists(select 1 from AscmDeliveryOrderMain m where m.id=d.mainId and m.batchId=" + batchId + ")";
                IList<AscmDeliveryOrderDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderDetail>(hql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderDetail>(ilist);
                    if (isSetMain)
                        SetMain(list);
                    if (isSetMaterial)
                        SetMaterial(list);
                    if (isSetNotifyDetail)
                        SetNotifyDetail(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliveryOrderDetail)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmMaterialItem> GetMaterialListByBatch(int batchId)
        {
            List<AscmMaterialItem> list = null;
            try
            {
                string sort = " order by id ";
                //if (!string.IsNullOrEmpty(sortName))
                //{
                //    sort = " order by " + sortName.Trim() + " ";
                //    if (!string.IsNullOrEmpty(sortOrder))
                //        sort += sortOrder.Trim();
                //}
                string sql = "from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId =" + batchId + ")";

                string where = "", whereQueryWord = "";
                //if (!string.IsNullOrEmpty(queryWord))
                //    whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                //where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " and " + where;
                //IList<AscmDeliveryOrderDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderDetail>(sql + sort);
                IList<AscmDeliveryOrderDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderDetail>(sql + sort);
                if (ilist != null)
                {
                    List<AscmDeliveryOrderDetail> listAscmDeliveryOrderDetail = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderDetail>(ilist);
                    //SetMain(list);
                    SetMaterial(listAscmDeliveryOrderDetail);
                    //SetNotifyDetail(list);
                    list=new List<AscmMaterialItem>();
                    foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in listAscmDeliveryOrderDetail)
                    {
                        AscmMaterialItem ascmMaterialItem = list.Find(p => p.id == ascmDeliveryOrderDetail.materialId);
                        if (ascmMaterialItem == null)
                        {
                            ascmMaterialItem = ascmDeliveryOrderDetail.ascmMaterialItem;
                            ascmMaterialItem.deliveryQuantity = 0;
                            list.Add(ascmMaterialItem);
                        }
                        ascmMaterialItem.deliveryQuantity += ascmDeliveryOrderDetail.deliveryQuantity;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public List<AscmDeliveryOrderDetail> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmDeliveryOrderDetail> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmDeliveryOrderDetail";

                string where = ""; //whereQueryWord = "";
                //if (!string.IsNullOrEmpty(queryWord))
                //    whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                //where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }
                IList<AscmDeliveryOrderDetail> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderDetail>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderDetail>(sql + sort);

                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderDetail>(ilist);
                    SetMain(list);
                    SetMaterial(list);
                    SetNotifyDetail(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliveryOrderMain)", ex);
                throw ex;
            }
            return list;
        }
        public void SetMain(List<AscmDeliveryOrderDetail> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmDeliveryOrderDetail.mainId + "";
                }
                string sql = "from AscmDeliveryOrderMain where id in (" + ids + ")";
                IList<AscmDeliveryOrderMain> ilistAscmDeliveryOrderMain = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderMain>(sql);
                if (ilistAscmDeliveryOrderMain != null && ilistAscmDeliveryOrderMain.Count > 0)
                {
                    List<AscmDeliveryOrderMain> listAscmDeliveryOrderMain = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderMain>(ilistAscmDeliveryOrderMain);
                    AscmDeliveryOrderMainService.GetInstance().SetSupplier(listAscmDeliveryOrderMain);
                    AscmDeliveryOrderMainService.GetInstance().SetBatch(listAscmDeliveryOrderMain);
                    AscmDeliveryOrderMainService.GetInstance().SetWipEntity(listAscmDeliveryOrderMain);
                    foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in list)
                    {
                        ascmDeliveryOrderDetail.ascmDeliveryOrderMain = listAscmDeliveryOrderMain.Find(e => e.id == ascmDeliveryOrderDetail.mainId);
                    }
                }
            }
        }
        public void SetMaterial(List<AscmDeliveryOrderDetail> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmDeliveryOrderDetail.materialId + "";
                }
                string sql = "from AscmMaterialItem where id in (" + ids + ")";
                IList<AscmMaterialItem> ilistAscmMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                if (ilistAscmMaterialItem != null && ilistAscmMaterialItem.Count > 0)
                {
                    List<AscmMaterialItem> listAscmMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistAscmMaterialItem);
                    foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in list)
                    {
                        ascmDeliveryOrderDetail.ascmMaterialItem = listAscmMaterialItem.Find(e => e.id == ascmDeliveryOrderDetail.materialId);
                    }
                }
            }
        }
        private void SetNotifyDetail(List<AscmDeliveryOrderDetail> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmDeliveryOrderDetail.notifyDetailId + "";
                }
                string sql = "from AscmDeliveryNotifyDetail where id in (" + ids + ")";
                IList<AscmDeliveryNotifyDetail> ilistAscmDeliveryNotifyDetail = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryNotifyDetail>(sql);
                if (ilistAscmDeliveryNotifyDetail != null && ilistAscmDeliveryNotifyDetail.Count > 0)
                {
                    List<AscmDeliveryNotifyDetail> listAscmDeliveryNotifyDetail = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryNotifyDetail>(ilistAscmDeliveryNotifyDetail);
                    AscmDeliveryNotifyDetailService.GetInstance().SetMain(listAscmDeliveryNotifyDetail);
                    foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in list)
                    {
                        ascmDeliveryOrderDetail.ascmDeliveryNotifyDetail = listAscmDeliveryNotifyDetail.Find(e => e.id == ascmDeliveryOrderDetail.notifyDetailId);
                    }
                }
            }
        }
    }
}
