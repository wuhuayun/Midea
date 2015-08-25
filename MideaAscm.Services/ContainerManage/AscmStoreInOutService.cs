using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Dal.ContainerManage.Entities;

namespace MideaAscm.Services.ContainerManage
{
    public class AscmStoreInOutService
    {
        private static AscmStoreInOutService ascmStoreInOutService;
        public static AscmStoreInOutService GetInstance()
        {
            if (ascmStoreInOutService == null)
                ascmStoreInOutService = new AscmStoreInOutService();
            return ascmStoreInOutService;
        }

        public void Save(AscmStoreInOut ascmStoreInOut)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save<AscmStoreInOut>(ascmStoreInOut);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save ascmStoreInOut)", ex);
                throw ex;
            }
        }

        public List<AscmStoreInOut> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord)
        {
            List<AscmStoreInOut> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmStoreInOut ";

                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, queryWord);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmStoreInOut> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmStoreInOut>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmStoreInOut>(ilist);
                    SetSupplier(list);
                    SetContainer(list);
                    SetSpecName(list);

                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmContainer)", ex);
                throw ex;
            }
            return list;
        }

        private void SetSupplier(List<AscmStoreInOut> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmStoreInOut ascmStoreInOut in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmStoreInOut.supplierId + "";
                }
                string sql = "from AscmSupplier where id in (" + ids + ")";
                IList<AscmSupplier> ilistAscmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql);
                if (ilistAscmSupplier != null && ilistAscmSupplier.Count > 0)
                {
                    List<AscmSupplier> listAscmSupplier = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(ilistAscmSupplier);
                    foreach (AscmStoreInOut ascmStoreInOut in list)
                    {
                        ascmStoreInOut.ascmSupplier = listAscmSupplier.Find(e => e.id == ascmStoreInOut.supplierId);
                    }
                }
            }
        }

        /// <summary>
        /// 2013年11月13日10:53:20
        /// 2013年11月13日
        /// 添加容器地址信息
        /// </summary>
        /// <param name="list"></param>
        private void SetContainer(List<AscmStoreInOut> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmStoreInOut ascmStoreInOut in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'" + ascmStoreInOut.containerId + "'";
                }
                string sql = "from AscmContainer where sn in (" + ids + ")";
                IList<AscmContainer> ilistAscmContainer = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainer>(sql);
                if (ilistAscmContainer != null && ilistAscmContainer.Count > 0)
                {
                    List<AscmContainer> listAscmContainer = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainer>(ilistAscmContainer);

                    SetSpec(listAscmContainer);
                    SetAscmReadingHead(listAscmContainer);
                    foreach (AscmStoreInOut ascmStoreInOut in list)
                    {
                        ascmStoreInOut.ascmContainer = listAscmContainer.Find(e => e.sn == ascmStoreInOut.containerId);
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



        private void SetSpecName(List<AscmStoreInOut> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmStoreInOut ascmContainer in list)
                {
                    if (ascmContainer.ascmContainer != null)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += "'" + ascmContainer.ascmContainer.specId + "'";
                    }
                }
                if (string.IsNullOrEmpty(ids))
                {
                    return;
                }
                string sql = "from AscmContainerSpec where id in (" + ids + ")";
                IList<AscmContainerSpec> ilistAscmContainerSpec = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainerSpec>(sql);
                if (ilistAscmContainerSpec != null && ilistAscmContainerSpec.Count > 0)
                {
                    List<AscmContainerSpec> listAscmContainerSpec = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainerSpec>(ilistAscmContainerSpec);
                    foreach (AscmStoreInOut ascmContainer in list)
                    {
                        ascmContainer.specName = listAscmContainerSpec.Find(e => e.id == ascmContainer.ascmContainer.specId).spec;
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
                    if (ascmContainer != null)
                    {
                        ids += "'" + ascmContainer.specId + "'";
                    }
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
        /// 出入库 报表查询
        /// 2013、07、17
        /// </summary>
        /// <param name="docNumber"></param>
        /// <returns></returns>
        public List<AscmStoreInOut> GetReportList(string docNumber)
        {           
            string strHql = "select b.specId, b.supplierId, count(b.specId), a.createUser, a.direction, a.docNumber from AscmStoreInOut as a,AscmContainer as b where a.containerId=b.sn and a.docNumber='" + docNumber + "' GROUP BY  b.supplierId, b.specId, a.createUser, a.direction, a.docNumber";
            List<AscmStoreInOut> st = new List<AscmStoreInOut>();
            try
            {
                if (docNumber.Contains("direction"))
                {
                    strHql = "select b.specId, b.supplierId, count(b.specId), a.direction  from AscmStoreInOut as a,AscmContainer as b where a.containerId=b.sn and " + docNumber + " GROUP BY  b.supplierId, b.specId, a.direction";
                }
                System.Collections.IList ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find(strHql);
                if (ilist.Count != 0)
                {
                    if (docNumber.Contains("direction"))
                    {
                        foreach (object[] obj in ilist)
                        {
                            foreach (object[] obj2 in YnDaoHelper.GetInstance().nHibernateHelper.Find("select name, docNumber from AscmSupplier where id=" + obj[1].ToString() + ""))
                            {
                                st.Add(new AscmStoreInOut {direction = obj[3].ToString(), supplierName = obj2[0].ToString(), specCount = Convert.ToInt32(obj[2].ToString()), specName = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select spec from AscmContainerSpec where id=" + obj[0].ToString() + "").ToString(), supplierDoc = obj2[1].ToString() });
                            }
                        }
                    }
                    else
                    {
                        foreach (object[] obj in ilist)
                        {
                            foreach (object[] obj2 in YnDaoHelper.GetInstance().nHibernateHelper.Find("select name, docNumber from AscmSupplier where id=" + obj[1].ToString() + ""))
                            {
                                st.Add(new AscmStoreInOut { docNumber = obj[5].ToString(), direction = obj[4].ToString(), createUser = obj[3].ToString(), supplierName = obj2[0].ToString(), specCount = Convert.ToInt32(obj[2].ToString()), specName = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select spec from AscmContainerSpec where id=" + obj[0].ToString() + "").ToString(), supplierDoc = obj2[1].ToString() });
                            }
                        }
                    }
                }
                return st;
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find 出入库报表)", ex);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docNumber"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public List<AscmStoreInOut> GetReportList(string docNumber, string direction)
        {
            string strHql = "select b.specId, b.supplierId, count(b.specId), a.direction  from AscmStoreInOut as a,AscmContainer as b where a.containerId=b.sn and a.docNumber='" + docNumber + "' GROUP BY  b.supplierId, b.specId, a.direction";
            List<AscmStoreInOut> st = new List<AscmStoreInOut>();
            System.Collections.IList ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find(strHql);
            if (ilist.Count != 0)
            {
                foreach (object[] obj in ilist)
                {
                    foreach (object[] obj2 in YnDaoHelper.GetInstance().nHibernateHelper.Find("select name, docNumber from AscmSupplier where id=" + obj[1].ToString() + ""))
                    {
                        st.Add(new AscmStoreInOut { docNumber = obj[5].ToString(), direction = obj[4].ToString(), createUser = obj[3].ToString(), supplierName = obj2[0].ToString(), specCount = Convert.ToInt32(obj[2].ToString()), specName = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select spec from AscmContainerSpec where id=" + obj[0].ToString() + "").ToString(), supplierDoc = obj2[1].ToString() });
                    }
                }
            }
            return st;
        }

        /// <summary>
        /// 取得最新的一次出入库记录
        /// False表示已存在该物品的记录
        /// </summary>
        /// <param name="epcId"></param>
        /// <param name="direction"></param>
        /// <returns>false代表已存在该记录，true代表不存在</returns>
        public bool GetLastByEpc(string epcId, string direction)
        {
            try
            {
                string strHql = "select count(*) from AscmStoreInOut a where epcId='" + epcId + "' and direction='" + direction + "' and readTime=(select max(readTime) from AscmStoreInOut where  containerId=a.containerId)";

                if (YnDaoHelper.GetInstance().nHibernateHelper.GetCount(strHql) > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmStoreInOut)", ex);
                throw ex;
            }

        }
        /// <summary>
        /// 得到一次最新的出入库记录
        /// </summary>
        /// <param name="epcId"></param>
        /// <returns></returns>
        public object GetLastByEpc(string epcId)
        {
            try
            {
                string strHql = "select direction  from AscmStoreInOut a where epcId='" + epcId + "' and readTime=(select max(readTime) from AscmStoreInOut where  containerId=a.containerId)";
                object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(strHql);
                return obj;

            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmStoreInOut)", ex);
                throw ex;
            }
        }

        /// <summary>
        /// 手持机出入库
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="containerId"></param>
        /// <param name="direction"></param>
        public string AscmStoreInOutSave(string userName, string containerId, string direction,string docNumber)
        {
            NHibernate.IQuery query = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().CreateQuery("from AscmStoreInOut where containerId='" + containerId + "' and readTime=(select max(readTime) from AscmStoreInOut where containerId='" + containerId + "')");
            Dal.ContainerManage.Entities.AscmStoreInOut ack = query.UniqueResult<Dal.ContainerManage.Entities.AscmStoreInOut>();

            if (ack != null && ack.direction == direction)
            {
                return "请勿重复提交！";
            }
            else
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {

                        Dal.SupplierPreparation.Entities.AscmContainer ascmContainer = MideaAscm.Services.SupplierPreparation.AscmContainerService.GetInstance().Get(containerId);
                        Dal.ContainerManage.Entities.AscmStoreInOut ascmStoreInOut = new Dal.ContainerManage.Entities.AscmStoreInOut();
                        ascmStoreInOut.containerId = ascmContainer.sn;
                       
                        ascmStoreInOut.createUser = userName;
                        ascmStoreInOut.direction = direction;
                        ascmStoreInOut.containerId = ascmContainer.sn;
                        ascmStoreInOut.supplierId = ascmContainer.supplierId;
                        ascmStoreInOut.epcId = ascmContainer.rfid;
                        ascmStoreInOut.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        ascmStoreInOut.readTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        ascmStoreInOut.docNumber = docNumber;//每次提交都产生新的单据号
                        ascmStoreInOut.id = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmStoreInOut)") + 1;
                        if (direction == "STOREIN")
                        {
                            ascmStoreInOut.status = AscmStoreInOut.StatusDefine.inStored;
                            object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select to_char(id) from  AscmReadingHead where address= '容器监管中心' and ip='0.0.0.0'");

                            ascmContainer.place = obj.ToString();



                        }
                        else
                        {
                            object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select to_char(id) from  AscmReadingHead where address= '厂区外' and ip='0.0.0.0'");
                            ascmStoreInOut.status = AscmStoreInOut.StatusDefine.outStored;
                            if (ascmContainer.supplierId != 6128)
                            {
                                ascmContainer.place = obj.ToString();
                            }
                            else
                            {
                                ascmContainer.place = null;
                            }

                        }
                        ascmContainer.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmContainer.modifyUser = userName;

                        YnDaoHelper.GetInstance().nHibernateHelper.Update<Dal.SupplierPreparation.Entities.AscmContainer>(ascmContainer);
                        YnDaoHelper.GetInstance().nHibernateHelper.Save<Dal.ContainerManage.Entities.AscmStoreInOut>(ascmStoreInOut);
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

        /// <summary>
        /// 出入库实时显示
        /// </summary>
        /// <param name="docNumber"></param>
        /// <returns></returns>
        public List<AscmStoreInOut> GetCheckInfBydocNumber(string docNumber=null)
        {
            string number = "";
            if (string.IsNullOrEmpty(docNumber))
            {
                number = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select max(docNumber) from AscmStoreInOut").ToString();
            }
            else 
            {
                number = docNumber;
            }
            string strHql = "select b.specId, b.supplierId, count(b.specId), a.direction  from AscmStoreInOut as a, AscmContainer as b where a.containerId=b.sn and a.docNumber='" + number + "' GROUP BY  b.supplierId, b.specId, a.direction ";
            List<AscmStoreInOut> st = new List<AscmStoreInOut>();
            try
            {
               
                System.Collections.IList ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find(strHql);
                string str_readTime = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select max(createTime) from AscmStoreInOut where docNumber='" + number + "'").ToString();
                if (ilist.Count != 0)
                {
                        foreach (object[] obj in ilist)
                        {
                            if (obj[1].ToString() == "6128")
                            {
                                st.Add(new AscmStoreInOut { direction = obj[3].ToString(), supplierName = "美的", specCount = Convert.ToInt32(obj[2].ToString()), specName = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select spec from AscmContainerSpec where id=" + obj[0].ToString() + "").ToString(), readTime = str_readTime });
                            }
                            else
                            {
                                foreach (object[] obj2 in YnDaoHelper.GetInstance().nHibernateHelper.Find("select vendorSiteCode, vendorId from AscmSupplierAddress where vendorId=" + obj[1].ToString() + ""))
                                {
                                    st.Add(new AscmStoreInOut { direction = obj[3].ToString(), supplierName = obj2[0].ToString(), specCount = Convert.ToInt32(obj[2].ToString()), specName = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select spec from AscmContainerSpec where id=" + obj[0].ToString() + "").ToString(), readTime = str_readTime });
                                }
                            }
                        }
                    
                }
                return st;
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(根据单号查询出入库信息失败)", ex);
                throw ex;
            }
        }

    }
}
