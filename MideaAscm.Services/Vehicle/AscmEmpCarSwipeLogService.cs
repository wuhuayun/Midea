using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Vehicle.Entities;

namespace MideaAscm.Services.Vehicle
{
    public class AscmEmpCarSwipeLogService
    {
        private static AscmEmpCarSwipeLogService ascmEmpCarSwipeLogServices;
        public static AscmEmpCarSwipeLogService GetInstance()
        {
            if (ascmEmpCarSwipeLogServices == null)
                ascmEmpCarSwipeLogServices = new AscmEmpCarSwipeLogService();
            return ascmEmpCarSwipeLogServices;
        }
        public AscmEmpCarSwipeLog Get(string id)
        {
            AscmEmpCarSwipeLog ascmEmpCarSwipeLog = null;
            try
            {
                ascmEmpCarSwipeLog = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmEmpCarSwipeLog>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmEmpCarSwipeLog)", ex);
                throw ex;
            }
            return ascmEmpCarSwipeLog;
        }
        ///<summary>获取卡信息列表</summary>
        ///<returns>返回卡信息列表</returns>
        public List<AscmEmpCarSwipeLog> GetList(string sql)
        {
            List<AscmEmpCarSwipeLog> list = null;
            try
            {
                IList<AscmEmpCarSwipeLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmpCarSwipeLog>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmEmpCarSwipeLog>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmEmpCarSwipeLog)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmEmpCarSwipeLog> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord,string whereOther)
        {
            List<AscmEmpCarSwipeLog> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmEmpCarSwipeLog ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    queryWord = queryWord.Trim().Replace(" ", "%");
                    whereQueryWord = " (employeeName like '%" + queryWord.Trim() + "%' or plateNumber like '%" + queryWord.Trim() + "%' or rfid like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmEmpCarSwipeLog> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmpCarSwipeLog>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmpCarSwipeLog>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmEmpCarSwipeLog>(ilist);
                    SetDoor(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmEmpCarSwipeLog)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmEmpCarSwipeLog> listAscmEmpCarSwipeLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmEmpCarSwipeLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmEmpCarSwipeLog)", ex);
                throw ex;
            }
        }
        public void Save(AscmEmpCarSwipeLog ascmEmpCarSwipeLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmEmpCarSwipeLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmEmpCarSwipeLog)", ex);
                throw ex;
            }
        }
        public void Update(AscmEmpCarSwipeLog ascmEmpCarSwipeLog)
        {
            //int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmEmpCarSwipeLog where id<>" + ascmEmpCarSwipeLog.id + " and docNumber='" + ascmEmpCarSwipeLog.docNumber + "'");
            //if (count == 0)
            //{
            //    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            //    {
            //        try
            //        {
            //            YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmEmpCarSwipeLog>(ascmEmpCarSwipeLog);
            //            tx.Commit();//正确执行提交
            //        }
            //        catch (Exception ex)
            //        {
            //            tx.Rollback();//回滚
            //            YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmEmpCarSwipeLog)", ex);
            //            throw ex;
            //        }
            //    }
            //}
            //else
            //{
            //    throw new Exception("已经存在员工编号\"" + ascmEmpCarSwipeLog.name + "\"！");
            //}
        }
        public void Delete(string id)
        {
            try
            {
                AscmEmpCarSwipeLog ascmEmpCarSwipeLog = Get(id);
                Delete(ascmEmpCarSwipeLog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmEmpCarSwipeLog ascmEmpCarSwipeLog)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmEmpCarSwipeLog>(ascmEmpCarSwipeLog);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmEmpCarSwipeLog)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmEmpCarSwipeLog> listAscmEmpCarSwipeLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmEmpCarSwipeLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmEmpCarSwipeLog)", ex);
                throw ex;
            }
        }
        public void AddLog(int doorId, string readingHead, string rfid, string employeeName, string plateNumber, bool pass, string description, DateTime createTime, string direction)
        {
            try
            {
                AscmEmpCarSwipeLog ascmEmpCarSwipeLog = GetAddLog(doorId, readingHead, rfid, employeeName, plateNumber,pass, description, createTime, direction);

                Save(ascmEmpCarSwipeLog);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("增加失败(Add AscmEmpCarSwipeLog)", ex);
                throw ex;
            }
        }
        public AscmEmpCarSwipeLog GetAddLog(int doorId, string readingHead, string rfid, string employeeName, string plateNumber, bool pass, string description, DateTime createTime, string direction)
        {
            try
            {
                DateTime dtServerTime = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentDate("AscmEmpCarSwipeLog");

                AscmEmpCarSwipeLog ascmEmpCarSwipeLog = new AscmEmpCarSwipeLog();
                ascmEmpCarSwipeLog.doorId = doorId;
                ascmEmpCarSwipeLog.readingHeadId = 0;
                ascmEmpCarSwipeLog.readingHead = readingHead;
                ascmEmpCarSwipeLog.rfid = rfid;
                ascmEmpCarSwipeLog.employeeName = employeeName;
                ascmEmpCarSwipeLog.plateNumber = plateNumber;
                ascmEmpCarSwipeLog.createTime = createTime.ToString("yyyy-MM-dd HH:mm:ss");
                ascmEmpCarSwipeLog.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                ascmEmpCarSwipeLog.status = 1;
                ascmEmpCarSwipeLog.pass = pass;
                ascmEmpCarSwipeLog.direction = direction;
                ascmEmpCarSwipeLog.description = description;

                return ascmEmpCarSwipeLog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SetDoor(List<AscmEmpCarSwipeLog> list)
        {
            if (list != null && list.Count > 0)
            {
                var doorIds = list.Select(P => P.doorId).Distinct();
                var count = doorIds.Count();
                string ids = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += doorIds.ElementAt(i);
                    if ((i + 1) % 500 == 0 || (i + 1) == count)
                    {
                        if (!string.IsNullOrEmpty(ids))
                        {
                            string sql = "from AscmDoor where id in (" + ids + ")";
                            IList<AscmDoor> ilistAscmDoor = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDoor>(sql);
                            if (ilistAscmDoor != null && ilistAscmDoor.Count > 0)
                            {
                                List<AscmDoor> listAscmDoor = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDoor>(ilistAscmDoor);
                                foreach (AscmEmpCarSwipeLog ascmEmpCarSwipeLog in list)
                                {
                                    ascmEmpCarSwipeLog.ascmDoor = listAscmDoor.Find(e => e.id == ascmEmpCarSwipeLog.doorId);
                                }
                            }
                        }
                        ids = string.Empty;
                    }
                }
            }
        }
    }
}
