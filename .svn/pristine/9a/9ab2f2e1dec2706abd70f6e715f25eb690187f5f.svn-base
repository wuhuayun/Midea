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

namespace MideaAscm.Services.Warehouse
{
    public class AscmWmsContainerDeliveryService
    {
        private static AscmWmsContainerDeliveryService ascmWmsContainerDeliveryServices;
        public static AscmWmsContainerDeliveryService GetInstance()
        {
            if (ascmWmsContainerDeliveryServices == null)
                ascmWmsContainerDeliveryServices = new AscmWmsContainerDeliveryService();
            return ascmWmsContainerDeliveryServices;
        }
        #region base
        public AscmWmsContainerDelivery Get(int id)
        {
            AscmWmsContainerDelivery ascmWmsContainerDelivery = null;
            try
            {
                ascmWmsContainerDelivery = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsContainerDelivery>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsContainerDelivery)", ex);
                throw ex;
            }
            return ascmWmsContainerDelivery;
        }
        public List<AscmWmsContainerDelivery> GetList(string sql)
        {
            List<AscmWmsContainerDelivery> list = null;
            try
            {
                IList<AscmWmsContainerDelivery> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsContainerDelivery>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsContainerDelivery>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsContainerDelivery)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsContainerDelivery> GetList(string containerSn, int preparationMainId, bool isSetMaterial = true)
        {
            List<AscmWmsContainerDelivery> list = null;
            try
            {
                string sql = "from AscmWmsContainerDelivery where containerSn='" + containerSn + "' and preparationMainId=" + preparationMainId;

                IList<AscmWmsContainerDelivery> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsContainerDelivery>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsContainerDelivery>(ilist);
                    if (isSetMaterial)
                        SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsContainerDelivery)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsContainerDelivery> GetList(string containerSn, int preparationMainId, string sessionKey)
        {
            List<AscmWmsContainerDelivery> list = null;
            try
            {
                string sql = "from AscmWmsContainerDelivery where containerSn='" + containerSn + "' and preparationMainId=" + preparationMainId;

                IList<AscmWmsContainerDelivery> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsContainerDelivery>(sql, sessionKey);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsContainerDelivery>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsContainerDelivery)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsContainerDelivery> GetListByPreparationMainId(int mainId, bool isSetMaterial = true)
        {
            List<AscmWmsContainerDelivery> list = null;
            try
            {
                string sql = "from AscmWmsContainerDelivery where preparationMainId=" + mainId;

                IList<AscmWmsContainerDelivery> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsContainerDelivery>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsContainerDelivery>(ilist);
                    if (isSetMaterial)
                        SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsContainerDelivery)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsContainerDelivery> GetList(List<int> listPreMainId, string whereOther)
        {
            List<AscmWmsContainerDelivery> list = null;
            if (listPreMainId != null && listPreMainId.Count > 0)
            {
                list = new List<AscmWmsContainerDelivery>();
                var iePreMainId = listPreMainId.Distinct();
                var count = iePreMainId.Count();
                string ids = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += iePreMainId.ElementAt(i);
                    if ((i + 1) % 500 == 0 || (i + 1) == count)
                    {
                        if (!string.IsNullOrEmpty(ids))
                        {
                            string hql = "from AscmWmsContainerDelivery";
                            string where = "";
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "preparationMainId in(" + ids + ")");
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                            hql += " where " + where;
                            IList<AscmWmsContainerDelivery> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsContainerDelivery>(hql);
                            if (ilist != null && ilist.Count > 0)
                            {
                                list.AddRange(ilist);
                            }
                        }
                        ids = string.Empty;
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 获取设置了配料人员和备料单状态的容器备料列表
        /// </summary>
        /// <param name="listWipEntityId">作业ID列表</param>
        /// <returns>返回设置了配料人员和备料单状态的容器备料列表</returns>
        public List<AscmWmsContainerDelivery> GetListByWipEntity(List<int> listWipEntityId)
        {
            List<AscmWmsContainerDelivery> list = null;
            if (listWipEntityId != null && listWipEntityId.Count > 0)
            {
                list = new List<AscmWmsContainerDelivery>();
                var ieWipEntityId = listWipEntityId.Distinct();
                var count = ieWipEntityId.Count();
                string ids = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ieWipEntityId.ElementAt(i);
                    if ((i + 1) % 500 == 0 || (i + 1) == count)
                    {
                        if (!string.IsNullOrEmpty(ids))
                        {
                            string hql = "select new AscmWmsContainerDelivery(awcd,aui.userName,awpm.status) from AscmWmsContainerDelivery awcd,AscmUserInfo aui,AscmWmsPreparationMain awpm";
                            string where = "";
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awcd.preparationMainId=awpm.id");
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awcd.createUser=aui.userId");
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awcd.wipEntityId in(" + ids + ")");
                            hql += " where " + where;
                            List<AscmWmsContainerDelivery> _list = GetList(hql);
                            if (_list != null && _list.Count > 0)
                                list.AddRange(_list);
                        }
                        ids = string.Empty;
                    }
                }
                if (list.Count > 0)
                    list = list.Distinct().ToList();
            }
            return list;
        }
        public List<AscmWmsContainerDelivery> GetListByWipEntity(List<int> listWipEntityId, string whereOther)
        {
            List<AscmWmsContainerDelivery> list = null;
            if (listWipEntityId != null && listWipEntityId.Count > 0)
            {
                list = new List<AscmWmsContainerDelivery>();
                var ieWipEntityId = listWipEntityId.Distinct();
                var count = ieWipEntityId.Count();
                string ids = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ieWipEntityId.ElementAt(i);
                    if ((i + 1) % 500 == 0 || (i + 1) == count)
                    {
                        if (!string.IsNullOrEmpty(ids))
                        {
                            string hql = "from AscmWmsContainerDelivery";
                            string where = "";
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wipEntityId in(" + ids + ")");
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                            hql += " where " + where;
                            List<AscmWmsContainerDelivery> _list = GetList(hql);
                            if (_list != null && _list.Count > 0)
                                list.AddRange(_list);
                        }
                        ids = string.Empty;
                    }
                }
                if (list.Count > 0)
                    list = list.Distinct().ToList();
            }
            return list;
        }
        public void Save(List<AscmWmsContainerDelivery> listAscmWmsContainerDelivery)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsContainerDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsContainerDelivery)", ex);
                throw ex;
            }
        }
        public void Save(AscmWmsContainerDelivery ascmWmsContainerDelivery)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsContainerDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsContainerDelivery)", ex);
                throw ex;
            }
        }
        public void Update(AscmWmsContainerDelivery ascmWmsContainerDelivery)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWmsContainerDelivery>(ascmWmsContainerDelivery);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWmsContainerDelivery)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmWmsContainerDelivery)", ex);
                throw ex;
            }
        }
        public void Delete(AscmWmsContainerDelivery ascmWmsContainerDelivery)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWmsContainerDelivery>(ascmWmsContainerDelivery);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsContainerDelivery)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmWmsContainerDelivery> listAscmWmsContainerDelivery)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsContainerDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsContainerDelivery)", ex);
                throw ex;
            }
        }
        public void SetMaterial(List<AscmWmsContainerDelivery> list)
        {
            if (list != null && list.Count > 0)
            {
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
                                foreach (AscmWmsContainerDelivery containerDelivery in list)
                                {
                                    AscmMaterialItem materialItem = listMaterialItem.Find(P => P.id == containerDelivery.materialId);
                                    if (materialItem != null)
                                    {
                                        containerDelivery.materialDocNumber = materialItem.docNumber;
                                        containerDelivery.materialName = materialItem.description;
                                    }
                                }
                            }
                        }
                        ids = string.Empty;
                    }
                }
            }
        }
        public void SetWipEntities(List<AscmWmsContainerDelivery> list)
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
                            IList<AscmWipEntities> ilistWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                            if (ilistWipEntities != null && ilistWipEntities.Count > 0)
                            {
                                List<AscmWipEntities> listWipEntities = ilistWipEntities.ToList();
                                foreach (AscmWmsContainerDelivery containerDelivery in list)
                                {
                                    AscmWipEntities wipEntities = listWipEntities.Find(P => P.wipEntityId == containerDelivery.wipEntityId);
                                    if (wipEntities != null)
                                        containerDelivery.wipEntityName = wipEntities.name;
                                }
                            }
                        }
                        ids = string.Empty;
                    }
                }
            }
        }
        #endregion

        #region 应用
        public AscmWmsContainerDelivery Add(string containerSn, int preparationMainId, int materialId, int warelocationId, decimal quantity)
        {
            try
            {
                AscmWmsContainerDelivery ascmWmsContainerDelivery = new AscmWmsContainerDelivery();
                ascmWmsContainerDelivery.organizationId = 775;
                ascmWmsContainerDelivery.createUser = "";
                ascmWmsContainerDelivery.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmWmsContainerDelivery.modifyUser = "";
                ascmWmsContainerDelivery.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmWmsContainerDelivery.containerSn = containerSn;
                ascmWmsContainerDelivery.preparationMainId = preparationMainId;
                ascmWmsContainerDelivery.materialId = materialId;
                ascmWmsContainerDelivery.quantity = quantity;

                int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsContainerDelivery");
                maxId++;
                ascmWmsContainerDelivery.id = maxId;
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsContainerDelivery);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }
                return ascmWmsContainerDelivery;
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsContainerDelivery)", ex);
                throw ex;
            }
        }
        public void Update(int id, decimal quantity)
        {
            try
            {
                AscmWmsContainerDelivery ascmWmsContainerDelivery = Get(id);
                if (ascmWmsContainerDelivery == null)
                    throw new Exception("系统错误：获取数据失败");
                ascmWmsContainerDelivery.modifyUser = "";
                ascmWmsContainerDelivery.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmWmsContainerDelivery.quantity = quantity;

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsContainerDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("更新失败(Update AscmWmsContainerDelivery)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmWmsContainerDelivery ascmWmsContainerDelivery = Get(id);
                if (ascmWmsContainerDelivery == null)
                    throw new Exception("系统错误：获取数据失败");

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Delete(ascmWmsContainerDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsContainerDelivery)", ex);
                throw ex;
            }
        }
        public void Clear(string containerSn, int preparationMainId)
        {
            try
            {
                List<AscmWmsContainerDelivery> listAscmWmsContainerDelivery = GetList(containerSn, preparationMainId);
                if (listAscmWmsContainerDelivery == null)
                    throw new Exception("系统错误：获取数据失败");

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsContainerDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsContainerDelivery)", ex);
                throw ex;
            }
        }
        public AscmWmsContainerDelivery GetCurrent(string containerSn, string sessionKey)
        {
            try
            {
                string sort = " order by id desc ";
                string sql = "from AscmWmsContainerDelivery where containerSn='" + containerSn + "' and (status is null or status='') and createTime>='" + DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd 00:00:00") + "'";

                YnPage ynPage = new YnPage();
                ynPage.SetPageSize(1);
                ynPage.SetCurrentPage(1);
                IList<AscmWmsContainerDelivery> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsContainerDelivery>(sql + sort, sql, ynPage, sessionKey);
                //IList<AscmContainerDelivery> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainerDelivery>(sql, sessionKey);
                if (ilist != null && ilist.Count > 0)
                {
                    return ilist[0];
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsContainerDelivery)", ex);
                throw ex;
            }
            return null;
        }
        #endregion
    }
}
