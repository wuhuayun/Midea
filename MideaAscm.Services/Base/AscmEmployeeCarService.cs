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
    public class AscmEmployeeCarService
    {
        #region base
        private static AscmEmployeeCarService ascmEmployeeCarServices;
        public static AscmEmployeeCarService GetInstance()
        {
            //return ascmEmployeeCarServices ?? new AscmEmployeeCarService();
            if (ascmEmployeeCarServices == null)
                ascmEmployeeCarServices = new AscmEmployeeCarService();
            return ascmEmployeeCarServices;
        }
        public AscmEmployeeCar Get(int id)
        {
            AscmEmployeeCar ascmEmployeeCar = null;
            try
            {
                ascmEmployeeCar = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmEmployeeCar>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmEmployeeCar)", ex);
                throw ex;
            }
            return ascmEmployeeCar;
        }
        public AscmEmployeeCar GetByRfid(string rfid)
        {
            AscmEmployeeCar ascmEmployeeCar = null;
            try
            {
                IList<AscmEmployeeCar> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmployeeCar>("from AscmEmployeeCar where id in (select bindId from AscmRfid where id='" + rfid + "')");// and status='" + AscmRfid.StatusDefine.inUse + "'
                if (ilist != null && ilist.Count > 0)
                {
                    List<AscmEmployeeCar> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmEmployeeCar>(ilist);
                    //SetEmployee(list);
                    return list[0];
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmEmployeeCar)", ex);
                throw ex;
            }
            return ascmEmployeeCar;
        }
        public List<AscmEmployeeCar> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmEmployeeCar> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmEmployeeCar ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    queryWord = queryWord.Trim().Replace(" ", "%");
                    whereQueryWord = " (plateNumber like '%" + queryWord.Trim() + "%' or employeeDocNumber like '%" + queryWord.Trim() + "%' or employeeName like '%" + queryWord.Trim() + "%' or rfid like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                //IList<AscmEmployeeCar> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmployeeCar>(sql + sort);
                IList<AscmEmployeeCar> ilist = null;
                if (ynPage==null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmployeeCar>(sql + sort);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmployeeCar>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmEmployeeCar>(ilist);
                    //SetEmployee(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmEmployeeCar)", ex);
                throw ex;
            }
            return list;
        }
        public void SetEmployee(List<AscmEmployeeCar> list)
        {
            if (list != null && list.Count > 0)
            {
                //string ids = string.Empty;
                //foreach (AscmEmployeeCar ascmEmployeeCar in list)
                //{
                //    if (!string.IsNullOrEmpty(ids))
                //        ids += ",";
                //    ids += "" + ascmEmployeeCar.employeeId + "";
                //}
                //string sql = "from AscmEmployee where id in (" + ids + ")";
                //IList<AscmEmployee> ilistAscmEmployee = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmployee>(sql);
                //if (ilistAscmEmployee != null && ilistAscmEmployee.Count > 0)
                //{
                //    List<AscmEmployee> listAscmEmployee = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmEmployee>(ilistAscmEmployee);
                //    foreach (AscmEmployeeCar ascmEmployeeCar in list)
                //    {
                //        ascmEmployeeCar.ascmEmployee = listAscmEmployee.Find(e => e.id == ascmEmployeeCar.employeeId);
                //    }
                //}
            }
        }
        public void Save(AscmEmployeeCar ascmEmployeeCar)
        {
            try
            {
                int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmEmployeeCar where plateNumber='" + ascmEmployeeCar.plateNumber + "'");
                if (count == 0)
                {
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmEmployeeCar");
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            maxId++;
                            ascmEmployeeCar.id = maxId;
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmEmployeeCar);
                            tx.Commit();//正确执行提交
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            throw ex;
                        }
                    }
                }
                else
                {
                    throw new Exception("已经存在车辆牌号\"" + ascmEmployeeCar.plateNumber + "\"！");
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmEmployeeCar)", ex);
                throw ex;
            }
        }
        public void Update(AscmEmployeeCar ascmEmployeeCar)
        {
            int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmEmployeeCar where id<>" + ascmEmployeeCar.id + " and plateNumber='" + ascmEmployeeCar.plateNumber + "'");
            if (count == 0)
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmEmployeeCar>(ascmEmployeeCar);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmEmployeeCar)", ex);
                        throw ex;
                    }
                }
            }
            else
            {
                throw new Exception("已经存在车辆牌号\"" + ascmEmployeeCar.plateNumber + "\"！");
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmEmployeeCar ascmEmployeeCar = Get(id);
                Delete(ascmEmployeeCar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmEmployeeCar ascmEmployeeCar)
        {
            try
            {
                ////删除与用户的关联
                //string sql = "from YnUserRoleLink where ids.roleId=" + ascmEmployeeCar.id;
                //IList<YnUserRoleLink> ilistUserRoleLink = YnDaoHelper.GetInstance().nHibernateHelper.Find<YnUserRoleLink>(sql);
                //if (ilistUserRoleLink != null && ilistUserRoleLink.Count > 0)
                //{
                //    List<YnUserRoleLink> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnUserRoleLink>(ilistUserRoleLink);
                //    YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(list);
                //}
                ////删除与模块的关联
                //sql = "from YnWebModuleRoleLink where ascmEmployeeCar.id=" + ascmEmployeeCar.id;
                //IList<YnWebModuleRoleLink> ilistModuleRoleLink = YnDaoHelper.GetInstance().nHibernateHelper.Find<YnWebModuleRoleLink>(sql);
                //if (ilistModuleRoleLink != null && ilistModuleRoleLink.Count > 0)
                //{
                //    List<YnWebModuleRoleLink> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnWebModuleRoleLink>(ilistModuleRoleLink);
                //    YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(list);
                //}

                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmEmployeeCar>(ascmEmployeeCar);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmEmployeeCar)", ex);
                throw ex;
            }
        }
        public void Save(bool _new, AscmEmployeeCar ascmEmployeeCar, AscmRfid ascmRfid_Old, AscmRfid ascmRfid_New_Update, AscmRfid ascmRfid_New_Save)
        {
            try
            {
                DateTime dtServerTime = MideaAscm.Dal.YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentDate("AscmEmployeeCar");
                ascmEmployeeCar.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                if (ascmRfid_Old != null)
                    ascmRfid_Old.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                if (ascmRfid_New_Update != null)
                    ascmRfid_New_Update.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                if (ascmRfid_New_Save != null)
                    ascmRfid_New_Save.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        if (_new)
                        {
                            ascmEmployeeCar.createTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmEmployeeCar);
                        }
                        else
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmEmployeeCar);
                        }
                        if (ascmRfid_Old != null)
                            YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmRfid_Old);
                        if (ascmRfid_New_Update != null)
                            YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmRfid_New_Update);
                        if (ascmRfid_New_Save != null)
                        {
                            ascmRfid_New_Save.createTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmRfid_New_Save);
                        }
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmEmployeeCar)", ex);
                throw ex;
            }
        }
        #endregion

        #region 业务
        public void SetExemption(string employeeCarExemptionLevel)
        {
            try
            {
                string[] employeeCarExemptionLevels = employeeCarExemptionLevel.Split(',');
                string whereEmployeeCarExemptionLevel  ="";
                foreach (string _employeeCarExemptionLevels in employeeCarExemptionLevels)
                {
                    if (_employeeCarExemptionLevels.Trim() == "")
                        continue;
                    if (AscmEmployeeCar.EmployeeLevelDefine.GetList().IndexOf(_employeeCarExemptionLevels.Trim()) < 0)
                        throw new Exception("员工级别【"+_employeeCarExemptionLevels.Trim()+"】不存在!");
                    if (!string.IsNullOrEmpty(whereEmployeeCarExemptionLevel))
                        whereEmployeeCarExemptionLevel += ",";
                    whereEmployeeCarExemptionLevel += "'" + _employeeCarExemptionLevels.Trim() + "'";
                }
                if (whereEmployeeCarExemptionLevel != "")
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.ExecuteNonQuery("update Ascm_Employee_Car set exemption=1 where employeeLevel in (" + whereEmployeeCarExemptionLevel + ")");
                    YnDaoHelper.GetInstance().nHibernateHelper.ExecuteNonQuery("update Ascm_Employee_Car set exemption=0 where (employeeLevel is null or employeeLevel not in (" + whereEmployeeCarExemptionLevel + "))");
                }
                else
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.ExecuteNonQuery("update Ascm_Employee_Car set exemption=0");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
