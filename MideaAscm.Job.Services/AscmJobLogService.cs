using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Job.Dal.Entities;
using MideaAscm.Job.Dal;
using YnBaseDal;

namespace MideaAscm.Job.Services
{
    public class AscmJobLogService
    {
        private static AscmJobLogService service;
        public static AscmJobLogService GetInstance()
        {
            if (service == null)
                service = new AscmJobLogService();
            return service;
        }

        public List<AscmJobLog> GetList(YnPage ynPage, string sortName, string sortOrder, string jobName, string queryWord)
        {
            List<AscmJobLog> list = null;
            try
            {
                string sort = "";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                string sql = "from AscmJobLog";
                string where = "";
                if (!string.IsNullOrEmpty(jobName))
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, " jobName=upper('" + jobName.Trim() + "')");
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmJobLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmJobLog>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmJobLog>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmJobLog)", ex);
                throw ex;
            }
            return list;
        }
    }
}
