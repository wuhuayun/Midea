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
    public class AscmReadingHeadService
    {
        private static AscmReadingHeadService ascmReadingHeadServices;
        public static AscmReadingHeadService GetInstance()
        {
            //return ascmReadingHeadServices ?? new AscmReadingHeadService();
            if (ascmReadingHeadServices == null)
                ascmReadingHeadServices = new AscmReadingHeadService();
            return ascmReadingHeadServices;
        }
        public AscmReadingHead Get(int id)
        {
            AscmReadingHead ascmReadingHead = null;
            try
            {
                ascmReadingHead = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmReadingHead>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmReadingHead)", ex);
                throw ex;
            }
            return ascmReadingHead;
        }
        ///<summary>获取卡信息列表</summary>
        ///<returns>返回卡信息列表</returns>
        public List<AscmReadingHead> GetList(string sql)
        {
            List<AscmReadingHead> list = null;
            try
            {
                IList<AscmReadingHead> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmReadingHead>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmReadingHead>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmReadingHead)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmReadingHead> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord,string whereOther)
        {
            List<AscmReadingHead> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmReadingHead ";

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

                IList<AscmReadingHead> ilist = null;
                if (ynPage == null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmReadingHead>(sql + sort);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmReadingHead>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmReadingHead>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmReadingHead)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmReadingHead> listAscmReadingHead)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmReadingHead);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmReadingHead)", ex);
                throw ex;
            }
        }
        public void Save(AscmReadingHead ascmReadingHead)
        {
            try
            {
                //int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmReadingHead where docNumber='" + ascmReadingHead.docNumber + "'");
                //if (count == 0)
                //{
                //    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmReadingHead");
                //    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                //    {
                //        try
                //        {
                //            maxId++;
                //            ascmReadingHead.id = maxId;
                //            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmReadingHead);
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
                //    throw new Exception("已经存在员工编号\"" + ascmReadingHead.name + "\"！");
                //}
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmReadingHead)", ex);
                throw ex;
            }
        }
        public void Update(AscmReadingHead ascmReadingHead)
        {
            //int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmReadingHead where id<>" + ascmReadingHead.id + " and docNumber='" + ascmReadingHead.docNumber + "'");
            //if (count == 0)
            //{
            //    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            //    {
            //        try
            //        {
            //            YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmReadingHead>(ascmReadingHead);
            //            tx.Commit();//正确执行提交
            //        }
            //        catch (Exception ex)
            //        {
            //            tx.Rollback();//回滚
            //            YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmReadingHead)", ex);
            //            throw ex;
            //        }
            //    }
            //}
            //else
            //{
            //    throw new Exception("已经存在员工编号\"" + ascmReadingHead.name + "\"！");
            //}
        }
        public void Update(List<AscmReadingHead> list)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(list);
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Update AscmReadingHead)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmReadingHead ascmReadingHead = Get(id);
                Delete(ascmReadingHead);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmReadingHead ascmReadingHead)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmReadingHead>(ascmReadingHead);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmReadingHead)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmReadingHead> listAscmReadingHead)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmReadingHead);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmReadingHead)", ex);
                throw ex;
            }
        }
    }
}
