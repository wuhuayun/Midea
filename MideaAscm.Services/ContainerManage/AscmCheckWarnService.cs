using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using System.Data;
using Oracle.DataAccess.Client;

namespace MideaAscm.Services.ContainerManage
{
    public class AscmCheckWarnService
    {
        private static AscmCheckWarnService service;
        public static AscmCheckWarnService GetInstance()
        {
            if (service == null)
                service = new AscmCheckWarnService();
            return service;
        }

        /// <summary>
        /// 超期预警数据查询
        /// 覃小华于2013/07/15修改
        /// </summary>
        /// <param name="ynPage"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="queryWord"></param>
        /// <param name="whereOther"></param>
        /// <param name="extendedTime"></param>
        /// <returns></returns>
        public List<Dal.SupplierPreparation.Entities.AscmContainer> Getlist(YnPage ynPage, string sortName, string sortOrder, string whereOther, string DateRequired)
        {
            List<Dal.SupplierPreparation.Entities.AscmContainer> list = null;
            try
            {
                string strHql = @"select new AscmContainer(container,(suppli.name),(to_char(to_date(container.storeInTime,'yyyy-MM-dd HH24:mi:ss')+suppli.warnHours/24,'yyyy-MM-dd HH24:mi:ss')),(ceil(((to_date('" + System.DateTime.Now.ToString() + "','yyyy-MM-dd HH24:mi:ss')-suppli.warnHours/24)-to_date(container.storeInTime, 'yyyy-MM-dd HH24:mi:ss'))*24)))  from AscmContainer container, AscmSupplier suppli  where container.storeInTime is not null and  container.supplierId=suppli.id  and  (suppli.warnHours is not null and suppli.warnHours!=0) and ceil(((to_date('" + System.DateTime.Now.ToString() + "','yyyy-MM-dd HH24:mi:ss')-suppli.warnHours/24)-to_date(container.storeInTime, 'yyyy-MM-dd HH24:mi:ss'))*24)>-25 and (container.place not in (select to_char(id) from  AscmReadingHead where ip='0.0.0.0') or container.place is null)";
                string strHql1 = "from AscmContainer container, AscmSupplier suppli where container.storeInTime is not null and  container.supplierId=suppli.id  and  (suppli.warnHours is not null or suppli.warnHours!=0) and ceil(((to_date('" + System.DateTime.Now.ToString() + "','yyyy-MM-dd HH24:mi:ss')-suppli.warnHours/24)-to_date(container.storeInTime, 'yyyy-MM-dd HH24:mi:ss'))*24)>-25 and (container.place not in (select to_char(id) from  AscmReadingHead where address= '容器监管中心' and ip='0.0.0.0') or container.place is null)";
                if (!string.IsNullOrEmpty(DateRequired))
                {
                    DateTime DatereadTime = new DateTime();
                    if (!string.IsNullOrEmpty(DateRequired) && DateTime.TryParse(DateRequired, out DatereadTime))
                    {
                        strHql = @"select new AscmContainer(container,(suppli.name),to_char(to_date(container.storeInTime,'yyyy-MM-dd HH24:mi:ss')+suppli.warnHours/24,'yyyy-MM-dd HH24:mi:ss'),(ceil(((to_date('" + DatereadTime.ToString() + "','yyyy-MM-dd HH24:mi:ss')-suppli.warnHours/24)-to_date(container.storeInTime, 'yyyy-MM-dd HH24:mi:ss'))*24)))  from AscmContainer container, AscmSupplier suppli  where container.storeInTime is not null and  container.supplierId=suppli.id  and  (suppli.warnHours is not null and suppli.warnHours!=0) and ceil(((to_date('" + DatereadTime.ToString() + "','yyyy-MM-dd HH24:mi:ss')-suppli.warnHours/24)-to_date(container.storeInTime, 'yyyy-MM-dd HH24:mi:ss'))*24)>-25 and (container.place not in (select to_char(id) from  AscmReadingHead where ip='0.0.0.0') or container.place is null)";
                        strHql1 = "from AscmContainer container, AscmSupplier suppli where container.storeInTime is not null and  container.supplierId=suppli.id  and  (suppli.warnHours is not null and suppli.warnHours!=0) and ceil(((to_date('" + DatereadTime.ToString() + "','yyyy-MM-dd HH24:mi:ss')-suppli.warnHours/24)-to_date(container.storeInTime, 'yyyy-MM-dd HH24:mi:ss'))*24)>-25 and (container.place not in (select to_char(id) from  AscmReadingHead where ip='0.0.0.0') or container.place is null)";
                    }                   
                }
                string sort = " order by store.id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string where = "";            
                //in (select id from AscmSupplier where warnHours is not null or warnHours!=0) and sn not in (SELECT containerId  FROM AscmStoreInOut a WHERE readTime=(SELECT max(readTime) FROM AscmStoreInOut WHERE containerId=a.containerId) and  direction='STOREOUT')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                if (!string.IsNullOrEmpty(where))
                {
                    strHql += " and" + where;
                    strHql1 += " and" + where;
                }
                IList<Dal.SupplierPreparation.Entities.AscmContainer> ilist = 
                    
                    //YnDaoHelper.GetInstance().nHibernateHelper.Find<Dal.SupplierPreparation.Entities.AscmContainer>(strHql1);
                    YnDaoHelper.GetInstance().nHibernateHelper.Find<Dal.SupplierPreparation.Entities.AscmContainer>(strHql,
                    strHql1, ynPage);
                if (ilist != null && ilist.Count>0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<Dal.SupplierPreparation.Entities.AscmContainer>(ilist);
                    SetAscmReadingHead(list);

                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWipEntities)", ex);
                throw ex;
            }
            return list;
        }
        /// <summary>
        /// 设置地址
        /// 2013/08/22
        /// </summary>
        /// <param name="list"></param>
        private void SetAscmReadingHead(List<Dal.SupplierPreparation.Entities.AscmContainer> list)
        {

            System.Text.StringBuilder sbSn = new System.Text.StringBuilder();
            sbSn.Clear();
            var t = list.Select(e => e.place).Distinct();
            foreach (string ascmTagLog in t)
            {
                if (!string.IsNullOrEmpty(ascmTagLog))
                {
                    if (sbSn.Length != 0)
                    {
                        sbSn.Append(",");
                    }
                    sbSn.AppendFormat("{0}", ascmTagLog);
                }
            }
            if (string.IsNullOrEmpty(sbSn.ToString()))
            {
                return;
            }
            IList<MideaAscm.Dal.Base.Entities.AscmReadingHead> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<MideaAscm.Dal.Base.Entities.AscmReadingHead>("from AscmReadingHead where id in (" + sbSn.ToString() + ")");
            List<MideaAscm.Dal.Base.Entities.AscmReadingHead> listAscmReadingHead = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList(ilist);
            if (ilist.Count > 0)
            {
                foreach (Dal.SupplierPreparation.Entities.AscmContainer ascmTagLog in list)
                {
                    ascmTagLog.ascmReadingHead = listAscmReadingHead.Find(e => e.id.ToString() == ascmTagLog.place);
                }

            }
        }
        /// <summary>
        /// 超期预警数据查询Excel导出
        /// 覃小华于2013/10/22修改
        /// </summary>
        /// <param name="ynPage"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="queryWord"></param>
        /// <param name="whereOther"></param>
        /// <param name="extendedTime"></param>
        /// <returns></returns>
        public List<Dal.SupplierPreparation.Entities.AscmContainer> GetlistForExcellExport(string whereOther, string DateRequired)
        {
            List<Dal.SupplierPreparation.Entities.AscmContainer> list = null;
            try
            {
                string strHql = @"select new AscmContainer(container,(suppli.name),(to_char(to_date(container.storeInTime,'yyyy-MM-dd HH24:mi:ss')+suppli.warnHours/24,'yyyy-MM-dd HH24:mi:ss')),(ceil(((to_date('" + System.DateTime.Now.ToString() + "','yyyy-MM-dd HH24:mi:ss')-suppli.warnHours/24)-to_date(container.storeInTime, 'yyyy-MM-dd HH24:mi:ss'))*24)))  from AscmContainer container, AscmSupplier suppli  where container.storeInTime is not null and  container.supplierId=suppli.id  and  (suppli.warnHours is not null and suppli.warnHours!=0) and ceil(((to_date('" + System.DateTime.Now.ToString() + "','yyyy-MM-dd HH24:mi:ss')-suppli.warnHours/24)-to_date(container.storeInTime, 'yyyy-MM-dd HH24:mi:ss'))*24)>-25 and (container.place not in (select to_char(id) from  AscmReadingHead where ip='0.0.0.0') or container.place is null)";
                //string strHql1 = "from AscmContainer container, AscmSupplier suppli where container.storeInTime is not null and  container.supplierId=suppli.id  and  (suppli.warnHours is not null or suppli.warnHours!=0) and ceil(((to_date('" + System.DateTime.Now.ToString() + "','yyyy-MM-dd HH24:mi:ss')-suppli.warnHours/24)-to_date(container.storeInTime, 'yyyy-MM-dd HH24:mi:ss'))*24)>-25 and (container.place not in (select to_char(id) from  AscmReadingHead where address= '容器监管中心' and ip='0.0.0.0') or container.place is null)";
                if (!string.IsNullOrEmpty(DateRequired))
                {
                    DateTime DatereadTime = new DateTime();
                    if (!string.IsNullOrEmpty(DateRequired) && DateTime.TryParse(DateRequired, out DatereadTime))
                    {
                        strHql = @"select new AscmContainer(container,(suppli.name),to_char(to_date(container.storeInTime,'yyyy-MM-dd HH24:mi:ss')+suppli.warnHours/24,'yyyy-MM-dd HH24:mi:ss'),(ceil(((to_date('" + DatereadTime.ToString() + "','yyyy-MM-dd HH24:mi:ss')-suppli.warnHours/24)-to_date(container.storeInTime, 'yyyy-MM-dd HH24:mi:ss'))*24)))  from AscmContainer container, AscmSupplier suppli  where container.storeInTime is not null and  container.supplierId=suppli.id  and  (suppli.warnHours is not null and suppli.warnHours!=0) and ceil(((to_date('" + DatereadTime.ToString() + "','yyyy-MM-dd HH24:mi:ss')-suppli.warnHours/24)-to_date(container.storeInTime, 'yyyy-MM-dd HH24:mi:ss'))*24)>-25 and (container.place not in (select to_char(id) from  AscmReadingHead where ip='0.0.0.0') or container.place is null)";
                      //  strHql1 = "from AscmContainer container, AscmSupplier suppli where container.storeInTime is not null and  container.supplierId=suppli.id  and  (suppli.warnHours is not null and suppli.warnHours!=0) and ceil(((to_date('" + DatereadTime.ToString() + "','yyyy-MM-dd HH24:mi:ss')-suppli.warnHours/24)-to_date(container.storeInTime, 'yyyy-MM-dd HH24:mi:ss'))*24)>-25 and (container.place not in (select to_char(id) from  AscmReadingHead where ip='0.0.0.0') or container.place is null)";
                    }
                }
                //string sort = " order by store.id ";
                string where = "";
                //in (select id from AscmSupplier where warnHours is not null or warnHours!=0) and sn not in (SELECT containerId  FROM AscmStoreInOut a WHERE readTime=(SELECT max(readTime) FROM AscmStoreInOut WHERE containerId=a.containerId) and  direction='STOREOUT')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                if (!string.IsNullOrEmpty(where))
                {
                    strHql += " and" + where;
                   // strHql1 += " and" + where;
                }
                IList<Dal.SupplierPreparation.Entities.AscmContainer> ilist =

                    //YnDaoHelper.GetInstance().nHibernateHelper.Find<Dal.SupplierPreparation.Entities.AscmContainer>(strHql1);
                    YnDaoHelper.GetInstance().nHibernateHelper.Find<Dal.SupplierPreparation.Entities.AscmContainer>(strHql
                 );
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<Dal.SupplierPreparation.Entities.AscmContainer>(ilist);

                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWipEntities)", ex);
                throw ex;
            }
            return list;
        }
        /// <summary>
        /// 预警规则
        /// </summary>
        /// <param name="ynPage"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="queryWord"></param>
        /// <param name="whereOther"></param>
        /// <returns></returns>
        public List<AscmSupplier> GetWarnRuleList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmSupplier> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmSupplier where warnHours is not null and  warnHours!=0";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    queryWord = queryWord.Trim().Replace(" ", "%");
                    whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or docNumber like '%" + queryWord.Trim() + "%'";
                    int _fValue = -1;
                    if (int.TryParse(queryWord, out _fValue))
                    {
                        whereQueryWord += " or id like '" + queryWord + "%'";
                    }
                    whereQueryWord += ")";
                }

                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += "and" + where;
                //IList<AscmSupplier> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql + sort);
                IList<AscmSupplier> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmSupplier)", ex);
                throw ex;
            }
            return list;
        }
    }
}
