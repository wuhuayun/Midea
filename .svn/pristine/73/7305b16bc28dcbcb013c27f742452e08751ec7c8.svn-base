using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using Oracle.DataAccess.Client;
using YnBaseDal;
using System.Data;
using NHibernate;

namespace MideaAscm.Services.Vehicle
{
    public class AscmReadingHeadLogService
    {
        private static AscmReadingHeadLogService ascmReadingHeadLogServices;
        public static AscmReadingHeadLogService GetInstance()
        {
            if (ascmReadingHeadLogServices == null)
                ascmReadingHeadLogServices = new AscmReadingHeadLogService();
            return ascmReadingHeadLogServices;
        }
        public AscmReadingHeadLog Get(string id)
        {
            AscmReadingHeadLog ascmReadingHeadLog = null;
            try
            {
                ascmReadingHeadLog = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmReadingHeadLog>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmReadingHeadLog)", ex);
                throw ex;
            }
            return ascmReadingHeadLog;
        }
        public List<AscmReadingHeadLog> GetList(string sql)
        {
            List<AscmReadingHeadLog> list = null;
            try
            {
                IList<AscmReadingHeadLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmReadingHeadLog>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmReadingHeadLog>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmReadingHeadLog)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmReadingHeadLog> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord,string whereOther)
        {
            List<AscmReadingHeadLog> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmReadingHeadLog ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " sn like '%" + queryWord.Trim() + "%'";
                    //whereQueryWord = " rfid like '%" + queryWord.Trim() + "%' or sn like '%"+queryWord.Trim() + "%'";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmReadingHeadLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmReadingHeadLog>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmReadingHeadLog>(ilist);
                    SetReadingHead(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmReadingHeadLog)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmReadingHeadLog> listAscmReadingHeadLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmReadingHeadLog);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmReadingHeadLog)", ex);
                throw ex;
            }
        }
        public void Save(AscmReadingHeadLog ascmReadingHeadLog, string sessionKey = null)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession(sessionKey).BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmReadingHeadLog, sessionKey);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmReadingHeadLog)", ex);
                throw ex;
            }
        }
        public void Update(AscmReadingHeadLog ascmReadingHeadLog)
        {
            //int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmReadingHeadLog where id<>" + ascmReadingHeadLog.id + " and docNumber='" + ascmReadingHeadLog.docNumber + "'");
            //if (count == 0)
            //{
            //    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            //    {
            //        try
            //        {
            //            YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmReadingHeadLog>(ascmReadingHeadLog);
            //            tx.Commit();//正确执行提交
            //        }
            //        catch (Exception ex)
            //        {
            //            tx.Rollback();//回滚
            //            YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmReadingHeadLog)", ex);
            //            throw ex;
            //        }
            //    }
            //}
            //else
            //{
            //    throw new Exception("已经存在员工编号\"" + ascmReadingHeadLog.name + "\"！");
            //}
        }
        public void Delete(string id)
        {
            try
            {
                AscmReadingHeadLog ascmReadingHeadLog = Get(id);
                Delete(ascmReadingHeadLog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmReadingHeadLog ascmReadingHeadLog)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmReadingHeadLog>(ascmReadingHeadLog);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmReadingHeadLog)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmReadingHeadLog> listAscmReadingHeadLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmReadingHeadLog);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmReadingHeadLog)", ex);
                throw ex;
            }
        }
        private void SetReadingHead(List<AscmReadingHeadLog> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmReadingHeadLog ascmReadingHeadLog in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmReadingHeadLog.readingHeadId;
                }
                string sql = "from AscmReadingHead where id in (" + ids + ")";
                IList<AscmReadingHead> ilistAscmReadingHead = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmReadingHead>(sql);
                if (ilistAscmReadingHead != null && ilistAscmReadingHead.Count > 0)
                {
                    List<AscmReadingHead> listAscmReadingHead = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmReadingHead>(ilistAscmReadingHead);
                    foreach (AscmReadingHeadLog ascmReadingHeadLog in list)
                    {
                        ascmReadingHeadLog.ascmReadingHead = listAscmReadingHead.Find(e => e.id == ascmReadingHeadLog.readingHeadId);
                    }
                }
            }
        }

        #region 应用
        public AscmReadingHeadLog GetAddLog(int readingHeadId, string rfid, string sn, DateTime createTime, bool processed, string sessionKey = null)
        {
            try
            {
                DateTime dtServerTime = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentDate("AscmReadingHead", sessionKey);

                AscmReadingHeadLog ascmReadingHeadLog = new AscmReadingHeadLog();
                ascmReadingHeadLog.readingHeadId = readingHeadId;
                ascmReadingHeadLog.rfid = rfid;
                ascmReadingHeadLog.sn = sn;
                ascmReadingHeadLog.createTime = createTime.ToString("yyyy-MM-dd HH:mm:ss");
                ascmReadingHeadLog.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                ascmReadingHeadLog.processed = processed;
                return ascmReadingHeadLog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void AddLog(int readingHeadId, string rfid, string sn, DateTime createTime, bool processed, string sessionKey = null)
        {
            try
            {
                AscmReadingHeadLog ascmReadingHeadLog = GetAddLog(readingHeadId, rfid, sn, createTime, processed, sessionKey);

                Save(ascmReadingHeadLog, sessionKey);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("增加失败(Add AscmReadingHeadLog)", ex);
                throw ex;
            }
        }




        public void ExecuteOraProcedure(int ascmReadingHeadid, string tag2)
        {
            
            try
            {
                OracleParameter[] commandParameters = new OracleParameter[] {
                    new OracleParameter {
                        ParameterName = "p_ascmReadingHead_id",
                        OracleDbType = OracleDbType.Int32,
                        Value = ascmReadingHeadid,
                        Direction = ParameterDirection.Input
                    },
                    new OracleParameter {
                        ParameterName = "p_varlist",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = tag2,
                        Direction = ParameterDirection.Input
                    },
                };
                ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                //OracleCommand command = (OracleCommand)session.Connection.CreateCommand();
                IDbCommand command = session.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "P_ReadingHead_Tag";
                Array.ForEach<OracleParameter>(commandParameters, P => command.Parameters.Add(P));
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("执行存储过程失败", ex);
                throw ex;
            }
        }

        #endregion
    }
}
