using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.IEntity;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Dal;
using MideaAscm.Services.FromErp;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Services.MesInterface;
using MideaAscm.Services.Warehouse;
using MideaAscm.Services.GetMaterialManage;
using MideaAscm.Dal.GetMaterialManage.Entities;
using NHibernate;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Services.Base;

namespace MideaAscm.Services.IEntity
{
    public class WmsAndLogisticsService
    {
        private static WmsAndLogisticsService service;
        public static WmsAndLogisticsService GetInstance()
        {
            if (service == null)
                service = new WmsAndLogisticsService();
            return service;
        } 

        /// <summary>
        /// 备料信息
        /// </summary>
        /// <param name="list">数据List</param>
        public void UpdateWmsPreparationInfo(List<WmsAndLogistics> list)
        {
            List<AscmWipRequirementOperations> listRequirementOperations = new List<AscmWipRequirementOperations>();
            try
            {
                if (list != null && list.Count > 0)
                {
                    foreach (WmsAndLogistics wmsAndLogistics in list)
                    {
                        string sql = "from AscmWipRequirementOperations";
                        string where = "", whereQueryWord = "";
                        if (!string.IsNullOrEmpty(wmsAndLogistics.wipEntityId.ToString()) && !string.IsNullOrEmpty(wmsAndLogistics.materialId.ToString()))
                        {
                            whereQueryWord = "wipEntityId =" + wmsAndLogistics.wipEntityId.ToString();
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                            whereQueryWord = "inventoryItemId = " + wmsAndLogistics.materialId.ToString();
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                            if (!string.IsNullOrEmpty(where))
                                sql += " where " + where;
                            IList<AscmWipRequirementOperations> ilistRequirementOperations = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql);
                            if (ilistRequirementOperations.Count > 0 && ilistRequirementOperations != null)
                            {
                                List<AscmWipRequirementOperations> templist = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilistRequirementOperations);
                                //foreach (AscmWipRequirementOperations ascmWipRequirementOperations in templist)
                                //{
                                //    ascmWipRequirementOperations.wmsPreparationQuantity += wmsAndLogistics.quantity;
                                //    if (!string.IsNullOrEmpty(ascmWipRequirementOperations.wmsPreparationString))
                                //        ascmWipRequirementOperations.wmsPreparationString += ",";
                                //    ascmWipRequirementOperations.wmsPreparationString += wmsAndLogistics.preparationString + "[" + wmsAndLogistics.quantity + "]";
                                //    listRequirementOperations.Add(ascmWipRequirementOperations);
                                //}

                                foreach (AscmWipRequirementOperations ascmWipRequirementOperations in templist)
                                {
                                    if (ascmWipRequirementOperations.supplySubinventory == wmsAndLogistics.warehouseId)
                                    {
                                        //作业BOM子库与备料子库相同
                                        ascmWipRequirementOperations.wmsPreparationQuantity += wmsAndLogistics.quantity;
                                        if (!string.IsNullOrEmpty(ascmWipRequirementOperations.wmsPreparationString))
                                            ascmWipRequirementOperations.wmsPreparationString += ",";
                                        ascmWipRequirementOperations.wmsPreparationString += wmsAndLogistics.preparationString + "[" + wmsAndLogistics.quantity + "]";
                                        listRequirementOperations.Add(ascmWipRequirementOperations);
                                    }
                                    else
                                    {
                                        //作业BOM子库与备料子库不同.具体步骤如下：①判断是否包含该任务，包括则关联BOM，否则创建任务；②与作业BOM子库与备料子库相同操作
                                        string createTime = "";
                                        int taskId = AscmGetMaterialTaskService.GetInstance().IsContainsMeetTheConditionTask(wmsAndLogistics.warehouseId, ascmWipRequirementOperations.inventoryItemId, ascmWipRequirementOperations.wipEntityId, ref createTime);
                                        
                                        if (taskId > 0)
                                        {
                                            ascmWipRequirementOperations.taskId = taskId;//修改任务与Bom关联信息
                                            ascmWipRequirementOperations.wmsPreparationWarehouse = wmsAndLogistics.warehouseId;

                                            ascmWipRequirementOperations.wmsPreparationQuantity += wmsAndLogistics.quantity;
                                            if (!string.IsNullOrEmpty(ascmWipRequirementOperations.wmsPreparationString))
                                                ascmWipRequirementOperations.wmsPreparationString += ",";
                                            ascmWipRequirementOperations.wmsPreparationString += wmsAndLogistics.preparationString + "[" + wmsAndLogistics.quantity + "]";
                                            listRequirementOperations.Add(ascmWipRequirementOperations);
                                        }
                                        else
                                        {
                                            AscmMaterialItem ascmMaterialItem = AscmMaterialItemService.GetInstance().Get(ascmWipRequirementOperations.inventoryItemId);
                                            if (ascmMaterialItem == null)
                                                throw new Exception("Bom关联物料实体查询失败！");

                                            AscmWipEntities ascmWipEntities = AscmWipEntitiesService.GetInstance().Get(ascmWipRequirementOperations.wipEntityId);
                                            if (ascmWipEntities == null)
                                                throw new Exception("Bom关联作业实体查询失败！");

                                            List<AscmDiscreteJobs> listAscmDiscreteJobs = AscmDiscreteJobsService.GetInstance().GetList("from AscmDiscreteJobs where jobId = '" + ascmWipEntities.name + "'", true);
                                            if (listAscmDiscreteJobs == null || listAscmDiscreteJobs.Count == 0)
                                                throw new Exception("Bom关联作业实体查询失败！");

                                            string productLine = listAscmDiscreteJobs[0].productLine;
                                            int identificationId = listAscmDiscreteJobs[0].identificationId;
                                            string rankerId = listAscmDiscreteJobs[0].workerId;
                                            string uploadDate = listAscmDiscreteJobs[0].time;
                                            string taskTime = listAscmDiscreteJobs[0].onlineTime;
                                            
                                            AscmGetMaterialTask ascmGetMaterialTask = new AscmGetMaterialTask();
                                            int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmGetMaterialTask where taskId like '%T%' and createTime like '%" + createTime + "%'");
                                            ascmGetMaterialTask.id = maxId++;
                                            ascmGetMaterialTask.taskId = (maxId == 0) ? "T0001" : "T" + (int.Parse(AscmGetMaterialTaskService.GetInstance().Get(maxId).taskId.Substring(1, 4)) + 1).ToString();
                                            ascmGetMaterialTask.warehouserId = wmsAndLogistics.warehouseId;
                                            object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select description from AscmWarehouse where id = '" + wmsAndLogistics.warehouseId + "'");
                                            if (obj != null)
                                                ascmGetMaterialTask.warehouserPlace = obj.ToString();
                                            ascmGetMaterialTask.createUser = "sa";
                                            ascmGetMaterialTask.createTime = createTime + " " + DateTime.Now.TimeOfDay.ToString("HH:mm");
                                            ascmGetMaterialTask.modifyUser = "sa";
                                            ascmGetMaterialTask.modifyTime = createTime + " " + DateTime.Now.TimeOfDay.ToString("HH:mm");
                                            ascmGetMaterialTask.IdentificationId = identificationId;
                                            ascmGetMaterialTask.productLine = productLine;

                                            if (ascmGetMaterialTask.IdentificationId == 1)
                                                ascmGetMaterialTask.mtlCategoryStatus = ascmMaterialItem.zMtlCategoryStatus;
                                            else if (ascmGetMaterialTask.IdentificationId == 2)
                                                ascmGetMaterialTask.mtlCategoryStatus = ascmMaterialItem.dMtlCategoryStatus;

                                            ascmGetMaterialTask.rankerId = rankerId;
                                            ascmGetMaterialTask.status = AscmGetMaterialTask.StatusDefine.notAllocate;

                                            if (ascmGetMaterialTask.mtlCategoryStatus == MtlCategoryStatusDefine.preStock)
                                                ascmGetMaterialTask.materialDocNumber = ascmMaterialItem.docNumber;

                                            ascmGetMaterialTask.materialType = AscmGetMaterialTask.materialTypeDefine.ts;
                                            ascmGetMaterialTask.uploadDate = uploadDate.Substring(0,10);
                                            ascmGetMaterialTask.taskTime = taskTime;

                                            object which = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(which) from AscmGetMaterialTask where taskId like '%T%' and createTime like '%" + createTime + "%'");
                                            if (which == null)
                                                throw new Exception("领料任务第几次生产查询失败！");
                                            ascmGetMaterialTask.which = int.Parse(which.ToString());

                                            AscmGetMaterialTaskService.GetInstance().Save(ascmGetMaterialTask);//新建新任务

                                            ascmWipRequirementOperations.taskId = ascmGetMaterialTask.id;//添加新任务与Bom关联关系
                                            ascmWipRequirementOperations.wmsPreparationWarehouse = wmsAndLogistics.warehouseId;

                                            ascmWipRequirementOperations.wmsPreparationQuantity += wmsAndLogistics.quantity;
                                            if (!string.IsNullOrEmpty(ascmWipRequirementOperations.wmsPreparationString))
                                                ascmWipRequirementOperations.wmsPreparationString += ",";
                                            ascmWipRequirementOperations.wmsPreparationString += wmsAndLogistics.preparationString + "[" + wmsAndLogistics.quantity + "]";
                                            listRequirementOperations.Add(ascmWipRequirementOperations);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (listRequirementOperations != null && listRequirementOperations.Count > 0)
                    {
                        AscmWipRequirementOperationsService.GetInstance().Update(listRequirementOperations);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 备料撤销
        /// </summary>
        /// <param name="list">接口List</param>
        /// <returns>true:撤销成功，false:撤销失败</returns>
        public bool UnDoWmsPreparationInfo(List<WmsAndLogistics> list)
        {
            List<AscmWipRequirementOperations> listRequirementOperations = new List<AscmWipRequirementOperations>();
            try
            {
                if (list != null && list.Count > 0)
                {
                    foreach (WmsAndLogistics wmsAndLogistics in list)
                    {
                        string sql = "from AscmWipRequirementOperations";
                        string where = "", whereQueryWord = "";
                        if (!string.IsNullOrEmpty(wmsAndLogistics.wipEntityId.ToString()) && !string.IsNullOrEmpty(wmsAndLogistics.materialId.ToString()))
                        {
                            whereQueryWord = "wipEntityId =" + wmsAndLogistics.wipEntityId.ToString();
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                            whereQueryWord = "inventoryItemId = " + wmsAndLogistics.materialId.ToString();
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                            if (!string.IsNullOrEmpty(where))
                                sql += " where " + where;
                            IList<AscmWipRequirementOperations> ilistRequirementOperations = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql);
                            if (ilistRequirementOperations != null && ilistRequirementOperations.Count > 0)
                            {
                                List<AscmWipRequirementOperations> templist = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilistRequirementOperations);

                                sql = "from AscmGetMaterialTask";
                                where = "";
                                int nI = 0;
                                string ids = string.Empty;
                                foreach (AscmWipRequirementOperations ascmWipRequirementOperations in templist)
                                {
                                    if (!string.IsNullOrEmpty(ids))
                                        ids += ",";
                                    if (!string.IsNullOrEmpty(ascmWipRequirementOperations.taskId.ToString()) && ascmWipRequirementOperations.taskId != 0)
                                        ids += ascmWipRequirementOperations.taskId.ToString();
                                }
                                where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");

                                if (!string.IsNullOrEmpty(where))
                                {
                                    sql += " where " + where;
                                    IList<AscmGetMaterialTask> ilistAscmGetMaterialTask = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);

                                    if (ilistAscmGetMaterialTask != null && ilistAscmGetMaterialTask.Count > 0)
                                    {

                                        List<AscmGetMaterialTask> listAscmGetMaterialTask = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilistAscmGetMaterialTask);
                                        AscmGetMaterialTaskService.GetInstance().SumQuantity(listAscmGetMaterialTask);
                                        foreach (AscmGetMaterialTask ascmGetMaterialTask in listAscmGetMaterialTask)
                                        {
                                            if (ascmGetMaterialTask.totalGetMaterialQuantity > 0)
                                            {
                                                nI++;
                                            }
                                        }
                                    }
                                }

                                if (nI == 0)
                                {
                                    foreach (AscmWipRequirementOperations ascmWipRequirementOperations in templist)
                                    {
                                        ascmWipRequirementOperations.wmsPreparationQuantity -= wmsAndLogistics.quantity;
                                        string wmsQuantityString = wmsAndLogistics.preparationString + "[" + wmsAndLogistics.quantity + "]";
                                        if (!string.IsNullOrEmpty(ascmWipRequirementOperations.wmsPreparationString) && ascmWipRequirementOperations.wmsPreparationString.IndexOf(',') > -1)
                                        {
                                            wmsQuantityString = ascmWipRequirementOperations.wmsPreparationString.Replace(wmsQuantityString, "");
                                            if (!string.IsNullOrEmpty(wmsQuantityString))
                                            {
                                                string[] wmsQuantityArray = wmsQuantityString.Split(',');
                                                wmsQuantityString = "";
                                                foreach (string item in wmsQuantityArray)
                                                {
                                                    if (!string.IsNullOrEmpty(wmsQuantityString))
                                                        wmsQuantityString += ",";
                                                    if (!string.IsNullOrEmpty(item))
                                                        wmsQuantityString += item;
                                                }
                                            }
                                        }
                                        else if (!string.IsNullOrEmpty(ascmWipRequirementOperations.wmsPreparationString) && ascmWipRequirementOperations.wmsPreparationString.IndexOf(',') == -1)
                                        {
                                            wmsQuantityString = ascmWipRequirementOperations.wmsPreparationString.Replace(wmsQuantityString, "");
                                        }
                                        else if (string.IsNullOrEmpty(ascmWipRequirementOperations.wmsPreparationString))
                                        {
                                            wmsQuantityString = "";
                                        }
                                        ascmWipRequirementOperations.wmsPreparationString = wmsQuantityString;
                                        listRequirementOperations.Add(ascmWipRequirementOperations);
                                    }
                                }
                            }
                        }
                    }
                    if (listRequirementOperations != null && listRequirementOperations.Count > 0)
                    {
                        AscmWipRequirementOperationsService.GetInstance().Update(listRequirementOperations);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// 领料确认
        /// </summary>
        /// <param name="listWmsAndLogistics">接口List</param>
        public void DoMaterialRequisition(List<WmsAndLogistics> listWmsAndLogistics)
        {
            try
            {


                if (listWmsAndLogistics == null || listWmsAndLogistics.Count == 0)
                    return;

                //记录领料确认日志
                AscmWmsLogisticsMainLog wmsLogisticsMainLog = AscmWmsLogisticsMainLogService.GetInstance().Create();
                AscmWmsLogisticsMainLogService.GetInstance().Save(wmsLogisticsMainLog, listWmsAndLogistics);

                //获取备料主表ID
                string preparationMainIds = string.Join(",", listWmsAndLogistics.Select(P => P.preparationString));

                //获取备料明细
                List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetSetWipEntityNameList(preparationMainIds);
                if (listPreparationDetail == null || listPreparationDetail.Count == 0)
                {
                    wmsLogisticsMainLog.returnCode = AscmWmsLogisticsMainLog.ReturnCodeDefine.error;
                    wmsLogisticsMainLog.returnMessage = "获取备料明细失败";
                    AscmWmsLogisticsMainLogService.GetInstance().Update(wmsLogisticsMainLog);
                    return;
                }

                //获取作业、作业BOM
                string wipEntityIds = string.Join(",", listWmsAndLogistics.Select(P => P.wipEntityId).Distinct());
                List<AscmWipRequirementOperations> listBom = null;
                string whereWipEntityIds = AscmCommonHelperService.GetInstance().IsJudgeListCount(wipEntityIds, "wipEntityId");
                if (!string.IsNullOrEmpty(whereWipEntityIds))
                    listBom = AscmWipRequirementOperationsService.GetInstance().GetList("from AscmWipRequirementOperations where " + whereWipEntityIds);
                if (listBom == null || listBom.Count == 0)
                {
                    wmsLogisticsMainLog.returnCode = AscmWmsLogisticsMainLog.ReturnCodeDefine.error;
                    wmsLogisticsMainLog.returnMessage = "获取作业BOM失败";
                    AscmWmsLogisticsMainLogService.GetInstance().Update(wmsLogisticsMainLog);
                    return;
                }
                List<AscmWipRequirementOperations> listBomUpdate = new List<AscmWipRequirementOperations>();
                List<AscmWipDiscreteJobs> listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList("from AscmWipDiscreteJobs where " + whereWipEntityIds);
                if (listWipDiscreteJobs == null || listWipDiscreteJobs.Count == 0)
                {
                    wmsLogisticsMainLog.returnCode = AscmWmsLogisticsMainLog.ReturnCodeDefine.error;
                    wmsLogisticsMainLog.returnMessage = "获取作业失败";
                    AscmWmsLogisticsMainLogService.GetInstance().Update(wmsLogisticsMainLog);
                    return;
                }

                //存储生成的领料单
                List<AscmWmsMtlRequisitionMain> listRequisitionMain = new List<AscmWmsMtlRequisitionMain>();
                //按作业分组生成领料单主表（MES限制只能上传单个作业）
                var result = listWmsAndLogistics.GroupBy(P => P.wipEntityId);

                string maxIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_wms_mtl_req_main_id", "", "", 10, result.Count());
                int maxId = Convert.ToInt32(maxIdKey);
                int maxId_Detail = 0;
                foreach (IGrouping<int, WmsAndLogistics> ig in result)
                {
                    //生成领料单
                    AscmWmsMtlRequisitionMain requisitionMain = new AscmWmsMtlRequisitionMain();
                    requisitionMain.id = maxId++;
                    requisitionMain.organizationId = 775;
                    requisitionMain.docNumber = AscmMesService.GetInstance().GetMesRequisitionBillNo();  //获取MES领料单闭环单号
                    requisitionMain.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    requisitionMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    requisitionMain.status = AscmWmsMtlRequisitionMain.StatusDefine.failed;
                    requisitionMain.wipEntityId = ig.Key;
                    requisitionMain.workerId = ig.FirstOrDefault().workerId;
                    listRequisitionMain.Add(requisitionMain);

                    //存储更新了发料数量的备料明细
                    List<AscmWmsPreparationDetail> listPreparationDetailUpdate = new List<AscmWmsPreparationDetail>();
                    requisitionMain.listPreparationDetail = listPreparationDetailUpdate;

                    //按物料分组生成领料单明细
                    List<AscmWmsMtlRequisitionDetail> listRequisitionDetail = new List<AscmWmsMtlRequisitionDetail>();
                    requisitionMain.listDetail = listRequisitionDetail;

                    //领料明细与备料明细关联
                    List<AscmWmsMtlReqDetailLink> listReqDetailLink = new List<AscmWmsMtlReqDetailLink>();
                    requisitionMain.listReqDetailLink = listReqDetailLink;

                    var result2 = ig.GroupBy(P => P.materialId);
                    foreach (IGrouping<int, WmsAndLogistics> ig2 in result2)
                    {
                        //作业物料的领料数量
                        decimal requisitionQuantity = ig2.Sum(P => P.quantity);
                        if (requisitionQuantity > decimal.Zero)
                        {
                            //对应的备料单
                            var iePreparationMainId = string.Join(",", ig2.Select(P => P.preparationString)).Split(',').Distinct();
                            //对应的作业BOM
                            AscmWipRequirementOperations wipRequirementOperations = listBom.Find(P => P.wipEntityId == ig.Key && P.inventoryItemId == ig2.Key);
                            bool modify = false;
                            foreach (AscmWmsPreparationDetail preparationDetail in listPreparationDetail)
                            {
                                if (iePreparationMainId.Contains(preparationDetail.mainId.ToString())
                                    && preparationDetail.wipEntityId == ig.Key
                                    && preparationDetail.materialId == ig2.Key
                                    && preparationDetail.sendLogisticsQuantity > preparationDetail.issueQuantity)
                                {
                                    AscmWmsMtlRequisitionDetail requisitionDetail =
                                        listRequisitionDetail.Find(P => P.materialId == preparationDetail.materialId
                                            && P.warehouseId == preparationDetail.warehouseId
                                            && P.warelocationId == preparationDetail.warelocationId);
                                    if (requisitionDetail == null)
                                    {
                                        requisitionDetail = new AscmWmsMtlRequisitionDetail();
                                        requisitionDetail.id = ++maxId_Detail;
                                        requisitionDetail.mainId = requisitionMain.id;
                                        requisitionDetail.wipEntityName = preparationDetail.wipEntityName;
                                        requisitionDetail.materialId = preparationDetail.materialId;
                                        requisitionDetail.warehouseId = preparationDetail.warehouseId;
                                        requisitionDetail.warelocationId = preparationDetail.warelocationId;
                                        requisitionDetail.quantity = decimal.Zero;
                                        listRequisitionDetail.Add(requisitionDetail);
                                    }
                                    //发料数量=传递给物流领料模块数量-已发料数量
                                    decimal issueQuantity = preparationDetail.sendLogisticsQuantity - preparationDetail.issueQuantity;
                                    //实际发料数量
                                    decimal realIssueQuantity = requisitionQuantity > issueQuantity ? issueQuantity : requisitionQuantity;
                                    //领料明细领料数量
                                    requisitionDetail.quantity += realIssueQuantity;
                                    //更新备料明细发料数量
                                    preparationDetail.issueQuantity += realIssueQuantity;
                                    listPreparationDetailUpdate.Add(preparationDetail);

                                    AscmWmsMtlReqDetailLink reqDetailLink = new AscmWmsMtlReqDetailLink();
                                    reqDetailLink.reqDetailLinkPK = new AscmWmsMtlReqDetailLinkPK { reqDetailId = requisitionDetail.id, preDetailId = preparationDetail.id };
                                    reqDetailLink.quantity = realIssueQuantity;
                                    listReqDetailLink.Add(reqDetailLink);

                                    //更新BOM仓库发料数量
                                    if (wipRequirementOperations != null)
                                    {
                                        modify = true;
                                        wipRequirementOperations.ascmIssuedQuantity += realIssueQuantity;
                                    }

                                    requisitionQuantity -= realIssueQuantity;
                                    if (requisitionQuantity == decimal.Zero)
                                        break;
                                }
                            }
                            if (modify)
                                listBomUpdate.Add(wipRequirementOperations);
                        }
                    }
                }

                //设置领料明细ID
                string maxDetailIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_wms_mtl_req_detail_id", "", "", 10, maxId_Detail);
                int maxDetailId = Convert.ToInt32(maxDetailIdKey);
                string warelocationIds = string.Empty;
                List<int> listPreparationMainId = new List<int>();
                foreach (AscmWmsMtlRequisitionMain requisitionMain in listRequisitionMain)
                {
                    foreach (AscmWmsMtlRequisitionDetail requisitionDetail in requisitionMain.listDetail)
                    {
                        int detailId = requisitionDetail.id;
                        requisitionDetail.id = maxDetailId++;
                        requisitionMain.listReqDetailLink.FindAll(P => P.reqDetailLinkPK.reqDetailId == detailId).ForEach(P => P.reqDetailLinkPK.reqDetailId = requisitionDetail.id);
                        if (requisitionDetail.warelocationId > 0)
                        {
                            if (!string.IsNullOrEmpty(warelocationIds))
                                warelocationIds += ",";
                            warelocationIds += requisitionDetail.warelocationId;
                        }
                    }
                    if (requisitionMain.listPreparationDetail != null && requisitionMain.listPreparationDetail.Count > 0)
                        listPreparationMainId.AddRange(requisitionMain.listPreparationDetail.Select(P => P.mainId));
                }

                //获取备料单
                List<AscmWmsPreparationMain> listPreparationMain = null;
                if (listPreparationMainId.Count > 0)
                {
                    string hql = "from AscmWmsPreparationMain where id in(" + string.Join(",", listPreparationMainId.Distinct()) + ")";
                    listPreparationMain = AscmWmsPreparationMainService.GetInstance().GetList(hql);
                }

                //货位物料库存
                List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetListByWarelocationIds(warelocationIds);

                //记录上传MES日志
                List<AscmMesInteractiveLog> listMesInteractiveLog = new List<AscmMesInteractiveLog>();
                string maxLogIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_mes_interactive_log_id", "", "", 10, listRequisitionMain.Count);
                int maxLogId = Convert.ToInt32(maxLogIdKey);

                //上传MES
                foreach (AscmWmsMtlRequisitionMain requisitionMain in listRequisitionMain)
                {
                    //领料明细
                    List<AscmWmsMtlRequisitionDetail> listRequisitionDetail = requisitionMain.listDetail;
                    if (listRequisitionDetail.Count == 0)
                        continue;

                    //需要更新的备料明细
                    List<AscmWmsPreparationDetail> listPreparationDetailUpdate = requisitionMain.listPreparationDetail;

                    //领料单与备料单关联
                    List<AscmWmsMtlReqMainLink> listReqMainLink = new List<AscmWmsMtlReqMainLink>();
                    List<AscmWmsMtlReqDetailLink> listReqDetailLink = requisitionMain.listReqDetailLink;

                    //更新备料单状态
                    List<AscmWmsPreparationMain> listPreparationMainUpdate = null;
                    if (listPreparationMain != null && listPreparationMain.Count > 0 && listPreparationDetailUpdate != null && listPreparationDetailUpdate.Count > 0)
                    {
                        List<AscmWmsPreparationMain> _listPreparationMain = listPreparationMain.FindAll(P => listPreparationDetailUpdate.Select(T => T.mainId).Contains(P.id));
                        if (_listPreparationMain != null && _listPreparationMain.Count > 0)
                        {
                            //设置领料单创建人和手工单号
                            requisitionMain.createUser = _listPreparationMain.First().createUser;
                            requisitionMain.manualDocNumber = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("AscmWmsMtlRequisitionMain", requisitionMain.createUser, "yyyyMMdd", 4);

                            listPreparationMainUpdate = new List<AscmWmsPreparationMain>();
                            foreach (AscmWmsPreparationMain preparationMain in _listPreparationMain)
                            {
                                string previousStatus = preparationMain.status;
                                if (previousStatus == AscmWmsPreparationMain.StatusDefine.preparedUnPick)
                                {
                                    preparationMain.status = AscmWmsPreparationMain.StatusDefine.picked;
                                    //2014-3-25 如果备料单中存在未领完的物料，则单据状态置为“已备齐_待领料”
                                    var details = listPreparationDetail.Where(P => P.mainId == preparationMain.id);
                                    if (details != null && details.Count() > 0)
                                    {
                                        foreach (AscmWmsPreparationDetail detail in details)
                                        {
                                            var bom = listBom.Find(P => P.wipEntityId == detail.wipEntityId && P.inventoryItemId == detail.materialId);
                                            if (bom != null && bom.ascmIssuedQuantity < bom.requiredQuantity)
                                            {
                                                preparationMain.status = AscmWmsPreparationMain.StatusDefine.preparedUnPick;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (previousStatus == AscmWmsPreparationMain.StatusDefine.preparingUnPick)
                                {
                                    preparationMain.status = AscmWmsPreparationMain.StatusDefine.preparingUnConfirm;
                                }
                                if (previousStatus != preparationMain.status)
                                    listPreparationMainUpdate.Add(preparationMain);

                                AscmWmsMtlReqMainLink reqMainLink = new AscmWmsMtlReqMainLink();
                                reqMainLink.reqMainLinkPK = new AscmWmsMtlReqMainLinkPK { reqMainId = requisitionMain.id, preMainId = preparationMain.id };
                                listReqMainLink.Add(reqMainLink);
                            }
                        }
                    }

                    //创建上传MES日志
                    AscmMesInteractiveLog mesInteractiveLog = new AscmMesInteractiveLog();
                    mesInteractiveLog.id = maxLogId++;
                    mesInteractiveLog.billId = requisitionMain.id;
                    mesInteractiveLog.docNumber = requisitionMain.docNumber;
                    mesInteractiveLog.billType = AscmMesInteractiveLog.BillTypeDefine.mtlRequisition;
                    mesInteractiveLog.createUser = requisitionMain.createUser;
                    mesInteractiveLog.modifyUser = requisitionMain.createUser;
                    mesInteractiveLog.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    mesInteractiveLog.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    listMesInteractiveLog.Add(mesInteractiveLog);

                    //减少货位物料库存
                    List<AscmLocationMaterialLink> listLocationMaterialLinkUpdate = null;
                    if (listLocationMaterialLink != null && listLocationMaterialLink.Count > 0)
                    {
                        listLocationMaterialLinkUpdate = new List<AscmLocationMaterialLink>();
                        foreach (AscmWmsMtlRequisitionDetail requisitionDetail in listRequisitionDetail)
                        {
                            AscmLocationMaterialLink locationMaterialLink = listLocationMaterialLink.Find(P => P.pk.warelocationId == requisitionDetail.warelocationId && P.pk.materialId == requisitionDetail.materialId);
                            if (locationMaterialLink != null)
                            {
                                locationMaterialLink.modifyUser = requisitionMain.createUser;
                                locationMaterialLink.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                locationMaterialLink.quantity -= requisitionDetail.quantity;
                                listLocationMaterialLinkUpdate.Add(locationMaterialLink);
                            }
                            else
                            {
                                requisitionDetail.returnCode = -1;
                                requisitionDetail.returnMessage = "获取货位失败";
                            }
                        }
                    }

                    //更新作业BOM发料数量
                    List<AscmWipRequirementOperations> _listBom = listBomUpdate.FindAll(P => P.wipEntityId == requisitionMain.wipEntityId);
                    //更新作业备料状态
                    AscmWipDiscreteJobs wipDiscreteJobs = listWipDiscreteJobs.Find(P => P.wipEntityId == requisitionMain.wipEntityId);
                    //List<AscmWipDiscreteJobsStatus> listStatus = null;
                    if (wipDiscreteJobs != null)
                    {
                        //2014-3-25 如果作业下的备料单状态存在“已备齐_待领料”，则作业状态置为“待领料”
                        bool isUnPick = false;
                        foreach (AscmWmsPreparationMain main in listPreparationMain)
                        {
                            if (main.status != AscmWmsPreparationMain.StatusDefine.preparedUnPick)
                                continue;

                            isUnPick = listPreparationDetail.Where(P => P.mainId == main.id).Select(P => P.wipEntityId).Contains(wipDiscreteJobs.wipEntityId);
                            break;
                        }

                        //旧的逻辑
                        if (isUnPick)
                            wipDiscreteJobs.ascmStatus = AscmWipDiscreteJobs.AscmStatusDefine.unPick;
                        else if (_listBom.Exists(P => P.ascmIssuedQuantity < P.requiredQuantity))
                            wipDiscreteJobs.ascmStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing;
                        else
                            wipDiscreteJobs.ascmStatus = AscmWipDiscreteJobs.AscmStatusDefine.picked;
                    }

                    //执行事务
                    ISession session = null;
                    session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                    session.Clear();
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        try
                        {
                            AscmMesService.GetInstance().DoMtlRequisition(requisitionMain, listRequisitionDetail, requisitionMain.createUser, mesInteractiveLog);
                            if (mesInteractiveLog.returnCode == "0")
                            {
                                mesInteractiveLog.returnMessage = "领料成功";
                                requisitionMain.status = AscmWmsMtlRequisitionMain.StatusDefine.succeeded;
                            }

                            //添加领料单
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(requisitionMain);
                            //添加领料单明细
                            YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listRequisitionDetail);
                            //添加领料单与备料单关联
                            YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listReqMainLink);
                            //添加领料明细与备料明细关联
                            YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listReqDetailLink);
                            //更新货位物料库存
                            if (listLocationMaterialLinkUpdate != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listLocationMaterialLinkUpdate);
                            //更新备料单状态
                            if (listPreparationMainUpdate != null && listPreparationMainUpdate.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listPreparationMainUpdate);
                            //更新备料单明细发料数量
                            if (listPreparationDetailUpdate != null)
                                listPreparationDetailUpdate.ForEach(P => session.Merge(P));
                            //更新作业备料状态
                            if (wipDiscreteJobs != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.Update(wipDiscreteJobs);
                            //更新作业备料状态--明细状态
                            //if (listStatus != null && listStatus.Count>0)
                            //    YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listStatus);
                            //更新作业BOM发料数量
                            if (_listBom != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(_listBom);

                            tx.Commit();
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();
                            mesInteractiveLog.returnCode = "-1";
                            mesInteractiveLog.returnMessage = ex.Message;
                            YnBaseClass2.Helper.LogHelper.GetLog().Error("领料失败(Find AscmWmsMtlRequisitionMain)", ex);
                        }
                    }

                    //新的作业状态逻辑，保存到明细状态
                    List<AscmWipDiscreteJobsStatus> listStatus = AscmWipDiscreteJobsService.Instance.GetListByStrWhere(string.Format(" AND wipEntityId IN ({0}) ", wipEntityIds));
                    List<AscmWmsPreparationMain> mylistPreparationMain = AscmWmsPreparationMainService.GetInstance().GetList(string.Format(" from AscmWmsPreparationMain where wipEntityId in ({0}) ", wipEntityIds));
                    List<AscmWhTeamUser> listWhUser = AscmWhTeamUserService.Instance.GetList();

                    if (listStatus != null && listStatus.Count > 0 && listWhUser != null && listWhUser.Count > 0 && mylistPreparationMain != null)
                    {
                        foreach (var jobStatus in listStatus)
                        {
                            List<string> myTeamUserIds = null;
                            AscmWhTeamUser whTeamLeader = listWhUser.FirstOrDefault(p => p.M_UserId == jobStatus.leaderId);
                            if (whTeamLeader != null) myTeamUserIds = listWhUser.Where(p => p.M_TeamId == whTeamLeader.M_TeamId).Select(p => p.M_UserId).ToList();
                            if (myTeamUserIds == null) continue;

                            List<AscmWmsPreparationMain> subListPreparationMain
                                = mylistPreparationMain.Where(p => p.wipEntityId == jobStatus.wipEntityId && myTeamUserIds.Contains(p.createUser)).ToList();
                            if (subListPreparationMain == null || subListPreparationMain.Count == 0) continue;

                            int prepMainTotal = subListPreparationMain.Count;
                            int pickedCount = 0;
                            int un_pickedCount = 0;
                            foreach (var prepMain in subListPreparationMain)
                            {
                                if (prepMain.status == AscmWmsPreparationMain.StatusDefine.picked)
                                {
                                    pickedCount++;
                                }

                                if (prepMain.status == AscmWmsPreparationMain.StatusDefine.preparedUnPick ||
                                    prepMain.status == AscmWmsPreparationMain.StatusDefine.preparingUnPick)
                                {
                                    un_pickedCount++;
                                }
                            }

                            if (pickedCount == prepMainTotal)
                            {
                                jobStatus.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.picked;
                            }
                            else if (un_pickedCount == prepMainTotal)
                            {
                                jobStatus.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.unPick;
                            }
                            else
                            {
                                jobStatus.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing;
                            }
                        }
                    }

                    session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                    session.Clear();
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        try
                        {
                            //更新作业备料状态--明细状态
                            if (listStatus != null && listStatus.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listStatus);
                            }
                            tx.Commit();
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();
                        }
                    }
                }

                //保存上传MES日志
                AscmMesInteractiveLogService.GetInstance().Save(listMesInteractiveLog);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw ex;
            }
        }
    }
}
