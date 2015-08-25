﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Services.Base;

namespace MideaAscm.Services.FromErp
{
    public class AscmWipRequirementOperationsService
    {
        private static AscmWipRequirementOperationsService ascmDeliveryOrderMainServices;
        public static AscmWipRequirementOperationsService GetInstance()
        {
            //return ascmDeliveryOrderMainServices ?? new AscmWipRequirementOperationsService();
            if (ascmDeliveryOrderMainServices == null)
                ascmDeliveryOrderMainServices = new AscmWipRequirementOperationsService();
            return ascmDeliveryOrderMainServices;
        }
        public AscmWipRequirementOperations Get(int id)
        {
            AscmWipRequirementOperations wipRequirementOperations = null;
            try
            {
                wipRequirementOperations = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWipRequirementOperations>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWipRequirementOperations)", ex);
                throw ex;
            }
            return wipRequirementOperations;
        }
        public List<AscmWipRequirementOperations> GetList(string sql)
        {
            List<AscmWipRequirementOperations> list = null;
            try
            {
                IList<AscmWipRequirementOperations> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWipRequirementOperations)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWipRequirementOperations> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther, string whereWipDiscreteJobs)
        {
            List<AscmWipRequirementOperations> list = null;
            try
            {
                string whereWipEntityId = "";
                /*
                List<AscmWipDiscreteJobs> listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList(ynPage, "", "", queryWord, whereWipDiscreteJobs);
                foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in listWipDiscreteJobs)
                {
                    if (whereWipEntityId != "")
                        whereWipEntityId += ",";
                    whereWipEntityId += ascmWipDiscreteJobs.wipEntityId;
                }

                if (!string.IsNullOrEmpty(whereWipEntityId))
                    whereWipEntityId = "wipEntityId in ( " + whereWipEntityId + ")";
                else
                     = "wipEntityId in ( -1)";
                */
                if (!string.IsNullOrEmpty(whereWipDiscreteJobs))
                    whereWipEntityId = "wipEntityId in ( select wipEntityId from AscmWipDiscreteJobs where " + whereWipDiscreteJobs + ")";
                string sort = " order by wipEntityId,inventoryItemId ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                //sort = ""; //不能在里面加order ，否则效率非常低
                string sql = "from AscmWipRequirementOperations";
                //string detailCount = "select count(*) from AscmCuxWipReleaseLines where releaseHeaderId= a.releaseHeaderId";
                //string totalNumber = "select sum(deliveryQuantity) from AscmDeliveryOrderDetail where mainId= a.id";
                //string sql1 = "select new AscmWipRequirementOperations(a,(" + detailCount + "),(" + totalNumber + ")) from AscmWipRequirementOperations a ";
                string sql1 = "select new AscmWipRequirementOperations(a) from AscmWipRequirementOperations a ";//,(" + detailCount + ")

                string where = "", whereQueryWord = "";
                //if (!string.IsNullOrEmpty(queryWord))
                //    whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereWipEntityId);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    sql1 += " where " + where;
                }
                //IList<AscmWipRequirementOperations> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql + sort);
                IList<AscmWipRequirementOperations> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql1 + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql1 + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilist);
                    SetWipEntities(list);
                    SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWipRequirementOperations)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWipRequirementOperations> GetSumList(YnPage ynPage, string sortName, string sortOrder, string queryWord,int organizationId, string whereOther, string whereWipDiscreteJobs, string supplySubinventory)
        {
            List<AscmWipRequirementOperations> list = null;
            try
            {
                string whereWipEntityId = "", whereWipEntityId_Count = "";
                /*
                List<AscmWipDiscreteJobs> listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList(ynPage, "", "", queryWord, whereWipDiscreteJobs);
                foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in listWipDiscreteJobs)
                {
                    if (whereWipEntityId != "")
                        whereWipEntityId += ",";
                    whereWipEntityId += ascmWipDiscreteJobs.wipEntityId;
                }
                if (!string.IsNullOrEmpty(whereWipEntityId))
                    whereWipEntityId = "wipEntityId in ( " + whereWipEntityId + ")";
                else
                    whereWipEntityId = "wipEntityId in ( -1)";
                 * */
                if (!string.IsNullOrEmpty(whereWipDiscreteJobs))
                {
                    whereWipEntityId = "wipEntityId in ( select wipEntityId from AscmWipDiscreteJobs where " + whereWipDiscreteJobs + ")";
                    whereWipEntityId_Count = "wipEntityId in ( select wipEntityId from Ascm_Wip_Discrete_Jobs where " + whereWipDiscreteJobs + ")";
                }
                string sort = " order by inventoryItemId ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                //sort = ""; //不能在里面加order ，否则效率非常低
                //string sql = "select inventoryItemId from AscmWipRequirementOperations";
                string sqlCount = "select inventoryItemId from ASCM_WIP_REQUIRE_OPERAT ";
                //string requiredQuantitySum = "select sum(requiredQuantity) from AscmCuxWipReleaseLines where releaseHeaderId= a.releaseHeaderId";
                //string totalNumber = "select sum(deliveryQuantity) from AscmDeliveryOrderDetail where mainId= a.id";
                //string sql1 = "select new AscmWipRequirementOperations(a,(" + detailCount + "),(" + totalNumber + ")) from AscmWipRequirementOperations a ";
                //string sql1 = "select new AscmWipRequirementOperations(inventoryItemId) from AscmWipRequirementOperations t ";//,(" + detailCount + ")
                string sql1 = "select new AscmWipRequirementOperations(inventoryItemId,count(*),sum(requiredQuantity),sum(quantityIssued)) from AscmWipRequirementOperations";

                string where = "", whereQueryWord = "", where_Count = "", whereSupplySubinventory = "";
                //if (!string.IsNullOrEmpty(queryWord))
                //    whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereWipEntityId);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereSupplySubinventory);

                where_Count = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where_Count, whereQueryWord);
                where_Count = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where_Count, whereOther);
                where_Count = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where_Count, whereWipEntityId_Count);
                where_Count = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where_Count, whereSupplySubinventory);

                where_Count = where_Count.Replace("AscmMaterialItem", "Ascm_Material_Item");

                if (!string.IsNullOrEmpty(where))
                {
                    sqlCount += " where " + where_Count;
                    sql1 += " where " + where;
                }
                sqlCount += " group by inventoryItemId ";
                sql1 += " group by inventoryItemId ";

                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObjectBySQL("select count(*) from ("+sqlCount+")");
                int count = 0;
                int.TryParse(object1.ToString(), out count);
                IList<AscmWipRequirementOperations> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql1 + sort, count, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilist);
                    //SetWipEntities(list);
                    SetMaterial(list);
                    //现有量
                    SetQuantity(list, organizationId, supplySubinventory);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWipRequirementOperations)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWipRequirementOperations> GetList(int wipEntityId, string whereOther)
        {
            List<AscmWipRequirementOperations> list = null;
            try 
            {
                string sql = "from AscmWipRequirementOperations";
                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wipEntityId=" + wipEntityId);
                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }
                IList<AscmWipRequirementOperations> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilist);
                    SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWipRequirementOperations)", ex);
                throw ex;
            }
            return list;
        }
        /// <summary>
        /// 汇总物料@2014-4-22
        /// </summary>
        /// <param name="ynPage"></param>
        /// <param name="userName"></param>
        /// <param name="queryStartTime"></param>
        /// <param name="queryEndTime"></param>
        /// <param name="queryUserName"></param>
        /// <returns></returns>
        public List<AscmWipRequirementOperations> GetSumList(YnPage ynPage, string userName, string queryStartTime, string queryEndTime, string queryUserName)
        {
            List<AscmWipRequirementOperations> list = null;
            try
            {
                string sql = "select new AscmWipRequirementOperations(inventoryItemId, sum(requiredQuantity), sum(quantityIssued), sum(getMaterialQuantity)) from AscmWipRequirementOperations where taskId in ({0}) group by inventoryItemId";
                string sql_page = "select inventoryItemId, sum(requiredQuantity), sum(quantityIssued), sum(getMaterialQuantity) from ASCM_WIP_REQUIRE_OPERAT where taskId in ({0}) group by inventoryItemId";
                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
                string userLogistisClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName);
                string sqlParam = "select id from AscmGetMaterialTask";
                string where = "", whereQueryWord = "";

                if (string.IsNullOrEmpty(queryStartTime) && string.IsNullOrEmpty(queryEndTime))
                    throw new Exception("汇总物料失败：请选择起止日期！");

                if (!string.IsNullOrEmpty(queryStartTime) && !string.IsNullOrEmpty(queryEndTime))
                {
                    queryStartTime = queryStartTime + " 00:00:00";
                    queryEndTime = queryEndTime + " 23:59:59";

                    whereQueryWord = "createTime >= '" + queryStartTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    whereQueryWord = "createTime <= '" + queryEndTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else if (!string.IsNullOrEmpty(queryStartTime) && string.IsNullOrEmpty(queryEndTime))
                {
                    whereQueryWord = "createTime like '" + queryStartTime + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(queryStartTime) && !string.IsNullOrEmpty(queryEndTime))
                {
                    whereQueryWord = "createTime like '" + queryEndTime + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(userLogistisClass) && userRole == "领料员")
                {
                    whereQueryWord = "workerId = '" + userName + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else if (!string.IsNullOrEmpty(userLogistisClass) && (userRole.IndexOf("班长") > -1 || userRole.IndexOf("组长") > -1))
                {
                    whereQueryWord = "logisticsClass in (" + userLogistisClass + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(where))
                    sqlParam += " where " + where;

                string hql = string.Format(sql,sqlParam);
                string sqlParam_pape = sqlParam.Replace("AscmGetMaterialTask", "ASCM_GETMATERIAL_TASK");
                string Tsql = string.Format(sql_page, sqlParam_pape);

                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObjectBySQL("select count(*) from (" + Tsql + ")");
                int count = 0;
                int.TryParse(object1.ToString(), out count);
                IList<AscmWipRequirementOperations> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(hql, count, ynPage);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilist);
                    SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("汇总失败(Sum AscmWipRequirementOperations)", ex);
                throw ex;
            }
            return list;
        }
        public void Update(AscmWipRequirementOperations ascmDeliveryNotify)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWipRequirementOperations>(ascmDeliveryNotify);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWipRequirementOperations)", ex);
                    throw ex;
                }
            }
        }
        public void Update(List<AscmWipRequirementOperations> listAscmDeliveryNotify)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmDeliveryNotify);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("更新失败(Update AscmWipRequirementOperations)", ex);
                    throw ex;
                }
            }
        }
        public void SetWipEntities(List<AscmWipRequirementOperations> list)
        {
            if (list != null && list.Count > 0)
            {
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
                            sql = "from AscmWipDiscreteJobs where wipEntityId in (" + ids + ")";
                            IList<AscmWipDiscreteJobs> ilistAscmWipDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sql);
                            List<AscmWipEntities> listAscmWipEntities = null;
                            List<AscmWipDiscreteJobs> listAscmWipDiscreteJobs = null;
                            if (ilistAscmWipEntities != null && ilistAscmWipEntities.Count > 0)
                                listAscmWipEntities = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilistAscmWipEntities);
                            if (ilistAscmWipDiscreteJobs != null && ilistAscmWipDiscreteJobs.Count > 0)
                            {
                                listAscmWipDiscreteJobs = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobs>(ilistAscmWipDiscreteJobs);
                                AscmWipDiscreteJobsService.GetInstance().SetLookupValues(listAscmWipDiscreteJobs);
                                AscmWipDiscreteJobsService.GetInstance().SetScheduleGroups(listAscmWipDiscreteJobs);
                                AscmWipDiscreteJobsService.GetInstance().SetMaterialItem(listAscmWipDiscreteJobs);
                            }
                            foreach (AscmWipRequirementOperations wipRequirementOperations in list)
                            {
                                if (listAscmWipEntities != null)
                                {
                                    AscmWipEntities ascmWipEntities = listAscmWipEntities.Find(e => e.wipEntityId == wipRequirementOperations.wipEntityId);
                                    if (ascmWipEntities != null)
                                        wipRequirementOperations.ascmWipEntities = ascmWipEntities;
                                }
                                if (listAscmWipDiscreteJobs != null)
                                {
                                    AscmWipDiscreteJobs ascmWipDiscreteJobs = listAscmWipDiscreteJobs.Find(e => e.wipEntityId == wipRequirementOperations.wipEntityId);
                                    if (ascmWipDiscreteJobs != null)
                                        wipRequirementOperations.ascmWipDiscreteJobs = ascmWipDiscreteJobs;
                                }
                            }
                        }
                        ids = string.Empty;
                    }
                }
            }
        }
        public void SetMaterial(List<AscmWipRequirementOperations> list)
        {
            if (list != null && list.Count > 0)
            {
                var inventoryItemIds = list.Select(P => P.inventoryItemId).Distinct();
                var count = inventoryItemIds.Count();
                string ids = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += inventoryItemIds.ElementAt(i);
                    if ((i + 1) % 500 == 0 || (i + 1) == count)
                    { 
                        if (!string.IsNullOrEmpty(ids))
                        {
                            string sql = "from AscmMaterialItem where id in (" + ids + ")";
                            IList<AscmMaterialItem> ilistAscmMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                            if (ilistAscmMaterialItem != null && ilistAscmMaterialItem.Count > 0)
                            {
                                List<AscmMaterialItem> listAscmMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistAscmMaterialItem);
                                foreach (AscmWipRequirementOperations wipRequirementOperations in list)
                                {
                                    AscmMaterialItem ascmMaterialItem = listAscmMaterialItem.Find(P => P.id == wipRequirementOperations.inventoryItemId);
                                    if (ascmMaterialItem != null)
                                        wipRequirementOperations.ascmMaterialItem = ascmMaterialItem;
                                }
                            }
                        }
                        ids = string.Empty;
                    }
                }
            }
        }
        public void SetQuantity(List<AscmWipRequirementOperations> list, int organizationId, string subinventoryCode)
        {
            if (list != null && list.Count > 0)
            {
                var inventoryItemIds = list.Select(P => P.inventoryItemId).Distinct();
                var count = inventoryItemIds.Count();
                string ids = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += inventoryItemIds.ElementAt(i);
                    if ((i + 1) % 500 == 0 || (i + 1) == count)
                    {
                        if (!string.IsNullOrEmpty(ids))
                        {
                            //现有数、库存表
                            string sql = "select new AscmMtlOnhandQuantitiesDetail(inventoryItemId,sum(transactionQuantity)) from AscmMtlOnhandQuantitiesDetail ";

                            string where = " inventoryItemId in (" + ids + ")", whereSubinventoryCode = "";
                            if (!string.IsNullOrEmpty(subinventoryCode))
                                whereSubinventoryCode = "subinventoryCode='" + subinventoryCode + "'";

                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereSubinventoryCode);
                            if (!string.IsNullOrEmpty(where))
                            {
                                sql += " where " + where;
                            }
                            sql += " group by inventoryItemId ";

                            IList<AscmMtlOnhandQuantitiesDetail> ilistAscmMtlOnhandQuantitiesDetail = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMtlOnhandQuantitiesDetail>(sql);
                            if (ilistAscmMtlOnhandQuantitiesDetail != null && ilistAscmMtlOnhandQuantitiesDetail.Count > 0)
                            {
                                List<AscmMtlOnhandQuantitiesDetail> listAscmMtlOnhandQuantitiesDetail = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMtlOnhandQuantitiesDetail>(ilistAscmMtlOnhandQuantitiesDetail);
                                foreach (AscmWipRequirementOperations wipRequirementOperations in list)
                                {
                                    AscmMtlOnhandQuantitiesDetail ascmMtlOnhandQuantitiesDetail = listAscmMtlOnhandQuantitiesDetail.Find(P => P.inventoryItemId == wipRequirementOperations.inventoryItemId);
                                    if (ascmMtlOnhandQuantitiesDetail != null)
                                        wipRequirementOperations.transactionQuantity = ascmMtlOnhandQuantitiesDetail.transactionQuantity;
                                }
                            }
                            //接收中数量
                            sql = "select new AscmRcvSupply(itemId,sum(toOrgPrimaryQuantity)) from AscmRcvSupply ";

                            where = " itemId in (" + ids + ")";
                            if (!string.IsNullOrEmpty(where))
                            {
                                sql += " where " + where;
                            }
                            sql += " group by itemId ";
                            IList<AscmRcvSupply> ilistAscmRcvSupply = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmRcvSupply>(sql);
                            if (ilistAscmRcvSupply != null && ilistAscmRcvSupply.Count > 0)
                            {
                                List<AscmRcvSupply> listAscmRcvSupply = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmRcvSupply>(ilistAscmRcvSupply);
                                foreach (AscmWipRequirementOperations wipRequirementOperations in list)
                                {
                                    AscmRcvSupply ascmRcvSupply = listAscmRcvSupply.Find(P => P.itemId == wipRequirementOperations.inventoryItemId);
                                    if (ascmRcvSupply != null)
                                        wipRequirementOperations.toOrgPrimaryQuantity = ascmRcvSupply.toOrgPrimaryQuantity;
                                }
                            }
                        }
                        ids = string.Empty;
                    }
                }
            }
        }
        public void SetOnhandQuantity(List<AscmWipRequirementOperations> list)
        {
            if (list == null && list.Count == 0)
                return;

            string where = string.Empty;
            var gb = list.Where(P => !string.IsNullOrEmpty(P.supplySubinventory) && P.inventoryItemId > 0).GroupBy(P => P.supplySubinventory);
            foreach (IGrouping<string, AscmWipRequirementOperations> ig in gb)
            {
                string whereOther = string.Empty;
                var materialIds = ig.Select(P => P.inventoryItemId).Distinct();
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

            foreach (AscmWipRequirementOperations bom in list)
            {
                bom.transactionQuantity = ilistMtlOnhandQuantitiesDetail.Where(P => P.subinventoryCode == bom.supplySubinventory && P.inventoryItemId == bom.inventoryItemId).Sum(P => P.transactionQuantity);
            }
        }

        /// <summary>
        /// 加载物料@2014-4-19
        /// </summary>
        /// <param name="ynPage"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="queryWord"></param>
        /// <param name="whereOther"></param>
        /// <param name="taskId"></param>
        /// <param name="jobId"></param>
        /// <param name="queryBomWarehouse"></param>
        /// <param name="queryBomMtlCategory"></param>
        /// <returns></returns>
        public List<AscmWipRequirementOperations> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther, string taskId, string jobId, string queryBomWarehouse, string queryBomMtlCategory, bool isSetOnhandQuantity = true)
        {
            List<AscmWipRequirementOperations> list = null;

            try
            {
                string hql = "select new AscmWipRequirementOperations(awro, ami.docNumber, ami.description) from AscmWipRequirementOperations awro, AscmMaterialItem ami";
                string sql = "select count(*) from AscmWipRequirementOperations awro";

                string where = "", whereQueryWord = "", whereOtherWord = "";

                string sort = " order by ami.docNumber";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                whereQueryWord = "awro.inventoryItemId = ami.id";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = "ami.description like '%" + queryWord + "%'";
                    whereOtherWord = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOtherWord, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(taskId))
                {
                    whereQueryWord = "awro.taskId = " + taskId;
                    whereOtherWord = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOtherWord, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(jobId))
                {
                    whereQueryWord = "awro.wipEntityId = " + jobId;
                    whereOtherWord = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOtherWord, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryBomWarehouse))
                {
                    queryBomWarehouse = queryBomWarehouse.ToUpper().Trim();
                    whereQueryWord = "awro.supplySubinventory like '" + queryBomWarehouse + "%'";
                    whereOtherWord = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOtherWord, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryBomMtlCategory))
                {
                    whereQueryWord = "ami.docNumber like '" + queryBomMtlCategory + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(whereOtherWord))
                    sql += " where " + whereOtherWord;

                object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(sql);
                int count = 0;
                int.TryParse(obj.ToString(), out count);

                if (!string.IsNullOrEmpty(where))
                {
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOtherWord);
                    hql += " where " + where + sort;
                }

                IList<AscmWipRequirementOperations> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(hql, count, ynPage);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilist);
                    if (isSetOnhandQuantity)
                        SetOnhandQuantity(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("获取失败(Get AscmWipRequirementOperationsList)", ex);
                throw ex;
            }

            return list;
        }

        /// <summary>
        /// 加载物料@2014-4-19
        /// 覃小华重新
        /// 2015年3月17日
        /// </summary>
        /// <param name="ynPage"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="queryWord"></param>
        /// <param name="whereOther"></param>
        /// <param name="taskId"></param>
        /// <param name="jobId"></param>
        /// <param name="queryBomWarehouse"></param>
        /// <param name="queryBomMtlCategory"></param>
        /// <returns></returns>
        public List<AscmWipRequirementOperations> GetList(string sortName, string sortOrder, string queryWord, string whereOther, string taskId, string jobId, string queryBomWarehouse, string queryBomMtlCategory, bool isSetOnhandQuantity = true)
        {
            List<AscmWipRequirementOperations> list = null;

            try
            {
                string hql = "select new AscmWipRequirementOperations(awro, ami.docNumber, ami.description) from AscmWipRequirementOperations awro, AscmMaterialItem ami";
                //string sql = "select count(*) from AscmWipRequirementOperations awro";

                string where = "", whereQueryWord = "", whereOtherWord = "";

                string sort = " order by ami.docNumber";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                whereQueryWord = "awro.inventoryItemId = ami.id";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = "ami.description like '%" + queryWord + "%'";
                    whereOtherWord = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOtherWord, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(taskId))
                {
                    whereQueryWord = "awro.taskId = " + taskId;
                    whereOtherWord = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOtherWord, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(jobId))
                {
                    whereQueryWord = "awro.wipEntityId = " + jobId;
                    whereOtherWord = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOtherWord, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryBomWarehouse))
                {
                    queryBomWarehouse = queryBomWarehouse.ToUpper().Trim();
                    whereQueryWord = "awro.supplySubinventory like '" + queryBomWarehouse + "%'";
                    whereOtherWord = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOtherWord, whereQueryWord);
                }

                //if (!string.IsNullOrEmpty(queryBomMtlCategory))
                //{
                whereQueryWord = "awro.taskId in (select id from AscmGetMaterialTask where " + (string.IsNullOrEmpty(queryBomMtlCategory) ? "mtlCategoryStatus is null" : " mtlCategoryStatus ='" + queryBomMtlCategory + "'") + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
               // }

                if (!string.IsNullOrEmpty(whereOtherWord))
                    //sql += " where " + whereOtherWord;

             //   object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(sql);
                //int count = 0;
                //int.TryParse(obj.ToString(), out count);

                if (!string.IsNullOrEmpty(where))
                {
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOtherWord);
                    hql += " where " + where + sort;
                }

                IList<AscmWipRequirementOperations> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(hql);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilist);
                    if (isSetOnhandQuantity)
                        SetOnhandQuantity(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("获取失败(Get AscmWipRequirementOperationsList)", ex);
                throw ex;
            }

            return list;
        }

        /// <summary>
        /// 创建bom@2014-4-28
        /// </summary>
        /// <param name="ynPage"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="queryWord"></param>
        /// <param name="whereOther"></param>
        /// <returns></returns>
        public List<AscmWipRequirementOperations> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmWipRequirementOperations> list = null;

            try
            {
                string hql = "select new AscmWipRequirementOperations(awro, ami.docNumber, ami.description, ami.zMtlCategoryStatus, ami.dMtlCategoryStatus, ami.wipSupplyType, adj.jobDate, adj.identificationId, adj.productLine, adj.which, adj.workerId, adj.onlineTime) from AscmWipRequirementOperations awro, AscmMaterialItem ami, AscmDiscreteJobs adj";
                string where = "", whereQueryWord = "";

                whereQueryWord = "awro.inventoryItemId = ami.id";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                whereQueryWord = "awro.wipEntityId = adj.wipEntityId";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (string.IsNullOrEmpty(queryWord))
                    throw new Exception("作业号为空！");

                whereQueryWord = "awro.wipEntityId in (" + queryWord + ")";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    hql += " where " + where;

                IList<AscmWipRequirementOperations> ilist = null;
                if (ynPage != null)
                {
                    object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObjectBySQL("select count(*) from AscmWipRequirementOperations where wipEntityId in (" + queryWord + ")");
                    int count = 0;
                    if (int.TryParse(obj.ToString(), out count) && count > 0)
                    {
                        ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(hql, count, ynPage);
                    }
                }
                else
                {
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(hql);
                }

                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("获取失败(Get AscmWipRequirementOperationsList)", ex);
                throw ex;
            }

            return list;
        }
    }
}
