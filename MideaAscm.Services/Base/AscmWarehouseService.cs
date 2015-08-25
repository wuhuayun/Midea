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
    public class AscmWarehouseService
    {
        private static AscmWarehouseService ascmWarehouseServices;
        public static AscmWarehouseService GetInstance()
        {
            //return ascmWarehouseServices ?? new AscmWarehouseService();
            if (ascmWarehouseServices == null)
                ascmWarehouseServices = new AscmWarehouseService();
            return ascmWarehouseServices;
        }
        public AscmWarehouse Get(string id)
        {
            AscmWarehouse ascmWarehouse = null;
            try
            {
                ascmWarehouse = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWarehouse>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWarehouse)", ex);
                throw ex;
            }
            return ascmWarehouse;
        }
        public List<AscmWarehouse> GetList(string sql)
        {
            List<AscmWarehouse> list = null;
            try
            {
                IList<AscmWarehouse> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarehouse>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarehouse>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWarehouse)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWarehouse> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmWarehouse> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmWarehouse ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    queryWord = queryWord.Trim().Replace(" ", "%");
                    //whereQueryWord = " (id like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                    whereQueryWord = "upper(id) like '%" + queryWord.Trim().ToUpper() + "%'";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmWarehouse> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarehouse>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarehouse>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarehouse>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWarehouse)", ex);
                throw ex;
            }
            return list;
        }
        public void Update(AscmWarehouse ascmWarehouse)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWarehouse>(ascmWarehouse);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWarehouse)", ex);
                    throw ex;
                }
            }
        }
    }
}
