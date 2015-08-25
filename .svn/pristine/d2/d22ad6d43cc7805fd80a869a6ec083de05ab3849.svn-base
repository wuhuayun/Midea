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
    public class AscmDeliveryOrderMainService
    {
        private static AscmDeliveryOrderMainService ascmDeliveryOrderMainServices;
        public static AscmDeliveryOrderMainService GetInstance()
        {
            //return ascmDeliveryOrderMainServices ?? new AscmDeliveryOrderMainService();
            if (ascmDeliveryOrderMainServices == null)
                ascmDeliveryOrderMainServices = new AscmDeliveryOrderMainService();
            return ascmDeliveryOrderMainServices;
        }
        public AscmDeliveryOrderMain Get(string id)
        {
            AscmDeliveryOrderMain ascmDeliveryNotify = null;
            try
            {
                ascmDeliveryNotify = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDeliveryOrderMain>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDeliveryOrderMain)", ex);
                throw ex;
            }
            return ascmDeliveryNotify;
        }
        public List<AscmDeliveryOrderMain> GetList(string sql)
        {
            List<AscmDeliveryOrderMain> list = null;
            try
            {
                IList<AscmDeliveryOrderMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderMain>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderMain>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliveryOrderMain)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmDeliveryOrderMain> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmDeliveryOrderMain> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                sort = ""; //不能在里面加order ，否则效率非常低
                string sql = "from AscmDeliveryOrderMain";
                string detailCount = "select count(*) from AscmDeliveryOrderDetail where mainId= a.id";
                string totalNumber = "select sum(deliveryQuantity) from AscmDeliveryOrderDetail where mainId= a.id";
                string sql1 = "select new AscmDeliveryOrderMain(a,(" + detailCount + "),(" + totalNumber + ")) from AscmDeliveryOrderMain a ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                    whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    sql1 += " where " + where;
                }
                //IList<AscmDeliveryOrderMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderMain>(sql + sort);
                IList<AscmDeliveryOrderMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderMain>(sql1 + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderMain>(ilist);
                    SetSupplier(list);
                    SetBatch(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliveryOrderMain)", ex);
                throw ex;
            }
            return list;
        }
        public void SetSupplier(List<AscmDeliveryOrderMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDeliveryOrderMain ascmDeliveryNotify in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmDeliveryNotify.supplierId + "";
                }
                string sql = "from AscmSupplier where id in (" + ids + ")";
                IList<AscmSupplier> ilistAscmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql);
                if (ilistAscmSupplier != null && ilistAscmSupplier.Count > 0)
                {
                    List<AscmSupplier> listAscmSupplier = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(ilistAscmSupplier);
                    foreach (AscmDeliveryOrderMain ascmDeliveryOrderMain in list)
                    {
                        ascmDeliveryOrderMain.ascmSupplier = listAscmSupplier.Find(e => e.id == ascmDeliveryOrderMain.supplierId);
                    }
                }
            }
        }
        public void SetBatch(List<AscmDeliveryOrderMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDeliveryOrderMain ascmDeliveryNotify in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmDeliveryNotify.batchId + "";
                }
                string sql = "from AscmDeliveryOrderBatch where id in (" + ids + ")";
                IList<AscmDeliveryOrderBatch> ilistAscmDeliveryOrderBatch = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderBatch>(sql);
                if (ilistAscmDeliveryOrderBatch != null && ilistAscmDeliveryOrderBatch.Count > 0)
                {
                    List<AscmDeliveryOrderBatch> listAscmDeliveryOrderBatch = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderBatch>(ilistAscmDeliveryOrderBatch);
                    foreach (AscmDeliveryOrderMain ascmDeliveryOrderMain in list)
                    {
                        ascmDeliveryOrderMain.ascmDeliveryOrderBatch = listAscmDeliveryOrderBatch.Find(e => e.id == ascmDeliveryOrderMain.batchId);
                    }
                }
            }
        }
        public void SetWipEntity(List<AscmDeliveryOrderMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDeliveryOrderMain ascmDeliveryNotify in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmDeliveryNotify.wipEntityId;
                }
                string sql = "from AscmWipEntities where id in (" + ids + ")";
                IList<AscmWipEntities> ilistAscmWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                if (ilistAscmWipEntities != null && ilistAscmWipEntities.Count > 0)
                {
                    List<AscmWipEntities> listAscmWipEntities = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilistAscmWipEntities);
                    foreach (AscmDeliveryOrderMain ascmDeliveryOrderMain in list)
                    {
                        ascmDeliveryOrderMain.ascmWipEntities = listAscmWipEntities.Find(P => P.wipEntityId == ascmDeliveryOrderMain.wipEntityId);
                    }
                }
            }
        }
        public void Update(AscmDeliveryOrderMain ascmDeliveryNotify)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmDeliveryOrderMain>(ascmDeliveryNotify);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmDeliveryOrderMain)", ex);
                    throw ex;
                }
            }
        }
    }
}
