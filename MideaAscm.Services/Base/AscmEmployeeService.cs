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
    public class AscmEmployeeService
    {
        private static AscmEmployeeService ascmEmployeeServices;
        public static AscmEmployeeService GetInstance()
        {
            //return ascmEmployeeServices ?? new AscmEmployeeService();
            if (ascmEmployeeServices == null)
                ascmEmployeeServices = new AscmEmployeeService();
            return ascmEmployeeServices;
        }
        public AscmEmployee Get(int id)
        {
            AscmEmployee ascmEmployee = null;
            try
            {
                ascmEmployee = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmEmployee>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmEmployee)", ex);
                throw ex;
            }
            return ascmEmployee;
        }
        public AscmEmployee GetByRfid(string rfid)
        {
            AscmEmployee ascmEmployee = null;
            try
            {
                IList<AscmEmployee> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmployee>("from AscmEmployee where id in (select bindId from AscmRfid where id='" + rfid + "')");// and status='" + AscmRfid.StatusDefine.inUse + "'
                if (ilist != null&&ilist.Count>0)
                {
                    return ilist[0];
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmEmployee)", ex);
                throw ex;
            }
            return ascmEmployee;
        }
        public List<AscmEmployee> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord,string whereOther)
        {
            List<AscmEmployee> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmEmployee ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    queryWord = queryWord.Trim().Replace(" ", "%");
                    whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or docNumber like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                //IList<AscmEmployee> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmployee>(sql + sort);
                IList<AscmEmployee> ilist = null;
                if (ynPage!=null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmployee>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmployee>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmEmployee>(ilist);
                    //SetDepartment(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmEmployee)", ex);
                throw ex;
            }
            return list;
        }
        /*private void SetDepartment(List<AscmEmployee> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmEmployee ascmEmployee in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmEmployee.departmentId + "";
                }
                string sql = "from YnFrame.Dal.Entities.YnDepartment where id in (" + ids + ")";
                IList<YnFrame.Dal.Entities.YnDepartment> ilistYnDepartment = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.Find<YnFrame.Dal.Entities.YnDepartment>(sql);
                if (ilistYnDepartment != null && ilistYnDepartment.Count > 0)
                {
                    List<YnFrame.Dal.Entities.YnDepartment> listYnDepartment = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnFrame.Dal.Entities.YnDepartment>(ilistYnDepartment);
                    foreach (AscmEmployee ascmEmployee in list)
                    {
                        ascmEmployee.ynDepartment = listYnDepartment.Find(e => e.id == ascmEmployee.departmentId);
                    }
                }
            }
        }*/
        public void Save(AscmEmployee ascmEmployee)
        {
            try
            {
                int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmEmployee where docNumber='" + ascmEmployee.docNumber + "'");
                if (count == 0)
                {
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmEmployee");
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            maxId++;
                            ascmEmployee.id = maxId;
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmEmployee);
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
                    throw new Exception("已经存在员工编号\"" + ascmEmployee.name + "\"！");
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmEmployee)", ex);
                throw ex;
            }
        }
        public void Update(AscmEmployee ascmEmployee)
        {
            int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmEmployee where id<>" + ascmEmployee.id + " and docNumber='" + ascmEmployee.docNumber + "'");
            if (count == 0)
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmEmployee>(ascmEmployee);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmEmployee)", ex);
                        throw ex;
                    }
                }
            }
            else
            {
                throw new Exception("已经存在员工编号\"" + ascmEmployee.name + "\"！");
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmEmployee ascmEmployee = Get(id);
                Delete(ascmEmployee);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmEmployee ascmEmployee)
        {
            try
            {
                //删除与用户的关联
                string sql = "from AscmEmployeeCar where employeeId=" + ascmEmployee.id;
                IList<AscmEmployeeCar> ilistAscmEmployeeCar = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmployeeCar>(sql);
                if (ilistAscmEmployeeCar != null && ilistAscmEmployeeCar.Count > 0)
                {
                    List<AscmEmployeeCar> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmEmployeeCar>(ilistAscmEmployeeCar);
                    YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(list);
                }
                ////删除与模块的关联
                //sql = "from YnWebModuleRoleLink where ascmEmployee.id=" + ascmEmployee.id;
                //IList<YnWebModuleRoleLink> ilistModuleRoleLink = YnDaoHelper.GetInstance().nHibernateHelper.Find<YnWebModuleRoleLink>(sql);
                //if (ilistModuleRoleLink != null && ilistModuleRoleLink.Count > 0)
                //{
                //    List<YnWebModuleRoleLink> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnWebModuleRoleLink>(ilistModuleRoleLink);
                //    YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(list);
                //}

                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmEmployee>(ascmEmployee);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmEmployee)", ex);
                throw ex;
            }
        }
    }
}
