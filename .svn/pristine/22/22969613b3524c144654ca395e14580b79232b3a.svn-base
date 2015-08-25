using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Dal.GetMaterialManage.Entities;
using NHibernate;

namespace MideaAscm.Services.Base
{
    public class AscmMaterialItemService
    {
        private static AscmMaterialItemService ascmMaterialItemServices;
        public static AscmMaterialItemService GetInstance()
        {
            //return ascmMaterialItemServices ?? new AscmMaterialItemService();
            if (ascmMaterialItemServices == null)
                ascmMaterialItemServices = new AscmMaterialItemService();
            return ascmMaterialItemServices;
        }
        public AscmMaterialItem Get(int id)
        {
            AscmMaterialItem ascmMaterialItem = null;
            try
            {
                ascmMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmMaterialItem>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmMaterialItem)", ex);
                throw ex;
            }
            return ascmMaterialItem;
        }
        public void Save(AscmMaterialItem ascmMaterialItem)
        {                
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmMaterialItem);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmMaterialItem)",ex);
                throw ex;
            }
        }
        public void Update(AscmMaterialItem ascmMaterialItem)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmMaterialItem>(ascmMaterialItem);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmMaterialItem)");
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmMaterialItem)",ex);
                throw ex;
            }
        }
        public void Update(List<AscmMaterialItem> listAscmMaterialItem)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmMaterialItem);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmMaterialItem)");
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmMaterialItem)", ex);
                throw ex;
            }
        }
        public List<AscmMaterialItem> GetList(string sql, bool isSetMaterialSubCategory = false)
        {
            List<AscmMaterialItem> list = null;
            try
            {
                IList<AscmMaterialItem> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilist);
                    if (!isSetMaterialSubCategory)
                    {
                        SetMaterialSubCategory(list);
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmMaterialItem)",ex);
                throw ex;
            }
            return list;
        }
        public List<AscmMaterialItem> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther = "", bool isSetMaterialSubCategory = true)
        {
            List<AscmMaterialItem> list = null;
            try
            {
                string sort = " order by docNumber ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmMaterialItem ";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    queryWord = queryWord.Trim().Replace(" ", "%");
                    whereQueryWord = " (docNumber like '" + queryWord.Trim() + "%' or description like '" + queryWord.Trim() + "%')";
                    
                    /*long _fValue = -1;
                    if (long.TryParse(queryWord, out _fValue))
                    {
                        whereQueryWord += "(docNumber like '" + queryWord + "%')";
                    }
                    else
                    {
                        whereQueryWord = " (description like '%" + queryWord.Trim() + "%')";
                    }*/
                }

                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmMaterialItem> ilist=null;
                if(ynPage!=null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilist);
                    if (isSetMaterialSubCategory)
                        SetMaterialSubCategory(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmMaterialItem)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmMaterialItem> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string startTime, string endTime, string whereOther = "")
        {
            List<AscmMaterialItem> list = null;
            try
            {
                string sort = " order by docNumber";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmMaterialItem ";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " docNumber like '" + queryWord.Trim() + "%'";
                }

                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmMaterialItem> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmMaterialItem)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmMaterialItem> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string queryType, string queryStarDocnumber, string queryEndDocnumber, string zStatus, string dStatus, string wStatus, string queryDescribe)
        {
            List<AscmMaterialItem> list = null;
            try
            {
                string sql = "from AscmMaterialItem";
                string where = "", whereQueryWord = "";
                whereQueryWord = "wipSupplyType < 4";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(zStatus))
                {
                    if (zStatus != "qb")
                    {
                        if (zStatus != "kz")
                        {
                            whereQueryWord = "zMtlCategoryStatus = '" + zStatus + "'";
                        }
                        else
                        {
                            whereQueryWord = "zMtlCategoryStatus is null";
                        }
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                }

                if (!string.IsNullOrEmpty(dStatus))
                {
                    if (dStatus != "qb")
                    {
                        if (zStatus != "kz")
                        {
                            whereQueryWord = "dMtlCategoryStatus = '" + dStatus + "'";
                        }
                        else
                        {
                            whereQueryWord = "dMtlCategoryStatus is null";
                        }
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                }

                if (!string.IsNullOrEmpty(wStatus))
                {
                    if (wStatus != "qb")
                    {
                        if (zStatus != "kz")
                        {
                            whereQueryWord = "wMtlCategoryStatus = '" + wStatus + "'";
                        }
                        else
                        {
                            whereQueryWord = "wMtlCategoryStatus is null";
                        }
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                }

                if (string.IsNullOrEmpty(zStatus) && string.IsNullOrEmpty(dStatus) && string.IsNullOrEmpty(wStatus))
                {
                    whereQueryWord = "isFlag = 0";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryType))
                {
                    whereQueryWord = "wipSupplyType = " + queryType;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryDescribe))
                {
                    whereQueryWord = "description like '%" + queryDescribe + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryStarDocnumber) && !string.IsNullOrEmpty(queryEndDocnumber))
                {
                    if (queryStarDocnumber == queryEndDocnumber)
                    {
                        whereQueryWord = "docNumber like '" + queryStarDocnumber + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                    else
                    {
                        whereQueryWord = "docNumber > '" + queryStarDocnumber + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        whereQueryWord = "docNumber < '" + queryEndDocnumber + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                }
                else if (!string.IsNullOrEmpty(queryStarDocnumber) && string.IsNullOrEmpty(queryEndDocnumber))
                {
                    whereQueryWord = "docNumber like '" + queryStarDocnumber + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(queryStarDocnumber) && !string.IsNullOrEmpty(queryEndDocnumber))
                {
                    whereQueryWord = "docNumber like '" + queryEndDocnumber + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else
                {
                    whereQueryWord = "docNumber like '20%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where + " order by docNumber";

                IList<AscmMaterialItem> ilist = null;
                if (queryWord == "Export")
                {
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                    
                }
                else
                {
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql, sql, ynPage);
                }

                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmMaterialItem)", ex);
                throw ex;
            }

            return list;
        }
        public List<AscmMaterialItem> GetWarehouseMaterialSumList(int materialId)
        {
            List<AscmMaterialItem> listAscmMaterialItem = null;
            try
            {
                AscmMaterialItem ascmMaterialItem = Get(materialId);
                if (ascmMaterialItem == null)
                    throw new Exception("获取物料失败");
                string totalNumber = "select quantity from AscmLocationMaterialLink where warelocationId=a.id and materialid=" + materialId;
                string warehouseDes = "select description from AscmWarehouse where id=a.warehouseId";
                string sql = "select new AscmWarelocation(a,(" + totalNumber + "),(" + warehouseDes + ")) from AscmWarelocation a";

                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "a.id in(select warelocationId from AscmLocationMaterialLink where materialid=" + materialId + ")");
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmWarelocation> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarelocation>(sql);
                if (ilist != null)
                {
                    listAscmMaterialItem = new List<AscmMaterialItem>();
                    var groupByWarehouse = ilist.GroupBy(P => P.warehouseId);
                    foreach (IGrouping<string, AscmWarelocation> ig in groupByWarehouse)
                    {
                        AscmMaterialItem _ascmMaterialItem = ascmMaterialItem.GetOwner();
                        _ascmMaterialItem.warehouseName = ig.Key;
                        _ascmMaterialItem.warehouseDes = ig.First().warehouseDes;
                        _ascmMaterialItem.totalNumber = ig.Sum(P => P.totalNumber);
                        _ascmMaterialItem.id = ascmMaterialItem.id;
                        _ascmMaterialItem.docNumber = ascmMaterialItem.docNumber;
                        _ascmMaterialItem.unit = ascmMaterialItem.unit;
                        _ascmMaterialItem.description = ascmMaterialItem.description;

                        listAscmMaterialItem.Add(_ascmMaterialItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listAscmMaterialItem;
        }
        public void SetBuyerEmployee(List<AscmMaterialItem> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmMaterialItem ascmMaterialItem in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmMaterialItem.buyerId + "";
                }
                string sql = "from AscmEmployee where id in (" + ids + ")";
                //sql = "from AscmEmployee where id in (" + sql + ")";
                IList<AscmEmployee> ilistAscmEmployee = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmployee>(sql);
                if (ilistAscmEmployee != null && ilistAscmEmployee.Count > 0)
                {
                    List<AscmEmployee> listAscmEmployee = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmEmployee>(ilistAscmEmployee);
                    foreach (AscmMaterialItem ascmMaterialItem in list)
                    {
                        ascmMaterialItem.ascmBuyerEmployee = listAscmEmployee.Find(e => e.id == ascmMaterialItem.buyerId);
                    }
                }
            }
        }
        public void SetMaterialSubCategory(List<AscmMaterialItem> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmMaterialItem ascmMaterialItem in list)
                {
                    if ((!string.IsNullOrEmpty(ids)) && ascmMaterialItem.subCategoryId != null && ascmMaterialItem.subCategoryId != 0)
                        ids += ",";
                    if (ascmMaterialItem.subCategoryId != null && ascmMaterialItem.subCategoryId != 0)
                        ids += ascmMaterialItem.subCategoryId;
                }
                if (!string.IsNullOrEmpty(ids))
                {
                    string sql = "from AscmMaterialSubCategory where id in (" + ids + ")";
                    IList<AscmMaterialSubCategory> ilistAscmMaterialSubCateory = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialSubCategory>(sql);
                    if (ilistAscmMaterialSubCateory != null && ilistAscmMaterialSubCateory.Count > 0)
                    {
                        List<AscmMaterialSubCategory> listAscmMaterialSubCategory = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialSubCategory>(ilistAscmMaterialSubCateory);
                        foreach (AscmMaterialItem ascmMaterialItem in list)
                        {
                            ascmMaterialItem.ascmMaterialSubCategory = listAscmMaterialSubCategory.Find(e => e.id == ascmMaterialItem.subCategoryId);
                        }
                    }
                }
            }
        }

        public List<AscmMaterialItem> GetList(List<string> listMaterialDocs)
        {
            List<AscmMaterialItem> list = null;
            if (listMaterialDocs != null && listMaterialDocs.Count > 0)
            {
                list = new List<AscmMaterialItem>();

                int count = listMaterialDocs.Count;
                string docNumbers = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(docNumbers))
                        docNumbers += ",";
                    docNumbers += "'" + listMaterialDocs[i] + "'";
                    if ((i + 1) % 50 == 0 || (i + 1) == count)
                    {
                        if (!string.IsNullOrEmpty(docNumbers))
                        {
                            string sql = "from AscmMaterialItem where docNumber in(" + docNumbers + ")";
                            IList<AscmMaterialItem> ilistAscmMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                            if (ilistAscmMaterialItem != null)
                            {
                                list = list.Union(YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistAscmMaterialItem)).ToList();
                            }
                        }
                        docNumbers = string.Empty;
                    }
                }
            }
            return list;
        }
        public List<AscmMaterialItem> GetListByDocNumberSegment(string startDocNumber, string endDocNumber)
        {
            List<AscmMaterialItem> list = null;
            string whereOther = "";
            string whereStartDocNumber = "", whereEndDocNumber = "";
            if (!string.IsNullOrEmpty(startDocNumber))
            {
                whereStartDocNumber = "docNumber='" + startDocNumber + "'";
            }
            if (!string.IsNullOrEmpty(endDocNumber))
            {
                if (!string.IsNullOrEmpty(startDocNumber))
                    whereStartDocNumber = "docNumber>='" + startDocNumber + "'";
                whereEndDocNumber = "docNumber<='" + endDocNumber + "'";
            }
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartDocNumber);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndDocNumber);
            if (!string.IsNullOrEmpty(whereOther))
            {
                string hql = "from AscmMaterialItem";
                hql += " where " + whereOther;
                list = GetList(hql);
            }
            return list;
        }
        public List<int> GetListByDocNumberSegment2(string startDocNumber, string endDocNumber)
        {
            List<int> list = null;
            string whereOther = "";
            string whereStartDocNumber = "", whereEndDocNumber = "";
            if (!string.IsNullOrEmpty(startDocNumber))
            {
                whereStartDocNumber = "docNumber='" + startDocNumber + "'";
            }
            if (!string.IsNullOrEmpty(endDocNumber))
            {
                if (!string.IsNullOrEmpty(startDocNumber))
                    whereStartDocNumber = "docNumber>='" + startDocNumber + "'";
                whereEndDocNumber = "docNumber<='" + endDocNumber + "'";
            }
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartDocNumber);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndDocNumber);
            if (!string.IsNullOrEmpty(whereOther))
            {
                string sql = "select id from Ascm_Material_Item where " + whereOther;
                System.Collections.IList ilist = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);
                if (ilist != null)
                {
                    list = new List<int>();
                    for (int i = 0; i < ilist.Count; i++)
                    {
                        int id = 0;
                        object[] array = ilist[i] as object[];
                        if (int.TryParse(array[0].ToString(), out id))
                            list.Add(id);
                    }
                }
            }
            return list;
        }
    }
}
