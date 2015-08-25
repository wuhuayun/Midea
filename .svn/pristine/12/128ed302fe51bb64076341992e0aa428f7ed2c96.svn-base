using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Job.Dal.Entities;
using MideaAscm.Job.Dal;
using YnBaseDal;

namespace MideaAscm.Job.Services
{
    public class AscmJobArgumentService
    {
        private static AscmJobArgumentService service;
        public static AscmJobArgumentService GetInstance()
        {
            if (service == null)
                service = new AscmJobArgumentService();
            return service;
        }

        public AscmJobArgument Get(int objectId)
        {
            AscmJobArgument AscmJobArgument = null;
            try
            {
                AscmJobArgument = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmJobArgument>(objectId);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmJobArgument)", ex);
                throw ex;
            }
            return AscmJobArgument;
        }
        public List<AscmJobArgument> GetList(string jobName)
        {
            List<AscmJobArgument> list = null;
            try
            {
                if (!string.IsNullOrEmpty(jobName))
                {
                    string sql = "select a.* from (select rownum id,"
                                                + "usja.JOB_NAME,"
                                                + "usja.ARGUMENT_NAME,"
                                                + "usja.ARGUMENT_POSITION,"
                                                + "usja.ARGUMENT_TYPE,"
                                                + "usja.value,"
                                                + "usja.OUT_ARGUMENT"
                                                +" from user_scheduler_job_args usja where usja.JOB_NAME='" + jobName.Trim().ToUpper() + "') a";
                    NHibernate.ISQLQuery query = YnDaoHelper.GetInstance().nHibernateHelper.FindBySQLQuery(sql);
                    IList<AscmJobArgument> ilist = query.AddEntity("a", typeof(AscmJobArgument)).List<AscmJobArgument>();
                    if (ilist != null)
                    {
                        list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmJobArgument>(ilist);
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmJobArgument)", ex);
                throw ex;
            }
            return list;
        }
    }
}
