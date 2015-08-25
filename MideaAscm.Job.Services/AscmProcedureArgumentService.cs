using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Job.Dal.Entities;
using MideaAscm.Job.Dal;
using YnBaseDal;

namespace MideaAscm.Job.Services
{
    public class AscmProcedureArgumentService
    {
        private static AscmProcedureArgumentService service;
        public static AscmProcedureArgumentService GetInstance()
        {
            if (service == null)
                service = new AscmProcedureArgumentService();
            return service;
        }

        public AscmProcedureArgument Get(int objectId)
        {
            AscmProcedureArgument AscmProcedureArgument = null;
            try
            {
                AscmProcedureArgument = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmProcedureArgument>(objectId);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmProcedureArgument)", ex);
                throw ex;
            }
            return AscmProcedureArgument;
        }
        public List<AscmProcedureArgument> GetList(string objectType, string objectName, string procedureName)
        {
            List<AscmProcedureArgument> list = null;
            try
            {
                if (!string.IsNullOrEmpty(objectType))
                {
                    string sql = "";
                    if (objectType.Trim().ToUpper() == "PROCEDURE" && !string.IsNullOrEmpty(objectName))
                    {
                        sql = "select a.* from (select rownum id,ua.* from user_arguments ua where ua.OBJECT_NAME='" + objectName.Trim().ToUpper() + "') a";
                    }
                    else if (objectType.ToUpper().Trim() == "PACKAGE" && !string.IsNullOrEmpty(objectName) && !string.IsNullOrEmpty(procedureName))
                    {
                        sql = "select a.* from (select rownum id,ua.* from user_arguments ua where ua.PACKAGE_NAME='" + objectName.Trim().ToUpper() + "' and ua.OBJECT_NAME='" + procedureName.Trim().ToUpper() + "') a";
                    }
                    NHibernate.ISQLQuery query = YnDaoHelper.GetInstance().nHibernateHelper.FindBySQLQuery(sql + " order by position");
                    IList<AscmProcedureArgument> ilist = query.AddEntity("a", typeof(AscmProcedureArgument)).List<AscmProcedureArgument>();
                    if (ilist != null)
                    {
                        list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmProcedureArgument>(ilist);
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmProcedureArgument)", ex);
                throw ex;
            }
            return list;
        }
    }
}
