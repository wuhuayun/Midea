﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YnFrame.Dal.Entities;
using YnBaseClass2.Web;
using MideaAscm.Services.Base;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Services.GetMaterialManage;
using YnFrame.Services;
using Newtonsoft.Json;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Services.FromErp;
using MideaAscm.Dal;
using NHibernate;
using System.IO;
using NPOI.SS.UserModel;
using System.Collections;
using System.Data;
using System.Text;

namespace MideaAscm.Areas.Ascm.Controllers
{
    public class LogisticsController : YnFrame.Controllers.YnBaseController
    {
        //
        // GET: /Ascm/Logistics/ 

        public ActionResult Index()
        {
            return View();
        }

        #region 物流组
        public ActionResult LogisticsClassIndex()
        {
            return View();
        }

        public ActionResult LogisticsClassList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmLogisticsClassInfo> list = null;

            try
            {
                list = AscmLogisticsClassInfoService.GetInstance().GetList(ynPage, "", "", queryWord, "");
                if (list != null && list.Count > 0)
                {
                    foreach (AscmLogisticsClassInfo ascmLogisticsClassInfo in list)
                    {
                        jsonDataGridResult.rows.Add(ascmLogisticsClassInfo);
                    }
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogisticsClassEdit(int? id)
        {
            AscmLogisticsClassInfo ascmLogisticsClassInfo = null;
            try
            {
                if (id.HasValue)
                {
                    ascmLogisticsClassInfo = AscmLogisticsClassInfoService.GetInstance().Get(id.Value);

                    if (!string.IsNullOrEmpty(ascmLogisticsClassInfo.groupLeader))
                    {
                        AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(ascmLogisticsClassInfo.groupLeader);
                        ascmLogisticsClassInfo.groupUser = ascmUserInfo;
                    }

                    if (!string.IsNullOrEmpty(ascmLogisticsClassInfo.monitorLeader))
                    {
                        AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(ascmLogisticsClassInfo.monitorLeader);
                        ascmLogisticsClassInfo.monitorUser = ascmUserInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(ascmLogisticsClassInfo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult LogisticsClassSave(AscmLogisticsClassInfo ascmLogisticsClassInfo_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();

            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                AscmLogisticsClassInfo ascmLogisticsClassInfo = null;
                if (id.HasValue)
                {
                    ascmLogisticsClassInfo = AscmLogisticsClassInfoService.GetInstance().Get(id.Value);
                    ascmLogisticsClassInfo.modifyUser = userName;
                    ascmLogisticsClassInfo.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmLogisticsClassInfo.logisticsName = ascmLogisticsClassInfo_Model.logisticsName;
                }
                else
                {
                    object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmLogisticsClassInfo where logisticsName = '" + ascmLogisticsClassInfo_Model.logisticsName + "'");
                    if (obj == null)
                        throw new Exception("查询失败！");
                    int iCount = 0;
                    if (int.TryParse(obj.ToString(), out iCount) && iCount > 0)
                        throw new Exception("该物流组信息已存在！");

                    ascmLogisticsClassInfo = new AscmLogisticsClassInfo();
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmLogisticsClassInfo");
                    ascmLogisticsClassInfo.id = ++maxId;
                    ascmLogisticsClassInfo.logisticsClass = ("Class" + ascmLogisticsClassInfo.id.ToString()).Trim().ToUpper();
                    ascmLogisticsClassInfo.logisticsName = ascmLogisticsClassInfo_Model.logisticsName;
                    ascmLogisticsClassInfo.createUser = userName;
                    ascmLogisticsClassInfo.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmLogisticsClassInfo.modifyUser = userName;
                    ascmLogisticsClassInfo.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }

                if (ascmLogisticsClassInfo == null)
                    throw new Exception("保存物流组信息失败！");

                if (string.IsNullOrEmpty(ascmLogisticsClassInfo_Model.logisticsName))
                    throw new Exception("物流组名称不能为空！");

                if (ascmLogisticsClassInfo_Model.groupLeader == null || ascmLogisticsClassInfo_Model.monitorLeader == null)
                    throw new Exception("物流组长及物流班长不能为空！");

                ascmLogisticsClassInfo.groupLeader = ascmLogisticsClassInfo_Model.groupLeader;
                ascmLogisticsClassInfo.monitorLeader = ascmLogisticsClassInfo_Model.monitorLeader;

                if (id.HasValue)
                    AscmLogisticsClassInfoService.GetInstance().Update(ascmLogisticsClassInfo);
                else
                    AscmLogisticsClassInfoService.GetInstance().Save(ascmLogisticsClassInfo);

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult LogisticsClassDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();

            try
            {
                if (!id.HasValue)
                    throw new Exception("传入参数[id=null]错误！");

                AscmLogisticsClassInfo ascmLogisticsClassInfo = AscmLogisticsClassInfoService.GetInstance().Get(id.Value);
                if (ascmLogisticsClassInfo == null)
                    throw new Exception("找不到匹配的车间信息！");

                AscmLogisticsClassInfoService.GetInstance().Delete(ascmLogisticsClassInfo);
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message; ;
            }

            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 物流组用户
        public ActionResult LogisticsYnUserIndex()
        {
            return View();
        }

        public ActionResult LogisticsYnUserList(int? page, int? rows, string sort, string order, string queryWord, string queryLogisticsClass)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();

            try
            {
                string userName = string.Empty;
                string userRole = string.Empty;
                string userLogisticsName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
                userLogisticsName = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName);

                string whereQueryWord = "";
                if ((userRole.IndexOf("组长") > -1 || userRole.IndexOf("班长") > -1) && !string.IsNullOrEmpty(userLogisticsName))
                {
                    whereQueryWord = "logisticsClass = '" + userLogisticsName + "'";
                }
                else if (string.IsNullOrEmpty(userLogisticsName) && !string.IsNullOrEmpty(queryLogisticsClass))
                {
                    whereQueryWord = "logisticsClass = '" + queryLogisticsClass + "'";
                }

                List<AscmUserInfo> list = AscmUserInfoService.GetInstance().GetList(ynPage, "", "", queryWord, whereQueryWord, "物流部");
                if (list != null && list.Count > 0)
                {
                    foreach (AscmUserInfo ascmUserInfo in list)
                    {
                        jsonDataGridResult.rows.Add(ascmUserInfo);
                    }
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                jsonDataGridResult.message = ex.Message;
                jsonDataGridResult.result = false;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetLogisticsYnUserClass(string id)
        {
            AscmUserInfo ascmUserInfo = null;

            try
            {
                if (!string.IsNullOrEmpty(id))
                    ascmUserInfo = AscmUserInfoService.GetInstance().Get(id);

                List<AscmUserInfo> list = new List<AscmUserInfo>();
                list.Add(ascmUserInfo);

                AscmUserInfoService.GetInstance().SetUserLogisticsClass(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(ascmUserInfo, JsonRequestBehavior.AllowGet);
        }
        public ContentResult SetLogisticsYnUserClass(string id, AscmUserInfo ascmUserInfo_Model)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();

            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                AscmUserInfo ascmUserInfo = null;
                if (!string.IsNullOrEmpty(id))
                {
                    ascmUserInfo = AscmUserInfoService.GetInstance().Get(id);
                    if (!string.IsNullOrEmpty(ascmUserInfo_Model.logisticsClass))
                        ascmUserInfo.logisticsClass = ascmUserInfo_Model.logisticsClass;

                    AscmUserInfoService.GetInstance().Update(ascmUserInfo);
                }

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmUserInfo.userId;
                jsonObjectResult.entity = ascmUserInfo;
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        #endregion

        #region 物流组车辆
        public ActionResult LogisticsForkliftIndex()
        {
            return View();
        }

        public ActionResult LogisticsForkliftList(int? page, int? rows, string sort, string order, string queryWord, string queryLogisticsClass, string queryType)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmForklift> list = null;

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                //string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
                string userLogisticsClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName);

                string whereOther = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryLogisticsClass) || !string.IsNullOrEmpty(userLogisticsClass))
                {
                    whereQueryWord = "logisticsClass = '" + queryLogisticsClass + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryType))
                {
                    whereQueryWord = "forkliftType = '" + queryType + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                list = AscmForkliftService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmForklift ascmForklift in list)
                    {
                        jsonDataGridResult.rows.Add(ascmForklift);
                    }
                }
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogisticsForkliftEdit(int? id)
        {
            AscmForklift ascmForklift = null;
            try
            {
                if (id.HasValue)
                {
                    ascmForklift = AscmForkliftService.GetInstance().Get(id.Value);
                    if (ascmForklift != null && !string.IsNullOrEmpty(ascmForklift.workerId))
                    {
                        AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(ascmForklift.workerId);

                        List<AscmUserInfo> listAscmUserInfo = new List<AscmUserInfo>();
                        listAscmUserInfo.Add(ascmUserInfo);
                        AscmUserInfoService.GetInstance().SetUserLogisticsClass(listAscmUserInfo);

                        AscmUserInfoService.GetInstance().SetUserLogisticsClass(listAscmUserInfo);
                        if (!string.IsNullOrEmpty(ascmForklift.workerId))
                            ascmForklift.ascmUserInfo = listAscmUserInfo.Find(e => e.userId == ascmForklift.workerId);
                        if (ascmForklift.ascmUserInfo != null)
                            ascmForklift.logisticsClassName = ascmForklift.ascmUserInfo.logisticsClassName;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmForklift, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult LogisticsForkliftSave(AscmForklift ascmForklift_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                AscmForklift ascmForklift = null;
                if (id.HasValue)
                {
                    ascmForklift = AscmForkliftService.GetInstance().Get(id.Value);
                    if (ascmForklift == null)
                        throw new Exception("保存领料车辆信息失败！");
                    ascmForklift.modifyUser = userName;
                    ascmForklift.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    ascmForklift = new AscmForklift();
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmForklift");
                    ascmForklift.id = ++maxId;
                    ascmForklift.workerId = ascmForklift_Model.workerId;
                    ascmForklift.createUser = userName;
                    ascmForklift.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmForklift.modifyUser = userName;
                    ascmForklift.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }

                if (ascmForklift_Model.forkliftType == null || ascmForklift_Model.forkliftType == "")
                    throw new Exception("领料车辆车种不能为空！");
                if (ascmForklift_Model.forkliftSpec == null || ascmForklift_Model.forkliftSpec == "")
                    throw new Exception("领料车辆规格不能为空！");

                ascmForklift.forkliftSpec = ascmForklift_Model.forkliftSpec;
                ascmForklift.forkliftType = ascmForklift_Model.forkliftType;
                ascmForklift.status = ascmForklift_Model.status;
                ascmForklift.assetsId = ascmForklift_Model.assetsId;

                ascmForklift.tagId = ascmForklift_Model.tagId;
                ascmForklift.forkliftNumber = ascmForklift_Model.forkliftNumber;
                ascmForklift.forkliftWay = ascmForklift_Model.forkliftWay;
                ascmForklift.actionLimits = ascmForklift_Model.actionLimits;
                ascmForklift.workContent = ascmForklift_Model.workContent;
                ascmForklift.tip = ascmForklift_Model.tip;

                if (id.HasValue)
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmForklift where assetsId ='" + ascmForklift_Model.assetsId + "' and id <>" + id.Value + " ");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已存在此领料车辆信息【" + ascmForklift_Model.assetsId + "】！");
                    AscmForkliftService.GetInstance().Update(ascmForklift);
                }
                else
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmForklift where assetsId = '" + ascmForklift_Model.assetsId + "'");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已存在此领料车辆信息【" + ascmForklift_Model.assetsId + "】！");
                    AscmForkliftService.GetInstance().Save(ascmForklift);
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmForklift.id.ToString();
                jsonObjectResult.entity = ascmForklift;
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult LogisticsForkliftDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (!id.HasValue)
                    throw new Exception("传入参数[id=null]错误！");

                AscmForklift ascmForklift = AscmForkliftService.GetInstance().Get(id.Value);
                if (ascmForklift == null)
                    throw new Exception("找不到领料车辆");
                AscmForkliftService.GetInstance().Delete(ascmForklift);

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 任务生成规则
        public ActionResult LogisticsGenerateTaskRuleIndex()
        {
            return View();
        }

        public ActionResult LogisticsGenerateTaskRuleList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                List<AscmGenerateTaskRule> list = AscmGenerateTaskRuleService.GetInstance().GetList(ynPage, "identificationId,id", "", queryWord, "");
                if (list != null && list.Count > 0)
                {
                    foreach (AscmGenerateTaskRule ascmGenerateTaskRule in list)
                    {
                        jsonDataGridResult.rows.Add(ascmGenerateTaskRule);
                    }
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                jsonDataGridResult.message = ex.Message;
                jsonDataGridResult.result = false;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogisticsGenerateTaskRuleEdit(int? id)
        {
            AscmGenerateTaskRule ascmGenerateTaskRule = null;
            try
            {
                if (id.HasValue)
                {
                    ascmGenerateTaskRule = AscmGenerateTaskRuleService.GetInstance().Get(id.Value);
                    if (ascmGenerateTaskRule != null && ascmGenerateTaskRule.personName2 != null)
                    {
                        AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(ascmGenerateTaskRule.personName2);
                        ascmGenerateTaskRule.ascmUserInfo = ascmUserInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(ascmGenerateTaskRule, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult LogisticsGenerateTaskRuleSave(AscmGenerateTaskRule ascmGenerateTaskRule_Model, int? id)
        {
            JsonObjectResult jsonObjectReuslt = new JsonObjectResult();

            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                AscmGenerateTaskRule ascmGenerateTaskRule = null;
                if (id.HasValue)
                {
                    ascmGenerateTaskRule = AscmGenerateTaskRuleService.GetInstance().Get(id.Value);
                    ascmGenerateTaskRule.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmGenerateTaskRule.modifyUser = userName;
                }
                else
                {
                    ascmGenerateTaskRule = new AscmGenerateTaskRule();
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmGenerateTaskRule");
                    ascmGenerateTaskRule.id = ++maxId;
                    ascmGenerateTaskRule.createUser = userName;
                    ascmGenerateTaskRule.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmGenerateTaskRule.modifyUser = userName;
                    ascmGenerateTaskRule.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }

                if (ascmGenerateTaskRule == null)
                    throw new Exception("保存生成任务规则失败！");

                ascmGenerateTaskRule.identificationId = ascmGenerateTaskRule_Model.identificationId;
                ascmGenerateTaskRule.ruleType = ascmGenerateTaskRule_Model.ruleType;
                ascmGenerateTaskRule.ruleCode = ascmGenerateTaskRule_Model.ruleCode;
                ascmGenerateTaskRule.generateMode = ascmGenerateTaskRule_Model.generateMode;
                ascmGenerateTaskRule.isEnable = ascmGenerateTaskRule_Model.isEnable;
                ascmGenerateTaskRule.relatedRanker = ascmGenerateTaskRule_Model.relatedRanker;
                ascmGenerateTaskRule.others = ascmGenerateTaskRule_Model.others;
                ascmGenerateTaskRule.tip = ascmGenerateTaskRule_Model.tip;
                ascmGenerateTaskRule.isEnable = "是";
                ascmGenerateTaskRule.personName2 = ascmGenerateTaskRule_Model.personName2;
                ascmGenerateTaskRule.description = ascmGenerateTaskRule_Model.description;

                switch (ascmGenerateTaskRule.ruleType)
                {
                    case "TYPEOFPRESTOCK":
                        {
                            ascmGenerateTaskRule.generateMode = "PRODUCTLINE,SUBINVENTORY,TASKTIME,MATERIAL";
                            ascmGenerateTaskRule.description = "按同一产线，同一子库，同一时间，同一物料合并";
                            ascmGenerateTaskRule.priority = 3;
                        }
                        break;
                    case "TYPEOFMIXSTOCK":
                        {
                            ascmGenerateTaskRule.generateMode = "PRODUCTLINE,SUBINVENTORY,TASKTIME";
                            ascmGenerateTaskRule.description = "按同一产线，同一子库，同一时间合并";
                            ascmGenerateTaskRule.priority = 3;
                        }
                        break;
                    case "TYPEOFWAREHOUSE":
                        {
                            if (ascmGenerateTaskRule.identificationId == 1)
                            {
                                ascmGenerateTaskRule.generateMode = "SUBINVENTORY,PRODUCTLINE,TASKTIME";
                                ascmGenerateTaskRule.description = "按指定子库，同一产线，同一时间合并";
                                
                            }
                            else if (ascmGenerateTaskRule.identificationId == 2)
                            {
                                ascmGenerateTaskRule.generateMode = "SUBINVENTORY";
                                ascmGenerateTaskRule.description = "按同一子库合并";
                            }
                            ascmGenerateTaskRule.priority = 2;
                        }
                        break;
                    case "TYPEOFMATERIAL":
                        {
                            ascmGenerateTaskRule.generateMode = "MATERIAL,PRODUCTLINE,SUBINVENTORY,TASKTIME,";
                            ascmGenerateTaskRule.description = "按指定物料，同一产线，同一子库，同一时间合并";
                            ascmGenerateTaskRule.priority = 1;
                        }
                        break;
                    default:
                        {
                            ascmGenerateTaskRule.generateMode = "MATERIAL,PRODUCTLINE,SUBINVENTORY,TASKTIME,";
                            ascmGenerateTaskRule.description = "按指定物料，同一产线，同一子库，同一时间合并";
                        }
                        break;
                }

                AscmGenerateTaskRuleService.GetInstance().Save(ascmGenerateTaskRule);

                jsonObjectReuslt.result = true;
                jsonObjectReuslt.message = "";
                jsonObjectReuslt.id = ascmGenerateTaskRule.id.ToString();
                jsonObjectReuslt.entity = ascmGenerateTaskRule;
            }
            catch (Exception ex)
            {
                jsonObjectReuslt.result = false;
                jsonObjectReuslt.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectReuslt);
            return Content(sReturn);
        }
        public ActionResult LogisticsGenerateTaskRuleDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();

            try
            {
                if (id.HasValue)
                {
                    AscmGenerateTaskRule ascmGenerateTaskRule = AscmGenerateTaskRuleService.GetInstance().Get(id.Value);
                    if (ascmGenerateTaskRule == null)
                        throw new Exception("找不到生成任务的规则！");
                    AscmGenerateTaskRuleService.GetInstance().Delete(ascmGenerateTaskRule);
                    jsonObjectResult.result = true;
                    jsonObjectResult.message = "";
                }
                else
                {
                    throw new Exception("传入参数[id=null]错误！");
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogisticsGenerateTaskRuleEnable(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();

            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                if (id.HasValue)
                {
                    AscmGenerateTaskRule ascmGenerateTaskRule = AscmGenerateTaskRuleService.GetInstance().Get(id.Value);
                    if (ascmGenerateTaskRule == null)
                        throw new Exception("找不到生成任务的规则！");

                    if (!string.IsNullOrEmpty(ascmGenerateTaskRule.isEnable) && ascmGenerateTaskRule.isEnable == "否")
                    {
                        ascmGenerateTaskRule.isEnable = "是";
                        ascmGenerateTaskRule.modifyUser = userName;
                        ascmGenerateTaskRule.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        AscmGenerateTaskRuleService.GetInstance().Update(ascmGenerateTaskRule);
                    }
                }
                else
                {
                    throw new Exception("传入参数[id=null]错误！");
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogisticsGenerateTaskRuleDisEnable(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();

            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                if (id.HasValue)
                {
                    AscmGenerateTaskRule ascmGenerateTaskRule = AscmGenerateTaskRuleService.GetInstance().Get(id.Value);
                    if (ascmGenerateTaskRule == null)
                        throw new Exception("找不到生成任务的规则！");

                    if (!string.IsNullOrEmpty(ascmGenerateTaskRule.isEnable) && ascmGenerateTaskRule.isEnable == "是")
                    {
                        ascmGenerateTaskRule.isEnable = "否";
                        ascmGenerateTaskRule.modifyUser = userName;
                        ascmGenerateTaskRule.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        AscmGenerateTaskRuleService.GetInstance().Update(ascmGenerateTaskRule);
                    }
                }
                else
                {
                    throw new Exception("传入参数[id=null]错误！");
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 任务排配规则
        public ActionResult LogisticsAllocateTaskRuleIndex()
        {
            return View();
        }

        public ActionResult LogisticsAllocateTaskRuleList(int? page, int? rows, string sort, string order, string queryWord, string queryLogisticsClass)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string userName = string.Empty;
                string userLogisticsClass = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }
                userLogisticsClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName);

                string whereOther = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(userLogisticsClass))
                {
                    whereQueryWord = "logisticsClass = '" + userLogisticsClass + "'";
                    List<AscmUserInfo> listAscmUserInfo = AscmUserInfoService.GetInstance().GetList(null, "", "", "", whereQueryWord, false, false);
                    string ids_user = string.Empty;
                    if (listAscmUserInfo != null && listAscmUserInfo.Count > 0)
                    {
                        foreach (AscmUserInfo ascmUserInfo in listAscmUserInfo)
                        {
                            if (!string.IsNullOrEmpty(ids_user))
                                ids_user += ",";
                            ids_user += "'" + ascmUserInfo.userId + "'";
                        }
                    }
                    whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids_user, "workerName");
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(userLogisticsClass))
                {
                    if (!string.IsNullOrEmpty(queryLogisticsClass))
                    {
                        whereQueryWord = "logisticsClass = '" + queryLogisticsClass + "'";
                        List<AscmUserInfo> listAscmUserInfo = AscmUserInfoService.GetInstance().GetList(null, "", "", "", whereQueryWord, false, false);
                        string ids_user = string.Empty;
                        if (listAscmUserInfo != null && listAscmUserInfo.Count > 0)
                        {
                            foreach (AscmUserInfo ascmUserInfo in listAscmUserInfo)
                            {
                                if (!string.IsNullOrEmpty(ids_user))
                                    ids_user += ",";
                                ids_user += "'" + ascmUserInfo.userId + "'";
                            }
                        }
                        whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids_user, "workerName");
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    }
                }
                
                List<AscmAllocateRule> list = AscmAllocateRuleService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                AscmAllocateRuleService.GetInstance().SetWorker(list);
                AscmAllocateRuleService.GetInstance().SetZRanker(list);
                AscmAllocateRuleService.GetInstance().SetDRanker(list);

                if (list != null && list.Count > 0)
                {
                    foreach (AscmAllocateRule ascmAllocateRule in list)
                    {
                        jsonDataGridResult.rows.Add(ascmAllocateRule);
                    }
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                jsonDataGridResult.message = ex.Message;
                jsonDataGridResult.result = false;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogisticsAllocateTaskRuleEdit(int? id)
        {
            AscmAllocateRule ascmAllocateRule = null;
            try
            {
                if (id.HasValue)
                {
                    ascmAllocateRule = AscmAllocateRuleService.GetInstance().Get(id.Value);
                    if (ascmAllocateRule != null)
                    {
                        if (!string.IsNullOrEmpty(ascmAllocateRule.workerName))
                        {
                            AscmUserInfo ascmUserInfoWorker = AscmUserInfoService.GetInstance().Get(ascmAllocateRule.workerName);
                            if (ascmUserInfoWorker != null)
                                ascmAllocateRule.ascmUserInfoWorker = ascmUserInfoWorker;
                        }

                        if (!string.IsNullOrEmpty(ascmAllocateRule.zRankerName))
                        {
                            AscmUserInfo ascmUserInfoZRanker = AscmUserInfoService.GetInstance().Get(ascmAllocateRule.zRankerName);
                            if (ascmUserInfoZRanker != null)
                                ascmAllocateRule.ascmUserInfoZRanker = ascmUserInfoZRanker;
                        }

                        if (!string.IsNullOrEmpty(ascmAllocateRule.dRankerName))
                        {
                            AscmUserInfo ascmUserInfoDRanker = AscmUserInfoService.GetInstance().Get(ascmAllocateRule.dRankerName);
                            if (ascmUserInfoDRanker != null)
                                ascmAllocateRule.ascmUserInfoDRanker = ascmUserInfoDRanker;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(ascmAllocateRule, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult LogisticsAllocateTaskRuleSave(AscmAllocateRule ascmAllocateRule_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                AscmAllocateRule ascmAllocateRule = null;
                if (id.HasValue)
                {
                    ascmAllocateRule = AscmAllocateRuleService.GetInstance().Get(id.Value);
                    if (ascmAllocateRule == null)
                        throw new Exception("保存排配规则失败！");

                    ascmAllocateRule.modifyUser = userName;
                    ascmAllocateRule.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    ascmAllocateRule = new AscmAllocateRule();
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmAllocateRule");
                    ascmAllocateRule.id = ++maxId;
                    ascmAllocateRule.workerName = ascmAllocateRule_Model.workerName;
                    ascmAllocateRule.createUser = userName;
                    ascmAllocateRule.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmAllocateRule.modifyUser = userName;
                    ascmAllocateRule.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }

                if (string.IsNullOrEmpty(ascmAllocateRule.workerName))
                    throw new Exception("领料员不能为空！");

                ascmAllocateRule.zRankerName = ascmAllocateRule_Model.zRankerName;
                ascmAllocateRule.dRankerName = ascmAllocateRule_Model.dRankerName;
                ascmAllocateRule.ruleCode = ascmAllocateRule_Model.ruleCode;
                ascmAllocateRule.other = ascmAllocateRule_Model.other;
                ascmAllocateRule.tip1 = ascmAllocateRule_Model.tip1;
                ascmAllocateRule.tip2 = ascmAllocateRule_Model.tip2;

                if (id.HasValue)
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmAllocateRule where workername = '" + ascmAllocateRule_Model.workerName + "' and id <> " + id.Value + " ");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已存在此领料员的排配规则信息【" + ascmAllocateRule.workerName + "】!");
                    AscmAllocateRuleService.GetInstance().Update(ascmAllocateRule);
                }
                else
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmAllocateRule where workername ='" + ascmAllocateRule_Model.workerName + "'");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已存在此领料员的排配规则信息【" + ascmAllocateRule_Model.workerName + "】!");
                    AscmAllocateRuleService.GetInstance().Save(ascmAllocateRule);
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmAllocateRule.id.ToString();
                jsonObjectResult.entity = ascmAllocateRule;
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult LogisticsAllocateTaskRuleCount(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (!id.HasValue)
                    throw new Exception("选择须重置平衡值的领料员");

                AscmAllocateRule ascmAllocateRule = AscmAllocateRuleService.GetInstance().Get(id.Value);
                if (ascmAllocateRule == null)
                    throw new Exception("找不到排配规则！");
                ascmAllocateRule.taskCount = 0;
                AscmAllocateRuleService.GetInstance().Save(ascmAllocateRule);

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmAllocateRule.id.ToString();
                jsonObjectResult.entity = ascmAllocateRule;
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult LogisticsAllocateTaskRuleDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (!id.HasValue)
                    throw new Exception("传入参数[id=null]错误！");

                AscmAllocateRule ascmAllocateRule = AscmAllocateRuleService.GetInstance().Get(id.Value);
                if (ascmAllocateRule == null)
                    throw new Exception("找不到排配规则！");
                AscmAllocateRuleService.GetInstance().Delete(ascmAllocateRule);

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 导入排产单
        public ActionResult ImportWipDiscreteJobsIndex()
        {
            return View();
        }

        public ActionResult ImportWipDiscreteJobsList(int? page, int? rows, string sort, string order, string queryWord, string queryStartDate, string queryEndDate, string queryStartJobDate, string queryEndJobDate, string queryType, string queryPerson)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;
            
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmDiscreteJobs> list = null;

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
                string userLogisticsClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName);

                string whereOther = "", whereQueryWord = "";
                if (userRole.IndexOf("物流班长") > -1 || userRole.IndexOf("物流组长") > -1)
                {
                    string ids_userId = AscmAllocateRuleService.GetInstance().GetLogisticsRankerName(userLogisticsClass, userName);
                    if (!string.IsNullOrEmpty(ids_userId))
                        whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids_userId, "workerId");
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (userRole.IndexOf("排产员") > -1)
                {
                    whereQueryWord = "workerId = '" + userName + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (userRole.IndexOf("领料员") > -1)
                {
                    string ids_userId = AscmAllocateRuleService.GetInstance().GetLogisticsRankerName(userLogisticsClass, userName, true);
                    if (!string.IsNullOrEmpty(ids_userId))
                        whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids_userId, "workerId");
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (string.IsNullOrEmpty(queryStartDate) && string.IsNullOrEmpty(queryEndDate) && string.IsNullOrEmpty(queryStartJobDate) && string.IsNullOrEmpty(queryEndJobDate))
                    throw new Exception("请选择导入日期！");

                //上传日期
                if (!string.IsNullOrEmpty(queryStartDate) && !string.IsNullOrEmpty(queryEndDate))
                {
                    queryStartDate = queryStartDate + " 00:00:00";
                    queryEndDate = queryEndDate + " 23:59:59";
                    whereQueryWord = "time >= '" + queryStartDate + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    whereQueryWord = "time <= '" + queryEndDate + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (!string.IsNullOrEmpty(queryStartDate) && string.IsNullOrEmpty(queryEndDate))
                {
                    whereQueryWord = "time like '" + queryStartDate + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(queryStartDate) && string.IsNullOrEmpty(queryEndDate))
                {
                    whereQueryWord = "time like '" + queryEndDate + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                //作业日期
                if (!string.IsNullOrEmpty(queryStartJobDate) && !string.IsNullOrEmpty(queryEndJobDate))
                {
                    queryStartJobDate = queryStartJobDate + " 00:00:00";
                    queryEndJobDate = queryEndJobDate + " 23:59:59";
                    whereQueryWord = "jobDate >= '" + queryStartJobDate + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    whereQueryWord = "jobDate <= '" + queryEndJobDate + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (!string.IsNullOrEmpty(queryStartJobDate) && string.IsNullOrEmpty(queryEndJobDate))
                {
                    whereQueryWord = "jobDate like '" + queryStartJobDate + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(queryStartJobDate) && !string.IsNullOrEmpty(queryEndJobDate))
                {
                    whereQueryWord = "jobDate like '" + queryEndJobDate + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryType))
                {
                    whereQueryWord = "identificationId = " + queryType;
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryPerson))
                {
                    whereQueryWord = "workerId = '" + queryPerson + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                whereQueryWord = "status > 0";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                list = AscmDiscreteJobsService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                    {
                        jsonDataGridResult.rows.Add(ascmDiscreteJobs);
                    }
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult ImportWipDiscreteJobs(HttpPostedFileBase fileImport, AscmDiscreteJobs ascmDiscreteJobs_Model)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            string sError = "";
            DataTable dt = new DataTable();
            int colsCount = 0;
            colsCount = int.TryParse(AscmCommonHelperService.DiscreteJobsImportParam.ToString(), out colsCount) ? colsCount : 8;
            StringBuilder sb = new StringBuilder();

            string userName = string.Empty;
            string userRole = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }
            userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);

            try
            {
                for (int i = 0; i < colsCount; i++)
                {
                    dt.Columns.Add("第" + i.ToString() + "列");
                }

                if (fileImport != null)
                {
                    string fileName = fileImport.FileName;
                    string fileExtension = System.IO.Path.GetExtension(fileName).ToLower();

                    if (fileExtension != ".xls" && fileExtension != ".xlsx")
                        throw new Exception("仅支持Excel-2003|Excel-2007的格式");

                    using (Stream stream = fileImport.InputStream)
                    {
                        NPOI.SS.UserModel.IWorkbook wb = NPOI.SS.UserModel.WorkbookFactory.Create(stream);
                        ISheet sheet = wb.GetSheet("Sheet1");
                        IEnumerator rows = sheet.GetRowEnumerator();
                        while (rows.MoveNext())
                        {
                            DataRow dr = dt.NewRow();
                            NPOI.SS.UserModel.IRow row = (NPOI.SS.UserModel.IRow)rows.Current;
                            if (row.RowNum != 0)
                            {
                                List<NPOI.SS.UserModel.ICell> iCellList = new List<NPOI.SS.UserModel.ICell>();

                                for (int i = 0; i < colsCount; i++)
                                {
                                    NPOI.SS.UserModel.ICell iCell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                    iCellList.Add(iCell);
                                }

                                for (int i = 0; i < dt.Columns.Count; i++)
                                {
                                    if (iCellList[i] != null)
                                        dr[i] = iCellList[i].ToString();
                                }
                                dt.Rows.Add(dr);
                            }
                            sb.Append(dr[0].ToString());
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        List<AscmDiscreteJobs> list = new List<AscmDiscreteJobs>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            //判断该排产单是否存在
                            object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmDiscreteJobs where jobId = '" + dr[0].ToString() + "'");
                            if (object1 == null)
                                throw new Exception("导入排产作业查询异常！");
                            int iCount = 0;
                            object object2 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmWipEntities where name = '" + dr[0].ToString() + "'");
                            if (object2 == null)
                                throw new Exception("ERP下载作业查询异常！");
                            int wipEntityId = AscmWipEntitiesService.GetInstance().GetWipEntityId(dr[0].ToString());
                            int iTotal = 0;
                            if ((int.TryParse(object1.ToString(), out iCount) && iCount == 0) && (int.TryParse(object2.ToString(), out iTotal) && iTotal > 0))
                            {
                                //数据赋值
                                AscmDiscreteJobs ascmDiscreteJobs = new AscmDiscreteJobs();
                                int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmDiscreteJobs") + list.Count;
                                ascmDiscreteJobs.id = ++maxId;
                                ascmDiscreteJobs.jobId = dr[0].ToString();
                                ascmDiscreteJobs.jobDate = Convert.ToDateTime(ExcelUtils.ParseDateTime(dr[1].ToString())).Date.ToString("yyyy-MM-dd");
                                ascmDiscreteJobs.jobInfoId = dr[2].ToString();
                                ascmDiscreteJobs.jobDesc = dr[3].ToString();
                                ascmDiscreteJobs.count = Convert.ToInt32(dr[4].ToString());
                                ascmDiscreteJobs.productLine = dr[5].ToString().Substring(0, dr[5].ToString().IndexOf("("));
                                int length = dr[5].ToString().IndexOf("(") + 1;
                                int ilength = dr[5].ToString().IndexOf(")") - dr[5].ToString().IndexOf("(") - 1;
                                ascmDiscreteJobs.sequence = dr[5].ToString().Substring(dr[5].ToString().IndexOf("(") + 1, dr[5].ToString().IndexOf(")") - dr[5].ToString().IndexOf("(") - 1);
                                ascmDiscreteJobs.tip = dr[6].ToString();
                                ascmDiscreteJobs.status = 1;
                                ascmDiscreteJobs.identificationId = ascmDiscreteJobs_Model.identificationId;
                                ascmDiscreteJobs.onlineTime = dr[7].ToString();
                                ascmDiscreteJobs.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                ascmDiscreteJobs.createUser = userName;
                                ascmDiscreteJobs.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                ascmDiscreteJobs.modifyUser = userName;
                                ascmDiscreteJobs.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                ascmDiscreteJobs.workerId = userName;
                                //吴华允于2015年7月28日添加
                                ascmDiscreteJobs.personName = ascmDiscreteJobs_Model.personName;
                                string dateTime = DateTime.Now.Date.ToString("yyyy-MM-dd");
                                int maxWhich = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(which) from AscmDiscreteJobs where workerId = '" + userName + "' and time like '" + dateTime + "%' and status = 2");
                                ascmDiscreteJobs.which = ++maxWhich;
                                ascmDiscreteJobs.wipEntityId = wipEntityId;

                                list.Add(ascmDiscreteJobs);
                            }
                            else if ((int.TryParse(object1.ToString(), out iCount) && iCount > 0) && (int.TryParse(object2.ToString(), out iTotal) && iTotal > 0))
                            {
                                sError += "已存在该排产作业号[" + dr[0].ToString() + "] <br />";
                            }
                            else if ((int.TryParse(object1.ToString(), out iCount) && iCount == 0) && (int.TryParse(object2.ToString(), out iTotal) && iTotal == 0))
                            {
                                if (!string.IsNullOrEmpty(dr[0].ToString()))
                                    sError += "不存在ERP作业号[" + dr[0].ToString() + "] <br />";
                            }
                        }
                        if (list != null && list.Count > 0)
                        {
                            AscmDiscreteJobsService.GetInstance().Save(list);
                            jsonObjectResult.result = true;
                        }
                        jsonObjectResult.message = sError;
                    }
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult ExportWipDiscreteJobs(string queryWord, string queryStartDate, string queryEndDate, string queryStartJobDate, string queryEndJobDate, string queryType, string queryPerson)
        {
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            IWorkbook wb = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Sheet1");
            sheet.SetColumnWidth(0, 20 * 256);
            sheet.SetColumnWidth(1, 12 * 256);
            sheet.SetColumnWidth(2, 18 * 256);
            sheet.SetColumnWidth(3, 60 * 256);
            sheet.SetColumnWidth(4, 8 * 256);
            sheet.SetColumnWidth(5, 8 * 256);
            sheet.SetColumnWidth(6, 30 * 256);
            sheet.SetColumnWidth(7, 15 * 256);

            int iRow = 0;
            IRow titleRow = sheet.CreateRow(iRow);
            titleRow.Height = 20 * 20;
            titleRow.CreateCell(0).SetCellValue("作业号");
            titleRow.CreateCell(1).SetCellValue("作业日期");
            titleRow.CreateCell(2).SetCellValue("装配件");
            titleRow.CreateCell(3).SetCellValue("装配件描述");
            titleRow.CreateCell(4).SetCellValue("需求数");
            titleRow.CreateCell(5).SetCellValue("生产线");
            titleRow.CreateCell(6).SetCellValue("备注");
            titleRow.CreateCell(7).SetCellValue("上线时间");

            string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
            string userLogisticsClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName);

            string whereOther = "", whereQueryWord = "";
            if (userRole.IndexOf("物流班长") > -1 || userRole.IndexOf("物流组长") > -1)
            {
                string ids_userId = AscmAllocateRuleService.GetInstance().GetLogisticsRankerName(userLogisticsClass, userName);
                if (!string.IsNullOrEmpty(ids_userId))
                    whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids_userId, "workerId");
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
            }
            else if (userRole.IndexOf("排产员") > -1)
            {
                whereQueryWord = "workerId = '" + userName + "'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
            }
            else if (userRole.IndexOf("领料员") > -1)
            {
                string ids_userId = AscmAllocateRuleService.GetInstance().GetLogisticsRankerName(userLogisticsClass, userName, true);
                if (!string.IsNullOrEmpty(ids_userId))
                    whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids_userId, "workerId");
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
            }

            if (string.IsNullOrEmpty(queryStartDate) && string.IsNullOrEmpty(queryEndDate) && string.IsNullOrEmpty(queryStartJobDate) && string.IsNullOrEmpty(queryEndJobDate))
                throw new Exception("请选择导出日期！");

            //上传日期
            if (!string.IsNullOrEmpty(queryStartDate) && !string.IsNullOrEmpty(queryEndDate))
            {
                queryStartDate = queryStartDate + " 00:00:00";
                queryEndDate = queryEndDate + " 23:59:59";
                whereQueryWord = "time >= '" + queryStartDate + "'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                whereQueryWord = "time <= '" + queryEndDate + "'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
            }
            else if (!string.IsNullOrEmpty(queryStartDate) && string.IsNullOrEmpty(queryEndDate))
            {
                whereQueryWord = "time like '" + queryStartDate + "%'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
            }
            else if (string.IsNullOrEmpty(queryStartDate) && string.IsNullOrEmpty(queryEndDate))
            {
                whereQueryWord = "time like '" + queryEndDate + "%'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
            }

            //作业日期
            if (!string.IsNullOrEmpty(queryStartJobDate) && !string.IsNullOrEmpty(queryEndJobDate))
            {
                queryStartJobDate = queryStartJobDate + " 00:00:00";
                queryEndJobDate = queryEndJobDate + " 23:59:59";
                whereQueryWord = "time >= '" + queryStartJobDate + "'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                whereQueryWord = "time <= '" + queryEndJobDate + "'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
            }
            else if (!string.IsNullOrEmpty(queryStartJobDate) && string.IsNullOrEmpty(queryEndJobDate))
            {
                whereQueryWord = "time like '" + queryStartJobDate + "%'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
            }
            else if (string.IsNullOrEmpty(queryStartJobDate) && !string.IsNullOrEmpty(queryEndJobDate))
            {
                whereQueryWord = "time like '" + queryEndJobDate + "%'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
            }

            if (!string.IsNullOrEmpty(queryType))
            {
                whereQueryWord = "identificationId = " + queryType;
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
            }

            if (!string.IsNullOrEmpty(queryPerson))
            {
                whereQueryWord = "workerId = '" + queryPerson + "'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
            }

            whereQueryWord = "status > 0";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

            try
            {
                List<AscmDiscreteJobs> listAscmDiscreteJobs = AscmDiscreteJobsService.GetInstance().GetList(null, "", "", queryWord, whereOther, false);
                if (listAscmDiscreteJobs != null && listAscmDiscreteJobs.Count > 0)
                {
                    foreach (AscmDiscreteJobs ascmDiscreteJobs in listAscmDiscreteJobs)
                    {
                        iRow++;
                        IRow row = sheet.CreateRow(iRow);
                        row.Height = 20 * 20;
                        row.CreateCell(0).SetCellValue(ascmDiscreteJobs.jobId);
                        row.CreateCell(1).SetCellValue(ascmDiscreteJobs.jobDate);
                        row.CreateCell(2).SetCellValue(ascmDiscreteJobs.jobInfoId);
                        row.CreateCell(3).SetCellValue(ascmDiscreteJobs.jobDesc);
                        row.CreateCell(4).SetCellValue(ascmDiscreteJobs.count);
                        row.CreateCell(5).SetCellValue(ascmDiscreteJobs.lineAndSequence);
                        row.CreateCell(6).SetCellValue(ascmDiscreteJobs.tip);
                        row.CreateCell(7).SetCellValue(ascmDiscreteJobs.onlineTime);
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("导出失败(Export AscmDiscreteJobs)", ex);
                throw ex;
            }

            byte[] buffer = new byte[] { };
            using (System.IO.MemoryStream stream = new MemoryStream())
            {
                wb.Write(stream);
                buffer = stream.GetBuffer();
            }
            string fileName = DateTime.Now.ToString("yyyyMMhh") + "排产单.xls";

            return File(buffer, "application/vnd.ms-excel", fileName);
        }
        public ActionResult ImportWipDiscreteJobsEdit(int? id)
        {
            AscmDiscreteJobs ascmDiscreteJobs = null;
            try
            {
                if (id.HasValue)
                {
                    ascmDiscreteJobs = AscmDiscreteJobsService.GetInstance().Get(id.Value);
                    if (!string.IsNullOrEmpty(ascmDiscreteJobs.workerId))
                    {
                        YnUser ynUser = YnUserService.GetInstance().Get(ascmDiscreteJobs.workerId);
                        ascmDiscreteJobs.ynUser = ynUser;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(ascmDiscreteJobs, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult ImportWipDiscreteJobsEditSave(AscmDiscreteJobs ascmDiscreteJobs_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                AscmDiscreteJobs ascmDiscreteJobs = null;
                if (id.HasValue)
                {
                    ascmDiscreteJobs = AscmDiscreteJobsService.GetInstance().Get(id.Value);
                    ascmDiscreteJobs.modifyUser = userName;
                    ascmDiscreteJobs.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }

                //if (!string.IsNullOrEmpty(ascmDiscreteJobs_Model.tip))
                ascmDiscreteJobs.tip = ascmDiscreteJobs_Model.tip;

                if (id.HasValue)
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmDiscreteJobs where jobId = '" + ascmDiscreteJobs_Model.jobId + "' and id <> " + id.Value);
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已经存在此排产单信息【" + ascmDiscreteJobs_Model.jobId + "】!");
                    AscmDiscreteJobsService.GetInstance().Update(ascmDiscreteJobs);
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmDiscreteJobs.id.ToString();
                jsonObjectResult.entity = ascmDiscreteJobs;
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult ImportWipDiscreteJobsDelete(string releaseHeaderIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();

            try
            {
                string sql = "from AscmDiscreteJobs";
                string whereOther = "", whereQueryWord = "";

                if (string.IsNullOrEmpty(releaseHeaderIds))
                    throw new Exception("该作业已生成任务无法删除或传入参数[id = null]错误！");

                whereQueryWord = "id in (" + releaseHeaderIds + ")";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                whereQueryWord = "status = 1";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                List<AscmDiscreteJobs> list = AscmDiscreteJobsService.GetInstance().GetList(null, "", "", "", whereOther, false);
                if (list != null && list.Count > 0)
                {
                    AscmDiscreteJobsService.GetInstance().Delete(list);
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 自动生成分配任务
        public ActionResult GenerateLogisticsTaskIndex()
        {
            return View();
        }

        public ActionResult GenerateLogisticsTaskList(int? page, int? rows, string sort, string order, string queryWord, string queryDate)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmGetMaterialTask> list = null;

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
                string userLogisticsClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName);

                string whereOther = "", whereQueryWord = "";
                if (userRole.IndexOf("物流班长") > -1 || userRole.IndexOf("物流组长") > -1)
                {
                    string ids_userId = AscmAllocateRuleService.GetInstance().GetLogisticsRankerName(userLogisticsClass, userName);
                    if (!string.IsNullOrEmpty(ids_userId))
                        whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids_userId, "rankerId");
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (userRole.IndexOf("排产员") > -1)
                {
                    whereQueryWord = "rankerId = '" + userName + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (userRole.IndexOf("领料员") > -1)
                {
                    whereQueryWord = "workerId = '" + userName + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                //whereQueryWord = "status = '" + AscmGetMaterialTask.StatusDefine.notAllocate + "'";
                //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                if (!string.IsNullOrEmpty(queryDate))
                {
                    whereQueryWord = "uploadDate = '" + queryDate + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(whereOther) && !string.IsNullOrEmpty(queryDate))
                {
                    list = AscmGetMaterialTaskService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                    AscmGetMaterialTaskService.GetInstance().SetWorker(list);
                    AscmGetMaterialTaskService.GetInstance().SetRanker(list);
                    AscmGetMaterialTaskService.GetInstance().SetWarehousePlace(list);

                    if (list != null && list.Count > 0)
                    {
                        foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                        {
                            jsonDataGridResult.rows.Add(ascmGetMaterialTask);
                        }
                    }
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult GenerateLogisticsTask(string generateTaskDate)
        {
            JsonObjectResult jsonObjectReuslt = new JsonObjectResult();

            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);

                if (string.IsNullOrEmpty(generateTaskDate))
                    throw new Exception("生成日期为空！");
                
                //获取数据源
                string whereOther = "", whereQueryWord = "";

                if (!string.IsNullOrEmpty(generateTaskDate))
                {
                    generateTaskDate = Convert.ToDateTime(generateTaskDate).ToString("yyyy-MM-dd");
                    whereQueryWord = "time like '" + generateTaskDate + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                whereQueryWord = "status = 1";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                if (userRole.IndexOf(userName) > -1)
                {
                    whereQueryWord = "workerId = '" + userName + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                List<AscmDiscreteJobs> list_jobs = AscmDiscreteJobsService.GetInstance().GetList(null, "", "", "", whereOther);
                if (list_jobs == null || list_jobs.Count == 0)
                    throw new Exception("满足条件的排产单(作业)不存在或者作业数量为0 ！");

                string ids_jobs = string.Empty;
                if (list_jobs != null && list_jobs.Count > 0)
                {
                    foreach (AscmDiscreteJobs ascmDiscreteJobs in list_jobs)
                    {
                        if (!string.IsNullOrEmpty(ids_jobs) && ascmDiscreteJobs.wipEntityId > 0)
                            ids_jobs += ",";
                        if (ascmDiscreteJobs.wipEntityId > 0)
                            ids_jobs += ascmDiscreteJobs.wipEntityId;
                    }
                }

                whereQueryWord = "ami.wipSupplyType = 1";

                List<AscmWipRequirementOperations> list_operations = AscmWipRequirementOperationsService.GetInstance().GetList(null, "", "", ids_jobs, whereQueryWord);
                if (list_operations != null && list_operations.Count > 0)
                { 
                    //生成领料任务
                    List<AscmGetMaterialTask> list = GenerateTask(list_operations, generateTaskDate);

                    if (list != null && list.Count > 0)
                    {
                        //保存任务
                        SaveTask(list, userName, list_jobs);
                    }

                    //分配任务
                    AllocateTask(userName);
                }

                jsonObjectReuslt.result = true;
                jsonObjectReuslt.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectReuslt.result = false;
                jsonObjectReuslt.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectReuslt);
            return Content(sReturn);
        }

        //生成任务
        public List<AscmGetMaterialTask> GenerateTask(List<AscmWipRequirementOperations> list, string generateTaskDate)
        {
            List<AscmGetMaterialTask> listTask = new List<AscmGetMaterialTask>();

            try
            {
                List<AscmGenerateTaskRule> list_rule = AscmGenerateTaskRuleService.GetInstance().GetList(null, "identificationId,priority", "", "", "");
                if (list_rule == null || list_rule.Count == 0)
                    throw new Exception("生成领料任务规则不存在！");

                generateTaskDate = Convert.ToDateTime(generateTaskDate).ToString("yyyy-MM-dd");
                int iCount = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmGetMaterialTask where uploadDate = '" + generateTaskDate + "'");

                foreach (AscmWipRequirementOperations ascmWipRequirmentOperations in list)
                {
                    foreach (AscmGenerateTaskRule ascmGenerateTaskRule in list_rule)
                    {
                        AscmGetMaterialTask ascmGetMaterialTask = new AscmGetMaterialTask();
                        ascmGetMaterialTask.taskId = AscmCommonHelperService.GetInstance().getTaskId(iCount + listTask.Count + 1);
                        ascmGetMaterialTask.productLine = ascmWipRequirmentOperations.productLine;
                        ascmGetMaterialTask.IdentificationId = ascmWipRequirmentOperations.identificationId;
                        ascmGetMaterialTask.warehouserId = ascmWipRequirmentOperations.supplySubinventory;
                        ascmGetMaterialTask.materialDocNumber = ascmWipRequirmentOperations.docNumber;
                        ascmGetMaterialTask.materialType = ascmWipRequirmentOperations.wipSupplyType;
                        ascmGetMaterialTask.dateReleased = ascmWipRequirmentOperations.jobDate;

                        switch (ascmGetMaterialTask.IdentificationId)
                        {
                            case 1://总装
                                ascmGetMaterialTask.mtlCategoryStatus = ascmWipRequirmentOperations.zMtlCategoryStatus;
                                break;
                            case 2://电装
                                ascmGetMaterialTask.mtlCategoryStatus = ascmWipRequirmentOperations.dMtlCategoryStatus;
                                break;
                        }
                        ascmGetMaterialTask.uploadDate = generateTaskDate;
                        ascmGetMaterialTask.which = ascmWipRequirmentOperations.which;
                        ascmGetMaterialTask.rankerId = ascmWipRequirmentOperations.workerId;
                        ascmGetMaterialTask.taskTime = ascmWipRequirmentOperations.onlineTime;
                        ascmGetMaterialTask.status = AscmGetMaterialTask.StatusDefine.notAllocate;

                        if (ascmGenerateTaskRule.identificationId == ascmGetMaterialTask.IdentificationId)
                        {
							if (string.IsNullOrEmpty(ascmWipRequirmentOperations.supplySubinventory)) 
							{
								continue;
							}

                            string warehouseId = ascmWipRequirmentOperations.supplySubinventory.Substring(0, 4);
                            string docnumber = ascmWipRequirmentOperations.docNumber;
                            switch (ascmGenerateTaskRule.ruleType)
                            {
                                case AscmGenerateTaskRule.RuleTypeDefine.typeofPreStock:
                                    {
                                        if (ascmGetMaterialTask.mtlCategoryStatus == MtlCategoryStatusDefine.preStock)
                                        {
                                            bool isOk = IsJudgeCodeAndRanker(ascmGenerateTaskRule, ascmGetMaterialTask, warehouseId, docnumber, false, false);

                                            if (isOk)
                                            {
                                                ascmGetMaterialTask.ruleType = ascmGenerateTaskRule.ruleType;
                                                // 任务不存在
                                                if (ContainsTask(listTask, ascmGetMaterialTask) == null)
                                                {
                                                    if (ascmGetMaterialTask.listAscmWipRequirementOperations == null)
                                                        ascmGetMaterialTask.listAscmWipRequirementOperations = new List<AscmWipRequirementOperations>();
                                                    ascmGetMaterialTask.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);

                                                    listTask.Add(ascmGetMaterialTask);
                                                }
                                                else
                                                {
                                                    //任务存在,判断是否包含该BOM
                                                    AscmGetMaterialTask task = ContainsTask(listTask, ascmGetMaterialTask);
                                                    if (ContainsOperations(task.listAscmWipRequirementOperations, ascmWipRequirmentOperations) == null)
                                                    {
                                                        task.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case AscmGenerateTaskRule.RuleTypeDefine.typeofMixStock:
                                    {
                                        if (ascmGetMaterialTask.mtlCategoryStatus == MtlCategoryStatusDefine.mixStock)
                                        {
                                            bool isOk = IsJudgeCodeAndRanker(ascmGenerateTaskRule, ascmGetMaterialTask, warehouseId, docnumber, false, false);

                                            if (isOk)
                                            {
                                                ascmGetMaterialTask.ruleType = ascmGenerateTaskRule.ruleType;
                                                // 任务不存在
                                                if (ContainsTask(listTask, ascmGetMaterialTask) == null)
                                                {
                                                    if (ascmGetMaterialTask.listAscmWipRequirementOperations == null)
                                                        ascmGetMaterialTask.listAscmWipRequirementOperations = new List<AscmWipRequirementOperations>();
                                                    ascmGetMaterialTask.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);

                                                    ascmGetMaterialTask.materialDocNumber = "";
                                                    listTask.Add(ascmGetMaterialTask);
                                                }
                                                else
                                                {
                                                    //任务存在,判断是否包含该BOM
                                                    AscmGetMaterialTask task = ContainsTask(listTask, ascmGetMaterialTask);
                                                    if (ContainsOperations(task.listAscmWipRequirementOperations, ascmWipRequirmentOperations) == null)
                                                    {
                                                        task.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case AscmGenerateTaskRule.RuleTypeDefine.typeofWarehouse:
                                    {
                                        bool isOk = IsJudgeCodeAndRanker(ascmGenerateTaskRule, ascmGetMaterialTask, warehouseId, docnumber, false, false);

                                        if (isOk)
                                        {
                                            ascmGetMaterialTask.ruleType = ascmGenerateTaskRule.ruleType;
                                            // 任务不存在
                                            if (ContainsTask(listTask, ascmGetMaterialTask) == null)
                                            {
                                                if (ascmGetMaterialTask.listAscmWipRequirementOperations == null)
                                                    ascmGetMaterialTask.listAscmWipRequirementOperations = new List<AscmWipRequirementOperations>();
                                                ascmGetMaterialTask.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);

                                                ascmGetMaterialTask.mtlCategoryStatus = "";
                                                ascmGetMaterialTask.materialDocNumber = "";
                                                listTask.Add(ascmGetMaterialTask);
                                            }
                                            else
                                            {
                                                //任务存在,判断是否包含该BOM
                                                AscmGetMaterialTask task = ContainsTask(listTask, ascmGetMaterialTask);
                                                if (ContainsOperations(task.listAscmWipRequirementOperations, ascmWipRequirmentOperations) == null)
                                                {
                                                    task.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case AscmGenerateTaskRule.RuleTypeDefine.typeofMaterial:
                                    {
                                        bool isOk = IsJudgeCodeAndRanker(ascmGenerateTaskRule, ascmGetMaterialTask, warehouseId, docnumber, false, false);

                                        if (isOk)
                                        {
                                            ascmGetMaterialTask.ruleType = ascmGenerateTaskRule.ruleType;
                                            // 任务不存在
                                            if (ContainsTask(listTask, ascmGetMaterialTask) == null)
                                            {
                                                if (ascmGetMaterialTask.listAscmWipRequirementOperations == null)
                                                    ascmGetMaterialTask.listAscmWipRequirementOperations = new List<AscmWipRequirementOperations>();
                                                ascmGetMaterialTask.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);

                                                listTask.Add(ascmGetMaterialTask);
                                            }
                                            else
                                            {
                                                //任务存在,判断是否包含该BOM
                                                AscmGetMaterialTask task = ContainsTask(listTask, ascmGetMaterialTask);
                                                if (ContainsOperations(task.listAscmWipRequirementOperations, ascmWipRequirmentOperations) == null)
                                                {
                                                    task.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case AscmGenerateTaskRule.RuleTypeDefine.typeofProductLine:
                                    {
                                        bool isOk = IsJudgeCodeAndRanker(ascmGenerateTaskRule, ascmGetMaterialTask, warehouseId, docnumber, false, false);

                                        if (isOk)
                                        {
                                            ascmGetMaterialTask.ruleType = ascmGenerateTaskRule.ruleType;
                                            // 任务不存在
                                            if (ContainsTask(listTask, ascmGetMaterialTask) == null)
                                            {
                                                if (ascmGetMaterialTask.listAscmWipRequirementOperations == null)
                                                    ascmGetMaterialTask.listAscmWipRequirementOperations = new List<AscmWipRequirementOperations>();
                                                ascmGetMaterialTask.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);

                                                ascmGetMaterialTask.materialDocNumber = "";
                                                listTask.Add(ascmGetMaterialTask);
                                            }
                                            else
                                            {
                                                //任务存在,判断是否包含该BOM
                                                AscmGetMaterialTask task = ContainsTask(listTask, ascmGetMaterialTask);
                                                if (ContainsOperations(task.listAscmWipRequirementOperations, ascmWipRequirmentOperations) == null)
                                                {
                                                    task.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("生成任务失败(Generate GetMaterialTask) ", ex);
                throw ex;
            }

            return listTask;
        }
        #region 生成任务相关方法
        //判断规则及指定关系人
        public bool IsJudgeCodeAndRanker(AscmGenerateTaskRule ascmGenerateTaskRule, AscmGetMaterialTask ascmGetMaterialTask, string warehouseId, string docnumber, bool isRule, bool isRaner)
        {
            if (!string.IsNullOrEmpty(ascmGenerateTaskRule.ruleCode))
            {
                string[] myArray = ascmGenerateTaskRule.ruleCode.Split('&');
                string warehouseString = myArray[0].Substring(myArray[0].IndexOf("(") + 1, myArray[0].IndexOf(")") - myArray[0].IndexOf("(") - 1);
                string materialString = myArray[1].Substring(myArray[1].IndexOf("(") + 1, myArray[1].IndexOf(")") - myArray[1].IndexOf("(") - 1);

                if (warehouseString.Length > 0 && warehouseString.IndexOf(warehouseId) > -1)
                {
                    #region
                    if (materialString.Length > 0)
                    {
                        if (materialString.IndexOf("|") > -1)
                        {
                            string[] mtlArray = materialString.Split('|');
                            foreach (string mtl in mtlArray)
                            {
                                if (mtl.IndexOf(warehouseId) > -1)
                                {
                                    string material = mtl.Substring(mtl.IndexOf(":") + 1, mtl.Length - mtl.IndexOf(":") - 1);
                                    if (material.IndexOf("%") > -1)
                                    {
                                        string[] materialArray = material.Split('%');
                                        foreach (string item in materialArray)
                                        {
                                            if (item == docnumber.Substring(0, item.Length))
                                            {
                                                isRule = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (material == docnumber.Substring(0, material.Length))
                                        {
                                            isRule = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (materialString.IndexOf(warehouseId) > -1)
                            {
                                string material = materialString.Substring(materialString.IndexOf(":") + 1, materialString.Length - materialString.IndexOf(":") - 1);
                                if (material.IndexOf("%") > -1)
                                {
                                    string[] materialArray = material.Split('%');
                                    foreach (string item in materialArray)
                                    {
                                        if (item == docnumber.Substring(0, item.Length))
                                        {
                                            isRule = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (material == docnumber.Substring(0, material.Length))
                                    {
                                        isRule = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        isRule = true;
                    }
                    #endregion
                }
            }
            else
            {
                isRule = true;
            }

            if (!string.IsNullOrEmpty(ascmGenerateTaskRule.relatedRanker))
            {
                if (ascmGenerateTaskRule.relatedRanker == ascmGetMaterialTask.rankerId)
                    isRaner = true;
            }
            else
            {
                isRaner = true;
            }

            return (isRule && isRaner);
        }
        //判断是否包含任务
        public AscmGetMaterialTask ContainsTask(List<AscmGetMaterialTask> listTask, AscmGetMaterialTask ascmGetMaterialTask)
        {
            if (listTask.Count == 0)
                return null;

            switch (ascmGetMaterialTask.ruleType)
            {
                case AscmGenerateTaskRule.RuleTypeDefine.typeofPreStock:
                    {
                        foreach (AscmGetMaterialTask item in listTask)
                        {
                            if (item.rankerId == ascmGetMaterialTask.rankerId && item.uploadDate == ascmGetMaterialTask.uploadDate && item.IdentificationId == ascmGetMaterialTask.IdentificationId && item.warehouserId == ascmGetMaterialTask.warehouserId && item.materialDocNumber == ascmGetMaterialTask.materialDocNumber && item.mtlCategoryStatus == ascmGetMaterialTask.mtlCategoryStatus && item.productLine == ascmGetMaterialTask.productLine && item.taskTime == ascmGetMaterialTask.taskTime && item.which == ascmGetMaterialTask.which && item.dateReleased == ascmGetMaterialTask.dateReleased)
                                return item;
                        }
                    }
                    break;
                case AscmGenerateTaskRule.RuleTypeDefine.typeofMixStock:
                    {
                        foreach (AscmGetMaterialTask item in listTask)
                        {
                            if (item.rankerId == ascmGetMaterialTask.rankerId && item.uploadDate == ascmGetMaterialTask.uploadDate && item.IdentificationId == ascmGetMaterialTask.IdentificationId && item.warehouserId == ascmGetMaterialTask.warehouserId && item.mtlCategoryStatus == ascmGetMaterialTask.mtlCategoryStatus && item.productLine == ascmGetMaterialTask.productLine && item.taskTime == ascmGetMaterialTask.taskTime && item.which == ascmGetMaterialTask.which && item.dateReleased == ascmGetMaterialTask.dateReleased)
                                return item;
                        }
                    }
                    break;
                case AscmGenerateTaskRule.RuleTypeDefine.typeofWarehouse:
                    {
                        foreach (AscmGetMaterialTask item in listTask)
                        {
                            if (item.rankerId == ascmGetMaterialTask.rankerId && item.uploadDate == ascmGetMaterialTask.uploadDate && item.IdentificationId == ascmGetMaterialTask.IdentificationId && item.warehouserId == ascmGetMaterialTask.warehouserId && item.productLine == ascmGetMaterialTask.productLine && item.taskTime == ascmGetMaterialTask.taskTime && item.which == ascmGetMaterialTask.which && item.dateReleased == ascmGetMaterialTask.dateReleased)
                                return item;
                        }
                    }
                    break;
                case AscmGenerateTaskRule.RuleTypeDefine.typeofMaterial:
                    {
                        foreach (AscmGetMaterialTask item in listTask)
                        {
                            if (item.rankerId == ascmGetMaterialTask.rankerId && item.uploadDate == ascmGetMaterialTask.uploadDate && item.IdentificationId == ascmGetMaterialTask.IdentificationId && item.warehouserId == ascmGetMaterialTask.warehouserId && item.productLine == ascmGetMaterialTask.productLine && item.taskTime == ascmGetMaterialTask.taskTime && item.materialDocNumber == ascmGetMaterialTask.materialDocNumber && item.which == ascmGetMaterialTask.which && item.dateReleased == ascmGetMaterialTask.dateReleased)
                                return item;
                        }
                    }
                    break;
                case AscmGenerateTaskRule.RuleTypeDefine.typeofProductLine:
                    {
                        foreach (AscmGetMaterialTask item in listTask)
                        {
                            if (item.rankerId == ascmGetMaterialTask.rankerId && item.uploadDate == ascmGetMaterialTask.uploadDate && item.IdentificationId == ascmGetMaterialTask.IdentificationId && item.warehouserId == ascmGetMaterialTask.warehouserId && item.productLine == ascmGetMaterialTask.productLine && item.taskTime == ascmGetMaterialTask.taskTime && item.which == ascmGetMaterialTask.which && item.dateReleased == ascmGetMaterialTask.dateReleased)
                                return item;
                        }
                    }
                    break;

            }

            return null;
        }
        //判断是否包含BOM
        public AscmWipRequirementOperations ContainsOperations(List<AscmWipRequirementOperations> listAscmWipRequirementOperations, AscmWipRequirementOperations ascmWipRequirementOperations)
        {
            if (listAscmWipRequirementOperations.Count == 0)
                return null;

            foreach (AscmWipRequirementOperations item in listAscmWipRequirementOperations)
            {
                if (item.id == ascmWipRequirementOperations.id)
                    return item;
            }

            return null;
        }
        #endregion

        //保存任务
        public void SaveTask(List<AscmGetMaterialTask> list, string userName, List<AscmDiscreteJobs> list_jobs)
        {
            int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmGetMaterialTask");

            List<AscmWipRequirementOperations> listAscmWipRequirementOperations = new List<AscmWipRequirementOperations>();
            int count = 0;
            foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
            {
                ascmGetMaterialTask.id = maxId + 1 + count;
                ascmGetMaterialTask.createUser = userName;
                ascmGetMaterialTask.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmGetMaterialTask.modifyUser = userName;
                ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                foreach (AscmWipRequirementOperations ascmWipRequirementOperations in ascmGetMaterialTask.listAscmWipRequirementOperations)
                {
                    ascmWipRequirementOperations.taskId = ascmGetMaterialTask.id;
                    ascmWipRequirementOperations.modifyUser = userName;
                    ascmWipRequirementOperations.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                    listAscmWipRequirementOperations.Add(ascmWipRequirementOperations);
                }
                count++;
            }

            foreach (AscmDiscreteJobs ascmDiscreteJobs in list_jobs)
            {
                ascmDiscreteJobs.status = 2;
                ascmDiscreteJobs.modifyUser = userName;
                ascmDiscreteJobs.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }

            //执行事务
            ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
            session.Clear();
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    //保存生成任务
                    if (list != null)
                        AscmGetMaterialTaskService.GetInstance().Save(list);
                    //更新关联作业BOM
                    if (listAscmWipRequirementOperations != null)
                        listAscmWipRequirementOperations.ForEach(P => session.Merge(P));
                    //更新关联作业状态
                    if (list_jobs != null)
                        AscmDiscreteJobsService.GetInstance().Update(list_jobs);
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("生成任务失败(Generate AscmGetMateiralTask)", ex);
                }
            }
        }

        //分配任务
        public void AllocateTask(string userName)
        {
            try
            {
                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
                string userLogisticsClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName);

                string whereTaskOther = "", whereQueryWord = "", whereRuleOther = "";

                whereQueryWord = "status = '" + AscmGetMaterialTask.StatusDefine.notAllocate + "'";
                whereTaskOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereTaskOther, whereQueryWord);

                if (userRole.IndexOf("物流班长") > -1 || userRole.IndexOf("物流组长") > -1)
                {
                    string ids_userId = AscmAllocateRuleService.GetInstance().GetLogisticsRankerName(userLogisticsClass, userName);
                    if (!string.IsNullOrEmpty(ids_userId))
                        whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids_userId, "rankerId");
                    whereTaskOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereTaskOther, whereQueryWord);
                }
                else if (userRole.IndexOf("排产员") > -1)
                {
                    whereQueryWord = "rankerId = '" + userName + "'";
                    whereTaskOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereTaskOther, whereQueryWord);
                }

                List<AscmGetMaterialTask> listAscmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().GetList(null, "", "", "", whereTaskOther);
                if (listAscmGetMaterialTask == null || listAscmGetMaterialTask.Count == 0)
                    throw new Exception("须分配任务不存在！");

                if (userRole == "总装排产员")
                {
                    whereQueryWord = "zRankerName = '" + userName + "'";
                    whereRuleOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereRuleOther, whereQueryWord);
                }
                else if (userRole == "电装排产员")
                {
                    whereQueryWord = "dRankerName = '" + userName + "'";
                    whereRuleOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereRuleOther, whereQueryWord);
                }

                List<AscmAllocateRule> listAscmAllocateRule = AscmAllocateRuleService.GetInstance().GetList(null, "", "", "", whereRuleOther);

                if ((listAscmGetMaterialTask != null && listAscmGetMaterialTask.Count > 0) && (listAscmAllocateRule != null && listAscmAllocateRule.Count > 0))
                {
                    Allocate(listAscmGetMaterialTask, listAscmAllocateRule, userName);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("分配任务失败(Allocate GetMaterialTask) ", ex);
                throw ex;
            }
        }
        public void Allocate(List<AscmGetMaterialTask> listTask, List<AscmAllocateRule> listRule, string userName)
        {
            //List<AscmAllocateRule> listNewRule = null;

            foreach (AscmGetMaterialTask ascmGetMaterialTask in listTask)
            {
                #region 查找符合领料任务条件的领料员
                List<string> namelist = new List<string>();
                foreach (AscmAllocateRule ascmAllocateRule in listRule)
                {
                    string taskType = ascmGetMaterialTask.taskId.Substring(0, 1);
                    string sName = string.Empty;
                    if (taskType == "T")
                    {
                        //自动任务分配
                        if (!string.IsNullOrEmpty(ascmAllocateRule.ruleCode))
                        {
                            if (ascmGetMaterialTask.IdentificationId == 1)//总装
                            {
                                if (!string.IsNullOrEmpty(ascmAllocateRule.zRankerName))//对接总装排产员的领料员
                                {
                                    if (ascmAllocateRule.zRankerName == ascmGetMaterialTask.rankerId)
                                    {
                                        string[] strArr = ascmAllocateRule.ruleCode.Split('&');
                                        if (string.IsNullOrEmpty(ascmGetMaterialTask.mtlCategoryStatus))
                                        {
                                            #region 特殊子库
                                            string ruleCode = strArr[2].ToString();
                                            if (ruleCode.Length > 2)
                                            {
                                                string WarehouseStr = ruleCode.Substring(ruleCode.IndexOf('(') + 1, ruleCode.IndexOf(')') - ruleCode.IndexOf('(') - 1);
                                                string MaterialStr = ruleCode.Substring(ruleCode.LastIndexOf('(') + 1, ruleCode.LastIndexOf(')') - ruleCode.LastIndexOf('(') - 1);
                                                string Warehouse = ascmGetMaterialTask.warehouserId.Substring(0, 4);
                                                if (WarehouseStr.IndexOf(Warehouse) > -1)
                                                {
                                                    sName = ascmAllocateRule.id.ToString();
                                                    namelist.Add(sName);
                                                }
                                            }
                                            #endregion
                                        }
                                        else if (ascmGetMaterialTask.mtlCategoryStatus == "PRESTOCK")
                                        {
                                            #region 须备料
                                            string ruleCode = strArr[1].ToString();
                                            if (ruleCode.Length > 2)
                                            {
                                                string Status = ruleCode.Substring(ruleCode.IndexOf('[') + 1, ruleCode.IndexOf('(') - 1);
                                                string WarehouseStr = ruleCode.Substring(ruleCode.IndexOf('(') + 1, ruleCode.IndexOf(')') - ruleCode.IndexOf('(') - 1);
                                                string MaterialStr = ruleCode.Substring(ruleCode.LastIndexOf('(') + 1, ruleCode.LastIndexOf(')') - ruleCode.LastIndexOf('(') - 1);
                                                if (Status == "须备料")
                                                {
                                                    string Warehouse = ascmGetMaterialTask.warehouserId.Substring(0, 4);
                                                    if (WarehouseStr.IndexOf(Warehouse) > -1)
                                                    {
                                                        if (MaterialStr.Length > 2)
                                                        {
                                                            if (MaterialStr.IndexOf('|') > -1)
                                                            {
                                                                #region 有分割子库筛选物料
                                                                string[] MaterialArray = MaterialStr.Split('|');
                                                                foreach (string item1 in MaterialArray)
                                                                {
                                                                    string MaterialDocnumberStr = item1.Substring(item1.IndexOf(':') + 1, item1.Length - item1.IndexOf(':') - 1);
                                                                    string MaterialWarehouseStr = item1.Substring(0, item1.IndexOf(':'));
                                                                    if (MaterialDocnumberStr.IndexOf('%') > -1)
                                                                    {
                                                                        string[] MaterialDocnumberArray = MaterialDocnumberStr.Split('%');
                                                                        foreach (string item2 in MaterialDocnumberArray)
                                                                        {
                                                                            if (ascmGetMaterialTask.materialDocNumber.IndexOf(item2) > -1 && MaterialWarehouseStr == Warehouse)
                                                                            {
                                                                                sName = ascmAllocateRule.id.ToString();
                                                                                namelist.Add(sName);
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (ascmGetMaterialTask.materialDocNumber.IndexOf(MaterialDocnumberStr) > -1 && MaterialWarehouseStr == Warehouse)
                                                                        {
                                                                            sName = ascmAllocateRule.id.ToString();
                                                                            namelist.Add(sName);
                                                                        }
                                                                    }
                                                                }
                                                                #endregion
                                                            }
                                                            else
                                                            {
                                                                #region 无分割子库筛选物料
                                                                string MaterialDocnumberStr = MaterialStr.Substring(MaterialStr.IndexOf(':') + 1, MaterialStr.Length - MaterialStr.IndexOf(':') - 1);
                                                                string MaterialWarehouseStr = MaterialStr.Substring(0, MaterialStr.IndexOf(':'));
                                                                if (MaterialDocnumberStr.IndexOf('%') > -1)
                                                                {
                                                                    string[] MaterialDocnumberArray = MaterialDocnumberStr.Split('%');
                                                                    foreach (string item2 in MaterialDocnumberArray)
                                                                    {
                                                                        if (ascmGetMaterialTask.materialDocNumber.IndexOf(item2) > -1 && MaterialWarehouseStr == Warehouse)
                                                                        {
                                                                            sName = ascmAllocateRule.id.ToString();
                                                                            namelist.Add(sName);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (ascmGetMaterialTask.materialDocNumber.IndexOf(MaterialDocnumberStr) > -1 && MaterialWarehouseStr == Warehouse)
                                                                    {
                                                                        sName = ascmAllocateRule.id.ToString();
                                                                        namelist.Add(sName);
                                                                    }
                                                                }
                                                                #endregion
                                                            }
                                                        }
                                                        else
                                                        {
                                                            sName = ascmAllocateRule.id.ToString();
                                                            namelist.Add(sName);
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        else if (ascmGetMaterialTask.mtlCategoryStatus == "MIXSTOCK")
                                        {
                                            #region 须配料
                                            string ruleCode = strArr[0].ToString();
                                            if (ruleCode.Length > 2)
                                            {
                                                string Status = ruleCode.Substring(ruleCode.IndexOf('[') + 1, ruleCode.IndexOf('(') - 1);
                                                string WarehouseStr = ruleCode.Substring(ruleCode.IndexOf('(') + 1, ruleCode.IndexOf(')') - ruleCode.IndexOf('(') - 1);
                                                string MaterialStr = ruleCode.Substring(ruleCode.LastIndexOf('(') + 1, ruleCode.LastIndexOf(')') - ruleCode.LastIndexOf('(') - 1);
                                                if (Status == "须配料")
                                                {
                                                    string Warehouse = ascmGetMaterialTask.warehouserId.Substring(0, 4);
                                                    if (WarehouseStr.IndexOf(Warehouse) > -1)
                                                    {
                                                        sName = ascmAllocateRule.id.ToString();
                                                        namelist.Add(sName);
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(ascmAllocateRule.dRankerName))//对接电装排产员的领料员
                                {
                                    if (ascmAllocateRule.dRankerName == ascmGetMaterialTask.rankerId)
                                    {
                                        string[] strArr = ascmAllocateRule.ruleCode.Split('&');
                                        if (string.IsNullOrEmpty(ascmGetMaterialTask.mtlCategoryStatus))
                                        {
                                            #region 特殊子库
                                            string ruleCode = strArr[2].ToString();
                                            if (ruleCode.Length > 2)
                                            {
                                                string WarehouseStr = ruleCode.Substring(ruleCode.IndexOf('(') + 1, ruleCode.IndexOf(')') - ruleCode.IndexOf('(') - 1);
                                                string MaterialStr = ruleCode.Substring(ruleCode.LastIndexOf('(') + 1, ruleCode.LastIndexOf(')') - ruleCode.LastIndexOf('(') - 1);
                                                string Warehouse = ascmGetMaterialTask.warehouserId.Substring(0, 4);
                                                if (WarehouseStr.IndexOf(Warehouse) > -1)
                                                {
                                                    sName = ascmAllocateRule.id.ToString();
                                                    namelist.Add(sName);
                                                }
                                            }
                                            #endregion
                                        }
                                        else if (ascmGetMaterialTask.mtlCategoryStatus == "MIXSTOCK")
                                        {
                                            #region 须配料
                                            string ruleCode = strArr[0].ToString();
                                            if (ruleCode.Length > 2)
                                            {
                                                string Status = ruleCode.Substring(ruleCode.IndexOf('[') + 1, ruleCode.IndexOf('(') - 1);
                                                string WarehouseStr = ruleCode.Substring(ruleCode.IndexOf('(') + 1, ruleCode.IndexOf(')') - ruleCode.IndexOf('(') - 1);
                                                string MaterialStr = ruleCode.Substring(ruleCode.LastIndexOf('(') + 1, ruleCode.LastIndexOf(')') - ruleCode.LastIndexOf('(') - 1);
                                                if (Status == "须配料")
                                                {
                                                    string Warehouse = ascmGetMaterialTask.warehouserId.Substring(0, 4);
                                                    if (WarehouseStr.IndexOf(Warehouse) > -1)
                                                    {
                                                        sName = ascmAllocateRule.id.ToString();
                                                        namelist.Add(sName);
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (taskType == "L")
                    {
                        if (!string.IsNullOrEmpty(ascmGetMaterialTask.tip))
                        {
                            if (!string.IsNullOrEmpty(ascmAllocateRule.other))
                            {
                                if (ascmAllocateRule.other.IndexOf(ascmGetMaterialTask.tip.ToString()) > -1)
                                {
                                    sName = ascmAllocateRule.id.ToString();
                                    namelist.Add(sName);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 按平衡原则指定责任人
                if (namelist != null && namelist.Count > 0)
                {
                    string ids = string.Empty, id = string.Empty, count = string.Empty, worker = string.Empty;
                    foreach (string name in namelist)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += name;
                    }
                    string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");
                    if (!string.IsNullOrEmpty(where))
                    {
                        IList<AscmAllocateRule> ilistAllocateRule = AscmAllocateRuleService.GetInstance().GetList(null, "taskCount,id", "", "", where);
                        if (ilistAllocateRule != null && ilistAllocateRule.Count > 0)
                        {
                            List<AscmAllocateRule> listAllocateRule = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmAllocateRule>(ilistAllocateRule);

                            ids = listAllocateRule[0].id.ToString();
                            count = listAllocateRule[0].taskCount.ToString();
                            worker = listAllocateRule[0].workerName.ToString();

                            //获取责任人的物流组
                            string userLogisticsClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(worker);

                            ascmGetMaterialTask.workerId = worker;
                            ascmGetMaterialTask.status = "NOTEXECUTE";
                            ascmGetMaterialTask.logisticsClass = userLogisticsClass;
                            ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.modifyUser = userName;

                            AscmAllocateRule ascmAllocateRule = AscmAllocateRuleService.GetInstance().Get(int.Parse(ids));
                            ascmAllocateRule.taskCount = int.Parse(count) + 1;
                            ascmAllocateRule.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmAllocateRule.modifyUser = userName;
                            AscmAllocateRuleService.GetInstance().Update(ascmAllocateRule);
                        }
                    }
                }
                #endregion
            }

            //执行事务
            ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
            session.Clear();
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    //更新任务列表
                    if (listTask != null)
                        AscmGetMaterialTaskService.GetInstance().Update(listTask);
                    //更新分配规则列表
                    //if (listNewRule != null)
                    //    listNewRule.ForEach(P => session.Merge(P));
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("分配任务失败(Allocate AscmGetMateiralTask)", ex);
                }
            }
        }
        #endregion

        #region 领料任务生成
        public ActionResult GenerateLogisticsTaskPlanIndex()
        {
            return View();
        }

        public ActionResult GenerateLogisticsTaskPlanList(int? page, int? rows, string sort, string order, string queryWord, string queryDate)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmGetMaterialTask> list = null;

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
                string userLogisticsClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName);

                string whereOther = "", whereQueryWord = "";
                if (userRole.IndexOf("物流班长") > -1 || userRole.IndexOf("物流组长") > -1)
                {
                    string ids_userId = AscmAllocateRuleService.GetInstance().GetLogisticsRankerName(userLogisticsClass, userName);
                    if (!string.IsNullOrEmpty(ids_userId))
                        whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids_userId, "rankerId");
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (userRole.IndexOf("排产员") > -1)
                {
                    whereQueryWord = "rankerId = '" + userName + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                whereQueryWord = "status = '" + AscmGetMaterialTask.StatusDefine.notAllocate + "'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                if (!string.IsNullOrEmpty(queryDate))
                {
                    whereQueryWord = "uploadDate = '" + queryDate + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(whereOther) && !string.IsNullOrEmpty(queryDate))
                {
                    list = AscmGetMaterialTaskService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                    AscmGetMaterialTaskService.GetInstance().SetWarehousePlace(list);

                    if (list != null && list.Count > 0)
                    {
                        foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                        {
                            jsonDataGridResult.rows.Add(ascmGetMaterialTask);
                        }
                    }
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult GenerateLogisticsTaskPlan(string generateTaskDate)
        {
            JsonObjectResult jsonObjectReuslt = new JsonObjectResult();

            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);

                if (string.IsNullOrEmpty(generateTaskDate))
                    throw new Exception("生成日期为空！");

                //获取数据源
                string whereOther = "", whereQueryWord = "";

                if (!string.IsNullOrEmpty(generateTaskDate))
                {
                    generateTaskDate = Convert.ToDateTime(generateTaskDate).ToString("yyyy-MM-dd");
                    whereQueryWord = "time like '" + generateTaskDate + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                whereQueryWord = "status = 1";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                if (userRole.IndexOf(userName) > -1)
                {
                    whereQueryWord = "workerId = '" + userName + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                List<AscmDiscreteJobs> list_jobs = AscmDiscreteJobsService.GetInstance().GetList(null, "", "", "", whereOther);
                if (list_jobs == null || list_jobs.Count == 0)
                    throw new Exception("满足条件的排产单(作业)不存在或者作业数量为0 ！");

                string ids_jobs = string.Empty;
                if (list_jobs != null && list_jobs.Count > 0)
                {
                    foreach (AscmDiscreteJobs ascmDiscreteJobs in list_jobs)
                    {
                        if (!string.IsNullOrEmpty(ids_jobs) && ascmDiscreteJobs.wipEntityId > 0)
                            ids_jobs += ",";
                        if (ascmDiscreteJobs.wipEntityId > 0)
                            ids_jobs += ascmDiscreteJobs.wipEntityId;
                    }
                }

                whereQueryWord = "ami.wipSupplyType = 1";

                List<AscmWipRequirementOperations> list_operations = AscmWipRequirementOperationsService.GetInstance().GetList(null, "", "", ids_jobs, whereQueryWord);
                if (list_operations != null && list_operations.Count > 0)
                {
                    //生成领料任务
                    List<AscmGetMaterialTask> list = GenerateTask(list_operations, generateTaskDate);

                    if (list != null && list.Count > 0)
                    {
                        //保存任务
                        SaveTask(list, userName, list_jobs);
                    }
                }
                jsonObjectReuslt.result = true;
                jsonObjectReuslt.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectReuslt.result = false;
                jsonObjectReuslt.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectReuslt);
            return Content(sReturn);
        }
        #endregion

        #region 领料任务分配
        public ActionResult AllocateLogisticsTaskIndex()
        {
            return View();
        }

        public ActionResult AllocateLogisticsTaskList(int? page, int? rows, string sort, string order, string queryWord, string queryStartDate, string queryEndDate, string queryStartJobDate, string queryEndJobDate, string queryStatus, string queryLine, string queryType, string queryWarehouse)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmGetMaterialTask> list = null;

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
                string userLogisticsClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName);

                string whereOther = "", whereQueryWord = "";
                if (userRole.IndexOf("物流班长") > -1 || userRole.IndexOf("物流组长") > -1)
                {
                    string whereOrQueryWord = "";
                    string ids_userId = AscmAllocateRuleService.GetInstance().GetLogisticsRankerName(userLogisticsClass, userName);
                    if (!string.IsNullOrEmpty(ids_userId))
                        whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids_userId, "rankerId");
                    whereOrQueryWord = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(whereOrQueryWord, whereQueryWord);

                    whereQueryWord = "rankerId is null";
                    whereOrQueryWord = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(whereOrQueryWord, whereQueryWord);

                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereOrQueryWord);
                }
                else if (userRole.IndexOf("排产员") > -1)
                {
                    whereQueryWord = "rankerId = '" + userName + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (userRole.IndexOf("领料员") > -1)
                {
                    whereQueryWord = "workerId = '" + userName + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (string.IsNullOrEmpty(queryStartDate) && string.IsNullOrEmpty(queryEndDate) && string.IsNullOrEmpty(queryStartJobDate) && string.IsNullOrEmpty(queryEndJobDate))
                    throw new Exception("请选择生成日期或作业日期");

                if (!string.IsNullOrEmpty(queryStartDate) && !string.IsNullOrEmpty(queryEndDate))
                {
                    queryStartDate = queryStartDate + " 00:00:00";
                    queryEndDate = queryEndDate + " 23:59:59";
                    whereQueryWord = "createTime >= '" + queryStartDate + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    whereQueryWord = "createTime <= '" + queryEndDate + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (!string.IsNullOrEmpty(queryStartDate) && string.IsNullOrEmpty(queryEndDate))
                {
                    whereQueryWord = "createTime like '" + queryStartDate + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(queryStartDate) && !string.IsNullOrEmpty(queryEndDate))
                {
                    whereQueryWord = "createTime like '" + queryEndDate + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryStartJobDate) && !string.IsNullOrEmpty(queryEndJobDate))
                {
                    queryStartJobDate = queryStartJobDate + " 00:00:00";
                    queryEndJobDate = queryEndJobDate + " 23:59:59";
                    whereQueryWord = "dateReleased >= '" + queryStartJobDate + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    whereQueryWord = "dateReleased <= '" + queryEndJobDate + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (!string.IsNullOrEmpty(queryStartJobDate) && string.IsNullOrEmpty(queryEndJobDate))
                {
                    whereQueryWord = "dateReleased like '" + queryStartJobDate + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(queryStartJobDate) && !string.IsNullOrEmpty(queryEndJobDate))
                {
                    whereQueryWord = "dateReleased like '" + queryEndJobDate + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryStatus))
                {
                    whereQueryWord = "status = '" + queryStatus + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryLine))
                {
                    whereQueryWord = "productline like '" + queryLine.Trim() + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryType))
                {
                    whereQueryWord = "IdentificationId =" + queryType;
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryWarehouse))
                {
                    whereQueryWord = "warehouserId = '" + queryWarehouse + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                list = AscmGetMaterialTaskService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                AscmGetMaterialTaskService.GetInstance().SetRanker(list);
                AscmGetMaterialTaskService.GetInstance().SetWorker(list);
                AscmGetMaterialTaskService.GetInstance().SetWarehousePlace(list);
                AscmGetMaterialTaskService.GetInstance().SumQuantity(list);
                list = list.OrderBy(e => e.statusInt).ToList<AscmGetMaterialTask>();
                if (list != null && list.Count > 0)
                {
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                    {
                        jsonDataGridResult.rows.Add(ascmGetMaterialTask);
                    }
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult AllocateLogisticsTask()
        {
            JsonObjectResult jsonObjectReuslt = new JsonObjectResult();

            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                AllocateTask(userName);
                
                jsonObjectReuslt.result = true;
                jsonObjectReuslt.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectReuslt.result = false;
                jsonObjectReuslt.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectReuslt);
            return Content(sReturn);
        }
        public ActionResult AllocateLogisticsTaskEdit(int? id)
        {
            AscmGetMaterialTask ascmGetMaterialTask = null;
            try
            {
                if (id.HasValue)
                {
                    ascmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().Get(id.Value);
                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.workerId))
                    {
                        YnUser ascmWorker = YnUserService.GetInstance().Get(ascmGetMaterialTask.workerId);
                        ascmGetMaterialTask.ascmWorker = ascmWorker;
                    }
                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.rankerId))
                    {
                        YnUser ascmRanker = YnUserService.GetInstance().Get(ascmGetMaterialTask.rankerId);
                        ascmGetMaterialTask.ascmRanker = ascmRanker;
                    }

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.relatedMark))
                    {
                        List<AscmGetMaterialTask> list = new List<AscmGetMaterialTask>();
                        list.Add(ascmGetMaterialTask);

                        AscmGetMaterialTaskService.GetInstance().SetRelatedMark(list);

                        foreach (AscmGetMaterialTask item in list)
                        {
                            ascmGetMaterialTask.listAscmMarkTaskLog = item.listAscmMarkTaskLog;
                            AscmMarkTaskLogService.GetInstance().SetWipEntities(item.listAscmMarkTaskLog);
                            AscmMarkTaskLogService.GetInstance().SetGetMaterialTask(item.listAscmMarkTaskLog);
                            foreach (AscmMarkTaskLog ascmMarkTaskLog in item.listAscmMarkTaskLog)
                            {
                                if (!string.IsNullOrEmpty(item.relatedMarkInfo))
                                    item.relatedMarkInfo += ",";
                                item.relatedMarkInfo += ascmMarkTaskLog.ascmGetMaterialTaskId + "<" + ascmMarkTaskLog.ascmWipEntitiesName + ">";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(ascmGetMaterialTask, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult AllocateLogisticsTaskEditSave(AscmGetMaterialTask ascmGetMaterialTask_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }
            try
            {

                if (id.HasValue)
                {
                    AscmGetMaterialTask ascmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().Get(ascmGetMaterialTask_Model.id);
                    if (ascmGetMaterialTask.taskId.Substring(0, 1) == AscmCommonHelperService.GetInstance().GetConfigTaskWords(0))
                    {
                        string ids = string.Empty;
                        if (!string.IsNullOrEmpty(ascmGetMaterialTask.workerId))
                            ids += "'" + ascmGetMaterialTask.workerId + "'";
                        if (!string.IsNullOrEmpty(ids) && !string.IsNullOrEmpty(ascmGetMaterialTask_Model.WorkerName))
                            ids += ",";
                        if (!string.IsNullOrEmpty(ascmGetMaterialTask_Model.WorkerName))
                            ids += "'" + ascmGetMaterialTask_Model.WorkerName + "'";

                        string whereOther = "", whereQueryWord = "";
                        whereQueryWord = "workerName in (" + ids + ")";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                        List<AscmAllocateRule> list = AscmAllocateRuleService.GetInstance().GetList(null, "", "", "", whereOther);
                        if (list != null && list.Count > 0)
                        {
                            foreach (AscmAllocateRule ascmAllocateRule in list)
                            {
                                if (ascmAllocateRule.workerName == ascmGetMaterialTask.workerId)
                                {
                                    ascmAllocateRule.taskCount++;
                                }
                                else if (ascmAllocateRule.workerName == ascmGetMaterialTask_Model.WorkerName)
                                {
                                    ascmAllocateRule.taskCount--;
                                }
                                ascmAllocateRule.modifyUser = userName;
                                ascmAllocateRule.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            }
                        }

                        AscmAllocateRuleService.GetInstance().Update(list);
                    }
                    if (!string.IsNullOrEmpty(ascmGetMaterialTask_Model.WorkerName))
                    {
                        ascmGetMaterialTask.workerId = ascmGetMaterialTask_Model.WorkerName;
                        ascmGetMaterialTask.status = "NOTEXECUTE";
                        AscmGetMaterialTaskService.GetInstance().Update(ascmGetMaterialTask);
                    }
                }

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult AllocateLogisticsTaskAddSave(AscmGetMaterialTask ascmGetMaterialTask_Model, string warehouserPlace)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            AscmGetMaterialTask ascmGetMaterialTask = null;
            AscmMarkTaskLog ascmMarkTaskLog = null;

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
                string userLogisticsClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName);

                if (string.IsNullOrEmpty(ascmGetMaterialTask_Model.tip) || string.IsNullOrEmpty(ascmGetMaterialTask_Model.warehouserId))
                    throw new Exception("作业内容或子库不能为空！");
                int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmGetMaterialTask where TaskId like '%L%' AND CREATETIME like '%" + DateTime.Now.ToString("yyyy-MM-dd") + "%'");
                ascmGetMaterialTask_Model.taskId = (maxId == 0) ? "L1001" : "L" + (int.Parse(AscmGetMaterialTaskService.GetInstance().Get(maxId).taskId.Substring(1, 4)) + 1).ToString();
                ascmGetMaterialTask_Model.id = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmGetMaterialTask ") + 1;
                ascmGetMaterialTask_Model.createUser = userName;
                ascmGetMaterialTask_Model.modifyUser = userName;
                ascmGetMaterialTask_Model.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmGetMaterialTask_Model.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                if (!string.IsNullOrEmpty(userRole) && userRole.IndexOf("领料员") > -1)
                {
                    ascmGetMaterialTask_Model.workerId = userName;
                    ascmGetMaterialTask_Model.logisticsClass = userLogisticsClass;
                    ascmGetMaterialTask_Model.status = AscmGetMaterialTask.StatusDefine.notExecute;
                }
                else
                {
                    ascmGetMaterialTask_Model.status = AscmGetMaterialTask.StatusDefine.notAllocate;
                }

                ascmGetMaterialTask_Model.materialType = 1;
                ascmGetMaterialTask_Model.uploadDate = DateTime.Now.ToString("yyyy-MM-dd");
                ascmGetMaterialTask_Model.which = 1;

                if (!string.IsNullOrEmpty(ascmGetMaterialTask_Model.relatedMarkId.ToString()) && ascmGetMaterialTask_Model.relatedMarkId > 0)
                {
                    ascmMarkTaskLog = AscmMarkTaskLogService.GetInstance().Get(ascmGetMaterialTask_Model.relatedMarkId);
                    if (ascmMarkTaskLog != null)
                    {
                        ascmMarkTaskLog.isMark = 0;
                        ascmMarkTaskLog.modifyUser = userName;
                        ascmMarkTaskLog.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                        
                    }
                    ascmGetMaterialTask_Model.relatedMark += ascmGetMaterialTask_Model.relatedMarkId.ToString();
                }

                ascmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().Get(ascmMarkTaskLog.taskId);
                if (!string.IsNullOrEmpty(ascmGetMaterialTask.relatedMark))
                {
                    if (ascmGetMaterialTask.relatedMark.IndexOf(',') > -1)
                    {
                        if (ascmGetMaterialTask.relatedMark.IndexOf(ascmMarkTaskLog.id.ToString()) > -1)
                        {
                            ascmGetMaterialTask.relatedMark = ascmGetMaterialTask.relatedMark.Replace(ascmMarkTaskLog.id.ToString(), "");
                            string[] stringArray = ascmGetMaterialTask.relatedMark.Split(',');
                            string stringRelatedMark = "";
                            foreach (string item in stringArray)
                            {
                                if (!string.IsNullOrEmpty(item))
                                    stringRelatedMark += ",";
                                stringRelatedMark += item;
                            }
                            ascmGetMaterialTask.relatedMark = stringRelatedMark;
                        }
                    }
                    else
                    {
                        if (ascmGetMaterialTask.relatedMark == ascmMarkTaskLog.id.ToString())
                            ascmGetMaterialTask.relatedMark = null;
                    }
                }

                //保存临时任务
                if (ascmGetMaterialTask_Model != null)
                    AscmGetMaterialTaskService.GetInstance().Save(ascmGetMaterialTask_Model);

                //修改标记任务
                if (ascmGetMaterialTask != null)
                    AscmGetMaterialTaskService.GetInstance().Update(ascmGetMaterialTask);

                //修改关联标记
                if(ascmMarkTaskLog != null)
                AscmMarkTaskLogService.GetInstance().Update(ascmMarkTaskLog);

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult AllocateLogisticsTaskDelete(string releaseHeaderIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }
            try
            {
                string whereOther = "", whereQueryWord = "";

                if (!string.IsNullOrEmpty(releaseHeaderIds))
                {
                    whereQueryWord = "id in (" + releaseHeaderIds + ")";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                whereQueryWord = "taskId like '%L%'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                //whereQueryWord = "IdentificationId = " + AscmGetMaterialTask.IdentificationIdDefine.ls.ToString();
                //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                List<AscmGetMaterialTask> list = AscmGetMaterialTaskService.GetInstance().GetList(null, "", "", "", whereOther);
                if (list == null || list.Count == 0)
                    throw new Exception("非临时任务不允许删除！");
                AscmGetMaterialTaskService.GetInstance().Delete(list);

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        #endregion

        #region 领料任务监控
        public ActionResult MonitorLogisticsTaskIndex()
        {
            return View();
        }


        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <param name="queryWord"></param>
        /// <param name="queryStartDate"></param>
        /// <param name="queryEndDate"></param>
        /// <param name="queryStartJobDate"></param>
        /// <param name="queryEndJobDate"></param>
        /// <param name="queryStatus"></param>
        /// <param name="queryLine"></param>
        /// <param name="queryType"></param>
        /// <param name="queryWarehouse"></param>
        /// <param name="queryFormat"></param>
        /// <param name="queryPerson"></param>
        /// <param name="taskString"></param>
        /// <param name="queryWipEntity"></param>
        /// <returns></returns>
        public ActionResult MonitorLogisticsTaskList(int? page, int? rows, string sort, string order, string queryWord, string queryStartDate, string queryEndDate, string queryStartJobDate, string queryEndJobDate, string queryStatus, string queryLine, string queryType, string queryWarehouse, string queryFormat, string queryPerson, string taskString, string queryWipEntity)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;
            ActionResult result=null;
         //   string ids = "";
           JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
         //   List<object> list = null;

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
               string taskIds  = AscmGetMaterialTaskService.GetInstance().GetMonitorTaskList( "", "", queryWord, userName, queryStatus, queryLine, queryType, queryStartDate, queryEndDate, taskString, queryWarehouse, queryFormat, queryPerson, queryStartJobDate, queryEndJobDate, queryWipEntity);
               //var ss=  from p in list;
               
                if (!string.IsNullOrEmpty(taskIds))
                {
                    result = LoadWipDiscreteJobsList(ynPage, taskIds);
                    //foreach (object ascmGetMaterialTask  in   list)
                    //{
                    //    if (!string.IsNullOrEmpty(ids))
                    //        ids += ",";
                    //    ids += "" + ascmGetMaterialTask +"";                      
                    //}
                }
                
                //jsonDataGridResult.total = ynPage.GetRecordCount();
               // jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {

                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return result != null ? result : Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 作业号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LoadWipDiscreteJobsList(YnBaseDal.YnPage page,string ids)
        {
            List<AscmWipDiscreteJobs> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();

            try
            {
                list = AscmWipDiscreteJobsService.GetInstance().GetWipDiscreteJobsSumListI(page, ids);

                if (list != null && list.Count > 0)
                {
                    foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                    {
                        ascmWipDiscreteJobs.wipEntityIdentification = ascmWipDiscreteJobs.wipEntityId.ToString() +"_"+ ascmWipDiscreteJobs.mtlCategoryStatus;
                        jsonDataGridResult.rows.Add(ascmWipDiscreteJobs);
                    }
                    jsonDataGridResult.total = page.GetRecordCount();
                }

                jsonDataGridResult.result = true;
                jsonDataGridResult.message = "";
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <param name="queryWord"></param>
        /// <param name="taskId"></param>
        /// <param name="jobId"></param>
        /// <param name="queryBomWarehouse"></param>
        /// <param name="queryBomMtlCategory"></param>
        /// <returns></returns>
        public ActionResult LoadRequirementOperationsList(int? page, int? rows, string sort, string order, string queryWord, string taskId, string jobId, string queryBomWarehouse, string queryBomMtlCategory)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmWipRequirementOperations> list = null;

            try
            {
                list = AscmWipRequirementOperationsService.GetInstance().GetList("", "", "", "", taskId, jobId, queryBomWarehouse, queryBomMtlCategory);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmWipRequirementOperations ascmWipRequirementOperations in list)
                    {
                        jsonDataGridResult.rows.Add(ascmWipRequirementOperations);
                    }
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult LoadRequirementOperationsList(int? page, int? rows, string sort, string order, string queryWord, string taskId, string jobId, string queryBomWarehouse, string queryBomMtlCategory)
        //{
        //    YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
        //    ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
        //    ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

        //    JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
        //    List<AscmWipRequirementOperations> list = null;

        //    try
        //    {
        //        list = AscmWipRequirementOperationsService.GetInstance().GetList(ynPage, "", "", "", "", taskId, jobId, queryBomWarehouse, queryBomMtlCategory);
        //        if (list != null && list.Count > 0)
        //        {
        //            foreach (AscmWipRequirementOperations ascmWipRequirementOperations in list)
        //            {
        //                jsonDataGridResult.rows.Add(ascmWipRequirementOperations);
        //            }
        //        }
        //        jsonDataGridResult.result = true;
        //        jsonDataGridResult.total = ynPage.GetRecordCount();
        //    }
        //    catch (Exception ex)
        //    {
        //        jsonDataGridResult.result = false;
        //        jsonDataGridResult.message = ex.Message;
        //    }

        //    return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public ContentResult StartExcuteTask(string releaseHeaderIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            List<AscmGetMaterialTask> list = null;

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                string whereOther = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(releaseHeaderIds))
                {
                    whereQueryWord = "id in (" + releaseHeaderIds + ")";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                List<AscmGetMaterialTask> listAscmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().GetList(null, "", "", "", whereOther);
                if (listAscmGetMaterialTask != null && listAscmGetMaterialTask.Count > 0)
                {
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in listAscmGetMaterialTask)
                    {
                        if (ascmGetMaterialTask.status == AscmGetMaterialTask.StatusDefine.notExecute)
                        {
                            ascmGetMaterialTask.status = AscmGetMaterialTask.StatusDefine.execute;
                            ascmGetMaterialTask.starTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.modifyUser = userName;

                            if (list == null)
                                list = new List<AscmGetMaterialTask>();

                            list.Add(ascmGetMaterialTask);
                        }
                    }
                }

                if (list == null || list.Count == 0)
                    throw new Exception("选择的任务不满足开始的条件");

                AscmGetMaterialTaskService.GetInstance().Update(list);
                jsonObjectResult.result = true;
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult EndExcuteTask(string releaseHeaderIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            List<AscmGetMaterialTask> list = new List<AscmGetMaterialTask>();

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                string whereOther = "", whereQueryWord = "", whereOtherRule = "";
                if (!string.IsNullOrEmpty(releaseHeaderIds))
                {
                    whereQueryWord = "id in (" + releaseHeaderIds + ")";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                whereQueryWord = "status = '" + AscmGetMaterialTask.StatusDefine.execute + "'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                whereQueryWord = "ruleType = '" + AscmGenerateTaskRule.RuleTypeDefine.typeofWarehouse + "'";
                whereOtherRule = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOtherRule, whereQueryWord);
                List<AscmGetMaterialTask> listAscmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().GetList(null, "", "", "", whereOther);
                List<AscmGenerateTaskRule> listAscmGenerateTaskRule = AscmGenerateTaskRuleService.GetInstance().GetList(null, "", "", "", whereOtherRule);
                if (listAscmGetMaterialTask != null && listAscmGetMaterialTask.Count > 0 && listAscmGenerateTaskRule != null && listAscmGenerateTaskRule.Count > 0)
                {
                    AscmGetMaterialTaskService.GetInstance().SumQuantity(listAscmGetMaterialTask);
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in listAscmGetMaterialTask)
                    {
                        string warehousePlace = string.Empty;
                        if (!string.IsNullOrEmpty(ascmGetMaterialTask.warehouserId))
                            warehousePlace = ascmGetMaterialTask.warehouserId.Substring(0, 4).ToString();
                        if (ascmGetMaterialTask.taskId.IndexOf("T") > -1)
                        {
                            foreach (AscmGenerateTaskRule ascmGenerateTaskRule in listAscmGenerateTaskRule)
                            {
                                if (!string.IsNullOrEmpty(ascmGenerateTaskRule.relatedRanker) && ascmGenerateTaskRule.relatedRanker == ascmGetMaterialTask.rankerId)
                                {
                                    if (!string.IsNullOrEmpty(ascmGenerateTaskRule.ruleCode))
                                    {
                                        string[] myArray = ascmGenerateTaskRule.ruleCode.Split('&');
                                        string warehouseString = myArray[0].Substring(myArray[0].IndexOf("(") + 1, myArray[0].IndexOf(")") - myArray[0].IndexOf("(") - 1);

                                        if (!string.IsNullOrEmpty(warehousePlace) && warehouseString.IndexOf(warehousePlace) > -1)
                                        {
                                            ascmGetMaterialTask.status = "FINISH";
                                            ascmGetMaterialTask.endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                            ascmGetMaterialTask.modifyUser = userName;
                                            ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                                            list.Add(ascmGetMaterialTask);
                                            break;
                                        }
                                        else if (!string.IsNullOrEmpty(warehousePlace) && warehouseString.IndexOf(warehousePlace) == -1)
                                        {
                                            if (ascmGetMaterialTask.totalGetMaterialQuantity == ascmGetMaterialTask.totalRequiredQuantity)
                                            {
                                                ascmGetMaterialTask.status = "FINISH";
                                                if (!string.IsNullOrEmpty(ascmGetMaterialTask.endTime))
                                                    ascmGetMaterialTask.errorTime += ",";
                                                ascmGetMaterialTask.errorTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                                ascmGetMaterialTask.modifyUser = userName;
                                                ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                                                list.Add(ascmGetMaterialTask);
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (string.IsNullOrEmpty(ascmGenerateTaskRule.relatedRanker))
                                {
                                    if (!string.IsNullOrEmpty(ascmGenerateTaskRule.ruleCode))
                                    {
                                        string[] myArray = ascmGenerateTaskRule.ruleCode.Split('&');
                                        string warehouseString = myArray[0].Substring(myArray[0].IndexOf("(") + 1, myArray[0].IndexOf(")") - myArray[0].IndexOf("(") - 1);

                                        if (!string.IsNullOrEmpty(warehousePlace) && warehouseString.IndexOf(warehousePlace) > -1)
                                        {
                                            ascmGetMaterialTask.status = "FINISH";
                                            ascmGetMaterialTask.endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                            ascmGetMaterialTask.modifyUser = userName;
                                            ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                                            list.Add(ascmGetMaterialTask);
                                            break;
                                        }
                                        else if (!string.IsNullOrEmpty(warehousePlace) && warehouseString.IndexOf(warehousePlace) == -1)
                                        {
                                            if (ascmGetMaterialTask.totalGetMaterialQuantity == ascmGetMaterialTask.totalRequiredQuantity && ascmGetMaterialTask.totalGetMaterialQuantity != 0)
                                            {
                                                ascmGetMaterialTask.status = "FINISH";
                                                if (!string.IsNullOrEmpty(ascmGetMaterialTask.endTime))
                                                    ascmGetMaterialTask.errorTime += ",";
                                                ascmGetMaterialTask.errorTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                                ascmGetMaterialTask.modifyUser = userName;
                                                ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                                                list.Add(ascmGetMaterialTask);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            ascmGetMaterialTask.status = "FINISH";
                            ascmGetMaterialTask.endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.modifyUser = userName;
                            ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                            list.Add(ascmGetMaterialTask);
                        }
                    }
                }

                if (list == null || list.Count == 0)
                    throw new Exception("选择的任务不满足结束条件");

                AscmGetMaterialTaskService.GetInstance().Update(list);
                jsonObjectResult.result = true;
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult ConfrimedBatchGetMaterial(string releaseHeaderIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                bool result = AscmGetMaterialTaskService.GetInstance().BatchGetMaterialTask(userName, releaseHeaderIds);

                if (!result)
                    throw new Exception("请检查提交数据的接口！");
                jsonObjectResult.result = result;
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult GetMaterialNoticeInfo()
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                string message = AscmGetMaterialTaskService.GetInstance().GetNotifierMessageList(userName);

                jsonObjectResult.result = true;
                jsonObjectResult.message = message;
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult MarkTask(int? id, int? wipEntityId)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                bool isContainWarehouse = false;
                string warehouse = string.Empty;
                AscmGetMaterialTask ascmGetMaterialTask = null;
                if (id.HasValue)
                {
                    ascmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().Get(id.Value);
                    if (ascmGetMaterialTask == null)
                        throw new Exception("该任务不存在！");
                    warehouse = string.IsNullOrEmpty(ascmGetMaterialTask.warehouserId) ? null : ascmGetMaterialTask.warehouserId.Substring(0, 4).ToUpper().ToString();
                }

                string whereOther = "", whereQueryWord = "";

                whereQueryWord = "ruleType = '" + AscmGenerateTaskRule.RuleTypeDefine.typeofWarehouse + "'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                whereQueryWord = "identificationId = " + ascmGetMaterialTask.IdentificationId;
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                List<AscmGenerateTaskRule> list = AscmGenerateTaskRuleService.GetInstance().GetList(null, "", "", "", whereOther);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmGenerateTaskRule ascmGenerateTaskRule in list)
                    {
                        if (!string.IsNullOrEmpty(ascmGenerateTaskRule.relatedRanker) && ascmGenerateTaskRule.relatedRanker == ascmGetMaterialTask.rankerId)
                        {
                            if (!string.IsNullOrEmpty(ascmGenerateTaskRule.ruleCode))
                            {
                                string[] myArray = ascmGenerateTaskRule.ruleCode.Split('&');
                                string warehouseString = myArray[0].Substring(myArray[0].IndexOf("(") + 1, myArray[0].IndexOf(")") - myArray[0].IndexOf("(") - 1);
                                if (!string.IsNullOrEmpty(warehouse) && warehouseString.IndexOf(warehouse) > -1)
                                {
                                    isContainWarehouse = true;
                                    break;
                                }
                            }
                        }
                        else if (string.IsNullOrEmpty(ascmGenerateTaskRule.relatedRanker))
                        {
                            if (!string.IsNullOrEmpty(ascmGenerateTaskRule.ruleCode))
                            {
                                string[] myArray = ascmGenerateTaskRule.ruleCode.Split('&');
                                string warehouseString = myArray[0].Substring(myArray[0].IndexOf("(") + 1, myArray[0].IndexOf(")") - myArray[0].IndexOf("(") - 1);
                                if (!string.IsNullOrEmpty(warehouse) && warehouseString.IndexOf(warehouse) > -1)
                                {
                                    isContainWarehouse = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!isContainWarehouse)
                    throw new Exception("该子库无法标记！");

                if (isContainWarehouse)
                { 
                    AscmMarkTaskLog ascmMarkTaskLog = null;
                    if (!(id.HasValue && wipEntityId.HasValue))
                        throw new Exception("请选择须标记的作业！");

                    if (id.HasValue && wipEntityId.HasValue)
                    {
                        object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmMarkTaskLog where wipEntityId = " + wipEntityId.ToString() + " and taskId = " + id.ToString() + " and isMark = 1");
                        if (object1 == null)
                            throw new Exception("查询异常！");
                        int iCount = 0;
                        if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                            throw new Exception("该作业已标记！");

                        int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmMarkTaskLog");
                        ascmMarkTaskLog = new AscmMarkTaskLog();
                        ascmMarkTaskLog.id = ++maxId;
                        ascmMarkTaskLog.createUser = userName;
                        ascmMarkTaskLog.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmMarkTaskLog.modifyUser = userName;
                        ascmMarkTaskLog.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmMarkTaskLog.wipEntityId = wipEntityId.Value;
                        ascmMarkTaskLog.taskId = id.Value;
                        ascmMarkTaskLog.isMark = 1;
                        ascmMarkTaskLog.markType = "NONAUTO";
                        ascmMarkTaskLog.warehouseId = warehouse;

                        if (!string.IsNullOrEmpty(ascmGetMaterialTask.relatedMark))
                            ascmGetMaterialTask.relatedMark += ",";
                        ascmGetMaterialTask.relatedMark += ascmMarkTaskLog.id.ToString();
                        ascmGetMaterialTask.modifyUser = userName;
                        ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                        //执行事务
                        ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                        session.Clear();
                        using (ITransaction tx = session.BeginTransaction())
                        {
                            try
                            {
                                //更改任务标记
                                if (ascmGetMaterialTask != null)
                                    AscmGetMaterialTaskService.GetInstance().Update(ascmGetMaterialTask);
                                //保存标记
                                if (ascmMarkTaskLog != null)
                                AscmMarkTaskLogService.GetInstance().Save(ascmMarkTaskLog);
                            }
                            catch (Exception ex)
                            {
                                tx.Rollback();
                                YnBaseClass2.Helper.LogHelper.GetLog().Error("标记失败(Mark AscmMarkTaskLog)", ex);
                            }
                        }

                        jsonObjectResult.result = true;
                        jsonObjectResult.message = "";
                    }
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult UnMarkTask(int? id, int? wipEntityId)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            List<AscmGetMaterialTask> list = null;
            List<AscmMarkTaskLog> listAscmMarkTaskLog = null;

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                if (id.HasValue && wipEntityId.HasValue)
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmMarkTaskLog where wipEntityId = " + wipEntityId.ToString() + " and taskId = " + id.ToString() + " and isMark = 1");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount == 0)
                        throw new Exception("该标记不存在！");

                    string whereOther = "", whereQueryWord = "";
                    whereQueryWord = "taskId = " + id.Value.ToString();
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                    whereQueryWord = "wipEntityId = " + wipEntityId.Value.ToString();
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                    whereQueryWord = "isMark = 1";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                    whereQueryWord = "markType = 'NONAUTO'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                    listAscmMarkTaskLog = AscmMarkTaskLogService.GetInstance().GetList(null, "", "", "", whereOther, false, false);
                    if (listAscmMarkTaskLog == null || listAscmMarkTaskLog.Count == 0)
                        throw new Exception("标记不存在！");
                    
                    if (listAscmMarkTaskLog != null && listAscmMarkTaskLog.Count > 0)
                    {
                        foreach (AscmMarkTaskLog ascmMarkTaskLog in listAscmMarkTaskLog)
                        {
                            ascmMarkTaskLog.isMark = 0;
                            ascmMarkTaskLog.modifyUser = userName;
                            ascmMarkTaskLog.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                            AscmGetMaterialTask ascmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().Get(ascmMarkTaskLog.taskId);
                            if (ascmGetMaterialTask.relatedMark.IndexOf(",") > -1)
                            {
                                if (ascmGetMaterialTask.relatedMark.IndexOf(ascmMarkTaskLog.id.ToString()) > -1)
                                {
                                    string markString = ascmGetMaterialTask.relatedMark.Replace(ascmMarkTaskLog.id.ToString(), "");
                                    string[] markArray = markString.Split(',');
                                    string newMarkString = string.Empty;
                                    foreach (string item in markArray)
                                    {
                                        if (!string.IsNullOrEmpty(newMarkString))
                                            newMarkString += ",";
                                        if (!string.IsNullOrEmpty(item))
                                            newMarkString += item;
                                    }
                                    ascmGetMaterialTask.relatedMark = newMarkString;
                                }
                            }
                            else
                            {
                                if (ascmGetMaterialTask.relatedMark == ascmMarkTaskLog.id.ToString())
                                {
                                    ascmGetMaterialTask.relatedMark = "";
                                }
                            }

                            ascmGetMaterialTask.modifyUser = userName;
                            ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");


                            if (list == null)
                                list = new List<AscmGetMaterialTask>();
                            list.Add(ascmGetMaterialTask);
                        }
                    }

                    if (list == null || list.Count == 0)
                        throw new Exception("取消标记失败！");

                    //执行事务
                    ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                    session.Clear();
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        try
                        {
                            //更新（取消）任务标记
                            if (list != null)
                                AscmGetMaterialTaskService.GetInstance().Update(list);
                            //更新标记
                            if (listAscmMarkTaskLog != null)
                                AscmMarkTaskLogService.GetInstance().Update(listAscmMarkTaskLog);
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();
                            YnBaseClass2.Helper.LogHelper.GetLog().Error("标记失败(Mark AscmMarkTaskLog)", ex);
                        }
                    }

                    jsonObjectResult.result = true;
                    jsonObjectResult.message = "";
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult CloseGetMaterialTask(string releaseHeaderIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            List<AscmGetMaterialTask> list = null;

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                string whereOther = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(releaseHeaderIds))
                {
                    whereQueryWord = "id in (" + releaseHeaderIds + ")";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                whereQueryWord = "status != '" + AscmGetMaterialTask.StatusDefine.finish + "'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                List<AscmGetMaterialTask> listAscmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().GetList(null, "", "", "", whereOther);
                if (listAscmGetMaterialTask != null && listAscmGetMaterialTask.Count > 0)
                {
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in listAscmGetMaterialTask)
                    {
                        ascmGetMaterialTask.status = AscmGetMaterialTask.StatusDefine.close;
                        ascmGetMaterialTask.modifyUser = userName;
                        ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                        if (list == null)
                            list = new List<AscmGetMaterialTask>();
                        list.Add(ascmGetMaterialTask);
                    }
                }

                if (list == null || list.Count == 0)
                    throw new Exception("找不到更新的任务！");

                if (list != null && list.Count > 0)
                {
                    AscmGetMaterialTaskService.GetInstance().Save(list);
                }

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = true;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        #endregion

        #region 物料类别
        public ActionResult MaterialCategoryIndex()
        {
            return View();
        }

        //物料大类
        public ActionResult MaterialCategoryList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            List<AscmMaterialCategory> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmMaterialCategoryService.GetInstance().GetList(ynPage, "", "", queryWord, null);
                if (list != null)
                {
                    foreach (AscmMaterialCategory ascmMaterialCategory in list)
                    {
                        jsonDataGridResult.rows.Add(ascmMaterialCategory);
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
        public ActionResult MaterialCategoryEdit(int? id)
        {
            AscmMaterialCategory ascmMaterialCategory = null;
            try
            {
                if (id.HasValue)
                {
                    ascmMaterialCategory = AscmMaterialCategoryService.GetInstance().Get(id.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmMaterialCategory, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult MaterialCategorySave(AscmMaterialCategory ascmMaterialCategory_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                AscmMaterialCategory ascmMaterialCategory = null;
                if (id.HasValue)
                {
                    ascmMaterialCategory = AscmMaterialCategoryService.GetInstance().Get(id.Value);
                    ascmMaterialCategory.modifyUser = userName;
                    ascmMaterialCategory.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    ascmMaterialCategory = new AscmMaterialCategory();
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmMaterialCategory");
                    ascmMaterialCategory.id = ++maxId;
                    ascmMaterialCategory.createUser = userName;
                    ascmMaterialCategory.createUser = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmMaterialCategory.modifyUser = userName;
                    ascmMaterialCategory.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }

                if (ascmMaterialCategory == null)
                    throw new Exception("保存物料大类信息失败!");

                ascmMaterialCategory.description = ascmMaterialCategory_Model.description;
                ascmMaterialCategory.tip = ascmMaterialCategory_Model.tip;

                if (id.HasValue)
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmMaterialCategory id <>" + id.Value + " ");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已存在此物料大类信息【" + ascmMaterialCategory_Model.categoryCode + "】");
                    AscmMaterialCategoryService.GetInstance().Update(ascmMaterialCategory);
                }
                else
                {
                    AscmMaterialCategoryService.GetInstance().Save(ascmMaterialCategory);
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmMaterialCategory.id.ToString();
                jsonObjectResult.entity = ascmMaterialCategory;
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }

        //物料子类
        public ActionResult MaterialSubCategoryList(int? page, int? rows, string sort, string order, string queryWord, string queryOtherWord, string categoryCode)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber

            List<AscmMaterialSubCategory> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (string.IsNullOrEmpty(categoryCode))
                    throw new Exception("物料大类不能为空！");

                string whereOther = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(categoryCode))
                {
                    AscmMaterialCategory ascmMaterialCategory = AscmMaterialCategoryService.GetInstance().GetId(categoryCode);
                    whereQueryWord = "categoryId = " + ascmMaterialCategory.id.ToString();
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryWord) && !string.IsNullOrEmpty(queryOtherWord))
                {
                    if (queryWord == queryOtherWord)
                    {
                        whereQueryWord = "subCategoryCode like '" + queryWord + "%'";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    }
                    else
                    {
                        whereQueryWord = "subCategoryCode >= '" + queryWord + "'";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                        whereQueryWord = "subCategoryCode <= '" + queryOtherWord + "'";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    }
                }
                else if (!string.IsNullOrEmpty(queryWord) && string.IsNullOrEmpty(queryOtherWord))
                {
                    whereQueryWord = "subCategoryCode like '" + queryWord + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(queryWord) && !string.IsNullOrEmpty(queryOtherWord))
                {
                    whereQueryWord = "subCategoryCode like '" + queryOtherWord + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                list = AscmMaterialSubCategoryService.GetInstance().GetList(ynPage, "combinationCode", "", "", whereOther);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmMaterialSubCategory ascmMaterialSubCategory in list)
                    {
                        jsonDataGridResult.rows.Add(ascmMaterialSubCategory);
                    }
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MaterialSubCategoryEdit(int? id)
        {
            AscmMaterialSubCategory ascmMaterialSubCategory = null;
            try
            {
                if (id.HasValue)
                {
                    ascmMaterialSubCategory = AscmMaterialSubCategoryService.GetInstance().Get(id.Value);
                    if (ascmMaterialSubCategory != null)
                    {
                        AscmMaterialCategory ascmMaterialCategory = AscmMaterialCategoryService.GetInstance().Get(ascmMaterialSubCategory.categoryId);
                        ascmMaterialSubCategory.ascmMaterialCategory = ascmMaterialCategory;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(ascmMaterialSubCategory, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadMaterialSubCategoryList(int? page, int? rows, string sort, string order, string queryWord, int? categoryId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmMaterialSubCategory> list = null;
            try
            {
                string whereOther = "", whereQueryWord = "";
                if (categoryId != null && categoryId != 0)
                {
                    whereQueryWord = "categoryId = " + categoryId;
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                list = AscmMaterialSubCategoryService.GetInstance().GetList(ynPage, "combinationCode", "", "", whereOther);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmMaterialSubCategory ascmMaterialSubCategory in list)
                    {
                        jsonDataGridResult.rows.Add(ascmMaterialSubCategory);
                    }
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult MaterialSubCategorySave(AscmMaterialSubCategory ascmMaterialSubCategory_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                AscmMaterialSubCategory ascmMaterialSubCategory = null;
                if (id.HasValue)
                {
                    ascmMaterialSubCategory = AscmMaterialSubCategoryService.GetInstance().Get(id.Value);
                    ascmMaterialSubCategory.modifyUser = userName;
                    ascmMaterialSubCategory.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    ascmMaterialSubCategory = new AscmMaterialSubCategory();
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmMaterialSubCategory");
                    ascmMaterialSubCategory.id = ++maxId;
                    ascmMaterialSubCategory.createUser = userName;
                    ascmMaterialSubCategory.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmMaterialSubCategory.modifyUser = userName;
                    ascmMaterialSubCategory.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }

                if (ascmMaterialSubCategory == null)
                    throw new Exception("保存物料子类信息失败 ！");
                if (ascmMaterialSubCategory_Model.subCategoryCode == null || ascmMaterialSubCategory_Model.subCategoryCode == "")
                    throw new Exception("物料子类不能为空！");

                ascmMaterialSubCategory.zMtlCategoryStatus = ascmMaterialSubCategory_Model.zMtlCategoryStatus;
                ascmMaterialSubCategory.dMtlCategoryStatus = ascmMaterialSubCategory_Model.dMtlCategoryStatus;
                ascmMaterialSubCategory.wMtlCategoryStatus = ascmMaterialSubCategory_Model.wMtlCategoryStatus;
                ascmMaterialSubCategory.description = ascmMaterialSubCategory_Model.description;
                ascmMaterialSubCategory.tip = ascmMaterialSubCategory_Model.tip;

                if (id.HasValue)
                    AscmMaterialSubCategoryService.GetInstance().Update(ascmMaterialSubCategory);
                else
                    AscmMaterialSubCategoryService.GetInstance().Save(ascmMaterialSubCategory);

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                //jsonObjectResult.id = ascmMaterialSubCategory.id.ToString();
                //jsonObjectResult.entity = ascmMaterialSubCategory;
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult ChoiceMaterialSubCategorySave(string releaseHeaderIds, string zmtlCategoryStatus, string dmtlCategoryStatus, string wmtlCategoryStatus)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            List<AscmMaterialSubCategory> list = null;
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }
                
                string whereOther = AscmCommonHelperService.GetInstance().IsJudgeListCount(releaseHeaderIds, "id");
                list = AscmMaterialSubCategoryService.GetInstance().GetList(null, "", "", "", whereOther);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmMaterialSubCategory ascmMaterialSubCategory in list)
                    {
                        if (zmtlCategoryStatus != "kz" && !string.IsNullOrEmpty(zmtlCategoryStatus))
                        {
                            ascmMaterialSubCategory.zMtlCategoryStatus = zmtlCategoryStatus;
                        }
                        else if (zmtlCategoryStatus == "kz")
                        {
                            ascmMaterialSubCategory.zMtlCategoryStatus = "";
                        }

                        if (dmtlCategoryStatus != "kz" && !string.IsNullOrEmpty(dmtlCategoryStatus))
                        {
                            ascmMaterialSubCategory.dMtlCategoryStatus = dmtlCategoryStatus;
                        }
                        else if (dmtlCategoryStatus == "kz")
                        {
                            ascmMaterialSubCategory.dMtlCategoryStatus = "";
                        }

                        if (wmtlCategoryStatus != "kz" && !string.IsNullOrEmpty(wmtlCategoryStatus))
                        {
                            ascmMaterialSubCategory.wMtlCategoryStatus = wmtlCategoryStatus;
                        }
                        else if (wmtlCategoryStatus == "kz")
                        {
                            ascmMaterialSubCategory.wMtlCategoryStatus = "";
                        }

                        ascmMaterialSubCategory.modifyUser = userName;
                        ascmMaterialSubCategory.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    }
                    AscmMaterialSubCategoryService.GetInstance().Update(list);
                }
                jsonObjectResult.result = true;
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }

        //物料
        public ActionResult MaterialItemList(int? page, int? rows, string sort, string order, string queryWord, string queryOtherWord, string comCode)
        {
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber

            List<AscmMaterialItem> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (string.IsNullOrEmpty(comCode))
                    throw new Exception("组合码不能为空！");

                string whereOther = "", whereQueryWord = "";

                whereQueryWord = "wipSupplyType in (1,2,3)";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                if (!string.IsNullOrEmpty(comCode))
                {
                    AscmMaterialSubCategory ascmMaterialSubCategory = AscmMaterialSubCategoryService.GetInstance().GetId(comCode);
                    whereQueryWord = "subCategoryId =" + ascmMaterialSubCategory.id;
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    comCode = comCode.Replace(".", "");
                }

                if (!string.IsNullOrEmpty(queryWord) && !string.IsNullOrEmpty(queryOtherWord))
                {
                    queryWord = comCode + queryWord;
                    queryOtherWord = comCode + queryOtherWord;

                    whereQueryWord = "docNumber >= '" + queryWord + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    whereQueryWord = "docNumber <= '" + queryOtherWord + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (!string.IsNullOrEmpty(queryWord) && string.IsNullOrEmpty(queryOtherWord))
                {
                    queryWord = comCode + queryWord;
                    whereQueryWord = "docNumber like '" + queryWord + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(queryWord) && !string.IsNullOrEmpty(queryOtherWord))
                {
                    queryOtherWord = comCode + queryOtherWord;
                    whereQueryWord = "docNumber like '" + queryOtherWord + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                list = AscmMaterialItemService.GetInstance().GetList(ynPage, "docNumber", "", "", whereOther);
                if (list.Count > 0 && list != null)
                {
                    foreach (AscmMaterialItem ascmMaterialItem in list)
                    {
                        jsonDataGridResult.rows.Add(ascmMaterialItem);
                    }
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MaterialItemEdit(int? id)
        {
            AscmMaterialItem ascmMaterialItem = null;
            try
            {
                if (id.HasValue)
                {
                    ascmMaterialItem = AscmMaterialItemService.GetInstance().Get(id.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(ascmMaterialItem, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadMaterialItemList(int? page, int? rows, string sort, string order, string queryWord, int? subCategoryId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmMaterialItem> list = null;
            try
            {
                string whereOther = "", whereQueryWord = "";
                if (subCategoryId != null && subCategoryId != 0)
                {
                    whereQueryWord = "subCategoryId = " + subCategoryId;
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                whereQueryWord = "wipSupplyType in (1,2,3)";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                list = AscmMaterialItemService.GetInstance().GetList(ynPage, "docNumber", "", "", whereOther);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmMaterialItem ascmMaterialItem in list)
                    {
                        jsonDataGridResult.rows.Add(ascmMaterialItem);
                    }
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult MaterialItemSave(AscmMaterialItem ascmMaterialItem_Model, int? id)
        {
            JsonObjectResult jsonObjectReuslt = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                AscmMaterialItem ascmMaterialItem = null;
                if (id.HasValue)
                {
                    ascmMaterialItem = AscmMaterialItemService.GetInstance().Get(id.Value);
                    ascmMaterialItem.modifyUser = userName;
                    ascmMaterialItem.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    ascmMaterialItem = new AscmMaterialItem();
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmMaterialSubCategory");
                    ascmMaterialItem.id = ++maxId;
                    ascmMaterialItem.createUser = userName;
                    ascmMaterialItem.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmMaterialItem.modifyUser = userName;
                    ascmMaterialItem.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }

                if (ascmMaterialItem == null)
                    throw new Exception("保存物料子类信息失败 ！");

                ascmMaterialItem.zMtlCategoryStatus = ascmMaterialItem_Model.zMtlCategoryStatus;
                ascmMaterialItem.dMtlCategoryStatus = ascmMaterialItem_Model.dMtlCategoryStatus;
                ascmMaterialItem.wMtlCategoryStatus = ascmMaterialItem_Model.wMtlCategoryStatus;

                if (id.HasValue)
                    AscmMaterialItemService.GetInstance().Update(ascmMaterialItem);
                else
                    AscmMaterialItemService.GetInstance().Save(ascmMaterialItem);
                jsonObjectReuslt.result = true;
                jsonObjectReuslt.message = "";
                jsonObjectReuslt.id = ascmMaterialItem.id.ToString();
                jsonObjectReuslt.entity = ascmMaterialItem;
            }
            catch (Exception ex)
            {
                jsonObjectReuslt.result = false;
                jsonObjectReuslt.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectReuslt);
            return Content(sReturn);
        }
        #endregion

        #region 物料备料形式维护
        public ActionResult MaterialStockFormatIndex()
        {
            return View();
        }

        public ActionResult MaterialStockFormatList(int? page, int? rows, string sort, string order, string queryWord, string queryType, string queryStarDocnumber, string queryEndDocnumber, string zmtlCategoryStatus, string dmtlCategoryStatus, string wmtlCategoryStatus, string queryDescribe)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            List<AscmMaterialItem> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereQueryWord = "";

                whereQueryWord = "wipSupplyType < 4";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                if (!string.IsNullOrEmpty(queryType))
                {
                    whereQueryWord = "wipSupplyType = " + queryType;
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryStarDocnumber) && !string.IsNullOrEmpty(queryEndDocnumber))
                {
                    if (queryStarDocnumber == queryEndDocnumber)
                    {
                        whereQueryWord = "docNumber like '" + queryStarDocnumber + "%'";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    }
                    else
                    {
                        whereQueryWord = "docNumber >= '" + queryStarDocnumber + "'";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                        whereQueryWord = "docNumber <= '" + queryEndDocnumber + "'";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    }
                }
                else if (!string.IsNullOrEmpty(queryStarDocnumber) && string.IsNullOrEmpty(queryEndDocnumber))
                {
                    whereQueryWord = "docNumber like '" + queryStarDocnumber + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(queryStarDocnumber) && !string.IsNullOrEmpty(queryEndDocnumber))
                {
                    whereQueryWord = "docNumber like '" + queryEndDocnumber + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else
                {
                    whereQueryWord = "docNumber like '20%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(zmtlCategoryStatus))
                {
                    if (zmtlCategoryStatus != "qb")
                    {
                        if (zmtlCategoryStatus != "kz")
                            whereQueryWord = "zMtlCategoryStatus = '" + zmtlCategoryStatus + "'";
                        else
                            whereQueryWord = "zMtlCategoryStatus is null";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    }
                }

                if (!string.IsNullOrEmpty(dmtlCategoryStatus))
                {
                    if (dmtlCategoryStatus != "qb")
                    {
                        if (dmtlCategoryStatus != "kz")
                            whereQueryWord = "dMtlCategoryStatus = '" + dmtlCategoryStatus + "'";
                        else
                            whereQueryWord = "dMtlCategoryStatus is null";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    }
                }

                if (!string.IsNullOrEmpty(wmtlCategoryStatus))
                {
                    if (wmtlCategoryStatus != "qb")
                    {
                        if (wmtlCategoryStatus != "kz")
                            whereQueryWord = "wMtlCategoryStatus = '" + wmtlCategoryStatus + "'";
                        else
                            whereQueryWord = "wMtlCategoryStatus is null";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    }
                }

                if (string.IsNullOrEmpty(zmtlCategoryStatus) && string.IsNullOrEmpty(dmtlCategoryStatus) && string.IsNullOrEmpty(wmtlCategoryStatus))
                {
                    whereQueryWord = "isFlag = 0";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryDescribe))
                {
                    whereQueryWord = "description like '%" + queryDescribe + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                
                list = AscmMaterialItemService.GetInstance().GetList(ynPage, "", "", "", whereOther, false);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmMaterialItem ascmMaterialItem in list)
                    {
                        jsonDataGridResult.rows.Add(ascmMaterialItem);
                    }
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MaterialStockFormatEdit(int? id)
        {
            AscmMaterialItem ascmMaterialItem = null;
            try
            {
                if (id.HasValue)
                {
                    ascmMaterialItem = AscmMaterialItemService.GetInstance().Get(id.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(ascmMaterialItem, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult MaterialStockFormatSave(AscmMaterialItem ascmMaterialItem_Model, int? id)
        {
            JsonObjectResult jsonObjectReuslt = new JsonObjectResult();

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                AscmMaterialItem ascmMaterialItem = null;
                if (!id.HasValue)
                    throw new Exception("未选择或获取物料信息！");

                ascmMaterialItem = AscmMaterialItemService.GetInstance().Get(id.Value);
                ascmMaterialItem.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmMaterialItem.modifyUser = userName;
                ascmMaterialItem.zMtlCategoryStatus = ascmMaterialItem_Model.zMtlCategoryStatus;
                ascmMaterialItem.dMtlCategoryStatus = ascmMaterialItem_Model.dMtlCategoryStatus;
                ascmMaterialItem.wMtlCategoryStatus = ascmMaterialItem_Model.wMtlCategoryStatus;
                AscmMaterialItemService.GetInstance().Update(ascmMaterialItem);
                    
                jsonObjectReuslt.result = true;
                jsonObjectReuslt.message = "";
                jsonObjectReuslt.id = ascmMaterialItem.id.ToString();
                jsonObjectReuslt.entity = ascmMaterialItem;
            }
            catch (Exception ex)
            {
                jsonObjectReuslt.result = false;
                jsonObjectReuslt.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectReuslt);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult BatchMaterialStockFormatSave(AscmMaterialItem ascmMaterialItem_Model)
        {
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            JsonObjectResult jsonObjectReuslt = new JsonObjectResult();
            List<AscmMaterialItem> list = null;

            try
            {
                if (string.IsNullOrEmpty(ascmMaterialItem_Model.sDocnumber) || string.IsNullOrEmpty(ascmMaterialItem_Model.eDocnumber))
                    throw new Exception("编码段不能为空！");

                string whereOther = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(ascmMaterialItem_Model.wipSupplyType.ToString()) && ascmMaterialItem_Model.wipSupplyType.ToString() != "0")
                {
                    whereQueryWord = "wipSupplyType = " + ascmMaterialItem_Model.wipSupplyType.ToString();
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(ascmMaterialItem_Model.sDocnumber) && !string.IsNullOrEmpty(ascmMaterialItem_Model.eDocnumber))
                {
                    whereQueryWord = "docNumber >= '" + ascmMaterialItem_Model.sDocnumber + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    whereQueryWord = "docNumber <= '" + ascmMaterialItem_Model.eDocnumber + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }


                list = AscmMaterialItemService.GetInstance().GetList(null, "", "", "", whereOther, false);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmMaterialItem ascmMaterialItem in list)
                    {
                        if (!string.IsNullOrEmpty(ascmMaterialItem_Model.zMtlCategoryStatus))
                        {
                            ascmMaterialItem.zMtlCategoryStatus = ascmMaterialItem_Model.zMtlCategoryStatus;
                        }
                        if (!string.IsNullOrEmpty(ascmMaterialItem_Model.dMtlCategoryStatus))
                        {
                            ascmMaterialItem.dMtlCategoryStatus = ascmMaterialItem_Model.dMtlCategoryStatus;
                        }
                        if (!string.IsNullOrEmpty(ascmMaterialItem_Model.wMtlCategoryStatus))
                        {
                            ascmMaterialItem.wMtlCategoryStatus = ascmMaterialItem_Model.wMtlCategoryStatus;
                        }
                        ascmMaterialItem.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmMaterialItem.modifyUser = userName;
                    }
                    AscmMaterialItemService.GetInstance().Update(list);
                }
                jsonObjectReuslt.result = true;
                jsonObjectReuslt.message = "批量维护成功！";
            }
            catch (Exception ex)
            {
                jsonObjectReuslt.result = false;
                jsonObjectReuslt.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectReuslt);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult ChoiceMaterialStockFormatSave(string releaseHeaderIds, string zmtlCategoryStatus, string dmtlCategoryStatus, string wmtlCategoryStatus)
        {
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            JsonObjectResult jsonObjectReuslt = new JsonObjectResult();
            List<AscmMaterialItem> list = null;

            try
            {
                string whereOther = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(releaseHeaderIds))
                {
                    whereQueryWord = "id in (" + releaseHeaderIds + ")";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                list = AscmMaterialItemService.GetInstance().GetList(null, "", "", "", whereOther, false);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmMaterialItem ascmMaterialItem in list)
                    {
                        if (!string.IsNullOrEmpty(zmtlCategoryStatus))
                        {
                            ascmMaterialItem.zMtlCategoryStatus = zmtlCategoryStatus;
                        }
                        if (!string.IsNullOrEmpty(dmtlCategoryStatus))
                        {
                            ascmMaterialItem.dMtlCategoryStatus = dmtlCategoryStatus;
                        }
                        if (!string.IsNullOrEmpty(wmtlCategoryStatus))
                        {
                            ascmMaterialItem.wMtlCategoryStatus = wmtlCategoryStatus;
                        }
                        ascmMaterialItem.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmMaterialItem.modifyUser = userName;
                    }
                    AscmMaterialItemService.GetInstance().Update(list);
                }
                jsonObjectReuslt.result = true;
                jsonObjectReuslt.message = "批量维护成功！";
            }
            catch (Exception ex)
            {
                jsonObjectReuslt.result = false;
                jsonObjectReuslt.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectReuslt);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult MaterialStockFormatRepair()
        {
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            List<AscmMaterialItem> list = null;

            try
            {
                string sql = string.Empty;
                object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmMaterialItem where isFlag = 0");
                int nI = 0;
                int times = 0;
                if (int.TryParse(obj.ToString(), out nI) && nI > 0)
                {
                    decimal temp = Convert.ToDecimal(nI) / 10000;
                    times = Convert.ToInt16(Math.Ceiling(temp));
                }
                if (times > 0)
                {
                    for (int i = 0; i < times; i++)
                    {
                        #region 维护逻辑
                        sql = "from AscmMaterialItem where isFlag = 0 and rownum <= 10000 order by id";
                        IList<AscmMaterialItem> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                        if (ilist != null && ilist.Count > 0)
                        {
                            foreach (AscmMaterialItem ascmMaterialItem in ilist)
                            {
                                //添加物料大类
                                string categoryStr = ascmMaterialItem.docNumber.Substring(0, 4);
                                object objcategory = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmMaterialCategory where categoryCode = '" + categoryStr + "'");
                                int categoryNI = 0;
                                if (int.TryParse(objcategory.ToString(), out categoryNI) && categoryNI == 0)
                                {
                                    AscmMaterialCategory ascmMaterialCategory = new AscmMaterialCategory();
                                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmMaterialCategory");
                                    ascmMaterialCategory.id = ++maxId;
                                    ascmMaterialCategory.createUser = userName;
                                    ascmMaterialCategory.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                    ascmMaterialCategory.modifyUser = userName;
                                    ascmMaterialCategory.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                    ascmMaterialCategory.categoryCode = categoryStr;
                                    AscmMaterialCategoryService.GetInstance().Save(ascmMaterialCategory);
                                }

                                //添加物料小类
                                string subcategoryStr = ascmMaterialItem.docNumber.Substring(4, 3);
                                AscmMaterialCategory category = AscmMaterialCategoryService.GetInstance().GetId(categoryStr);
                                string combinationCode = categoryStr + "." + subcategoryStr;
                                object objsubcategory = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmMaterialSubCategory where subCategoryCode = '" + subcategoryStr + "' and categoryId = " + category.id);
                                int subcategoryNI = 0;
                                if (int.TryParse(objsubcategory.ToString(), out subcategoryNI) && subcategoryNI == 0)
                                {
                                    AscmMaterialSubCategory ascmMaterialSubCategory = new AscmMaterialSubCategory();
                                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmMaterialSubCategory");
                                    ascmMaterialSubCategory.id = ++maxId;
                                    ascmMaterialSubCategory.createUser = userName;
                                    ascmMaterialSubCategory.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                    ascmMaterialSubCategory.modifyUser = userName;
                                    ascmMaterialSubCategory.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                    ascmMaterialSubCategory.subCategoryCode = subcategoryStr;
                                    ascmMaterialSubCategory.categoryId = category.id;
                                    ascmMaterialSubCategory.combinationCode = categoryStr + "." + subcategoryStr;
                                    AscmMaterialSubCategoryService.GetInstance().Save(ascmMaterialSubCategory);
                                }
                                else if (int.TryParse(objsubcategory.ToString(), out subcategoryNI) && subcategoryNI > 0)
                                {
                                    sql = "from AscmMaterialSubCategory where combinationCode = '" + combinationCode + "'";
                                    IList<AscmMaterialSubCategory> ilistSubCategory = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialSubCategory>(sql);
                                    if (ilistSubCategory != null && ilistSubCategory.Count > 0)
                                    {
                                        foreach (AscmMaterialSubCategory ascmMaterialSubCategory in ilistSubCategory)
                                        {
                                            if (!string.IsNullOrEmpty(ascmMaterialSubCategory.zMtlCategoryStatus))
                                            {
                                                ascmMaterialItem.zMtlCategoryStatus = ascmMaterialSubCategory.zMtlCategoryStatus;
                                            }
                                            if (!string.IsNullOrEmpty(ascmMaterialSubCategory.dMtlCategoryStatus))
                                            {
                                                ascmMaterialItem.dMtlCategoryStatus = ascmMaterialSubCategory.dMtlCategoryStatus;
                                            }
                                            if (!string.IsNullOrEmpty(ascmMaterialSubCategory.wMtlCategoryStatus))
                                            {
                                                ascmMaterialItem.wMtlCategoryStatus = ascmMaterialSubCategory.wMtlCategoryStatus;
                                            }
                                        }
                                    }
                                }
                                AscmMaterialSubCategory subCategory = AscmMaterialSubCategoryService.GetInstance().GetId(combinationCode);
                                if (subCategory != null)
                                {
                                    ascmMaterialItem.subCategoryId = subCategory.id;
                                    ascmMaterialItem.isFlag = 1;
                                    AscmMaterialItemService.GetInstance().Update(ascmMaterialItem);
                                }
                            }
                        }
                        #endregion
                    }
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "已完成物料大类、小类及物料信息维护！";
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult MaterialImport(HttpPostedFileBase fileImport)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            string sError = "";
            DataTable dt = new DataTable();
            int colsCount = 0;
            colsCount = int.TryParse(AscmCommonHelperService.MaterialStockFormatImportParam.ToString(), out colsCount) ? colsCount : 6;
            StringBuilder sb = new StringBuilder();

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                for (int i = 0; i < colsCount; i++)
                {
                    dt.Columns.Add("第" + i.ToString() + "列");
                }

                if (fileImport != null)
                {
                    List<AscmMaterialItem> listAscmMaterialItem = new List<AscmMaterialItem>();
                    using (Stream stream = fileImport.InputStream)
                    {
                        NPOI.SS.UserModel.IWorkbook wb = NPOI.SS.UserModel.WorkbookFactory.Create(stream);
                        ISheet sheet = wb.GetSheet("Sheet1");
                        IEnumerator rows = sheet.GetRowEnumerator();
                        while (rows.MoveNext())
                        {
                            DataRow dr = dt.NewRow();
                            NPOI.SS.UserModel.IRow row = (NPOI.SS.UserModel.IRow)rows.Current;
                            if (row.RowNum != 0)
                            {
                                List<NPOI.SS.UserModel.ICell> iCellList = new List<NPOI.SS.UserModel.ICell>();

                                for (int i = 0; i < colsCount; i++)
                                {
                                    NPOI.SS.UserModel.ICell iCell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                    iCellList.Add(iCell);
                                }

                                for (int i = 0; i < dt.Columns.Count; i++)
                                {
                                    if (iCellList[i] != null)
                                        dr[i] = iCellList[i].ToString();
                                }
                                dt.Rows.Add(dr);
                            }
                            sb.Append(dr[0].ToString());
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmMaterialItem where docNumber = '" + dr[0].ToString() + "'");
                            if (obj == null)
                                throw new Exception("查询异常！");
                            int iCount = 0;
                            if (int.TryParse(obj.ToString(), out iCount) && iCount > 0)
                            {
                                List<AscmMaterialItem> tempList = AscmMaterialItemService.GetInstance().GetList("from AscmMaterialItem where docNumber = '" + dr[0].ToString() + "'");
                                if (tempList != null && tempList.Count > 0)
                                {
                                    foreach (AscmMaterialItem ascmMaterialItem in tempList)
                                    {
                                        string newError = "";
                                        if (dr[3].ToString() == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.inStock))
                                            ascmMaterialItem.zMtlCategoryStatus = MtlCategoryStatusDefine.inStock;
                                        else if (dr[3].ToString() == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.mixStock))
                                            ascmMaterialItem.zMtlCategoryStatus = MtlCategoryStatusDefine.mixStock;
                                        else if (dr[3].ToString() == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.preStock))
                                            ascmMaterialItem.zMtlCategoryStatus = MtlCategoryStatusDefine.preStock;
                                        else if (string.IsNullOrEmpty(dr[3].ToString()))
                                            ascmMaterialItem.wMtlCategoryStatus = "";
                                        else if (!string.IsNullOrEmpty(dr[3].ToString()))
                                            newError += "物料编码[" + ascmMaterialItem.docNumber + "]总装备料形式书写不正确:" + dr[3].ToString() + "<br />";
                                        if (!string.IsNullOrEmpty(newError))
                                        {
                                            sError += newError;
                                            continue;
                                        }

                                        if (dr[4].ToString() == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.inStock))
                                            ascmMaterialItem.dMtlCategoryStatus = MtlCategoryStatusDefine.inStock;
                                        else if (dr[4].ToString() == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.mixStock))
                                            ascmMaterialItem.dMtlCategoryStatus = MtlCategoryStatusDefine.mixStock;
                                        else if (dr[4].ToString() == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.preStock))
                                            ascmMaterialItem.dMtlCategoryStatus = MtlCategoryStatusDefine.preStock;
                                        else if (string.IsNullOrEmpty(dr[4].ToString()))
                                            ascmMaterialItem.wMtlCategoryStatus = "";
                                        else if (!string.IsNullOrEmpty(dr[4].ToString()))
                                            newError += "物料编码[" + ascmMaterialItem.docNumber + "]电装备料形式书写不正确:" + dr[4].ToString() + "<br />";
                                        if (!string.IsNullOrEmpty(newError))
                                        {
                                            sError += newError;
                                            continue;
                                        }

                                        if (dr[5].ToString() == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.inStock))
                                            ascmMaterialItem.wMtlCategoryStatus = MtlCategoryStatusDefine.inStock;
                                        else if (dr[5].ToString() == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.mixStock))
                                            ascmMaterialItem.wMtlCategoryStatus = MtlCategoryStatusDefine.mixStock;
                                        else if (dr[5].ToString() == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.preStock))
                                            ascmMaterialItem.wMtlCategoryStatus = MtlCategoryStatusDefine.preStock;
                                        else if (string.IsNullOrEmpty(dr[5].ToString()))
                                            ascmMaterialItem.wMtlCategoryStatus = "";
                                        else if (!string.IsNullOrEmpty(dr[5].ToString()))
                                            newError += "物料编码[" + ascmMaterialItem.docNumber + "]其他备料形式书写不正确:" + dr[5].ToString() + "<br />";
                                        if (!string.IsNullOrEmpty(newError))
                                        {
                                            sError += newError;
                                            continue;
                                        }

                                        ascmMaterialItem.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                        ascmMaterialItem.modifyUser = userName;

                                        listAscmMaterialItem.Add(ascmMaterialItem);
                                    }
                                }
                            }
                            else
                            {
                                sError += "未找到对应的物料编码:" + dr[0].ToString() + "<br />";
                            }
                        }

                        using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                        {
                            try
                            {
                                if (listAscmMaterialItem != null && listAscmMaterialItem.Count > 0)
                                {
                                    YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmMaterialItem);
                                }
                                tx.Commit();
                                sError += "【成功更新" + listAscmMaterialItem.Count.ToString() + "条记录!】";
                                jsonObjectResult.message = sError;
                                jsonObjectResult.result = true;
                            }
                            catch (Exception ex)
                            {
                                tx.Rollback();
                                sError += ex.Message;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message += ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult MaterialExport(string queryWord, string queryType, string queryStarDocnumber, string queryEndDocnumber, string zmtlCategoryStatus, string dmtlCategoryStatus, string wmtlCategoryStatus, string queryDescribe)
        {
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            IWorkbook wb = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Sheet1");
            sheet.SetColumnWidth(0, 20 * 256);
            sheet.SetColumnWidth(1, 13 * 256);
            sheet.SetColumnWidth(2, 30 * 256);
            sheet.SetColumnWidth(3, 15 * 256);
            sheet.SetColumnWidth(4, 15 * 256);
            sheet.SetColumnWidth(5, 15 * 256);

            int iRow = 0;
            IRow titleRow = sheet.CreateRow(iRow);
            titleRow.Height = 20 * 20;
            titleRow.CreateCell(0).SetCellValue("物料编码");
            titleRow.CreateCell(1).SetCellValue("供应类型");
            titleRow.CreateCell(2).SetCellValue("物料描述");
            titleRow.CreateCell(3).SetCellValue("总装备料形式");
            titleRow.CreateCell(4).SetCellValue("电装备料形式");
            titleRow.CreateCell(5).SetCellValue("其他备料形式");

            try
            {
                string whereOther = "", whereQueryWord = "";

                whereQueryWord = "wipSupplyType < 4";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                if (!string.IsNullOrEmpty(queryType))
                {
                    whereQueryWord = "wipSupplyType = " + queryType;
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryStarDocnumber) && !string.IsNullOrEmpty(queryEndDocnumber))
                {
                    if (queryStarDocnumber == queryEndDocnumber)
                    {
                        whereQueryWord = "docNumber like '" + queryStarDocnumber + "%'";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    }
                    else
                    {
                        whereQueryWord = "docNumber >= '" + queryStarDocnumber + "'";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                        whereQueryWord = "docNumber <= '" + queryEndDocnumber + "'";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    }
                }
                else if (!string.IsNullOrEmpty(queryStarDocnumber) && string.IsNullOrEmpty(queryEndDocnumber))
                {
                    whereQueryWord = "docNumber like '" + queryStarDocnumber + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(queryStarDocnumber) && !string.IsNullOrEmpty(queryEndDocnumber))
                {
                    whereQueryWord = "docNumber like '" + queryEndDocnumber + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                else
                {
                    whereQueryWord = "docNumber like '20%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(zmtlCategoryStatus))
                {
                    if (zmtlCategoryStatus != "qb")
                    {
                        if (zmtlCategoryStatus != "kz")
                            whereQueryWord = "zMtlCategoryStatus = '" + zmtlCategoryStatus + "'";
                        else
                            whereQueryWord = "zMtlCategoryStatus is null";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    }
                }

                if (!string.IsNullOrEmpty(dmtlCategoryStatus))
                {
                    if (dmtlCategoryStatus != "qb")
                    {
                        if (dmtlCategoryStatus != "kz")
                            whereQueryWord = "dMtlCategoryStatus = '" + dmtlCategoryStatus + "'";
                        else
                            whereQueryWord = "dMtlCategoryStatus is null";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    }
                }

                if (!string.IsNullOrEmpty(wmtlCategoryStatus))
                {
                    if (wmtlCategoryStatus != "qb")
                    {
                        if (wmtlCategoryStatus != "kz")
                            whereQueryWord = "wMtlCategoryStatus = '" + wmtlCategoryStatus + "'";
                        else
                            whereQueryWord = "wMtlCategoryStatus is null";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                    }
                }

                if (string.IsNullOrEmpty(zmtlCategoryStatus) && string.IsNullOrEmpty(dmtlCategoryStatus) && string.IsNullOrEmpty(wmtlCategoryStatus))
                {
                    whereQueryWord = "isFlag = 0";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryDescribe))
                {
                    whereQueryWord = "description like '%" + queryDescribe + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                List<AscmMaterialItem> list = AscmMaterialItemService.GetInstance().GetList(null, "", "", "", whereOther, false);

                if (list != null && list.Count > 0)
                {
                    foreach (AscmMaterialItem ascmMateiralItem in list)
                    {
                        iRow++;
                        IRow row = sheet.CreateRow(iRow);
                        row.Height = 20 * 20;
                        row.CreateCell(0).SetCellValue(ascmMateiralItem.docNumber);
                        row.CreateCell(1).SetCellValue(ascmMateiralItem.wipSupplyTypeCn);
                        row.CreateCell(2).SetCellValue(ascmMateiralItem.description);
                        row.CreateCell(3).SetCellValue(ascmMateiralItem._zMtlCategoryStatus);
                        row.CreateCell(4).SetCellValue(ascmMateiralItem._dMtlCategoryStatus);
                        row.CreateCell(5).SetCellValue(ascmMateiralItem._wMtlCategoryStatus);
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("导出失败(Export AscmMaterialItem)", ex);
                throw ex;
            }

            byte[] buffer = new byte[] { };
            using (System.IO.MemoryStream stream = new MemoryStream())
            {
                wb.Write(stream);
                buffer = stream.GetBuffer();
            }

            return File(buffer, "application/vnd.ms-excel", "备料形式维护.xls");
        }
        #endregion

        #region 仓库物料统计查询
        public ActionResult GetSumMaterialFromErpIndex()
        {
            return View();
        }

        public ActionResult GetSumMaterialFromErpList(int? page, int? rows, string sort, string order, string queryWord, int materialId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmMtlOnhandQuantitiesDetail> list = null;

            try
            {
                list = AscmMtlOnhandQuantitiesDetailService.GetInstance().GetSumList(ynPage, materialId);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmMtlOnhandQuantitiesDetail ascmMtlOnhandQuantitiesDetail in list)
                    {
                        jsonDataGridResult.rows.Add(ascmMtlOnhandQuantitiesDetail);
                    }
                    jsonDataGridResult.total = ynPage.GetRecordCount();
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.message = "";
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMtlRelatedWipDiscreteJobs(int? page, int? rows, string sort, string order, string queryWord, string queryStartTime, string queryEndTime, string queryDocnumber)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmDiscreteJobs> list = null;

            try
            {
                if (string.IsNullOrEmpty(queryDocnumber))
                    throw new Exception("请选择需查询库存的物料！");
                if (string.IsNullOrEmpty(queryStartTime) && string.IsNullOrEmpty(queryEndTime))
                    throw new Exception("请选择作业日期筛选关联作业号！");

                if (!string.IsNullOrEmpty(queryStartTime) && string.IsNullOrEmpty(queryEndTime))
                    queryEndTime = queryStartTime;
                else if (string.IsNullOrEmpty(queryStartTime) && !string.IsNullOrEmpty(queryEndTime))
                    queryStartTime = queryEndTime;
                
                object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select id from AscmMaterialItem where docNumber = '" + queryDocnumber + "' and rownum = 1");
                if (obj == null)
                    throw new Exception("未查询到指定的物料！");
                int materialId = 0;
                if (int.TryParse(obj.ToString(), out materialId) && materialId == 0)
                    throw new Exception("转换数据失败！");

                list = AscmDiscreteJobsService.GetInstance().GetRelatedDiscreteJobs(ynPage, "", "", queryWord, queryStartTime, queryEndTime, materialId);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                    {
                        jsonDataGridResult.rows.Add(ascmDiscreteJobs);
                    }
                    jsonDataGridResult.total = ynPage.GetRecordCount();
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.message = "";
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 员工任务统计查询
        public ActionResult GetWorkerTaskCountIndex()
        {
            return View();
        }

        public ActionResult GetWorkerTaskCountList(string queryStartTime, string queryEndTime, string queryLogisticsClass)
        {
            List<AscmGetMaterialTask> list = new List<AscmGetMaterialTask>(); ;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();

            string userName = string.Empty;
            string userRole = string.Empty;
            string userLogistisClass = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }
            userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
            userLogistisClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName, userRole);

            try
            {
                string sql = "select workerId, count(workerId),round(avg(round(to_number(to_date(endTime,'yyyy-mm-dd hh24:mi:ss')- to_date(starTime,'yyyy-mm-dd hh24:mi:ss'))*1440))) from Ascm_Getmaterial_Task";
                string where = "", whereQueryWord = "", groupString = "", orderString = "";
                groupString = " group by workerId";
                orderString = " order by workerId";
                whereQueryWord = "workerId is not null";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                whereQueryWord = "status = 'FINISH'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(userLogistisClass))
                {
                    whereQueryWord = "logisticsClass in (" + userLogistisClass + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (string.IsNullOrEmpty(queryStartTime) && string.IsNullOrEmpty(queryEndTime))
                    throw new Exception("查询统计:开始日期或结束日期至少一个不为空！");

                if (!string.IsNullOrEmpty(queryStartTime))
                {
                    string sTime = queryStartTime + " 00:00";
                    whereQueryWord = "starTime >= '" + sTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryEndTime))
                {
                    string eTime = queryEndTime + " 23:59";
                    whereQueryWord = "endTime <= '" + eTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryLogisticsClass))
                {
                    whereQueryWord = "logisticsClass = '" + userLogistisClass + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where + groupString + orderString;
                IList ilist = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);
                if (ilist != null && ilist.Count > 0)
                {
                    foreach (object[] obj in ilist)
                    {
                        AscmGetMaterialTask ascmGetMaterialTask = new AscmGetMaterialTask();
                        ascmGetMaterialTask.workerId = obj[0].ToString();
                        ascmGetMaterialTask.taskCount = obj[1].ToString();
                        ascmGetMaterialTask.avgTime = obj[2].ToString();
                        list.Add(ascmGetMaterialTask);
                    }
                }

                if (list != null && list.Count > 0)
                {
                    AscmGetMaterialTaskService.GetInstance().SetWorker(list);

                    foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                    {
                        jsonDataGridResult.rows.Add(ascmGetMaterialTask);
                    }
                }
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadWorkerTaskDetailList(int? page, int? rows, string sort, string order, string queryWord, string workerId, string queryStartTime, string queryEndTime)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmGetMaterialTask> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereQueryWord = "";

                whereQueryWord = "status = 'FINISH'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                if (!string.IsNullOrEmpty(workerId))
                {
                    whereQueryWord = "workerId = '" + workerId + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryStartTime))
                {
                    string sTime = queryStartTime + " 00:00";
                    whereQueryWord = "starTime >= '" + sTime + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryEndTime))
                {
                    string eTime = queryEndTime + " 23:59";
                    whereQueryWord = "endTime <= '" + eTime + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }


                list = AscmGetMaterialTaskService.GetInstance().GetList(ynPage, "createTime", "", "", whereOther);
                if (list != null && list.Count > 0)
                {
                    AscmGetMaterialTaskService.GetInstance().SetUsedTime(list);
                    AscmGetMaterialTaskService.GetInstance().SetRanker(list);
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                    {
                        jsonDataGridResult.rows.Add(ascmGetMaterialTask);
                    }
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error(ex.Message, ex);
                throw ex;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 汇总物料编码查询
        public ActionResult SumMaterialQuantityIndex()
        {
            return View();
        }

        public ActionResult SumGetMaterialQuantityList(int? page, int? rows, string sort, string order, string queryWord, string queryStartDate, string queryEndDate, string queryDocnumber)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            string userName = string.Empty;
            string userRole = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }
            userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);

            List<AscmMaterialItem> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string where = "", whereOther = "", whereQueryWord = "", whereMaterial = "", whereTask = "";

                if (string.IsNullOrEmpty(queryStartDate) && string.IsNullOrEmpty(queryEndDate))
                    throw new Exception("起止日期不能为空！");

                string startDate = string.Empty;
                if (!string.IsNullOrEmpty(queryStartDate))
                {
                    startDate = Convert.ToDateTime(queryStartDate).ToString("yyyy-MM-dd") + " 00:00:00";
                    whereQueryWord = "createTime >= '" + startDate + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                string endDate = string.Empty;
                if (!string.IsNullOrEmpty(queryEndDate))
                {
                    endDate = Convert.ToDateTime(queryEndDate).ToString("yyyy-MM-dd") + " 23:59:59";
                    whereQueryWord = "createTime <= '" + endDate + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                
                if (userRole.IndexOf("领料员") > -1)
                {
                    whereQueryWord = "workerId = '" + userName + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryDocnumber))
                {
                    whereQueryWord = " docNumber like '" + queryDocnumber + "%'";
                    whereMaterial = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereMaterial, whereQueryWord);
                
                    List<AscmMaterialItem> listAscmMaterialItem = AscmMaterialItemService.GetInstance().GetList(null, "", "", "", whereMaterial);
                    if (listAscmMaterialItem == null || listAscmMaterialItem.Count == 0)
                        throw new Exception("物料编码不存在！");

                    if (listAscmMaterialItem != null && listAscmMaterialItem.Count > 0)
                    {
                        string idsMaterial = string.Empty;
                        foreach (AscmMaterialItem item in listAscmMaterialItem)
                        {
                            if (!string.IsNullOrEmpty(idsMaterial))
                                idsMaterial += ",";
                            idsMaterial += item.id;
                        }

                        if (!string.IsNullOrEmpty(idsMaterial))
                        {
                            whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(idsMaterial, "inventoryitemid");
                            whereTask = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereTask, whereQueryWord);
                        }
                    }
                }

                List<AscmGetMaterialTask> listAscmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().GetList(null, "", "", "", whereOther);
                if (listAscmGetMaterialTask == null || listAscmGetMaterialTask.Count == 0)
                    throw new Exception("找不到指定日期的领料任务！");
                
                if (listAscmGetMaterialTask != null && listAscmGetMaterialTask.Count > 0)
                {
                    string ids = string.Empty;
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in listAscmGetMaterialTask)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += ascmGetMaterialTask.id;
                    }

                    if (!string.IsNullOrEmpty(ids))
                    {
                        whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "taskId");
                        whereTask = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereTask, whereQueryWord);
                    }
                }
                
                string sql = "select inventoryitemid, sum(requiredQuantity), sum(quantityIssued), sum(getMaterialQuantity), sum(requiredQuantity - quantityIssued) as quantityDifference, sum(requiredQuantity - getMaterialQuantity) as quantityGetMaterialDifference from ascm_wip_require_operat";
                    
                if (!string.IsNullOrEmpty(whereTask))
                    sql += " where " + whereTask + " group by inventoryitemid";
                IList ilistOperations = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);
                if (ilistOperations == null || ilistOperations.Count == 0)
                    throw new Exception("指定条件查询结果中未找到汇总编码");

                if (ilistOperations != null && ilistOperations.Count > 0)
                {
                    string idsOperations = string.Empty;
                    foreach (object[] obj in ilistOperations)
                    {
                        if (!string.IsNullOrEmpty(idsOperations))
                            idsOperations += ",";
                        idsOperations += obj[0].ToString();
                    }
                    whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(idsOperations,"id");
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                list = AscmMaterialItemService.GetInstance().GetList(ynPage,"","","",where);

                if (list != null && list.Count > 0)
                {
                    foreach (AscmMaterialItem ascmMaterialItem in list)
                    {
                        foreach (object[] obj in ilistOperations)
                        {
                            int id = Convert.ToInt32(obj[0].ToString());
                            if (ascmMaterialItem.id == id)
                            {
                                ascmMaterialItem.requiredQuantity = Convert.ToDecimal(obj[1].ToString());
                                ascmMaterialItem.quantityIssued = Convert.ToDecimal(obj[2].ToString());
                                ascmMaterialItem.getMaterialQuantity = Convert.ToDecimal(obj[3].ToString());
                                ascmMaterialItem.quantityDifference = Convert.ToDecimal(obj[4].ToString());
                                ascmMaterialItem.quantityGetMaterialDifference = Convert.ToDecimal(obj[5].ToString());
                            }
                        }
                        jsonDataGridResult.rows.Add(ascmMaterialItem);
                    }
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                jsonDataGridResult.message = ex.Message;
                jsonDataGridResult.result = false;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 异常作业统计查询
        public ActionResult NotFinalDiscreteJobsIndex()
        {
            //未完成作业统计
            return View();
        }

        public ActionResult NotFinalDiscreteJobsList(int? page, int? rows, string sort, string order, string queryWord, string queryStartDate, string queryEndDate, string queryDocnumber, string queryStatus)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDiscreteJobs> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            
            try
            {
                string hql = "select new AscmWipRequirementOperations(b.wipEntityId) from AscmGetMaterialTask a, AscmWipRequirementOperations b, AscmWipEntities c";

                if (string.IsNullOrEmpty(queryStartDate) && string.IsNullOrEmpty(queryEndDate))
                    throw new Exception("起止作业日期不能为空！");

                string where = "", whereQueryWord = "", whereWipEntity = "";

                whereQueryWord = "a.id = b.taskId";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                whereQueryWord = "b.wipEntityId = c.wipEntityId";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(queryStatus))
                {
                    whereQueryWord = "a.status = '" + queryStatus + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else
	            {
                    whereQueryWord = "a.status != 'FINISH'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
	            }

                if (!string.IsNullOrEmpty(queryStartDate))
                {
                    string startDate = Convert.ToDateTime(queryStartDate).ToString("yyyy-MM-dd") + " 00:00";
                    whereQueryWord = "a.dateReleased >= '" + startDate + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryEndDate))
                {
                    string endDate = Convert.ToDateTime(queryEndDate).ToString("yyyy-MM-dd") + " 00:00";
                    whereQueryWord = "a.dateReleased <= '" + endDate + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryDocnumber))
                {
                    whereQueryWord = "c.name like '" + queryDocnumber + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(where))
                    hql += " where " + where;

                IList<AscmWipRequirementOperations> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(hql);
                if (ilist != null && ilist.Count > 0)
                {
                    var wipEntityId = ilist.Select(P => P.wipEntityId).Distinct();
                    if (wipEntityId != null)
                    {
                        string wipEntityIds = string.Empty;
                        foreach (int item in wipEntityId)
                        {
                            if (!string.IsNullOrEmpty(wipEntityIds))
                                wipEntityIds += ",";
                            wipEntityIds += item;
                        }

                        whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(wipEntityIds, "wipEntityId");
                        whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryDocnumber))
                    {
                        whereQueryWord = "jobId like '" + queryDocnumber + "%'";
                        whereWipEntity = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereWipEntity, whereQueryWord);
                    }
                }

                if (!string.IsNullOrEmpty(whereWipEntity))
                {
                    list = AscmDiscreteJobsService.GetInstance().GetList(ynPage, "", "", "", whereWipEntity);
                    if (list != null && list.Count > 0)
                    {
                        foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                        {
                            jsonDataGridResult.rows.Add(ascmDiscreteJobs);
                        }
                    }
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadDiscreteJobs(int id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                AscmWipDiscreteJobs ascmDiscreateJobs = AscmWipDiscreteJobsService.GetInstance().Get(id.ToString());
                jsonObjectResult.result = true;
                jsonObjectResult.id = id.ToString();
                jsonObjectResult.entity = ascmDiscreateJobs;
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWipDiscreteJobs)", ex);
                throw ex;
            }

            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadBom(int? page, int? rows, string sort, string order, int? id, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmWipRequirementOperations> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (string.IsNullOrEmpty(id.ToString()))
                    throw new Exception("作业ID不能为空！");
                string whereOther = "", whereQueryWord = "";

                whereQueryWord = " wipentityid = " + id.ToString();
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                whereQueryWord = " wipSupplyType = 1 ";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                whereQueryWord = " (requiredQuantity - getMaterialQuantity) > 0";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                whereQueryWord = "taskId > 0";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                list = AscmWipRequirementOperationsService.GetInstance().GetList(ynPage, "", "", "", whereOther, "");
                if (list != null && list.Count > 0)
                {
                    AscmWipRequirementOperationsService.GetInstance().SetMaterial(list);
                    list = list.OrderBy(e => e.ascmMaterialItem_DocNumber).ToList<AscmWipRequirementOperations>();
                    foreach (AscmWipRequirementOperations ascmWipRequirementOperations in list)
                    {
                        jsonDataGridResult.rows.Add(ascmWipRequirementOperations);
                    }
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error(ex.Message, ex);
                throw ex;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 自定义控件方法
        // 物流组自定义控件
        public ActionResult LogisticsClassAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            List<AscmLogisticsClassInfo> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(q))
                {
                    whereQueryWord = "logisticsName like '%" + q.Trim() + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }
                list = AscmLogisticsClassInfoService.GetInstance().GetList(ynPage, "", "", "", whereOther);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmLogisticsClassInfo ascmLogisticsClassInfo in list)
                    {
                        jsonDataGridResult.rows.Add(ascmLogisticsClassInfo);
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

        // 作业自定义控件
        public ActionResult WipEntitiesAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            List<AscmDiscreteJobs> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmDiscreteJobsService.GetInstance().GetList(ynPage, sort, order, q, null);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                    {
                        jsonDataGridResult.rows.Add(ascmDiscreteJobs);
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

        // 仓库自定义控件
        public ActionResult WarehouseIdAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber

            List<AscmWarehouse> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmWarehouseService.GetInstance().GetList(ynPage, "", "", q, null);
                if (list != null && list.Count > 0)
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

        // 物料大类自定义控件
        public ActionResult MaterialCategoryAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber

            List<AscmMaterialCategory> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmMaterialCategoryService.GetInstance().GetList(ynPage, "categoryCode", "", q, "");
                if (list != null && list.Count > 0)
                {
                    foreach (AscmMaterialCategory ascmMaterialCategory in list)
                    {
                        jsonDataGridResult.rows.Add(ascmMaterialCategory);
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

        // 某角色的所属用户自定义控件
        public ActionResult UserAscxList(int? id, int? page, int? rows, string sort, string order, string q, string queryRole)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber

            List<AscmUserInfo> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryRole))
                {
                    whereQueryWord = "p.name = '" + queryRole + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                list = AscmUserInfoService.GetInstance().GetList(null, "", "", q, whereOther);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmUserInfo ascmUserInfo in list)
                    {
                        jsonDataGridResult.rows.Add(ascmUserInfo);
                    }
                    jsonDataGridResult.total = list.Count;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }

        // 标记自定义控件
        public ActionResult MarkIdAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber

            List<AscmMarkTaskLog> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);

                string whereOther = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(userRole) && userRole == "领料员")
                {
                    whereQueryWord = "createUser like '" + userName + "%'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                whereQueryWord = "markType = 'NONAUTO'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                list = AscmMarkTaskLogService.GetInstance().GetList(ynPage, "", "", q, whereOther, true, true);
                if (list != null && list.Count > 0)
                {
                    foreach (AscmMarkTaskLog ascmMarkTaskLog in list)
                    {
                        jsonDataGridResult.rows.Add(ascmMarkTaskLog);
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

        //物料编码自定义控件
        public ActionResult MaterialDocnumberAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmMaterialItem> list = null;
            try
            {
                list = AscmMaterialItemService.GetInstance().GetList(ynPage, "", "", q, "");
                if (list!= null && list.Count > 0)
                {
                    foreach (AscmMaterialItem ascmMaterialItem in list)
                    {
                        jsonDataGridResult.rows.Add(ascmMaterialItem);
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

        //领料车辆Rfid自定义控件
        public ActionResult ForkliftRfidTagAscxList(int? id, int? page, int? rows, string sort, string order, string q, string queryType)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber

            List<AscmRfid> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryType))
                {
                    whereQueryWord = "bindType = '" + AscmRfid.BindTypeDefine.forklift + "'";
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);
                }

                list = AscmRfidService.GetInstance().GetList(ynPage, "", "", q, whereOther);
                if (list != null  && list.Count > 0)
                {
                    foreach (AscmRfid ascmRfid in list)
                    {
                        jsonDataGridResult.rows.Add(ascmRfid);
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
    }
}
