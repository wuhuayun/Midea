using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Services.GetMaterialManage
{
    public class AscmGenerateTaskRuleService
    {
        private static AscmGenerateTaskRuleService service;
        public static AscmGenerateTaskRuleService GetInstance()
        {
            if (service == null)
                service = new AscmGenerateTaskRuleService();
            return service;
        }

        public AscmGenerateTaskRule Get(int id)
        {
            AscmGenerateTaskRule ascmGenerateTaskRule = null;
            try
            {
                ascmGenerateTaskRule = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmGenerateTaskRule>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmAllocateRule)", ex);
                throw ex;
            }
            return ascmGenerateTaskRule;
        }

        public List<AscmGenerateTaskRule> GetList(string sql)
        {
            List<AscmGenerateTaskRule> list = null;
            try
            {
                IList<AscmGenerateTaskRule> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGenerateTaskRule>(sql);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGenerateTaskRule>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmGenerateTaskRule)", ex);
                throw ex;
            }

            return list;
        }

        public List<AscmGenerateTaskRule> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmGenerateTaskRule> list = null;

            try
            {
                string sql = "from AscmGenerateTaskRule";
                string where = "", whereQueryWord = "";

                string sort = " order by id";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = "others like '%" + queryWord.Trim() + "%' or tip like '%" + queryWord.Trim() + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                whereQueryWord = "isEnable = '是'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmGenerateTaskRule> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGenerateTaskRule>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGenerateTaskRule>(sql);

                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGenerateTaskRule>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmGenerateTaskRule)", ex);
                throw ex;
            }

            return list;
        }

        public void Save(List<AscmGenerateTaskRule> listAscmGenerateTaskRule)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmGenerateTaskRule);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmGenerateTaskRule)", ex);
                throw ex;
            }
        }

        public void Save(AscmGenerateTaskRule ascmGenerateTaskRule)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmGenerateTaskRule);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmGenerateTaskRule)", ex);
                throw ex;
            }
        }

        public void Update(List<AscmGenerateTaskRule> listAscmGenerateTaskRule)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmGenerateTaskRule);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmGenerateTaskRule)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmGenerateTaskRule)", ex);
                throw ex;
            }
        }

        public void Update(AscmGenerateTaskRule ascmGenerateTaskRule)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmGenerateTaskRule>(ascmGenerateTaskRule);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmGenerateTaskRule)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmGenerateTaskRule)", ex);
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                AscmGenerateTaskRule ascmGenerateTaskRule = Get(id);
                Delete(ascmGenerateTaskRule);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(AscmGenerateTaskRule ascmGenerateTaskRule)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmGenerateTaskRule>(ascmGenerateTaskRule);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmGenerateTaskRule)", ex);
                throw ex;
            }
        }

        public void Delete(List<AscmGenerateTaskRule> listAscmGenerateTaskRule)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmGenerateTaskRule);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmGenerateTaskRule)", ex);
            }
        }


        public void SetRelatedRanker(List<AscmGenerateTaskRule> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmGenerateTaskRule ascmGenerateTaskRule in list)
                {
                    if (!string.IsNullOrEmpty(ids) && !string.IsNullOrEmpty(ascmGenerateTaskRule.relatedRanker))
                        ids += ",";
                    if (!string.IsNullOrEmpty(ascmGenerateTaskRule.relatedRanker))
                        ids += ascmGenerateTaskRule.relatedRanker;
                }

                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "userId");
                if (!string.IsNullOrEmpty(where))
                {
                    string sql = "from AscmUserInfo where " + where;
                    IList<AscmUserInfo> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUserInfo>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmUserInfo> listAscmUserInfo = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUserInfo>(ilist);
                        foreach (AscmGenerateTaskRule ascmGenerateTaskRule in list)
                        {
                            if (!string.IsNullOrEmpty(ascmGenerateTaskRule.relatedRanker))
                                ascmGenerateTaskRule.ascmUserInfo = listAscmUserInfo.Find(e => e.userId == ascmGenerateTaskRule.relatedRanker);
                        }
                    }
                }
            }
        }
    }
}
