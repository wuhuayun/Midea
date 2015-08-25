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
    public class AscmWmsBackInvoiceLinkService
    {
        private static AscmWmsBackInvoiceLinkService ascmWmsBackInvoiceLinkServices;
        public static AscmWmsBackInvoiceLinkService GetInstance()
        {
            if (ascmWmsBackInvoiceLinkServices == null)
                ascmWmsBackInvoiceLinkServices = new AscmWmsBackInvoiceLinkService();
            return ascmWmsBackInvoiceLinkServices;
        }

        public AscmWmsBackInvoiceLink Get(int id)
        {
            AscmWmsBackInvoiceLink ascmWmsBackInvoiceLink = null;
            try
            {
                ascmWmsBackInvoiceLink = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsBackInvoiceLink>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsBackInvoiceLink)", ex);
                throw ex;
            }
            return ascmWmsBackInvoiceLink;
        }
        public List<AscmWmsBackInvoiceLink> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmWmsBackInvoiceLink> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmWmsBackInvoiceLink ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }

                IList<AscmWmsBackInvoiceLink> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsBackInvoiceLink>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsBackInvoiceLink>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsBackInvoiceLink>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsBackInvoiceLink)", ex);
                throw ex;
            }
            return list;
        }
    }
}
