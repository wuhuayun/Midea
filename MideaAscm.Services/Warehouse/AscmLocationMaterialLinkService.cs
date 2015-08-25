using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;

using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Warehouse.Entities;

namespace MideaAscm.Services.Warehouse
{
    public class AscmLocationMaterialLinkService
    {
        private static AscmLocationMaterialLinkService service;
        public static AscmLocationMaterialLinkService GetInstance()
        {
            if (service == null)
                service = new AscmLocationMaterialLinkService();
            return service;
        }

        public AscmLocationMaterialLink Get(AscmLocationMaterialLinkPK pk)
        {
            AscmLocationMaterialLink locationMaterialLink = null;
            try
            {
                locationMaterialLink = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmLocationMaterialLink>(pk);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmLocationMaterialLink)", ex);
                throw ex;
            }
            return locationMaterialLink;
        }
        public List<AscmLocationMaterialLink> GetList(string hql)
        {
            List<AscmLocationMaterialLink> list = null;
            try
            {
                IList<AscmLocationMaterialLink> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLocationMaterialLink>(hql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmLocationMaterialLink>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmLocationMaterialLink)", ex);
                throw ex;
            }
            return list;
        }
        //public List<AscmLocationMaterialLink> GetList(int warelocationId)
        //{
        //    string hql = "select new AscmLocationMaterialLink(l,'','',m.docNumber,m.description) from AscmLocationMaterialLink l,AscmMaterialItem m";
        //    string where = string.Empty;
        //    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "l.pk.materialId=m.id");
        //    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "l.pk.warelocationId=" + warelocationId);
        //    hql += " where " + where;
        //    string sort = " order by m.docNumber";
        //    return GetList(hql + sort);
        //}

        //public AscmLocationMaterialLink Get(int id)
        //{
        //    AscmLocationMaterialLink locationMaterialLink = null;
        //    try
        //    {
        //        locationMaterialLink = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmLocationMaterialLink>(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmLocationMaterialLink)", ex);
        //        throw ex;
        //    }
        //    return locationMaterialLink;
        //}
        public List<AscmLocationMaterialLink> GetList(string sql, bool isSetMaterial = false, bool isSetWarelocation = false)
        {
            List<AscmLocationMaterialLink> list = null;
            try
            {
                IList<AscmLocationMaterialLink> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLocationMaterialLink>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmLocationMaterialLink>(ilist);
                    if (isSetMaterial)
                        SetMaterial(list);
                    if (isSetWarelocation)
                        SetWarelocation(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmLocationMaterialLink)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmLocationMaterialLink> GetList(int warelocationId, bool isSetMaterial = true)
        {
            List<AscmLocationMaterialLink> list = null;
            try
            {
                string sql = "from AscmLocationMaterialLink ";

                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, " pk.warelocationId=" + warelocationId);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmLocationMaterialLink> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLocationMaterialLink>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmLocationMaterialLink>(ilist);
                    if (isSetMaterial)
                        SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmLocationMaterialLink)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmLocationMaterialLink> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther, bool isSetMaterial = true, bool isSetWarelocation = true)
        {
            List<AscmLocationMaterialLink> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmLocationMaterialLink ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " pk.materialId in(select id from AscmMaterialItem where upper(docNumber) like '%" + queryWord.Trim().ToUpper() + "%' or description like '%" + queryWord.Trim() + "%')"
                                   + " or pk.warelocationId in(select id from AscmWarelocation where upper(docNumber) like '%" + queryWord.Trim().ToUpper() + "%' or description like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmLocationMaterialLink> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLocationMaterialLink>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLocationMaterialLink>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmLocationMaterialLink>(ilist);
                    if (isSetMaterial)
                        SetMaterial(list);
                    if (isSetWarelocation)
                        SetWarelocation(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmLocationMaterialLink)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmLocationMaterialLink> GetListByWarehouseId(string warehouseId, bool isSetMaterial = true, bool isSetWarelocation = true)
        {
            string whereOther = "", whereWarehouse = "";
            if (!string.IsNullOrEmpty(warehouseId))
                whereWarehouse = "pk.warelocationId in(select id from AscmWarelocation where warehouseId='" + warehouseId + "')";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);
            return GetList(null, "warelocationId", "", "", whereOther, isSetMaterial, isSetWarelocation);
        }
        public List<AscmLocationMaterialLink> GetListByWarehouseIds(string warehouseIds, bool isSetMaterial = true, bool isSetWarelocation = true)
        {
            string whereOther = "", whereWarehouse = "";
            if (!string.IsNullOrEmpty(warehouseIds))
                whereWarehouse = "pk.warelocationId in(select id from AscmWarelocation where warehouseId in(" + warehouseIds + "))";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);
            return GetList(null, "warelocationId", "", "", whereOther, isSetMaterial, isSetWarelocation);
        }
        public List<AscmLocationMaterialLink> GetListByWarelocationIds(string warelocationIds)
        {
            if (string.IsNullOrEmpty(warelocationIds))
                return null;

            List<AscmLocationMaterialLink> list = new List<AscmLocationMaterialLink>();
            var ieWarelocationId = warelocationIds.Split(',').Distinct();
            int count = ieWarelocationId.Count();
            string _warelocationIds = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(_warelocationIds))
                    _warelocationIds += ",";
                _warelocationIds += ieWarelocationId.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(_warelocationIds))
                    {
                        string hql = "from AscmLocationMaterialLink where pk.warelocationId in(" + _warelocationIds + ")";
                        IList<AscmLocationMaterialLink> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLocationMaterialLink>(hql);
                        if (ilist != null)
                        {
                            list.AddRange(ilist);
                        }
                    }
                    _warelocationIds = string.Empty;
                }
            }

            return list;
        }

        public List<AscmLocationMaterialLink> GetList(string sortName, string sortOrder, int? buildingId, string queryWord)
        {
            List<AscmLocationMaterialLink> list = null;
            try
            {
                StringBuilder strSQL = new StringBuilder();
                strSQL.Append(" SELECT a.Id, c.DocNumber as MaterialDocNumber, a.Quantity, b.DocNumber as LocationDocNumber ");
                strSQL.Append(" FROM ASCM_LOCATION_MATERIAL_LINK a  ");
                strSQL.Append(" LEFT JOIN ASCM_WARELOCATION b ON a.warelocationid = b.id ");
                strSQL.Append(" LEFT JOIN ASCM_MATERIAL_ITEM c ON a.materialid=c.id  ");
                strSQL.Append(" WHERE b.BUILDINGID IS NOT NULL ");

                if (buildingId.HasValue) 
                {
                    strSQL.Append(" AND  b.BUILDINGID = " + buildingId.Value);
                }
                if (!string.IsNullOrEmpty(queryWord))
                {
                    queryWord = queryWord.ToUpper();
                    strSQL.Append(" AND  c.DocNumber LIKE '%" + queryWord + "%'");
                }
                strSQL.Append(" ORDER BY a.id ");

                ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                OracleCommand command = (OracleCommand)session.Connection.CreateCommand();
                command.CommandText = strSQL.ToString();
                OracleDataAdapter da = new OracleDataAdapter(command);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];

                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    list = new List<AscmLocationMaterialLink>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        AscmLocationMaterialLink entity = new AscmLocationMaterialLink();
                        entity.materialDocNumber = dr["materialDocNumber"].ToString();
                        entity.quantity = Convert.ToDecimal(dr["quantity"]);
                        entity.locationDocNumber = dr["locationDocNumber"].ToString();
                        list.Add(entity);
                    }
                }
                
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmLocationMaterialLink)", ex);
                throw ex;
            }

            return list;
        }

        public void Save(List<AscmLocationMaterialLink> listAscmLocationMaterialLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmLocationMaterialLink);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmLocationMaterialLink)", ex);
                throw ex;
            }
        }
        public void Save(AscmLocationMaterialLink locationMaterialLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(locationMaterialLink);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmLocationMaterialLink)", ex);
                throw ex;
            }
        }
        public void Update(AscmLocationMaterialLink locationMaterialLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmLocationMaterialLink>(locationMaterialLink);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmLocationMaterialLink)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmLocationMaterialLink)", ex);
                throw ex;
            }
        }
        public void Delete(AscmLocationMaterialLinkPK pk)
        {
            try
            {
                AscmLocationMaterialLink locationMaterialLink = Get(pk);
                Delete(locationMaterialLink);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmLocationMaterialLink locationMaterialLink)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmLocationMaterialLink>(locationMaterialLink);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmLocationMaterialLink)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmLocationMaterialLink> listAscmLocationMaterialLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmLocationMaterialLink);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmLocationMaterialLink)", ex);
                throw ex;
            }
        }

        private void SetMaterial(List<AscmLocationMaterialLink> list)
        {
            if (list != null && list.Count > 0)
            {
                var materialIds = list.Select(P => P.pk.materialId).Distinct();
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
                                List<AscmMaterialItem> listMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistMaterialItem);
                                foreach (AscmLocationMaterialLink locationMaterialLink in list)
                                {
                                    AscmMaterialItem materialItem = listMaterialItem.Find(P => P.id == locationMaterialLink.pk.materialId);
                                    if (materialItem != null)
                                    {
                                        locationMaterialLink.materialDocNumber = materialItem.docNumber;
                                        locationMaterialLink.materialDescription = materialItem.description;
                                    }
                                }
                            }
                        }
                        ids = string.Empty;
                    }
                }
            }
        }
        private void SetWarelocation(List<AscmLocationMaterialLink> list)
        {
            if (list != null && list.Count > 0)
            {
                var warelocationIds = list.Select(P => P.pk.warelocationId).Distinct();
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
                            IList<AscmWarelocation> ilistWarelocation = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarelocation>(sql);
                            if (ilistWarelocation != null && ilistWarelocation.Count > 0)
                            {
                                List<AscmWarelocation> listWarelocation = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarelocation>(ilistWarelocation);
                                foreach (AscmLocationMaterialLink locationMaterialLink in list)
                                {
                                    AscmWarelocation warelocation = listWarelocation.Find(P => P.id == locationMaterialLink.pk.warelocationId);
                                    if (warelocation != null)
                                    {
                                        locationMaterialLink.locationDocNumber = warelocation.docNumber;
                                        locationMaterialLink.warehouseId = warelocation.warehouseId;
                                    }
                                }
                            }
                        }
                        ids = string.Empty;
                    }
                }
            }
        }
    }
}
