﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Services.SupplierPreparation;
using YnBaseClass2.Web;
using MideaAscm.Dal;
using Newtonsoft.Json;
using NHibernate;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Services.Base;
using MideaAscm.Dal.FromErp.Entities;
using YnFrame.Services;
using MideaAscm.Dal.Base;
using NPOI.SS.UserModel;

namespace MideaAscm.Areas.Ascm.Controllers
{
    public class SupplierPreparationController : YnFrame.Controllers.YnBaseController
    {
        //
        // GET: /Ascm/GysBl/
        //备料管理
        public ActionResult Index()
        {
            return View();
        }

        #region 容器规格管理
        public ActionResult ContainerSpecIndex()
        {
            //容器规格管理
            return View();
        }
        public ActionResult ContainerSpecList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmContainerSpec> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmContainerSpecService.GetInstance().GetList(ynPage, "", "", queryWord, null);
                if (list != null)
                {
                    foreach (AscmContainerSpec ascmContainerSpec in list)
                    {
                        jsonDataGridResult.rows.Add(ascmContainerSpec);
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
        public ActionResult ContainerSpecEdit(int? id)
        {
            AscmContainerSpec ascmContainerSpec = null;
            try
            {
                if (id.HasValue)
                {
                    ascmContainerSpec = AscmContainerSpecService.GetInstance().Get(id.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmContainerSpec, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult ContainerSpecSave(AscmContainerSpec ascmContainerSpec_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;
                if (ascmContainerSpec_Model.spec == null || ascmContainerSpec_Model.spec.Trim() == "")
                    throw new Exception("容器规格不能为空！");

                AscmContainerSpec ascmContainerSpec = null;
                if (id.HasValue)
                {
                    ascmContainerSpec = AscmContainerSpecService.GetInstance().Get(id.Value);
                    if (ascmContainerSpec == null)
                        throw new Exception("找不到容器规格");
                }
                else
                {
                    ascmContainerSpec = new AscmContainerSpec();
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmContainerSpec");
                    ascmContainerSpec.id = ++maxId;
                    ascmContainerSpec.createUser = userName;
                    ascmContainerSpec.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                ascmContainerSpec.spec = ascmContainerSpec_Model.spec.Trim();

                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmContainerSpec where id<>" + ascmContainerSpec.id + " and spec='" + ascmContainerSpec.spec + "'");
                if (object1 == null)
                    throw new Exception("查询异常！");
                int iCount = 0;
                if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                    throw new Exception("已经存在此容器规格【" + ascmContainerSpec.spec + "】");

                ascmContainerSpec.modifyUser = userName;
                ascmContainerSpec.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                if (!string.IsNullOrEmpty(ascmContainerSpec_Model.description))
                    ascmContainerSpec.description = ascmContainerSpec_Model.description.Trim();

                if (id.HasValue)
                    AscmContainerSpecService.GetInstance().Update(ascmContainerSpec);
                else
                    AscmContainerSpecService.GetInstance().Save(ascmContainerSpec);

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmContainerSpec.id.ToString();
                jsonObjectResult.entity = ascmContainerSpec;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult ContainerSpecDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    AscmContainerSpec ascmContainerSpec = AscmContainerSpecService.GetInstance().Get(id.Value);
                    if (ascmContainerSpec == null)
                        throw new Exception("找不到容器规格");
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmContainer where specId=" + ascmContainerSpec.id);
                    if (object1 != null)
                    {
                        int count = 0;
                        if (int.TryParse(object1.ToString(), out count) && count > 0)
                            throw new Exception("已存在规格【" + ascmContainerSpec.spec + "】的容器");
                    }

                    AscmContainerSpecService.GetInstance().Delete(ascmContainerSpec);
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
        public ActionResult ContainerSpecAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmContainerSpec> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmContainerSpecService.GetInstance().GetList(ynPage, "", "", q, null);
                if (list != null)
                {
                    foreach (AscmContainerSpec ascmContainerSpec in list)
                    {
                        jsonDataGridResult.rows.Add(ascmContainerSpec);
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

        #region 容器管理
        public ActionResult ContainerIndex()
        {
            //容器管理
            return View();
        }
        public ActionResult ContainerList(int? page, int? rows, string sort, string order, string queryWord, int? supplierId,
            int? status, string queryStartSn, string queryEndSn)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmContainer> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "", whereStatus = "";
                string whereStartSn = "", whereEndSn = "";
                if (supplierId.HasValue)
                    whereSupplier = "supplierId=" + supplierId.Value;
                if (status.HasValue)
                    whereStatus = " status = " + status.Value;
                long startSn = 0, endSn = 0;
                if (!string.IsNullOrEmpty(queryStartSn) && long.TryParse(queryStartSn.Trim(), out startSn))
                    whereStartSn = " sn >='" + queryStartSn.Trim() + "'";
                if (!string.IsNullOrEmpty(queryEndSn) && long.TryParse(queryEndSn.Trim(), out endSn))
                    whereEndSn = " sn <='" + queryEndSn.Trim() + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartSn);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndSn);

                list = AscmContainerService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                if (list != null)
                {
                    foreach (AscmContainer ascmContainer in list)
                    {
                        jsonDataGridResult.rows.Add(ascmContainer);
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
        public ActionResult ContainerEdit(string id)
        {
            AscmContainer ascmContainer = null;
            try
            {
                if (id != null && id.Trim() != "")
                {
                    ascmContainer = AscmContainerService.GetInstance().Get(id);
                    if (ascmContainer != null)
                    {
                        AscmSupplier ascmSupplier = AscmSupplierService.GetInstance().Get(ascmContainer.supplierId);
                        ascmContainer.supplier = ascmSupplier;
                        AscmContainerSpec ascmContainerSpec = AscmContainerSpecService.GetInstance().Get(ascmContainer.specId);
                        ascmContainer.containerSpec = ascmContainerSpec;
                        ascmContainer.startSn = ascmContainer.sn;
                        ascmContainer.endSn = ascmContainer.sn;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmContainer, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult ContainerSave(AscmContainer ascmContainer_Model, string id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                if (id != null && id.Trim() != "")
                {
                    AscmContainer ascmContainer = AscmContainerService.GetInstance().Get(id);
                    if (ascmContainer == null)
                        throw new Exception("保存容器失败！");
                    if (!string.IsNullOrEmpty(ascmContainer_Model.description))
                        ascmContainer.description = ascmContainer_Model.description.Trim();
                    ascmContainer.modifyUser = userName;
                    ascmContainer.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                    AscmContainerService.GetInstance().Update(ascmContainer);
                    jsonObjectResult.id = ascmContainer.sn;
                    jsonObjectResult.entity = ascmContainer;
                }
                else
                {
                    AscmContainerSpec ascmContainerSpec = AscmContainerSpecService.GetInstance().Get(ascmContainer_Model.specId);
                    if (ascmContainerSpec == null)
                        throw new Exception("容器规格不能为空！");

                    if (ascmContainer_Model.startSn == null || ascmContainer_Model.startSn.Trim() == "")
                        throw new Exception("开始编号不能为空");
                    if (ascmContainer_Model.endSn == null || ascmContainer_Model.endSn.Trim() == "")
                        throw new Exception("结束编号不能为空");
                    if (ascmContainer_Model.startSn.Trim().Length != ascmContainer_Model.endSn.Trim().Length)
                        throw new Exception("开始编号和结束编号长度不一致");
                    long startSn = 0, endSn = 0;
                    long.TryParse(ascmContainer_Model.startSn, out startSn);
                    long.TryParse(ascmContainer_Model.endSn, out endSn);
                    if (startSn == 0 || endSn == 0 || startSn > endSn)
                        throw new Exception("容器编号输入错误");
                    if (endSn - startSn + 1 > 500)
                        throw new Exception("单次添加不能超过500");
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmRfid where id between '" + ascmContainer_Model.startSn.Trim() + "' and '" + ascmContainer_Model.endSn.Trim() + "'");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int count = 0;
                    if (int.TryParse(object1.ToString(), out count) && count > 0)
                        throw new Exception("已存在编号");

                    List<AscmContainer> listAscmContainer = new List<AscmContainer>();
                    List<AscmRfid> listAscmRfid = new List<AscmRfid>();
                    for (; startSn <= endSn; startSn++)
                    {
                        AscmContainer ascmContainer = new AscmContainer();
                        string sn = string.Format("{0:000000}", startSn);//生成六位数的ID
                        ascmContainer.sn = sn;
                        ascmContainer.specId = ascmContainer_Model.specId;
                        ascmContainer.rfid = ascmContainer.sn + "000000000000000000";
                        ascmContainer.supplierId = ascmContainer_Model.supplierId;
                        if (!string.IsNullOrEmpty(ascmContainer_Model.description))
                            ascmContainer.description = ascmContainer_Model.description.Trim();
                        ascmContainer.status = ascmContainer_Model.status;
                        ascmContainer.createUser = userName;
                        ascmContainer.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmContainer.modifyUser = userName;
                        ascmContainer.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        listAscmContainer.Add(ascmContainer);

                        AscmRfid ascmRfid = new AscmRfid();
                        ascmRfid.id = ascmContainer.sn;
                        ascmRfid.epcId = ascmContainer.rfid;
                        ascmRfid.createUser = userName;
                        ascmRfid.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmRfid.modifyUser = userName;
                        ascmRfid.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmRfid.bindType = AscmRfid.BindTypeDefine.container;
                        ascmRfid.bindId = ascmContainer.sn;
                        //ascmRfid.bindType = AscmRfid.BindTypeDefine.container;
                        ascmRfid.status = AscmRfid.StatusDefine.inUse;
                        listAscmRfid.Add(ascmRfid);
                    }

                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            if (listAscmContainer.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmContainer);

                            if (listAscmRfid.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdateList(listAscmRfid);

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
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult ContainerDelete(string startSn, string endSn)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (startSn == null || startSn.Trim() == "")
                    throw new Exception("开始编号不能为空");
                if (endSn == null || endSn.Trim() == "")
                    throw new Exception("结束编号不能为空");
                if (startSn.Trim().Length != endSn.Trim().Length)
                    throw new Exception("开始编号和结束编号长度不一致");
                long deleteStartSn = 0, deleteEndSn = 0;
                long.TryParse(startSn, out deleteStartSn);
                long.TryParse(endSn, out deleteEndSn);
                if (deleteStartSn == 0 || deleteEndSn == 0 || deleteStartSn > deleteEndSn)
                    throw new Exception("容器编号输入错误");
                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmContainerDelivery where containerSn between '" + startSn.Trim() + "' and '" + endSn.Trim() + "'");
                if (object1 == null)
                    throw new Exception("查询异常！");
                int count = 0;
                if (int.TryParse(object1.ToString(), out count) && count > 0)
                    throw new Exception("不能删除已绑定物料的容器");

                List<AscmContainer> listAscmContainer = AscmContainerService.GetInstance().GetList("from AscmContainer where sn between '" + startSn.Trim() + "' and '" + endSn.Trim() + "'");
                if (listAscmContainer != null && listAscmContainer.Count > 0)
                {
                    //foreach (AscmContainer ascmContainer in listAscmContainer)
                    //{
                    //    if (ascmContainer.status != AscmContainer.StatusDefine.unuse)
                    //        throw new Exception("不能删除此状态的容器");
                    //}

                    List<AscmRfid> listAscmRfid = AscmRfidService.GetInstance().GetList("from AscmRfid where id between '" + startSn.Trim() + "' and '" + endSn.Trim() + "'");

                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            if (listAscmContainer != null && listAscmContainer.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmContainer);

                            if (listAscmRfid != null && listAscmRfid.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmRfid);

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
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 托盘管理
        public ActionResult PalletIndex()
        {
            //托盘管理
            return View();
        }
        public ActionResult PalletList(int? page, int? rows, string sort, string order, string queryWord, int? supplierId,
            string status, string queryStartSn, string queryEndSn)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmPallet> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "", whereStatus = "";
                string whereStartSn = "", whereEndSn = "";
                if (supplierId.HasValue)
                    whereSupplier = "supplierId=" + supplierId.Value;
                if (!string.IsNullOrEmpty(status))
                    whereStatus = " status = '" + status + "'";
                long startSn = 0, endSn = 0;
                if (!string.IsNullOrEmpty(queryStartSn) && long.TryParse(queryStartSn.Trim(), out startSn))
                    whereStartSn = " sn >='" + queryStartSn.Trim() + "'";
                if (!string.IsNullOrEmpty(queryEndSn) && long.TryParse(queryEndSn.Trim(), out endSn))
                    whereEndSn = " sn <='" + queryEndSn.Trim() + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartSn);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndSn);

                list = AscmPalletService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                if (list != null)
                {
                    foreach (AscmPallet ascmPallet in list)
                    {
                        jsonDataGridResult.rows.Add(ascmPallet);
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
        public ActionResult PalletEdit(string id)
        {
            AscmPallet ascmPallet = null;
            try
            {
                if (id != null && id.Trim() != "")
                {
                    ascmPallet = AscmPalletService.GetInstance().Get(id);
                    if (ascmPallet != null)
                    {
                        AscmSupplier ascmSupplier = AscmSupplierService.GetInstance().Get(ascmPallet.supplierId);
                        ascmPallet.supplier = ascmSupplier;
                        ascmPallet.startSn = ascmPallet.sn;
                        ascmPallet.endSn = ascmPallet.sn;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmPallet, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult PalletSave(AscmPallet ascmPallet_Model, string id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }

                if (id != null && id.Trim() != "")
                {
                    AscmPallet ascmPallet = AscmPalletService.GetInstance().Get(id);
                    if (ascmPallet == null)
                        throw new Exception("保存托盘失败！");
                    if (!string.IsNullOrEmpty(ascmPallet_Model.description))
                        ascmPallet.description = ascmPallet_Model.description.Trim();
                    ascmPallet.modifyUser = userName;
                    ascmPallet.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                    AscmPalletService.GetInstance().Update(ascmPallet);
                    jsonObjectResult.id = ascmPallet.sn;
                    jsonObjectResult.entity = ascmPallet;
                }
                else
                {
                    if (ascmPallet_Model.startSn == null || ascmPallet_Model.startSn.Trim() == "")
                        throw new Exception("开始编号不能为空");
                    if (ascmPallet_Model.endSn == null || ascmPallet_Model.endSn.Trim() == "")
                        throw new Exception("结束编号不能为空");
                    if (ascmPallet_Model.startSn.Trim().Length != ascmPallet_Model.endSn.Trim().Length)
                        throw new Exception("开始编号和结束编号长度不一致");
                    long startSn = 0, endSn = 0;
                    long.TryParse(ascmPallet_Model.startSn, out startSn);
                    long.TryParse(ascmPallet_Model.endSn, out endSn);
                    if (startSn == 0 || endSn == 0 || startSn > endSn)
                        throw new Exception("托盘编号输入错误");
                    if (endSn - startSn + 1 > 500)
                        throw new Exception("单次添加不能超过500");
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmRfid where id between '" + ascmPallet_Model.startSn.Trim() + "' and '" + ascmPallet_Model.endSn.Trim() + "'");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int count = 0;
                    if (int.TryParse(object1.ToString(), out count) && count > 0)
                        throw new Exception("已存在编号");

                    List<AscmPallet> listAscmPallet = new List<AscmPallet>();
                    List<AscmRfid> listAscmRfid = new List<AscmRfid>();
                    for (; startSn <= endSn; startSn++)
                    {
                        AscmPallet ascmPallet = new AscmPallet();
                        string sn = string.Format("{0:" + YnBaseClass2.Helper.StringHelper.Repeat('0', ascmPallet_Model.startSn.Trim().Length) + "}", startSn);
                        ascmPallet.sn = sn;
                        ascmPallet.rfid = sn;
                        ascmPallet.supplierId = ascmPallet_Model.supplierId;
                        if (!string.IsNullOrEmpty(ascmPallet_Model.description))
                            ascmPallet.description = ascmPallet_Model.description.Trim();
                        ascmPallet.status = ascmPallet_Model.status;
                        ascmPallet.createUser = userName;
                        ascmPallet.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmPallet.modifyUser = userName;
                        ascmPallet.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        listAscmPallet.Add(ascmPallet);

                        AscmRfid ascmRfid = new AscmRfid();
                        ascmRfid.id = sn;
                        ascmRfid.createUser = userName;
                        ascmRfid.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmRfid.modifyUser = userName;
                        ascmRfid.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmRfid.bindType = AscmRfid.BindTypeDefine.pallet;
                        ascmRfid.bindId = sn;
                        ascmRfid.status = AscmRfid.StatusDefine.inUse;
                        listAscmRfid.Add(ascmRfid);
                    }

                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            if (listAscmPallet.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmPallet);

                            if (listAscmRfid.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdateList(listAscmRfid);

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
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult PalletDelete(string startSn, string endSn)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (startSn == null || startSn.Trim() == "")
                    throw new Exception("开始编号不能为空");
                if (endSn == null || endSn.Trim() == "")
                    throw new Exception("结束编号不能为空");
                if (startSn.Trim().Length != endSn.Trim().Length)
                    throw new Exception("开始编号和结束编号长度不一致");
                long deleteStartSn = 0, deleteEndSn = 0;
                long.TryParse(startSn, out deleteStartSn);
                long.TryParse(endSn, out deleteEndSn);
                if (deleteStartSn == 0 || deleteEndSn == 0 || deleteStartSn > deleteEndSn)
                    throw new Exception("托盘编号输入错误");
                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmPalletDelivery where palletSn between '" + startSn.Trim() + "' and '" + endSn.Trim() + "'");
                if (object1 == null)
                    throw new Exception("查询异常！");
                int count = 0;
                if (int.TryParse(object1.ToString(), out count) && count > 0)
                    throw new Exception("不能删除已绑定物料的托盘");

                List<AscmPallet> listAscmPallet = AscmPalletService.GetInstance().GetList("from AscmPallet where sn between '" + startSn.Trim() + "' and '" + endSn.Trim() + "'");
                if (listAscmPallet != null && listAscmPallet.Count > 0)
                {
                    //foreach (AscmPallet ascmPallet in listAscmPallet)
                    //{
                    //    if (ascmPallet.status != AscmPallet.StatusDefine.unuse)
                    //        throw new Exception("不能删除此状态的托盘");
                    //}
                    List<AscmRfid> listAscmRfid = AscmRfidService.GetInstance().GetList("from AscmRfid where id between '" + startSn.Trim() + "' and '" + endSn.Trim() + "'");

                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            if (listAscmPallet != null && listAscmPallet.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmPallet);

                            if (listAscmRfid != null && listAscmRfid.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmRfid);

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
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 司机管理
        public ActionResult DriverIndex()
        {
            //供应商司机管理
            return View();
        }
        public ActionResult DriverList(int? page, int? rows, string sort, string order, string queryWord, int? supplierId, string status)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDriver> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "", whereStatus = "";
                if (supplierId.HasValue)
                    whereSupplier = "supplierId=" + supplierId.Value;
                if (!string.IsNullOrEmpty(status))
                    whereStatus = " status = '" + status + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);

                list = AscmDriverService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                if (list != null)
                {
                    foreach (AscmDriver ascmDriver in list)
                    {
                        jsonDataGridResult.rows.Add(ascmDriver);
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
        public ActionResult DriverEdit(int? id)
        {
            AscmDriver ascmDriver = null;
            try
            {
                if (id.HasValue)
                {
                    ascmDriver = AscmDriverService.GetInstance().Get(id.Value);
                    if (ascmDriver != null)
                    {
                        AscmSupplier ascmSupplier = AscmSupplierService.GetInstance().Get(ascmDriver.supplierId);
                        ascmDriver.supplier = ascmSupplier;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmDriver, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult DriverSave(AscmDriver ascmDriver_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }


                if (ascmDriver_Model.rfid == null || ascmDriver_Model.rfid == "")
                    throw new Exception("必须输入RFID编号");
                string rfid = ascmDriver_Model.rfid.Trim();
                AscmSupplier ascmSupplier = null;
                AscmDriver ascmDriver = null;
                AscmRfid ascmRfid = null;
                AscmRfid ascmRfidOld = null;
                if (id.HasValue)
                {
                    ascmDriver = AscmDriverService.GetInstance().Get(id.Value);
                    if (ascmDriver == null)
                        throw new Exception("保存司机失败！");
                    ascmDriver.modifyUser = userName;
                    ascmDriver.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmSupplier = AscmSupplierService.GetInstance().Get(ascmDriver.supplierId);
                    if (ascmDriver.rfid != rfid)
                    {
                        object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmRfid where id='" + rfid + "'");
                        if (object1 == null)
                            throw new Exception("查询异常！");
                        int count = 0;
                        if (int.TryParse(object1.ToString(), out count) && count > 0)
                            throw new Exception("已存在编号【" + rfid + "】");

                        ascmRfidOld = AscmRfidService.GetInstance().Get(ascmDriver.sn);
                        if (ascmRfidOld != null)
                        {
                            object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmDriverDelivery where driverSn='" + ascmRfidOld.id + "'");
                            if (object1 == null)
                                throw new Exception("查询异常！");
                            count = 0;
                            if (int.TryParse(object1.ToString(), out count) && count > 0)
                                throw new Exception("不能修改已绑定物料的司机编号");
                        }

                        ascmRfid = new AscmRfid();
                        ascmRfid.id = rfid;
                        ascmRfid.createUser = userName;
                        ascmRfid.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmRfid.modifyUser = userName;
                        ascmRfid.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmRfid.bindType = AscmRfid.BindTypeDefine.driver;
                        ascmRfid.bindId = rfid;
                        ascmRfid.status = AscmRfid.StatusDefine.inUse;
                    }
                }
                else
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmRfid where id='" + rfid + "'");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int count = 0;
                    if (int.TryParse(object1.ToString(), out count) && count > 0)
                        throw new Exception("已存在编号");
                    ascmSupplier = AscmSupplierService.GetInstance().Get(ascmDriver_Model.supplierId);
                    if (ascmSupplier == null)
                        throw new Exception("供应商不存在");

                    ascmDriver = new AscmDriver();
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmDriver");
                    ascmDriver.id = ++maxId;             
                    ascmDriver.supplierId = ascmDriver_Model.supplierId;
                    ascmDriver.status = ascmDriver_Model.status;
                    ascmDriver.createUser = userName;
                    ascmDriver.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmDriver.modifyUser = userName;
                    ascmDriver.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                    ascmRfid = new AscmRfid();
                    ascmRfid.id = rfid;
                    ascmRfid.createUser = userName;
                    ascmRfid.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmRfid.modifyUser = userName;
                    ascmRfid.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmRfid.bindType = AscmRfid.BindTypeDefine.driver;
                    ascmRfid.bindId = rfid;
                    ascmRfid.status = AscmRfid.StatusDefine.inUse;
                }

                if (!string.IsNullOrEmpty(ascmDriver_Model.name))
                {
                    ascmDriver.name = ascmDriver_Model.name.Trim();

                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(
                        "select count(*) from AscmDriver where supplierId=" + ascmDriver.supplierId + " and name='" + ascmDriver.name + "' and id<>" + ascmDriver.id);
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int count = 0;
                    if (int.TryParse(object1.ToString(), out count) && count > 0)
                        throw new Exception("供应商【" + ascmSupplier.name + "】中已存在司机【" + ascmDriver.name + "】");
                }

                ascmDriver.sn = rfid;
                ascmDriver.rfid = rfid;
                ascmDriver.sex = ascmDriver_Model.sex;
                if (!string.IsNullOrEmpty(ascmDriver_Model.idNumber))
                    ascmDriver.idNumber = ascmDriver_Model.idNumber.Trim();
                if (!string.IsNullOrEmpty(ascmDriver_Model.mobileTel))
                    ascmDriver.mobileTel = ascmDriver_Model.mobileTel.Trim();
                if (!string.IsNullOrEmpty(ascmDriver_Model.plateNumber))
                    ascmDriver.plateNumber = ascmDriver_Model.plateNumber.Trim();
                ascmDriver.load = ascmDriver_Model.load;
                if (!string.IsNullOrEmpty(ascmDriver_Model.description))
                    ascmDriver.description = ascmDriver_Model.description.Trim();
                ascmDriver.type = ascmDriver_Model.type;

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        if (ascmDriver != null)
                            YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdate(ascmDriver);

                        if (ascmRfid != null)
                            YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdate(ascmRfid);

                        if (ascmRfidOld != null)
                            YnDaoHelper.GetInstance().nHibernateHelper.Delete(ascmRfidOld);

                        tx.Commit();//正确执行提交
                        jsonObjectResult.result = true;
                        jsonObjectResult.id = ascmDriver.id.ToString();
                        jsonObjectResult.entity = ascmDriver;
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
        public ActionResult DriverDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    AscmDriver ascmDriver = AscmDriverService.GetInstance().Get(id.Value);
                    if (ascmDriver == null)
                        throw new Exception("找不到司机");
                    AscmRfid ascmRfid = AscmRfidService.GetInstance().Get(ascmDriver.rfid);
                    if (ascmRfid != null)
                    {
                        object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmDriverDelivery where driverSn='" + ascmRfid.id + "'");
                        if (object1 == null)
                            throw new Exception("查询异常！");
                        int count = 0;
                        if (int.TryParse(object1.ToString(), out count) && count > 0)
                            throw new Exception("不能删除已绑定物料的司机");
                    }

                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Delete(ascmDriver);

                            if (ascmRfid != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.Delete(ascmRfid);

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
        public ActionResult DriverAscxList(int? page, int? rows, string sort, string order, int? supplierId, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDriver> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "";
                if (supplierId.HasValue)
                    whereSupplier = "supplierId=" + supplierId.Value;

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);

                list = AscmDriverService.GetInstance().GetList(ynPage, "", "", q, whereOther);
                if (list != null)
                {
                    foreach (AscmDriver ascmDriver in list)
                    {
                        jsonDataGridResult.rows.Add(ascmDriver);
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

        #region 供应商送货合单管理
        public ActionResult SupplierDeliveryBatSumIndex()
        {
            //供应商到厂放行时长
            decimal supplierPassDuration = (decimal)0.5;
            decimal.TryParse(YnParameterService.GetInstance().GetValue(MyParameter.supplierPassDuration), out supplierPassDuration);

            ViewData["supplierPassDuration"] = supplierPassDuration * 60;//分钟

            if (User.Identity.IsAuthenticated)
            {
                AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(User.Identity.Name);
                if (ascmUserInfo.extExpandType == "erp")
                {
                    AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                    if (ascmUserInfo.supplierId.HasValue)
                    {
                        ViewData["supplierId"] = ascmUserInfo.supplierId.Value;
                    }
                }
                if (ascmUserInfo.ascmSupplier != null && ascmUserInfo.ascmSupplier.passDuration > 0)
                {
                    ViewData["supplierPassDuration"] = ascmUserInfo.ascmSupplier.passDuration * 60;//分钟
                }
            }
            
            //ViewData["appointmentStartTime"] = DateTime.Now.ToString("yyyy-MM-dd");
            //ViewData["appointmentEndTime"] = DateTime.Now.ToString("yyyy-MM-dd");

            return View();
        }
        public ActionResult SupplierDeliveryBatSumMainList(int? page, int? rows, string sort, string order, int? supplierId, string queryWord)
        {
            //YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            //ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            //ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDeliBatSumMain> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (supplierId.HasValue)
                {
                    string whereOther = "", whereSupplier = "", whereStatus = "";
                    whereSupplier = "supplierId=" + supplierId.Value;
                    //已确认合单保留显示一天
                    whereStatus = " (status is null or status='' or status='" + AscmDeliBatSumMain.StatusDefine.unConfirm
                                + "' or (status='" + AscmDeliBatSumMain.StatusDefine.confirm + "' and confirmTime >= '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00") + "'))";

                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);

                    list = AscmDeliBatSumMainService.GetInstance().GetList(sort, order, queryWord, whereOther);
                    if (list != null)
                    {
                        foreach (AscmDeliBatSumMain ascmDeliBatSumMain in list)
                        {
                            jsonDataGridResult.rows.Add(ascmDeliBatSumMain);
                        }
                    }
                    //jsonDataGridResult.total = ynPage.GetRecordCount();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SupplierDeliveryBatSumDetailList(string sort, string order, int? mainId)
        {
            //YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            //ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            //ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (mainId.HasValue)
                {
                    List<AscmDeliBatSumDetail> list = AscmDeliBatSumDetailService.GetInstance().GetListByMainId(mainId.Value);
                    AscmDeliBatSumDetailService.GetInstance().SetMaterial(list);
                    if (list != null)
                    {
                        list = list.OrderBy(P => P.containerBindNumber).ToList();
                        foreach (AscmDeliBatSumDetail ascmDeliBatSumDetail in list)
                        {
                            jsonDataGridResult.rows.Add(ascmDeliBatSumDetail);
                        }
                    }
                    //jsonDataGridResult.total = ynPage.GetRecordCount();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SupplierDeliveryBatSumDetailList1(string sort, string order, int? mainId,
            string startInventoryItemId, string endInventoryItemId, string deliveryNotifyNum, string batchDocNumber)
        {
            //YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            //ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            //ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (mainId.HasValue)
                {
                    string whereOther = "";

                    #region 物料编码
                    string whereStartInventoryItemId = "", whereEndInventoryItemId = "";
                    if (startInventoryItemId != null && startInventoryItemId != "")
                    {
                        whereStartInventoryItemId = "materialId in(select id from AscmMaterialItem where docNumber='" + startInventoryItemId.Trim() + "')";
                    }
                    if (endInventoryItemId != null && endInventoryItemId != "")
                    {
                        if (startInventoryItemId != null && startInventoryItemId != "")
                        {
                            whereStartInventoryItemId = "materialId in(select id from AscmMaterialItem where docNumber>='" + startInventoryItemId.Trim() + "' and docNumber<='" + endInventoryItemId.Trim() + "')";
                        }
                        else
                        {
                            whereEndInventoryItemId = "materialId in(select id from AscmMaterialItem where docNumber<='" + endInventoryItemId.Trim() + "')";
                        }
                    }
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartInventoryItemId);
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndInventoryItemId);
                    #endregion

                    #region 批单号
                    string wherebatchDocNumber = "";
                    if (batchDocNumber != null && batchDocNumber != "")
                    {
                        wherebatchDocNumber = "batchId in(select id from AscmDeliveryOrderBatch where docNumber like %'" + batchDocNumber.Trim() + "'%)";
                    }
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, wherebatchDocNumber);
                    #endregion

                    #region 送货通知单
                    //string wheredeliveryNotifyNum = "";
                    //if (deliveryNotifyNum != null && deliveryNotifyNum != "")
                    //{
                    //    wheredeliveryNotifyNum = "batchId =(select id from AscmDeliveryOrderBatch where docNumber like %'" + batchDocNumber.Trim() + "'%)";
                    //}
                    //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, wheredeliveryNotifyNum);
                    #endregion
                    List<AscmDeliBatSumDetail> list = AscmDeliBatSumDetailService.GetInstance().GetList("", "", mainId.Value, "", whereOther);
                    if (list != null)
                    {
                        foreach (AscmDeliBatSumDetail ascmDeliBatSumDetail in list)
                        {
                            jsonDataGridResult.rows.Add(ascmDeliBatSumDetail);
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
        public ActionResult SupplierDeliveryBatSumMainEdit(int? id)
        {
            AscmDeliBatSumMain ascmDeliBatSumMain = null;
            try
            {
                if (id.HasValue)
                {
                    ascmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().Get(id.Value);
                    if (ascmDeliBatSumMain != null)
                    {
                        AscmDriver ascmDriver = AscmDriverService.GetInstance().Get(ascmDeliBatSumMain.driverId);
                        ascmDeliBatSumMain.ascmDriver = ascmDriver;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmDeliBatSumMain, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult SupplierDeliveryBatSumMainSave(AscmDeliBatSumMain ascmDeliBatSumMain_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userId = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userId = User.Identity.Name;
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("用户错误！");
                AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(userId);
                if (ascmUserInfo.extExpandType != "erp")
                    throw new Exception("供应商用户错误！");
                AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                if (ascmUserInfo.ascmSupplier == null)
                    throw new Exception("供应商用户错误！");

                AscmDeliBatSumMain ascmDeliBatSumMain = null;
                if (id.HasValue)
                {
                    ascmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().Get(id.Value);
                    if (ascmDeliBatSumMain == null)
                        throw new Exception("保存供应商送货合单失败");
                    if (ascmDeliBatSumMain.status != AscmDeliBatSumMain.StatusDefine.unConfirm)
                        throw new Exception("供应商送货合单状态错误");
                    ascmDeliBatSumMain.modifyUser = userId;
                    ascmDeliBatSumMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    ////2013.11.28
                    ////一张司机卡和一个子库一次只能生成一张合单，在该合单被美的接收后才允许绑定第二张合单
                    //List<AscmDeliBatSumMain> listAscmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().GetList("from AscmDeliBatSumMain where driverId=" + ascmDeliBatSumMain_Model.driverId + " and status='" + AscmDeliBatSumMain.StatusDefine.unConfirm + "' and warehouseId='" + ascmDeliBatSumMain_Model.warehouseId+"'");
                    //if (listAscmDeliBatSumMain.Count>0)
                    //    throw new Exception("一张司机卡和一个子库一次只能生成一张合单，在该合单被美的接收后才允许绑定第二张合单");
                    ascmDeliBatSumMain = new AscmDeliBatSumMain();
                    int maxId = AscmDeliBatSumMainService.GetInstance().GetMaxId();
                    ascmDeliBatSumMain.id = ++maxId;
                    //2014-1-2 根据业务要求合单号与条码号统一
                    //ascmDeliBatSumMain.docNumber = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("AscmDeliBatSumMain" + ascmUserInfo.ascmSupplier.id, "", "yyyyMMdd", 2);
                    //ascmDeliBatSumMain.barcode = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("AscmDeliBatSumMainBarcode", "", "", 9);
                    ascmDeliBatSumMain.docNumber = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("AscmDeliBatSumMain", "HD", "yyMMdd", 5);
                    ascmDeliBatSumMain.barcode = ascmDeliBatSumMain.docNumber;
                    ascmDeliBatSumMain.createUser = userId;
                    ascmDeliBatSumMain.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmDeliBatSumMain.modifyUser = userId;
                    ascmDeliBatSumMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmDeliBatSumMain.status = AscmDeliBatSumMain.StatusDefine.unConfirm;
                }
                ascmDeliBatSumMain.warehouseId = ascmDeliBatSumMain_Model.warehouseId;
                ascmDeliBatSumMain.supplierId = ascmUserInfo.ascmSupplier.id;
                ascmDeliBatSumMain.driverId = ascmDeliBatSumMain_Model.driverId;

                if (id.HasValue)
                    AscmDeliBatSumMainService.GetInstance().Update(ascmDeliBatSumMain);
                else
                    AscmDeliBatSumMainService.GetInstance().Save(ascmDeliBatSumMain);
                jsonObjectResult.result = true;
                jsonObjectResult.id = ascmDeliBatSumMain.id.ToString();
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult SupplierDeliveryBatSumMainDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    AscmDeliBatSumMain ascmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().Get(id.Value);
                    if (ascmDeliBatSumMain == null)
                        throw new Exception("找不到供应商送货合单");
                    if (ascmDeliBatSumMain.status != AscmDeliBatSumMain.StatusDefine.unConfirm)
                        throw new Exception("供应商合单状态错误");
                    //List<AscmDeliBatSumDetail> listAscmDeliBatSumDetail = AscmDeliBatSumDetailService.GetInstance().GetListByMainId(id.Value);
                    List<AscmDeliBatSumDetail> listAscmDeliBatSumDetail = AscmDeliBatSumDetailService.GetInstance().GetList("from AscmDeliBatSumDetail where mainId=" + id);
                    List<AscmContainerDelivery> listAscmContainerDelivery = null;
                    List<AscmPalletDelivery> listAscmPalletDelivery = null;
                    List<AscmDriverDelivery> listAscmDriverDelivery = null;
                    if (listAscmDeliBatSumDetail != null && listAscmDeliBatSumDetail.Count > 0)
                    {
                        string batchIds = "";
                        foreach (AscmDeliBatSumDetail ascmDeliBatSumDetail in listAscmDeliBatSumDetail)
                        {
                            if (!string.IsNullOrEmpty(batchIds))
                                batchIds += ",";
                            batchIds += ascmDeliBatSumDetail.batchId;
                        }
                        if (!string.IsNullOrEmpty(batchIds))
                        {
                            IList<AscmContainerDelivery> ilistAscmContainerDelivery = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainerDelivery>("from AscmContainerDelivery where deliveryOrderBatchId in(" + batchIds + ")");
                            if (ilistAscmContainerDelivery != null)
                                listAscmContainerDelivery = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainerDelivery>(ilistAscmContainerDelivery);
                            IList<AscmPalletDelivery> ilistAscmPalletDelivery = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmPalletDelivery>("from AscmPalletDelivery where deliveryOrderBatchId in(" + batchIds + ")");
                            if (ilistAscmPalletDelivery != null)
                                listAscmPalletDelivery = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmPalletDelivery>(ilistAscmPalletDelivery);
                            IList<AscmDriverDelivery> ilistAscmDriverDelivery = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDriverDelivery>("from AscmDriverDelivery where deliveryOrderBatchId in(" + batchIds + ")");
                            if (ilistAscmDriverDelivery != null)
                                listAscmDriverDelivery = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDriverDelivery>(ilistAscmDriverDelivery);
                        }
                    }

                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Delete(ascmDeliBatSumMain);

                            if (listAscmDeliBatSumDetail != null && listAscmDeliBatSumDetail.Count > 0)
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmDeliBatSumDetail);

                            if (listAscmContainerDelivery != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmContainerDelivery);

                            if (listAscmPalletDelivery != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmPalletDelivery);

                            if (listAscmDriverDelivery != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmDriverDelivery);

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
        public ActionResult SupplierDeliveryBatSumDetailAdd(int? id, string deliveryOrderBatchJson, bool appointmentTimeFilter)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue && !string.IsNullOrEmpty(deliveryOrderBatchJson))
                {
                    double _supplierPassDuration = 0;
                    AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(User.Identity.Name);
                    AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                    if (ascmUserInfo.ascmSupplier != null)
                        _supplierPassDuration = ascmUserInfo.ascmSupplier.passDuration;

                    AscmDeliBatSumMain ascmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().Get(id.Value);
                    if (ascmDeliBatSumMain == null)
                        throw new Exception("找不到供应商送货合单");
                    if (ascmDeliBatSumMain.status != AscmDeliBatSumMain.StatusDefine.unConfirm)
                        throw new Exception("供应商合单状态错误");
                    List<AscmDeliveryOrderBatch> listAscmDeliveryOrderBatch = JsonConvert.DeserializeObject<List<AscmDeliveryOrderBatch>>(deliveryOrderBatchJson);
                    if (listAscmDeliveryOrderBatch != null && listAscmDeliveryOrderBatch.Count > 0)
                    {
                        //List<AscmDeliBatSumDetail> listAscmDeliBatSumDetail = AscmDeliBatSumDetailService.GetInstance().GetListByMainId(id.Value);
                        List<AscmDeliBatSumDetail> listAscmDeliBatSumDetail = AscmDeliBatSumDetailService.GetInstance().GetList("from AscmDeliBatSumDetail where mainId=" + id);
                        List<AscmDeliBatSumDetail> listAscmDeliBatSumDetailAdd = new List<AscmDeliBatSumDetail>();
                        int maxId = AscmDeliBatSumDetailService.GetInstance().GetMaxId();
                        foreach (AscmDeliveryOrderBatch ascmDeliveryOrderBatch in listAscmDeliveryOrderBatch)
                        {
                            AscmDeliBatSumDetail ascmDeliBatSumDetail = null;
                            if (listAscmDeliBatSumDetail != null && listAscmDeliBatSumDetail.Count > 0)
                                ascmDeliBatSumDetail = listAscmDeliBatSumDetail.Find(P => P.batchId == ascmDeliveryOrderBatch.id);
                            if (ascmDeliBatSumDetail == null)
                            {
                                ascmDeliBatSumDetail = new AscmDeliBatSumDetail();
                                ascmDeliBatSumDetail.id = ++maxId;
                                ascmDeliBatSumDetail.mainId = id.Value;
                                ascmDeliBatSumDetail.batchId = ascmDeliveryOrderBatch.id;
                                ascmDeliBatSumDetail.materialId = ascmDeliveryOrderBatch.materialIdTmp;
                                ascmDeliBatSumDetail.totalNumber = (int)ascmDeliveryOrderBatch.totalNumber;
                                ascmDeliBatSumDetail.appointmentStartTime = ascmDeliveryOrderBatch.appointmentStartTime;
                                ascmDeliBatSumDetail.appointmentEndTime = ascmDeliveryOrderBatch.appointmentEndTime;
                                /* 2014.3.14 合单中加入批单不改预约时间，取通知时间
                                #region 取送货通知预约时间
                                // 取送货通知预约时间
                                List<AscmDeliveryNotifyMain> list_AscmDeliveryNotifyMain = GetDeliveryNotifyList(ascmDeliveryOrderBatch.id.ToString());
                                string appointment_StartTime = "", appointment_EndTime = "";
                                AscmDeliBatSumMainService.GetInstance().GetAppointmentTime(list_AscmDeliveryNotifyMain, ref appointment_StartTime, ref appointment_EndTime);
                                if (list_AscmDeliveryNotifyMain != null && list_AscmDeliveryNotifyMain.Count() > 0)
                                {
                                    ascmDeliBatSumDetail.appointmentStartTime = appointment_StartTime;
                                    ascmDeliBatSumDetail.appointmentEndTime = appointment_EndTime;
                                }
                                #endregion
                                 * */
                                listAscmDeliBatSumDetailAdd.Add(ascmDeliBatSumDetail);
                            }
                        }
                        // 取送货通知预约时间
                        //List<AscmDeliveryNotifyMain> listAscmDeliveryNotifyMain = GetDeliveryNotifyList(listAscmDeliBatSumDetailAdd.Union(listAscmDeliBatSumDetail).ToList());
                        string appointmentStartTime = "", appointmentEndTime = "";
                        List<AscmDeliBatSumDetail> listAscmDeliBatSumDetailAll = new List<AscmDeliBatSumDetail>();
                        listAscmDeliBatSumDetailAll.AddRange(listAscmDeliBatSumDetail);
                        listAscmDeliBatSumDetailAll.AddRange(listAscmDeliBatSumDetailAdd);
                        AscmDeliBatSumMainService.GetInstance().GetAppointmentTime2(_supplierPassDuration, listAscmDeliBatSumDetailAll, ref appointmentStartTime, ref appointmentEndTime, !appointmentTimeFilter);
                        // 执行事务操作
                        //throw new Exception("error");
                        using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                        {
                            try
                            {
                                if (listAscmDeliBatSumDetailAdd.Count > 0)
                                    YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmDeliBatSumDetailAdd);

                                if (ascmDeliBatSumMain.appointmentStartTime != appointmentStartTime || ascmDeliBatSumMain.appointmentEndTime != appointmentEndTime)
                                {
                                    ascmDeliBatSumMain.appointmentStartTime = appointmentStartTime;
                                    ascmDeliBatSumMain.appointmentEndTime = appointmentEndTime;
                                    YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmDeliBatSumMain);
                                }

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
        public ActionResult SupplierDeliveryBatSumDetailRemove(int? id, string detailIds)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue && !string.IsNullOrEmpty(detailIds))
                {
                    double _supplierPassDuration = 0;
                    AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(User.Identity.Name);
                    AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                    if (ascmUserInfo.ascmSupplier != null)
                        _supplierPassDuration = ascmUserInfo.ascmSupplier.passDuration;

                    AscmDeliBatSumMain ascmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().Get(id.Value);
                    if (ascmDeliBatSumMain == null)
                        throw new Exception("找不到供应商送货合单");
                    if (ascmDeliBatSumMain.status != AscmDeliBatSumMain.StatusDefine.unConfirm)
                        throw new Exception("供应商合单状态错误");
                    //List<AscmDeliBatSumDetail> listAscmDeliBatSumDetailDelete =
                    //    AscmDeliBatSumDetailService.GetInstance().GetList("from AscmDeliBatSumDetail where mainId=" + id + " and id in(" + detailIds + ")");
                    //List<AscmDeliBatSumDetail> listAscmDeliBatSumDetail = AscmDeliBatSumDetailService.GetInstance().GetListByMainId(id.Value);
                    List<AscmDeliBatSumDetail> listAscmDeliBatSumDetail = AscmDeliBatSumDetailService.GetInstance().GetList("from AscmDeliBatSumDetail where mainId=" + id);
                    if (listAscmDeliBatSumDetail != null && listAscmDeliBatSumDetail.Count > 0)
                    {
                        List<AscmDeliBatSumDetail> listAscmDeliBatSumDetailDelete = new List<AscmDeliBatSumDetail>();
                        List<string> listDetailId = detailIds.Split(',').ToList();
                        string batchIds = "", batchIds2 = "";
                        foreach (AscmDeliBatSumDetail ascmDeliBatSumDetail in listAscmDeliBatSumDetail)
                        {
                            if (listDetailId.Contains(ascmDeliBatSumDetail.id.ToString()))
                            {
                                if (!string.IsNullOrEmpty(batchIds))
                                    batchIds += ",";
                                batchIds += ascmDeliBatSumDetail.batchId;
                                listAscmDeliBatSumDetailDelete.Add(ascmDeliBatSumDetail);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(batchIds2))
                                    batchIds2 += ",";
                                batchIds2 += ascmDeliBatSumDetail.batchId;
                            }
                        }
                        // 取送货通知预约时间
                        List<AscmDeliveryNotifyMain> listAscmDeliveryNotifyMain = GetDeliveryNotifyList(batchIds2);
                        string appointmentStartTime = "", appointmentEndTime = "";
                        //AscmDeliBatSumMainService.GetInstance().GetAppointmentTime(listAscmDeliveryNotifyMain, ref appointmentStartTime, ref appointmentEndTime);
                        List<AscmDeliBatSumDetail> listAscmDeliBatSumDetailAll = new List<AscmDeliBatSumDetail>();
                        listAscmDeliBatSumDetailAll.AddRange(listAscmDeliBatSumDetail);
                        for (int irow = listAscmDeliBatSumDetailAll.Count - 1; irow >= 0; irow--)
                        {
                            AscmDeliBatSumDetail _ascmDeliBatSumDetail = listAscmDeliBatSumDetailDelete.Find(P => P.id == listAscmDeliBatSumDetailAll[irow].id);
                            if (_ascmDeliBatSumDetail != null)
                                listAscmDeliBatSumDetailAll.RemoveAt(irow);
                        }
                        AscmDeliBatSumMainService.GetInstance().GetAppointmentTime2(_supplierPassDuration, listAscmDeliBatSumDetailAll, ref appointmentStartTime, ref appointmentEndTime,true);
                        if (listAscmDeliBatSumDetailAll.Count == 0)
                        {
                            appointmentStartTime = "";
                            appointmentEndTime = "";
                        }
                        // 删除容器、托盘、司机的绑定
                        List<AscmContainerDelivery> listAscmContainerDelivery = null;
                        List<AscmPalletDelivery> listAscmPalletDelivery = null;
                        List<AscmDriverDelivery> listAscmDriverDelivery = null;
                        if (!string.IsNullOrEmpty(batchIds))
                        {
                            IList<AscmContainerDelivery> ilistAscmContainerDelivery = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainerDelivery>("from AscmContainerDelivery where deliveryOrderBatchId in(" + batchIds + ")");
                            if (ilistAscmContainerDelivery != null)
                                listAscmContainerDelivery = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainerDelivery>(ilistAscmContainerDelivery);
                            IList<AscmPalletDelivery> ilistAscmPalletDelivery = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmPalletDelivery>("from AscmPalletDelivery where deliveryOrderBatchId in(" + batchIds + ")");
                            if (ilistAscmPalletDelivery != null)
                                listAscmPalletDelivery = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmPalletDelivery>(ilistAscmPalletDelivery);
                            IList<AscmDriverDelivery> ilistAscmDriverDelivery = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDriverDelivery>("from AscmDriverDelivery where deliveryOrderBatchId in(" + batchIds + ")");
                            if (ilistAscmDriverDelivery != null)
                                listAscmDriverDelivery = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDriverDelivery>(ilistAscmDriverDelivery);
                        }
                        // 执行事务操作
                        //throw new Exception("error");
                        using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                        {
                            try
                            {
                                if (listAscmDeliBatSumDetailDelete != null && listAscmDeliBatSumDetailDelete.Count > 0)
                                    YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmDeliBatSumDetailDelete);

                                if (listAscmContainerDelivery != null && listAscmContainerDelivery.Count > 0)
                                    YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmContainerDelivery);

                                if (listAscmPalletDelivery != null && listAscmPalletDelivery.Count > 0)
                                    YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmPalletDelivery);

                                if (listAscmDriverDelivery != null && listAscmDriverDelivery.Count > 0)
                                    YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmDriverDelivery);

                                if (ascmDeliBatSumMain.appointmentStartTime != appointmentStartTime || ascmDeliBatSumMain.appointmentEndTime != appointmentEndTime)
                                {
                                    ascmDeliBatSumMain.appointmentStartTime = appointmentStartTime;
                                    ascmDeliBatSumMain.appointmentEndTime = appointmentEndTime;
                                    YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmDeliBatSumMain);
                                }

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
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        private List<AscmDeliveryNotifyMain> GetDeliveryNotifyList(List<AscmDeliBatSumDetail> list)
        {
            string batchIds = "";
            if (list != null)
            {
                foreach (AscmDeliBatSumDetail ascmDeliBatSumDetail in list)
                {
                    if (!string.IsNullOrEmpty(batchIds))
                        batchIds += ",";
                    batchIds += ascmDeliBatSumDetail.batchId;
                }
            }
            return GetDeliveryNotifyList(batchIds);
        }
        private List<AscmDeliveryNotifyMain> GetDeliveryNotifyList(string batchIds)
        {
            List<AscmDeliveryNotifyMain> listAscmDeliveryNotifyMain = null;
            if (!string.IsNullOrEmpty(batchIds))
            {
                try
                {
                    string sql = "from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId in(" + batchIds + "))";
                    IList<AscmDeliveryOrderDetail> ilistDeliveryOrderDetail = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderDetail>(sql);
                    if (ilistDeliveryOrderDetail != null)
                    {
                        string notifyDetailId = "";
                        foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in ilistDeliveryOrderDetail)
                        {
                            if (!string.IsNullOrEmpty(notifyDetailId))
                                notifyDetailId += ",";
                            notifyDetailId += ascmDeliveryOrderDetail.notifyDetailId;
                        }
                        if (!string.IsNullOrEmpty(notifyDetailId))
                        {
                            sql = "from AscmDeliveryNotifyMain where id in (select mainId from AscmDeliveryNotifyDetail where id in(" + notifyDetailId + "))";
                            IList<AscmDeliveryNotifyMain> ilistDeliveryNotifyMain = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryNotifyMain>(sql);
                            if (ilistDeliveryNotifyMain != null)
                            {
                                listAscmDeliveryNotifyMain = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryNotifyMain>(ilistDeliveryNotifyMain);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return listAscmDeliveryNotifyMain;
        }
        /* 合单打印 */
        public JsonResult SupplierDeliveryBatSumPrint(int? id)
        {
            DeliveryBatSumMain deliveryBatSumMain = null;
            if (id.HasValue)
            {
                //供方简称
                string supplierShortName = "select vendorSiteCode from AscmSupplierAddress a where a.vendorSiteCodeAlt=s.docNumber and rownum=1";
                //司机编号
                string driverSn = "select sn from AscmDriver d where d.id=h.driverId";
                string hql = string.Format("select new AscmDeliBatSumMain(h,s.docNumber,({0}),({1})) from AscmDeliBatSumMain h,AscmSupplier s", supplierShortName, driverSn);
                string where = string.Empty;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "h.supplierId=s.id");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "h.id=" + id);
                hql += " where " + where;
                AscmDeliBatSumMain main = AscmDeliBatSumMainService.GetInstance().Get(hql);
                if (main != null)
                {
                    deliveryBatSumMain = GetDeliveryBatSumMain(main);
                    //容器绑定数量
                    string containerBindNumber = "select sum(quantity) from AscmContainerDelivery where deliveryOrderBatchId=l.batchId and batSumMainId=l.mainId";
                    //容器数量
                    string containerNumber = "select count(distinct containerSn) from AscmContainerDelivery where deliveryOrderBatchId=l.batchId and batSumMainId=l.mainId";
                    hql = string.Format("select new AscmDeliBatSumDetail(l,({0}),0M,0M,({1})) from AscmDeliBatSumDetail l", containerBindNumber, containerNumber);
                    where = string.Empty;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "mainId=" + id);
                    hql += " where " + where;
                    List<AscmDeliBatSumDetail> listDetail = AscmDeliBatSumDetailService.GetInstance().GetList(hql);
                    if (listDetail != null && listDetail.Count > 0)
                    {
                        deliveryBatSumMain.listDetail = new List<DeliveryBatSumDetail>();
                        //容器规格
                        hql = "select new AscmContainerDelivery(acs.spec,count(distinct ac.sn)) from AscmContainerDelivery acd,AscmContainer ac,AscmContainerSpec acs";
                        where = string.Empty;
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "acd.containerSn=ac.sn");
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "ac.specId=acs.id");
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "acd.batSumMainId=" + id);
                        hql += " where " + where;
                        hql += " group by acs.spec";
                        hql += " order by acs.spec";
                        List<AscmContainerDelivery> listContainerDelivery = AscmContainerDeliveryService.GetInstance().GetList(hql);
                        List<AscmContainerSpec> listascmContainerSpec = null;
                        if (listContainerDelivery != null)
                        {
                            foreach (AscmContainerDelivery containerDelivery in listContainerDelivery)
                            {
                              //if (!string.IsNullOrEmpty(deliveryBatSumMain.containers))
                              //      deliveryBatSumMain.containers += " ";
                                  deliveryBatSumMain.containers += containerDelivery.containerQuantity;
                                  hql = "from AscmContainerSpec where spec='"+containerDelivery.containerSpec+"'";
                                listascmContainerSpec = AscmContainerSpecService.GetInstance().GetList(hql);
                                deliveryBatSumMain.listContainerQuantity.Add(containerDelivery.containerQuantity);
                                deliveryBatSumMain.listContainerSpec.Add(containerDelivery.containerSpec);
                                deliveryBatSumMain.listContainerDescription.Add(listascmContainerSpec[0].description);
                            }
                        }

                        AscmDeliBatSumDetailService.GetInstance().SetDeliveryOrderBatch(listDetail, true);
                        AscmDeliBatSumDetailService.GetInstance().SetMaterial(listDetail);
                        AscmDeliBatSumDetailService.GetInstance().SetWipEntityName(listDetail);

                        deliveryBatSumMain.supplyWarehouse = listDetail.Where(P => !string.IsNullOrEmpty(P.batchSupperWarehouse)).FirstOrDefault().batchSupperWarehouse;
                        deliveryBatSumMain.deliveryPlace = listDetail.Where(P => !string.IsNullOrEmpty(P.batchWipLine)).FirstOrDefault().batchWipLine;
                        deliveryBatSumMain.deliveryPlace = MideaAscm.Security.Utility.GetSubString(deliveryBatSumMain.deliveryPlace, 8);

                        //更新单据条码
                        List<AscmDeliBatSumDetail> listDetailUpdate = new List<AscmDeliBatSumDetail>(); 

                        int i = 0;
                        decimal prePageIndex = decimal.Zero, pageRowCount = 7M;
                        //按物料编码排序
                        listDetail = listDetail.OrderBy(P => P.materialDocNumber).ToList();
                        string barcode = string.Empty;
                        string materialDocNumber = string.Empty;
                        foreach (AscmDeliBatSumDetail detail in listDetail)
                        {
                            decimal pageIndex = Math.Floor(i / pageRowCount) + 1;
                            if (prePageIndex != pageIndex)
                            {
                                prePageIndex = pageIndex;
                                if (!string.IsNullOrEmpty(detail.barcode))
                                    barcode = detail.barcode;
                                else
                                    barcode = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("DeliBatSumBarcode", "", "", 8);
                            }
                            if (detail.barcode != barcode)
                            {
                                detail.barcode = barcode;
                                listDetailUpdate.Add(detail);
                            }
                            i++;
                            //统计相同种物料的数量
                            decimal totalQuantity = 0;
                            if (materialDocNumber != detail.materialDocNumber)
                            {
                                materialDocNumber = detail.materialDocNumber;
                                totalQuantity = listDetail.Where(P => P.materialDocNumber == detail.materialDocNumber).Sum(P => P.totalNumber);
                            }
                            deliveryBatSumMain.listDetail.Add(GetDeliveryBatSumDetail(detail, totalQuantity));
                        }

                        if (listDetailUpdate.Count > 0)
                        {
                            ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                            session.Clear();
                            using (ITransaction tx = session.BeginTransaction())
                            {
                                try
                                {
                                    listDetailUpdate.ForEach(P => session.Merge(P));
                                    tx.Commit();
                                }
                                catch (Exception ex)
                                {
                                    tx.Rollback();
                                    YnBaseClass2.Helper.LogHelper.GetLog().Error("合单条码更新失败(Update AscmDeliBatSumDetail)", ex);
                                }
                            }
                        }
                    }
                }
            }
            return Json(new { success = true, data = deliveryBatSumMain }, JsonRequestBehavior.AllowGet);
        }
        public DeliveryBatSumMain GetDeliveryBatSumMain(AscmDeliBatSumMain main)
        {
            DeliveryBatSumMain deliveryBatSumMain = null;
            if (main != null)
            {
                deliveryBatSumMain = new DeliveryBatSumMain();
                deliveryBatSumMain.id = main.id;
                deliveryBatSumMain.docNumber = main.docNumber == null ? string.Empty : main.docNumber;
                deliveryBatSumMain.supplierShortName = main.supplierNameCn == null ? string.Empty : main.supplierNameCn;
                deliveryBatSumMain.supplierDocNumber = main.supplierDocNumberCn == null ? string.Empty : main.supplierDocNumberCn;
                deliveryBatSumMain.driverSn = main.driverSnCn == null ? string.Empty : main.driverSnCn;
                deliveryBatSumMain.warehouse = main.warehouseId == null ? string.Empty : main.warehouseId;
                deliveryBatSumMain.deliveryPlace = string.Empty;
                deliveryBatSumMain.appointmentStartTime = main.appointmentStartTimeShow;
                deliveryBatSumMain.appointmentEndTime = main.appointmentEndTimeShow;
                deliveryBatSumMain.containers = 0;
                deliveryBatSumMain.listContainerQuantity = new List<long>();
                deliveryBatSumMain.listContainerSpec = new List<string>();
                deliveryBatSumMain.listContainerDescription = new List<string>();
                deliveryBatSumMain.status = main.statusCn == null ? string.Empty : main.statusCn;

                deliveryBatSumMain.supplierShortName = MideaAscm.Security.Utility.GetSubString(deliveryBatSumMain.supplierShortName, 16);
            }
            return deliveryBatSumMain;
        }
        public DeliveryBatSumDetail GetDeliveryBatSumDetail(AscmDeliBatSumDetail detail, decimal totalQuantity)
        {
            DeliveryBatSumDetail deliveryBatSumDetail = null;
            if (detail != null)
            {
                deliveryBatSumDetail = new DeliveryBatSumDetail();
                deliveryBatSumDetail.id = detail.id;
                deliveryBatSumDetail.mainId = detail.mainId;
                deliveryBatSumDetail.barcode = detail.barcode == null ? string.Empty : detail.barcode;
                deliveryBatSumDetail.batchBarCode = detail.batchBarCode == null ? string.Empty : detail.batchBarCode;
                deliveryBatSumDetail.batchDocNumber = detail.batchDocNumber == null ? string.Empty : detail.batchDocNumber;
                deliveryBatSumDetail.materialDocNumber = detail.materialDocNumber == null ? string.Empty : detail.materialDocNumber;
                deliveryBatSumDetail.materialDescription = detail.materialDescription == null ? string.Empty : detail.materialDescription;
                deliveryBatSumDetail.materialUnit = detail.materialUnit == null ? string.Empty : detail.materialUnit;
                deliveryBatSumDetail.deliveryNumber = detail.totalNumber;
                deliveryBatSumDetail.containerBindNumber = detail.containerBindNumber;
                deliveryBatSumDetail.containerNumber = detail.containerNumber;
                deliveryBatSumDetail.comment = detail.batchComments == null ? string.Empty : detail.batchComments;
                deliveryBatSumDetail.wipEntities = detail.wipEntityNames == null ? string.Empty : detail.wipEntityNames;
                deliveryBatSumDetail.totalQuantity = totalQuantity;

                deliveryBatSumDetail.materialDescription = MideaAscm.Security.Utility.GetSubString(deliveryBatSumDetail.materialDescription, 50, "...");
                deliveryBatSumDetail.comment = MideaAscm.Security.Utility.GetSubString(deliveryBatSumDetail.comment, 10, "...");
                deliveryBatSumDetail.wipEntities = MideaAscm.Security.Utility.GetSubString(deliveryBatSumDetail.wipEntities, 70, "...");
            }
            return deliveryBatSumDetail;
        }
        public class DeliveryBatSumMain
        {
            public int id { get; set; }
            public string docNumber { get; set; }
            public string supplierShortName { get; set; }
            public string supplierDocNumber { get; set; }
            public string driverSn { get; set; }
            public string supplyWarehouse { get; set; }
            public string warehouse { get; set; }
            public string deliveryPlace { get; set; }
            public string appointmentStartTime { get; set; }
            public string appointmentEndTime { get; set; }
            public long containers { get; set; }
            public List<long> listContainerQuantity { get; set; }
            public List<string> listContainerSpec { get; set; }
            public List<string> listContainerDescription { get; set; }
            public string status { get; set; }

            public List<DeliveryBatSumDetail> listDetail { get; set; }
        }
        public class DeliveryBatSumDetail
        {
            public int id { get; set; }
            public int mainId { get; set; }
            public string barcode { get; set; }
            public string batchBarCode { get; set; }
            public string batchDocNumber { get; set; }
            public string materialDocNumber { get; set; }
            public string materialDescription { get; set; }
            public string materialUnit { get; set; }
            public decimal deliveryNumber { get; set; }
            public decimal containerBindNumber { get; set; }
            public decimal containerNumber { get; set; }
            public string comment { get; set; }
            public string wipEntities { get; set; }
            public decimal totalQuantity { get; set; }
        }
        #endregion

        #region 供应商送货
        public ActionResult SupplierDeliveryBatSumConfirm()
        {
            if (User.Identity.IsAuthenticated)
            {
                AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(User.Identity.Name);
                if (ascmUserInfo.extExpandType == "erp")
                {
                    AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                    if (ascmUserInfo.supplierId.HasValue)
                    {
                        ViewData["supplierId"] = ascmUserInfo.supplierId.Value;
                    }
                }
            }

            return View();
        }
        public ActionResult SupplierDeliveryBatSumConfirmSubmit(int? id)
        {
            //确认
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    double _supplierPassDuration = 0;
                    AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(User.Identity.Name);
                    AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                    if (ascmUserInfo.ascmSupplier != null)
                        _supplierPassDuration = ascmUserInfo.ascmSupplier.passDuration;

                    AscmDeliBatSumMain ascmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().Get(id.Value);
                    if (ascmDeliBatSumMain == null)
                        throw new Exception("供应商合单不存在");
                    if (ascmDeliBatSumMain.status != AscmDeliBatSumMain.StatusDefine.unConfirm)
                        throw new Exception("供应商合单状态错误");
                    string _whereOther = "driverId=" + ascmDeliBatSumMain.driverId + "";
                    _whereOther += " and confirmTime is not null and confirmTime>='" + DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd") + "' and confirmTime<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'";
                    _whereOther += " and status<>'" + AscmDeliBatSumMain.StatusDefine.inPlant + "' and  status<>'" + AscmDeliBatSumMain.StatusDefine.unConfirm + "'";
                    int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmDeliBatSumMain where " + _whereOther);
                    if (count > 0)
                        throw new Exception("此司机卡有未接受单据，不能再次确认");
                    //if (ascmDeliBatSumMain.totalNumber != ascmDeliBatSumMain.containerBindNumber + ascmDeliBatSumMain.palletBindNumber + ascmDeliBatSumMain.driverBindNumber)
                    //    throw new Exception("合单总数量与绑定数量总和不相等");

                    string userId = string.Empty;
                    if (User.Identity.IsAuthenticated)
                        userId = User.Identity.Name;
                    ascmDeliBatSumMain.modifyUser = userId;
                    ascmDeliBatSumMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmDeliBatSumMain.confirmor = userId;
                    ascmDeliBatSumMain.confirmTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmDeliBatSumMain.status = AscmDeliBatSumMain.StatusDefine.confirm;

                    List<AscmAppointmentDistribution> listAscmAppointmentDistribution=null;
                    AscmAppointmentDistributionService.GetInstance().GetAppointmentDistribution(_supplierPassDuration, new List<AscmDeliBatSumMain>{ascmDeliBatSumMain}, ref listAscmAppointmentDistribution);
                    if (listAscmAppointmentDistribution != null)
                    {
                        int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmAppointmentDistribution");
                        foreach (AscmAppointmentDistribution ascmAppointmentDistribution in listAscmAppointmentDistribution)
                        {
                            if (ascmAppointmentDistribution.id == 0)
                            {
                                maxId++;
                                ascmAppointmentDistribution.id = maxId;
                            }

                        }
                    }
                    //throw new Exception("error");
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmDeliBatSumMain>(ascmDeliBatSumMain);
                            /*2014.3.14 取消自动分配
                            if (listAscmAppointmentDistribution!=null)
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdateList(listAscmAppointmentDistribution);
                             * */
                            tx.Commit();//正确执行提交
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            throw ex;
                        }
                    }

                    jsonObjectResult.result = true;
                    jsonObjectResult.id = ascmDeliBatSumMain.id.ToString();
                    jsonObjectResult.message = "";
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SupplierDeliveryBatSumDriverConfirmSubmit_OLd(int? driverId)
        {
            //确认
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (driverId.HasValue)
                {
                    double _supplierPassDuration = 0;
                    AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(User.Identity.Name);
                    AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                    if (ascmUserInfo.ascmSupplier != null)
                        _supplierPassDuration = ascmUserInfo.ascmSupplier.passDuration;

                    string userId = string.Empty;
                    if (User.Identity.IsAuthenticated)
                        userId = User.Identity.Name;
                    string _whereOther = "driverId=" + driverId.Value + "";
                    _whereOther += " and confirmTime is not null and confirmTime>='" + DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd") + "' and confirmTime<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'";
                    _whereOther += " and status<>'" + AscmDeliBatSumMain.StatusDefine.inPlant + "' and  status<>'" + AscmDeliBatSumMain.StatusDefine.unConfirm + "'";
                    int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmDeliBatSumMain where " + _whereOther);
                    if (count > 0)
                        throw new Exception("此司机卡有未接受单据，不能再次确认");

                    //List<AscmDeliBatSumMain> listAscmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().GetList("from AscmDeliBatSumMain where driverId=" + driverId.Value + " and status='" + AscmDeliBatSumMain.StatusDefine.unConfirm + "'");
                    string whereOther = "driverId=" + driverId.Value + " and status='" + AscmDeliBatSumMain.StatusDefine.unConfirm + "'";
                    List<AscmDeliBatSumMain> listAscmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().GetList("", "", "", whereOther);
                    foreach (AscmDeliBatSumMain ascmDeliBatSumMain in listAscmDeliBatSumMain)
                    {
                        //2014.1.2之前做过一个限制。必须绑定容器才能确认送货，现在是有部分供应商是推预约到货的，不需要绑定容器。就是对这部分供应商不需要做这个限制 刘涛让取消此限制
                        //if (ascmDeliBatSumMain.totalNumber != ascmDeliBatSumMain.containerBindNumber + ascmDeliBatSumMain.palletBindNumber + ascmDeliBatSumMain.driverBindNumber)
                        //    throw new Exception("合单总数量与绑定数量总和不相等");

                        ascmDeliBatSumMain.modifyUser = userId;
                        ascmDeliBatSumMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmDeliBatSumMain.confirmor = userId;
                        ascmDeliBatSumMain.confirmTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmDeliBatSumMain.status = AscmDeliBatSumMain.StatusDefine.confirm;
                    }
                    List<AscmAppointmentDistribution> listAscmAppointmentDistribution = null;
                    AscmAppointmentDistributionService.GetInstance().GetAppointmentDistribution(_supplierPassDuration, listAscmDeliBatSumMain, ref listAscmAppointmentDistribution);
                    if (listAscmAppointmentDistribution != null)
                    {
                        int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmAppointmentDistribution");
                        foreach (AscmAppointmentDistribution ascmAppointmentDistribution in listAscmAppointmentDistribution)
                        {
                            if (ascmAppointmentDistribution.id == 0)
                            {
                                maxId++;
                                ascmAppointmentDistribution.id = maxId;
                            }

                        }
                    }

                    //throw new Exception("error");
                    ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        try
                        {
                            if (listAscmDeliBatSumMain != null)
                                listAscmDeliBatSumMain.ForEach(P => session.Merge(P));
                            if (listAscmAppointmentDistribution != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdateList(listAscmAppointmentDistribution);
                            tx.Commit();//正确执行提交
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            throw ex;
                        }
                    }

                    jsonObjectResult.result = true;
                    //jsonObjectResult.id = ascmDeliBatSumMain.id.ToString();
                    jsonObjectResult.message = "";
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SupplierDeliveryBatSumDriverConfirmSubmit(int? driverId, string batchSumIds)
        {
            //确认
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (driverId.HasValue)
                {
                    double _supplierPassDuration = 0;
                    AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(User.Identity.Name);
                    AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                    if (ascmUserInfo.ascmSupplier != null)
                        _supplierPassDuration = ascmUserInfo.ascmSupplier.passDuration;

                    string userId = string.Empty;
                    if (User.Identity.IsAuthenticated)
                        userId = User.Identity.Name;

                    string _whereOther = "driverId=" + driverId.Value + "";
                    _whereOther += " and confirmTime is not null and confirmTime>='" + DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd") + "' and confirmTime<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'";
                    _whereOther += " and status<>'" + AscmDeliBatSumMain.StatusDefine.inPlant + "' and  status<>'" + AscmDeliBatSumMain.StatusDefine.unConfirm + "'";
                    int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmDeliBatSumMain where " + _whereOther);
                    if (count > 0)
                        throw new Exception("此司机卡有未接受单据，不能再次确认");

                    //List<AscmDeliBatSumMain> listAscmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().GetList("from AscmDeliBatSumMain where driverId=" + driverId.Value + " and status='" + AscmDeliBatSumMain.StatusDefine.unConfirm + "'");
                    string[] listBatchSumId = batchSumIds.Split(',');
                    string whereBatchSumId = "";
                    foreach (string batchSumId in listBatchSumId)
                    {
                        if (string.IsNullOrEmpty(batchSumId))
                            continue;
                        if (whereBatchSumId != "")
                            whereBatchSumId += ",";
                        whereBatchSumId += batchSumId;
                    }
                    if (string.IsNullOrEmpty(whereBatchSumId))
                        throw new Exception("没有选择合单，不能确认");
                    string whereOther = "status='" + AscmDeliBatSumMain.StatusDefine.unConfirm + "' and id in (" + whereBatchSumId + ")";

                    List<AscmDeliBatSumMain> listAscmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().GetList("", "", "", whereOther);
                    if (listAscmDeliBatSumMain.Count == 0)
                        throw new Exception("没有找到合单");
                    foreach (AscmDeliBatSumMain ascmDeliBatSumMain in listAscmDeliBatSumMain)
                    {
                        //2014.1.2之前做过一个限制。必须绑定容器才能确认送货，现在是有部分供应商是推预约到货的，不需要绑定容器。就是对这部分供应商不需要做这个限制 刘涛让取消此限制
                        //if (ascmDeliBatSumMain.totalNumber != ascmDeliBatSumMain.containerBindNumber + ascmDeliBatSumMain.palletBindNumber + ascmDeliBatSumMain.driverBindNumber)
                        //    throw new Exception("合单总数量与绑定数量总和不相等");

                        ascmDeliBatSumMain.modifyUser = userId;
                        ascmDeliBatSumMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmDeliBatSumMain.confirmor = userId;
                        ascmDeliBatSumMain.driverId = driverId.Value;
                        ascmDeliBatSumMain.confirmTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmDeliBatSumMain.status = AscmDeliBatSumMain.StatusDefine.confirm;
                    }
                    List<AscmAppointmentDistribution> listAscmAppointmentDistribution = null;
                    AscmAppointmentDistributionService.GetInstance().GetAppointmentDistribution(_supplierPassDuration, listAscmDeliBatSumMain, ref listAscmAppointmentDistribution);
                    if (listAscmAppointmentDistribution != null)
                    {
                        int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmAppointmentDistribution");
                        foreach (AscmAppointmentDistribution ascmAppointmentDistribution in listAscmAppointmentDistribution)
                        {
                            if (ascmAppointmentDistribution.id == 0)
                            {
                                maxId++;
                                ascmAppointmentDistribution.id = maxId;
                            }

                        }
                    }

                    //throw new Exception("error");
                    ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        try
                        {
                            if (listAscmDeliBatSumMain != null)
                                listAscmDeliBatSumMain.ForEach(P => session.Merge(P));
                            if (listAscmAppointmentDistribution != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdateList(listAscmAppointmentDistribution);
                            tx.Commit();//正确执行提交
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            throw ex;
                        }
                    }

                    jsonObjectResult.result = true;
                    //jsonObjectResult.id = ascmDeliBatSumMain.id.ToString();
                    jsonObjectResult.message = "";
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SupplierDeliveryBatSumQuery()
        {
            int supplierId = -1;
            if (User.Identity.IsAuthenticated)
            {
                AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(User.Identity.Name);
                if (ascmUserInfo.extExpandType == "erp")
                {
                    AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                    if (ascmUserInfo.supplierId.HasValue)
                    {
                        supplierId = ascmUserInfo.supplierId.Value;
                    }
                }
            }
            ViewData["supplierId"] = supplierId;

            return View();
        }
        public ActionResult DeliveryBatSumQuery()
        {
            return View();
        }
        public ActionResult DeliveryBatSumQueryList(int? page, int? rows, string sort, string order, int? supplierId, string queryWord,
            string startCreateTime, string endCreateTime, string status, int? driverId, string batchSumBarCode)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmDeliBatSumMain> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "", whereStatus = "";
                string whereStartCreateTime = "", whereEndCreateTime = "", whereDriver = "";

                if (supplierId.HasValue)
                    whereSupplier = " supplierId=" + supplierId.Value;
                if (!string.IsNullOrEmpty(status))
                {
                    foreach (string itemStatus in status.Split(','))
                    {
                        if (!string.IsNullOrEmpty(whereStatus))
                            whereStatus += " or ";
                        whereStatus += " status='" + itemStatus + "'";
                    }
                }
                if (driverId.HasValue)
                    whereDriver = " driverId=" + driverId.Value;

                DateTime dtStartCreateTime, dtEndCreateTime;
                if (!string.IsNullOrEmpty(startCreateTime) && DateTime.TryParse(startCreateTime, out dtStartCreateTime))
                    whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(endCreateTime) && DateTime.TryParse(endCreateTime, out dtEndCreateTime))
                    whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereDriver);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);
                if (batchSumBarCode != null && batchSumBarCode.Trim() != "")
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "barcode='" + batchSumBarCode.Trim() + "'");

                list = AscmDeliBatSumMainService.GetInstance().GetList(ynPage, sort, order, queryWord, whereOther);
                if (list != null)
                {
                    foreach (AscmDeliBatSumMain ascmDeliBatSumMain in list)
                    {
                        jsonDataGridResult.rows.Add(ascmDeliBatSumMain);
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
        public ActionResult SupplierDeliveryBatSumRecall(int? id) 
        {
            //召回
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    AscmDeliBatSumMain ascmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().Get(id.Value);
                    if (ascmDeliBatSumMain == null)
                        throw new Exception("供应商合单不存在");
                    if (ascmDeliBatSumMain.status != AscmDeliBatSumMain.StatusDefine.confirm)
                        throw new Exception("供应商合单状态错误");

                    string userId = string.Empty;
                    if (User.Identity.IsAuthenticated)
                        userId = User.Identity.Name;
                    ascmDeliBatSumMain.modifyUser = userId;
                    ascmDeliBatSumMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ascmDeliBatSumMain.status = AscmDeliBatSumMain.StatusDefine.unConfirm;
                    AscmDeliBatSumMainService.GetInstance().Update(ascmDeliBatSumMain);

                    jsonObjectResult.result = true;
                    jsonObjectResult.id = ascmDeliBatSumMain.id.ToString();
                    jsonObjectResult.message = "";
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SupplierDeliveryBatSumRecallByDriver(int? id)
        {
            //召回
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    string whereOther = "driverId=" + id.Value + " and status='" + AscmDeliBatSumMain.StatusDefine.confirm + "'";
                    string sql = "from AscmDeliBatSumMain where " + whereOther;
                    List<AscmDeliBatSumMain> listAscmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().GetList(sql);

                    //AscmDeliBatSumMain ascmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().Get(id.Value);
                    if (listAscmDeliBatSumMain == null || listAscmDeliBatSumMain.Count == 0)
                        throw new Exception("供应商司机卡下已确定合单不存在");
                    //if (ascmDeliBatSumMain.status != AscmDeliBatSumMain.StatusDefine.confirm)
                        //throw new Exception("供应商合单状态错误");

                    string userId = string.Empty;
                    if (User.Identity.IsAuthenticated)
                        userId = User.Identity.Name;
                    foreach (AscmDeliBatSumMain ascmDeliBatSumMain in listAscmDeliBatSumMain)
                    {
                        //2014.3.23改为司机卡召回

                        ascmDeliBatSumMain.modifyUser = userId;
                        ascmDeliBatSumMain.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmDeliBatSumMain.confirmor = "";
                        ascmDeliBatSumMain.driverId = 0;
                        ascmDeliBatSumMain.confirmTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        ascmDeliBatSumMain.status = AscmDeliBatSumMain.StatusDefine.unConfirm;
                    }
                    //throw new Exception("error");
                    ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        try
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmDeliBatSumMain);
                            tx.Commit();//正确执行提交
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            throw ex;
                        }
                    }
                    jsonObjectResult.result = true;
                    //jsonObjectResult.id = ascmDeliBatSumMain.id.ToString();
                    jsonObjectResult.message = "";
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        // 送货合单查询报表导出
        public ActionResult DeliveryBatSumExport(int? supplierId, string queryWord,
            string startCreateTime, string endCreateTime, string status, int? driverId, string batchSumBarCode)
        {
            IWorkbook wb = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Sheet1");
            sheet.SetColumnWidth(0, 16 * 256);
            sheet.SetColumnWidth(1, 14 * 256);
            sheet.SetColumnWidth(2, 26 * 256);
            sheet.SetColumnWidth(3, 10 * 256);
            sheet.SetColumnWidth(4, 17 * 256);
            sheet.SetColumnWidth(5, 17 * 256);
            sheet.SetColumnWidth(6, 10 * 256);
            sheet.SetColumnWidth(7, 12 * 256);
            sheet.SetColumnWidth(8, 10 * 256);
            sheet.SetColumnWidth(9, 17 * 256);
            sheet.SetColumnWidth(10, 17 * 256);
            sheet.SetColumnWidth(11, 17 * 256);

            IRow titleRow = sheet.CreateRow(0);
            titleRow.Height = 20 * 20;
            titleRow.CreateCell(0).SetCellValue("合单号");
            titleRow.CreateCell(1).SetCellValue("司机编号");
            titleRow.CreateCell(2).SetCellValue("供应商名称");
            titleRow.CreateCell(3).SetCellValue("状态");
            titleRow.CreateCell(4).SetCellValue("预约开始时间");
            titleRow.CreateCell(5).SetCellValue("预约最后时间");
            titleRow.CreateCell(6).SetCellValue("总数量");
            titleRow.CreateCell(7).SetCellValue("容器绑定数量");
            titleRow.CreateCell(8).SetCellValue("确认人");
            titleRow.CreateCell(9).SetCellValue("确认时间");
            titleRow.CreateCell(10).SetCellValue("到厂时间");
            titleRow.CreateCell(11).SetCellValue("接收时间");

            string fileDownloadName = "供应商送货合单查询报表";

            string whereOther = "", whereSupplier = "", whereStatus = "";
            string whereStartCreateTime = "", whereEndCreateTime = "", whereDriver = "";

            if (supplierId.HasValue)
                whereSupplier = " supplierId=" + supplierId.Value;
            if (!string.IsNullOrEmpty(status))
            {
                foreach (string itemStatus in status.Split(','))
                {
                    if (!string.IsNullOrEmpty(whereStatus))
                        whereStatus += " or ";
                    whereStatus += " status='" + itemStatus + "'";
                }
            }
            if (driverId.HasValue)
                whereDriver = " driverId=" + driverId.Value;

            DateTime dtStartCreateTime, dtEndCreateTime;
            if (!string.IsNullOrEmpty(startCreateTime) && DateTime.TryParse(startCreateTime, out dtStartCreateTime))
                whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
            if (!string.IsNullOrEmpty(endCreateTime) && DateTime.TryParse(endCreateTime, out dtEndCreateTime))
                whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereDriver);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);
            if (batchSumBarCode != null && batchSumBarCode.Trim() != "")
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "barcode='" + batchSumBarCode.Trim() + "'");
            List<AscmDeliBatSumMain> list = AscmDeliBatSumMainService.GetInstance().GetList(null, string.Empty, string.Empty, queryWord, whereOther);
            if (list != null)
            {
                int rowIndex = 0;
                foreach (AscmDeliBatSumMain deliBatSumMain in list)
                {
                    IRow row = sheet.CreateRow(++rowIndex);
                    row.Height = 20 * 20;
                    row.CreateCell(0).SetCellValue(deliBatSumMain.docNumber);
                    row.CreateCell(1).SetCellValue(deliBatSumMain.driverSn);
                    row.CreateCell(2).SetCellValue(deliBatSumMain.supplierName);
                    row.CreateCell(3).SetCellValue(deliBatSumMain.statusCn);
                    row.CreateCell(4).SetCellValue(deliBatSumMain.appointmentStartTimeShow);
                    row.CreateCell(5).SetCellValue(deliBatSumMain.appointmentEndTimeShow);
                    row.CreateCell(6).SetCellValue(deliBatSumMain.totalNumber.ToString());
                    row.CreateCell(7).SetCellValue(deliBatSumMain.containerBindNumber.ToString());
                    row.CreateCell(8).SetCellValue(deliBatSumMain.confirmor);
                    row.CreateCell(9).SetCellValue(deliBatSumMain._confirmTime);
                    row.CreateCell(10).SetCellValue(deliBatSumMain._toPlantTime);
                    row.CreateCell(11).SetCellValue(deliBatSumMain._acceptTime);
                }
            }

            byte[] buffer = new byte[] { };
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                wb.Write(stream);
                buffer = stream.GetBuffer();
            }
            return File(buffer, "application/vnd.ms-excel", fileDownloadName + ".xls");
        }
        // 送货批单明细报表导出
        public ActionResult DeliveryBatDetailExport(int? mainId)
        {
            IWorkbook wb = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Sheet1");
            sheet.SetColumnWidth(0, 16 * 256);
            sheet.SetColumnWidth(1, 12 * 256);
            sheet.SetColumnWidth(2, 16 * 256);
            sheet.SetColumnWidth(3, 30 * 256);
            sheet.SetColumnWidth(4, 17 * 256);
            sheet.SetColumnWidth(5, 12 * 256);
            sheet.SetColumnWidth(6, 12 * 256);
            sheet.SetColumnWidth(7, 10 * 256);
            sheet.SetColumnWidth(8, 12 * 256);
            sheet.SetColumnWidth(9, 10 * 256);
            sheet.SetColumnWidth(10, 12 * 256);

            IRow titleRow = sheet.CreateRow(0);
            titleRow.Height = 20 * 20;
            titleRow.CreateCell(0).SetCellValue("批送货单号");
            titleRow.CreateCell(1).SetCellValue("批条码号");
            titleRow.CreateCell(2).SetCellValue("物料编码");
            titleRow.CreateCell(3).SetCellValue("物料描述");
            titleRow.CreateCell(4).SetCellValue("生成日期");
            titleRow.CreateCell(5).SetCellValue("出货子库");
            titleRow.CreateCell(6).SetCellValue("收货子库");
            titleRow.CreateCell(7).SetCellValue("状态");
            titleRow.CreateCell(8).SetCellValue("送货地点");
            titleRow.CreateCell(9).SetCellValue("总数量");
            titleRow.CreateCell(10).SetCellValue("容器绑定数量");

            string fileDownloadName = "送货批单明细报表";

            if (mainId.HasValue)
            {
                List<AscmDeliBatSumDetail> list = AscmDeliBatSumDetailService.GetInstance().GetListByMainId(mainId.Value);
                if (list != null)
                {
                    int rowIndex = 0;
                    foreach (AscmDeliBatSumDetail deliBatSumDetail in list)
                    {
                        IRow row = sheet.CreateRow(++rowIndex);
                        row.Height = 20 * 20;
                        row.CreateCell(0).SetCellValue(deliBatSumDetail.batchDocNumber);
                        row.CreateCell(1).SetCellValue(deliBatSumDetail.batchBarCode);
                        row.CreateCell(2).SetCellValue(deliBatSumDetail.materialDocNumber);
                        row.CreateCell(3).SetCellValue(deliBatSumDetail.materialDescription);
                        row.CreateCell(4).SetCellValue(deliBatSumDetail.batchCreateTime);
                        row.CreateCell(5).SetCellValue(deliBatSumDetail.batchSupperWarehouse);
                        row.CreateCell(6).SetCellValue(deliBatSumDetail.batchWarehouseId);
                        row.CreateCell(7).SetCellValue(deliBatSumDetail.batchStatusCn);
                        row.CreateCell(8).SetCellValue(deliBatSumDetail.batchWipLine);
                        row.CreateCell(9).SetCellValue(deliBatSumDetail.totalNumber.ToString());
                        row.CreateCell(10).SetCellValue(deliBatSumDetail.containerBindNumber.ToString());
                    }
                }
            }

            byte[] buffer = new byte[] { };
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                wb.Write(stream);
                buffer = stream.GetBuffer();
            }
            return File(buffer, "application/vnd.ms-excel", fileDownloadName + ".xls");
        }
        //批单明细
        public ActionResult SupplierDeliveryBatSumDetailIdList(string detailIds)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (!string.IsNullOrEmpty(detailIds))
                {
                    List<AscmDeliBatSumDetail> list = AscmDeliBatSumDetailService.GetInstance().GetList("from AscmDeliBatSumDetail where id in(" + detailIds + ")");
                    AscmDeliBatSumDetailService.GetInstance().SetDeliveryOrderBatch(list);
                    AscmDeliBatSumDetailService.GetInstance().SetMaterial(list);
                    if (list != null)
                    {
                        foreach (AscmDeliBatSumDetail ascmDeliBatSumDetail in list)
                        {
                            jsonDataGridResult.rows.Add(ascmDeliBatSumDetail);
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

        public ActionResult ContainerUnitQuantityList(int supplierId,string materialDocNumber)
        {
              List<AscmContainerUnitQuantity> list = null;
              try
              {
                 list = AscmContainerUnitQuantityService.GetInstance().GetList(supplierId,materialDocNumber);
              }
              catch (Exception ex)
              {
                  throw ex;
              }
              return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
