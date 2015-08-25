using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using System.Security.Principal;
using YnFrame.Dal.Entities;
using YnFrame.Services;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Services.GetMaterialManage;
using MideaAscm.Services.Warehouse;

namespace MideaAscm.Services.Base
{
    public class AscmUserInfoService
    {
        private static AscmUserInfoService ascmUserInfoServices;
        public static AscmUserInfoService GetInstance()
        {
            //return ascmUserInfoServices ?? new AscmUserInfoService();
            if (ascmUserInfoServices == null)
                ascmUserInfoServices = new AscmUserInfoService();
            return ascmUserInfoServices;
        }
        public AscmUserInfo Get(string id)
        {
            AscmUserInfo ascmUserInfo = null;
            try
            {
                ascmUserInfo = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmUserInfo>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmUserInfo)", ex);
                throw ex;
            }
            return ascmUserInfo;
        }
        public AscmUserInfo TryGet(string id)
        {
            AscmUserInfo ascmUserInfo = null;
            try
            {
                string sql = "from AscmUserInfo where userId='" + id + "' or userName='" + id + "'";
                IList<AscmUserInfo> ilist = YnDaoHelper.GetInstance().nHibernateHelper.FindTop<AscmUserInfo>(sql, 1);
                if (ilist != null && ilist.Count > 0)
                {
                    ascmUserInfo = ilist[0];
                }
                if (ascmUserInfo == null)
                {
                    //判断是否erp用户
                    ascmUserInfo = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmUserInfo>("erp_" + id);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmUserInfo)", ex);
                throw ex;
            }
            return ascmUserInfo;
        }
        public List<AscmUserInfo> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther, bool isSetDepartmentPosition = true, bool isLogisticsClassName = true)
        {
            List<AscmUserInfo> list = null;
            try
            {
                string sort = " order by userId ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmUserInfo a";
                string where = "", whereQueryWord = "", whereOrganization = " (organizationId=0 or organizationId is null) ";
                if (!string.IsNullOrEmpty(queryWord))
                    whereQueryWord = " (userId like '%" + queryWord.Trim() + "%' or userName like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                //if (!string.IsNullOrEmpty(organizationId))
                //    whereOrganization = " a.organizationId=" + organizationId;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOrganization);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmUserInfo> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUserInfo>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUserInfo>(sql + sort);
                if (ilist != null)
                {
                    
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUserInfo>(ilist);
                    List<YnFrame.Dal.Entities.YnUser> list1 = new List<YnFrame.Dal.Entities.YnUser>();
                    foreach (AscmUserInfo ascmUserInfo in list)
                    {
                        list1.Add((YnFrame.Dal.Entities.YnUser)ascmUserInfo);
                    }
                    //YnFrame.Services.YnUserService.GetInstance().SetRole(list1);
                    if (isSetDepartmentPosition)
                        YnFrame.Services.YnUserService.GetInstance().SetDepartmentPosition(list1);

                    if (isLogisticsClassName)
                        AscmUserInfoService.GetInstance().SetUserLogisticsClass(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find YnUser)", ex);
                throw ex;
            }
            return list;
        }

        public List<AscmUserInfo> GetList(int teamId)
        {
            List<AscmUserInfo> list = new List<AscmUserInfo>();
            try
            {
                StringBuilder sql = new StringBuilder(" from AscmUserInfo a ");
                StringBuilder userIds = new StringBuilder();
                List<AscmWhTeamUser> listTeamUser = AscmWhTeamUserService.Instance.GetList(teamId);
                if (listTeamUser == null || listTeamUser.Count == 0) 
                {
                    return list;
                }

                for (int i = 0; i < listTeamUser.Count; i++)
                {
                    userIds.Append("'" + listTeamUser[i].M_UserId + "'");

                    if (i < listTeamUser.Count - 1) 
                    {
                        userIds.Append(",");
                    }
                }

                sql.AppendFormat(" where userId in ({0}) ", userIds);
                sql.AppendFormat(" order by userId ");

                var ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUserInfo>(sql.ToString());
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUserInfo>(ilist);

                    AscmWhTeamUser leader = listTeamUser.FirstOrDefault(a => a.isLeader == true);
                    if (leader != null) 
                    {
                        foreach (var item in list)
                        {
                            if (item.userId == leader.M_UserId) 
                            {
                                item.isLeader = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find YnUser)", ex);
                throw ex;
            }
            return list;
        }

        //查询所属部门用户@2014/4/25
        public List<AscmUserInfo> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther, string departName, bool isSetDepartmentPosition = true, bool isLogisticsClassName = true)
        {
            List<AscmUserInfo> list = null;
            try
            {
                string hql = "select u.* from YnUser u, YnDepartmentPositionUserLink l, YnDepartment d";

                string sort = " order by u.userId ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                string where = "", whereQueryWord = "", whereOrganization = " (u.organizationId=0 or u.organizationId is null) ";

                whereQueryWord = "l.departmentId = d.id";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                whereQueryWord = "l.userId = u.userId";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(departName))
                {
                    whereQueryWord = "d.name = '" + departName + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " (u.userId like '%" + queryWord.Trim() + "%' or u.userName like '%" + queryWord.Trim() + "%' or u.description like '%" + queryWord.Trim() + "%')";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOrganization);

                if (!string.IsNullOrEmpty(where))
                    hql += " where " + where;
                //string hql_Param = hql.Replace("u.*", "count(u.userId)");
                //object obj = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.GetObjectBySQL(hql_Param);
                //int count = 0;
                //int.TryParse(obj.ToString(), out count);

                //IList<AscmUserInfo> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUserInfo>(hql, count, ynPage);
                NHibernate.ISQLQuery query = YnDaoHelper.GetInstance().nHibernateHelper.FindBySQLQuery(hql);
                IList<AscmUserInfo> ilist = query.AddEntity("u", typeof(AscmUserInfo)).List<AscmUserInfo>();
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUserInfo>(ilist);
                    List<YnFrame.Dal.Entities.YnUser> list1 = new List<YnFrame.Dal.Entities.YnUser>();
                    foreach (AscmUserInfo ascmUserInfo in list)
                    {
                        list1.Add((YnFrame.Dal.Entities.YnUser)ascmUserInfo);
                    }
                    //YnFrame.Services.YnUserService.GetInstance().SetRole(list1);
                    if (isSetDepartmentPosition)
                        YnFrame.Services.YnUserService.GetInstance().SetDepartmentPosition(list1);

                    if (isLogisticsClassName)
                        AscmUserInfoService.GetInstance().SetUserLogisticsClass(list);
                }

            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find YnUser)", ex);
                throw ex;
            }

            return list;
        }
        //查询某角色所属部门用户@2014/5/5
        public List<AscmUserInfo> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmUserInfo> list = null;
            try
            {
                string hql = "select u.* from YnDepartmentPositionUserLink l, YnPosition p, YnUser u";

                string sort = " order by u.userId ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                string where = "", whereQueryWord = "", whereOrganization = " (u.organizationId=0 or u.organizationId is null) ";

                whereQueryWord = "p.id = l.positionId";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                whereQueryWord = "u.userId = l.userId";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = "u.userId like '" + queryWord + "%' or u.userName like '" + queryWord + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOrganization);

                if (!string.IsNullOrEmpty(where))
                    hql += " where " + where;

                NHibernate.ISQLQuery query = YnDaoHelper.GetInstance().nHibernateHelper.FindBySQLQuery(hql);
                IList<AscmUserInfo> ilist = query.AddEntity("u", typeof(AscmUserInfo)).List<AscmUserInfo>();
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUserInfo>(ilist);
                    List<YnFrame.Dal.Entities.YnUser> list1 = new List<YnFrame.Dal.Entities.YnUser>();
                    foreach (AscmUserInfo ascmUserInfo in list)
                    {
                        list1.Add((YnFrame.Dal.Entities.YnUser)ascmUserInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find YnUser)", ex);
                throw ex;
            }

            return list;
        }
        public void SetEmployee(List<AscmUserInfo> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmUserInfo ascmUserInfo in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmUserInfo.employeeId + "";
                }
                string sql = "from AscmEmployee where id in (" + ids + ")";
                //sql = "from AscmEmployee where id in (" + sql + ")";
                IList<AscmEmployee> ilistAscmEmployee = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmEmployee>(sql);
                if (ilistAscmEmployee != null && ilistAscmEmployee.Count > 0)
                {
                    List<AscmEmployee> listAscmEmployee = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmEmployee>(ilistAscmEmployee);
                    foreach (AscmUserInfo ascmUserInfo in list)
                    {
                        ascmUserInfo.ascmEmployee = listAscmEmployee.Find(e => e.id == ascmUserInfo.employeeId);
                    }
                }
            }
        }
        public void SetSupplier(AscmUserInfo ascmUserInfo)
        {
            if (ascmUserInfo != null )
            {
                //string sql = "from AscmSupplier where id in (select numberValue from AscmAkWebUserSecAttrValues where attributeCode='" + MideaAscm.Dal.FromErp.Entities.AscmAkWebUserSecAttrValues.AttributeCodeDefine.supplierUser + "' and id=" + ascmUserInfo.extExpandId + ")";
                string sql = "from AscmSupplier where id in (select numberValue from AscmAkWebUserSecAttrValues where attributeCode='" + MideaAscm.Dal.FromErp.Entities.AscmAkWebUserSecAttrValues.AttributeCodeDefine.supplierUser + "' and webUserId=" + ascmUserInfo.extExpandId + ")";
                //sql = "from AscmEmployee where id in (" + sql + ")";
                IList<AscmSupplier> ilistAscmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql);
                if (ilistAscmSupplier != null && ilistAscmSupplier.Count > 0)
                {
                    ascmUserInfo.ascmSupplier = ilistAscmSupplier[0];

                }
            }
        }
        public void Update(AscmUserInfo ascmUserInfo)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmUserInfo>(ascmUserInfo);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmUserInfo)", ex);
                    throw ex;
                }
            }
        }

        #region 应用
        /// <summary>手持登录</summary>
        public AscmUserInfo TryGet2(string userId)
        {
            AscmUserInfo ascmUserInfo = null;
            try
            {
                string sql = "from AscmUserInfo where extExpandId='" + userId + "'";
                IList<AscmUserInfo> ilist = YnDaoHelper.GetInstance().nHibernateHelper.FindTop<AscmUserInfo>(sql, 1);
                if (ilist != null && ilist.Count > 0)
                {
                    ascmUserInfo = ilist[0];
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmUserInfo)", ex);
                throw ex;
            }
            return ascmUserInfo;
        }
        /// <summary>手持登录</summary>
        public AscmUserInfo MobileLogin(string userId, string userPwd, string connString, ref string errorMsg)
        {
            AscmUserInfo ascmUserInfo = null;
            errorMsg = string.Empty;
            using (Oracle.DataAccess.Client.OracleConnection conn = new Oracle.DataAccess.Client.OracleConnection(connString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT userId,userName,password,employeeId,extExpandType,extExpandId FROM ynUser WHERE extExpandId = :extExpandId";
                cmd.CommandType = System.Data.CommandType.Text;

                Oracle.DataAccess.Client.OracleParameter parm = new Oracle.DataAccess.Client.OracleParameter();
                parm.ParameterName = ":extExpandId";
                parm.OracleDbType = Oracle.DataAccess.Client.OracleDbType.NVarchar2;
                parm.Size = 20;
                parm.Value = userId;
                parm.Direction = System.Data.ParameterDirection.Input;
                cmd.Parameters.Add(parm);

                using (Oracle.DataAccess.Client.OracleDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                {
                    cmd.Parameters.Clear();

                    if (reader.Read())
                    {
                        ascmUserInfo = new AscmUserInfo();
                        ascmUserInfo.userId = reader["userId"].ToString();
                        ascmUserInfo.userName = reader["userName"].ToString();
                        ascmUserInfo.password = reader["password"].ToString();
                        int employeeId = 0;
                        int.TryParse(reader["employeeId"].ToString(), out employeeId);
                        ascmUserInfo.employeeId = employeeId;
                        ascmUserInfo.extExpandType = reader["extExpandType"].ToString();
                        ascmUserInfo.extExpandId = reader["extExpandId"].ToString();

                        if (ascmUserInfo.extExpandType == "erp")
                        {
                            byte[] result = Encoding.Default.GetBytes(userPwd);
                            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                            userPwd = BitConverter.ToString(md5.ComputeHash(result)).Replace("-", "");
                            if (ascmUserInfo.password != userPwd)
                            {
                                errorMsg = "密码不正确";
                            }
                            else if (!string.IsNullOrEmpty(ascmUserInfo.userName))
                            {
                                Oracle.DataAccess.Client.OracleCommand cmd2 = new Oracle.DataAccess.Client.OracleCommand();
                                cmd2.Connection = conn;
                                cmd2.CommandText = "SELECT id,name FROM ascm_supplier WHERE docNumber = :docNumber";
                                cmd2.CommandType = System.Data.CommandType.Text;
                                cmd2.Parameters.Add(new Oracle.DataAccess.Client.OracleParameter {
                                    ParameterName = ":docNumber",
                                    OracleDbType = Oracle.DataAccess.Client.OracleDbType.NVarchar2,
                                    Size = 20,
                                    Value = ascmUserInfo.userName,
                                    Direction = System.Data.ParameterDirection.Input
                                });

                                using (Oracle.DataAccess.Client.OracleDataReader reader2 = cmd2.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                                {
                                    cmd2.Parameters.Clear();
                                    if (reader2.Read())
                                    {
                                        int id = 0;
                                        if (int.TryParse(reader2["id"].ToString(), out id))
                                        {
                                            AscmSupplier ascmSupplier = new AscmSupplier();
                                            ascmSupplier.id = id;
                                            ascmSupplier.name = reader2["name"].ToString();
                                            ascmUserInfo.ascmSupplier = ascmSupplier;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return ascmUserInfo;
        }
        #endregion

        #region 2014/3/25
        public string GetUserRoleName(string userName)
        {
            string userRole = string.Empty;
            try
            {
                List<YnRole> listYnRole = YnRoleService.GetInstance().GetListInUser(userName);
                if (listYnRole != null && listYnRole.Count > 0)
                {
                    foreach (YnRole ynRole in listYnRole)
                    {
                        if (!string.IsNullOrEmpty(userRole))
                            userRole += ",";
                        userRole += ynRole.name;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询用户角色(Search YnRole)", ex);
                throw ex;
            }

            return userRole;
        }

        public string GetUserLogisticsName(string userName, string userRole)
        {
            string logisticsName = string.Empty;

            try
            {
                if (userRole != "领料员" && !string.IsNullOrEmpty(userRole))
                {
                    string sql = "from AscmLogisticsClassInfo";
                    string where = "", whereQueryWord = "";

                    whereQueryWord = "groupLeader ='" + userName + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(where, whereQueryWord);
                    whereQueryWord = "monitorLeader = '" + userName + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(where, whereQueryWord);

                    if (!string.IsNullOrEmpty(where))
                    {
                        sql += " where " + where;
                        IList<AscmLogisticsClassInfo> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLogisticsClassInfo>(sql);
                        if (ilist != null && ilist.Count > 0)
                        {
                            List<AscmLogisticsClassInfo> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmLogisticsClassInfo>(ilist);
                            foreach (AscmLogisticsClassInfo ascmLogisticsClassInfo in list)
                            {
                                if (!string.IsNullOrEmpty(logisticsName))
                                    logisticsName += ",";
                                logisticsName += "'" + ascmLogisticsClassInfo.logisticsClass + "'";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return logisticsName;
        }
        #endregion

        // 查询用户所属班组@2014/4/14
        public string GetUserLogisticsName(string userName)
        {
            string userLogisticsName = "";

            try
            {
                string sql = "select logisticsClass from AscmUserInfo";
                string where = "", whereQueryWord = "";

                if (!string.IsNullOrEmpty(userName))
                {
                    whereQueryWord = "userId = '" + userName.Trim() + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(sql);
                if (obj != null)
                {
                    userLogisticsName = obj.ToString();
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询用户(Search AscmUserInfo LogisticsName)", ex);
                throw ex;
            }

            return userLogisticsName;
        }

        /// 设置用户物流班组@2014/4/14
        public void SetUserLogisticsClass(List<AscmUserInfo> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmUserInfo ascmUserInfo in list)
                {
                    if (!string.IsNullOrEmpty(ids) && !string.IsNullOrEmpty(ascmUserInfo.logisticsClass))
                        ids += ",";
                    if (!string.IsNullOrEmpty(ascmUserInfo.logisticsClass))
                        ids += "'" + ascmUserInfo.logisticsClass + "'";
                }

                string where = "";
                if (!string.IsNullOrEmpty(ids))
                    where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "logisticsClass");

                string sql = "from AscmLogisticsClassInfo";
                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;

                    IList<AscmLogisticsClassInfo> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLogisticsClassInfo>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmLogisticsClassInfo> listLogisticsClass = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmLogisticsClassInfo>(ilist);
                        foreach (AscmUserInfo ascmUserInfo in list)
                        {
                            ascmUserInfo.ascmLogisticsClassInfo = listLogisticsClass.Find(e => e.logisticsClass == ascmUserInfo.logisticsClass);
                        }
                    }
                }
            }
        }
    }
}
