using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;

namespace MideaAscm.Services.Warehouse
{
    public class AscmWhTeamUserService
    {
        private static AscmWhTeamUserService _Instance = new AscmWhTeamUserService();
        public static AscmWhTeamUserService Instance
        {
            get 
            {
                return _Instance;
            }      
        }

        public AscmWhTeamUser Get(int teamId, string userId)
        {
            AscmWhTeamUser AscmTeamUser = null;
            try
            {
                AscmTeamUser = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWhTeamUser>(new AscmWhTeamUserPK { teamId = teamId, userId = userId });
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error(" 查询失败 (Get AscmTeamUser) ", ex);
                throw ex;
            }
            return AscmTeamUser;
        }

        public List<AscmWhTeamUser> GetList(string strWhere = "")
        {
            List<AscmWhTeamUser> list = null;
            try
            {
                string sql = " from AscmWhTeamUser ";
                if (!string.IsNullOrEmpty(strWhere)) sql += " where 1=1 " + strWhere;

                IList<AscmWhTeamUser> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWhTeamUser>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWhTeamUser>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWhTeamUser)", ex);
                throw ex;
            }

            return list;
        }

        public List<AscmWhTeamUser> GetList(int teamId) 
        {
            string strWhere = " AND teamId=" + teamId;

            return GetList(strWhere);
        }

        public List<AscmWhTeamUser> GetListForTeam(string userId)
        {
            var allList = GetList();
            if (allList == null || allList.Count == 0) 
            {
                return allList;
            }

            var teamUser = allList.FirstOrDefault(a => a.M_UserId == userId);
            int teamId = teamUser != null ? teamUser.M_TeamId : 0;
            var list = allList.Where(a => a.M_TeamId == teamId).ToList();

            return list;
        }

        public string GetLeaderId(string userId)
        {
            string strWhere = " AND userId='" + userId + "' ";
            var list =  GetList(strWhere);
            if (list == null || list.Count == 0) 
            {
                return null;
            }

            var teamUserList = GetList(list[0].M_TeamId);
            if (teamUserList == null || teamUserList.Count == 0) 
            {
                return null;
            }

            var teamLeader = teamUserList.FirstOrDefault(a => a.isLeader == true);
            if (teamLeader == null) 
            {
                return null;
            }

            return teamLeader.M_UserId;
        }

        public bool ExistUserId(List<string> userIdList)
        {
            if (userIdList == null || userIdList.Count == 0)
            {
                return false;
            }

            List<AscmWhTeamUser> allList = GetList();
            if (allList == null || allList.Count == 0)
            {
                return false;
            }

            var list = allList.Where(a => !string.IsNullOrEmpty(a.M_UserId) && userIdList.Contains(a.M_UserId)).ToList();
            if (list != null && list.Count > 0) 
            {
                return true;
            }

            return false;
        }

        public List<string> GetLeaderIds(List<string> userIdList)
        {
            if (userIdList == null || userIdList.Count == 0) 
            {
                return new List<string>();
            }

            List<AscmWhTeamUser> allList = GetList("");
            if (allList == null || allList.Count == 0) 
            {
                return new List<string>();
            }

            Dictionary<int, string> dict = new Dictionary<int, string>();
            foreach (var item in allList)
            {
                if (userIdList.Contains(item.M_UserId)) 
                {
                    dict[item.M_TeamId] = "";
                }
            }

            if (dict.Count == 0) 
            {
                return new List<string>();
            }
            
            var teamIds = new List<int>(dict.Keys);
            var leaderIdlist = allList.Where(a => a.isLeader == true && teamIds.Contains(a.M_TeamId)).Select(a => a.M_UserId).ToList();
            if (leaderIdlist == null || leaderIdlist.Count == 0) 
            {
                return new List<string>(); 
            }

            return leaderIdlist;
        }

        public string GetLeaderIdsForWhere(List<string> userIdList) 
        {
            List<string> list = GetLeaderIds(userIdList);
            if (list == null && list.Count == 0) 
            {
                return "";
            }

            StringBuilder str = new StringBuilder();
            foreach (var item in userIdList)
            {
                str.Append("'" + item + "',");
            }
            str.Remove(str.Length - 1, 1);

            return str.ToString();
        }

        public void Save(AscmWhTeamUser ascmWhTeamUser)
        {
            try
            {
                int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmWhTeamUser where teamId=" + ascmWhTeamUser.pk.teamId + " and userId='" + ascmWhTeamUser.pk.userId + "'");
                if (count == 0)
                {
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWhTeamUser);
                            tx.Commit();        //正确执行提交
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();      //回滚
                            throw ex;
                        }
                    }
                }
                else
                {
                    //throw new Exception("已经存在组别与用户的关联！");
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWhTeamUser)", ex);
                throw ex;
            }
        }

        public void Update(AscmWhTeamUser ascmWhTeamUser)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWhTeamUser>(ascmWhTeamUser);
                    tx.Commit();    //正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();  //回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWhTeamUser)", ex);
                    throw ex;
                }
            }
        }

        public void Delete(int teamId, string userId)
        {
            try
            {
                AscmWhTeamUser ascmWhTeamUser = Get(teamId, userId);
                Delete(ascmWhTeamUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(AscmWhTeamUser ascmWhTeamUser)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWhTeamUser>(ascmWhTeamUser);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWhTeamUser)", ex);
                throw ex;
            }
        }

        public void Delete(List<AscmWhTeamUser> listAscmWhTeamUser)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWhTeamUser);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWhTeamUser List)", ex);
                throw ex;
            }
        }
    }
}
