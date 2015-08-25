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
    public class AscmDoorService
    {
        private static AscmDoorService ascmDoorServices;
        public static AscmDoorService GetInstance()
        {
            //return ascmDoorServices ?? new AscmDoorService();
            if (ascmDoorServices == null)
                ascmDoorServices = new AscmDoorService();
            return ascmDoorServices;
        }
        public AscmDoor Get(int id)
        {
            AscmDoor ascmDoor = null;
            try
            {
                ascmDoor = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDoor>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDoor)", ex);
                throw ex;
            }
            return ascmDoor;
        }
        public List<AscmDoor> GetList()
        {
            List<AscmDoor> list = null;
            try
            {
                string sql = "from AscmDoor ";
                IList<AscmDoor> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDoor>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDoor>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDoor)", ex);
                throw ex;
            }
            return list;
        }
        public void Update(AscmDoor ascmDoor)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmDoor>(ascmDoor);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmDoor)", ex);
                    throw ex;
                }
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmDoor ascmDoor = Get(id);
                Delete(ascmDoor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmDoor ascmDoor)
        {
            try
            {
                ////删除与用户的关联
                //string sql = "from AscmEmployeeCar where employeeId=" + ascmEmployee.id;
                //IList<AscmEmployeeCar> ilistAscmEmployeeCar = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmployeeCar>(sql);
                //if (ilistAscmEmployeeCar != null && ilistAscmEmployeeCar.Count > 0)
                //{
                //    List<AscmEmployeeCar> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmEmployeeCar>(ilistAscmEmployeeCar);
                //    YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(list);
                //}
                ////删除与模块的关联
                //sql = "from YnWebModuleRoleLink where ascmEmployee.id=" + ascmEmployee.id;
                //IList<YnWebModuleRoleLink> ilistModuleRoleLink = YnDaoHelper.GetInstance().nHibernateHelper.Find<YnWebModuleRoleLink>(sql);
                //if (ilistModuleRoleLink != null && ilistModuleRoleLink.Count > 0)
                //{
                //    List<YnWebModuleRoleLink> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnWebModuleRoleLink>(ilistModuleRoleLink);
                //    YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(list);
                //}

                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmDoor>(ascmDoor);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmDoor)", ex);
                throw ex;
            }
        }
    }
}
