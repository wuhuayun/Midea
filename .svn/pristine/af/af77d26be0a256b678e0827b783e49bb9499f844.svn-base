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
    public class AscmDeliveryNotifyDetailService
    {
        private static AscmDeliveryNotifyDetailService ascmDeliveryNotifyDetailServices;
        public static AscmDeliveryNotifyDetailService GetInstance()
        {
            //return ascmDeliveryNotifyDetailServices ?? new AscmDeliveryNotifyDetailService();
            if (ascmDeliveryNotifyDetailServices == null)
                ascmDeliveryNotifyDetailServices = new AscmDeliveryNotifyDetailService();
            return ascmDeliveryNotifyDetailServices;
        }
        public AscmDeliveryNotifyDetail Get(string id)
        {
            AscmDeliveryNotifyDetail ascmDeliveryNotify = null;
            try
            {
                ascmDeliveryNotify = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDeliveryNotifyDetail>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDeliveryNotifyDetail)", ex);
                throw ex;
            }
            return ascmDeliveryNotify;
        }
        public List<AscmDeliveryNotifyDetail> GetList(int mainId)
        {
            List<AscmDeliveryNotifyDetail> list = null;
            try
            {
                string sort = " order by id ";
                //if (!string.IsNullOrEmpty(sortName))
                //{
                //    sort = " order by " + sortName.Trim() + " ";
                //    if (!string.IsNullOrEmpty(sortOrder))
                //        sort += sortOrder.Trim();
                //}
                string sql = "from AscmDeliveryNotifyDetail where mainId=" + mainId;

                string where = "", whereQueryWord = "";
                //if (!string.IsNullOrEmpty(queryWord))
                //    whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                //where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " and " + where;
                //IList<AscmDeliveryNotifyDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryNotifyDetail>(sql + sort);
                IList<AscmDeliveryNotifyDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryNotifyDetail>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryNotifyDetail>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliveryNotifyDetail)", ex);
                throw ex;
            }
            return list;
        }
        public void SetMain(List<AscmDeliveryNotifyDetail> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDeliveryNotifyDetail ascmDeliveryNotifyDetail in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmDeliveryNotifyDetail.mainId + "";
                }
                string sql = "from AscmDeliveryNotifyMain where id in (" + ids + ")";
                IList<AscmDeliveryNotifyMain> ilistAscmDeliveryNotifyMain = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryNotifyMain>(sql);
                if (ilistAscmDeliveryNotifyMain != null && ilistAscmDeliveryNotifyMain.Count > 0)
                {
                    List<AscmDeliveryNotifyMain> listAscmDeliveryNotifyMain = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryNotifyMain>(ilistAscmDeliveryNotifyMain);
                    foreach (AscmDeliveryNotifyDetail ascmDeliveryNotifyDetail in list)
                    {
                        ascmDeliveryNotifyDetail.ascmDeliveryNotifyMain = listAscmDeliveryNotifyMain.Find(e => e.id == ascmDeliveryNotifyDetail.mainId);
                    }
                }
            }
        }
    }
}
