﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using YnBaseClass2.Web;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Services.Base;
using Newtonsoft.Json;
using MideaAscm.Dal;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Services.Warehouse;
using NHibernate;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Services.SupplierPreparation;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Services.FromErp;
using MideaAscm.Services.MesInterface;
using YnFrame.Dal.Entities;
using YnFrame.Services;
using System.Web.Security;
using NPOI.SS.UserModel;
using System.IO;
using MideaAscm.Dal.IEntity;
using MideaAscm.Services.IEntity;
using System.Data;
using System.Collections;
using Aspose.Words.Reporting;
using Aspose.Words;
using System.Reflection;
using MideaAscm.View.Services;
using MideaAscm.View.Dal.Entities;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Services.GetMaterialManage;
using MideaAscm.Code;

namespace MideaAscm.Areas.Ascm.Controllers
{
    public class WarehouseController : YnFrame.Controllers.YnBaseController
    {
        //
        // GET: /Ascm/Ckgl/
        //仓库管理
        public ActionResult Index()
        {
            return View();
        }

        #region 仓管人员
        public ActionResult WarehouseUserInfoList(int? page, int? rows, string sort, string order, string queryWord)
        {
            //YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            //ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            //ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string sql = @"select u.*
                              from ynuser                       u,
                                   yndepartment                 d,
                                   ynposition                   p,
                                   yndepartmentpositionuserlink l
                             where u.userid = l.userid
                               and d.id = l.departmentid
                               and p.id = l.positionid
                               and d.name = '财务部'
                               and p.name like '仓管员%'";
                if (!string.IsNullOrWhiteSpace(queryWord))
                    sql += " and (upper(u.userid) like '%" + queryWord.Trim().ToUpper() + "%' or upper(u.username) like '%" + queryWord.Trim().ToUpper() + "%')";
                sql += " order by u.userid";
                NHibernate.ISQLQuery query = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.FindBySQLQuery(sql);
                IList<YnUser> ilist = query.AddEntity("u", typeof(YnUser)).List<YnUser>();
                if (ilist != null && ilist.Count > 0)
                    jsonDataGridResult.rows.AddRange(ilist.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WarehouseUserInfoIndex()
        {
            return View();
        }
        [HttpPost]
        public ContentResult WarehouseUserInfoSave(YnUser ynUser_model, string id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                YnUser ynUser = null;
                if (!string.IsNullOrEmpty(id))
                    ynUser = YnUserService.GetInstance().Get(id);
                else
                    ynUser = new YnUser();
                if (ynUser_model == null)
                    throw new Exception("获取仓管员信息失败");
                //object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from YnUser where userId<>'" + ynUser_model.userId +"' and userName='" + ynUser_model.userName+"'");
                //if (object1 == null)
                //    throw new Exception("查询异常！");
                //int count = 0;
                //if (int.TryParse(object1.ToString(), out count) && count > 0)
                //    throw new Exception("已存在仓管员账号或仓管员姓名");

                if (string.IsNullOrEmpty(id))
                {
                    ynUser.userId = ynUser_model.userId;
                    ynUser.extExpandType = "wms";
                }
                ynUser.userName = ynUser_model.userName;
                ynUser.sex = ynUser_model.sex;
                ynUser.email = ynUser_model.email;
                ynUser.officeTel = ynUser_model.officeTel;
                ynUser.mobileTel = ynUser_model.mobileTel;
                ynUser.isAccountLocked = ynUser_model.isAccountLocked;
                if (!string.IsNullOrEmpty(id))
                    YnUserService.GetInstance().Update(ynUser);
                else
                    YnUserService.GetInstance().Save(ynUser);

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ynUser.userId;
                jsonObjectResult.entity = ynUser;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult WarehouseUserList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmUserInfo> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string organizationId = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketOrganizationId();

                string where = " extExpandType='wms'";
                list = AscmUserInfoService.GetInstance().GetList(ynPage, sort, order, queryWord, where, false, false);
                foreach (AscmUserInfo ascmUserInfo in list)
                {
                    jsonDataGridResult.rows.Add(ascmUserInfo.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WmsDepPositionUserIndex()
        {
            return View();
        }
        public ActionResult WmsDepartmentTree(int? withPosition)
        {
            List<YnDepartment> list = null;
            List<JsonTreeResult> listJsonTreeResult = new List<JsonTreeResult>();
            try
            {
                int? id = null;
                string organizationId = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketOrganizationId();
                list = YnDepartmentService.GetInstance().GetList(organizationId, 1, null, null);
                foreach (YnDepartment ynDepartment in list)
                {
                    if (ynDepartment.name == "财务部")
                    {
                        JsonTreeResult jsonTreeResult = new JsonTreeResult();
                        jsonTreeResult.id = ynDepartment.id;
                        jsonTreeResult.text = ynDepartment.name;
                        jsonTreeResult.state = ynDepartment.state;
                        id = ynDepartment.id;

                        if (withPosition.HasValue && withPosition.Value == 1)
                        {
                            List<YnPosition> listYnPosition = YnPositionService.GetInstance().GetListInDepartment(ynDepartment.id);
                            if (listYnPosition != null && listYnPosition.Count > 0)
                            {
                                foreach (YnPosition ynPosition in listYnPosition.OrderBy(P => P.sortNo))
                                {
                                    JsonTreeResult jsonTreeResult_Position = new JsonTreeResult();
                                    jsonTreeResult_Position.id = 1000 + ynPosition.id;
                                    jsonTreeResult_Position.text = "<img alt='' border='0' src='" + (Request.ApplicationPath == "/" ? "" : Request.ApplicationPath) + "/Areas/YnPublic/Content/images/info.gif' width='16' height='16'  style='vertical-align:middle;'/>" + ynPosition.name;
                                    jsonTreeResult_Position.attributes = ynPosition;
                                    jsonTreeResult.children.Add(jsonTreeResult_Position);
                                }
                            }
                            //if (listYnPosition.Count > 0)
                            //{
                            //    jsonTreeResult.state = "closed";
                            //}
                        }

                        listJsonTreeResult.Add(jsonTreeResult);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(listJsonTreeResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult WmsMesUserSave(YnUser ynUser_Model, string id, int? departmentId, int? positionId)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string organizationId = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketOrganizationId();

                YnUser ynUser = null;
                if (!string.IsNullOrEmpty(id))
                {
                    ynUser = YnUserService.GetInstance().Get(id);
                }
                else
                {
                    ynUser = new YnUser();
                    ynUser.userId = ynUser_Model.userId.Trim();
                    if (ynUser_Model.userId == null || ynUser_Model.userId.Trim() == "")
                        throw new Exception("用户ID不能为空！");
                    ynUser.organizationId = 0;
                    if (!string.IsNullOrEmpty(organizationId))
                        ynUser.organizationId = Convert.ToInt32(organizationId);
                }

                if (ynUser == null)
                    throw new Exception("保存用户失败！");
                if (ynUser_Model.userName == null || ynUser_Model.userName.Trim() == "")
                    throw new Exception("用户姓名不能为空！");

                ynUser.userName = ynUser_Model.userName.Trim();
                ynUser.sex = ynUser_Model.sex.Trim();
                ynUser.email = ynUser_Model.email;
                ynUser.officeTel = ynUser_Model.officeTel;
                ynUser.mobileTel = ynUser_Model.mobileTel;
                ynUser.isAccountLocked = ynUser_Model.isAccountLocked;
                ynUser.extExpandType = "mes";
                ynUser.extExpandId = ynUser.userId;

                if (!string.IsNullOrEmpty(id))
                {
                    YnUserService.GetInstance().Update(ynUser);
                }
                else
                {
                    string _organizationWhere = "";
                    if (!string.IsNullOrEmpty(organizationId))
                        _organizationWhere = " organizationId=" + organizationId + " and ";

                    object object1 = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from YnUser where " + _organizationWhere + " userId='" + ynUser_Model.userId.Trim() + "'");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已经存在此用户【" + ynUser_Model.userId.Trim() + "】！");
                    YnUserService.GetInstance().Save(ynUser);

                    if (departmentId.HasValue && positionId.HasValue)
                    {
                        List<YnDepartmentPositionUserLink> listYnDepartmentPositionUserLink = new List<YnDepartmentPositionUserLink>();
                        YnDepartmentPositionUserLinkPK ynDepartmentPositionUserLinkPK = new YnDepartmentPositionUserLinkPK();
                        ynDepartmentPositionUserLinkPK.departmentId = departmentId.Value;
                        ynDepartmentPositionUserLinkPK.positionId = positionId.Value;
                        ynDepartmentPositionUserLinkPK.userId = ynUser.userId;

                        YnDepartmentPositionUserLink _ynDepartmentPositionUserLink = YnDepartmentPositionUserLinkService.GetInstance().Get(ynDepartmentPositionUserLinkPK);
                        if (_ynDepartmentPositionUserLink == null)
                        {
                            YnDepartmentPositionUserLink ynDepartmentPositionUserLink = new YnDepartmentPositionUserLink();
                            ynDepartmentPositionUserLink.ids = ynDepartmentPositionUserLinkPK;

                            listYnDepartmentPositionUserLink.Add(ynDepartmentPositionUserLink);
                            YnDepartmentPositionUserLinkService.GetInstance().Save(listYnDepartmentPositionUserLink);
                        }

                        jsonObjectResult.result = true;
                        jsonObjectResult.message = "";
                    }
                }


                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ynUser.userId.ToString();
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult WmsMesUserDelete(string userIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (!string.IsNullOrEmpty(userIds))
                {
                    string[] arrId = userIds.Split(',');
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            foreach (string id in arrId)
                            {
                                if (string.IsNullOrEmpty(id))
                                    continue;
                                YnUser ynUser = YnUserService.GetInstance().Get(id);
                                if (ynUser == null)
                                    throw new Exception("获取用户失败！");

                                YnUserService.GetInstance().Delete(id);
                            }


                            tx.Commit();//正确执行提交

                            jsonObjectResult.result = true;
                            jsonObjectResult.message = "";
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete YnUser)", ex);
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
        public ActionResult WarehouseUserAscxList(int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                List<AscmUserInfo> list = AscmUserInfoService.GetInstance().GetList(ynPage, sort, order, q, "extExpandType='mes'", false, false);
                //List<AscmUserInfo> list = AscmUserInfoService.GetInstance().GetList(ynPage, sort, order, q, "", false, false);//测试
                if (list != null && list.Count() > 0)
                {
                    foreach (AscmUserInfo ascmUserInfo in list)
                    {
                        jsonDataGridResult.rows.Add(ascmUserInfo);
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

        #region 厂房
        public ActionResult WorkshopBuildingIndex()
        {
            return View();
        }
        public ActionResult WorkshopBuildingList(string sort, string order)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                List<AscmWorkshopBuilding> list = AscmWorkshopBuildingService.GetInstance().GetList(sort, order);
                if (list != null)
                {
                    foreach (AscmWorkshopBuilding ascmWorkshopBuilding in list)
                    {
                        jsonDataGridResult.rows.Add(ascmWorkshopBuilding);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WorkshopBuildingEdit(int? id)
        {
            AscmWorkshopBuilding ascmWorkshopBuilding = null;
            try
            {
                if (id.HasValue)
                {
                    ascmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().Get(id.Value);
                    if (ascmWorkshopBuilding != null && !string.IsNullOrEmpty(ascmWorkshopBuilding.vertical))
                    {
                        ascmWorkshopBuilding.vertical =
                            MideaAscm.Code.AscmUtl.GetInstance().ConvertLetterToInt(ascmWorkshopBuilding.vertical).ToString();
                    }
                }
                ascmWorkshopBuilding = ascmWorkshopBuilding ?? new AscmWorkshopBuilding();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmWorkshopBuilding, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult WorkshopBuildingSave(AscmWorkshopBuilding ascmWorkshopBuilding_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (ascmWorkshopBuilding_Model.name == null || ascmWorkshopBuilding_Model.name.Trim() == "")
                    throw new Exception("厂房名称不能为空");
                if (ascmWorkshopBuilding_Model.code == null || ascmWorkshopBuilding_Model.code.Trim() == "")
                    throw new Exception("厂房代号不能为空");
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^(\d|[a-z]|[A-Z])$");
                if (!reg.IsMatch(ascmWorkshopBuilding_Model.code))
                    throw new Exception("厂房代号只能是单个数字或字母");
                int iVertical = 0;
                string vertical = string.Empty;
                if (int.TryParse(ascmWorkshopBuilding_Model.vertical, out iVertical))
                    vertical = MideaAscm.Code.AscmUtl.GetInstance().ConvertIntToLetter(iVertical);

                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;

                AscmWorkshopBuilding ascmWorkshopBuilding = null;
                List<AscmWarelocation> listAscmWarelocation = null;
                List<AscmWarelocation> listAscmWarelocationDelete = null;
                List<AscmBuildingWarehouseLink> listAscmBuildingWarehouseLink = null;
                if (id.HasValue)
                {
                    ascmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().Get(id.Value);
                    if (ascmWorkshopBuilding == null)
                        throw new Exception("找不到厂房");
                    //厂房代号变更
                    if (ascmWorkshopBuilding.code != ascmWorkshopBuilding_Model.code)
                    {
                        //当厂房存在货位
                        //1>、如果货位关联了物料，则禁止更改代号
                        string sql = "select count(*) from AscmLocationMaterialLink where warelocationId in(select id from AscmWarelocation where buildingId=" + id.Value + ")";
                        object obj1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(sql);
                        if (obj1 != null)
                        {
                            int _count = 0;
                            if (int.TryParse(obj1.ToString(), out _count) && _count > 0)
                                throw new Exception("厂房货位存在物料，不能更改厂房代号");
                        }
                        //2>、如果货位没有关联物料，则同步更新货位编码
                        listAscmWarelocation = AscmWarelocationService.GetInstance().GetList("", "", id, "", "");
                        if (listAscmWarelocation != null && listAscmWarelocation.Count > 0)
                        {
                            foreach (AscmWarelocation ascmWarelocation in listAscmWarelocation)
                            {
                                if (!string.IsNullOrEmpty(ascmWarelocation.docNumber))
                                {
                                    string code = ascmWarelocation.docNumber.Substring(0, 1);
                                    ascmWarelocation.docNumber = ascmWarelocation.docNumber.Replace(code, ascmWorkshopBuilding_Model.code);
                                    ascmWarelocation.modifyUser = userName;
                                    ascmWarelocation.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                }
                            }
                        }
                    }
                    //区域范围调整处理
                    if (!string.IsNullOrEmpty(ascmWorkshopBuilding.vertical))
                    {
                        int origH = ascmWorkshopBuilding.horizontal;
                        int origV = MideaAscm.Code.AscmUtl.GetInstance().ConvertLetterToInt(ascmWorkshopBuilding.vertical);
                        //如果区域范围缩小
                        if (origH > ascmWorkshopBuilding_Model.horizontal || origV > iVertical)
                        {
                            //获取新坐标范围外的区域
                            string buildingAreas = string.Empty;
                            for (int iV = 65; iV <= origV; iV++)
                            {
                                for (int iH = 1; iH <= origH; iH++)
                                {
                                    if (iV > iVertical)
                                    {
                                        if (!string.IsNullOrEmpty(buildingAreas))
                                            buildingAreas += ",";
                                        buildingAreas += "'" + MideaAscm.Code.AscmUtl.GetInstance().ConvertIntToLetter(iV) + iH + "'";
                                    }
                                    else if (iH > ascmWorkshopBuilding_Model.horizontal)
                                    {
                                        if (!string.IsNullOrEmpty(buildingAreas))
                                            buildingAreas += ",";
                                        buildingAreas += "'" + MideaAscm.Code.AscmUtl.GetInstance().ConvertIntToLetter(iV) + iH + "'";
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(buildingAreas))
                            {
                                //1>、如果坐标范围外的货位关联了物料，则禁止更改坐标
                                string sql = "select count(*) from AscmLocationMaterialLink where warelocationId in(select id from AscmWarelocation where buildingId=" + id.Value + " and buildingArea in(" + buildingAreas + "))";
                                object obj1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(sql);
                                if (obj1 != null)
                                {
                                    int _count = 0;
                                    if (int.TryParse(obj1.ToString(), out _count) && _count > 0)
                                        throw new Exception("坐标范围外的货位存在物料，不能缩小坐标");
                                }
                                //2>、如果坐标范围外的货位没有关联物料，则同步更新货位编码
                                if (listAscmWarelocation == null)
                                    listAscmWarelocation = AscmWarelocationService.GetInstance().GetList("", "", id, "", "");
                                if (listAscmWarelocation != null && listAscmWarelocation.Count > 0)
                                {
                                    listAscmWarelocationDelete = listAscmWarelocation.Where(P => buildingAreas.Replace("'", "").Split(',').Contains(P.buildingArea)).ToList();
                                    if (listAscmWarelocationDelete != null && listAscmWarelocationDelete.Count > 0)
                                    {
                                        listAscmWarelocation = listAscmWarelocation.Except(listAscmWarelocationDelete).ToList();
                                        //删除货位后，删除不含货位的子库关联
                                        var ieWarehouseId = listAscmWarelocationDelete.Select(P => P.warehouseId).Distinct()
                                            .Where(P => !listAscmWarelocation.Select(T => T.warehouseId).Distinct().Contains(P));
                                        if (ieWarehouseId != null)
                                        {
                                            string warehouseIds = string.Empty;
                                            foreach (string warehouseId in ieWarehouseId)
                                            {
                                                if (!string.IsNullOrEmpty(warehouseIds))
                                                    warehouseIds += ",";
                                                warehouseIds += "'" + warehouseId + "'";
                                            }
                                            if (!string.IsNullOrEmpty(warehouseIds))
                                            {
                                                sql = "from AscmBuildingWarehouseLink where buildingId=" + id.Value + " and warehouseId in(" + warehouseIds + ")";
                                                listAscmBuildingWarehouseLink = AscmBuildingWarehouseLinkService.GetInstance().GetList(sql, false);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    ascmWorkshopBuilding = new AscmWorkshopBuilding();
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWorkshopBuilding");
                    ascmWorkshopBuilding.id = ++maxId;
                    ascmWorkshopBuilding.createUser = userName;
                    ascmWorkshopBuilding.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                ascmWorkshopBuilding.name = ascmWorkshopBuilding_Model.name.Trim();
                ascmWorkshopBuilding.code = ascmWorkshopBuilding_Model.code;

                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmWorkshopBuilding where id<>" + ascmWorkshopBuilding.id + " and (name='" + ascmWorkshopBuilding.name + "' or code='" + ascmWorkshopBuilding.code + "')");
                if (object1 == null)
                    throw new Exception("查询异常！");
                int count = 0;
                if (int.TryParse(object1.ToString(), out count) && count > 0)
                    throw new Exception("已存在厂房名称或厂房代号");

                ascmWorkshopBuilding.modifyUser = userName;
                ascmWorkshopBuilding.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmWorkshopBuilding.sortNo = ascmWorkshopBuilding_Model.sortNo;
                if (!string.IsNullOrEmpty(ascmWorkshopBuilding_Model.description))
                    ascmWorkshopBuilding.description = ascmWorkshopBuilding_Model.description.Trim();
                ascmWorkshopBuilding.horizontal = ascmWorkshopBuilding_Model.horizontal;
                if (!string.IsNullOrEmpty(vertical))
                    ascmWorkshopBuilding.vertical = vertical;

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        //保存厂房
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdate(ascmWorkshopBuilding);
                        //更新厂房货位
                        if (listAscmWarelocation != null && listAscmWarelocation.Count > 0)
                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmWarelocation);
                        //删除厂房货位
                        if (listAscmWarelocationDelete != null && listAscmWarelocationDelete.Count > 0)
                            YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWarelocationDelete);
                        //删除厂房与子库的关联
                        if (listAscmBuildingWarehouseLink != null && listAscmBuildingWarehouseLink.Count > 0)
                            YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmBuildingWarehouseLink);

                        tx.Commit();//正确执行提交
                        jsonObjectResult.result = true;
                        jsonObjectResult.id = ascmWorkshopBuilding.id.ToString();
                        jsonObjectResult.entity = ascmWorkshopBuilding;
                        jsonObjectResult.message = "";
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
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult WorkshopBuildingDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (!id.HasValue)
                    throw new Exception("系统错误：参数传值错误");

                AscmWorkshopBuilding ascmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().Get(id.Value);
                if (ascmWorkshopBuilding == null)
                    throw new Exception("系统错误：厂房不存在");

                //当厂房存在货位
                //1>、如果货位关联了物料，则禁止删除
                string sql = "select count(*) from AscmLocationMaterialLink where warelocationId in(select id from AscmWarelocation where buildingId=" + id.Value + ")";
                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(sql);
                if (object1 != null)
                {
                    int count = 0;
                    if (int.TryParse(object1.ToString(), out count) && count > 0)
                        throw new Exception("厂房货位存在物料");
                }
                //2>、如果货位没有关联物料，一并删除货位
                List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList("docNumber", "asc", id, "", "");

                //获取厂房与子库的关联
                List<AscmBuildingWarehouseLink> listAscmBuildingWarehouseLink =
                    AscmBuildingWarehouseLinkService.GetInstance().GetList(ascmWorkshopBuilding.id, false);

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        //删除厂房
                        YnDaoHelper.GetInstance().nHibernateHelper.Delete(ascmWorkshopBuilding);
                        //删除厂房与子库的关联
                        if (listAscmBuildingWarehouseLink != null && listAscmBuildingWarehouseLink.Count > 0)
                            YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmBuildingWarehouseLink);
                        //删除厂房货位
                        if (listAscmWarelocation != null && listAscmWarelocation.Count > 0)
                            YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWarelocation);

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
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        /* 厂房与子库关联 */
        public ActionResult BuildingWarehouseLinkList(int? buildingId)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (buildingId.HasValue)
                {
                    List<AscmBuildingWarehouseLink> list = AscmBuildingWarehouseLinkService.GetInstance().GetList(buildingId.Value);
                    if (list != null)
                    {
                        foreach (AscmBuildingWarehouseLink ascmBuildingWarehouseLink in list)
                        {
                            jsonDataGridResult.rows.Add(ascmBuildingWarehouseLink);
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
        /// <summary>厂房区域与子库关联</summary>
        public ActionResult BuildingWarehouseLinkAdd(int? id, string buildingAreas, string warehouseId)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                #region 验证
                if (!id.HasValue)
                    throw new Exception("系统错误：参数厂房ID传值错误");
                if (string.IsNullOrEmpty(buildingAreas))
                    throw new Exception("系统错误：参数厂房区域传值错误");
                if (string.IsNullOrEmpty(warehouseId))
                    throw new Exception("系统错误：参数子库ID传值错误");
                AscmWorkshopBuilding ascmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().Get(id.Value);
                if (ascmWorkshopBuilding == null)
                    throw new Exception("系统错误：获取厂房失败");
                AscmWarehouse ascmWarehouse = AscmWarehouseService.GetInstance().Get(warehouseId);
                if (ascmWarehouse == null)
                    throw new Exception("系统错误：获取子库失败");
                #endregion

                #region 厂房与子库关联
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;

                List<AscmBuildingWarehouseLink> listBuildingWarehouseLink = AscmBuildingWarehouseLinkService.GetInstance().GetList(id.Value);
                AscmBuildingWarehouseLink ascmBuildingWarehouseLink =
                    listBuildingWarehouseLink.Find(P => P.buildingId == id.Value && P.warehouseId == warehouseId);
                if (ascmBuildingWarehouseLink == null)
                {
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmBuildingWarehouseLink");
                    ascmBuildingWarehouseLink = new AscmBuildingWarehouseLink();
                    ascmBuildingWarehouseLink.id = ++maxId;
                    ascmBuildingWarehouseLink.createUser = userName;
                    ascmBuildingWarehouseLink.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmBuildingWarehouseLink.buildingId = id.Value;
                    ascmBuildingWarehouseLink.warehouseId = warehouseId;
                }
                ascmBuildingWarehouseLink.modifyUser = userName;
                ascmBuildingWarehouseLink.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                #endregion

                #region 货位与子库关联
                string whereOther = "";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "buildingId=" + id.Value);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "buildingArea in('" + buildingAreas.Replace(",", "','") + "')");
                List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList("", "", "", whereOther);
                List<AscmWarelocation> listAscmWarelocationUpdate = null;
                if (listAscmWarelocation != null)
                {
                    listAscmWarelocationUpdate = new List<AscmWarelocation>();
                    foreach (AscmWarelocation ascmWarelocation in listAscmWarelocation)
                    {
                        if (string.IsNullOrEmpty(ascmWarelocation.warehouseId))
                        {
                            ascmWarelocation.warehouseId = warehouseId;
                            ascmWarelocation.modifyUser = userName;
                            ascmWarelocation.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            listAscmWarelocationUpdate.Add(ascmWarelocation);
                        }
                    }
                }
                #endregion

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdate(ascmBuildingWarehouseLink);

                        if (listAscmWarelocationUpdate != null && listAscmWarelocationUpdate.Count > 0)
                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmWarelocationUpdate);

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
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BuildingWarehouseLinkRemove(string linkIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (string.IsNullOrEmpty(linkIds))
                    throw new Exception("系统错误：参数传值错误");

                List<AscmBuildingWarehouseLink> listBuildingWarehouseLink = AscmBuildingWarehouseLinkService.GetInstance().GetList("from AscmBuildingWarehouseLink where id in(" + linkIds + ")");
                if (listBuildingWarehouseLink != null)
                    AscmBuildingWarehouseLinkService.GetInstance().Delete(listBuildingWarehouseLink);

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BuildingWarehouseLinkEdit(int? id)
        {
            AscmBuildingWarehouseLink ascmBuildingWarehouseLink = null;
            try
            {
                if (id.HasValue)
                {
                    ascmBuildingWarehouseLink = AscmBuildingWarehouseLinkService.GetInstance().Get(id.Value);
                    if (ascmBuildingWarehouseLink != null)
                    {
                        ascmBuildingWarehouseLink.ascmWarehouse = AscmWarehouseService.GetInstance().Get(ascmBuildingWarehouseLink.warehouseId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmBuildingWarehouseLink, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult BuildingWarehouseLinkSave(AscmBuildingWarehouseLink ascmBuildingWarehouseLink_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;

                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^(\d|[a-z]|[A-Z])$");
                if (!reg.IsMatch(ascmBuildingWarehouseLink_Model.warehouseCode))
                    throw new Exception("子库代号不能为空且只能是单个数字或字母");

                AscmBuildingWarehouseLink ascmBuildingWarehouseLink = null;
                if (id.HasValue)
                    ascmBuildingWarehouseLink = AscmBuildingWarehouseLinkService.GetInstance().Get(id.Value);
                if (ascmBuildingWarehouseLink == null)
                    throw new Exception("保存厂房与子库的关联失败！");

                AscmWorkshopBuilding ascmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().Get(ascmBuildingWarehouseLink.buildingId);
                if (ascmWorkshopBuilding == null)
                    throw new Exception("找不到厂房");
                ascmBuildingWarehouseLink.warehouseCode = ascmBuildingWarehouseLink_Model.warehouseCode;

                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmBuildingWarehouseLink where id<>" + ascmBuildingWarehouseLink.id + " and buildingId=" + ascmBuildingWarehouseLink.buildingId + " and warehouseCode='" + ascmBuildingWarehouseLink.warehouseCode + "'");
                if (object1 == null)
                    throw new Exception("查询异常！");
                int count = 0;
                if (int.TryParse(object1.ToString(), out count) && count > 0)
                    throw new Exception("厂房【" + ascmWorkshopBuilding.name + "】中已存在子库代号【" + ascmBuildingWarehouseLink.warehouseCode + "】");

                ascmBuildingWarehouseLink.modifyUser = userName;
                ascmBuildingWarehouseLink.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                AscmBuildingWarehouseLinkService.GetInstance().Update(ascmBuildingWarehouseLink);

                jsonObjectResult.result = true;
                jsonObjectResult.id = ascmBuildingWarehouseLink.id.ToString();
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        /// <summary>厂房区域图</summary>
        public ActionResult WorkshopBuildingMap(int? id)
        {
            AscmWorkshopBuilding ascmWorkshopBuilding = null;
            try
            {
                if (id.HasValue)
                {
                    ascmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().Get(id.Value);
                    //List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList("from AscmWarelocation where buildingId=" + ascmWorkshopBuilding.id);
                    //int x = ascmWorkshopBuilding.horizontal;
                    //int y = 0;
                    //if (!string.IsNullOrEmpty(ascmWorkshopBuilding.vertical))
                    //    y = MideaAscm.Code.AscmUtl.GetInstance().ConvertLetterToInt(ascmWorkshopBuilding.vertical);
                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //sb.Append("<table class=\"buildingAreaTable\" id=\"tBuildingArea\">");
                    //for (int i = 65; i <= y; i++)
                    //{
                    //    string vertical = MideaAscm.Code.AscmUtl.GetInstance().ConvertIntToLetter(i);
                    //    sb.Append("<tr>");
                    //    for (int j = 1; j <= x; j++)
                    //    {
                    //        string areaField = "";
                    //        string maxDocNumber = listAscmWarelocation.Where(P => P.buildingArea == (vertical + j)).Max(P => P.docNumber);
                    //        if (!string.IsNullOrEmpty(maxDocNumber))
                    //        {
                    //            int length = maxDocNumber.Length;
                    //            areaField = string.Join("*", maxDocNumber.Substring(length - 2, 1), maxDocNumber.Substring(length - 3, 1), maxDocNumber.Substring(length - 1, 1));
                    //        }
                    //        string areaField1 = "<div style='float:center;'>" + vertical + j + "</div>";
                    //        string areaField2 = "<div style='float:center;font-size:15px;color:#FFFFFF;padding-top:30px;'>&nbsp;" + areaField + "</div>";
                    //        sb.Append("<td id=\"" + vertical + j + "\">" + areaField1 + areaField2 + "</td>");
                    //    }
                    //    sb.Append("</tr>");
                    //}
                    //sb.Append("</table>");
                    //ascmWorkshopBuilding.buildingArea = sb.ToString();
                }
                if (ascmWorkshopBuilding == null)
                    ascmWorkshopBuilding = new AscmWorkshopBuilding();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ascmWorkshopBuilding);
        }
        public ActionResult LoadWorkshopBuildingMap(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    AscmWorkshopBuilding ascmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().Get(id.Value);
                    if (ascmWorkshopBuilding != null)
                    {
                        List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList("from AscmWarelocation where buildingId=" + ascmWorkshopBuilding.id);
                        int x = ascmWorkshopBuilding.horizontal;
                        int y = 0;
                        if (!string.IsNullOrEmpty(ascmWorkshopBuilding.vertical))
                            y = MideaAscm.Code.AscmUtl.GetInstance().ConvertLetterToInt(ascmWorkshopBuilding.vertical);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<table class=\"buildingAreaTable\" id=\"tBuildingArea\">");
                        for (int i = 65; i <= y; i++)
                        {
                            string vertical = MideaAscm.Code.AscmUtl.GetInstance().ConvertIntToLetter(i);
                            sb.Append("<tr>");
                            for (int j = 1; j <= x; j++)
                            {
                                string areaField = "";
                                string maxDocNumber = listAscmWarelocation.Where(P => P.buildingArea == (vertical + j)).Max(P => P.docNumber);
                                if (!string.IsNullOrEmpty(maxDocNumber))
                                {
                                    int length = maxDocNumber.Length;
                                    areaField = string.Join("*", maxDocNumber.Substring(length - 2, 1), maxDocNumber.Substring(length - 3, 1), maxDocNumber.Substring(length - 1, 1));
                                }
                                string areaField1 = "<div style='float:center;'>" + vertical + j + "</div>";
                                string areaField2 = "<div style='float:center;font-size:15px;color:#FFFFFF;padding-top:30px;'>&nbsp;" + areaField + "</div>";
                                sb.Append("<td id=\"" + vertical + j + "\">" + areaField1 + areaField2 + "</td>");
                            }
                            sb.Append("</tr>");
                        }
                        sb.Append("</table>");
                        jsonObjectResult.result = true;
                        jsonObjectResult.entity = sb.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BuildingAreaWarelocationSave(int? id, int horizontal, int vertical, int layer, string buildingAreas)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (!id.HasValue)
                    throw new Exception("系统错误：参数厂房Id传值错误");
                AscmWorkshopBuilding ascmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().Get(id.Value);
                if (ascmWorkshopBuilding == null)
                    throw new Exception("系统错误：找不到厂房");
                if (buildingAreas == null || buildingAreas.Trim() == "")
                    throw new Exception("系统错误：区域参数传值错误");

                string _vertical = MideaAscm.Code.AscmUtl.GetInstance().ConvertIntToLetter(vertical);
                List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList("", "", id, "", "buildingArea in('" + buildingAreas.Replace(",", "','") + "')");
                List<AscmWarelocation> listAscmWarelocationDelete = null;
                if (listAscmWarelocation != null && listAscmWarelocation.Count > 0)
                {
                    listAscmWarelocationDelete = new List<AscmWarelocation>();
                    var groupbyBuildingArea = listAscmWarelocation.GroupBy(P => P.buildingArea);
                    foreach (IGrouping<string, AscmWarelocation> ig in groupbyBuildingArea)
                    {
                        string maxShelfNo = ig.Max(P => P.shelfNo);
                        int origH = Convert.ToInt32(maxShelfNo.Substring(1, 1));
                        int origV = MideaAscm.Code.AscmUtl.GetInstance().ConvertLetterToInt(maxShelfNo.Substring(0, 1));
                        List<string> listShelfNo = new List<string>();
                        //如果坐标范围缩小
                        if (origH > horizontal || origV > vertical)
                        {
                            //获取新坐标范围外的货架
                            for (int iV = 65; iV <= origV; iV++)
                            {
                                for (int iH = 1; iH <= origH; iH++)
                                {
                                    if (iV > vertical)
                                    {
                                        listShelfNo.Add(MideaAscm.Code.AscmUtl.GetInstance().ConvertIntToLetter(iV) + iH);
                                    }
                                    else if (iH > horizontal)
                                    {
                                        listShelfNo.Add(MideaAscm.Code.AscmUtl.GetInstance().ConvertIntToLetter(iV) + iH);
                                    }
                                }
                            }
                        }
                        List<AscmWarelocation> findAscmWarelocation = ig.Where(P => listShelfNo.Contains(P.shelfNo) || P.layer > layer).ToList();
                        if (findAscmWarelocation.Count > 0)
                        {
                            string ids = string.Empty;
                            foreach (AscmWarelocation ascmWarelocation in findAscmWarelocation)
                            {
                                if (!string.IsNullOrEmpty(ids))
                                    ids += ",";
                                ids += ascmWarelocation.id;
                            }
                            if (!string.IsNullOrEmpty(ids))
                            {
                                //如果坐标范围外的货位关联了物料，则禁止更改坐标
                                string sql = "select count(*) from AscmLocationMaterialLink where warelocationId in(" + ids + ")";
                                object obj1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(sql);
                                if (obj1 != null)
                                {
                                    int _count = 0;
                                    if (int.TryParse(obj1.ToString(), out _count) && _count > 0)
                                        throw new Exception("厂房区域【" + ig.Key + "】坐标范围外的货位存在物料，不能缩小坐标或层");
                                }
                            }
                        }
                        listAscmWarelocationDelete.AddRange(findAscmWarelocation);
                    }
                }

                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;

                List<AscmWarelocation> listAscmWarelocationAdd = new List<AscmWarelocation>();
                int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWarelocation");
                //支持对多个区域同时添加货位
                string[] arrayBuildingArea = buildingAreas.Split(',');
                for (int i = 0; i < arrayBuildingArea.Length; i++)
                {
                    for (int v = 65; v <= vertical; v++)
                    {
                        for (int h = 1; h <= horizontal; h++)
                        {
                            for (int l = 1; l <= layer; l++)
                            {
                                string shelfNo = MideaAscm.Code.AscmUtl.GetInstance().ConvertIntToLetter(v) + h;
                                string docNumber = ascmWorkshopBuilding.code + arrayBuildingArea[i] + shelfNo + l;
                                if (listAscmWarelocation != null && listAscmWarelocation.Count > 0)
                                {
                                    if (listAscmWarelocation.Exists(P => P.docNumber == docNumber))
                                        continue;
                                }
                                AscmWarelocation ascmWarelocation = new AscmWarelocation();
                                ascmWarelocation.id = ++maxId;
                                ascmWarelocation.createUser = userName;
                                ascmWarelocation.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                ascmWarelocation.modifyUser = userName;
                                ascmWarelocation.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                ascmWarelocation.buildingId = ascmWorkshopBuilding.id;
                                ascmWarelocation.buildingArea = arrayBuildingArea[i];
                                ascmWarelocation.layer = l;
                                ascmWarelocation.shelfNo = shelfNo;
                                ascmWarelocation.docNumber = docNumber;
                                listAscmWarelocationAdd.Add(ascmWarelocation);
                            }
                        }
                    }
                }

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        if (listAscmWarelocationAdd.Count > 0)
                            YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWarelocationAdd);

                        if (listAscmWarelocationDelete != null && listAscmWarelocationDelete.Count > 0)
                            YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWarelocationDelete);

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
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        /// <summary>载入厂房区域货位图</summary>
        public ActionResult LoadBuildingLocationMap(int? id, string buildingArea)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (!id.HasValue)
                    throw new Exception("系统错误：参数厂房ID传值错误");
                AscmWorkshopBuilding ascmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().Get(id.Value);
                if (ascmWorkshopBuilding == null)
                    throw new Exception("系统错误：找不到厂房");
                if (string.IsNullOrEmpty(buildingArea))
                    throw new Exception("系统错误：参数厂房区域传值错误");
                string whereOther = "";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " buildingId=" + id.Value);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " buildingArea='" + buildingArea.Trim() + "'");
                List<AscmWarelocation> listAscmWarelocation =
                    AscmWarelocationService.GetInstance().GetList(null, "", "", "", whereOther, false, false);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (listAscmWarelocation != null && listAscmWarelocation.Count > 0)
                {
                    sb.Append("<table class=\"buildingAreaLocationTable\" id=\"tBuildingAreaLocation\">");
                    string maxShelfNo = listAscmWarelocation.Max(P => P.shelfNo);
                    int v = MideaAscm.Code.AscmUtl.GetInstance().ConvertLetterToInt(maxShelfNo.Substring(0, 1));
                    int h = Convert.ToInt32(maxShelfNo.Substring(1, 1));
                    int l = listAscmWarelocation.Max(P => P.layer);
                    for (int iV = 65; iV <= v; iV++)
                    {
                        sb.Append("<tr>");
                        for (int iH = 1; iH <= h; iH++)
                        {
                            sb.Append("<td>");
                            for (int iL = l; iL > 0; iL--)
                            {
                                string shelfNo = MideaAscm.Code.AscmUtl.GetInstance().ConvertIntToLetter(iV) + iH;
                                string docNumber = ascmWorkshopBuilding.code + buildingArea + shelfNo + iL;
                                AscmWarelocation ascmWarelocation = listAscmWarelocation.Find(P => P.docNumber == docNumber);
                                if (ascmWarelocation != null)
                                {
                                    sb.Append("<div id=\"" + ascmWarelocation.id + "\">");
                                    string areaField1 = "<span style='float:center;font-size:18px;height:30px;line-height:30px;'>" + ascmWarelocation.docNumber + "</span>";
                                    string areaField2 = "</br><span style='float:center;font-size:13px;color:#FFFFFF;padding-top:10px;height:16px;line-height:16px;'>&nbsp;" + ascmWarelocation.warehouseId + "</span>";
                                    string areaField3 = "</br><span style='float:center;font-size:13px;color:#FFFFFF;padding-top:0px;height:16px;line-height:16px;'>&nbsp;" + ascmWarelocation.categoryCode + "</span>";
                                    string areaField = areaField1 + areaField2 + areaField3;
                                    sb.Append(areaField);
                                }
                                else
                                {
                                    sb.Append("<div>");
                                }
                                sb.Append("</div>");
                            }
                            sb.Append("</td>");
                        }
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                }
                jsonObjectResult.result = true;
                jsonObjectResult.entity = sb.ToString();
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>设置厂房区域的物料大类</summary>
        public ActionResult SetBuildingAreaCategory(int? id, string buildingAreas, string categoryCode)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            if (!id.HasValue)
            {
                jsonObjectResult.message = "参数厂房ID传值错误";
            }
            else if (string.IsNullOrWhiteSpace(buildingAreas))
            {
                jsonObjectResult.message = "参数厂房区域传值错误";
            }
            else if (string.IsNullOrWhiteSpace(categoryCode))
            {
                jsonObjectResult.message = "物料大类不能为空";
            }
            else
            {
                categoryCode = categoryCode.Trim();
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^([0-9]|[a-z]|[A-Z]){4}$");
                if (!reg.IsMatch(categoryCode))
                {
                    jsonObjectResult.message = "物料大类只能由4位字符组成";
                }
                else
                {
                    try
                    {
                        string userName = string.Empty;
                        if (User.Identity.IsAuthenticated)
                            userName = User.Identity.Name;

                        string whereOther = "";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "buildingId=" + id.Value);
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "buildingArea in('" + buildingAreas.Replace(",", "','") + "')");
                        List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList("", "", "", whereOther);
                        List<AscmWarelocation> listAscmWarelocationUpdate = null;
                        if (listAscmWarelocation != null && listAscmWarelocation.Count > 0)
                        {
                            listAscmWarelocationUpdate = new List<AscmWarelocation>();
                            foreach (AscmWarelocation ascmWarelocation in listAscmWarelocation)
                            {
                                if (string.IsNullOrEmpty(ascmWarelocation.categoryCode))
                                {
                                    ascmWarelocation.categoryCode = categoryCode;
                                    ascmWarelocation.modifyUser = userName;
                                    ascmWarelocation.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                    listAscmWarelocationUpdate.Add(ascmWarelocation);
                                }
                            }

                            if (listAscmWarelocationUpdate != null && listAscmWarelocationUpdate.Count > 0)
                            {
                                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                                {
                                    try
                                    {
                                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmWarelocationUpdate);

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
                        else
                        {
                            jsonObjectResult.message = "请先为此区域设置货位";
                        }
                    }
                    catch (Exception ex)
                    {
                        jsonObjectResult.message = ex.Message;
                    }
                }
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>货位与子库关联</summary>
        public ActionResult LocationWarehouseLinkAdd(int? id, string locationIds, string warehouseId)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (!id.HasValue)
                    throw new Exception("系统错误：参数厂房ID传值错误");
                AscmWorkshopBuilding ascmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().Get(id.Value);
                if (ascmWorkshopBuilding == null)
                    throw new Exception("系统错误：获取厂房失败");
                if (string.IsNullOrEmpty(locationIds))
                    throw new Exception("系统错误：参数货位ID传值错误");
                if (string.IsNullOrEmpty(warehouseId))
                    throw new Exception("系统错误：参数子库ID传值错误");
                AscmWarehouse ascmWarehouse = AscmWarehouseService.GetInstance().Get(warehouseId);
                if (ascmWarehouse == null)
                    throw new Exception("系统错误：获取子库失败");

                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;

                AscmBuildingWarehouseLink ascmBuildingWarehouseLinkSaveOrUpdate = null;
                List<AscmBuildingWarehouseLink> listBuildingWarehouseLink = AscmBuildingWarehouseLinkService.GetInstance().GetList(id.Value);
                if (listBuildingWarehouseLink != null)
                    ascmBuildingWarehouseLinkSaveOrUpdate = listBuildingWarehouseLink.Find(P => P.warehouseId == warehouseId);
                if (ascmBuildingWarehouseLinkSaveOrUpdate == null)
                {
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmBuildingWarehouseLink");
                    ascmBuildingWarehouseLinkSaveOrUpdate = new AscmBuildingWarehouseLink();
                    ascmBuildingWarehouseLinkSaveOrUpdate.id = ++maxId;
                    ascmBuildingWarehouseLinkSaveOrUpdate.createUser = userName;
                    ascmBuildingWarehouseLinkSaveOrUpdate.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmBuildingWarehouseLinkSaveOrUpdate.buildingId = id.Value;
                }
                ascmBuildingWarehouseLinkSaveOrUpdate.warehouseId = warehouseId;
                ascmBuildingWarehouseLinkSaveOrUpdate.modifyUser = userName;
                ascmBuildingWarehouseLinkSaveOrUpdate.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                string whereOther = "";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "id in(" + locationIds + ")");
                List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList("", "", "", whereOther);
                if (listAscmWarelocation != null)
                {
                    foreach (AscmWarelocation ascmWarelocation in listAscmWarelocation)
                    {
                        ascmWarelocation.warehouseId = warehouseId;
                        ascmWarelocation.modifyUser = userName;
                        ascmWarelocation.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    }
                }

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdate(ascmBuildingWarehouseLinkSaveOrUpdate);

                        if (listAscmWarelocation != null && listAscmWarelocation.Count > 0)
                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmWarelocation);

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
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>设置货位的物料大类</summary>
        public ActionResult SetLocationCategory(string locationIds, string categoryCode)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (string.IsNullOrEmpty(locationIds))
                    throw new Exception("系统错误：参数厂房区域传值错误");
                if (categoryCode == null || categoryCode.Trim() == "")
                    throw new Exception("物料大类不能为空");

                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;

                string whereOther = "";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "id in(" + locationIds + ")");
                List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList("", "", "", whereOther);
                if (listAscmWarelocation != null && listAscmWarelocation.Count > 0)
                {
                    string sql = "from AscmLocationMaterialLink where pk.warelocationId in(" + locationIds + ")";
                    List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList(sql);

                    foreach (AscmWarelocation ascmWarelocation in listAscmWarelocation)
                    {
                        if (listLocationMaterialLink != null && listLocationMaterialLink.Count > 0)
                        {
                            if (listLocationMaterialLink.Exists(P => P.pk.warelocationId == ascmWarelocation.id))
                            {
                                if (ascmWarelocation.categoryCode != categoryCode)
                                {
                                    throw new Exception("货位[" + ascmWarelocation.docNumber + "]已设置物料大类[" + ascmWarelocation.categoryCode + "]并且已存在物料");
                                }
                            }
                        }
                        ascmWarelocation.categoryCode = categoryCode;
                        ascmWarelocation.modifyUser = userName;
                        ascmWarelocation.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    }

                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmWarelocation);

                            tx.Commit();//正确执行提交
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            throw ex;
                        }
                    }
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>厂房区域与仓管员关联</summary>
        public ActionResult BuildingAreaUserInfoAdd(int? id, string buildingAreas, string userId)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                #region 验证
                if (!id.HasValue)
                    throw new Exception("系统错误：参数厂房ID传值错误");
                if (string.IsNullOrEmpty(buildingAreas))
                    throw new Exception("系统错误：参数厂房区域传值错误");
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("系统错误：仓管员ID传值错误");
                #endregion

                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;

                #region 货位与仓管员关联
                string whereOther = "";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "buildingId=" + id.Value);
                //string _buildingAreas = string.Empty;
                //string[] arrayBuildingArea = buildingAreas.Split(',');
                //foreach (string buildingArea in arrayBuildingArea)
                //{
                //    if (!string.IsNullOrEmpty(_buildingAreas))
                //        _buildingAreas += ",";
                //    _buildingAreas += "'" + buildingArea + "'";
                //}
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "buildingArea in('" + buildingAreas.Replace(",", "','") + "')");
                List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList("", "", "", whereOther);
                List<AscmWarelocation> listAscmWarelocationUpdate = null;
                if (listAscmWarelocation != null && listAscmWarelocation.Count > 0)
                {
                    listAscmWarelocationUpdate = new List<AscmWarelocation>();
                    foreach (AscmWarelocation ascmWarelocation in listAscmWarelocation)
                    {
                        if (string.IsNullOrEmpty(ascmWarelocation.warehouseUserId))
                        {
                            ascmWarelocation.warehouseUserId = userId;
                            ascmWarelocation.modifyUser = userName;
                            ascmWarelocation.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            listAscmWarelocationUpdate.Add(ascmWarelocation);
                        }
                    }
                }
                #endregion

                if (listAscmWarelocationUpdate != null && listAscmWarelocationUpdate.Count > 0)
                {
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmWarelocationUpdate);

                            tx.Commit();//正确执行提交
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            throw ex;
                        }
                    }
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>货位与仓管员关联</summary>
        public ActionResult LocationUserInfoAdd(int? id, string locationIds, string userId)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (!id.HasValue)
                    throw new Exception("系统错误：参数厂房ID传值错误");
                AscmWorkshopBuilding ascmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().Get(id.Value);
                if (ascmWorkshopBuilding == null)
                    throw new Exception("系统错误：获取厂房失败");
                if (string.IsNullOrEmpty(locationIds))
                    throw new Exception("系统错误：参数货位ID传值错误");
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("系统错误：仓管员ID传值错误");

                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;

                string whereOther = "";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "id in(" + locationIds + ")");
                List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList("", "", "", whereOther);
                if (listAscmWarelocation != null && listAscmWarelocation.Count > 0)
                {
                    foreach (AscmWarelocation ascmWarelocation in listAscmWarelocation)
                    {
                        ascmWarelocation.warehouseUserId = userId;
                        ascmWarelocation.modifyUser = userName;
                        ascmWarelocation.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    }

                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmWarelocation);

                            tx.Commit();//正确执行提交
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            throw ex;
                        }
                    }
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>厂房内部信息显示</summary>
        public ActionResult LoadBuildingAreaInfo(int? id, string buildingAreas)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (id.HasValue && !string.IsNullOrEmpty(buildingAreas))
                {
                    string whereOther = "";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " buildingId=" + id.Value);
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " buildingArea in('" + buildingAreas.Trim().Replace(",", "','") + "')");
                    List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList("", "", "", whereOther);
                    //显示用户名而非用户ID
                    List<YnUser> listYnUser = null;
                    if (listAscmWarelocation != null && listAscmWarelocation.Count > 0)
                    {
                        var warehouseUserIds = listAscmWarelocation.Where(P => !string.IsNullOrEmpty(P.warehouseUserId)).Select(P => P.warehouseUserId).Distinct();
                        if (warehouseUserIds != null)
                        {
                            listYnUser = YnUserService.GetInstance().GetList("from YnUser where userId in('" + string.Join("','", warehouseUserIds) + "')");
                        }
                    }

                    string[] arrayBuildingArea = buildingAreas.Split(',');
                    foreach (string buildingArea in arrayBuildingArea)
                    {
                        sb.Append("<p style=\"font-size:14px;margin:5px;\" align=\"center\">" + buildingArea.Trim() + "区域内部信息</p>");
                        if (listAscmWarelocation != null && listAscmWarelocation.Count > 0)
                        {
                            var findAscmWarelocation = listAscmWarelocation.Where(P => P.buildingArea == buildingArea);
                            if (findAscmWarelocation != null)
                            {
                                //var groupByWarehouse = findAscmWarelocation.Where(P => !string.IsNullOrEmpty(P.warehouseId)).GroupBy(P => P.warehouseId);
                                var groupByWarehouse = findAscmWarelocation.GroupBy(P => P.warehouseId);
                                sb.Append("<ul style=\"border-bottom: 1px dashed #95B8E7;padding:5px\">");

                                StringBuilder subContent = new StringBuilder();
                                StringBuilder subTest = new StringBuilder();
                                foreach (IGrouping<string, AscmWarelocation> ig in groupByWarehouse)
                                {
                                    subContent.Append("<li>" + ig.Key + "仓管人员：");
                                    subTest.Append(ig.Key);//测试是否有值

                                    //string warehouseUserIds = string.Join("；", ig.Where(P=>!string.IsNullOrEmpty(P.warehouseUserId)).Select(P => P.warehouseUserId).Distinct());
                                    string warehouseUserIds = string.Empty;
                                    if (listYnUser != null)
                                        warehouseUserIds = string.Join("；", listYnUser.Where(P => ig.Select(T => T.warehouseUserId).Contains(P.userId)).Select(P => P.userName).Distinct());
                                    subContent.Append(warehouseUserIds);
                                    subTest.Append(warehouseUserIds);//测试是否有值
                                    subContent.Append("</li>");
                                    

                                    subContent.Append("<li style=\"margin-bottom:5px;\"><span style=\"visibility:hidden;\">" + ig.Key + "</span>物料大类：");
                                    string categoryCodes = string.Join("；", ig.Where(P => !string.IsNullOrEmpty(P.categoryCode)).Select(P => P.categoryCode).Distinct());
                                    subContent.Append(categoryCodes);
                                    subTest.Append(categoryCodes);//测试是否有值
                                    subContent.Append("</li>");
                                }

                                //如果测试有值，则显示
                                if (subTest.Length > 0) 
                                {
                                    sb.Append(subContent);
                                }

                                sb.Append("</ul>");
                            }
                        }
                    }
                }
                jsonObjectResult.result = true;
                jsonObjectResult.entity = sb.ToString();
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>通过查询物料，查找货位</summary>
        public ActionResult LocationMaterialList(string sort, string order, string queryWord, int? buildingId)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();

            try
            {
                if (buildingId.HasValue)
                {
                    List<AscmLocationMaterialLink> list = AscmLocationMaterialLinkService.GetInstance().GetList(sort, order, buildingId, queryWord);
                    if (list != null)
                    {
                        foreach (AscmLocationMaterialLink ascmLocationMaterial in list)
                        {
                            jsonDataGridResult.rows.Add(ascmLocationMaterial);
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
        #endregion

        #region 物料导入导出
        public class WarelocationMaterial
        {
            /// <summary>厂房名称</summary>
            public string buildingName { get; set; }
            /// <summary>子库</summary>
            public string warehouseId { get; set; }
            /// <summary>货位编码</summary>
            public string warelocationDoc { get; set; }
            /// <summary>物料编码</summary>
            public string materialDoc { get; set; }
            /// <summary>数量</summary>
            public decimal quantity { get; set; }
            /// <summary>行号</summary>
            public int rowNumber { get; set; }
        }
        [HttpPost]
        public ActionResult MaterialImport(HttpPostedFileBase fileImport)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            string sError = "";
            try
            {
                if (fileImport != null)
                {
                    string userName = string.Empty;
                    if (User.Identity.IsAuthenticated)
                        userName = User.Identity.Name;

                    //定义索引
                    int buildingIndex = 0, warehouseIndex = 1, warelocationIndex = 2, materialIndex = 3, quantityIndex = 4;
                    List<WarelocationMaterial> listWarelocationMaterial = null;
                    using (Stream stream = fileImport.InputStream)
                    {
                        listWarelocationMaterial = new List<WarelocationMaterial>();

                        IWorkbook wb = WorkbookFactory.Create(stream);
                        ISheet sheet = wb.GetSheet("Sheet1");
                        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                        while (rows.MoveNext())
                        {
                            IRow row = (IRow)rows.Current;
                            if (row.RowNum != 0)
                            {
                                ICell buildingCell = row.GetCell(buildingIndex, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                ICell warehouseCell = row.GetCell(warehouseIndex, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                ICell warelocationCell = row.GetCell(warelocationIndex, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                ICell materialCell = row.GetCell(materialIndex, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                ICell quantityCell = row.GetCell(quantityIndex, MissingCellPolicy.CREATE_NULL_AS_BLANK);

                                WarelocationMaterial warelocationMaterial = new WarelocationMaterial();
                                if (buildingCell != null)
                                {
                                    warelocationMaterial.buildingName = buildingCell.ToString().Trim();
                                }
                                if (warehouseCell != null)
                                {
                                    warelocationMaterial.warehouseId = warehouseCell.ToString().Trim();
                                }
                                if (warelocationCell != null)
                                {
                                    warelocationMaterial.warelocationDoc = warelocationCell.ToString().Trim();
                                }
                                if (materialCell != null)
                                {
                                    warelocationMaterial.materialDoc = materialCell.ToString().Trim();
                                }
                                if (quantityCell != null)
                                {
                                    decimal quantity = decimal.Zero;
                                    decimal.TryParse(quantityCell.ToString().Trim(), out quantity);
                                    warelocationMaterial.quantity = quantity;
                                }
                                warelocationMaterial.rowNumber = row.RowNum + 1;
                                listWarelocationMaterial.Add(warelocationMaterial);
                            }
                        }
                    }
                    if (listWarelocationMaterial != null && listWarelocationMaterial.Count > 0)
                    {
                        //厂房
                        var buildingNames = listWarelocationMaterial.Where(P => !string.IsNullOrEmpty(P.buildingName)).Select(P => P.buildingName).Distinct();
                        string sql = "from AscmWorkshopBuilding where name in('" + string.Join("','", buildingNames) + "')";
                        List<AscmWorkshopBuilding> listAscmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().GetList(sql);
                        //货位
                        var warelocationDocs = listWarelocationMaterial.Where(P => !string.IsNullOrEmpty(P.warelocationDoc)).Select(P => P.warelocationDoc).Distinct();
                        List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList(warelocationDocs.ToList());
                        //物料
                        var materialDocs = listWarelocationMaterial.Where(P => !string.IsNullOrEmpty(P.materialDoc)).Select(P => P.materialDoc).Distinct();
                        if (materialDocs.Count() == 0)
                            throw new Exception("导入文件中缺少物料编码");
                        List<AscmMaterialItem> listAscmMaterialItem = AscmMaterialItemService.GetInstance().GetList(materialDocs.ToList());

                        #region 导入
                        sError = "【未成功更新数据】<br>";
                        List<AscmLocationMaterialLink> listLink_Add = new List<AscmLocationMaterialLink>();
                        List<AscmLocationMaterialLink> listLink_update = new List<AscmLocationMaterialLink>();
                        List<AscmLocationMaterialLink> listLink = AscmLocationMaterialLinkService.GetInstance().GetList(null, "", "", "", "");
                        //int maxId_Link = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmLocationMaterialLink");
                        AscmLocationMaterialLink locationMaterialLinkUpdate = null;
                        foreach (WarelocationMaterial warelocationMaterial in listWarelocationMaterial)
                        {
                            string sBuilding = warelocationMaterial.buildingName;
                            int iRowNumber = warelocationMaterial.rowNumber;
                            var find_workShopBuilding = listAscmWorkshopBuilding.Find(item => item.name == sBuilding);
                            if (find_workShopBuilding != null)
                            {
                                string sWarehouseId = warelocationMaterial.warehouseId;
                                string sLocationDoc = warelocationMaterial.warelocationDoc;
                                string sMaterialItem = warelocationMaterial.materialDoc;
                                decimal sQuantity = warelocationMaterial.quantity;

                                var find_warelocation = listAscmWarelocation.Find(item => item.buildingId == find_workShopBuilding.id && item.warehouseId == sWarehouseId && item.docNumber == sLocationDoc);
                                var find_materialItem = listAscmMaterialItem.Find(item => item.docNumber == sMaterialItem);
                                if (find_warelocation != null)
                                {
                                    if (find_materialItem != null)
                                    {
                                        AscmLocationMaterialLinkPK pk = new AscmLocationMaterialLinkPK { warelocationId = find_warelocation.id, materialId = find_materialItem.id };
                                        var find_link = listLink.Find(item => item.pk.Equals(pk));
                                        //decimal dQuantity = 0;
                                        //if (Decimal.TryParse(sQuantity, out dQuantity))
                                        //{
                                        if (find_link != null)
                                        {
                                            find_link.quantity = sQuantity;
                                            find_link.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            find_link.modifyUser = userName;
                                            listLink_update.Add(find_link);
                                        }
                                        else
                                        {
                                            locationMaterialLinkUpdate = new AscmLocationMaterialLink();
                                            locationMaterialLinkUpdate.pk = pk;
                                            //locationMaterialLinkUpdate.id = ++maxId_Link;
                                            locationMaterialLinkUpdate.organizationId = 775;
                                            locationMaterialLinkUpdate.createUser = userName;
                                            locationMaterialLinkUpdate.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            //locationMaterialLinkUpdate.warelocationId = find_warelocation.id;
                                            //locationMaterialLinkUpdate.materialId = find_materialItem.id;
                                            locationMaterialLinkUpdate.quantity = sQuantity;
                                            listLink_Add.Add(locationMaterialLinkUpdate);
                                        }
                                        //}
                                        //else
                                        //{
                                        //    sError += "&nbsp;&nbsp;" + sBuilding + "," + sWarehouseId + "," + sLocationDoc + "物料数量格式异常；<br>";
                                        //}
                                    }
                                    else
                                    {
                                        sError += "&nbsp;&nbsp;[" + iRowNumber + "]" + sBuilding + "," + sWarehouseId + "," + sLocationDoc + "未匹配到相应物料；<br>";
                                    }
                                }
                                else
                                {
                                    sError += "&nbsp;&nbsp;[" + iRowNumber + "]" + sBuilding + "," + sWarehouseId + "," + sLocationDoc + "未匹配到相应货位；<br>";
                                }
                            }
                            else
                            {
                                sError += "&nbsp;&nbsp;[" + iRowNumber + "]" + sBuilding + "未匹配到相应厂房；<br>";
                            }
                        }
                        using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                        {
                            try
                            {
                                if (listLink_Add != null && listLink_Add.Count() > 0)
                                {
                                    YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listLink_Add);
                                }
                                if (listLink_update != null && listLink_update.Count() > 0)
                                {
                                    YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listLink_update);
                                }
                                tx.Commit();
                                sError += "【成功更新" + (listLink_Add.Count() + listLink_update.Count()) + "条】";
                                jsonObjectResult.message = sError;
                                jsonObjectResult.result = true;
                            }
                            catch (Exception ex)
                            {
                                tx.Rollback();
                                sError += ex.Message;
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message += ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult MaterialExport(int? buildingId)
        {
            IWorkbook wb = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Sheet1");
            sheet.SetColumnWidth(0, 12 * 256);
            sheet.SetColumnWidth(1, 12 * 256);
            sheet.SetColumnWidth(2, 12 * 256);
            sheet.SetColumnWidth(3, 15 * 256);
            sheet.SetColumnWidth(4, 10 * 256);

            int iRow = 0;
            IRow titleRow = sheet.CreateRow(0);
            titleRow.Height = 20 * 20;
            titleRow.CreateCell(0).SetCellValue("厂房");
            titleRow.CreateCell(1).SetCellValue("子库");
            titleRow.CreateCell(2).SetCellValue("货位");
            titleRow.CreateCell(3).SetCellValue("物料编码");
            titleRow.CreateCell(4).SetCellValue("物料数量");
            titleRow.CreateCell(5).SetCellValue("物料大类");
            List<AscmWorkshopBuilding> listAscmWorkshopBuilding = null;

            if (buildingId.HasValue)
                listAscmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().GetList("from AscmWorkshopBuilding where id=" + buildingId.Value);
            else
                listAscmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().GetList("", "");
            List<AscmBuildingWarehouseLink> listAscmBuildingWarehouseLink = AscmBuildingWarehouseLinkService.GetInstance().GetList("from AscmBuildingWarehouseLink");
            List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList("from AscmWarelocation");
            if (listAscmWorkshopBuilding != null && listAscmWorkshopBuilding.Count() > 0)
            {
                foreach (AscmWorkshopBuilding ascmWorkshopBuilding in listAscmWorkshopBuilding)
                {
                    List<AscmBuildingWarehouseLink> listBuildingWarehouseLink = listAscmBuildingWarehouseLink.Where(item => item.buildingId == ascmWorkshopBuilding.id).ToList();
                    if (listBuildingWarehouseLink != null && listBuildingWarehouseLink.Count() > 0)
                    {
                        foreach (AscmBuildingWarehouseLink ascmBuildingWarehouseLink in listBuildingWarehouseLink)
                        {
                            List<AscmWarelocation> list = listAscmWarelocation.Where(item => item.buildingId == ascmWorkshopBuilding.id && item.warehouseId == ascmBuildingWarehouseLink.warehouseId).OrderBy(item => item.docNumber).ToList();
                            if (list != null && list.Count() > 0)
                            {
                                foreach (AscmWarelocation ascmWarelocation in list)
                                {
                                    iRow++;
                                    IRow row = sheet.CreateRow(iRow);
                                    row.Height = 20 * 20;
                                    row.CreateCell(0).SetCellValue(ascmWorkshopBuilding.name);
                                    row.CreateCell(1).SetCellValue(ascmWarelocation.warehouseId);
                                    row.CreateCell(2).SetCellValue(ascmWarelocation.docNumber);
                                    row.CreateCell(5).SetCellValue(ascmWarelocation.categoryCode);
                                }
                            }
                        }
                    }
                }
            }

            byte[] buffer = new byte[] { };
            using (System.IO.MemoryStream stream = new MemoryStream())
            {
                wb.Write(stream);
                buffer = stream.GetBuffer();
            }
            return File(buffer, "application/vnd.ms-excel", "子库货位.xls");
        }
        #endregion

        #region 未关联物料查询
        public ActionResult MaterialNotRelatedQuery()
        {
            return View();
        }
        public ActionResult MaterialNotRelatedList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string ids = "";
                List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList("from AscmLocationMaterialLink");
                var materialIdList = listLocationMaterialLink.Select(item => item.pk.materialId).Distinct();
                if (materialIdList != null)
                {
                    foreach (var materialId in materialIdList)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += materialId;
                    }
                    string whereOther = " id not in (" + ids + ")";

                    List<AscmMaterialItem> list = AscmMaterialItemService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                    if (list != null && list.Count() > 0)
                    {
                        foreach (AscmMaterialItem materialItem in list)
                        {
                            jsonDataGridResult.rows.Add(materialItem);
                        }
                        jsonDataGridResult.total = ynPage.GetRecordCount();
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

        #region 仓库(子库)
        public ActionResult WarehouseIndex()
        {
            //List<AppointmentArrivalOfGoods> list;
            //try
            //{
            //    list = AscmWmsIncAccCheckoutService.GetInstance().GetAppointmentArrivalOfGoods();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //List<AscmWipDiscreteJobsV> list = MideaAscm.View.Services.AscmWipDiscreteJobsVService.GetInstance().GetWmsLedMonitorList();

            //测试领料确认
            //List<AscmWmsLogisticsDetailLog> list = AscmWmsLogisticsMainLogService.GetInstance().GetAllDetail("mainId=90");
            //if (list != null && list.Count > 0)
            //{
            //    List<WmsAndLogistics> listWmsAndLogistics = new List<WmsAndLogistics>();
            //    foreach (AscmWmsLogisticsDetailLog detailLog in list)
            //    {
            //        WmsAndLogistics wmsAndLogistics = new WmsAndLogistics();
            //        wmsAndLogistics.wipEntityId = detailLog.wipEntityId;
            //        wmsAndLogistics.materialId = detailLog.materialId;
            //        wmsAndLogistics.quantity = detailLog.quantity;
            //        wmsAndLogistics.preparationString = detailLog.preparationString;
            //        listWmsAndLogistics.Add(wmsAndLogistics);
            //    }
            //    WmsAndLogisticsService.GetInstance().DoMaterialRequisition(listWmsAndLogistics);
            //}

            return View();
        }
        public ActionResult WarehouseList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmWarehouse> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmWarehouseService.GetInstance().GetList(ynPage, "", "", queryWord, null);
                foreach (AscmWarehouse ascmWarehouse in list)
                {
                    jsonDataGridResult.rows.Add(ascmWarehouse.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WarehouseEdit(string id)
        {
            AscmWarehouse ascmWarehouse = null;
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    ascmWarehouse = AscmWarehouseService.GetInstance().Get(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmWarehouse.GetOwner(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult WarehouseSave(AscmSupplier ascmWarehouse_Model, string id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                AscmWarehouse ascmWarehouse = null;
                if (!string.IsNullOrEmpty(id))
                {
                    ascmWarehouse = AscmWarehouseService.GetInstance().Get(id);
                }
                else
                {
                    ascmWarehouse = new AscmWarehouse();
                    throw new Exception("不允许增加仓库！");
                }
                if (ascmWarehouse == null)
                    throw new Exception("保存仓库失败！");
                if (ascmWarehouse_Model.name == null || ascmWarehouse_Model.name.Trim() == "")
                    throw new Exception("仓库名称不能为空！");

                //ascmWarehouse.name = ascmWarehouse_Model.name;
                //ascmWarehouse.enabled = ascmWarehouse_Model.enabled;
                //ascmWarehouse.description = ascmWarehouse_Model.description;
                if (!string.IsNullOrEmpty(id))
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmWarehouse where name='" + ascmWarehouse_Model.name.Trim() + "' and id<>" + id + "");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已经存在此仓库【" + ascmWarehouse_Model.name.Trim() + "】！");
                    //AscmWarehouseService.GetInstance().Update(ascmWarehouse);

                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmWarehouse.id.ToString();
                jsonObjectResult.entity = ascmWarehouse;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult WarehouseAscxList(int? page, int? rows, string sort, string order, string q, int? inUnloadingPoint, int? inUnloadingPointMap)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmWarehouse> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "";
                if (inUnloadingPoint.HasValue)
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " id in(select warehouseId from AscmUnloadingPoint)");
                if (inUnloadingPointMap.HasValue)
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " id in(select warehouseId from AscmUnloadingPointMap)");

                list = AscmWarehouseService.GetInstance().GetList(ynPage, "", "", q, whereOther);
                if (list != null)
                {
                    foreach (AscmWarehouse ascmWarehouse in list)
                    {
                        jsonDataGridResult.rows.Add(ascmWarehouse);
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
        public ActionResult WarehouseComboboxList(string q)
        {
            List<AscmWarehouse> list = null;
            try
            {
                if (!string.IsNullOrEmpty(q))
                    q = q.Trim().ToUpper();
                list = AscmWarehouseService.GetInstance().GetList(null, "", "", q, "");
                list = list ?? new List<AscmWarehouse>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 货位管理
        public ActionResult WarelocationIndex()
        {
            //货位
            return View();
        }
        public ActionResult WarelocationList(int? page, int? rows, string sort, string order, string queryWord, int? buildingId, string warehouseId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (buildingId.HasValue)
                {
                    string whereOther = "";

                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " buildingId=" + buildingId.Value);
                    if (!string.IsNullOrEmpty(warehouseId))
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " warehouseId='" + warehouseId + "'");

                    List<AscmWarelocation> list = AscmWarelocationService.GetInstance().GetList(ynPage, sort, order, queryWord, whereOther);
                    if (list != null)
                    {
                        foreach (AscmWarelocation ascmWarelocation in list)
                        {
                            jsonDataGridResult.rows.Add(ascmWarelocation);
                        }
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
        public ActionResult WarelocationListByWarehouse(int? page, int? rows, string sort, string order, string queryWord, string warehouseId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (!string.IsNullOrEmpty(warehouseId))
                {
                    string whereOther = "";

                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " warehouseId='" + warehouseId + "'");

                    List<AscmWarelocation> list = AscmWarelocationService.GetInstance().GetList(ynPage, sort, order, queryWord, whereOther, false, false);
                    if (list != null)
                    {
                        foreach (AscmWarelocation ascmWarelocation in list)
                        {
                            jsonDataGridResult.rows.Add(ascmWarelocation);
                        }
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
        public ActionResult WarelocationEdit(int? id)
        {
            AscmWarelocation ascmWarelocation = null;
            try
            {
                if (id.HasValue)
                {
                    ascmWarelocation = AscmWarelocationService.GetInstance().Get(id.Value);
                    if (ascmWarelocation != null)
                    {
                        ascmWarelocation.ascmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().Get(ascmWarelocation.buildingId);
                        ascmWarelocation.ascmWarehouse = AscmWarehouseService.GetInstance().Get(ascmWarelocation.warehouseId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmWarelocation, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult WarelocationSave(AscmWarelocation ascmWarelocation_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                #region 验证
                if (ascmWarelocation_Model.lowerLimit > ascmWarelocation_Model.upperLimit)
                    throw new Exception("下限不能大于上限");
                if (ascmWarelocation_Model.categoryCode == null || ascmWarelocation_Model.categoryCode.Trim() == "")
                    throw new Exception("物料大类不能为空");
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"\d{4}");
                if (!reg.IsMatch(ascmWarelocation_Model.categoryCode))
                    throw new Exception("物料大类只能由4个数字组成");
                if (ascmWarelocation_Model.type == 0)
                    throw new Exception("必须选择货位形式");
                reg = new System.Text.RegularExpressions.Regex(@"\d{2}");
                if (ascmWarelocation_Model.type == AscmWarelocation.TypeDefine.shelf || ascmWarelocation_Model.type == AscmWarelocation.TypeDefine.highShelf)
                {
                    if (ascmWarelocation_Model.shelfNo == null || ascmWarelocation_Model.shelfNo.Trim() == "")
                        throw new Exception("货架号不能为空");
                    if (!reg.IsMatch(ascmWarelocation_Model.shelfNo))
                        throw new Exception("货架号必须由2个数字组成");
                    if (ascmWarelocation_Model.No == null || ascmWarelocation_Model.No.Trim() == "")
                        throw new Exception("货架号不能为空");
                    if (!reg.IsMatch(ascmWarelocation_Model.No))
                        throw new Exception("货位号必须由2个数字组成");
                }
                else if (ascmWarelocation_Model.type == AscmWarelocation.TypeDefine.floor)
                {
                    if (ascmWarelocation_Model.floorRow == null || ascmWarelocation_Model.floorRow.Trim() == "")
                        throw new Exception("行不能为空");
                    if (!reg.IsMatch(ascmWarelocation_Model.floorRow))
                        throw new Exception("行必须由2个数字组成");
                    if (ascmWarelocation_Model.floorColumn == null || ascmWarelocation_Model.floorColumn.Trim() == "")
                        throw new Exception("列不能为空");
                    if (!reg.IsMatch(ascmWarelocation_Model.floorColumn))
                        throw new Exception("列必须由2个数字组成");
                }
                if (ascmWarelocation_Model.layer == 0)
                    throw new Exception("必须输入层号");

                AscmWorkshopBuilding ascmWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().Get(ascmWarelocation_Model.buildingId);
                if (ascmWorkshopBuilding == null)
                    throw new Exception("系统错误：没有指定厂房");
                AscmWarehouse ascmWarehouse = AscmWarehouseService.GetInstance().Get(ascmWarelocation_Model.warehouseId);
                if (ascmWarehouse == null)
                    throw new Exception("系统错误：没有指定子库");
                AscmBuildingWarehouseLink ascmBuildingWarehouseLink = AscmBuildingWarehouseLinkService.GetInstance().Get(ascmWorkshopBuilding.id, ascmWarehouse.id);
                if (ascmBuildingWarehouseLink == null)
                    throw new Exception("子库【" + ascmWarehouse.id + "】缺少代号");
                #endregion

                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;

                AscmWarelocation ascmWarelocation = null;
                if (id.HasValue)
                {
                    ascmWarelocation = AscmWarelocationService.GetInstance().Get(id.Value);
                    if (ascmWarelocation == null)
                        throw new Exception("系统错误：找不到货位");
                }
                else
                {
                    ascmWarelocation = new AscmWarelocation();
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWarelocation");
                    ascmWarelocation.id = ++maxId;
                    ascmWarelocation.createUser = userName;
                    ascmWarelocation.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                ascmWarelocation.ascmWorkshopBuilding = ascmWorkshopBuilding;
                ascmWarelocation.buildingId = ascmWorkshopBuilding.id;
                ascmWarelocation.ascmWarehouse = ascmWarehouse;
                ascmWarelocation.warehouseId = ascmWarehouse.id;
                ascmWarelocation.categoryCode = ascmWarelocation_Model.categoryCode;
                ascmWarelocation.type = ascmWarelocation_Model.type;
                ascmWarelocation.layer = ascmWarelocation_Model.layer;
                //货位编码
                ascmWarelocation.docNumber = ascmWorkshopBuilding.code.ToString();
                ascmWarelocation.docNumber += ascmBuildingWarehouseLink.warehouseCode.ToString();
                ascmWarelocation.docNumber += ascmWarelocation.categoryCode.Substring(2); //取物料大类后两位
                ascmWarelocation.docNumber += ascmWarelocation.type.ToString();
                if (ascmWarelocation.type == AscmWarelocation.TypeDefine.shelf || ascmWarelocation.type == AscmWarelocation.TypeDefine.highShelf)
                {
                    ascmWarelocation.shelfNo = ascmWarelocation_Model.shelfNo;
                    ascmWarelocation.layer = ascmWarelocation_Model.layer;

                    ascmWarelocation.docNumber += ascmWarelocation.shelfNo;
                    ascmWarelocation.docNumber += ascmWarelocation.layer;
                    ascmWarelocation.docNumber += ascmWarelocation.No;
                }
                else if (ascmWarelocation.type == AscmWarelocation.TypeDefine.floor)
                {
                    ascmWarelocation.floorRow = ascmWarelocation_Model.floorRow;
                    ascmWarelocation.floorColumn = ascmWarelocation_Model.floorColumn;

                    ascmWarelocation.docNumber += ascmWarelocation.floorRow;
                    ascmWarelocation.docNumber += ascmWarelocation.layer;
                    ascmWarelocation.docNumber += ascmWarelocation.floorColumn;
                }

                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmWarelocation where id<>" + ascmWarelocation.id + " and docNumber='" + ascmWarelocation.docNumber + "'");
                if (object1 == null)
                    throw new Exception("查询异常！");
                int count = 0;
                if (int.TryParse(object1.ToString(), out count) && count > 0)
                    throw new Exception("已存在货位编码【" + ascmWarelocation.docNumber + "】");

                ascmWarelocation.upperLimit = ascmWarelocation_Model.upperLimit;
                ascmWarelocation.lowerLimit = ascmWarelocation_Model.lowerLimit;
                ascmWarelocation.modifyUser = userName;
                ascmWarelocation.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                if (!string.IsNullOrEmpty(ascmWarelocation_Model.description))
                    ascmWarelocation.description = ascmWarelocation_Model.description.Trim();

                if (id.HasValue)
                    AscmWarelocationService.GetInstance().Update(ascmWarelocation);
                else
                    AscmWarelocationService.GetInstance().Save(ascmWarelocation);

                jsonObjectResult.result = true;
                jsonObjectResult.id = ascmWarelocation.id.ToString();
                jsonObjectResult.entity = ascmWarelocation;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult WarelocationDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (!id.HasValue)
                    throw new Exception("系统错误：参数传值错误");

                AscmWarelocation ascmWarelocation = AscmWarelocationService.GetInstance().Get(id.Value);
                if (ascmWarelocation == null)
                    throw new Exception("系统错误：货位不存在");

                List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList(ascmWarelocation.id, false);
                if (listLocationMaterialLink != null)
                {
                    int count = listLocationMaterialLink.Where(P => P.quantity != decimal.Zero).Count();
                    if (count > 0)
                        throw new Exception("不能删除已存放物料的货位");
                }

                //删除未接收的批单货位分配
                string sql = "from AscmAssignWarelocation";
                string where = string.Empty;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "warelocationId=" + id.Value);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "batchId in(select id from AscmDeliveryOrderBatch where ascmStatus is null or ascmStatus<>'" + AscmDeliveryOrderBatch.AscmStatusDefine.received + "')");
                sql += " where " + where;
                List<AscmAssignWarelocation> listAssignWarelocation = AscmAssignWarelocationService.GetInstance().GetList(sql);

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        //删除货位
                        YnDaoHelper.GetInstance().nHibernateHelper.Delete(ascmWarelocation);
                        //删除货位关联的物料
                        if (listLocationMaterialLink != null)
                            YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listLocationMaterialLink);
                        //删除未接收的批单货位分配
                        if (listAssignWarelocation != null && listAssignWarelocation.Count > 0)
                            YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAssignWarelocation);

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
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public class EasyBuildingWarehouse
        {
            public int id { get; set; }
            public string name { get; set; }
            public string code { get; set; }
            public int buildingId { get; set; }
            public string warehouseId { get; set; }
            public string docNumber { get; set; }
            public List<EasyBuildingWarehouse> children { get; set; }
        }
        public ActionResult BuildingWarehouseList()
        {
            List<EasyBuildingWarehouse> list = new List<EasyBuildingWarehouse>();
            try
            {
                List<AscmWorkshopBuilding> listWorkshopBuilding = AscmWorkshopBuildingService.GetInstance().GetList("", "");
                if (listWorkshopBuilding != null)
                {
                    int id = 0;
                    foreach (AscmWorkshopBuilding ascmWorkshopBuilding in listWorkshopBuilding)
                    {
                        EasyBuildingWarehouse workshopBuilding = new EasyBuildingWarehouse();
                        workshopBuilding.id = ++id;
                        workshopBuilding.name = ascmWorkshopBuilding.name;
                        workshopBuilding.code = ascmWorkshopBuilding.code;
                        workshopBuilding.buildingId = ascmWorkshopBuilding.id;
                        workshopBuilding.children = new List<EasyBuildingWarehouse>();
                        list.Add(workshopBuilding);
                        List<AscmBuildingWarehouseLink> listBuildingWarehouseLink = AscmBuildingWarehouseLinkService.GetInstance().GetList(ascmWorkshopBuilding.id);
                        if (listBuildingWarehouseLink != null)
                        {
                            foreach (AscmBuildingWarehouseLink ascmBuildingWarehouseLink in listBuildingWarehouseLink)
                            {
                                EasyBuildingWarehouse warehouse = new EasyBuildingWarehouse();
                                warehouse.id = ++id;
                                warehouse.name = "[<font color='red'>" + ascmBuildingWarehouseLink.warehouseId + "</font>]" + ascmBuildingWarehouseLink.warehouseName;
                                warehouse.docNumber = ascmBuildingWarehouseLink.warehouseId;
                                warehouse.buildingId = ascmWorkshopBuilding.id;
                                warehouse.warehouseId = ascmBuildingWarehouseLink.warehouseId;
                                workshopBuilding.children.Add(warehouse);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /* 货位与物料关联 */
        public ActionResult LocationMaterialLinkList(int? page, int? rows, string sort, string order, string queryWord, int? warelocationId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (warelocationId.HasValue)
                {
                    string whereOther = " pk.warelocationId=" + warelocationId.Value;
                    List<AscmLocationMaterialLink> list = AscmLocationMaterialLinkService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                    if (list != null)
                    {
                        foreach (AscmLocationMaterialLink locationMaterialLink in list)
                        {
                            jsonDataGridResult.rows.Add(locationMaterialLink);
                        }
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
        public ActionResult LocationMaterialLinkAdd(int? warelocationId, string materialItemJson)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (warelocationId.HasValue && !string.IsNullOrEmpty(materialItemJson))
                {
                    AscmWarelocation ascmWarelocation = AscmWarelocationService.GetInstance().Get(warelocationId.Value);
                    if (ascmWarelocation == null)
                        throw new Exception("找不到货位");

                    List<AscmMaterialItem> listAscmMaterialItem = JsonConvert.DeserializeObject<List<AscmMaterialItem>>(materialItemJson);
                    if (listAscmMaterialItem != null && listAscmMaterialItem.Count > 0)
                    {
                        string userName = string.Empty;
                        if (User.Identity.IsAuthenticated)
                            userName = User.Identity.Name;

                        List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList(warelocationId.Value);
                        List<AscmLocationMaterialLink> listLocationMaterialLinkAdd = new List<AscmLocationMaterialLink>();
                        //int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmLocationMaterialLink");
                        foreach (AscmMaterialItem materialItem in listAscmMaterialItem)
                        {
                            AscmLocationMaterialLink locationMaterialLink = listLocationMaterialLink.Find(P => P.pk.materialId == materialItem.id);
                            if (locationMaterialLink == null)
                            {
                                locationMaterialLink = new AscmLocationMaterialLink();
                                locationMaterialLink.pk = new AscmLocationMaterialLinkPK { warelocationId = warelocationId.Value, materialId = materialItem.id };
                                //locationMaterialLink.id = ++maxId;
                                locationMaterialLink.createUser = userName;
                                locationMaterialLink.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                locationMaterialLink.modifyUser = userName;
                                locationMaterialLink.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                //locationMaterialLink.warelocationId = warelocationId.Value;
                                //locationMaterialLink.materialId = materialItem.id;
                                listLocationMaterialLinkAdd.Add(locationMaterialLink);
                            }
                        }
                        //if (listLocationMaterialLinkAdd.Count > 0)
                        //    AscmLocationMaterialLinkService.GetInstance().Save(listLocationMaterialLinkAdd);

                        //获取"待备料","备料中_未确认","备料中_已确认"的备料单，设置相关物料的备料单明细的货位
                        List<AscmWmsPreparationMain> listAscmWmsPreparationMain = null;
                        List<AscmWmsPreparationDetail> listPreparationDetailUpdate = new List<AscmWmsPreparationDetail>();
                        string whereQueryWord = "", whereOther = "", whereDetailOther = "", sql_Detail = "from AscmWmsPreparationDetail";

                        whereQueryWord = "status in ('" + AscmWmsPreparationMain.StatusDefine.unPrepare + "','" + AscmWmsPreparationMain.StatusDefine.preparingUnConfirm + "', '" + AscmWmsPreparationMain.StatusDefine.preparing + "')";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                        listAscmWmsPreparationMain = AscmWmsPreparationMainService.GetInstance().GetList(null, "", "", "", whereOther);

                        if (listLocationMaterialLinkAdd.Count > 0)
                        {
                            string materialIds = string.Join(",", listLocationMaterialLinkAdd.Select(P => P.pk.materialId.ToString()).Distinct());
                            whereQueryWord = "materialId in (" + materialIds + ")";
                            whereDetailOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereDetailOther, whereQueryWord);
                        }

                        if (listAscmWmsPreparationMain != null && listAscmWmsPreparationMain.Count > 0)
                        {
                            string mainIds = string.Join(",", listAscmWmsPreparationMain.Select(P => P.id.ToString()).Distinct());
                            whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(mainIds, "mainId");
                            whereDetailOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereDetailOther, whereQueryWord);
                        }

                        if (!string.IsNullOrEmpty(whereDetailOther))
                            sql_Detail += " where " + whereDetailOther;

                        List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetList(sql_Detail);
                        if (listPreparationDetail != null && listPreparationDetail.Count > 0)
                        {
                            //货位物料
                            string warehouseIds = "'" + string.Join("','", listPreparationDetail.Select(P => P.warehouseId).Distinct()) + "'";
                            //设置物料货位
                            foreach (AscmWmsPreparationDetail preparationDetail in listPreparationDetail)
                            {
                                if (preparationDetail.warelocationId == 0)
                                {
                                    AscmLocationMaterialLink locationMaterialLink = listLocationMaterialLinkAdd.Find(P => P.pk.materialId == preparationDetail.materialId);
                                    if (locationMaterialLink != null)
                                    {
                                        preparationDetail.warelocationId = locationMaterialLink.pk.warelocationId;
                                        listPreparationDetailUpdate.Add(preparationDetail);
                                    }
                                }
                            }

                        }
                        
                        using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                        {
                            try
                            {
                                //添加货位与物料关联
                                if (listLocationMaterialLinkAdd.Count > 0)
                                    YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listLocationMaterialLinkAdd);

                                //备料单明细更新"设置货位"
                                if (listPreparationDetailUpdate.Count > 0)
                                    YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listPreparationDetailUpdate);

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
        public ActionResult LocationMaterialLinkRemove(int? id, string linkIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (!id.HasValue)
                    throw new Exception("系统错误：参数货位ID传值错误");
                if (string.IsNullOrEmpty(linkIds))
                    throw new Exception("系统错误：参数传值错误");
                List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList("from AscmLocationMaterialLink where pk.warelocationId=" + id.Value + " and pk.materialId in(" + linkIds + ")", true);
                if (listLocationMaterialLink == null)
                    throw new Exception("系统错误：获取货物关联的物料失败");

                List<AscmLocationMaterialLink> listLocationMaterialLinkDelete = new List<AscmLocationMaterialLink>();
                //listLocationMaterialLinkDelete.AddRange(listLocationMaterialLink.Where(P => P.quantity == decimal.Zero));
                //jsonObjectResult.message = "移除成功";
                //if (listLocationMaterialLinkDelete.Count < listLocationMaterialLink.Count)
                //    jsonObjectResult.message = "不能移除库存数量不为0的物料";
                string message = string.Empty;
                string materialIds = string.Empty;
                foreach (AscmLocationMaterialLink locationMaterialLink in listLocationMaterialLink)
                {
                    if (locationMaterialLink.quantity == decimal.Zero)
                    {
                        if (!string.IsNullOrEmpty(materialIds))
                            materialIds += ",";
                        materialIds += locationMaterialLink.pk.materialId;

                        listLocationMaterialLinkDelete.Add(locationMaterialLink);
                    }
                    else
                    {
                        message += string.Format("<li>物料[{0}]：<font color='red'>{1}</font></li>", locationMaterialLink.materialDocNumber, "库存数量不为0，不能移除");
                    }
                }
                //删除未接收的批单货位分配
                List<AscmAssignWarelocation> listAssignWarelocation = null;
                if (!string.IsNullOrEmpty(materialIds))
                {
                    string sql = "from AscmAssignWarelocation";
                    string where = string.Empty;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "warelocationId=" + id.Value);
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "materialId in(" + materialIds + ")");
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "batchId in(select id from AscmDeliveryOrderBatch where ascmStatus is null or ascmStatus<>'" + AscmDeliveryOrderBatch.AscmStatusDefine.received + "')");
                    sql += " where " + where;
                    listAssignWarelocation = AscmAssignWarelocationService.GetInstance().GetList(sql);
                }

                if (!string.IsNullOrEmpty(message))
                    message = string.Format("<ul>{0}</ul>", message);
                else
                    message = "移除成功";


                //获取"待备料","备料中_未确认","备料中_已确认"的备料单，设置相关物料的备料单明细的货位
                List<AscmWmsPreparationMain> listAscmWmsPreparationMain = null;
                List<AscmWmsPreparationDetail> listPreparationDetailUpdate = new List<AscmWmsPreparationDetail>();
                string whereQueryWord = "", whereOther = "", whereDetailOther = "", sql_Detail = "from AscmWmsPreparationDetail";

                whereQueryWord = "status in ('" + AscmWmsPreparationMain.StatusDefine.unPrepare + "','" + AscmWmsPreparationMain.StatusDefine.preparingUnConfirm + "', '" + AscmWmsPreparationMain.StatusDefine.preparing + "')";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                listAscmWmsPreparationMain = AscmWmsPreparationMainService.GetInstance().GetList(null, "", "", "", whereOther);

                if (!string.IsNullOrEmpty(linkIds))
                {
                    whereQueryWord = "materialId in (" + linkIds + ")";
                    whereDetailOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereDetailOther, whereQueryWord);
                }

                if (listAscmWmsPreparationMain != null && listAscmWmsPreparationMain.Count > 0)
                {
                    string mainIds = string.Join(",", listAscmWmsPreparationMain.Select(P => P.id.ToString()).Distinct());
                    whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(mainIds, "mainId");
                    whereDetailOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereDetailOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(whereDetailOther))
                    sql_Detail += " where " + whereDetailOther;

                List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetList(sql_Detail);
                if (listPreparationDetail != null && listPreparationDetail.Count > 0)
                {
                    //货位物料
                    string warehouseIds = "'" + string.Join("','", listPreparationDetail.Select(P => P.warehouseId).Distinct()) + "'";
                    List<AscmLocationMaterialLink> listAscmLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetListByWarehouseIds(warehouseIds, false, true);
                    if (listAscmLocationMaterialLink != null && listAscmLocationMaterialLink.Count > 0)
                    {
                        //设置物料货位
                        foreach (AscmWmsPreparationDetail preparationDetail in listPreparationDetail)
                        {
                            if (preparationDetail.warelocationId > 0)
                            {
                                AscmLocationMaterialLink locationMaterialLink = listAscmLocationMaterialLink.Find(P => P.warehouseId == preparationDetail.warehouseId && P.pk.materialId == preparationDetail.materialId);
                                if (locationMaterialLink != null)
                                {
                                    preparationDetail.warelocationId = 0;
                                    listPreparationDetailUpdate.Add(preparationDetail);
                                }
                            }
                        }
                    }
                }

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        //删除货位关联物料
                        if (listLocationMaterialLinkDelete.Count > 0)
                            YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listLocationMaterialLinkDelete);

                        //备料单明细更新"设置货位"
                        if (listPreparationDetailUpdate.Count > 0)
                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listPreparationDetailUpdate);

                        //删除未接收批单的货位分配
                        if (listAssignWarelocation != null && listAssignWarelocation.Count > 0)
                            YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAssignWarelocation);

                        tx.Commit();//正确执行提交
                        jsonObjectResult.result = true;
                        jsonObjectResult.message = message;
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
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        /*物料数据更改*/
        [HttpPost]
        public ActionResult MaterialSave(string detailJson)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (!string.IsNullOrEmpty(detailJson))
                {
                    List<AscmLocationMaterialLink> listLocationMaterialLink = null;
                    List<AscmLocationMaterialLink> listLocationMaterialLink_Model = JsonConvert.DeserializeObject<List<AscmLocationMaterialLink>>(detailJson);
                    List<AscmLocationMaterialLink> listLocationMaterialLink_Update = new List<AscmLocationMaterialLink>();
                    List<AscmLocationMaterialLinkLog> listLog = new List<AscmLocationMaterialLinkLog>();
                    if (listLocationMaterialLink_Model != null && listLocationMaterialLink_Model.Count() > 0)
                    {
                        listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList(" from AscmLocationMaterialLink");
                        int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmLocationMaterialLinkLog");
                        string userName = string.Empty;
                        if (User.Identity.IsAuthenticated)
                            userName = User.Identity.Name;
                        foreach (AscmLocationMaterialLink locationMaterialLink_Model in listLocationMaterialLink_Model)
                        {
                            #region 货位物料关联表
                            AscmLocationMaterialLink locationMaterialLink = new AscmLocationMaterialLink();
                            locationMaterialLink = locationMaterialLink_Model;
                            locationMaterialLink.modifyUser = userName;
                            locationMaterialLink.modifyTime = DateTime.Now.ToString();
                            listLocationMaterialLink_Update.Add(locationMaterialLink);
                            #endregion

                            #region 日志
                            AscmLocationMaterialLinkLog locationMaterialLinkLog = new AscmLocationMaterialLinkLog();
                            locationMaterialLinkLog.id = ++maxId;
                            locationMaterialLinkLog.modifyTime = DateTime.Now.ToString();
                            locationMaterialLinkLog.modifyUser = userName;
                            locationMaterialLinkLog.organizationId = 775;
                            locationMaterialLinkLog.newQuantity = locationMaterialLink_Model.quantity;
                            var find = listLocationMaterialLink.Find(item => item.pk.materialId == locationMaterialLink_Model.pk.materialId && item.pk.warelocationId == locationMaterialLink_Model.pk.warelocationId);
                            if (find != null)
                            {
                                locationMaterialLinkLog.materialId = find.pk.materialId;
                                locationMaterialLinkLog.warelocationId = find.pk.warelocationId;
                                locationMaterialLinkLog.oldQuantity = find.quantity;
                            }
                            listLog.Add(locationMaterialLinkLog);
                            #endregion
                        }
                    }
                    YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().Clear();
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            if (listLocationMaterialLink_Update != null && listLocationMaterialLink_Update.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdateList(listLocationMaterialLink_Update);
                            if (listLog != null && listLog.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listLog);

                            tx.Commit();
                            jsonObjectResult.result = true;
                            jsonObjectResult.message = "";
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();
                            throw ex;
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
        #endregion

        #region 货位转移查询
        public ActionResult LocationTransferQuery()
        {
            ViewData["dtStart"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewData["dtEnd"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }
        public ActionResult LocationTransferList(int? page, int? rows, string sort, string order, string startModifyTime, string endModifyTime)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereStartTime = "", whereEndTime = "";

                DateTime dtStartTime, dtEndTime;
                if (!string.IsNullOrEmpty(startModifyTime) && DateTime.TryParse(startModifyTime, out dtStartTime))
                    whereStartTime = "modifyTime>='" + dtStartTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endModifyTime) && DateTime.TryParse(endModifyTime, out dtEndTime))
                    whereEndTime = "modifyTime<'" + dtEndTime.ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndTime);

                List<AscmWmsLocationTransfer> list = AscmWmsLocationTransferService.GetInstance().GetList(ynPage, sort, order, "", whereOther);
                if (list != null)
                {
                    foreach (AscmWmsLocationTransfer ascmWmsLocationTransfer in list)
                    {
                        jsonDataGridResult.rows.Add(ascmWmsLocationTransfer);
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

        #region 作业领料单
        public ActionResult CuxWipReleaseQuery()
        {
            //物料需求表
            ViewData["startPlanTime"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewData["endPlanTime"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }
        public ActionResult CuxWipReleaseHeadersList(int? page, int? rows, string sort, string order, string queryWord,
            string releaseNumber, string releaseType, string startPlanTime, string endPlanTime, int? statusType, int? scheduleGroupId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmCuxWipReleaseHeaders> list = AscmCuxWipReleaseHeadersService.GetInstance().GetList(ynPage, releaseNumber, startPlanTime, endPlanTime, statusType);
            if (list != null && list.Count > 0)
            {
                AscmCuxWipReleaseHeadersService.GetInstance().SetScheduleGroups(list);
                jsonDataGridResult.rows.AddRange(list);
            }
            jsonDataGridResult.total = ynPage.GetRecordCount();

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CuxWipReleaseLinesView(int? id)
        {
            //作业领料单
            AscmCuxWipReleaseHeaders ascmCuxWipReleaseHeaders = null;
            try
            {
                if (id.HasValue)
                {
                    ascmCuxWipReleaseHeaders = AscmCuxWipReleaseHeadersService.GetInstance().Get(id.Value);
                    AscmCuxWipReleaseHeadersService.GetInstance().SetWipEntities(new List<AscmCuxWipReleaseHeaders> { ascmCuxWipReleaseHeaders });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ascmCuxWipReleaseHeaders);
        }
        public ActionResult CuxWipReleaseLinesList(int? id, string notInMaterialIds)
        {
            List<AscmCuxWipReleaseLines> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereNotInMaterialIds = "";
                if (!string.IsNullOrEmpty(notInMaterialIds))
                    whereNotInMaterialIds = "inventoryItemId not in(" + notInMaterialIds + ")";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereNotInMaterialIds);

                list = AscmCuxWipReleaseLinesService.GetInstance().GetList(id.Value, whereOther);
                foreach (AscmCuxWipReleaseLines ascmCuxWipReleaseLines in list)
                {
                    jsonDataGridResult.rows.Add(ascmCuxWipReleaseLines.GetOwner());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 作业需求报表
        public ActionResult WipDiscreteJobsQuery()
        {
            //作业需求报表
            return View();
        }
        public ActionResult WipDiscreteJobsQueryList(int? page, int? rows, string sort, string order, string queryWord,
            string wipEntitiesName, string startPlanTime, string endPlanTime, string status, int? scheduleGroupId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmWipRequirementOperations> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereWipDiscreteJobs = "", whereWipEntities = "";

                string whereWipEntitiesName = "", whereStartPlanTime = "", whereEndPlanTime = "", whereStatus = "", whereScheduleGroup = "";
                if (!string.IsNullOrEmpty(wipEntitiesName))
                    whereWipEntitiesName = "name like '%" + wipEntitiesName + "%'";
                if (!string.IsNullOrEmpty(status))
                    whereStatus = "statusType = '" + status + "'";
                if (scheduleGroupId.HasValue)
                    whereScheduleGroup = "scheduleGroupId=" + scheduleGroupId.Value;

                DateTime dtStartPlanTime, dtEndPlanTime;
                if (!string.IsNullOrEmpty(startPlanTime) && DateTime.TryParse(startPlanTime, out dtStartPlanTime))
                    whereStartPlanTime = "scheduledStartDate>='" + dtStartPlanTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endPlanTime) && DateTime.TryParse(endPlanTime, out dtEndPlanTime))
                    whereEndPlanTime = "scheduledStartDate<'" + dtEndPlanTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereWipDiscreteJobs = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipDiscreteJobs, whereStartPlanTime);
                whereWipDiscreteJobs = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipDiscreteJobs, whereEndPlanTime);
                whereWipDiscreteJobs = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipDiscreteJobs, whereStatus);
                whereWipDiscreteJobs = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipDiscreteJobs, whereScheduleGroup);
                whereWipEntities = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntities, whereWipEntitiesName);
                if (!string.IsNullOrEmpty(whereWipEntities))
                {
                    whereWipEntities = "wipEntityId in (select wipEntityId from AscmWipEntities where " + whereWipEntities + ")";// and numrow<100
                    whereWipDiscreteJobs = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipDiscreteJobs, whereWipEntities);
                }

                //if (!string.IsNullOrEmpty(whereWipDiscreteJobs))
                //{
                //    whereWipDiscreteJobs = "wipEntityId in (select wipEntityId from AscmWipDiscreteJobs where " + whereWipDiscreteJobs + ")";// and numrow<100
                //}

                //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereReleaseNumber);
                //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWipDiscreteJobs);

                list = AscmWipRequirementOperationsService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther, whereWipDiscreteJobs);
                foreach (AscmWipRequirementOperations ascmWipRequirementOperations in list)
                {
                    jsonDataGridResult.rows.Add(ascmWipRequirementOperations.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WipDiscreteJobsAscx(int? page, int? rows, string sort, string order, string queryWord,
            string startPlanTime, string endPlanTime, string status, int? scheduleGroupId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "";

                string whereStartPlanTime = "", whereEndPlanTime = "", whereStatus = "", whereScheduleGroup = "";
                if (!string.IsNullOrEmpty(status))
                    whereStatus = "statusType = '" + status + "'";
                if (scheduleGroupId.HasValue)
                    whereScheduleGroup = "scheduleGroupId=" + scheduleGroupId.Value;

                DateTime dtStartPlanTime, dtEndPlanTime;
                if (!string.IsNullOrEmpty(startPlanTime) && DateTime.TryParse(startPlanTime, out dtStartPlanTime))
                    whereStartPlanTime = "scheduledStartDate>='" + dtStartPlanTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endPlanTime) && DateTime.TryParse(endPlanTime, out dtEndPlanTime))
                    whereEndPlanTime = "scheduledStartDate<'" + dtEndPlanTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartPlanTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndPlanTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereScheduleGroup);

                List<AscmWipDiscreteJobs> list = AscmWipDiscreteJobsService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                {
                    jsonDataGridResult.rows.Add(ascmWipDiscreteJobs);
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WipRequireOperationAscx(int? wipEntityId, string notInMaterialIds)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (wipEntityId.HasValue)
                {
                    string whereOther = "", whereNotInMaterialIds = "";
                    if (!string.IsNullOrEmpty(notInMaterialIds))
                        whereNotInMaterialIds = "inventoryItemId not in(" + notInMaterialIds + ")";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereNotInMaterialIds);

                    List<AscmWipRequirementOperations> list = AscmWipRequirementOperationsService.GetInstance().GetList(wipEntityId.Value, whereOther);
                    if (list != null)
                    {
                        foreach (AscmWipRequirementOperations ascmWipRequirementOperations in list)
                        {
                            jsonDataGridResult.rows.Add(ascmWipRequirementOperations);
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
        #endregion

        #region 车间任务物料需求报表
        public ActionResult WipDiscreteJobsSumQuery()
        {
            //车间任务物料需求报表
            return View();
        }
        public ActionResult WipDiscreteJobsQuerySumList(int? page, int? rows, string sort, string order, string queryWord,
            string materialItem_DocNumber, string startPlanTime, string endPlanTime, string status, string supplySubinventory)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmWipRequirementOperations> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereWipDiscreteJobs = "";

                string whereReleaseNumber = "", whereStartPlanTime = "", whereEndPlanTime = "", whereStatus = "";
                if (!string.IsNullOrEmpty(materialItem_DocNumber))
                    whereReleaseNumber = " inventoryItemId in (select id from AscmMaterialItem where docNumber like '%" + materialItem_DocNumber + "%')";
                if (!string.IsNullOrEmpty(status))
                    whereStatus = "statusType = '" + status + "'";

                DateTime dtStartPlanTime, dtEndPlanTime;
                if (!string.IsNullOrEmpty(startPlanTime) && DateTime.TryParse(startPlanTime, out dtStartPlanTime))
                    whereStartPlanTime = "scheduledStartDate>='" + dtStartPlanTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                else
                    throw new Exception("必须输入时间范围!");
                if (!string.IsNullOrEmpty(endPlanTime) && DateTime.TryParse(endPlanTime, out dtEndPlanTime))
                    whereEndPlanTime = "scheduledStartDate<'" + dtEndPlanTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                else
                    throw new Exception("必须输入时间范围!");

                TimeSpan ts = dtEndPlanTime.Subtract(dtStartPlanTime);
                if (ts.Days > 2)
                    throw new Exception("查询时间范围不能超过2天!");
                whereWipDiscreteJobs = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipDiscreteJobs, whereStartPlanTime);
                whereWipDiscreteJobs = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipDiscreteJobs, whereEndPlanTime);
                whereWipDiscreteJobs = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipDiscreteJobs, whereStatus);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereReleaseNumber);

                //if (!string.IsNullOrEmpty(whereWipDiscreteJobs))
                //{
                //    whereWipDiscreteJobs = "wipEntityId in (select wipEntityId from AscmWipDiscreteJobs where " + whereWipDiscreteJobs + ")";// and numrow<100
                //}

                //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereReleaseNumber);
                //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWipDiscreteJobs);
                int organizationId = 775;
                //int.TryParse(YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketOrganizationId(),out organizationId);
                list = AscmWipRequirementOperationsService.GetInstance().GetSumList(ynPage, "", "", queryWord, organizationId, whereOther, whereWipDiscreteJobs, supplySubinventory);
                foreach (AscmWipRequirementOperations ascmWipRequirementOperations in list)
                {
                    jsonDataGridResult.rows.Add(ascmWipRequirementOperations.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 合单查询
        public ActionResult IncomingAcceptanceIndex()
        {
            return View();
        }
		public ActionResult IncAccRecDeliBatSumList(int? page, int? rows, string sort, string order, string barcode, string docNumber, int? supplierId, string returnCode, string startModifyTime, string endModifyTime)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            string hql = "from AscmDeliBatSumMain h";
            //供方简称
            string supplierShortName = "select vendorSiteCode from AscmSupplierAddress a where a.vendorSiteCodeAlt=s.docNumber and rownum=1";
            //容器数
            string containerQuantity = "select count(distinct d.containerSn) from AscmContainerDelivery d where d.batSumMainId=h.id";
            //入库容器数
            string checkContainerQuantity = "select count(distinct d.containerSn) from AscmContainerDelivery d where d.batSumMainId=h.id and d.status='" + AscmContainerDelivery.StatusDefine.inWarehouseDoor + "'";
            //总数
            string totalQuantity = "select sum(l.totalNumber) from AscmDeliBatSumDetail l where l.mainId=h.id";
            //入库数
            string checkQuantity = "select sum(d.quantity) from AscmContainerDelivery d where d.batSumMainId=h.id and d.status='" + AscmContainerDelivery.StatusDefine.inWarehouseDoor + "'";
            //容器绑定数
            string containerBindQuantity = "select sum(d.quantity) from AscmContainerDelivery d where d.batSumMainId=h.id";
            string newHql = string.Format("select new AscmDeliBatSumMain(h,s.docNumber,({0}),({1}),({2}),({3}),({4}),({5})) from AscmDeliBatSumMain h,AscmSupplier s", supplierShortName, containerQuantity, checkContainerQuantity, totalQuantity, checkQuantity, containerBindQuantity);

            string whereOther = string.Empty;
            //接收“已确认”和“入厂”的合单
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "h.status in('" + AscmDeliBatSumMain.StatusDefine.confirm + "','" + AscmDeliBatSumMain.StatusDefine.inPlant + "')");
            if (!string.IsNullOrWhiteSpace(barcode))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "h.id=(select mainId from AscmDeliBatSumDetail where barcode='" + barcode.Trim() + "' and rownum=1)");
            if (!string.IsNullOrWhiteSpace(docNumber))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "h.docNumber='" + docNumber.Trim().ToUpper() + "'");
            if (supplierId.HasValue)
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "h.supplierId='" + supplierId.Value + "'");

			string whereStartTime = "", whereEndTime = "";
			DateTime dtStartTime, dtEndTime;
			if (!string.IsNullOrEmpty(startModifyTime) && DateTime.TryParse(startModifyTime, out dtStartTime))
				whereStartTime = "h.modifyTime>='" + dtStartTime.ToString("yyyy-MM-dd 00:00:00") + "'";
			if (!string.IsNullOrEmpty(endModifyTime) && DateTime.TryParse(endModifyTime, out dtEndTime))
				whereEndTime = "h.modifyTime<'" + dtEndTime.ToString("yyyy-MM-dd 00:00:00") + "'";

			whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartTime);
			whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndTime);
			
			if (!string.IsNullOrEmpty(returnCode))
			{
				if (returnCode == "0")
				{
					whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "h.returnCode='0'");
				}
				else
				{
					whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "h.returnCode is not null and h.returnCode!='0'");
				}
			}

            hql += " where " + whereOther;

            string where = string.Empty;
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "h.supplierId=s.id");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
            newHql += " where " + where;

            string orderBy = string.Empty;
            if (!string.IsNullOrEmpty(sort))
            {
                orderBy = " order by h." + sort.Trim() + " ";
                if (!string.IsNullOrEmpty(order))
                    orderBy += order.Trim();
            }
            if (!string.IsNullOrEmpty(orderBy))
                newHql += orderBy;

            List<AscmDeliBatSumMain> listDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().GetList(ynPage, hql, newHql);
            if (listDeliBatSumMain != null && listDeliBatSumMain.Count > 0)
            {
                //设置接收时间
                AscmDeliBatSumMainService.GetInstance().SetAcceptTime(listDeliBatSumMain);
                jsonDataGridResult.rows.AddRange(listDeliBatSumMain);
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult IncomingAcceptanceDetailList(int? mainId, string barcode)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            if (mainId.HasValue)
            {
                string where = string.Empty;
                if (!string.IsNullOrWhiteSpace(barcode))
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "barcode='" + barcode + "'");
                List<AscmDeliBatSumDetail> list = AscmDeliBatSumDetailService.GetInstance().GetListByMainId(mainId.Value, where);
                if (list != null && list.Count > 0)
                {
                    List<AscmDeliveryOrderBatch> listDeliveryOrderBatch = new List<AscmDeliveryOrderBatch>();
                    list.ForEach(P => listDeliveryOrderBatch.Add(
                        new AscmDeliveryOrderBatch { 
                            id = P.batchId, 
                            barCode = P.batchBarCode, 
                            docNumber = P.batchDocNumber,
                            materialIdTmp = P.materialId, 
                            actualWarehouseId = P.batchWarehouseId }));
                    //获取货位分配
                    AscmDeliveryOrderBatchService.GetInstance().SetAssignWarelocation(listDeliveryOrderBatch);
                    list.ForEach(P => P.assignWarelocation = listDeliveryOrderBatch.Find(T => T.id == P.batchId).assignWarelocation);

                    //2014.4.3注释 合单查询时“接收数量”和“预分配货位”只获取不设置
                    ////设置接收数量
                    //BatchSetReceivedQuantity(listDeliveryOrderBatch);
                    //list.Where(P=>P.receivedQuantity == decimal.Zero).ToList().ForEach(P => P.receivedQuantity = listDeliveryOrderBatch.Find(T => T.id == P.batchId).receivedQuantity);
                    ////预分配货位
                    //BatchAssignWarelocation(listDeliveryOrderBatch);
                    //list.ForEach(P => P.assignWarelocation = listDeliveryOrderBatch.Find(T => T.id == P.batchId).assignWarelocation);

                    jsonDataGridResult.rows.AddRange(list);
                }
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        //合单导出excel
        public ActionResult IncAccExportMain( string barcode, string docNumber, int? supplierId, string returnCode, string startModifyTime, string endModifyTime)
        {
            IWorkbook wb = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Sheet1");
            sheet.SetColumnWidth(0, 6 * 256);
            sheet.SetColumnWidth(1, 16 * 256);
            sheet.SetColumnWidth(2, 12 * 256);
            sheet.SetColumnWidth(3, 12 * 256);
            sheet.SetColumnWidth(4, 12 * 256);
            sheet.SetColumnWidth(5, 10 * 256);
            sheet.SetColumnWidth(6, 9 * 256);
            sheet.SetColumnWidth(7, 11 * 256);
            sheet.SetColumnWidth(8, 9 * 256);
            sheet.SetColumnWidth(9, 9 * 256);
            sheet.SetColumnWidth(10, 11 * 256);
            sheet.SetColumnWidth(11, 18 * 256);
            sheet.SetColumnWidth(12, 18 * 256);
            sheet.SetColumnWidth(13, 18 * 256);
            sheet.SetColumnWidth(14, 12 * 256);
            sheet.SetColumnWidth(15, 18 * 256);
            sheet.SetColumnWidth(16, 18 * 256);
            sheet.SetColumnWidth(17, 18 * 256);

            IRow titleRow = sheet.CreateRow(0);
            titleRow.Height = 20 * 20;
            titleRow.CreateCell(0).SetCellValue("序号");
            titleRow.CreateCell(1).SetCellValue("合单号");
            titleRow.CreateCell(2).SetCellValue("供方编码");
            titleRow.CreateCell(3).SetCellValue("供方名称");
            titleRow.CreateCell(4).SetCellValue("送货仓库");
            titleRow.CreateCell(5).SetCellValue("状态");
            titleRow.CreateCell(6).SetCellValue("容器数");
            titleRow.CreateCell(7).SetCellValue("入库容器");
            titleRow.CreateCell(8).SetCellValue("总数");
            titleRow.CreateCell(9).SetCellValue("入库数");
            titleRow.CreateCell(10).SetCellValue("容器绑定数");
            titleRow.CreateCell(11).SetCellValue("最早到货时间");
            titleRow.CreateCell(12).SetCellValue("最晚到货时间");
            titleRow.CreateCell(13).SetCellValue("到厂时间");
            titleRow.CreateCell(14).SetCellValue("接收人");
            titleRow.CreateCell(15).SetCellValue("接收时间");
            titleRow.CreateCell(16).SetCellValue("上次状态");
            titleRow.CreateCell(17).SetCellValue("上传日期");

            string hql = "from AscmDeliBatSumMain h";
            //供方简称
            string supplierShortName = "select vendorSiteCode from AscmSupplierAddress a where a.vendorSiteCodeAlt=s.docNumber and rownum=1";
            //容器数
            string containerQuantity = "select count(distinct d.containerSn) from AscmContainerDelivery d where d.batSumMainId=h.id";
            //入库容器数
            string checkContainerQuantity = "select count(distinct d.containerSn) from AscmContainerDelivery d where d.batSumMainId=h.id and d.status='" + AscmContainerDelivery.StatusDefine.inWarehouseDoor + "'";
            //总数
            string totalQuantity = "select sum(l.totalNumber) from AscmDeliBatSumDetail l where l.mainId=h.id";
            //入库数
            string checkQuantity = "select sum(d.quantity) from AscmContainerDelivery d where d.batSumMainId=h.id and d.status='" + AscmContainerDelivery.StatusDefine.inWarehouseDoor + "'";
            //容器绑定数
            string containerBindQuantity = "select sum(d.quantity) from AscmContainerDelivery d where d.batSumMainId=h.id";
            string newHql = string.Format("select new AscmDeliBatSumMain(h,s.docNumber,({0}),({1}),({2}),({3}),({4}),({5})) from AscmDeliBatSumMain h,AscmSupplier s", supplierShortName, containerQuantity, checkContainerQuantity, totalQuantity, checkQuantity, containerBindQuantity);

            string whereOther = string.Empty;
            //接收“已确认”和“入厂”的合单
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "h.status in('" + AscmDeliBatSumMain.StatusDefine.confirm + "','" + AscmDeliBatSumMain.StatusDefine.inPlant + "')");
            if (!string.IsNullOrWhiteSpace(barcode))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "h.id=(select mainId from AscmDeliBatSumDetail where barcode='" + barcode.Trim() + "' and rownum=1)");
            if (!string.IsNullOrWhiteSpace(docNumber))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "h.docNumber='" + docNumber.Trim().ToUpper() + "'");
            if (supplierId.HasValue)
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "h.supplierId='" + supplierId.Value + "'");

            string whereStartTime = "", whereEndTime = "";
            DateTime dtStartTime, dtEndTime;
            if (!string.IsNullOrEmpty(startModifyTime) && DateTime.TryParse(startModifyTime, out dtStartTime))
                whereStartTime = "h.modifyTime>='" + dtStartTime.ToString("yyyy-MM-dd 00:00:00") + "'";
            if (!string.IsNullOrEmpty(endModifyTime) && DateTime.TryParse(endModifyTime, out dtEndTime))
                whereEndTime = "h.modifyTime<'" + dtEndTime.ToString("yyyy-MM-dd 00:00:00") + "'";

            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartTime);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndTime);

            if (!string.IsNullOrEmpty(returnCode))
            {
                if (returnCode == "0")
                {
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "h.returnCode='0'");
                }
                else
                {
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "h.returnCode is not null and h.returnCode!='0'");
                }
            }

            hql += " where " + whereOther;

            string where = string.Empty;
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "h.supplierId=s.id");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
            newHql += " where " + where;

            List<AscmDeliBatSumMain> listAscmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().GetList(null, hql, newHql);
           
            string fileDownloadName = HttpUtility.UrlEncode("合单"+DateTime.Now.ToString("yyyyMMddHHmmss")+".xls", Encoding.UTF8);
                if (listAscmDeliBatSumMain != null && listAscmDeliBatSumMain.Count > 0)
                {
                    AscmDeliBatSumMainService.GetInstance().SetAcceptTime(listAscmDeliBatSumMain);
                    AscmDeliBatSumMainService.GetInstance().SetSupplier(listAscmDeliBatSumMain);
                    int rowIndex = 0;
                    foreach (AscmDeliBatSumMain ascmDeliBatSumMain in listAscmDeliBatSumMain)
                    {
                        rowIndex++;
                        IRow row = sheet.CreateRow(rowIndex);
                        row.Height = 20 * 20;
                        row.CreateCell(0).SetCellValue(rowIndex);
                        row.CreateCell(1).SetCellValue(ascmDeliBatSumMain.docNumber);
                        row.CreateCell(2).SetCellValue(ascmDeliBatSumMain.supplierDocNumberCn);
                        row.CreateCell(3).SetCellValue(ascmDeliBatSumMain.supplierNameCn);
                        row.CreateCell(4).SetCellValue(ascmDeliBatSumMain.warehouseId);
                        row.CreateCell(5).SetCellValue(ascmDeliBatSumMain.statusCn);
                        row.CreateCell(6).SetCellValue(ascmDeliBatSumMain.containerQuantity.ToString());
                        row.CreateCell(7).SetCellValue(ascmDeliBatSumMain.checkContainerQuantity.ToString());
                        row.CreateCell(8).SetCellValue(ascmDeliBatSumMain.totalQuantity.ToString());
                        row.CreateCell(9).SetCellValue(ascmDeliBatSumMain.checkQuantity.ToString());
                        row.CreateCell(10).SetCellValue(ascmDeliBatSumMain.containerBindQuantity.ToString());
                        row.CreateCell(11).SetCellValue(ascmDeliBatSumMain.appointmentStartTimeShow);
                        row.CreateCell(12).SetCellValue(ascmDeliBatSumMain.appointmentEndTimeShow);
                        row.CreateCell(13).SetCellValue(ascmDeliBatSumMain._toPlantTime);
                        row.CreateCell(14).SetCellValue(ascmDeliBatSumMain.receiver);
                        row.CreateCell(15).SetCellValue(ascmDeliBatSumMain._acceptTime);
                        row.CreateCell(16).SetCellValue(ascmDeliBatSumMain.uploadStatusCn);
                        row.CreateCell(17).SetCellValue(ascmDeliBatSumMain.uploadTimeShow);
                    }            
                }

            byte[] buffer = new byte[] { };
            using (System.IO.MemoryStream stream = new MemoryStream())
            {
                wb.Write(stream);
                buffer = stream.GetBuffer();
            }
            return File(buffer, "application/vnd.ms-excel", fileDownloadName);
        }
        //合单明细导出excel
        public ActionResult IncAccExportDetail(int mainId, string docNumber)
        {
            IWorkbook wb = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Sheet1");
            sheet.SetColumnWidth(0, 6 * 256);
            sheet.SetColumnWidth(1, 10 * 256);
            sheet.SetColumnWidth(2, 12 * 256);
            sheet.SetColumnWidth(3, 16 * 256);
            sheet.SetColumnWidth(4, 26 * 256);
            sheet.SetColumnWidth(5, 9 * 256);
            sheet.SetColumnWidth(6, 9 * 256);
            sheet.SetColumnWidth(7, 9 * 256);
            sheet.SetColumnWidth(8, 14 * 256);
            sheet.SetColumnWidth(9, 15 * 256);
            sheet.SetColumnWidth(10, 13 * 256);
            sheet.SetColumnWidth(11, 18 * 256);
            sheet.SetColumnWidth(12, 10 * 256);
            sheet.SetColumnWidth(13, 18 * 256);
            sheet.SetColumnWidth(14, 18 * 256);

            IRow titleRow = sheet.CreateRow(0);
            titleRow.Height = 20 * 20;
            titleRow.CreateCell(0).SetCellValue("序号");
            titleRow.CreateCell(1).SetCellValue("执行结果");
            titleRow.CreateCell(2).SetCellValue("批条码号");
            titleRow.CreateCell(3).SetCellValue("物料编码");
            titleRow.CreateCell(4).SetCellValue("物料描述");
            titleRow.CreateCell(5).SetCellValue("送货数");
            titleRow.CreateCell(6).SetCellValue("校验数");
            titleRow.CreateCell(7).SetCellValue("接收数");
            titleRow.CreateCell(8).SetCellValue("收货子库");
            titleRow.CreateCell(9).SetCellValue("货位分配");
            titleRow.CreateCell(10).SetCellValue("容器绑定数");
            titleRow.CreateCell(11).SetCellValue("生成日期");
            titleRow.CreateCell(12).SetCellValue("状态");
            titleRow.CreateCell(13).SetCellValue("上传状态");
            titleRow.CreateCell(14).SetCellValue("上传日期");

            string fileDownloadName = HttpUtility.UrlEncode(docNumber + "_合单明细.xls", Encoding.UTF8);
            if (mainId >= 0)
            {
                List<AscmDeliBatSumDetail> listAscmDeliBatSumDetail = AscmDeliBatSumDetailService.GetInstance().GetListByMainId(mainId);
                if (listAscmDeliBatSumDetail != null && listAscmDeliBatSumDetail.Count > 0)
                {
                    int rowIndex = 0;
                    foreach (AscmDeliBatSumDetail ascmDeliBatSumDetail in listAscmDeliBatSumDetail)
                    {
                        rowIndex++;
                        IRow row = sheet.CreateRow(rowIndex);
                        row.Height = 20 * 20;
                        row.CreateCell(0).SetCellValue(rowIndex);
                        row.CreateCell(1).SetCellValue(ascmDeliBatSumDetail.ascmStatusCn);
                        row.CreateCell(2).SetCellValue(ascmDeliBatSumDetail.batchBarCode);
                        row.CreateCell(3).SetCellValue(ascmDeliBatSumDetail.materialDocNumber);
                        row.CreateCell(4).SetCellValue(ascmDeliBatSumDetail.materialDescription);
                        row.CreateCell(5).SetCellValue(ascmDeliBatSumDetail.totalNumber.ToString());
                        row.CreateCell(6).SetCellValue(ascmDeliBatSumDetail.checkQuantity.ToString());
                        row.CreateCell(7).SetCellValue(ascmDeliBatSumDetail.receivedQuantity.ToString());
                        row.CreateCell(8).SetCellValue(ascmDeliBatSumDetail.batchWarehouseId);
                        row.CreateCell(9).SetCellValue(ascmDeliBatSumDetail.assignWarelocation);
                        row.CreateCell(10).SetCellValue(ascmDeliBatSumDetail.containerBindNumber.ToString());
                        row.CreateCell(11).SetCellValue(ascmDeliBatSumDetail.batchCreateTime);
                        row.CreateCell(12).SetCellValue(ascmDeliBatSumDetail.batchStatusCn);
                        row.CreateCell(13).SetCellValue(ascmDeliBatSumDetail.uploadStatusCn);
                        row.CreateCell(14).SetCellValue(ascmDeliBatSumDetail.uploadTimeShow);
                    }
                }
            }

            byte[] buffer = new byte[] { };
            using (System.IO.MemoryStream stream = new MemoryStream())
            {
                wb.Write(stream);
                buffer = stream.GetBuffer();
            }
            return File(buffer, "application/vnd.ms-excel", fileDownloadName);
        }
        #endregion

        #region 合单接收
        public ActionResult DeliBatSumIncAcc()
        {
            return View();
        }

		/// <summary>合单接收--添加</summary>
		public ActionResult DeliBatSumIncAccAdd(string deliBatSumBarcode)
        {
            JsonEntityResult jsonResult = new JsonEntityResult();
            if (!string.IsNullOrWhiteSpace(deliBatSumBarcode))
            {
                //通过合单条码获取合单明细
                string hql = "from AscmDeliBatSumDetail where barcode='" + deliBatSumBarcode.Trim() + "'";
                List<AscmDeliBatSumDetail> listDetail = AscmDeliBatSumDetailService.GetInstance().GetList(hql);
                if (listDetail == null || listDetail.Count == 0)
                {
                    jsonResult.retCode = 1;
                    jsonResult.retMessage = "合单条码不存在";
                    jsonResult.key = deliBatSumBarcode;
                    return Json(jsonResult, JsonRequestBehavior.AllowGet);
                }

                //获取合单号
                int batSumMainId = listDetail.FirstOrDefault().mainId;
                string deliBatSumDocNumber = string.Empty;
                hql = "select docNumber from AscmDeliBatSumMain where id=" + batSumMainId;
                object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(hql);
                if (obj != null)
                    deliBatSumDocNumber = obj.ToString();

                //获取送货批
                //入库校验数
                string checkQuantity = "select sum(quantity) from AscmContainerDelivery where deliveryOrderBatchId=t.id and batSumMainId=" + batSumMainId + " and status='" + AscmContainerDelivery.StatusDefine.inWarehouseDoor + "'";
                //接收数量
                string receivedQuantity = "select sum(receivedQuantity) from AscmDeliBatOrderLink where batchId=t.id";
                hql = string.Format("select new AscmDeliveryOrderBatch(t,({0}),({1})) from AscmDeliveryOrderBatch t", receivedQuantity, checkQuantity);
                string where = string.Empty;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "id in(" + string.Join(",", listDetail.Select(P => P.batchId)) + ")");
                hql += " where " + where;
                List<AscmDeliveryOrderBatch> listDeliveryOrderBatch = AscmDeliveryOrderBatchService.GetInstance().GetList(hql);
                if (listDeliveryOrderBatch == null || listDeliveryOrderBatch.Count == 0)
                {
                    jsonResult.retCode = 1;
                    jsonResult.retMessage = "找不到送货批";
                    jsonResult.key = deliBatSumBarcode;
                    return Json(jsonResult, JsonRequestBehavior.AllowGet);
                }
                foreach (AscmDeliveryOrderBatch deliveryOrderBatch in listDeliveryOrderBatch)
                {
                    AscmDeliBatSumDetail deliBatSumDetail = listDetail.Find(P => P.batchId == deliveryOrderBatch.id);
                    if (deliBatSumDetail != null)
                    {
                        //指定"物料Id"
                        deliveryOrderBatch.materialIdTmp = deliBatSumDetail.materialId;
                        //指定"送货数量"
                        deliveryOrderBatch.totalNumber = deliBatSumDetail.totalNumber;
                    }
                }
                //设置物料
                AscmDeliveryOrderBatchService.GetInstance().SetMaterial(listDeliveryOrderBatch);
                //设置货位
                AscmDeliveryOrderBatchService.GetInstance().SetAssignWarelocation(listDeliveryOrderBatch);

                jsonResult.jsonResults = new List<JsonEntityResult>();
                //可以接收的批单
                List<AscmDeliveryOrderBatch> listQualifiedDeliveryOrderBatch = new List<AscmDeliveryOrderBatch>();
                foreach (AscmDeliveryOrderBatch deliveryOrderBatch in listDeliveryOrderBatch)
                {
                    if (deliveryOrderBatch.status == AscmDeliveryOrderBatch.StatusDefine.cancel)
                        jsonResult.jsonResults.Add(new JsonEntityResult { retCode = 2, retMessage = "此单已取消", key = deliveryOrderBatch.barCode });
                    else if (deliveryOrderBatch.status == AscmDeliveryOrderBatch.StatusDefine.closed)
                        jsonResult.jsonResults.Add(new JsonEntityResult { retCode = 2, retMessage = "此单已关闭", key = deliveryOrderBatch.barCode });
                    else  if (deliveryOrderBatch.ascmStatus == AscmDeliveryOrderBatch.AscmStatusDefine.received)
                        jsonResult.jsonResults.Add(new JsonEntityResult { retCode = 2, retMessage = "此单已接收", key = deliveryOrderBatch.barCode });
                    else
                    {
                        ////设置送货批接收数量
                        //SetReceivedQuantity(deliveryOrderBatch);
                        ////预分配货位
                        //AssignWarelocation(deliveryOrderBatch);

                        deliveryOrderBatch.deliBatSumDocNumber = deliBatSumDocNumber;
                        deliveryOrderBatch.deliBatSumBarcode = deliBatSumBarcode;
                        listQualifiedDeliveryOrderBatch.Add(deliveryOrderBatch);
                    }
                }
                //批量设置送货批接收数量
                BatchSetReceivedQuantity(listQualifiedDeliveryOrderBatch);
                //批量预分配货位
                BatchAssignWarelocation(listQualifiedDeliveryOrderBatch);
                //按物料编码排序
                listQualifiedDeliveryOrderBatch.OrderBy(P => P.materialDocNumber).ToList().ForEach(P => jsonResult.jsonResults.Add(new JsonEntityResult { success = true, key = P.barCode, entity = P }));

                jsonResult.success = true;
            }
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>批量设置送货批接收数量</summary>
        public void BatchSetReceivedQuantity(List<AscmDeliveryOrderBatch> list)
        {
            if (list == null || list.Count == 0)
                return;

            //获取已设置接收数量的批单
            string sql = "select batchId from ascm_deli_bat_order_link where batchId in(" + string.Join(",", list.Select(P => P.id)) + ")";
            IList ilist = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);
            List<string> listBatchId = new List<string>();
            foreach (object[] item in ilist)
            {
                if (item != null && item.Length > 0)
                    listBatchId.Add(item[0].ToString());
            }
            if (listBatchId.Count >= list.Count)
                return;

            //设置送货批接收数量
            var deliveryOrderBatchs = list.Where(P => !listBatchId.Contains(P.id.ToString()));
            string hql = "select new AscmDeliveryOrderDetail(d,m.batchId,m.wipEntityId) from AscmDeliveryOrderDetail d,AscmDeliveryOrderMain m";
            string where = string.Empty;
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "d.mainId=m.id");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "m.batchId in(" + string.Join(",", deliveryOrderBatchs.Select(P => P.id)) + ")");
            hql += " where " + where;
            List<AscmDeliveryOrderDetail> listDeliOrderDetail = AscmDeliveryOrderDetailService.GetInstance().GetList(hql);
            if (listDeliOrderDetail == null && listDeliOrderDetail.Count == 0)
                return;

            List<AscmDeliBatOrderLink> listDeliBatOrderLink = new List<AscmDeliBatOrderLink>();
            foreach (AscmDeliveryOrderBatch deliveryOrderBatch in deliveryOrderBatchs)
            {
                deliveryOrderBatch.receivedQuantity = decimal.Zero;
                foreach (AscmDeliveryOrderDetail deliveryOrderDetail in listDeliOrderDetail.Where(P => P.batchId == deliveryOrderBatch.id))
                {
                    AscmDeliBatOrderLink deliBatOrderLink = new AscmDeliBatOrderLink();
                    deliBatOrderLink.id = deliveryOrderDetail.id;
                    deliBatOrderLink.batchId = deliveryOrderBatch.id;
                    deliBatOrderLink.batchBarCode = deliveryOrderBatch.barCode;
                    deliBatOrderLink.batchDocNumber = deliveryOrderBatch.docNumber;
                    deliBatOrderLink.warehouseId = deliveryOrderBatch.receivedWarehouseId;
                    deliBatOrderLink.mainId = deliveryOrderDetail.mainId;
                    deliBatOrderLink.wipEntityId = deliveryOrderDetail.wipEntityId;
                    deliBatOrderLink.materialId = deliveryOrderDetail.materialId;
                    deliBatOrderLink.deliveryQuantity = deliveryOrderDetail.deliveryQuantity;
                    deliBatOrderLink.receivedQuantity = deliveryOrderDetail.deliveryQuantity;
                    listDeliBatOrderLink.Add(deliBatOrderLink);

                    deliveryOrderBatch.receivedQuantity += deliveryOrderDetail.deliveryQuantity;
                }
            }
            AscmDeliBatOrderLinkService.GetInstance().SaveOrUpdate(listDeliBatOrderLink);
        }
        /// <summary>批量预分配货位</summary>
        public void BatchAssignWarelocation(List<AscmDeliveryOrderBatch> list)
        { 
            if (list == null || list.Count == 0)
                return;

            //获取未分配货位的批单
            var result = list.Where(P => string.IsNullOrEmpty(P.assignWarelocation));
            if (result == null || result.Count() == 0)
                return;

            result.ToList().ForEach(P => AssignWarelocation(P));
        }
        /// <summary>设置送货批接收数量</summary>
        public void SetReceivedQuantity(AscmDeliveryOrderBatch deliveryOrderBatch)
        {
            //如果不存在相关数据，则说明未设置接收数量
            int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(id) from AscmDeliBatOrderLink where batchId=" + deliveryOrderBatch.id);
            if (count > 0)
                return;

            //获取送货单明细
            //string hql = "from AscmDeliveryOrderDetail d where exists(select 1 from AscmDeliveryOrderMain m where m.id=d.mainId and m.batchId=" + deliveryOrderBatch.id + ")";
            //List<AscmDeliveryOrderDetail> listDeliOrderDetail = AscmDeliveryOrderDetailService.GetInstance().GetList(hql);
            //if (listDeliOrderDetail == null || listDeliOrderDetail.Count == 0)
            //    return;
            //获取送货单明细，并增加作业信息
            string hql = "select new AscmDeliveryOrderDetail(d,m.batchId,m.wipEntityId) from AscmDeliveryOrderDetail d,AscmDeliveryOrderMain m";
            string where = string.Empty;
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "d.mainId=m.id");
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "m.batchId=" + deliveryOrderBatch.id);
            hql += " where " + where;
            List<AscmDeliveryOrderDetail> listDeliOrderDetail = AscmDeliveryOrderDetailService.GetInstance().GetList(hql);
            if (listDeliOrderDetail == null || listDeliOrderDetail.Count == 0)
                return;

            //系统设置接收数量为送货数量
            deliveryOrderBatch.receivedQuantity = decimal.Zero;
            List<AscmDeliBatOrderLink> listDeliBatOrderLink = new List<AscmDeliBatOrderLink>();
            foreach (AscmDeliveryOrderDetail deliveryOrderDetail in listDeliOrderDetail)
            {
                AscmDeliBatOrderLink deliBatOrderLink = new AscmDeliBatOrderLink();
                deliBatOrderLink.id = deliveryOrderDetail.id;
                deliBatOrderLink.batchId = deliveryOrderBatch.id;
                deliBatOrderLink.batchBarCode = deliveryOrderBatch.barCode;
                deliBatOrderLink.batchDocNumber = deliveryOrderBatch.docNumber;
                deliBatOrderLink.warehouseId = deliveryOrderBatch.receivedWarehouseId;
                deliBatOrderLink.mainId = deliveryOrderDetail.mainId;
                deliBatOrderLink.wipEntityId = deliveryOrderDetail.wipEntityId;
                deliBatOrderLink.materialId = deliveryOrderDetail.materialId;
                deliBatOrderLink.deliveryQuantity = deliveryOrderDetail.deliveryQuantity;
                deliBatOrderLink.receivedQuantity = deliveryOrderDetail.deliveryQuantity;
                listDeliBatOrderLink.Add(deliBatOrderLink);

                deliveryOrderBatch.receivedQuantity += deliveryOrderDetail.deliveryQuantity;
            }
            AscmDeliBatOrderLinkService.GetInstance().SaveOrUpdate(listDeliBatOrderLink);
        }
        /// <summary>预分配货位</summary>
        public void AssignWarelocation(AscmDeliveryOrderBatch deliveryOrderBatch)
        {
            if (!string.IsNullOrEmpty(deliveryOrderBatch.assignWarelocation))
                return;
            
            //预分配货位
            //未预分配货位的批单，获取满足条件的货位
            string whereOther = string.Empty;
            string whereMaterial = "id in(select pk.warelocationId from AscmLocationMaterialLink where pk.materialId=" + deliveryOrderBatch.materialIdTmp + ")";
            string whereCategory = "categoryCode='0000'"; //物料大类为‘0000’时表示通用，即货位可以存放任何物料
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereMaterial + " or " + whereCategory);
            //初次预分配货位时将物料分配到满足条件的第一个货位上
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "rownum=1");
            List<AscmWarelocation> listWarelocation = AscmWarelocationService.GetInstance().GetList("docNumber", "asc", null, deliveryOrderBatch.receivedWarehouseId, whereOther);
            if (listWarelocation == null || listWarelocation.Count == 0)
                return;
            
            //预分配货位
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
                userName = User.Identity.Name;
            AscmWarelocation warelocation = listWarelocation.FirstOrDefault();
            string maxIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_assign_warelocation_id", "", "", 10, 1);
            int maxId = Convert.ToInt32(maxIdKey);
            AscmAssignWarelocation assignWarelocation = new AscmAssignWarelocation();
            assignWarelocation.id = maxId;
            assignWarelocation.organizationId = 775;
            assignWarelocation.createUser = userName;
            assignWarelocation.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            assignWarelocation.modifyUser = userName;
            assignWarelocation.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            assignWarelocation.batchId = deliveryOrderBatch.id;
            assignWarelocation.batchBarCode = deliveryOrderBatch.barCode;
            assignWarelocation.batchDocNumber = deliveryOrderBatch.docNumber;
            assignWarelocation.warelocationId = warelocation.id;
            assignWarelocation.assignQuantity = deliveryOrderBatch.receivedQuantity;
            assignWarelocation.materialId = deliveryOrderBatch.materialIdTmp;
            AscmAssignWarelocationService.GetInstance().Save(assignWarelocation);
            //显示分配的货位
            deliveryOrderBatch.assignWarelocation = warelocation.docNumber;
        }
        public class JsonEntityResult
        {
            public bool success { get; set; }
            public int retCode { get; set; }
            public string retMessage { get; set; }
            public string key { get; set; }
            public object entity { get; set; }
            public List<JsonEntityResult> jsonResults { get; set; }
        }
        #endregion

        #region 批单接收
        public ActionResult DeliBatIncAccEdit()
        {
            return View();
        }

		/// <summary>批单接收--添加</summary>
		public ActionResult DeliBatIncAccAdd(string batchBarCode)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            if (!string.IsNullOrWhiteSpace(batchBarCode))
            {
                //获取送货批
                string totalQuantity = "select sum(deliveryQuantity) from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId=t.id)";
                string materialId = "select materialId from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId=t.id) and rownum=1";
                string receivedQuantity = "select sum(receivedQuantity) from AscmDeliBatOrderLink where batchId=t.id"; //接收数量
                string hql = string.Format("select new AscmDeliveryOrderBatch(t,0L,({0}),({1}),({2})) from AscmDeliveryOrderBatch t", totalQuantity, materialId, receivedQuantity);
                string where = string.Empty;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "barCode='" + batchBarCode.Trim() + "'");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "rownum=1");
                hql += " where " + where;
                List<AscmDeliveryOrderBatch> listDeliveryOrderBatch = AscmDeliveryOrderBatchService.GetInstance().GetList(hql);
                if (listDeliveryOrderBatch == null || listDeliveryOrderBatch.Count == 0)
                {
                    jsonObjectResult.message = "送货批不存在";
                    return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
                }

                //设置物料
                AscmDeliveryOrderBatchService.GetInstance().SetMaterial(listDeliveryOrderBatch);
                //设置货位
                AscmDeliveryOrderBatchService.GetInstance().SetAssignWarelocation(listDeliveryOrderBatch);

                //送货批状态判断
                AscmDeliveryOrderBatch deliveryOrderBatch = listDeliveryOrderBatch.FirstOrDefault();
                if (deliveryOrderBatch.status == AscmDeliveryOrderBatch.StatusDefine.cancel)
                    jsonObjectResult.message = "此单已取消";
                else if (deliveryOrderBatch.status == AscmDeliveryOrderBatch.StatusDefine.closed)
                    jsonObjectResult.message = "此单已关闭";
                else if (deliveryOrderBatch.ascmStatus == AscmDeliveryOrderBatch.AscmStatusDefine.received)
                    jsonObjectResult.message = "此单已接收";
                else
                {
                    //设置接收数量
                    SetReceivedQuantity(deliveryOrderBatch);
                    //预分配货位
                    AssignWarelocation(deliveryOrderBatch);

                    jsonObjectResult.result = true;
                    jsonObjectResult.entity = deliveryOrderBatch;
                }
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeliBatOrderLinkDetail(int? id)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            if (id.HasValue)
            {
                //2014.4.3 实体AscmDeliBatOrderLink中增加字段：作业ID、物料ID
                string wipEntityName = "select name from AscmWipEntities where wipEntityId=l.wipEntityId";
                string wipEntityStatus = "select statusType from AscmWipDiscreteJobs where wipEntityId=l.wipEntityId";
                string hql = string.Format("select new AscmDeliBatOrderLink(l,({0}),({1}),m.docNumber,m.description,m.unit) from AscmDeliBatOrderLink l,AscmMaterialItem m", wipEntityName, wipEntityStatus);
                string where = string.Empty;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "l.materialId=m.id");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "l.batchId=" + id.Value);
                hql += " where " + where;
                List<AscmDeliBatOrderLink> list = AscmDeliBatOrderLinkService.GetInstance().GetList(hql);
                
                //获取2014.4.3 以前的数据
                if (list == null || list.Count == 0)
                {
                    wipEntityName = "select name from AscmWipEntities where wipEntityId=h.wipEntityId";
                    wipEntityStatus = "select statusType from AscmWipDiscreteJobs where wipEntityId=h.wipEntityId";
                    hql = string.Format("select new AscmDeliBatOrderLink(l,({0}),({1}),m.docNumber,m.description,m.unit) from AscmDeliBatOrderLink l,AscmDeliveryOrderDetail d,AscmDeliveryOrderMain h,AscmMaterialItem m", wipEntityName, wipEntityStatus);
                    where = string.Empty;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "l.id=d.id");
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "l.mainId=h.id");
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "d.materialId=m.id");
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "l.batchId=" + id.Value);
                    hql += " where " + where;
                    list = AscmDeliBatOrderLinkService.GetInstance().GetList(hql);
                }

                if (list != null && list.Count > 0)
                    jsonDataGridResult.rows.AddRange(list);
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeliBatReceivedQuantityUpdate(int? batchId, decimal? receivedQuantity)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (batchId.HasValue && receivedQuantity.HasValue)
                {
                    string userName = string.Empty;
                    if (User.Identity.IsAuthenticated)
                        userName = User.Identity.Name;
                    //1、重新分配送货单接收数量
                    string sql = "from AscmDeliBatOrderLink where batchId=" + batchId.Value;
                    List<AscmDeliBatOrderLink> listDeliBatOrderLink = AscmDeliBatOrderLinkService.GetInstance().GetList(sql);
                    if (listDeliBatOrderLink != null && listDeliBatOrderLink.Count > 0)
                    {
                        decimal _receivedQuantity = receivedQuantity.Value;
                        foreach (AscmDeliBatOrderLink deliBatOrderLink in listDeliBatOrderLink)
                        {
                            if (_receivedQuantity >= deliBatOrderLink.deliveryQuantity)
                                deliBatOrderLink.receivedQuantity = deliBatOrderLink.deliveryQuantity;
                            else
                                deliBatOrderLink.receivedQuantity = _receivedQuantity;
                            _receivedQuantity -= deliBatOrderLink.receivedQuantity;
                            deliBatOrderLink.modifyUser = userName;
                            deliBatOrderLink.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        }
                    }
                    //2、重新分配货位
                    sql = "from AscmAssignWarelocation where batchId=" + batchId.Value;
                    List<AscmAssignWarelocation> listAssignWarelocation = AscmAssignWarelocationService.GetInstance().GetList(sql);
                    List<AscmAssignWarelocation> listAssignWarelocationUpdate = null;
                    List<AscmAssignWarelocation> listAssignWarelocationDelete = null;
                    if (listAssignWarelocation != null && listAssignWarelocation.Count > 0)
                    {
                        int i = 0;
                        listAssignWarelocationUpdate = new List<AscmAssignWarelocation>();
                        listAssignWarelocationDelete = new List<AscmAssignWarelocation>();
                        foreach (AscmAssignWarelocation assignWarelocation in listAssignWarelocation)
                        {
                            if (i == 0)
                            {
                                assignWarelocation.assignQuantity = receivedQuantity.Value;
                                assignWarelocation.modifyUser = userName;
                                assignWarelocation.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                listAssignWarelocationUpdate.Add(assignWarelocation);
                            }
                            else
                            {
                                listAssignWarelocationDelete.Add(assignWarelocation);
                            }
                            i++;
                        }
                    }

                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            if (listDeliBatOrderLink != null && listDeliBatOrderLink.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listDeliBatOrderLink);

                            if (listAssignWarelocationUpdate != null && listAssignWarelocationUpdate.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAssignWarelocationUpdate);

                            if (listAssignWarelocationDelete != null && listAssignWarelocationDelete.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAssignWarelocationDelete);

                            tx.Commit();
                            jsonObjectResult.result = true;
                            jsonObjectResult.message = "";
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();
                            throw ex;
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
        public ActionResult DeliBatReceivedWarehouseUpdate(int? batchId, string receivedWarehouseId, int? materialId, decimal? receivedQuantity)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (batchId.HasValue && !string.IsNullOrEmpty(receivedWarehouseId) && materialId.HasValue && receivedQuantity.HasValue)
                {
                    AscmDeliveryOrderBatch deliveryOrderBatch = AscmDeliveryOrderBatchService.GetInstance().Get(batchId.Value);
                    if (deliveryOrderBatch != null && deliveryOrderBatch.receivedWarehouseId != receivedWarehouseId)
                    {
                        string userName = string.Empty;
                        if (User.Identity.IsAuthenticated)
                            userName = User.Identity.Name;

                        //指定实际接收子库
                        deliveryOrderBatch.actualWarehouseId = receivedWarehouseId;
                        deliveryOrderBatch.assignWarelocation = string.Empty;
                        //更新接收子库
                        string hql = "from AscmDeliBatOrderLink where batchId=" + batchId.Value;
                        List<AscmDeliBatOrderLink> listDeliBatOrderLink = AscmDeliBatOrderLinkService.GetInstance().GetList(hql);
                        if (listDeliBatOrderLink != null && listDeliBatOrderLink.Count > 0)
                        {
                            foreach (AscmDeliBatOrderLink deliBatOrderLink in listDeliBatOrderLink)
                            {
                                deliBatOrderLink.warehouseId = receivedWarehouseId;
                                deliBatOrderLink.modifyUser = userName;
                                deliBatOrderLink.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            }
                        }
                        //获取前子库的货位分配
                        hql = "from AscmAssignWarelocation where batchId=" + batchId.Value;
                        List<AscmAssignWarelocation> listAssignWarelocation = AscmAssignWarelocationService.GetInstance().GetList(hql);

                        //为新指定的收货子库分配货位
                        AscmAssignWarelocation assignWarelocation = null;
                        //1、获取满足条件的货位 
                        string whereOther = "";
                        string whereMaterial = "id in(select pk.warelocationId from AscmLocationMaterialLink where pk.materialId=" + materialId.Value + ")";
                        string whereCategory = "categoryCode='0000'"; //物料大类为‘0000’时表示通用，即货位可以存放任何物料
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereMaterial + " or " + whereCategory);
                        List<AscmWarelocation> listWarelocation = AscmWarelocationService.GetInstance().GetList("docNumber", "asc", null, deliveryOrderBatch.receivedWarehouseId, whereOther);
                        //2、预分配货位
                        if (listWarelocation != null && listWarelocation.Count > 0)
                        {
                            //初次预分配货位时将物料分配到满足条件的第一个货位上
                            AscmWarelocation warelocation = listWarelocation.First();
                            string maxIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_assign_warelocation_id", "", "", 10, 1);
                            int maxId = Convert.ToInt32(maxIdKey);
                            assignWarelocation = new AscmAssignWarelocation();
                            assignWarelocation.id = maxId;
                            assignWarelocation.organizationId = 775;
                            assignWarelocation.createUser = userName;
                            assignWarelocation.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            assignWarelocation.modifyUser = userName;
                            assignWarelocation.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            assignWarelocation.batchId = deliveryOrderBatch.id;
                            assignWarelocation.batchBarCode = deliveryOrderBatch.barCode;
                            assignWarelocation.batchDocNumber = deliveryOrderBatch.docNumber;
                            assignWarelocation.warelocationId = warelocation.id;
                            assignWarelocation.assignQuantity = receivedQuantity.Value;
                            assignWarelocation.materialId = materialId.Value;
                            //显示分配的货位
                            deliveryOrderBatch.assignWarelocation = warelocation.docNumber;
                        }

                        //提交
                        using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                        {
                            try
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.Update(deliveryOrderBatch);

                                if (listDeliBatOrderLink != null && listDeliBatOrderLink.Count > 0)
                                    YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listDeliBatOrderLink);

                                if (listAssignWarelocation != null && listAssignWarelocation.Count > 0)
                                    YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAssignWarelocation);

                                if (assignWarelocation != null)
                                    YnDaoHelper.GetInstance().nHibernateHelper.Save(assignWarelocation);

                                tx.Commit();
                                jsonObjectResult.result = true;
                                jsonObjectResult.entity = deliveryOrderBatch;
                                jsonObjectResult.message = "";
                            }
                            catch (Exception ex)
                            {
                                tx.Rollback();
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
        public ActionResult DeliBatAssignWarelocation(string batchIds, int? buildingId, string warehouseId)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            //送货批单
            List<AscmDeliveryOrderBatch> list = null;
            if (!string.IsNullOrEmpty(batchIds))
            {
                string whereOther = string.Empty;
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " id in(" + batchIds + ")");
                list = AscmDeliveryOrderBatchService.GetInstance().GetIncomingAcceptanceList(null, null, null, "", whereOther);
            }
            //货位及货位与物料关联
            List<AscmWarelocation> listWarelocation = null;
            List<AscmLocationMaterialLink> listLocationMaterialLink = null;
            if (list != null && list.Count > 0)
            {
                string warehouseIds = string.Empty, materialIds = string.Empty;
                foreach (AscmDeliveryOrderBatch deliveryOrderBatch in list)
                {
                    if (!string.IsNullOrEmpty(warehouseIds))
                        warehouseIds += ",";
                    warehouseIds += "'" + deliveryOrderBatch.receivedWarehouseId + "'";
                    if (!string.IsNullOrEmpty(materialIds))
                        materialIds += ",";
                    materialIds += deliveryOrderBatch.materialIdTmp;
                }
                //通过物料获取满足条件的货位
                string sql = "from AscmLocationMaterialLink";
                string whereOther = string.Empty;
                if (!string.IsNullOrEmpty(materialIds))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " pk.materialId in(" + materialIds + ")");
                if (!string.IsNullOrEmpty(whereOther))
                    sql += " where " + whereOther;
                listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList(sql);
                //通过子库和物料获取可以分配的货位
                whereOther = string.Empty;
                if (string.IsNullOrEmpty(warehouseId) && !string.IsNullOrEmpty(warehouseIds))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " warehouseId in(" + warehouseIds + ")");
                string whereMaterial = " categoryCode='0000'";  //物料大类为‘0000’时表示通用，即货位可以存放任何物料
                if (listLocationMaterialLink != null)
                {
                    string warelocationIds = string.Empty;
                    foreach (AscmLocationMaterialLink locationMaterialLink in listLocationMaterialLink)
                    {
                        if (!string.IsNullOrEmpty(warelocationIds))
                            warelocationIds += ",";
                        warelocationIds += locationMaterialLink.pk.warelocationId;
                    }
                    if (!string.IsNullOrEmpty(warelocationIds))
                        whereMaterial += " or id in(" + warelocationIds + ")";
                }
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereMaterial);
                listWarelocation = AscmWarelocationService.GetInstance().GetList("docNumber", "asc", buildingId, warehouseId, whereOther);
            }
            //分配货位
            List<AscmAssignWarelocation> listAssignWarelocation = null;
            if (listWarelocation != null && listWarelocation.Count > 0)
            {
                //获取已预分配货位的数据
                string warelocationIds = string.Empty;
                foreach (AscmWarelocation ascmWarelocation in listWarelocation)
                {
                    if (!string.IsNullOrEmpty(warelocationIds))
                        warelocationIds += ",";
                    warelocationIds += ascmWarelocation.id;
                }
                string whereOther = string.Empty;
                if (!string.IsNullOrEmpty(warelocationIds))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " warelocationId in(" + warelocationIds + ")");
                if (!string.IsNullOrEmpty(batchIds))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " batchId in(" + batchIds + ")");
                listAssignWarelocation = AscmAssignWarelocationService.GetInstance().GetList(null, "", "", "", whereOther);
                //开始预分配货位
                foreach (AscmDeliveryOrderBatch deliveryOrderBatch in list)
                {
                    //判断送货批是否存在货位分配
                    bool isExist = listAssignWarelocation.Exists(P => listWarelocation.Select(T => T.id).Contains(P.warelocationId)
                                                              && P.batchId == deliveryOrderBatch.id
                                                              && P.materialId == deliveryOrderBatch.materialIdTmp);
                    int i = 0;
                    foreach (AscmWarelocation ascmWarelocation in listWarelocation)
                    {
                        //判断仓库子库是否匹配
                        bool isWarehouse = ascmWarelocation.warehouseId == deliveryOrderBatch.receivedWarehouseId;
                        //判断货位物料是否匹配
                        bool isMaterial = ascmWarelocation.categoryCode == "0000" ||
                            listLocationMaterialLink.Exists(P => P.pk.warelocationId == ascmWarelocation.id && P.pk.materialId == deliveryOrderBatch.materialIdTmp);
                        //必须满足以上两个条件才能进行货位分配（注：当所选货位是通用货位时，是没有预先为货位指定物料的，到货接收成功后需增加对应的货位物料关联信息）
                        if (isWarehouse && isMaterial)
                        {
                            AscmAssignWarelocation assignWarelocation = null;
                            if (listAssignWarelocation != null && listAssignWarelocation.Count > 0)
                            {
                                assignWarelocation = listAssignWarelocation.Find(P => P.warelocationId == ascmWarelocation.id
                                    && P.batchId == deliveryOrderBatch.id
                                    && P.materialId == deliveryOrderBatch.materialIdTmp);
                            }
                            if (assignWarelocation == null)
                            {
                                assignWarelocation = new AscmAssignWarelocation();
                                if (i == 0 && !isExist)
                                {
                                    //初次预分配货位时将物料分配到满足条件的第一个货位上
                                    assignWarelocation.assignQuantity = deliveryOrderBatch.receivedQuantity;
                                }
                            }
                            assignWarelocation.warelocationId = ascmWarelocation.id;
                            assignWarelocation.locationDocNumber = ascmWarelocation.docNumber;
                            assignWarelocation.categoryCode = ascmWarelocation.categoryCode;
                            assignWarelocation.batchId = deliveryOrderBatch.id;
                            assignWarelocation.batchBarCode = deliveryOrderBatch.barCode;
                            assignWarelocation.batchDocNumber = deliveryOrderBatch.docNumber;
                            assignWarelocation.materialId = deliveryOrderBatch.materialIdTmp;
                            assignWarelocation.materialDocNumber = deliveryOrderBatch.materialDocNumber;
                            //获取货位上对应物料的已存数量
                            if (listLocationMaterialLink != null)
                            {
                                AscmLocationMaterialLink locationMaterialLink = listLocationMaterialLink.Find(P => P.pk.warelocationId == ascmWarelocation.id
                                    && P.pk.materialId == deliveryOrderBatch.materialIdTmp);
                                if (locationMaterialLink != null)
                                    assignWarelocation.quantity = locationMaterialLink.quantity;
                            }
                            jsonDataGridResult.rows.Add(assignWarelocation);
                            i++;
                        }
                    }
                }
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeliBatAssignWarelocationSave(string batchIds, string assignWarelocationJson)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                List<AscmAssignWarelocation> listAssignWarelocationDelete = null;
                List<AscmAssignWarelocation> listAssignWarelocationSaveOrUpdate = null;
                //刷新批单货位分配
                List<AscmDeliveryOrderBatch> listDeliveryOrderBatch = new List<AscmDeliveryOrderBatch>();
                if (!string.IsNullOrEmpty(batchIds) && !string.IsNullOrEmpty(assignWarelocationJson))
                {
                    List<AscmAssignWarelocation> listAssignWarelocation_Model = JsonConvert.DeserializeObject<List<AscmAssignWarelocation>>(assignWarelocationJson);
                    if (listAssignWarelocation_Model != null && listAssignWarelocation_Model.Count > 0)
                    {
                        string ids = string.Empty;
                        foreach (AscmAssignWarelocation assignWarelocation_Model in listAssignWarelocation_Model)
                        {
                            if (assignWarelocation_Model.id > 0)
                            {
                                if (!string.IsNullOrEmpty(ids))
                                    ids += ",";
                                ids += assignWarelocation_Model.id;
                            }
                        }
                        List<AscmAssignWarelocation> listAssignWarelocation = null;
                        if (!string.IsNullOrEmpty(ids))
                            listAssignWarelocation = AscmAssignWarelocationService.GetInstance().GetList(null, "", "", "", "id in(" + ids + ")");

                        string userName = string.Empty;
                        if (User.Identity.IsAuthenticated)
                            userName = User.Identity.Name;
                        int maxId = 0;
                        listAssignWarelocationDelete = new List<AscmAssignWarelocation>();
                        listAssignWarelocationSaveOrUpdate = new List<AscmAssignWarelocation>();
                        foreach (AscmAssignWarelocation assignWarelocation_Model in listAssignWarelocation_Model)
                        {
                            AscmAssignWarelocation assignWarelocation = null;
                            if (listAssignWarelocation != null)
                                assignWarelocation = listAssignWarelocation.Find(P => P.id == assignWarelocation_Model.id);
                            if (assignWarelocation != null && assignWarelocation_Model.assignQuantity == 0)
                                listAssignWarelocationDelete.Add(assignWarelocation);
                            if (assignWarelocation_Model.assignQuantity == 0)
                                continue;
                            if (assignWarelocation == null)
                            {
                                assignWarelocation = new AscmAssignWarelocation();
                                assignWarelocation.id = --maxId;
                                assignWarelocation.createUser = userName;
                                assignWarelocation.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            }
                            assignWarelocation.batchId = assignWarelocation_Model.batchId;
                            assignWarelocation.batchDocNumber = assignWarelocation_Model.batchDocNumber;
                            assignWarelocation.warelocationId = assignWarelocation_Model.warelocationId;
                            assignWarelocation.materialId = assignWarelocation_Model.materialId;
                            assignWarelocation.assignQuantity = assignWarelocation_Model.assignQuantity;
                            assignWarelocation.modifyUser = userName;
                            assignWarelocation.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            listAssignWarelocationSaveOrUpdate.Add(assignWarelocation);
                            //更新批单货位分配显示
                            AscmDeliveryOrderBatch deliveryOrderBatch = listDeliveryOrderBatch.Find(P => P.id == assignWarelocation.batchId);
                            if (deliveryOrderBatch == null)
                            {
                                deliveryOrderBatch = new AscmDeliveryOrderBatch();
                                deliveryOrderBatch.id = assignWarelocation.batchId;
                                deliveryOrderBatch.assignWarelocation = string.Empty;
                                listDeliveryOrderBatch.Add(deliveryOrderBatch);
                            }
                            if (!string.IsNullOrEmpty(deliveryOrderBatch.assignWarelocation))
                                deliveryOrderBatch.assignWarelocation += "、";
                            deliveryOrderBatch.assignWarelocation += assignWarelocation_Model.locationDocNumber;
                        }
                        if (maxId < 0)
                        {
                            string maxIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_assign_warelocation_id", "", "", 10, Math.Abs(maxId));
                            maxId = Convert.ToInt32(maxIdKey);
                            foreach (AscmAssignWarelocation assignWarelocation in listAssignWarelocationSaveOrUpdate)
                            {
                                if (assignWarelocation.id < 0)
                                    assignWarelocation.id = maxId++;
                            }
                        }
                    }
                }

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        if (listAssignWarelocationSaveOrUpdate != null && listAssignWarelocationSaveOrUpdate.Count > 0)
                            YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdateList(listAssignWarelocationSaveOrUpdate);

                        if (listAssignWarelocationDelete != null && listAssignWarelocationDelete.Count > 0)
                            YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAssignWarelocationDelete);

                        tx.Commit();
                        jsonObjectResult.result = true;
                        jsonObjectResult.message = "";
                        jsonObjectResult.entity = listDeliveryOrderBatch;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeliBatAssignIncAccComplete(string batchIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (string.IsNullOrEmpty(batchIds))
                {
                    jsonObjectResult.message = "参数传值错误";
                    return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
                }
                //获取送货单明细扩展信息
                List<AscmDeliBatOrderLink> listDeliBatOrderLink = AscmDeliBatOrderLinkService.GetInstance().GetList("from AscmDeliBatOrderLink where batchId in(" + batchIds + ")");
                if (listDeliBatOrderLink == null || listDeliBatOrderLink.Count == 0)
                {
                    jsonObjectResult.message = "获取送货单明细扩展信息失败";
                    return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
                }
                //获取批送货单
                List<AscmDeliveryOrderBatch> listDeliveryOrderBatch = AscmDeliveryOrderBatchService.GetInstance().GetList("from AscmDeliveryOrderBatch where id in(" + batchIds + ")");
                if (listDeliveryOrderBatch == null || listDeliveryOrderBatch.Count == 0)
                {
                    jsonObjectResult.message = "获取送货批单失败";
                    return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
                }
                //获取货位预分配信息
                List<AscmAssignWarelocation> listAssignWarelocation = AscmAssignWarelocationService.GetInstance().GetList("from AscmAssignWarelocation where batchId in(" + batchIds + ")");
                if (listAssignWarelocation == null || listAssignWarelocation.Count == 0)
                {
                    jsonObjectResult.message = "获取货位分配数据失败，可能原因是货位已被移除或是货位上的物料已被移除";
                    return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
                }
                //获取货位与物料的关联信息
                List<AscmLocationMaterialLink> listLocationMaterialLink = new List<AscmLocationMaterialLink>();
                string warelocationIds = string.Empty;
                foreach (AscmAssignWarelocation assignWarelocation in listAssignWarelocation)
                {
                    if (!string.IsNullOrEmpty(warelocationIds))
                        warelocationIds += ",";
                    warelocationIds += assignWarelocation.warelocationId;
                }
                if (!string.IsNullOrEmpty(warelocationIds))
                    listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList("from AscmLocationMaterialLink where warelocationId in(" + warelocationIds + ")");
                if (listLocationMaterialLink.Count == 0)
                {
                    jsonObjectResult.message = "获取货位的物料数据失败，可能原因是货位已被移除或是货位上的物料已被移除";
                    return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
                }
                //获取登录用户ID
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;
                //接收失败批单
                List<AscmDeliveryOrderBatch> listDeliveryOrderBatchFail = new List<AscmDeliveryOrderBatch>();
                //与MES交互日志列表
                List<AscmMesInteractiveLog> listAscmMesInteractiveLog = new List<AscmMesInteractiveLog>();
                foreach (AscmDeliveryOrderBatch ascmDeliveryOrderBatch in listDeliveryOrderBatch)
                {
                    AscmMesInteractiveLog ascmMesInteractiveLog = new AscmMesInteractiveLog();
                    ascmMesInteractiveLog.billId = ascmDeliveryOrderBatch.id;
                    ascmMesInteractiveLog.docNumber = ascmDeliveryOrderBatch.barCode;
                    ascmMesInteractiveLog.billType = AscmMesInteractiveLog.BillTypeDefine.incAccSystem;
                    ascmMesInteractiveLog.createUser = userName;
                    ascmMesInteractiveLog.modifyUser = userName;
                    ascmMesInteractiveLog.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmMesInteractiveLog.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                    //获取送货批下送货单明细扩展信息
                    var findDeliBatOrderLink = listDeliBatOrderLink.Where(P => P.batchId == ascmDeliveryOrderBatch.id);
                    if (findDeliBatOrderLink != null)
                    {
                        //只取接收数量大于0的送货单传给MES
                        var findListDeliBatOrderLink = findDeliBatOrderLink.Where(P => P.receivedQuantity > decimal.Zero).ToList();
                        List<AscmLocationMaterialLink> listLocationMaterialLinkSaveOrUpdate = new List<AscmLocationMaterialLink>();
                        //更新货位物料库存
                        foreach (AscmAssignWarelocation assignWarelocation in listAssignWarelocation)
                        {
                            if (assignWarelocation.batchId != ascmDeliveryOrderBatch.id)
                                continue;
                            AscmLocationMaterialLinkPK pk = new AscmLocationMaterialLinkPK { warelocationId = assignWarelocation.warelocationId, materialId = assignWarelocation.materialId };
                            AscmLocationMaterialLink locationMaterialLink = listLocationMaterialLink.Find(P => P.pk.Equals(pk));
                            if (locationMaterialLink == null)
                            {
                                locationMaterialLink = new AscmLocationMaterialLink();
                                locationMaterialLink.pk = pk;
                                locationMaterialLink.createUser = userName;
                                locationMaterialLink.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            }
                            locationMaterialLink.modifyUser = userName;
                            locationMaterialLink.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            locationMaterialLink.quantity += assignWarelocation.assignQuantity;
                            listLocationMaterialLinkSaveOrUpdate.Add(locationMaterialLink);
                        }
                        //执行事务
                        YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().Clear();
                        using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                        {
                            try
                            {
                                //调用MES接口，执行到货接收系统单
                                AscmMesService.GetInstance().DoSysReceive(findListDeliBatOrderLink, userName, ascmMesInteractiveLog);
								ascmDeliveryOrderBatch.returnCode = ascmMesInteractiveLog.returnCode;
								ascmDeliveryOrderBatch.returnMessage = ascmMesInteractiveLog.returnMessage;
								if (ascmMesInteractiveLog.returnCode == "0")
								{
									//更改送货批状态
									ascmDeliveryOrderBatch.ascmStatus = AscmDeliveryOrderBatch.AscmStatusDefine.received;
									if (listLocationMaterialLinkSaveOrUpdate != null) 
									{
										YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdateList(listLocationMaterialLinkSaveOrUpdate);
									}
									ascmMesInteractiveLog.returnMessage = "接收成功";
								}
								YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmDeliveryOrderBatch);

                                tx.Commit();
                            }
                            catch (Exception ex)
                            {
                                tx.Rollback();
                                ascmMesInteractiveLog.returnCode = "-1";
                                ascmMesInteractiveLog.returnMessage = ex.Message;
                            }
                        }
                    }
                    listAscmMesInteractiveLog.Add(ascmMesInteractiveLog);
                }
                string maxLogIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_mes_interactive_log_id", "", "", 10, listAscmMesInteractiveLog.Count);
                int maxLogId = Convert.ToInt32(maxLogIdKey);
                string errorMsg = string.Empty;
                foreach (AscmMesInteractiveLog ascmMesInteractiveLog in listAscmMesInteractiveLog)
                {
                    ascmMesInteractiveLog.id = maxLogId++;
                    if (ascmMesInteractiveLog.returnCode != "0")
                    {
                        AscmDeliveryOrderBatch failAscmDeliveryOrderBatch = listDeliveryOrderBatch.Find(P => P.id == ascmMesInteractiveLog.billId && P.ascmStatus != AscmDeliveryOrderBatch.AscmStatusDefine.received);
                        if (failAscmDeliveryOrderBatch != null)
                        {
                            failAscmDeliveryOrderBatch.ascmStatus = AscmDeliveryOrderBatch.AscmStatusDefine.receiveFail;
                            listDeliveryOrderBatchFail.Add(failAscmDeliveryOrderBatch);
                        }
                        errorMsg += string.Format("<li>送货批<font color='red'>{0}</font>：{1}</li>", ascmMesInteractiveLog.docNumber, ascmMesInteractiveLog.returnMessage);
                    }
                }
                if (!string.IsNullOrEmpty(errorMsg))
                    errorMsg = string.Format("<ul>{0}</ul>", errorMsg);
                //更改接收失败的送货批状态
                if (listDeliveryOrderBatchFail.Count > 0)
                    YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listDeliveryOrderBatchFail);
                //保存与MES的交互日志
                AscmMesInteractiveLogService.GetInstance().Save(listAscmMesInteractiveLog);
                //更改合单状态
                string where = string.Empty;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "status='" + AscmDeliBatSumMain.StatusDefine.confirm + "'");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "id in(select mainId from AscmDeliBatSumDetail where batchId in(" + string.Join(",", listDeliveryOrderBatch.Select(P => P.id).Distinct()) + "))");
                string hql = "from AscmDeliBatSumMain";
                hql += " where " + where;
                List<AscmDeliBatSumMain> listAscmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().GetList(hql);
				List<AscmDeliBatSumDetail> listAscmDeliBatSumDetail = AscmDeliBatSumDetailService.GetInstance().GetList(" from AscmDeliBatSumDetail where batchId in(" + string.Join(",", listDeliveryOrderBatch.Select(P => P.id).Distinct()) + ") ");
				if (listAscmDeliBatSumMain == null) listAscmDeliBatSumMain = new List<AscmDeliBatSumMain>();
				if (listAscmDeliBatSumDetail == null) listAscmDeliBatSumDetail = new List<AscmDeliBatSumDetail>();

				foreach (var deliveryOrderBatch in listDeliveryOrderBatch)
				{
					var findDeliBatSumDetails = listAscmDeliBatSumDetail.Where(p => p.batchId == deliveryOrderBatch.id).ToList();
					if (findDeliBatSumDetails != null) 
					{
						foreach (var detail in findDeliBatSumDetails)
						{
							detail.returnCode = deliveryOrderBatch.returnCode;
							detail.returnMessage = deliveryOrderBatch.returnMessage;
							detail.uploadTime = AscmUtl.Now;
						}
					}
				}

				foreach (var deliBatSumDetail in listAscmDeliBatSumDetail)
				{
					var findDeliBatSumMains = listAscmDeliBatSumMain.Where(p => p.id == deliBatSumDetail.mainId).ToList();
					if (findDeliBatSumMains != null)
					{
						foreach (var main in findDeliBatSumMains)
						{
							main.returnCode = deliBatSumDetail.returnCode;
							main.returnMessage = deliBatSumDetail.returnMessage;
							main.uploadTime = AscmUtl.Now;
						}
					}
				}

                if (listAscmDeliBatSumMain != null && listAscmDeliBatSumMain.Count > 0)
                {
                    listAscmDeliBatSumMain.ForEach(P => P.status = AscmDeliBatSumMain.StatusDefine.inPlant);
                    AscmDeliBatSumMainService.GetInstance().Update(listAscmDeliBatSumMain);
                }
				if (listAscmDeliBatSumDetail != null && listAscmDeliBatSumDetail.Count > 0)
				{
					AscmDeliBatSumDetailService.GetInstance().Update(listAscmDeliBatSumDetail);
				}

                jsonObjectResult.result = true;
                jsonObjectResult.message = errorMsg;
                jsonObjectResult.entity = listAscmMesInteractiveLog;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 物料标签打印
        public ActionResult DeliBatMaterialAscxList(int? page, int? rows, string sort, string order, string queryWord,
            string receivedBatchIds, string startReceivedTime, string endReceivedTime, string receivedUserId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            //获取接收成功的批单ID
            string whereOther = string.Empty;
            //显示本次接收的批单
            if (!string.IsNullOrEmpty(receivedBatchIds))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "billId in(" + receivedBatchIds + ")");
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "billType='" + AscmMesInteractiveLog.BillTypeDefine.incAccSystem + "'");
            //接收成功的批单
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "returnCode='0'");
            if (!string.IsNullOrEmpty(receivedUserId))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "createUser='" + receivedUserId + "'");
            //接收时间
            string whereStartReceivedTime = "", whereEndReceivedTime = "";
            DateTime dtStartReceivedTime, dtEndReceivedTime;
            if (!string.IsNullOrEmpty(startReceivedTime) && DateTime.TryParse(startReceivedTime, out dtStartReceivedTime))
            {
                whereStartReceivedTime = "createTime>='" + dtStartReceivedTime.ToString("yyyy-MM-dd HH:mm") + "'";
                whereEndReceivedTime = "createTime<'" + dtStartReceivedTime.AddDays(1).ToString("yyyy-MM-dd 00:00") + "'";
            }
            if (!string.IsNullOrEmpty(endReceivedTime) && DateTime.TryParse(endReceivedTime, out dtEndReceivedTime))
                whereEndReceivedTime = "createTime<'" + dtEndReceivedTime.AddDays(1).ToString("yyyy-MM-dd HH:mm") + "'";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartReceivedTime);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndReceivedTime);
            List<AscmMesInteractiveLog> listMesInteractiveLog = AscmMesInteractiveLogService.GetInstance().GetList(ynPage, sort, order, "", whereOther);
            if (listMesInteractiveLog != null && listMesInteractiveLog.Count > 0)
            {
                string hql = "select new AscmAssignWarelocation(aaw,ami.docNumber,ami.description,aw.warehouseId,aw.docNumber) from AscmAssignWarelocation aaw,AscmMaterialItem ami,AscmWarelocation aw";
                string where = string.Empty;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "aaw.materialId=ami.id");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "aaw.warelocationId=aw.id");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "aaw.batchId in(" + string.Join(",", listMesInteractiveLog.Select(P => P.billId)) + ")");
                hql += " where " + where;
                List<AscmAssignWarelocation> listAssignWarelocation = AscmAssignWarelocationService.GetInstance().GetList(hql);
                if (listAssignWarelocation != null && listAssignWarelocation.Count > 0)
                {
                    //获取批单对应的供应商简称
                    //供方简称
                    string supplierShortName = "select vendorSiteCode from AscmSupplierAddress a where a.vendorSiteCodeAlt=s.docNumber and rownum=1";
                    hql = string.Format("select new AscmDeliveryOrderBatch(b.id,({0})) from AscmDeliveryOrderBatch b,AscmSupplier s", supplierShortName);
                    where = string.Empty;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "b.supplierId=s.id");
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "b.id in(" + string.Join(",", listMesInteractiveLog.Select(P => P.billId)) + ")");
                    hql += " where " + where;
                    List<AscmDeliveryOrderBatch> listDeliveryOrderBatch = AscmDeliveryOrderBatchService.GetInstance().GetList(hql);

                    var gb = listAssignWarelocation.GroupBy(P => P.batchId);
                    foreach (IGrouping<int, AscmAssignWarelocation> ig in gb)
                    {
                        var first = ig.FirstOrDefault();
                        AscmAssignWarelocation assignWarelocation = new AscmAssignWarelocation();
                        assignWarelocation.batchId = ig.Key;
                        assignWarelocation.batchBarCode = first.batchBarCode;
                        assignWarelocation.materialDocNumber = first.materialDocNumber;
                        assignWarelocation.materialDescription = first.materialDescription;
                        assignWarelocation.assignQuantity = ig.Sum(P => P.assignQuantity);
                        assignWarelocation.warehouseId = first.warehouseId;
                        assignWarelocation.locationDocNumber = string.Join("、", ig.Select(P => P.locationDocNumber));
                        if (listDeliveryOrderBatch != null && listDeliveryOrderBatch.Count > 0)
                        {
                            AscmDeliveryOrderBatch deliveryOrderBatch = listDeliveryOrderBatch.Find(P => P.id == ig.Key);
                            if (deliveryOrderBatch != null)
                                assignWarelocation.supplierShortName = deliveryOrderBatch.ispShortName;
                        }
                        jsonDataGridResult.rows.Add(assignWarelocation);
                    }
                    jsonDataGridResult.total = ynPage.GetRecordCount();
                }
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult WmsMaterialLabelPrint(string printType, string dataJson)
        {
            if (string.IsNullOrEmpty(dataJson))
                return Json(new { success = false, errorMsg = "参数传值错误" }, JsonRequestBehavior.AllowGet);
            List<AscmAssignWarelocation> listAssignWarelocation = JsonConvert.DeserializeObject<List<AscmAssignWarelocation>>(dataJson);
            if (listAssignWarelocation == null || listAssignWarelocation.Count == 0)
                return Json(new { success = false, errorMsg = "参数序列化错误" }, JsonRequestBehavior.AllowGet);

            List<AscmWmsMaterialLabel> list = new List<AscmWmsMaterialLabel>();
            if (printType == AscmWmsMaterialLabel.PrintType.batchPrint)
            {
                foreach (AscmAssignWarelocation assignWarelocation in listAssignWarelocation)
                {
                    AscmWmsMaterialLabel materialLabel = CreateMaterialLabel(assignWarelocation, null, null);
                    materialLabel.printType = AscmWmsMaterialLabel.PrintType.batchPrint;
                    list.Add(materialLabel);
                }
            }
            else if (printType == AscmWmsMaterialLabel.PrintType.wipEntityPrint)
            {
                //获取送货单
                List<AscmDeliveryOrderMain> listDeliveryOrderMain = null;
                var batchIds = listAssignWarelocation.Select(P => P.batchId);
                if (batchIds.Count() > 0)
                {
                    string totalNumber = "select sum(deliveryQuantity) from AscmDeliveryOrderDetail where mainId=m.id";
                    string hql = string.Format("select new AscmDeliveryOrderMain(m.id,m.wipEntityId,m.batchId,({0})) from AscmDeliveryOrderMain m", totalNumber);
                    string where = string.Empty;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "m.batchId in(" + string.Join(",", batchIds) + ")");
                    hql += " where " + where;
                    listDeliveryOrderMain = AscmDeliveryOrderMainService.GetInstance().GetList(hql);
                }
                if (listDeliveryOrderMain == null || listDeliveryOrderMain.Count == 0)
                    return Json(new { success = false, errorMsg = "获取送货单失败" }, JsonRequestBehavior.AllowGet);

                //获取作业信息
                List<AscmWipDiscreteJobs> listWipDiscreteJobs = null;
                var wipEntityIds = listDeliveryOrderMain.Where(P => P.wipEntityId > 0).Select(P => P.wipEntityId);
                if (wipEntityIds.Count() > 0)
                {
                    string hql = "select new AscmWipDiscreteJobs(j.wipEntityId,j.scheduledStartDate,j.netQuantity,e.name) from AscmWipDiscreteJobs j,AscmWipEntities e";
                    string where = string.Empty;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "j.wipEntityId=e.wipEntityId");
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "j.wipEntityId in(" + string.Join(",", wipEntityIds) + ")");
                    hql += " where " + where;
                    listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList(hql);
                }

                foreach (AscmAssignWarelocation assignWarelocation in listAssignWarelocation)
                {
                    int count = 0;
                    if (listWipDiscreteJobs != null && listWipDiscreteJobs.Count > 0)
                    {
                        var deliveryOrderMains = listDeliveryOrderMain.Where(P => P.batchId == assignWarelocation.batchId);
                        if (deliveryOrderMains != null)
                        {
                            foreach (AscmDeliveryOrderMain deliveryOrderMain in deliveryOrderMains)
                            {
                                AscmWipDiscreteJobs wipDiscreteJobs = listWipDiscreteJobs.Find(P => P.wipEntityId == deliveryOrderMain.wipEntityId);
                                if (wipDiscreteJobs != null)
                                {
                                    count++;
                                    AscmWmsMaterialLabel materialLabel = CreateMaterialLabel(assignWarelocation, deliveryOrderMain, wipDiscreteJobs);
                                    materialLabel.printType = AscmWmsMaterialLabel.PrintType.wipEntityPrint;
                                    list.Add(materialLabel);
                                }
                            }
                        }
                    }
                    if (count == 0)
                    {
                        AscmWmsMaterialLabel materialLabel = CreateMaterialLabel(assignWarelocation, null, null);
                        materialLabel.printType = AscmWmsMaterialLabel.PrintType.wipEntityPrint;
                        list.Add(materialLabel);
                    }
                }
            }
            if (list.Count > 0)
            {
                string labelNoKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("MaterialLabelNo", "", "", 12, list.Count);
                Int64 labelNo = Convert.ToInt64(labelNoKey);
                foreach (AscmWmsMaterialLabel materialLabel in list)
                {
                    materialLabel.labelNo = labelNo.ToString().PadLeft(12, '0');
                    labelNo++;
                }
                AscmWmsMaterialLabelService.GetInstance().Save(list);
            }

            return Json(new { success = true, data = list }, JsonRequestBehavior.AllowGet);
        }
        private AscmWmsMaterialLabel CreateMaterialLabel(AscmAssignWarelocation assignWarelocation, AscmDeliveryOrderMain deliveryOrderMain, AscmWipDiscreteJobs wipDiscreteJobs)
        {
            if (assignWarelocation == null)
                return null;

            AscmWmsMaterialLabel materialLabel = new AscmWmsMaterialLabel();
            materialLabel.title = "物料标签";
            materialLabel.warehouseId = assignWarelocation.warehouseId;
            if (wipDiscreteJobs != null)
            {
                materialLabel.wipEntityDate = wipDiscreteJobs.scheduledStartDate;
                materialLabel.wipEntityName = wipDiscreteJobs.wipEntityName;
                materialLabel.wipEntityQuantity = wipDiscreteJobs.netQuantity;
            }

            materialLabel.locationDocNumber = assignWarelocation.locationDocNumber;//yfj 货位编码
            materialLabel.supplierShortName = assignWarelocation.supplierShortName;
            materialLabel.quantity = deliveryOrderMain != null ? deliveryOrderMain.totalNumber : assignWarelocation.assignQuantity;
            materialLabel.enterWarehouseDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            materialLabel.materialId = assignWarelocation.materialId;
            materialLabel.materialDocNumber = assignWarelocation.materialDocNumber;
            materialLabel.materialDescription = assignWarelocation.materialDescription;
            materialLabel.checkResult = AscmWmsMaterialLabel.CheckResult.unCheck;
            return materialLabel;
        }
        #endregion

        #region 手工单接收
        public ActionResult WmsIncManAccEdit(int? id)
        {
            //手工单接受
            AscmWmsIncManAccMain ascmWmsIncManAccMain = new AscmWmsIncManAccMain();
            try
            {
                if (id.HasValue)
                {
                    ascmWmsIncManAccMain = AscmWmsIncManAccMainService.GetInstance().Get(id.Value);
                    YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight();
                    ynWebRight.rightAdd = false;
                    ynWebRight.rightDelete = false;
                    ynWebRight.rightEdit = false;
                }
            }
            catch (Exception ex)
            {
            }
            return View(ascmWmsIncManAccMain);
        }
        [HttpPost]
        public ActionResult WmsIncManAccSave(AscmWmsIncManAccMain ascmWmsIncManAccMain_Model, int? id, string detailJson)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                #region 检验
                if (string.IsNullOrEmpty(detailJson))
                {
                    throw new Exception("系统错误：送货单明细不能为NULL或空");
                }
                List<AscmWmsIncManAccDetail> listAscmWmsIncManAccDetail_Model = JsonConvert.DeserializeObject<List<AscmWmsIncManAccDetail>>(detailJson);
                if (ascmWmsIncManAccMain_Model.supplierId == 0)
                {
                    throw new Exception("请输入供应商！");
                }
                if (ascmWmsIncManAccMain_Model.supplierAddressId == 0)
                {
                    throw new Exception("请输入供应商地址！");
                }
                if (string.IsNullOrEmpty(ascmWmsIncManAccMain_Model.warehouseId))
                {
                    throw new Exception("请输入收货子库！");
                }
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;
                #endregion

                #region 主表
                AscmWmsIncManAccMain ascmWmsIncManAccMain = null;
                List<AscmWmsIncManAccDetail> listAscmWmsIncManAccDetail = null;
                if (id.HasValue)
                {
                    ascmWmsIncManAccMain = AscmWmsIncManAccMainService.GetInstance().Get(id.Value);
                    listAscmWmsIncManAccDetail = AscmWmsIncManAccDetailService.GetInstance().GetList(id.Value);
                }
                else
                {
                    ascmWmsIncManAccMain = new AscmWmsIncManAccMain();
                    ascmWmsIncManAccMain.organizationId = 775;
                    ascmWmsIncManAccMain.createUser = userName;
                    ascmWmsIncManAccMain.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsIncManAccMain");
                    ascmWmsIncManAccMain.id = ++maxId;
                    ascmWmsIncManAccMain.responsiblePerson = userName;
                    //获取MES闭环单号
                    ascmWmsIncManAccMain.docNumber = AscmMesService.GetInstance().GetMesManualBillNo();
                }
                if (ascmWmsIncManAccMain == null)
                    throw new Exception("提交手工单失败！");

                ascmWmsIncManAccMain.modifyUser = userName;
                ascmWmsIncManAccMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ascmWmsIncManAccMain.wipEntityId = 0;
                ascmWmsIncManAccMain.supplierId = ascmWmsIncManAccMain_Model.supplierId;
                ascmWmsIncManAccMain.supplierAddressId = ascmWmsIncManAccMain_Model.supplierAddressId;
                ascmWmsIncManAccMain.supperWarehouse = ascmWmsIncManAccMain_Model.supperWarehouse;
                ascmWmsIncManAccMain.supperPlateNumber = ascmWmsIncManAccMain_Model.supperPlateNumber;
                ascmWmsIncManAccMain.supperTelephone = ascmWmsIncManAccMain_Model.supperTelephone;
                ascmWmsIncManAccMain.warehouseId = ascmWmsIncManAccMain_Model.warehouseId;
                ascmWmsIncManAccMain.wipEntityId = ascmWmsIncManAccMain_Model.wipEntityId;
                ascmWmsIncManAccMain.memo = ascmWmsIncManAccMain_Model.memo;
                #endregion

                #region 明细
                int maxId_Detail = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsIncManAccDetail");
                List<AscmWmsIncManAccDetail> listAscmWmsIncManAccDetailAdd = new List<AscmWmsIncManAccDetail>();
                List<AscmWmsIncManAccDetail> listAscmWmsIncManAccDetailUpdate = new List<AscmWmsIncManAccDetail>();
                List<AscmWmsIncManAccDetail> listAscmWmsIncManAccDetailDelete = new List<AscmWmsIncManAccDetail>();
                if (listAscmWmsIncManAccDetail_Model != null)
                {
                    int iRec = 0;
                    foreach (AscmWmsIncManAccDetail ascmWmsIncManAccDetail_Model in listAscmWmsIncManAccDetail_Model)
                    {
                        AscmWmsIncManAccDetail wmsIncManAccDetail = null;
                        if (ascmWmsIncManAccDetail_Model.incManAccDetailId == 0)
                        {
                            wmsIncManAccDetail = new AscmWmsIncManAccDetail();
                            wmsIncManAccDetail.incManAccDetailId = ++maxId_Detail;
                            wmsIncManAccDetail.organizationId = 775;
                            wmsIncManAccDetail.createUser = userName;
                            wmsIncManAccDetail.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            wmsIncManAccDetail.incManAccMainId = ascmWmsIncManAccMain.id;
                            listAscmWmsIncManAccDetailAdd.Add(wmsIncManAccDetail);
                        }
                        else if (listAscmWmsIncManAccDetail != null)
                        {
                            wmsIncManAccDetail = listAscmWmsIncManAccDetail.Find(P => P.incManAccDetailId == ascmWmsIncManAccDetail_Model.incManAccDetailId);
                            if (wmsIncManAccDetail == null)
                                throw new Exception("收货明细异常！");
                            listAscmWmsIncManAccDetailUpdate.Add(wmsIncManAccDetail);
                        }
                        wmsIncManAccDetail.modifyUser = userName;
                        wmsIncManAccDetail.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        wmsIncManAccDetail.materialId = ascmWmsIncManAccDetail_Model.materialId;
                        wmsIncManAccDetail.requestDeliveryDate = ascmWmsIncManAccDetail_Model.requestDeliveryDate;
                        wmsIncManAccDetail.deliveryQuantity = ascmWmsIncManAccDetail_Model.deliveryQuantity;
                        wmsIncManAccDetail.receivedQuantity = ascmWmsIncManAccDetail_Model.receivedQuantity;
                        wmsIncManAccDetail.warelocationId = ascmWmsIncManAccDetail_Model.warelocationId;
                        iRec++;
                    }
                    if (listAscmWmsIncManAccDetail != null)
                    {
                        foreach (AscmWmsIncManAccDetail wmsIncManAccDetail in listAscmWmsIncManAccDetail)
                        {
                            AscmWmsIncManAccDetail ascmWmsIncManAccDetail_Model = listAscmWmsIncManAccDetail_Model.Find(P => P.incManAccDetailId == wmsIncManAccDetail.incManAccDetailId);
                            if (ascmWmsIncManAccDetail_Model == null)
                                listAscmWmsIncManAccDetailDelete.Add(wmsIncManAccDetail);
                        }
                    }
                }
                #endregion

                #region 货位物料
                List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList(null, "", "", "", "");
                List<AscmLocationMaterialLink> listLocationMaterialLinkUpdate = new List<AscmLocationMaterialLink>();
                List<AscmLocationMaterialLink> listLocationMaterialLinkAdd = new List<AscmLocationMaterialLink>();
                if (listAscmWmsIncManAccDetail_Model != null && listAscmWmsIncManAccDetail_Model.Count() > 0)
                {
                    //int maxId_Link = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmLocationMaterialLink");
                    foreach (AscmWmsIncManAccDetail wmsIncManAccDetail in listAscmWmsIncManAccDetail_Model)
                    {
                        AscmLocationMaterialLink locationMaterialLink = listLocationMaterialLink.Find(P => P.pk.warelocationId == wmsIncManAccDetail.warelocationId && P.pk.materialId == wmsIncManAccDetail.materialId);
                        if (locationMaterialLink == null)//目标仓库不存在此物料；
                        {
                            locationMaterialLink = new AscmLocationMaterialLink();
                            locationMaterialLink.pk = new AscmLocationMaterialLinkPK { warelocationId = wmsIncManAccDetail.warelocationId, materialId = wmsIncManAccDetail.materialId };
                            //locationMaterialLink.id = ++maxId_Link;
                            locationMaterialLink.organizationId = 775;
                            locationMaterialLink.createUser = userName;
                            locationMaterialLink.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            locationMaterialLink.quantity = wmsIncManAccDetail.receivedQuantity;
                            //locationMaterialLink.warelocationId = wmsIncManAccDetail.warelocationId;
                            //locationMaterialLink.materialId = wmsIncManAccDetail.materialId;
                            listLocationMaterialLinkAdd.Add(locationMaterialLink);
                        }
                        else
                        {
                            locationMaterialLink.quantity = locationMaterialLink.quantity + wmsIncManAccDetail.receivedQuantity;
                            listLocationMaterialLinkUpdate.Add(locationMaterialLink);
                        }
                        locationMaterialLink.modifyUser = userName;
                        locationMaterialLink.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                #endregion

                #region 保存
                AscmMesInteractiveLog ascmMesInteractiveLog = new AscmMesInteractiveLog();
                ascmMesInteractiveLog.billId = ascmWmsIncManAccMain.id;
                ascmMesInteractiveLog.docNumber = ascmWmsIncManAccMain.docNumber;
                ascmMesInteractiveLog.billType = AscmMesInteractiveLog.BillTypeDefine.incAccManual;
                ascmMesInteractiveLog.createUser = userName;
                ascmMesInteractiveLog.modifyUser = userName;
                ascmMesInteractiveLog.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmMesInteractiveLog.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                //执行事务
                YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().Clear();
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        AscmMesService.GetInstance().DoManualReceive(ascmWmsIncManAccMain, listAscmWmsIncManAccDetail_Model, userName, ascmMesInteractiveLog);
                        if (ascmMesInteractiveLog.returnCode == "0")
                        {
                            if (id.HasValue)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmWmsIncManAccMain);
                            }
                            else
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsIncManAccMain);
                            }
                            if (listAscmWmsIncManAccDetailAdd != null && listAscmWmsIncManAccDetailAdd.Count > 0)
                            {
								//明细记录的返回状态
								//foreach (var detail in listAscmWmsIncManAccDetailAdd)
								//{
								//    detail.returnCode = ascmMesInteractiveLog.returnCode;
								//    detail.returnMessage = ascmMesInteractiveLog.returnMessage;
								//}

                                YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsIncManAccDetailAdd);
                            }
                            if (listAscmWmsIncManAccDetailUpdate != null && listAscmWmsIncManAccDetailUpdate.Count > 0)
                            {
								//明细记录的返回状态
								//foreach (var detail in listAscmWmsIncManAccDetailUpdate)
								//{
								//    detail.returnCode = ascmMesInteractiveLog.returnCode;
								//    detail.returnMessage = ascmMesInteractiveLog.returnMessage;
								//}

                                YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmWmsIncManAccDetailUpdate);
                            }
                            if (listAscmWmsIncManAccDetailDelete != null && listAscmWmsIncManAccDetailDelete.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsIncManAccDetailDelete);
                            }
                            if (listLocationMaterialLinkUpdate != null && listLocationMaterialLinkUpdate.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listLocationMaterialLinkUpdate);
                            }
                            if (listLocationMaterialLinkAdd != null && listLocationMaterialLinkAdd.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listLocationMaterialLinkAdd);
                            }
                            ascmMesInteractiveLog.returnMessage = "";
                        }

                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        ascmMesInteractiveLog.returnMessage = ex.Message;
                    }
                }
                #endregion

                //int maxLogId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmMesInteractiveLog");
                string maxLogIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_mes_interactive_log_id", "", "", 10);
                int maxLogId = Convert.ToInt32(maxLogIdKey);
                ascmMesInteractiveLog.id = maxLogId;
                AscmMesInteractiveLogService.GetInstance().Save(ascmMesInteractiveLog);
                string errorMsg = string.Empty;
                if (!string.IsNullOrEmpty(ascmMesInteractiveLog.returnMessage))
                {
                    errorMsg += string.Format("<li>手工单<font color='red'>{0}</font>：{1}</li>", ascmMesInteractiveLog.docNumber, ascmMesInteractiveLog.returnMessage);
                }
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    errorMsg = string.Format("<ul>{0}</ul>", errorMsg);
                    jsonObjectResult.message = errorMsg;
                }
                else
                {
                    jsonObjectResult.result = true;
                    jsonObjectResult.id = ascmWmsIncManAccMain.id.ToString();
                    jsonObjectResult.entity = ascmWmsIncManAccMain;
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult MaterialAdd(string materialDoc, string warehouseId)
        {
            AscmMaterialItem materialItem = null;
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                List<AscmLocationMaterialLink> list_AscmLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList("from AscmLocationMaterialLink", true, true);
                if (!string.IsNullOrEmpty(materialDoc))
                {
                    string whereOther = " docNumber='" + materialDoc + "'";
                    List<AscmMaterialItem> listAscmMaterialItem = AscmMaterialItemService.GetInstance().GetList(null, "", "", "", whereOther);
                    if (listAscmMaterialItem != null && listAscmMaterialItem.Count() > 0)
                        materialItem = listAscmMaterialItem.First();
                    if (!string.IsNullOrEmpty(warehouseId) && materialItem != null)
                    {
                        var find_LocationMaterialLink = list_AscmLocationMaterialLink.Find(item => item.pk.materialId == materialItem.id && item.warehouseId == warehouseId);
                        if (find_LocationMaterialLink != null)
                        {
                            materialItem.warelocationId = find_LocationMaterialLink.pk.warelocationId;
                            materialItem.warelocationDoc = find_LocationMaterialLink.locationDocNumber;
                        }
                        jsonObjectResult.entity = materialItem;
                        jsonObjectResult.result = true;
                    }
                    else
                    {
                        jsonObjectResult.result = false;
                        jsonObjectResult.message = "未匹配到相应物料！";
                    }
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        /*
         *  2014/5/13
         *  新增“导入物料”、“导出物料”、“导出物料模板”
         */
        public ActionResult ManAccExportMaterial(string detailJson)
        {
            IWorkbook wb = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Sheet1");
            sheet.SetColumnWidth(0, 12 * 256);
            sheet.SetColumnWidth(1, 16 * 256);
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 18 * 256);
            sheet.SetColumnWidth(4, 10 * 256);
            sheet.SetColumnWidth(5, 10 * 256);

            IRow titleRow = sheet.CreateRow(0);
            titleRow.Height = 20 * 20;
            titleRow.CreateCell(0).SetCellValue("序号");
            titleRow.CreateCell(1).SetCellValue("物料编码");
            titleRow.CreateCell(2).SetCellValue("物料描述");
            titleRow.CreateCell(3).SetCellValue("货位");
            titleRow.CreateCell(4).SetCellValue("单位");
            titleRow.CreateCell(5).SetCellValue("实收数量");

            string fileDownloadName = HttpUtility.UrlEncode("中央空调顺德工厂手工单接收物料明细.xls", Encoding.UTF8);
            if (!string.IsNullOrWhiteSpace(detailJson))
            {
                List<AscmWmsIncManAccDetail> listWmsIncManAccDetail = JsonConvert.DeserializeObject<List<AscmWmsIncManAccDetail>>(detailJson);
                if (listWmsIncManAccDetail != null && listWmsIncManAccDetail.Count > 0)
                {
                    int rowIndex = 0;
                    foreach (AscmWmsIncManAccDetail wmsIncManAccDetail in listWmsIncManAccDetail)
                    {
                        rowIndex++;
                        IRow row = sheet.CreateRow(rowIndex);
                        row.Height = 20 * 20;
                        row.CreateCell(0).SetCellValue(rowIndex);
                        row.CreateCell(1).SetCellValue(wmsIncManAccDetail.materialDocNumber);
                        row.CreateCell(2).SetCellValue(wmsIncManAccDetail.materialName);
                        row.CreateCell(3).SetCellValue(wmsIncManAccDetail.warelocationdocNumber);
                        row.CreateCell(4).SetCellValue(wmsIncManAccDetail.materialUnit);
                        row.CreateCell(5).SetCellValue(wmsIncManAccDetail.receivedQuantity);
                    }
                }
            }

            byte[] buffer = new byte[] { };
            using (System.IO.MemoryStream stream = new MemoryStream())
            {
                wb.Write(stream);
                buffer = stream.GetBuffer();
            }
            return File(buffer, "application/vnd.ms-excel", fileDownloadName);
        }

        public ActionResult ManAccImportMaterial(HttpPostedFileBase fileImport)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            jsonDataGridResult.result = true;
            try
            {
                if (fileImport != null)
                {
                    string userName = string.Empty;
                    if (User.Identity.IsAuthenticated)
                        userName = User.Identity.Name;

                    List<AscmWmsIncManAccDetail> listAscmWmsIncManAccDetail = null;
                    using (Stream stream = fileImport.InputStream)
                    {
                        listAscmWmsIncManAccDetail = new List<AscmWmsIncManAccDetail>();

                        IWorkbook wb = WorkbookFactory.Create(stream);
                        ISheet sheet = wb.GetSheet("Sheet1");
                        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                        while (rows.MoveNext())
                        {
                            IRow row = (IRow)rows.Current;
                            if (row.RowNum != 0)
                            {
                                ICell indexCell = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                ICell materialDocNumberCell = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                ICell materialNameCell = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                ICell warelocationdocNumberCell = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                ICell materialUnitCell = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                ICell receivedQuantityCell = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK);

                                AscmWmsIncManAccDetail ascmWmsIncManAccDetail = new AscmWmsIncManAccDetail();
                                if (materialDocNumberCell != null)
                                {
                                    ascmWmsIncManAccDetail.materialDocNumber = materialDocNumberCell.ToString().Trim();
                                }
                                if (materialNameCell != null)
                                {
                                    ascmWmsIncManAccDetail.materialName = materialNameCell.ToString().Trim();
                                }
                                if (warelocationdocNumberCell != null)
                                {
                                    ascmWmsIncManAccDetail.warelocationdocNumber = warelocationdocNumberCell.ToString().Trim();
                                }
                                if (materialUnitCell != null)
                                {
                                    ascmWmsIncManAccDetail.materialUnit = materialUnitCell.ToString().Trim(); ;
                                }
                                if (receivedQuantityCell != null)
                                {
                                    int quantity = 0;
                                    int.TryParse(receivedQuantityCell.ToString().Trim(), out quantity);
                                    ascmWmsIncManAccDetail.receivedQuantity = quantity;
                                }

                                listAscmWmsIncManAccDetail.Add(ascmWmsIncManAccDetail);
                            }
                        }

                        StringBuilder material_DocList = new StringBuilder();
                        StringBuilder warelocation_DocList = new StringBuilder();
                        foreach (var detail in listAscmWmsIncManAccDetail)
                        {
                            material_DocList.Append("'" + detail.materialDocNumber + "',");
                            warelocation_DocList.Append("'" + detail.warelocationdocNumber + "',");
                        }
                        if (material_DocList.Length > 0) material_DocList.Remove(material_DocList.Length - 1, 1);
                        if (warelocation_DocList.Length > 0) warelocation_DocList.Remove(warelocation_DocList.Length - 1, 1);

                        string material_Where = material_DocList.Length > 0 ? " docNumber in (" + material_DocList + ") " : "";
                        string warelocation_Where = warelocation_DocList.Length > 0 ? " docNumber in (" + warelocation_DocList + ") " : "";
                        List<AscmMaterialItem> listAscmMaterialItem = AscmMaterialItemService.GetInstance().GetList(null, "", "", "", material_Where);
                        List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList(null, "", "", "", warelocation_Where);
                        if (listAscmMaterialItem == null) listAscmMaterialItem = new List<AscmMaterialItem>();
                        if (listAscmWarelocation == null) listAscmWarelocation = new List<AscmWarelocation>();

                        foreach (AscmWmsIncManAccDetail wmsIncManAccDetail in listAscmWmsIncManAccDetail)
                        {
                            AscmMaterialItem materialItem = listAscmMaterialItem.FirstOrDefault(p => p.docNumber == wmsIncManAccDetail.materialDocNumber);
                            AscmWarelocation warelocation = listAscmWarelocation.FirstOrDefault(p => p.docNumber == wmsIncManAccDetail.warelocationdocNumber);
                            if (materialItem != null) 
                            {
                                wmsIncManAccDetail.materialId = materialItem.id;
                            }
                            if (warelocation != null)
                            {
                                wmsIncManAccDetail.warelocationId = warelocation.id;
                                wmsIncManAccDetail.warelocationdocNumber = warelocation.docNumber;
                            }

                            jsonDataGridResult.rows.Add(wmsIncManAccDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message += ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonDataGridResult);
            return Content(sReturn);
        }

        public ActionResult ManAccExportMaterialTemplate()
        {
            IWorkbook wb = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Sheet1");
            sheet.SetColumnWidth(0, 12 * 256);
            sheet.SetColumnWidth(1, 16 * 256);
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 18 * 256);
            sheet.SetColumnWidth(4, 10 * 256);
            sheet.SetColumnWidth(5, 10 * 256);

            IRow titleRow = sheet.CreateRow(0);
            titleRow.Height = 20 * 20;
            titleRow.CreateCell(0).SetCellValue("序号");
            titleRow.CreateCell(1).SetCellValue("物料编码");
            titleRow.CreateCell(2).SetCellValue("物料描述");
            titleRow.CreateCell(3).SetCellValue("货位");
            titleRow.CreateCell(4).SetCellValue("单位");
            titleRow.CreateCell(5).SetCellValue("实收数量");

            string fileDownloadName = HttpUtility.UrlEncode("手工单接收物料明细_模板.xls", Encoding.UTF8);
            byte[] buffer = new byte[] { };
            using (System.IO.MemoryStream stream = new MemoryStream())
            {
                wb.Write(stream);
                buffer = stream.GetBuffer();
            }
            return File(buffer, "application/vnd.ms-excel", fileDownloadName);
        }
        #endregion

        #region 手工单查询
        public ActionResult WmsIncManAccMainQuery()
        {
            ViewData["dtStart"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewData["dtEnd"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }
		public ActionResult WmsIncManAccMainList(int? page, int? rows, string sort, string order, string queryWord, string supplierDoc, string startModifyTime, string endModifyTime, string returnCode)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereStartTime = "", whereEndTime = "";

                DateTime dtStartTime, dtEndTime;
                if (!string.IsNullOrEmpty(startModifyTime) && DateTime.TryParse(startModifyTime, out dtStartTime))
                    whereStartTime = "modifyTime>='" + dtStartTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endModifyTime) && DateTime.TryParse(endModifyTime, out dtEndTime))
                    whereEndTime = "modifyTime<'" + dtEndTime.ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndTime);
				if (!string.IsNullOrEmpty(returnCode))
				{
					if (returnCode == "0")
					{
						whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "returnCode='0'");
					}
					else
					{
						whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "returnCode is not null and returnCode!='0'");
					}
				}

                List<AscmWmsIncManAccMain> list = AscmWmsIncManAccMainService.GetInstance().GetList(ynPage, sort, order, queryWord, whereOther);
                if (!string.IsNullOrEmpty(supplierDoc))
                    list = list.Where(item => item.ascmSupplier.docNumber == supplierDoc).OrderBy(item => item.modifyTime).ToList();
                if (list != null)
                {
                    foreach (AscmWmsIncManAccMain ascmWmsIncManAccMain in list)
                    {
                        jsonDataGridResult.rows.Add(ascmWmsIncManAccMain);
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
        public ActionResult WmsIncManAccView(int? id)
        {
            AscmWmsIncManAccMain ascmWmsIncManAccMain = null;
            try
            {
                ascmWmsIncManAccMain = AscmWmsIncManAccMainService.GetInstance().Get(id.Value);
                AscmSupplier ascmSupplier = AscmSupplierService.GetInstance().Get(ascmWmsIncManAccMain.supplierId);
                if (ascmSupplier != null)
                    ascmWmsIncManAccMain.ascmSupplier = ascmSupplier;
                AscmSupplierAddress ascmSupplierAddress = AscmSupplierAddressService.GetInstance().Get(ascmWmsIncManAccMain.supplierAddressId);
                if (ascmSupplierAddress != null)
                    ascmWmsIncManAccMain.ascmSupplierAddress = ascmSupplierAddress;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ascmWmsIncManAccMain);
        }
        public ActionResult WmsIncManAccDetialList(int? id)
        {
            List<AscmWmsIncManAccDetail> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmWmsIncManAccDetailService.GetInstance().GetList(id.Value);
                foreach (AscmWmsIncManAccDetail wmsIncManAccDetail in list)
                {
                    jsonDataGridResult.rows.Add(wmsIncManAccDetail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 作业备料
        public ActionResult WipDiscreteJobsPrepare()
        {
            //物料备料形式
            List<SelectListItem> listMaterialPrepareType = new List<SelectListItem>();
            listMaterialPrepareType.Add(new SelectListItem { Text = "", Value = "" });
            List<string> listMtlPrepareTypes = new List<string>();
            listMtlPrepareTypes.AddRange(AscmWipScheduleGroups.TypeDefine.GetMtlPrepareTypes(AscmWipScheduleGroups.TypeDefine.GA));
            listMtlPrepareTypes.AddRange(AscmWipScheduleGroups.TypeDefine.GetMtlPrepareTypes(AscmWipScheduleGroups.TypeDefine.QC));
            foreach (string mtlPrepareType in listMtlPrepareTypes)
            {
                listMaterialPrepareType.Add(new SelectListItem { Text = AscmWipScheduleGroups.TypeDefine.GetMtlPrepareTypeText(mtlPrepareType), Value = mtlPrepareType });
            }
            ViewData["listMaterialPrepareType"] = listMaterialPrepareType;

            return View();
        }
        public ActionResult WipDiscreteJobsList(int? page, int? rows, string sort, string order,
            string startScheduledStartDate, string endScheduledStartDate,
            string startWipEntitiesName, string endWipEntitiesName, string wipSupplyType,
            string startSupplySubInventory, string endSupplySubInventory,
            string startMaterialDocNumber, string endMaterialDocNumber,
            string startScheduleGroupName, string endScheduleGroupName, string jobStartNO, string wipEntityType)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            string whereOther = "", whereBom = "";
            //作业状态
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "statusType in(" + AscmWipDiscreteJobs.StatusTypeDefine.yff + "," + AscmWipDiscreteJobs.StatusTypeDefine.wc + "," + AscmWipDiscreteJobs.StatusTypeDefine.wcbjf + ")");
            //作业日期
            string whereStartScheduledStartDate = "", whereEndScheduledStartDate = "";
            DateTime dtStartScheduledStartDate, dtEndScheduledStartDate;
            if (!string.IsNullOrEmpty(startScheduledStartDate) && DateTime.TryParse(startScheduledStartDate, out dtStartScheduledStartDate))
            {
                whereStartScheduledStartDate = "scheduledStartDate>='" + dtStartScheduledStartDate.ToString("yyyy-MM-dd 00:00:00") + "'";
                whereEndScheduledStartDate = "scheduledStartDate<'" + dtStartScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            }
            if (!string.IsNullOrEmpty(endScheduledStartDate) && DateTime.TryParse(endScheduledStartDate, out dtEndScheduledStartDate))
                whereEndScheduledStartDate = "scheduledStartDate<'" + dtEndScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartScheduledStartDate);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndScheduledStartDate);

            //计划组、车间
            #region 旧的逻辑：开始计划组和结束计划组
            //string whereScheduleGroupOther = "";
            //string whereStartScheduleGroup = "", whereEndScheduleGroup = "";
            //if (!string.IsNullOrEmpty(startScheduleGroupName))
            //    whereStartScheduleGroup = "scheduleGroupName='" + startScheduleGroupName + "'";
            //if (!string.IsNullOrEmpty(endScheduleGroupName))
            //{
            //    if (!string.IsNullOrEmpty(startScheduleGroupName))
            //        whereStartScheduleGroup = "translate(schedulegroupname, '一二三四五六七八九', '123456789')>=translate('" + startScheduleGroupName + "', '一二三四五六七八九', '123456789')";
            //    whereEndScheduleGroup = "translate(schedulegroupname, '一二三四五六七八九', '123456789')<=translate('" + endScheduleGroupName + "', '一二三四五六七八九', '123456789')";
            //}
            //whereScheduleGroupOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduleGroupOther, whereStartScheduleGroup);
            //whereScheduleGroupOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduleGroupOther, whereEndScheduleGroup);
            //List<AscmWipScheduleGroups> listWipScheduleGroups = AscmWipScheduleGroupsService.GetInstance().GetList(null, "", "", "", whereScheduleGroupOther);
            //if (listWipScheduleGroups != null && listWipScheduleGroups.Count > 0)
            //    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "scheduleGroupId in(" + string.Join(",", listWipScheduleGroups.Select(P => P.scheduleGroupId)) + ")");
            #endregion
            string whereScheduleGroupOther = "";
            if (!string.IsNullOrEmpty(startScheduleGroupName))
                whereScheduleGroupOther = " scheduleGroupName in (" + startScheduleGroupName + ") ";
            List<AscmWipScheduleGroups> listWipScheduleGroups = AscmWipScheduleGroupsService.GetInstance().GetList(null, "", "", "", whereScheduleGroupOther);
            if (listWipScheduleGroups != null && listWipScheduleGroups.Count > 0)
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "scheduleGroupId in(" + string.Join(",", listWipScheduleGroups.Select(P => P.scheduleGroupId)) + ")");

            //作业号
            string whereWipEntity = "";
            string whereStartWipEntityName = "", whereEndWipEntityName = "";
            if (!string.IsNullOrEmpty(startWipEntitiesName))
                whereStartWipEntityName = "name='" + startWipEntitiesName + "'";
            if (!string.IsNullOrEmpty(endWipEntitiesName))
            {
                if (!string.IsNullOrEmpty(startWipEntitiesName))
                    whereStartWipEntityName = "name>='" + startWipEntitiesName + "'";
                whereEndWipEntityName = "name<='" + endWipEntitiesName + "'";
            }
            whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, whereStartWipEntityName);
            whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, whereEndWipEntityName);
            if (!string.IsNullOrEmpty(whereWipEntity))
            {
                //内、外机作业分开查询
                //AscmWipEntities.WipEntityType bWipEntityType = AscmWipEntitiesService.GetInstance().GetWipEntityType(startWipEntitiesName);
                //AscmWipEntities.WipEntityType eWipEntityType = AscmWipEntitiesService.GetInstance().GetWipEntityType(endWipEntitiesName);
                //if (bWipEntityType == AscmWipEntities.WipEntityType.withinTheMachine && bWipEntityType == eWipEntityType)
                //    whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, "instr(upper(substr(name,1,(length(name)-3))),'N',-1) > instr(upper(substr(name,1,(length(name)-3))),'W',-1)");
                //else if (eWipEntityType == AscmWipEntities.WipEntityType.outsideTheMachine && bWipEntityType == eWipEntityType)
                //    whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, "instr(upper(substr(name,1,(length(name)-3))),'W',-1) > instr(upper(substr(name,1,(length(name)-3))),'N',-1)");

                //AscmWipEntities.WipEntityType wipEntityType = entityType;
                if (wipEntityType == AscmWipEntities.WipEntityType.withinTheMachine.ToString())
                    whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, "instr(upper(substr(name,1,(length(name)-3))),'N',-1) > instr(upper(substr(name,1,(length(name)-3))),'W',-1)");
                else if (wipEntityType == AscmWipEntities.WipEntityType.outsideTheMachine.ToString())
                    whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, "instr(upper(substr(name,1,(length(name)-3))),'W',-1) > instr(upper(substr(name,1,(length(name)-3))),'N',-1)");

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "wipEntityId in (select wipEntityId from AscmWipEntities where " + whereWipEntity + ")");
            }

            //作业号开头
            if (!string.IsNullOrEmpty(jobStartNO))
            {
                if (jobStartNO != "ALL")
                {
                    if (jobStartNO == "OTHER")
                    {
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther,
                            " wipEntityId in ( select wipEntityId from AscmWipEntities where substr(name, 0, 1) not in ('M','X','V','U','E','B','G','J','P','R','T','F') ) ");
                    }
                    else
                    {
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther,
                            " wipEntityId in ( select wipEntityId from AscmWipEntities where substr(name, 0, 1) in (" + jobStartNO + ") )");
                    }
                }
            }

            //供应类型
            string whereWipSupplyType = "";
            if (wipSupplyType == "pullType") //拉式
                whereWipSupplyType = "wipSupplyType in(" + AscmMaterialItem.WipSupplyTypeDefine.zpls + "," + AscmMaterialItem.WipSupplyTypeDefine.lsgx + ")";
            else if (wipSupplyType == "pushType") //推式
                whereWipSupplyType = "wipSupplyType in(" + AscmMaterialItem.WipSupplyTypeDefine.ts + ")";
            whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, whereWipSupplyType);
            //供应子库
            string whereStartSupplySubInventory = "", whereEndSupplySubInventory = "";
            if (!string.IsNullOrEmpty(startSupplySubInventory))
                whereStartSupplySubInventory = "supplySubinventory='" + startSupplySubInventory + "'";
            if (!string.IsNullOrEmpty(endSupplySubInventory))
            {
                if (!string.IsNullOrEmpty(startSupplySubInventory))
                    whereStartSupplySubInventory = "supplySubinventory>='" + startSupplySubInventory + "'";
                whereEndSupplySubInventory = "supplySubinventory<='" + endSupplySubInventory + "'";
            }
            whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, whereStartSupplySubInventory);
            whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, whereEndSupplySubInventory);
            //物料编码
            string whereInventoryItem = "";
            string whereStartInventoryItemId = "", whereEndInventoryItemId = "";
            if (startMaterialDocNumber != null && startMaterialDocNumber.Trim() != "")
                whereStartInventoryItemId = "upper(docNumber)='" + startMaterialDocNumber.Trim().ToUpper() + "'";
            if (endMaterialDocNumber != null && endMaterialDocNumber.Trim() != "")
            {
                if (startMaterialDocNumber != null && startMaterialDocNumber.Trim() != "")
                    whereStartInventoryItemId = "upper(docNumber)>='" + startMaterialDocNumber.Trim().ToUpper() + "'";
                whereEndInventoryItemId = "upper(docNumber)<='" + endMaterialDocNumber.Trim().ToUpper() + "'";
            }
            whereInventoryItem = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereInventoryItem, whereStartInventoryItemId);
            whereInventoryItem = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereInventoryItem, whereEndInventoryItemId);
            if (!string.IsNullOrEmpty(whereInventoryItem))
                whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, "inventoryItemId in(select id from AscmMaterialItem where " + whereInventoryItem + ")");
            if (!string.IsNullOrEmpty(whereBom))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "wipEntityId in (select wipEntityId from AscmWipRequirementOperations where " + whereBom + ")");

            List<AscmWipDiscreteJobs> listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList(ynPage, sort, order, "", whereOther, true, false, true, false);
            if (listWipDiscreteJobs != null)
            {
                //获取排产计划产线
                AscmWipDiscreteJobsService.GetInstance().SetScheduleProductionLine(listWipDiscreteJobs);
                var result = listWipDiscreteJobs.OrderBy(P => P.ascmWipEntities_Name);
                foreach (AscmWipDiscreteJobs wipDiscreteJobs in result)
                {
                    if (listWipScheduleGroups != null)
                        wipDiscreteJobs.ascmWipScheduleGroups = listWipScheduleGroups.Find(P => P.scheduleGroupId == wipDiscreteJobs.scheduleGroupId);
                    jsonDataGridResult.rows.Add(wipDiscreteJobs);
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetWmsPreparationDetailList(int? page, int? rows, string sort, string order,
            string wipEntityIds, string wipSupplyType, string startSupplySubInventory,
            string endSupplySubInventory, string startMaterialDocNumber, string endMaterialDocNumber, string materialPrepareType)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmWipRequirementOperations> listBom = GetWipRequireOperationList(null, wipEntityIds, wipSupplyType, startSupplySubInventory, endSupplySubInventory, startMaterialDocNumber, endMaterialDocNumber);
            if (listBom != null)
            {
                //计划组
                List<string> listScheduleGroupNames = null;
                KeyValuePair<string, string> kvp = AscmWipScheduleGroups.TypeDefine.GetMtlPrepareType(materialPrepareType);
                if (!string.IsNullOrEmpty(kvp.Key))
                    listScheduleGroupNames = AscmWipScheduleGroups.TypeDefine.GetWipScheduleGroupNames(kvp.Key);
                //货位物料
                List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetListByWarehouseIds("'" + string.Join("','", listBom.Select(P => P.supplySubinventory).Distinct()) + "'", false, true);
                //按作业号、物料编码排序
                int id = 0;
                List<AscmWmsPreparationDetail> listPrepareDetail = new List<AscmWmsPreparationDetail>();
                var result = listBom.OrderBy(P => P.ascmMaterialItem_DocNumber).OrderBy(P => P.wipEntitiesName);
                foreach (AscmWipRequirementOperations bom in result)
                {
                    if (bom.requiredQuantity == decimal.Zero)  //需求数量为0的排除
                        continue;
                    bool isAdd = true;
                    if (listScheduleGroupNames != null)
                        isAdd = listScheduleGroupNames.Contains(bom.ascmWipDiscreteJobsAscmWipScheduleGroupsName);
                    if (kvp.Key == AscmWipScheduleGroups.TypeDefine.GA)
                        isAdd = bom.ascmMaterialItem_zMtlCategoryStatus == kvp.Value;
                    else if (kvp.Key == AscmWipScheduleGroups.TypeDefine.QC)
                        isAdd = bom.ascmMaterialItem_dMtlCategoryStatus == kvp.Value;
                    if (!isAdd)
                        continue;
                    AscmWmsPreparationDetail preparationDetail = new AscmWmsPreparationDetail();
                    preparationDetail.id = --id;
                    preparationDetail.materialId = bom.inventoryItemId;
                    preparationDetail.warehouseId = bom.supplySubinventory;
                    preparationDetail.wipSupplyType = bom.wipSupplyTypeCn;
                    preparationDetail.planQuantity = bom.requiredQuantity;
                    preparationDetail.materialDocNumber = bom.ascmMaterialItem_DocNumber;
                    preparationDetail.materialName = bom.ascmMaterialItem_Description;
                    preparationDetail.materialUnit = bom.ascmMaterialItem_unit;
                    preparationDetail.wipEntityId = bom.wipEntityId;
                    if (listLocationMaterialLink != null)
                    {
                        AscmLocationMaterialLink locationMaterialLink = listLocationMaterialLink.Find(P => P.pk.materialId == bom.inventoryItemId && P.warehouseId == preparationDetail.warehouseId);
                        if (locationMaterialLink != null)
                        {
                            preparationDetail.warelocationId = locationMaterialLink.pk.warelocationId;
                            preparationDetail.locationDocNumber = locationMaterialLink.locationDocNumber;
                        }
                    }
                    preparationDetail.wipEntityName = bom.wipEntitiesName;
                    preparationDetail.select = false;
                    //jsonDataGridResult.rows.Add(preparationDetail);
                    listPrepareDetail.Add(preparationDetail);
                }
                //设置库存现有量
                AscmWmsPreparationDetailService.GetInstance().SetOnhandQuantity(listPrepareDetail);
                listPrepareDetail.ForEach(P => jsonDataGridResult.rows.Add(P));
            }
            return Content(JsonConvert.SerializeObject(jsonDataGridResult));
        }
        public List<AscmWipRequirementOperations> GetWipRequireOperationList(YnBaseDal.YnPage ynPage,
            string wipEntityIds, string wipSupplyType, string startSupplySubInventory,
            string endSupplySubInventory, string startMaterialDocNumber, string endMaterialDocNumber)
        {
            string whereOther = "", whereMaterialOther = "";
            //作业
            string whereWipEntityId = "";
            if (!string.IsNullOrEmpty(wipEntityIds))
                whereWipEntityId = "wipEntityId in (" + wipEntityIds + ")";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWipEntityId);
            //供应类型
            string whereWipSupplyType = "";
            if (wipSupplyType == "pullType") //拉式
                whereWipSupplyType = "wipSupplyType in(" + AscmMaterialItem.WipSupplyTypeDefine.zpls + "," + AscmMaterialItem.WipSupplyTypeDefine.lsgx + ")";
            else if (wipSupplyType == "pushType") //推式
                whereWipSupplyType = "wipSupplyType in(" + AscmMaterialItem.WipSupplyTypeDefine.ts + ")";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWipSupplyType);
            //供应子库
            string whereStartSupplySubInventory = "", whereEndSupplySubInventory = "";
            if (!string.IsNullOrEmpty(startSupplySubInventory))
                whereStartSupplySubInventory = "supplySubinventory='" + startSupplySubInventory + "'";
            if (!string.IsNullOrEmpty(endSupplySubInventory))
            {
                if (!string.IsNullOrEmpty(startSupplySubInventory))
                    whereStartSupplySubInventory = "supplySubinventory>='" + startSupplySubInventory + "'";
                whereEndSupplySubInventory = "supplySubinventory<='" + endSupplySubInventory + "'";
            }
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartSupplySubInventory);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndSupplySubInventory);
            //物料编码
            string whereStartMaterialDocNumber = "", whereEndMaterialDocNumber = "";
            if (startMaterialDocNumber != null && startMaterialDocNumber.Trim() != "")
                whereStartMaterialDocNumber = "docNumber='" + startMaterialDocNumber.Trim() + "'";
            if (endMaterialDocNumber != null && endMaterialDocNumber.Trim() != "")
            {
                if (startMaterialDocNumber != null && startMaterialDocNumber.Trim() != "")
                    whereStartMaterialDocNumber = "docNumber>='" + startMaterialDocNumber.Trim() + "'";
                whereEndMaterialDocNumber = "docNumber<='" + endMaterialDocNumber.Trim() + "'";
            }
            whereMaterialOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMaterialOther, whereStartMaterialDocNumber);
            whereMaterialOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMaterialOther, whereEndMaterialDocNumber);
            if (!string.IsNullOrEmpty(whereMaterialOther))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "inventoryItemId in(select id from AscmMaterialItem where " + whereMaterialOther + ")");
            return AscmWipRequirementOperationsService.GetInstance().GetList(ynPage, "", "", "", whereOther, "");
        }
        public ActionResult GetWmsPreparationDetailSumList(string preparationDetailRows)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            if (!string.IsNullOrEmpty(preparationDetailRows))
            {
                List<AscmWmsPreparationDetail> listPreparationDetail_Model = JsonConvert.DeserializeObject<List<AscmWmsPreparationDetail>>(preparationDetailRows);
                if (listPreparationDetail_Model != null && listPreparationDetail_Model.Count > 0)
                {
                    var result = listPreparationDetail_Model.OrderBy(P => P.materialDocNumber).GroupBy(P => P.materialId);
                    foreach (IGrouping<int, AscmWmsPreparationDetail> ig in result)
                    {
                        AscmWmsPreparationDetail preparationDetail_Model = ig.FirstOrDefault();
                        AscmWmsPreparationDetail preparationDetail = new AscmWmsPreparationDetail();
                        preparationDetail.materialId = ig.Key;
                        preparationDetail.materialDocNumber = preparationDetail_Model.materialDocNumber;
                        preparationDetail.materialName = preparationDetail_Model.materialName;
                        preparationDetail.materialUnit = preparationDetail_Model.materialUnit;
                        preparationDetail.warehouseId = preparationDetail_Model.warehouseId;
                        preparationDetail.wipSupplyType = preparationDetail_Model.wipSupplyType;
                        preparationDetail.planQuantity = ig.Sum(P => P.planQuantity);
                        preparationDetail.onhandQuantity = preparationDetail_Model.onhandQuantity;
                        jsonDataGridResult.rows.Add(preparationDetail);
                    }
                }
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateWmsPreparation(string pattern, string detailJson)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            if (!string.IsNullOrEmpty(detailJson))
            {
                List<AscmWmsPreparationDetail> listPreparationDetail_Model = JsonConvert.DeserializeObject<List<AscmWmsPreparationDetail>>(detailJson);
                if (listPreparationDetail_Model != null && listPreparationDetail_Model.Count > 0)
                {
                    string userName = string.Empty;
                    if (User.Identity.IsAuthenticated)
                        userName = User.Identity.Name;

                    List<AscmWmsPreparationMain> listPreparationMain = null;
                    List<AscmWipDiscreteJobs> listWipDiscreteJobs = null;
                    if (pattern == AscmWmsPreparationMain.PatternDefine.wipJob)
                    {
                        listPreparationMain = CreateWipJobPreparation(userName, listPreparationDetail_Model);
                        if (listPreparationMain != null && listPreparationMain.Count > 0)
                        {
                            //生成作业备料单，更改作业的备料状态
                            List<int> listWipEntityId = listPreparationMain.Select(P => P.wipEntityId).ToList();
                            listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList(listWipEntityId, "ascmStatus is null");
                            if (listWipDiscreteJobs != null)
                                listWipDiscreteJobs.ForEach(P => P.ascmStatus = AscmWipDiscreteJobs.AscmStatusDefine.unPrepare);
                        }
                    }
                    else if (pattern == AscmWmsPreparationMain.PatternDefine.wipRequire)
                    {
                        AscmWmsPreparationMain preparationMain = CreateWipRequirePreparation(userName, listPreparationDetail_Model);
                        if (preparationMain != null)
                        {
                            listPreparationMain = new List<AscmWmsPreparationMain>();
                            listPreparationMain.Add(preparationMain);
                        }
                    }
                    if (listPreparationMain != null && listPreparationMain.Count > 0)
                    {
                        List<AscmWmsPreparationDetail> listPreparationDetail = new List<AscmWmsPreparationDetail>();
                        listPreparationMain.ForEach(P => listPreparationDetail.AddRange(P.listDetail));
                        //执行事务
                        YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().Clear();
                        using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                        {
                            try
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listPreparationMain);
                                if (listPreparationDetail.Count > 0)
                                    YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listPreparationDetail);
                                if (listWipDiscreteJobs != null && listWipDiscreteJobs.Count > 0)
                                    YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listWipDiscreteJobs);

                                //增加明细状态
                                foreach (AscmWmsPreparationMain mainInfo in listPreparationMain)
                                {
                                    string teamLeaderId = AscmWhTeamUserService.Instance.GetLeaderId(mainInfo.createUser);
                                    if (teamLeaderId == null) continue;

                                    AscmWipDiscreteJobsStatus jobStatus = AscmWipDiscreteJobsService.Instance.Get(mainInfo.wipEntityId, teamLeaderId);
                                    if (jobStatus != null)
                                    {
                                        if (jobStatus.subStatus == AscmWipDiscreteJobs.AscmStatusDefine.unPick
                                            || jobStatus.subStatus == AscmWipDiscreteJobs.AscmStatusDefine.picked) 
                                        {
                                            jobStatus.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing;
                                            YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWipDiscreteJobsStatus>(jobStatus);
                                        }
                                    }
                                    else 
                                    {
                                        YnUser leaderUser = YnUserService.GetInstance().Get(teamLeaderId);
                                        jobStatus = new AscmWipDiscreteJobsStatus();
                                        jobStatus.id = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId(" select max(id) from AscmWipDiscreteJobsStatus ") + 1;
                                        jobStatus.leaderId = teamLeaderId;
                                        jobStatus.leaderName = leaderUser != null ? leaderUser.userName : teamLeaderId;
                                        jobStatus.organizationId = mainInfo.organizationId;
                                        jobStatus.wipEntityId = mainInfo.wipEntityId;
                                        jobStatus.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.unPrepare;

                                        //AscmWipDiscreteJobsService.Instance.Save(jobStatus);
                                        YnDaoHelper.GetInstance().nHibernateHelper.Save<AscmWipDiscreteJobsStatus>(jobStatus);
                                    }
                                }

                                tx.Commit();//正确执行提交

                                jsonObjectResult.result = true;
                                string message = "备料单";
                                if (listPreparationMain.Count > 0)
                                {
                                    var result = listPreparationMain.OrderBy(P => P.docNumber);
                                    message += "【<font color='red'>" + result.First().docNumber + "</font>";
                                    if (listPreparationMain.Count > 1)
                                        message += "至<font color='red'>" + result.Last().docNumber + "</font>";
                                    message += "】";
                                    //提供导出pdf
                                    jsonObjectResult.id = string.Join(",", listPreparationMain.Select(P => P.id));
                                }
                                message += "生成成功！";
                                jsonObjectResult.message = message;
                            }
                            catch (Exception ex)
                            {
                                tx.Rollback();//回滚
                                YnBaseClass2.Helper.LogHelper.GetLog().Error("备料单生成失败", ex);
                                jsonObjectResult.message = ex.Message;
                            }
                        }
                    }
                }
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public List<AscmWmsPreparationMain> CreateWipJobPreparation(string userName, List<AscmWmsPreparationDetail> listPreparationDetail_Model)
        {
            List<AscmWmsPreparationMain> listPreparationMain = new List<AscmWmsPreparationMain>();
            var result = listPreparationDetail_Model.GroupBy(P => P.wipEntityId);
            string maxMainIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_wms_preparation_main_id", "", "", 10, result.Count());
            string maxDetailIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_wms_preparation_detail_id", "", "", 10, listPreparationDetail_Model.Count);
            int maxMainId = Convert.ToInt32(maxMainIdKey);
            int maxDetailId = Convert.ToInt32(maxDetailIdKey);
            foreach (IGrouping<int, AscmWmsPreparationDetail> ig in result)
            {
                AscmWmsPreparationMain preparationMain = new AscmWmsPreparationMain();
                preparationMain.id = maxMainId++;
                preparationMain.createUser = userName;
                preparationMain.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                preparationMain.docNumber = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("AscmWmsPreparationMain", "ZB", "yyMMdd", 4);
                preparationMain.modifyUser = userName;
                preparationMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                preparationMain.status = AscmWmsPreparationMain.StatusDefine.unPrepare;
                preparationMain.pattern = AscmWmsPreparationMain.PatternDefine.wipJob;
                preparationMain.wipEntityId = ig.Key;
                preparationMain.listDetail = new List<AscmWmsPreparationDetail>();
                listPreparationMain.Add(preparationMain);
                foreach (AscmWmsPreparationDetail preparationDetail_Model in ig)
                {
                    AscmWmsPreparationDetail preparationDetail = new AscmWmsPreparationDetail();
                    preparationDetail.id = maxDetailId++;
                    preparationDetail.createUser = userName;
                    preparationDetail.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    preparationDetail.mainId = preparationMain.id;
                    preparationDetail.modifyUser = userName;
                    preparationDetail.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    preparationDetail.materialId = preparationDetail_Model.materialId;
                    preparationDetail.warehouseId = preparationDetail_Model.warehouseId;
                    preparationDetail.wipSupplyType = preparationDetail_Model.wipSupplyType;
                    preparationDetail.warelocationId = preparationDetail_Model.warelocationId;
                    preparationDetail.planQuantity = preparationDetail_Model.planQuantity;
                    preparationDetail.wipEntityId = preparationDetail_Model.wipEntityId;
                    preparationMain.listDetail.Add(preparationDetail);
                }
            }
            return listPreparationMain;
        }
        public AscmWmsPreparationMain CreateWipRequirePreparation(string userName, List<AscmWmsPreparationDetail> listPreparationDetail_Model)
        {
            AscmWmsPreparationMain preparationMain = new AscmWmsPreparationMain();
            string maxMainIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_wms_preparation_main_id", "", "", 10);
            int maxMainId = Convert.ToInt32(maxMainIdKey);
            preparationMain.id = maxMainId;
            preparationMain.createUser = userName;
            preparationMain.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            preparationMain.docNumber = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("AscmWmsPreparationMain", "XB", "yyMMdd", 4);
            preparationMain.modifyUser = userName;
            preparationMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            preparationMain.status = AscmWmsPreparationMain.StatusDefine.unPrepare;
            preparationMain.pattern = AscmWmsPreparationMain.PatternDefine.wipRequire;
            preparationMain.listDetail = new List<AscmWmsPreparationDetail>();
            string maxDetailIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_wms_preparation_detail_id", "", "", 10, listPreparationDetail_Model.Count);
            int maxDetailId = Convert.ToInt32(maxDetailIdKey);
            foreach (AscmWmsPreparationDetail preparationDetail_Model in listPreparationDetail_Model)
            {
                AscmWmsPreparationDetail preparationDetail = new AscmWmsPreparationDetail();
                preparationDetail.id = maxDetailId++;
                preparationDetail.createUser = userName;
                preparationDetail.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                preparationDetail.mainId = preparationMain.id;
                preparationDetail.modifyUser = userName;
                preparationDetail.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                preparationDetail.materialId = preparationDetail_Model.materialId;
                preparationDetail.warehouseId = preparationDetail_Model.warehouseId;
                preparationDetail.wipSupplyType = preparationDetail_Model.wipSupplyType;
                preparationDetail.warelocationId = preparationDetail_Model.warelocationId;
                preparationDetail.planQuantity = preparationDetail_Model.planQuantity;
                preparationDetail.wipEntityId = preparationDetail_Model.wipEntityId;
                preparationMain.listDetail.Add(preparationDetail);
            }
            return preparationMain;
        }
        #endregion

        #region 备料管理
        public ActionResult WmsPreparationIndex()
        {
            ////备料单状态
            //List<SelectListItem> listPrepareStatus = new List<SelectListItem>();
            //listPrepareStatus.Add(new SelectListItem { Text = "", Value = "" });
            //AscmWmsPreparationMain.StatusDefine.GetList().ForEach(P => listPrepareStatus.Add(new SelectListItem { Text = AscmWmsPreparationMain.StatusDefine.DisplayText(P), Value = P }));
            //ViewData["listPrepareStatus"] = listPrepareStatus;

            return View();
        }
        public ActionResult WmsPreparationList(int? page, int? rows, string sort, string order, string queryWord,
            string startCreateTime, string endCreateTime, string pattern, string createUserId, string status,
            string startWipEntityName, string endWipEntityName, string startDocNumber, string endDocNumber,
            string startScheduledStartDate, string endScheduledStartDate, string startMaterialDocNumber, string endMaterialDocNumber)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            string whereOther = "", whereDetail = "";
            //备料单生成日期
            string whereStartCreateTime = "", whereEndCreateTime = "";
            DateTime dtStartCreateTime, dtEndCreateTime;
            if (!string.IsNullOrEmpty(startCreateTime) && DateTime.TryParse(startCreateTime, out dtStartCreateTime))
            {
                whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                whereEndCreateTime = "createTime<'" + dtStartCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            }
            if (!string.IsNullOrEmpty(endCreateTime) && DateTime.TryParse(endCreateTime, out dtEndCreateTime))
                whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);
            //备料单备料方式
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "pattern='" + pattern + "'");
            //打单员
            if (!string.IsNullOrEmpty(createUserId))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "createUser='" + createUserId + "'");
            //状态(支持多选)
            if (!string.IsNullOrEmpty(status))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "status in('" + string.Join("','", status.Split(',')) + "')");
            //备料单号
            string whereStartDocNumber = "", whereEndDocNumber = "";
            if (!string.IsNullOrWhiteSpace(startDocNumber))
                whereStartDocNumber = "docNumber='" + startDocNumber.ToUpper() + "'";
            if (!string.IsNullOrWhiteSpace(endDocNumber))
            {
                if (!string.IsNullOrWhiteSpace(startDocNumber))
                    whereStartDocNumber = "docNumber>='" + startDocNumber.ToUpper() + "'";
                whereEndDocNumber = "docNumber<='" + endDocNumber.ToUpper() + "'";
            }
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartDocNumber);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndDocNumber);
            //物料编码
            string whereMaterialDocNumber = "";
            string whereStartMaterialDocNumber = "", whereEndMaterialDocNumber = "";
            if (!string.IsNullOrEmpty(startMaterialDocNumber))
                whereStartMaterialDocNumber = "upper(docNumber)='" + startMaterialDocNumber.ToUpper() + "'";
            if (!string.IsNullOrEmpty(endMaterialDocNumber))
            {
                if (!string.IsNullOrEmpty(startMaterialDocNumber))
                    whereStartMaterialDocNumber = "upper(docNumber)>='" + startMaterialDocNumber.ToUpper() + "'";
                whereEndMaterialDocNumber = "upper(docNumber)<='" + endMaterialDocNumber.ToUpper() + "'";
            }
            whereMaterialDocNumber = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMaterialDocNumber, whereStartMaterialDocNumber);
            whereMaterialDocNumber = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMaterialDocNumber, whereEndMaterialDocNumber);
            if (!string.IsNullOrEmpty(whereMaterialDocNumber))
                whereDetail = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereDetail, "materialId in(select id from AscmMaterialItem where " + whereMaterialDocNumber + ")");
            bool isWipJob = pattern == AscmWmsPreparationMain.PatternDefine.wipJob;
            //作业号
            string whereWipEntityName = "";
            string whereStartWipEntityName = "", whereEndWipEntityName = "";
            if (!string.IsNullOrEmpty(startWipEntityName))
                whereStartWipEntityName = "name='" + startWipEntityName + "'";
            if (!string.IsNullOrEmpty(endWipEntityName))
            {
                if (!string.IsNullOrEmpty(startWipEntityName))
                    whereStartWipEntityName = "name>='" + startWipEntityName + "'";
                whereEndWipEntityName = "name<='" + endWipEntityName + "'";
            }
            whereWipEntityName = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntityName, whereStartWipEntityName);
            whereWipEntityName = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntityName, whereEndWipEntityName);
            if (!string.IsNullOrEmpty(whereWipEntityName))
            {
                //内、外机作业分开查询
                AscmWipEntities.WipEntityType bWipEntityType = AscmWipEntitiesService.GetInstance().GetWipEntityType(whereStartWipEntityName);
                AscmWipEntities.WipEntityType eWipEntityType = AscmWipEntitiesService.GetInstance().GetWipEntityType(whereEndWipEntityName);
                if (bWipEntityType == AscmWipEntities.WipEntityType.withinTheMachine && bWipEntityType == eWipEntityType)
                    whereEndWipEntityName = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereEndWipEntityName, "instr(upper(substr(name,1,(length(name)-3))),'N',-1) > instr(upper(substr(name,1,(length(name)-3))),'W',-1)");
                else if (eWipEntityType == AscmWipEntities.WipEntityType.outsideTheMachine && bWipEntityType == eWipEntityType)
                    whereEndWipEntityName = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereEndWipEntityName, "instr(upper(substr(name,1,(length(name)-3))),'W',-1) > instr(upper(substr(name,1,(length(name)-3))),'N',-1)");

                string whereWipEntityId = "wipEntityId in(select wipEntityId from AscmWipEntities where " + whereWipEntityName + ")";
                if (isWipJob)
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWipEntityId);
                else
                    whereDetail = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereDetail, whereWipEntityId);
            }
            //作业日期
            string whereScheduledStartDate = "";
            string whereStartScheduledStartDate = "", whereEndScheduledStartDate = "";
            DateTime dtStartScheduledStartDate, dtEndScheduledStartDate;
            if (!string.IsNullOrEmpty(startScheduledStartDate) && DateTime.TryParse(startScheduledStartDate, out dtStartScheduledStartDate))
            {
                whereStartScheduledStartDate = "scheduledStartDate>='" + dtStartScheduledStartDate.ToString("yyyy-MM-dd 00:00:00") + "'";
                whereEndScheduledStartDate = "scheduledStartDate<'" + dtStartScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            }
            if (!string.IsNullOrEmpty(endScheduledStartDate) && DateTime.TryParse(endScheduledStartDate, out dtEndScheduledStartDate))
                whereEndScheduledStartDate = "scheduledStartDate<'" + dtEndScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            whereScheduledStartDate = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduledStartDate, whereStartScheduledStartDate);
            whereScheduledStartDate = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduledStartDate, whereEndScheduledStartDate);
            if (!string.IsNullOrEmpty(whereScheduledStartDate))
            {
                string whereWipEntityId = "wipEntityId in(select wipEntityId from AscmWipDiscreteJobs where " + whereScheduledStartDate + ")";
                if (isWipJob)
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWipEntityId);
                else
                    whereDetail = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereDetail, whereWipEntityId);
            }
            if (!string.IsNullOrEmpty(whereDetail))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "id in(select mainId from AscmWmsPreparationDetail where " + whereDetail + ")");

            //备料单批量执行"设置货位"
            List<AscmWmsPreparationMain> listAscmWmsPreparationMain = AscmWmsPreparationMainService.GetInstance().GetList(null, sort, order, queryWord, whereOther);
            WmsPreparationDetailBatchSetLocation(listAscmWmsPreparationMain);

            List<AscmWmsPreparationMain> list = AscmWmsPreparationMainService.GetInstance().GetList(ynPage, sort, order, queryWord, whereOther);
            if (list != null)
            {
                if (isWipJob)
                    AscmWmsPreparationMainService.GetInstance().SetWipDiscreteJobs(list);
                AscmWmsPreparationMainService.GetInstance().SetDetailInfo(list, !isWipJob, !isWipJob, !isWipJob, !isWipJob);
                list.ForEach(P => jsonDataGridResult.rows.Add(P));
            }
            jsonDataGridResult.total = ynPage.GetRecordCount();
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }

        //作业备料
        public ActionResult WmsWipJobPreparationEdit(int? id)
        {
            AscmWmsPreparationMain preparationMain = null;
            if (id.HasValue)
            {
                preparationMain = AscmWmsPreparationMainService.GetInstance().Get(id.Value);
                if (preparationMain != null)
                {
                    List<AscmWmsPreparationMain> listPreparationMain = new List<AscmWmsPreparationMain>();
                    listPreparationMain.Add(preparationMain);
                    AscmWmsPreparationMainService.GetInstance().SetWipDiscreteJobs(listPreparationMain);
                    AscmWmsPreparationMainService.GetInstance().SetDetailInfo(listPreparationMain);
                }
            }
            preparationMain = preparationMain ?? new AscmWmsPreparationMain();
            return View(preparationMain);
        }
        public ActionResult WmsWipJobPreparationDetailList(int? id, bool isLoadERP)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            if (id.HasValue)
            {
                List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetWipJobPreparationDetail(id.Value);
                if (listPreparationDetail != null)
                {
                    //设置库存现有量
                    AscmWmsPreparationDetailService.GetInstance().SetOnhandQuantity(listPreparationDetail);
                    //设置作业物料已领数量
                    AscmWmsPreparationDetailService.GetInstance().SetReceivedQuantity(listPreparationDetail.FirstOrDefault().wipEntityId, listPreparationDetail);

                    listPreparationDetail.ForEach(P => jsonDataGridResult.rows.Add(P));
                }
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        //需求备料
        public ActionResult WmsWipRequirePreparationEidt(int? id)
        {
            AscmWmsPreparationMain preparationMain = null;
            if (id.HasValue)
            {
                preparationMain = AscmWmsPreparationMainService.GetInstance().Get(id.Value);
                if (preparationMain != null)
                {
                    List<AscmWmsPreparationMain> listPreparationMain = new List<AscmWmsPreparationMain>();
                    listPreparationMain.Add(preparationMain);
                    AscmWmsPreparationMainService.GetInstance().SetDetailInfo(listPreparationMain, true, true, true, true);
                }
            }
            preparationMain = preparationMain ?? new AscmWmsPreparationMain();
            return View(preparationMain);
        }
        public ActionResult WmsWipRequirePreparationDetailSumList(int? id)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            if (id.HasValue)
            {
                List<AscmWmsPreparationDetail> listPreparationDetailSum = AscmWmsPreparationDetailService.GetInstance().GetSumList(id.Value);
                if (listPreparationDetailSum != null)
                    listPreparationDetailSum.ForEach(P => jsonDataGridResult.rows.Add(P));
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WmsWipRequirePreparationDetailList(int? id, int? materialId)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            if (id.HasValue)
            {
                List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetWipRequirePreparationDetail(id.Value, materialId);
                if (listPreparationDetail != null)
                    listPreparationDetail.ForEach(P => jsonDataGridResult.rows.Add(P));
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }

        //备料单明细执行"加入"操作
        public ActionResult WmsPreparationDetailAdd(int? id, string preparationDetailRows)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            if (id.HasValue && !string.IsNullOrEmpty(preparationDetailRows))
            {
                AscmWmsPreparationMain preparationMain = AscmWmsPreparationMainService.GetInstance().Get(id.Value);
                if (preparationMain != null)
                {
                    List<AscmWmsPreparationDetail> listPreparationDetail_Model = JsonConvert.DeserializeObject<List<AscmWmsPreparationDetail>>(preparationDetailRows);
                    if (listPreparationDetail_Model != null && listPreparationDetail_Model.Count > 0)
                    {
                        string userName = string.Empty;
                        if (User.Identity.IsAuthenticated)
                            userName = User.Identity.Name;

                        List<AscmWmsPreparationDetail> listPreparationDetailUpdate = null;
                        List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetList("from AscmWmsPreparationDetail where mainId=" + id.Value);
                        if (listPreparationDetail != null && listPreparationDetail.Count > 0)
                        {
                            listPreparationDetailUpdate = new List<AscmWmsPreparationDetail>();
                            foreach (AscmWmsPreparationDetail preparationDetail_Model in listPreparationDetail_Model)
                            {
                                if (preparationDetail_Model.prepareQuantity > 0)
                                {
                                    AscmWmsPreparationDetail preparationDetail = listPreparationDetail.Find(P => P.id == preparationDetail_Model.id);
                                    if (preparationDetail != null)
                                    {
                                        preparationDetail.modifyUser = userName;
                                        preparationDetail.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        preparationDetail.warehouseId = preparationDetail_Model.warehouseId;
                                        preparationDetail.warelocationId = preparationDetail_Model.warelocationId;
                                        preparationDetail.prepareQuantity = preparationDetail_Model.prepareQuantity; //本次备料数量
                                        preparationDetail.wipEntityName = preparationDetail_Model.wipEntityName;
                                        preparationDetail.materialDocNumber = preparationDetail_Model.materialDocNumber;
                                        preparationDetail.materialName = preparationDetail_Model.materialName;
                                        preparationDetail.materialUnit = preparationDetail_Model.materialUnit;
                                        listPreparationDetailUpdate.Add(preparationDetail);
                                    }
                                }
                            }
                        }

                        string error = string.Empty;
                        bool result = AscmWmsPreparationMainService.GetInstance().DoWebPreparation(preparationMain, listPreparationDetailUpdate, ref error);
                        jsonObjectResult.result = result;
                        jsonObjectResult.message = error;
                        if (result)
                            jsonObjectResult.entity = preparationMain;
                    }
                }
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        //作业备料单明细执行"取消加入"操作
        public ActionResult WmsWipJobPreparationDetailCancel(int? id, int? wipEntityId, string materialIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            if (wipEntityId.HasValue && !string.IsNullOrEmpty(materialIds))
            {
                List<AscmWmsContainerDelivery> listContainerDelivery = AscmWmsContainerDeliveryService.GetInstance().GetList("from AscmWmsContainerDelivery where wipEntityId=" + wipEntityId + " and materialId in(" + materialIds + ")");
                if (listContainerDelivery != null && listContainerDelivery.Count > 0)
                {
                    List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetList("from AscmWmsPreparationDetail where wipEntityId=" + wipEntityId + " and materialId in(" + materialIds + ")");
                    if (listPreparationDetail != null && listPreparationDetail.Count > 0)
                    {
                        string userName = string.Empty;
                        if (User.Identity.IsAuthenticated)
                            userName = User.Identity.Name;

                        List<AscmWipRequirementOperations> listBomUpdate = null;
                        List<AscmWmsContainerDelivery> listContainerDeliveryUpdate = null;
                        List<AscmWipRequirementOperations> listBom = AscmWipRequirementOperationsService.GetInstance().GetList("from AscmWipRequirementOperations where wipEntityId=" + wipEntityId + " and inventoryItemId in(" + materialIds + ")");
                        foreach (AscmWmsPreparationDetail preparationDetail in listPreparationDetail)
                        {
                            List<AscmWmsContainerDelivery> _listContainerDelivery = listContainerDelivery.FindAll(P => P.preparationMainId == preparationDetail.mainId && P.materialId == preparationDetail.materialId);
                            if (_listContainerDelivery != null && _listContainerDelivery.Count > 0)
                            {
                                //已传递给物料领料模块的数量
                                decimal sendLogisticsQuantity = preparationDetail.sendLogisticsQuantity;
                                //备料数量
                                decimal prepareQuantity = _listContainerDelivery.Sum(P => P.quantity);
                                //取消加入数量
                                decimal cancelQuantity = prepareQuantity - sendLogisticsQuantity;
                                if (cancelQuantity > decimal.Zero)
                                {
                                    listContainerDeliveryUpdate = listContainerDeliveryUpdate ?? new List<AscmWmsContainerDelivery>();
                                    foreach (AscmWmsContainerDelivery containerDelivery in _listContainerDelivery.OrderBy(P => P.statusSn))
                                    {
                                        decimal realCancelQuantity = cancelQuantity > containerDelivery.quantity ? containerDelivery.quantity : cancelQuantity;
                                        containerDelivery.quantity -= realCancelQuantity;
                                        cancelQuantity -= realCancelQuantity;
                                        containerDelivery.modifyUser = userName;
                                        containerDelivery.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        listContainerDeliveryUpdate.Add(containerDelivery);
                                        //更新作业BOM备料数量
                                        if (listBom != null)
                                        {
                                            listBomUpdate = listBomUpdate ?? new List<AscmWipRequirementOperations>();
                                            AscmWipRequirementOperations wipRequirementOperations = listBom.Find(P => P.wipEntityId == containerDelivery.wipEntityId && P.inventoryItemId == containerDelivery.materialId);
                                            if (wipRequirementOperations != null)
                                            {
                                                wipRequirementOperations.ascmPreparedQuantity -= realCancelQuantity;
                                                listBomUpdate.Add(wipRequirementOperations);
                                            }
                                        }
                                        if (cancelQuantity == decimal.Zero)
                                            break;
                                    }
                                }
                            }
                        }
                        if (listContainerDeliveryUpdate != null && listContainerDeliveryUpdate.Count > 0)
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().Clear();
                            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                            {
                                try
                                {
                                    YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listContainerDeliveryUpdate);

                                    if (listBomUpdate != null && listBomUpdate.Count > 0)
                                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listBomUpdate);

                                    tx.Commit();
                                    jsonObjectResult.result = true;
                                }
                                catch (Exception ex)
                                {
                                    tx.Rollback();//回滚
                                    YnBaseClass2.Helper.LogHelper.GetLog().Error("备料单取消加入失败", ex);
                                    jsonObjectResult.message = ex.Message;
                                }
                            }
                            //更新备料单状态
                            if (jsonObjectResult.result)
                            {
                                string whereOther = string.Empty;
                                string preparationMainIds = string.Join(",", listPreparationDetail.Where(P => listContainerDeliveryUpdate.Select(T => T.materialId).Contains(P.materialId)).Select(P => P.mainId));
                                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "id in(" + preparationMainIds + ")");
                                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "status in('" + AscmWmsPreparationMain.StatusDefine.preparingUnConfirm + "','" + AscmWmsPreparationMain.StatusDefine.preparing + "','" + AscmWmsPreparationMain.StatusDefine.prepared + "')");
                                List<AscmWmsPreparationMain> listPreparationMain = AscmWmsPreparationMainService.GetInstance().GetList(null, "", "", "", whereOther);
                                if (listPreparationMain != null && listPreparationMain.Count > 0)
                                {
                                    List<AscmWmsPreparationMain> listPreparationMainUpdate = new List<AscmWmsPreparationMain>();
                                    foreach (AscmWmsPreparationMain preparationMain in listPreparationMain)
                                    {
                                        string previousStatus = preparationMain.status;
                                        if (preparationMain.containerBindNumber == decimal.Zero)
                                            preparationMain.status = AscmWmsPreparationMain.StatusDefine.unPrepare;
                                        else if ((previousStatus == AscmWmsPreparationMain.StatusDefine.prepared && preparationMain.containerBindNumber < preparationMain.totalNumber) ||
                                            (previousStatus == AscmWmsPreparationMain.StatusDefine.preparing && listContainerDeliveryUpdate.Exists(P => P.preparationMainId == preparationMain.id)))
                                            preparationMain.status = AscmWmsPreparationMain.StatusDefine.preparingUnConfirm;
                                        else
                                            preparationMain.status = AscmWmsPreparationMain.StatusDefine.preparingUnConfirm;

                                        if (previousStatus != preparationMain.status)
                                        {
                                            preparationMain.modifyUser = userName;
                                            preparationMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            listPreparationMainUpdate.Add(preparationMain);
                                        }
                                    }
                                    if (listPreparationMainUpdate.Count > 0)
                                    {
                                        AscmWmsPreparationMainService.GetInstance().Update(listPreparationMainUpdate);
                                        if (id.HasValue)
                                            jsonObjectResult.entity = listPreparationMainUpdate.Find(P => P.id == id.Value);
                                    }
                                }

                                //if (id.HasValue) 
                                //{
                                //    AscmWmsPreparationMain mainInfo = AscmWmsPreparationMainService.GetInstance().Get(id.Value);
                                //    if (mainInfo != null) 
                                //    {
                                //        if (mainInfo.containerBindNumber == decimal.Zero)
                                //            mainInfo.status = AscmWmsPreparationMain.StatusDefine.unPrepare;
                                //        else if ((mainInfo.status == AscmWmsPreparationMain.StatusDefine.prepared && mainInfo.containerBindNumber < mainInfo.totalNumber) ||
                                //            (mainInfo.status == AscmWmsPreparationMain.StatusDefine.preparing && listContainerDeliveryUpdate.Exists(P => P.preparationMainId == mainInfo.id)))
                                //            mainInfo.status = AscmWmsPreparationMain.StatusDefine.preparingUnConfirm;
                                //        else
                                //            mainInfo.status = AscmWmsPreparationMain.StatusDefine.preparingUnConfirm;

                                //        AscmWmsPreparationMainService.GetInstance().Update(mainInfo);
                                //        jsonObjectResult.entity = mainInfo;
                                //    }
                                //}
                            }
                        }
                    }
                }
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        //备料单明细执行"设置货位"操作
        public ActionResult WmsPreparationDetailSetLocation(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            if (id.HasValue)
            {
                List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetList("from AscmWmsPreparationDetail where mainId=" + id.Value);
                if (listPreparationDetail != null && listPreparationDetail.Count > 0)
                {
                    //货位物料
                    string warehouseIds = "'" + string.Join("','", listPreparationDetail.Select(P => P.warehouseId).Distinct()) + "'";
                    List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetListByWarehouseIds(warehouseIds, false, true);
                    if (listLocationMaterialLink != null && listLocationMaterialLink.Count > 0)
                    {
                        //设置物料货位
                        List<AscmWmsPreparationDetail> listPreparationDetailUpdate = new List<AscmWmsPreparationDetail>();
                        foreach (AscmWmsPreparationDetail preparationDetail in listPreparationDetail)
                        {
                            if (preparationDetail.warelocationId == 0)
                            {
                                AscmLocationMaterialLink locationMaterialLink = listLocationMaterialLink.Find(P => P.warehouseId == preparationDetail.warehouseId && P.pk.materialId == preparationDetail.materialId);
                                if (locationMaterialLink != null)
                                {
                                    preparationDetail.warelocationId = locationMaterialLink.pk.warelocationId;
                                    listPreparationDetailUpdate.Add(preparationDetail);
                                }
                            }
                        }
                        //执行事务
                        YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().Clear();
                        using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                        {
                            try
                            {
                                if (listPreparationDetailUpdate.Count > 0)
                                    YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listPreparationDetailUpdate);

                                tx.Commit();
                                jsonObjectResult.result = true;
                            }
                            catch (Exception ex)
                            {
                                tx.Rollback();//回滚
                                YnBaseClass2.Helper.LogHelper.GetLog().Error("货位设置失败", ex);
                            }
                        }
                    }
                }
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        //备料单执行"单据确认"操作
        public ActionResult WmsPreparationConfirm(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            //单据确认：备料单状态如果是“备料中_未确认”，则更新状态为“备料中_已确认”；“已备齐”的单据不需要确认
            if (id.HasValue)
            {
                AscmWmsPreparationMain preparationMain = AscmWmsPreparationMainService.GetInstance().Get(id.Value);
                if (preparationMain != null)
                {
                    if (preparationMain.status == AscmWmsPreparationMain.StatusDefine.preparingUnConfirm)
                    {
                        string userName = string.Empty;
                        if (User.Identity.IsAuthenticated)
                            userName = User.Identity.Name;

                        preparationMain.status = AscmWmsPreparationMain.StatusDefine.preparing;
                        preparationMain.modifyUser = userName;
                        preparationMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        AscmWmsPreparationMainService.GetInstance().Update(preparationMain, true);

                        AscmWipDiscreteJobsStatus jobsStatus = AscmWipDiscreteJobsService.Instance.Get(preparationMain.wipEntityId, AscmWhTeamUserService.Instance.GetLeaderId(preparationMain.createUser));
                        if (jobsStatus != null) 
                        {
                            jobsStatus.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing;
                            AscmWipDiscreteJobsService.Instance.Update(jobsStatus);
                        }

                        jsonObjectResult.result = true;
                        jsonObjectResult.entity = preparationMain;
                    }
                }
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }

        //备料单批量执行"单据确认"操作
        public ActionResult WmsPreparationBatchConfirm(string preparationMainIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            if (!string.IsNullOrEmpty(preparationMainIds))
            {
                List<AscmWmsPreparationMain> listPreparationMain = AscmWmsPreparationMainService.GetInstance().GetListByIds(preparationMainIds);
                if (listPreparationMain != null && listPreparationMain.Count > 0)
                {
                    string userName = string.Empty;
                    if (User.Identity.IsAuthenticated)
                        userName = User.Identity.Name;

                    List<AscmWmsPreparationMain> listPreparationMainUpdate = new List<AscmWmsPreparationMain>();
                    foreach (AscmWmsPreparationMain preparationMain in listPreparationMain)
                    {
                        if (preparationMain.status == AscmWmsPreparationMain.StatusDefine.preparingUnConfirm)
                        {
                            preparationMain.status = AscmWmsPreparationMain.StatusDefine.preparing;
                            preparationMain.modifyUser = userName;
                            preparationMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            listPreparationMainUpdate.Add(preparationMain);

                            AscmWipDiscreteJobsStatus jobsStatus = AscmWipDiscreteJobsService.Instance.Get(preparationMain.wipEntityId, AscmWhTeamUserService.Instance.GetLeaderId(preparationMain.createUser));
                            if (jobsStatus != null)
                            {
                                jobsStatus.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing;
                                AscmWipDiscreteJobsService.Instance.Update(jobsStatus);
                            }
                        }
                    }
                    if (listPreparationMainUpdate.Count > 0)
                    {
                        AscmWmsPreparationMainService.GetInstance().Update(listPreparationMainUpdate, true);
                        jsonObjectResult.result = true;
                    }
                }
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        //备料单批量执行"加入"操作
        public ActionResult WmsPreparationBatchAdd(string preparationMainIds, string startMaterialDocNumber, string endMaterialDocNumber)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            List<AscmWmsPreparationMain> listPreparationMain = AscmWmsPreparationMainService.GetInstance().GetListByIds(preparationMainIds);
            if (listPreparationMain != null && listPreparationMain.Count > 0)
            {
                string hql = "select new AscmWmsPreparationDetail(awpd,ami.docNumber,'','','',awe.name,awro.ascmPreparedQuantity) from AscmWmsPreparationDetail awpd,AscmWipEntities awe,AscmWipRequirementOperations awro,AscmMaterialItem ami";
                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.wipEntityId=awe.wipEntityId");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.wipEntityId=awro.wipEntityId");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.materialId=awro.inventoryItemId");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.materialId=ami.id");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "awpd.mainId in(" + preparationMainIds + ")");
                //物料编码
                string whereStartMaterialDocNumber = "", whereEndMaterialDocNumber = "";
                if (!string.IsNullOrEmpty(startMaterialDocNumber))
                    whereStartMaterialDocNumber = "ami.docNumber='" + startMaterialDocNumber + "'";
                if (!string.IsNullOrEmpty(endMaterialDocNumber))
                {
                    if (!string.IsNullOrEmpty(startMaterialDocNumber))
                        whereStartMaterialDocNumber = "ami.docNumber>='" + startMaterialDocNumber + "'";
                    whereEndMaterialDocNumber = "ami.docNumber<='" + endMaterialDocNumber + "'";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereStartMaterialDocNumber);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereEndMaterialDocNumber);
                hql += " where " + where;
                List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetList(hql);
                if (listPreparationDetail != null && listPreparationDetail.Count > 0)
                {
                    StringBuilder sbMessage = new StringBuilder();
                    foreach (AscmWmsPreparationMain preparationMain in listPreparationMain)
                    {
                        List<AscmWmsPreparationDetail> _listPreparationDetail = listPreparationDetail.FindAll(P => P.mainId == preparationMain.id && P.prepareQuantity > decimal.Zero);
                        if (_listPreparationDetail != null && _listPreparationDetail.Count > 0)
                        {
                            string error = string.Empty;
                            bool result = AscmWmsPreparationMainService.GetInstance().DoWebPreparation(preparationMain, _listPreparationDetail, ref error, true);
                            if (!string.IsNullOrEmpty(error))
                                sbMessage.Append(error);
                        }
                    }
                    jsonObjectResult.message = sbMessage.ToString();
                    jsonObjectResult.result = string.IsNullOrEmpty(sbMessage.ToString());
                }
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        //备料单批量执行"设置货位"操作
        public void WmsPreparationDetailBatchSetLocation(List<AscmWmsPreparationMain> listAscmWmsPreparationMain)
        {
            if (listAscmWmsPreparationMain != null && listAscmWmsPreparationMain.Count > 0)
            {
                string mainIds = string.Join(",", listAscmWmsPreparationMain.Select(P => P.id.ToString()).Distinct());
                List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetList("from AscmWmsPreparationDetail where mainId in (" + mainIds + ")");
                if (listPreparationDetail != null && listPreparationDetail.Count > 0)
                {
                    //货位物料
                    string warehouseIds = "'" + string.Join("','", listPreparationDetail.Select(P => P.warehouseId).Distinct()) + "'";
                    List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetListByWarehouseIds(warehouseIds, false, true);
                    if (listLocationMaterialLink != null && listLocationMaterialLink.Count > 0)
                    {
                        //设置物料货位
                        List<AscmWmsPreparationDetail> listPreparationDetailUpdate = new List<AscmWmsPreparationDetail>();
                        foreach (AscmWmsPreparationDetail preparationDetail in listPreparationDetail)
                        {
                            if (preparationDetail.warelocationId == 0)
                            {
                                AscmLocationMaterialLink locationMaterialLink = listLocationMaterialLink.Find(P => P.warehouseId == preparationDetail.warehouseId && P.pk.materialId == preparationDetail.materialId);
                                if (locationMaterialLink != null)
                                {
                                    preparationDetail.warelocationId = locationMaterialLink.pk.warelocationId;
                                    listPreparationDetailUpdate.Add(preparationDetail);
                                }
                            }
                        }
                        //执行事务
                        YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().Clear();
                        using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                        {
                            try
                            {
                                if (listPreparationDetailUpdate.Count > 0)
                                    YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listPreparationDetailUpdate);

                                tx.Commit();
                            }
                            catch (Exception ex)
                            {
                                tx.Rollback();//回滚
                                YnBaseClass2.Helper.LogHelper.GetLog().Error("货位设置失败", ex);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 作业备料监控
        public ActionResult WmsWipJobPreparationMonitorIndex()
        {
            ////作业备料状态
            //List<SelectListItem> listPrepareStatus = new List<SelectListItem>();
            //listPrepareStatus.Add(new SelectListItem { Text = "", Value = "" });
            //AscmWipDiscreteJobs.AscmStatusDefine.GetList().ForEach(P => listPrepareStatus.Add(new SelectListItem { Text = AscmWipDiscreteJobs.AscmStatusDefine.DisplayText(P), Value = P }));
            //ViewData["listPrepareStatus"] = listPrepareStatus;

            return View();
        }
        public ActionResult WmsWipJobPreparationMonitorList(int? page, int? rows, string sort, string order,
            string startScheduledStartDate, string endScheduledStartDate,
            string startWipEntitiesName, string endWipEntitiesName, string wipSupplyType, string queryLeader, string prepareStatus,
            string startSupplySubInventory, string endSupplySubInventory,
            string startMaterialDocNumber, string endMaterialDocNumber,
            string startScheduleGroupName, string endScheduleGroupName,string mystatus)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            string whereOther = "", whereBom = "";
            
            //作业状态
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "statusType in(" + AscmWipDiscreteJobs.StatusTypeDefine.yff + "," + AscmWipDiscreteJobs.StatusTypeDefine.wc + "," + AscmWipDiscreteJobs.StatusTypeDefine.wcbjf + ")");
            
            //负责人
            if (queryLeader == "ME" && User.Identity.IsAuthenticated) 
            {
                string userId =  User.Identity.Name;
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " b.leaderId='" + userId + "' ");
            }

            //作业日期
            string whereStartScheduledStartDate = "", whereEndScheduledStartDate = "";
            DateTime dtStartScheduledStartDate, dtEndScheduledStartDate;
            if (!string.IsNullOrEmpty(startScheduledStartDate) && DateTime.TryParse(startScheduledStartDate, out dtStartScheduledStartDate))
            {
                whereStartScheduledStartDate = "scheduledStartDate>='" + dtStartScheduledStartDate.ToString("yyyy-MM-dd 00:00:00") + "'";
                whereEndScheduledStartDate = "scheduledStartDate<'" + dtStartScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            }
            if (!string.IsNullOrEmpty(endScheduledStartDate) && DateTime.TryParse(endScheduledStartDate, out dtEndScheduledStartDate))
                whereEndScheduledStartDate = "scheduledStartDate<'" + dtEndScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartScheduledStartDate);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndScheduledStartDate);
            
            //作业号
            string whereWipEntity = "";
            string whereStartWipEntityName = "", whereEndWipEntityName = "";
            if (!string.IsNullOrEmpty(startWipEntitiesName))
                whereStartWipEntityName = "name='" + startWipEntitiesName + "'";
            if (!string.IsNullOrEmpty(endWipEntitiesName))
            {
                if (!string.IsNullOrEmpty(startWipEntitiesName))
                    whereStartWipEntityName = "name>='" + startWipEntitiesName + "'";
                whereEndWipEntityName = "name<='" + endWipEntitiesName + "'";
            }
            whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, whereStartWipEntityName);
            whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, whereEndWipEntityName);
            if (!string.IsNullOrEmpty(whereWipEntity))
            {
                //内、外机作业分开查询
                AscmWipEntities.WipEntityType bWipEntityType = AscmWipEntitiesService.GetInstance().GetWipEntityType(startWipEntitiesName);
                AscmWipEntities.WipEntityType eWipEntityType = AscmWipEntitiesService.GetInstance().GetWipEntityType(endWipEntitiesName);
                if (bWipEntityType == AscmWipEntities.WipEntityType.withinTheMachine && bWipEntityType == eWipEntityType)
                    whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, "instr(upper(substr(name,1,(length(name)-3))),'N',-1) > instr(upper(substr(name,1,(length(name)-3))),'W',-1)");
                else if (eWipEntityType == AscmWipEntities.WipEntityType.outsideTheMachine && bWipEntityType == eWipEntityType)
                    whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, "instr(upper(substr(name,1,(length(name)-3))),'W',-1) > instr(upper(substr(name,1,(length(name)-3))),'N',-1)");

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "a.wipEntityId in (select wipEntityId from Ascm_Wip_Entities where " + whereWipEntity + ")");
            }

            //供应类型
            string whereWipSupplyType = "";
            if (wipSupplyType == "pullType") //拉式
                whereWipSupplyType = "wipSupplyType in(" + AscmMaterialItem.WipSupplyTypeDefine.zpls + "," + AscmMaterialItem.WipSupplyTypeDefine.lsgx + ")";
            else if (wipSupplyType == "pushType") //推式
                whereWipSupplyType = "wipSupplyType in(" + AscmMaterialItem.WipSupplyTypeDefine.ts + ")";
            whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, whereWipSupplyType);
            
            //备料状态
            if (!string.IsNullOrEmpty(prepareStatus))
            {
                //if (prepareStatus == AscmWipDiscreteJobs.AscmStatusDefine.unPrint)
                //    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "ascmStatus='' or ascmStatus is null");
                //else
                //    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "ascmStatus='" + prepareStatus + "'");

                //支持"备料状态"多选
                string wherePrepareStatus = string.Empty;
                string _prepareStatus = string.Empty;
                foreach (string status in prepareStatus.Split(','))
                {
                    if (status == AscmWipDiscreteJobs.AscmStatusDefine.unPrint||status==AscmWipDiscreteJobs.AscmStatusDefine.unPrepare)
                        wherePrepareStatus = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(wherePrepareStatus, "ascmStatus='' or ascmStatus is null");
                    else
                    {
                        if (!string.IsNullOrEmpty(_prepareStatus))
                            _prepareStatus += ",";
                        _prepareStatus += "'" + status + "'";
                    }
                }
                if (!string.IsNullOrEmpty(_prepareStatus))
                    wherePrepareStatus = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(wherePrepareStatus, "ascmStatus in(" + _prepareStatus + ")");
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, wherePrepareStatus);
            }

            //计划组、车间(比较计划组名称)
            string whereScheduleGroupOther = "";
            string whereStartScheduleGroup = "", whereEndScheduleGroup = "";
            if (!string.IsNullOrEmpty(startScheduleGroupName))
                whereStartScheduleGroup = "scheduleGroupName='" + startScheduleGroupName + "'";
            if (!string.IsNullOrEmpty(endScheduleGroupName))
            {
                if (!string.IsNullOrEmpty(startScheduleGroupName))
                    whereStartScheduleGroup = "translate(schedulegroupname, '一二三四五六七八九', '123456789')>=translate('" + startScheduleGroupName + "', '一二三四五六七八九', '123456789')";
                whereEndScheduleGroup = "translate(schedulegroupname, '一二三四五六七八九', '123456789')<=translate('" + endScheduleGroupName + "', '一二三四五六七八九', '123456789')";
            }
            whereScheduleGroupOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduleGroupOther, whereStartScheduleGroup);
            whereScheduleGroupOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduleGroupOther, whereEndScheduleGroup);
            List<AscmWipScheduleGroups> listWipScheduleGroups = null;
            if (string.IsNullOrEmpty(whereScheduleGroupOther))
            {
                //电装、总装计划组
                List<string> listScheduleGroupNames = new List<string>();
                listScheduleGroupNames.AddRange(AscmWipScheduleGroups.TypeDefine.GetWipScheduleGroupNames(AscmWipScheduleGroups.TypeDefine.GA));
                listScheduleGroupNames.AddRange(AscmWipScheduleGroups.TypeDefine.GetWipScheduleGroupNames(AscmWipScheduleGroups.TypeDefine.QC));
                listWipScheduleGroups = AscmWipScheduleGroupsService.GetInstance().GetList(null, "", "", "", "scheduleGroupName in('" + string.Join("','", listScheduleGroupNames) + "')");
            }
            else
                listWipScheduleGroups = AscmWipScheduleGroupsService.GetInstance().GetList(null, "", "", "", whereScheduleGroupOther);
            if (listWipScheduleGroups != null && listWipScheduleGroups.Count > 0)
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "scheduleGroupId in(" + string.Join(",", listWipScheduleGroups.Select(P => P.scheduleGroupId)) + ")");
            
            //供应子库
            string whereStartSupplySubInventory = "", whereEndSupplySubInventory = "";
            if (!string.IsNullOrEmpty(startSupplySubInventory))
                whereStartSupplySubInventory = "supplySubinventory='" + startSupplySubInventory + "'";
            if (!string.IsNullOrEmpty(endSupplySubInventory))
            {
                if (!string.IsNullOrEmpty(startSupplySubInventory))
                    whereStartSupplySubInventory = "supplySubinventory>='" + startSupplySubInventory + "'";
                whereEndSupplySubInventory = "supplySubinventory<='" + endSupplySubInventory + "'";
            }
            whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, whereStartSupplySubInventory);
            whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, whereEndSupplySubInventory);
            
            //物料编码
            string whereInventoryItem = "";
            string whereStartInventoryItemId = "", whereEndInventoryItemId = "";
            if (startMaterialDocNumber != null && startMaterialDocNumber.Trim() != "")
                whereStartInventoryItemId = "upper(docNumber)='" + startMaterialDocNumber.Trim().ToUpper() + "'";
            if (endMaterialDocNumber != null && endMaterialDocNumber.Trim() != "")
            {
                if (startMaterialDocNumber != null && startMaterialDocNumber.Trim() != "")
                    whereStartInventoryItemId = "upper(docNumber)>='" + startMaterialDocNumber.Trim().ToUpper() + "'";
                whereEndInventoryItemId = "upper(docNumber)<='" + endMaterialDocNumber.Trim().ToUpper() + "'";
            }
            whereInventoryItem = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereInventoryItem, whereStartInventoryItemId);
            whereInventoryItem = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereInventoryItem, whereEndInventoryItemId);
            
            if (!string.IsNullOrEmpty(whereInventoryItem))
                whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, "inventoryItemId in(select id from Ascm_Material_Item where " + whereInventoryItem + ")");
            if (!string.IsNullOrEmpty(whereBom))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "a.wipEntityId in (select wipEntityId from Ascm_Wip_Require_Operat where " + whereBom + ")");

            List<AscmWipDiscreteJobs> listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetListForMonitor(ynPage, "ascmStatus", order, "", whereOther, true, false, true, false);
            //List<AscmWipDiscreteJobs> listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList("from ASCM_WIP_DISCRETE_JOBS where createuser='63980'");
            if (listWipDiscreteJobs != null)
            {
                //获取排产计划产线
                AscmWipDiscreteJobsService.GetInstance().SetScheduleProductionLine(listWipDiscreteJobs);
                //缺料情况
                AscmWipDiscreteJobsService.GetInstance().GetLackOfMaterial(listWipDiscreteJobs);
                //发料校验
                AscmWipDiscreteJobsService.GetInstance().GetWmsStoreIssueCheck(listWipDiscreteJobs);
                //按照“备料状态〉作业号〉作业时间”排序
                var result = listWipDiscreteJobs.OrderBy(P => P.ascmStatusSn);
                foreach (AscmWipDiscreteJobs wipDiscreteJobs in result)
                {
                    if (listWipScheduleGroups != null)
                        wipDiscreteJobs.ascmWipScheduleGroups = listWipScheduleGroups.Find(P => P.scheduleGroupId == wipDiscreteJobs.scheduleGroupId);
                    jsonDataGridResult.rows.Add(wipDiscreteJobs);
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TempTest_WmsWipJobPreparationMonitorList(int? page, int? rows, string sort, string order,
            string startScheduledStartDate, string endScheduledStartDate,
            string startWipEntitiesName, string endWipEntitiesName, string wipSupplyType, string prepareStatus,
            string startSupplySubInventory, string endSupplySubInventory,
            string startMaterialDocNumber, string endMaterialDocNumber,
            string startScheduleGroupName, string endScheduleGroupName)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            string whereOther = "", whereBom = "";
            //作业状态
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "statusType in(" + AscmWipDiscreteJobs.StatusTypeDefine.yff + "," + AscmWipDiscreteJobs.StatusTypeDefine.wc + "," + AscmWipDiscreteJobs.StatusTypeDefine.wcbjf + ")");
            //作业日期
            string whereStartScheduledStartDate = "", whereEndScheduledStartDate = "";
            DateTime dtStartScheduledStartDate, dtEndScheduledStartDate;
            if (!string.IsNullOrEmpty(startScheduledStartDate) && DateTime.TryParse(startScheduledStartDate, out dtStartScheduledStartDate))
            {
                whereStartScheduledStartDate = "scheduledStartDate>='" + dtStartScheduledStartDate.ToString("yyyy-MM-dd 00:00:00") + "'";
                whereEndScheduledStartDate = "scheduledStartDate<'" + dtStartScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            }
            if (!string.IsNullOrEmpty(endScheduledStartDate) && DateTime.TryParse(endScheduledStartDate, out dtEndScheduledStartDate))
                whereEndScheduledStartDate = "scheduledStartDate<'" + dtEndScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartScheduledStartDate);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndScheduledStartDate);
            //作业号

            //AscmWipDiscreteJobs aa;


            string whereWipEntity = "";
            string whereStartWipEntityName = "", whereEndWipEntityName = "";
            if (!string.IsNullOrEmpty(startWipEntitiesName))
                whereStartWipEntityName = "name='" + startWipEntitiesName + "'";
            if (!string.IsNullOrEmpty(endWipEntitiesName))
            {
                if (!string.IsNullOrEmpty(startWipEntitiesName))
                    whereStartWipEntityName = "name>='" + startWipEntitiesName + "'";
                whereEndWipEntityName = "name<='" + endWipEntitiesName + "'";
            }
            whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, whereStartWipEntityName);
            whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, whereEndWipEntityName);
            if (!string.IsNullOrEmpty(whereWipEntity))
            {
                //内、外机作业分开查询
                AscmWipEntities.WipEntityType bWipEntityType = AscmWipEntitiesService.GetInstance().GetWipEntityType(startWipEntitiesName);
                AscmWipEntities.WipEntityType eWipEntityType = AscmWipEntitiesService.GetInstance().GetWipEntityType(endWipEntitiesName);
                if (bWipEntityType == AscmWipEntities.WipEntityType.withinTheMachine && bWipEntityType == eWipEntityType)
                    whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, "instr(upper(substr(name,1,(length(name)-3))),'N',-1) > instr(upper(substr(name,1,(length(name)-3))),'W',-1)");
                else if (eWipEntityType == AscmWipEntities.WipEntityType.outsideTheMachine && bWipEntityType == eWipEntityType)
                    whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, "instr(upper(substr(name,1,(length(name)-3))),'W',-1) > instr(upper(substr(name,1,(length(name)-3))),'N',-1)");

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "wipEntityId in (select wipEntityId from AscmWipEntities where " + whereWipEntity + ")");
            }
            //供应类型
            string whereWipSupplyType = "";
            if (wipSupplyType == "pullType") //拉式
                whereWipSupplyType = "wipSupplyType in(" + AscmMaterialItem.WipSupplyTypeDefine.zpls + "," + AscmMaterialItem.WipSupplyTypeDefine.lsgx + ")";
            else if (wipSupplyType == "pushType") //推式
                whereWipSupplyType = "wipSupplyType in(" + AscmMaterialItem.WipSupplyTypeDefine.ts + ")";
            whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, whereWipSupplyType);
            //备料状态
            if (!string.IsNullOrEmpty(prepareStatus))
            {
                //if (prepareStatus == AscmWipDiscreteJobs.AscmStatusDefine.unPrint)
                //    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "ascmStatus='' or ascmStatus is null");
                //else
                //    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "ascmStatus='" + prepareStatus + "'");

                //支持"备料状态"多选
                string wherePrepareStatus = string.Empty;
                string _prepareStatus = string.Empty;
                foreach (string status in prepareStatus.Split(','))
                {
                    if (status == AscmWipDiscreteJobs.AscmStatusDefine.unPrint)
                        wherePrepareStatus = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(wherePrepareStatus, "ascmStatus='' or ascmStatus is null");
                    else
                    {
                        if (!string.IsNullOrEmpty(_prepareStatus))
                            _prepareStatus += ",";
                        _prepareStatus += "'" + status + "'";
                    }
                }
                if (!string.IsNullOrEmpty(_prepareStatus))
                    wherePrepareStatus = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(wherePrepareStatus, "ascmStatus in(" + _prepareStatus + ")");
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, wherePrepareStatus);
            }
            //计划组、车间(比较计划组名称)
            string whereScheduleGroupOther = "";
            string whereStartScheduleGroup = "", whereEndScheduleGroup = "";
            if (!string.IsNullOrEmpty(startScheduleGroupName))
                whereStartScheduleGroup = "scheduleGroupName='" + startScheduleGroupName + "'";
            if (!string.IsNullOrEmpty(endScheduleGroupName))
            {
                if (!string.IsNullOrEmpty(startScheduleGroupName))
                    whereStartScheduleGroup = "translate(schedulegroupname, '一二三四五六七八九', '123456789')>=translate('" + startScheduleGroupName + "', '一二三四五六七八九', '123456789')";
                whereEndScheduleGroup = "translate(schedulegroupname, '一二三四五六七八九', '123456789')<=translate('" + endScheduleGroupName + "', '一二三四五六七八九', '123456789')";
            }
            whereScheduleGroupOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduleGroupOther, whereStartScheduleGroup);
            whereScheduleGroupOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduleGroupOther, whereEndScheduleGroup);
            List<AscmWipScheduleGroups> listWipScheduleGroups = null;
            if (string.IsNullOrEmpty(whereScheduleGroupOther))
            {
                //电装、总装计划组
                List<string> listScheduleGroupNames = new List<string>();
                listScheduleGroupNames.AddRange(AscmWipScheduleGroups.TypeDefine.GetWipScheduleGroupNames(AscmWipScheduleGroups.TypeDefine.GA));
                listScheduleGroupNames.AddRange(AscmWipScheduleGroups.TypeDefine.GetWipScheduleGroupNames(AscmWipScheduleGroups.TypeDefine.QC));
                listWipScheduleGroups = AscmWipScheduleGroupsService.GetInstance().GetList(null, "", "", "", "scheduleGroupName in('" + string.Join("','", listScheduleGroupNames) + "')");
            }
            else
                listWipScheduleGroups = AscmWipScheduleGroupsService.GetInstance().GetList(null, "", "", "", whereScheduleGroupOther);
            if (listWipScheduleGroups != null && listWipScheduleGroups.Count > 0)
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "scheduleGroupId in(" + string.Join(",", listWipScheduleGroups.Select(P => P.scheduleGroupId)) + ")");
            //供应子库
            string whereStartSupplySubInventory = "", whereEndSupplySubInventory = "";
            if (!string.IsNullOrEmpty(startSupplySubInventory))
                whereStartSupplySubInventory = "supplySubinventory='" + startSupplySubInventory + "'";
            if (!string.IsNullOrEmpty(endSupplySubInventory))
            {
                if (!string.IsNullOrEmpty(startSupplySubInventory))
                    whereStartSupplySubInventory = "supplySubinventory>='" + startSupplySubInventory + "'";
                whereEndSupplySubInventory = "supplySubinventory<='" + endSupplySubInventory + "'";
            }
            whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, whereStartSupplySubInventory);
            whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, whereEndSupplySubInventory);
            //物料编码
            string whereInventoryItem = "";
            string whereStartInventoryItemId = "", whereEndInventoryItemId = "";
            if (startMaterialDocNumber != null && startMaterialDocNumber.Trim() != "")
                whereStartInventoryItemId = "upper(docNumber)='" + startMaterialDocNumber.Trim().ToUpper() + "'";
            if (endMaterialDocNumber != null && endMaterialDocNumber.Trim() != "")
            {
                if (startMaterialDocNumber != null && startMaterialDocNumber.Trim() != "")
                    whereStartInventoryItemId = "upper(docNumber)>='" + startMaterialDocNumber.Trim().ToUpper() + "'";
                whereEndInventoryItemId = "upper(docNumber)<='" + endMaterialDocNumber.Trim().ToUpper() + "'";
            }
            whereInventoryItem = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereInventoryItem, whereStartInventoryItemId);
            whereInventoryItem = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereInventoryItem, whereEndInventoryItemId);
            if (!string.IsNullOrEmpty(whereInventoryItem))
                whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, "inventoryItemId in(select id from AscmMaterialItem where " + whereInventoryItem + ")");
            if (!string.IsNullOrEmpty(whereBom))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "wipEntityId in (select wipEntityId from AscmWipRequirementOperations where " + whereBom + ")");

            List<AscmWipDiscreteJobs> listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetListForMonitor(ynPage, "ascmStatus", order, "", whereOther, true, false, true, false);



            if (listWipDiscreteJobs != null)
            {
                //获取排产计划产线
                AscmWipDiscreteJobsService.GetInstance().SetScheduleProductionLine(listWipDiscreteJobs);
                //缺料情况
                AscmWipDiscreteJobsService.GetInstance().GetLackOfMaterial(listWipDiscreteJobs);
                //发料校验
                AscmWipDiscreteJobsService.GetInstance().GetWmsStoreIssueCheck(listWipDiscreteJobs);
                //按照“备料状态〉作业号〉作业时间”排序
                var result = listWipDiscreteJobs.OrderBy(P => P._scheduledStartDate).OrderBy(P => P.ascmWipEntities_Name).OrderBy(P => P.ascmStatusSn);
                foreach (AscmWipDiscreteJobs wipDiscreteJobs in result)
                {
                    if (listWipScheduleGroups != null)
                        wipDiscreteJobs.ascmWipScheduleGroups = listWipScheduleGroups.Find(P => P.scheduleGroupId == wipDiscreteJobs.scheduleGroupId);
                    jsonDataGridResult.rows.Add(wipDiscreteJobs);
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WmsWipJobPreparationList(int? wipEntityId, string leaderId)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            if (wipEntityId.HasValue)
            {
                StringBuilder userIds = new StringBuilder();

                //获取同组别的用户Id
                if (!string.IsNullOrEmpty(leaderId)) 
                {
                    List<AscmWhTeamUser> allTeamUsers = AscmWhTeamUserService.Instance.GetList("");
                    if (allTeamUsers != null && allTeamUsers.Count > 0) 
                    {
                        AscmWhTeamUser leaderUser = allTeamUsers.FirstOrDefault(a => a.isLeader == true && a.M_UserId == leaderId);
                        if (leaderUser != null) 
                        {
                            List<AscmWhTeamUser> teamUsers = allTeamUsers.Where(a => a.M_TeamId == leaderUser.M_TeamId).ToList();
                            if (teamUsers != null) 
                            {
                                foreach (var teamUser in teamUsers)
                                {
                                    userIds.Append("'" + teamUser.M_UserId + "',");
                                }
                                if (userIds.Length > 0) userIds.Remove(userIds.Length - 1, 1);
                            }
                        }
                    }
                }

                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wipEntityId=" + wipEntityId.Value);
                if (userIds.Length > 0) where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, " createUser in (" + userIds + ") ");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "pattern='" + AscmWmsPreparationMain.PatternDefine.wipJob + "'");
                string hql = "from AscmWmsPreparationMain where " + where;
                List<AscmWmsPreparationMain> listPreparationMain = AscmWmsPreparationMainService.GetInstance().GetList(hql);
                List<YnUser> listUser = null;
                if (userIds.Length > 0) 
                {
                    listUser = YnUserService.GetInstance().GetList("from YnUser where userId in( " + userIds + ") ");
                }

                if (listPreparationMain != null && listPreparationMain.Count > 0)
                {
                    if (listUser != null && listUser.Count > 0) 
                    {
                        foreach (var mainInfo in listPreparationMain)
                        {
                            var userInfo = listUser.FirstOrDefault(a => a.userId == mainInfo.createUser);
                            mainInfo.createUserName = userInfo != null ? userInfo.userName : "";
                        }
                    }

                    //设置备料单子库段和物料大类段
                    AscmWmsPreparationMainService.GetInstance().SetDetailInfo(listPreparationMain);
                    listPreparationMain.ForEach(P => jsonDataGridResult.rows.Add(P));
                }
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        //作业批量执行"备料确认"操作
        public ActionResult WmsWipJobPreparationConfirm(string wipEntityIds, string leaderIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            var currUserId = User.Identity.Name;
            StringBuilder strUserIds = new StringBuilder();
            if (leaderIds != null) 
            {
                string[] idList = leaderIds.Split(',');
                if (idList != null && idList.Length > 0) 
                {
                    foreach (var userId in idList)
                    {
                        if (userId.Trim() != "" && userId.Trim() != currUserId) 
                        {
                            jsonObjectResult.result = false;
                            jsonObjectResult.message = "当前用户不是负责人，权限不够！";

                            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
                        }

                        strUserIds.Append("'" + userId + "',");
                    }
                }
            }
            if (strUserIds.Length > 0) 
            {
                strUserIds.Remove(strUserIds.Length - 1, 1);
            }

            List<AscmWhTeamUser> teamUsers = AscmWhTeamUserService.Instance.GetListForTeam(currUserId);
            List<string> teamUserIds = teamUsers == null || teamUsers.Count == 0 ? new List<string>() : teamUsers.Select(a => a.M_UserId).ToList();

            if (!string.IsNullOrEmpty(wipEntityIds))
            {
                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wipEntityId in(" + wipEntityIds + ")");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "pattern='" + AscmWmsPreparationMain.PatternDefine.wipJob + "'");
                
                //不考虑“待备料”、“已备齐_待领料”、“备料中_待领料”、“已领料”的备料单
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, 
                    "status not in('" + AscmWmsPreparationMain.StatusDefine.unPrepare + "','"
                     + AscmWmsPreparationMain.StatusDefine.preparedUnPick + "','"
                     + AscmWmsPreparationMain.StatusDefine.preparingUnPick + "','"
                    + AscmWmsPreparationMain.StatusDefine.picked + "')");

                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "locked=0");
                string hql = "from AscmWmsPreparationMain where " + where;
                List<AscmWmsPreparationMain> listPreparationMain = AscmWmsPreparationMainService.GetInstance().GetList(hql);
                
                //过滤为同一仓库组别的备料单
                if (listPreparationMain != null && listPreparationMain.Count > 0)
                {
                    listPreparationMain = listPreparationMain.Where(a => teamUserIds.Contains(a.createUser)).ToList();
                }

                if (listPreparationMain != null && listPreparationMain.Count > 0)
                {
                    string preparationMainIds = string.Join(",", listPreparationMain.Select(P => P.id).Distinct());
                    List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetContainerBindNumberList(preparationMainIds);
                    if (listPreparationDetail != null && listPreparationDetail.Count > 0)
                    {
                        string userName = string.Empty;
                        if (User.Identity.IsAuthenticated)
                            userName = User.Identity.Name;
                        //传递给物流领料模块仓库备料信息
                        List<WmsAndLogistics> listWmsAndLogistics = null;
                        List<AscmWmsPreparationMain> listPreparationMainUpdate = null;
                        List<AscmWmsPreparationDetail> listPreparationDetailUpdate = null;
                        foreach (string wipEntityId in wipEntityIds.Split(','))
                        {
                            //获取可执行作业“备料确认”的备料单
                            List<AscmWmsPreparationMain> _listPreparationMain = listPreparationMain.FindAll(P => P.wipEntityId.ToString() == wipEntityId && (P.status == AscmWmsPreparationMain.StatusDefine.preparing || P.status == AscmWmsPreparationMain.StatusDefine.prepared));
                            if (_listPreparationMain == null || _listPreparationMain.Count == 0)
                                continue;
                            foreach (AscmWmsPreparationMain preparationMain in _listPreparationMain)
                            {
                                bool prepareConfirm = false;
                                preparationMain.listDetail = listPreparationDetail.FindAll(P => P.mainId == preparationMain.id);
                                if (preparationMain.listDetail != null && preparationMain.listDetail.Count > 0)
                                {
                                    foreach (AscmWmsPreparationDetail preparationDetail in preparationMain.listDetail)
                                    {
                                        List<AscmWmsPreparationDetail> _listPreparationDetail = listPreparationDetail.FindAll(P => P.wipEntityId.ToString() == wipEntityId && P.materialId == preparationDetail.materialId);
                                        if (_listPreparationDetail != null && _listPreparationDetail.Count > 0)
                                        {
                                            foreach (AscmWmsPreparationDetail _preparationDetail in _listPreparationDetail)
                                            {
                                                //本次传递给物料领料模块备料数量=备料数量-已传递给物料领料模块备料数量
                                                decimal sendLogisticsQuantity = _preparationDetail.containerBindNumber - _preparationDetail.sendLogisticsQuantity;
                                                if (sendLogisticsQuantity > 0)
                                                {
                                                    prepareConfirm = true;
                                                    listWmsAndLogistics = listWmsAndLogistics ?? new List<WmsAndLogistics>();
                                                    listPreparationDetailUpdate = listPreparationDetailUpdate ?? new List<AscmWmsPreparationDetail>();

                                                    WmsAndLogistics wmsAndLogistics = new WmsAndLogistics();
                                                    wmsAndLogistics.wipEntityId = _preparationDetail.wipEntityId;
                                                    wmsAndLogistics.materialId = _preparationDetail.materialId;
                                                    wmsAndLogistics.warehouseId = _preparationDetail.warehouseId;
                                                    wmsAndLogistics.quantity = sendLogisticsQuantity;
                                                    wmsAndLogistics.preparationString = _preparationDetail.mainId.ToString();
                                                    listWmsAndLogistics.Add(wmsAndLogistics);
                                                    //更新备料明细中传递给物料领料模块的数量
                                                    _preparationDetail.modifyUser = userName;
                                                    _preparationDetail.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                    _preparationDetail.sendLogisticsQuantity += sendLogisticsQuantity;
                                                    listPreparationDetailUpdate.Add(_preparationDetail);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (prepareConfirm)
                                {
                                    //更新备料单状态
                                    listPreparationMainUpdate = listPreparationMainUpdate ?? new List<AscmWmsPreparationMain>();
                                    preparationMain.modifyUser = userName;
                                    preparationMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    if (preparationMain.status == AscmWmsPreparationMain.StatusDefine.preparing)
                                        preparationMain.status = AscmWmsPreparationMain.StatusDefine.preparingUnPick;
                                    else if (preparationMain.status == AscmWmsPreparationMain.StatusDefine.prepared)
                                        preparationMain.status = AscmWmsPreparationMain.StatusDefine.preparedUnPick;
                                    listPreparationMainUpdate.Add(preparationMain);
                                }
                            }
                        }
                        if (listWmsAndLogistics!=null && listWmsAndLogistics.Count > 0)
                        {
                            //通知物流领料模块
                            WmsAndLogisticsService.GetInstance().UpdateWmsPreparationInfo(listWmsAndLogistics);
                            //更新作业的备料状态
                            List<int> listWipEntityId = listWmsAndLogistics.Select(P => P.wipEntityId).Distinct().ToList();
                            List<AscmWipDiscreteJobs> listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList(listWipEntityId, "ascmStatus='" + AscmWipDiscreteJobs.AscmStatusDefine.preparing + "'");
                            
                            //原有的逻辑，修改主单据的状态
                            if (listWipDiscreteJobs != null)
                                listWipDiscreteJobs.ForEach(P => P.ascmStatus = AscmWipDiscreteJobs.AscmStatusDefine.unPick);

                            //新的逻辑，修改明细的状态
                            StringBuilder strStatusWhere = new StringBuilder();
                            strStatusWhere.AppendFormat(" and wipEntityId in({0}) ", wipEntityIds);
                            strStatusWhere.AppendFormat(" and leaderId in({0}) ", strUserIds);
                            List<AscmWipDiscreteJobsStatus> listStatus = AscmWipDiscreteJobsService.Instance.GetListByStrWhere(strStatusWhere + "");
                            if (listStatus != null) 
                            {
                                listStatus.ForEach(P => P.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.unPick);
                            }

                            //执行事务
                            ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                            session.Clear();
                            using (ITransaction tx = session.BeginTransaction())
                            {
                                try
                                {
                                    if (listPreparationMainUpdate != null && listPreparationMainUpdate.Count > 0)
                                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listPreparationMainUpdate);
                                    if (listWipDiscreteJobs != null && listWipDiscreteJobs.Count > 0)
                                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listWipDiscreteJobs);
                                    if (listStatus != null && listStatus.Count > 0)
                                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listStatus);//明细状态
                                    if (listPreparationDetailUpdate != null && listPreparationDetailUpdate.Count > 0)
                                        listPreparationDetailUpdate.ForEach(P => session.Merge(P));

                                    tx.Commit();
                                    jsonObjectResult.result = true;
                                }
                                catch (Exception ex)
                                {
                                    tx.Rollback();
                                    jsonObjectResult.message = ex.Message;
                                }
                            }
                        }
                    }
                }
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }

        //作业批量执行"备料取消"操作
        public ActionResult WmsWipJobPreparationCancel(string wipEntityIds, string leaderIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            var currUserId = User.Identity.Name;
            StringBuilder strUserIds = new StringBuilder();
            if (leaderIds != null)
            {
                string[] idList = leaderIds.Split(',');
                if (idList != null && idList.Length > 0)
                {
                    foreach (var userId in idList)
                    {
                        if (userId.Trim() != "" && userId.Trim() != currUserId)
                        {
                            jsonObjectResult.result = false;
                            jsonObjectResult.message = "当前用户不是负责人，权限不够！";
                            break;
                        }

                        strUserIds.Append("'" + userId + "',");
                    }
                }
            }
            if (strUserIds.Length > 0)
            {
                strUserIds.Remove(strUserIds.Length - 1, 1);
            }

            List<AscmWhTeamUser> teamUsers = AscmWhTeamUserService.Instance.GetListForTeam(currUserId);
            List<string> teamUserIds = teamUsers == null || teamUsers.Count == 0 ? new List<string>() : teamUsers.Select(a => a.M_UserId).ToList();

            if (!string.IsNullOrEmpty(wipEntityIds))
            {
                string where = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wipEntityId in(" + wipEntityIds + ")");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "pattern='" + AscmWmsPreparationMain.PatternDefine.wipJob + "'");
                //不考虑“待备料”和“已领料”的备料单
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "status not in('" + AscmWmsPreparationMain.StatusDefine.unPrepare + "','" + AscmWmsPreparationMain.StatusDefine.picked + "')");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "locked=0");
                string hql = "from AscmWmsPreparationMain where " + where;
                List<AscmWmsPreparationMain> listPreparationMain = AscmWmsPreparationMainService.GetInstance().GetList(hql);

                //过滤为同一仓库组别的备料单
                if (listPreparationMain != null && listPreparationMain.Count > 0)
                {
                    listPreparationMain = listPreparationMain.Where(a => teamUserIds.Contains(a.createUser)).ToList();
                }
                
                if (listPreparationMain != null && listPreparationMain.Count > 0)
                {
                    string preparationMainIds = string.Join(",", listPreparationMain.Select(P => P.id).Distinct());
                    List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetListByMainIds(preparationMainIds);
                    if (listPreparationDetail != null || listPreparationDetail.Count > 0)
                    {
                        string userName = string.Empty;
                        if (User.Identity.IsAuthenticated)
                            userName = User.Identity.Name;
                        //传递给物料领料模块仓库备料取消信息
                        List<WmsAndLogistics> listWmsAndLogistics = null;
                        List<AscmWmsPreparationMain> listPreparationMainUpdate = null;
                        List<AscmWmsPreparationDetail> listPreparationDetailUpdate = null;
                        foreach (string wipEntityId in wipEntityIds.Split(','))
                        {
                            //获取可执行作业“备料取消”的备料单
                            List<AscmWmsPreparationMain> _listPreparationMain = listPreparationMain.FindAll(P => P.wipEntityId.ToString() == wipEntityId && (P.status == AscmWmsPreparationMain.StatusDefine.preparingUnPick || P.status == AscmWmsPreparationMain.StatusDefine.preparedUnPick));
                            if (_listPreparationMain == null || _listPreparationMain.Count == 0)
                                continue;
                            foreach (AscmWmsPreparationMain preparationMain in _listPreparationMain)
                            {
                                bool prepareCancel = false;
                                preparationMain.listDetail = listPreparationDetail.FindAll(P => P.mainId == preparationMain.id);
                                if (preparationMain.listDetail != null && preparationMain.listDetail.Count > 0)
                                {
                                    foreach (AscmWmsPreparationDetail preparationDetail in preparationMain.listDetail)
                                    {
                                        List<AscmWmsPreparationDetail> _listPreparationDetail = listPreparationDetail.FindAll(P => P.wipEntityId.ToString() == wipEntityId && P.materialId == preparationDetail.materialId);
                                        if (_listPreparationDetail != null && _listPreparationDetail.Count > 0)
                                        {
                                            foreach (AscmWmsPreparationDetail _preparationDetail in _listPreparationDetail)
                                            {
                                                //取消数量=传递给物料领料模块数量-已发料数量
                                                decimal cancelQuantity = _preparationDetail.sendLogisticsQuantity - _preparationDetail.issueQuantity;
                                                if (cancelQuantity > 0)
                                                {
                                                    prepareCancel = true;
                                                    listWmsAndLogistics = listWmsAndLogistics ?? new List<WmsAndLogistics>();
                                                    listPreparationDetailUpdate = listPreparationDetailUpdate ?? new List<AscmWmsPreparationDetail>();

                                                    WmsAndLogistics wmsAndLogistics = new WmsAndLogistics();
                                                    wmsAndLogistics.wipEntityId = _preparationDetail.wipEntityId;
                                                    wmsAndLogistics.materialId = _preparationDetail.materialId;
                                                    wmsAndLogistics.warehouseId = _preparationDetail.warehouseId;
                                                    wmsAndLogistics.quantity = cancelQuantity;
                                                    wmsAndLogistics.preparationString = _preparationDetail.mainId.ToString();
                                                    listWmsAndLogistics.Add(wmsAndLogistics);
                                                    //更新备料明细中传递给物料领料模块的数量
                                                    _preparationDetail.modifyUser = userName;
                                                    _preparationDetail.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                    _preparationDetail.sendLogisticsQuantity -= cancelQuantity;
                                                    listPreparationDetailUpdate.Add(_preparationDetail);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (prepareCancel)
                                {
                                    //更新备料单状态
                                    listPreparationMainUpdate = listPreparationMainUpdate ?? new List<AscmWmsPreparationMain>();
                                    preparationMain.modifyUser = userName;
                                    preparationMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    if (preparationMain.status == AscmWmsPreparationMain.StatusDefine.preparingUnPick)
                                        preparationMain.status = AscmWmsPreparationMain.StatusDefine.preparing;
                                    else if (preparationMain.status == AscmWmsPreparationMain.StatusDefine.preparedUnPick)
                                        preparationMain.status = AscmWmsPreparationMain.StatusDefine.prepared;
                                    listPreparationMainUpdate.Add(preparationMain);
                                }
                            }
                        }
                        if (listWmsAndLogistics.Count > 0)
                        {
                            //通知物流领料模块
                            bool result = WmsAndLogisticsService.GetInstance().UnDoWmsPreparationInfo(listWmsAndLogistics);
                            if (result)
                            {
                                //更新作业的备料状态
                                List<int> listWipEntityId = listWmsAndLogistics.Select(P => P.wipEntityId).Distinct().ToList();
                                List<AscmWipDiscreteJobs> listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList(listWipEntityId, "ascmStatus='" + AscmWipDiscreteJobs.AscmStatusDefine.unPick + "'");
                                
                                //旧的逻辑，保留
                                if (listWipDiscreteJobs != null)
                                    listWipDiscreteJobs.ForEach(P => P.ascmStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing);

                                //新的逻辑，把状态保存到明细
                                StringBuilder strStatusWhere = new StringBuilder();
                                strStatusWhere.AppendFormat(" and wipEntityId in({0}) ", wipEntityIds);
                                strStatusWhere.AppendFormat(" and leaderId in({0}) ", strUserIds);
                                List<AscmWipDiscreteJobsStatus> listStatus = AscmWipDiscreteJobsService.Instance.GetListByStrWhere(strStatusWhere + "");
                                if (listStatus != null)
                                {
                                    listStatus.ForEach(P => P.subStatus = AscmWipDiscreteJobs.AscmStatusDefine.preparing);
                                }

                                //执行事务
                                YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().Clear();
                                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                                {
                                    try
                                    {
                                        if (listPreparationMainUpdate != null && listPreparationMainUpdate.Count > 0)
                                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listPreparationMainUpdate);
                                        if (listWipDiscreteJobs != null && listWipDiscreteJobs.Count > 0)
                                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listWipDiscreteJobs);
                                        if (listStatus != null && listStatus.Count > 0)
                                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listStatus);//明细状态
                                        if (listPreparationDetailUpdate != null && listPreparationDetailUpdate.Count > 0)
                                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listPreparationDetailUpdate);

                                        tx.Commit();
                                        jsonObjectResult.result = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        tx.Rollback();
                                        jsonObjectResult.message = ex.Message;
                                    }
                                }
                            }
                            else
                                jsonObjectResult.message = "物流领料模块不允许执行【备料取消】";
                        }
                    }
                }
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }

		//作业批量执行"备料取消"操作
		public ActionResult WmsWipJobPreparationStop(string statusIds)
		{
			JsonObjectResult jsonObjectResult = new JsonObjectResult();

			StringBuilder strWhereIds = new StringBuilder();
			if (statusIds != null)
			{
				string[] idList = statusIds.Split(',');
				if (idList != null && idList.Length > 0)
				{
					foreach (var strId in idList)
					{
						if (strWhereIds.Length > 0) strWhereIds.Append(",");

						int val = 0;
						int.TryParse(strId, out val);

						if (val > 0) strWhereIds.Append(strId);
					}
				}


			}

			List<AscmWipDiscreteJobsStatus> listStatus = new List<AscmWipDiscreteJobsStatus>();
			string strStatusWhere = " and id in (" + strWhereIds + ") ";
			if (strWhereIds.Length > 0) 
			{
				listStatus = AscmWipDiscreteJobsService.Instance.GetListByStrWhere(strStatusWhere);
			}

			//执行事务
			ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
			session.Clear();
			using (ITransaction tx = session.BeginTransaction())
			{
				try
				{
					if (listStatus != null && listStatus.Count > 0)
					{
						foreach (var item in listStatus)
						{
							item.isStop = true;
						}

						YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listStatus);
					}

					tx.Commit();
					jsonObjectResult.result = true;
				}
				catch (Exception ex)
				{
					tx.Rollback();
					jsonObjectResult.message = ex.Message;
				}
			}
			
			return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
		}

        //作业备料监控导出excel
        public ActionResult WmsWipJobPreparationExport(string startScheduledStartDate, string endScheduledStartDate,
            string startWipEntitiesName, string endWipEntitiesName, string wipSupplyType, string queryLeader, string prepareStatus,
            string startSupplySubInventory, string endSupplySubInventory,
            string startMaterialDocNumber, string endMaterialDocNumber,
            string startScheduleGroupName, string endScheduleGroupName)
        {
            IWorkbook wb = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Sheet1");
            sheet.SetColumnWidth(0, 8 * 256);
            sheet.SetColumnWidth(1, 18 * 256);
            sheet.SetColumnWidth(2, 9 * 256);
            sheet.SetColumnWidth(3, 18 * 256);
            sheet.SetColumnWidth(4, 16 * 256);
            sheet.SetColumnWidth(5, 26 * 256);
            sheet.SetColumnWidth(6, 9 * 256);
            sheet.SetColumnWidth(7, 12 * 256);
            sheet.SetColumnWidth(8, 18 * 256);
            sheet.SetColumnWidth(9, 14 * 256);
            sheet.SetColumnWidth(10, 10 * 256);
            sheet.SetColumnWidth(11, 10 * 256);
            sheet.SetColumnWidth(12, 12 * 256);
            sheet.SetColumnWidth(13, 12 * 256);

            IRow titleRow = sheet.CreateRow(0);
            titleRow.Height = 20 * 20;
            titleRow.CreateCell(0).SetCellValue("序号");
            titleRow.CreateCell(1).SetCellValue("作业日期");
            titleRow.CreateCell(2).SetCellValue("产线");
            titleRow.CreateCell(3).SetCellValue("作业号");
            titleRow.CreateCell(4).SetCellValue("装配件编码");
            titleRow.CreateCell(5).SetCellValue("装配件描述");
            titleRow.CreateCell(6).SetCellValue("计划组");
            titleRow.CreateCell(7).SetCellValue("作业数量");
            titleRow.CreateCell(8).SetCellValue("缺料情况");
            titleRow.CreateCell(9).SetCellValue("已配料人员");
            titleRow.CreateCell(10).SetCellValue("容器数");
            titleRow.CreateCell(11).SetCellValue("校验数");
            titleRow.CreateCell(12).SetCellValue("负责人");
            titleRow.CreateCell(13).SetCellValue("备料状态");


            string fileDownloadName = HttpUtility.UrlEncode(DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", Encoding.UTF8);
            fileDownloadName = "作业备料监控明细" + fileDownloadName;

             string whereOther = "", whereBom = "";
            
            //作业状态
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "statusType in(" + AscmWipDiscreteJobs.StatusTypeDefine.yff + "," + AscmWipDiscreteJobs.StatusTypeDefine.wc + "," + AscmWipDiscreteJobs.StatusTypeDefine.wcbjf + ")");
            
            //负责人
            if (queryLeader == "ME" && User.Identity.IsAuthenticated) 
            {
                string userId =  User.Identity.Name;
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " b.leaderId='" + userId + "' ");
            }

            //作业日期
            string whereStartScheduledStartDate = "", whereEndScheduledStartDate = "";
            DateTime dtStartScheduledStartDate, dtEndScheduledStartDate;
            if (!string.IsNullOrEmpty(startScheduledStartDate) && DateTime.TryParse(startScheduledStartDate, out dtStartScheduledStartDate))
            {
                whereStartScheduledStartDate = "scheduledStartDate>='" + dtStartScheduledStartDate.ToString("yyyy-MM-dd 00:00:00") + "'";
                whereEndScheduledStartDate = "scheduledStartDate<'" + dtStartScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            }
            if (!string.IsNullOrEmpty(endScheduledStartDate) && DateTime.TryParse(endScheduledStartDate, out dtEndScheduledStartDate))
                whereEndScheduledStartDate = "scheduledStartDate<'" + dtEndScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartScheduledStartDate);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndScheduledStartDate);
            
            //作业号
            string whereWipEntity = "";
            string whereStartWipEntityName = "", whereEndWipEntityName = "";
            if (!string.IsNullOrEmpty(startWipEntitiesName))
                whereStartWipEntityName = "name='" + startWipEntitiesName + "'";
            if (!string.IsNullOrEmpty(endWipEntitiesName))
            {
                if (!string.IsNullOrEmpty(startWipEntitiesName))
                    whereStartWipEntityName = "name>='" + startWipEntitiesName + "'";
                whereEndWipEntityName = "name<='" + endWipEntitiesName + "'";
            }
            whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, whereStartWipEntityName);
            whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, whereEndWipEntityName);
            if (!string.IsNullOrEmpty(whereWipEntity))
            {
                //内、外机作业分开查询
                AscmWipEntities.WipEntityType bWipEntityType = AscmWipEntitiesService.GetInstance().GetWipEntityType(startWipEntitiesName);
                AscmWipEntities.WipEntityType eWipEntityType = AscmWipEntitiesService.GetInstance().GetWipEntityType(endWipEntitiesName);
                if (bWipEntityType == AscmWipEntities.WipEntityType.withinTheMachine && bWipEntityType == eWipEntityType)
                    whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, "instr(upper(substr(name,1,(length(name)-3))),'N',-1) > instr(upper(substr(name,1,(length(name)-3))),'W',-1)");
                else if (eWipEntityType == AscmWipEntities.WipEntityType.outsideTheMachine && bWipEntityType == eWipEntityType)
                    whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, "instr(upper(substr(name,1,(length(name)-3))),'W',-1) > instr(upper(substr(name,1,(length(name)-3))),'N',-1)");

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "a.wipEntityId in (select wipEntityId from Ascm_Wip_Entities where " + whereWipEntity + ")");
            }

            //供应类型
            string whereWipSupplyType = "";
            if (wipSupplyType == "pullType") //拉式
                whereWipSupplyType = "wipSupplyType in(" + AscmMaterialItem.WipSupplyTypeDefine.zpls + "," + AscmMaterialItem.WipSupplyTypeDefine.lsgx + ")";
            else if (wipSupplyType == "pushType") //推式
                whereWipSupplyType = "wipSupplyType in(" + AscmMaterialItem.WipSupplyTypeDefine.ts + ")";
            whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, whereWipSupplyType);
            
            //备料状态
            if (!string.IsNullOrEmpty(prepareStatus))
            {
                //if (prepareStatus == AscmWipDiscreteJobs.AscmStatusDefine.unPrint)
                //    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "ascmStatus='' or ascmStatus is null");
                //else
                //    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "ascmStatus='" + prepareStatus + "'");

                //支持"备料状态"多选
                string wherePrepareStatus = string.Empty;
                string _prepareStatus = string.Empty;
                foreach (string status in prepareStatus.Split(','))
                {
                    if (status == AscmWipDiscreteJobs.AscmStatusDefine.unPrint)
                        wherePrepareStatus = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(wherePrepareStatus, "ascmStatus='' or ascmStatus is null");
                    else
                    {
                        if (!string.IsNullOrEmpty(_prepareStatus))
                            _prepareStatus += ",";
                        _prepareStatus += "'" + status + "'";
                    }
                }
                if (!string.IsNullOrEmpty(_prepareStatus))
                    wherePrepareStatus = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(wherePrepareStatus, "ascmStatus in(" + _prepareStatus + ")");
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, wherePrepareStatus);
            }

            //计划组、车间(比较计划组名称)
            string whereScheduleGroupOther = "";
            string whereStartScheduleGroup = "", whereEndScheduleGroup = "";
            if (!string.IsNullOrEmpty(startScheduleGroupName))
                whereStartScheduleGroup = "scheduleGroupName='" + startScheduleGroupName + "'";
            if (!string.IsNullOrEmpty(endScheduleGroupName))
            {
                if (!string.IsNullOrEmpty(startScheduleGroupName))
                    whereStartScheduleGroup = "translate(schedulegroupname, '一二三四五六七八九', '123456789')>=translate('" + startScheduleGroupName + "', '一二三四五六七八九', '123456789')";
                whereEndScheduleGroup = "translate(schedulegroupname, '一二三四五六七八九', '123456789')<=translate('" + endScheduleGroupName + "', '一二三四五六七八九', '123456789')";
            }
            whereScheduleGroupOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduleGroupOther, whereStartScheduleGroup);
            whereScheduleGroupOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduleGroupOther, whereEndScheduleGroup);
            List<AscmWipScheduleGroups> listWipScheduleGroups = null;
            if (string.IsNullOrEmpty(whereScheduleGroupOther))
            {
                //电装、总装计划组
                List<string> listScheduleGroupNames = new List<string>();
                listScheduleGroupNames.AddRange(AscmWipScheduleGroups.TypeDefine.GetWipScheduleGroupNames(AscmWipScheduleGroups.TypeDefine.GA));
                listScheduleGroupNames.AddRange(AscmWipScheduleGroups.TypeDefine.GetWipScheduleGroupNames(AscmWipScheduleGroups.TypeDefine.QC));
                listWipScheduleGroups = AscmWipScheduleGroupsService.GetInstance().GetList(null, "", "", "", "scheduleGroupName in('" + string.Join("','", listScheduleGroupNames) + "')");
            }
            else
                listWipScheduleGroups = AscmWipScheduleGroupsService.GetInstance().GetList(null, "", "", "", whereScheduleGroupOther);
            if (listWipScheduleGroups != null && listWipScheduleGroups.Count > 0)
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "scheduleGroupId in(" + string.Join(",", listWipScheduleGroups.Select(P => P.scheduleGroupId)) + ")");
            
            //供应子库
            string whereStartSupplySubInventory = "", whereEndSupplySubInventory = "";
            if (!string.IsNullOrEmpty(startSupplySubInventory))
                whereStartSupplySubInventory = "supplySubinventory='" + startSupplySubInventory + "'";
            if (!string.IsNullOrEmpty(endSupplySubInventory))
            {
                if (!string.IsNullOrEmpty(startSupplySubInventory))
                    whereStartSupplySubInventory = "supplySubinventory>='" + startSupplySubInventory + "'";
                whereEndSupplySubInventory = "supplySubinventory<='" + endSupplySubInventory + "'";
            }
            whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, whereStartSupplySubInventory);
            whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, whereEndSupplySubInventory);
            
            //物料编码
            string whereInventoryItem = "";
            string whereStartInventoryItemId = "", whereEndInventoryItemId = "";
            if (startMaterialDocNumber != null && startMaterialDocNumber.Trim() != "")
                whereStartInventoryItemId = "upper(docNumber)='" + startMaterialDocNumber.Trim().ToUpper() + "'";
            if (endMaterialDocNumber != null && endMaterialDocNumber.Trim() != "")
            {
                if (startMaterialDocNumber != null && startMaterialDocNumber.Trim() != "")
                    whereStartInventoryItemId = "upper(docNumber)>='" + startMaterialDocNumber.Trim().ToUpper() + "'";
                whereEndInventoryItemId = "upper(docNumber)<='" + endMaterialDocNumber.Trim().ToUpper() + "'";
            }
            whereInventoryItem = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereInventoryItem, whereStartInventoryItemId);
            whereInventoryItem = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereInventoryItem, whereEndInventoryItemId);
            
            if (!string.IsNullOrEmpty(whereInventoryItem))
                whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, "inventoryItemId in(select id from Ascm_Material_Item where " + whereInventoryItem + ")");
            if (!string.IsNullOrEmpty(whereBom))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "a.wipEntityId in (select wipEntityId from Ascm_Wip_Require_Operat where " + whereBom + ")");

            List<AscmWipDiscreteJobs> listAscmWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetListForMonitor(null, "ascmStatus", "", "", whereOther, true, false, true, false);
            //List<AscmWipDiscreteJobs> listAscmWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList("from AscmWipDiscreteJobs where createuser='53448'");//测试
            if (listAscmWipDiscreteJobs != null)
            {
                //获取排产计划产线
                AscmWipDiscreteJobsService.GetInstance().SetScheduleProductionLine(listAscmWipDiscreteJobs);
                //缺料情况
                AscmWipDiscreteJobsService.GetInstance().GetLackOfMaterial(listAscmWipDiscreteJobs);
                //发料校验
                AscmWipDiscreteJobsService.GetInstance().GetWmsStoreIssueCheck(listAscmWipDiscreteJobs);
                //按照“备料状态〉作业号〉作业时间”排序
                var result = listAscmWipDiscreteJobs.OrderBy(P => P.ascmStatusSn);

                AscmWipDiscreteJobsService.GetInstance().SetMaterialItem(listAscmWipDiscreteJobs);
                AscmWipDiscreteJobsService.GetInstance().SetWipEntities(listAscmWipDiscreteJobs);
                AscmWipDiscreteJobsService.GetInstance().SetScheduleGroups(listAscmWipDiscreteJobs);
                if (listAscmWipDiscreteJobs != null && listAscmWipDiscreteJobs.Count > 0)
                {
                    int rowIndex = 0;
                    foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in listAscmWipDiscreteJobs)
                    {
                        rowIndex++;
                        IRow row = sheet.CreateRow(rowIndex);
                        row.Height = 20 * 20;
                        row.CreateCell(0).SetCellValue(rowIndex);
                        row.CreateCell(1).SetCellValue(ascmWipDiscreteJobs._scheduledStartDate);
                        row.CreateCell(2).SetCellValue(ascmWipDiscreteJobs.productionLine);
                        row.CreateCell(3).SetCellValue(ascmWipDiscreteJobs.ascmWipEntities_Name);
                        row.CreateCell(4).SetCellValue(ascmWipDiscreteJobs.ascmMaterialItem_DocNumber);
                        row.CreateCell(5).SetCellValue(ascmWipDiscreteJobs.ascmMaterialItem_Description);
                        row.CreateCell(6).SetCellValue(ascmWipDiscreteJobs.ascmWipScheduleGroupsName);
                        row.CreateCell(7).SetCellValue(ascmWipDiscreteJobs.netQuantity.ToString());
                        row.CreateCell(8).SetCellValue(ascmWipDiscreteJobs.jobLackOfMaterial);
                        row.CreateCell(9).SetCellValue(ascmWipDiscreteJobs.jobCompoundedPerson);
                        row.CreateCell(10).SetCellValue(ascmWipDiscreteJobs.containerQuantity.ToString());
                        row.CreateCell(11).SetCellValue(ascmWipDiscreteJobs.checkQuantity.ToString());
                        row.CreateCell(12).SetCellValue(ascmWipDiscreteJobs.leaderName);
                        row.CreateCell(13).SetCellValue(ascmWipDiscreteJobs.subStatusCn);
                    }
                }
            }

            byte[] buffer = new byte[] { };
            using (System.IO.MemoryStream stream = new MemoryStream())
            {
                wb.Write(stream);
                buffer = stream.GetBuffer();
            }
            return File(buffer, "application/vnd.ms-excel", fileDownloadName);
        }

		//备料缺料查询
        public ActionResult WmsWipJobLackOfMaterialQuery(int? id)
        {
            string whereOther = null;
            if (id.HasValue) whereOther = " wipEntityId=" + id.Value;

            AscmWipDiscreteJobs ascmWipDiscreteJobs = new AscmWipDiscreteJobs();
            List<AscmWipDiscreteJobs> listWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList(null, "", "", "", whereOther, true, true, true, true);
            if (listWipDiscreteJobs != null && listWipDiscreteJobs.Count > 0)
            {
                //获取排产计划产线
                AscmWipDiscreteJobsService.GetInstance().SetScheduleProductionLine(listWipDiscreteJobs);
                ascmWipDiscreteJobs = listWipDiscreteJobs[0];
            }

            return View(ascmWipDiscreteJobs);
        }
        public ActionResult WmsWipJobLackOfMaterialList(int id)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            //获取作业BOM
            string hql = "from AscmWipRequirementOperations";
            string where = "";
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "wipEntityId=" + id);
            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "supplySubinventory>='W112材料' and supplySubinventory<='W412材料'");
            hql += " where " + where;
            List<AscmWipRequirementOperations> listBom = AscmWipRequirementOperationsService.GetInstance().GetList(hql);
            if (listBom != null && listBom.Count > 0)
            {
                //设置物料
                AscmWipRequirementOperationsService.GetInstance().SetMaterial(listBom);
                //设置库存现有量
                AscmWipRequirementOperationsService.GetInstance().SetOnhandQuantity(listBom);
                List<AscmWipRequirementOperations> lackOfMaterialList = listBom.FindAll(P => (P.ascmMaterialItem_dMtlCategoryStatus == MtlCategoryStatusDefine.mixStock || P.ascmMaterialItem_zMtlCategoryStatus == MtlCategoryStatusDefine.mixStock) && P.ascmPreparedQuantity < P.requiredQuantity);
                if (lackOfMaterialList != null && lackOfMaterialList.Count > 0)
                    lackOfMaterialList.OrderBy(P => P.ascmMaterialItem_DocNumber).ToList().ForEach(P => jsonDataGridResult.rows.Add(P));
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 导出
        public ActionResult WmsPreparationExportToPdf(string mainIdList, bool bSingle, string pattern)
        {
            try
            {
                if (!string.IsNullOrEmpty(mainIdList))
                {
                    string userName = string.Empty;
                    if (User.Identity.IsAuthenticated)
                        userName = User.Identity.Name;

                    string fileName = string.Empty;
                    string templatePath = string.Empty;
                    if (pattern == AscmWmsPreparationMain.PatternDefine.wipRequire)
                    {
                        templatePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\_data\AscmCkblDemo2.doc");
                        fileName = "需求备料单";
                    }
                    else if (pattern == AscmWmsPreparationMain.PatternDefine.wipJob)
                    {
                        templatePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\_data\AscmCkblDemo3.doc");
                        fileName = "作业备料单";
                    }
                    else if (pattern == "wipReturn")
                    {
                        templatePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\_data\AscmCkblDemo4.doc");
                        fileName = "作业退料单";
                    }
                    Aspose.Words.Document doc = new Aspose.Words.Document(templatePath);
                    HttpResponse httpResponse = System.Web.HttpContext.Current.Response;

                    string sql = "from AscmWmsPreparationMain where id in (" + mainIdList + ")";
                    List<AscmWmsPreparationMain> listMain = AscmWmsPreparationMainService.GetInstance().GetList(sql);
                    if (pattern.Trim() == AscmWmsPreparationMain.PatternDefine.wipJob || pattern.Trim() == "wipReturn")
                    {
                        AscmWmsPreparationMainService.GetInstance().SetWipDiscreteJobs(listMain);
                    }
                    sql = "from AscmWmsPreparationDetail where mainId in (" + mainIdList + ")";
                    List<AscmWmsPreparationDetail> listDetail = AscmWmsPreparationDetailService.GetInstance().GetList(sql);
                    AscmWmsPreparationDetailService.GetInstance().SetWipDiscreteJobs(listDetail);
                    AscmWmsPreparationDetailService.GetInstance().SetMaterial(listDetail);
                    AscmWmsPreparationDetailService.GetInstance().SetWipEntities(listDetail);
                    AscmWmsPreparationDetailService.GetInstance().SetWarelocationDocNumber(listDetail);
                    AscmWmsPreparationDetailService.GetInstance().SetOnhandQuantity(listDetail);
                    AscmWmsPreparationDetailService.GetInstance().SetRecSupplyQuantity(listDetail);
                    List<AscmWipDiscreteJobs> listAscmWipDiscreteJobs = null;

                    if (listMain == null)
                        throw new Exception("没有找到备料单");

                    foreach (AscmWmsPreparationMain ascmWmsPreparationMain in listMain)
                    {
                        //ascmWmsPreparationMain.ascmTitle = "备料单";
                        ascmWmsPreparationMain.ascmDate = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
                        ascmWmsPreparationMain.printer = userName;

                        System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                        if (ascmWmsPreparationMain.docNumber != null)
                        {
                            #region Code128
                            MideaAscm.Code.BarCode128 barCode128 = new Code.BarCode128();
                            barCode128.TitleFont = new System.Drawing.Font("宋体", 10);
                            barCode128.HeightImage = 45;
                            barCode128.WidthMultiple = Convert.ToByte(0.7);
                            System.Drawing.Bitmap bitmap = barCode128.GetCodeImage(ascmWmsPreparationMain.docNumber, ascmWmsPreparationMain.docNumber, Code.BarCode128.Encode.Code128B);
                            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Gif);
                            bitmap.Dispose();
                            #endregion
                        }
                        ascmWmsPreparationMain.ascmBarcode = memoryStream.ToArray();
                        if (listDetail != null && listDetail.Count() > 0)
                        {
                            ascmWmsPreparationMain.warehouseIdStart = listDetail.Where(item => item.mainId == ascmWmsPreparationMain.id).OrderBy(item => item.warehouseId).First().warehouseId;
                            ascmWmsPreparationMain.warehouseIdEnd = listDetail.Where(item => item.mainId == ascmWmsPreparationMain.id).OrderBy(item => item.warehouseId).Last().warehouseId;
                            ascmWmsPreparationMain.wipSupplyTypeCn = listDetail.Find(item => item.mainId == ascmWmsPreparationMain.id).ascmMaterialItem.wipSupplyTypeCn;

                            if (pattern.Trim() == AscmWmsPreparationMain.PatternDefine.wipRequire)
                            {
                                var listWhere = listDetail.Where(item => item.mainId == ascmWmsPreparationMain.id);
                                ascmWmsPreparationMain.billNoStart = listDetail.Where(item => item.mainId == ascmWmsPreparationMain.id).OrderBy(item => item.wipEntityName).First().wipEntityName;
                                ascmWmsPreparationMain.billNoEnd = listDetail.Where(item => item.mainId == ascmWmsPreparationMain.id).OrderBy(item => item.wipEntityName).Last().wipEntityName;
                                ascmWmsPreparationMain.materialDocNumberStart = listDetail.Where(item => item.mainId == ascmWmsPreparationMain.id).OrderBy(item => item.materialDocNumber).First().materialDocNumber;
                                ascmWmsPreparationMain.materialDocNumberEnd = listDetail.Where(item => item.mainId == ascmWmsPreparationMain.id).OrderBy(item => item.materialDocNumber).Last().materialDocNumber;
                                ascmWmsPreparationMain.jobScheduleGroupsStart = listDetail.Where(item => item.mainId == ascmWmsPreparationMain.id).OrderBy(item => item.jobScheduleGroupsName).First().jobScheduleGroupsName;
                                ascmWmsPreparationMain.jobScheduleGroupsEnd = listDetail.Where(item => item.mainId == ascmWmsPreparationMain.id).OrderBy(item => item.jobScheduleGroupsName).Last().jobScheduleGroupsName;
                                ascmWmsPreparationMain.jobProductionLineStart = listDetail.Where(item => item.mainId == ascmWmsPreparationMain.id).OrderBy(item => item.jobProductionLine).FirstOrDefault().jobProductionLine;//.First().jobProductionLine;
                                ascmWmsPreparationMain.jobProductionLineEnd = listDetail.Where(item => item.mainId == ascmWmsPreparationMain.id).OrderBy(item => item.jobProductionLine).Last().jobProductionLine;

                                var vbillIds = listDetail.Where(item => item.mainId == ascmWmsPreparationMain.id).Select(item => item.wipEntityId).Distinct();
                                if (vbillIds != null)
                                {
                                    string billIds = string.Empty;
                                    foreach (int billId in vbillIds)
                                    {
                                        if (!string.IsNullOrEmpty(billIds))
                                            billIds += ",";
                                        billIds += "'" + billId + "'";
                                    }
                                    if (!string.IsNullOrEmpty(billIds))
                                    {
                                        string whereOther = " id in (" + billIds + ")";
                                        listAscmWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList(null, "", "", "", whereOther);
                                        ascmWmsPreparationMain.scheduledStartDateStart = listAscmWipDiscreteJobs.OrderBy(item => item._scheduledStartDate).First()._scheduledStartDate;
                                        ascmWmsPreparationMain.scheduledStartDateEnd = listAscmWipDiscreteJobs.OrderBy(item => item._scheduledStartDate).Last()._scheduledStartDate;
                                    }
                                }

                                #region 需求备料 对同种物料进行合并
                                List<AscmWmsPreparationDetail> listVar = new List<AscmWmsPreparationDetail>();
                                var groupByMaterial = listDetail.GroupBy(P => P.materialId);
                                foreach (IGrouping<int, AscmWmsPreparationDetail> ig in groupByMaterial)
                                {
                                    AscmWmsPreparationDetail _ascmWmsPreparationDetail = ig.First();
                                    AscmWmsPreparationDetail ascmWmsPreparationDetail = new AscmWmsPreparationDetail();
                                    ascmWmsPreparationDetail.materialId = ig.Key;
                                    ascmWmsPreparationDetail.ascmMaterialItem = _ascmWmsPreparationDetail.ascmMaterialItem;
                                    ascmWmsPreparationDetail.warehouseId = _ascmWmsPreparationDetail.warehouseId;
                                    ascmWmsPreparationDetail.wipSupplyType = _ascmWmsPreparationDetail.wipSupplyType;
                                    ascmWmsPreparationDetail.mainId = _ascmWmsPreparationDetail.mainId;
                                    ascmWmsPreparationDetail.wipEntityId = _ascmWmsPreparationDetail.wipEntityId;
                                    ascmWmsPreparationDetail.planQuantity = ig.Sum(P => P.planQuantity);
                                    ascmWmsPreparationDetail.issueQuantity = ig.Sum(P => P.issueQuantity);
                                    listVar.Add(ascmWmsPreparationDetail);
                                }
                                listDetail = listVar;
                                #endregion
                            }
                        }
                    }
                    fileName += DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";

                    DataSet dataSet = new DataSet();
                    DataTable dtMain = ToDataTable(listMain);
                    //明细按物料编码排序
                    listDetail = listDetail.OrderBy(P => P.materialDocNumber).ToList();
                    DataTable dtDetial = ToDataTable(listDetail);
                    dtMain.TableName = "WmsPreparation";
                    dtDetial.TableName = "WmsPreparationDetail";
                    dataSet.Tables.Add(dtMain);
                    dataSet.Tables.Add(dtDetial);

                    dataSet.Relations.Add(new DataRelation("WmsPreparationDetail", dtMain.Columns["id"], dtDetial.Columns["mainId"], false));
                    DocumentBuilder builder = new DocumentBuilder(doc);
                    doc.MailMerge.FieldMergingCallback = new HandleMergeField(bSingle, dtMain.Rows.Count, builder);
                    doc.MailMerge.ExecuteWithRegions(dataSet);

                    if (!bSingle)
                    {
                        Aspose.Words.Layout.LayoutCollector layoutCollector = new Aspose.Words.Layout.LayoutCollector(doc);
                        NodeCollection nodes = doc.GetChildNodes(NodeType.Table, true);
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            builder.MoveToMergeField("ascmPageBreak", true, true);
                            if (i < nodes.Count - 1)
                            {
                                Node node = nodes[i];
                                int startPageIndex = layoutCollector.GetStartPageIndex(node);
                                int endPageIndex = layoutCollector.GetEndPageIndex(node);
                                bool isPageBreak = startPageIndex < endPageIndex;
                                if (!isPageBreak)
                                {
                                    node = nodes[i + 1];
                                    int nextStartPageIndex = layoutCollector.GetStartPageIndex(node);
                                    int nextEndPageIndex = layoutCollector.GetEndPageIndex(node);
                                    isPageBreak = endPageIndex == nextStartPageIndex && nextStartPageIndex < nextEndPageIndex;
                                }
                                if (isPageBreak)
                                {
                                    builder.InsertBreak(BreakType.PageBreak);
                                }
                                else
                                {
                                    int j = 0;
                                    while (j < 18)
                                    {
                                        j++;
                                        builder.InsertBreak(BreakType.ParagraphBreak);
                                    }
                                }
                            }
                            doc.UpdatePageLayout();
                            layoutCollector = new Aspose.Words.Layout.LayoutCollector(doc);
                        }
                    }
                    doc.Save(httpResponse, fileName, Aspose.Words.ContentDisposition.Attachment, null);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }
        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
        public static DataTable ToDataTable<T>(List<T> list)
        {
            IList<T> ilist = YnBaseClass2.Helper.ConvertHelper.ConvertToGenericList<T>(list);
            return ToDataTable<T>(ilist);
        }
        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="propertyName">需要返回的列的列名</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);

            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
        /// <summary>
        /// Please note that the functionality provided by attaching the event handler MergeFieldEventHandler 
        /// has been changed to implementing the IFieldMergingCallback interface in version 9.2 up.
        /// </summary>
        private class HandleMergeField : IFieldMergingCallback
        {
            private bool isPageBreak;
            private int count;
            private DocumentBuilder builder;

            internal HandleMergeField(bool isPageBreak, int count, DocumentBuilder builder)
            {
                this.isPageBreak = isPageBreak;
                this.count = count;
                this.builder = builder;
            }

            /// <summary>
            /// Called for every merge field encountered in the document.
            /// We can either return some data to the mail merge engine or do something
            /// else with the document. In this case we choose to modify cell formatting.
            /// </summary>
            public void FieldMerging(FieldMergingArgs e)
            {
                if (isPageBreak && e.FieldName.Equals("ascmPageBreak"))
                {
                    builder.MoveToMergeField(e.FieldName, true, true);
                    //如果是最后一个域则不用增加分页
                    if (e.RecordIndex < count - 1)
                        builder.InsertBreak(BreakType.PageBreak);
                }
            }

            public void ImageFieldMerging(ImageFieldMergingArgs e)
            {
                //Do Nothing
                if (e.FieldName.Equals("ascmBarcode"))
                {

                }
            }
        }
        /// <summary>  
        /// 通过DataRow 填充实体  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="dr"></param>  
        /// <returns></returns>  
        public static T GetModelByDataRow<T>(System.Data.DataRow dr) where T : new()
        {
            T model = new T();
            foreach (PropertyInfo pInfo in model.GetType().GetProperties())
            {
                object val = GetValueByColumnName(dr, pInfo.Name);
                if (pInfo.CanWrite)
                    pInfo.SetValue(model, val, null);
            }
            return model;
        }
        //返回DataRow 中对应的列的值。  
        public static object GetValueByColumnName(System.Data.DataRow dr, string columnName)
        {
            if (dr.Table.Columns.IndexOf(columnName) >= 0)
            {
                if (dr[columnName] == DBNull.Value)
                    return null;
                return dr[columnName];
            }
            return null;
        }
        #endregion

        #region 物料领料
        public ActionResult WmsRequisitionIndex()
        {
            return View();
        }
        public ActionResult WmsRequisitionList(int? page, int? rows, string sort, string order, string queryWord,
            string startCreateTime, string endCreateTime, string startWipEntityName, string endWipEntityName,
			string startScheduledStartDate, string endScheduledStartDate, string startDocNumber, string endDocNumber, string returnCode)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "";
                //领料单生成日期
                string whereStartCreateTime = "", whereEndCreateTime = "";
                DateTime dtStartCreateTime, dtEndCreateTime;
                if (!string.IsNullOrEmpty(startCreateTime))
                {
                    if (DateTime.TryParse(startCreateTime, out dtStartCreateTime))
                    {
                        whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                        whereEndCreateTime = "createTime<'" + dtStartCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                    }
                }
                if (!string.IsNullOrEmpty(endCreateTime))
                {
                    if (DateTime.TryParse(endCreateTime, out dtEndCreateTime))
                    {
                        whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                    }
                }
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);
                //作业号
                string whereStartWipEntitiesName = "", whereEndWipEntitiesName = "";
                if (!string.IsNullOrEmpty(startWipEntityName))
                {
                    whereStartWipEntitiesName = "wipEntityId=(select wipEntityId from AscmWipEntities where name='" + startWipEntityName + "')";
                }
                if (!string.IsNullOrEmpty(endWipEntityName))
                {
                    if (!string.IsNullOrEmpty(startWipEntityName))
                        whereStartWipEntitiesName = "wipEntityId in(select wipEntityId from AscmWipEntities where name>='" + startWipEntityName + "')";
                    whereEndWipEntitiesName = "wipEntityId in(select wipEntityId from AscmWipEntities where name<='" + endWipEntityName + "')";
                }
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartWipEntitiesName);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndWipEntitiesName);
                //作业日期
                string whereStartScheduledStartDate = "", whereEndScheduledStartDate = "";
                DateTime dtStartScheduledStartDate, dtEndScheduledStartDate;
                if (!string.IsNullOrEmpty(startScheduledStartDate))
                {
                    if (DateTime.TryParse(startScheduledStartDate, out dtStartScheduledStartDate))
                    {
                        whereStartScheduledStartDate = "wipEntityId in(select wipEntityId from AscmWipDiscreteJobs where scheduledStartDate>='" + dtStartScheduledStartDate.ToString("yyyy-MM-dd 00:00:00") + "')";
                        whereEndScheduledStartDate = "wipEntityId in(select wipEntityId from AscmWipDiscreteJobs where scheduledStartDate<'" + dtStartScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "')";
                    }
                }
                if (!string.IsNullOrEmpty(endScheduledStartDate))
                {
                    if (DateTime.TryParse(endScheduledStartDate, out dtEndScheduledStartDate))
                    {
                        whereEndScheduledStartDate = "wipEntityId in(select wipEntityId from AscmWipDiscreteJobs where scheduledStartDate<'" + dtEndScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "')";
                    }
                }
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartScheduledStartDate);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndScheduledStartDate);
                //领料单号
                string whereStartDocNumber = "", whereEndDocNumber = "";
                if (!string.IsNullOrEmpty(startDocNumber))
                {
                    whereStartDocNumber = "docNumber='" + startDocNumber + "'";
                }
                if (!string.IsNullOrEmpty(endDocNumber))
                {
                    if (!string.IsNullOrEmpty(startDocNumber))
                        whereStartDocNumber = "docNumber>='" + startDocNumber + "'";
                    whereEndDocNumber = "docNumber<='" + endDocNumber + "'";
                }
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartDocNumber);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndDocNumber);
				if (!string.IsNullOrEmpty(returnCode))
				{
					if (returnCode == "0")
					{
						whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "returnCode='0'");
					}
					else
					{
						whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "returnCode is not null and returnCode!='0'");
					}
				}

                List<AscmWmsMtlRequisitionMain> list = AscmWmsMtlRequisitionMainService.GetInstance().GetList(ynPage, sort, order, queryWord, whereOther);
                if (list != null)
                {
                    AscmWmsMtlRequisitionMainService.GetInstance().SetWipDiscreteJobs(list);
                    AscmWmsMtlRequisitionMainService.GetInstance().SetPreparationDocNumbers(list);
                    list = list.OrderBy(item => item.modifyTime).ToList();
                    foreach (AscmWmsMtlRequisitionMain ascmWmsMtlRequisitionMain in list)
                    {
                        jsonDataGridResult.rows.Add(ascmWmsMtlRequisitionMain);
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
        public ActionResult WmsRequisitionView(int? id)
        {
            AscmWmsMtlRequisitionMain ascmWmsMtlRequisitionMain = null;
            try
            {
                if (id.HasValue)
                {
                    ascmWmsMtlRequisitionMain = AscmWmsMtlRequisitionMainService.GetInstance().Get(id.Value);
                }
                ascmWmsMtlRequisitionMain = ascmWmsMtlRequisitionMain ?? new AscmWmsMtlRequisitionMain();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ascmWmsMtlRequisitionMain);
        }
        public ActionResult WmsRequisitionDetailView(int? id)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (id.HasValue)
                {
                    List<AscmWmsMtlRequisitionDetail> listAscmWmsMtlRequisitionDetail = AscmWmsMtlRequisitionDetailService.GetInstance().GetList2("mainId=" + id.Value);
                    if (listAscmWmsMtlRequisitionDetail != null && listAscmWmsMtlRequisitionDetail.Count > 0)
                    {
                        AscmWmsMtlRequisitionDetailService.GetInstance().SetMaterial(listAscmWmsMtlRequisitionDetail);
                        AscmWmsMtlRequisitionDetailService.GetInstance().SetWarelocation(listAscmWmsMtlRequisitionDetail);
                        //按物料编码排序
                        listAscmWmsMtlRequisitionDetail = listAscmWmsMtlRequisitionDetail.OrderBy(P => P.materialDocNumber).ToList();
                        foreach (AscmWmsMtlRequisitionDetail ascmWmsMtlRequisitionDetail in listAscmWmsMtlRequisitionDetail)
                        {
                            jsonDataGridResult.rows.Add(ascmWmsMtlRequisitionDetail);
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
        public ActionResult WmsMtlRequisitionOk(string requisitionMainIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (string.IsNullOrEmpty(requisitionMainIds))
                    throw new Exception("系统错误：参数传值错误");

                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;

                string hql = "from AscmWmsMtlRequisitionMain where id in(" + requisitionMainIds + ") and status='" + AscmWmsMtlRequisitionMain.StatusDefine.failed + "'";
                List<AscmWmsMtlRequisitionMain> listAscmWmsMtlRequisitionMain = AscmWmsMtlRequisitionMainService.GetInstance().GetList(hql);
                if (listAscmWmsMtlRequisitionMain == null || listAscmWmsMtlRequisitionMain.Count == 0)
                    throw new Exception("系统错误：获取领料单失败");
                List<AscmWmsMtlRequisitionDetail> listAscmWmsMtlRequisitionDetail = AscmWmsMtlRequisitionDetailService.GetInstance().GetList2("mainId in(" + requisitionMainIds + ")");
                if (listAscmWmsMtlRequisitionDetail == null || listAscmWmsMtlRequisitionDetail.Count == 0)
                    throw new Exception("系统错误：获取领料单明细失败");

                //与MES交互日志列表
                List<AscmMesInteractiveLog> listAscmMesInteractiveLog = new List<AscmMesInteractiveLog>();
                foreach (AscmWmsMtlRequisitionMain ascmWmsMtlRequisitionMain in listAscmWmsMtlRequisitionMain)
                {
                    AscmMesInteractiveLog ascmMesInteractiveLog = new AscmMesInteractiveLog();
                    ascmMesInteractiveLog.billId = ascmWmsMtlRequisitionMain.id;
                    ascmMesInteractiveLog.docNumber = ascmWmsMtlRequisitionMain.docNumber;
                    ascmMesInteractiveLog.billType = AscmMesInteractiveLog.BillTypeDefine.mtlRequisition;
                    ascmMesInteractiveLog.createUser = userName;
                    ascmMesInteractiveLog.modifyUser = userName;
                    ascmMesInteractiveLog.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmMesInteractiveLog.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    listAscmMesInteractiveLog.Add(ascmMesInteractiveLog);

                    ascmWmsMtlRequisitionMain.status = AscmWmsMtlRequisitionMain.StatusDefine.succeeded;
                    ascmWmsMtlRequisitionMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmWmsMtlRequisitionMain.modifyUser = userName;

                    List<AscmWmsMtlRequisitionDetail> findAscmWmsMtlRequisitionDetails = listAscmWmsMtlRequisitionDetail.Where(P => P.mainId == ascmWmsMtlRequisitionMain.id).ToList();

                    //执行事务
                    YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().Clear();
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            AscmMesService.GetInstance().DoMtlRequisition(ascmWmsMtlRequisitionMain, findAscmWmsMtlRequisitionDetails, userName, ascmMesInteractiveLog);
                            if (ascmMesInteractiveLog.returnCode == "0")
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmWmsMtlRequisitionMain);
                                ascmMesInteractiveLog.returnMessage = "领料成功";
                            }

                            tx.Commit();
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();
                            ascmMesInteractiveLog.returnMessage = ex.Message;
                        }
                    }
                }

                string maxLogIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_mes_interactive_log_id", "", "", 10, listAscmMesInteractiveLog.Count);
                int maxLogId = Convert.ToInt32(maxLogIdKey);
                string errorMsg = string.Empty;
                foreach (AscmMesInteractiveLog ascmMesInteractiveLog in listAscmMesInteractiveLog)
                {
                    ascmMesInteractiveLog.id = maxLogId++;
                    if (!string.IsNullOrEmpty(ascmMesInteractiveLog.returnMessage))
                    {
                        errorMsg += string.Format("<li>作业领料<font color='red'>{0}</font>：{1}</li>", ascmMesInteractiveLog.docNumber, ascmMesInteractiveLog.returnMessage);
                    }
                }
                if (!string.IsNullOrEmpty(errorMsg))
                    errorMsg = string.Format("<ul>{0}</ul>", errorMsg);
                //保存与MES的交互日志
                AscmMesInteractiveLogService.GetInstance().Save(listAscmMesInteractiveLog);
                jsonObjectResult.result = true;
                jsonObjectResult.message = errorMsg;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        //领料查询导出excel
        public ActionResult WmsRequisitionExport(string startCreateTime, string endCreateTime, string startWipEntityName, string endWipEntityName,
            string startScheduledStartDate, string endScheduledStartDate, string startDocNumber, string endDocNumber, string returnCode)
        {
            IWorkbook wb = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Sheet1");
            sheet.SetColumnWidth(0, 8 * 256);
            sheet.SetColumnWidth(1, 18 * 256);
            sheet.SetColumnWidth(2, 18 * 256);
            sheet.SetColumnWidth(3, 18 * 256);
            sheet.SetColumnWidth(4, 15 * 256);
            sheet.SetColumnWidth(5, 18 * 256);
            sheet.SetColumnWidth(6, 12 * 256);
            sheet.SetColumnWidth(7, 12 * 256);
            sheet.SetColumnWidth(8, 18 * 256);
            sheet.SetColumnWidth(9, 14 * 256);
            sheet.SetColumnWidth(10, 14 * 256);
            sheet.SetColumnWidth(11, 18 * 256);

            IRow titleRow = sheet.CreateRow(0);
            titleRow.Height = 20 * 20;
            titleRow.CreateCell(0).SetCellValue("序号");
            titleRow.CreateCell(1).SetCellValue("领料单号");
            titleRow.CreateCell(2).SetCellValue("作业号");
            titleRow.CreateCell(3).SetCellValue("作业日期");
            titleRow.CreateCell(4).SetCellValue("装配件编码");
            titleRow.CreateCell(5).SetCellValue("备料单");
            titleRow.CreateCell(6).SetCellValue("计划组");
            titleRow.CreateCell(7).SetCellValue("产线");
            titleRow.CreateCell(8).SetCellValue("单据日期");
            titleRow.CreateCell(9).SetCellValue("状态");
            titleRow.CreateCell(10).SetCellValue("上传状态");
            titleRow.CreateCell(11).SetCellValue("上传日期");

            string fileDownloadName = HttpUtility.UrlEncode(DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", Encoding.UTF8);
            fileDownloadName = "领料查询明细" + fileDownloadName;


            string whereOther = "";
            //领料单生成日期
            string whereStartCreateTime = "", whereEndCreateTime = "";
            DateTime dtStartCreateTime, dtEndCreateTime;
            if (!string.IsNullOrEmpty(startCreateTime))
            {
                if (DateTime.TryParse(startCreateTime, out dtStartCreateTime))
                {
                    whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                    whereEndCreateTime = "createTime<'" + dtStartCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                }
            }
            if (!string.IsNullOrEmpty(endCreateTime))
            {
                if (DateTime.TryParse(endCreateTime, out dtEndCreateTime))
                {
                    whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                }
            }
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);
            //作业号
            string whereStartWipEntitiesName = "", whereEndWipEntitiesName = "";
            if (!string.IsNullOrEmpty(startWipEntityName))
            {
                whereStartWipEntitiesName = "wipEntityId=(select wipEntityId from AscmWipEntities where name='" + startWipEntityName + "')";
            }
            if (!string.IsNullOrEmpty(endWipEntityName))
            {
                if (!string.IsNullOrEmpty(startWipEntityName))
                    whereStartWipEntitiesName = "wipEntityId in(select wipEntityId from AscmWipEntities where name>='" + startWipEntityName + "')";
                whereEndWipEntitiesName = "wipEntityId in(select wipEntityId from AscmWipEntities where name<='" + endWipEntityName + "')";
            }
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartWipEntitiesName);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndWipEntitiesName);
            //作业日期
            string whereStartScheduledStartDate = "", whereEndScheduledStartDate = "";
            DateTime dtStartScheduledStartDate, dtEndScheduledStartDate;
            if (!string.IsNullOrEmpty(startScheduledStartDate))
            {
                if (DateTime.TryParse(startScheduledStartDate, out dtStartScheduledStartDate))
                {
                    whereStartScheduledStartDate = "wipEntityId in(select wipEntityId from AscmWipDiscreteJobs where scheduledStartDate>='" + dtStartScheduledStartDate.ToString("yyyy-MM-dd 00:00:00") + "')";
                    whereEndScheduledStartDate = "wipEntityId in(select wipEntityId from AscmWipDiscreteJobs where scheduledStartDate<'" + dtStartScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "')";
                }
            }
            if (!string.IsNullOrEmpty(endScheduledStartDate))
            {
                if (DateTime.TryParse(endScheduledStartDate, out dtEndScheduledStartDate))
                {
                    whereEndScheduledStartDate = "wipEntityId in(select wipEntityId from AscmWipDiscreteJobs where scheduledStartDate<'" + dtEndScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "')";
                }
            }
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartScheduledStartDate);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndScheduledStartDate);
            //领料单号
            string whereStartDocNumber = "", whereEndDocNumber = "";
            if (!string.IsNullOrEmpty(startDocNumber))
            {
                whereStartDocNumber = "docNumber='" + startDocNumber + "'";
            }
            if (!string.IsNullOrEmpty(endDocNumber))
            {
                if (!string.IsNullOrEmpty(startDocNumber))
                    whereStartDocNumber = "docNumber>='" + startDocNumber + "'";
                whereEndDocNumber = "docNumber<='" + endDocNumber + "'";
            }
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartDocNumber);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndDocNumber);
            if (!string.IsNullOrEmpty(returnCode))
            {
                if (returnCode == "0")
                {
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "returnCode='0'");
                }
                else
                {
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "returnCode is not null and returnCode!='0'");
                }
            }

            List<AscmWmsMtlRequisitionMain> listAscmWmsMtlRequisitionMain = AscmWmsMtlRequisitionMainService.GetInstance().GetList(null, "", "", "", whereOther);
            if (listAscmWmsMtlRequisitionMain != null)
            {
                AscmWmsMtlRequisitionMainService.GetInstance().SetWipDiscreteJobs(listAscmWmsMtlRequisitionMain);
                AscmWmsMtlRequisitionMainService.GetInstance().SetPreparationDocNumbers(listAscmWmsMtlRequisitionMain);
                listAscmWmsMtlRequisitionMain = listAscmWmsMtlRequisitionMain.OrderBy(item => item.modifyTime).ToList();

                if (listAscmWmsMtlRequisitionMain != null && listAscmWmsMtlRequisitionMain.Count > 0)
                {
                    int rowIndex = 0;
                    foreach (AscmWmsMtlRequisitionMain ascmWmsMtlRequisitionMain in listAscmWmsMtlRequisitionMain)
                    {
                        rowIndex++;
                        IRow row = sheet.CreateRow(rowIndex);
                        row.Height = 20 * 20;
                        row.CreateCell(0).SetCellValue(rowIndex);
                        row.CreateCell(1).SetCellValue(ascmWmsMtlRequisitionMain.docNumber);
                        row.CreateCell(2).SetCellValue(ascmWmsMtlRequisitionMain.jobWipEntityName);
                        row.CreateCell(3).SetCellValue(ascmWmsMtlRequisitionMain.jobScheduledStartDate);
                        row.CreateCell(4).SetCellValue(ascmWmsMtlRequisitionMain.jobPrimaryItemDoc);
                        row.CreateCell(5).SetCellValue(ascmWmsMtlRequisitionMain.preparationDocNumbers);
                        row.CreateCell(6).SetCellValue(ascmWmsMtlRequisitionMain.jobScheduleGroupsName);
                        row.CreateCell(7).SetCellValue(ascmWmsMtlRequisitionMain.jobProductionLine);
                        row.CreateCell(8).SetCellValue(ascmWmsMtlRequisitionMain.createTimeCn);
                        row.CreateCell(9).SetCellValue(ascmWmsMtlRequisitionMain.statusCn);
                        row.CreateCell(10).SetCellValue(ascmWmsMtlRequisitionMain.uploadStatusCn);
                        row.CreateCell(11).SetCellValue(ascmWmsMtlRequisitionMain.uploadTimeShow);
                    }
                }
            }

            byte[] buffer = new byte[] { };
            using (System.IO.MemoryStream stream = new MemoryStream())
            {
                wb.Write(stream);
                buffer = stream.GetBuffer();
            }
            return File(buffer, "application/vnd.ms-excel", fileDownloadName);
        }
        #endregion

        #region 供应商退货
        public ActionResult WmsBackInvoiceEdit(int? id)
        {
            AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain = new AscmWmsBackInvoiceMain();
            try
            {
                if (id.HasValue)
                {
                    ascmWmsBackInvoiceMain = AscmWmsBackInvoiceMainService.GetInstance().Get(id.Value);
                    YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight();
                    ynWebRight.rightAdd = false;
                    ynWebRight.rightDelete = false;
                    ynWebRight.rightEdit = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ascmWmsBackInvoiceMain);
        }
        [HttpPost]
        public ActionResult WmsBackInvoiceSave(AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain_Model, int? id, string detailJson)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(ascmWmsBackInvoiceMain_Model.manualDocNumber))
                    throw new Exception("必须输入手工退货单号");
                if (ascmWmsBackInvoiceMain_Model.reasonId == 0)
                    throw new Exception("必须选择退货原因");
                //if (ascmWmsBackInvoiceMain_Model.batSumMainId == 0)
                //    throw new Exception("必须选择供应商送货合单");
                if (ascmWmsBackInvoiceMain_Model.supplierId == 0)
                    throw new Exception("系统错误：缺少供应商信息！");
                if (string.IsNullOrEmpty(detailJson))
                    throw new Exception("系统错误：退货明细参数不能为NULL或空");
                List<AscmWmsBackInvoiceDetail> listAscmWmsBackInvoiceDetail_Model = JsonConvert.DeserializeObject<List<AscmWmsBackInvoiceDetail>>(detailJson);
                if (listAscmWmsBackInvoiceDetail_Model == null)
                    throw new Exception("系统错误：退货明细参数序列化错误");

                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;
                #endregion

                #region 主表
                AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain = null;
                List<AscmWmsBackInvoiceDetail> listAscmWmsBackInvoiceDetail = null;
                if (id.HasValue)
                {
                    ascmWmsBackInvoiceMain = AscmWmsBackInvoiceMainService.GetInstance().Get(id.Value);
                    listAscmWmsBackInvoiceDetail = AscmWmsBackInvoiceDetailService.GetInstance().GetListByMainId(id.Value);
                }
                else
                {
                    ascmWmsBackInvoiceMain = new AscmWmsBackInvoiceMain();
                    ascmWmsBackInvoiceMain.organizationId = 775;
                    ascmWmsBackInvoiceMain.createUser = userName;
                    ascmWmsBackInvoiceMain.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsBackInvoiceMain");
                    ascmWmsBackInvoiceMain.id = ++maxId;
                    //获取MES闭环单号
                    //ascmWmsBackInvoiceMain.docNumber = "aa";
                    ascmWmsBackInvoiceMain.docNumber = AscmMesService.GetInstance().GetMesRejectBillNo();
                    if (string.IsNullOrEmpty(ascmWmsBackInvoiceMain.docNumber))
                        throw new Exception("系统错误：从MES获取供应商退货闭环单号失败");
                    ascmWmsBackInvoiceMain.status = AscmWmsBackInvoiceMain.StatusDefine.unVerify;
                }
                if (ascmWmsBackInvoiceMain == null)
                    throw new Exception("提交供应商退货单失败！");

                ascmWmsBackInvoiceMain.modifyUser = userName;
                ascmWmsBackInvoiceMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ascmWmsBackInvoiceMain.manualDocNumber = ascmWmsBackInvoiceMain_Model.manualDocNumber.Trim();
                ascmWmsBackInvoiceMain.reasonId = ascmWmsBackInvoiceMain_Model.reasonId;
                ascmWmsBackInvoiceMain.warehouseId = ascmWmsBackInvoiceMain_Model.warehouseId;
                ascmWmsBackInvoiceMain.supplierId = ascmWmsBackInvoiceMain_Model.supplierId;
                ascmWmsBackInvoiceMain.accountStatus = ascmWmsBackInvoiceMain_Model.accountStatus;
                ascmWmsBackInvoiceMain.responsiblePerson = userName;
                if (!string.IsNullOrEmpty(ascmWmsBackInvoiceMain_Model.memo))
                    ascmWmsBackInvoiceMain.memo = ascmWmsBackInvoiceMain_Model.memo.Trim();
                #endregion

                #region 明细
                int maxId_Detail = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsBackInvoiceDetail");
                List<AscmWmsBackInvoiceDetail> listAscmWmsBackInvoiceDetailAdd = new List<AscmWmsBackInvoiceDetail>();
                List<AscmWmsBackInvoiceDetail> listAscmWmsBackInvoiceDetailUpdate = new List<AscmWmsBackInvoiceDetail>();
                List<AscmWmsBackInvoiceDetail> listAscmWmsBackInvoiceDetailDelete = new List<AscmWmsBackInvoiceDetail>();

                int iRec = 0;
                string batchIds = string.Empty;
                foreach (AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail_Model in listAscmWmsBackInvoiceDetail_Model)
                {
                    AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail = null;
                    if (ascmWmsBackInvoiceDetail_Model.id < 0)
                    {
                        ascmWmsBackInvoiceDetail = new AscmWmsBackInvoiceDetail();
                        ascmWmsBackInvoiceDetail.id = ++maxId_Detail;
                        ascmWmsBackInvoiceDetail.organizationId = 775;
                        ascmWmsBackInvoiceDetail.createUser = userName;
                        ascmWmsBackInvoiceDetail.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        ascmWmsBackInvoiceDetail.mainId = ascmWmsBackInvoiceMain.id;
                        listAscmWmsBackInvoiceDetailAdd.Add(ascmWmsBackInvoiceDetail);
                    }
                    else if (listAscmWmsBackInvoiceDetail != null)
                    {
                        ascmWmsBackInvoiceDetail = listAscmWmsBackInvoiceDetail.Find(P => P.id == ascmWmsBackInvoiceDetail_Model.id);
                        if (ascmWmsBackInvoiceDetail == null)
                            throw new Exception("系统错误：退货明细异常！");
                        listAscmWmsBackInvoiceDetailUpdate.Add(ascmWmsBackInvoiceDetail);
                    }
                    ascmWmsBackInvoiceDetail.modifyUser = userName;
                    ascmWmsBackInvoiceDetail.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    ascmWmsBackInvoiceDetail.materialId = ascmWmsBackInvoiceDetail_Model.ascmMaterialItem.id;
                    ascmWmsBackInvoiceDetail.batchId = ascmWmsBackInvoiceDetail_Model.batchId;
                    ascmWmsBackInvoiceDetail.deliveryQuantity = ascmWmsBackInvoiceDetail_Model.deliveryQuantity;
                    ascmWmsBackInvoiceDetail.returnQuantity = ascmWmsBackInvoiceDetail_Model.returnQuantity;
                    ascmWmsBackInvoiceDetail.warelocationId = ascmWmsBackInvoiceDetail_Model.warelocationId;
                    ascmWmsBackInvoiceDetail.warehouseId = ascmWmsBackInvoiceDetail_Model.warehouseId;
                    ascmWmsBackInvoiceDetail.ascmMaterialItem = ascmWmsBackInvoiceDetail_Model.ascmMaterialItem;
                    ascmWmsBackInvoiceDetail.ascmDeliveryOrderBatch = ascmWmsBackInvoiceDetail_Model.ascmDeliveryOrderBatch;
                    //ascmWmsBackInvoiceDetail.batchBarCodeNum = ascmWmsBackInvoiceDetail_Model.batchBarCodeNum;
                    ascmWmsBackInvoiceDetail.docNumber = ascmWmsBackInvoiceDetail_Model.docNumber;
                    iRec++;

                    if (!string.IsNullOrEmpty(batchIds))
                        batchIds += ",";
                    batchIds += ascmWmsBackInvoiceDetail_Model.batchId;
                }
                if (listAscmWmsBackInvoiceDetail != null)
                {
                    foreach (AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail in listAscmWmsBackInvoiceDetail)
                    {
                        AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail_Model = listAscmWmsBackInvoiceDetail_Model.Find(P => P.id == ascmWmsBackInvoiceDetail.id);
                        if (ascmWmsBackInvoiceDetail_Model == null)
                            listAscmWmsBackInvoiceDetailDelete.Add(ascmWmsBackInvoiceDetail);
                    }
                }
                #endregion

                #region 与送货单行表关联
                int maxId_Detail_Link = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsBackInvoiceLink");
                List<AscmWmsBackInvoiceLink> listAscmWmsBackInvoiceLink = new List<AscmWmsBackInvoiceLink>();
                foreach (AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail in listAscmWmsBackInvoiceDetailAdd)
                {
                    #region 批单退货
                    if (ascmWmsBackInvoiceDetail.batchId != 0)
                    {
                        List<AscmDeliveryOrderDetail> listDeliveryOrderDetail = AscmDeliveryOrderDetailService.GetInstance().GetListByBatch(ascmWmsBackInvoiceDetail.batchId);
                        if (listDeliveryOrderDetail == null || listDeliveryOrderDetail.Count == 0)
                            throw new Exception("获取送货单行表数据失败");
                        //获取已经退过货的批明细
                        List<AscmWmsBackInvoiceLink> findListWmsBackInvoiceLink =
                            AscmWmsBackInvoiceLinkService.GetInstance().GetList(null, "", "", "", "batchId=" + ascmWmsBackInvoiceDetail.batchId);
                        //退货数量
                        decimal rejectQuantity = ascmWmsBackInvoiceDetail.returnQuantity;
                        if (findListWmsBackInvoiceLink != null)
                        {
                            var _findListWmsBackInvoiceLink = findListWmsBackInvoiceLink.Where(P => P.batchId == ascmWmsBackInvoiceDetail.batchId);
                            if (_findListWmsBackInvoiceLink != null)
                            {
                                //批单累计退货总数量
                                decimal rejectSumQuantity = _findListWmsBackInvoiceLink.Sum(P => P.rejectQuantity);
                                if (rejectSumQuantity + rejectQuantity > ascmWmsBackInvoiceDetail.deliveryQuantity)
                                    throw new Exception("批单[" + ascmWmsBackInvoiceDetail.batchBarCode + "]累计退货总数量[" + (rejectSumQuantity + rejectQuantity) + "]大于送货总数量[" + ascmWmsBackInvoiceDetail.deliveryQuantity + "]");
                            }
                        }
                        foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in listDeliveryOrderDetail)
                        {
                            AscmWmsBackInvoiceLink ascmWmsBackInvoiceLink = new AscmWmsBackInvoiceLink();
                            ascmWmsBackInvoiceLink.id = ++maxId_Detail_Link;
                            ascmWmsBackInvoiceLink.detailId = ascmWmsBackInvoiceDetail.id;
                            ascmWmsBackInvoiceLink.batchId = ascmWmsBackInvoiceDetail.batchId;
                            ascmWmsBackInvoiceLink.deliOrderMainId = ascmDeliveryOrderDetail.mainId;
                            ascmWmsBackInvoiceLink.barCode = ascmDeliveryOrderDetail.mainBarCode;
                            ascmWmsBackInvoiceLink.deliOrderDetailId = ascmDeliveryOrderDetail.id;
                            //ascmWmsBackInvoiceLink.warehouseId = ascmDeliveryOrderDetail.warehouseId;
                            ascmWmsBackInvoiceLink.warehouseId = ascmWmsBackInvoiceDetail.warehouseId;

                            ascmWmsBackInvoiceLink.materialId = ascmDeliveryOrderDetail.materialId;
                            ascmWmsBackInvoiceLink.materialDocNumber = ascmDeliveryOrderDetail.materialDocNumber;
                            decimal deliveryQuantity = ascmDeliveryOrderDetail.deliveryQuantity;
                            if (findListWmsBackInvoiceLink != null)
                            {
                                var _findListWmsBackInvoiceLink = findListWmsBackInvoiceLink.Where(P => P.deliOrderDetailId == ascmDeliveryOrderDetail.id);
                                if (_findListWmsBackInvoiceLink != null)
                                {
                                    deliveryQuantity -= _findListWmsBackInvoiceLink.Sum(P => P.rejectQuantity);
                                }
                            }
                            ascmWmsBackInvoiceLink.deliveryQuantity = deliveryQuantity;
                            if (rejectQuantity >= deliveryQuantity)
                                ascmWmsBackInvoiceLink.rejectQuantity = deliveryQuantity;
                            else
                                ascmWmsBackInvoiceLink.rejectQuantity = rejectQuantity;
                            rejectQuantity -= ascmWmsBackInvoiceLink.rejectQuantity;
                            if (ascmWmsBackInvoiceLink.rejectQuantity > 0)
                                listAscmWmsBackInvoiceLink.Add(ascmWmsBackInvoiceLink);
                        }
                    }
                    #endregion

                    #region 手工单退货
                    else
                    {
                        //获取已经退过货的手工单明细
                        List<AscmWmsBackInvoiceLink> findListWmsBackInvoiceLink =
                            AscmWmsBackInvoiceLinkService.GetInstance().GetList(null, "", "", "", "barCode='" + ascmWmsBackInvoiceDetail.docNumber + "'");
                        //退货数量
                        decimal rejectQuantity = ascmWmsBackInvoiceDetail.returnQuantity;
                        if (findListWmsBackInvoiceLink != null)
                        {
                            var _findListWmsBackInvoiceLink = findListWmsBackInvoiceLink.Where(P => P.materialId == ascmWmsBackInvoiceDetail.materialId);
                            if (_findListWmsBackInvoiceLink != null)
                            {
                                //批单累计退货总数量
                                decimal rejectSumQuantity = _findListWmsBackInvoiceLink.Sum(P => P.rejectQuantity);
                                if (rejectSumQuantity + rejectQuantity > ascmWmsBackInvoiceDetail.deliveryQuantity)
                                    throw new Exception("手工单[" + ascmWmsBackInvoiceDetail.docNumber + "]累计退货总数量[" + (rejectSumQuantity + rejectQuantity) + "]大于送货总数量[" + ascmWmsBackInvoiceDetail.deliveryQuantity + "]");
                            }
                        }
                        AscmWmsBackInvoiceLink ascmWmsBackInvoiceLink = new AscmWmsBackInvoiceLink();
                        ascmWmsBackInvoiceLink.id = ++maxId_Detail_Link;
                        ascmWmsBackInvoiceLink.detailId = ascmWmsBackInvoiceDetail.id;
                        ascmWmsBackInvoiceLink.batchId = ascmWmsBackInvoiceDetail.batchId;
                        ascmWmsBackInvoiceLink.deliOrderMainId = ascmWmsBackInvoiceDetail.mainId;
                        ascmWmsBackInvoiceLink.barCode = ascmWmsBackInvoiceDetail.docNumber;
                        ascmWmsBackInvoiceLink.deliOrderDetailId = ascmWmsBackInvoiceDetail.id;
                        ascmWmsBackInvoiceLink.warehouseId = ascmWmsBackInvoiceDetail.warehouseId;
                        ascmWmsBackInvoiceLink.materialId = ascmWmsBackInvoiceDetail.materialId;
                        ascmWmsBackInvoiceLink.materialDocNumber = ascmWmsBackInvoiceDetail.materialDocNumber;
                        decimal deliveryQuantity = ascmWmsBackInvoiceDetail.deliveryQuantity;
                        if (findListWmsBackInvoiceLink != null)
                        {
                            var _findListWmsBackInvoiceLink = findListWmsBackInvoiceLink.Where(P => P.materialId == ascmWmsBackInvoiceDetail.materialId);
                            if (_findListWmsBackInvoiceLink != null)
                            {
                                deliveryQuantity -= _findListWmsBackInvoiceLink.Sum(P => P.rejectQuantity);
                            }
                        }
                        ascmWmsBackInvoiceLink.deliveryQuantity = deliveryQuantity;
                        if (rejectQuantity >= deliveryQuantity)
                            ascmWmsBackInvoiceLink.rejectQuantity = deliveryQuantity;
                        else
                            ascmWmsBackInvoiceLink.rejectQuantity = rejectQuantity;
                        rejectQuantity -= ascmWmsBackInvoiceLink.rejectQuantity;
                        if (ascmWmsBackInvoiceLink.rejectQuantity > 0)
                            listAscmWmsBackInvoiceLink.Add(ascmWmsBackInvoiceLink);
                    }
                    #endregion
                }
                if (listAscmWmsBackInvoiceLink.Count == 0)
                    throw new Exception("获取送货单行表数据失败");
                #endregion

                #region 物料减少
                List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList(null, "", "", "", "");
                List<AscmLocationMaterialLink> listLocationMaterialLinkUpdate = new List<AscmLocationMaterialLink>();
                if (listAscmWmsBackInvoiceDetail_Model != null && listAscmWmsBackInvoiceDetail_Model.Count() > 0)
                {
                    int maxId_Link = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmLocationMaterialLink");
                    foreach (AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail in listAscmWmsBackInvoiceDetail_Model)
                    {
                        AscmLocationMaterialLink locationMaterialLink = listLocationMaterialLink.Find(P => P.pk.warelocationId == ascmWmsBackInvoiceDetail.warelocationId && P.materialDocNumber == ascmWmsBackInvoiceDetail.materialDocNumber);
                        if (locationMaterialLink == null)//目标仓库不存在此物料；
                        {
                            throw new Exception("货位上没有物料【" + ascmWmsBackInvoiceDetail.materialDocNumber + "】");
                        }
                        else
                        {
                            locationMaterialLink.modifyUser = userName;
                            locationMaterialLink.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            locationMaterialLink.quantity = locationMaterialLink.quantity - ascmWmsBackInvoiceDetail.returnQuantity;
                            listLocationMaterialLinkUpdate.Add(locationMaterialLink);
                        }
                    }
                }
                #endregion

                #region 保存
                AscmMesInteractiveLog ascmMesInteractiveLog = new AscmMesInteractiveLog();
                ascmMesInteractiveLog.billId = ascmWmsBackInvoiceMain.id;
                ascmMesInteractiveLog.docNumber = ascmWmsBackInvoiceMain.docNumber;
                ascmMesInteractiveLog.billType = AscmMesInteractiveLog.BillTypeDefine.goodsReject;
                ascmMesInteractiveLog.createUser = userName;
                ascmMesInteractiveLog.modifyUser = userName;
                ascmMesInteractiveLog.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmMesInteractiveLog.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                //执行事务
                YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().Clear();
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        AscmMesService.GetInstance().DoGoodsReject(ascmWmsBackInvoiceMain, listAscmWmsBackInvoiceLink, userName, ascmMesInteractiveLog);
                        if (ascmMesInteractiveLog.returnCode == "0")
                        {
                            if (id.HasValue)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmWmsBackInvoiceMain);
                            }
                            else
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsBackInvoiceMain);
                            }
                            if (listAscmWmsBackInvoiceDetailAdd != null && listAscmWmsBackInvoiceDetailAdd.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsBackInvoiceDetailAdd);
                            }
                            if (listAscmWmsBackInvoiceDetailUpdate != null && listAscmWmsBackInvoiceDetailUpdate.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmWmsBackInvoiceDetailUpdate);
                            }
                            if (listAscmWmsBackInvoiceDetailDelete != null && listAscmWmsBackInvoiceDetailDelete.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsBackInvoiceDetailDelete);
                            }
                            if (listLocationMaterialLinkUpdate != null && listLocationMaterialLinkUpdate.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listLocationMaterialLinkUpdate);
                            }
                            if (listAscmWmsBackInvoiceLink.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsBackInvoiceLink);
                            }
                            ascmMesInteractiveLog.returnMessage = "退货成功";
                        }

                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        ascmMesInteractiveLog.returnCode = "-1";
                        ascmMesInteractiveLog.returnMessage = ex.Message;
                    }
                }
                #endregion

                //int maxLogId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmMesInteractiveLog");
                string maxLogIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_mes_interactive_log_id", "", "", 10);
                int maxLogId = Convert.ToInt32(maxLogIdKey);
                ascmMesInteractiveLog.id = maxLogId;
                AscmMesInteractiveLogService.GetInstance().Save(ascmMesInteractiveLog);
                string errorMsg = string.Empty;
                if (ascmMesInteractiveLog.returnCode != "0")
                {
                    errorMsg += string.Format("<li>供应商退货<font color='red'>{0}</font>：{1}</li>", ascmMesInteractiveLog.docNumber, ascmMesInteractiveLog.returnMessage);
                }
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    errorMsg = string.Format("<ul>{0}</ul>", errorMsg);
                    jsonObjectResult.message = errorMsg;
                }
                else
                {
                    jsonObjectResult.result = true;
                    jsonObjectResult.id = ascmWmsBackInvoiceMain.id.ToString();
                    jsonObjectResult.entity = ascmWmsBackInvoiceMain;
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult WmsBackInvoiceDetailAdd(string deliveryOrderBatchJson)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (string.IsNullOrEmpty(deliveryOrderBatchJson))
                    throw new Exception("系统错误：参数不能为NULL或空");
                List<AscmDeliveryOrderBatch> listAscmDeliveryOrderBatch = JsonConvert.DeserializeObject<List<AscmDeliveryOrderBatch>>(deliveryOrderBatchJson);
                if (listAscmDeliveryOrderBatch == null)
                    throw new Exception("系统错误：参数序列化错误");
                List<AscmWmsBackInvoiceDetail> listAscmWmsBackInvoiceDetail = new List<AscmWmsBackInvoiceDetail>();
                List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList("from AscmLocationMaterialLink", true, true);
                List<AscmWmsBackInvoiceDetail> list_WmsInc = AscmWmsBackInvoiceDetailService.GetInstance().GetList("from AscmWmsBackInvoiceDetail where batchId!=0");
                foreach (AscmDeliveryOrderBatch ascmDeliveryOrderBatch in listAscmDeliveryOrderBatch)
                {
                    AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail = new AscmWmsBackInvoiceDetail();
                    ascmWmsBackInvoiceDetail.batchId = ascmDeliveryOrderBatch.id;
                    ascmWmsBackInvoiceDetail.materialId = ascmDeliveryOrderBatch.materialId;
                    ascmWmsBackInvoiceDetail.deliveryQuantity = ascmDeliveryOrderBatch.totalNumber;
                    ascmWmsBackInvoiceDetail.ascmDeliveryOrderBatch = ascmDeliveryOrderBatch;
                    ascmWmsBackInvoiceDetail.ascmMaterialItem = ascmDeliveryOrderBatch.ascmMaterialItem;
                    ascmWmsBackInvoiceDetail.docNumber = ascmDeliveryOrderBatch.barCode;
                    ascmWmsBackInvoiceDetail.warehouseId = ascmDeliveryOrderBatch.warehouseId;
                    if (ascmDeliveryOrderBatch != null)
                    {
                        var find_LocationMaterialLink = listLocationMaterialLink.Find(item => item.pk.materialId == ascmDeliveryOrderBatch.materialIdTmp);
                        if (find_LocationMaterialLink != null)
                        {
                            ascmWmsBackInvoiceDetail.warelocationId = find_LocationMaterialLink.pk.warelocationId;
                            ascmWmsBackInvoiceDetail.warelocationDoc = find_LocationMaterialLink.locationDocNumber;
                        }
                    }
                    decimal dToTal = 0;
                    var find = list_WmsInc.Where(item => item.batchId == ascmDeliveryOrderBatch.id && item.materialId == ascmDeliveryOrderBatch.materialIdTmp);
                    if (find != null)
                        dToTal = find.Sum(item => item.returnQuantity);
                    ascmWmsBackInvoiceDetail.returnQuantityTotal = dToTal;
                    listAscmWmsBackInvoiceDetail.Add(ascmWmsBackInvoiceDetail);
                }
                jsonObjectResult.result = true;
                jsonObjectResult.entity = listAscmWmsBackInvoiceDetail;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WmsBackInvoiceDetailAddWmsInc(string wmsIncManAccJson)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (string.IsNullOrEmpty(wmsIncManAccJson))
                    throw new Exception("系统错误：参数不能为NULL或空");
                List<AscmWmsIncManAccDetail> listAscmWmsIncManAccDetail = JsonConvert.DeserializeObject<List<AscmWmsIncManAccDetail>>(wmsIncManAccJson);
                if (listAscmWmsIncManAccDetail == null)
                    throw new Exception("系统错误：参数序列化错误");
                List<AscmWmsBackInvoiceDetail> listAscmWmsBackInvoiceDetail = new List<AscmWmsBackInvoiceDetail>();
                List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList("from AscmLocationMaterialLink", true, true);
                List<AscmWmsBackInvoiceDetail> list_WmsInc = AscmWmsBackInvoiceDetailService.GetInstance().GetList("from AscmWmsBackInvoiceDetail where batchId=0");
                foreach (AscmWmsIncManAccDetail wmsIncManAccDetail in listAscmWmsIncManAccDetail)
                {
                    AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail = new AscmWmsBackInvoiceDetail();
                    ascmWmsBackInvoiceDetail.docNumber = wmsIncManAccDetail.docNumber;
                    ascmWmsBackInvoiceDetail.materialId = wmsIncManAccDetail.materialId;
                    ascmWmsBackInvoiceDetail.deliveryQuantity = wmsIncManAccDetail.receivedQuantity;
                    ascmWmsBackInvoiceDetail.ascmMaterialItem = wmsIncManAccDetail.ascmMaterialItem;
                    ascmWmsBackInvoiceDetail.warelocationId = wmsIncManAccDetail.warelocationId;
                    ascmWmsBackInvoiceDetail.warehouseId = wmsIncManAccDetail.warehouseId;
                    ascmWmsBackInvoiceDetail.warelocationDoc = wmsIncManAccDetail.warelocationdocNumber;
                    decimal dToTal = 0;
                    var find = list_WmsInc.Where(item => item.docNumber == wmsIncManAccDetail.docNumber && item.materialId == wmsIncManAccDetail.materialId);
                    if (find != null)
                        dToTal = find.Sum(item => item.returnQuantity);
                    ascmWmsBackInvoiceDetail.returnQuantityTotal = dToTal;
                    listAscmWmsBackInvoiceDetail.Add(ascmWmsBackInvoiceDetail);
                }
                jsonObjectResult.result = true;
                jsonObjectResult.entity = listAscmWmsBackInvoiceDetail;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WmsIncManAccSelectList(int? page, int? rows, string sort, string order, string queryWord, int? supplierId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (supplierId.HasValue)
                {
                    string whereOther = " supplierId=" + supplierId.Value;
                    string mainIdList = string.Empty;
                    List<AscmWmsIncManAccMain> listAscmWmsIncManAccMain = AscmWmsIncManAccMainService.GetInstance().GetList(null, "", "", queryWord, whereOther);
                    if (listAscmWmsIncManAccMain != null && listAscmWmsIncManAccMain.Count() > 0)
                    {
                        foreach (AscmWmsIncManAccMain ascmWmsIncManAssMain in listAscmWmsIncManAccMain)
                        {
                            if (!string.IsNullOrEmpty(mainIdList))
                                mainIdList += ",";
                            mainIdList += ascmWmsIncManAssMain.id;
                        }
                    }
                    if (!string.IsNullOrEmpty(mainIdList))
                    {
                        string sOther = " incManAccMainId in (" + mainIdList + ")";
                        List<AscmWmsIncManAccDetail> listAscmWmsIncManAccDetail = AscmWmsIncManAccDetailService.GetInstance().GetList(ynPage, "", "", sOther);
                        if (listAscmWmsIncManAccDetail != null && listAscmWmsIncManAccDetail.Count() > 0)
                        {
                            foreach (AscmWmsIncManAccDetail wmsIncManAccDetail in listAscmWmsIncManAccDetail)
                            {
                                AscmWmsIncManAccMain ascmWmsIncManAccMain = listAscmWmsIncManAccMain.Find(item => item.id == wmsIncManAccDetail.incManAccMainId);
                                wmsIncManAccDetail.ascmWmsIncManAccMain = ascmWmsIncManAccMain;

                                jsonDataGridResult.rows.Add(wmsIncManAccDetail);
                            }
                            jsonDataGridResult.total = ynPage.GetRecordCount();
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
        #endregion

        #region 供应商退货查询
        public ActionResult WmsBackInvoiceQuery()
        {
            ViewData["dtStart"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewData["dtEnd"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }
		public ActionResult WmsBackInvoiceMainList(int? page, int? rows, string sort, string order, string queryWord, string supplierDoc, string startPlanTime, string endPlanTime, string returnCode)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmWmsBackInvoiceMain> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereStartModifyTime = "", whereEndModifyTime = "";

                DateTime dtStartPlanTime, dtEndPlanTime;
                if (!string.IsNullOrEmpty(startPlanTime) && DateTime.TryParse(startPlanTime, out dtStartPlanTime))
                    whereStartModifyTime = "modifyTime>='" + dtStartPlanTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endPlanTime) && DateTime.TryParse(endPlanTime, out dtEndPlanTime))
                    whereEndModifyTime = "modifyTime<'" + dtEndPlanTime.ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartModifyTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndModifyTime);
				if (!string.IsNullOrEmpty(returnCode))
				{
					if (returnCode == "0")
					{
						whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "returnCode='0'");
					}
					else
					{
						whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "returnCode is not null and returnCode!='0'");
					}
				}

                list = AscmWmsBackInvoiceMainService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                if (!string.IsNullOrEmpty(supplierDoc))
                    list = list.Where(item => item.ascmSupplier.docNumber == supplierDoc).OrderBy(item => item.modifyTime).ToList();
                if (list != null && list.Count() > 0)
                {
                    foreach (AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain in list)
                    {
                        jsonDataGridResult.rows.Add(ascmWmsBackInvoiceMain);
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
        public ActionResult WmsBackInvoiceMainView(int? id)
        {
            AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain = null;
            try
            {
                ascmWmsBackInvoiceMain = AscmWmsBackInvoiceMainService.GetInstance().Get(id.Value);
                AscmSupplier ascmSuppliser = AscmSupplierService.GetInstance().Get(ascmWmsBackInvoiceMain.supplierId);
                ascmWmsBackInvoiceMain.ascmSupplier = ascmSuppliser;
                AscmMtlTransactionReasons ascmMtlTransactionReasons = AscmMtlTransactionReasonsService.GetInstance().Get(ascmWmsBackInvoiceMain.reasonId);
                ascmWmsBackInvoiceMain.ascmMtlTransactionReasons = ascmMtlTransactionReasons;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ascmWmsBackInvoiceMain);
        }
        public ActionResult WmsBackInvoiceDetialList(int? id)
        {
            List<AscmWmsBackInvoiceDetail> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmWmsBackInvoiceDetailService.GetInstance().GetList(null, "", "", id.Value, "", "");
                List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList(" from AscmWarelocation");
                foreach (AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail in list)
                {
                    var find = listAscmWarelocation.Find(item => item.id == ascmWmsBackInvoiceDetail.warelocationId);
                    if (find != null)
                        ascmWmsBackInvoiceDetail.warelocationDoc = find.docNumber;
                    jsonDataGridResult.rows.Add(ascmWmsBackInvoiceDetail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 子库存转移
        public ActionResult WmsStockTransEdit(int? id)
        {
            AscmWmsStockTransMain ascmWmsStockTransMain = new AscmWmsStockTransMain();
            try
            {
                if (id.HasValue)
                {
                    ascmWmsStockTransMain = AscmWmsStockTransMainService.GetInstance().Get(id.Value);
                    YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight();
                    ynWebRight.rightAdd = false;
                    ynWebRight.rightDelete = false;
                    ynWebRight.rightEdit = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ascmWmsStockTransMain);
        }
        [HttpPost]
        public ActionResult WmsStockTransSave(AscmWmsStockTransMain ascmWmsStockTransMain_Model, int? id, string detailJson, string statusOption)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(detailJson))
                    throw new Exception("系统错误：仓库转移明细参数不能为NULL或空");
                List<AscmWmsStockTransDetail> listAscmWmsStockTransDetail_Model = JsonConvert.DeserializeObject<List<AscmWmsStockTransDetail>>(detailJson);
                if (listAscmWmsStockTransDetail_Model == null)
                    throw new Exception("系统错误：仓库转移明细参数序列化错误");
                if (ascmWmsStockTransMain_Model.manualDocNumber == null || ascmWmsStockTransMain_Model.manualDocNumber.Trim() == "")
                    throw new Exception("手工单号不能为空");

                string userName = string.Empty;
                string userId = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userId = User.Identity.Name;//用户名
                    userName = AscmUserInfoService.GetInstance().Get(userId).userName;//姓名
                #endregion

                #region 主表
                AscmWmsStockTransMain ascmWmsStockTransMain = null;
                List<AscmWmsStockTransDetail> listAscmWmsStockTransDetail = null;
                if (id.HasValue)
                {
                    ascmWmsStockTransMain = AscmWmsStockTransMainService.GetInstance().Get(id.Value);
                    listAscmWmsStockTransDetail = AscmWmsStockTransDetailService.GetInstance().GetListByMainId(id.Value);
                }
                else
                {
                    ascmWmsStockTransMain = new AscmWmsStockTransMain();
                    ascmWmsStockTransMain.organizationId = 775;
                    ascmWmsStockTransMain.createUser = userId;
                    ascmWmsStockTransMain.createUserName = userName;
                    ascmWmsStockTransMain.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    ascmWmsStockTransMain.status = statusOption;//保存状态
                    //ascmWmsStockTransMain.manualDocNumber = userName + DateTime.Now.ToString("yyyyMM") + "001";//手工单号（自动生成 规则：姓名缩写+yyyy+MM+001）
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsStockTransMain");
                    ascmWmsStockTransMain.id = ++maxId;
                    //获取MES闭环单号
                    ascmWmsStockTransMain.docNumber = AscmMesService.GetInstance().GetMesTransferBillNo();
                    if (string.IsNullOrEmpty(ascmWmsStockTransMain.docNumber))
                        throw new Exception("系统错误：从MES获取仓库转移闭环单号失败");
                }
                if (ascmWmsStockTransMain == null)
                    throw new Exception("提交仓库转移单失败！");

                ascmWmsStockTransMain.manualDocNumber = ascmWmsStockTransMain_Model.manualDocNumber.Trim();
                ascmWmsStockTransMain.modifyUser = userId;
                ascmWmsStockTransMain.modifyUserName = userName;
                ascmWmsStockTransMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ascmWmsStockTransMain.fromWarehouseId = ascmWmsStockTransMain_Model.fromWarehouseId;
                ascmWmsStockTransMain.toWarehouseId = ascmWmsStockTransMain_Model.toWarehouseId;
                ascmWmsStockTransMain.responsiblePerson = userName;
                ascmWmsStockTransMain.transType = ascmWmsStockTransMain_Model.transType;
                ascmWmsStockTransMain.reasonId = ascmWmsStockTransMain_Model.reasonId;
                ascmWmsStockTransMain.fromWarehouseUser = ascmWmsStockTransMain_Model.fromWarehouseUser;
                ascmWmsStockTransMain.toWarehouseUser = ascmWmsStockTransMain_Model.toWarehouseUser;
                ascmWmsStockTransMain.reference = ascmWmsStockTransMain_Model.reference;
                if (!string.IsNullOrEmpty(ascmWmsStockTransMain_Model.memo))
                    ascmWmsStockTransMain.memo = ascmWmsStockTransMain_Model.memo.Trim();
                #endregion

                #region 明细
                int maxId_Detail = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsStockTransDetail");
                List<AscmWmsStockTransDetail> listAscmWmsStockTransDetailAdd = new List<AscmWmsStockTransDetail>();
                List<AscmWmsStockTransDetail> listAscmWmsStockTransDetailUpdate = new List<AscmWmsStockTransDetail>();
                List<AscmWmsStockTransDetail> listAscmWmsStockTransDetailDelete = new List<AscmWmsStockTransDetail>();

                int iRec = 0;
                foreach (AscmWmsStockTransDetail ascmWmsStockTransDetail_Model in listAscmWmsStockTransDetail_Model)
                {
                    AscmWmsStockTransDetail wmsStockTransDetail = null;
                    if (ascmWmsStockTransDetail_Model.id < 0)
                    {
                        wmsStockTransDetail = new AscmWmsStockTransDetail();
                        wmsStockTransDetail.id = ++maxId_Detail;
                        wmsStockTransDetail.organizationId = 775;
                        wmsStockTransDetail.createUser = userName;
                        wmsStockTransDetail.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        wmsStockTransDetail.mainId = ascmWmsStockTransMain.id;
                        listAscmWmsStockTransDetailAdd.Add(wmsStockTransDetail);
                    }
                    else if (listAscmWmsStockTransDetail != null)
                    {
                        wmsStockTransDetail = listAscmWmsStockTransDetail.Find(P => P.id == ascmWmsStockTransDetail_Model.id);
                        if (wmsStockTransDetail == null)
                            throw new Exception("系统错误：仓库转移明细异常！");
                        listAscmWmsStockTransDetailUpdate.Add(wmsStockTransDetail);
                    }
                    wmsStockTransDetail.modifyUser = userName;
                    wmsStockTransDetail.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    wmsStockTransDetail.materialId = ascmWmsStockTransDetail_Model.ascmMaterialItem.id;
                    wmsStockTransDetail.fromWarelocationId = ascmWmsStockTransDetail_Model.fromWarelocationId;
                    wmsStockTransDetail.toWarelocationId = ascmWmsStockTransDetail_Model.toWarelocationId;
                    wmsStockTransDetail.quantity = ascmWmsStockTransDetail_Model.quantity;
                    wmsStockTransDetail.reference = ascmWmsStockTransMain_Model.reference;
                    iRec++;
                }
                if (listAscmWmsStockTransDetail != null)
                {
                    foreach (AscmWmsStockTransDetail wmsStockTransDetail in listAscmWmsStockTransDetail)
                    {
                        AscmWmsStockTransDetail ascmWmsStockTransDetail_Model = listAscmWmsStockTransDetail_Model.Find(P => P.id == wmsStockTransDetail.id);
                        if (ascmWmsStockTransDetail_Model == null)
                            listAscmWmsStockTransDetailDelete.Add(wmsStockTransDetail);
                    }
                }
                #endregion

                #region 物料数量转移
                List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList(null, "", "", "", "");
                List<AscmLocationMaterialLink> listLocationMaterialLinkUpdate = new List<AscmLocationMaterialLink>();
                List<AscmLocationMaterialLink> listLocationMaterialLinkAdd = new List<AscmLocationMaterialLink>();
                if (listAscmWmsStockTransDetail_Model != null && listAscmWmsStockTransDetail_Model.Count() > 0)
                {
                    //int maxId_Link = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmLocationMaterialLink");
                    foreach (AscmWmsStockTransDetail wmsStockTransDetail in listAscmWmsStockTransDetail_Model)
                    {
                        AscmLocationMaterialLink locationMaterialLinkUpdate_From = listLocationMaterialLink.Find(P => P.pk.warelocationId == wmsStockTransDetail.fromWarelocationId && P.pk.materialId == wmsStockTransDetail.materialId);
                        if (locationMaterialLinkUpdate_From == null)
                            throw new Exception("系统错误：物料转移异常！");
                        AscmLocationMaterialLink locationMaterialLinkUpdate_To = listLocationMaterialLink.Find(P => P.pk.warelocationId == wmsStockTransDetail.toWarelocationId && P.pk.materialId == wmsStockTransDetail.materialId);
                        if (locationMaterialLinkUpdate_To == null)//目标仓库不存在此物料；
                        {
                            locationMaterialLinkUpdate_To = new AscmLocationMaterialLink();
                            locationMaterialLinkUpdate_To.pk = new AscmLocationMaterialLinkPK { warelocationId = wmsStockTransDetail.toWarelocationId, materialId = wmsStockTransDetail.materialId };
                            //locationMaterialLinkUpdate_To.id = ++maxId_Link;
                            locationMaterialLinkUpdate_To.organizationId = 775;
                            locationMaterialLinkUpdate_To.createUser = userName;
                            locationMaterialLinkUpdate_To.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            //locationMaterialLinkUpdate_To.warelocationId = wmsStockTransDetail.toWarelocationId;
                            //locationMaterialLinkUpdate_To.materialId = wmsStockTransDetail.materialId;
                            locationMaterialLinkUpdate_To.quantity = wmsStockTransDetail.quantity;
                            listLocationMaterialLinkAdd.Add(locationMaterialLinkUpdate_To);
                        }
                        else
                        {
                            locationMaterialLinkUpdate_To.quantity = locationMaterialLinkUpdate_To.quantity + wmsStockTransDetail.quantity;
                            listLocationMaterialLinkUpdate.Add(locationMaterialLinkUpdate_To);
                        }
                        locationMaterialLinkUpdate_To.modifyUser = userName;
                        locationMaterialLinkUpdate_To.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        locationMaterialLinkUpdate_From.quantity = locationMaterialLinkUpdate_From.quantity - wmsStockTransDetail.quantity;
                        listLocationMaterialLinkUpdate.Add(locationMaterialLinkUpdate_From);

                    }
                }
                #endregion

                #region 保存
                //执行事务
                YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().Clear();
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        if (id.HasValue)
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmWmsStockTransMain);
                        }
                        else
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsStockTransMain);
                        }
                        if (listAscmWmsStockTransDetailAdd != null && listAscmWmsStockTransDetailAdd.Count > 0)
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsStockTransDetailAdd);
                        }
                        if (listAscmWmsStockTransDetailUpdate != null && listAscmWmsStockTransDetailUpdate.Count > 0)
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmWmsStockTransDetailUpdate);
                        }
                        if (listAscmWmsStockTransDetailDelete != null && listAscmWmsStockTransDetailDelete.Count > 0)
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsStockTransDetailDelete);
                        }
                        if (listLocationMaterialLinkUpdate != null && listLocationMaterialLinkUpdate.Count() > 0)
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listLocationMaterialLinkUpdate);
                        }
                        if (listLocationMaterialLinkAdd != null && listLocationMaterialLinkAdd.Count() > 0)
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listLocationMaterialLinkAdd);
                        }

                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        jsonObjectResult.message = ex.Message;
                    }
                }
                #endregion

                jsonObjectResult.result = true;
                jsonObjectResult.id = ascmWmsStockTransMain.id.ToString();
                jsonObjectResult.entity = ascmWmsStockTransMain;
                //}
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult WmsStockTransCheck(int id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
                userName = User.Identity.Name;

            AscmWmsStockTransMain ascmWmsStockTransMain = AscmWmsStockTransMainService.GetInstance().Get(id);
            List<AscmWmsStockTransDetail> listAscmWmsStockTransDetail = AscmWmsStockTransDetailService.GetInstance().GetListByMainId(id);
            foreach (AscmWmsStockTransDetail detail in listAscmWmsStockTransDetail)
            {
                if (string.IsNullOrEmpty(detail.reference))
                    detail.reference = "";
            }

            AscmMesInteractiveLog ascmMesInteractiveLog = new AscmMesInteractiveLog();
            ascmMesInteractiveLog.billId = ascmWmsStockTransMain.id;
            ascmMesInteractiveLog.docNumber = ascmWmsStockTransMain.docNumber;
            ascmMesInteractiveLog.billType = AscmMesInteractiveLog.BillTypeDefine.stockTrans;
            ascmMesInteractiveLog.modifyUser = userName;
            ascmMesInteractiveLog.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            ascmMesInteractiveLog.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    AscmMesService.GetInstance().DoStockTrans(ascmWmsStockTransMain, listAscmWmsStockTransDetail, userName, ascmMesInteractiveLog);
                    if (ascmMesInteractiveLog.returnCode == "0")
                    {
                        ascmMesInteractiveLog.returnMessage = "转移成功";
                        ascmWmsStockTransMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        ascmWmsStockTransMain.status = "confirm";
                        AscmWmsStockTransMainService.GetInstance().Update(ascmWmsStockTransMain);
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    ascmMesInteractiveLog.returnCode = "-1";
                    ascmMesInteractiveLog.returnMessage = ex.Message;
                    jsonObjectResult.message = ex.Message;
                }
            }
                string maxLogIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_mes_interactive_log_id", "", "", 10);
                int maxLogId = Convert.ToInt32(maxLogIdKey);
                ascmMesInteractiveLog.id = maxLogId;
                AscmMesInteractiveLogService.GetInstance().Save(ascmMesInteractiveLog);
                string errorMsg = string.Empty;
                if (ascmMesInteractiveLog.returnCode != "0")
                {
                    errorMsg += string.Format("<li>子库存转移<font color='red'>{0}</font>：{1}</li>", ascmMesInteractiveLog.docNumber, ascmMesInteractiveLog.returnMessage);
                }
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    errorMsg = string.Format("<ul>{0}</ul>", errorMsg);
                    jsonObjectResult.message = errorMsg;
                }
                else
                {
                    jsonObjectResult.result = true;
                }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }       
        public ActionResult WmsStockTransDetailAdd(string locationMaterialJson, string toWarehouseId)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (string.IsNullOrEmpty(locationMaterialJson))
                    throw new Exception("系统错误：参数不能为NULL或空");
                List<AscmLocationMaterialLink> listLocationMaterialLink = JsonConvert.DeserializeObject<List<AscmLocationMaterialLink>>(locationMaterialJson);
                if (listLocationMaterialLink == null)
                    throw new Exception("系统错误：参数序列化错误");
                List<AscmWmsStockTransDetail> listAscmWmsStockTransDetail = new List<AscmWmsStockTransDetail>();
                //List<AscmLocationMaterialLink> list_AscmLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList("from AscmLocationMaterialLink", true, true);
                string whereOther = "";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "warelocationId in(select id from AscmWarelocation where warehouseId='" + toWarehouseId + "')");
                List<AscmLocationMaterialLink> list_AscmLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList(null, "", "", "", whereOther, false, false);
                
                foreach (AscmLocationMaterialLink locationMaterialLink in listLocationMaterialLink)
                {
                    AscmWmsStockTransDetail wmsStockTransDetail = new AscmWmsStockTransDetail();
                    wmsStockTransDetail.materialId = locationMaterialLink.pk.materialId;
                    //wmsStockTransDetail.ascmMaterialItem = locationMaterialLink.ascmMaterialItem;
                    wmsStockTransDetail.ascmMaterialItem = new AscmMaterialItem { id = locationMaterialLink.pk.materialId, docNumber = locationMaterialLink.materialDocNumber, description = locationMaterialLink.materialDescription };
                    wmsStockTransDetail.fromWarelocationId = locationMaterialLink.pk.warelocationId;
                    //wmsStockTransDetail.fromWarelocation = locationMaterialLink.ascmWarelocation;
                    wmsStockTransDetail.fromWarelocation = new AscmWarelocation { id = locationMaterialLink.pk.warelocationId, docNumber = locationMaterialLink.locationDocNumber, warehouseId = locationMaterialLink.warehouseId };
                    wmsStockTransDetail.fromQuantity = locationMaterialLink.quantity;
                    if (!string.IsNullOrEmpty(toWarehouseId))
                    {
                        var find_LocationMaterialLink = list_AscmLocationMaterialLink.Find(item => item.pk.materialId == locationMaterialLink.pk.materialId && item.warehouseId == toWarehouseId);
                        if (find_LocationMaterialLink != null)
                        {
                            wmsStockTransDetail.toWarelocationId = find_LocationMaterialLink.pk.warelocationId;
                            //wmsStockTransDetail.toWarelocation = find_LocationMaterialLink.ascmWarelocation;
                            wmsStockTransDetail.fromWarelocation = new AscmWarelocation { id = find_LocationMaterialLink.pk.warelocationId, docNumber = find_LocationMaterialLink.locationDocNumber, warehouseId = find_LocationMaterialLink.warehouseId };
                        }
                    }
                    listAscmWmsStockTransDetail.Add(wmsStockTransDetail);
                }
                jsonObjectResult.result = true;
                jsonObjectResult.entity = listAscmWmsStockTransDetail;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LocationMaterialAscxList(int? page, int? rows, string sort, string order, string queryWord, string warehouseId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereBuilding = "", whereWarehouse = "";
                if (!string.IsNullOrEmpty(warehouseId))
                    whereWarehouse = " warelocationId in(select id from AscmWarelocation where warehouseId='" + warehouseId + "')";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereBuilding);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);

                List<AscmLocationMaterialLink> list = AscmLocationMaterialLinkService.GetInstance().GetList(ynPage, sort, order, queryWord, whereOther);
                if (list != null)
                {
                    foreach (AscmLocationMaterialLink locationMaterialLink in list)
                    {
                        jsonDataGridResult.rows.Add(locationMaterialLink);
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
        public ActionResult WareLocationCbList(string warehouseId, string materialDocNumber)
        {
            List<AscmWarelocation> list = null;
            try
            {
                string whereOther = "";
                if (!string.IsNullOrEmpty(materialDocNumber))
                    whereOther += " (categoryCode='" + materialDocNumber.Substring(0, 4) + "' or categoryCode='0000')";
                list = AscmWarelocationService.GetInstance().GetList("", "", null, warehouseId, whereOther);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 子库存转移查询
        public ActionResult WmsStockTransQuery()
        {
            ViewData["dtStart"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewData["dtEnd"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }
		public ActionResult WmsStockTransMainList(int? page, int? rows, string sort, string order, string queryWord, string startPlanTime, string endPlanTime, string returnCode,string user)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmWmsStockTransMain> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereStartModifyTime = "", whereEndModifyTime = "";

                DateTime dtStartPlanTime, dtEndPlanTime;
                if (!string.IsNullOrEmpty(startPlanTime) && DateTime.TryParse(startPlanTime, out dtStartPlanTime))
                    whereStartModifyTime = "modifyTime>='" + dtStartPlanTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endPlanTime) && DateTime.TryParse(endPlanTime, out dtEndPlanTime))
                    whereEndModifyTime = "modifyTime<'" + dtEndPlanTime.ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartModifyTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndModifyTime);
				if (!string.IsNullOrEmpty(returnCode))
				{
					if (returnCode == "0")
					{
						whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "returnCode='0'");
					}
					else
					{
						whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "returnCode is not null and returnCode!='0'");
					}
				}

                if (!string.IsNullOrEmpty(user))
                {
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "(toWarehouseUser='" + user + "' or createUser='" + user + "' or modifyUser='" + user + "' or fromWarehouseUser='" + user + "')");
                }
                list = AscmWmsStockTransMainService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);              
                if (list != null && list.Count() > 0)
                {
                    foreach (AscmWmsStockTransMain ascmWmsStorcTrancMain in list)
                    {
                        jsonDataGridResult.rows.Add(ascmWmsStorcTrancMain);
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
        public ActionResult WmsStockTransMainView(int? id)
        {
            AscmWmsStockTransMain ascmWmsStockTransMain = null;
            try
            {
                ascmWmsStockTransMain = AscmWmsStockTransMainService.GetInstance().Get(id.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ascmWmsStockTransMain);
        }
        public ActionResult WmsStockTransDetialList(int? id)
        {
            List<AscmWmsStockTransDetail> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmWmsStockTransDetailService.GetInstance().GetList("", "", id.Value, "", "");
                foreach (AscmWmsStockTransDetail wmsStockTransDetail in list)
                {
                    jsonDataGridResult.rows.Add(wmsStockTransDetail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WmsStockTransUpdate(string statusOption, int id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
                userName = User.Identity.Name;
            try
            {
                AscmWmsStockTransMain ascmWmsStockTransMain = AscmWmsStockTransMainService.GetInstance().Get(id);
                ascmWmsStockTransMain.status = statusOption;
                ascmWmsStockTransMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ascmWmsStockTransMain.modifyUser = userName;
                AscmWmsStockTransMainService.GetInstance().Update(ascmWmsStockTransMain);
                jsonObjectResult.result = true;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WmsStockTransDelete(string mainIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (!string.IsNullOrEmpty(mainIds))
                {
                    string[] arrMainId = mainIds.Split(',');
                    try
                    {
                        foreach (string mainId in arrMainId)
                        {
                            if (string.IsNullOrEmpty(mainId))
                                continue;
                            AscmWmsStockTransMain ascmWmsStockTransMain = AscmWmsStockTransMainService.GetInstance().Get(Convert.ToInt32(mainId));
                            if (ascmWmsStockTransMain == null)
                                throw new Exception("获取转移单号失败！");

                            AscmWmsStockTransMainService.GetInstance().Delete(ascmWmsStockTransMain);
                            List<AscmWmsStockTransDetail> listDetail = AscmWmsStockTransDetailService.GetInstance().GetListByMainId(Convert.ToInt32(mainId));
                            AscmWmsStockTransDetailService.GetInstance().Delete(listDetail);
                        }
                        jsonObjectResult.result = true;
                    }
                    catch (Exception ex)
                    {
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete StockTransMain)", ex);
                        throw ex;
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

        #region 作业退料
        public ActionResult WmsJobMtlReturnGenerate()
        {
            return View();
        }
        //public ActionResult WipDiscreteJobsToMtlReturnAdd1(string wipEntityId, string queryWarehouseId, string queryWipSupplyType, 
        //    string queryStartMaterialDocNumber, string queryEndMaterialDocNumber,
        //    string startScheduledStartDate, string endScheduledStartDate,
        //    string startScheduleGroupName, string endScheduleGroupName)
        //{
        //    JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
        //    try
        //    {
        //        string whereOther = "";
        //        string whereWipSupplyType = "", whereSupplySubInventory = "", whereInventoryItem = "";
        //        string whereWipEntityId = "";
        //        if (!string.IsNullOrEmpty(wipEntityId))
        //            whereWipEntityId = " wipEntityId =" + wipEntityId;
        //        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWipEntityId);
                
        //        //供应类型
        //        if (queryWipSupplyType == "pullType")
        //            whereWipSupplyType = " wipSupplyType in(2,3) ";
        //        else if (queryWipSupplyType == "pushType")
        //            whereWipSupplyType = "wipSupplyType in(1)";
        //        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWipSupplyType);
                
        //        //供应子库：领料单与MES交互时，子库是单一的
        //        if (!string.IsNullOrEmpty(queryWarehouseId))
        //        {
        //            whereSupplySubInventory = "supplySubinventory='" + queryWarehouseId + "'";
        //            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplySubInventory);
        //        }
        //        //物料编码
        //        if (queryStartMaterialDocNumber != null && queryStartMaterialDocNumber != "")
        //        {
        //            whereInventoryItem = "inventoryItemId in(select id from AscmMaterialItem where docNumber>='" + queryStartMaterialDocNumber.Trim() + "')";
        //            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereInventoryItem);
        //        }
        //        if (queryEndMaterialDocNumber != null && queryEndMaterialDocNumber != "")
        //        {
        //            whereInventoryItem = "inventoryItemId in(select id from AscmMaterialItem where docNumber<'" + queryEndMaterialDocNumber.Trim() + "')";
        //            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereInventoryItem);
        //        }

        //        //作业日期
        //        string whereWipJob = "";
        //        string whereStartScheduledStartDate = "", whereEndScheduledStartDate = "";
        //        DateTime dtStartScheduledStartDate, dtEndScheduledStartDate;
        //        if (!string.IsNullOrEmpty(startScheduledStartDate) && DateTime.TryParse(startScheduledStartDate, out dtStartScheduledStartDate))
        //        {
        //            whereStartScheduledStartDate = "scheduledStartDate>='" + dtStartScheduledStartDate.ToString("yyyy-MM-dd 00:00:00") + "'";
        //        }
        //        if (!string.IsNullOrEmpty(endScheduledStartDate) && DateTime.TryParse(endScheduledStartDate, out dtEndScheduledStartDate)) 
        //        {
        //            whereEndScheduledStartDate = "scheduledStartDate<'" + dtEndScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
        //        }

        //        whereWipJob = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipJob, whereStartScheduledStartDate);
        //        whereWipJob = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipJob, whereEndScheduledStartDate);

        //        //计划组、车间(比较计划组名称)
        //        string whereScheduleGroupOther = "";
        //        string whereStartScheduleGroup = "", whereEndScheduleGroup = "";
        //        if (!string.IsNullOrEmpty(startScheduleGroupName))
        //            whereStartScheduleGroup = "scheduleGroupName='" + startScheduleGroupName + "'";
        //        if (!string.IsNullOrEmpty(endScheduleGroupName))
        //        {
        //            if (!string.IsNullOrEmpty(startScheduleGroupName))
        //                whereStartScheduleGroup = "translate(schedulegroupname, '一二三四五六七八九', '123456789')>=translate('" + startScheduleGroupName + "', '一二三四五六七八九', '123456789')";
        //            whereEndScheduleGroup = "translate(schedulegroupname, '一二三四五六七八九', '123456789')<=translate('" + endScheduleGroupName + "', '一二三四五六七八九', '123456789')";
        //        }
        //        whereScheduleGroupOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduleGroupOther, whereStartScheduleGroup);
        //        whereScheduleGroupOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduleGroupOther, whereEndScheduleGroup);
        //        List<AscmWipScheduleGroups> listWipScheduleGroups = null;
        //        if (string.IsNullOrEmpty(whereScheduleGroupOther))
        //        {
        //            //电装、总装计划组
        //            List<string> listScheduleGroupNames = new List<string>();
        //            listScheduleGroupNames.AddRange(AscmWipScheduleGroups.TypeDefine.GetWipScheduleGroupNames(AscmWipScheduleGroups.TypeDefine.GA));
        //            listScheduleGroupNames.AddRange(AscmWipScheduleGroups.TypeDefine.GetWipScheduleGroupNames(AscmWipScheduleGroups.TypeDefine.QC));
        //            listWipScheduleGroups = AscmWipScheduleGroupsService.GetInstance().GetList(null, "", "", "", "scheduleGroupName in('" + string.Join("','", listScheduleGroupNames) + "')");
        //        }
        //        else 
        //        {
        //            listWipScheduleGroups = AscmWipScheduleGroupsService.GetInstance().GetList(null, "", "", "", whereScheduleGroupOther);
        //        }

        //        if (listWipScheduleGroups != null && listWipScheduleGroups.Count > 0) 
        //        {
        //            whereWipJob = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "scheduleGroupId in(" + string.Join(",", listWipScheduleGroups.Select(P => P.scheduleGroupId)) + ")");
        //        }     

        //        //作业BOM
        //        List<AscmWipRequirementOperations> listAscmWipRequirementOperations =
        //            AscmWipRequirementOperationsService.GetInstance().GetList(null, "", "", "", whereOther, whereWipJob);
        //        if (listAscmWipRequirementOperations != null)
        //        {
        //            List<AscmLocationMaterialLink> listLocationMaterialLink = null;
        //            if (!string.IsNullOrEmpty(queryWarehouseId))
        //                listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetListByWarehouseId(queryWarehouseId);
        //            else
        //            {
        //                var ieWarehouseIds = listAscmWipRequirementOperations.Select(P => P.supplySubinventory).Distinct();
        //                if (ieWarehouseIds != null)
        //                {
        //                    string warehouseIds = string.Empty;
        //                    foreach (string warehouseId in ieWarehouseIds)
        //                    {
        //                        if (!string.IsNullOrEmpty(warehouseId))
        //                        {
        //                            if (!string.IsNullOrEmpty(warehouseIds))
        //                                warehouseIds += ",";
        //                            warehouseIds += "'" + warehouseId + "'";
        //                        }
        //                    }
        //                    if (!string.IsNullOrEmpty(warehouseIds))
        //                        listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetListByWarehouseIds(warehouseIds);
        //                }
        //            }

        //            foreach (AscmWipRequirementOperations ascmWipRequirementOperations in listAscmWipRequirementOperations)
        //            {
        //                AscmWmsMtlReturnDetail wmsMtlReturnDetail = new AscmWmsMtlReturnDetail();
        //                wmsMtlReturnDetail.materialId = ascmWipRequirementOperations.inventoryItemId;
        //                wmsMtlReturnDetail.warehouseId = ascmWipRequirementOperations.supplySubinventory;
        //                wmsMtlReturnDetail.requiredQuantity = ascmWipRequirementOperations.requiredQuantity;
        //                wmsMtlReturnDetail.quantityIssued = ascmWipRequirementOperations.quantityIssued;
        //                wmsMtlReturnDetail.quantity = 0;//退货数量
        //                //wmsMtlReturnDetail.warelocationDoc = null;
        //                wmsMtlReturnDetail.ascmMaterialItem = ascmWipRequirementOperations.ascmMaterialItem;
        //                wmsMtlReturnDetail.wipEntityId = ascmWipRequirementOperations.wipEntityId;
        //                //if (listLocationMaterialLink != null)
        //                //{
        //                //    AscmLocationMaterialLink locationMaterialLink =
        //                //        listLocationMaterialLink.Find(P => P.materialId == ascmWipRequirementOperations.inventoryItemId && P.warehouseId == ascmWipRequirementOperations.supplySubinventory);
        //                //    if (locationMaterialLink != null)
        //                //    {
        //                //        wmsMtlReturnDetail.warelocationId = locationMaterialLink.warelocationId;
        //                //        wmsMtlReturnDetail.ascmWarelocation = locationMaterialLink.ascmWarelocation;
        //                //    }
        //                //}
        //                jsonDataGridResult.rows.Add(wmsMtlReturnDetail);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult WipDiscreteJobsToMtlReturnAdd1(int? page, int? rows, string sort, string order, string queryWord, string wipSupplyType,
            string startSupplySubInventory, string endSupplySubInventory, string startScheduleGroupName, string endScheduleGroupName,
            string startCreateTime, string endCreateTime, string pattern,string startWipEntityName, string endWipEntityName, 
            string startScheduledStartDate, string endScheduledStartDate, string startMaterialDocNumber, string endMaterialDocNumber)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            string whereOther = "", whereDetail = "",whereBom = "";
            //备料单生成日期
            string whereStartCreateTime = "", whereEndCreateTime = "";
            DateTime dtStartCreateTime, dtEndCreateTime;
            if (!string.IsNullOrEmpty(startCreateTime) && DateTime.TryParse(startCreateTime, out dtStartCreateTime))
            {
                whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                whereEndCreateTime = "createTime<'" + dtStartCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            }
            if (!string.IsNullOrEmpty(endCreateTime) && DateTime.TryParse(endCreateTime, out dtEndCreateTime))
                whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);
            //备料单备料方式
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "pattern='" + pattern + "'");

            //供应类型
            string whereWipSupplyType = "";
            if (wipSupplyType == "pullType") //拉式
                whereWipSupplyType = "wipSupplyType in(" + AscmMaterialItem.WipSupplyTypeDefine.zpls + "," + AscmMaterialItem.WipSupplyTypeDefine.lsgx + ")";
            else if (wipSupplyType == "pushType") //推式
                whereWipSupplyType = "wipSupplyType in(" + AscmMaterialItem.WipSupplyTypeDefine.ts + ")";
            if (!string.IsNullOrEmpty(whereWipSupplyType))
                whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, whereWipSupplyType);

            //供应子库
            string whereStartSupplySubInventory = "", whereEndSupplySubInventory = "";
            if (!string.IsNullOrEmpty(startSupplySubInventory))
                whereStartSupplySubInventory = "supplySubinventory='" + startSupplySubInventory + "'";
            if (!string.IsNullOrEmpty(endSupplySubInventory))
            {
                if (!string.IsNullOrEmpty(startSupplySubInventory))
                    whereStartSupplySubInventory = "supplySubinventory>='" + startSupplySubInventory + "'";
                whereEndSupplySubInventory = "supplySubinventory<='" + endSupplySubInventory + "'";
            }
            whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, whereStartSupplySubInventory);
            whereBom = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBom, whereEndSupplySubInventory);
            if (!string.IsNullOrEmpty(whereBom))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "wipEntityId in (select wipEntityId from AscmWipRequirementOperations where " + whereBom + ")");

            //物料编码
            string whereMaterialDocNumber = "";
            string whereStartMaterialDocNumber = "", whereEndMaterialDocNumber = "";
            if (!string.IsNullOrEmpty(startMaterialDocNumber))
                whereStartMaterialDocNumber = "upper(docNumber)='" + startMaterialDocNumber.ToUpper() + "'";
            if (!string.IsNullOrEmpty(endMaterialDocNumber))
            {
                if (!string.IsNullOrEmpty(startMaterialDocNumber))
                    whereStartMaterialDocNumber = "upper(docNumber)>='" + startMaterialDocNumber.ToUpper() + "'";
                whereEndMaterialDocNumber = "upper(docNumber)<='" + endMaterialDocNumber.ToUpper() + "'";
            }
            whereMaterialDocNumber = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMaterialDocNumber, whereStartMaterialDocNumber);
            whereMaterialDocNumber = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMaterialDocNumber, whereEndMaterialDocNumber);
            if (!string.IsNullOrEmpty(whereMaterialDocNumber))
                whereDetail = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereDetail, "materialId in(select id from AscmMaterialItem where " + whereMaterialDocNumber + ")");
            
            bool isWipJob = pattern == AscmWmsPreparationMain.PatternDefine.wipJob;
            
            //作业号
            string whereWipEntityName = "";
            string whereStartWipEntityName = "", whereEndWipEntityName = "";
            if (!string.IsNullOrEmpty(startWipEntityName))
                whereStartWipEntityName = "name='" + startWipEntityName + "'";
            if (!string.IsNullOrEmpty(endWipEntityName))
            {
                if (!string.IsNullOrEmpty(startWipEntityName))
                    whereStartWipEntityName = "name>='" + startWipEntityName + "'";
                whereEndWipEntityName = "name<='" + endWipEntityName + "'";
            }
            whereWipEntityName = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntityName, whereStartWipEntityName);
            whereWipEntityName = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntityName, whereEndWipEntityName);
            if (!string.IsNullOrEmpty(whereWipEntityName))
            {
                //内、外机作业分开查询
                AscmWipEntities.WipEntityType bWipEntityType = AscmWipEntitiesService.GetInstance().GetWipEntityType(whereStartWipEntityName);
                AscmWipEntities.WipEntityType eWipEntityType = AscmWipEntitiesService.GetInstance().GetWipEntityType(whereEndWipEntityName);
                if (bWipEntityType == AscmWipEntities.WipEntityType.withinTheMachine && bWipEntityType == eWipEntityType)
                    whereEndWipEntityName = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereEndWipEntityName, "instr(upper(substr(name,1,(length(name)-3))),'N',-1) > instr(upper(substr(name,1,(length(name)-3))),'W',-1)");
                else if (eWipEntityType == AscmWipEntities.WipEntityType.outsideTheMachine && bWipEntityType == eWipEntityType)
                    whereEndWipEntityName = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereEndWipEntityName, "instr(upper(substr(name,1,(length(name)-3))),'W',-1) > instr(upper(substr(name,1,(length(name)-3))),'N',-1)");

                string whereWipEntityId = "wipEntityId in(select wipEntityId from AscmWipEntities where " + whereWipEntityName + ")";
                if (isWipJob)
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWipEntityId);
                else
                    whereDetail = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereDetail, whereWipEntityId);
            }
            //作业日期
            string whereScheduledStartDate = "";
            string whereStartScheduledStartDate = "", whereEndScheduledStartDate = "";
            DateTime dtStartScheduledStartDate, dtEndScheduledStartDate;
            if (!string.IsNullOrEmpty(startScheduledStartDate) && DateTime.TryParse(startScheduledStartDate, out dtStartScheduledStartDate))
            {
                whereStartScheduledStartDate = "scheduledStartDate>='" + dtStartScheduledStartDate.ToString("yyyy-MM-dd 00:00:00") + "'";
                whereEndScheduledStartDate = "scheduledStartDate<'" + dtStartScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            }
            if (!string.IsNullOrEmpty(endScheduledStartDate) && DateTime.TryParse(endScheduledStartDate, out dtEndScheduledStartDate))
                whereEndScheduledStartDate = "scheduledStartDate<'" + dtEndScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            whereScheduledStartDate = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduledStartDate, whereStartScheduledStartDate);
            whereScheduledStartDate = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduledStartDate, whereEndScheduledStartDate);
            if (!string.IsNullOrEmpty(whereScheduledStartDate))
            {
                string whereWipEntityId = "wipEntityId in(select wipEntityId from AscmWipDiscreteJobs where " + whereScheduledStartDate + ")";
                if (isWipJob)
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWipEntityId);
                else
                    whereDetail = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereDetail, whereWipEntityId);
            }
            if (!string.IsNullOrEmpty(whereDetail))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "id in(select mainId from AscmWmsPreparationDetail where " + whereDetail + ")");

            //计划组、车间(比较计划组名称)
            string whereScheduleGroupOther = "";
            string whereStartScheduleGroup = "", whereEndScheduleGroup = "";
            if (!string.IsNullOrEmpty(startScheduleGroupName))
                whereStartScheduleGroup = "scheduleGroupName='" + startScheduleGroupName + "'";
            if (!string.IsNullOrEmpty(endScheduleGroupName))
            {
                if (!string.IsNullOrEmpty(startScheduleGroupName))
                    whereStartScheduleGroup = "translate(schedulegroupname, '一二三四五六七八九', '123456789')>=translate('" + startScheduleGroupName + "', '一二三四五六七八九', '123456789')";
                whereEndScheduleGroup = "translate(schedulegroupname, '一二三四五六七八九', '123456789')<=translate('" + endScheduleGroupName + "', '一二三四五六七八九', '123456789')";
            }
            whereScheduleGroupOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduleGroupOther, whereStartScheduleGroup);
            whereScheduleGroupOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduleGroupOther, whereEndScheduleGroup);
            List<AscmWipScheduleGroups> listWipScheduleGroups = null;
            if (string.IsNullOrEmpty(whereScheduleGroupOther))
            {
                //电装、总装计划组
                List<string> listScheduleGroupNames = new List<string>();
                listScheduleGroupNames.AddRange(AscmWipScheduleGroups.TypeDefine.GetWipScheduleGroupNames(AscmWipScheduleGroups.TypeDefine.GA));
                listScheduleGroupNames.AddRange(AscmWipScheduleGroups.TypeDefine.GetWipScheduleGroupNames(AscmWipScheduleGroups.TypeDefine.QC));
                listWipScheduleGroups = AscmWipScheduleGroupsService.GetInstance().GetList(null, "", "", "", "scheduleGroupName in('" + string.Join("','", listScheduleGroupNames) + "')");
            }
            else
                listWipScheduleGroups = AscmWipScheduleGroupsService.GetInstance().GetList(null, "", "", "", whereScheduleGroupOther);
            if (listWipScheduleGroups != null && listWipScheduleGroups.Count > 0)
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "wipEntityId in(select wipEntityId from AscmWipDiscreteJobs where scheduleGroupId in(" + string.Join(",", listWipScheduleGroups.Select(P => P.scheduleGroupId)) + "))");

            //备料单批量执行"设置货位"
            //List<AscmWmsPreparationMain> listAscmWmsPreparationMain = AscmWmsPreparationMainService.GetInstance().GetList(null, sort, order, queryWord, whereOther);
            //WmsPreparationDetailBatchSetLocation(listAscmWmsPreparationMain);

            List<AscmWmsPreparationMain> list = AscmWmsPreparationMainService.GetInstance().GetList(ynPage, sort, order, queryWord, whereOther);
            if (list != null)
            {
                if (isWipJob)
                    AscmWmsPreparationMainService.GetInstance().SetWipDiscreteJobs(list);
                AscmWmsPreparationMainService.GetInstance().SetDetailInfo(list, !isWipJob, !isWipJob, !isWipJob, !isWipJob);
                list.ForEach(P => jsonDataGridResult.rows.Add(P));
            }
            jsonDataGridResult.total = ynPage.GetRecordCount();
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WmsJobMtlReturnIndex(string WipEntityName, string WarehouseId, string WipSupplyType, string StartMaterialDocNumber, string EndMaterialDocNumber)
        {
            ViewData["WipEntityName"] = WipEntityName;
            ViewData["WarehouseId"] = WarehouseId;
            ViewData["WipSupplyType"] = WipSupplyType;
            ViewData["StartMaterialDocNumber"] = StartMaterialDocNumber;
            ViewData["EndMaterialDocNumber"] = EndMaterialDocNumber;

            List<SelectListItem> listReturnArea = new List<SelectListItem>();
            foreach (string returnArea in AscmWmsMtlReturnMain.ReturnAreaDefine.GetList())
            {
                string text = AscmWmsMtlReturnMain.ReturnAreaDefine.DisplayText(returnArea);
                listReturnArea.Add(new SelectListItem { Text = text, Value = returnArea });
            }
            ViewData["listReturnArea"] = listReturnArea;

            return View();
        }
        public ActionResult WmsJobMtlReturnEdit(int? id)
        {
            AscmWmsMtlReturnMain ascmWmsMtlReturnMain = null;
            try
            {
                if (id.HasValue)
                {
                    ascmWmsMtlReturnMain = AscmWmsMtlReturnMainService.GetInstance().Get(id.Value);
                    YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight();
                    ynWebRight.rightAdd = false;
                    ynWebRight.rightDelete = false;
                    ynWebRight.rightEdit = false;
                }
                // 退料区域
                List<SelectListItem> listReturnArea = new List<SelectListItem>();
                string defaultReturnArea = AscmWmsMtlReturnMain.ReturnAreaDefine.onWip;
                if (ascmWmsMtlReturnMain != null)
                    defaultReturnArea = ascmWmsMtlReturnMain.returnArea;
                foreach (string returnArea in AscmWmsMtlReturnMain.ReturnAreaDefine.GetList())
                {
                    string text = AscmWmsMtlReturnMain.ReturnAreaDefine.DisplayText(returnArea);
                    bool isSelected = defaultReturnArea == returnArea;
                    listReturnArea.Add(new SelectListItem { Text = text, Value = returnArea, Selected = isSelected });
                }
                ViewData["listReturnArea"] = listReturnArea;
                ascmWmsMtlReturnMain = ascmWmsMtlReturnMain ?? new AscmWmsMtlReturnMain();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ascmWmsMtlReturnMain);
        }
        [HttpPost]
        public ActionResult WmsJobMtlReturnSave(AscmWmsMtlReturnMain ascmWmsMtlReturnMain_Model, int? id, string detailJson)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(detailJson))
                    throw new Exception("系统错误：作业退料单明细参数不能为NULL或空");
                List<AscmWmsMtlReturnDetail> listAscmWmsMtlReturnDetail_Model = JsonConvert.DeserializeObject<List<AscmWmsMtlReturnDetail>>(detailJson);
                if (listAscmWmsMtlReturnDetail_Model == null)
                    throw new Exception("系统错误：作业退料单明细参数序列化错误");

                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;
                #endregion

                #region 主表
                AscmWmsMtlReturnMain ascmWmsMtlReturnMain = null;
                List<AscmWmsMtlReturnDetail> listAscmWmsMtlReturnDetail = null;
                if (id.HasValue)
                {
                    ascmWmsMtlReturnMain = AscmWmsMtlReturnMainService.GetInstance().Get(id.Value);
                    listAscmWmsMtlReturnDetail = AscmWmsMtlReturnDetailService.GetInstance().GetListByMainId(id.Value);
                }
                else
                {
                    ascmWmsMtlReturnMain = new AscmWmsMtlReturnMain();
                    ascmWmsMtlReturnMain.organizationId = 775;
                    ascmWmsMtlReturnMain.createUser = userName;
                    ascmWmsMtlReturnMain.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsMtlReturnMain");
                    ascmWmsMtlReturnMain.id = ++maxId;
                    //获取MES闭环单号
                    ascmWmsMtlReturnMain.docNumber = AscmMesService.GetInstance().GetMesReturnedBillNo();
                    if (string.IsNullOrEmpty(ascmWmsMtlReturnMain.docNumber))
                        throw new Exception("系统错误：从MES获取退料单闭环单号失败");
                }
                if (ascmWmsMtlReturnMain == null)
                    throw new Exception("提交作业退料单失败！");

                ascmWmsMtlReturnMain.modifyUser = userName;
                ascmWmsMtlReturnMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ascmWmsMtlReturnMain.warehouseId = ascmWmsMtlReturnMain_Model.warehouseId;
                ascmWmsMtlReturnMain.returnArea = ascmWmsMtlReturnMain_Model.returnArea;
                ascmWmsMtlReturnMain.wipEntityId = ascmWmsMtlReturnMain_Model.wipEntityId;
                ascmWmsMtlReturnMain.reasonId = ascmWmsMtlReturnMain_Model.reasonId;
                if (!string.IsNullOrEmpty(ascmWmsMtlReturnMain_Model.memo))
                    ascmWmsMtlReturnMain.memo = ascmWmsMtlReturnMain_Model.memo.Trim();
                ascmWmsMtlReturnMain.billType = AscmWmsMtlReturnMain.BillTypeDefine.wipEntity;
                #endregion

                #region 明细
                int maxId_Detail = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsMtlReturnDetail");
                List<AscmWmsMtlReturnDetail> listAscmWmsMtlReturnDetailAdd = new List<AscmWmsMtlReturnDetail>();
                List<AscmWmsMtlReturnDetail> listAscmWmsMtlReturnDetailUpdate = new List<AscmWmsMtlReturnDetail>();
                List<AscmWmsMtlReturnDetail> listAscmWmsMtlReturnDetailDelete = new List<AscmWmsMtlReturnDetail>();

                int iRec = 0;
                foreach (AscmWmsMtlReturnDetail ascmWmsMtlReturnDetail_Model in listAscmWmsMtlReturnDetail_Model)
                {
                    AscmWmsMtlReturnDetail wmsMtlReturnDetail = null;
                    if (ascmWmsMtlReturnDetail_Model.id < 0)
                    {
                        wmsMtlReturnDetail = new AscmWmsMtlReturnDetail();
                        wmsMtlReturnDetail.id = ++maxId_Detail;
                        wmsMtlReturnDetail.createUser = userName;
                        wmsMtlReturnDetail.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        wmsMtlReturnDetail.mainId = ascmWmsMtlReturnMain.id;
                        listAscmWmsMtlReturnDetailAdd.Add(wmsMtlReturnDetail);
                    }
                    else if (listAscmWmsMtlReturnDetail != null)
                    {
                        wmsMtlReturnDetail = listAscmWmsMtlReturnDetail.Find(P => P.id == ascmWmsMtlReturnDetail_Model.id);
                        if (wmsMtlReturnDetail == null)
                            throw new Exception("系统错误：作业退料明细异常！");
                        listAscmWmsMtlReturnDetailUpdate.Add(wmsMtlReturnDetail);
                    }
                    wmsMtlReturnDetail.modifyUser = userName;
                    wmsMtlReturnDetail.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    wmsMtlReturnDetail.materialId = ascmWmsMtlReturnDetail_Model.materialId;
                    wmsMtlReturnDetail.warelocationId = ascmWmsMtlReturnDetail_Model.warelocationId;
                    wmsMtlReturnDetail.quantity = ascmWmsMtlReturnDetail_Model.quantity;
                    wmsMtlReturnDetail.requiredQuantity = ascmWmsMtlReturnDetail_Model.requiredQuantity;
                    wmsMtlReturnDetail.quantityIssued = ascmWmsMtlReturnDetail_Model.quantityIssued;
                    iRec++;
                }
                if (listAscmWmsMtlReturnDetail != null)
                {
                    foreach (AscmWmsMtlReturnDetail wmsMtlReturnDetail in listAscmWmsMtlReturnDetail)
                    {
                        AscmWmsMtlReturnDetail ascmWmsMtlReturnDetail_Model = listAscmWmsMtlReturnDetail_Model.Find(P => P.id == wmsMtlReturnDetail.id);
                        if (ascmWmsMtlReturnDetail_Model == null)
                            listAscmWmsMtlReturnDetailDelete.Add(wmsMtlReturnDetail);
                    }
                }
                #endregion

                #region 物料增加
                List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList(null, "", "", "", "");
                List<AscmLocationMaterialLink> listLocationMaterialLinkUpdate = new List<AscmLocationMaterialLink>();
                List<AscmLocationMaterialLink> listLocationMaterialLinkAdd = new List<AscmLocationMaterialLink>();
                if (listAscmWmsMtlReturnDetail_Model != null && listAscmWmsMtlReturnDetail_Model.Count() > 0)
                {
                    //int maxId_Link = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmLocationMaterialLink");
                    foreach (AscmWmsMtlReturnDetail wmsMtlReturnDetail in listAscmWmsMtlReturnDetail_Model)
                    {
                        AscmLocationMaterialLink locationMaterialLink = listLocationMaterialLink.Find(P => P.pk.warelocationId == wmsMtlReturnDetail.warelocationId && P.pk.materialId == wmsMtlReturnDetail.materialId);
                        if (locationMaterialLink == null)
                        {
                            locationMaterialLink = new AscmLocationMaterialLink();
                            locationMaterialLink.pk = new AscmLocationMaterialLinkPK { warelocationId = wmsMtlReturnDetail.warelocationId, materialId = wmsMtlReturnDetail.materialId };
                            //locationMaterialLink.id = ++maxId_Link;
                            locationMaterialLink.organizationId = 775;
                            locationMaterialLink.createUser = userName;
                            locationMaterialLink.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            locationMaterialLink.quantity = wmsMtlReturnDetail.quantity;
                            //locationMaterialLink.warelocationId = wmsMtlReturnDetail.warelocationId;
                            //locationMaterialLink.materialId = wmsMtlReturnDetail.materialId;
                            listLocationMaterialLinkAdd.Add(locationMaterialLink);
                        }
                        else
                        {
                            locationMaterialLink.quantity = locationMaterialLink.quantity + wmsMtlReturnDetail.quantity;
                            listLocationMaterialLinkUpdate.Add(locationMaterialLink);
                        }
                        locationMaterialLink.modifyUser = userName;
                        locationMaterialLink.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                #endregion

                #region 保存
                AscmMesInteractiveLog ascmMesInteractiveLog = new AscmMesInteractiveLog();
                ascmMesInteractiveLog.billId = ascmWmsMtlReturnMain.id;
                ascmMesInteractiveLog.docNumber = ascmWmsMtlReturnMain.docNumber;
                ascmMesInteractiveLog.billType = AscmMesInteractiveLog.BillTypeDefine.mtlReturnManual;
                ascmMesInteractiveLog.createUser = userName;
                ascmMesInteractiveLog.modifyUser = userName;
                ascmMesInteractiveLog.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmMesInteractiveLog.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                //执行事务
                YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().Clear();
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        AscmMesService.GetInstance().DoMaterialReturn(ascmWmsMtlReturnMain, listAscmWmsMtlReturnDetail_Model, userName, ascmMesInteractiveLog);
                        if (ascmMesInteractiveLog.returnCode == "0")
                        {
                            if (id.HasValue)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmWmsMtlReturnMain);
                            }
                            else
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsMtlReturnMain);
                            }
                            if (listAscmWmsMtlReturnDetailAdd != null && listAscmWmsMtlReturnDetailAdd.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsMtlReturnDetailAdd);
                            }
                            if (listAscmWmsMtlReturnDetailUpdate != null && listAscmWmsMtlReturnDetailUpdate.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmWmsMtlReturnDetailUpdate);
                            }
                            if (listAscmWmsMtlReturnDetailDelete != null && listAscmWmsMtlReturnDetailDelete.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsMtlReturnDetailDelete);
                            }
                            if (listLocationMaterialLinkAdd != null && listLocationMaterialLinkAdd.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listLocationMaterialLinkAdd);
                            }
                            if (listLocationMaterialLinkUpdate != null && listLocationMaterialLinkUpdate.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listLocationMaterialLinkUpdate);
                            }
                            ascmMesInteractiveLog.returnMessage = "";
                        }

                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        ascmMesInteractiveLog.returnMessage = ex.Message;
                    }
                }
                #endregion

                //int maxLogId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmMesInteractiveLog");
                string maxLogIdKey = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("ascm_mes_interactive_log_id", "", "", 10);
                int maxLogId = Convert.ToInt32(maxLogIdKey);
                ascmMesInteractiveLog.id = maxLogId;
                AscmMesInteractiveLogService.GetInstance().Save(ascmMesInteractiveLog);
                string errorMsg = string.Empty;
                if (!string.IsNullOrEmpty(ascmMesInteractiveLog.returnMessage))
                {
                    errorMsg += string.Format("<li>作业退料<font color='red'>{0}</font>：{1}</li>", ascmMesInteractiveLog.docNumber, ascmMesInteractiveLog.returnMessage);
                }
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    errorMsg = string.Format("<ul>{0}</ul>", errorMsg);
                    jsonObjectResult.message = errorMsg;
                }
                else
                {
                    jsonObjectResult.result = true;
                    jsonObjectResult.id = ascmWmsMtlReturnMain.id.ToString();
                    jsonObjectResult.entity = ascmWmsMtlReturnMain;
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult WipRequireOperationToMtlReturnAdd(string wipRequireOperationRows)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (string.IsNullOrEmpty(wipRequireOperationRows))
                    throw new Exception("系统错误：参数传值错误");

                List<AscmWipRequirementOperations> listWipRequirementOperations =
                        JsonConvert.DeserializeObject<List<AscmWipRequirementOperations>>(wipRequireOperationRows);
                if (listWipRequirementOperations == null)
                    throw new Exception("系统错误：参数序列化失败");

                foreach (AscmWipRequirementOperations ascmWipRequirementOperations in listWipRequirementOperations)
                {
                    AscmWmsMtlReturnDetail wmsMtlReturnDetail = new AscmWmsMtlReturnDetail();
                    wmsMtlReturnDetail.materialId = ascmWipRequirementOperations.inventoryItemId;
                    wmsMtlReturnDetail.ascmMaterialItem = ascmWipRequirementOperations.ascmMaterialItem;
                    jsonDataGridResult.rows.Add(wmsMtlReturnDetail);
                }
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WipDiscreteJobsToMtlReturnAdd(string wipEntityName, string queryWarehouseId, string queryWipSupplyType)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "";
                string whereWipSupplyType = "", whereSupplySubInventory = "";
                //string whereInventoryItem = "";
                string whereWipEntityId = "";
                if (!string.IsNullOrEmpty(wipEntityName))
                    whereWipEntityId = " wipEntityId = (select wipEntityId from AscmWipEntities where name= '" + wipEntityName + "')";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWipEntityId);
                //供应类型
                if (queryWipSupplyType == "pullType")
                    whereWipSupplyType = "wipSupplyType in(2,3)";
                else if (queryWipSupplyType == "pushType")
                    whereWipSupplyType = "wipSupplyType not in(2,3)";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWipSupplyType);
                //供应子库：领料单与MES交互时，子库是单一的
                if (!string.IsNullOrEmpty(queryWarehouseId))
                {
                    whereSupplySubInventory = "supplySubinventory='" + queryWarehouseId + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplySubInventory);
                }
                //物料编码
                //if (queryStartMaterialDocNumber != null && queryStartMaterialDocNumber != "")
                //{
                //    whereInventoryItem = "inventoryItemId in(select id from AscmMaterialItem where docNumber>='" + queryStartMaterialDocNumber.Trim() + "')";
                //    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereInventoryItem);
                //}
                //if (queryEndMaterialDocNumber != null && queryEndMaterialDocNumber != "")
                //{
                //    whereInventoryItem = "inventoryItemId in(select id from AscmMaterialItem where docNumber<'" + queryEndMaterialDocNumber.Trim() + "')";
                //    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereInventoryItem);
                //}

                List<AscmWipRequirementOperations> listAscmWipRequirementOperations =
                    AscmWipRequirementOperationsService.GetInstance().GetList(null, "", "", "", whereOther, "");
                if (listAscmWipRequirementOperations != null)
                {
                    List<AscmLocationMaterialLink> listLocationMaterialLink = null;
                    if (!string.IsNullOrEmpty(queryWarehouseId))
                        listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetListByWarehouseId(queryWarehouseId);
                    else
                    {
                        var ieWarehouseIds = listAscmWipRequirementOperations.Select(P => P.supplySubinventory).Distinct();
                        if (ieWarehouseIds != null)
                        {
                            string warehouseIds = string.Empty;
                            foreach (string warehouseId in ieWarehouseIds)
                            {
                                if (!string.IsNullOrEmpty(warehouseId))
                                {
                                    if (!string.IsNullOrEmpty(warehouseIds))
                                        warehouseIds += ",";
                                    warehouseIds += "'" + warehouseId + "'";
                                }
                            }
                            if (!string.IsNullOrEmpty(warehouseIds))
                                listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetListByWarehouseIds(warehouseIds);
                        }
                    }

                    foreach (AscmWipRequirementOperations ascmWipRequirementOperations in listAscmWipRequirementOperations)
                    {
                        AscmWmsMtlReturnDetail wmsMtlReturnDetail = new AscmWmsMtlReturnDetail();
                        wmsMtlReturnDetail.materialId = ascmWipRequirementOperations.inventoryItemId;
                        wmsMtlReturnDetail.warehouseId = ascmWipRequirementOperations.supplySubinventory;
                        wmsMtlReturnDetail.requiredQuantity = ascmWipRequirementOperations.requiredQuantity;
                        wmsMtlReturnDetail.quantityIssued = ascmWipRequirementOperations.quantityIssued;
                        wmsMtlReturnDetail.quantity = 0;//退货数量
                        wmsMtlReturnDetail.warelocationDoc = null;
                        wmsMtlReturnDetail.ascmMaterialItem = ascmWipRequirementOperations.ascmMaterialItem;
                        wmsMtlReturnDetail.wipEntityId = ascmWipRequirementOperations.wipEntityId;
                        if (listLocationMaterialLink != null)
                        {
                            AscmLocationMaterialLink locationMaterialLink =
                                listLocationMaterialLink.Find(P => P.pk.materialId == ascmWipRequirementOperations.inventoryItemId && P.warehouseId == ascmWipRequirementOperations.supplySubinventory);
                            if (locationMaterialLink != null)
                            {
                                wmsMtlReturnDetail.warelocationId = locationMaterialLink.pk.warelocationId;
                                //wmsMtlReturnDetail.ascmWarelocation = locationMaterialLink.ascmWarelocation;
                                wmsMtlReturnDetail.ascmWarelocation = new AscmWarelocation { id = locationMaterialLink.pk.warelocationId, docNumber = locationMaterialLink.locationDocNumber, warehouseId = locationMaterialLink.warehouseId };
                            }
                        }
                        jsonDataGridResult.rows.Add(wmsMtlReturnDetail);
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

        #region ERP退料单退料
        public ActionResult WmsErpMtlReturnEdit(int? id)
        {
            AscmWmsMtlReturnMain ascmWmsMtlReturnMain = null;
            try
            {
                if (id.HasValue)
                {
                    ascmWmsMtlReturnMain = AscmWmsMtlReturnMainService.GetInstance().Get(id.Value);
                    if (ascmWmsMtlReturnMain != null)
                    {
                        AscmCuxWipReleaseHeaders ascmCuxWipReleaseHeaders = AscmCuxWipReleaseHeadersService.GetInstance().Get(ascmWmsMtlReturnMain.releaseHeaderId);
                        ascmWmsMtlReturnMain.ascmCuxWipReleaseHeaders = ascmCuxWipReleaseHeaders;
                    }
                    YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight();
                    ynWebRight.rightAdd = false;
                    ynWebRight.rightDelete = false;
                    ynWebRight.rightEdit = false;
                }
                // 退料区域
                List<SelectListItem> listReturnArea = new List<SelectListItem>();
                string defaultReturnArea = AscmWmsMtlReturnMain.ReturnAreaDefine.onWip;
                if (ascmWmsMtlReturnMain != null)
                    defaultReturnArea = ascmWmsMtlReturnMain.returnArea;
                foreach (string returnArea in AscmWmsMtlReturnMain.ReturnAreaDefine.GetList())
                {
                    string text = AscmWmsMtlReturnMain.ReturnAreaDefine.DisplayText(returnArea);
                    bool isSelected = defaultReturnArea == returnArea;
                    listReturnArea.Add(new SelectListItem { Text = text, Value = returnArea, Selected = isSelected });
                }
                ViewData["listReturnArea"] = listReturnArea;
                ascmWmsMtlReturnMain = ascmWmsMtlReturnMain ?? new AscmWmsMtlReturnMain();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ascmWmsMtlReturnMain);
        }
        public ActionResult WipReleaseLineToMtlReturnAdd(string wipReleaseLineRows)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (string.IsNullOrEmpty(wipReleaseLineRows))
                    throw new Exception("系统错误：参数传值错误");

                List<AscmCuxWipReleaseLines> listWipReleaseLines =
                        JsonConvert.DeserializeObject<List<AscmCuxWipReleaseLines>>(wipReleaseLineRows);
                if (listWipReleaseLines == null)
                    throw new Exception("系统错误：参数序列化失败");

                List<AscmLocationMaterialLink> list_AscmLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList("from AscmLocationMaterialLink", true, true);
                foreach (AscmCuxWipReleaseLines ascmCuxWipReleaseLines in listWipReleaseLines)
                {
                    AscmWmsMtlReturnDetail wmsMtlReturnDetail = new AscmWmsMtlReturnDetail();
                    wmsMtlReturnDetail.materialId = ascmCuxWipReleaseLines.inventoryItemId;
                    wmsMtlReturnDetail.ascmMaterialItem = ascmCuxWipReleaseLines.ascmMaterialItem;
                    wmsMtlReturnDetail.warehouseId = ascmCuxWipReleaseLines.subInventory;
                    wmsMtlReturnDetail.printQuantity = ascmCuxWipReleaseLines.printQuantity;
                    var find_LocationMaterialLink = list_AscmLocationMaterialLink.Find(item => item.pk.materialId == ascmCuxWipReleaseLines.ascmMaterialItem.id && item.warehouseId == ascmCuxWipReleaseLines.subInventory);
                    if (find_LocationMaterialLink != null)
                    {
                        wmsMtlReturnDetail.warelocationId = find_LocationMaterialLink.pk.warelocationId;
                        //wmsMtlReturnDetail.ascmWarelocation = find_LocationMaterialLink.ascmWarelocation;
                        wmsMtlReturnDetail.ascmWarelocation = new AscmWarelocation { id = find_LocationMaterialLink.pk.warelocationId, docNumber = find_LocationMaterialLink.locationDocNumber, warehouseId = find_LocationMaterialLink.warehouseId };
                    }
                    jsonDataGridResult.rows.Add(wmsMtlReturnDetail);
                }
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WipReleaseLineToMtlReturnSave(AscmWmsMtlReturnMain ascmWmsMtlReturnMain_Model, int? id, string detailJson)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(detailJson))
                    throw new Exception("系统错误： ERP退料单明细参数不能为NULL或空");
                List<AscmWmsMtlReturnDetail> listAscmWmsMtlReturnDetail_Model = JsonConvert.DeserializeObject<List<AscmWmsMtlReturnDetail>>(detailJson);
                if (listAscmWmsMtlReturnDetail_Model == null)
                    throw new Exception("系统错误：ERP退料单明细参数序列化错误");

                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;
                #endregion

                #region 主表
                AscmWmsMtlReturnMain ascmWmsMtlReturnMain = null;
                List<AscmWmsMtlReturnDetail> listAscmWmsMtlReturnDetail = null;
                if (id.HasValue)
                {
                    ascmWmsMtlReturnMain = AscmWmsMtlReturnMainService.GetInstance().Get(id.Value);
                    listAscmWmsMtlReturnDetail = AscmWmsMtlReturnDetailService.GetInstance().GetListByMainId(id.Value);
                }
                else
                {
                    ascmWmsMtlReturnMain = new AscmWmsMtlReturnMain();
                    ascmWmsMtlReturnMain.organizationId = 775;
                    ascmWmsMtlReturnMain.createUser = userName;
                    ascmWmsMtlReturnMain.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsMtlReturnMain");
                    ascmWmsMtlReturnMain.id = ++maxId;
                    //获取MES闭环单号
                    ascmWmsMtlReturnMain.docNumber = AscmMesService.GetInstance().GetMesReturnedBillNo();
                    if (string.IsNullOrEmpty(ascmWmsMtlReturnMain.docNumber))
                        throw new Exception("系统错误：从MES获取退料单闭环单号失败");
                }
                if (ascmWmsMtlReturnMain == null)
                    throw new Exception("提交ERP退料单失败！");

                AscmCuxWipReleaseHeaders ascmCuxWipReleaseHeaders = AscmCuxWipReleaseHeadersService.GetInstance().Get(ascmWmsMtlReturnMain_Model.releaseHeaderId);
                ascmWmsMtlReturnMain.ascmCuxWipReleaseHeaders = ascmCuxWipReleaseHeaders;

                ascmWmsMtlReturnMain.modifyUser = userName;
                ascmWmsMtlReturnMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ascmWmsMtlReturnMain.warehouseId = ascmWmsMtlReturnMain_Model.warehouseId;
                ascmWmsMtlReturnMain.returnArea = ascmWmsMtlReturnMain_Model.returnArea;
                ascmWmsMtlReturnMain.releaseHeaderId = ascmWmsMtlReturnMain_Model.releaseHeaderId;
                ascmWmsMtlReturnMain.reasonId = ascmWmsMtlReturnMain_Model.reasonId;
                if (!string.IsNullOrEmpty(ascmWmsMtlReturnMain_Model.memo))
                    ascmWmsMtlReturnMain.memo = ascmWmsMtlReturnMain_Model.memo.Trim();
                ascmWmsMtlReturnMain.billType = AscmWmsMtlReturnMain.BillTypeDefine.erpReturnBill;
                #endregion

                #region 明细
                int maxId_Detail = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsMtlReturnDetail");
                List<AscmWmsMtlReturnDetail> listAscmWmsMtlReturnDetailAdd = new List<AscmWmsMtlReturnDetail>();
                List<AscmWmsMtlReturnDetail> listAscmWmsMtlReturnDetailUpdate = new List<AscmWmsMtlReturnDetail>();
                List<AscmWmsMtlReturnDetail> listAscmWmsMtlReturnDetailDelete = new List<AscmWmsMtlReturnDetail>();

                int iRec = 0;
                foreach (AscmWmsMtlReturnDetail ascmWmsMtlReturnDetail_Model in listAscmWmsMtlReturnDetail_Model)
                {
                    AscmWmsMtlReturnDetail wmsMtlReturnDetail = null;
                    if (ascmWmsMtlReturnDetail_Model.id < 0)
                    {
                        wmsMtlReturnDetail = new AscmWmsMtlReturnDetail();
                        wmsMtlReturnDetail.id = ++maxId_Detail;
                        wmsMtlReturnDetail.createUser = userName;
                        wmsMtlReturnDetail.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        wmsMtlReturnDetail.mainId = ascmWmsMtlReturnMain.id;
                        listAscmWmsMtlReturnDetailAdd.Add(wmsMtlReturnDetail);
                    }
                    else if (listAscmWmsMtlReturnDetail != null)
                    {
                        wmsMtlReturnDetail = listAscmWmsMtlReturnDetail.Find(P => P.id == ascmWmsMtlReturnDetail_Model.id);
                        if (wmsMtlReturnDetail == null)
                            throw new Exception("系统错误：ERP退料明细异常！");
                        listAscmWmsMtlReturnDetailUpdate.Add(wmsMtlReturnDetail);
                    }
                    wmsMtlReturnDetail.modifyUser = userName;
                    wmsMtlReturnDetail.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    wmsMtlReturnDetail.materialId = ascmWmsMtlReturnDetail_Model.materialId;
                    wmsMtlReturnDetail.warehouseId = ascmWmsMtlReturnDetail_Model.warehouseId;
                    wmsMtlReturnDetail.warelocationId = ascmWmsMtlReturnDetail_Model.warelocationId;
                    wmsMtlReturnDetail.quantity = ascmWmsMtlReturnDetail_Model.quantity;
                    wmsMtlReturnDetail.requiredQuantity = ascmWmsMtlReturnDetail_Model.requiredQuantity;
                    wmsMtlReturnDetail.quantityIssued = ascmWmsMtlReturnDetail_Model.quantityIssued;
                    iRec++;
                }
                if (listAscmWmsMtlReturnDetail != null)
                {
                    foreach (AscmWmsMtlReturnDetail wmsMtlReturnDetail in listAscmWmsMtlReturnDetail)
                    {
                        AscmWmsMtlReturnDetail ascmWmsMtlReturnDetail_Model = listAscmWmsMtlReturnDetail_Model.Find(P => P.id == wmsMtlReturnDetail.id);
                        if (ascmWmsMtlReturnDetail_Model == null)
                            listAscmWmsMtlReturnDetailDelete.Add(wmsMtlReturnDetail);
                    }
                }
                #endregion

                #region 物料增加
                List<AscmLocationMaterialLink> listLocationMaterialLink = AscmLocationMaterialLinkService.GetInstance().GetList(null, "", "", "", "");
                List<AscmLocationMaterialLink> listLocationMaterialLinkUpdate = new List<AscmLocationMaterialLink>();
                List<AscmLocationMaterialLink> listLocationMaterialLinkAdd = new List<AscmLocationMaterialLink>();
                if (listAscmWmsMtlReturnDetail_Model != null && listAscmWmsMtlReturnDetail_Model.Count() > 0)
                {
                    //int maxId_Link = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmLocationMaterialLink");
                    foreach (AscmWmsMtlReturnDetail wmsMtlReturnDetail in listAscmWmsMtlReturnDetail_Model)
                    {
                        AscmLocationMaterialLink locationMaterialLink = listLocationMaterialLink.Find(P => P.pk.warelocationId == wmsMtlReturnDetail.warelocationId && P.pk.materialId == wmsMtlReturnDetail.materialId);
                        if (locationMaterialLink == null)
                        {
                            locationMaterialLink = new AscmLocationMaterialLink();
                            locationMaterialLink.pk = new AscmLocationMaterialLinkPK { warelocationId = wmsMtlReturnDetail.warelocationId, materialId = wmsMtlReturnDetail.materialId };
                            //locationMaterialLink.id = ++maxId_Link;
                            locationMaterialLink.organizationId = 775;
                            locationMaterialLink.createUser = userName;
                            locationMaterialLink.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            locationMaterialLink.quantity = wmsMtlReturnDetail.quantity;
                            //locationMaterialLink.warelocationId = wmsMtlReturnDetail.warelocationId;
                            //locationMaterialLink.materialId = wmsMtlReturnDetail.materialId;
                            listLocationMaterialLinkAdd.Add(locationMaterialLink);
                        }
                        else
                        {
                            locationMaterialLink.quantity = locationMaterialLink.quantity + wmsMtlReturnDetail.quantity;
                            listLocationMaterialLinkUpdate.Add(locationMaterialLink);
                        }
                        locationMaterialLink.modifyUser = userName;
                        locationMaterialLink.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                #endregion

                #region 保存
                AscmMesInteractiveLog ascmMesInteractiveLog = new AscmMesInteractiveLog();
                ascmMesInteractiveLog.billId = ascmWmsMtlReturnMain.id;
                ascmMesInteractiveLog.docNumber = ascmWmsMtlReturnMain.docNumber;
                ascmMesInteractiveLog.billType = AscmMesInteractiveLog.BillTypeDefine.mtlReturnSystem;
                ascmMesInteractiveLog.createUser = userName;
                ascmMesInteractiveLog.modifyUser = userName;
                ascmMesInteractiveLog.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmMesInteractiveLog.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                //执行事务
                YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().Clear();
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        AscmMesService.GetInstance().DoSysReturn(ascmWmsMtlReturnMain, listAscmWmsMtlReturnDetail_Model, userName, ascmMesInteractiveLog);
                        if (ascmMesInteractiveLog.returnCode == "0")
                        {
                            if (id.HasValue)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmWmsMtlReturnMain);
                            }
                            else
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsMtlReturnMain);
                            }
                            if (listAscmWmsMtlReturnDetailAdd != null && listAscmWmsMtlReturnDetailAdd.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsMtlReturnDetailAdd);
                            }
                            if (listAscmWmsMtlReturnDetailUpdate != null && listAscmWmsMtlReturnDetailUpdate.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmWmsMtlReturnDetailUpdate);
                            }
                            if (listAscmWmsMtlReturnDetailDelete != null && listAscmWmsMtlReturnDetailDelete.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsMtlReturnDetailDelete);
                            }
                            if (listLocationMaterialLinkAdd != null && listLocationMaterialLinkAdd.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listLocationMaterialLinkAdd);
                            }
                            if (listLocationMaterialLinkUpdate != null && listLocationMaterialLinkUpdate.Count > 0)
                            {
                                YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listLocationMaterialLinkUpdate);
                            }
                            ascmMesInteractiveLog.returnMessage = "";
                        }

                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        ascmMesInteractiveLog.returnMessage = ex.Message;
                    }
                }
                #endregion

                int maxLogId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmMesInteractiveLog");
                ascmMesInteractiveLog.id = ++maxLogId;
                AscmMesInteractiveLogService.GetInstance().Save(ascmMesInteractiveLog);
                string errorMsg = string.Empty;
                if (!string.IsNullOrEmpty(ascmMesInteractiveLog.returnMessage))
                {
                    errorMsg += string.Format("<li>ERP退料单退料<font color='red'>{0}</font>：{1}</li>", ascmMesInteractiveLog.docNumber, ascmMesInteractiveLog.returnMessage);
                }
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    errorMsg = string.Format("<ul>{0}</ul>", errorMsg);
                    jsonObjectResult.message = errorMsg;
                }
                else
                {
                    jsonObjectResult.result = true;
                    jsonObjectResult.id = ascmWmsMtlReturnMain.id.ToString();
                    jsonObjectResult.entity = ascmWmsMtlReturnMain;
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        #endregion

        #region 作业退料查询
        public ActionResult WmsJobMtlReturnQuery()
        {
            ViewData["dtStart"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewData["dtEnd"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }
		public ActionResult WmsJobMtlReturnMainList(int? page, int? rows, string sort, string order, string queryWord, string wipEntityName, string startPlanTime, string endPlanTime, string returnCode)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmWmsMtlReturnMain> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereWipEntityId = "", whereStartModifyTime = "", whereEndModifyTime = "", whereBillType = "";

                if (!string.IsNullOrEmpty(wipEntityName))
                {
                    whereWipEntityId = " wipEntityId in(select wipEntityId from AscmWipEntities where name like %'" + wipEntityName + "%')";
                }
                whereBillType = "billType='" + AscmWmsMtlReturnMain.BillTypeDefine.wipEntity + "'";

                DateTime dtStartPlanTime, dtEndPlanTime;
                if (!string.IsNullOrEmpty(startPlanTime) && DateTime.TryParse(startPlanTime, out dtStartPlanTime))
                    whereStartModifyTime = "modifyTime>='" + dtStartPlanTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endPlanTime) && DateTime.TryParse(endPlanTime, out dtEndPlanTime))
                    whereEndModifyTime = "modifyTime<'" + dtEndPlanTime.ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWipEntityId);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereBillType);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartModifyTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndModifyTime);
				if (!string.IsNullOrEmpty(returnCode))
				{
					if (returnCode == "0")
					{
						whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "returnCode='0'");
					}
					else
					{
						whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "returnCode is not null and returnCode!='0'");
					}
				}

                list = AscmWmsMtlReturnMainService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                list = list.OrderBy(item => item.modifyTime).ToList();
                if (list != null && list.Count() > 0)
                {
                    foreach (AscmWmsMtlReturnMain ascmWmsMtlReturnMain in list)
                    {
                        jsonDataGridResult.rows.Add(ascmWmsMtlReturnMain);
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
        public ActionResult WmsJobMtlReturnMainView(int? id)
        {
            AscmWmsMtlReturnMain ascmWmsMtlReturnMain = null;
            try
            {
                ascmWmsMtlReturnMain = AscmWmsMtlReturnMainService.GetInstance().Get(id.Value);
                AscmWipEntities ascmWipEntities = AscmWipEntitiesService.GetInstance().Get(ascmWmsMtlReturnMain.wipEntityId);
                ascmWmsMtlReturnMain.ascmWipEntities = ascmWipEntities;
                AscmMtlTransactionReasons ascmMtlTransactionReasons = AscmMtlTransactionReasonsService.GetInstance().Get(ascmWmsMtlReturnMain.reasonId);
                ascmWmsMtlReturnMain.ascmMtlTransactionReasons = ascmMtlTransactionReasons;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ascmWmsMtlReturnMain);
        }
        public ActionResult WmsJobMtlReturnDetialList(int? id)
        {
            List<AscmWmsMtlReturnDetail> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmWmsMtlReturnDetailService.GetInstance().GetList(null, "", "", id.Value, "", "");
                foreach (AscmWmsMtlReturnDetail wmsMtlReturnDetail in list)
                {
                    jsonDataGridResult.rows.Add(wmsMtlReturnDetail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 事务原因
        public ActionResult TransactionReasonAscxList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                List<AscmMtlTransactionReasons> list = AscmMtlTransactionReasonsService.GetInstance().GetList(ynPage, "", "", queryWord, null);
                if (list != null)
                {
                    foreach (AscmMtlTransactionReasons ascmMtlTransactionReasons in list)
                    {
                        jsonDataGridResult.rows.Add(ascmMtlTransactionReasons);
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

        #region 仓库员物料绑定
        public ActionResult WmsKeeperMaterialIndex()
        {
            return View();
        }
        #endregion

        #region 计划组、车间
        public ActionResult WipScheduleGroupAscxList(int? page, int? rows, string sort, string order, string q, bool fromMonitor = false)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            string sortName = "translate(schedulegroupname, '一二三四五六七八九', '123456789')";
            string whereOther = string.Empty;
            if (fromMonitor)
            {
                List<string> listScheduleGroupNames = new List<string>();
                listScheduleGroupNames.AddRange(AscmWipScheduleGroups.TypeDefine.GetWipScheduleGroupNames(AscmWipScheduleGroups.TypeDefine.GA));
                listScheduleGroupNames.AddRange(AscmWipScheduleGroups.TypeDefine.GetWipScheduleGroupNames(AscmWipScheduleGroups.TypeDefine.QC));
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "scheduleGroupName in('" + string.Join("','", listScheduleGroupNames) + "')");
            }
            List<AscmWipScheduleGroups> list = AscmWipScheduleGroupsService.GetInstance().GetList(ynPage, sortName, "", q, whereOther);
            if (list != null)
                jsonDataGridResult.rows.AddRange(list);
            jsonDataGridResult.total = ynPage.GetRecordCount();
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 作业号
        public ActionResult WipEntityComboList(int? page, int? rows, string sort, string order, string q,
            string startScheduledStartDate, string endScheduledStartDate,
            string startScheduleGroupName, string endScheduleGroupName,
            bool filterWipEntityStatus, bool isMultiGroupName)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            string whereOther = "";
            //作业状态
            if (filterWipEntityStatus)
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "statusType in(" + AscmWipDiscreteJobs.StatusTypeDefine.yff + "," + AscmWipDiscreteJobs.StatusTypeDefine.wc + "," + AscmWipDiscreteJobs.StatusTypeDefine.wcbjf + ")");
            //作业日期
            string whereStartScheduledStartDate = "", whereEndScheduledStartDate = "";
            DateTime dtStartScheduledStartDate, dtEndScheduledStartDate;
            if (!string.IsNullOrEmpty(startScheduledStartDate) && DateTime.TryParse(startScheduledStartDate, out dtStartScheduledStartDate))
            {
                whereStartScheduledStartDate = "scheduledStartDate>='" + dtStartScheduledStartDate.ToString("yyyy-MM-dd 00:00:00") + "'";
                whereEndScheduledStartDate = "scheduledStartDate<'" + dtStartScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            }
            if (!string.IsNullOrEmpty(endScheduledStartDate) && DateTime.TryParse(endScheduledStartDate, out dtEndScheduledStartDate))
                whereEndScheduledStartDate = "scheduledStartDate<'" + dtEndScheduledStartDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartScheduledStartDate);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndScheduledStartDate);
            //计划组、车间(比较计划组名称)
            string whereScheduleGroupOther = "";
            string whereStartScheduleGroup = "", whereEndScheduleGroup = "";

            if (isMultiGroupName && !string.IsNullOrEmpty(startScheduleGroupName))
            {
                whereScheduleGroupOther = " scheduleGroupName in (" + startScheduleGroupName + ") ";
            }
            else 
            {
                if (!string.IsNullOrEmpty(startScheduleGroupName))
                    whereStartScheduleGroup = "scheduleGroupName='" + startScheduleGroupName + "'";
                if (!string.IsNullOrEmpty(endScheduleGroupName))
                {
                    if (!string.IsNullOrEmpty(startScheduleGroupName))
                        whereStartScheduleGroup = "translate(schedulegroupname, '一二三四五六七八九', '123456789')>=translate('" + startScheduleGroupName + "', '一二三四五六七八九', '123456789')";
                    whereEndScheduleGroup = "translate(schedulegroupname, '一二三四五六七八九', '123456789')<=translate('" + endScheduleGroupName + "', '一二三四五六七八九', '123456789')";
                }
                whereScheduleGroupOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduleGroupOther, whereStartScheduleGroup);
                whereScheduleGroupOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereScheduleGroupOther, whereEndScheduleGroup);
            }

            List<AscmWipScheduleGroups> listWipScheduleGroups = AscmWipScheduleGroupsService.GetInstance().GetList(null, "", "", "", whereScheduleGroupOther);
            if (listWipScheduleGroups != null && listWipScheduleGroups.Count > 0)
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "scheduleGroupId in(" + string.Join(",", listWipScheduleGroups.Select(P => P.scheduleGroupId)) + ")");
            if (!string.IsNullOrEmpty(whereOther))
                whereOther = "wipEntityId in(select wipEntityId from AscmWipDiscreteJobs where " + whereOther + ")";

            List<AscmWipEntities> list = AscmWipEntitiesService.GetInstance().GetList(ynPage, sort, order, q, whereOther);
            if (list != null && list.Count() > 0)
                list.ForEach(P => jsonDataGridResult.rows.Add(P));
            jsonDataGridResult.total = ynPage.GetRecordCount();
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadWipEntityCombo(string q)
        {
            int? page = 1;
            int? rows = 200;
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmWipEntities> list = AscmWipEntitiesService.GetInstance().GetList(ynPage, "name", "asc", q + "%", "");
            if (list == null)
                list = new List<AscmWipEntities>();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MES事务类型
        public ActionResult MesTransStyleList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                List<AscmMesTransStyle> list = AscmMesTransStyleService.GetInstance().GetList(ynPage, sort, order, queryWord, "");
                if (list != null)
                {
                    foreach (AscmMesTransStyle ascmMesTransStyle in list)
                    {
                        jsonDataGridResult.rows.Add(ascmMesTransStyle);
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

        #region MES接口日志查询
        public ActionResult AscmMesInteractiveLogQuery()
        {
            ViewData["dtStart"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewData["dtEnd"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }

        public ActionResult AscmMesInteractiveLogList(int? page, int? rows, string sort, string order, string queryWord, string startPlanTime, string endPlanTime, string billType, string returnCode)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = string.Empty;
                DateTime dtStartPlanTime, dtEndPlanTime;
                if (!string.IsNullOrEmpty(startPlanTime) && DateTime.TryParse(startPlanTime, out dtStartPlanTime))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "modifyTime>='" + dtStartPlanTime.ToString("yyyy-MM-dd 00:00:00") + "'");
                if (!string.IsNullOrEmpty(endPlanTime) && DateTime.TryParse(endPlanTime, out dtEndPlanTime))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "modifyTime<'" + dtEndPlanTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'");
                if (!string.IsNullOrEmpty(billType))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "billType='" + billType + "'");

                if (!string.IsNullOrEmpty(returnCode))
                {
                    if (returnCode == "0")
                    {
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "returnCode='0'");
                    }
                    else
                    {
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "returnCode!='0'");
                    }
                }

                List<AscmMesInteractiveLog> list = AscmMesInteractiveLogService.GetInstance().GetList(ynPage, sort, order, queryWord, whereOther);
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
        
        public ActionResult AscmMesInteractiveLogExport(string queryWord, string startPlanTime, string endPlanTime, string billType)
        {
            IWorkbook wb = new NPOI.XSSF.UserModel.XSSFWorkbook(); //new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Sheet1");
            sheet.SetColumnWidth(0, 12 * 256);
            sheet.SetColumnWidth(1, 16 * 256);
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 18 * 256);
            sheet.SetColumnWidth(4, 10 * 256);

            IRow titleRow = sheet.CreateRow(0);
            titleRow.Height = 20 * 20;
            titleRow.CreateCell(0).SetCellValue("单据号");
            titleRow.CreateCell(1).SetCellValue("单据类型");
            titleRow.CreateCell(2).SetCellValue("返回信息");
            titleRow.CreateCell(3).SetCellValue("更新时间");
            titleRow.CreateCell(4).SetCellValue("更新人");

            string fileDownloadName = "MES接口日志";

            string whereOther = string.Empty;
            DateTime dtStartPlanTime, dtEndPlanTime;
            if (!string.IsNullOrEmpty(startPlanTime) && DateTime.TryParse(startPlanTime, out dtStartPlanTime))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "modifyTime>='" + dtStartPlanTime.ToString("yyyy-MM-dd 00:00:00") + "'");
            if (!string.IsNullOrEmpty(endPlanTime) && DateTime.TryParse(endPlanTime, out dtEndPlanTime))
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "modifyTime<'" + dtEndPlanTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'");
            if (!string.IsNullOrEmpty(billType))
            {
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "billType='" + billType + "'");
                fileDownloadName += "【" + AscmMesInteractiveLog.BillTypeDefine.DisplayText(billType) + "】";
            }
            List<AscmMesInteractiveLog> list = AscmMesInteractiveLogService.GetInstance().GetList(null, "", "", queryWord, whereOther);
            if (list != null)
            {
                int rowIndex = 0;
                foreach (AscmMesInteractiveLog mesInteractiveLog in list)
                {
                    IRow row = sheet.CreateRow(++rowIndex);
                    row.Height = 20 * 20;
                    row.CreateCell(0).SetCellValue(mesInteractiveLog.docNumber);
                    row.CreateCell(1).SetCellValue(mesInteractiveLog.billTypeCn);
                    row.CreateCell(2).SetCellValue(mesInteractiveLog.returnMessage);
                    row.CreateCell(3).SetCellValue(mesInteractiveLog.modifyTime);
                    row.CreateCell(4).SetCellValue(mesInteractiveLog.modifyUser);
                }
            }

            byte[] buffer = new byte[] { };
            using (System.IO.MemoryStream stream = new MemoryStream())
            {
                wb.Write(stream);
                buffer = stream.GetBuffer();
            }
            return File(buffer, "application/vnd.ms-excel", fileDownloadName + ".xlsx");
        }
        #endregion

        #region 仓库人员--组别与用户
        // Methods
        public ActionResult WhTeamUserIndex()
        {
            return base.View();
        }

        public ActionResult WhTeamDelete(int? id)
        {
            Exception exception;
            JsonObjectResult data = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    using (ITransaction transaction = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            AscmWhTeamService.Instance.Delete(id.Value);
                            transaction.Commit();
                            data.result = true;
                            data.message = "";
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            transaction.Rollback();
                            throw exception;
                        }
                    }
                }
                else
                {
                    data.message = "传入参数[id=null]错误！";
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                data.message = exception.Message;
            }

            return base.Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WhTeamEdit(int? id)
        {
            AscmWhTeam team = null;
            try
            {
                if (id.HasValue)
                {
                    team = AscmWhTeamService.Instance.Get(id.Value);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return base.Json(team.GetOwner(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult WhTeamList(string queryWord)
        {
            List<AscmWhTeam> list = null;
            JsonDataGridResult data = new JsonDataGridResult();
            try
            {
                StringBuilder strWhere  = new StringBuilder();

                if (!string.IsNullOrEmpty(queryWord))
                {
                    queryWord = queryWord.Replace("'", "");
                    strWhere.AppendFormat(" and (name like '%{0}%' or description like '%{0}%') ", queryWord);
                }
                list = AscmWhTeamService.Instance.GetList(strWhere.ToString());
                foreach (AscmWhTeam team in list)
                {
                    data.rows.Add(team.GetOwner());
                }
                data.total = list.Count;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return base.Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ContentResult WhTeamSave(AscmWhTeam ascmWhTeam_Model, int? id)
        {
            JsonObjectResult result = new JsonObjectResult();
            try
            {
                object obj2;
                int num;
                AscmWhTeam ascmWhTeam = null;
                if (id.HasValue)
                {
                    ascmWhTeam = AscmWhTeamService.Instance.Get(id.Value);
                }
                else
                {
                    ascmWhTeam = new AscmWhTeam();
                }
                if (ascmWhTeam == null)
                {
                    throw new Exception("保存组别失败！");
                }
                if ((ascmWhTeam_Model.name == null) || (ascmWhTeam_Model.name.Trim() == ""))
                {
                    throw new Exception("组别名称不能为空！");
                }
                
                ascmWhTeam.name = ascmWhTeam_Model.name;
                ascmWhTeam.sortNo = ascmWhTeam_Model.sortNo;
                ascmWhTeam.description = ascmWhTeam_Model.description;
                if (ascmWhTeam.organizationId == 0) 
                {
                    string ticketOrganizationId = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketOrganizationId();
                    int orgId = 0;
                    int.TryParse(ticketOrganizationId, out orgId);
                    ascmWhTeam.organizationId = orgId;
                }

                if (id.HasValue)
                {
                    obj2 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(string.Concat(new object[] { "select count(*) from AscmWhTeam where name='", ascmWhTeam_Model.name.Trim(), "' and id<>", id.Value }), null);
                    if (obj2 == null)
                    {
                        throw new Exception("查询异常！");
                    }
                    num = 0;
                    if (int.TryParse(obj2.ToString(), out num) && (num > 0))
                    {
                        throw new Exception("已经存在此组别【" + ascmWhTeam_Model.name.Trim() + "】！");
                    }
                    AscmWhTeamService.Instance.Update(ascmWhTeam);
                }
                else
                {
                    obj2 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmWhTeam where name='" + ascmWhTeam_Model.name.Trim() + "'", null);
                    if (obj2 == null)
                    {
                        throw new Exception("查询异常！");
                    }
                    num = 0;
                    if (int.TryParse(obj2.ToString(), out num) && (num > 0))
                    {
                        throw new Exception("已经存在此组别【" + ascmWhTeam_Model.name.Trim() + "】！");
                    }
                    AscmWhTeamService.Instance.Save(ascmWhTeam);
                }
                result.result = true;
                result.message = "";
                result.id = ascmWhTeam.id.ToString();
            }
            catch (Exception exception)
            {
                result.message = exception.Message;
            }
            string content = JsonConvert.SerializeObject(result);
            return base.Content(content);
        }

        public ActionResult WhTeamRemoveUser(int? teamId, string userIds)
        {
            JsonObjectResult data = new JsonObjectResult();
            try
            {
                if (!teamId.HasValue)
                {
                    throw new Exception("请选择组别!");
                }
                if (!string.IsNullOrEmpty(userIds))
                {
                    List<AscmWhTeamUser> list = new List<AscmWhTeamUser>();
                    string[] strArray = userIds.Split(new char[] { ',' });
                    foreach (string str in strArray)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            AscmWhTeamUserPK mypk = new AscmWhTeamUserPK
                            {
                                teamId = teamId.Value,
                                userId = str
                            };
                            AscmWhTeamUser item = new AscmWhTeamUser
                            {
                                pk = mypk
                            };
                            list.Add(item);
                        }
                    }
                    if (list.Count > 0)
                    {
                        AscmWhTeamUserService.Instance.Delete(list);
                    }
                    data.result = true;
                    data.message = "";
                }
            }
            catch (Exception exception)
            {
                data.message = exception.Message;
            }
            return base.Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectUserByQueryWord(int? page, int? rows, string sort, string order, string queryWord, int? organizationId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);
            List<YnUser> list = null;
            JsonDataGridResult data = new JsonDataGridResult();
            try
            {
                string ticketOrganizationId = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketOrganizationId();
                if (organizationId.HasValue)
                {
                    ticketOrganizationId = organizationId.ToString();
                }
                list = YnUserService.GetInstance().GetList(ticketOrganizationId, ynPage, sort, order, queryWord);
                foreach (YnUser user in list)
                {
                    data.rows.Add(user.GetOwner());
                }
                data.total = ynPage.GetRecordCount();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return base.Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WhTeamAddUser(int? teamId, string userIds)
        {
            JsonObjectResult data = new JsonObjectResult();
            try
            {
                if (teamId == null)
                {
                    throw new Exception("请选择组别!");
                }
                if (!string.IsNullOrEmpty(userIds))
                {
                    string ticketOrganizationId = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketOrganizationId();
                    int orgId = 0;
                    int.TryParse(ticketOrganizationId, out orgId);
                    string[] strArray = userIds.Split(new char[] { ',' });
                    List<string> listUserId = new List<string>(strArray);
                    bool isExistUserId = AscmWhTeamUserService.Instance.ExistUserId(listUserId);
                    if (!isExistUserId)
                    {
                        foreach (string str in strArray)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                AscmWhTeamUserPK mypk = new AscmWhTeamUserPK
                                {
                                    teamId = teamId.Value,
                                    userId = str
                                };
                                AscmWhTeamUser item = new AscmWhTeamUser
                                {
                                    pk = mypk,
                                    organizationId = orgId
                                };

                                AscmWhTeamUserService.Instance.Save(item);
                            }
                        }

                        data.result = true;
                        data.message = "";
                    }
                    else 
                    {
                        data.result = false;
                        data.message = "用户重复添加！";
                    }
                    
                }
            }
            catch (Exception exception)
            {
                data.message = exception.Message;
            }
            return base.Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TeamUserList(int? teamId)
        {
            List<AscmUserInfo> list = null;
            JsonDataGridResult data = new JsonDataGridResult();
            try
            {
                if (teamId == null)
                {
                    throw new Exception("请选择组别!");
                }

                list = AscmUserInfoService.GetInstance().GetList(teamId.Value);
                if (list == null) 
                {
                    list = new List<AscmUserInfo>();
                }

                foreach (AscmUserInfo user in list)
                {
                    data.rows.Add(user.GetOwner());
                }
                data.total = list.Count;
            }
            catch (Exception exception)
            {
                data.result = false;
                data.message = exception.Message;
            }
            return base.Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetWhTeamLeader(int? teamId, string userId)
        {
            JsonObjectResult data = new JsonObjectResult();
            try
            {
                if (!teamId.HasValue)
                {
                    throw new Exception("请选择组别!");
                }

                if (string.IsNullOrEmpty(userId)) 
                {
                    throw new Exception("请选择用户!");
                }

                List<AscmWhTeamUser> listTeamUser = AscmWhTeamUserService.Instance.GetList(teamId.Value);
                if (listTeamUser != null) 
                {
                    foreach (var item in listTeamUser)
                    {
                        if (item.M_UserId == userId)
                        {
                            item.isLeader = true;
                        }
                        else 
                        {
                            item.isLeader = false;
                        }

                        AscmWhTeamUserService.Instance.Update(item);
                    }
                }
                
                data.result = true;
                data.message = "";
            }
            catch (Exception exception)
            {
                data.message = exception.Message;
            }
            return base.Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
