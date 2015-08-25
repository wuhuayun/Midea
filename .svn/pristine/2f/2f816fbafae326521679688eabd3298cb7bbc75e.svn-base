using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;

namespace MideaAscm.Services.FromErp
{
    public class AscmMtlTransactionReasonsService
    {
        private static AscmMtlTransactionReasonsService ascmMtlTransactionReasonsServices;
        public static AscmMtlTransactionReasonsService GetInstance()
        {
            if (ascmMtlTransactionReasonsServices == null)
                ascmMtlTransactionReasonsServices = new AscmMtlTransactionReasonsService();
            return ascmMtlTransactionReasonsServices;
        }

        public AscmMtlTransactionReasons Get(int id)
        {
            AscmMtlTransactionReasons ascmMtlTransactionReasons = null;
            try
            {
                ascmMtlTransactionReasons = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmMtlTransactionReasons>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmMtlTransactionReasons)", ex);
                throw ex;
            }
            return ascmMtlTransactionReasons;
        }
        public List<AscmMtlTransactionReasons> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmMtlTransactionReasons> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmMtlTransactionReasons ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    queryWord = queryWord.Trim().Replace(" ", "%");
                    whereQueryWord = " (reasonName like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmMtlTransactionReasons> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMtlTransactionReasons>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMtlTransactionReasons>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMtlTransactionReasons>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmMtlTransactionReasons)", ex);
                throw ex;
            }
            return list;
        }
    }
}
