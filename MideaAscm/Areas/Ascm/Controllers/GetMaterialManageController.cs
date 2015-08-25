using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YnBaseClass2.Web;
using Newtonsoft.Json;
using NHibernate;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Services.GetMaterialManage;
using MideaAscm.Services.FromErp;
using MideaAscm.Dal;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Services.Base;
using System.Data;
using System.IO;
using YnFrame.Dal.Entities;
using MideaAscm.Dal.FromErp.Entities;
using YnFrame.Services;
using System.Collections;
using Net.SourceForge.Koogra;
using System.Text;
using MideaAscm.Dal.IEntity;
using MideaAscm.Services.IEntity;
using NPOI.SS.UserModel;

namespace MideaAscm.Areas.Ascm.Controllers
{
    public class GetMaterialManageController : YnFrame.Controllers.YnBaseController
    {
        //领料作业计划管理模块
        //
        // GET: /Ascm/GetMaterialManage/

        public ActionResult Index()
        {
            return View();
        }

        #region 信息关联管理{领料员关联领料车辆}
        public ActionResult WorkerRelatedForkliftIndex()
        {
            //领料员与领料车辆管理
            return View();
        }
        //领料员管理
        public ActionResult YnUserList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            List<YnUser> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string sql = "from YnPosition where roleId = 7";
                IList<YnPosition> ilist = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.Find<YnPosition>(sql);
                string ids = string.Empty;
                if (ilist != null && ilist.Count > 0)
                {
                    foreach (YnPosition ynPosition in ilist)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += ynPosition.id;
                    }
                }
                sql = "from YnDepartmentPositionUserLink";
                string whereQueryWord = "", where = "";
                where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "positionid");
                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    IList<YnDepartmentPositionUserLink> ilistUser = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.Find<YnDepartmentPositionUserLink>(sql);
                    ids = string.Empty;
                    if (ilistUser != null && ilistUser.Count > 0)
                    {
                        foreach (YnDepartmentPositionUserLink ynDepartmentPositionUserLink in ilistUser)
                        {
                            if (!string.IsNullOrEmpty(ids))
                                ids += ",";
                            ids += "'" + ynDepartmentPositionUserLink.ids.userId + "'";
                        }
                    }

                    if (!string.IsNullOrEmpty(ids))
                    {
                        sql = "from YnUser";
                        where = "";
                        if (!string.IsNullOrEmpty(queryWord))
                        {
                            whereQueryWord = "userName like '%" + queryWord.Trim() + "%'";
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        }
                        if (!string.IsNullOrEmpty(ids))
                        {
                            whereQueryWord = "userId in (" + ids + ")";
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        }
                        if (!string.IsNullOrEmpty(where))
                        {
                            sql += " where " + where;

                            IList<YnUser> ilistYnUser = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.Find<YnUser>(sql, sql, ynPage);
                            if (ilistYnUser != null && ilistYnUser.Count > 0)
                            {
                                list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnUser>(ilistYnUser);
                                foreach (YnUser ynUser in list)
                                {
                                    jsonDataGridResult.rows.Add(ynUser);
                                }
                            }
                        }
                    }

                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult YnUserEdit(string id)
        {
            YnUser ynUser = null;
            try
            {
                if (!string.IsNullOrEmpty(id))
                    ynUser = YnUserService.GetInstance().Get(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ynUser, JsonRequestBehavior.AllowGet);
        }

        //领料车辆管理
        public ActionResult ForkliftList(int? page, int? rows, string sort, string order, string queryWord, string queryOtherWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            List<AscmForklift> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmForkliftService.GetInstance().GetList(ynPage, "", "", queryWord, queryOtherWord);
                //AscmForkliftService.GetInstance().SetLogisticsClassName(list);
                if (list != null)
                {
                    foreach (AscmForklift ascmForklift in list)
                    {
                        jsonDataGridResult.rows.Add(ascmForklift);
                    }
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult,JsonRequestBehavior.AllowGet);
        }
        public ActionResult ForkliftEdit(int? id)
        {
            AscmForklift ascmForklift = null;
            try
            {
                if (id.HasValue)
                {
                    ascmForklift = AscmForkliftService.GetInstance().Get(id.Value);
                    //if (ascmForklift != null)
                    //{
                    //    YnUser ynUser = YnUserService.GetInstance().Get(ascmForklift.workerId);
                    //    ascmForklift.ascmWorker = ynUser;
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmForklift, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult ForkliftSave(AscmForklift ascmForklift_Model, int? id)
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
                ascmForklift.logisticsClass = ascmForklift_Model.logisticsClass;
                ascmForklift.status = ascmForklift_Model.status;
                ascmForklift.assetsId = ascmForklift_Model.assetsId;

                if (!string.IsNullOrEmpty(ascmForklift_Model.tagId))
                    ascmForklift.tagId = ascmForklift_Model.tagId;
                if (!string.IsNullOrEmpty(ascmForklift_Model.forkliftNumber))
                    ascmForklift.forkliftNumber = ascmForklift_Model.forkliftNumber;
                if (!string.IsNullOrEmpty(ascmForklift_Model.forkliftWay))
                    ascmForklift.forkliftWay = ascmForklift_Model.forkliftWay;
                if (!string.IsNullOrEmpty(ascmForklift_Model.forkliftWay))
                    ascmForklift.actionLimits = ascmForklift_Model.actionLimits;
                if (!string.IsNullOrEmpty(ascmForklift_Model.workContent))
                    ascmForklift.workContent = ascmForklift_Model.workContent;
                if (!string.IsNullOrEmpty(ascmForklift_Model.tip))
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
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult ForkliftDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    AscmForklift ascmForklift = AscmForkliftService.GetInstance().Get(id.Value);
                    if (ascmForklift == null)
                        throw new Exception("找不到领料车辆");
                    AscmForkliftService.GetInstance().Delete(ascmForklift);
                    jsonObjectResult.result = true;
                    jsonObjectResult.message = "";
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
        public ActionResult ForkliftAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            List<AscmForklift> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmForkliftService.GetInstance().GetList(ynPage, "", "", q, null);
                if (list != null)
                {
                    foreach (AscmForklift ascmForklift in list)
                    {
                        jsonDataGridResult.rows.Add(ascmForklift);
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

        #region 物料类别管理
        public ActionResult MaterialCategoryIndex()
        {
            //物料大类、物料子类、物料信息管理
            return View();
        }
        //物料大类管理
        public ActionResult MaterialCategoryList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            List<AscmMaterialCategory> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmMaterialCategoryService.GetInstance().GetList(ynPage,"","",queryWord,null);
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
            return Json(jsonDataGridResult,JsonRequestBehavior.AllowGet);
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

                if (!string.IsNullOrEmpty(ascmMaterialCategory.description))
                    ascmMaterialCategory.description = ascmMaterialCategory_Model.description;
                if (!string.IsNullOrEmpty(ascmMaterialCategory_Model.tip))
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
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }

        //物料子类管理
        public ActionResult MaterialSubCategoryList(int? page, int? rows, string sort, string order, string queryWord, string queryOtherWord, string categoryCode)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber

            List<AscmMaterialSubCategory> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string sql = "from AscmMaterialSubCategory";
                string where = "", whereQueryWord = "";
                if (string.IsNullOrEmpty(categoryCode))
                    throw new Exception("物料大类不能为空！");

                if (!string.IsNullOrEmpty(categoryCode))
                {
                    AscmMaterialCategory ascmMaterialCategory = AscmMaterialCategoryService.GetInstance().GetId(categoryCode);
                    whereQueryWord = "categoryId = " + ascmMaterialCategory.id.ToString();
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(queryWord) && !string.IsNullOrEmpty(queryOtherWord))
                {
                    if (queryWord == queryOtherWord)
                    {
                        whereQueryWord = "subCategoryCode like '" + queryWord + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                    else
                    {
                        whereQueryWord = "subCategoryCode >= '" + queryWord + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        whereQueryWord = "subCategoryCode <= '" + queryOtherWord + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                }
                else if (!string.IsNullOrEmpty(queryWord) && string.IsNullOrEmpty(queryOtherWord))
                {
                    whereQueryWord = "subCategoryCode like '" + queryWord + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where,whereQueryWord);
                }
                else if (string.IsNullOrEmpty(queryWord) && !string.IsNullOrEmpty(queryOtherWord))
                {
                    whereQueryWord = "subCategoryCode like '" + queryOtherWord + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where + " order by combinationCode";
                IList<AscmMaterialSubCategory> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialSubCategory>(sql, sql, ynPage);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialSubCategory>(ilist);
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
                throw ex;
            }
            return Json(jsonDataGridResult,JsonRequestBehavior.AllowGet);
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
                string sql = "from AscmMaterialSubCategory";
                string where = "", whereQueryWord = "";
                if (categoryId != null && categoryId != 0)
                    whereQueryWord = "categoryId = " + categoryId;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where + " order by combinationCode";
                IList<AscmMaterialSubCategory> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialSubCategory>(sql, sql, ynPage);
                if (ilist.Count > 0 && ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialSubCategory>(ilist);
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

                if (!string.IsNullOrEmpty(ascmMaterialSubCategory_Model.description))
                    ascmMaterialSubCategory.description = ascmMaterialSubCategory_Model.description;
                if (!string.IsNullOrEmpty(ascmMaterialSubCategory_Model.tip))
                    ascmMaterialSubCategory.tip = ascmMaterialSubCategory_Model.tip;

                if (id.HasValue)
                {
                    AscmMaterialSubCategoryService.GetInstance().Update(ascmMaterialSubCategory);
                }
                else
                {
                    AscmMaterialSubCategoryService.GetInstance().Save(ascmMaterialSubCategory);
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmMaterialSubCategory.id.ToString();
                jsonObjectResult.entity = ascmMaterialSubCategory;
            }
            catch (Exception ex)
            {
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

                string sql = "from AscmMaterialSubCategory";
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(releaseHeaderIds, "id");

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmMaterialSubCategory> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialSubCategory>(sql);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialSubCategory>(ilist);
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
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }

        //物料信息管理
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
                string sql = "from AscmMaterialItem";
                string where = "", whereQueryWord = "";

                if (string.IsNullOrEmpty(comCode))
                    throw new Exception("组合码不能为空！");
                whereQueryWord = "wipSupplyType in (1,2,3)";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(comCode))
                {
                    AscmMaterialSubCategory ascmMaterialSubCategory = AscmMaterialSubCategoryService.GetInstance().GetId(comCode);
                    whereQueryWord = "subCategoryId =" + ascmMaterialSubCategory.id;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    comCode = comCode.Replace(".", "");
                }

                if (!string.IsNullOrEmpty(queryWord) && !string.IsNullOrEmpty(queryOtherWord))
                {
                    queryWord = comCode + queryWord;
                    queryOtherWord = comCode + queryOtherWord;

                    whereQueryWord = "docNumber >= '" + queryWord + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    whereQueryWord = "docNumber <= '" + queryOtherWord + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else if (!string.IsNullOrEmpty(queryWord) && string.IsNullOrEmpty(queryOtherWord))
                {
                    queryWord = comCode + queryWord;
                    whereQueryWord = "docNumber like '" + queryWord + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(queryWord) && !string.IsNullOrEmpty(queryOtherWord))
                {
                    queryOtherWord = comCode + queryOtherWord;
                    whereQueryWord = "docNumber like '" + queryOtherWord + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where + " order by docNumber";
                IList<AscmMaterialItem> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql, sql, ynPage);
                if (ilist.Count > 0 && ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilist);
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
                throw ex;
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
                string sql = "from AscmMaterialItem";
                string where = "", whereQueryWord = "";
                if (subCategoryId != null && subCategoryId != 0)
                {
                    whereQueryWord = "subCategoryId = " + subCategoryId;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    whereQueryWord = "wipSupplyType in (1,2,3)";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where,whereQueryWord);
                    if (!string.IsNullOrEmpty(where))
                        sql += " where " + where + " order by docNumber";
                    IList<AscmMaterialItem> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql, sql, ynPage);
                    if (ilist != null && ilist.Count > 0)
                    {
                        list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilist);
                        foreach (AscmMaterialItem ascmMaterialItem in list)
                        {
                            jsonDataGridResult.rows.Add(ascmMaterialItem);
                        }
                    }
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
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
                {
                    AscmMaterialItemService.GetInstance().Update(ascmMaterialItem);
                }
                else
                {
                    AscmMaterialItemService.GetInstance().Save(ascmMaterialItem);
                }
                jsonObjectReuslt.result = true;
                jsonObjectReuslt.message = "";
                jsonObjectReuslt.id = ascmMaterialItem.id.ToString();
                jsonObjectReuslt.entity = ascmMaterialItem;
            }
            catch (Exception ex)
            {
                jsonObjectReuslt.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectReuslt);
            return Content(sReturn);
        }
        #endregion

        #region 排配规则管理
        public ActionResult AllocateRuleIndex()
        {
            //排配规则管理
            return View();
        }
        public ActionResult AllocateRuleList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            string userName = string.Empty;
            string userRole = string.Empty;
            string userLogistisClass = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
            userLogistisClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName, userRole);

            List<AscmAllocateRule> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmAllocateRuleService.GetInstance().GetList(ynPage, "", "", queryWord, "", userLogistisClass);
                if (list != null)
                {
                    foreach (AscmAllocateRule ascmAllocateRule in list)
                    {
                        jsonDataGridResult.rows.Add(ascmAllocateRule);
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
        public ActionResult AllocateRuleEdit(int? id)
        {
            AscmAllocateRule ascmAllocateRule = null;
            try
            {
                if (id.HasValue)
                {
                    ascmAllocateRule = AscmAllocateRuleService.GetInstance().Get(id.Value);
                    if (ascmAllocateRule != null)
                    {
                        YnUser ynUserWorker = YnUserService.GetInstance().Get(ascmAllocateRule.workerName);
                        if (ynUserWorker != null)
                            ascmAllocateRule.ynUserWorker = ynUserWorker;
                        YnUser ynUserZRanker = YnUserService.GetInstance().Get(ascmAllocateRule.zRankerName);
                        if (ynUserZRanker != null)
                            ascmAllocateRule.ynUserZRanker = ynUserZRanker;
                        YnUser ynUserDRanker = YnUserService.GetInstance().Get(ascmAllocateRule.dRankerName);
                        if (ynUserDRanker != null)
                            ascmAllocateRule.ynUserDRanker = ynUserDRanker;
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
        public ContentResult AllocateRuleSave(AscmAllocateRule ascmAllocateRule_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                YnUser ynUser = YnUserService.GetInstance().Get(ascmAllocateRule_Model.workerName);
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
                        throw new Exception("已存在此领料员的排配规则信息【" + ynUser.userName + "】!");
                    AscmAllocateRuleService.GetInstance().Update(ascmAllocateRule);
                }
                else
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmAllocateRule where workername ='" + ascmAllocateRule_Model.workerName + "'");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已存在此领料员的排配规则信息【" + ynUser.userName + "】!");
                    AscmAllocateRuleService.GetInstance().Save(ascmAllocateRule);
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmAllocateRule.id.ToString();
                jsonObjectResult.entity = ascmAllocateRule;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult AllocateRuleDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    AscmAllocateRule ascmAllocateRule = AscmAllocateRuleService.GetInstance().Get(id.Value);
                    if (ascmAllocateRule == null)
                        throw new Exception("找不到排配规则！");
                    AscmAllocateRuleService.GetInstance().Delete(ascmAllocateRule);
                    jsonObjectResult.result = true;
                    jsonObjectResult.message = "";
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
        public ActionResult AllocateRuleAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            List<AscmAllocateRule> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmAllocateRuleService.GetInstance().GetList(ynPage, "", "", "", "","");
                if (list != null && list.Count > 0)
                {
                    foreach (AscmAllocateRule ascmAllocateRule in list)
                    {
                        jsonDataGridResult.rows.Add(ascmAllocateRule);
                    }
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WorkerUserAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            return CommonUserAscxList(id, page, rows, sort, order, q, 7);
        }
        public ActionResult ZRankerUserAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            return CommonUserAscxList(id, page, rows, sort, order, q, 8);
        }
        public ActionResult DRankerUserAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            return CommonUserAscxList(id, page, rows, sort, order, q, 9);
        }
        public ActionResult GrouperUserAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            return CommonUserAscxList(id, page, rows, sort, order, q, 10);
        }
        public ActionResult MonitorUserAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            return CommonUserAscxList(id, page, rows, sort, order, q, 11);
        }
        /// <summary>
        /// 公用用户控件方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <param name="q"></param>
        /// <param name="roleid">7-领料员，8-总装排产员，9-电装排产员，10-物流组长，11-物流班长</param>
        /// <returns></returns>
        public ActionResult CommonUserAscxList(int? id, int? page, int? rows, string sort, string order, string q, int roleid)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            List<YnUser> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string sql = "from YnPosition where roleId = " + roleid.ToString(); 
                IList<YnPosition> ilist = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.Find<YnPosition>(sql);
                string ids = string.Empty;
                if (ilist != null && ilist.Count > 0)
                {
                    foreach (YnPosition ynPosition in ilist)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += ynPosition.id;
                    }
                }
                sql = "from YnDepartmentPositionUserLink";
                string queryWord = "", whereQueryWord = "", where = "";
                where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "positionid");
                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    IList<YnDepartmentPositionUserLink> ilistUser = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.Find<YnDepartmentPositionUserLink>(sql);
                    ids = string.Empty;
                    if (ilistUser != null && ilistUser.Count > 0)
                    {
                        foreach (YnDepartmentPositionUserLink ynDepartmentPositionUserLink in ilistUser)
                        {
                            if (!string.IsNullOrEmpty(ids))
                                ids += ",";
                            ids += "'" + ynDepartmentPositionUserLink.ids.userId + "'";
                        }
                    }

                    if (!string.IsNullOrEmpty(ids))
                    {
                        sql = "from YnUser";
                        where = "";
                        if (!string.IsNullOrEmpty(q))
                        {
                            queryWord = q.Trim().Replace(" ", "%");
                            whereQueryWord = "userName like '%" + queryWord + "%'";
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        }
                        if (!string.IsNullOrEmpty(ids))
                        {
                            whereQueryWord = "userId in (" + ids + ")";
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        }
                        if (!string.IsNullOrEmpty(where))
                        {
                            sql += " where " + where;

                            IList<YnUser> ilistYnUser = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.Find<YnUser>(sql, sql, ynPage);
                            if (ilistYnUser != null && ilistYnUser.Count > 0)
                            {
                                list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnUser>(ilistYnUser);
                                foreach (YnUser ynUser in list)
                                {
                                    jsonDataGridResult.rows.Add(ynUser);
                                }
                            }
                        }
                    }

                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 排产单管理
        public ActionResult DiscreteJobsIndex()
        {
            //排产单管理
            return View();
        }
        public ActionResult DiscreteJobsList(int? page, int? rows, string sort, string order, string queryWord, string queryType, string queryRanker)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            List<AscmDiscreteJobs> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            
            string userName = string.Empty;
            string userRole = string.Empty;
            string userLogistisClass = string.Empty;
            string queryDate = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
            userLogistisClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName, userRole);

            if (string.IsNullOrEmpty(queryWord))
            {
                queryDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            }
            else
            {
                queryDate = queryWord;
            }

            if (!string.IsNullOrEmpty(queryType))
            {
                switch (queryType)
                {
                    case "zz":
                        queryType = "1";
                        break;
                    case "dz":
                        queryType = "2";
                        break;
                }
            }

            try
            {
                list = AscmDiscreteJobsService.GetInstance().GetList(ynPage, "", "", queryWord, queryDate, queryType, queryRanker, userLogistisClass, userRole, userName);
                if (list != null)
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
        public ActionResult DiscreteJobsEdit(int? id)
        {
            AscmDiscreteJobs ascmDiscreteJobs = null;
            try
            {
                if (id.HasValue)
                {
                    ascmDiscreteJobs = AscmDiscreteJobsService.GetInstance().Get(id.Value);
                    YnUser ynUser = YnUserService.GetInstance().Get(ascmDiscreteJobs.workerId);
                    ascmDiscreteJobs.ynUser = ynUser;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmDiscreteJobs, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult DiscreteJobsSave(HttpPostedFileBase fileLoad, AscmDiscreteJobs ascmDiscreteJobs_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            DataTable dt = null;
            StringBuilder sb = new StringBuilder();
            try
            {
                string userName = string.Empty;
                string userRole = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                    List<YnRole> listYnRole = YnRoleService.GetInstance().GetListInUser(userName);
                    foreach (YnRole role in listYnRole)
                    {
                        if (role.name == "总装排产员" || role.name == "电装排产员")
                        {
                            userRole = role.name;
                        }
                    }
                }

                AscmDiscreteJobs ascmDiscreteJobs = null;
                //string fileUrl = "";
                if (fileLoad != null)
                {
                    string fileName = fileLoad.FileName;
                    string fileExtension = System.IO.Path.GetExtension(fileName).ToLower();

                    if (fileExtension != ".xls")
                        throw new Exception("仅支持Excel-2003的格式");

                    #region //上传Excel文件
                    //string serverPath = Server.MapPath(Request.ApplicationPath);
                    //string serverDirectory = System.IO.Path.Combine(serverPath, "_data\\UnloadingExcelFile");
                    //if (!Directory.Exists(serverDirectory))
                    //{
                    //    Directory.CreateDirectory(serverDirectory);
                    //}
                    //string serverFileName = System.IO.Path.GetFileNameWithoutExtension(fileName) + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + fileExtension;
                    //string serverFilePath = System.IO.Path.Combine(serverDirectory, serverFileName);

                    //fileLoad.SaveAs(serverFilePath);

                    //fileUrl = (Request.ApplicationPath == "/" ? "" : Request.ApplicationPath) + "/_data/UnloadingExcelFile/" + serverFileName;
                    //通过文件流转换byte[]最终转换为读取Excel文件数据的内存流
                    //FileStream fileStream = new FileStream(fileLoad.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    //byte[] bytes = new byte[fileStream.Length];
                    //fileStream.Read(bytes, 0, bytes.Length);
                    //fileStream.Close();
                    #endregion

                    Stream stream = fileLoad.InputStream;
                    byte[] bytes = new byte[fileLoad.ContentLength];
                    stream.Read(bytes, 0, bytes.Length);
                    stream.Close();

                    try
                    {
                        using (MemoryStream memoryStream = new MemoryStream(bytes))
                        {
                            dt = ExcelUtils.TranslateToTable(memoryStream, "sheet1");
                        }

                        //int icount = dt.Rows.Count;  //得到Excel的行数

                        if (dt.Rows.Count > 0)
                        {
                            List<string> ListString = new List<string>();
                            foreach (DataRow dr in dt.Rows)
                            {
                                //判断该排产单是否存在
                                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmDiscreteJobs where jobId = '" + dr[0].ToString() + "'");
                                if (object1 == null)
                                    throw new Exception("查询异常！");
                                int iCount = 0;
                                object object2 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmWipEntities where name = '" + dr[0].ToString() + "'");
                                if (object2 == null)
                                    throw new Exception("查询异常！");
                                int iTotal = 0;
                                if ((int.TryParse(object1.ToString(), out iCount) && iCount == 0) && (int.TryParse(object2.ToString(), out iTotal) && iTotal > 0))
                                {
                                    //数据赋值
                                    ascmDiscreteJobs = new AscmDiscreteJobs();
                                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmDiscreteJobs");
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
                                    if (userRole == "总装排产员")
                                    {
                                        ascmDiscreteJobs.identificationId = 1;
                                    }
                                    else if (userRole == "电装排产员")
                                    {
                                        ascmDiscreteJobs.identificationId = 2;
                                    }
                                    else
                                    {
                                        ascmDiscreteJobs.identificationId = 0;
                                    }

                                    if (ascmDiscreteJobs.identificationId == 2)
                                    {
                                        ascmDiscreteJobs.onlineTime = "上午";
                                    }
                                    else
                                    {
                                        ascmDiscreteJobs.onlineTime = dr[7].ToString();
                                    }
                                    ascmDiscreteJobs.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                    ascmDiscreteJobs.createUser = userName;
                                    ascmDiscreteJobs.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                    ascmDiscreteJobs.modifyUser = userName;
                                    ascmDiscreteJobs.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                    ascmDiscreteJobs.workerId = userName;
                                    string dateTime = DateTime.Now.Date.ToString("yyyy-MM-dd");
                                    int maxWhich = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(which) from AscmDiscreteJobs where workerId = '" + userName + "' and time like '" + dateTime + "%' and status = 2");
                                    ascmDiscreteJobs.which = ++maxWhich;

                                    AscmDiscreteJobsService.GetInstance().Save(ascmDiscreteJobs);
                                }
                                else if ((int.TryParse(object1.ToString(), out iCount) && iCount > 0) || (int.TryParse(object2.ToString(), out iTotal) && iTotal == 0))
                                {
                                    ListString.Add(dr[0].ToString());
                                }
                            }
                            if (ListString.Count > 0)
                            {
                                foreach (string item in ListString)
                                {
                                    if (!string.IsNullOrEmpty(sb.ToString()))
                                        sb.Append(",");
                                    sb.Append(item.ToString());
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = sb.ToString();
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult DiscreteJobsImport(HttpPostedFileBase fileImport)
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
                                throw new Exception("排产作业查询异常！");
                            int iCount = 0;
                            object object2 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmWipEntities where name = '" + dr[0].ToString() + "'");
                            if (object2 == null)
                                throw new Exception("ERP作业查询异常！");
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
                                if (userRole == "总装排产员")
                                {
                                    ascmDiscreteJobs.identificationId = 1;
                                }
                                else if (userRole == "电装排产员")
                                {
                                    ascmDiscreteJobs.identificationId = 2;
                                }
                                else
                                {
                                    ascmDiscreteJobs.identificationId = 0;
                                }

                                if (ascmDiscreteJobs.identificationId == 2)
                                {
                                    ascmDiscreteJobs.onlineTime = "上午";
                                }
                                else
                                {
                                    ascmDiscreteJobs.onlineTime = dr[7].ToString();
                                }
                                ascmDiscreteJobs.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                ascmDiscreteJobs.createUser = userName;
                                ascmDiscreteJobs.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                ascmDiscreteJobs.modifyUser = userName;
                                ascmDiscreteJobs.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                ascmDiscreteJobs.workerId = userName;
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
                        if (list != null &&list.Count > 0)
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
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult DiscreteJobsEditSave(AscmDiscreteJobs ascmDiscreteJobs_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                //判断用户是否有排产员的权限
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                    List<YnRole> listYnRole = YnRoleService.GetInstance().GetListInUser(userName);
                    int nI = 0;
                    foreach (YnRole role in listYnRole)
                    {
                        if (role.name == "总装排产员" || role.name == "电装排产员")
                        {
                            nI++;
                        }
                    }
                    if (nI == 0)
                        throw new Exception("该用户没有排产员权限！");
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
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult DiscreteJobsDelete(string releaseHeaderIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string sql = "from AscmDiscreteJobs";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(releaseHeaderIds))
                {
                    int nI = 0;
                    whereQueryWord = "id in (" + releaseHeaderIds + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    whereQueryWord = "status = 1";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    if (!string.IsNullOrEmpty(where))
                        sql += " where " + where;
                   IList<AscmDiscreteJobs> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(sql);
                   if (ilist != null && ilist.Count > 0)
                   {
                       List<AscmDiscreteJobs> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDiscreteJobs>(ilist);
                       foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                       {
                           AscmDiscreteJobsService.GetInstance().Delete(ascmDiscreteJobs);
                           nI++;
                       }
                   }
                   if (nI > 0)
                   {
                       jsonObjectResult.result = true;
                       jsonObjectResult.message = "";
                   }
                }
                else
                {
                    jsonObjectResult.message = "该作业已生成任务无法删除或传入参数[id = null]错误！";
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        #endregion 

        #region  领料作业计划管理
        public ActionResult JobPlanIndex()
        {
            //领料作业计划管理
            return View();
        }

        [HttpPost]
        public ContentResult GetTasksList(string jobDate)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();

            string userName = string.Empty;
            string userRole = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }
            List<YnRole> listYnRole = YnRoleService.GetInstance().GetListInUser(userName);
            int nI = 0;
            foreach (YnRole role in listYnRole)
            {
                if (role.name == "领料员")
                {
                    nI++;
                }
            }
            if (nI > 0)
            {
                userRole = userName;
            }

            try
            {
                string sql = "from AscmDiscreteJobs";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(jobDate))
                {
                    whereQueryWord = "time like '" + jobDate + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(userRole))
                {
                    whereQueryWord = "workerId = '" + userRole + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                whereQueryWord = "status = 1";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                
                List<AscmDiscreteJobs> list = AscmDiscreteJobsService.GetInstance().GetList(sql, true);
                //关联信息
                setWipEntities(list);
                setWipRequirementOperations(list);
                setMaterialItem(list);
                setWarehouse(list);

                List<AscmGetMaterialCreateTask> listTasks = createTasks(list);
                
                foreach(AscmGetMaterialCreateTask task in listTasks)
                {
                    jsonDataGridResult.rows.Add(task);
                }

                saveTasks(listTasks);
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                jsonDataGridResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonDataGridResult);
            return Content(sReturn);
        }
        public void saveTasks(List<AscmGetMaterialCreateTask> listTasks)
        {
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }
            string bom_ids = string.Empty;
            int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmGetMaterialTask");

            List<AscmGetMaterialTask> myTasks = new List<AscmGetMaterialTask>();
            List<AscmDiscreteJobs> myJobs = new List<AscmDiscreteJobs>();
            List<AscmWipRequirementOperations> myBoms = new List<AscmWipRequirementOperations>();

            foreach (AscmGetMaterialCreateTask task in listTasks)
            {
                //任务表
                AscmGetMaterialTask myTask = new AscmGetMaterialTask();
                myTask.id = maxId + 1 + myTasks.Count;
                myTask.taskId = task.taskId;
                myTask.productLine = task.productLine;
                myTask.warehouserId = task.warehouserId;
                myTask.mtlCategoryStatus = task.categoryStatus;
                myTask.rankerId = task.rankMan;
                myTask.workerId = string.Empty;
                myTask.starTime = string.Empty;
                myTask.endTime = string.Empty;
                myTask.status = "NOTALLOCATE";
                myTask.tip = string.Empty;

                myTask.createUser = userName;
                myTask.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                myTask.modifyUser = userName;
                myTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                myTask.IdentificationId = task.IdentificationId;
                myTask.materialType = task.materialType;
                myTask.materialDocNumber = task.materialDocNumber;
                myTask.uploadDate = task.uploadDate;
                myTask.taskTime = task.taskTime;
                myTask.warehouserPlace = task.warehouserPlace;
                myTask.which = task.which;

                myTasks.Add(myTask);

                foreach (AscmDiscreteJobs job in task.ascmDiscreteJobsList)
                {
                    foreach (AscmWipRequirementOperations bom in job.listAscmWipBom)
                    {
                        //BOM清单
                        bom.taskId = myTask.id;
                        myBoms.Add(bom);
                    }
                }
            }

            string ids = string.Empty;
            foreach (AscmGetMaterialCreateTask task in listTasks)
            {
                foreach (AscmDiscreteJobs job in task.ascmDiscreteJobsList)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'" + job.jobId + "'";
                }
            }
            string sql = "from AscmDiscreteJobs where JOBID in (" + ids + ")";
            IList<AscmDiscreteJobs> ilistAscmDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(sql);
            if (ilistAscmDiscreteJobs != null && ilistAscmDiscreteJobs.Count > 0)
            {
                myJobs = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDiscreteJobs>(ilistAscmDiscreteJobs);
                foreach (AscmDiscreteJobs ascmDiscreteJobs in myJobs)
                {
                    ascmDiscreteJobs.status = 2;
                }
            }
            foreach (AscmWipRequirementOperations bom in myBoms)
            {
                if (!string.IsNullOrEmpty(bom_ids))
                    bom_ids += ",";
                bom_ids += "" + bom.id + "";
            }
            //添加任务
            AscmGetMaterialTaskService.GetInstance().Save(myTasks);
            //更新作业表
            //AscmDiscreteJobsService.GetInstance().Update(myJobs);
            //更新BOM表
            //AscmWipRequirementOperationsService.GetInstance().Update(myBoms);
        }

        public List<AscmGetMaterialCreateTask> createTasks(List<AscmDiscreteJobs> list)
        {
            string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
            int iCount = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmGetMaterialTask where createTime like '" + date + "%'");

            List<AscmGetMaterialCreateTask> listTasks = new List<AscmGetMaterialCreateTask>();
            foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
            {
                if (ascmDiscreteJobs.listAscmWipBom == null)
                    ascmDiscreteJobs.listAscmWipBom = new List<AscmWipRequirementOperations>();
                foreach (AscmWipRequirementOperations ascmWipRequirementOperations in ascmDiscreteJobs.listAscmWipRequirementOperations)
                {
                    if (ascmDiscreteJobs.status == AscmDiscreteJobs.StatusDefine.defaultvalue)
                    {
                        AscmGetMaterialCreateTask task = new AscmGetMaterialCreateTask();
                        if(task.ascmDiscreteJobsList == null)
                            task.ascmDiscreteJobsList = new List<AscmDiscreteJobs>();
                        task.taskId = getTaskId(iCount + listTasks.Count + 1);
                        task.IdentificationId = ascmDiscreteJobs.identificationId;
                        task.productLine = ascmDiscreteJobs.productLine;
                        task.warehouserId = ascmWipRequirementOperations.supplySubinventory;
                        task.materialDocNumber = ((ascmWipRequirementOperations.ascmMaterialItem) == null) ? null : ascmWipRequirementOperations.ascmMaterialItem.docNumber;
                        task.materialType = ascmWipRequirementOperations.wipSupplyType;
                        task.dateReleased = ascmWipRequirementOperations.ascmWipDiscreteJobs.scheduledStartDate;

                        switch (task.IdentificationId)
                        {
                            case 1://总装
                                task.categoryStatus = ((ascmWipRequirementOperations.ascmMaterialItem) == null) ? null : ((AscmCommonHelperService.GetInstance().JudgeSpecWmRelatedMaterial(task.warehouserId, task.materialDocNumber)) ? "" : ascmWipRequirementOperations.ascmMaterialItem.zMtlCategoryStatus);
                                break;
                            case 2://电装
                                task.categoryStatus = ((ascmWipRequirementOperations.ascmMaterialItem) == null) ? null : ((AscmCommonHelperService.GetInstance().JudgeSpecWmRelatedMaterial(task.warehouserId, task.materialDocNumber)) ? "" : ascmWipRequirementOperations.ascmMaterialItem.dMtlCategoryStatus);
                                break;
                        }
                        task.uploadDate = DateTime.Parse(ascmDiscreteJobs.time).ToString("yyyy-MM-dd");
                        task.taskTime = string.IsNullOrEmpty(ascmDiscreteJobs.onlineTime) ? "上午" : ascmDiscreteJobs.onlineTime;
                        task.warehouserPlace = ((ascmWipRequirementOperations.ascmWarehouse) == null) ? null : ascmWipRequirementOperations.ascmWarehouse.description;
                        task.rankMan = ascmDiscreteJobs.workerId;
                        task.rankManName = ascmDiscreteJobs.rankerName;
                        task.which = ascmDiscreteJobs.which;

                        if (!string.IsNullOrEmpty(task.IdentificationId.ToString()) && !string.IsNullOrEmpty(task.productLine) && !string.IsNullOrEmpty(task.warehouserId) && !string.IsNullOrEmpty(task.materialDocNumber) && !string.IsNullOrEmpty(task.materialType.ToString()) && !string.IsNullOrEmpty(task.taskTime) && !string.IsNullOrEmpty(task.uploadDate) && !string.IsNullOrEmpty(task.rankMan) && !string.IsNullOrEmpty(task.which.ToString()))
                        {
                            if (task.categoryStatus == "PRESTOCK" || task.categoryStatus == "MIXSTOCK" || task.categoryStatus == "")
                            {
                                #region
                                switch (task.IdentificationId)
                                {
                                    case 1://总装
                                        if ((task.warehouserId.Substring(0, 4).ToUpper() == "W112") && (task.materialDocNumber.Substring(0, 4) == "2014"))
                                        {
                                            //W112开头的压缩机
                                            if (contains(listTasks, task) == null)
                                            {
                                                AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                task.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);

                                                listTasks.Add(task);
                                            }
                                            else
                                            {
                                                //任务存在 获取该任务
                                                AscmGetMaterialCreateTask existTask = contains(listTasks, task);
                                                if (containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs) == null)
                                                {
                                                    //任务不包含作业
                                                    AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                    ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                    existTask.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);
                                                }
                                                else
                                                {
                                                    //任务包含作业
                                                    AscmDiscreteJobs existJob = containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs);
                                                    existJob.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                }
                                            }
                                        }
                                        else if ((task.warehouserId.Substring(0, 4).ToUpper() == "B631") && (task.materialType == 1))
                                        {
                                            //B631开头的子库且为推式
                                            if (contains(listTasks, task) == null)
                                            {
                                                AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                task.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);

                                                task.materialDocNumber = null;
                                                task.categoryStatus = null;
                                                listTasks.Add(task);
                                            }
                                            else
                                            {
                                                //任务存在 获取该任务
                                                AscmGetMaterialCreateTask existTask = contains(listTasks, task);
                                                if (containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs) == null)
                                                {
                                                    //任务不包含作业
                                                    AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                    ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                    existTask.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);
                                                }
                                                else
                                                {
                                                    //任务包含作业
                                                    AscmDiscreteJobs existJob = containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs);
                                                    existJob.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                }
                                            }
                                        }
                                        else if ((task.warehouserId.Substring(0, 4).ToUpper() == "B632") && (task.materialType == 1))
                                        {
                                            //B632开头的子库且为推式
                                            if (contains(listTasks, task) == null)
                                            {
                                                AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                task.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);

                                                task.materialDocNumber = null;
                                                task.categoryStatus = null;
                                                listTasks.Add(task);
                                            }
                                            else
                                            {
                                                //任务存在 获取该任务
                                                AscmGetMaterialCreateTask existTask = contains(listTasks, task);
                                                if (containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs) == null)
                                                {
                                                    //任务不包含作业
                                                    AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                    ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                    existTask.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);
                                                }
                                                else
                                                {
                                                    //任务包含作业
                                                    AscmDiscreteJobs existJob = containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs);
                                                    existJob.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                }
                                            }
                                        }
                                        else if ((task.warehouserId.Substring(0, 4).ToUpper() == "E221") && (task.materialType == 1))
                                        {
                                            //E221开头的子库且为推式
                                            if (contains(listTasks, task) == null)
                                            {
                                                AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                task.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);

                                                task.materialDocNumber = null;
                                                task.categoryStatus = null;
                                                listTasks.Add(task);
                                            }
                                            else
                                            {
                                                //任务存在 获取该任务
                                                AscmGetMaterialCreateTask existTask = contains(listTasks, task);
                                                if (containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs) == null)
                                                {
                                                    //任务不包含作业
                                                    AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                    ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                    existTask.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);
                                                }
                                                else
                                                {
                                                    //任务包含作业
                                                    AscmDiscreteJobs existJob = containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs);
                                                    existJob.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                }
                                            }
                                        }
                                        else if ((task.warehouserId.Substring(0, 4).ToUpper() == "E222") && (task.materialType == 1))
                                        {
                                            //E222开头的子库且为推式
                                            if (contains(listTasks, task) == null)
                                            {
                                                AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                task.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);

                                                task.materialDocNumber = null;
                                                task.categoryStatus = null;
                                                listTasks.Add(task);
                                            }
                                            else
                                            {
                                                //任务存在 获取该任务
                                                AscmGetMaterialCreateTask existTask = contains(listTasks, task);
                                                if (containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs) == null)
                                                {
                                                    //任务不包含作业
                                                    AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                    ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                    existTask.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);
                                                }
                                                else
                                                {
                                                    //任务包含作业
                                                    AscmDiscreteJobs existJob = containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs);
                                                    existJob.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                }
                                            }
                                        }
                                        else if ((task.warehouserId.Substring(0, 1).ToUpper() == "W") && (task.materialType == 1) && (task.warehouserId.Substring(0,4).ToUpper().ToString() != "W412"))
                                        {
                                            //以W开头的字库且为推式
                                            if (task.categoryStatus == "PRESTOCK")//需备料
                                            {
                                                if (contains(listTasks, task) == null)
                                                {
                                                    AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                    ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                    task.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);

                                                    listTasks.Add(task);
                                                }
                                                else
                                                {
                                                    //任务存在 获取该任务
                                                    AscmGetMaterialCreateTask existTask = contains(listTasks, task);
                                                    if (containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs) == null)
                                                    {
                                                        //任务不包含作业
                                                        AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                        ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                        existTask.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);
                                                    }
                                                    else
                                                    {
                                                        //任务包含作业
                                                        AscmDiscreteJobs existJob = containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs);
                                                        existJob.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                    }
                                                }
                                            }
                                            else if (task.categoryStatus == "MIXSTOCK")//需配料
                                            {
                                                if (contains(listTasks, task) == null)
                                                {
                                                    AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                    ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                    task.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);

                                                    task.materialDocNumber = null;
                                                    listTasks.Add(task);
                                                }
                                                else
                                                {
                                                    //任务存在 获取该任务
                                                    AscmGetMaterialCreateTask existTask = contains(listTasks, task);
                                                    if (containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs) == null)
                                                    {
                                                        //任务不包含作业
                                                        AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                        ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                        existTask.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);
                                                    }
                                                    else
                                                    {
                                                        //任务包含作业
                                                        AscmDiscreteJobs existJob = containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs);
                                                        existJob.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case 2://电装
                                        if ((task.warehouserId.Substring(0, 4).ToUpper() == "B631") && (task.materialType == 1))
                                        {
                                            //B631开头的子库且为推式
                                            if (contains(listTasks, task) == null)
                                            {
                                                AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                task.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);

                                                task.materialDocNumber = null;
                                                task.taskTime = null;
                                                task.categoryStatus = null;
                                                listTasks.Add(task);
                                            }
                                            else
                                            {
                                                //任务存在 获取该任务
                                                AscmGetMaterialCreateTask existTask = contains(listTasks, task);
                                                if (containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs) == null)
                                                {
                                                    //任务不包含作业
                                                    AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                    ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                    existTask.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);
                                                }
                                                else
                                                {
                                                    //任务包含作业
                                                    AscmDiscreteJobs existJob = containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs);
                                                    existJob.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                }
                                            }
                                        }
                                        else if ((task.warehouserId.Substring(0, 4).ToUpper() == "B632") && (task.materialType == 1))
                                        {
                                            //B632开头的子库且为推式
                                            if (contains(listTasks, task) == null)
                                            {
                                                AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                task.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);
                                                
                                                task.materialDocNumber = null;
                                                task.taskTime = null;
                                                task.categoryStatus = null;
                                                listTasks.Add(task);
                                            }
                                            else
                                            {
                                                //任务存在 获取该任务
                                                AscmGetMaterialCreateTask existTask = contains(listTasks, task);
                                                if (containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs) == null)
                                                {
                                                    //任务不包含作业
                                                    AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                    ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                    existTask.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);
                                                }
                                                else
                                                {
                                                    //任务包含作业
                                                    AscmDiscreteJobs existJob = containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs);
                                                    existJob.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                }
                                            }
                                        }
                                        else if ((task.warehouserId.Substring(0, 1).ToUpper() == "W") && (task.materialType == 1) && (task.warehouserId.Substring(0, 4).ToUpper().ToString() != "W412"))
                                        {
                                            //W开头的子库且为推式
                                            if (contains(listTasks, task) == null)
                                            {
                                                AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                task.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);

                                                task.productLine = null;
                                                task.materialDocNumber = null;
                                                task.taskTime = null;
                                                task.categoryStatus = "MIXSTOCK";
                                                listTasks.Add(task);
                                            }
                                            else
                                            {
                                                //任务存在 获取该任务
                                                AscmGetMaterialCreateTask existTask = contains(listTasks, task);
                                                if (containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs) == null)
                                                {
                                                    //任务不包含作业
                                                    AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                                    ascm_DiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                    existTask.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);
                                                }
                                                else
                                                {
                                                    //任务包含作业
                                                    AscmDiscreteJobs existJob = containsJob(existTask.ascmDiscreteJobsList, ascmDiscreteJobs);
                                                    existJob.listAscmWipBom.Add(ascmWipRequirementOperations);
                                                }
                                            }
                                        }
                                        break;
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            return listTasks;
        }
        public AscmDiscreteJobs containsJob(List<AscmDiscreteJobs> listJobs, AscmDiscreteJobs job)
        {
            if (listJobs.Count == 0)
                return null;
            foreach (AscmDiscreteJobs item in listJobs)
            {
                if (item.jobId == job.jobId)
                    return item;
            }
            return null;
        }
        public string getTaskId(int number)
        {   
            string autoTask = AscmCommonHelperService.GetInstance().GetConfigTaskWords(0);

            int a = number / 1000;
            int b = (number - a * 1000) / 100;
            int c = (number - a * 1000 - b * 100) / 10;
            int d = (number - a * 1000 - b * 100 - c * 10);

            return autoTask + a.ToString() + b.ToString() + c.ToString() + d.ToString();
        }
        public AscmGetMaterialCreateTask contains(List<AscmGetMaterialCreateTask> listTasks, AscmGetMaterialCreateTask task)
        {
            if (listTasks.Count == 0)
                return null;

            switch (task.IdentificationId)
            {
                case 1://总装
                    if ((task.warehouserId.Substring(0, 4).ToUpper() == "W112") && (task.materialDocNumber.Substring(0, 4) == "2014"))
                    {
                        //W112开头的压缩机
                        foreach (AscmGetMaterialCreateTask item in listTasks)
                        {
                            if ((item.rankMan == task.rankMan) && (item.IdentificationId == task.IdentificationId) && (item.warehouserId == task.warehouserId) && (item.materialDocNumber == task.materialDocNumber) && (item.categoryStatus == task.categoryStatus) && (item.productLine == task.productLine) && (item.uploadDate == task.uploadDate) && (item.taskTime == task.taskTime) && (item.which == task.which))
                                return item;
                        }
                    }
                    else if ((task.warehouserId.Substring(0, 4).ToUpper() == "B631") && (task.materialType == 1))
                    {
                        //B631开头的子库且为推式
                        foreach (AscmGetMaterialCreateTask item in listTasks)
                        {
                            if ((item.rankMan == task.rankMan) && (item.IdentificationId == task.IdentificationId) && (item.warehouserId == task.warehouserId) && (item.uploadDate == task.uploadDate) && (item.taskTime == task.taskTime) && (item.productLine == task.productLine) && (item.which == task.which))
                                return item;
                        }
                    }
                    else if ((task.warehouserId.Substring(0, 4).ToUpper() == "B632") && (task.materialType == 1))
                    {
                        //B632开头的子库且为推式
                        foreach (AscmGetMaterialCreateTask item in listTasks)
                        {
                            if ((item.rankMan == task.rankMan) && (item.IdentificationId == task.IdentificationId) && (item.warehouserId == task.warehouserId) && (item.uploadDate == task.uploadDate) && (item.taskTime == task.taskTime) && (item.productLine == task.productLine) && (item.which == task.which))
                                return item;
                        }
                    }
                    else if ((task.warehouserId.Substring(0, 4).ToUpper() == "E221") && (task.materialType == 1))
                    {
                        //E221开头的子库且为推式
                        foreach (AscmGetMaterialCreateTask item in listTasks)
                        {
                            if ((item.rankMan == task.rankMan) && (item.IdentificationId == task.IdentificationId) && (item.warehouserId == task.warehouserId) && (item.uploadDate == task.uploadDate) && (item.taskTime == task.taskTime) && (item.productLine == task.productLine) && (item.which == task.which))
                                return item;
                        }
                    }
                    else if ((task.warehouserId.Substring(0, 4).ToUpper() == "E222") && (task.materialType == 1))
                    {
                        //E222开头的子库且为推式
                        foreach (AscmGetMaterialCreateTask item in listTasks)
                        {
                            if ((item.rankMan == task.rankMan) && (item.IdentificationId == task.IdentificationId) && (item.warehouserId == task.warehouserId) && (item.uploadDate == task.uploadDate) && (item.taskTime == task.taskTime) && (item.productLine == task.productLine) && (item.which == task.which))
                                return item;
                        }
                    }
                    else if ((task.warehouserId.Substring(0, 1).ToUpper() == "W") && (task.materialType == 1))
                    {
                        //以W开头的字库且为推式
                        if (task.categoryStatus == "PRESTOCK")//需备料
                        {
                            foreach (AscmGetMaterialCreateTask item in listTasks)
                            {
                                if ((item.rankMan == task.rankMan) && (item.IdentificationId == task.IdentificationId) && (item.warehouserId == task.warehouserId) && (item.materialDocNumber == task.materialDocNumber) && (item.categoryStatus == task.categoryStatus) && (item.uploadDate == task.uploadDate) && (item.taskTime == task.taskTime) && (item.which == task.which))
                                    return item;
                            }
                        }
                        else if (task.categoryStatus == "MIXSTOCK")//需配料
                        {
                            foreach (AscmGetMaterialCreateTask item in listTasks)
                            {
                                if ((item.rankMan == task.rankMan) && (item.IdentificationId == task.IdentificationId) && (item.warehouserId == task.warehouserId) && (item.categoryStatus == task.categoryStatus) && (item.productLine == task.productLine) && (item.uploadDate == task.uploadDate) && (item.taskTime == task.taskTime))
                                    return item;
                            }
                        }
                    }
                    break;
                case 2://电装
                    if ((task.warehouserId.Substring(0, 4).ToUpper() == "B631") && (task.materialType == 1))
                    {
                        //B631开头的子库且为推式
                        foreach (AscmGetMaterialCreateTask item in listTasks)
                        {
                            if ((item.rankMan == task.rankMan) && (item.IdentificationId == task.IdentificationId) && (item.warehouserId == task.warehouserId) && (item.uploadDate == task.uploadDate) && (item.productLine == task.productLine) && (item.which == task.which))
                                return item;
                        }
                    }
                    else if ((task.warehouserId.Substring(0, 4).ToUpper() == "B632") && (task.materialType == 1))
                    {
                        //B632开头的子库且为推式
                        foreach (AscmGetMaterialCreateTask item in listTasks)
                        {
                            if ((item.rankMan == task.rankMan) && (item.IdentificationId == task.IdentificationId) && (item.warehouserId == task.warehouserId) && (item.uploadDate == task.uploadDate) && (item.productLine == task.productLine) && (item.which == task.which))
                                return item;
                        }
                    }
                    else if ((task.warehouserId.Substring(0, 1).ToUpper() == "W") && (task.materialType == 1))
                    {
                        //W开头的子库且为推式
                        foreach (AscmGetMaterialCreateTask item in listTasks)
                        {
                            if ((item.rankMan == task.rankMan) && (item.IdentificationId == task.IdentificationId) && (item.warehouserId == task.warehouserId) && (item.uploadDate == task.uploadDate) && (item.which == task.which))
                                return item;
                        }
                    }
                    break;
            }
            return null;
        }
        public void setWipEntities(List<AscmDiscreteJobs> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'" + ascmDiscreteJobs.jobId + "'";
                }
                string sql = "from AscmWipEntities";
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "name");

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmWipEntities> ilistAscmWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                if (ilistAscmWipEntities != null && ilistAscmWipEntities.Count > 0)
                {
                    List<AscmWipEntities> listAscmWipEntities = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilistAscmWipEntities);
                    foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                    {
                        ascmDiscreteJobs.ascmWipEntities = listAscmWipEntities.Find(e => e.name == ascmDiscreteJobs.jobId);
                    }
                }
            }
        }
        public void setWipRequirementOperations(List<AscmDiscreteJobs> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmDiscreteJobs.wipEntitiesId + "";
                }

                string sql = "from AscmWipRequirementOperations";
                string where = "", whereQueryWord = "";
                where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "wipEntityId");

                whereQueryWord = "wipSupplyType = 1";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmWipRequirementOperations> ilistAscmWipRequirementOperations = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql);
                if (ilistAscmWipRequirementOperations != null && ilistAscmWipRequirementOperations.Count > 0)
                {
                    List<AscmWipRequirementOperations> listAscmWipRequirementOperations = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilistAscmWipRequirementOperations);
                    foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                    {
                        ascmDiscreteJobs.listAscmWipRequirementOperations = listAscmWipRequirementOperations.FindAll(e => e.wipEntityId == ascmDiscreteJobs.wipEntitiesId);
                    }
                }
            }
        }
        public void setMaterialItem(List<AscmDiscreteJobs> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                {                    
                    foreach (AscmWipRequirementOperations ascmWipRequirementOperations in ascmDiscreteJobs.listAscmWipRequirementOperations)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += "" + ascmWipRequirementOperations.inventoryItemId + "";
                    }
                }
                string sql = "from AscmMaterialItem";
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmMaterialItem> ilistAscmWipBom = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                if (ilistAscmWipBom != null && ilistAscmWipBom.Count > 0)
                {
                    List<AscmMaterialItem> listAscmWipBom = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistAscmWipBom);
                    foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                    {
                        foreach (AscmWipRequirementOperations ascmWipRequirementOperations in ascmDiscreteJobs.listAscmWipRequirementOperations)
                        {
                            ascmWipRequirementOperations.ascmMaterialItem = listAscmWipBom.Find(e => e.id == ascmWipRequirementOperations.inventoryItemId);
                        }
                    }
                }
            }
        }
        public void setWarehouse(List<AscmDiscreteJobs> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                {
                    foreach (AscmWipRequirementOperations ascmWipRequirementOperations in ascmDiscreteJobs.listAscmWipRequirementOperations)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += "'" + ascmWipRequirementOperations.supplySubinventory + "'";
                    }
                }
                string sql = "from AscmWarehouse";
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmWarehouse> ilistAscmWarehouse = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarehouse>(sql);
                if (ilistAscmWarehouse != null && ilistAscmWarehouse.Count > 0)
                {
                    List<AscmWarehouse> listAscmWarehouse = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarehouse>(ilistAscmWarehouse);
                    foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                    {
                        foreach (AscmWipRequirementOperations ascmWipRequirementOperations in ascmDiscreteJobs.listAscmWipRequirementOperations)
                        {
                            ascmWipRequirementOperations.ascmWarehouse = listAscmWarehouse.Find(e => e.id == ascmWipRequirementOperations.supplySubinventory);
                        }
                    }
                }
            }
        }
        #endregion

        #region  领料作业任务分配管理
        public ActionResult JobAllocateIndex()
        {
            //领料作业计划管理
            return View();
        }
        [HttpPost]
        public ContentResult GetAllocateTasksList(int? page, int? rows, string sort, string order, string queryCreateTime, string queryStatus)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            List<AscmGetMaterialTask> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
                string userLogistisClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName, userRole);

                string where = "",whereQueryWord = "";
                string sql = "from AscmGetMaterialTask";
                if (!string.IsNullOrEmpty(queryCreateTime))
                {
                    whereQueryWord = " (createTime like '" + queryCreateTime + "%')";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                if (!string.IsNullOrEmpty(queryStatus))
                {
                    whereQueryWord = " (status = '" + queryStatus + "')";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(userLogistisClass))
                {
                    IList<AscmLogisticsClassInfo> ilistAscmLogisticsClassInfo = AscmLogisticsClassInfoService.GetInstance().GetList("from AscmLogisticsClassInfo where logisticsClass in (" + userLogistisClass + ")", false, false);
                    string ids = string.Empty;
                    if (ilistAscmLogisticsClassInfo != null && ilistAscmLogisticsClassInfo.Count > 0)
                    {
                        foreach (AscmLogisticsClassInfo ascmlogisticsClassInfo in ilistAscmLogisticsClassInfo)
                        {
                            if (!string.IsNullOrEmpty(ids))
                                ids += ",";
                            ids += ascmlogisticsClassInfo.id;
                        }
                    }

                    string newCondition = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "logisticsClassId");
                    if (!string.IsNullOrEmpty(newCondition))
                    {
                        string newsql = "from AscmAllocateRule where " + newCondition;
                        IList<AscmAllocateRule> ilistAscmAllocateRule = AscmAllocateRuleService.GetInstance().GetList(newsql, false, false, false);
                        ids = "";
                        if (ilistAscmAllocateRule != null && ilistAscmAllocateRule.Count > 0)
                        {
                            foreach (AscmAllocateRule ascmAllocateRule in ilistAscmAllocateRule)
                            {
                                if (!string.IsNullOrEmpty(ids) && (!string.IsNullOrEmpty(ascmAllocateRule.zRankerName) || !string.IsNullOrEmpty(ascmAllocateRule.dRankerName)))
                                    ids += ",";
                                if (!string.IsNullOrEmpty(ascmAllocateRule.zRankerName))
                                    ids += "'" + ascmAllocateRule.zRankerName + "'";
                                if (!string.IsNullOrEmpty(ids) && !string.IsNullOrEmpty(ascmAllocateRule.dRankerName))
                                    ids += ",";
                                if (!string.IsNullOrEmpty(ascmAllocateRule.dRankerName))
                                    ids += "'" + ascmAllocateRule.dRankerName + "'";
                            }
                        }

                        string whereOther = "";
                        whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "rankerId");
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(whereOther, whereQueryWord);
                        whereQueryWord = "rankerId is null";
                        whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(whereOther, whereQueryWord);

                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                    }
                }
                else if (string.IsNullOrEmpty(userLogistisClass) && userRole == "领料员")
                {
                    whereQueryWord = "workerId = '" + userName + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where + " order by IdentificationId,workerId,warehouserId,mtlCategoryStatus,productLine";
                IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql, sql, ynPage);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                    setDiscreteJobs(list);
                    AscmGetMaterialTaskService.GetInstance().SetRanker(list);
                    AscmGetMaterialTaskService.GetInstance().SetWorker(list);
                    AscmGetMaterialTaskService.GetInstance().SetLogisticsClassName(list);
                    list = list.OrderBy(e => e.statusInt).ToList<AscmGetMaterialTask>();
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
                throw ex;
            }
            string sReturn = JsonConvert.SerializeObject(jsonDataGridResult);
            return Content(sReturn);
        }
        public void setDiscreteJobs(List<AscmGetMaterialTask> list)
        {
            if (list != null && list.Count > 0)
            {
                List<AscmWipRequirementOperations> listAscmWipRequirementOperations = new List<AscmWipRequirementOperations>();
                List<AscmWipEntities> listAscmWipEntities = new List<AscmWipEntities>();
                List<AscmDiscreteJobs> listAscmDiscreteJobs = new List<AscmDiscreteJobs>();

                string ids = string.Empty;
                string entity_ids = string.Empty;
                string entity_names = string.Empty;
                
                foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmGetMaterialTask.id + "";
                }
                string[] strArray = ids.Split(',');
                string where = "", whereQueryWord = "";
                string sql = "from AscmWipRequirementOperations";
                bool flag = false;
                if (strArray.Length >= 1000)
                {
                    decimal temp = Convert.ToDecimal(strArray.Length) / 1000;
                    int count = Convert.ToInt16(Math.Ceiling(temp));
                    for (int i = 0; i < count; i++)
                    {
                        string str = string.Empty;
                        for (int j = i * 1000; j < strArray.Length; j++)
                        {
                            if (j < (i + 1) * 1000 && j >= i * 1000)
                            {
                                if (!string.IsNullOrEmpty(str))
                                    str += ",";
                                str += strArray[j];
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            whereQueryWord = "taskId in (" + str + ")";
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(where, whereQueryWord);
                        }
                    }
                }
                else
                {
                    whereQueryWord = "taskId in (" + ids + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                
                IList<AscmWipRequirementOperations> ilistAscmWipRequirementOperations = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql);
                if (ilistAscmWipRequirementOperations != null && ilistAscmWipRequirementOperations.Count > 0)
                {
                    listAscmWipRequirementOperations = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilistAscmWipRequirementOperations);

                    foreach (AscmWipRequirementOperations ascmWipRequirementOperations in listAscmWipRequirementOperations)
                    {
                        if (!string.IsNullOrEmpty(entity_ids))
                            entity_ids += ",";
                        entity_ids += "" + ascmWipRequirementOperations.wipEntityId + "";
                    }

                    string[] entity_strArray = entity_ids.Split(',');
                    string entity_where = "", entity_whereQueryWord = "";
                    string entity_sql = "from AscmWipEntities";
                    bool entity_flag = false;
                    if (entity_strArray.Length >= 1000)
                    {
                        decimal temp = Convert.ToDecimal(entity_strArray.Length) / 1000;
                        int count = Convert.ToInt16(Math.Ceiling(temp));
                        for (int i = 0; i < count; i++)
                        {
                            string str = string.Empty;
                            for (int j = i * 1000; j < entity_strArray.Length; j++)
                            {
                                if (j < (i + 1) * 1000 && j >= i * 1000)
                                {
                                    if (!string.IsNullOrEmpty(str))
                                        str += ",";
                                    str += entity_strArray[j];
                                    entity_flag = true;
                                }
                            }
                            if (entity_flag)
                            {
                                entity_whereQueryWord = "wipEntityId in (" + str + ")";
                                entity_where = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(entity_where, entity_whereQueryWord);
                            }
                        }
                    }
                    else
                    {
                        entity_whereQueryWord = "wipEntityId in (" + entity_ids + ")";
                        entity_where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(entity_where, entity_whereQueryWord);
                    }
                    if (!string.IsNullOrEmpty(entity_where))
                        entity_sql += " where " + entity_where;

                    IList<AscmWipEntities> ilistAscmWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(entity_sql);
                    if (ilistAscmWipEntities != null && ilistAscmWipEntities.Count > 0)
                    {
                        listAscmWipEntities = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilistAscmWipEntities);
                        foreach (AscmWipEntities ascmWipEntities in listAscmWipEntities)
                        {
                            if (!string.IsNullOrEmpty(entity_names))
                                entity_names += ",";
                            entity_names += "'" + ascmWipEntities.name + "'";
                        }

                        string[] names_strArray = entity_names.Split(',');
                        string names_where = "", names_whereQueryWord = "";
                        string names_sql = "from AscmDiscreteJobs";
                        if (names_strArray.Length >= 1000)
                        {
                            decimal temp = Convert.ToDecimal(names_strArray.Length) / 1000;
                            int count = Convert.ToInt16(Math.Ceiling(temp));
                            for (int i = 0; i < count; i++)
                            {
                                string str = string.Empty;
                                for (int j = i * 1000; j < names_strArray.Length; j++)
                                {
                                    if (j < (i + 1) * 1000 && j >= i * 1000)
                                    {
                                        if (!string.IsNullOrEmpty(str))
                                            str += ",";
                                        str += names_strArray[j];
                                        entity_flag = true;
                                    }
                                }
                                if (entity_flag)
                                {
                                    names_whereQueryWord = "jobId in (" + str + ")";
                                    names_where = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(names_where, names_whereQueryWord);
                                }
                            }
                        }
                        else
                        {
                            names_whereQueryWord = "jobId in (" + entity_names + ")";
                            names_where = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(names_where, names_whereQueryWord);
                        }
                        if (!string.IsNullOrEmpty(names_where))
                            names_sql += " where " + names_where;

                        IList<AscmDiscreteJobs> ilistAscmDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(names_sql);
                        if (ilistAscmDiscreteJobs != null && ilistAscmDiscreteJobs.Count > 0)
                        {
                            listAscmDiscreteJobs = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDiscreteJobs>(ilistAscmDiscreteJobs);

                            setWipEntities(listAscmDiscreteJobs);
                            setWipRequirementOperations(listAscmDiscreteJobs);
                            setMaterialItem(listAscmDiscreteJobs);
                            setWarehouse(listAscmDiscreteJobs);
                        }
                    }
                }
                ///////////////////////////////////////////////////////////////////////////////////////
                foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                {
                    if (ascmGetMaterialTask.ascmDiscreteJobsList == null)
                        ascmGetMaterialTask.ascmDiscreteJobsList = new List<AscmDiscreteJobs>();
                    foreach (AscmDiscreteJobs ascmDiscreteJobs in listAscmDiscreteJobs)
                    {
                        foreach (AscmWipRequirementOperations ascmWipRequirementOperations in listAscmWipRequirementOperations)
                        {
                            if (ascmDiscreteJobs.ascmWipEntities.wipEntityId == ascmWipRequirementOperations.wipEntityId && ascmWipRequirementOperations.taskId == ascmGetMaterialTask.id)
                            {
                                if (containsJob(ascmGetMaterialTask.ascmDiscreteJobsList, ascmDiscreteJobs) == null)
                                {
                                    AscmDiscreteJobs ascm_DiscreteJobs = (AscmDiscreteJobs)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ascmDiscreteJobs), typeof(AscmDiscreteJobs));
                                    ascmGetMaterialTask.ascmDiscreteJobsList.Add(ascm_DiscreteJobs);
                                }
                            }
                        }
                    }
                }
                foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                {
                    if (ascmGetMaterialTask.ascmDiscreteJobsList == null)
                        ascmGetMaterialTask.ascmDiscreteJobsList = new List<AscmDiscreteJobs>();
                    foreach (AscmDiscreteJobs ascmDiscreteJobs in ascmGetMaterialTask.ascmDiscreteJobsList)
                    {
                        if (ascmDiscreteJobs.listAscmWipBom == null)
                            ascmDiscreteJobs.listAscmWipBom = new List<AscmWipRequirementOperations>();
                        foreach (AscmWipRequirementOperations ascmWipRequirementOperations in listAscmWipRequirementOperations)
                        {
                            if (ascmDiscreteJobs.ascmWipEntities.wipEntityId == ascmWipRequirementOperations.wipEntityId && ascmWipRequirementOperations.taskId == ascmGetMaterialTask.id)
                            {
                                ascmDiscreteJobs.listAscmWipBom.Add(ascmWipRequirementOperations);
                            }
                        }
                    }
                }
            }
        }
        [HttpPost]
        public ContentResult AllocateTaskList(int? page, int? rows, string sort, string order, string queryWord)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();

            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                string sql = " from AscmGetMaterialTask ";
                string where = "", whereQueryWord = "";
                whereQueryWord = "status = 'NOTALLOCATE'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                sql += " where " + where;
                IList<AscmGetMaterialTask> ilistTask = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                sql = " from AscmAllocateRule ";
                IList<AscmAllocateRule> ilistRule = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmAllocateRule>(sql);
                if (ilistTask != null && ilistTask.Count > 0)
                {
                    TaskAllocate(ilistTask, ilistRule,sql,where,whereQueryWord,userName);
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public void TaskAllocate(IList<AscmGetMaterialTask> ilistTask, IList<AscmAllocateRule> ilistRule, string sql, string where, string whereQueryWord, string userName)
        {
            foreach (AscmGetMaterialTask ascmGetMaterialTask in ilistTask)
            {
                #region 查找符合领料任务条件的领料员
                List<string> namelist = new List<string>();
                foreach (AscmAllocateRule ascmAllocateRule in ilistRule)
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
                    string ids = string.Empty, count = string.Empty, worker = string.Empty, logisticsClass = string.Empty;
                    where = "";
                    foreach (string str in namelist)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += str;
                    }
                    where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");
                    if (!string.IsNullOrEmpty(where))
                    {
                        sql = "from AscmAllocateRule";
                        if (!string.IsNullOrEmpty(where))
                            sql += " where " + where + "order by taskCount,id";
                        IList<AscmAllocateRule> ilistAllocateRule = AscmAllocateRuleService.GetInstance().GetList(sql, false, false, false);
                        if (ilistAllocateRule != null && ilistAllocateRule.Count > 0)
                        {
                            List<AscmAllocateRule> listAllocateRule = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmAllocateRule>(ilistAllocateRule);
                            for (int i = 0; i < ilistAllocateRule.Count; i++)
                            {
                                if (i == 0)
                                {
                                    ids = ilistAllocateRule[i].id.ToString();
                                    count = ilistAllocateRule[i].taskCount.ToString();
                                    worker = ilistAllocateRule[i].workerName.ToString();
                                    logisticsClass = ilistAllocateRule[i].logisticsClass.ToString();
                                }
                            }

                            ascmGetMaterialTask.workerId = worker;
                            ascmGetMaterialTask.status = "NOTEXECUTE";
                            ascmGetMaterialTask.logisticsClass = logisticsClass;
                            ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.modifyUser = userName;
                            int taskcount = int.Parse(count) + 1;
                            AscmAllocateRule ascmAllocateRule = AscmAllocateRuleService.GetInstance().Get(int.Parse(ids));
                            ascmAllocateRule.taskCount = taskcount;
                            ascmAllocateRule.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmAllocateRule.modifyUser = userName;
                            AscmAllocateRuleService.GetInstance().Update(ascmAllocateRule);
                            AscmGetMaterialTaskService.GetInstance().Update(ascmGetMaterialTask);
                        }
                    }
                }
                #endregion
            }
        }
        public ActionResult TaskEdit(int? id)
        {
            AscmGetMaterialTask ascmGetMaterialTask = null;
            try
            {
                if (id.HasValue)
                {
                    ascmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().Get(id.Value);
                    YnUser ascmWorker = YnUserService.GetInstance().Get(ascmGetMaterialTask.workerId);
                    ascmGetMaterialTask.ascmWorker = ascmWorker;
                    YnUser ascmRanker = YnUserService.GetInstance().Get(ascmGetMaterialTask.rankerId);
                    ascmGetMaterialTask.ascmRanker = ascmRanker;

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
        public ContentResult TaskDelete(string selectRow)
        {
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                AscmGetMaterialTask[] rows = JsonConvert.DeserializeObject<AscmGetMaterialTask[]>(selectRow);
                List<AscmGetMaterialTask> listTask = new List<AscmGetMaterialTask>();
                foreach (AscmGetMaterialTask ascmGetMaterialTask in rows)
                {
                    if (ascmGetMaterialTask.taskId.Substring(0, 1) != "T")
                    {
                        if (ascmGetMaterialTask.status == "NOTEXECUTE")
                        {
                            listTask.Add(ascmGetMaterialTask);
                        }
                        else
                        {
                            jsonObjectResult.result = false;
                            jsonObjectResult.message = "只能删除已分配或未分配的任务！";
                            string s_Return = JsonConvert.SerializeObject(jsonObjectResult);
                            return Content(s_Return);
                        }
                    }
                    else
                    {
                        jsonObjectResult.result = false;
                        jsonObjectResult.message = "只能删除手动任务";
                        string s_Return = JsonConvert.SerializeObject(jsonObjectResult);
                        return Content(s_Return);
                    }
                }
                if (listTask != null && listTask.Count > 0)
                {
                    string ids = string.Empty;
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in listTask)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        if (!string.IsNullOrEmpty(ascmGetMaterialTask.relatedMark))
                            ids += ascmGetMaterialTask.relatedMark;
                    }

                    string sql = "from AscmMarkTaskLog";
                    string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");
                    if (!string.IsNullOrEmpty(where))
                    {
                        sql += " where " + where;

                        IList<AscmMarkTaskLog> ilist = AscmMarkTaskLogService.GetInstance().GetList(sql, false, false);
                        if (ilist != null && ilist.Count > 0)
                        {
                            List<AscmMarkTaskLog> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMarkTaskLog>(ilist);
                            foreach (AscmMarkTaskLog ascmMarkTaskLog in list)
                            {
                                ascmMarkTaskLog.isMark = 1;
                                ascmMarkTaskLog.modifyUser = userName;
                                ascmMarkTaskLog.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            }
                            AscmMarkTaskLogService.GetInstance().Update(list);
                        }
                    }
                    AscmGetMaterialTaskService.GetInstance().Delete(listTask);
                }
                jsonObjectResult.result = true;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult AddTaskSave(AscmGetMaterialTask taskModel, string warehouserPlace)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                string userRole = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }
                List<YnRole> listYnRole = YnRoleService.GetInstance().GetListInUser(userName);
                int nI = 0;
                foreach (YnRole role in listYnRole)
                {
                    if (role.name == "领料员")
                    {
                        nI++;
                    }
                }
                if (nI != 0)
                {
                    userRole = userName;
                }

                if (string.IsNullOrEmpty(taskModel.tip))
                    throw new Exception("作业内容不能为空！");
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmGetMaterialTask where TaskId like '%L%' AND CREATETIME like '%" + date + "%'");
                taskModel.taskId = (maxId == 0) ? "L1001" : "L"+(int.Parse(AscmGetMaterialTaskService.GetInstance().Get(maxId).taskId.Substring(1, 4)) + 1).ToString();
                taskModel.id = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmGetMaterialTask ")+1;
                taskModel.createUser = userName;
                taskModel.modifyUser = userName;
                taskModel.createTime = datetime;
                taskModel.modifyTime = datetime;
                if (!string.IsNullOrEmpty(userRole))
                {
                    taskModel.workerId = userRole;
                    taskModel.status = AscmGetMaterialTask.StatusDefine.notExecute;
                }
                else
                {
                    taskModel.status = AscmGetMaterialTask.StatusDefine.notAllocate;
                }
                taskModel.materialType = 1;
                taskModel.uploadDate = date;

                if (!string.IsNullOrEmpty(taskModel.relatedMarkId.ToString()) && taskModel.relatedMarkId > 0)
                {
                    AscmMarkTaskLog ascmMarkTaskLog = AscmMarkTaskLogService.GetInstance().Get(taskModel.relatedMarkId);
                    if (ascmMarkTaskLog != null)
                    {
                        ascmMarkTaskLog.isMark = 0;
                        ascmMarkTaskLog.modifyUser = userName;
                        ascmMarkTaskLog.modifyTime = datetime;

                        AscmMarkTaskLogService.GetInstance().Update(ascmMarkTaskLog);
                    }
                    taskModel.relatedMark += taskModel.relatedMarkId.ToString();
                }

                AscmGetMaterialTaskService.GetInstance().Save(taskModel);
                jsonObjectResult.result = true;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult EditTaskSave(AscmGetMaterialTask ascmGetMaterialTask_Model, int? id)
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
                    AscmGetMaterialTask ascmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().Get(ascmGetMaterialTask_Model.id);
                    List<AscmAllocateRule> list = new List<AscmAllocateRule>();
                    if (ascmGetMaterialTask.taskId.Substring(0, 1) == AscmCommonHelperService.GetInstance().GetConfigTaskWords(0))
                    {
                        List<AscmAllocateRule> listMinus = AscmAllocateRuleService.GetInstance().GetList("from AscmAllocateRule where workerName = '" + ascmGetMaterialTask.workerId + "'", false, false, false);
                        if (listMinus != null && listMinus.Count > 0)
                        {
                            foreach (AscmAllocateRule ascmAllocateRule in listMinus)
                            {
                                ascmAllocateRule.taskCount--;
                                ascmAllocateRule.modifyUser = userName;
                                ascmAllocateRule.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                list.Add(ascmAllocateRule);
                            }
                        }
                        List<AscmAllocateRule> listPlus = AscmAllocateRuleService.GetInstance().GetList("from AscmAllocateRule where workerName = '" + ascmGetMaterialTask_Model.WorkerName + "'", false, false, false);
                        if (listPlus != null && listPlus.Count > 0)
                        {
                            foreach (AscmAllocateRule ascmAllocateRule in listPlus)
                            {
                                ascmAllocateRule.taskCount++;
                                ascmAllocateRule.modifyUser = userName;
                                ascmAllocateRule.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                list.Add(ascmAllocateRule);
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
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public string GetYnUserId(string username)
        {
            string UserId = string.Empty;
            try
            {
                string sql = "from YnUser";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(username))
                    whereQueryWord = "userName like '" + username + "'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<YnUser> ilist = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.Find<YnUser>(sql);
                if (ilist != null && ilist.Count > 0)
                {
                    List<YnUser> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnUser>(ilist);
                    UserId = list[0].userId.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return UserId;
        }
        public ActionResult WarehouserIdSelectComboAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber

            List<AscmWarehouse> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmWarehouseService.GetInstance().GetList(ynPage, "", "", q, null);
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
        public ActionResult MaterialDocNumberSelectComboAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber

            List<AscmMaterialItem> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmMaterialItemService.GetInstance().GetList(ynPage, "", "", q, "", "",null);
                if (list != null)
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
        public ActionResult MarkIdSelectComboAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber

            string userName = string.Empty;
            string userRole = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }
            List<YnRole> listYnRole = YnRoleService.GetInstance().GetListInUser(userName);
            int nI = 0;
            foreach (YnRole role in listYnRole)
            {
                if (role.name == "领料员")
                {
                    nI++;
                }
            }
            if (nI > 0)
            {
                userRole = userName;
            }

            List<AscmMarkTaskLog> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();

            try
            {
                string sql = "from AscmWipEntities";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(q))
                {
                    whereQueryWord = "name like '" + q.Trim() + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    IList<AscmWipEntities> ilistWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                    string ids = string.Empty;
                    if (ilistWipEntities != null && ilistWipEntities.Count > 0)
                    {
                        List<AscmWipEntities> listWipEntities = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilistWipEntities);
                        foreach (AscmWipEntities ascmWipEntities in listWipEntities)
                        {
                            if (!string.IsNullOrEmpty(ids))
                                ids += ",";
                            ids += ascmWipEntities.wipEntityId;
                        }
                    }
                    where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "wipEntityId");
                }

                sql = "from AscmMarkTaskLog";
                if (!string.IsNullOrEmpty(userRole))
                {
                    whereQueryWord = "createUser like '" + userRole + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                whereQueryWord = "markType = 'NONAUTO'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                whereQueryWord = "isMark = 1";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmMarkTaskLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMarkTaskLog>(sql,sql,ynPage);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMarkTaskLog>(ilist);
                    AscmMarkTaskLogService.GetInstance().SetGetMaterialTask(list);
                    AscmMarkTaskLogService.GetInstance().SetWipEntities(list);
                    foreach (AscmMarkTaskLog ascmMarkTaskLog in list)
                    {
                        jsonDataGridResult.rows.Add(ascmMarkTaskLog);
                    }
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 领料作业监控管理
        public ActionResult GetMaterialMonitorIndex()
        {
            //领料作业监控
            return View();
        }
        public ActionResult MonitorTaskList(int? page, int? rows, string sort, string order, string queryWord, string queryStatus, string queryLine, string queryType, string queryStartDate, string queryEndDate, string taskString, string queryWarehouse, string queryFormat, string queryPerson, string queryStartJobDate, string queryEndJobDate, string queryWipEntity)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmGetMaterialTask> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }
            
            try
            {
                list = AscmGetMaterialTaskService.GetInstance().GetMonitorTaskList(ynPage, "", "", queryWord, userName, queryStatus, queryLine, queryType, queryStartDate, queryEndDate, taskString, queryWarehouse, queryFormat, queryPerson, queryStartJobDate, queryEndJobDate, queryWipEntity);
                if (list != null && list.Count > 0)
                {
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
                jsonDataGridResult.message = ex.Message;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MonitorJobList(int? id)
        {
            List<AscmWipDiscreteJobs> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string sql = "select distinct wipentityId,sum(requiredQuantity),sum(getMaterialQuantity),sum(wmsPreparationQuantity) from ascm_wip_require_operat";
                string where = "", whereQueryWord = "";
                if (id != null && id > 0)
                    whereQueryWord = "taskId = " + id;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                whereQueryWord = "group by wipentityId";
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where + whereQueryWord;
                IList ilist = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);

                sql = "from AscmWipDiscreteJobs ";
                where = "";
                string ids = string.Empty;
                foreach (object[] obj in ilist)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += obj[0].ToString();
                }

                if (!string.IsNullOrEmpty(ids))
                    whereQueryWord = "id in (" + ids + ")";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where,whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmWipDiscreteJobs> ilistDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sql);
                if (ilistDiscreteJobs != null && ilistDiscreteJobs.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobs>(ilistDiscreteJobs);
                    AscmWipDiscreteJobsService.GetInstance().SetWipEntities(list);
                    AscmWipDiscreteJobsService.GetInstance().SetMaterialItem(list);
                    AscmWipDiscreteJobsService.GetInstance().SetDiscreteJobs(list);
                    AscmWipDiscreteJobsService.GetInstance().SetMarkTaskLog(id, list);
                    SetTotalSum(ilist, list);
                    list = list.OrderBy(e => e.ascmDiscreteJobs_line).ToList<AscmWipDiscreteJobs>();
                        
                    foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                    {
                        jsonDataGridResult.rows.Add(ascmWipDiscreteJobs);
                    }
                }
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                jsonDataGridResult.message = ex.Message;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadJobBom(int? page, int? rows, string sort, string order, int? taskId, int? jobId, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmWipRequirementOperations> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string sql = " from AscmWipRequirementOperations ";
                string where = "", whereQueryWord = "";
                if (taskId != null && taskId > 0)
                    whereQueryWord = "taskid = " + taskId;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (jobId != null && jobId > 0)
                    whereQueryWord = "wipentityid = " + jobId;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmWipRequirementOperations> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql, sql, ynPage);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilist);
                    AscmWipRequirementOperationsService.GetInstance().SetMaterial(list);
                    AscmWipRequirementOperationsService.GetInstance().SetWipEntities(list);
                    AscmWipRequirementOperationsService.GetInstance().SetOnhandQuantity(list);
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
                jsonDataGridResult.message = ex.Message;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public void SetTotalSum(IList ilist, List<AscmWipDiscreteJobs> list)
        {
            foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
            {
                foreach (object[] obj in ilist)
                {
                    if (obj[0].ToString() == ascmWipDiscreteJobs.wipEntityId.ToString())
                    {
                        ascmWipDiscreteJobs.totalRequiredQuantity = Convert.ToDecimal(obj[1].ToString());
                        ascmWipDiscreteJobs.totalGetMaterialQuantity = Convert.ToDecimal(obj[2].ToString());
                        ascmWipDiscreteJobs.totalPreparationQuantity = Convert.ToDecimal(obj[3].ToString());
                    }
                }
            }
        }
        public ActionResult StartExcuteTask(string releaseHeaderIds)
        {
            List<AscmGetMaterialTask> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                string sql = " from AscmGetMaterialTask ";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(releaseHeaderIds))
                    whereQueryWord = "id in (" + releaseHeaderIds + ")";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                    {
                        if (ascmGetMaterialTask.status == "NOTEXECUTE")
                        {
                            ascmGetMaterialTask.status = "EXECUTE";
                            ascmGetMaterialTask.starTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.modifyUser = userName;
                            AscmGetMaterialTaskService.GetInstance().Update(ascmGetMaterialTask);
                        }
                    }
                }
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                jsonDataGridResult.message = ex.Message;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EndExcuteTask(string releaseHeaderIds)
        {
            List<AscmGetMaterialTask> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();

            string userName = string.Empty;
            string userRole = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }
            userRole = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName);

            try
            {
                string sql = " from AscmGetMaterialTask ";
                string where = "", whereQueryWord = "";
                whereQueryWord = " status = 'EXECUTE'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(userRole))
                {
                    whereQueryWord = "workerId = '" + userName + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                if (!string.IsNullOrEmpty(releaseHeaderIds))
                {
                    whereQueryWord = "id in (" + releaseHeaderIds + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                    AscmGetMaterialTaskService.GetInstance().SumQuantity(list);
                    List<AscmGetMaterialTask> newlist = new List<AscmGetMaterialTask>();
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                    {
                        string warehousePlace = string.Empty;
                        if (!string.IsNullOrEmpty(ascmGetMaterialTask.warehouserId))
                        {
                            warehousePlace = ascmGetMaterialTask.warehouserId.Substring(0, 4).ToString();
                        }
                        string taskWord = ascmGetMaterialTask.taskId.Substring(0, 1).ToString();
                        if (taskWord == "T")
                        {
                            if (warehousePlace != "B631" && warehousePlace != "B632" && warehousePlace != "E222" && warehousePlace != "E221")
                            { 
                                if (ascmGetMaterialTask.totalGetMaterialQuantity == ascmGetMaterialTask.totalRequiredQuantity)
                                {
                                    ascmGetMaterialTask.status = "FINISH";
                                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.endTime))
                                        ascmGetMaterialTask.errorTime += ",";
                                    ascmGetMaterialTask.errorTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                    ascmGetMaterialTask.modifyUser = userName;
                                    ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                    newlist.Add(ascmGetMaterialTask);
                                }
                            }
                            else
                            {
                                ascmGetMaterialTask.status = "FINISH";
                                ascmGetMaterialTask.endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                ascmGetMaterialTask.modifyUser = userName;
                                ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                newlist.Add(ascmGetMaterialTask);
                            }
                        }
                        else if (taskWord == "L")
                        {
                            ascmGetMaterialTask.status = "FINISH";
                            ascmGetMaterialTask.endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.modifyUser = userName;
                            ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            newlist.Add(ascmGetMaterialTask);
                        }
                    }
                    if (newlist != null && newlist.Count > 0)
                    {
                        AscmGetMaterialTaskService.GetInstance().Update(newlist);
                    }
                }
            }
            catch (Exception ex)
            {
                jsonDataGridResult.message = ex.Message;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfrimedGetMaterial(int? page, int? rows, string sort, string order, string queryWord, string releaseHeaderIds)
        {
            List<AscmGetMaterialTask> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();

            string userName = string.Empty;
            string userRole = string.Empty;
            bool Flag = false;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }
            userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);

            try
            {
                //验证任务是否在执行中
                string sql = "from AscmGetMaterialTask";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(releaseHeaderIds))
                    whereQueryWord = "id in (" + releaseHeaderIds + ")";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmGetMaterialTask> ilistGetMaterialTask = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                if (ilistGetMaterialTask.Count == 0 || ilistGetMaterialTask == null)
                    throw new Exception("查询异常！");
                int iTaskCount = 0;
                foreach (AscmGetMaterialTask ascmGetMaterialTask in ilistGetMaterialTask)
                {
                    if (ascmGetMaterialTask.status != "EXECUTE")
                    {
                        iTaskCount++;
                    }
                }
                if (iTaskCount > 0)
                    throw new Exception("所选包含未在执行中任务！");
                
                //更新领料数量
                sql = "from AscmWipRequirementOperations";
                where = "";
                if (!string.IsNullOrEmpty(releaseHeaderIds))
                    whereQueryWord = "taskId in (" + releaseHeaderIds + ")";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmWipRequirementOperations> ilistRequirementOperations = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql);
                if (ilistRequirementOperations.Count > 0 && ilistRequirementOperations != null)
                {
                    List<AscmWipRequirementOperations> listRequirementOperations = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilistRequirementOperations);
                    List<AscmWipRequirementOperations> newlist = new List<AscmWipRequirementOperations>();
                    List<WmsAndLogistics> listWmsAndLogistics = new List<WmsAndLogistics>();
                    foreach (AscmWipRequirementOperations ascmWipRequirementOperations in listRequirementOperations)
                    {
                        if (ascmWipRequirementOperations.wmsPreparationQuantity > 0)
                        {
                            WmsAndLogistics wmsAndLogistics = new WmsAndLogistics();
                            wmsAndLogistics.wipEntityId = ascmWipRequirementOperations.wipEntityId;
                            wmsAndLogistics.materialId = ascmWipRequirementOperations.inventoryItemId;
                            wmsAndLogistics.quantity = ascmWipRequirementOperations.wmsPreparationQuantity;
                            if (!string.IsNullOrEmpty(ascmWipRequirementOperations.wmsPreparationString))
                            {
                                if (ascmWipRequirementOperations.wmsPreparationString.IndexOf(',') > -1)
                                {
                                    string[] strarr = ascmWipRequirementOperations.wmsPreparationString.Split(',');
                                    foreach (string str in strarr)
                                    {
                                        if (!string.IsNullOrEmpty(str))
                                        {
                                            string temp = str.Substring(0, str.IndexOf('['));
                                            if (!string.IsNullOrEmpty(wmsAndLogistics.preparationString))
                                                wmsAndLogistics.preparationString += ",";
                                            wmsAndLogistics.preparationString += temp;
                                        }
                                    }
                                }
                                else
                                {
                                    string temp = ascmWipRequirementOperations.wmsPreparationString.Substring(0, ascmWipRequirementOperations.wmsPreparationString.IndexOf('['));
                                    wmsAndLogistics.preparationString += temp;
                                }
                            }
                            wmsAndLogistics.workerId = userName;
                            listWmsAndLogistics.Add(wmsAndLogistics);
                            
                            ascmWipRequirementOperations.getMaterialQuantity += ascmWipRequirementOperations.wmsPreparationQuantity;
                            if (!string.IsNullOrEmpty(ascmWipRequirementOperations.getMaterialString))
                                ascmWipRequirementOperations.getMaterialString += ",";
                            ascmWipRequirementOperations.getMaterialString += ascmWipRequirementOperations.wmsPreparationQuantity.ToString();
                            ascmWipRequirementOperations.wmsPreparationQuantity = 0;
                            ascmWipRequirementOperations.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmWipRequirementOperations.modifyUser = userName;
                            newlist.Add(ascmWipRequirementOperations);
                        }
                    }
                    if (newlist.Count > 0 && listWmsAndLogistics.Count > 0)
                    {
                        WmsAndLogisticsService.GetInstance().DoMaterialRequisition(listWmsAndLogistics);
                        AscmWipRequirementOperationsService.GetInstance().Update(newlist);
                        Flag = true;
                    }
                }

                //记录领料异常时间点
                if (Flag)
                {
                    sql = "from AscmGetMaterialTask";
                    where = "";
                    if (!string.IsNullOrEmpty(releaseHeaderIds))
                        whereQueryWord = "id in (" + releaseHeaderIds + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    if (!string.IsNullOrEmpty(where))
                        sql += " where " + where;
                    IList<AscmGetMaterialTask> ilistAscmGetMaterialTask = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                    if (ilistAscmGetMaterialTask != null && ilistAscmGetMaterialTask.Count > 0)
                    {
                        List<AscmGetMaterialTask> listAscmGetMaterialTask = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilistAscmGetMaterialTask);
                        foreach (AscmGetMaterialTask ascmGetMaterialTask in listAscmGetMaterialTask)
                        {
                            if (!string.IsNullOrEmpty(ascmGetMaterialTask.errorTime))
                                ascmGetMaterialTask.errorTime += ",";
                            if (!string.IsNullOrEmpty(ascmGetMaterialTask.starTime))
                                ascmGetMaterialTask.endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.errorTime += DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.modifyUser = userName;
                        }
                        AscmGetMaterialTaskService.GetInstance().Update(listAscmGetMaterialTask);
                    }
                }

                //刷新列表
                YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
                ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

                sql = "from AscmGetMaterialTask";
                where = "";
                whereQueryWord = "status != 'NOTALLOCATE'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(userRole))
                {
                    whereQueryWord = "workerId = '" + userRole + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql, sql, ynPage);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                    AscmGetMaterialTaskService.GetInstance().SetRanker(list);
                    AscmGetMaterialTaskService.GetInstance().SetWorker(list);
                    AscmGetMaterialTaskService.GetInstance().SumQuantity(list);
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                    {
                        jsonDataGridResult.rows.Add(ascmGetMaterialTask);
                    }
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.total = ynPage.GetRecordCount();
                if (Flag)
                {
                    jsonDataGridResult.message = "领料数据已提交！";
                }
            }
            catch (Exception ex)
            {
                jsonDataGridResult.message = ex.Message;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult ConfrimedBatchGetMaterial(int? page, int? rows, string sort, string order, string queryWord, string releaseHeaderIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();

            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                bool result = AscmGetMaterialTaskService.GetInstance().BatchGetMaterialTask(userName, releaseHeaderIds);

                jsonObjectResult.result = result;
                if (result)
                    jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
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
                jsonObjectResult.result = true;
                jsonObjectResult.message = AscmGetMaterialTaskService.GetInstance().GetNotifierMessageList(userName);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error(ex.Message, ex);
                throw ex;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult MarkTask(int? id, int? wipEntityId)
        {
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string warehouse = string.Empty;
                if (id.HasValue)
                {
                    AscmGetMaterialTask ascmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().Get(id.Value);
                    if (ascmGetMaterialTask == null)
                        throw new Exception("该任务不存在！");
                    warehouse = string.IsNullOrEmpty(ascmGetMaterialTask.warehouserId) ? null : ascmGetMaterialTask.warehouserId.Substring(0, 4).ToUpper().ToString();
                }

                if (AscmCommonHelperService.GetInstance().IsJudgeSpecWareHouse(warehouse))
                {
                    AscmMarkTaskLog ascmMarkTaskLog = null;
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

                        AscmGetMaterialTask ascmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().Get(id.Value);
                        if (!string.IsNullOrEmpty(ascmGetMaterialTask.relatedMark))
                            ascmGetMaterialTask.relatedMark += ",";
                        ascmGetMaterialTask.relatedMark += ascmMarkTaskLog.id.ToString();
                        ascmGetMaterialTask.modifyUser = userName;
                        ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        AscmGetMaterialTaskService.GetInstance().Update(ascmGetMaterialTask);

                        AscmMarkTaskLogService.GetInstance().Save(ascmMarkTaskLog);
                        
                        jsonObjectResult.result = true;
                        jsonObjectResult.message = "";
                    }
                    else
                    {
                        jsonObjectResult.result = false;
                        jsonObjectResult.message = "该标记不存在！";
                    }
                }
                else
                {
                    jsonObjectResult.result = false;
                    jsonObjectResult.message = "非特殊子库违法标记失败！";
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult UnMarkTask(int? id, int? wipEntityId)
        {
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            List<AscmMarkTaskLog> list = null;
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

                    string sql = "from AscmMarkTaskLog";
                    string where = "", whereQueryWord = "";
                    whereQueryWord = "taskId = " + id.Value.ToString();
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    whereQueryWord = "wipEntityId = " + wipEntityId.Value.ToString();
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    whereQueryWord = "isMark = 1";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    whereQueryWord = "markType = 'NONAUTO'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    if (!string.IsNullOrEmpty(where))
                        sql += " where " + where;
                    IList<AscmMarkTaskLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMarkTaskLog>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMarkTaskLog>(ilist);
                        List<AscmGetMaterialTask> listAscmGetMaterialTask = new List<AscmGetMaterialTask>();
                        int nI = 0;
                        foreach (AscmMarkTaskLog ascmMarkTaskLog in list)
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
                            listAscmGetMaterialTask.Add(ascmGetMaterialTask);

                            nI++;
                        }
                        if (nI > 0)
                        {
                            AscmGetMaterialTaskService.GetInstance().Update(listAscmGetMaterialTask);
                            AscmMarkTaskLogService.GetInstance().Update(list);
                            jsonObjectResult.result = true;
                            jsonObjectResult.message = "";
                        }
                        else
                        {
                            jsonObjectResult.result = false;
                            jsonObjectResult.message = "取消标记失败！";
                        }
                    }
                    else
                    {
                        jsonObjectResult.result = false;
                        jsonObjectResult.message = "该标记不存在！";
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
        [HttpPost]
        public ContentResult CloseGetMaterialTask(string releaseHeaderIds)
        {
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            List<AscmGetMaterialTask> list = null;
            try
            {
                string sql = " from AscmGetMaterialTask ";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(releaseHeaderIds))
                    whereQueryWord = "id in (" + releaseHeaderIds + ")";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                bool flag = false;
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                    int nI = 0;
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                    {
                        ascmGetMaterialTask.status = AscmGetMaterialTask.StatusDefine.close;
                        ascmGetMaterialTask.modifyUser = userName;
                        ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        nI++;
                    }
                    if (nI > 0)
                    {
                        AscmGetMaterialTaskService.GetInstance().Save(list);
                        flag = true;
                    }
                }
                if (flag)
                {
                    jsonObjectResult.result = true;
                    jsonObjectResult.message = "";
                }
                else
                {
                    jsonObjectResult.result = false;
                    jsonObjectResult.message = "error!";
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

        #region 未完成作业统计
        public ActionResult NotFinalDiscreteJobsIndex()
        { 
            //未完成作业统计
            return View();
        }
        public ActionResult NotFinalDiscreteJobsList(int? page, int? rows, string sort, string order, string queryWord, string queryStartDate, string queryEndDate, string queryDocnumber)
        { 
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDiscreteJobs> list = null;
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
                string startDate = string.Empty;
                if (!string.IsNullOrEmpty(queryStartDate))
                {
                    startDate = Convert.ToDateTime(queryStartDate).ToString("yyyy-MM-dd") + " 23:59";
                }
                else
                {
                    startDate = DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd") + " 23:59";
                }

                string endDate = string.Empty;
                if (!string.IsNullOrEmpty(queryEndDate))
                {
                    endDate = Convert.ToDateTime(queryEndDate).ToString("yyyy-MM-dd") + " 23:59";
                }
                else
                {
                    endDate = DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59";
                }

                string sql = "from AscmGetMaterialTask";
                string where = "", whereQueryWord = "";
                whereQueryWord = "createTime > '" + startDate + "'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                whereQueryWord = "createTime <= '" + endDate + "'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                whereQueryWord = "status != 'FINISH'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(userLogistisClass))
                {
                    whereQueryWord = "logisticsClass in (" + userLogistisClass + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else if (string.IsNullOrEmpty(userLogistisClass) && userRole == "领料员")
                {
                    whereQueryWord = "workerId = '" + userName + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmGetMaterialTask> ilistAscmGetMaterialTask = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                if (ilistAscmGetMaterialTask != null && ilistAscmGetMaterialTask.Count > 0)
                {
                    string ids = string.Empty;
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in ilistAscmGetMaterialTask)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += ascmGetMaterialTask.id;
                    }

                    sql = "select wipEntityId from Ascm_Wip_Require_Operat";
                    where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "taskId");

                    whereQueryWord = "taskId > 0";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    if (!string.IsNullOrEmpty(where))
                    {
                        sql += " where " + where + " group by wipEntityId";

                        IList ilistWipEntityId = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);
                        if (ilistWipEntityId != null && ilistWipEntityId.Count > 0)
                        {
                            string idsWipEntityId = string.Empty;
                            foreach (object[] obj in ilistWipEntityId)
                            {
                                if (!string.IsNullOrEmpty(idsWipEntityId))
                                    idsWipEntityId += ",";
                                idsWipEntityId += obj[0].ToString();
                            }

                            sql = "from AscmWipEntities";
                            where = AscmCommonHelperService.GetInstance().IsJudgeListCount(idsWipEntityId, "wipEntityId");

                            if (!string.IsNullOrEmpty(where))
                                sql += " where " + where;

                            IList<AscmWipEntities> ilistAscmWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                            if (ilistAscmWipEntities != null && ilistAscmWipEntities.Count > 0)
                            {
                                string idsWipentitiesName = string.Empty;
                                foreach (AscmWipEntities ascmWipEntities in ilistAscmWipEntities)
                                {
                                    if (!string.IsNullOrEmpty(idsWipentitiesName))
                                        idsWipentitiesName += ",";
                                    idsWipentitiesName += "'" + ascmWipEntities.name + "'";
                                }

                                sql = "from AscmDiscreteJobs";
                                if (!string.IsNullOrEmpty(queryDocnumber))
                                {
                                    whereQueryWord = "jobId like '%" + queryDocnumber + "%'";
                                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                                }
                                else
                                {
                                    where = AscmCommonHelperService.GetInstance().IsJudgeListCount(idsWipentitiesName, "jobId");
                                }

                                if (!string.IsNullOrEmpty(where))
                                    sql += " where " + where + "order by jobDate,identificationId,workerId";

                                IList<AscmDiscreteJobs> ilistAscmDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(sql, sql, ynPage);
                                if (ilistAscmDiscreteJobs != null && ilistAscmDiscreteJobs.Count > 0)
                                {
                                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDiscreteJobs>(ilistAscmDiscreteJobs);
                                    setWipEntities(list);
                                    AscmDiscreteJobsService.GetInstance().SetRanker(list);
                                    foreach (AscmDiscreteJobs ascmDiscreteJobs in list)
                                    {
                                        jsonDataGridResult.rows.Add(ascmDiscreteJobs);
                                    }
                                }
                                jsonDataGridResult.total = ynPage.GetRecordCount();
                                jsonDataGridResult.result = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error(ex.Message,ex);
                throw ex;
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
                string sql = "from AscmWipRequirementOperations";
                string where = "", whereQueryWord = "";

                whereQueryWord = " wipentityid = " + id.ToString();
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                whereQueryWord = " wipSupplyType = 1 ";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                whereQueryWord = " (requiredQuantity - getMaterialQuantity) > 0";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                whereQueryWord = "taskId > 0";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmWipRequirementOperations> ilistOperations = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql,sql,ynPage);
                if (ilistOperations != null && ilistOperations.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipRequirementOperations>(ilistOperations);
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

        #region  仓库物料查询
        public ActionResult WareHouseMaterialIndex()
        {
            //仓库物料查询
            return View();
        }
        public ActionResult WareHouseMaterialList(int? page, int? rows, string sort, string order, string queryWord, string startTime, string endTime)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            List<AscmMaterialItem> list = null;
            List<AscmMaterialItem> newlist = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                int count = 0;
                if (!string.IsNullOrEmpty(queryWord))
                {
                    //调用仓库统计
                    int nI = Convert.ToInt32(queryWord);
                    newlist = AscmMaterialItemService.GetInstance().GetWarehouseMaterialSumList(nI);
                    count = newlist.Count;
                    if (newlist != null && newlist.Count > 0)
                    {
                        foreach (AscmMaterialItem ascmMaterialItem in newlist)
                        {
                            jsonDataGridResult.rows.Add(ascmMaterialItem);
                        }
                    }
                    else
                    {
                        foreach (AscmMaterialItem ascmMaterialItem in list)
                        {
                            jsonDataGridResult.rows.Add(ascmMaterialItem);
                        }
                    }
                }
                jsonDataGridResult.total = count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MaterialOfDiscreteJobList(int? page, int? rows, string sort, string order, string queryWord, int? materialId, string startTime, string endTime)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmWipDiscreteJobs> list = null;
            try
            {
                if (string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
                    throw new Exception("需求起止时间不能为空！");

                startTime = startTime + " 00:00:00";
                endTime = endTime + " 23:59:59";
                string sql = "select wipEntityId from AscmWipRequirementOperations";
                string where = "", whereQueryWord = "";
                if (materialId != null && materialId != 0)
                {
                    whereQueryWord = "inventoryItemId = " + materialId;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    whereQueryWord = "mpsDateRequired > '" + startTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    whereQueryWord = "mpsDateRequired <= '" + endTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    if (!string.IsNullOrEmpty(where))
                        sql += " where " + where;
                    string sSql = "from AscmWipDiscreteJobs where wipEntityId in (" + sql + ")";
                    IList<AscmWipDiscreteJobs> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sSql,sSql,ynPage);
                    if (ilist != null && ilist.Count > 0)
                    {
                        list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobs>(ilist);
                        AscmWipDiscreteJobsService.GetInstance().SetWipEntities(list);
                        foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                        {
                            jsonDataGridResult.rows.Add(ascmWipDiscreteJobs);
                        }
                    }
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch(Exception ex)
            {
                jsonDataGridResult.message = ex.Message;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  任务量统计
        public ActionResult TaskStatistics()
        {
            //任务量统计
            return View();
        }
        public ActionResult TaskStatisticsList(string queryWord, string strSatTime, string strEndTime)
        {
            List<AscmMonitoringService.Monitoring> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmMonitoringService.GetInstance().GetListForMonitoring(queryWord, strSatTime, strEndTime);
                if (list != null)
                {
                    foreach (AscmMonitoringService.Monitoring ascmDiscreteJobs in list)
                    {
                        jsonDataGridResult.rows.Add(ascmDiscreteJobs);
                    }
                }
                //jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TaskDetailStatisticsList(string workerId, string userName)
        {
            List<Dal.GetMaterialManage.Entities.AscmGetMaterialTask> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmMonitoringService.GetInstance().TaskDetailStatisticsList(workerId);
                if (list != null)
                {
                    foreach (AscmGetMaterialTask ascmDiscreteJobs in list)
                    {
                        DateTime dateStaTime = DateTime.Parse(ascmDiscreteJobs.starTime);
                        DateTime dateEndTime = DateTime.Parse(ascmDiscreteJobs.endTime);
                        ascmDiscreteJobs.strUsedTime = dateEndTime.Subtract(dateStaTime).TotalMinutes.ToString() + " 分钟";
                        ascmDiscreteJobs.strUserName = userName;
                        jsonDataGridResult.rows.Add(ascmDiscreteJobs);
                    }
                }
                //jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 工作量统计
        public ActionResult WorkloadStatisticsIndex()
        { 
            //工作量统计
            return View();
        }
        public ActionResult WorkloadStatisticsList(string starTime, string endTime)
        {
            List<WorkloadStatistics> list = new List<WorkloadStatistics>(); ;
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

                if (!string.IsNullOrEmpty(starTime))
                {
                    string sTime = starTime + " 23:59";
                    whereQueryWord = "starTime > '" + sTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else
                {
                    string sTime = DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd") + " 23:59";
                    whereQueryWord = "starTime > '" + sTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(endTime))
                {
                    string eTime = endTime + " 23:59";
                    whereQueryWord = "endTime < '" + eTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else
                {
                    string eTime = DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59";
                    whereQueryWord = "endTime <= '" + eTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where + groupString + orderString;
                IList ilist = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);
                if (ilist.Count > 0 && ilist != null)
                {
                    foreach (object[] obj in ilist)
                    {
                        WorkloadStatistics workloadStatistics = new WorkloadStatistics();
                        workloadStatistics.workerId = obj[0].ToString();
                        workloadStatistics.taskCount = obj[1].ToString();
                        workloadStatistics.avgTime = obj[2].ToString();
                        list.Add(workloadStatistics);
                    }
                }
                if (list.Count > 0 && list != null)
                {
                    setWorkloadStatistics(list);
                    foreach (WorkloadStatistics workloadStatistics in list)
                    {
                        jsonDataGridResult.rows.Add(workloadStatistics);
                    }
                }
                jsonDataGridResult.result = true;
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error(ex.Message,ex);
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadDetailStatistics(int? page, int? rows, string sort, string order, string workerId, string starTime, string endTime, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;
            
            List<AscmGetMaterialTask> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string sql = "from AscmGetMaterialTask";
                string where = "", whereQueryWord = "";
                whereQueryWord = "status = 'FINISH'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where,whereQueryWord);
                if (!string.IsNullOrEmpty(workerId))
                {
                    whereQueryWord = "workerId = '" + workerId + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(starTime) && starTime != "null")
                {
                    string sTime = starTime + " 23:59";
                    whereQueryWord = "starTime > '" + sTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else
                {
                    string sTime = DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd") + " 23:59";
                    whereQueryWord = "starTime > '" + sTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(endTime) && endTime != "null")
                {
                    string eTime = endTime + " 23:59";
                    whereQueryWord = "endTime <= '" + eTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else
                {
                    string eTime = DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59";
                    whereQueryWord = "endTime < '" + eTime + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where + " order by createTime";
                IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql,sql,ynPage);
                if (ilist.Count > 0 && ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                    setUsedTime(list);
                    AscmGetMaterialTaskService.GetInstance().SetRanker(list);
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                    {
                        jsonDataGridResult.rows.Add(ascmGetMaterialTask);
                    }
                }
                jsonDataGridResult.result = true;
                jsonDataGridResult.total = ynPage.GetCurrentPage();
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error(ex.Message, ex);
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public void setWorkloadStatistics(List<WorkloadStatistics> list)
        {
            if (list.Count > 0 && list != null)
            {
                string ids = string.Empty;
                foreach (WorkloadStatistics workloadStatistics in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    if (!string.IsNullOrEmpty(workloadStatistics.workerId))
                        ids += "'" + workloadStatistics.workerId + "'";
                }
                if (!string.IsNullOrEmpty(ids))
                {
                    string sql = "from YnUser where userId in (" + ids + ")";
                    IList<YnUser> ilistUser = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.Find<YnUser>(sql);
                    if (ilistUser.Count > 0 && ilistUser != null)
                    {
                        List<YnUser> listUser = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnUser>(ilistUser);
                        foreach (WorkloadStatistics workloadSttistics in list)
                        {
                            workloadSttistics.ynUser = listUser.Find(e => e.userId == workloadSttistics.workerId);
                        }
                    }
                }
            }
        }
        public void setUsedTime(List<AscmGetMaterialTask> list)
        {
            if (list.Count > 0 && list != null)
            {
                foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                {
                    DateTime starTime = DateTime.Parse(ascmGetMaterialTask.starTime);
                    DateTime endTime = DateTime.Parse(ascmGetMaterialTask.endTime);
                    ascmGetMaterialTask.strUsedTime = endTime.Subtract(starTime).TotalMinutes.ToString() + " 分钟";
                }
            }
        }
        #endregion

        #region  自动生成计划分配任务
        public ActionResult AutoCreateAndAllocateTaskIndex()
        {
            //自动生成计划分配任务
            return View();
        }
        public ActionResult AutoTaskList(int? page, int? rows, string sort, string order, string queryWord, string queryDate, string queryType)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            string userName = string.Empty;
            string userRole = string.Empty;
            string userLogistisClass = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }
            userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
            userLogistisClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(userName, userRole);

            List<AscmGetMaterialTask> list = new List<AscmGetMaterialTask>();
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            string sql = string.Empty;
            string where = "", whereQueryWord = "";
            object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmDiscreteJobs where status = 1 and time like '" + queryDate + "%' and workerId = '" + userName + "'");
            if (obj == null)
                throw new Exception("查询失败！");
            int iCount = 0;
            if (int.TryParse(obj.ToString(), out iCount) && iCount == 0)
            {
                if (queryType == "bool")
                {
                    queryType = "false";
                }
            }
            try
            {
                if (queryType == "bool")
                {
                    if (!string.IsNullOrEmpty(queryDate))
                    {
                        sql = "from AscmDiscreteJobs";
                        if (!string.IsNullOrEmpty(queryDate))
                        {
                            whereQueryWord = "time like '" + queryDate + "%'";
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        }

                        if (!string.IsNullOrEmpty(userName))
                        {
                            whereQueryWord = "workerId = '" + userName + "'";
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        }

                        whereQueryWord = "status = 1";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                        if (!string.IsNullOrEmpty(where))
                            sql += " where " + where;

                        List<AscmDiscreteJobs> listDiscreteJobs = AscmDiscreteJobsService.GetInstance().GetList(sql, true);
                        setWipEntities(listDiscreteJobs);
                        setWipRequirementOperations(listDiscreteJobs);
                        setMaterialItem(listDiscreteJobs);
                        setWarehouse(listDiscreteJobs);

                        List<AscmGetMaterialCreateTask> listTasks = createTasks(listDiscreteJobs);
                        saveTasks(listTasks);

                        //分配任务
                        sql = " from AscmGetMaterialTask ";
                        where = "";
                        whereQueryWord = "status = 'NOTALLOCATE'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                        sql += " where " + where;
                        IList<AscmGetMaterialTask> ilistTask = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                        sql = " from AscmAllocateRule ";
                        IList<AscmAllocateRule> ilistRule = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmAllocateRule>(sql);
                        if (ilistTask != null && ilistTask.Count > 0)
                        {
                            TaskAllocate(ilistTask, ilistRule, sql, where, whereQueryWord,userName);
                        }
                    }
                }
                sql = "from AscmGetMaterialTask";
                where = "";
                if (!string.IsNullOrEmpty(queryDate))
                {
                    whereQueryWord = "uploadDate like '" + queryDate + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else
                {
                    string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    whereQueryWord = "uploadDate like '" + date + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(userLogistisClass))
                {
                    IList<AscmLogisticsClassInfo> ilistAscmLogisticsClassInfo = AscmLogisticsClassInfoService.GetInstance().GetList("from AscmLogisticsClassInfo where logisticsClass in (" + userLogistisClass + ")", false, false);
                    string ids = string.Empty;
                    if (ilistAscmLogisticsClassInfo != null && ilistAscmLogisticsClassInfo.Count > 0)
                    {
                        foreach (AscmLogisticsClassInfo ascmlogisticsClassInfo in ilistAscmLogisticsClassInfo)
                        {
                            if (!string.IsNullOrEmpty(ids))
                                ids += ",";
                            ids += ascmlogisticsClassInfo.id;
                        }
                    }

                    string newCondition = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "logisticsClassId");
                    if (!string.IsNullOrEmpty(newCondition))
                    {
                        string newsql = "from AscmAllocateRule where " + newCondition;
                        IList<AscmAllocateRule> ilistAscmAllocateRule = AscmAllocateRuleService.GetInstance().GetList(newsql, false, false, false);
                        ids = "";
                        if (ilistAscmAllocateRule != null && ilistAscmAllocateRule.Count > 0)
                        {
                            foreach (AscmAllocateRule ascmAllocateRule in ilistAscmAllocateRule)
                            {
                                if (!string.IsNullOrEmpty(ids) && (!string.IsNullOrEmpty(ascmAllocateRule.zRankerName) || !string.IsNullOrEmpty(ascmAllocateRule.dRankerName)))
                                    ids += ",";
                                if (!string.IsNullOrEmpty(ascmAllocateRule.zRankerName))
                                    ids += "'" + ascmAllocateRule.zRankerName + "'";
                                if (!string.IsNullOrEmpty(ids) && !string.IsNullOrEmpty(ascmAllocateRule.dRankerName))
                                    ids += ",";
                                if (!string.IsNullOrEmpty(ascmAllocateRule.dRankerName))
                                    ids += "'" + ascmAllocateRule.dRankerName + "'";
                            }
                        }

                        whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "rankerId");
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                }
                else if (string.IsNullOrEmpty(userLogistisClass) && userRole.IndexOf("排产员") > -1)
                {
                    whereQueryWord = "rankerId = '" + userName + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where + " order by taskTime,warehouserId,mtlCategoryStatus,productLine";
                IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql, sql, ynPage);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                    AscmGetMaterialTaskService.GetInstance().SetRanker(list);
                    AscmGetMaterialTaskService.GetInstance().SetWorker(list);
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
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 汇总物料编码
        public ActionResult SumMaterialQuantityIndex()
        {
            //汇总物料编码
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
            List<YnRole> listYnRole = YnRoleService.GetInstance().GetListInUser(userName);
            int nI = 0;
            foreach (YnRole role in listYnRole)
            {
                if (role.name == "领料员")
                {
                    nI++;
                }
            }
            if (nI != 0)
            {
                userRole = userName;
            }

            List<AscmMaterialItem> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string startDate = string.Empty;
                if (!string.IsNullOrEmpty(queryStartDate))
                {
                    startDate = Convert.ToDateTime(queryStartDate).ToString("yyyy-MM-dd") + " 00:00:00";
                }
                else
                {
                    startDate = DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00";
                }

                string endDate = string.Empty;
                if (!string.IsNullOrEmpty(queryEndDate))
                {
                    endDate = Convert.ToDateTime(queryEndDate).ToString("yyyy-MM-dd") + " 23:59:59";
                }
                else
                {
                    endDate = DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59:59";
                }

                string sql = "from AscmGetMaterialTask";
                string where = "", whereQueryWord = "";
                whereQueryWord = "createTime > '" + startDate + "'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                whereQueryWord = "createTime <= '" + endDate + "'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(userRole))
                {
                    whereQueryWord = "workerId = '" + userRole + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmGetMaterialTask> ilistAscmGetMaterialTask = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                if (ilistAscmGetMaterialTask != null && ilistAscmGetMaterialTask.Count > 0)
                {
                    string ids = string.Empty;
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in ilistAscmGetMaterialTask)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += ascmGetMaterialTask.id;
                    }

                    where = "";
                    sql = "select inventoryitemid, sum(requiredQuantity), sum(quantityIssued), sum(getMaterialQuantity), sum(requiredQuantity - quantityIssued) as quantityDifference, sum(requiredQuantity - getMaterialQuantity) as quantityGetMaterialDifference from ascm_wip_require_operat";
                    if (!string.IsNullOrEmpty(ids))
                    {
                        where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "taskId");

                        if (!string.IsNullOrEmpty(where))
                            sql += " where " + where + " group by inventoryitemid";
                        IList ilistOperations = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);
                        if (ilistOperations != null && ilistOperations.Count > 0)
                        {
                            ids = "";
                            foreach (object[] obj in ilistOperations)
                            {
                                if (!string.IsNullOrEmpty(ids))
                                    ids += ",";
                                ids += obj[0].ToString();
                            }

                            where = "";
                            sql = "from AscmMaterialItem";
                            if (!string.IsNullOrEmpty(ids))
                            {
                                where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");

                                if (!string.IsNullOrEmpty(queryDocnumber))
                                {
                                    whereQueryWord = " docNumber like '" + queryDocnumber + "%'";
                                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                                }

                                if (!string.IsNullOrEmpty(where))
                                    sql += " where " + where;

                                IList<AscmMaterialItem> ilistAscmMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql, sql, ynPage);
                                if (ilistAscmMaterialItem != null && ilistAscmMaterialItem.Count > 0)
                                {
                                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistAscmMaterialItem);
                                    foreach (AscmMaterialItem ascmMaterialItem in ilistAscmMaterialItem)
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
                            }
                        }
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

        #region 备料形式批量维护
        public ActionResult StockFormatIndex()
        {
            return View();
        }
        public ActionResult StockFormatInfoList(int? page, int? rows, string sort, string order, string queryWord, string queryType, string queryStarDocnumber, string queryEndDocnumber, string zStatus, string dStatus, string wStatus, string queryDescribe)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;
            
            List<AscmMaterialItem> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmMaterialItemService.GetInstance().GetList(ynPage, "", "", queryWord, queryType, queryStarDocnumber, queryEndDocnumber, zStatus, dStatus, wStatus, queryDescribe);
                if (list != null && list.Count > 0)
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error(ex.Message, ex);
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DataRepair(int? page, int? rows, string sort, string order, string queryWord)
        {
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            List<AscmMaterialItem> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            string sql = string.Empty;
            try
            {
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

                sql = "from AscmMaterialItem where isFlag = 0 order by docNumber";
                YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows
                ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber
                IList<AscmMaterialItem> newilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql,sql,ynPage);
                if (newilist != null && newilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(newilist);
                    foreach (AscmMaterialItem ascmMaterialItem in list)
                    {
                        jsonDataGridResult.rows.Add(ascmMaterialItem);
                    }
                }
                jsonDataGridResult.result = true;
                if (newilist.Count == 0)
                {
                    jsonDataGridResult.message = "已完成物料大类、小类及物料信息维护！";
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                jsonDataGridResult.message = ex.Message;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StockFormatEdit(int? id)
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
        public ContentResult StockFormatSave(AscmMaterialItem ascmMaterialItem_Model, int? id)
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
                if (id.HasValue)
                {
                    ascmMaterialItem = AscmMaterialItemService.GetInstance().Get(id.Value);
                    ascmMaterialItem.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmMaterialItem.modifyUser = userName;
                    ascmMaterialItem.zMtlCategoryStatus = ascmMaterialItem_Model.zMtlCategoryStatus;
                    ascmMaterialItem.dMtlCategoryStatus = ascmMaterialItem_Model.dMtlCategoryStatus;
                    ascmMaterialItem.wMtlCategoryStatus = ascmMaterialItem_Model.wMtlCategoryStatus;
                    AscmMaterialItemService.GetInstance().Update(ascmMaterialItem);
                    jsonObjectReuslt.message = "";
                }
                else
                {
                    jsonObjectReuslt.message = "未选择或获取物料信息！";
                }
                jsonObjectReuslt.result = true;
                jsonObjectReuslt.id = ascmMaterialItem.id.ToString();
                jsonObjectReuslt.entity = ascmMaterialItem;
            }
            catch (Exception ex)
            {
                jsonObjectReuslt.message = ex.Message;
                throw ex;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectReuslt);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult BatchStockFormatSave(AscmMaterialItem ascmMaterialItem_Model)
        {
            JsonObjectResult jsonObjectReuslt = new JsonObjectResult();

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                if (string.IsNullOrEmpty(ascmMaterialItem_Model.sDocnumber) || string.IsNullOrEmpty(ascmMaterialItem_Model.eDocnumber))
                    throw new Exception("编码段不能为空！");

                string sql = "from AscmMaterialItem";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(ascmMaterialItem_Model.wipSupplyType.ToString()) && ascmMaterialItem_Model.wipSupplyType.ToString() != "0")
                {
                    whereQueryWord = "wipSupplyType = " + ascmMaterialItem_Model.wipSupplyType.ToString();
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(ascmMaterialItem_Model.sDocnumber) && !string.IsNullOrEmpty(ascmMaterialItem_Model.eDocnumber))
                {
                    whereQueryWord = "docNumber >= '" + ascmMaterialItem_Model.sDocnumber + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    whereQueryWord = "docNumber <= '" + ascmMaterialItem_Model.eDocnumber + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmMaterialItem> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                List<AscmMaterialItem> newlist = null;
                if (ilist != null && ilist.Count > 0)
                {
                    newlist = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilist);
                    foreach (AscmMaterialItem ascmMaterialItem in newlist)
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
                    AscmMaterialItemService.GetInstance().Update(newlist);
                }
                jsonObjectReuslt.result = true;
                jsonObjectReuslt.message = "批量维护成功！";
            }
            catch (Exception ex)
            {
                jsonObjectReuslt.message = ex.Message;
                throw ex;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectReuslt);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult ChoiceStockFormatSave(string releaseHeaderIds, string zmtlCategoryStatus, string dmtlCategoryStatus, string wmtlCategoryStatus)
        {
            JsonObjectResult jsonObjectReuslt = new JsonObjectResult();

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                string sql = "from AscmMaterialItem";
                string where = "", whereQueryWord = "";

                if (!string.IsNullOrEmpty(releaseHeaderIds))
                {
                    whereQueryWord = "id in (" + releaseHeaderIds + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    if (!string.IsNullOrEmpty(where))
                        sql += " where " + where;

                    IList<AscmMaterialItem> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmMaterialItem> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilist);
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
                }
            }
            catch (Exception ex)
            {
                jsonObjectReuslt.message = ex.Message;
                throw ex;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectReuslt);
            return Content(sReturn);
        }
        public ActionResult MaterialImport(HttpPostedFileBase fileImport)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            string sError = "";

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                if (fileImport != null)
                {
                    List<ImOrExMaterialDefine> list = null;
                    List<AscmMaterialItem> listAscmMaterialItem = new List<AscmMaterialItem>();
                    using (Stream stream = fileImport.InputStream)
                    {
                        list = new List<ImOrExMaterialDefine>();

                        NPOI.SS.UserModel.IWorkbook wb = NPOI.SS.UserModel.WorkbookFactory.Create(stream);
                        ISheet sheet = wb.GetSheet("Sheet1");
                        IEnumerator rows = sheet.GetRowEnumerator();
                        while (rows.MoveNext())
                        {
                            NPOI.SS.UserModel.IRow row = (NPOI.SS.UserModel.IRow)rows.Current;
                            if (row.RowNum != 0)
                            {
                                List<NPOI.SS.UserModel.ICell> iCellList = new List<NPOI.SS.UserModel.ICell>();
                                for (int i = 0; i < ImOrExMaterialDefine.GetList().Count; i++)
                                {
                                    NPOI.SS.UserModel.ICell iCell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                    iCellList.Add(iCell);
                                }

                                ImOrExMaterialDefine ImportMaterial = new ImOrExMaterialDefine();

                                if (iCellList[0].ToString() != null)
                                    ImportMaterial.materialDocnumber = iCellList[0].ToString().Trim();
                                if (iCellList[1].ToString() != null)
                                    ImportMaterial.wipSupplyType = iCellList[1].ToString().Trim();
                                if (iCellList[2].ToString() != null)
                                    ImportMaterial.materialDescription = iCellList[2].ToString().Trim();
                                if (iCellList[3].ToString() != null)
                                    ImportMaterial.zMtlCategoryStatus = iCellList[3].ToString().Trim();
                                if (iCellList[4].ToString() != null)
                                    ImportMaterial.dMtlCategoryStatus = iCellList[4].ToString().Trim();
                                if (iCellList[5].ToString() != null)
                                    ImportMaterial.wMtlCategoryStatus = iCellList[5].ToString().Trim();
                                ImportMaterial.rowNumber = row.RowNum + 1;
                                list.Add(ImportMaterial);
                            }
                        }
                    }
                    if (list != null && list.Count > 0)
                    {
                        foreach (ImOrExMaterialDefine ImportMaterial in list)
                        {
                            object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmMaterialItem where docNumber = '" + ImportMaterial.materialDocnumber + "'");
                            if (obj == null)
                                throw new Exception("查询异常！");
                            int iCount = 0;
                            if (int.TryParse(obj.ToString(), out iCount) && iCount > 0)
                            {
                                List<AscmMaterialItem> tempList = AscmMaterialItemService.GetInstance().GetList("from AscmMaterialItem where docNumber = '" + ImportMaterial.materialDocnumber + "'");
                                if (tempList != null && tempList.Count > 0)
                                {
                                    foreach (AscmMaterialItem ascmMaterialItem in tempList)
                                    {
                                        if (ImportMaterial.zMtlCategoryStatus == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.inStock))
                                        {
                                            ascmMaterialItem.zMtlCategoryStatus = MtlCategoryStatusDefine.inStock;
                                        }
                                        else if (ImportMaterial.zMtlCategoryStatus == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.mixStock))
                                        {
                                            ascmMaterialItem.zMtlCategoryStatus = MtlCategoryStatusDefine.mixStock;
                                        }
                                        else if (ImportMaterial.zMtlCategoryStatus == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.preStock))
                                        {
                                            ascmMaterialItem.zMtlCategoryStatus = MtlCategoryStatusDefine.preStock;
                                        }
                                        else if (string.IsNullOrEmpty(ImportMaterial.zMtlCategoryStatus))
                                        {
                                            ascmMaterialItem.wMtlCategoryStatus = "";
                                        }
                                        else if (!string.IsNullOrEmpty(ImportMaterial.zMtlCategoryStatus))
                                        {
                                            sError += "物料编码[" + ascmMaterialItem.docNumber + "]总装备料形式书写不正确:" + ImportMaterial.zMtlCategoryStatus;
                                            continue;
                                        }

                                        if (ImportMaterial.dMtlCategoryStatus == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.inStock))
                                        {
                                            ascmMaterialItem.dMtlCategoryStatus = MtlCategoryStatusDefine.inStock;
                                        }
                                        else if (ImportMaterial.dMtlCategoryStatus == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.mixStock))
                                        {
                                            ascmMaterialItem.dMtlCategoryStatus = MtlCategoryStatusDefine.mixStock;
                                        }
                                        else if (ImportMaterial.dMtlCategoryStatus == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.preStock))
                                        {
                                            ascmMaterialItem.dMtlCategoryStatus = MtlCategoryStatusDefine.preStock;
                                        }
                                        else if (string.IsNullOrEmpty(ImportMaterial.dMtlCategoryStatus))
                                        {
                                            ascmMaterialItem.wMtlCategoryStatus = "";
                                        }
                                        else if (!string.IsNullOrEmpty(ImportMaterial.dMtlCategoryStatus))
                                        {
                                            sError += "物料编码[" + ascmMaterialItem.docNumber + "]电装备料形式书写不正确:" + ImportMaterial.dMtlCategoryStatus;
                                            continue;
                                        }

                                        if (ImportMaterial.wMtlCategoryStatus == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.inStock))
                                        {
                                            ascmMaterialItem.wMtlCategoryStatus = MtlCategoryStatusDefine.inStock;
                                        }
                                        else if (ImportMaterial.wMtlCategoryStatus == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.mixStock))
                                        {
                                            ascmMaterialItem.wMtlCategoryStatus = MtlCategoryStatusDefine.mixStock;
                                        }
                                        else if (ImportMaterial.wMtlCategoryStatus == MtlCategoryStatusDefine.DisplayText(MtlCategoryStatusDefine.preStock))
                                        {
                                            ascmMaterialItem.wMtlCategoryStatus = MtlCategoryStatusDefine.preStock;
                                        }
                                        else if (string.IsNullOrEmpty(ImportMaterial.wMtlCategoryStatus))
                                        {
                                            ascmMaterialItem.wMtlCategoryStatus = "";
                                        }
                                        else if (!string.IsNullOrEmpty(ImportMaterial.wMtlCategoryStatus))
                                        {
                                            sError += "物料编码[" + ascmMaterialItem.docNumber + "]其他备料形式书写不正确:" + ImportMaterial.wMtlCategoryStatus;
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
                                sError +="未找到对应的物料编码:" + ImportMaterial.materialDocnumber+"<br />";
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
                jsonObjectResult.message += ex.Message;
                YnBaseClass2.Helper.LogHelper.GetLog().Error(ex.Message,ex);
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult MaterialExport(string queryWord, string queryType, string queryStarDocnumber, string queryEndDocnumber, string zStatus, string dStatus, string wStatus, string queryDescribe)
        {
            List<ImOrExMaterialDefine> list = new List<ImOrExMaterialDefine>();
            NPOI.SS.UserModel.IWorkbook wb = new NPOI.HSSF.UserModel.HSSFWorkbook();
            try
            {
                ISheet sheet = wb.CreateSheet("Sheet1");
                sheet.SetColumnWidth(0, 20 * 256);
                sheet.SetColumnWidth(1, 13 * 256);
                sheet.SetColumnWidth(2, 30 * 256);
                sheet.SetColumnWidth(3, 15 * 256);
                sheet.SetColumnWidth(4, 15 * 256);
                sheet.SetColumnWidth(5, 15 * 256);

                int iRow = 0;
                NPOI.SS.UserModel.IRow titleRow = sheet.CreateRow(0);
                titleRow.Height = 20 * 20;
                for (int i = 0; i < ImOrExMaterialDefine.GetList().Count; i++)
                {
                    titleRow.CreateCell(i).SetCellValue(ImOrExMaterialDefine.DisplayText(ImOrExMaterialDefine.GetList()[i].ToString()));
                }

                YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                List<AscmMaterialItem> listAscmMaterialItem = AscmMaterialItemService.GetInstance().GetList(ynPage, "", "", queryWord, queryType, queryStarDocnumber, queryEndDocnumber, zStatus, dStatus, wStatus, queryDescribe);
                if (listAscmMaterialItem != null && listAscmMaterialItem.Count > 0)
                {
                    ImOrExMaterialDefine titleExportMaterial = new ImOrExMaterialDefine();
                    List<string> titleCols = ImOrExMaterialDefine.GetList();
                    titleExportMaterial.materialDocnumber = ImOrExMaterialDefine.DisplayText(titleCols[0]);
                    titleExportMaterial.wipSupplyType = ImOrExMaterialDefine.DisplayText(titleCols[1]);
                    titleExportMaterial.materialDescription = ImOrExMaterialDefine.DisplayText(titleCols[2]);
                    titleExportMaterial.zMtlCategoryStatus = ImOrExMaterialDefine.DisplayText(titleCols[3]);
                    titleExportMaterial.dMtlCategoryStatus = ImOrExMaterialDefine.DisplayText(titleCols[4]);
                    titleExportMaterial.wMtlCategoryStatus = ImOrExMaterialDefine.DisplayText(titleCols[5]);
                    titleExportMaterial.rowNumber = iRow++;
                    list.Add(titleExportMaterial);

                    foreach (AscmMaterialItem ascmMaterialItem in listAscmMaterialItem)
                    {
                        ImOrExMaterialDefine contentExportMaterial = new ImOrExMaterialDefine();
                        contentExportMaterial.materialDocnumber = ascmMaterialItem.docNumber;
                        contentExportMaterial.wipSupplyType = AscmMaterialItem.WipSupplyTypeDefine.DisplayText(ascmMaterialItem.wipSupplyType);
                        contentExportMaterial.materialDescription = ascmMaterialItem.description;
                        contentExportMaterial.zMtlCategoryStatus = MtlCategoryStatusDefine.DisplayText(ascmMaterialItem.zMtlCategoryStatus);
                        contentExportMaterial.dMtlCategoryStatus = MtlCategoryStatusDefine.DisplayText(ascmMaterialItem.dMtlCategoryStatus);
                        contentExportMaterial.wMtlCategoryStatus = MtlCategoryStatusDefine.DisplayText(ascmMaterialItem.wMtlCategoryStatus);
                        contentExportMaterial.rowNumber = iRow++;
                        list.Add(contentExportMaterial);
                    }
                }

                if (list != null && list.Count > 0)
                {
                    foreach (ImOrExMaterialDefine ExportMaterial in list)
                    {
                        NPOI.SS.UserModel.IRow row = sheet.CreateRow(ExportMaterial.rowNumber);
                        row.Height = 20 * 20;
                        row.CreateCell(0).SetCellValue(ExportMaterial.materialDocnumber);
                        row.CreateCell(1).SetCellValue(ExportMaterial.wipSupplyType);
                        row.CreateCell(2).SetCellValue(ExportMaterial.materialDescription);
                        row.CreateCell(3).SetCellValue(ExportMaterial.zMtlCategoryStatus);
                        row.CreateCell(4).SetCellValue(ExportMaterial.dMtlCategoryStatus);
                        row.CreateCell(5).SetCellValue(ExportMaterial.wMtlCategoryStatus);
                    }
                }
            }
            catch (Exception ex)
            {
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

        #region 基础数据管理
        public ActionResult LogisticsClassIndex()
        {
            return View();
        }
        //车间信息管理
        public ActionResult LogisticsClassInfoList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            List<AscmLogisticsClassInfo> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmLogisticsClassInfoService.GetInstance().GetList(ynPage, "", "", "", "");
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error(ex.Message, ex);
                throw ex;
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
                    //YnUser groupUser = YnUserService.GetInstance().Get(ascmLogisticsClassInfo.groupLeader);
                    //ascmLogisticsClassInfo.groupUser = groupUser;
                    //YnUser monitorUser = YnUserService.GetInstance().Get(ascmLogisticsClassInfo.monitorLeader);
                    //ascmLogisticsClassInfo.monitorUser = monitorUser;
                    //ascmLogisticsClassInfo.logisticsClassName = AscmCommonHelperService.GetInstance().DisplayLogisticsClass(ascmLogisticsClassInfo.logisticsClass);
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
                    if (int.TryParse(obj.ToString(), out iCount) && iCount == 0)
                    {
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
                    else
                    {
                        throw new Exception("该物流组信息已存在！");
                    }
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
                {
                    AscmLogisticsClassInfoService.GetInstance().Update(ascmLogisticsClassInfo);
                }
                else
                {
                    AscmLogisticsClassInfoService.GetInstance().Save(ascmLogisticsClassInfo);
                }

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
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
                if (id.HasValue)
                {
                    AscmLogisticsClassInfo ascmLogisticsClassInfo = AscmLogisticsClassInfoService.GetInstance().Get(id.Value);
                    if (ascmLogisticsClassInfo == null)
                        throw new Exception("找不到匹配的车间信息！");

                    AscmLogisticsClassInfoService.GetInstance().Delete(ascmLogisticsClassInfo);
                    jsonObjectResult.result = true;
                    jsonObjectResult.message = "";
                }
                else
                {
                    jsonObjectResult.result = false;
                    jsonObjectResult.message = "传入参数[id=null]错误！";
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message; ;
            }

            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadLogisticsWorkerList(int? page, int? rows, string sort, string order, string queryWord, int? logisticsClassId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmAllocateRule> list = null;

            try
            {
                if (logisticsClassId.HasValue)
                {
                    string sql = "from AscmAllocateRule";
                    string where = "", whereQueryWord = "";

                    whereQueryWord = "logisticsClassId = " + logisticsClassId;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    if (!string.IsNullOrEmpty(where))
                    {
                        sql += " where " + where;
                        IList<AscmAllocateRule> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmAllocateRule>(sql, sql, ynPage);
                        if (ilist != null && ilist.Count > 0)
                        {
                            list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmAllocateRule>(ilist);
                            AscmAllocateRuleService.GetInstance().SetWorker(list);
                            AscmAllocateRuleService.GetInstance().SetZRanker(list);
                            AscmAllocateRuleService.GetInstance().SetDRanker(list);
                            AscmAllocateRuleService.GetInstance().SetLogisticsClass(list);
                            AscmAllocateRuleService.GetInstance().SetLogisticsClassName(list);
                            foreach (AscmAllocateRule ascmAllocateRule in list)
                            {
                                jsonDataGridResult.rows.Add(ascmAllocateRule);
                            }
                        }
                        jsonDataGridResult.total = ynPage.GetRecordCount();
                        jsonDataGridResult.result = true;
                    }
                }
                else
                {
                    jsonDataGridResult.message = "传入参数[logisticsClassId=null]错误！";
                }
            }
            catch (Exception ex)
            {
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        //车间领料员管理
        public ActionResult LogisticsWorkerList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE);//pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1);//pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmAllocateRule> list = null;
            try
            {
                string sql = "from AscmLogisticsClassInfo";
                string where = "", whereQueryWord = "";

                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = "logisticsClass = '" + AscmCommonHelperService.GetInstance().DisplayEnglishLogisticsClass(queryWord) + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;

                    IList<AscmLogisticsClassInfo> ilistAscmLogisticsClassInfo = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLogisticsClassInfo>(sql);
                    string ids = string.Empty;
                    if (ilistAscmLogisticsClassInfo != null && ilistAscmLogisticsClassInfo.Count > 0)
                    {
                        foreach (AscmLogisticsClassInfo ascmLogisticsClassInfo in ilistAscmLogisticsClassInfo)
                        {
                            if (!string.IsNullOrEmpty(ids))
                                ids += ",";
                            ids += ascmLogisticsClassInfo.id;
                        }
                    }

                    where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "logisticsClassId");
                    sql = "from AscmAllocateRule";

                    if (!string.IsNullOrEmpty(where))
                    {
                        sql += " where " + where;

                        IList<AscmAllocateRule> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmAllocateRule>(sql);
                        if (ilist != null && ilist.Count > 0)
                        {
                            list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmAllocateRule>(ilist);
                            AscmAllocateRuleService.GetInstance().SetWorker(list);
                            AscmAllocateRuleService.GetInstance().SetZRanker(list);
                            AscmAllocateRuleService.GetInstance().SetDRanker(list);
                            AscmAllocateRuleService.GetInstance().SetLogisticsClass(list);
                            AscmAllocateRuleService.GetInstance().SetLogisticsClassName(list);
                            foreach (AscmAllocateRule ascmAllocateRule in list)
                            {
                                jsonDataGridResult.rows.Add(ascmAllocateRule);
                            }
                        }
                        jsonDataGridResult.result = true;
                        jsonDataGridResult.total = ynPage.GetRecordCount();
                    }
                }
            }
            catch (Exception ex)
            {
                jsonDataGridResult.message = ex.Message;
            }

            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogisticsWorkerDelete(int? id)
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
                    AscmAllocateRule ascmAllocateRule = AscmAllocateRuleService.GetInstance().Get(id.Value);
                    if (ascmAllocateRule == null)
                        throw new Exception("找不到匹配的领料员！");

                    ascmAllocateRule.logisticsClassId = 0;
                    ascmAllocateRule.modifyUser = userName;
                    ascmAllocateRule.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    AscmAllocateRuleService.GetInstance().Update(ascmAllocateRule);
                    jsonObjectResult.result = true;
                    jsonObjectResult.message = "";
                }
                else
                {
                    jsonObjectResult.result = false;
                    jsonObjectResult.message = "传入参数[id=null]错误！";
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }

            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult LogisticsWorkerSave(AscmAllocateRule ascmAllocateRule_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();

            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            try
            {
                if (!string.IsNullOrEmpty(ascmAllocateRule_Model.id.ToString()))
                {
                    List<AscmAllocateRule> list = AscmAllocateRuleService.GetInstance().GetList("from AscmAllocateRule where workerName = '" + ascmAllocateRule_Model.workerName + "'", false, false, false);
                    if (list == null)
                        throw new Exception("找不到匹配的领料员");

                    if (list != null && list.Count > 0)
                    {
                        foreach (AscmAllocateRule ascmAllocateRule in list)
                        {
                            ascmAllocateRule.logisticsClassId = id.Value; ;
                            ascmAllocateRule.modifyUser = userName;
                            ascmAllocateRule.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        }

                        AscmAllocateRuleService.GetInstance().Update(list);
                    }
                    jsonObjectResult.result = true;
                    jsonObjectResult.message = "";
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

            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        #endregion
    }
}
