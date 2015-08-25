using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.SupplierPreparation.Entities;

namespace MideaAscm.Services.ContainerManage
{
    public class AscmContainerInfoService
    {
        private static AscmContainerInfoService ascmContainerInfoService;
        public static AscmContainerInfoService GetInstance()
        {
            //return ascmRfidServices ?? new AscmRfidService();
            if (ascmContainerInfoService == null)
                ascmContainerInfoService = new AscmContainerInfoService();
            return ascmContainerInfoService;
        }

        public AscmContainer Get(string sn)
        {
            AscmContainer ascmContainer = null;
            try
            {
                ascmContainer = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmContainer>(sn);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmContainer)", ex);
                throw ex;
            }
            return ascmContainer;
        }

        public List<AscmContainer> GetList(string sql, bool isSetSupplier = false, bool isSetSpec = false)
        {
            List<AscmContainer> list = null;
            try
            {
                IList<AscmContainer> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainer>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainer>(ilist);
                    if (isSetSupplier)
                        SetSupplier(list);
                    if (isSetSpec)
                        SetSpec(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmContainer)", ex);
                throw ex;
            }
            return list;
        }


        /// <summary>
        /// 通过EPCID查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="isSetSupplier"></param>
        /// <param name="isSetSpec"></param>
        /// <returns></returns>
        public AscmContainer GetByEpcId(string epcId, string sessionKey = null)
        {
            AscmContainer ascmContainer = null;
            try
            {
                NHibernate.ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                if (sessionKey != null)
                    session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession(sessionKey);
                NHibernate.IQuery query = session.CreateQuery("from AscmContainer where rfid='"+epcId+"' ");
                if (query.List().Count > 0)
                {
                    ascmContainer = query.UniqueResult<AscmContainer>();
                    if (!string.IsNullOrEmpty(ascmContainer.supplierId.ToString()))
                    {
                        ascmContainer.supplier = MideaAscm.Services.Base.AscmSupplierService.GetInstance().Get(ascmContainer.supplierId, sessionKey);
                    }
                    if (!string.IsNullOrEmpty(ascmContainer.specId.ToString()))
                    {
                        ascmContainer.containerSpec= MideaAscm.Services.SupplierPreparation.AscmContainerSpecService.GetInstance().Get(ascmContainer.specId, sessionKey);
                    }
                }
              
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmContainer)", ex);
                throw ex;
            }
            return ascmContainer;
        }

        /// <summary>
        /// 导出Excell用
        /// </summary>
        /// <param name="queryWord"></param>
        /// <param name="whereOther"></param>
        /// <returns></returns>
        public List<AscmContainer> GetList(string queryWord, string whereOther)
        {
            List<AscmContainer> list = null;
            try
            {
                string sql = "from AscmContainer ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " (sn like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmContainer> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainer>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainer>(ilist);
                    SetSupplier(list);
                    SetSpec(list);
                    SetAscmReadingHead(list);//设置地址
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmContainer)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmContainer> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmContainer> list = null;
            try
            {
                string sort = " order by sn ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmContainer ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " (sn like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmContainer> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainer>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainer>(ilist);
                    SetSupplier(list);
                    SetSpec(list);
                    SetAscmReadingHead(list);//设置地址
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmContainer)", ex);
                throw ex;
            }
            return list;
        }

        /// <summary>
        /// 得到过滤过的数据
        /// </summary>
        /// <param name="ynPage"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="queryWord"></param>
        /// <param name="whereOther"></param>
        /// <returns></returns>
        public List<AscmContainer> GetFilterList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther,string strfilter)
        {
            List<AscmContainer> list = null;
            try
            {
                string sort = " order by sn ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmContainer ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " (sn like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, strfilter);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where + "";
                IList<AscmContainer> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainer>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainer>(ilist);
                    SetSupplier(list);
                    SetSpec(list);
                    SetAscmReadingHead(list);  //设置地址
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmContainer)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmContainer> listAscmContainer)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmContainer);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmContainer)", ex);
                throw ex;
            }
        }

        public void Save(AscmContainer ascmContainer)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmContainer);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmContainer)", ex);
                throw ex;
            }
        }

        public void Update(AscmContainer ascmContainer)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmContainer>(ascmContainer);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmContainer)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmContainer)", ex);
                throw ex;
            }
        }

        public void Delete(string sn)
        {
            try
            {
                AscmContainer ascmContainer = Get(sn);
                Delete(ascmContainer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(AscmContainer ascmContainer)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmContainer>(ascmContainer);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmContainer)", ex);
                throw ex;
            }
        }

        public void Delete(List<AscmContainer> listAscmContainer)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmContainer);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmContainer)", ex);
                throw ex;
            }
        }



        private void SetSupplier(List<AscmContainer> list)
        {
            if (list != null && list.Count > 0)
            {
                var q = (from a in list select a.supplierId).Distinct().ToList();
                string ids = string.Empty;
                foreach (var  ascmContainer in q)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmContainer.ToString() + "";
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
                var Q = (from a in list select a.specId).Distinct().ToList();
                string ids = string.Empty;
                foreach (var  ascmContainer in Q)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmContainer.ToString() + "";
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
        public AscmContainer GetAllInfo(string sn)
        {
            AscmContainer ascmContainer = null;
            try
            {
                ascmContainer = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmContainer>(sn);
                if (ascmContainer != null)
                {
                    ascmContainer.SpecName = MideaAscm.Services.SupplierPreparation.AscmContainerSpecService.GetInstance().Get(ascmContainer.specId).spec;
                    ascmContainer.supplier = MideaAscm.Services.Base.AscmSupplierService.GetInstance().Get(ascmContainer.supplierId);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmContainer)", ex);
                throw ex;
            }
            return ascmContainer;
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
                if (!string.IsNullOrEmpty(ascmTagLog))
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

        public List<MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer> ExportExcell(int days, string whereOthers)
        {
            string sql = "from AscmContainer where status!=0 and status!=1";
            if (!string.IsNullOrEmpty(whereOthers))
            {
                sql += " and" + whereOthers;
            }
            string subsql = " and rfid not in (select a.epcId from AscmTagLog a where a.id=(select max(id) from AscmTagLog where epcId=a.epcId) and a.readTime>='" + System.DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd HH:mm:ss") + "')";
            sql += subsql;
            string sqlII = "from AscmContainer where status!=0 and status!=1 and supplierId!=6128 and sn in (select containerId  from AscmStoreInOut a where a.direction='" + MideaAscm.Dal.ContainerManage.Entities.AscmStoreInOut.DirectionDefine.storeOut + "' and a.createTime=(select max(createTime)  from AscmStoreInOut  where containerId=a.containerId))";
            IList<AscmContainer> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainer>(sql);
            IList<AscmContainer> iListII = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainer>(sqlII);           
            List<AscmContainer> list = null;
            if (ilist != null)
            {
                list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainer>(ilist);
                if (iListII != null && iListII.Count > 0)
                {
                    foreach (AscmContainer ascon in iListII)
                    {
                        list.Remove(ascon);
                    }
                }
                SetSupplier(list);
                SetSpec(list);
                SetAscmReadingHead(list);  //设置地址
            }
            return list;
        }

        /// <summary>
        /// 容器
        /// </summary>
        /// <returns></returns>
        public List<MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer> DispayData()
        {
            string sql = "select count(sn), specId, supplierId from AscmContainer  where status!=0 and status!=1 and place='5' group by specId, supplierId   order by supplierId";
            IList<object[]> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<object[]>(sql);
         //   IList<AscmContainer> iListII = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainer>(sqlII);
            List<AscmContainer> list = new List<AscmContainer> ();
            if (ilist != null&&ilist.Count>0)
            {
                foreach (object[] obj in ilist)
                {
                    list.Add(new AscmContainer() {SpecCount=obj[0].ToString(),specId=(int)obj[1],supplierId=(int)obj[2] });
                }
                SetSupplier(list);
                SetSpec(list);
                setSiteCode(list);
            }
            return list;
        }

        /// <summary>
        /// 容器流转显示
        /// </summary>
        /// <returns></returns>
        public List<MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer> DispayFlowData()
        {
            string sql = "select distinct(SUBSTR(address,0,1)) from AscmReadingHead where ip!='0.0.0.0'";
            IList<object> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<object>(sql);
             IList<object[]> ilistCount=null;
             List<AscmContainer> list = new List<AscmContainer>();
            foreach(object obj_Place in ilist)
            {
                string sqlI = "select count(sn), specId, supplierId, '"+obj_Place+"#厂房' from AscmContainer  group by specId, supplierId where status!=0 and status!=1 and place=(select to_char(id) from AscmReadingHead where SUBSTR(address,0,1)='" + obj_Place + "')  order by supplierId";
               ilistCount= YnDaoHelper.GetInstance().nHibernateHelper.Find<object[]>(sqlI);
               if (ilistCount.Count > 0 && ilistCount != null)
               {
                   foreach (object[] obj in ilistCount)
                   {
                       list.Add(new AscmContainer() { SpecCount = obj[0].ToString(), specId = (int)obj[1], supplierId = (int)obj[2], place = obj[3].ToString() });
                   }     
                   //list.AddRange(ilistCount);
               }               
            }
            if (list != null && list.Count > 0)
            {                        
                SetSupplier(list);
                SetSpec(list);
                setSiteCode(list);
            }
            return list;
        }
        /// <summary>
        /// LED超期预警
        /// </summary>
        /// <returns></returns>
        public List<MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer> DisplayWarInf()
        {
              List<MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer> list=   AscmCheckWarnService.GetInstance().GetlistForExcellExport("", "");
              var result = from l in list
                           where l.extendedTime > 0 || l.extendedTime ==0
                           group l by new { l.supplierId, l.specId,l._supplierName } into g
                           select new
                          {
                              supplierId = g.Key.supplierId,
                              SpecCount = g.Count(),
                              specId = g.Key.specId,
                              supplierName=g.Key._supplierName
                          };
              List<MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer> Rlist = new List<AscmContainer>();
              foreach (var t in result)
              {
                  Rlist.Add(new AscmContainer { supplierId = t.supplierId, SpecCount = t.SpecCount.ToString(), specId = t.specId, _supplierName = t.supplierName });
              }
              SetSpec(Rlist);
              setSiteCode(Rlist);
              return Rlist;          
        }

        public string SuplierSiteCode()
        {
            string hql = "from  AscmSupplier where id in (select DISTINCT supplierId from AscmContainer)";
            IList<MideaAscm.Dal.Base.Entities.AscmSupplier> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<MideaAscm.Dal.Base.Entities.AscmSupplier>(hql);
            List<MideaAscm.Dal.Base.Entities.AscmSupplier> list = null;
            if (ilist != null && ilist.Count > 0)
            {
                list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<MideaAscm.Dal.Base.Entities.AscmSupplier>(ilist);
                System.Text.StringBuilder sbSn = new System.Text.StringBuilder();
                sbSn.Clear();
                var q = list.Select(e => e.id).Distinct();
                foreach (int supplierId in q)
                {
                    if (sbSn.Length != 0)
                    {
                        sbSn.Append(",");
                    }
                    sbSn.AppendFormat("{0}", supplierId);

                }
                if (string.IsNullOrEmpty(sbSn.ToString()))
                {
                    return "";
                }
                IList<MideaAscm.Dal.Base.Entities.AscmSupplierAddress> ilistII = YnDaoHelper.GetInstance().nHibernateHelper.Find<MideaAscm.Dal.Base.Entities.AscmSupplierAddress>("from AscmSupplierAddress where vendorId in (" + sbSn.ToString() + ")");
                List<MideaAscm.Dal.Base.Entities.AscmSupplierAddress> listAscmReadingHead = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList(ilistII);
                if (ilist.Count > 0)
                {
                    foreach (Dal.Base.Entities.AscmSupplier ascmTagLog in list)
                    {
                        if (ascmTagLog.id == 6128)
                        {
                            ascmTagLog.name = "美的";
                        }
                        else
                        {
                            ascmTagLog.name = listAscmReadingHead.Find(e => e.vendorId == ascmTagLog.id).vendorSiteCode;
                        }
                    }

                }

            }
             return YnBaseClass2.Helper.ObjectHelper.Serialize(list);



        }




        void setSiteCode(List<MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer>  list)
        {
            System.Text.StringBuilder sbSn = new System.Text.StringBuilder();
            sbSn.Clear();
            var q = list.Select(e => e.supplierId).Distinct();
            foreach (int supplierId in q)
            {            
                    if (sbSn.Length != 0)
                    {
                        sbSn.Append(",");
                    }
                    sbSn.AppendFormat("{0}", supplierId);
        
            }
            if (string.IsNullOrEmpty(sbSn.ToString()))
            {
                return;
            }
            IList<MideaAscm.Dal.Base.Entities.AscmSupplierAddress> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<MideaAscm.Dal.Base.Entities.AscmSupplierAddress>("from AscmSupplierAddress where vendorId in (" + sbSn.ToString() + ")");
            List<MideaAscm.Dal.Base.Entities.AscmSupplierAddress> listAscmReadingHead = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList(ilist);
            if (ilist.Count > 0)
            {
                foreach (Dal.SupplierPreparation.Entities.AscmContainer ascmTagLog in list)
                {
                    if (ascmTagLog.supplierId == 6128)
                    {
                        ascmTagLog._supplierName = "美的";
                    }
                    else
                    {
                        ascmTagLog._supplierName = listAscmReadingHead.Find(e => e.vendorId == ascmTagLog.supplierId).vendorSiteCode;
                    }
                }

            }
          
        }





    }
}
