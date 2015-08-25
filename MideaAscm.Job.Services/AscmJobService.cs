using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Job.Dal.Entities;
using MideaAscm.Job.Dal;
using YnBaseDal;
using NHibernate;
//using DDTek.Oracle;
using Oracle.DataAccess.Client;
using System.Data;

namespace MideaAscm.Job.Services
{
    public class AscmJobService
    {
        private static AscmJobService service;
        public static AscmJobService GetInstance()
        {
            if (service == null)
                service = new AscmJobService();
            return service;
        }

        public AscmJob Get(string jobName)
        {
            AscmJob ascmJob = null;
            try
            {
                ascmJob = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmJob>(jobName.ToUpper());
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmJob)", ex);
                throw ex;
            }
            return ascmJob;
        }
        public List<AscmJob> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord)
        {
            List<AscmJob> list = null;
            try
            {
                string sort = "";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                string sql = "from AscmJob";
                IList<AscmJob> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmJob>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmJob>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmJob)", ex);
                throw ex;
            }
            return list;
        }

        #region 作业
        public void CreateJob(AscmJob ascmJob)
        {
            try
            {
                //int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCountByList("select count(*) from AscmJob where jobName='" + ascmJob.jobName + "'");
                //if (count > 0)
                //    throw new Exception("作业【" + ascmJob.jobName + "】已存在！");
                AscmJob _ascmJob = Get(ascmJob.jobName);
                if (_ascmJob != null)
                    throw new Exception("作业【" + ascmJob.jobName + "】已存在！");
                var commandParameters = GetCommandParameters(ascmJob);
                ExecuteOracleProcedure("ascm_scheduler.sp_create_job", commandParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateJob(AscmJob ascmJob)
        {
            try
            {
                //int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCountByList("select count(*) from AscmJob where jobName='" + ascmJob.jobName + "'");
                //if (count == 0)
                //    throw new Exception("作业【" + ascmJob.jobName + "】不存在！");
                AscmJob _ascmJob = Get(ascmJob.jobName);
                if (_ascmJob == null)
                    throw new Exception("作业【" + ascmJob.jobName + "】不存在！");
                var commandParameters = GetCommandParameters(ascmJob);
                
                //放在事务中执行报异常
                //ExecuteOracleProcedure("ascm_scheduler.sp_update_job", commandParameters);

                ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                IDbCommand command = session.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "ascm_scheduler.sp_update_job";
                Array.ForEach<OracleParameter>(commandParameters, P => command.Parameters.Add(P));
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void EnableJob(string jobName)
        {
            try
            {
                //int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCountByList("select count(*) from AscmJob where jobName='" + jobName + "'");
                //if (count == 0)
                //    throw new Exception("作业【" + jobName + "】不存在！");
                AscmJob _ascmJob = Get(jobName);
                if (_ascmJob == null)
                    throw new Exception("作业【" + jobName + "】不存在！");
                OracleParameter[] commandParameters = new OracleParameter[] { 
                    new OracleParameter {
                        ParameterName = "i_job_name",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = jobName,
                        Direction = ParameterDirection.Input
                    },
                    new OracleParameter {
                        ParameterName = "i_commit_semantics",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = "STOP_ON_FIRST_ERROR",
                        Direction = ParameterDirection.Input
                    }
                };
                ExecuteOracleProcedure("ascm_scheduler.sp_enable", commandParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DisableJob(string jobName)
        {
            try
            {
                //int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCountByList("select count(*) from AscmJob where jobName='" + jobName + "'");
                //if (count == 0)
                //    throw new Exception("作业【" + jobName + "】不存在！");
                AscmJob _ascmJob = Get(jobName);
                if (_ascmJob == null)
                    throw new Exception("作业【" + jobName + "】不存在！");
                OracleParameter[] commandParameters = new OracleParameter[] { 
                    new OracleParameter {
                        ParameterName = "i_job_name",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = jobName,
                        Direction = ParameterDirection.Input
                    },
                    new OracleParameter {
                        ParameterName = "i_force",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = "FALSE",
                        Direction = ParameterDirection.Input
                    },
                    new OracleParameter {
                        ParameterName = "i_commit_semantics",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = "STOP_ON_FIRST_ERROR",
                        Direction = ParameterDirection.Input
                    }
                };
                ExecuteOracleProcedure("ascm_scheduler.sp_disable", commandParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RunJob(string jobName)
        {
            try
            {
                //int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCountByList("select count(*) from AscmJob where jobName='" + jobName + "'");
                //if (count == 0)
                //    throw new Exception("作业【" + jobName + "】不存在！");
                AscmJob _ascmJob = Get(jobName);
                if (_ascmJob == null)
                    throw new Exception("作业【" + jobName + "】不存在！");
                OracleParameter[] commandParameters = new OracleParameter[] { 
                    new OracleParameter {
                        ParameterName = "i_job_name",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = jobName,
                        Direction = ParameterDirection.Input
                    },
                    new OracleParameter {
                        ParameterName = "i_use_current_session",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = "TRUE",
                        Direction = ParameterDirection.Input
                    }
                };
                ExecuteOracleProcedure("ascm_scheduler.sp_run_job", commandParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void StopJob(string jobName)
        {
            try
            {
                //int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCountByList("select count(*) from AscmJob where jobName='" + jobName + "'");
                //if (count == 0)
                //    throw new Exception("作业【" + jobName + "】不存在！");
                AscmJob _ascmJob = Get(jobName);
                if (_ascmJob == null)
                    throw new Exception("作业【" + jobName + "】不存在！");
                OracleParameter[] commandParameters = new OracleParameter[] { 
                    new OracleParameter {
                        ParameterName = "i_job_name",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = jobName,
                        Direction = ParameterDirection.Input
                    },
                    new OracleParameter {
                        ParameterName = "i_force",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = "FALSE",
                        Direction = ParameterDirection.Input
                    },
                    new OracleParameter {
                        ParameterName = "i_commit_semantics",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = "STOP_ON_FIRST_ERROR",
                        Direction = ParameterDirection.Input
                    }
                };
                ExecuteOracleProcedure("ascm_scheduler.sp_stop_job", commandParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }
        public void DropJob(string jobName)
        {
            try
            {
                //int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCountByList("select count(*) from AscmJob where jobName='" + jobName + "'");
                //if (count == 0)
                //    throw new Exception("作业【" + jobName + "】不存在！");
                AscmJob _ascmJob = Get(jobName);
                if (_ascmJob == null)
                    throw new Exception("作业【" + jobName + "】不存在！");
                OracleParameter[] commandParameters = new OracleParameter[] { 
                    new OracleParameter {
                        ParameterName = "i_job_name",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = jobName,
                        Direction = ParameterDirection.Input
                    },
                    new OracleParameter {
                        ParameterName = "i_force",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = "FALSE",
                        Direction = ParameterDirection.Input
                    },
                    new OracleParameter {
                        ParameterName = "i_defer",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = "FALSE",
                        Direction = ParameterDirection.Input
                    },
                    new OracleParameter {
                        ParameterName = "i_commit_semantics",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = "STOP_ON_FIRST_ERROR",
                        Direction = ParameterDirection.Input
                    }
                };
                ExecuteOracleProcedure("ascm_scheduler.sp_drop_job", commandParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }
        public void ExecuteOracleProcedure(string commandText, params OracleParameter[] commandParameters)
        {
            ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    IDbCommand command = session.Connection.CreateCommand();
                    //OracleCommand command = (OracleCommand)session.Connection.CreateCommand();
                    tx.Enlist(command);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = commandText;
                    Array.ForEach<OracleParameter>(commandParameters, P => command.Parameters.Add(P));
                    command.ExecuteNonQuery();

                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("运行失败(RunJob)", ex);
                    throw ex;
                }
            }
        }
        public OracleParameter[] GetCommandParameters(AscmJob ascmJob)
        {
            string[] arguments = null;
            if (ascmJob.listProcedureArgument != null)
                arguments = ascmJob.listProcedureArgument.Select(P => P.value).ToArray();
            return new OracleParameter[]{
                new OracleParameter{
                    ParameterName = "i_job_name",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = ascmJob.jobName,
                    Direction = ParameterDirection.Input
                },
                new OracleParameter{
                    ParameterName = "i_job_type",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = "STORED_PROCEDURE",
                    Direction = ParameterDirection.Input
                },
                new OracleParameter{
                    ParameterName = "i_job_action",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = ascmJob.jobAction,
                    Direction = ParameterDirection.Input
                },
                new OracleParameter{
                    ParameterName = "i_number_of_arguments",
                    OracleDbType = OracleDbType.Int32,
                    Value = arguments == null ? 0 : arguments.Length,
                    Direction = ParameterDirection.Input
                },
                new OracleParameter{
                    ParameterName = "i_start_date",
                    OracleDbType = OracleDbType.Varchar2, //OracleDbType.TimestampWithTZ,
                    Value = ascmJob.startDate,
                    Direction = ParameterDirection.Input
                },
                new OracleParameter{
                    ParameterName = "i_repeat_interval",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = ascmJob.repeatInterval,
                    Direction = ParameterDirection.Input
                },
                new OracleParameter{
                    ParameterName = "i_end_date",
                    OracleDbType = OracleDbType.Varchar2, //OracleDbType.TimestampWithTZ,
                    Value = ascmJob.endDate,
                    Direction = ParameterDirection.Input
                },
                new OracleParameter{
                    ParameterName = "i_job_class",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = "DEFAULT_JOB_CLASS",
                    Direction = ParameterDirection.Input
                },
                new OracleParameter{
                    ParameterName = "i_enabled",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = ascmJob.enabled,
                    Direction = ParameterDirection.Input
                },
                new OracleParameter{
                    ParameterName = "i_auto_drop",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = ascmJob.autoDrop,
                    Direction = ParameterDirection.Input
                },
                new OracleParameter{
                    ParameterName = "i_comments",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = ascmJob.comments,
                    Direction = ParameterDirection.Input
                },
                new OracleParameter{
                    ParameterName = "i_credential_name",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = null,
                    Direction = ParameterDirection.Input
                },
                new OracleParameter{
                    ParameterName = "i_destination_name",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = null,
                    Direction = ParameterDirection.Input
                },
                new OracleParameter{
                    ParameterName = "i_arguments_array",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = arguments,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                }
            };
        }
        #endregion
    }
}
