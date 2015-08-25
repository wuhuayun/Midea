using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Services.FromErp
{
    public class AscmCuxWipReleaseLinesService
    {
        private static AscmCuxWipReleaseLinesService ascmCuxWipReleaseLinesServices;
        public static AscmCuxWipReleaseLinesService GetInstance()
        {
            //return ascmCuxWipReleaseLinesServices ?? new AscmCuxWipReleaseLinesService();
            if (ascmCuxWipReleaseLinesServices == null)
                ascmCuxWipReleaseLinesServices = new AscmCuxWipReleaseLinesService();
            return ascmCuxWipReleaseLinesServices;
        }
        public AscmCuxWipReleaseLines Get(string id)
        {
            AscmCuxWipReleaseLines ascmDeliveryOrder = null;
            try
            {
                ascmDeliveryOrder = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmCuxWipReleaseLines>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmCuxWipReleaseLines)", ex);
                throw ex;
            }
            return ascmDeliveryOrder;
        }
        public List<AscmCuxWipReleaseLines> GetList(string whereOther, bool isSetMaterial = false, 
            bool isSetWipRequirementOperations = false, bool isSetWipReleaseHeader = false)
        {
            List<AscmCuxWipReleaseLines> list = null;
            try
            {
                string sort = " order by inventoryItemId ";
                string sql = "from AscmCuxWipReleaseLines";

                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmCuxWipReleaseLines> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmCuxWipReleaseLines>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmCuxWipReleaseLines>(ilist);
                    if (isSetWipReleaseHeader)
                        SetWipReleaseHeader(list);
                    if (isSetMaterial)
                        SetMaterial(list);
                    if (isSetWipRequirementOperations)
                        SetWipRequirementOperations(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmCuxWipReleaseLines)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmCuxWipReleaseLines> GetList(int releaseHeaderId, string whereOther = "")
        {
            List<AscmCuxWipReleaseLines> list = null;
            try
            {
                string sort = " order by inventoryItemId ";
                //if (!string.IsNullOrEmpty(sortName))
                //{
                //    sort = " order by " + sortName.Trim() + " ";
                //    if (!string.IsNullOrEmpty(sortOrder))
                //        sort += sortOrder.Trim();
                //}
                AscmCuxWipReleaseHeaders ascmCuxWipReleaseHeaders=AscmCuxWipReleaseHeadersService.GetInstance().Get(releaseHeaderId);
                string sql = "from AscmCuxWipReleaseLines where releaseHeaderId=" + releaseHeaderId;

                string where = "", whereQueryWord = "";
                //if (!string.IsNullOrEmpty(queryWord))
                //    whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " and " + where;
                //IList<AscmCuxWipReleaseLines> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmCuxWipReleaseLines>(sql + sort);
                IList<AscmCuxWipReleaseLines> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmCuxWipReleaseLines>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmCuxWipReleaseLines>(ilist);
                    SetMaterial(list);
                    SetWipRequirementOperations(list);
                    //计算未发数
                    foreach (AscmCuxWipReleaseLines ascmCuxWipReleaseLines in list)
                    {
                        ascmCuxWipReleaseLines.QUANTITY_AV = 0;
                        if (ascmCuxWipReleaseLines.ascmWipRequirementOperations != null)
                        {
                            if (ascmCuxWipReleaseHeaders.releaseType == "RETURN")
                            {
                                ascmCuxWipReleaseLines.QUANTITY_AV = ascmCuxWipReleaseLines.ascmWipRequirementOperations.quantityIssued;
                            }
                            else
                            {
                                int sign_REQUIRED_QUANTITY = 0;
                                if (ascmCuxWipReleaseLines.ascmWipRequirementOperations.requiredQuantity > 0)
                                {
                                    sign_REQUIRED_QUANTITY = 1;
                                }
                                else if (ascmCuxWipReleaseLines.ascmWipRequirementOperations.requiredQuantity < 0)
                                {
                                    sign_REQUIRED_QUANTITY = -1;
                                }
                                int sign_QUANTITY_ISSUED = 0;
                                if (ascmCuxWipReleaseLines.ascmWipRequirementOperations.quantityIssued > 0)
                                {
                                    sign_QUANTITY_ISSUED = 1;
                                }
                                else if (ascmCuxWipReleaseLines.ascmWipRequirementOperations.quantityIssued < 0)
                                {
                                    sign_QUANTITY_ISSUED = -1;
                                }
                                if (sign_REQUIRED_QUANTITY == sign_QUANTITY_ISSUED)
                                {
                                    ascmCuxWipReleaseLines.QUANTITY_AV = ascmCuxWipReleaseLines.ascmWipRequirementOperations.requiredQuantity - ascmCuxWipReleaseLines.ascmWipRequirementOperations.quantityIssued;
                                }
                                else if (Math.Abs(ascmCuxWipReleaseLines.ascmWipRequirementOperations.requiredQuantity) >= Math.Abs(ascmCuxWipReleaseLines.ascmWipRequirementOperations.quantityIssued))
                                {
                                    ascmCuxWipReleaseLines.QUANTITY_AV = ascmCuxWipReleaseLines.ascmWipRequirementOperations.requiredQuantity - ascmCuxWipReleaseLines.ascmWipRequirementOperations.quantityIssued;
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmCuxWipReleaseLines)", ex);
                throw ex;
            }
            return list;
        }

        private void SetWipReleaseHeader(List<AscmCuxWipReleaseLines> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmCuxWipReleaseLines ascmCuxWipReleaseLines in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmCuxWipReleaseLines.releaseHeaderId + "";
                }
                string sql = "from AscmCuxWipReleaseHeaders where releaseHeaderId in (" + ids + ")";
                IList<AscmCuxWipReleaseHeaders> ilistAscmCuxWipReleaseHeaders = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmCuxWipReleaseHeaders>(sql);
                if (ilistAscmCuxWipReleaseHeaders != null && ilistAscmCuxWipReleaseHeaders.Count > 0)
                {
                    List<AscmCuxWipReleaseHeaders> listAscmCuxWipReleaseHeaders = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmCuxWipReleaseHeaders>(ilistAscmCuxWipReleaseHeaders);
                    foreach (AscmCuxWipReleaseLines ascmCuxWipReleaseLines in list)
                    {
                        ascmCuxWipReleaseLines.ascmCuxWipReleaseHeaders = listAscmCuxWipReleaseHeaders.Find(e => e.releaseHeaderId == ascmCuxWipReleaseLines.releaseHeaderId);
                    }
                }
            }
        }
        private void SetMaterial(List<AscmCuxWipReleaseLines> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmCuxWipReleaseLines ascmCuxWipReleaseLines in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmCuxWipReleaseLines.inventoryItemId + "";
                }
                string sql = "from AscmMaterialItem where id in (" + ids + ")";
                IList<AscmMaterialItem> ilistAscmMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                if (ilistAscmMaterialItem != null && ilistAscmMaterialItem.Count > 0)
                {
                    List<AscmMaterialItem> listAscmMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistAscmMaterialItem);
                    foreach (AscmCuxWipReleaseLines ascmCuxWipReleaseLines in list)
                    {
                        ascmCuxWipReleaseLines.ascmMaterialItem = listAscmMaterialItem.Find(e => e.id == ascmCuxWipReleaseLines.inventoryItemId);
                    }
                }
            }
        }
        private void SetWipRequirementOperations(List<AscmCuxWipReleaseLines> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                string ids1 = string.Empty;
                foreach (AscmCuxWipReleaseLines ascmCuxWipReleaseLines in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmCuxWipReleaseLines.inventoryItemId + "";
                    if (!string.IsNullOrEmpty(ids1))
                        ids1 += ",";
                    ids1 += "" + ascmCuxWipReleaseLines.wipEntityId + "";
                }
                string sql = "from AscmWipRequirementOperations where inventoryItemId in (" + ids + ") and wipEntityId in (" + ids1 + ")";
                IList<AscmWipRequirementOperations> ilistAscmWipRequirementOperations = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql);
                if (ilistAscmWipRequirementOperations != null && ilistAscmWipRequirementOperations.Count > 0)
                {
                    List<AscmWipRequirementOperations> listAscmWipRequirementOperations = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilistAscmWipRequirementOperations);
                    foreach (AscmCuxWipReleaseLines ascmCuxWipReleaseLines in list)
                    {
                        ascmCuxWipReleaseLines.ascmWipRequirementOperations = listAscmWipRequirementOperations.Find(e => e.inventoryItemId == ascmCuxWipReleaseLines.inventoryItemId && e.wipEntityId == ascmCuxWipReleaseLines.wipEntityId);
                    }
                }
            }
        }
    }
}
