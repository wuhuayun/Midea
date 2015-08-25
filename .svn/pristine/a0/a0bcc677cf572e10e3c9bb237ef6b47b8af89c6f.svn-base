using System;
using System.Collections.Generic;
using MideaAscm.Dal.ContainerManage.Entities;
using YnBaseDal;
using MideaAscm.Dal;
using NHibernate;
using System.Linq;

namespace MideaAscm.Services.ContainerManage
{
    public class AscmTagLogService
    {
        private static AscmTagLogService service;
        public static AscmTagLogService GetInstance()
        {
            if (service == null)
                service = new AscmTagLogService();
            return service;
        }

        /// <summary>
        /// 得到最新的一次流转记录
        /// 根据EPCID得到
        /// </summary>
        /// <param name="strEpcId">根据EPCID</param>
        /// <returns></returns>
        public string Get(string strEpcId)
        {
            try
            {

                using (NHibernate.ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession())
                {

                    if (session.IsOpen)
                    {
                        NHibernate.IQuery query = session.CreateQuery("from AscmTagLog where readTime in (select to_char(max(to_date(readTime,'yyyy-MM-dd HH24:mi:ss')),'yyyy-MM-dd HH24:mi:ss') from AscmTagLog  where epcId='" + strEpcId + "' ) and epcId='" + strEpcId + "' ");
                        AscmTagLog ascmTagLog = query.UniqueResult<AscmTagLog>();
                        if (ascmTagLog != null)
                        {
                            object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select address from AscmReadingHead  where id=" + ascmTagLog.readingHeadId + "");
                            ascmTagLog.place = obj.ToString();
                            return
                                YnBaseClass2.Helper.ObjectHelper.Serialize<AscmTagLog>(ascmTagLog);
                        }
                        else 
                        {
                            return null;
                        }

                    }

                }


            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmContainer)", ex);
                throw ex;
            }
            return null;
        }
        /// <summary>
        /// 流转信息查询
        /// 2013/07/16
        /// </summary>
        /// <param name="ynPage"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="queryWord"></param>
        /// <returns></returns>
        public List<AscmTagLog> GetFlowInfo(YnPage ynPage, string sortName, string sortOrder, string queryWord)
        {
            List<AscmTagLog> list = null;
            try
            {
                string sort = " order by createTime desc";
                //if (!string.IsNullOrEmpty(sortName))
                //{
                //    sort = " order by " + sortName.Trim() + " ";
                //    if (!string.IsNullOrEmpty(sortOrder))
                //        sort += sortOrder.Trim();
                //}
                string strHql = "from AscmTagLog a where a.readTime=(select max(readTime) from AscmTagLog where epcId=a.epcId)";
                // string where = "";
                //where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where,queryWord);
                if (!string.IsNullOrEmpty(queryWord))
                {
                    strHql += " and "+ queryWord;
                }
                IList<AscmTagLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmTagLog>(strHql + sort, strHql, ynPage);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmTagLog>(ilist);
                    SetObjectId(list);//设置物件的ID
                    SetSupplier(list);//设置物件的供应商
                    SetAscmReadingHead(list);  //设置地址
                }

            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmTagLog)", ex);
                throw ex;
            }
            return list;
        }

     
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="queryWord"></param>
        /// <returns></returns>
        public List<AscmTagLog> GetFlowInfo(string queryWord)
        {
            List<AscmTagLog> list = null;
            try
            {
                string sort = " order by createTime desc";
                string strHql = "from AscmTagLog";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    strHql += " where " + queryWord;
                }
                IList<AscmTagLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmTagLog>(strHql + sort);
                IList<MideaAscm.Dal.Base.Entities.AscmReadingHead> ilistAdress = YnDaoHelper.GetInstance().nHibernateHelper.Find<MideaAscm.Dal.Base.Entities.AscmReadingHead>("from AscmReadingHead where ip!='0.0.0.0'");
                List<MideaAscm.Dal.Base.Entities.AscmReadingHead> listAdress = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<MideaAscm.Dal.Base.Entities.AscmReadingHead>(ilistAdress);
                string flowString = "";
                AscmTagLog astddd=  new AscmTagLog();
                int count = 0;
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmTagLog>(ilist);
                    var q = (from a in ilist select a.epcId).Distinct();
                    foreach (var t in q)
                    {
                        var find= list.FindAll(e => e.epcId == t);
                        if (find.Count > 1)
                        {
                            foreach (AscmTagLog AscmTagLog in find)
                            {
                                count++;
                                var result =listAdress.Find(e => e.id == AscmTagLog.readingHeadId);
                                if (count != find.Count)
                                {
                                    if (result == null)
                                    {
                                        flowString += AscmTagLog.createTime + " " + Environment.NewLine;
                                       
                                    }
                                    else 
                                    {
                                        flowString += AscmTagLog.createTime +" "+ result.address + Environment.NewLine;
                                    }
                                    list.Remove(AscmTagLog);
                                }
                                else 
                                {
                                    if (result == null)
                                    {
                                        flowString += AscmTagLog.createTime + " " ;

                                    }
                                    else
                                    {
                                        flowString += AscmTagLog.createTime + " " + result.address ;
                                    }                                   
                                    AscmTagLog.place = flowString;                                  
                                }
                            }
                        }
                        else 
                        {
                            var result = listAdress.Find(e => e.id == find[0].readingHeadId);
                            if(result==null)
                            {
                            flowString = find[0].createTime;
                            }
                            else
                            {
                                flowString = find[0].createTime + "  " + result.address;
                            }

                            find[0].place = flowString;
                        }
                        flowString = "";
                    }
                }
                if (ilist != null && ilist.Count > 0)
                {
                  
                    SetObjectId(list);//设置物件的ID
                    SetSupplier(list);//设置物件的供应商
                   // SetAscmReadingHead(list);  //设置地址
                }

            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmTagLog)", ex);
                throw ex;
            }
            return list;
        }

        /// <summary>
        /// 流转信息查询
        /// 2013/10/9
        /// </summary>
        /// <param name="ynPage"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="queryWord"></param>
        /// <returns></returns>
        public List<AscmTagLog> GetFlowDetailInfo(string whereStr)
        {

            List<AscmTagLog> list = null;

            try
            {
                string sort = " order by createTime desc";
                //if (!string.IsNullOrEmpty(sortName))
                //{
                //    sort = " order by " + sortName.Trim() + " ";
                //    if (!string.IsNullOrEmpty(sortOrder))
                //        sort += sortOrder.Trim();
                //}
                string strHql = "from AscmTagLog";
                // string where = "";
                //where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where,queryWord);
                if (!string.IsNullOrEmpty(whereStr))
                {
                    strHql += " a where " + whereStr;
                }
                IList<AscmTagLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmTagLog>(strHql+sort);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmTagLog>(ilist);
                    SetObjectId(list);//设置物件的ID
                    SetSupplier(list);//设置物件的供应商
                    SetAscmReadingHead(list);  //设置地址
                }

            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmTagLog)", ex);
                throw ex;
            }
            return list;
        }
        /// <summary>
        /// 覃小华于2013/07/16添加
        /// </summary>
        /// <param name="list"></param>
        private void SetObjectId(List<AscmTagLog> list)
        {
            foreach (AscmTagLog ascmTagLog in list)
            {
                object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select id  from AscmRfid where epcId='" + ascmTagLog.epcId + "'");
                if (obj != null)
                {
                    ascmTagLog.objectID = obj.ToString();
                }
            }
        }
        /// <summary>
        /// 覃小华于2013/07/16添加
        /// </summary>
        /// <param name="list"></param>
        private void SetSupplier(List<AscmTagLog> list)
        {
            foreach (AscmTagLog ascmTagLog in list)
            {
                if (ascmTagLog.bindType == AscmTagLog.BindTypeDefine.container)
                {
                    object suplierID = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select supplierId from AscmContainer where sn='" + ascmTagLog.objectID + "'");
                    if (suplierID != null)
                    {

                        ascmTagLog.supplierName = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select name from AscmSupplier where id=" + suplierID + "").ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 设置地址
        /// 2013/08/22
        /// </summary>
        /// <param name="list"></param>
        private void SetAscmReadingHead(List<AscmTagLog> list)
        {

            System.Text.StringBuilder sbSn = new System.Text.StringBuilder();
            sbSn.Clear();
            var t = list.Select(e => e.readingHeadId).Distinct();
            foreach (int ascmTagLog in t)
            {
                if (sbSn.Length != 0)
                {
                    sbSn.Append(",");
                }
                sbSn.AppendFormat("{0}", ascmTagLog);
            }
            IList<MideaAscm.Dal.Base.Entities.AscmReadingHead> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<MideaAscm.Dal.Base.Entities.AscmReadingHead>("from AscmReadingHead where id in (" + sbSn.ToString() + ")");
            List<MideaAscm.Dal.Base.Entities.AscmReadingHead> listAscmReadingHead = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList(ilist);
            if (ilist.Count > 0)
            {
                foreach (AscmTagLog ascmTagLog in list)
                {
                    ascmTagLog.ascmReadingHead = listAscmReadingHead.Find(e => e.id == ascmTagLog.readingHeadId);
                }

            }
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="Epc">EPC</param>
        /// <param name="ReadHeadId">读头ID</param>
        public void AscmTagLogSave(string Epc, int ReadHeadId, string sessionKey = null)
        {

            try
            {

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession(sessionKey).BeginTransaction())
                {
                    try
                    {
                        //有没有该标签的信息
                        object objType = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select bindType from AscmRfid where  epcId='" + Epc + "'", sessionKey);
                        if (objType != null)
                        {
                            object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select id from AscmTagLog where readingHeadId=" + ReadHeadId + " and epcId='" + Epc + "' and readTime=(select max(readTime) from AscmTagLog where epcId='" + Epc + "')", sessionKey);
                            //说明有，要新增
                            if (obj != null)
                            {
                                AscmTagLog ascmTagLog = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmTagLog>(Convert.ToInt32(obj.ToString()), sessionKey);
                                ascmTagLog.createTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                ascmTagLog.readTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmTagLog>(ascmTagLog, sessionKey);

                            }
                            else
                            {

                                //object objId=YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select max(id) from AscmTagLog", sessionKey);
                                //int id=0;
                                //if (objId != null)
                                //{
                                //   id = (int)objId;                                                                     
                                //}
                                AscmTagLog ascmTagLog = new AscmTagLog();
                                // ascmTagLog.id = ++id;
                                ascmTagLog.readingHeadId = ReadHeadId;
                                ascmTagLog.createTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                ascmTagLog.readTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                ascmTagLog.epcId = Epc;
                                ascmTagLog.bindType = objType.ToString();
                                YnDaoHelper.GetInstance().nHibernateHelper.Save<AscmTagLog>(ascmTagLog, sessionKey);
                                //  tx.Commit();//正确执行提交
                                if (AscmTagLog.BindTypeDefine.container == objType.ToString() && MideaAscm.Services.ContainerManage.AscmContainerInfoService.GetInstance().GetByEpcId(Epc, sessionKey) != null)
                                {
                                    MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer ascmContainer = MideaAscm.Services.ContainerManage.AscmContainerInfoService.GetInstance().GetByEpcId(Epc, sessionKey);
                                    ascmContainer.place = ReadHeadId.ToString();
                                    YnDaoHelper.GetInstance().nHibernateHelper.Update<MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer>(ascmContainer, sessionKey);
                                    // MideaAscm.Services.ContainerManage.AscmContainerInfoService.GetInstance().Update(ascmContainer, sessionKey);
                                }
                                tx.Commit();//正确执行提交
                            }

                        }
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save ascmStoreInOut)", ex);
                throw ex;
            }

        }
        public void DeleteRecode(string docNember,bool IsDoc)
        {
            
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        if (!IsDoc)
                        {
                            AscmStoreInOut ascmStoreInOut = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmStoreInOut>(Convert.ToInt32(docNember));
                            YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmStoreInOut>(ascmStoreInOut);
                        }
                        else 
                        {
                            IList<AscmStoreInOut> ilistascmStoreInOut = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmStoreInOut>("from AscmStoreInOut where docNumber='"+docNember+"'");
                            if (ilistascmStoreInOut != null && ilistascmStoreInOut.Count > 0)
                            {
                                foreach (AscmStoreInOut st in ilistascmStoreInOut)
                                {
                                    
                                    YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmStoreInOut>(st);
                                }
                            }
                        }
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete ascmStoreInOut)", ex);
                        throw ex;
                    }
                }
          
        }

    }
}
