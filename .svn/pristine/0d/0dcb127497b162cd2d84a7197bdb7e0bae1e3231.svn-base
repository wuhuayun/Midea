using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;

namespace MideaAscm.Services.Base
{
    public class AscmSupplierService
    {
        private static AscmSupplierService ascmSupplierServices;
        public static AscmSupplierService GetInstance()
        {
            //return ascmSupplierServices ?? new AscmSupplierService();
            if (ascmSupplierServices == null)
                ascmSupplierServices = new AscmSupplierService();
            return ascmSupplierServices;

        }
        public AscmSupplier Get(int id, string sessionKey = null)
        {
            AscmSupplier ascmSupplier = null;
            try
            {
                ascmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmSupplier>(id, sessionKey);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmSupplier)", ex);
                throw ex;
            }
            return ascmSupplier;
        }
        public List<AscmSupplier> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord,string whereOther)
        {
            List<AscmSupplier> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmSupplier ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    queryWord = queryWord.Trim().Replace(" ", "%");
                    whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or docNumber like '%" + queryWord.Trim() + "%'";
                    int _fValue = -1;
                    if (int.TryParse(queryWord, out _fValue))
                    {
                        whereQueryWord += " or id like '" + queryWord + "%'";
                    }
                    whereQueryWord += ")";
                }

                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                //IList<AscmSupplier> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql + sort);
                IList<AscmSupplier> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmSupplier)", ex);
                throw ex;
            }
            return list;
        }
        public void Update(AscmSupplier ascmSupplier)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmSupplier>(ascmSupplier);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmSupplier)", ex);
                    throw ex;
                }
            }
        }
        public void SetSupplierAddress(List<AscmSupplier> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmSupplier ascmSupplier in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmSupplier.id;
                }
                string sql = "from AscmSupplierAddress where vendorId in (" + ids + ")";
                IList<AscmSupplierAddress> ilistAscmSupplierAddress = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplierAddress>(sql);
                if (ilistAscmSupplierAddress != null && ilistAscmSupplierAddress.Count > 0)
                {
                    List<AscmSupplierAddress> listAscmSupplierAddress = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplierAddress>(ilistAscmSupplierAddress);
                    foreach (AscmSupplier ascmSupplier in list)
                    {
                        ascmSupplier.ascmSupplierAddress = listAscmSupplierAddress.Find(e => e.vendorId == ascmSupplier.id);
                    }
                }
            }
        }

        #region 应用
        /// <summary>获取当天需要送货的供应商列表</summary>
        public List<string> GetDoorLedSupplier()
        {
            List<string> list = new List<string>();
            try
            {
                string sql = "from AscmSupplier";
                string where = "", whereDeliveryNotify = "";
                whereDeliveryNotify = "id in (select supplierId from AscmDeliveryNotifyMain where {0})";
                whereDeliveryNotify = string.Format(whereDeliveryNotify, "appointmentStartTime>='" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' and appointmentEndTime<='" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereDeliveryNotify);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmSupplier> ilistSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql);
                if (ilistSupplier != null && ilistSupplier.Count > 0)
                {
                    List<AscmSupplier> listSupplier = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(ilistSupplier);
                    list = listSupplier.Select(P => P.name).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        #endregion
    }
}
