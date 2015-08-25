using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;

namespace MideaAscm.Services.Warehouse
{
    public class AscmWhTeamService
    {
        private static AscmWhTeamService _Instance = new AscmWhTeamService();
        public static AscmWhTeamService Instance
        {
            get 
            {
                return _Instance;
            }      
        }

        public AscmWhTeam Get(int id)
        {
            AscmWhTeam AscmWhTeam = null;
            try
            {
                AscmWhTeam = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWhTeam>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error(" 查询失败 (Get AscmWhTeam) ", ex);
                throw ex;
            }
            return AscmWhTeam;
        }

        public List<AscmWhTeam> GetList(string strWhere)
        {
            List<AscmWhTeam> list = null;
            try
            {
                string sql = " from AscmWhTeam ";
                if (!string.IsNullOrEmpty(strWhere)) sql += " where 1=1 " + strWhere;

                sql += " order by sortNo ";

                IList<AscmWhTeam> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWhTeam>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWhTeam>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWhTeam)", ex);
                throw ex;
            }

            return list;
        }

        public void Save(AscmWhTeam AscmWhTeam)
        {
            try
            {
                int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmWhTeam where id=" + AscmWhTeam.id);
                if (count == 0)
                {
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            int id = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId(" select max(id) from AscmWhTeam ") + 1;
                            AscmWhTeam.id = id;
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(AscmWhTeam);
                            tx.Commit();        //正确执行提交
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();      //回滚
                            throw ex;
                        }
                    }
                }
                else
                {
                    throw new Exception("已经存在组别！");
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWhTeam)", ex);
                throw ex;
            }
        }

        public void Update(AscmWhTeam AscmWhTeam)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWhTeam>(AscmWhTeam);
                    tx.Commit();    //正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();  //回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWhTeam)", ex);
                    throw ex;
                }
            }
        }

        public void Delete(int id)
        {
            try
            {
                AscmWhTeam AscmWhTeam = Get(id);
                Delete(AscmWhTeam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(AscmWhTeam AscmWhTeam)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWhTeam>(AscmWhTeam);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWhTeam)", ex);
                throw ex;
            }
        }

        public void Delete(List<AscmWhTeam> listAscmWhTeam)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWhTeam);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWhTeam List)", ex);
                throw ex;
            }
        }
    }
}
