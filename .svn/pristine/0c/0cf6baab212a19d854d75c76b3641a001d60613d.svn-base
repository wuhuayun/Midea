using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Services.SupplierPreparation;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Services.Base;
using YnBaseDal;
using MideaAscm.Dal;
using System.Collections;

namespace MideaAscm.Services.Warehouse
{
    public class AscmWmsIncAccCheckoutService
    {
        private static AscmWmsIncAccCheckoutService service;
        public static AscmWmsIncAccCheckoutService GetInstance()
        {
            if (service == null)
                service = new AscmWmsIncAccCheckoutService();
            return service;
        }

        public CurrentDeliBatSum GetCurrentDeliBatSum(string warehouseId = "W312材料")
        {
            CurrentDeliBatSum currentDeliBatSum = new CurrentDeliBatSum();
            string status = AscmDeliBatSumMain.StatusDefine.inPlant;
            try
            {
                AscmContainerDelivery ascmContainerDelivery = AscmContainerDeliveryService.GetInstance().GetCurrent(warehouseId);
                if (ascmContainerDelivery == null)
                    return null;
                //string whereOther = " warehouseId='" + warehouseId + "' and status='" + status + "' and id in (select batSumMainId from AscmContainerDelivery where )";
                string whereOther = " id=" + ascmContainerDelivery.batSumMainId;
                List<AscmDeliBatSumMain> listAscmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().GetList("id", "desc", "", whereOther);
                if (listAscmDeliBatSumMain != null && listAscmDeliBatSumMain.Count() > 0)
                {
                    AscmDeliBatSumMain ascmDeliBatSumMain = listAscmDeliBatSumMain[0];// listAscmDeliBatSumMain.OrderByDescending(item => item.toPlantTime).First();
                    currentDeliBatSum.supplierName = ascmDeliBatSumMain.ascmSupplier.ascmSupplierAddress.vendorSiteCode;
                    currentDeliBatSum.deliBatSumNo = ascmDeliBatSumMain.docNumber;
                    currentDeliBatSum.containerNumber = ascmDeliBatSumMain.containerNumber;
                    currentDeliBatSum.toPlantTime = ascmContainerDelivery.modifyTime;
                    currentDeliBatSum.inWarehouseContainerNumber = ascmDeliBatSumMain.inWarehouseContainerNumber;
                    List<AscmDeliBatSumDetail> listAscmDeliBatSumDetail = AscmDeliBatSumDetailService.GetInstance().GetListByMainId(ascmDeliBatSumMain.id);
                    List<AscmContainerDelivery> listAscmContainerDelivery = AscmContainerDeliveryService.GetInstance().GetListByDeliverySumMainId(ascmDeliBatSumMain.id);
                    listAscmContainerDelivery = listAscmContainerDelivery.Where(item => item.status == AscmContainerDelivery.StatusDefine.inWarehouseDoor).ToList();
                    if (listAscmDeliBatSumDetail != null && listAscmDeliBatSumDetail.Count() > 0)
                    {
                        List<CurrentDeliBatSumList> listCurrentDeliBatSumList = new List<CurrentDeliBatSumList>();
                        foreach (AscmDeliBatSumDetail ascmDeliBatSumDetail in listAscmDeliBatSumDetail)
                        {
                            CurrentDeliBatSumList currentDeliBatSumList = new CurrentDeliBatSumList();
                            currentDeliBatSumList.batchDocNumber = ascmDeliBatSumDetail.batchDocNumber;
                            currentDeliBatSumList.materialDocNumber = ascmDeliBatSumDetail.materialDocNumber;
                            currentDeliBatSumList.materialName = ascmDeliBatSumDetail.materialDescription;
                            currentDeliBatSumList.totalNumber = ascmDeliBatSumDetail.totalNumber;
                            if (listAscmContainerDelivery != null && listAscmContainerDelivery.Count() > 0)
                            {
                                List<AscmContainerDelivery> vList = listAscmContainerDelivery.Where(item => item.deliveryOrderBatchId == ascmDeliBatSumDetail.batchId && item.materialId == ascmDeliBatSumDetail.materialId).ToList();
                                if (vList != null && vList.Count() > 0)
                                    currentDeliBatSumList.receiveNumber = vList.Sum(item => item.quantity);
                            }
                            listCurrentDeliBatSumList.Add(currentDeliBatSumList);
                        }
                        currentDeliBatSum.list = listCurrentDeliBatSumList;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return currentDeliBatSum;
        }

        /// <summary>入库实时校验</summary>
        public DeliBatSumCheckIn Get(string warehouseId = "W312材料")
        {
            DeliBatSumCheckIn deliBatSumCheckIn = new DeliBatSumCheckIn();
            if (!string.IsNullOrWhiteSpace(warehouseId))
            {
                try
                {
                    string hql = "from AscmContainerDelivery";
                    string where = string.Empty;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "status='" + AscmContainerDelivery.StatusDefine.inWarehouseDoor + "'");
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "batSumMainId in (select id from AscmDeliBatSumMain where warehouseId='" + warehouseId + "')");
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "modifyTime is not null");
                    hql += " where " + where;
                    string sort = " order by modifyTime desc ";
                    IList<AscmContainerDelivery> ilist = YnDaoHelper.GetInstance().nHibernateHelper.FindTop<AscmContainerDelivery>(hql + sort, 1);
                    if (ilist != null && ilist.Count > 0)
                    {
                        AscmContainerDelivery containerDelivery = ilist[0];
                        //供方简称
                        string supplierShortName = "select vendorSiteCode from AscmSupplierAddress a where a.vendorSiteCodeAlt=s.docNumber and rownum=1";
                        hql = string.Format("select new AscmDeliBatSumMain(h.id,h.docNumber,({0})) from AscmDeliBatSumMain h,AscmSupplier s", supplierShortName);
                        where = string.Empty;
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "h.supplierId=s.id");
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "h.id=" + containerDelivery.batSumMainId);
                        hql += " where " + where;
                        AscmDeliBatSumMain main = AscmDeliBatSumMainService.GetInstance().Get(hql);
                        if (main != null)
                        {
                            deliBatSumCheckIn.docNumber = main.docNumber;
                            deliBatSumCheckIn.supplierShortName = main.supplierNameCn;
                            deliBatSumCheckIn.containerCheckIns = new List<ContainerCheckIn>();
                            deliBatSumCheckIn.checkTime = containerDelivery.modifyTime;
                            deliBatSumCheckIn.warelocations =new List<string>();

                            //容器
                            hql = "select new AscmContainerDelivery(acd.containerSn,acd.status,acs.spec) from AscmContainerDelivery acd,AscmContainer ac,AscmContainerSpec acs";
                            where = string.Empty;
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "acd.containerSn=ac.sn");
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "ac.specId=acs.id");
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "acd.batSumMainId=" + main.id);
                            hql += " where " + where;
                            IList<AscmContainerDelivery> ilist2 = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainerDelivery>(hql);
                            if (ilist2 != null && ilist2.Count > 0)
                            {
                                var result = ilist2.GroupBy(P => P.containerSpec);
                                foreach (IGrouping<string, AscmContainerDelivery> ig in result)
                                {
                                    ContainerCheckIn containerCheckIn = new ContainerCheckIn();
                                    containerCheckIn.spec = ig.Key;
                                    containerCheckIn.quantity = ig.GroupBy(P => P.containerSn).Count();
                                    containerCheckIn.checkQuantity = ig.Where(P => P.status == AscmContainerDelivery.StatusDefine.inWarehouseDoor).GroupBy(P => P.containerSn).Count();
                                    deliBatSumCheckIn.containerCheckIns.Add(containerCheckIn);
                                }
                            }

                            //货位指引
                            string sql = "select distinct materialid from ascm_deli_bat_sum_detail where mainid=" + main.id;
                            IList ilistMaterialId = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);
                            foreach (object[] item in ilistMaterialId)
                            {
                                if (item != null && item.Length > 0 && !string.IsNullOrEmpty(item[0].ToString()))
                                {
                                    sql = "select buildingarea from ascm_warelocation"
                                        + " where warehouseid='" + warehouseId + "'"
                                        + "   and (id in(select warelocationId from ascm_location_material_link where materialid=" + item[0].ToString() + ") or categoryCode='0000')"
                                        + "   and rownum=1";
                                    object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObjectBySQL(sql);
                                    if (obj != null && !string.IsNullOrEmpty(obj.ToString()) && !deliBatSumCheckIn.warelocations.Contains(obj.ToString()))
                                        deliBatSumCheckIn.warelocations.Add(obj.ToString());
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return deliBatSumCheckIn;
        }

        /// <summary>预约到货</summary>
        public List<AppointmentArrivalOfGoods> GetAppointmentArrivalOfGoods(string warehouseId = "W312材料")
        {
            List<AppointmentArrivalOfGoods> list = new List<AppointmentArrivalOfGoods>();

            if (string.IsNullOrEmpty(warehouseId))
                return list;

            //获取今日的合单
            string supplierShortName = "select vendorSiteCode from AscmSupplierAddress a where a.vendorSiteCodeAlt=s.docNumber and rownum=1";
            string hql = string.Format("select new AscmDeliBatSumMain(h,'',({0}),'') from AscmDeliBatSumMain h,AscmSupplier s", supplierShortName);
            string where = string.Empty;
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "h.supplierId=s.id");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "h.warehouseId='" + warehouseId + "'");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "h.appointmentStartTime>='" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "'");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "h.appointmentStartTime<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'");
            hql += " where " + where;
            IList<AscmDeliBatSumMain> ilistDeliBatSumMain = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliBatSumMain>(hql);
            if (ilistDeliBatSumMain == null || ilistDeliBatSumMain.Count == 0)
                return list;

            //获取合单明细和批单状态
            hql = "select new AscmDeliBatSumDetail(d,b.ascmStatus,m.docNumber) from AscmDeliBatSumDetail d,AscmDeliveryOrderBatch b,AscmMaterialItem m";
            where = string.Empty;
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "d.batchId=b.id");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "d.materialId=m.id");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "d.mainId in(" + string.Join(",", ilistDeliBatSumMain.Select(P => P.id)) + ")");
            hql += " where " + where;
            IList<AscmDeliBatSumDetail> ilistDeliBatSumDetail = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliBatSumDetail>(hql);
            if (ilistDeliBatSumDetail == null || ilistDeliBatSumDetail.Count == 0)
                return list;

            foreach (AscmDeliBatSumMain main in ilistDeliBatSumMain)
            {
                //过滤已入厂的合单
                if (main.status == AscmDeliBatSumMain.StatusDefine.inPlant)
                    continue;

                //过滤已接收的合单（通过批单接收状态）
                var details = ilistDeliBatSumDetail.Where(P => P.mainId == main.id).ToList();
                if (details.Count == 0 || details.Exists(P => P.ascmStatus == MideaAscm.Dal.FromErp.Entities.AscmDeliveryOrderBatch.AscmStatusDefine.received 
                    || P.ascmStatus == MideaAscm.Dal.FromErp.Entities.AscmDeliveryOrderBatch.AscmStatusDefine.receiveFail))
                {
                    continue;
                }

                AppointmentArrivalOfGoods appointmentArrivalOfGoods = new AppointmentArrivalOfGoods();
                appointmentArrivalOfGoods.id = main.id;
                appointmentArrivalOfGoods.docNumber = main.docNumber;
                appointmentArrivalOfGoods.category = details.FirstOrDefault().mtlDocNumber.Substring(0, 4);
                appointmentArrivalOfGoods.supplierShortName = main.supplierNameCn;
                appointmentArrivalOfGoods.appointmentStartTime = main.appointmentStartTimeShow;
                appointmentArrivalOfGoods.appointmentEndTime = main.appointmentEndTimeShow;
                appointmentArrivalOfGoods.status = main.statusCn;
                list.Add(appointmentArrivalOfGoods);
            }
            //过滤已校验的合单（通过入库校验）
            if (list.Count > 0)
            {
                string sql = "select batSumMainId from ascm_container_delivery";
                where = string.Empty;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "batSumMainId in(" + string.Join(",", list.Select(P => P.id)) + ")");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "status='" + AscmContainerDelivery.StatusDefine.inWarehouseDoor + "'");
                sql += " where " + where;
                IList ilist = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);
                List<string> listMainId = new List<string>();
                foreach (object[] item in ilist)
                {
                    if (item != null && item.Length > 0)
                        listMainId.Add(item[0].ToString());
                }
                if (listMainId.Count > 0)
                    list.RemoveAll(P => listMainId.Contains(P.id.ToString()));
            }

            if (list.Count > 0)
            {
                foreach (AppointmentArrivalOfGoods appointmentArrivalOfGoods in list)
                {
                    appointmentArrivalOfGoods.checkResult = CheckResultType.unArrive;
                    DateTime appointmentStartTime, appointmentEndTime;
                    if (DateTime.TryParse(appointmentArrivalOfGoods.appointmentEndTime, out appointmentEndTime))
                    {
                        if (DateTime.Now.CompareTo(appointmentEndTime) > 0)
                            appointmentArrivalOfGoods.checkResult = CheckResultType.overtime;
                        else if (DateTime.TryParse(appointmentArrivalOfGoods.appointmentStartTime, out appointmentStartTime) && DateTime.Now.CompareTo(appointmentStartTime) >= 0)
                            appointmentArrivalOfGoods.checkResult = CheckResultType.arrived;
                    }
                }
            }
            return list;
        }

        /// <summary>发料校验</summary>
        public List<AscmWmsStoreIssueCheck> GetWmsLedStoreIssueCheck()
        {
            List<AscmWmsStoreIssueCheck> listStoreIssueCheck = new List<AscmWmsStoreIssueCheck>();
            //获取当天的领料单
            string whereOther = "";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "createTime>='" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "'");
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "createTime<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'");
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "checkOutNo>0");

            List<AscmWmsMtlRequisitionMain> listRequisitionMain = AscmWmsMtlRequisitionMainService.GetInstance().GetList(null, "", "", "", whereOther);
            if (listRequisitionMain == null || listRequisitionMain.Count == 0)
                return listStoreIssueCheck;
            AscmWmsMtlRequisitionMainService.GetInstance().SetWipDiscreteJobs(listRequisitionMain, true, true, false, false);
            //获取当天的领料明细
            List<AscmWmsMtlRequisitionDetail> listRequisitionDetail = AscmWmsMtlRequisitionDetailService.GetInstance().GetList(listRequisitionMain);
            if (listRequisitionDetail == null || listRequisitionDetail.Count == 0)
                return listStoreIssueCheck;
            //获取领料单与备料单关联
            List<AscmWmsMtlReqMainLink> listReqMainLink = AscmWmsMtlRequisitionMainService.GetInstance().GetReqMainLinkList(listRequisitionMain);
            //获取备料容器
            List<AscmWmsContainerDelivery> listContainerDelivery = AscmWmsContainerDeliveryService.GetInstance().GetList(listReqMainLink.Select(P => P.reqMainLinkPK.preMainId).ToList(), "");

            //按领料员分组
            var result = listRequisitionMain.Where(P => !string.IsNullOrEmpty(P.workerId)).GroupBy(P => P.workerId);
            foreach (IGrouping<string, AscmWmsMtlRequisitionMain> ig in result)
            {
                //按领料次数分组
                var result2 = ig.GroupBy(P => P.checkOutNo);
                foreach (IGrouping<int, AscmWmsMtlRequisitionMain> ig2 in result2)
                {
                    AscmWmsStoreIssueCheck storeIssueCheck = new AscmWmsStoreIssueCheck();
                    storeIssueCheck.workerId = ig.Key;
                    storeIssueCheck.checkTime = ig2.First().checkTime;
                    storeIssueCheck.times = ig2.Key;
                    storeIssueCheck.destination = ig2.First().jobScheduleGroupsName;
                    listStoreIssueCheck.Add(storeIssueCheck);

                    //获取应发容器
                    List<AscmWmsContainerDelivery> listContainerDeliverySub = new List<AscmWmsContainerDelivery>();
                    foreach (AscmWmsMtlRequisitionMain requisitionMain in ig2)
                    {
                        List<AscmWmsMtlRequisitionDetail> listRequisitionDetailSub = listRequisitionDetail.Where(P => P.mainId == requisitionMain.id).ToList();
                        storeIssueCheck.shouldMaterialNum += listRequisitionDetailSub.Sum(P => P.quantity);
                        //考虑相同作业、相同物料、取自不同货位，所以应按物料分组
                        var detailResult = listRequisitionDetailSub.GroupBy(P => P.materialId);
                        foreach (IGrouping<int, AscmWmsMtlRequisitionDetail> detailIg in detailResult)
                        {
                            var findContainerDelivery = listContainerDeliverySub.Where(P => P.wipEntityId == requisitionMain.wipEntityId
                                && P.materialId == detailIg.Key
                                && listReqMainLink.Where(T => T.reqMainLinkPK.reqMainId == requisitionMain.id).Select(T => T.reqMainLinkPK.preMainId).Contains(P.preparationMainId));
                            if (findContainerDelivery != null)
                            {
                                listContainerDeliverySub.AddRange(findContainerDelivery);
                            }
                        }
                    }
                    if (listContainerDeliverySub.Count > 0)
                    {
                        storeIssueCheck.shouldContainerNum = listContainerDeliverySub.Select(P => P.containerSn).Distinct().Count();

                        var findContainerDelivery = listContainerDeliverySub.Where(P => P.status == AscmWmsContainerDelivery.StatusDefine.outWarehouseDoor);
                        if (findContainerDelivery.Count() > 0)
                        {
                            storeIssueCheck.realMaterialNum = findContainerDelivery.Sum(P => P.quantity);
                            storeIssueCheck.realContainerNum = findContainerDelivery.Select(P => P.containerSn).Distinct().Count();
                        }
                    }

                    //设置产线
                    storeIssueCheck.productionLine = string.Join("、", ig2.Where(P => !string.IsNullOrEmpty(P.jobProductionLine)).Select(P => P.jobProductionLine).Distinct().OrderBy(P => P));

                    //设置发料状态
                    storeIssueCheck.status = IssueStatus.prepared;
                    //获取校验的最后时间
                    string lastCheckTime = ig2.Where(P => !string.IsNullOrEmpty(P.checkTime)).Max(P => P.checkTime);
                    if (!string.IsNullOrEmpty(lastCheckTime))
                    {
                        //暂定最后校验时间与当前时间间隔30秒以内的设置为“正在出仓”
                        DateTime checkTime;
                        if (DateTime.TryParse(lastCheckTime, out checkTime) && DateTime.Now.Subtract(checkTime).TotalMilliseconds < 30)
                            storeIssueCheck.status = IssueStatus.outingOfWarehouse;
                    }
                    if (storeIssueCheck.waitContainerNum == 0 && storeIssueCheck.status != IssueStatus.outingOfWarehouse)
                        storeIssueCheck.status = IssueStatus.outedOfWarehouse;
                }
            }
            return listStoreIssueCheck;
        }

        /// <summary>需求备料平台</summary>
        public List<AscmWmsWipRequireLedMonitor> GetWipRequireLedMonitorList()
        {
            List<AscmWmsWipRequireLedMonitor> listWipRequireLedMonitorList = new List<AscmWmsWipRequireLedMonitor>();
            //获取当天需求备料明细
            string hql = "select new AscmWmsPreparationDetail(d.materialId,m.docNumber,d.planQuantity,d.issueQuantity,d.wipEntityId,w.name) from AscmWmsPreparationDetail d,AscmMaterialItem m,AscmWipEntities w";
            string where = string.Empty;
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "d.materialId=m.id");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "d.wipEntityId=w.wipEntityId");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "d.mainId in(select id from AscmWmsPreparationMain where pattern='" + AscmWmsPreparationMain.PatternDefine.wipRequire + "')");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "d.createTime>='" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "'");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "d.createTime<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "d.warehouseId in('W112材料','W114材料','W312材料')");
            hql += " where " + where;
            List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetList(hql);
            if (listPreparationDetail != null && listPreparationDetail.Count > 0)
            { 
                //取物流领料模块数据
                hql = "select new AscmWipRequirementOperations(o.wipEntityId,o.inventoryItemId,t.workerId,t.productLine) from AscmWipRequirementOperations o,AscmGetMaterialTask t";
                where = string.Empty;
                string _where = string.Empty;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "o.taskId=t.id");
                var result = listPreparationDetail.GroupBy(P => P.wipEntityId);
                foreach (IGrouping<int, AscmWmsPreparationDetail> ig in result)
                {
                    string whereOther = string.Empty;
                    var materialIds = ig.Select(P => P.materialId).Distinct();
                    var count = materialIds.Count();
                    string ids = string.Empty;
                    for (int i = 0; i < count; i++)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += materialIds.ElementAt(i);
                        if ((i + 1) % 500 == 0 || (i + 1) == count)
                        {
                            if (!string.IsNullOrEmpty(ids))
                            {
                                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(whereOther, "o.inventoryItemId in(" + ids + ")");
                            }
                            ids = string.Empty;
                        }
                    }
                    _where = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(_where, "o.wipEntityId=" + ig.Key + " and (" + whereOther + ")");
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, _where);
                hql += " where " + where;
                List<MideaAscm.Dal.FromErp.Entities.AscmWipRequirementOperations> listBom = MideaAscm.Services.FromErp.AscmWipRequirementOperationsService.GetInstance().GetList(hql);

                //按物料分组统计
                var gb = listPreparationDetail.GroupBy(P => P.materialId);
                foreach (IGrouping<int, AscmWmsPreparationDetail> ig in gb)
                {
                    string materialDocNumber = ig.FirstOrDefault().materialDocNumber;
                    AscmWmsWipRequireLedMonitor wipRequireLedMonitor = new AscmWmsWipRequireLedMonitor();
                    wipRequireLedMonitor.materialDocNumber = materialDocNumber;
                    wipRequireLedMonitor.netQuantity = ig.Sum(P => P.planQuantity);
                    wipRequireLedMonitor.issuedQuantity = ig.Sum(P => P.issueQuantity);
                    if (listBom != null && listBom.Count > 0)
                    {
                        var _listBom = listBom.Where(P => P.inventoryItemId == ig.Key && ig.Select(T => T.wipEntityId).Contains(P.wipEntityId));
                        if (_listBom != null && _listBom.Count() > 0)
                        {
                            wipRequireLedMonitor.workerId = string.Join("、", _listBom.Select(P => P.workerId));
                            wipRequireLedMonitor.productionLine = string.Join("、", _listBom.Select(P => P.productLine));
                        }
                    }
                    listWipRequireLedMonitorList.Add(wipRequireLedMonitor);
                }
            }

            return listWipRequireLedMonitorList;
        }
    }
}
