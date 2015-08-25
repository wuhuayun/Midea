using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;

namespace MideaAscm.Services.ContainerManage
{
    public class AscmTagManageService
    {
        private static AscmTagManageService ascmTagManageService;
        public static AscmTagManageService GetInstance()
        {
            //return ascmRfidServices ?? new AscmRfidService();
            if (ascmTagManageService == null)
                ascmTagManageService = new AscmTagManageService();
            return ascmTagManageService;
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
            if (ascmRfid == null)
                return ascmRfid;
            else if (!string.IsNullOrEmpty(ascmRfid.bindType))
                ascmRfid.bindType = ascmRfid.bindType.ToUpper();

            return ascmRfid;
        }

        public List<AscmRfid> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
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
                    whereQueryWord = " (id like '%" + queryWord.Trim() + "%' or status like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                //IList<AscmEmployee> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmployee>(sql + sort);
                IList<AscmRfid> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmRfid>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmRfid>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmRfid>(ilist);
                    //SetDepartment(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmRfid)", ex);
                throw ex;
            }
            return list;
        }

        public void Save(AscmRfid ascmRfid)
        {
            try
            {
                int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmRfid where id='" + ascmRfid.id + "'");
                if (count == 0)
                {
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(organizationId) from AscmRfid");
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            maxId++;
                            ascmRfid.organizationId = maxId;
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmRfid);
                            tx.Commit();//正确执行提交
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            throw ex;
                        }
                    }
                }
                else
                {
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("已存在(Save AscmRfid)"+ascmRfid.id.ToString());
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmRfid)", ex);
                throw ex;
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

        public void Update(AscmRfid ascmRfid)
        {
            int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmRfid where id='" + ascmRfid.id + "'");
            if (count > 0)
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmRfid>(ascmRfid);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmRfid)", ex);
                        throw ex;
                    }
                }
            }
            else
            {
                throw new Exception("标签SN不存在，无法更新\"" + ascmRfid.id + "\"！");
            }
        }
    }
}
