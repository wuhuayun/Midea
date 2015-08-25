using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using MideaAscm.Services.GetMaterialManage;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Services.FromErp
{
    public class AscmMtlOnhandQuantitiesDetailService
    {
        private static AscmMtlOnhandQuantitiesDetailService ascmMtlOnhandQuantitiesDetailServices;
        public static AscmMtlOnhandQuantitiesDetailService GetInstance()
        {
            //return ascmDeliveryOrderMainServices ?? new ascmDeliveryOrderMainServices();
            if (ascmMtlOnhandQuantitiesDetailServices == null)
                ascmMtlOnhandQuantitiesDetailServices = new AscmMtlOnhandQuantitiesDetailService();
            return ascmMtlOnhandQuantitiesDetailServices;
        }
        public AscmMtlOnhandQuantitiesDetail Get(int id)
        {
            AscmMtlOnhandQuantitiesDetail ascmMtlOnhandQuantitiesDetail = null;
            try
            {
                ascmMtlOnhandQuantitiesDetail = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmMtlOnhandQuantitiesDetail>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get ascmMtlOnhandQuantitiesDetail)", ex);
                throw ex;
            }
            return ascmMtlOnhandQuantitiesDetail;
        }

        public List<AscmMtlOnhandQuantitiesDetail> GetSumList(YnPage ynPage, int materialId)
        {
            List<AscmMtlOnhandQuantitiesDetail> list = null;

            try
            {
                string hql = "select new AscmMtlOnhandQuantitiesDetail(inventoryItemId, subinventoryCode, sum(transactionQuantity)) from AscmMtlOnhandQuantitiesDetail where inventoryItemId = {0} group by inventoryItemId, subinventoryCode";
                string sql = "select inventoryItemId, subinventoryCode, sum(transactionQuantity) from ASCM_MTL_ONHAND_QUANTITIES where inventoryItemId = {0} group by inventoryItemId, subinventoryCode";

                if (materialId > 0)
                {
                    hql = string.Format(hql, materialId.ToString());
                    sql = string.Format(sql, materialId.ToString());
                }

                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObjectBySQL("select count(*) from (" + sql + ")");
                int count = 0;
                int.TryParse(object1.ToString(), out count);
                IList<AscmMtlOnhandQuantitiesDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMtlOnhandQuantitiesDetail>(hql, count, ynPage);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMtlOnhandQuantitiesDetail>(ilist);
                    SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("统计失败(Sum AscmMtlOnhandQuantitiesDetail)", ex);
                throw ex;
            }

            return list;
        }

        public void SetMaterial(List<AscmMtlOnhandQuantitiesDetail> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids  = string.Empty;
                foreach (AscmMtlOnhandQuantitiesDetail ascmMtlOnhandQuantitiesDetail in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmMtlOnhandQuantitiesDetail.inventoryItemId;
                }

                string sql = "from AscmMaterialItem";
                string where = "", whereQueryWord = "";
                whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmMaterialItem> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                if (ilist != null && ilist.Count > 0)
                {
                    List<AscmMaterialItem> newlist = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilist);
                    foreach (AscmMtlOnhandQuantitiesDetail ascmMtlOnhandQuantitiesDetail in list)
                    {
                        ascmMtlOnhandQuantitiesDetail.ascmMaterialItem = newlist.Find(e => e.id == ascmMtlOnhandQuantitiesDetail.inventoryItemId);
                    }
                }
            }
        }
    }
}
