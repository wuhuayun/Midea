using System;
using System.Collections.Generic;
using System.Collections;
using System.Web.Mvc;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Dal.ContainerManage.Entities;
using YnBaseClass2.Web;
using MideaAscm.Services.ContainerManage;
using MideaAscm.Services.SupplierPreparation;
using MideaAscm.Services.Base;
using MideaAscm.Dal;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Web;
using System.Linq;

namespace MideaAscm.Areas.Ascm.Controllers
{
    public class ContainerManageController : YnFrame.Controllers.YnBaseController
    {
        //
        // GET: /Ascm/ContainerManage/

        #region RFID管理
        public ActionResult TagIndex()
        {
            //RFID管理
            return View();
        }
        public ActionResult TagList(int? page, int? rows, string sort, string order, string queryType, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmRfid> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "";
                if (!string.IsNullOrEmpty(queryType))
                    whereOther = " bindType='" + queryType.Trim() + "' ";
                list = AscmTagManageService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                foreach (AscmRfid ascmRfid in list)
                {
                    jsonDataGridResult.rows.Add(ascmRfid.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddTagList(bool? rfid)
        {
            List<AscmRfid> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();

            try
            {
                if (rfid.HasValue)
                {

                    list = GetRFIDTagList();
                }
                else
                {
                    list = GetTagList();
                }
                foreach (AscmRfid ascmRfid in list)
                {
                    jsonDataGridResult.rows.Add(ascmRfid.GetOwner());
                    jsonDataGridResult.message = jsonDataGridResult.message + ascmRfid.epcId + ",";
                }
                if (list.Count > 0)
                {
                    jsonDataGridResult.message = jsonDataGridResult.message.Remove(jsonDataGridResult.message.LastIndexOf(','), 1);
                }
                jsonDataGridResult.total = list.Count;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        ///读取容器标签（模拟）
        /// </summary>
        /// <returns></returns>
        public List<AscmRfid> GetTagList()
        {
            List<AscmRfid> list = new List<AscmRfid>();
            try
            {
                string ids = System.Web.Configuration.WebConfigurationManager.AppSettings["ContainerReader"];
                if (string.IsNullOrEmpty(ids))
                {
                    return list;
                }
                List<object> logs = MideaAscm.Services.Base.AscmReadLogService.GetInstance().GetAscmReadingHeadLog(ids);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Clear();
                if (logs != null)
                {
                    foreach (object obj in logs)
                    {
                        if (sb.Length != 0)
                        {
                            sb.Append(",");
                        }
                        sb.AppendFormat("'{0}'", obj);
                    }
                    //过滤没有注册的RFID标签
                    string hql = "from AscmRfid  where epcId in (" + sb.ToString() + ") and bindType='" + AscmRfid.BindTypeDefine.container + "'";
                    list = AscmRfidService.GetInstance().GetList(hql);
                    //过滤已经绑定的RFID标签
                    List<AscmRfid> TempeList = new List<AscmRfid>();
                    hql = "from AscmRfid  where epcId  in (select rfid  from AscmContainer where  rfid in (" + sb.ToString() + ") )";
                    TempeList = AscmRfidService.GetInstance().GetList(hql);
                    if (list != null && list.Count > 0 && TempeList != null && TempeList.Count > 0)
                    {
                        foreach (AscmRfid ascmRfid in TempeList)
                        {
                            list.Remove(ascmRfid);
                        }
                    }

                    List<string> ExRfid = new List<string>();
                    if (logs.Count != list.Count)
                    {
                        foreach (object epc in logs)
                        {
                            if (list.Find(e => e.epcId == epc.ToString()) == null)
                            {
                                ExRfid.Add(epc.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        /// <summary>
        ///读取标签（模拟）
        /// </summary>
        /// <returns></returns>
        public List<AscmRfid> GetRFIDTagList()
        {
            List<AscmRfid> list = new List<AscmRfid>();
            try
            {
                string ids = System.Web.Configuration.WebConfigurationManager.AppSettings["ContainerReader"];
                if (string.IsNullOrEmpty(ids))
                {
                    return list;
                }
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;

                List<object> logs = MideaAscm.Services.Base.AscmReadLogService.GetInstance().GetAscmReadingHeadLog(ids);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Clear();
                if (logs != null && logs.Count > 0)
                {
                    foreach (object obj in logs)
                    {
                        if (sb.Length != 0)
                        {
                            sb.Append(",");
                        }
                        sb.AppendFormat("'{0}'", obj);
                    }
                    IList<object> listWrong = YnDaoHelper.GetInstance().nHibernateHelper.Find<object>("select epcId from AscmRfid where epcId in (" + sb.ToString() + ")");

                    if (listWrong != null && listWrong.Count > 0)
                    {
                        foreach (object obj in listWrong)
                        {
                            logs.Remove(obj);
                        }
                    }

                    foreach (object epc in logs)
                    {
                        AscmRfid testRfid = new AscmRfid();
                        testRfid.id = epc.ToString().Substring(0, 6);
                        testRfid.epcId = epc.ToString();
                        testRfid.createUser = userName;
                        testRfid.createTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        testRfid.modifyUser = userName;
                        testRfid.modifyTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        list.Add(testRfid);
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public ActionResult EditTag(string id)
        {
            AscmRfid ascmRfid = null;
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    ascmRfid = AscmTagManageService.GetInstance().Get(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmRfid.GetOwner(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult EditTagSave(string edit_Id, string editType, string editStatus)
        {
            AscmRfid ascmRfid = null;
            try
            {
                if (!string.IsNullOrEmpty(edit_Id))
                {
                    ascmRfid = AscmTagManageService.GetInstance().Get(edit_Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            if (ascmRfid != null)
            {
                try
                {
                    if (string.IsNullOrEmpty(editType))
                        throw new Exception("必须选择RFID类型！");

                    string userName = string.Empty;
                    if (User.Identity.IsAuthenticated)
                    {
                        userName = User.Identity.Name;
                    }
                    //ascmRfid.id = edit_Id;
                    ascmRfid.bindType = editType;
                    ascmRfid.status = editStatus;
                    ascmRfid.modifyUser = userName;
                    ascmRfid.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    AscmTagManageService.GetInstance().Update(ascmRfid);
                    //标签作废，容器也随之作废
                    if (editType == AscmRfid.BindTypeDefine.container)
                    {
                        AscmContainer ascmContainer = AscmContainerInfoService.GetInstance().Get(edit_Id);
                        if (ascmContainer != null)
                        {

                            switch (editStatus)
                            {
                                case AscmRfid.StatusDefine.cancel:
                                    if (ascmContainer.status != AscmContainer.StatusDefine.invalid && ascmContainer.status != AscmContainer.StatusDefine.losted)
                                    {
                                        ascmContainer.status = AscmContainer.StatusDefine.invalid;
                                        ascmContainer.modifyUser = userName;
                                        ascmContainer.modifyTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        AscmContainerInfoService.GetInstance().Update(ascmContainer);
                                    }
                                    break;
                                case AscmRfid.StatusDefine.inUse:
                                    if (ascmContainer.status == AscmContainer.StatusDefine.invalid)
                                    {
                                        ascmContainer.status = AscmContainer.StatusDefine.unuse;
                                        ascmContainer.modifyUser = userName;
                                        ascmContainer.modifyTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        AscmContainerInfoService.GetInstance().Update(ascmContainer);
                                    }
                                    break;
                                case AscmRfid.StatusDefine.none:
                                    if (ascmContainer.status == AscmContainer.StatusDefine.invalid)
                                    {
                                        ascmContainer.status = AscmContainer.StatusDefine.unuse;
                                        ascmContainer.modifyUser = userName;
                                        ascmContainer.modifyTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        AscmContainerInfoService.GetInstance().Update(ascmContainer);
                                    }
                                    break;
                            }
                        }
                    }
                    jsonObjectResult.result = true;
                    jsonObjectResult.message = "";
                    //jsonObjectResult.id = ascmEmployeeCar.id.ToString();
                    //jsonObjectResult.entity = ascmEmployeeCar;
                }
                catch (Exception ex)
                {
                    jsonObjectResult.message = ex.Message;
                }
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]//覃小华于2013/07/15日修改
        public ContentResult AddTagSave(string addTagType, string addTagText)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            string sReturn = "";
            string[] sArray = null;
            if (!string.IsNullOrEmpty(addTagText))
            {
                sArray = addTagText.Split(',');
            }
            else
            {
                return Content(sReturn);
            }
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;
                for (int i = 0; i < sArray.Length; i++)
                {
                    AscmRfid ascmRfid = AscmTagManageService.GetInstance().Get(sArray[i].Substring(0, 6));
                    if (ascmRfid == null)
                    {
                        ascmRfid = new AscmRfid();
                        ascmRfid.epcId = sArray[i];
                        ascmRfid.bindType = addTagType;
                        ascmRfid.id = sArray[i].Substring(0, 6);
                        ascmRfid.createUser = userName;
                        ascmRfid.createTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        ascmRfid.modifyUser = userName;
                        ascmRfid.modifyTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        AscmTagManageService.GetInstance().Save(ascmRfid);
                    }

                    else
                    {   //提示重复系统过滤的标签
                        jsonObjectResult.message += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + sArray[i] + "<br>";
                    }
                }
                jsonObjectResult.result = true;

            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }
            sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        #endregion

        #region 容器管理
        public ActionResult ContainerInfoIndex()
        {
            //容器管理
            return View();
        }
        public ActionResult ContainerList(int? page, int? rows, string sort, string order, string querySn, int? supplierId, string status, string spec, string place)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmContainer> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "", whereStatus = "", whereSpec = "", wherePlace = "";
                if (supplierId.HasValue)
                    whereSupplier = "supplierId=" + supplierId.Value;
                if (!string.IsNullOrEmpty(status))
                    whereStatus = " status = '" + status + "'";
                if (!string.IsNullOrEmpty(spec) && spec != "0")
                    whereSpec = "specId=" + spec + "";
                if (!string.IsNullOrEmpty(place) && place != "0")
                    wherePlace = "place='" + place + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSpec);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, wherePlace);
                list = AscmContainerInfoService.GetInstance().GetList(ynPage, "", "", querySn, whereOther);
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
                    ascmContainer = AscmContainerInfoService.GetInstance().Get(id);
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
        public ContentResult ContainerEditSave(AscmContainer ascmContainer_Model, string id)
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
                    AscmContainer ascmContainer = AscmContainerInfoService.GetInstance().Get(id);
                    if (ascmContainer == null)
                    {
                        jsonObjectResult.result = false;
                        // throw new Exception("保存容器失败！");
                    }
                    ascmContainer.description = ascmContainer_Model.description;
                    //if (!string.IsNullOrEmpty(ascmContainer_Model.status))
                    //    ascmContainer.status = ascmContainer_Model.status.Trim();
                    if (!string.IsNullOrEmpty(ascmContainer_Model.status.ToString()))
                        ascmContainer.status = ascmContainer_Model.status;
                   // ascmContainer.description = ascmContainer_Model.description;
                    ascmContainer.modifyUser = userName;
                    ascmContainer.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    AscmContainerInfoService.GetInstance().Update(ascmContainer);
                    AscmRfid rfid = AscmRfidService.GetInstance().Get(id);
                    if (rfid != null)
                    {
                        if (ascmContainer_Model.status == AscmContainer.StatusDefine.invalid)
                        {
                            if (rfid.status != AscmRfid.StatusDefine.cancel)
                            {
                                rfid.status = AscmRfid.StatusDefine.cancel;
                                rfid.modifyUser = userName;
                                rfid.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                AscmTagManageService.GetInstance().Update(rfid);
                            }
                        }
                        else
                        {
                            if (rfid.status == AscmRfid.StatusDefine.cancel)
                            {
                                rfid.status = AscmRfid.StatusDefine.inUse;
                                rfid.modifyUser = userName;
                                rfid.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                AscmTagManageService.GetInstance().Update(rfid);
                            }
                        }

                    }

                    jsonObjectResult.id = ascmContainer.sn;
                    jsonObjectResult.entity = ascmContainer;

                }
                else
                {
                    jsonObjectResult.result = false;
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
        [HttpPost]
        public ContentResult ContainerAddSave(AscmContainer ascmContainer_Model, string rfidText)
        {
            string sReturn = "";
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            if (YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmSupplier where id=" + ascmContainer_Model.supplierId + "") == 0)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = "没有选择供应商！";
                sReturn = JsonConvert.SerializeObject(jsonObjectResult);
                return Content(sReturn);

            }
            jsonObjectResult.message = "";
            string[] sArray = null;
            if (!string.IsNullOrEmpty(rfidText))
            {
                sArray = rfidText.Split(',');
            }
            else
            {
                return Content("");
            }
            List<AscmContainer> listAscmContainer = new List<AscmContainer>();
            try
            {
                for (int i = 0; i < sArray.Length; i++)
                {
                    AscmContainer ascmContainer = AscmContainerInfoService.GetInstance().Get(sArray[i].Substring(0, 6));
                    if (ascmContainer == null)
                    {
                        string userName = string.Empty;
                        if (User.Identity.IsAuthenticated)
                        {
                            userName = User.Identity.Name;
                        }
                        ascmContainer = new AscmContainer();

                        ascmContainer.sn = sArray[i].Substring(0, 6);
                        ascmContainer.createUser = userName;
                        ascmContainer.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        ascmContainer.modifyUser = userName;
                        ascmContainer.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        ascmContainer.specId = ascmContainer_Model.specId;
                        ascmContainer.rfid = sArray[i];
                        ascmContainer.supplierId = ascmContainer_Model.supplierId;
                        //ascmContainer.place = "容器监管中心";
                        ascmContainer.status = 3; //默认为未使用
                        ascmContainer.description = ascmContainer_Model.description;
                        listAscmContainer.Add(ascmContainer);
                    }
                    else
                    {   //提示重复系统过滤的标签
                        jsonObjectResult.message += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + sArray[i] + "<br>";
                    }
                }
                AscmContainerInfoService.GetInstance().Save(listAscmContainer);

                jsonObjectResult.result = true;

            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }
            sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult ContainerDelete(string sn)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }
                AscmContainerInfoService.GetInstance().Delete(sn);
                AscmRfid rfid = AscmRfidService.GetInstance().Get(sn);
                if (rfid != null)
                {
                    rfid.status = AscmRfid.StatusDefine.cancel;
                    rfid.modifyUser = userName;
                    rfid.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    AscmTagManageService.GetInstance().Update(rfid);
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

        /// <summary>
        /// 规格查询
        /// 2013年9月10日11:37:00
        /// </summary>
        /// <returns></returns>
        public ActionResult RfidSpeac()
        {
            List<MideaAscm.Dal.SupplierPreparation.Entities.AscmContainerSpec> list = MideaAscm.Services.SupplierPreparation.AscmContainerSpecService.GetInstance().GetList("from AscmContainerSpec");
            list.Insert(0, new AscmContainerSpec { spec = "全部" });
            return Content(JsonConvert.SerializeObject(list));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ContainerPlace()
        {
            List<MideaAscm.Dal.Base.Entities.AscmReadingHead> list = MideaAscm.Services.Base.AscmReadingHeadService.GetInstance().GetList("from AscmReadingHead");
            list.Insert(0, new AscmReadingHead { address = "全部" });
            return Content(JsonConvert.SerializeObject(list));
        }

        /// <summary>
        /// 文件上传
        /// 2013年8月3日12:02:00
        /// 作者：覃小华
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult Upload(HttpPostedFileBase file, bool isRfid = true)
        {
            string message = "";
            if (file.ContentLength == 0)
            {
                return Content("没有文件", "text/plain");
            }
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file.InputStream);
            file.InputStream.Dispose();
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            if (!isRfid)
            {
                StringBuilder sbSn = new StringBuilder();
                List<AscmContainer> listAscmContainer = new List<AscmContainer>();
                while (rows.MoveNext())
                {
                    IRow row = (HSSFRow)rows.Current;
                    if (row.RowNum == 0)
                    {
                        if (row.LastCellNum != 3)
                        {
                            //return Content("格式错误！", "text/plain");
                        }
                        for (int i = 0; i < row.LastCellNum; i++)
                        {
                            ICell cell = row.GetCell(i);

                            if (i == 0 && cell.StringCellValue != "供应商")
                            {
                                return Content("格式错误！", "text/plain");
                            }
                            if (i == 1 && cell.StringCellValue != "规格")
                            {
                                return Content("格式错误！", "text/plain");
                            }
                            if (i == 2 && cell.StringCellValue != "RFID")
                            {
                                return Content("格式错误！", "text/plain");
                            }
                            if (i > 2)
                            {
                                break;
                            }

                        }
                    }
                    if (row.RowNum > 0)
                    {
                        AscmContainer ascmContainer = new AscmContainer();
                        ICell cell;
                        for (int i = 0; i < row.LastCellNum; i++)
                        {
                            cell = row.GetCell(i);
                            if (cell != null)
                            {
                                switch (i)
                                {
                                    case 0:
                                        ascmContainer._supplierName = cell.StringCellValue;
                                        break;
                                    case 1:
                                        ascmContainer.SpecName = cell.StringCellValue;
                                        break;

                                    case 2:
                                        if (!string.IsNullOrEmpty(cell.StringCellValue) && cell.StringCellValue.Length == 24)
                                        {
                                            if (listAscmContainer.FindAll(e => e.rfid == cell.StringCellValue).Count > 0)
                                            {
                                                listAscmContainer.Remove(listAscmContainer.Find(e => e.rfid == cell.StringCellValue));
                                                if (!string.IsNullOrEmpty(message))
                                                {
                                                    message += "  ";
                                                }
                                                message += cell.StringCellValue;
                                            }
                                        }
                                        else
                                        {
                                            ascmContainer = null;
                                        }
                                        break;
                                }


                            }
                            else
                            {
                                ascmContainer = null;
                                break;
                            }

                        }
                        if (ascmContainer != null)
                        {
                            listAscmContainer.Add(ascmContainer);
                        }
                        if (row.RowNum > 501)
                        {
                            message += "  你已上传大于500条！系统一次只能处理500条，其他的被自动忽略！";
                            break;
                        }
                    }
                }
                if (listAscmContainer.Count == 0)
                {
                    return Content("上传的全部容器异常未保存！", "text/plain");
                }
                var qRfid = listAscmContainer.Select(e => e.rfid);
                foreach (string sr in qRfid)
                {
                    if (sbSn.Length != 0)
                    {
                        sbSn.Append(",");
                    }
                    sbSn.AppendFormat("'{0}'", sr);
                }
                IList<AscmRfid> ilistRfid = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmRfid>("from AscmRfid where epcId in (" + sbSn.ToString() + ") and bindType='" + AscmRfid.BindTypeDefine.container + "'");
                if (ilistRfid == null || ilistRfid.Count == 0)
                {
                    return Content("上传的EPID全部都未经注册或者不是绑定为容器", "text/plain");
                }
                if (listAscmContainer.Count != ilistRfid.Count)
                {
                    List<string> lRfid = ilistRfid.ToList<AscmRfid>().Select(e => e.epcId).ToList<string>();
                    var t = from q in listAscmContainer where !lRfid.Contains(q.rfid) select q;
                    while (t.Count() > 0)
                    {
                        listAscmContainer.Remove(t.ToList<AscmContainer>()[0]);
                    }
                }
                if (listAscmContainer.Count == 0)
                {
                    return Content("上传的EPID全部异常", "text/plain");
                }
                IList<AscmContainer> tempeAscmContainer = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainer>("from AscmContainer where rfid in (" + sbSn.ToString() + ")");
                if (tempeAscmContainer != null && tempeAscmContainer.Count > 0)  //去除重复的
                {
                    foreach (AscmContainer ascmContainer in tempeAscmContainer)
                    {
                        listAscmContainer.Remove(listAscmContainer.Find(e => e.rfid == ascmContainer.rfid));
                    }
                }
                if (listAscmContainer.Count == 0)
                {
                    return Content("上传的EPID全部异常", "text/plain");
                }

                var suplier = (from ascon in listAscmContainer select ascon._supplierName).Distinct();
                var lspec = (from spec in listAscmContainer select spec.SpecName).Distinct();
                sbSn.Clear();
                int Intcount = 0;  //次数也作为统计数目用
                foreach (var sp in suplier)
                {
                    Intcount++;
                    if (sbSn.Length != 0)
                    {
                        sbSn.Append(",");
                    }
                    sbSn.AppendFormat("'{0}'", sp);
                }
                IList<AscmSupplier> sps = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>("from AscmSupplier where docNumber in (" + sbSn.ToString() + ")");
                if (sps == null || sps.Count == 0)
                {
                    return Content("上传的供应商全部异常", "text/plain");
                }

                //去除数据库中没有的供应商的
                if (sps.Count != Intcount)
                {
                    string[] temp = new string[sps.Count];
                    for (int i = 0; i < sps.Count; i++)
                    {
                        temp[i] = sps[i].docNumber;
                    }
                    var lastee = from p in listAscmContainer where !temp.Contains(p._supplierName) select p;
                    while (lastee.Count() > 0)
                    {
                        listAscmContainer.Remove(lastee.ToList<AscmContainer>()[0]);
                    }
                }
                Intcount = 0;
                sbSn.Clear();
                foreach (var sp in lspec)
                {
                    Intcount++;
                    if (sbSn.Length != 0)
                    {
                        sbSn.Append(",");
                    }
                    sbSn.AppendFormat("'{0}'", sp);
                }
                IList<AscmContainerSpec> listSpec = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainerSpec>("from AscmContainerSpec where  spec  in (" + sbSn.ToString() + ")");
                if (listSpec == null || listSpec.Count == 0)
                {
                    return Content("全部容器规格异常", "text/plain");
                }
                if (listSpec.Count != Intcount)
                {
                    string[] temp = new string[listSpec.Count];
                    for (int i = 0; i < listSpec.Count; i++)
                    {
                        temp[i] = listSpec[i].spec;
                    }
                    var lastee = from p in listAscmContainer where !temp.Contains(p.SpecName) select p;
                    while (lastee.Count() > 0)
                    {
                        listAscmContainer.Remove(lastee.ToList<AscmContainer>()[0]);
                    }
                }

                List<AscmSupplier> listSuppler = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(sps);
                List<AscmContainerSpec> listContainerSpec = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainerSpec>(listSpec);
                foreach (AscmContainer ascmContainer in listAscmContainer)
                {

                    ascmContainer.supplierId = listSuppler.Find(e => e.docNumber == ascmContainer._supplierName).id;
                    ascmContainer.specId = listContainerSpec.Find(e => e.spec == ascmContainer.SpecName).id;
                    ascmContainer.createUser = userName;
                    ascmContainer.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    ascmContainer.modifyUser = userName;
                    ascmContainer.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    ascmContainer.status = AscmContainer.StatusDefine.unuse; //默认为未使用

                }
                AscmContainerInfoService.GetInstance().Save(listAscmContainer);
            }
            else
            {
                StringBuilder sbSn = new StringBuilder();
                sbSn.Clear();
                List<AscmRfid> listRfid = new List<AscmRfid>();
                while (rows.MoveNext())
                {
                    IRow row = (HSSFRow)rows.Current;
                    if (row.RowNum == 0)
                    {
                        if (row.LastCellNum != 2)
                        {
                            // return Content("格式错误！", "text/plain");
                        }
                        for (int i = 0; i < row.LastCellNum; i++)
                        {
                            ICell cell = row.GetCell(i);

                            if (i == 0 && cell.StringCellValue != "RFID")
                            {
                                return Content("格式错误！", "text/plain");
                            }
                            if (i == 1 && cell.StringCellValue != "类型")
                            {
                                return Content("格式错误！", "text/plain");
                            }
                            if (i > 1)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        AscmRfid rfid = new AscmRfid();
                        for (int i = 0; i < row.LastCellNum; i++)
                        {
                            ICell cell = row.GetCell(i);
                            if (cell != null)
                            {
                                if (i == 0)
                                {
                                    if (!string.IsNullOrEmpty(cell.StringCellValue))
                                    {
                                        if (listRfid.FindAll(e => e.epcId == cell.StringCellValue).Count > 0)
                                        {
                                            listRfid.Remove(listRfid.Find(e => e.epcId == cell.StringCellValue));
                                            if (!string.IsNullOrEmpty(message))
                                            {
                                                message += "  ";
                                            }
                                            message += cell.StringCellValue;
                                            break;
                                        }
                                        rfid.epcId = cell.StringCellValue;
                                        rfid.id = cell.StringCellValue.Substring(0, 6);
                                    }
                                }
                                if (i == 1)
                                {
                                    switch (cell.StringCellValue)
                                    {
                                        case "容器":
                                            rfid.bindType = AscmRfid.BindTypeDefine.container;
                                            break;
                                        case "员工车辆":
                                            rfid.bindType = AscmRfid.BindTypeDefine.employeeCar;
                                            break;
                                        case "托盘":
                                            rfid.bindType = AscmRfid.BindTypeDefine.pallet;
                                            break;
                                        case "司机":
                                            rfid.bindType = AscmRfid.BindTypeDefine.driver;
                                            break;
                                        case "领料车辆":
                                            rfid.bindType = AscmRfid.BindTypeDefine.forklift;
                                            break;
                                    }
                                    break;
                                }
                            }
                            else 
                            {
                                rfid = null;
                                break;
                            }

                        }
                        if (row.RowNum > 501)
                        {
                            message += "  你已上传大于500条！系统一次只能处理500条，其他的被自动忽略！";
                            break;
                        }
                        if (rfid.epcId != null && rfid.epcId.Length == 24 && !string.IsNullOrEmpty(rfid.bindType))
                        {

                            rfid.createUser = userName;
                            rfid.createTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            rfid.modifyUser = userName;
                            rfid.modifyTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            listRfid.Add(rfid);
                        }
                       

                    }
                }
                if (listRfid.Count == 0)
                {
                    return Content("上传的RFID全部异常！", "text/plain");
                }
                var qEpc = listRfid.Select(e => e.epcId);
                foreach (string sr in qEpc)
                {
                    if (sbSn.Length != 0)
                    {
                        sbSn.Append(",");
                    }
                    sbSn.AppendFormat("'{0}'", sr);
                }
                IList<AscmRfid> ilistRfid = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmRfid>("from AscmRfid where epcId in (" + sbSn.ToString() + ")");
                if (ilistRfid != null && ilistRfid.Count > 0)
                {
                    foreach (AscmRfid trfid in ilistRfid)
                    {
                        foreach (AscmRfid trfidII in listRfid.FindAll(e => e.epcId == trfid.epcId))
                        {
                            listRfid.Remove(trfidII);
                        }
                    }
                }
                if (listRfid.Count == 0)
                {
                    return Content("上传的RFID全部异常！", "text/plain");
                }
                foreach (AscmRfid sRfid in listRfid)
                {
                    MideaAscm.Services.ContainerManage.AscmTagManageService.GetInstance().Save(sRfid);
                }


            }

            if (string.IsNullOrEmpty(message))
            {

                return Content("成功！", "text/plain");
            }
            else
            {
                return Content("成功！,但以下标签重复，请重新上传" + message, "text/plain");
            }

        }
        #endregion

        #region 出入库管理
        public ActionResult StoreInIndex()
        {
            //入库管理
            return View();
        }
        public ActionResult StoreOutIndex()
        {
            //出库管理
            return View();
        }
        public ActionResult StoreInByQuery()
        {
            //手动入库
            return View();
        }
        public ActionResult StoreOutByQuery()
        {
            //手动出库
            return View();
        }

        /// <summary>
        /// 出入库供应商选择
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult SupplierSelectList(bool? IsOut)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string readId = "";
                string sql = "from AscmSupplier  where id in (select supplierId  from  AscmContainer where rfid in (select distinct log.rfid from AscmReadingHeadLog log where log.processed=false))";
                if (IsOut.HasValue)
                {
                    if (IsOut.Value)
                    {
                        readId = System.Web.Configuration.WebConfigurationManager.AppSettings["ContainerReaderOut"]; ;
                    }
                    else
                    {
                        readId = System.Web.Configuration.WebConfigurationManager.AppSettings["ContainerReaderIn"]; ;
                    }
                    sql = "from AscmSupplier where id in (select supplierId  from AscmContainer where rfid in (select distinct log.rfid from AscmReadingHeadLog log where log.processed=false and log.readingHeadId=" + readId + " ))";
                }              
                IList<AscmSupplier> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql);
                if (ilist != null)
                {
                    foreach (AscmSupplier ascmsupplier in ilist)
                    {
                        jsonDataGridResult.rows.Add(ascmsupplier);
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmSupplier)", ex);
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 容器手动入库
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <param name="querySn"></param>
        /// <param name="supplierId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ActionResult ContainerInList(int? page, int? rows, string sort, string order, string querySn, int? supplierId, string status, string spec)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmContainer> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "", whereStatus = "", whereSpec = "";
                if (supplierId.HasValue)
                    whereSupplier = "supplierId=" + supplierId.Value;
                if (!string.IsNullOrEmpty(status))
                    whereStatus = " status = '" + status + "'";
                if (!string.IsNullOrEmpty(spec) && spec != "0")
                    whereSpec = "specId=" + spec + "";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSpec);
                list = AscmContainerInfoService.GetInstance().GetFilterList(ynPage, "", "", querySn, whereOther, "sn not in (select containerId  from AscmStoreInOut a where a.direction='" + AscmStoreInOut.DirectionDefine.storeIn + "' and a.createTime=(select max(createTime)  from AscmStoreInOut  where containerId=a.containerId))");
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


        /// <summary>
        /// 容器出库手动
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <param name="querySn"></param>
        /// <param name="supplierId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ActionResult ContainerOuntList(int? page, int? rows, string sort, string order, string querySn, int? supplierId, string status, string spec)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmContainer> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "", whereStatus = "", whereSpec = "";
                if (supplierId.HasValue)
                    whereSupplier = "supplierId=" + supplierId.Value;
                if (!string.IsNullOrEmpty(status))
                    whereStatus = " status = '" + status + "'";
                if (!string.IsNullOrEmpty(spec) && spec != "0")
                    whereSpec = "specId=" + spec + "";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSpec);
                list = AscmContainerInfoService.GetInstance().GetFilterList(ynPage, "", "", querySn, whereOther, "sn in (select containerId  from AscmStoreInOut a where a.direction='" + AscmStoreInOut.DirectionDefine.storeIn + "' and a.createTime=(select max(createTime)  from AscmStoreInOut  where containerId=a.containerId))");
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



        /// <summary>
        /// 覃小华添加于2015年4月14日10:46:48
        /// </summary>
        /// <returns></returns>
        public ActionResult ContainerSpecList()
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(1); //pageNumber;

            List<AscmContainerSpec> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmContainerSpecService.GetInstance().GetList(ynPage, "", "", "", null);
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
            return Json(jsonDataGridResult.rows, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 读取RFID 修改于2013/07/17
        /// RFID读取模块还没有成功所以就没有完成
        /// </summary>
        /// <returns></returns>
        public ActionResult ReadContainerList(string direction, string secods)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            List<AscmContainer> list = null;
            AscmContainer ascmContainer;
            string ids = "";
            if ("STOREOUT" == direction)
            {
                 ids = System.Web.Configuration.WebConfigurationManager.AppSettings["ContainerReaderOut"]; ;
            }               
            else
            {
                 ids = System.Web.Configuration.WebConfigurationManager.AppSettings["ContainerReaderIn"]; ;
            } 
                        
            if (string.IsNullOrEmpty(ids))
            {
                return Content("请先配置容器中心读头ID！");
            }

            List<object> logs;

            try
            {
                if (string.IsNullOrEmpty(secods))
                {
                    logs = MideaAscm.Services.Base.AscmReadLogService.GetInstance().GetAscmReadingHeadLog(ids);
                }
                else
                {
                    logs = MideaAscm.Services.Base.AscmReadLogService.GetInstance().GetAscmReadingHeadLog(ids, secods);
                }
                if (logs != null)
                {
                    list = new List<AscmContainer>();
                    foreach (object epc in logs)
                    {
                        ascmContainer = AscmContainerInfoService.GetInstance().GetByEpcId(epc.ToString());
                        if (ascmContainer == null)
                        {
                            ascmContainer = new AscmContainer() { sn=epc.ToString().Substring(0,6) , rfid = epc.ToString() };
                            if (MideaAscm.Services.Base.AscmRfidService.GetInstance().GetByEpc(epc.ToString()) == null)
                            {
                                ascmContainer.status = 4;  //未注册
                            }
                            else
                            {
                                ascmContainer.status = 5; //未绑定
                            }

                        }
                        else
                        {

                            object obj = MideaAscm.Services.ContainerManage.AscmStoreInOutService.GetInstance().GetLastByEpc(epc.ToString());
                            if (obj != null)
                            {
                                if (obj.ToString() == direction && ascmContainer.status != AscmContainer.StatusDefine.invalid)
                                {
                                    ascmContainer.status = 6; //已存在该的出入库记录
                                }
                            }
                            else
                            {
                                if (AscmStoreInOut.DirectionDefine.storeOut == direction && ascmContainer.status != AscmContainer.StatusDefine.invalid)
                                {
                                    ascmContainer.status = 7; //没有入库，无法出库
                                }
                            }

                        }
                        list.Add(ascmContainer);

                    }
                }
                if (list != null)
                {
                    var tempe = list.OrderByDescending(e => e.status);
                    foreach (AscmContainer fascmContainer in tempe)
                    {
                        jsonDataGridResult.rows.Add(fascmContainer);
                    }
                }
                //list.FindAll(e => e.status < 4);
            }
            catch (Exception ex)
            {
                jsonDataGridResult.result = false;
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="PdocNumber"></param>
        /// <returns></returns>
        public ContentResult ReadLogDelet(string data, string direction)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string readId = "";
                AscmContainer[] rows = JsonConvert.DeserializeObject<AscmContainer[]>(data);
               // object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select to_char(id) from  AscmReadingHead where address= '容器监管中心' and ip='0.0.0.0'");
                string ids = string.Empty;
                foreach (AscmContainer ascmSupplier in rows)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'"+ascmSupplier.rfid+"'";
                }
                if ("STOREOUT" == direction)
                {

                    readId = System.Web.Configuration.WebConfigurationManager.AppSettings["ContainerReaderOut"]; ;
                }
                else
                {
                    readId = System.Web.Configuration.WebConfigurationManager.AppSettings["ContainerReaderIn"]; ;
                    

                }
                AscmReadLogService.GetInstance().DeleteAscmReadingHeadLog(readId, ids);
                jsonObjectResult.result = true;
               // jsonObjectResult.message = docNumber;
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
        public ContentResult StoreIn(string data, string PdocNumber)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string docNumber = "";
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }
                if (string.IsNullOrEmpty(PdocNumber))
                {
                    docNumber = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("AscmStoreInOut", "", "yyyyMMdd", 2);
                }
                else
                {
                    docNumber = PdocNumber;
                }
                AscmContainer[] rows = JsonConvert.DeserializeObject<AscmContainer[]>(data);
                object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select to_char(id) from  AscmReadingHead where address= '容器监管中心' and ip='0.0.0.0'");
                string ids = "";
                foreach (AscmContainer ascmContainer in rows)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'"+ascmContainer.rfid+"'";
                    ascmContainer.place = obj.ToString();
                    ascmContainer.modifyUser = userName;
                    ascmContainer.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    AscmContainerInfoService.GetInstance().Update(ascmContainer);
                    StoreInRecord(ascmContainer, docNumber);
                }
                AscmReadLogService.GetInstance().DeleteAscmReadingHeadLog(System.Web.Configuration.WebConfigurationManager.AppSettings["ContainerReaderIn"], ids);
                jsonObjectResult.result = true;
                jsonObjectResult.message = docNumber;
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        private void StoreInRecord(AscmContainer ascmContainer, string docNumber)
        {
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }
                int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmStoreInOut");
                AscmStoreInOut ascmStoreInOut = new AscmStoreInOut();
                ascmStoreInOut.id = ++maxId;
                ascmStoreInOut.createUser = userName;
                ascmStoreInOut.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ascmStoreInOut.containerId = ascmContainer.sn;
                ascmStoreInOut.supplierId = ascmContainer.supplierId;
                ascmStoreInOut.direction = AscmStoreInOut.DirectionDefine.storeIn;
                ascmStoreInOut.epcId = ascmContainer.rfid;
                ascmStoreInOut.readTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ascmStoreInOut.stakeHolders = userName;
                ascmStoreInOut.status = AscmStoreInOut.StatusDefine.inStored;
                ascmStoreInOut.tip = string.Empty;
                ascmStoreInOut.docNumber = docNumber;
                AscmStoreInOutService.GetInstance().Save(ascmStoreInOut);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ContentResult StoreOut(string data, string PdocNumber)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                string docNumber = "";
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }
                if (string.IsNullOrEmpty(PdocNumber))
                {
                    docNumber = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("AscmStoreInOut", "", "yyyyMMdd", 2);
                }
                else
                {
                    docNumber = PdocNumber;
                }
                // = YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("AscmDocNumberLog" + AscmStoreInOut.DirectionDefine.storeIn, "", "yyyyMMdd", 2);
                AscmContainer[] rows = JsonConvert.DeserializeObject<AscmContainer[]>(data);
                object obj = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select to_char(id) from  AscmReadingHead where address= '厂区外' and ip='0.0.0.0'");
                string ids = "";
                foreach (AscmContainer ascmContainer in rows)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmContainer.rfid;

                    if (ascmContainer.supplierId != 6128)
                    {
                        ascmContainer.place = obj.ToString();
                    }
                    else
                    {
                        ascmContainer.place = null;
                    }
                    ascmContainer.modifyUser = userName;
                    ascmContainer.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    AscmContainerInfoService.GetInstance().Update(ascmContainer);
                    StoreOutRecord(ascmContainer, docNumber);
                }   
                AscmReadLogService.GetInstance().DeleteAscmReadingHeadLog(System.Web.Configuration.WebConfigurationManager.AppSettings["ContainerReaderOut"], ids);
                jsonObjectResult.result = true;
                jsonObjectResult.message = docNumber;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        private void StoreOutRecord(AscmContainer ascmContainer, string docNumber)
        {
            try
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                {
                    userName = User.Identity.Name;
                }
                int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmStoreInOut");
                AscmStoreInOut ascmStoreInOut = new AscmStoreInOut();
                ascmStoreInOut.id = ++maxId;
                ascmStoreInOut.createUser = userName;
                ascmStoreInOut.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ascmStoreInOut.containerId = ascmContainer.sn;
                ascmStoreInOut.supplierId = ascmContainer.supplierId;
                ascmStoreInOut.direction = AscmStoreInOut.DirectionDefine.storeOut;
                ascmStoreInOut.epcId = ascmContainer.rfid;
                ascmStoreInOut.readTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ascmStoreInOut.stakeHolders = userName;
                ascmStoreInOut.status = AscmStoreInOut.StatusDefine.outStored;
                ascmStoreInOut.tip = string.Empty;
                ascmStoreInOut.docNumber = docNumber;
                AscmStoreInOutService.GetInstance().Save(ascmStoreInOut);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 容器流转历史
        public ActionResult ContainerHistoryIndex()
        {
            //容器流转历史
            return View();
        }

        public ActionResult ContainerHistoryList(int? page, int? rows, string sort, string order, AscmStoreInOut model)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmStoreInOut> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereStr = "", whereSupplier = "", whereDirection = "", whereStratTime = "", whereEndTime = "", whereDocNumber = "";
                if (!string.IsNullOrEmpty(model.docNumber))
                    whereDocNumber = " docNumber like '%" + model.docNumber + "%'";
                if (model.supplierId > 0)
                    whereSupplier = " supplierId in(" + model.supplierId + ")";
                if (!string.IsNullOrEmpty(model.direction))
                    whereDirection = " direction in('" + model.direction + "')";
                DateTime dtStartTime, dtEndTime;
                if (!string.IsNullOrEmpty(model.queryStartTime) && DateTime.TryParse(model.queryStartTime, out dtStartTime))
                    whereStratTime = "readTime>='" + dtStartTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(model.queryEndTime) && DateTime.TryParse(model.queryEndTime, out dtEndTime))
                    whereEndTime = "readTime<='" + dtEndTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereStr = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereStr, whereDocNumber);
                whereStr = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereStr, whereSupplier);
                whereStr = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereStr, whereDirection);
                whereStr = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereStr, whereStratTime);
                whereStr = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereStr, whereEndTime);

                list = AscmStoreInOutService.GetInstance().GetList(ynPage, sort, order, whereStr);
                if (list != null)
                {
                    foreach (AscmStoreInOut ascmStoreInOut in list)
                    {
                        jsonDataGridResult.rows.Add(ascmStoreInOut);
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


        /// <summary>
        /// 流转信息查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <param name="bindType"></param>
        /// <param name="RequiredTime"></param>
        /// <returns></returns>
        public ActionResult ListflowInfo(int? page, int? rows, string sort, string order, string bindType, string BRequiredTime, string ERequiredTime, string sn)
        {

            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmTagLog> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string wherebindType = "", whereRequiredTime = "", whereStr = "", whereSn = "";
                if (!string.IsNullOrEmpty(bindType))
                {
                    wherebindType = " bindType= '" + bindType + "'";
                }
                if (!string.IsNullOrEmpty(BRequiredTime) && !string.IsNullOrEmpty(ERequiredTime))
                {
                    whereRequiredTime = " readTime  between '" + BRequiredTime + "' and '" + ERequiredTime + " 23:59:59'";
                }
                if (!string.IsNullOrEmpty(sn))
                {
                    whereSn = "epcId  like '%" + sn + "%'";
                }
                whereStr = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereStr, wherebindType);
                whereStr = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereStr, whereRequiredTime);
                whereStr = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereStr, whereSn);
                list = AscmTagLogService.GetInstance().GetFlowInfo(ynPage, sort, order, whereStr);
                if (list != null)
                {
                    foreach (AscmTagLog ascmStoreInOut in list)
                    {
                        jsonDataGridResult.rows.Add(ascmStoreInOut);
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
        public ActionResult GetFlowDetailInfo(string sn)
        {
            List<AscmTagLog> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereStr = "", whereSn = "";
                if (!string.IsNullOrEmpty(sn))
                {
                    whereSn = "epcId ='" + sn + "' and  a.readTime!=(select max(readTime) from AscmTagLog where epcId='" + sn + "')";
                }
                whereStr = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereStr, whereSn);
                list = AscmTagLogService.GetInstance().GetFlowDetailInfo(whereStr);
                if (list != null)
                {
                    foreach (AscmTagLog ascmStoreInOut in list)
                    {
                        jsonDataGridResult.rows.Add(ascmStoreInOut);
                    }
                }
                ///jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteRecod(string docNumber, bool IsDoc)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {

                MideaAscm.Services.ContainerManage.AscmTagLogService.GetInstance().DeleteRecode(docNumber, IsDoc);
                jsonObjectResult.result = true;
            }
            catch (Exception ex)
            {
                jsonObjectResult.result = false;
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region  容器盘点
        //
        // GET: /Ascm/ContainerManage/

        public ActionResult ContainerCheck()
        {
            //如果点击结束盘点，系统默认完成盘点
            if (YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(sn) from AscmContainer where isCheck=0 and status!=1 and place!='厂区外'") == 0 && YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(ack.id) from AscmCheck as ack where ack.count is null") > 0)
            {
                EndCheck();
            }
            return View();
        }


        [HttpPost]
        public ContentResult EndCheck()
        {
            string strCode = "1";
            try
            {
                Dal.ContainerManage.Entities.AscmCheck ack = AscmContaierCheckService.GetInstance().Get();
                ack.count = ack.totalNumber - YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmCheckContainerInfo where checkId=" + ack.id + " and status='" + AscmCheckContainerInfo.StatusDefine.lost + "'") + YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmCheckContainerInfo where checkId=" + ack.id + " and status='" + AscmCheckContainerInfo.StatusDefine.find + "'");
                if (ack.totalNumber < ack.count)
                {
                    ack.identificationId = Dal.ContainerManage.Entities.AscmCheck.IdentificationDefine.inventoryProfit;
                }
                if (ack.totalNumber > ack.count)
                {
                    ack.identificationId = Dal.ContainerManage.Entities.AscmCheck.IdentificationDefine.inventoryLosses;
                }
                if (ack.totalNumber == ack.count)
                {
                    ack.identificationId = Dal.ContainerManage.Entities.AscmCheck.IdentificationDefine.iinventoryDeuce;
                }
                AscmContaierCheckService.GetInstance().UpdateCheck(ack);
                AscmContaierCheckService.GetInstance().ResetCheckInfo();
            }
            catch (Exception ex)
            {
                strCode = ex.Message;
                throw ex;
            }
            return Content(strCode);
        }

        /// <summary>
        /// 开始盘点方法
        /// </summary>
        /// <returns>返回状态代码，1表示有未完成盘点的任务，2表示新建盘点</returns>
        [HttpPost]
        public ContentResult CheckBegin()
        {
            string strRes = "";
            if (Convert.ToInt32(YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(ack.id) from AscmCheck as ack where ack.count is null")) > 0)
            {
                strRes = "1";
            }
            else
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;
                MideaAscm.Dal.ContainerManage.Entities.AscmCheck ack = new Dal.ContainerManage.Entities.AscmCheck();
                ack.id = Convert.ToInt32(YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("AscmCheck", "", "yyyyMMdd", 2));
                ack.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ack.createUser = userName;
                ack.count = null;
                ack.totalNumber = Convert.ToInt32(YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(sn) from AscmContainer where status in (3,2)"));
                MideaAscm.Services.ContainerManage.AscmContaierCheckService.GetInstance().SaveCheck(ack);
                strRes = "2";
            }
            return Content(strRes);
        }
        /// <summary>
        /// 盘点结束方法
        /// </summary>
        /// <param name="asckCheck"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult CheckEndSave()
        {

            //提交完毕之后检查是否还有未盘点数据，返回提醒用户是否继续提交并保持原有数据不变化
            JsonObjectResult jsob = CheckSave(true);
            if (YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(sn) from AscmContainer where isCheck=0 and status!=1 and (place!=( select to_char(id) from  AscmReadingHead where address= '厂区外' and ip='0.0.0.0') or place is null)") > 0)
            {
                //id为2表示还有为完成的盘点任务
                jsob.id = "2";
                return Content(JsonConvert.SerializeObject(jsob));
            }
            Dal.ContainerManage.Entities.AscmCheck ack = AscmContaierCheckService.GetInstance().Get();
            ack.count = ack.totalNumber - YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmCheckContainerInfo where checkId=" + ack.id + " and status='LOST'") + YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmCheckContainerInfo where checkId=" + ack.id + " and status='FIND'");
            if (ack.totalNumber < ack.count)
            {
                ack.identificationId = Dal.ContainerManage.Entities.AscmCheck.IdentificationDefine.inventoryProfit;
            }
            if (ack.totalNumber > ack.count)
            {
                ack.identificationId = Dal.ContainerManage.Entities.AscmCheck.IdentificationDefine.inventoryLosses;
            }
            if (ack.totalNumber == ack.count)
            {
                ack.identificationId = Dal.ContainerManage.Entities.AscmCheck.IdentificationDefine.iinventoryDeuce;
            }
            AscmContaierCheckService.GetInstance().UpdateCheck(ack);
            AscmContaierCheckService.GetInstance().ResetCheckInfo();
            return Content(JsonConvert.SerializeObject(jsob));

        }
        /// <summary>
        /// 暂存盘点信息方法
        /// </summary>
        /// <param name="asckCheck"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult CheckTempeSave()
        {
            return Content(JsonConvert.SerializeObject(CheckSave(true)));
        }

        /// <summary>
        /// 保存盘点
        /// </summary>
        /// <param name="boolIsBrows"></param>
        /// <returns></returns>
        private JsonObjectResult CheckSave(bool boolIsBrows)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            string strLost = Request.Form["checkInfo"];
            if (string.IsNullOrEmpty(strLost))
            {
                jsonObjectResult.result = true;
                return jsonObjectResult;
            }
            List<checkeInfo> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<checkeInfo>>(strLost);
            List<MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer> listCon = new List<Dal.SupplierPreparation.Entities.AscmContainer>();
            List<MideaAscm.Dal.ContainerManage.Entities.AscmCheckContainerInfo> listCheckInfo = new List<Dal.ContainerManage.Entities.AscmCheckContainerInfo>();
            MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer ascmc;
            Dal.ContainerManage.Entities.AscmCheckContainerInfo ascmCheckInf;
            string userName = string.Empty;
            if (User.Identity.IsAuthenticated)
                userName = User.Identity.Name;
            int intIdMax = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmCheckContainerInfo)");
            int intCheckIdMax = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmCheck)");
            foreach (checkeInfo info in list.FindAll(p => p.supplierId == ""))
            {
                ascmc = MideaAscm.Services.SupplierPreparation.AscmContainerService.GetInstance().Get(info.id);
                if (ascmc.isCheck == 1) //已盘点
                {
                    if (ascmc.status == 0 && info.status != "LOST") //但是状态更新了
                    {
                        ascmc.status = AscmContainer.StatusDefine.unuse;
                        listCon.Add(ascmc);
                    }
                    if (ascmc.status == 3 && info.status != "FIND")////但是状态更新了
                    {
                        ascmc.status = AscmContainer.StatusDefine.losted;
                        listCon.Add(ascmc);
                    }

                }
                else
                {
                    if (info.status == "LOST")
                    {
                        ascmc.status = MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer.StatusDefine.losted;
                    }

                    if (info.status == "FIND")
                    {
                        ascmc.status = MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer.StatusDefine.unuse;
                    }
                    ascmc.isCheck = 1;
                    listCon.Add(ascmc);
                }
                if (info.status != "")
                {
                    //获取是否已存在该容器的盘点信息
                    object id = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select id from AscmCheckContainerInfo where status='" + info.status + "' and checkId=" + intCheckIdMax + " and containerId='" + info.id + "'");
                    if (id != null)
                    {
                        ascmCheckInf = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmCheckContainerInfo>((int)id);
                        ascmCheckInf.status = info.status;
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmCheckContainerInfo>(ascmCheckInf);
                    }
                    else
                    {
                        ascmCheckInf = new Dal.ContainerManage.Entities.AscmCheckContainerInfo();
                        ascmCheckInf.checkId = intCheckIdMax;
                        ascmCheckInf.id = ++intIdMax;
                        ascmCheckInf.containerId = info.id;
                        ascmCheckInf.supplierId = ascmc.supplierId;
                        ascmCheckInf.status = info.status;
                        ascmCheckInf.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        ascmCheckInf.createUser = userName;
                        listCheckInfo.Add(ascmCheckInf);
                    }
                }
            }
            foreach (checkeInfo inf in list.FindAll(p => p.supplierId != ""))
            {
                AscmContaierCheckService.GetInstance().ExecuteOraDMLSQL("update ascm_container set ischeck=1 where ischeck=0 and status<>1 and (place<>(select to_char(id)  from ascm_reading_head where address='厂区外' AND ip='0.0.0.0') or place is NULL)  and supplierid='" + inf.supplierId + "'");
            }
            try
            {
                MideaAscm.Services.SupplierPreparation.AscmContainerService.GetInstance().UpdateList(listCon);
                MideaAscm.Services.ContainerManage.AscmContaierCheckService.GetInstance().SaveCheckInfo(listCheckInfo);
                jsonObjectResult.result = true;
                jsonObjectResult.message = "提交成功";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return jsonObjectResult;
        }

        /// <summary>
        /// 2013-07-11新增 
        /// 覃小华
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public ActionResult GetContainerBySpec(string supplierId)
        {

            List<Dal.SupplierPreparation.Entities.AscmContainer> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = MideaAscm.Services.ContainerManage.AscmContaierCheckService.GetInstance().GetListBySpec(supplierId);
                if (list != null)
                {
                    foreach (AscmContainer ascmWorker in list)
                    {
                        jsonDataGridResult.rows.Add(ascmWorker);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 2013-07-11新增 
        /// 覃小华
        /// 得到需要盘点的容器根据供应商ID和规格ID得到
        /// </summary>
        /// <returns></returns>
        public ActionResult GetContainerListBy2Id(int? page, int? rows, string sort, string order, string supplierId, string specid, string sn, string place, string status)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;
            List<Dal.SupplierPreparation.Entities.AscmContainer> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereSpec = "", wherePlace = "", whereOther = "", whereSn = "", whereStatus = "";
                if (!string.IsNullOrEmpty(specid) && specid != "0")
                    whereSpec = "specId=" + specid + "";
                if (!string.IsNullOrEmpty(place) && place != "0")
                    wherePlace = "place='" + place + "'";
                if (!string.IsNullOrEmpty(sn))
                    whereSn = "sn like '%" + sn + "%'";
                if (!string.IsNullOrEmpty(status))
                    whereStatus = " status = '" + status + "'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSpec);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, wherePlace);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSn);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                list = MideaAscm.Services.ContainerManage.AscmContaierCheckService.GetInstance().GetListBy2Id(ynPage, "", "", supplierId, whereOther);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="specid"></param>
        /// <returns></returns>
        public ActionResult GetContainerPlaceListBy2Id(string supplierId)
        {
            if (string.IsNullOrEmpty(supplierId))
            {
                return Content("");
            }
            List<Dal.Base.Entities.AscmReadingHead> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = MideaAscm.Services.ContainerManage.AscmContaierCheckService.GetInstance().GetPlaceListBy2Id(supplierId);
                if (list != null)
                {
                    list.Insert(0, new AscmReadingHead { address = "全部" });
                    foreach (Dal.Base.Entities.AscmReadingHead ascmContainer in list)
                    {
                        jsonDataGridResult.rows.Add(ascmContainer);
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

        #region  超期预警
        public ActionResult ContainerWarn()
        {
            return View();
        }


        [HttpPost]
        public ContentResult SupplierDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                AscmSupplier ascmSupplier = null;
                if (id.HasValue)
                {
                    ascmSupplier = MideaAscm.Services.Base.AscmSupplierService.GetInstance().Get(id.Value);
                }
                else
                {
                    ascmSupplier = new AscmSupplier();
                    throw new Exception("没有该供应商！");
                }
                if (ascmSupplier == null)
                    throw new Exception("删除供应商预警失败！");

                ascmSupplier.warnHours = null;
                MideaAscm.Services.Base.AscmSupplierService.GetInstance().Update(ascmSupplier);
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmSupplier.id.ToString();

            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
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
                list = AscmContaierCheckService.GetInstance().Getlist(ynPage, "", "", queryWord, null);
                foreach (AscmSupplier ascmSupplier in list)
                {
                    jsonDataGridResult.rows.Add(ascmSupplier);
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 超期预警查询   
        /// 覃小华于2013/07/15修改
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <param name="queryWord"></param>
        /// <param name="readTime"></param>
        /// <param name="extendedTime"></param>
        /// <returns></returns>
        public ActionResult ContainerWarnList(int? page, int? rows, string sort, string order, string queryWord, string DateRequired)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;
            List<Dal.SupplierPreparation.Entities.AscmContainer> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", strSupplerId = "";
                int intSuplierId;
                if (!string.IsNullOrEmpty(queryWord) && int.TryParse(queryWord, out intSuplierId))
                {
                    strSupplerId = "container.supplierId=" + intSuplierId.ToString() + "";
                }
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, strSupplerId);
                list = AscmCheckWarnService.GetInstance().Getlist(ynPage, "", "", whereOther, DateRequired);
                list.OrderBy(g => g._supplierName);
                foreach (Dal.SupplierPreparation.Entities.AscmContainer ascmSupplier in list)
                {
                    jsonDataGridResult.rows.Add(ascmSupplier);
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 容器预警规则
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <param name="queryWord"></param>
        /// <returns></returns>
        public ActionResult WarnRuleList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmSupplier> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = MideaAscm.Services.ContainerManage.AscmCheckWarnService.GetInstance().GetWarnRuleList(ynPage, "", "", queryWord, null);
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


        #endregion

        #region 用于[容器盘点]处理请求的类
        /// <summary>
        /// 用于[容器盘点]处理请求的类
        /// </summary>
        class checkeInfo
        {
            public string id { get; set; }
            public string status { get; set; }
            public string count { get; set; }
            public string supplierId { get; set; }
            public checkeInfo(string id, string status, string count, string supplierId)
            {
                this.id = id;
                this.status = status;
                this.count = count;
                this.supplierId = supplierId;
            }
        }
        #endregion


        #region  导出
        /// <summary>
        /// 异常容器导出
        /// </summary>
        /// <param name="days"></param>
        /// <param name="supplierId"></param>
        /// <param name="spec"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        public FileResult ExportExcell(string days, int? supplierId, string spec, string place)
        {
            string whereOther = "", whereSupplier = "", whereSpec = "", wherePlace = "";
            if (supplierId.HasValue)
                whereSupplier = "supplierId=" + supplierId.Value;
            if (!string.IsNullOrEmpty(spec) && spec != "0")
                whereSpec = "specId=" + spec + "";
            if (!string.IsNullOrEmpty(place) && place != "0")
                wherePlace = "place='" + place + "'";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSpec);
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, wherePlace);
            List<AscmContainer> listContainer = MideaAscm.Services.ContainerManage.AscmContainerInfoService.GetInstance().ExportExcell(Convert.ToInt32(days), whereOther);
            string filename = string.Format("异常容器_{0}.xls", System.DateTime.Now.ToString("yyyy-MM-dd"));
            return  ExportStream(listContainer,new string[] { "容器编号", "EPC", "规格", "供应商名称", "位置", "描述", "状态", "最后更新时间" },new string[] { "sn", "rfid", "spec", "supplierName", "ReadingHeadAddress", "description", "statusCn", "modifyTime" },filename,"异常数据");
          
        }

        /// <summary>
        /// 导出流转历史表
        /// </summary>
        /// <param name="bindType"></param>
        /// <param name="BRequiredTime"></param>
        /// <param name="ERequiredTime"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
        public FileResult ExportflowInfExcell(string bindType, string BRequiredTime, string ERequiredTime, string sn)
        {
            string wherebindType = "", whereRequiredTime = "", whereStr = "", whereSn = "";
            if (!string.IsNullOrEmpty(bindType))
            {
                wherebindType = " bindType= '" + bindType + "'";
            }
            if (!string.IsNullOrEmpty(BRequiredTime) && !string.IsNullOrEmpty(ERequiredTime))
            {
                whereRequiredTime = " readTime  between '" + BRequiredTime + "' and '" + ERequiredTime + " 23:59:59'";
            }
            if (!string.IsNullOrEmpty(sn))
            {
                whereSn = "epcId  like '%" + sn + "%'";
            }
            whereStr = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereStr, wherebindType);
            whereStr = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereStr, whereRequiredTime);
            whereStr = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereStr, whereSn);
            List<AscmTagLog> listContainer = MideaAscm.Services.ContainerManage.AscmTagLogService.GetInstance().GetFlowInfo(whereStr);
            string filename = string.Format("容器流转历史_{0}.xls", System.DateTime.Now.ToString("yyyy-MM-dd"));
            return ExportStream(listContainer, new string[] { "编号", "供应商", "类型", "流转历史" }, new string[] { "objectID", "supplierName", "_bindType", "place"}, filename, "流转历史");
        }



        /// <summary>
        /// 容器基础导出
        /// </summary>
        /// <param name="querySn"></param>
        /// <param name="supplierId"></param>
        /// <param name="status"></param>
        /// <param name="spec"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        public FileResult ExportContainerInfExcell(string querySn, int? supplierId, string status, string spec, string place)
        {       
            List<AscmContainer> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereSupplier = "", whereStatus = "", whereSpec = "", wherePlace = "";
                if (supplierId.HasValue)
                    whereSupplier = "supplierId=" + supplierId.Value;
                if (!string.IsNullOrEmpty(status))
                    whereStatus = " status = '" + status + "'";
                if (!string.IsNullOrEmpty(spec) && spec != "0")
                    whereSpec = "specId=" + spec + "";
                if (!string.IsNullOrEmpty(place) && place != "0")
                    wherePlace = "place='" + place + "'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSpec);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, wherePlace);
                list = AscmContainerInfoService.GetInstance().GetList(querySn, whereOther);
                string filename = string.Format("容器信息_{0}.xls", System.DateTime.Now.ToString("yyyy-MM-dd"));
                return ExportStream(list, new string[] { "容器编号", "EPC", "规格", "供应商名称", "位置", "描述", "状态", "最后更新时间" }, new string[] { "sn", "rfid", "spec", "supplierName", "ReadingHeadAddress", "description", "statusCn", "modifyTime" }, filename, "容器信息");
            }
            catch (Exception ex)
            {
                throw ex;
            }


 
        }
        /// <summary>
        /// 导出预警数据
        /// </summary>
        /// <param name="queryWord"></param>
        /// <param name="DateRequired"></param>
        /// <returns></returns>
        public FileResult ExprotWarmContainerDate(string queryWord, string DateRequired)
        {

            List<Dal.SupplierPreparation.Entities.AscmContainer> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", strSupplerId = "";
                int intSuplierId;
                if (!string.IsNullOrEmpty(queryWord) && int.TryParse(queryWord, out intSuplierId))
                {
                    strSupplerId = "container.supplierId=" + intSuplierId.ToString() + "";
                }
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, strSupplerId);
                list = AscmCheckWarnService.GetInstance().GetlistForExcellExport(whereOther, DateRequired);
                //list.OrderBy(g => g._supplierName);
                foreach (Dal.SupplierPreparation.Entities.AscmContainer ascmContainer in list)
                {
                    if (ascmContainer.extendedTime > 0)
                    {
                        ascmContainer.description = "已超期" + ascmContainer.extendedTime;
                    }
                    if (ascmContainer.extendedTime < 0 && ascmContainer.extendedTime >-25)
                    {
                        ascmContainer.description = "还差" +Math.Abs(ascmContainer.extendedTime)+"小时即将超期";
                    }
                    if (ascmContainer.extendedTime == 0)
                    {
                        ascmContainer.description = "已超期";
                    }
                    if (ascmContainer.extendedTime < -25 || ascmContainer.extendedTime == -25)
                    {
                        ascmContainer.description = "未超期";
                    }
                }
                string filename = string.Format("超期预警_{0}.xls", System.DateTime.Now.ToString("yyyy-MM-dd"));
                return ExportStream(list, new string[] { "容器编号", "供应商全称", "入厂日期", "截止时间", "是否超期" }, new string[] { "sn", "_supplierName", "storeInTime", "deadline", "description" }, filename, "超期预警");

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        /// <summary>
        /// 导出通用函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="Tablehead"></param>
        /// <param name="PropertyNames"></param>
        /// <param name="fileName"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private FileResult ExportStream<T>(List<T> data, string[] Tablehead, string[] PropertyNames,string fileName,string sheetName=null)
        {
            Aspose.Cells.Workbook book = new Aspose.Cells.Workbook();
            book.BuiltInDocumentProperties.Author = "ASCM系统自动产生";
            book.BuiltInDocumentProperties.Company = "东莞思谷数字有限公司";
            Aspose.Cells.Worksheet sheet = book.Worksheets[0];
            if (!string.IsNullOrEmpty(sheetName))  //产生excel的标题和Sheet名称
            {
                book.BuiltInDocumentProperties.Title = sheetName;
                sheet.Name = sheetName;
            }
            sheet.Cells.ImportArray(Tablehead, 0, 0, false); //写表头
            sheet.Cells.ImportCustomObjects(data, PropertyNames,  //表内容
            false,
            1,
            0,
            data.Count,
            true,
            "dd/mm/yyyy",
            false
            );
            MemoryStream stream = book.SaveToStream();
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/vnd.ms-excel", fileName);
        }
        #endregion

    }
}
