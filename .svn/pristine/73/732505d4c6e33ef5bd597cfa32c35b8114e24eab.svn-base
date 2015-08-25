using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Warehouse.Entities;

using System.Xml.Serialization;
using System.IO;

namespace MideaAscm.Services.Warehouse
{
    public class AscmWmsLocationTransferService
    {
        private static AscmWmsLocationTransferService ascmWmsLocationTransferServices;
        public static AscmWmsLocationTransferService GetInstance()
        {
            if (ascmWmsLocationTransferServices == null)
                ascmWmsLocationTransferServices = new AscmWmsLocationTransferService();
            return ascmWmsLocationTransferServices;
        }

        public AscmWmsLocationTransfer Get(int id)
        {
            AscmWmsLocationTransfer ascmWmsLocationTransfer = null;
            try
            {
                ascmWmsLocationTransfer = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsLocationTransfer>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsLocationTransfer)", ex);
                throw ex;
            }
            return ascmWmsLocationTransfer;
        }
        public List<AscmWmsLocationTransfer> GetList(string sql)
        {
            List<AscmWmsLocationTransfer> list = null;
            try
            {
                IList<AscmWmsLocationTransfer> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsLocationTransfer>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsLocationTransfer>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsLocationTransfer)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsLocationTransfer> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther, bool IsSetMaterialItem = true, bool IsSetToWarelocation = true, bool IsSetFromWarelocation = true)
        {
            List<AscmWmsLocationTransfer> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmWmsLocationTransfer ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmWmsLocationTransfer> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsLocationTransfer>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsLocationTransfer>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsLocationTransfer>(ilist);
                    if (IsSetMaterialItem)
                        SetMaterialItem(list);
                    if (IsSetToWarelocation)
                        SetToWarelocation(list);
                    if (IsSetFromWarelocation)
                        SetFromWarelocation(list);

                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsLocationTransfer)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmWmsLocationTransfer> listAscmWmsLocationTransfer)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsLocationTransfer);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsLocationTransfer)", ex);
                throw ex;
            }
        }
        public void Save(AscmWmsLocationTransfer ascmWmsLocationTransfer)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsLocationTransfer);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsLocationTransfer)", ex);
                throw ex;
            }
        }
        public void Update(AscmWmsLocationTransfer ascmWmsLocationTransfer)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWmsLocationTransfer>(ascmWmsLocationTransfer);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWmsLocationTransfer)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmWmsLocationTransfer)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmWmsLocationTransfer ascmWmsLocationTransfer = Get(id);
                Delete(ascmWmsLocationTransfer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmWmsLocationTransfer ascmWmsLocationTransfer)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWmsLocationTransfer>(ascmWmsLocationTransfer);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsLocationTransfer)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmWmsLocationTransfer> listAscmWmsLocationTransfer)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsLocationTransfer);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsLocationTransfer)", ex);
                throw ex;
            }
        }
        public void SetMaterialItem(List<AscmWmsLocationTransfer> list)
        {
            if (list != null && list.Count() > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsLocationTransfer ascmWmsLocationTransfer in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsLocationTransfer.materialId;
                }
                string sql = "from AscmMaterialItem where id in (" + ids + ")";
                IList<AscmMaterialItem> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                if (ilist != null && ilist.Count() > 0)
                {
                    List<AscmMaterialItem> listAscmMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilist);
                    foreach (AscmWmsLocationTransfer ascmWmsLocationTransfer in list)
                    {
                        ascmWmsLocationTransfer.ascmMaterialItem = listAscmMaterialItem.Find(item => item.id == ascmWmsLocationTransfer.materialId);
                    }
                }
            }
        }
        public void SetToWarelocation(List<AscmWmsLocationTransfer> list)
        {
            if (list != null && list.Count() > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsLocationTransfer ascmWmsLocationTransfer in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsLocationTransfer.toWarelocationId;
                }
                string sql = "from AscmWarelocation where id in (" + ids + ")";
                IList<AscmWarelocation> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarelocation>(sql);
                if (ilist != null && ilist.Count() > 0)
                {
                    List<AscmWarelocation> listAscmWarelocation = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarelocation>(ilist);
                    foreach (AscmWmsLocationTransfer ascmWmsLocationTransfer in list)
                    {
                        ascmWmsLocationTransfer.toWarelocation = listAscmWarelocation.Find(item => item.id == ascmWmsLocationTransfer.toWarelocationId);
                    }
                }
            }
        }
        public void SetFromWarelocation(List<AscmWmsLocationTransfer> list)
        {
            if (list != null && list.Count() > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsLocationTransfer ascmWmsLocationTransfer in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsLocationTransfer.fromWarelocationId;
                }
                string sql = "from AscmWarelocation where id in (" + ids + ")";
                IList<AscmWarelocation> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarelocation>(sql);
                if (ilist != null && ilist.Count() > 0)
                {
                    List<AscmWarelocation> listAscmWarelocation = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarelocation>(ilist);
                    foreach (AscmWmsLocationTransfer ascmWmsLocationTransfer in list)
                    {
                        ascmWmsLocationTransfer.fromWarelocation = listAscmWarelocation.Find(item => item.id == ascmWmsLocationTransfer.fromWarelocationId);
                    }
                }
            }
        }

        #region 应用
        /// <summary>取操作：保存货位转移信息，并返回其ID</summary>
        public int AddWmsLocationTransfer(AscmWmsLocationTransfer ascmWmsLocationTransfer)
        {
            int id = -1;
            try
            {
                if (ascmWmsLocationTransfer == null)
                    throw new Exception("获取货位转移信息失败");

                string sql = "from AscmLocationMaterialLink where pk.warelocationId=" + ascmWmsLocationTransfer.fromWarelocationId + " and pk.materialId=" + ascmWmsLocationTransfer.materialId;
                IList<AscmLocationMaterialLink> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLocationMaterialLink>(sql);
                if (ilist == null || ilist.Count == 0)
                    throw new Exception("获取来源货位失败");

                int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsLocationTransfer");
                id = ++maxId;
                ascmWmsLocationTransfer.id = id;

                //DateTime dtServerTime = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentDate("AscmLocationMaterialLink");

                //AscmLocationMaterialLink ascmLocationMaterialLink = ilist.First();
                //ascmLocationMaterialLink.modifyUser = ascmWmsLocationTransfer.modifyUser;
                //ascmLocationMaterialLink.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                //ascmLocationMaterialLink.quantity -= ascmWmsLocationTransfer.quantity;

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsLocationTransfer);

                        //YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmLocationMaterialLink);

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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("增加失败(Add AscmWmsLocationTransfer)", ex);
                throw ex;
            }
            return id;
        }
        /// <summary>存操作：保存货位转移信息，并返回其ID</summary>
        public void UpdateWmsLocationTransfer(AscmWmsLocationTransfer ascmWmsLocationTransfer)
        {
            try
            {
                if (ascmWmsLocationTransfer == null)
                    throw new Exception("获取货位转移信息失败");

                string sql = "from AscmLocationMaterialLink "
                           + "where warelocationId in(" + ascmWmsLocationTransfer.fromWarelocationId + "," + ascmWmsLocationTransfer.toWarelocationId + ") "
                           + "and materialId=" + ascmWmsLocationTransfer.materialId;

                IList<AscmLocationMaterialLink> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLocationMaterialLink>(sql);
                if (ilist == null || ilist.Count == 0)
                    throw new Exception("获取货位失败");
                AscmLocationMaterialLink fromLocationMaterialLink = ilist.First(P => P.pk.warelocationId == ascmWmsLocationTransfer.fromWarelocationId);
                if (fromLocationMaterialLink == null)
                    throw new Exception("获取来源货位失败");
                DateTime dtServerTime = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentDate("AscmLocationMaterialLink");
                List<AscmLocationMaterialLink> listLocationMaterialLinkSaveOrUpdate = new List<AscmLocationMaterialLink>();

                fromLocationMaterialLink.quantity -= ascmWmsLocationTransfer.quantity;
                fromLocationMaterialLink.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                fromLocationMaterialLink.modifyUser = ascmWmsLocationTransfer.modifyUser;
                listLocationMaterialLinkSaveOrUpdate.Add(fromLocationMaterialLink);

                AscmLocationMaterialLink toLocationMaterialLink = null;
                toLocationMaterialLink = ilist.First(P => P.pk.warelocationId == ascmWmsLocationTransfer.toWarelocationId);
                //如果货位之前没存放此物料，则新增
                if (toLocationMaterialLink == null)
                {
                    toLocationMaterialLink = new AscmLocationMaterialLink();
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsLocationTransfer");
                    //toLocationMaterialLink.id = ++maxId;
                    toLocationMaterialLink.createTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                    toLocationMaterialLink.createUser = ascmWmsLocationTransfer.modifyUser;
                    //toLocationMaterialLink.warelocationId = ascmWmsLocationTransfer.toWarelocationId;
                    //toLocationMaterialLink.materialId = ascmWmsLocationTransfer.materialId;
                    toLocationMaterialLink.pk = new AscmLocationMaterialLinkPK { warelocationId = ascmWmsLocationTransfer.toWarelocationId, materialId = ascmWmsLocationTransfer.materialId };
                }
                toLocationMaterialLink.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                toLocationMaterialLink.modifyUser = ascmWmsLocationTransfer.modifyUser;
                toLocationMaterialLink.quantity += ascmWmsLocationTransfer.quantity;
                listLocationMaterialLinkSaveOrUpdate.Add(toLocationMaterialLink);

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmWmsLocationTransfer);

                        YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdateList(listLocationMaterialLinkSaveOrUpdate);

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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("增加失败(Update AscmWmsLocationTransfer)", ex);
                throw ex;
            }
        }
        #endregion
    }
}
