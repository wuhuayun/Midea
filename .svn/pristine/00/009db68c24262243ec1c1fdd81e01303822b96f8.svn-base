using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Services.FromErp
{
    public class AscmWipScheduleGroupsService
    {
        private static AscmWipScheduleGroupsService ascmWipScheduleGroupsServices;
        public static AscmWipScheduleGroupsService GetInstance()
        {
            //return ascmWipScheduleGroupsInfoServices ?? new AscmWipScheduleGroupsService();
            if (ascmWipScheduleGroupsServices == null)
                ascmWipScheduleGroupsServices = new AscmWipScheduleGroupsService();
            return ascmWipScheduleGroupsServices;
        }

        public AscmWipScheduleGroups Get(int id)
        {
            AscmWipScheduleGroups ascmWipScheduleGroups = null;
            try
            {
                ascmWipScheduleGroups = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWipScheduleGroups>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWipScheduleGroups)", ex);
                throw ex;
            }
            return ascmWipScheduleGroups;
        }
        public List<AscmWipScheduleGroups> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmWipScheduleGroups> list = null;
            try
            {
                string sort = " order by scheduleGroupId ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmWipScheduleGroups ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    queryWord = queryWord.Trim().Replace(" ", "%");
                    whereQueryWord = " upper(scheduleGroupName) like '%" + queryWord.Trim().ToUpper() + "%'";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmWipScheduleGroups> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipScheduleGroups>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipScheduleGroups>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipScheduleGroups>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWipScheduleGroups)", ex);
                throw ex;
            }
            return list;
        }
    }
}
