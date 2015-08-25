using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Dal.Warehouse.Entities;

namespace MideaAscm.Services.FromErp
{
    public class AscmWipEntitiesService
    {
        private static AscmWipEntitiesService ascmWipEntitiesServices;
        public static AscmWipEntitiesService GetInstance()
        {
            if (ascmWipEntitiesServices == null)
                ascmWipEntitiesServices = new AscmWipEntitiesService();
            return ascmWipEntitiesServices;
        }
        public AscmWipEntities Get(int id)
        {
            AscmWipEntities ascmDeliveryNotify = null;
            try
            {
                ascmDeliveryNotify = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWipEntities>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWipEntities)", ex);
                throw ex;
            }
            return ascmDeliveryNotify;
        }
        public List<AscmWipEntities> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmWipEntities> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmWipEntities";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    int index = queryWord.IndexOf('%');
                    if (index >= 0)
                        whereQueryWord = " (upper(name) like '" + queryWord.Trim().ToUpper() + "%')";
                    else
                        whereQueryWord = " (upper(name) like '%" + queryWord.Trim().ToUpper() + "%')";                 
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }

                IList<AscmWipEntities> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWipEntities)", ex);
                throw ex;
            }
            return list;
        }

        /// <summary>判断作业是否为内/外机</summary>
        public AscmWipEntities.WipEntityType GetWipEntityType(string wipEntityName)
        {
            if (string.IsNullOrWhiteSpace(wipEntityName))
                return AscmWipEntities.WipEntityType.none;

            if (wipEntityName.Length > 3)
                wipEntityName = wipEntityName.Substring(0, wipEntityName.Length - 3);

            int nIndex = wipEntityName.ToUpper().LastIndexOf('N');
            int wIndex = wipEntityName.ToUpper().LastIndexOf('W');
            if ( nIndex > 0 && nIndex > wIndex)
                return AscmWipEntities.WipEntityType.withinTheMachine;
            else if (wIndex > 0 && wIndex > nIndex)
                return AscmWipEntities.WipEntityType.outsideTheMachine;
            else
                return AscmWipEntities.WipEntityType.none;
        }

        /// <summary>获取作业ID</summary>
        public int GetWipEntityId(string wipEntityName)
        {
            int wipEntityId = 0;

            try
            {
                if (!string.IsNullOrEmpty(wipEntityName))
                {
                    IList<AscmWipEntities> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>("from AscmWipEntities where name = '" + wipEntityName + "'");
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmWipEntities> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilist);

                        wipEntityId = list[0].wipEntityId;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWipEntities)", ex);
                throw ex;
            }

            return wipEntityId;
        }
    }
}
