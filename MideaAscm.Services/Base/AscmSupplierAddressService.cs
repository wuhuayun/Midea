using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;

namespace MideaAscm.Services.Base
{
    public class AscmSupplierAddressService
    {
        private static AscmSupplierAddressService ascmSupplierAddressServices;
        public static AscmSupplierAddressService GetInstance()
        {
            //return ascmSupplierAddressServices ?? new AscmSupplierAddressService();
            if (ascmSupplierAddressServices == null)
                ascmSupplierAddressServices = new AscmSupplierAddressService();
            return ascmSupplierAddressServices;

        }
        public AscmSupplierAddress Get(int id)
        {
            AscmSupplierAddress ascmSupplierAddress = null;
            try
            {
                ascmSupplierAddress = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmSupplierAddress>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmSupplierAddress)", ex);
                throw ex;
            }
            return ascmSupplierAddress;
        }
        public List<AscmSupplierAddress> GetList(int vendorId)
        {
            List<AscmSupplierAddress> list = null;
            try
            {
                string sort = " order by vendorSiteId ";
                string sql = "from AscmSupplierAddress where vendorId=" + vendorId;

                IList<AscmSupplierAddress> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplierAddress>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplierAddress>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmSupplierAddress)", ex);
                throw ex;
            }
            return list;
        }
        public void Update(AscmSupplierAddress ascmSupplierAddress)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmSupplierAddress>(ascmSupplierAddress);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmSupplierAddress)", ex);
                    throw ex;
                }
            }
        }
    }
}
