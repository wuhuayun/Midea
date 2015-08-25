using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MideaAscm.Dal.Base.Entities;
using YnBaseClass2.Web;
using MideaAscm.Services.Base;
using MideaAscm.Dal;
using Newtonsoft.Json;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Services.FromErp;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Services.SupplierPreparation;

namespace MideaAscm.Areas.Ascm.Controllers
{
    public class PurchaseController : YnFrame.Controllers.YnBaseController
    {
        //
        // GET: /Ascm/Purchase/
        //采购管理
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PreparationMonitorIndex()
        {
            //供应商备料状态实时监控
            return View();
        }

        #region 物料备料送货实时监控
        public ActionResult MaterialMonitorIndex()
        {
            //物料送货状态实时监控
            return View();
        }
        public ActionResult MaterialMonitorList(int? page, int? rows, string sort, string order, string queryWord,
            int? supplierId, string warehouse, string startCreateTime, string endCreateTime, string status, int? employeeBuyer)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDeliveryOrderBatch> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "", whereWarehouse = "", whereEmployeeBuyer = "";
                string whereStartCreateTime = "", whereEndCreateTime = "", whereStatus = "";

                if (supplierId.HasValue)
                    whereSupplier = "supplierId=" + supplierId.Value;
                if (!string.IsNullOrEmpty(warehouse))
                    whereWarehouse = "warehouseId = '" + warehouse + "'";
                //if (!string.IsNullOrEmpty(status))
                whereStatus = "status = '" + MideaAscm.Dal.FromErp.Entities.AscmDeliveryOrderMain.StatusDefine.open + "'";

                DateTime dtStartCreateTime, dtEndCreateTime;
                if (!string.IsNullOrEmpty(startCreateTime) && DateTime.TryParse(startCreateTime, out dtStartCreateTime))
                    whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endCreateTime) && DateTime.TryParse(endCreateTime, out dtEndCreateTime))
                    whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                if (employeeBuyer.HasValue)
                    whereEmployeeBuyer = "id in(select batchId from AscmDeliveryOrderMain where id in(select mainId from AscmDeliveryOrderDetail where materialId in(select id from AscmMaterialItem where buyerId=" + employeeBuyer + ")))";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEmployeeBuyer);

                list = AscmDeliveryOrderBatchService.GetInstance().GetMonitorList(ynPage, "", "", queryWord, whereOther);
                if (list != null)
                {
                    foreach (AscmDeliveryOrderBatch ascmDeliveryOrderBatch in list)
                    {
                        jsonDataGridResult.rows.Add(ascmDeliveryOrderBatch.GetOwner());
                    }
                    jsonDataGridResult.total = ynPage.GetRecordCount();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 供应商
        public ActionResult SupplierIndex()
        {
            //供应商管理
            return View();
        }
        public ActionResult SupplierList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmSupplier> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmSupplierService.GetInstance().GetList(ynPage, "", "", queryWord, null);
                foreach (AscmSupplier ascmSupplier in list)
                {
                    jsonDataGridResult.rows.Add(ascmSupplier.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SupplierEdit(int? id)
        {
            AscmSupplier ascmSupplier = null;
            try
            {
                if (id.HasValue)
                {
                    ascmSupplier = AscmSupplierService.GetInstance().Get(id.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmSupplier.GetOwner(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult SupplierSave(AscmSupplier ascmSupplier_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                AscmSupplier ascmSupplier = null;
                if (id.HasValue)
                {
                    ascmSupplier = AscmSupplierService.GetInstance().Get(id.Value);
                }
                else
                {
                    ascmSupplier = new AscmSupplier();
                    throw new Exception("不允许增加供应商！");
                }
                if (ascmSupplier == null)
                    throw new Exception("保存供应商失败！");
                if (ascmSupplier_Model.name == null || ascmSupplier_Model.name.Trim() == "")
                    throw new Exception("供应商名称不能为空！");

                ascmSupplier.warnHours = ascmSupplier_Model.warnHours;
                //ascmSupplier.name = ascmSupplier_Model.name;
                //ascmSupplier.enabled = ascmSupplier_Model.enabled;
                //ascmSupplier.description = ascmSupplier_Model.description;
                ascmSupplier.passDuration = ascmSupplier_Model.passDuration;
                if (id.HasValue)
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmSupplier where name='" + ascmSupplier_Model.name.Trim() + "' and id<>" + id.Value + "");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已经存在此供应商【" + ascmSupplier_Model.name.Trim() + "】！");
                    AscmSupplierService.GetInstance().Update(ascmSupplier);

                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmSupplier.id.ToString();
                jsonObjectResult.entity = ascmSupplier;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult SupplierAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmSupplier> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmSupplierService.GetInstance().GetList(ynPage, "", "", q,null);
                foreach (AscmSupplier ascmSupplier in list)
                {
                    jsonDataGridResult.rows.Add(ascmSupplier.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDefaultSupplierAddress(int? id)
        {
            AscmSupplierAddress ascmSupplierAddress = null;
            try
            {
                if (id.HasValue)
                {
                    List<AscmSupplierAddress> listAscmSupplierAddress = AscmSupplierAddressService.GetInstance().GetList(id.Value);
                    if (listAscmSupplierAddress != null && listAscmSupplierAddress.Count > 0)
                        ascmSupplierAddress = listAscmSupplierAddress[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ascmSupplierAddress = ascmSupplierAddress ?? new AscmSupplierAddress();
            return Json(ascmSupplierAddress.GetOwner(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SupplierAddressList(int? id)
        {
            List<AscmSupplierAddress> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (id.HasValue)
                {
                    list = AscmSupplierAddressService.GetInstance().GetList(id.Value);
                    foreach (AscmSupplierAddress ascmSupplierAddress in list)
                    {
                        jsonDataGridResult.rows.Add(ascmSupplierAddress.GetOwner());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 送货通知
        public ActionResult AppointmentDeliveryIndex()
        {
            //预约送货
            YnFrame.Dal.Entities.YnUser ynUser = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketUserData();
            if (ynUser == null)
                throw new Exception("用户错误！");
            AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(ynUser.userId);
            ViewData["employeeId"] = "";
            if (ascmUserInfo.extExpandType == "erp" && ascmUserInfo.employeeId > 0)
            {
                ascmUserInfo.ascmEmployee = AscmEmployeeService.GetInstance().Get(ascmUserInfo.employeeId);
                ViewData["employeeId"] = ascmUserInfo.employeeId;
            }

            return View();
        }
        /*public ActionResult AppointmentDeliveryMainList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDeliveryNotifyMain> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmDeliveryNotifyMainService.GetInstance().GetList(ynPage, "", "", queryWord,"");
                foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                {
                    jsonDataGridResult.rows.Add(ascmDeliveryNotifyMain.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }*/
        public ActionResult DeliveryNotifyQuery()
        {
            YnFrame.Dal.Entities.YnUser ynUser = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketUserData();
            if (ynUser == null)
                throw new Exception("用户错误！");
            AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(ynUser.userId);
            ViewData["employeeId"] = "";
            if (ascmUserInfo.extExpandType == "erp" && ascmUserInfo.employeeId>0)
            {
                ascmUserInfo.ascmEmployee = AscmEmployeeService.GetInstance().Get(ascmUserInfo.employeeId);
                ViewData["employeeId"] = ascmUserInfo.employeeId;
            }
            //送货通知
            return View();
        }
        public ActionResult DeliveryNotifyMainList(int? page, int? rows, string sort, string order, string queryWord,
            string supplier, string materialItem, string warehouse, string alert, string filter, string startReleasedTime, string endReleasedTime, string startNeedTime, string endNeedTime, string status, string employeeBuyer, string wipEntityIds)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDeliveryNotifyMain> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "", whereMaterialItem = "", whereWarehouse = "", whereAlert = "", whereFilter = "", whereStartReleaseTime = "", whereEndReleaseTime = "";
                string whereStartNeedTime = "", whereEndNeedTime = "", whereStatus = "", whereEmployeeBuyer = "", whereWipEntityIds = "";
                if (!string.IsNullOrEmpty(supplier))
                    whereSupplier = "a.supplierId=" + supplier;
                if (!string.IsNullOrEmpty(materialItem))
                    whereMaterialItem = "a.materialId= " + materialItem;
                if (!string.IsNullOrEmpty(warehouse))
                    whereWarehouse = "a.warehouseId = '" + warehouse + "'";
                if (!string.IsNullOrEmpty(alert))
                {
                    string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    if (alert == AscmDeliveryNotifyMain.AlertDefine.alert)
                    {
                        whereAlert = "a.appointmentStartTime <='" + datetime + "' and a.appointmentEndTime >= '" + datetime + "'";
                    }
                    if (alert == AscmDeliveryNotifyMain.AlertDefine.overdue)
                    {
                        whereAlert = "a.appointmentEndTime < '" + datetime + "'";
                    }
                }
                //if (!string.IsNullOrEmpty(filter))
                //{
                //    whereFilter = "e.receivedQuantity < a.releasedquantity";
                //}

                if (!string.IsNullOrEmpty(filter))
                    whereFilter = "a.totalReceiveQuantity < a.releasedQuantity";

                if (!string.IsNullOrEmpty(wipEntityIds))
                    whereWipEntityIds = "a.wipEntityId > 0";

                if (!string.IsNullOrEmpty(status))
                    whereStatus = "status = '" + status + "'";

                if (!string.IsNullOrEmpty(employeeBuyer))
                    whereEmployeeBuyer = "a.materialId in (select id from Ascm_Material_Item where Ascm_Material_Item.buyerId=" + employeeBuyer + ")";
                
                DateTime dtStartReleasedTime,dtEndReleasedTime;
                if (!string.IsNullOrEmpty(startReleasedTime) && DateTime.TryParse(startReleasedTime, out dtStartReleasedTime))
                    whereStartReleaseTime = "a.releasedTime>='" + dtStartReleasedTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endReleasedTime) && DateTime.TryParse(endReleasedTime, out dtEndReleasedTime))
                    whereEndReleaseTime = "a.releasedTime<'" + dtEndReleasedTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                DateTime dtStartNeedTime, dtEndNeedTime;
                if (!string.IsNullOrEmpty(startNeedTime) && DateTime.TryParse(startNeedTime, out dtStartNeedTime))
                    whereStartNeedTime = "a.needTime>='" + dtStartNeedTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endReleasedTime) && DateTime.TryParse(endNeedTime, out dtEndNeedTime))
                    whereEndNeedTime = "a.needTime<'" + dtEndNeedTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereMaterialItem);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereAlert);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereFilter);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartReleaseTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndReleaseTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartNeedTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndNeedTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEmployeeBuyer);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWipEntityIds);

                list = AscmDeliveryNotifyMainService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                {
                    jsonDataGridResult.rows.Add(ascmDeliveryNotifyMain.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeliveryNotifyDetailList(int? id)
        {
            List<AscmDeliveryNotifyDetail> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (id.HasValue)
                {
                    list = AscmDeliveryNotifyDetailService.GetInstance().GetList(id.Value);
                    foreach (AscmDeliveryNotifyDetail ascmDeliveryNotifyDetail in list)
                    {
                        jsonDataGridResult.rows.Add(ascmDeliveryNotifyDetail.GetOwner());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeliveryNotifyMainEdit(int? id)
        {
            AscmDeliveryNotifyMain ascmDeliveryNotifyMain = null;
            try
            {
                if (id.HasValue)
                {
                    ascmDeliveryNotifyMain = AscmDeliveryNotifyMainService.GetInstance().Get(id.Value);
                    //ascmDeliveryNotifyMain.ynDepartment = YnFrame.Services.YnDepartmentService.GetInstance().Get(ascmEmployee.departmentId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmDeliveryNotifyMain.GetOwner(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult DeliveryNotifyMainSave(AscmDeliveryNotifyMain ascmDeliveryNotifyMain_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                AscmDeliveryNotifyMain ascmDeliveryNotifyMain = null;
                if (id.HasValue)
                {
                    ascmDeliveryNotifyMain = AscmDeliveryNotifyMainService.GetInstance().Get(id.Value);
                }
                YnFrame.Dal.Entities.YnUser ynUser = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketUserData();
                if (ynUser == null)
                    throw new Exception("用户错误！");
                AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(ynUser.userId);
                if (ascmUserInfo.extExpandType == "erp" && ascmUserInfo.employeeId > 0)
                {
                    ascmUserInfo.ascmEmployee = AscmEmployeeService.GetInstance().Get(ascmUserInfo.employeeId);
                }
                else
                {
                    throw new Exception("当前用户不能提交修改！");
                }

                if (ascmDeliveryNotifyMain == null)
                    throw new Exception("保存预约信息失败！");

                ascmDeliveryNotifyMain.ascmMaterialItem = AscmMaterialItemService.GetInstance().Get(ascmDeliveryNotifyMain.materialId);
                if (ascmDeliveryNotifyMain.ascmMaterialItem==null)
                    throw new Exception("送货通知没有关联物料！");
                if (ascmDeliveryNotifyMain.ascmMaterialItem.buyerId != ascmUserInfo.employeeId)
                    throw new Exception("不能修改其他采购员的送货通知！");

                //if (ascmDeliveryNotifyMain_Model.name == null || ascmDeliveryNotifyMain_Model.name.Trim() == "")
                    //throw new Exception("....不能为空！");
                if(ascmDeliveryNotifyMain.status!=AscmDeliveryNotifyMain.StatusDefine.confirm)
                    throw new Exception("送货通知已经【" + ascmDeliveryNotifyMain.statusCn + "】！");
                DateTime appointmentStartTime = DateTime.Now;
                if (DateTime.TryParse(ascmDeliveryNotifyMain_Model.appointmentStartTime, out appointmentStartTime))
                {
                    ascmDeliveryNotifyMain.appointmentStartTime = appointmentStartTime.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    throw new Exception("预约开始送货时间格式错误！");
                }
                DateTime appointmentEndTime = DateTime.Now;
                if (DateTime.TryParse(ascmDeliveryNotifyMain_Model.appointmentEndTime, out appointmentEndTime))
                {
                    ascmDeliveryNotifyMain.appointmentEndTime = appointmentEndTime.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    throw new Exception("预约截止送货时间格式错误！");
                }
                System.TimeSpan ts = DateTime.Now.Subtract(appointmentStartTime);
                if (ts.Minutes >= 0)
                    throw new Exception("预约开始时间应在现在时间之后！");
                ts = appointmentEndTime.Subtract(appointmentStartTime);
                if(ts.Minutes<=0)
                    throw new Exception("预约开始时间应在截止时间之前！");
                //ascmDeliveryNotifyMain.sex = ascmDeliveryNotifyMain_Model.sex.Trim();

                AscmDeliveryNotifyMainService.GetInstance().Update(ascmDeliveryNotifyMain);


                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmDeliveryNotifyMain.id.ToString();
                jsonObjectResult.entity = ascmDeliveryNotifyMain;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        #endregion

        #region 送货单
        public ActionResult DeliveryOrderQuery()
        {
            //送货单统计和单据
            return View();
        }
        public ActionResult DeliveryOrderMainList(int? page, int? rows, string sort, string order, string queryWord,
            string supplier, string warehouse, string startDeliveryTime, string endDeliveryTime, string status)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDeliveryOrderMain> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "", whereWarehouse="";
                string whereStartDeliveryTime = "", whereEndDeliveryTime = "", whereStatus = "";

                if (!string.IsNullOrEmpty(supplier))
                    whereSupplier = "supplierId=" + supplier;
                if (!string.IsNullOrEmpty(warehouse))
                    whereWarehouse = "warehouseId = '" + warehouse + "'"; 
                if (!string.IsNullOrEmpty(status))
                    whereStatus = "status = '" + status + "'";

                DateTime dtStartDeliveryTime, dtEndDeliveryTime;
                if (!string.IsNullOrEmpty(startDeliveryTime) && DateTime.TryParse(startDeliveryTime, out dtStartDeliveryTime))
                    whereStartDeliveryTime = "deliveryTime>='" + dtStartDeliveryTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endDeliveryTime) && DateTime.TryParse(endDeliveryTime, out dtEndDeliveryTime))
                    whereEndDeliveryTime = "deliveryTime<'" + dtEndDeliveryTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartDeliveryTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndDeliveryTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereMaterialItem);

                list = AscmDeliveryOrderMainService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                foreach (AscmDeliveryOrderMain ascmDeliveryOrderMain in list)
                {
                    jsonDataGridResult.rows.Add(ascmDeliveryOrderMain.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeliveryOrderDetailList(int? id)
        {
            List<AscmDeliveryOrderDetail> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (id.HasValue)
                {
                    list = AscmDeliveryOrderDetailService.GetInstance().GetList(id.Value);
                    foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in list)
                    {
                        jsonDataGridResult.rows.Add(ascmDeliveryOrderDetail.GetOwner());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 批送货单
        public ActionResult DeliveryOrderBatchQuery()
        {
            //送货单统计和单据
            return View();
        }
        public ActionResult DeliveryOrderBatchList(int? page, int? rows, string sort, string order, string queryWord,
            string supplier, string warehouse, string startCreateTime, string endCreateTime, string status)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDeliveryOrderBatch> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "", whereWarehouse = "";
                string whereStartCreateTime = "", whereEndCreateTime = "", whereStatus = "";

                if (!string.IsNullOrEmpty(supplier))
                    whereSupplier = "supplierId=" + supplier;
                if (!string.IsNullOrEmpty(warehouse))
                    whereWarehouse = "warehouseId = '" + warehouse + "'";
                if (!string.IsNullOrEmpty(status))
                    whereStatus = "status = '" + status + "'";

                DateTime dtStartCreateTime, dtEndCreateTime;
                if (!string.IsNullOrEmpty(startCreateTime) && DateTime.TryParse(startCreateTime, out dtStartCreateTime))
                    whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endCreateTime) && DateTime.TryParse(endCreateTime, out dtEndCreateTime))
                    whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereMaterialItem);

                list = AscmDeliveryOrderBatchService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                foreach (AscmDeliveryOrderBatch ascmDeliveryOrderBatch in list)
                {
                    jsonDataGridResult.rows.Add(ascmDeliveryOrderBatch.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeliveryOrderBatchDetailList(int? id)
        {
            List<AscmDeliveryOrderDetail> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (id.HasValue)
                {
                    list = AscmDeliveryOrderDetailService.GetInstance().GetListByBatch(id.Value);
                    foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in list)
                    {
                        jsonDataGridResult.rows.Add(ascmDeliveryOrderDetail.GetOwner());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeliveryOrderBatchNotInBatchSumList(int? page, int? rows, string sort, string order, string queryWord,
            string supplier, string warehouse, string startCreateTime, string endCreateTime, string status, string ascmStatus)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "", whereWarehouse = "", whereAscmStatus = "";
                string whereStartCreateTime = "", whereEndCreateTime = "", whereStatus = "";
                string whereNotInSum = " id not in(select batchId from AscmDeliBatSumDetail where mainId in(select id from AscmDeliBatSumMain where status='" + AscmDeliBatSumMain.StatusDefine.unConfirm + "'))";

                if (!string.IsNullOrEmpty(supplier))
                    whereSupplier = "supplierId=" + supplier;
                if (!string.IsNullOrEmpty(warehouse))
                    whereWarehouse = "warehouseId = '" + warehouse + "'";
                if (!string.IsNullOrEmpty(status))
                    whereStatus = "status = '" + status + "'";

                DateTime dtStartCreateTime, dtEndCreateTime;
                if (!string.IsNullOrEmpty(startCreateTime) && DateTime.TryParse(startCreateTime, out dtStartCreateTime))
                    whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endCreateTime) && DateTime.TryParse(endCreateTime, out dtEndCreateTime))
                    whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(ascmStatus))
                {
                    if (ascmStatus == "INSTORAGE")
                        whereAscmStatus = "ascmStatus='" + ascmStatus + "'";
                    else
                        whereAscmStatus = "ascmStatus!='INSTORAGE'  or ascmStatus is null";
                }

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereNotInSum);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereAscmStatus);

                List<AscmDeliveryOrderBatch> list = AscmDeliveryOrderBatchService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                if (list != null)
                {
                    foreach (AscmDeliveryOrderBatch ascmDeliveryOrderBatch in list)
                    {
                        jsonDataGridResult.rows.Add(ascmDeliveryOrderBatch.GetOwner());
                    }
                    jsonDataGridResult.total = ynPage.GetRecordCount();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeliveryOrderBatchAscxList(int? page, int? rows, string sort, string order, string queryWord,
            string supplier, string warehouse, string startCreateTime, string endCreateTime, string status, string ascmStatus)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "", whereWarehouse = "", whereBatSumMain = "", whereAscmStatus = "";
                string whereStartCreateTime = "", whereEndCreateTime = "", whereStatus = "";

                if (!string.IsNullOrEmpty(supplier))
                    whereSupplier = "supplierId=" + supplier;
                if (!string.IsNullOrEmpty(warehouse))
                    whereWarehouse = "warehouseId = '" + warehouse + "'";
                if (!string.IsNullOrEmpty(status))
                    whereStatus = "status = '" + status + "'";

                DateTime dtStartCreateTime, dtEndCreateTime;
                if (!string.IsNullOrEmpty(startCreateTime) && DateTime.TryParse(startCreateTime, out dtStartCreateTime))
                    whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endCreateTime) && DateTime.TryParse(endCreateTime, out dtEndCreateTime))
                    whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(ascmStatus))
                {
                    if (ascmStatus == "INSTORAGE")
                        whereAscmStatus = "ascmStatus='" + ascmStatus + "'";
                    else
                        whereAscmStatus = "ascmStatus!='INSTORAGE'  or ascmStatus is null";
                }

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereBatSumMain);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereAscmStatus);

                List<AscmDeliveryOrderBatch> list = AscmDeliveryOrderBatchService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                if (list != null)
                {
                    AscmDeliveryOrderBatchService.GetInstance().SetDeliveryNotifyMain(list);
                    foreach (AscmDeliveryOrderBatch ascmDeliveryOrderBatch in list)
                    {
                        jsonDataGridResult.rows.Add(ascmDeliveryOrderBatch.GetOwner());
                    }
                    jsonDataGridResult.total = ynPage.GetRecordCount();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeliveryOrderBatchAscxList1(int? page, int? rows, string sort, string order, string queryWord,
           string supplier, string warehouse, string startCreateTime, string endCreateTime, string status, string ascmStatus,
            string startConfirmDate, string endConfirmDate, string startNotifyDate, string endNotifyDate, string startMaterialDocNumber, string endMaterialDocNumber,
            string appointmentStartTime, string appointmentEndTime, string queryFilter, bool? isSupplierSelect, int? supplierDeliveryBatSumId, bool appointmentTimeFilter)
        {

            //isSupplierSelect是否供应商选择批单方式，如果是则不显示已生成的批单
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "", whereWarehouse = "", whereAscmStatus = "";//whereBatSumMain = "", 
                string whereStartCreateTime = "", whereEndCreateTime = "", whereStatus = "";

                if (!string.IsNullOrEmpty(supplier) && supplier != "null")
                    whereSupplier = "supplierId=" + supplier;
                if (!string.IsNullOrEmpty(warehouse) && warehouse != "null")
                    whereWarehouse = "warehouseId = '" + warehouse + "'";
                if (!string.IsNullOrEmpty(status))
                    whereStatus = "status = '" + status + "'";

                DateTime dtStartCreateTime, dtEndCreateTime;
                if (!string.IsNullOrEmpty(startCreateTime) && DateTime.TryParse(startCreateTime, out dtStartCreateTime))
                    whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd HH:mm:00") + "'";
                if (!string.IsNullOrEmpty(endCreateTime) && DateTime.TryParse(endCreateTime, out dtEndCreateTime))
                    //whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                    whereEndCreateTime = "createTime<='" + dtEndCreateTime.ToString("yyyy-MM-dd HH:mm:00") + "'";


                if (!string.IsNullOrEmpty(ascmStatus))
                {
                    if (ascmStatus == "INSTORAGE")
                        if (ascmStatus == AscmDeliveryOrderBatch.AscmStatusDefine.inStorage)
                            whereAscmStatus = "ascmStatus='" + ascmStatus + "'";
                        else
                            whereAscmStatus = " ascmStatus is null or ascmStatus<>'" + AscmDeliveryOrderBatch.AscmStatusDefine.inStorage + "'";
                }

                #region 物料编码
                string whereStartMaterialId = "", whereEndMaterialId = "";
                if (startMaterialDocNumber != null && startMaterialDocNumber != "")
                {
                    whereStartMaterialId = "id in (select batchId from AscmDeliveryOrderMain where id in(select mainId from AscmDeliveryOrderDetail where materialId in(select id from AscmMaterialItem where docNumber='" + startMaterialDocNumber.Trim() + "')))";
                }
                if (endMaterialDocNumber != null && endMaterialDocNumber != "")
                {
                    if (startMaterialDocNumber != null && startMaterialDocNumber != "")
                    {
                        whereStartMaterialId = "id in (select batchId from AscmDeliveryOrderMain where id in(select mainId from AscmDeliveryOrderDetail where materialId  in(select id from AscmMaterialItem where docNumber>='" + startMaterialDocNumber.Trim() + "' and docNumber<='" + endMaterialDocNumber.Trim() + "')))";
                    }
                    else
                    {
                        whereEndMaterialId = "id in (select batchId from AscmDeliveryOrderMain where id in(select mainId from AscmDeliveryOrderDetail where materialId in(select id from AscmMaterialItem where docNumber<='" + endMaterialDocNumber.Trim() + "')))";
                    }
                }
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartMaterialId);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndMaterialId);
                #endregion

                #region 确认时间
                string whereStartConfirmDate = "", whereEndConfirmDate = "";
                DateTime dtStartConfirmDate, dtEndConfirmDate;
                if (!string.IsNullOrEmpty(startConfirmDate) && DateTime.TryParse(startConfirmDate, out dtStartConfirmDate))
                    whereStartConfirmDate = "id in(select batchId from AscmDeliveryOrderMain where id in (select mainId from AscmDeliveryOrderDetail where notifyDetailId in (select id from AscmDeliveryNotifyDetail where mainId in (select id from AscmDeliveryNotifyMain where confirmTime>='" + dtStartConfirmDate.ToString("yyyy-MM-dd 00:00:00") + "'))))";
                if (!string.IsNullOrEmpty(endConfirmDate) && DateTime.TryParse(endConfirmDate, out dtEndConfirmDate))
                    whereEndConfirmDate = "id in(select batchId from AscmDeliveryOrderMain where id in (select mainId from AscmDeliveryOrderDetail where notifyDetailId in (select id from AscmDeliveryNotifyDetail where mainId in (select id from AscmDeliveryNotifyMain where confirmTime<'" + dtEndConfirmDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'))))";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartConfirmDate);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndConfirmDate);
                #endregion

                #region 通知时间
                string whereStartNotifyDate = "", whereEndNotifyDate = "";
                DateTime dtStartNotifyDate, dtEndNotifyDate;
                if (!string.IsNullOrEmpty(startNotifyDate) && DateTime.TryParse(startNotifyDate, out dtStartNotifyDate))
                    whereStartNotifyDate = "id in(select batchId from AscmDeliveryOrderMain where id in (select mainId from AscmDeliveryOrderDetail where notifyDetailId in (select id from AscmDeliveryNotifyDetail where mainId in (select id from AscmDeliveryNotifyMain where releasedTime>='" + dtStartNotifyDate.ToString("yyyy-MM-dd 00:00:00") + "'))))";
                if (!string.IsNullOrEmpty(endNotifyDate) && DateTime.TryParse(endNotifyDate, out dtEndNotifyDate))
                    whereEndNotifyDate = "id in(select batchId from AscmDeliveryOrderMain where id in (select mainId from AscmDeliveryOrderDetail where notifyDetailId in (select id from AscmDeliveryNotifyDetail where mainId in (select id from AscmDeliveryNotifyMain where releasedTime<'" + dtEndNotifyDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'))))";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartNotifyDate);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndNotifyDate);
                #endregion

                #region 预约时间
                DateTime dtAppointmentStartTime, dtAppointmentEndTime;
                string _appointmentStartTime = appointmentStartTime;
                string _appointmentEndTime = appointmentEndTime;
                if (!string.IsNullOrEmpty(_appointmentEndTime) && DateTime.TryParse(_appointmentEndTime, out dtAppointmentEndTime))
                {
                    _appointmentEndTime = dtAppointmentEndTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                }
                AscmDeliBatSumMain ascmDeliBatSumMain = null;
                if (supplierDeliveryBatSumId.HasValue)
                {
                    ascmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().Get(supplierDeliveryBatSumId.Value);
                    //ascmDeliBatSumMain.appointmentStartTime = "2013-11-20 09:00";
                    //ascmDeliBatSumMain.appointmentEndTime = "2013-11-20 10:00";
                    DateTime dtSumMainStartTime, dtSumMainEndTime;
                    DateTime dtTmp;
                    if (DateTime.TryParse(ascmDeliBatSumMain.appointmentStartTime, out dtSumMainStartTime) && DateTime.TryParse(ascmDeliBatSumMain.appointmentEndTime, out dtSumMainEndTime))
                    {
                        if (!string.IsNullOrEmpty(_appointmentStartTime) && DateTime.TryParse(_appointmentStartTime, out dtTmp))
                        {
                            if (dtTmp < dtSumMainStartTime)
                                _appointmentStartTime = dtSumMainStartTime.ToString("yyyy-MM-dd HH:mm");
                        }
                        if (!string.IsNullOrEmpty(_appointmentEndTime) && DateTime.TryParse(_appointmentEndTime, out dtTmp))
                        {
                            if (dtTmp > dtSumMainEndTime)
                                _appointmentEndTime = dtSumMainEndTime.ToString("yyyy-MM-dd HH:mm");
                        }
                    }
                    //2013.11.28对相同司机合单进行判断
                    List<AscmDeliBatSumMain> listAscmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().GetList("from AscmDeliBatSumMain where id<>" + ascmDeliBatSumMain.id + " and driverId=" + ascmDeliBatSumMain.driverId + " and status='" + AscmDeliBatSumMain.StatusDefine.unConfirm + "'");
                    foreach (AscmDeliBatSumMain ascmDeliBatSumMain_Other in listAscmDeliBatSumMain)
                    {
                        DateTime dtSumMainStartTime_Other, dtSumMainEndTime_Other;
                        if (DateTime.TryParse(ascmDeliBatSumMain_Other.appointmentStartTime, out dtSumMainStartTime_Other) && DateTime.TryParse(ascmDeliBatSumMain_Other.appointmentEndTime, out dtSumMainEndTime_Other))
                        {
                            if (!string.IsNullOrEmpty(_appointmentStartTime) && DateTime.TryParse(_appointmentStartTime, out dtTmp))
                            {
                                if (dtTmp < dtSumMainStartTime_Other)
                                    _appointmentStartTime = dtSumMainStartTime_Other.ToString("yyyy-MM-dd HH:mm");
                            }
                            if (!string.IsNullOrEmpty(_appointmentEndTime) && DateTime.TryParse(_appointmentEndTime, out dtTmp))
                            {
                                if (dtTmp > dtSumMainEndTime_Other)
                                    _appointmentEndTime = dtSumMainEndTime_Other.ToString("yyyy-MM-dd HH:mm");
                            }
                        }
                    }
                    //5)	最晚到货时间早于当前时间的批单，该批单总是可以勾选入合单
                    if (!string.IsNullOrEmpty(_appointmentEndTime) && DateTime.TryParse(_appointmentEndTime, out dtTmp))
                    {
                        if (dtTmp < DateTime.Now)
                        {
                            _appointmentStartTime = null;
                            _appointmentEndTime = null;
                        }
                    }
                }
                if (appointmentTimeFilter)
                {
                    string whereAppointment = "";
                    //if (!string.IsNullOrEmpty(_appointmentStartTime) && DateTime.TryParse(_appointmentStartTime, out dtAppointmentStartTime))
                    //    whereAppointment = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereAppointment, "appointmentStartTime>='" + dtAppointmentStartTime.ToString("yyyy-MM-dd HH:mm") + "'");
                    //if (!string.IsNullOrEmpty(_appointmentEndTime) && DateTime.TryParse(_appointmentEndTime, out dtAppointmentEndTime))
                    //    whereAppointment = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereAppointment, "appointmentEndTime<='" + dtAppointmentEndTime.ToString("yyyy-MM-dd HH:mm") + "'");
                    if (!string.IsNullOrEmpty(_appointmentStartTime) && DateTime.TryParse(_appointmentStartTime, out dtAppointmentStartTime) &&
                       !string.IsNullOrEmpty(_appointmentEndTime) && DateTime.TryParse(_appointmentEndTime, out dtAppointmentEndTime))
                    {
                        whereAppointment = "((appointmentEndTime between '" + dtAppointmentStartTime.ToString("yyyy-MM-dd HH:mm") + "' and '" + dtAppointmentEndTime.ToString("yyyy-MM-dd HH:mm") + "') or ";
                        whereAppointment += "(appointmentStartTime between '" + dtAppointmentStartTime.ToString("yyyy-MM-dd HH:mm") + "' and '" + dtAppointmentEndTime.ToString("yyyy-MM-dd HH:mm") + "' ) or ";
                        whereAppointment += "('" + dtAppointmentStartTime.ToString("yyyy-MM-dd HH:mm") + "' between appointmentStartTime and appointmentEndTime ) or ";
                        whereAppointment += "('" + dtAppointmentEndTime.ToString("yyyy-MM-dd HH:mm") + "' between appointmentStartTime and appointmentEndTime ))";
                    }
                    if (whereAppointment != "")
                    {
                        whereAppointment = "(" + whereAppointment + ") or appointmentStartTime=appointmentEndTime or appointmentEndTime<'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "'";
                        whereAppointment = "id in(select batchId from AscmDeliveryOrderMain where id in (select mainId from AscmDeliveryOrderDetail where notifyDetailId in (select id from AscmDeliveryNotifyDetail where mainId in (select id from AscmDeliveryNotifyMain where " + whereAppointment + "))))";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereAppointment);
                    }
                    //whereAscmStatus = " ascmStatus is null or ascmStatus<>'" + AscmDeliveryOrderBatch.AscmStatusDefine.inStorage + "'";
                    //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "ascmStatus<>'" + AscmDeliveryOrderBatch.AscmStatusDefine.inStorage + "'");
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "ascmStatus is null or ascmStatus=''");
                }

                //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereAppointmentEndTime);
                #endregion

                #region 已经完成批单不能显示
                if (isSupplierSelect.HasValue)
                {
                    //whereBatSumMain = " select * from AscmDeliBatSumMain where status in ('" + AscmDeliBatSumMain.StatusDefine.unConfirm + "','" + AscmDeliBatSumMain.StatusDefine.confirm + "')";
                    //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereAscmStatus);
                }
                if (supplierDeliveryBatSumId.HasValue)
                {
                    if (string.IsNullOrEmpty(queryFilter))
                    {
                        //string whereBatSumMain = " id not in (select batchId from AscmDeliBatSumDetail where mainId in (select id from AscmDeliBatSumMain where driverId=" + ascmDeliBatSumMain.driverId + "))";
                        string whereBatSumMain = " id not in (select batchId from AscmDeliBatSumDetail)";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereBatSumMain);
                    }
                }
                #endregion

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereBatSumMain);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereAscmStatus);

                List<AscmDeliveryOrderBatch> list = AscmDeliveryOrderBatchService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                if (list != null)
                {
                    AscmDeliveryOrderBatchService.GetInstance().SetDeliveryNotifyMain(list);
                    if (!string.IsNullOrEmpty(sort) && sort.ToLower() == "appointmentEndTime".ToLower())
                    {
                        list = list.OrderBy(p => p.appointmentEndTime).ToList();
                    }
                    else if (!string.IsNullOrEmpty(sort) && sort.ToLower() == "warehouseId".ToLower())
                    {
                        list = list.OrderBy(p => p.warehouseId).ToList();
                    }

                    foreach (AscmDeliveryOrderBatch ascmDeliveryOrderBatch in list)
                    {
                        jsonDataGridResult.rows.Add(ascmDeliveryOrderBatch.GetOwner());
                    }
                    jsonDataGridResult.total = ynPage.GetRecordCount();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 送货单明细
        public ActionResult DeliveryOrderDetailQuery()
        {
            //送货单物料统计和单据
            return View();
        }
        public ActionResult DeliveryOrderDetailList2(int? page, int? rows, string sort, string order, string queryWord,
            string supplier, string materialItem, string warehouse, string startDeliveryTime, string endDeliveryTime, string status)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDeliveryOrderDetail> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereMain = "";

                string whereSupplier = "", whereWarehouse = "",whereMaterialItem = "";
                string whereStartDeliveryTime = "", whereEndDeliveryTime = "", whereStatus = "";
                
                if (!string.IsNullOrEmpty(materialItem))
                    whereMaterialItem = "materialId= " + materialItem;

                if (!string.IsNullOrEmpty(supplier))
                    whereSupplier = "supplierId=" + supplier;
                if (!string.IsNullOrEmpty(warehouse))
                    whereWarehouse = "warehouseId = '" + warehouse + "'";
                if (!string.IsNullOrEmpty(status))
                    whereStatus = "status = '" + status + "'";

                DateTime dtStartDeliveryTime, dtEndDeliveryTime;
                if (!string.IsNullOrEmpty(startDeliveryTime) && DateTime.TryParse(startDeliveryTime, out dtStartDeliveryTime))
                    whereStartDeliveryTime = "deliveryTime>='" + dtStartDeliveryTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endDeliveryTime) && DateTime.TryParse(endDeliveryTime, out dtEndDeliveryTime))
                    whereEndDeliveryTime = "deliveryTime<'" + dtEndDeliveryTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereMain = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMain, whereSupplier);
                whereMain = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMain, whereWarehouse);
                whereMain = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMain, whereStatus);
                whereMain = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMain, whereStartDeliveryTime);
                whereMain = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMain, whereEndDeliveryTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereMaterialItem);

                if (!string.IsNullOrEmpty(whereMain))
                {
                    whereMain = "mainId in (select id from AscmDeliveryOrderMain where " + whereMain + ")";
                }

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereMain);

                list = AscmDeliveryOrderDetailService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in list)
                {
                    jsonDataGridResult.rows.Add(ascmDeliveryOrderDetail.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 供应商送货通知
        public ActionResult SupplierAppointmentDelivery()
        {
            //供应商客户端预约送货查询
            return View();
        }
        public ActionResult SupplierDeliveryNotifyMainList(int? page, int? rows, string sort, string order, string queryWord,
            string startReleasedTime, string endReleasedTime, string startNeedTime, string endNeedTime, string status, string employeeBuyer)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDeliveryNotifyMain> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                YnFrame.Dal.Entities.YnUser ynUser = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketUserData();
                if (ynUser == null)
                    throw new Exception("用户错误！");
                AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(ynUser.userId);
                if (ascmUserInfo.extExpandType != "erp")
                    throw new Exception("供应商用户错误！");
                AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                if (ascmUserInfo.ascmSupplier == null)
                    throw new Exception("供应商用户错误！");



                string whereOther = "", whereSupplier = "", whereStartReleaseTime = "", whereEndReleaseTime = "";
                string whereStartNeedTime = "", whereEndNeedTime = "", whereStatus = "", whereEmployeeBuyer = "";
                //if (!string.IsNullOrEmpty(supplier))
                whereSupplier = "supplierId=" + ascmUserInfo.ascmSupplier.id;
                if (!string.IsNullOrEmpty(status))
                {
                    whereStatus = "status = '" + status + "'";
                }
                else
                {
                    foreach (string s1 in MideaAscm.Dal.FromErp.Entities.AscmDeliveryNotifyMain.StatusDefine.GetSupplierList())
                    {
                        if (whereStatus != "")
                            whereStatus += ",";
                        whereStatus += "'" + s1 + "'";
                    }
                    if (whereStatus != "")
                        whereStatus = " status in (" + whereStatus + ")";
                }

                if (!string.IsNullOrEmpty(employeeBuyer))
                {
                    whereEmployeeBuyer = "materialId in (select id from AscmMaterialItem where buyerId=" + employeeBuyer + ")";
                }

                DateTime dtStartReleasedTime, dtEndReleasedTime;
                if (!string.IsNullOrEmpty(startReleasedTime) && DateTime.TryParse(startReleasedTime, out dtStartReleasedTime))
                    whereStartReleaseTime = "releasedTime>='" + dtStartReleasedTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endReleasedTime) && DateTime.TryParse(endReleasedTime, out dtEndReleasedTime))
                    whereEndReleaseTime = "releasedTime<'" + dtEndReleasedTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                DateTime dtStartNeedTime, dtEndNeedTime;
                if (!string.IsNullOrEmpty(startNeedTime) && DateTime.TryParse(startNeedTime, out dtStartNeedTime))
                    whereStartNeedTime = "needTime>='" + dtStartNeedTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endReleasedTime) && DateTime.TryParse(endNeedTime, out dtEndNeedTime))
                    whereEndNeedTime = "needTime<'" + dtEndNeedTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier); 
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartReleaseTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndReleaseTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartNeedTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndNeedTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEmployeeBuyer);

                list = AscmDeliveryNotifyMainService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                {
                    jsonDataGridResult.rows.Add(ascmDeliveryNotifyMain.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 供应商送货单
        public ActionResult SupplierDeliveryOrderQuery()
        {
            //供应商送货单统计和单据
            return View();
        }
        public ActionResult SupplierDeliveryOrderMainList(int? page, int? rows, string sort, string order, string queryWord,
            string startDeliveryTime, string endDeliveryTime, string status)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDeliveryOrderMain> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                YnFrame.Dal.Entities.YnUser ynUser = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketUserData();
                if (ynUser == null)
                    throw new Exception("用户错误！");
                AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(ynUser.userId);
                if (ascmUserInfo.extExpandType != "erp")
                    throw new Exception("供应商用户错误！");
                AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                if (ascmUserInfo.ascmSupplier == null)
                    throw new Exception("供应商用户错误！");

                string whereOther = "", whereSupplier = "";
                string whereStartDeliveryTime = "", whereEndDeliveryTime = "", whereStatus = "";

                whereSupplier = "supplierId=" + ascmUserInfo.ascmSupplier.id;

                if (!string.IsNullOrEmpty(status))
                {
                    whereStatus = "status = '" + status + "'";
                }
                else
                {
                    foreach (string s1 in MideaAscm.Dal.FromErp.Entities.AscmDeliveryOrderMain.StatusDefine.GetSupplierList())
                    {
                        if (whereStatus != "")
                            whereStatus += ",";
                        whereStatus += "'" + s1 + "'";
                    }
                    if (whereStatus != "")
                        whereStatus = " status in (" + whereStatus + ")";
                }

                DateTime dtStartDeliveryTime, dtEndDeliveryTime;
                if (!string.IsNullOrEmpty(startDeliveryTime) && DateTime.TryParse(startDeliveryTime, out dtStartDeliveryTime))
                    whereStartDeliveryTime = "deliveryTime>='" + dtStartDeliveryTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endDeliveryTime) && DateTime.TryParse(endDeliveryTime, out dtEndDeliveryTime))
                    whereEndDeliveryTime = "deliveryTime<'" + dtEndDeliveryTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartDeliveryTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndDeliveryTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereMaterialItem);

                list = AscmDeliveryOrderMainService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                foreach (AscmDeliveryOrderMain ascmDeliveryOrderMain in list)
                {
                    jsonDataGridResult.rows.Add(ascmDeliveryOrderMain.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SupplierDeliveryOrderBatchList(int? page, int? rows, string sort, string order, string queryWord,
            string startCreateTime, string endCreateTime, string status)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDeliveryOrderBatch> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                YnFrame.Dal.Entities.YnUser ynUser = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketUserData();
                if (ynUser == null)
                    throw new Exception("用户错误！");
                AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(ynUser.userId);
                if (ascmUserInfo.extExpandType != "erp")
                    throw new Exception("供应商用户错误！");
                AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                if (ascmUserInfo.ascmSupplier == null)
                    throw new Exception("供应商用户错误！");

                string whereOther = "", whereSupplier = "";
                string whereStartCreateTime = "", whereEndCreateTime = "", whereStatus = "";

                whereSupplier = "supplierId=" + ascmUserInfo.ascmSupplier.id;
                if (!string.IsNullOrEmpty(status))
                {
                    whereStatus = "status = '" + status + "'";
                }
                else
                {
                    foreach (string s1 in MideaAscm.Dal.FromErp.Entities.AscmDeliveryOrderMain.StatusDefine.GetSupplierList())
                    {
                        if (whereStatus != "")
                            whereStatus += ",";
                        whereStatus += "'" + s1 + "'";
                    }
                    if (whereStatus != "")
                        whereStatus = " status in (" + whereStatus + ")";
                }

                DateTime dtStartCreateTime, dtEndCreateTime;
                if (!string.IsNullOrEmpty(startCreateTime) && DateTime.TryParse(startCreateTime, out dtStartCreateTime))
                    whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endCreateTime) && DateTime.TryParse(endCreateTime, out dtEndCreateTime))
                    whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereMaterialItem);
                list = AscmDeliveryOrderBatchService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                foreach (AscmDeliveryOrderBatch ascmDeliveryOrderBatch in list)
                {
                    jsonDataGridResult.rows.Add(ascmDeliveryOrderBatch.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 供应商送货单明细
        public ActionResult SupplierDeliveryOrderDetailQuery()
        {
            //供应商送货单物料统计和单据
            return View();
        }
        public ActionResult SupplierDeliveryOrderDetailList2(int? page, int? rows, string sort, string order, string queryWord,
            string materialItem, string startDeliveryTime, string endDeliveryTime, string status, int? wipEntityId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDeliveryOrderDetail> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                YnFrame.Dal.Entities.YnUser ynUser = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketUserData();
                if (ynUser == null)
                    throw new Exception("用户错误！");
                AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(ynUser.userId);
                if (ascmUserInfo.extExpandType != "erp")
                    throw new Exception("供应商用户错误！");
                AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                if (ascmUserInfo.ascmSupplier == null)
                    throw new Exception("供应商用户错误！");

                string whereOther = "", whereMain = "";

                string whereSupplier = "", whereWarehouse = "", whereMaterialItem = "";
                string whereStartDeliveryTime = "", whereEndDeliveryTime = "", whereStatus = "", whereWipEntity = "";

                if (!string.IsNullOrEmpty(materialItem))
                    whereMaterialItem = "materialId= " + materialItem;

                whereSupplier = "supplierId=" + ascmUserInfo.ascmSupplier.id;

                if (!string.IsNullOrEmpty(status))
                {
                    whereStatus = "status = '" + status + "'";
                }
                else
                {
                    foreach (string s1 in MideaAscm.Dal.FromErp.Entities.AscmDeliveryOrderMain.StatusDefine.GetSupplierList())
                    {
                        if (whereStatus != "")
                            whereStatus += ",";
                        whereStatus += "'" + s1 + "'";
                    }
                    if (whereStatus != "")
                        whereStatus = " status in (" + whereStatus + ")";
                }

                DateTime dtStartDeliveryTime, dtEndDeliveryTime;
                if (!string.IsNullOrEmpty(startDeliveryTime) && DateTime.TryParse(startDeliveryTime, out dtStartDeliveryTime))
                    whereStartDeliveryTime = "deliveryTime>='" + dtStartDeliveryTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endDeliveryTime) && DateTime.TryParse(endDeliveryTime, out dtEndDeliveryTime))
                    whereEndDeliveryTime = "deliveryTime<'" + dtEndDeliveryTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                if (wipEntityId.HasValue)
                    whereWipEntity = "wipEntityId=" + wipEntityId.Value;

                whereMain = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMain, whereSupplier);
                whereMain = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMain, whereWarehouse);
                whereMain = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMain, whereStatus);
                whereMain = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMain, whereStartDeliveryTime);
                whereMain = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMain, whereEndDeliveryTime);
                whereMain = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMain, whereWipEntity);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereMaterialItem);

                if (!string.IsNullOrEmpty(whereMain))
                {
                    whereMain = "mainId in (select id from AscmDeliveryOrderMain where " + whereMain + ")";
                }

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereMain);

                list = AscmDeliveryOrderDetailService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in list)
                {
                    jsonDataGridResult.rows.Add(ascmDeliveryOrderDetail.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
