using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using System.Data;
using Oracle.DataAccess.Client;
using MideaAscm.Dal.ContainerManage.Entities;
using MideaAscm.Dal.SupplierPreparation.Entities;

namespace MideaAscm.Services.ContainerManage
{
    public class AscmContaierCheckService
    {
        private static AscmContaierCheckService ascmContaierCheckService;
        public static AscmContaierCheckService GetInstance()
        {
            if (ascmContaierCheckService == null)
                ascmContaierCheckService = new AscmContaierCheckService();
            return ascmContaierCheckService;
        }

        public Dal.ContainerManage.Entities.AscmCheck Get()
        {
            Dal.ContainerManage.Entities.AscmCheck ack = null;
            try
            {

                NHibernate.IQuery query = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().CreateQuery("from AscmCheck where id in (select max(id) from AscmCheck)");
                ack = query.UniqueResult<Dal.ContainerManage.Entities.AscmCheck>();

            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmCheck)", ex);
                throw ex;
            }
            return ack;
        }

        /// <summary>
        /// 得到各个供应商的容器总数量
        /// </summary>
        /// <param name="ynPage"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="queryWord"></param>
        /// <param name="whereOther"></param>
        /// <returns></returns>
        public List<AscmSupplier> Getlist(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmSupplier> list = null;

            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmSupplier where id in (select distinct supplierId from AscmContainer where status!=1 and (place!=(select to_char(id) from  AscmReadingHead where address= '厂区外' and ip='0.0.0.0') or place is null))";
                // string sql = "from AscmSupplier sp  where id in (select supplierId from AscmContainer)";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                    whereQueryWord = " (name like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                //where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += "and" + where;
                }
                //IList<AscmWipEntities> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql + sort);
                IList<AscmSupplier> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(ilist);
                    setContainerAomount(list);
                    SetExceptionCount(list);

                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmSupplier)", ex);
                throw ex;
            }
            return list;
        }

        /// <summary>
        /// 把数量加入到供应商实体
        /// 2013/07/18修改
        /// </summary>
        /// <param name="list"></param>
        private void setContainerAomount(List<AscmSupplier> list)
        {
            string sql = "select supplierId, count(sn) from AscmContainer where status not in (0,1) and (place!=(select to_char(id) from  AscmReadingHead where address= '厂区外' and ip='0.0.0.0') or place is null) group by supplierId ";
            IList<object[]> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<object[]>(sql);
            if (ilist != null)
            {
                List<object[]> listAscmMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<object[]>(ilist);
                foreach (AscmSupplier ascmSupplier in list)
                {
                    if (listAscmMaterialItem.Find(e => e[0].ToString() == ascmSupplier.id.ToString())!= null)
                    {
                        ascmSupplier.containerAmount = Convert.ToInt32(listAscmMaterialItem.Find(e => e[0].ToString() == ascmSupplier.id.ToString())[1].ToString());
                    }                   
                }
            }
        }

        /// <summary>
        /// 把异常数量加入到供应商实体
        /// 2013年8月5日11:48:18新增
        /// 作者：覃小华
        /// </summary>
        /// <param name="list"></param>
        private void SetExceptionCount(List<AscmSupplier> list)
        {
            string sql = "select supplierId, count(sn) from AscmContainer where status=0 and (place!=(select to_char(id) from  AscmReadingHead where address= '厂区外' and ip='0.0.0.0') or place is null) group by supplierId ";
            IList<object[]> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<object[]>(sql);
            if (ilist != null)
            {
                List<object[]> listAscmMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<object[]>(ilist);
                foreach (AscmSupplier ascmSupplier in list)
                {
                    if (listAscmMaterialItem.Find(e => e[0].ToString() == ascmSupplier.id.ToString()) != null)
                    {
                        ascmSupplier.ExceptionCount = Convert.ToInt32(listAscmMaterialItem.Find(e => e[0].ToString() == ascmSupplier.id.ToString())[1].ToString());
                    }
                }
            }
        }
        public void SaveCheck(Dal.ContainerManage.Entities.AscmCheck ack)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save<Dal.ContainerManage.Entities.AscmCheck>(ack);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmCheck)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmCheck)", ex);
                throw ex;
            }

        }

        /// <summary>
        /// 批量提交盘点容器信息
        /// </summary>
        /// <param name="list"></param>
        public void SaveCheckInfo(List<Dal.ContainerManage.Entities.AscmCheckContainerInfo> list)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(list);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmCheckContainerInfo)", ex);
                        throw ex;
                    }

                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmCheckContainerInfo)", ex);
                throw ex;
            }

        }

        public void UpdateCheck(Dal.ContainerManage.Entities.AscmCheck ack)
        {

            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<Dal.ContainerManage.Entities.AscmCheck>(ack);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Updae AscmCheck)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Update AscmCheck)", ex);
                throw ex;
            }
        }
        public void ResetCheckInfo()
        {
            ExecuteOraDMLSQL("update ascm_container set ischeck=0 where ischeck=1 and status<>1 and place not in (select to_char(id) from  ascm_reading_head where address= '厂区外' and ip='0.0.0.0')");
          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        public void ExecuteOraDMLSQL(string commandText)
        {
            ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
            using (ITransaction tx = session.BeginTransaction())
            {
                //OracleCommand command = (OracleCommand)session.Connection.CreateCommand();
                IDbCommand command = session.Connection.CreateCommand();
                try
                {

                    tx.Enlist(command);
                    command.CommandType = CommandType.Text;
                    command.CommandText = commandText;
                    command.Prepare();
                    command.ExecuteNonQuery();
                    //command.Dispose();
                    tx.Commit();//正确执行提交

                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("执行存储过程失败", ex);
                    throw ex;
                }
                finally
                {
                    command.Dispose();
                }
            }
        }

        /// <summary>
        /// 具体容器规格的数量列表
        /// </summary>
        /// <returns></returns>
        public List<Dal.SupplierPreparation.Entities.AscmContainer> GetListBySpec(string supplierId)
        {
            string strHql = "select count(sn), (select spec from AscmContainerSpec where id=specId),specId from AscmContainer where supplierId=" + supplierId + " and status!=1 and  (place!=(select to_char(id) from  AscmReadingHead where address= '厂区外' and ip='0.0.0.0') or place is null)  group by specId ";
            IList<object[]> list=YnDaoHelper.GetInstance().nHibernateHelper.Find<object[]>(strHql);
            List<Dal.SupplierPreparation.Entities.AscmContainer> rlist = new List<Dal.SupplierPreparation.Entities.AscmContainer>();
            foreach (object[] obj in list)
            {
                rlist.Add(new Dal.SupplierPreparation.Entities.AscmContainer() {SpecCount=obj[0].ToString(),SpecName=obj[1].ToString(),specId=(int)obj[2],supplierId=Convert.ToInt32(supplierId) });
            }
            return rlist;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns> YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther
        public List<Dal.SupplierPreparation.Entities.AscmContainer> GetListBy2Id(YnPage ynPage, string sortName, string sortOrder, string supplierId, string whereOther)
        {
            string strHql = "from AscmContainer where supplierId=" + supplierId + " and status!=1 and (place!=( select to_char(id) from  AscmReadingHead where address= '厂区外' and ip='0.0.0.0') or place is null)";
            string strOrder = "order by status";
            if (!string.IsNullOrEmpty(whereOther))
                strHql += "and " + whereOther;
            IList<Dal.SupplierPreparation.Entities.AscmContainer> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<Dal.SupplierPreparation.Entities.AscmContainer>(strHql+strOrder,strHql,ynPage);
            List<Dal.SupplierPreparation.Entities.AscmContainer> rlist=null;
            if (ilist != null)
            {
               rlist = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<Dal.SupplierPreparation.Entities.AscmContainer>(ilist);
               SetSupplier(rlist);
               SetSpec(rlist);
               SetAscmReadingHead(rlist);  //设置地址
            }
           
            return rlist;      
        }

        public List<Dal.Base.Entities.AscmReadingHead> GetPlaceListBy2Id(string supplierId)
        {
            string strHql = "from AscmReadingHead where id in (select DISTINCT place from AscmContainer where supplierId=" + supplierId + " and status!=1 and  (place!=(select to_char(id) from  AscmReadingHead where address= '厂区外' and ip='0.0.0.0') or place is null))";

            string strOrder = " order by id";
            IList<Dal.Base.Entities.AscmReadingHead> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<Dal.Base.Entities.AscmReadingHead>(strHql+strOrder);
            List<Dal.Base.Entities.AscmReadingHead> rlist = null;
            if (ilist != null)
            {
                rlist = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<Dal.Base.Entities.AscmReadingHead>(ilist);
            }

            return rlist;      
 
        }

        private void SetSupplier(List<Dal.SupplierPreparation.Entities.AscmContainer> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmContainer ascmContainer in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmContainer.supplierId + "";
                }
                string sql = "from AscmSupplier where id in (" + ids + ")";
                IList<AscmSupplier> ilistAscmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql);
                if (ilistAscmSupplier != null && ilistAscmSupplier.Count > 0)
                {
                    List<AscmSupplier> listAscmSupplier = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(ilistAscmSupplier);
                    foreach (AscmContainer ascmContainer in list)
                    {
                        ascmContainer.supplier = listAscmSupplier.Find(e => e.id == ascmContainer.supplierId);
                    }
                }
            }
        }

        private void SetSpec(List<AscmContainer> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmContainer ascmContainer in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmContainer.specId + "";
                }
                string sql = "from AscmContainerSpec where id in (" + ids + ")";
                IList<AscmContainerSpec> ilistAscmContainerSpec = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainerSpec>(sql);
                if (ilistAscmContainerSpec != null && ilistAscmContainerSpec.Count > 0)
                {
                    List<AscmContainerSpec> listAscmContainerSpec = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainerSpec>(ilistAscmContainerSpec);
                    foreach (AscmContainer ascmContainer in list)
                    {
                        ascmContainer.containerSpec = listAscmContainerSpec.Find(e => e.id == ascmContainer.specId);
                    }
                }
            }
        }

        /// <summary>
        /// 设置地址
        /// 2013/08/22
        /// </summary>
        /// <param name="list"></param>
        private void SetAscmReadingHead(List<Dal.SupplierPreparation.Entities.AscmContainer> list)
        {

            System.Text.StringBuilder sbSn = new System.Text.StringBuilder();
            sbSn.Clear();
            var t = list.Select(e => e.place).Distinct();
            foreach (string ascmTagLog in t)
            {
                if(!string.IsNullOrEmpty(ascmTagLog))
                {
                if (sbSn.Length != 0)
                {
                    sbSn.Append(",");
                }
                sbSn.AppendFormat("{0}", ascmTagLog);
                }
            }
            if (string.IsNullOrEmpty(sbSn.ToString()))
            {
                return;
            }
            IList<MideaAscm.Dal.Base.Entities.AscmReadingHead> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<MideaAscm.Dal.Base.Entities.AscmReadingHead>("from AscmReadingHead where id in (" + sbSn.ToString() + ")");
            List<MideaAscm.Dal.Base.Entities.AscmReadingHead> listAscmReadingHead = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList(ilist);
            if (ilist.Count > 0)
            {
                foreach (Dal.SupplierPreparation.Entities.AscmContainer ascmTagLog in list)
                {
                    ascmTagLog.ascmReadingHead = listAscmReadingHead.Find(e => e.id.ToString() == ascmTagLog.place);
                }

            }
        }
        /// <summary>
        /// 手持端盘点功能
        /// </summary>
        /// <param name="strSn"></param>
        public  string  ContainerCheck(string strSn, string status, string userName)
        {
            //如果没有盘点任务开启
            if (YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmCheck as ck where ck.count is null") == 0)
            {
                return "没有开启的盘点任务";
            }
            Dal.SupplierPreparation.Entities.AscmContainer ascmContainer = MideaAscm.Services.SupplierPreparation.AscmContainerService.GetInstance().Get(strSn);
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    if (!string.IsNullOrEmpty(status))
                    {
                        //如果状态是丢失
                        if (status == "LOST")
                        {
                            ascmContainer.status = Dal.SupplierPreparation.Entities.AscmContainer.StatusDefine.losted;
                        }
                        else
                        {
                            ascmContainer.status = Dal.SupplierPreparation.Entities.AscmContainer.StatusDefine.unuse;
                        }
                        //盘点ID
                        int intcheckId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmCheck)");
                        //如果没有重复就新建
                        if (YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmCheckContainerInfo where containerId='" + strSn + "' and checkId=" + intcheckId + "") == 0)
                        {
                            Dal.ContainerManage.Entities.AscmCheckContainerInfo CheckInfo = new Dal.ContainerManage.Entities.AscmCheckContainerInfo();
                            CheckInfo.id = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmCheckContainerInfo)") + 1;
                            CheckInfo.checkId = intcheckId;
                            CheckInfo.containerId = ascmContainer.sn;
                            CheckInfo.status = status;
                            CheckInfo.supplierId = ascmContainer.supplierId;
                            CheckInfo.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            CheckInfo.createUser = userName;
                            YnDaoHelper.GetInstance().nHibernateHelper.Save<Dal.ContainerManage.Entities.AscmCheckContainerInfo>(CheckInfo);
                        }
                        else    //重复就更新
                        {
                            Dal.ContainerManage.Entities.AscmCheckContainerInfo ascmCheckContainerInfo = YnDaoHelper.GetInstance().nHibernateHelper.Get<Dal.ContainerManage.Entities.AscmCheckContainerInfo>(Convert.ToInt32(YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select id from AscmCheckContainerInfo where containerId='" + strSn + "' and checkId=" + intcheckId + "")));
                            ascmCheckContainerInfo.status = status;
                            YnDaoHelper.GetInstance().nHibernateHelper.Update<Dal.ContainerManage.Entities.AscmCheckContainerInfo>(ascmCheckContainerInfo);
                            tx.Commit();//正确执行提交
                            return "系统已更新该容器的盘点信息";
                        }
                    }
                    ascmContainer.isCheck = 1;
                    YnDaoHelper.GetInstance().nHibernateHelper.Update<Dal.SupplierPreparation.Entities.AscmContainer>(ascmContainer);
                    tx.Commit();//正确执行提交
                    return "";
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmCheckContainerInfo)", ex);
                    throw ex;
                }
            }
        }











        }
    
}
