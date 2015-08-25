using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Services.FromErp;
using Oracle.DataAccess.Client;
using System.Data;

namespace MideaAscm.Services.Warehouse
{
    public class AscmWmsPreparationDetailService
    {
        private static AscmWmsPreparationDetailService service;
        public static AscmWmsPreparationDetailService GetInstance()
        {
            if (service == null)
                service = new AscmWmsPreparationDetailService();
            return service;
        }

        public AscmWmsPreparationDetail Get(int id)
        {
            AscmWmsPreparationDetail ascmWmsPreparationDetail = null;
            try
            {
                ascmWmsPreparationDetail = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsPreparationDetail>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsPreparationDetail)", ex);
                throw ex;
            }
            return ascmWmsPreparationDetail;
        }
        public List<AscmWmsPreparationDetail> GetList(string sql)
        {
            List<AscmWmsPreparationDetail> list = null;
            try
            {
                IList<AscmWmsPreparationDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsPreparationDetail>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsPreparationDetail>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsPreparationDetail)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsPreparationDetail> GetList(string sortName, string sortOrder, int mainId, string queryWord, string whereOther)
        {
            return GetList(null, sortName, sortOrder, mainId, queryWord, whereOther);
        }
        public List<AscmWmsPreparationDetail> GetList(YnPage ynPage, string sortName, string sortOrder, int mainId, string queryWord, string whereOther)
        {
            List<AscmWmsPreparationDetail> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                string sql = "from AscmWmsPreparationDetail ";

                string locationDocNumber = "select docNumber from AscmWarelocation where id=a.warelocationId";
                string wipEntityName = "select name from AscmWipEntities where wipEntityId=a.wipEntityId";
                string containerBindNumber = "select ascmPreparedQuantity from AscmWipRequirementOperations where wipEntityId=a.wipEntityId and inventoryItemId=a.materialId";
                string sql1 = "select new AscmWmsPreparationDetail(a,(" + locationDocNumber + "),(" + wipEntityName + "),(" + containerBindNumber + ")) from AscmWmsPreparationDetail a";

                string where = "", whereQueryWord = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, " mainId=" + mainId);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    sql1 += " where " + where;
                }
                IList<AscmWmsPreparationDetail> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsPreparationDetail>(sql1 + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsPreparationDetail>(sql1 + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsPreparationDetail>(ilist);
                    SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsPreparationDetail)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsPreparationDetail> GetBomBindNumberList(int mainId)
        {
            string hql = "select new AscmWmsPreparationDetail(awpd,awro.ascmPreparedQuantity) from AscmWmsPreparationDetail awpd,AscmWipRequirementOperations awro";
            string where = "";
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.wipEntityId=awro.wipEntityId");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.materialId=awro.inventoryItemId");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.mainId=" + mainId);
            hql += " where " + where;
            return GetList(hql);
        }
        public List<AscmWmsPreparationDetail> GetListByMainId(int mainId)
        {
            return GetList("", "", mainId, "", "");
        }

        /// <summary>
        /// 获取作业备料明细列表
        /// </summary>
        /// <param name="mainId">作业备料主表ID</param>
        /// <param name="materialId">物料ID</param>
        /// <returns>返回作业备料明细列表</returns>
        public List<AscmWmsPreparationDetail> GetWipJobPreparationDetail(int mainId)
        {
            List<AscmWmsPreparationDetail> list = null;
            try
            {
                string hql = string.Format("select new AscmWmsPreparationDetail(awpd,ami.docNumber,ami.description,ami.unit,{0},awe.name,awro.ascmPreparedQuantity) from AscmWmsPreparationDetail awpd,AscmWipEntities awe,AscmWipRequirementOperations awro,AscmMaterialItem ami", "(select docNumber from AscmWarelocation where id=awpd.warelocationId)");
                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.wipEntityId=awe.wipEntityId");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.wipEntityId=awro.wipEntityId");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.materialId=awro.inventoryItemId");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.materialId=ami.id");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.mainId=" + mainId);
                hql += " where " + where;
                hql += " order by ami.docNumber";
                IList<AscmWmsPreparationDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsPreparationDetail>(hql);
                if (ilist != null)
                    list = ilist.ToList();
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsPreparationDetail)", ex);
                throw ex;
            }
            return list;
        }
        /// <summary>
        /// 获取备料明细物料合计的列表(针对需求备料)
        /// </summary>
        /// <param name="mainId">备料主表ID</param>
        /// <returns>返回备料明细物料合计的列表</returns>
        public List<AscmWmsPreparationDetail> GetSumList(int mainId)
        {
            List<AscmWmsPreparationDetail> list = null;
            try
            {
                string hql = "select new AscmWmsPreparationDetail(ami.id,ami.docNumber,ami.description,ami.unit,sum(awpd.planQuantity),sum(awro.ascmPreparedQuantity)) from AscmWmsPreparationDetail awpd,AscmWipRequirementOperations awro,AscmMaterialItem ami";
                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.wipEntityId=awro.wipEntityId");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.materialId=awro.inventoryItemId");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.materialId=ami.id");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.mainId=" + mainId);
                hql += " where " + where;
                hql += " group by ami.id,ami.docNumber,ami.description,ami.unit";
                hql += " order by ami.docNumber";
                IList<AscmWmsPreparationDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsPreparationDetail>(hql);
                if (ilist != null)
                    list = ilist.ToList();
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsPreparationDetail)", ex);
                throw ex;
            }
            return list;
        }
        /// <summary>
        /// 获取需求备料明细列表
        /// </summary>
        /// <param name="mainId">需求备料主表ID</param>
        /// <param name="materialId">物料ID</param>
        /// <returns>返回需求备料明细列表</returns>
        public List<AscmWmsPreparationDetail> GetWipRequirePreparationDetail(int mainId, int? materialId)
        {
            List<AscmWmsPreparationDetail> list = null;
            try
            {
                string hql = string.Format("select new AscmWmsPreparationDetail(awpd,{0},awe.name,awro.ascmPreparedQuantity) from AscmWmsPreparationDetail awpd,AscmWipEntities awe,AscmWipRequirementOperations awro", "(select docNumber from AscmWarelocation where id=awpd.warelocationId)");
                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.wipEntityId=awe.wipEntityId");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.wipEntityId=awro.wipEntityId");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.materialId=awro.inventoryItemId");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.mainId=" + mainId);
                if (materialId.HasValue)
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.materialId=" + materialId.Value);
                hql += " where " + where;
                hql += " order by awe.name ";
                IList<AscmWmsPreparationDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsPreparationDetail>(hql);
                if (ilist != null)
                {
                    list = ilist.ToList();
                    //根据业务要求，需求备料明细要设置作业装配件信息
                    var wipEntityIds = list.Select(P => P.wipEntityId).Distinct();
                    var count = wipEntityIds.Count();
                    string ids = string.Empty;
                    for (int i = 0; i < count; i++)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += wipEntityIds.ElementAt(i);
                        if ((i + 1) % 500 == 0 || (i + 1) == count)
                        {
                            if (!string.IsNullOrEmpty(ids))
                            {
                                hql = "select new AscmWipDiscreteJobs(awdj.wipEntityId,ami.docNumber,ami.description) from AscmWipDiscreteJobs awdj,AscmMaterialItem ami where awdj.primaryItemId=ami.id and awdj.wipEntityId in(" + ids + ")";
                                IList<AscmWipDiscreteJobs> ilistWipDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(hql);
                                if (ilistWipDiscreteJobs != null && ilistWipDiscreteJobs.Count > 0)
                                {
                                    List<AscmWipDiscreteJobs> listWipDiscreteJobs = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobs>(ilistWipDiscreteJobs);
                                    foreach (AscmWmsPreparationDetail wmsPreparationDetail in list)
                                        wmsPreparationDetail.ascmWipDiscreteJobs = listWipDiscreteJobs.Find(P => P.wipEntityId == wmsPreparationDetail.wipEntityId);
                                }
                            }
                            ids = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsPreparationDetail)", ex);
                throw ex;
            }
            return list;
        }
        
        /// <summary>
        /// 获取备料明细列表
        /// </summary>
        /// <param name="mainIds">备料主表ID连接字符串</param>
        /// <param name="whereOther">其他查询条件</param>
        /// <returns>返回备料明细列表</returns>
        public List<AscmWmsPreparationDetail> GetListByMainIds(string mainIds, string whereOther = "")
        {
            if (string.IsNullOrEmpty(mainIds))
                return null;
            List<AscmWmsPreparationDetail> list = new List<AscmWmsPreparationDetail>();
            var ieMainId = mainIds.Split(',').Distinct();
            int count = ieMainId.Count();
            string _mainIds = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(_mainIds))
                    _mainIds += ",";
                _mainIds += ieMainId.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(_mainIds))
                    {
                        string hql = "from AscmWmsPreparationDetail";
                        string where = "";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "mainId in(" + _mainIds + ")");
                        if (!string.IsNullOrEmpty(whereOther))
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                        hql += " where " + where;
                        List<AscmWmsPreparationDetail> _list = GetList(hql);
                        if (_list != null && _list.Count > 0)
                            list.AddRange(_list);
                    }
                    _mainIds = string.Empty;
                }
            }
            return list;
        }
        /// <summary>
        /// 获取设置了作业名称的备料明细列表
        /// </summary>
        /// <param name="mainIds">备料主表ID连接字符串</param>
        /// <returns>返回设置了作业名称的备料明细列表</returns>
        public List<AscmWmsPreparationDetail> GetSetWipEntityNameList(string mainIds)
        {
            if (string.IsNullOrEmpty(mainIds))
                return null;
            List<AscmWmsPreparationDetail> list = new List<AscmWmsPreparationDetail>();
            var ieMainId = mainIds.Split(',').Distinct();
            int count = ieMainId.Count();
            string _mainIds = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(_mainIds))
                    _mainIds += ",";
                _mainIds += ieMainId.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(_mainIds))
                    {
                        string hql = "select new AscmWmsPreparationDetail(awpd,awe.name,0M) from AscmWmsPreparationDetail awpd,AscmWipEntities awe";
                        string where = "";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.wipEntityId=awe.wipEntityId");
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.mainId in(" + _mainIds + ")");
                        hql += " where " + where;
                        List<AscmWmsPreparationDetail> _list = GetList(hql);
                        if (_list != null && _list.Count > 0)
                            list.AddRange(_list);
                    }
                    _mainIds = string.Empty;
                }
            }
            return list;
        }
        /// <summary>
        /// 获取设置了容器绑定数量的备料明细列表
        /// </summary>
        /// <param name="mainIds">备料主表ID连接字符串</param>
        /// <param name="whereOther">其他查询条件</param>
        /// <returns>返回设置了容器绑定数量的备料明细列表</returns>
        public List<AscmWmsPreparationDetail> GetContainerBindNumberList(string mainIds, string whereOther = "")
        {
            if (string.IsNullOrEmpty(mainIds))
                return null;
            List<AscmWmsPreparationDetail> list = new List<AscmWmsPreparationDetail>();
            var ieMainId = mainIds.Split(',').Distinct();
            int count = ieMainId.Count();
            string _mainIds = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(_mainIds))
                    _mainIds += ",";
                _mainIds += ieMainId.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(_mainIds))
                    {
                        string hql = string.Format("select new AscmWmsPreparationDetail(awpd,{0}) from AscmWmsPreparationDetail awpd", "(select sum(quantity) from AscmWmsContainerDelivery awcd where awcd.preparationMainId=awpd.mainId and awcd.wipEntityId=awpd.wipEntityId and awcd.materialId=awpd.materialId)");
                        string where = "";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "mainId in(" + _mainIds + ")");
                        if (!string.IsNullOrEmpty(whereOther))
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                        hql += " where " + where;
                        List<AscmWmsPreparationDetail> _list = GetList(hql);
                        if (_list != null && _list.Count > 0)
                            list.AddRange(_list);
                    }
                    _mainIds = string.Empty;
                }
            }
            return list;
        }
        /// <summary>
        /// 获取设置了容器绑定数量和作业名称的备料明细列表
        /// </summary>
        /// <param name="mainIds">备料主表ID连接字符串</param>
        /// <returns>返回设置了容器绑定数量和作业名称的备料明细列表</returns>
        public List<AscmWmsPreparationDetail> GetContainerBindNumberList(string mainIds)
        {
            if (string.IsNullOrEmpty(mainIds))
                return null;
            List<AscmWmsPreparationDetail> list = new List<AscmWmsPreparationDetail>();
            var ieMainId = mainIds.Split(',').Distinct();
            int count = ieMainId.Count();
            string _mainIds = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(_mainIds))
                    _mainIds += ",";
                _mainIds += ieMainId.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(_mainIds))
                    {
                        string hql = string.Format("select new AscmWmsPreparationDetail(awpd,awe.name,{0}) from AscmWmsPreparationDetail awpd,AscmWipEntities awe", "(select sum(quantity) from AscmWmsContainerDelivery awcd where awcd.preparationMainId=awpd.mainId and awcd.wipEntityId=awpd.wipEntityId and awcd.materialId=awpd.materialId)");
                        string where = "";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.wipEntityId=awe.wipEntityId");
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.mainId in(" + _mainIds + ")");
                        hql += " where " + where;
                        List<AscmWmsPreparationDetail> _list = GetList(hql);
                        if (_list != null && _list.Count > 0)
                            list.AddRange(_list);
                    }
                    _mainIds = string.Empty;
                }
            }
            return list;
        }
        /// <summary>
        /// 获取设置了作业BOM容器绑定数量的备料明细列表
        /// </summary>
        /// <param name="mainIds">备料主表ID连接字符串</param>
        /// <param name="whereOther">其他查询条件</param>
        /// <returns>返回设置了作业BOM容器绑定数量的备料明细列表</returns>
        public List<AscmWmsPreparationDetail> GetBomBindNumberList(string mainIds, string whereOther = "")
        {
            if (string.IsNullOrEmpty(mainIds))
                return null;
            List<AscmWmsPreparationDetail> list = new List<AscmWmsPreparationDetail>();
            var ieMainId = mainIds.Split(',').Distinct();
            int count = ieMainId.Count();
            string _mainIds = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(_mainIds))
                    _mainIds += ",";
                _mainIds += ieMainId.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(_mainIds))
                    {
                        string hql = "select new AscmWmsPreparationDetail(awpd,awro.ascmPreparedQuantity) from AscmWmsPreparationDetail awpd,AscmWipRequirementOperations awro,AscmMaterialItem ami";
                        string where = "";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.wipEntityId=awro.wipEntityId");
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.materialId=awro.inventoryItemId");
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.materialId=ami.id");
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.mainId in(" + _mainIds + ")");
                        if (!string.IsNullOrEmpty(whereOther))
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                        hql += " where " + where;
                        List<AscmWmsPreparationDetail> _list = GetList(hql);
                        if (_list != null && _list.Count > 0)
                            list.AddRange(_list);
                    }
                    _mainIds = string.Empty;
                }
            }
            return list;
        }

        public void Save(List<AscmWmsPreparationDetail> listAscmWmsPreparationDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsPreparationDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsPreparationDetail)", ex);
                throw ex;
            }
        }
        public void Save(AscmWmsPreparationDetail ascmWmsPreparationDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsPreparationDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsPreparationDetail)", ex);
                throw ex;
            }
        }
        public void Update(AscmWmsPreparationDetail ascmWmsPreparationDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWmsPreparationDetail>(ascmWmsPreparationDetail);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWmsPreparationDetail)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmWmsPreparationDetail)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmWmsPreparationDetail ascmWmsPreparationDetail = Get(id);
                Delete(ascmWmsPreparationDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmWmsPreparationDetail ascmWmsPreparationDetail)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWmsPreparationDetail>(ascmWmsPreparationDetail);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsPreparationDetail)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmWmsPreparationDetail> listAscmWmsPreparationDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsPreparationDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsPreparationDetail)", ex);
                throw ex;
            }
        }

        public void SetMaterial(List<AscmWmsPreparationDetail> list)
        {
            if (list == null || list.Count == 0)
                return;

            var materialIds = list.Select(P => P.materialId).Distinct();
            var count = materialIds.Count();
            string ids = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(ids))
                    ids += ",";
                ids += materialIds.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string sql = "from AscmMaterialItem where id in (" + ids + ")";
                        IList<AscmMaterialItem> ilistMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                        if (ilistMaterialItem != null && ilistMaterialItem.Count > 0)
                        {
                            List<AscmMaterialItem> listMaterialItem = ilistMaterialItem.ToList();
                            foreach (AscmWmsPreparationDetail preparationDetail in list)
                            {
                                AscmMaterialItem materialItem = listMaterialItem.Find(P => P.id == preparationDetail.materialId);
                                if (materialItem != null)
                                {
                                    preparationDetail.ascmMaterialItem = materialItem;
                                    preparationDetail.materialDocNumber = materialItem.docNumber;
                                    preparationDetail.materialName = materialItem.description;
                                    preparationDetail.materialUnit = materialItem.unit;
                                }
                            }
                        }
                    }
                    ids = string.Empty;
                }
            }
        }
        public void SetWipEntities(List<AscmWmsPreparationDetail> list)
        {
            if (list == null || list.Count == 0)
                return;

            var wipEntityIds = list.Select(P => P.wipEntityId).Distinct();
            var count = wipEntityIds.Count();
            string ids = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(ids))
                    ids += ",";
                ids += wipEntityIds.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string sql = "from AscmWipEntities where wipEntityId in (" + ids + ")";
                        IList<AscmWipEntities> ilistAscmWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                        if (ilistAscmWipEntities != null && ilistAscmWipEntities.Count > 0)
                        {
                            List<AscmWipEntities> listAscmWipEntities = ilistAscmWipEntities.ToList();
                            foreach (AscmWmsPreparationDetail ascmWmsPreparationDetail in list)
                            {
                                AscmWipEntities ascmWipEntities = listAscmWipEntities.Find(P => P.wipEntityId == ascmWmsPreparationDetail.wipEntityId);
                                if (ascmWipEntities != null)
                                    ascmWmsPreparationDetail.wipEntityName = ascmWipEntities.name;
                            }
                        }
                    }
                    ids = string.Empty;
                }
            }
        }
        public void SetPrepareQuantity(List<AscmWmsPreparationDetail> list)
        {
            if (list == null || list.Count == 0)
                return;

            var preparationMainIds = list.Select(P => P.wipEntityId).Distinct();
            var count = preparationMainIds.Count();
            string ids = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(ids))
                    ids += ",";
                ids += preparationMainIds.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string hql = "from AscmWmsContainerDelivery where preparationMainId in(" + preparationMainIds + ")";
                        List<AscmWmsContainerDelivery> listContainerDelivery = AscmWmsContainerDeliveryService.GetInstance().GetList(hql);
                        if (listContainerDelivery != null && listContainerDelivery.Count > 0)
                        {
                            foreach (AscmWmsPreparationDetail preparationDetail in list)
                            {
                                List<AscmWmsContainerDelivery> _listContainerDelivery = listContainerDelivery.FindAll(P => P.preparationMainId == preparationDetail.mainId && P.wipEntityId == preparationDetail.wipEntityId && P.materialId == preparationDetail.materialId);
                                if (_listContainerDelivery != null && _listContainerDelivery.Count > 0)
                                    preparationDetail.containerBindNumber = _listContainerDelivery.Sum(P => P.quantity);
                            }
                        }
                    }
                    ids = string.Empty;
                }
            }
        }
        public void SetBomPrepareQuantity(List<AscmWmsPreparationDetail> list)
        {
            if (list == null || list.Count == 0)
                return;

            var wipEntityIds = list.Select(P => P.wipEntityId).Distinct();
            var count = wipEntityIds.Count();
            string ids = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(ids))
                    ids += ",";
                ids += wipEntityIds.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(ids))
                    {
                        List<AscmWmsPreparationDetail> _list = list.FindAll(P => ids.Split(',').Contains(P.wipEntityId.ToString()));
                        string materialIds = string.Join(",", _list.Select(P => P.materialId).Distinct());
                        string hql = "from AscmWipRequirementOperations where wipEntityId in(" + ids + ") and inventoryItemId in(" + materialIds + ")";
                        IList<AscmWipRequirementOperations> ilistWipRequirementOperations = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(hql);
                        if (ilistWipRequirementOperations != null && ilistWipRequirementOperations.Count > 0)
                        {
                            List<AscmWipRequirementOperations> listWipRequirementOperations = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilistWipRequirementOperations);
                            foreach (AscmWmsPreparationDetail preparationDetail in list)
                            {
                                AscmWipRequirementOperations wipRequirementOperations = listWipRequirementOperations.Find(P => P.wipEntityId == preparationDetail.wipEntityId && P.inventoryItemId == preparationDetail.materialId);
                                if (wipRequirementOperations != null)
                                    preparationDetail.containerBindNumber = wipRequirementOperations.ascmPreparedQuantity;
                            }
                        }
                    }
                    ids = string.Empty;
                }
            }
        }
        public void SetWipDiscreteJobs(List<AscmWmsPreparationDetail> list)
        {
            if (list == null || list.Count == 0)
                return;

            var wipEntityIds = list.Select(P => P.wipEntityId).Distinct();
            var count = wipEntityIds.Count();
            string ids = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(ids))
                    ids += ",";
                ids += wipEntityIds.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string sql = "from AscmWipDiscreteJobs where wipEntityId in(" + ids + ")";
                        IList<AscmWipDiscreteJobs> ilistWipDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sql);
                        if (ilistWipDiscreteJobs != null && ilistWipDiscreteJobs.Count > 0)
                        {
                            List<AscmWipDiscreteJobs> listWipDiscreteJobs = ilistWipDiscreteJobs.ToList();
                            AscmWipDiscreteJobsService.GetInstance().SetWipEntities(listWipDiscreteJobs);
                            AscmWipDiscreteJobsService.GetInstance().SetScheduleGroups(listWipDiscreteJobs);
                            AscmWipDiscreteJobsService.GetInstance().SetMaterialItem(listWipDiscreteJobs);
                            list.ForEach(P => P.ascmWipDiscreteJobs = listWipDiscreteJobs.Find(T => T.wipEntityId == P.wipEntityId));
                        }
                    }
                    ids = string.Empty;
                }
            }
        }
        public void SetWarelocationDocNumber(List<AscmWmsPreparationDetail> list)
        {
            if (list == null || list.Count == 0)
                return;

            var warelocationIds = list.Select(P => P.warelocationId).Distinct();
            var count = warelocationIds.Count();
            string ids = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(ids))
                    ids += ",";
                ids += warelocationIds.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string sql = "from AscmWarelocation where id in (" + ids + ")";
                        IList<AscmWarelocation> ilistAscmWarelocation = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarelocation>(sql);
                        if (ilistAscmWarelocation != null && ilistAscmWarelocation.Count > 0)
                        {
                            List<AscmWarelocation> listAscmWarelocation = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarelocation>(ilistAscmWarelocation);
                            foreach (AscmWmsPreparationDetail ascmWmsPreparationDetail in list)
                            {
                                AscmWarelocation ascmWarelocation = listAscmWarelocation.Find(P => P.id == ascmWmsPreparationDetail.warelocationId);
                                if (ascmWarelocation != null)
                                    ascmWmsPreparationDetail.locationDocNumber = ascmWarelocation.docNumber;
                            }
                        }
                    }
                    ids = string.Empty;
                }
            }
        }
        /// <summary>设置库存现有量</summary>
        public void SetOnhandQuantity(List<AscmWmsPreparationDetail> list)
        {
            if (list == null && list.Count == 0)
                return;

            string where = string.Empty;
            var gb = list.Where(P => !string.IsNullOrEmpty(P.warehouseId) && P.materialId > 0).GroupBy(P => P.warehouseId);
            foreach (IGrouping<string, AscmWmsPreparationDetail> ig in gb)
            {
                string whereOther = string.Empty;
                var materialIds = ig.Select(P => P.materialId).Distinct();
                var count = materialIds.Count();
                string ids = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += materialIds.ElementAt(i);
                    if ((i + 1) % 500 == 0 || (i + 1) == count)
                    {
                        if (!string.IsNullOrEmpty(ids))
                        {
                            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(whereOther, "inventoryItemId in(" + ids + ")");
                        }
                        ids = string.Empty;
                    }
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(where, "subinventoryCode='" + ig.Key + "' and (" + whereOther + ")");
            }
            
            if (string.IsNullOrEmpty(where))
                return;

            string hql = "select new AscmMtlOnhandQuantitiesDetail(q.inventoryItemId,q.subinventoryCode,q.transactionQuantity) from AscmMtlOnhandQuantitiesDetail q";
            hql += " where " + where;
            IList<AscmMtlOnhandQuantitiesDetail> ilistMtlOnhandQuantitiesDetail = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMtlOnhandQuantitiesDetail>(hql);
            if (ilistMtlOnhandQuantitiesDetail == null || ilistMtlOnhandQuantitiesDetail.Count == 0)
                return;

            foreach (AscmWmsPreparationDetail detail in list)
            {
                detail.onhandQuantity = ilistMtlOnhandQuantitiesDetail.Where(P => P.subinventoryCode == detail.warehouseId && P.inventoryItemId == detail.materialId).Sum(P => P.transactionQuantity);
            }
        }
        /// <summary>设置作业备料单已领数量</summary>
        public void SetReceivedQuantity(int wipEntityId, List<AscmWmsPreparationDetail> list)
        {
            if (list == null && list.Count == 0)
                return;

            var materialIds = list.Select(P => P.materialId).Distinct();
            var count = materialIds.Count();
            string ids = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(ids))
                    ids += ",";
                ids += materialIds.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string hql = "select new AscmWmsLogisticsDetailLog(" + wipEntityId + ",materialId,sum(quantity)) from AscmWmsLogisticsDetailLog where wipEntityId=" + wipEntityId + " and materialId in (" + ids + ") group by materialId";
                        IList<AscmWmsLogisticsDetailLog> ilistLogisticsDetailLog = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsLogisticsDetailLog>(hql);
                        if (ilistLogisticsDetailLog != null && ilistLogisticsDetailLog.Count > 0)
                        {
                            List<AscmWmsLogisticsDetailLog> listLogisticsDetailLog = ilistLogisticsDetailLog.ToList();
                            foreach (AscmWmsPreparationDetail preparationDetail in list)
                            {
                                AscmWmsLogisticsDetailLog logisticsDetailLog = listLogisticsDetailLog.Find(P => P.materialId == preparationDetail.materialId);
                                if (logisticsDetailLog != null)
                                    preparationDetail.receivedQuantity = logisticsDetailLog.quantity;
                            }
                        }
                    }
                    ids = string.Empty;
                }
            }
        }
        /// <summary>设置需求备料单已领数量</summary>
        public void SetReceivedQuantity(List<AscmWmsPreparationDetail> list)
        {
            if (list == null && list.Count == 0)
                return;

            string where = string.Empty;
            var gb = list.GroupBy(P => P.wipEntityId);
            foreach (IGrouping<string, AscmWmsPreparationDetail> ig in gb)
            {
                string whereOther = string.Empty;
                var materialIds = ig.Select(P => P.materialId).Distinct();
                var count = materialIds.Count();
                string ids = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += materialIds.ElementAt(i);
                    if ((i + 1) % 500 == 0 || (i + 1) == count)
                    {
                        if (!string.IsNullOrEmpty(ids))
                        {
                            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(whereOther, "materialId in(" + ids + ")");
                        }
                        ids = string.Empty;
                    }
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(where, "wipEntityId='" + ig.Key + "' and (" + whereOther + ")");
            }

            if (string.IsNullOrEmpty(where))
                return;

            string hql = "select new AscmWmsLogisticsDetailLog(wipEntityId,materialId,sum(quantity)) from AscmWmsLogisticsDetailLog"
                       + " where " + where
                       + " group by wipEntityId,materialId";
            IList<AscmWmsLogisticsDetailLog> ilistLogisticsDetailLog = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsLogisticsDetailLog>(hql);
            if (ilistLogisticsDetailLog == null || ilistLogisticsDetailLog.Count == 0)
                return;

            List<AscmWmsLogisticsDetailLog> listLogisticsDetailLog = ilistLogisticsDetailLog.ToList();
            foreach (AscmWmsPreparationDetail detail in list)
            {
                AscmWmsLogisticsDetailLog logisticsDetailLog = listLogisticsDetailLog.Find(P =>P.wipEntityId == detail.wipEntityId && P.materialId == detail.materialId);
                if (logisticsDetailLog != null)
                    detail.receivedQuantity = logisticsDetailLog.quantity;
            }
        }
        /// <summary>设置接收中数量</summary>
        public void SetRecSupplyQuantity(List<AscmWmsPreparationDetail> list)
        {
            if (list == null && list.Count == 0)
                return;

            var gb = list.GroupBy(P => P.materialId);
            foreach (IGrouping<int, AscmWmsPreparationDetail> ig in gb)
            {
                decimal recSupplyQuantity = GetRecSupplyQuantity(ig.Key);
                ig.ToList().ForEach(P => P.recSupplyQuantity = recSupplyQuantity);
            }
        }
        /// <summary>获取接收中数量</summary>
        public decimal GetRecSupplyQuantity(int materialId)
        {
            decimal recSupplyQuantity = decimal.Zero;
            try
            {
                OracleParameter[] commandParameters = new OracleParameter[] {
                    // 库存组织
                    new OracleParameter {
                        ParameterName = "i_organization_id",
                        OracleDbType = OracleDbType.Int32,
                        Value = 775,
                        Direction = ParameterDirection.Input
                    },
                    // 物料ID
                    new OracleParameter {
                        ParameterName = "i_material_id",
                        OracleDbType = OracleDbType.Int32,
                        Value = materialId,
                        Direction = ParameterDirection.Input
                    },
                    // 接收中数量
                    new OracleParameter {
                        ParameterName = "o_quantity",
                        OracleDbType = OracleDbType.Decimal,
                        Direction = ParameterDirection.Output
                    }
                };
                MesInterface.AscmMesService.GetInstance().ExecuteOraProcedure("cux_ascm_interface_utl.get_rec_supply", ref commandParameters);
                decimal.TryParse(commandParameters[2].Value.ToString(), out recSupplyQuantity);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return recSupplyQuantity;
        }
    }
}
