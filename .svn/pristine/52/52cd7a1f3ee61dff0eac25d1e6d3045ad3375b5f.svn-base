using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Warehouse.Entities;

namespace MideaAscm.Services.Warehouse
{
    public class AscmWarelocationService
    {
        private static AscmWarelocationService ascmWarelocationServices;
        public static AscmWarelocationService GetInstance()
        {
            if (ascmWarelocationServices == null)
                ascmWarelocationServices = new AscmWarelocationService();
            return ascmWarelocationServices;
        }

        public AscmWarelocation Get(int id)
        {
            AscmWarelocation ascmWarelocation = null;
            try
            {
                ascmWarelocation = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWarelocation>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWarelocation)", ex);
                throw ex;
            }
            return ascmWarelocation;
        }
        public List<AscmWarelocation> GetList(string sql, bool isSetWorkshopBuilding = false, bool isSetWarehouse = false)
        {
            List<AscmWarelocation> list = null;
            try
            {
                IList<AscmWarelocation> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarelocation>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarelocation>(ilist);
                    if (isSetWorkshopBuilding)
                        SetWorkshopBuilding(list);
                    if (isSetWarehouse)
                        SetWarehouse(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWarelocation)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWarelocation> GetList(string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmWarelocation> list = null;
            try
            {
                string sort = " order by docNumber ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmWarelocation ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " docNumber like '%" + queryWord.Trim() + "%'";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }

                IList<AscmWarelocation> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarelocation>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarelocation>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWarelocation)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWarelocation> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther,
            bool isSetWorkshopBuilding = true, bool isSetWarehouse = true, bool isSetWarehouseUser = true)
        {
            List<AscmWarelocation> list = null;
            try
            {
                string sort = " order by docNumber ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmWarelocation ";

                string totalNumber = "select sum(quantity) from AscmLocationMaterialLink where warelocationId=a.id";
                string sql1 = "select new AscmWarelocation(a,(" + totalNumber + ")) from AscmWarelocation a";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " upper(docNumber) like '%" + queryWord.Trim().ToUpper() + "%' or upper(categoryCode) like '%" + queryWord.Trim().ToUpper() + "%'";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    sql1 += " where " + where;
                }

                IList<AscmWarelocation> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarelocation>(sql1 + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarelocation>(sql1 + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarelocation>(ilist);
                    if (isSetWorkshopBuilding)
                        SetWorkshopBuilding(list);
                    if (isSetWarehouse)
                        SetWarehouse(list);
                    if (isSetWarehouseUser)
                        SetWarehouseUser(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWarelocation)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWarelocation> GetList(string sortName, string sortOrder, int? buildingId, string warehouseId, string whereOther)
        {
            List<AscmWarelocation> list = null;
            try
            {
                string sort = "";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmWarelocation ";

                string where = "", whereBuildingId = "", whereWarehouseId = "";
                if (buildingId.HasValue)
                    whereBuildingId = " buildingId=" + buildingId.Value;
                if (!string.IsNullOrEmpty(warehouseId))
                    whereWarehouseId = " warehouseId='" + warehouseId + "'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereBuildingId);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereWarehouseId);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                list = GetList(sql + sort);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWarelocation)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWarelocation> GetList(string sortName, string sortOrder, int? buildingId, string warehouseId, string batchIds, string whereOther)
        {
            List<AscmWarelocation> list = null;
            try
            {
                string sort = "";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
             
                string where = "", whereBuildingId = "", whereWarehouseId = "";
                if (buildingId.HasValue)
                    whereBuildingId = " buildingId=" + buildingId.Value;
                if (!string.IsNullOrEmpty(warehouseId))
                    whereWarehouseId = " warehouseId='" + warehouseId + "'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereBuildingId);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereWarehouseId);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                string batchId = "select batchId from AscmAssignWarelocation where warelocationId=a.id and batchId in(" + batchIds + ")";
                string assignQuantity = "select assignQuantity from AscmAssignWarelocation where warelocationId=a.id and batchId in(" + batchIds + ")";
                string sql = "select new AscmWarelocation(a,(" + batchId + "),(" + assignQuantity + ")) from AscmWarelocation a";

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                list = GetList(sql + sort);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWarelocation)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmWarelocation> listAscmWarelocation)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWarelocation);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWarelocation)", ex);
                throw ex;
            }
        }
        public void Save(AscmWarelocation ascmWarelocation)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWarelocation);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWarelocation)", ex);
                throw ex;
            }
        }
        public void Update(AscmWarelocation ascmWarelocation)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWarelocation>(ascmWarelocation);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWarelocation)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmWarelocation)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmWarelocation ascmWarelocation = Get(id);
                Delete(ascmWarelocation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmWarelocation ascmWarelocation)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWarelocation>(ascmWarelocation);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWarelocation)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmWarelocation> listAscmWarelocation)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWarelocation);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWarelocation)", ex);
                throw ex;
            }
        }

        private void SetWorkshopBuilding(List<AscmWarelocation> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWarelocation ascmWarelocation in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWarelocation.buildingId;
                }
                string sql = "from AscmWorkshopBuilding where id in (" + ids + ")";
                IList<AscmWorkshopBuilding> ilistAscmWorkshopBuilding = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWorkshopBuilding>(sql);
                if (ilistAscmWorkshopBuilding != null && ilistAscmWorkshopBuilding.Count > 0)
                {
                    List<AscmWorkshopBuilding> listAscmWorkshopBuilding = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWorkshopBuilding>(ilistAscmWorkshopBuilding);
                    foreach (AscmWarelocation ascmWarelocation in list)
                    {
                        ascmWarelocation.ascmWorkshopBuilding = listAscmWorkshopBuilding.Find(e => e.id == ascmWarelocation.buildingId);
                    }
                }
            }
        }
        private void SetWarehouse(List<AscmWarelocation> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWarelocation ascmWarelocation in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'" + ascmWarelocation.warehouseId + "'";
                }
                string sql = "from AscmWarehouse where id in (" + ids + ")";
                IList<AscmWarehouse> ilistAscmWarehouse = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarehouse>(sql);
                if (ilistAscmWarehouse != null && ilistAscmWarehouse.Count > 0)
                {
                    List<AscmWarehouse> listAscmWarehouse = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarehouse>(ilistAscmWarehouse);
                    foreach (AscmWarelocation ascmWarelocation in list)
                    {
                        ascmWarelocation.ascmWarehouse = listAscmWarehouse.Find(e => e.id == ascmWarelocation.warehouseId);
                    }
                }
            }
        }
        private void SetWarehouseUser(List<AscmWarelocation> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWarelocation ascmWarelocation in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'" + ascmWarelocation.warehouseUserId + "'";
                }
                string sql = "from YnUser where userId in (" + ids + ")";
                IList<YnFrame.Dal.Entities.YnUser> ilistYnUser = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.Find<YnFrame.Dal.Entities.YnUser>(sql);
                if (ilistYnUser != null && ilistYnUser.Count > 0)
                {
                    List<YnFrame.Dal.Entities.YnUser> listYnUser = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnFrame.Dal.Entities.YnUser>(ilistYnUser);
                    foreach (AscmWarelocation ascmWarelocation in list)
                    {
                        YnFrame.Dal.Entities.YnUser ynUser = listYnUser.Find(e => e.userId == ascmWarelocation.warehouseUserId);
                        if (ynUser != null)
                        {
                            ascmWarelocation.warehouseUserName = ynUser.userName;
                        }
                    }
                }
            }
        }

        public List<AscmWarelocation> GetList(List<string> listDocs)
        {
            List<AscmWarelocation> list = null;
            if (listDocs != null && listDocs.Count > 0)
            {
                list = new List<AscmWarelocation>();

                int count = listDocs.Count;
                string docNumbers = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(docNumbers))
                        docNumbers += ",";
                    docNumbers += "'" + listDocs[i] + "'";
                    if ((i + 1) % 50 == 0 || (i + 1) == count)
                    {
                        if (!string.IsNullOrEmpty(docNumbers))
                        {
                            string sql = "from AscmWarelocation where docNumber in(" + docNumbers + ")";
                            IList<AscmWarelocation> ilistAscmWarelocation = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarelocation>(sql);
                            if (ilistAscmWarelocation != null)
                            {
                                list = list.Union(YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarelocation>(ilistAscmWarelocation)).ToList();
                            }
                        }
                        docNumbers = string.Empty;
                    }
                }
            }
            return list;
        }

        #region 应用
        public AscmWarelocation GetWarelocationByRfid(string rfid)
        {
            AscmWarelocation ascmWarelocation = null;
            try
            {
                if (!string.IsNullOrEmpty(rfid))
                {
                    IList<AscmWarelocation> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarelocation>("from AscmWarelocation where rfid='" + rfid.Trim() + "'");
                    if (ilist != null && ilist.Count > 0)
                    {
                        ascmWarelocation = ilist.First();
                        ascmWarelocation.listAscmLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList(ascmWarelocation.id, true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ascmWarelocation;
        }
        #endregion
    }
}
