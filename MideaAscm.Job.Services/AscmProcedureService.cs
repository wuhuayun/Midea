using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Job.Dal.Entities;
using MideaAscm.Job.Dal;
using YnBaseDal;

namespace MideaAscm.Job.Services
{
    public class AscmProcedureService
    {
        private static AscmProcedureService service;
        public static AscmProcedureService GetInstance()
        {
            if (service == null)
                service = new AscmProcedureService();
            return service;
        }

        public AscmProcedure Get(int id)
        {
            AscmProcedure AscmProceduresView = null;
            try
            {
                AscmProceduresView = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmProcedure>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmProceduresView)", ex);
                throw ex;
            }
            return AscmProceduresView;
        }
        public List<AscmProcedure> GetList(string queryWord)
        {
            List<AscmProcedure> list = null;
            try
            {
                string sql = "select a.* from (select rownum id,up.* from user_procedures up {0}) a";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                    whereQueryWord = " up.object_name||'.'||up.PROCEDURE_NAME like upper('%" + queryWord + "%') ";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, " up.SUBPROGRAM_ID>0 ");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                    where = " where " + where;
                sql = string.Format(sql, where);
                NHibernate.ISQLQuery query = YnDaoHelper.GetInstance().nHibernateHelper.FindBySQLQuery(sql);
                IList<AscmProcedure> ilist = query.AddEntity("a", typeof(AscmProcedure)).List<AscmProcedure>();
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmProcedure>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmProceduresView)", ex);
                throw ex;
            }
            return list;
        }
    }
}
