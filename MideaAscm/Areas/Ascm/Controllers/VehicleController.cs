using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MideaAscm.Dal.Vehicle.Entities;
using MideaAscm.Services.Vehicle;
using YnBaseClass2.Web;
using Newtonsoft.Json;
using MideaAscm.Dal;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Services.Base;
using NHibernate;
using System.IO;

namespace MideaAscm.Areas.Ascm.Controllers
{
    public class VehicleController : YnFrame.Controllers.YnBaseController
    {
        //
        // GET: /Ascm/Clgl/
        //车辆管理
        public ActionResult Index()
        {
            return View();
        }

        #region 供应商车辆出入日志查询
        public ActionResult TruckSwipeLogIndex()
        {
            //大门
            List<SelectListItem> listSelectDoor = new List<SelectListItem>();
            listSelectDoor.Add(new SelectListItem { Text = "", Value = "" });
            List<AscmDoor> listAscmDoor = AscmDoorService.GetInstance().GetList();
            if (listAscmDoor != null)
            {
                foreach (AscmDoor ascmDoor in listAscmDoor)
                {
                    listSelectDoor.Add(new SelectListItem { Text = ascmDoor.name, Value = ascmDoor.id.ToString() });
                }
            }
            ViewData["listSelectDoor"] = listSelectDoor;

            //车辆日志查询
            return View();
        }
        public ActionResult TruckSwipeLogList(int? page, int? rows, string sort, string order, string queryWord, int? doorId, string plateNumber, string queryStartTime, string queryEndTime)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereDoor = "", wherePlateNumber = "";
                string whereStartCreateTime = "", whereEndCreateTime = "";
                if (doorId.HasValue)
                    whereDoor = "doorId=" + doorId;
                if (!string.IsNullOrEmpty(plateNumber))
                    wherePlateNumber = "plateNumber like '%" + plateNumber.Trim() + "%'";
                DateTime dtStartCreateTime, dtEndCreateTime;
                if (!string.IsNullOrEmpty(queryStartTime) && DateTime.TryParse(queryStartTime, out dtStartCreateTime))
                    whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(queryEndTime) && DateTime.TryParse(queryEndTime, out dtEndCreateTime))
                    whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereDoor);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, wherePlateNumber);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);

                List<AscmTruckSwipeLog> list = AscmTruckSwipeLogService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                if (list != null)
                {
                    foreach (AscmTruckSwipeLog ascmTruckSwipeLog in list)
                    {
                        jsonDataGridResult.rows.Add(ascmTruckSwipeLog);
                    }
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

        #region 员工车辆出入日志
        public ActionResult EmployeeCarSwipeLogIndex()
        {
            //大门
            List<SelectListItem> listSelectDoor = new List<SelectListItem>();
            listSelectDoor.Add(new SelectListItem { Text = "", Value = "" });
            List<AscmDoor> listAscmDoor = AscmDoorService.GetInstance().GetList();
            if (listAscmDoor != null)
            {
                foreach (AscmDoor ascmDoor in listAscmDoor)
                {
                    listSelectDoor.Add(new SelectListItem { Text = ascmDoor.name, Value = ascmDoor.id.ToString() });
                }
            }
            ViewData["listSelectDoor"] = listSelectDoor;
            //方向
            List<SelectListItem> listSelectDirection = new List<SelectListItem>();
            listSelectDirection.Add(new SelectListItem { Text = "", Value = "" });
            List<string> listDirection = AscmEmpCarSwipeLog.DirectionDefine.GetList();
            if (listDirection != null)
            {
                foreach (string direction in listDirection)
                {
                    listSelectDirection.Add(new SelectListItem { Text = direction, Value = direction });
                }
            }
            ViewData["listSelectDirection"] = listSelectDirection;
            return View();
        }
        public ActionResult EmployeeCarSwipeLogList(int? page, int? rows, string sort, string order, string queryWord, int? doorId,
            string direction, string plateNumber, string employeeName, string queryStartTime, string queryEndTime)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = string.Empty;
                if (doorId.HasValue)
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "doorId=" + doorId);
                if (!string.IsNullOrEmpty(direction))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "direction='" + direction + "'");
                if (!string.IsNullOrEmpty(plateNumber))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "plateNumber like '%" + plateNumber.Trim() + "%'");
                if (!string.IsNullOrEmpty(employeeName))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "employeeName like '%" + employeeName.Trim() + "%'");
                DateTime dtStartCreateTime, dtEndCreateTime;
                if (!string.IsNullOrEmpty(queryStartTime) && DateTime.TryParse(queryStartTime, out dtStartCreateTime))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'");
                if (!string.IsNullOrEmpty(queryEndTime) && DateTime.TryParse(queryEndTime, out dtEndCreateTime))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'");

                List<AscmEmpCarSwipeLog> list = AscmEmpCarSwipeLogService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                if (list != null)
                    list.ForEach(P => jsonDataGridResult.rows.Add(P));

                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 卸货点管理
        public ActionResult UnloadingPointIndex()
        {
            //卸货点管理
            return View();
        }
        public ActionResult UnloadingPointList(int? page, int? rows, string sort, string order, string queryWord, string status, string warehouseId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmUnloadingPoint> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereStatus = "", whereWarehouse = "";
                if (!string.IsNullOrEmpty(status))
                    whereStatus = " status = '" + status + "'";
                if (!string.IsNullOrEmpty(warehouseId))
                    whereWarehouse = " warehouseId = '" + warehouseId + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);

                list = AscmUnloadingPointService.GetInstance().GetList(ynPage, sort, order, queryWord, whereOther);
                if (list != null)
                {
                    foreach (AscmUnloadingPoint ascmUnloadingPoint in list)
                    {
                        jsonDataGridResult.rows.Add(ascmUnloadingPoint);
                    }
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnloadingPointEdit(int? id)
        {
            AscmUnloadingPoint ascmUnloadingPoint = null;
            try
            {
                if (id.HasValue)
                {
                    ascmUnloadingPoint = AscmUnloadingPointService.GetInstance().Get(id.Value);
                    if (ascmUnloadingPoint != null && !string.IsNullOrEmpty(ascmUnloadingPoint.warehouseId))
                    {
                        ascmUnloadingPoint.ascmWarehouse = AscmWarehouseService.GetInstance().Get(ascmUnloadingPoint.warehouseId);
                        ascmUnloadingPoint.ascmUnloadingPointController = AscmUnloadingPointControllerService.GetInstance().Get(ascmUnloadingPoint.controllerId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmUnloadingPoint, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult UnloadingPointSave(AscmUnloadingPoint ascmUnloadingPoint_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }
                if (ascmUnloadingPoint_Model.name == null || ascmUnloadingPoint_Model.name.Trim() == "")
                    throw new Exception("卸货点名称不能为空");
                string name = ascmUnloadingPoint_Model.name.Trim();

                AscmWarehouse ascmWarehouse = AscmWarehouseService.GetInstance().Get(ascmUnloadingPoint_Model.warehouseId);
                if (ascmWarehouse == null)
                    throw new Exception("仓库不能为空");

                AscmUnloadingPoint ascmUnloadingPoint = null;
                if (id.HasValue)
                {
                    ascmUnloadingPoint = AscmUnloadingPointService.GetInstance().Get(id.Value);
                    if (ascmUnloadingPoint == null)
                        throw new Exception("保存卸货点失败！");
                }
                else
                {
                    ascmUnloadingPoint = new AscmUnloadingPoint();
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmUnloadingPoint");
                    ascmUnloadingPoint.id = ++maxId;
                    ascmUnloadingPoint.createUser = userName;
                    ascmUnloadingPoint.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmUnloadingPoint where id<>" + ascmUnloadingPoint.id + " and warehouseId='" + ascmWarehouse.id + "' and name='" + name + "'");
                if (object1 == null)
                    throw new Exception("查询异常！");
                int count = 0;
                if (int.TryParse(object1.ToString(), out count) && count > 0)
                    throw new Exception("仓库【" + ascmWarehouse.id + "】已存在卸货点名称");

                ascmUnloadingPoint.name = name;
                ascmUnloadingPoint.ascmWarehouse = ascmWarehouse;
                ascmUnloadingPoint.warehouseId = ascmWarehouse.id;
                ascmUnloadingPoint.controllerId = ascmUnloadingPoint_Model.controllerId;
                ascmUnloadingPoint.controllerAddress = ascmUnloadingPoint_Model.controllerAddress;
                if (!string.IsNullOrEmpty(ascmUnloadingPoint_Model.direction))
                    ascmUnloadingPoint.direction = ascmUnloadingPoint_Model.direction.Trim();
                if (!string.IsNullOrEmpty(ascmUnloadingPoint_Model.location))
                    ascmUnloadingPoint.location = ascmUnloadingPoint_Model.location.Trim();
                ascmUnloadingPoint.status = ascmUnloadingPoint_Model.status;
                ascmUnloadingPoint.ip = ascmUnloadingPoint_Model.ip;
                ascmUnloadingPoint.enabled = ascmUnloadingPoint_Model.enabled;
                if (!string.IsNullOrEmpty(ascmUnloadingPoint_Model.description))
                    ascmUnloadingPoint.description = ascmUnloadingPoint_Model.description.Trim();
                if (!string.IsNullOrEmpty(ascmUnloadingPoint_Model.memo))
                    ascmUnloadingPoint.memo = ascmUnloadingPoint_Model.memo.Trim();
                ascmUnloadingPoint.modifyUser = userName;
                ascmUnloadingPoint.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                if (id.HasValue)
                    AscmUnloadingPointService.GetInstance().Update(ascmUnloadingPoint);
                else
                    AscmUnloadingPointService.GetInstance().Save(ascmUnloadingPoint);

                jsonObjectResult.result = true;
                jsonObjectResult.id = ascmUnloadingPoint.id.ToString();
                //jsonObjectResult.entity = ascmUnloadingPoint;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult UnloadingPointDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    AscmUnloadingPoint ascmUnloadingPoint = AscmUnloadingPointService.GetInstance().Get(id.Value);
                    if (ascmUnloadingPoint == null)
                        throw new Exception("卸货点不存在");

                    List<AscmUnloadingPointMapLink> listAscmUnloadingPointMapLink =
                        AscmUnloadingPointMapLinkService.GetInstance().GetList("from AscmUnloadingPointMapLink where pointId=" + ascmUnloadingPoint.id);

                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Delete(ascmUnloadingPoint);

                            if (listAscmUnloadingPointMapLink != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmUnloadingPointMapLink);

                            tx.Commit();//正确执行提交
                            jsonObjectResult.result = true;
                            jsonObjectResult.message = "";
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
                    jsonObjectResult.message = "传入参数[id=null]错误！";
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnloadingPointNotInMapList(int? page, int? rows, string sort, string order, string warehouseId, int? mapId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmUnloadingPoint> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereWarehouse = "", whereNotIdMap = "";
                if (!string.IsNullOrEmpty(warehouseId))
                    whereWarehouse = " warehouseId = '" + warehouseId + "'";
                if (mapId.HasValue)
                    whereNotIdMap = " id not in(select pointId from AscmUnloadingPointMapLink where mapId=" + mapId + ")";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereNotIdMap);

                list = AscmUnloadingPointService.GetInstance().GetList(ynPage, "", "", "", whereOther);
                if (list != null)
                {
                    foreach (AscmUnloadingPoint ascmUnloadingPoint in list)
                    {
                        jsonDataGridResult.rows.Add(ascmUnloadingPoint);
                    }
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

        #region 卸货点控制器管理
        public ActionResult UnloadingPointControllerIndex()
        {
            //卸货点管理
            return View();
        }
        public ActionResult UnloadingPointControllerList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                List<AscmUnloadingPointController> list = AscmUnloadingPointControllerService.GetInstance().GetList(ynPage, sort, order, queryWord, null);
                if (list != null)
                {
                    foreach (AscmUnloadingPointController ascmUnloadingPointController in list)
                    {
                        jsonDataGridResult.rows.Add(ascmUnloadingPointController);
                    }
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnloadingPointControllerEdit(int? id)
        {
            AscmUnloadingPointController ascmUnloadingPointController = null;
            try
            {
                if (id.HasValue)
                {
                    ascmUnloadingPointController = AscmUnloadingPointControllerService.GetInstance().Get(id.Value);
                }
                ascmUnloadingPointController = ascmUnloadingPointController ?? new AscmUnloadingPointController();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmUnloadingPointController, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult UnloadingPointControllerSave(AscmUnloadingPointController ascmUnloadingPointController_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }
                if (ascmUnloadingPointController_Model.name == null || ascmUnloadingPointController_Model.name.Trim() == "")
                    throw new Exception("控制器名称不能为空");
                string name = ascmUnloadingPointController_Model.name.Trim();

                AscmUnloadingPointController ascmUnloadingPointController = null;
                if (id.HasValue)
                {
                    ascmUnloadingPointController = AscmUnloadingPointControllerService.GetInstance().Get(id.Value);
                    if (ascmUnloadingPointController == null)
                        throw new Exception("获取卸货点控制器失败！");
                }
                else
                {
                    ascmUnloadingPointController = new AscmUnloadingPointController();
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmUnloadingPointController");
                    ascmUnloadingPointController.id = ++maxId;
                    ascmUnloadingPointController.createUser = userName;
                    ascmUnloadingPointController.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmUnloadingPointController where id<>" + ascmUnloadingPointController.id + " and name='" + name + "'");
                if (object1 == null)
                    throw new Exception("查询异常！");
                int count = 0;
                if (int.TryParse(object1.ToString(), out count) && count > 0)
                    throw new Exception("控制器名称【" + name + "】已存在");

                ascmUnloadingPointController.name = name;
                if (!string.IsNullOrEmpty(ascmUnloadingPointController_Model.ip))
                    ascmUnloadingPointController.ip = ascmUnloadingPointController_Model.ip.Trim();
                ascmUnloadingPointController.port = ascmUnloadingPointController_Model.port;
                ascmUnloadingPointController.modifyUser = userName;
                ascmUnloadingPointController.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                if (id.HasValue)
                    AscmUnloadingPointControllerService.GetInstance().Update(ascmUnloadingPointController);
                else
                    AscmUnloadingPointControllerService.GetInstance().Save(ascmUnloadingPointController);

                jsonObjectResult.result = true;
                jsonObjectResult.id = ascmUnloadingPointController.id.ToString();
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult UnloadingPointControllerDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmUnloadingPoint where controllerId=" + id.Value);
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int count = 0;
                    if (int.TryParse(object1.ToString(), out count) && count > 0)
                        throw new Exception("卸货点中已指定此控制器");

                    AscmUnloadingPointController ascmUnloadingPointController = AscmUnloadingPointControllerService.GetInstance().Get(id.Value);
                    if (ascmUnloadingPointController == null)
                        throw new Exception("控制器不存在");


                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Delete(ascmUnloadingPointController);

                            tx.Commit();//正确执行提交
                            jsonObjectResult.result = true;
                            jsonObjectResult.message = "";
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
                    jsonObjectResult.message = "传入参数[id=null]错误！";
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 卸货点出入日志查询
        public ActionResult UnloadingPointLogIndex()
        {
            //卸货点日志查询
            return View();
        }
        public ActionResult UnloadingPointLogList(int? page, int? rows, string sort, string order, string queryWord, string warehouseId, string queryStartTime, string queryEndTime)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereWarehouse = "";
                string whereStartCreateTime = "", whereEndCreateTime = "";

                if (!string.IsNullOrEmpty(warehouseId))
                    whereWarehouse = "unloadingPointId in(select id from AscmUnloadingPoint where warehouseId='" + warehouseId + "')";
                DateTime dtStartCreateTime, dtEndCreateTime;
                if (!string.IsNullOrEmpty(queryStartTime) && DateTime.TryParse(queryStartTime, out dtStartCreateTime))
                    whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(queryEndTime) && DateTime.TryParse(queryEndTime, out dtEndCreateTime))
                    whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);

                List<AscmUnloadingPointLog> list = AscmUnloadingPointLogService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                if (list != null)
                {
                    foreach (AscmUnloadingPointLog ascmUnloadingPointLog in list)
                    {
                        jsonDataGridResult.rows.Add(ascmUnloadingPointLog);
                    }
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

        #region 卸货位地图管理
        public ActionResult UnloadingPointMapIndex()
        {
            //卸货点地图管理
            return View();
        }
        public ActionResult UnloadingPointMapList(int? page, int? rows, string sort, string order, string queryWord, string warehouseId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmUnloadingPointMap> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereWarehouse = "";
                if (!string.IsNullOrEmpty(warehouseId))
                    whereWarehouse = " warehouseId = '" + warehouseId + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);

                list = AscmUnloadingPointMapService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                if (list != null)
                {
                    foreach (AscmUnloadingPointMap ascmUnloadingPointMap in list)
                    {
                        jsonDataGridResult.rows.Add(ascmUnloadingPointMap);
                    }
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnloadingPointMapEdit(int? id)
        {
            AscmUnloadingPointMap ascmUnloadingPointMap = null;
            try
            {
                if (id.HasValue)
                {
                    ascmUnloadingPointMap = AscmUnloadingPointMapService.GetInstance().Get(id.Value);
                    if (ascmUnloadingPointMap != null && !string.IsNullOrEmpty(ascmUnloadingPointMap.warehouseId))
                    {
                        ascmUnloadingPointMap.ascmWarehouse = AscmWarehouseService.GetInstance().Get(ascmUnloadingPointMap.warehouseId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmUnloadingPointMap, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult UnloadingPointMapSave(HttpPostedFileBase imgUpload, AscmUnloadingPointMap ascmUnloadingPointMap_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }
                if (ascmUnloadingPointMap_Model.name == null || ascmUnloadingPointMap_Model.name.Trim() == "")
                    throw new Exception("卸货位名称不能为空");
                string name = ascmUnloadingPointMap_Model.name.Trim();

                AscmWarehouse ascmWarehouse = AscmWarehouseService.GetInstance().Get(ascmUnloadingPointMap_Model.warehouseId);
                if (ascmWarehouse == null)
                    throw new Exception("仓库不能为空");

                AscmUnloadingPointMap ascmUnloadingPointMap = null;
                if (id.HasValue)
                {
                    ascmUnloadingPointMap = AscmUnloadingPointMapService.GetInstance().Get(id.Value);
                    if (ascmUnloadingPointMap == null)
                        throw new Exception("保存卸货位地图失败！");
                }

                string imgUrl = ""; //smallImgUrl = "";
                int imgWidth = 0, imgHeight = 0;
                if (imgUpload != null)
                {
                    string fileName = imgUpload.FileName;
                    string fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                    //if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".gif" && fileExtension != ".png")
                    //    throw new Exception("只能上传后缀名为.jpg|.gif|.png的图片");
                    System.Drawing.Imaging.ImageFormat imageFormat = null;
                    if (fileExtension == ".jpg" || fileExtension == ".jpeg")
                        imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                    else if (fileExtension == ".gif")
                        imageFormat = System.Drawing.Imaging.ImageFormat.Gif;
                    else if (fileExtension == ".png")
                        imageFormat = System.Drawing.Imaging.ImageFormat.Png;
                    else
                        throw new Exception("只能上传后缀名为.jpg|.jpeg|.gif|.png的图片");

                    string serverPath = Server.MapPath(Request.ApplicationPath);
                    string serverDirectory = System.IO.Path.Combine(serverPath, "_data\\UnloadingPointMap");
                    if (!Directory.Exists(serverDirectory))
                    {
                        Directory.CreateDirectory(serverDirectory);
                    }
                    string serverImgName = System.IO.Path.GetFileNameWithoutExtension(fileName) + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + fileExtension;
                    string serverImgPath = System.IO.Path.Combine(serverDirectory, serverImgName);
                    //StreamWriter streamWriter = new StreamWriter(serverImgPath, true, System.Text.Encoding.GetEncoding("GB2312"));
                    imgUpload.SaveAs(serverImgPath);
                    //imgUrl = System.IO.Path.Combine(Request.ApplicationPath, "_data/UnloadingPointMap", serverImgName);
                    imgUrl = (Request.ApplicationPath == "/" ? "" : Request.ApplicationPath) + "/_data/UnloadingPointMap/" + serverImgName;

                    //string serverSmallImgName = System.IO.Path.GetFileNameWithoutExtension(fileName) + "_small_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + fileExtension;
                    //string serverSmallImgPath = System.IO.Path.Combine(serverDirectory, serverSmallImgName);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(imgUpload.InputStream);
                    imgWidth = img.Width;
                    imgHeight = img.Height;
                    //System.Drawing.Image newImg = new System.Drawing.Bitmap(img, 300, 140);                   
                    //newImg.Save(serverSmallImgPath, imageFormat);
                    //smallImgUrl = (Request.ApplicationPath == "/" ? "" : Request.ApplicationPath) + "/_data/UnloadingPointMap/" + serverSmallImgName;

                    DeleteUnloadingPointMapImg(ascmUnloadingPointMap);
                }

                if (ascmUnloadingPointMap == null)
                {
                    ascmUnloadingPointMap = new AscmUnloadingPointMap();
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmUnloadingPointMap");
                    ascmUnloadingPointMap.id = ++maxId;
                    ascmUnloadingPointMap.createUser = userName;
                    ascmUnloadingPointMap.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmUnloadingPointMap where id<>" + ascmUnloadingPointMap.id + " and warehouseId='" + ascmWarehouse.id + "' and name='" + name + "'");
                if (object1 == null)
                    throw new Exception("查询异常！");
                int count = 0;
                if (int.TryParse(object1.ToString(), out count) && count > 0)
                    throw new Exception("仓库【" + ascmWarehouse.id + "】已存在卸货位地图名称");

                ascmUnloadingPointMap.name = name;
                ascmUnloadingPointMap.ascmWarehouse = ascmWarehouse;
                ascmUnloadingPointMap.warehouseId = ascmWarehouse.id;
                if (!string.IsNullOrEmpty(ascmUnloadingPointMap_Model.direction))
                    ascmUnloadingPointMap.direction = ascmUnloadingPointMap_Model.direction.Trim();
                ascmUnloadingPointMap.width = ascmUnloadingPointMap_Model.width;
                ascmUnloadingPointMap.height = ascmUnloadingPointMap_Model.height;
                if (imgUpload != null)
                {
                    ascmUnloadingPointMap.imgUrl = imgUrl;
                    //ascmUnloadingPointMap.smallImgUrl = smallImgUrl;
                    ascmUnloadingPointMap.imgWidth = imgWidth;
                    ascmUnloadingPointMap.imgHeight = imgHeight;
                }
                if (!string.IsNullOrEmpty(ascmUnloadingPointMap_Model.description))
                    ascmUnloadingPointMap.description = ascmUnloadingPointMap_Model.description.Trim();
                if (!string.IsNullOrEmpty(ascmUnloadingPointMap_Model.memo))
                    ascmUnloadingPointMap.memo = ascmUnloadingPointMap_Model.memo.Trim();
                ascmUnloadingPointMap.modifyUser = userName;
                ascmUnloadingPointMap.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                if (id.HasValue)
                    AscmUnloadingPointMapService.GetInstance().Update(ascmUnloadingPointMap);
                else
                    AscmUnloadingPointMapService.GetInstance().Save(ascmUnloadingPointMap);

                jsonObjectResult.result = true;
                jsonObjectResult.id = ascmUnloadingPointMap.id.ToString();
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult UnloadingPointMapDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    AscmUnloadingPointMap ascmUnloadingPointMap = AscmUnloadingPointMapService.GetInstance().Get(id.Value);
                    if (ascmUnloadingPointMap == null)
                        throw new Exception("卸货位地图不存在");

                    List<AscmUnloadingPointMapLink> listAscmUnloadingPointMapLink =
                        AscmUnloadingPointMapLinkService.GetInstance().GetList(ascmUnloadingPointMap.id);

                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Delete(ascmUnloadingPointMap);

                            if (listAscmUnloadingPointMapLink != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmUnloadingPointMapLink);

                            tx.Commit();//正确执行提交
                            DeleteUnloadingPointMapImg(ascmUnloadingPointMap);
                            jsonObjectResult.result = true;
                            jsonObjectResult.message = "";
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
                    jsonObjectResult.message = "传入参数[id=null]错误！";
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        private void DeleteUnloadingPointMapImg(AscmUnloadingPointMap ascmUnloadingPointMap)
        {
            if (ascmUnloadingPointMap != null)
            {
                if (!string.IsNullOrEmpty(ascmUnloadingPointMap.imgUrl))
                {
                    string serverOriginalImgUrl = Server.MapPath(ascmUnloadingPointMap.imgUrl);
                    if (System.IO.File.Exists(serverOriginalImgUrl))
                        System.IO.File.Delete(serverOriginalImgUrl);
                }
                //if (!string.IsNullOrEmpty(ascmUnloadingPointMap.smallImgUrl))
                //{
                //    string serverOriginalSamllImgUrl = Server.MapPath(ascmUnloadingPointMap.smallImgUrl);
                //    if (System.IO.File.Exists(serverOriginalSamllImgUrl))
                //        System.IO.File.Delete(serverOriginalSamllImgUrl);
                //}
            }
        }
        public ActionResult GetUnloadingPointMapList(string warehouseId)
        {
            List<AscmUnloadingPointMap> list = null;
            try
            {
                string whereOther = "", whereWarehouse = "";
                if (!string.IsNullOrEmpty(warehouseId))
                    whereWarehouse = " warehouseId = '" + warehouseId + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);

                list = AscmUnloadingPointMapService.GetInstance().GetList(null, "", "", "", whereOther, false);
                list = list ?? new List<AscmUnloadingPointMap>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 卸货位地图关联卸货点
        public ActionResult UnloadingPointMapLink()
        {
            //卸货点地图设置
            return View();
        }
        public ActionResult UnloadingPointMapLinkList(int? mapId)
        {
            List<AscmUnloadingPointMapLink> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (mapId.HasValue)
                {
                    list = AscmUnloadingPointMapLinkService.GetInstance().GetList(mapId.Value);
                    if (list != null)
                    {
                        foreach (AscmUnloadingPointMapLink ascmUnloadingPointMapLink in list)
                        {
                            jsonDataGridResult.rows.Add(ascmUnloadingPointMapLink);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnloadingPointMapLinkAdd(int? mapId, string unloadingPointJson)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (mapId.HasValue && !string.IsNullOrEmpty(unloadingPointJson))
                {
                    AscmUnloadingPointMap ascmUnloadingPointMap = AscmUnloadingPointMapService.GetInstance().Get(mapId.Value);
                    if (ascmUnloadingPointMap == null)
                        throw new Exception("找不到卸货位地图");
                    
                    List<AscmUnloadingPoint> listAscmUnloadingPoint = JsonConvert.DeserializeObject<List<AscmUnloadingPoint>>(unloadingPointJson);
                    if (listAscmUnloadingPoint != null && listAscmUnloadingPoint.Count > 0)
                    {
                        string userName = string.Empty;
                        if (User.Identity.IsAuthenticated)
                            userName = User.Identity.Name;

                        List<AscmUnloadingPointMapLink> listMapLink = AscmUnloadingPointMapLinkService.GetInstance().GetList(mapId.Value);
                        List<AscmUnloadingPointMapLink> listMapLinkAdd = new List<AscmUnloadingPointMapLink>();
                        int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmUnloadingPointMapLink");
                        foreach (AscmUnloadingPoint ascmUnloadingPoint in listAscmUnloadingPoint)
                        {
                            AscmUnloadingPointMapLink ascmUnloadingPointMapLink = listMapLink.Find(P => P.pointId == ascmUnloadingPoint.id);
                            if (ascmUnloadingPointMapLink == null)
                            {
                                ascmUnloadingPointMapLink = new AscmUnloadingPointMapLink();
                                ascmUnloadingPointMapLink.id = ++maxId;
                                ascmUnloadingPointMapLink.createUser = userName;
                                ascmUnloadingPointMapLink.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                ascmUnloadingPointMapLink.modifyUser = userName;
                                ascmUnloadingPointMapLink.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                ascmUnloadingPointMapLink.mapId = mapId.Value;
                                ascmUnloadingPointMapLink.pointId = ascmUnloadingPoint.id;
                                listMapLinkAdd.Add(ascmUnloadingPointMapLink);
                            }
                        }
                        using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                        {
                            try
                            {
                                if (listMapLinkAdd.Count > 0)
                                    YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listMapLinkAdd);

                                tx.Commit();//正确执行提交
                                jsonObjectResult.result = true;
                                jsonObjectResult.message = "";
                            }
                            catch (Exception ex)
                            {
                                tx.Rollback();//回滚
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnloadingPointMapLinkRemove(string linkIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (!string.IsNullOrEmpty(linkIds))
                {
                    List<AscmUnloadingPointMapLink> listMapLink = AscmUnloadingPointMapLinkService.GetInstance().GetList("from AscmUnloadingPointMapLink where id in(" + linkIds + ")");
                    if (listMapLink != null)
                        AscmUnloadingPointMapLinkService.GetInstance().Delete(listMapLink);

                    jsonObjectResult.result = true;
                    jsonObjectResult.message = "";
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnloadingPointMapLinkEdit(int? id)
        {
            AscmUnloadingPointMapLink ascmUnloadingPointMapLink = null;
            try
            {
                if (id.HasValue)
                {
                    ascmUnloadingPointMapLink = AscmUnloadingPointMapLinkService.GetInstance().Get(id.Value);
                    if (ascmUnloadingPointMapLink != null)
                    {
                        ascmUnloadingPointMapLink.ascmUnloadingPoint = AscmUnloadingPointService.GetInstance().Get(ascmUnloadingPointMapLink.pointId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmUnloadingPointMapLink, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult UnloadingPointMapLinkSave(AscmUnloadingPointMapLink ascmUnloadingPointMapLink_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                AscmUnloadingPointMapLink ascmUnloadingPointMapLink = null;
                if (id.HasValue)
                    ascmUnloadingPointMapLink = AscmUnloadingPointMapLinkService.GetInstance().Get(id.Value);
                if (ascmUnloadingPointMapLink == null)
                    throw new Exception("保存卸货位地图关联卸货点失败！");

                ascmUnloadingPointMapLink.x = ascmUnloadingPointMapLink_Model.x;
                ascmUnloadingPointMapLink.y = ascmUnloadingPointMapLink_Model.y;
                if (!string.IsNullOrEmpty(ascmUnloadingPointMapLink_Model.memo))
                    ascmUnloadingPointMapLink.memo = ascmUnloadingPointMapLink_Model.memo.Trim();
                ascmUnloadingPointMapLink.modifyUser = userName;
                ascmUnloadingPointMapLink.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                AscmUnloadingPointMapLinkService.GetInstance().Update(ascmUnloadingPointMapLink);

                jsonObjectResult.result = true;
                jsonObjectResult.id = ascmUnloadingPointMapLink.id.ToString();
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        #endregion

        #region 卸货位监视
        public class DisplayUnloadingPointMonitor
        {
            private static string dl = "<dl>{0}</dl>";
            private static string dt = "<dt{0}>{1}</dt>";
            private static string dd = "<dd>{0}</dd>";
            private static string warehouseLegend = "<div class=\"warehouse_legend\">{0}</div>";
            private static string warehouse = "<div class=\"warehouse\"><h3>{0}</h3>{1}</div>";
            private static string unloadingPoint = "<div class=\"unloadingPoint\">{0}</div>";
            private static string img = "<img src=\"/Content/Images/Vehicle.jpg\" border=\"0\" alt=\"{0}\" />";

            public static string DisplayStatus(string status)
            {
                return Display(status, AscmUnloadingPoint.StatusDefine.DisplayText(status));
            }
            public static string DisplayUnloadingPoint(string status, string sn)
            {
                return Display(status, sn);
            }
            public static string Display(string status, string ddText)
            {
                string ddFormat = string.Format(dd, ddText);
                switch (status)
                {
                    case AscmUnloadingPoint.StatusDefine.idle:
                        return string.Format(dl, string.Format(dt, "", "") + ddFormat);
                    case AscmUnloadingPoint.StatusDefine.inUse:
                        return string.Format(dl, string.Format(dt, "", string.Format(img, ddText)) + ddFormat);
                    case AscmUnloadingPoint.StatusDefine.reserve:
                        return string.Format(dl, string.Format(dt, " class=\"blue\"", "") + ddFormat);
                    case AscmUnloadingPoint.StatusDefine.breakdown:
                        return string.Format(dl, string.Format(dt, " class=\"red\"", "") + ddFormat);
                    default: return "";
                }
            }
            public static string DisplayStatusList()
            {
                string display = "";
                List<string> listStatusDefine = AscmUnloadingPoint.StatusDefine.GetList();
                foreach (string statusDefine in listStatusDefine)
                {
                    display += DisplayStatus(statusDefine);
                }
                return display;
            }
            public static string DisplayWarehouseLegend()
            {
                return string.Format(warehouseLegend, DisplayStatusList());
            }
            public static string DisplayWarehouseList(List<AscmUnloadingPoint> list)
            {
                System.Text.StringBuilder display = new System.Text.StringBuilder();
                if (list != null)
                {
                    int rowNum = 6;
                    var groupByWarehouse = list.GroupBy(P => P.warehouseId);
                    foreach (IGrouping<string, AscmUnloadingPoint> ig in groupByWarehouse)
                    {
                        string displayRow = "";
                        string displayWarehouse = "";
                        List<AscmUnloadingPoint> listAscmUnloadingPoint = ig.OrderBy(P => P.id).ToList();
                        int count = listAscmUnloadingPoint.Count;
                        for (int i = 0; i < count; i++)
                        {
                            AscmUnloadingPoint ascmUnloadingPoint = listAscmUnloadingPoint[i];
                            displayRow += DisplayUnloadingPoint(ascmUnloadingPoint.status, ascmUnloadingPoint.name);
                            if (count == 1 || (i > 0 && (i + 1) % rowNum == 0) || (i == (count - 1) && count % rowNum != 0))
                            {
                                displayWarehouse += string.Format(unloadingPoint, displayRow);
                                displayRow = "";
                            }
                        }
                        display.Append(string.Format(warehouse, ig.First().warehouseDescription, displayWarehouse));
                    }
                }
                return display.ToString();
            }
        }
        public ActionResult UnloadingPointMonitor()
        {
            ViewData["warehouseLegend"] = DisplayUnloadingPointMonitor.DisplayWarehouseLegend();

            //卸货位监视
            return View();
        }
        public ContentResult LoadUnloadingPointMonitor(string warehouseId)
        {
            string result = "";
            try
            {
                string whereOther = "", whereWarehouse = "";
                if (!string.IsNullOrEmpty(warehouseId))
                    whereWarehouse = " warehouseId = '" + warehouseId + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);

                List<AscmUnloadingPoint> list = AscmUnloadingPointService.GetInstance().GetList(null, "", "", "", whereOther);
                result = DisplayUnloadingPointMonitor.DisplayWarehouseList(list);   
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Content(result);
        }
        public class DisplayWarehouseMap
        {
            public string dlCss { get; set; }
            public string dtCss { get; set; }
            public string ddCss { get; set; }
            public string warehouseMapCss { get; set; }
            public string dtImgCss { get; set; }
            public string display { get; set; }
        }
        public class DisplayUnloadingPointMapMonitor
        {
            public int dlWidth { get; set; }
            public int dlHeight { get; set; }

            private static string unloadingPoint = "<div style=\"position:relative;top:{0}px;left:{1}px;\">{2}</div>";

            public int GetDtWidth()
            {
                return dlWidth - 2;
            }
            public int GetDtHeight()
            {
                return dlHeight - 3 - GetDdHeight();
            }
            public int GetDdWidth()
            {
                return dlWidth - 6;
            }
            public int GetDdHeight()
            {
                return GetDdWidth() / 2;
            }
            public int GetDdLineHeight()
            {
                return GetDdHeight() - 2;
            }
            public int GetImgMaxHeight()
            {
                return GetDtHeight();
            }
            public int GetImgMaxWidth()
            {
                return GetDdWidth();
            }

            public DisplayWarehouseMap GetDisplayWarehouseMap(AscmUnloadingPointMap ascmUnloadingPointMap, List<AscmUnloadingPointMapLink> listAscmUnloadingPointMapLink)
            {
                DisplayWarehouseMap displayWarehouseMap = new DisplayWarehouseMap();
                dlWidth = ascmUnloadingPointMap.width;
                dlHeight = ascmUnloadingPointMap.height;
                displayWarehouseMap.dlCss = "{" + string.Format("\"width\":\"{0}px\",\"height\":\"{1}px\"", dlWidth, dlHeight) + "}";
                displayWarehouseMap.dtCss = "{" + string.Format("\"width\":\"{0}px\",\"height\":\"{1}px\"", GetDtWidth(), GetDtHeight()) + "}";
                displayWarehouseMap.ddCss = "{" + string.Format("\"width\":\"{0}px\",\"height\":\"{1}px\",\"line-height\":\"{2}px\"", GetDdWidth(), GetDdHeight(), GetDdLineHeight()) + "}";
                displayWarehouseMap.dtImgCss = "{" + string.Format("\"max-height\":\"{0}px\",\"max-width\":\"{1}px\"", GetImgMaxHeight(), GetImgMaxWidth()) + "}";
                displayWarehouseMap.warehouseMapCss = "{" + string.Format("\"width\":\"{0}px\",\"height\":\"{1}px\",\"background\":\"url({2}) 0px 0px no-repeat\"", ascmUnloadingPointMap.imgWidth, ascmUnloadingPointMap.imgHeight, ascmUnloadingPointMap.imgUrl) + "}";

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (AscmUnloadingPointMapLink ascmUnloadingPointMapLink in listAscmUnloadingPointMapLink)
                {
                    sb.Append(string.Format(unloadingPoint, ascmUnloadingPointMapLink.y, ascmUnloadingPointMapLink.x, DisplayUnloadingPointMonitor.DisplayUnloadingPoint(ascmUnloadingPointMapLink.pointStatus, ascmUnloadingPointMapLink.pointName)));
                }
                displayWarehouseMap.display = sb.ToString();

                return displayWarehouseMap;
            }
        }
        public ActionResult UnloadingPointMapMonitor()
        {
            ViewData["warehouseLegend"] = DisplayUnloadingPointMonitor.DisplayWarehouseLegend();

            //卸货位地图监视
            return View();
        }
        public ActionResult LoadUnloadingPointMapMonitor(string warehouseId, int? mapId)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (mapId.HasValue)
                {
                    AscmUnloadingPointMap ascmUnloadingPointMap = AscmUnloadingPointMapService.GetInstance().Get(mapId.Value);
                    if (ascmUnloadingPointMap != null)
                    {
                        List<AscmUnloadingPointMapLink> listAscmUnloadingPointMapLink = AscmUnloadingPointMapLinkService.GetInstance().GetList(ascmUnloadingPointMap.id);
                        DisplayUnloadingPointMapMonitor displayUnloadingPointMapMonitor = new DisplayUnloadingPointMapMonitor();
                        DisplayWarehouseMap displayWarehouseMap =
                            displayUnloadingPointMapMonitor.GetDisplayWarehouseMap(ascmUnloadingPointMap, listAscmUnloadingPointMapLink);

                        jsonObjectResult.result = true;
                        jsonObjectResult.entity = displayWarehouseMap;
                    }
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet); 
        }
        #endregion

        [YnFrame.Web.YnActionFilter(false)]
        public ActionResult LedUnloadingPointMapMonitor()
        {
            //卸货位地图监视
            return View();
        }
        [YnFrame.Web.YnActionFilter(false)]
        public ActionResult LedUnloadingPointMapMonitor1()
        {
            //卸货位地图监视
            return View();
        }

    }
}
