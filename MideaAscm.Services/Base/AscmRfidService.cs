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
    public class AscmRfidService
    {
        private static AscmRfidService ascmRfidServices;
        public static AscmRfidService GetInstance()
        {
            //return ascmRfidServices ?? new AscmRfidService();
            if (ascmRfidServices == null)
                ascmRfidServices = new AscmRfidService();
            return ascmRfidServices;
        }
        public AscmRfid Get(string id)
        {
            AscmRfid ascmRfid = null;
            try
            {
                ascmRfid = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmRfid>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmRfid)", ex);
                throw ex;
            }
            return ascmRfid;
        }
        public AscmRfid Get(string id, string sessionKey)
        {
            AscmRfid ascmRfid = null;
            try
            {
                ascmRfid = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmRfid>(id, sessionKey);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmRfid)", ex);
                throw ex;
            }
            return ascmRfid;
        }
        /// <summary>
        /// 2013/07/17
        /// </summary>
        /// <param name="epc"></param>
        /// <returns></returns>
        public AscmRfid GetByEpc(string epcId)
        {
            AscmRfid ascmRfid = null;
            try
            {
                NHibernate.ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                NHibernate.IQuery query = session.CreateQuery("from AscmRfid where epcId='" + epcId + "' ");
                ascmRfid = query.UniqueResult<AscmRfid>();
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmRfid)", ex);
                throw ex;
            }
            return ascmRfid;
        }
        ///<summary>获取卡信息列表</summary>
        ///<returns>返回卡信息列表</returns>
        public List<AscmRfid> GetList(string sql)
        {
            List<AscmRfid> list = null;
            try
            {
                IList<AscmRfid> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmRfid>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmRfid>(ilist);
                    SetBindDescription(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmRfid)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmRfid> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord,string whereOther)
        {
            List<AscmRfid> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmRfid ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    queryWord = queryWord.Trim().Replace(" ", "%");
                    whereQueryWord = " (id like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                //IList<AscmRfid> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmRfid>(sql + sort);
                IList<AscmRfid> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmRfid>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmRfid>(ilist);
                    SetBindDescription(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmRfid)", ex);
                throw ex;
            }
            return list;
        }
        private void SetBindDescription(List<AscmRfid> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids_EmployeeCar = string.Empty;
                foreach (AscmRfid ascmRfid in list)
                {
                    if (ascmRfid.bindType == AscmRfid.BindTypeDefine.employeeCar && !string.IsNullOrEmpty(ascmRfid.bindId))
                    {
                        if (!string.IsNullOrEmpty(ids_EmployeeCar))
                            ids_EmployeeCar += ",";
                        ids_EmployeeCar += "" + ascmRfid.bindId + "";
                    }
                }
                if (ids_EmployeeCar != "")
                {
                    string sql = "from AscmEmployeeCar where id in (" + ids_EmployeeCar + ")";
                    IList<AscmEmployeeCar> ilistAscmEmployeeCar = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmployeeCar>(sql);
                    if (ilistAscmEmployeeCar != null && ilistAscmEmployeeCar.Count > 0)
                    {
                        List<AscmEmployeeCar> listAscmEmployeeCar = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmEmployeeCar>(ilistAscmEmployeeCar);
                        AscmEmployeeCarService.GetInstance().SetEmployee(listAscmEmployeeCar);
                        foreach (AscmRfid ascmRfid in list)
                        {
                            AscmEmployeeCar ascmEmployeeCar = listAscmEmployeeCar.Find(e => e.id.ToString() == ascmRfid.bindId);
                            //if(ascmEmployeeCar!=null)
                            //    ascmRfid._bindDescription = ascmEmployeeCar ._employeeName+"->"+ ascmEmployeeCar.plateNumber;
                        }
                    }
                }
            }
        }
        public void Save(List<AscmRfid> listAscmRfid)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmRfid);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmRfid)", ex);
                throw ex;
            }
        }
        public void Save(AscmRfid ascmRfid)
        {
            try
            {
                //int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmRfid where docNumber='" + ascmRfid.docNumber + "'");
                //if (count == 0)
                //{
                //    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmRfid");
                //    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                //    {
                //        try
                //        {
                //            maxId++;
                //            ascmRfid.id = maxId;
                //            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmRfid);
                //            tx.Commit();//正确执行提交
                //        }
                //        catch (Exception ex)
                //        {
                //            tx.Rollback();//回滚
                //            throw ex;
                //        }
                //    }
                //}
                //else
                //{
                //    throw new Exception("已经存在员工编号\"" + ascmRfid.name + "\"！");
                //}
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmRfid)", ex);
                throw ex;
            }
        }
        public void Update(AscmRfid ascmRfid)
        {
            //int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmRfid where id<>" + ascmRfid.id + " and docNumber='" + ascmRfid.docNumber + "'");
            //if (count == 0)
            //{
            //    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            //    {
            //        try
            //        {
            //            YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmRfid>(ascmRfid);
            //            tx.Commit();//正确执行提交
            //        }
            //        catch (Exception ex)
            //        {
            //            tx.Rollback();//回滚
            //            YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmRfid)", ex);
            //            throw ex;
            //        }
            //    }
            //}
            //else
            //{
            //    throw new Exception("已经存在员工编号\"" + ascmRfid.name + "\"！");
            //}
        }
        public void Delete(string id)
        {
            try
            {
                AscmRfid ascmRfid = Get(id);
                Delete(ascmRfid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmRfid ascmRfid)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmRfid>(ascmRfid);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmRfid)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmRfid> listAscmRfid)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmRfid);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmRfid)", ex);
                throw ex;
            }
        }
    }
}
