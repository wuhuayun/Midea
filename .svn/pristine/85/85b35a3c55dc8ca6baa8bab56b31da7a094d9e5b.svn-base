using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;

namespace MideaAscm.Services.SupplierPreparation
{
    public class AscmContainerUnitQuantityService
    {
        private static AscmContainerUnitQuantityService ascmContainerUnitQuantityServices;
        public static AscmContainerUnitQuantityService GetInstance()
        {
            if (ascmContainerUnitQuantityServices == null)
                ascmContainerUnitQuantityServices = new AscmContainerUnitQuantityService();
            return ascmContainerUnitQuantityServices;
        }

        public AscmContainerUnitQuantity Get(int id)
        {
            AscmContainerUnitQuantity ascmContainerUnitQuantity = null;
            try
            {
                ascmContainerUnitQuantity = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmContainerUnitQuantity>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmContainerUnitQuantity)", ex);
                throw ex;
            }
            return ascmContainerUnitQuantity;
        }
        public List<AscmContainerUnitQuantity> GetList(int supplierId, string materialDocNumber)
        {
            List<AscmContainerUnitQuantity> list = null;
            try
            {
                string sql = "from AscmContainerUnitQuantity where materialDocNumber='" + materialDocNumber + "' and supplierId =" + supplierId;
                IList<AscmContainerUnitQuantity> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainerUnitQuantity>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainerUnitQuantity>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmContainerUnitQuantity)", ex);
                throw ex;
            }
            return list;
        }
    }
}
