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
using NHibernate;
using YnFrame.Services;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Dal.Vehicle.Entities;
using MideaAscm.Services.Vehicle;
using MideaAscm.Job.Services;
using MideaAscm.Job.Dal.Entities;
using MideaAscm.Dal.Base;
using System.IO;
using NPOI.SS.UserModel;

namespace MideaAscm.Areas.Ascm.Controllers
{
    public class PublicController : YnFrame.Controllers.YnBaseController
    {
        //
        // GET: /Ascm/Share/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FrameHome()
        {
            //框架头
            try
            {
                
            }
            catch (Exception ex)
            {
            }
            return PartialView("FrameHome");
        }
        public ActionResult VehicleIndex()
        {
            //供应商车辆管理
            return View();
        }

        #region ERP用户管理
        public ActionResult ErpUserInfoIndex()
        {
            //用户管理
            return View();
        }
        public ActionResult ErpUserInfoList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmUserInfo> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string organizationId = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketOrganizationId();

                string where = " extExpandType='erp' and not exists (select 1 from Ascm_Ak_Web_User_Sec_Attr b where b.attributeCode='" + AscmAkWebUserSecAttrValues.AttributeCodeDefine.supplierUser + "' and b.webUserId=u.extExpandId) ";
                //where = " extExpandType='erp'";
                //where = " not exists (from AscmAkWebUserSecAttrValues b where b.id=a.extExpandId) ";
                //where = " not exists (from AscmAkWebUserSecAttrValues b where b.id=a.extExpandId) ";
                list = AscmUserInfoService.GetInstance().GetList( ynPage, sort, order, queryWord, where);
                foreach (AscmUserInfo ascmUserInfo in list)
                {
                    //ynUser.listYnDepartment = YnDepartmentService.GetInstance().GetListInUser(ynUser.userId);
                    //ynUser.listYnDepartmentPositionLink = YnDepartmentPositionLinkService.GetInstance().GetListInUser(ynUser.userId);
                    //ynUser.listYnRole = YnRoleService.GetInstance().GetListInUser(ynUser.userId);
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
        #endregion

        #region 供应商用户管理
        public ActionResult SupplierUserInfoIndex()
        {
            //用户管理
            return View();
        }
        public ActionResult SupplierUserInfoList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmUserInfo> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string organizationId = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketOrganizationId();

                string where = " extExpandType='erp' and a.extExpandId in (select b.webUserId from AscmAkWebUserSecAttrValues b where b.attributeCode='" + AscmAkWebUserSecAttrValues.AttributeCodeDefine.supplierUser + "') ";
                list = AscmUserInfoService.GetInstance().GetList(ynPage, sort, order, queryWord, where);
                foreach (AscmUserInfo ascmUserInfo in list)
                {
                    //ynUser.listYnDepartment = YnDepartmentService.GetInstance().GetListInUser(ynUser.userId);
                    //ynUser.listYnDepartmentPositionLink = YnDepartmentPositionLinkService.GetInstance().GetListInUser(ynUser.userId);
                    //ynUser.listYnRole = YnRoleService.GetInstance().GetListInUser(ynUser.userId);
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
        #endregion

        #region 员工管理
        public ActionResult EmployeeIndex()
        {
            //员工管理
            return View();
        }
        public ActionResult EmployeeList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmEmployee> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmEmployeeService.GetInstance().GetList(ynPage, "", "", queryWord,"");
                foreach (AscmEmployee ascmEmployee in list)
                {
                    jsonDataGridResult.rows.Add(ascmEmployee.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmployeeEdit(int? id)
        {
            AscmEmployee ascmEmployee = null;
            try
            {
                if (id.HasValue)
                {
                    ascmEmployee = AscmEmployeeService.GetInstance().Get(id.Value);
                    ascmEmployee.ynDepartment = YnFrame.Services.YnDepartmentService.GetInstance().Get(ascmEmployee.departmentId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmEmployee.GetOwner(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult EmployeeSave(AscmEmployee ascmEmployee_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                AscmEmployee ascmEmployee = null;
                if (id.HasValue)
                {
                    ascmEmployee = AscmEmployeeService.GetInstance().Get(id.Value);
                }
                else
                {
                    ascmEmployee = new AscmEmployee();
                }
                if (ascmEmployee == null)
                    throw new Exception("保存员工基本信息失败！");
                if (ascmEmployee_Model.name == null || ascmEmployee_Model.name.Trim() == "")
                    throw new Exception("员工名称不能为空！");

                ascmEmployee.docNumber = ascmEmployee_Model.docNumber.Trim();
                ascmEmployee.name = ascmEmployee_Model.name.Trim();
                ascmEmployee.sex = ascmEmployee_Model.sex.Trim();
                ascmEmployee.idNumber = ascmEmployee_Model.idNumber.Trim();
                ascmEmployee.email = ascmEmployee_Model.email;
                ascmEmployee.officeTel = ascmEmployee_Model.officeTel;
                ascmEmployee.mobileTel = ascmEmployee_Model.mobileTel;
                ascmEmployee.memo = ascmEmployee_Model.memo;
                ascmEmployee.departmentId = ascmEmployee_Model.departmentId;

                if (!id.HasValue)
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmEmployee where docNumber='" + ascmEmployee_Model.docNumber.Trim() + "'");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已经存在此员工编号【" + ascmEmployee_Model.docNumber.Trim() + "】！");

                    object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmEmployee where name='" + ascmEmployee_Model.name.Trim() + "'");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    //int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已经存在此员工姓名【" + ascmEmployee_Model.name.Trim() + "】！");
                    AscmEmployeeService.GetInstance().Save(ascmEmployee);
                }
                else
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmEmployee where docNumber='" + ascmEmployee_Model.docNumber.Trim() + "' and id<>" + id.Value + "");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已经存在此员工编号【" + ascmEmployee_Model.docNumber.Trim() + "】！");

                    object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmEmployee where name='" + ascmEmployee_Model.name.Trim() + "' and id<>" + id.Value + "");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    //int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已经存在此员工姓名【" + ascmEmployee_Model.name.Trim() + "】！");
                    AscmEmployeeService.GetInstance().Update(ascmEmployee);

                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmEmployee.id.ToString();
                jsonObjectResult.entity = ascmEmployee;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult EmployeeDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            /*
                            //删除与用户的关联
                            string sql = "from YnUserRoleLink where ids.roleId=" + id;
                            IList<YnUserRoleLink> ilistUserRoleLink = YnDaoHelper.GetInstance().nHibernateHelper.Find<YnUserRoleLink>(sql);
                            if (ilistUserRoleLink != null && ilistUserRoleLink.Count > 0)
                            {
                                List<YnUserRoleLink> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnUserRoleLink>(ilistUserRoleLink);
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(list);
                            }
                            //删除与模块的关联
                            sql = "from YnWebModuleRoleLink where ynRole.id=" + id;
                            IList<YnWebModuleRoleLink> ilistModuleRoleLink = YnDaoHelper.GetInstance().nHibernateHelper.Find<YnWebModuleRoleLink>(sql);
                            if (ilistModuleRoleLink != null && ilistModuleRoleLink.Count > 0)
                            {
                                List<YnWebModuleRoleLink> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnWebModuleRoleLink>(ilistModuleRoleLink);
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(list);
                            }
                            //删除角色
                            YnRole ynRole = YnDaoHelper.GetInstance().nHibernateHelper.Get<YnRole>(id.Value);
                            YnDaoHelper.GetInstance().nHibernateHelper.Delete<YnRole>(ynRole);
                            */
                            AscmEmployeeService.GetInstance().Delete(id.Value);

                            tx.Commit();//正确执行提交

                            jsonObjectResult.result = true;
                            jsonObjectResult.message = "";
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmEmployee)", ex);
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
        public ActionResult EmployeeAscxList(int? id,int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmEmployee> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "";
                if (string.IsNullOrEmpty(q))
                {
                    if (id.HasValue)
                    {
                        whereOther = "id=" + id.Value;
                    }
                }
                list = AscmEmployeeService.GetInstance().GetList(ynPage, "", "", q,whereOther);
                foreach (AscmEmployee ascmEmployee in list)
                {
                    jsonDataGridResult.rows.Add(ascmEmployee.GetOwner());
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

        #region 员工车辆管理
        public ActionResult EmployeeCarIndex()
        {
            //员工车辆管理
            return View();
        }
        public ActionResult EmployeeCarList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmEmployeeCar> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmEmployeeCarService.GetInstance().GetList(ynPage, "", "", queryWord,"");
                foreach (AscmEmployeeCar ascmEmployeeCar in list)
                {
                    jsonDataGridResult.rows.Add(ascmEmployeeCar.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmployeeCarEdit(int? id)
        {
            AscmEmployeeCar ascmEmployeeCar = null;
            try
            {
                if (id.HasValue)
                {
                    ascmEmployeeCar = AscmEmployeeCarService.GetInstance().Get(id.Value);
                    //ascmEmployeeCar.ascmEmployee = AscmEmployeeService.GetInstance().Get(ascmEmployeeCar.employeeId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmEmployeeCar.GetOwner(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult EmployeeCarSave(AscmEmployeeCar ascmEmployeeCar_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                AscmEmployeeCar ascmEmployeeCar = null;
                if (id.HasValue)
                {
                    ascmEmployeeCar = AscmEmployeeCarService.GetInstance().Get(id.Value);
                }
                else
                {
                    ascmEmployeeCar = new AscmEmployeeCar();
                }
                if (ascmEmployeeCar == null)
                    throw new Exception("保存员工车辆基本信息失败！");
                if (ascmEmployeeCar_Model.plateNumber == null || ascmEmployeeCar_Model.plateNumber.Trim() == "")
                    throw new Exception("员工车辆车牌号不能为空！");
                if (ascmEmployeeCar_Model.rfid.Length != 10)
                {
                    throw new Exception("rfid编号可能有错误【" + ascmEmployeeCar_Model.rfid.Trim() + "】，请检查,请输入后4位或后6位！");
                }
                //if (ascmEmployeeCar_Model.employeeId==0)
                //    throw new Exception("必须选择输入员工！");

                ascmEmployeeCar.plateNumber = ascmEmployeeCar_Model.plateNumber.Trim();
                ascmEmployeeCar.spec = ascmEmployeeCar_Model.spec;
                ascmEmployeeCar.color = ascmEmployeeCar_Model.color;
                ascmEmployeeCar.seatCount = ascmEmployeeCar_Model.seatCount;
                ascmEmployeeCar.rfid = ascmEmployeeCar_Model.rfid.Trim();
                ascmEmployeeCar.memo = ascmEmployeeCar_Model.memo;
                //ascmEmployeeCar.employeeId = ascmEmployeeCar_Model.employeeId;

                ascmEmployeeCar.employeeDocNumber = ascmEmployeeCar_Model.employeeDocNumber;
                if (!string.IsNullOrEmpty(ascmEmployeeCar.employeeDocNumber))
                    ascmEmployeeCar.employeeDocNumber = ascmEmployeeCar.employeeDocNumber.Trim();

                ascmEmployeeCar.employeeName = ascmEmployeeCar_Model.employeeName;
                if (!string.IsNullOrEmpty(ascmEmployeeCar.employeeName))
                    ascmEmployeeCar.employeeName = ascmEmployeeCar.employeeName.Trim();

                ascmEmployeeCar.employeeSex = ascmEmployeeCar_Model.employeeSex;
                if (!string.IsNullOrEmpty(ascmEmployeeCar.employeeSex))
                    ascmEmployeeCar.employeeSex = ascmEmployeeCar.employeeSex.Trim();

                ascmEmployeeCar.employeeIdNumber = ascmEmployeeCar_Model.employeeIdNumber;
                if (!string.IsNullOrEmpty(ascmEmployeeCar.employeeIdNumber))
                    ascmEmployeeCar.employeeIdNumber = ascmEmployeeCar.employeeIdNumber.Trim();
                ascmEmployeeCar.employeeOfficeTel = ascmEmployeeCar_Model.employeeOfficeTel;
                ascmEmployeeCar.employeeMobileTel = ascmEmployeeCar_Model.employeeMobileTel;
                ascmEmployeeCar.exemption = ascmEmployeeCar_Model.exemption;
                ascmEmployeeCar.employeeLevel = ascmEmployeeCar_Model.employeeLevel;

                AscmRfid ascmRfid_Old = null;
                AscmRfid ascmRfid_New_Update = null;
                AscmRfid ascmRfid_New_Save = null;
                bool _new = true;
                if (!id.HasValue)
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmEmployeeCar where plateNumber='" + ascmEmployeeCar_Model.plateNumber.Trim() + "'");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已经存在此员工车辆车牌号【" + ascmEmployeeCar_Model.plateNumber.Trim() + "】！");
                    if (!string.IsNullOrEmpty(ascmEmployeeCar_Model.rfid))
                    {
                        object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmEmployeeCar where rfid='" + ascmEmployeeCar_Model.rfid.Trim() + "'");
                        if (object1 == null)
                            throw new Exception("查询异常！");
                        //int iCount = 0;
                        if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                            throw new Exception("已经分配此车辆RFID【" + ascmEmployeeCar_Model.rfid.Trim() + "】！");
                    }
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmEmployeeCar");
                    ascmEmployeeCar.id = maxId+1;
                    //AscmEmployeeCarService.GetInstance().Save(ascmEmployeeCar);
                }
                else
                {
                    _new = false;
                    if (ascmEmployeeCar.rfid!=null)
                        ascmRfid_Old = AscmRfidService.GetInstance().Get(ascmEmployeeCar.rfid.Trim());

                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmEmployeeCar where plateNumber='" + ascmEmployeeCar_Model.plateNumber.Trim() + "' and id<>" + id.Value + "");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已经存在此员工车辆车牌号【" + ascmEmployeeCar_Model.plateNumber.Trim() + "】！");
                    if (!string.IsNullOrEmpty(ascmEmployeeCar_Model.rfid))
                    {
                        object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmEmployeeCar where rfid='" + ascmEmployeeCar_Model.rfid.Trim() + "' and id<>" + id.Value + "");
                        if (object1 == null)
                            throw new Exception("查询异常！");
                        //int iCount = 0;
                        if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                            throw new Exception("已经分配此车辆RFID【" + ascmEmployeeCar_Model.rfid.Trim() + "】！");
                    }
                    //AscmEmployeeCarService.GetInstance().Update(ascmEmployeeCar);
                }
                ascmRfid_New_Update = AscmRfidService.GetInstance().Get(ascmEmployeeCar.rfid.Trim());
                if (ascmRfid_Old != null && ascmRfid_New_Update != null)
                {
                    if (ascmRfid_Old.id == ascmRfid_New_Update.id)
                    {
                        ascmRfid_Old = null;
                    }
                }
                if (ascmRfid_Old != null)
                {
                    ascmRfid_Old.bindType = AscmRfid.BindTypeDefine.employeeCar;
                    ascmRfid_Old.bindId = "";
                    ascmRfid_Old.status = AscmRfid.StatusDefine.none;
                    //ascmRfid_Old.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (ascmRfid_New_Update != null)
                {
                    ascmRfid_New_Update.bindType = AscmRfid.BindTypeDefine.employeeCar;
                    ascmRfid_New_Update.bindId = ascmEmployeeCar.id.ToString();
                    ascmRfid_New_Update.status = AscmRfid.StatusDefine.inUse;
                    //ascmRfid_New_Update.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    ascmRfid_New_Save = new AscmRfid();
                    ascmRfid_New_Save.id = ascmEmployeeCar.rfid;
                    ascmRfid_New_Save.createUser = "";
                    //ascmRfid_New_Save.createTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                    //ascmRfid_New_Save.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ascmRfid_New_Save.bindType = AscmRfid.BindTypeDefine.employeeCar;
                    ascmRfid_New_Save.bindId = ascmEmployeeCar.id.ToString();
                    ascmRfid_New_Save.status = AscmRfid.StatusDefine.inUse;
                }
                /*
                if (string.IsNullOrEmpty(ascmEmployeeCar_Model.rfid))
                {
                    ascmEmployeeCar.rfid = "";
                    //取消绑定
                    if (ascmRfid_Old != null)
                    {
                        ascmRfid_Old.bindId = "";
                        ascmRfid_Old.status = "";
                    }
                }
                else
                {
                    //增加绑定
                    ascmRfid_New = AscmRfidService.GetInstance().Get(ascmEmployeeCar_Model.rfid.Trim());
                    if (ascmRfid_New == null)
                        throw new Exception("RFID标签号码不存在！");
                    if (ascmRfid_New.bindType != AscmRfid.BindTypeDefine.employeeCar)
                        throw new Exception("请选择[" + AscmRfid.BindTypeDefine.DisplayText(ascmRfid_New.bindType) + "]类型的RFID标签!");
                    if (ascmRfid_Old != null)
                    {
                        //存在原绑定
                        if (ascmRfid_Old.id == ascmRfid_New.id)
                        {
                            //没有改变绑定
                            ascmRfid_Old = null;
                        }
                        else
                        {
                            //新绑定
                            ascmRfid_Old.bindId = "";
                            ascmRfid_Old.status = "";

                            if (!string.IsNullOrEmpty(ascmRfid_New.status) && ascmRfid_New.status != AscmRfid.StatusDefine.cancel)
                                throw new Exception("RFID标签【" + ascmRfid_New.id + "】已经处于[" + AscmRfid.StatusDefine.DisplayText(ascmRfid_New.status) + "]状态,不能重复绑定!");
                            if (!string.IsNullOrEmpty(ascmRfid_New.bindId))
                                throw new Exception("RFID标签【" + ascmRfid_New.id + "】已经绑定[" + ascmRfid_New.bindId + "],不能重复绑定!");

                            ascmRfid_New.bindId = ascmEmployeeCar.id.ToString();
                            ascmRfid_New.status = "";
                            ascmEmployeeCar.rfid = ascmRfid_New.id;
                        }
                    }
                    else
                    {
                        ascmRfid_New.bindId = ascmEmployeeCar.id.ToString();
                        ascmRfid_New.status = "";
                        ascmEmployeeCar.rfid = ascmRfid_New.id;
                    }
                }
                */
                AscmEmployeeCarService.GetInstance().Save(_new, ascmEmployeeCar, ascmRfid_Old, ascmRfid_New_Update, ascmRfid_New_Save);
                /*
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        if (!id.HasValue)
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmEmployeeCar);
                        }
                        else
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmEmployeeCar);
                        }
                        if (ascmRfid_Old!=null)
                            YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmRfid_Old);
                        if (ascmRfid_New != null)
                            YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmRfid_New);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }*/
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmEmployeeCar.id.ToString();
                jsonObjectResult.entity = ascmEmployeeCar;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult EmployeeCarDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            /*
                            //删除与用户的关联
                            string sql = "from YnUserRoleLink where ids.roleId=" + id;
                            IList<YnUserRoleLink> ilistUserRoleLink = YnDaoHelper.GetInstance().nHibernateHelper.Find<YnUserRoleLink>(sql);
                            if (ilistUserRoleLink != null && ilistUserRoleLink.Count > 0)
                            {
                                List<YnUserRoleLink> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnUserRoleLink>(ilistUserRoleLink);
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(list);
                            }
                            //删除与模块的关联
                            sql = "from YnWebModuleRoleLink where ynRole.id=" + id;
                            IList<YnWebModuleRoleLink> ilistModuleRoleLink = YnDaoHelper.GetInstance().nHibernateHelper.Find<YnWebModuleRoleLink>(sql);
                            if (ilistModuleRoleLink != null && ilistModuleRoleLink.Count > 0)
                            {
                                List<YnWebModuleRoleLink> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnWebModuleRoleLink>(ilistModuleRoleLink);
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(list);
                            }
                            //删除角色
                            YnRole ynRole = YnDaoHelper.GetInstance().nHibernateHelper.Get<YnRole>(id.Value);
                            YnDaoHelper.GetInstance().nHibernateHelper.Delete<YnRole>(ynRole);
                            */
                            AscmEmployeeCarService.GetInstance().Delete(id.Value);

                            tx.Commit();//正确执行提交

                            jsonObjectResult.result = true;
                            jsonObjectResult.message = "";
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmEmployeeCar)", ex);
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

        #region RFID管理
        public ActionResult RfidIndex()
        {
            //RFID管理
            return View();
        }
        public ActionResult RfidList(int? page, int? rows, string sort, string order, string queryWord, string queryType)
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
                list = AscmRfidService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
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
        [HttpPost]
        public ContentResult RfidSave(string rfidStart, string rfidEnd, string bindTypeDefine)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (string.IsNullOrEmpty(rfidStart) || string.IsNullOrEmpty(rfidEnd))
                    throw new Exception("RFID号码段录入错误!");
                long _rfidStart = 0;
                long _rfidEnd = 0;
                long.TryParse(rfidStart, out _rfidStart);
                long.TryParse(rfidEnd, out _rfidEnd);
                if (_rfidStart == 0 || _rfidEnd == 0 || _rfidStart > _rfidEnd)
                    throw new Exception("RFID号码段录入错误!");
                //if (_rfidEnd - _rfidStart + 1 != giftCardPublishMain_Model.number)
                //    throw new Exception("礼卡号码段与录入数量不匹配!");
                if(_rfidEnd - _rfidStart+1>500)
                    throw new Exception("一次添加不能超过500!");
                if (string.IsNullOrEmpty(bindTypeDefine))
                    throw new Exception("必须选择RFID类型！");

                List<AscmRfid> listAscmRfid_New = new List<AscmRfid>();
                List<AscmRfid> listAscmRfid = AscmRfidService.GetInstance().GetList(" from AscmRfid where id>='" + rfidStart + "' and id<='" + rfidEnd + "'");
                for (long code = _rfidStart; code <= _rfidEnd; code++)
                {
                    string sCode = string.Format("{0:" + YnBaseClass2.Helper.StringHelper.Repeat('0', rfidStart.Trim().Length) + "}", code);
                    AscmRfid ascmRfid = listAscmRfid.Find(P => P.id == sCode);
                    if (ascmRfid != null)
                        throw new Exception("RFID已经存在[" + sCode + "]!");
                    ascmRfid = new AscmRfid();
                    ascmRfid.id = sCode;
                    ascmRfid.bindType = bindTypeDefine;
                    ascmRfid.bindId = "";
                    ascmRfid.status = "";
                    listAscmRfid_New.Add(ascmRfid);
                }

                AscmRfidService.GetInstance().Save(listAscmRfid_New);

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                //jsonObjectResult.id = ascmEmployeeCar.id.ToString();
                //jsonObjectResult.entity = ascmEmployeeCar;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        [HttpPost]
        public ContentResult RfidDelete(string rfidStart, string rfidEnd, string bindTypeDefine)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (string.IsNullOrEmpty(rfidStart) || string.IsNullOrEmpty(rfidEnd))
                    throw new Exception("RFID号码段录入错误!");
                long _rfidStart = 0;
                long _rfidEnd = 0;
                long.TryParse(rfidStart, out _rfidStart);
                long.TryParse(rfidEnd, out _rfidEnd);
                if (_rfidStart == 0 || _rfidEnd == 0 || _rfidStart > _rfidEnd)
                    throw new Exception("RFID号码段录入错误!");
                //if (_rfidEnd - _rfidStart + 1 != giftCardPublishMain_Model.number)
                //    throw new Exception("礼卡号码段与录入数量不匹配!");
                if (_rfidEnd - _rfidStart + 1 > 500)
                    throw new Exception("一次删除不能超过500!");
                if (string.IsNullOrEmpty(bindTypeDefine))
                    throw new Exception("必须选择RFID类型！");

                List<AscmRfid> listAscmRfid = AscmRfidService.GetInstance().GetList(" from AscmRfid where id>='" + rfidStart + "' and id<='" + rfidEnd + "'");
                for (long code = _rfidStart; code <= _rfidEnd; code++)
                {
                    string sCode = string.Format("{0:" + YnBaseClass2.Helper.StringHelper.Repeat('0', rfidStart.Trim().Length) + "}", code);
                    AscmRfid ascmRfid = listAscmRfid.Find(P => P.id == sCode);
                    if (ascmRfid == null)
                        throw new Exception("RFID不存在[" + sCode + "]!");
                    if (!string.IsNullOrEmpty(ascmRfid.status) && ascmRfid.status != AscmRfid.StatusDefine.cancel)
                        throw new Exception("不能删除状态[" + AscmRfid.StatusDefine.DisplayText(ascmRfid.status) + "]电子标签!");
                    if (ascmRfid.bindType != bindTypeDefine)
                        throw new Exception("RFID“" + sCode + "”类型不符[" + AscmRfid.BindTypeDefine.DisplayText(ascmRfid.bindType) + "]!");
                }

                AscmRfidService.GetInstance().Delete(listAscmRfid);

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                //jsonObjectResult.id = ascmEmployeeCar.id.ToString();
                //jsonObjectResult.entity = ascmEmployeeCar;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult RfidAscxList(int? id, int? page, int? rows, string sort, string order, string q)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmRfid> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string sql = "from AscmRfid where bindType = 'FORKLIFT'";
                IList<AscmRfid> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmRfid>(sql, sql, ynPage);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmRfid>(ilist);
                    foreach (AscmRfid ascmRfid in list)
                    {
                        jsonDataGridResult.rows.Add(ascmRfid);
                    }
                }
                
                //list = AscmRfidService.GetInstance().GetList(ynPage, "", "", q, null);
                //if (list != null)
                //{
                //    foreach (AscmRfid ascmRfid in list)
                //    {
                //        jsonDataGridResult.rows.Add(ascmRfid);
                //    }
                //}
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult,JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 读写器管理
        public ActionResult ReadingHeadIndex()
        {
            //RFID管理
            return View();
        }
        public ActionResult ReadingHeadList(int? page, int? rows, string sort, string order, string queryWord, string queryType, string bingType)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmReadingHead> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "";
                if (!string.IsNullOrEmpty(queryType))
                {
                    whereOther = " bindType='" + queryType.Trim() + "' ";
                }
                if (!string.IsNullOrEmpty(bingType))
                {
                    if (whereOther != "")
                        whereOther += " and ";
                    whereOther = " bindType='" + bingType.Trim() + "' ";
                }
                list = AscmReadingHeadService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                foreach (AscmReadingHead ascmReadingHead in list)
                {
                    jsonDataGridResult.rows.Add(ascmReadingHead.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadingHeadEdit(int? id)
        {
            AscmReadingHead ascmReadingHead = null;
            try
            {
                if (id.HasValue)
                {
                    ascmReadingHead = AscmReadingHeadService.GetInstance().Get(id.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmReadingHead.GetOwner(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult ReadingHeadSave(AscmReadingHead ascmReadingHead_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                AscmReadingHead ascmReadingHead = null;
                if (id.HasValue)
                {
                    ascmReadingHead = AscmReadingHeadService.GetInstance().Get(id.Value);
                }
                else
                {
                    ascmReadingHead = new AscmReadingHead();
                }
                if (ascmReadingHead == null)
                    throw new Exception("保存RFID读头信息失败！");
                //if (string.IsNullOrEmpty(ascmReadingHead_Model.bindType))
                //    throw new Exception("必须选择RFID类型！");
                if (string.IsNullOrEmpty(ascmReadingHead_Model.ip))
                    throw new Exception("必须输入ip地址！");
                //if (ascmReadingHead_Model.plateNumber == null || ascmReadingHead_Model.plateNumber.Trim() == "")
                //    throw new Exception("员工车辆车牌号不能为空！");

                ascmReadingHead.bindType = ascmReadingHead_Model.bindType;
                ascmReadingHead.bindId = "";
                ascmReadingHead.ip = ascmReadingHead_Model.ip.Trim();
                ascmReadingHead.port = ascmReadingHead_Model.port;
                ascmReadingHead.status = "";
                if (ascmReadingHead_Model.address != null && ascmReadingHead_Model.address != "")
                    ascmReadingHead.address = ascmReadingHead_Model.address.Trim();


                if (!id.HasValue)
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmReadingHead where ip='" + ascmReadingHead_Model.ip.Trim() + "'");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已经存在此ip【" + ascmReadingHead_Model.ip.Trim() + "】！");
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmReadingHead");
                    ascmReadingHead.id = maxId + 1;
                }
                else
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmReadingHead where ip='" + ascmReadingHead_Model.ip.Trim() + "' and id<>" + id.Value + "");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已经存在此ip【" + ascmReadingHead_Model.ip.Trim() + "】！");
                    //AscmEmployeeCarService.GetInstance().Update(ascmEmployeeCar);
                }

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        if (!id.HasValue)
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmReadingHead);
                        }
                        else
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmReadingHead);
                        }

                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmReadingHead.id.ToString();
                jsonObjectResult.entity = ascmReadingHead;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult ReadingHeadDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            /*
                            //删除与用户的关联
                            string sql = "from YnUserRoleLink where ids.roleId=" + id;
                            IList<YnUserRoleLink> ilistUserRoleLink = YnDaoHelper.GetInstance().nHibernateHelper.Find<YnUserRoleLink>(sql);
                            if (ilistUserRoleLink != null && ilistUserRoleLink.Count > 0)
                            {
                                List<YnUserRoleLink> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnUserRoleLink>(ilistUserRoleLink);
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(list);
                            }
                            //删除与模块的关联
                            sql = "from YnWebModuleRoleLink where ynRole.id=" + id;
                            IList<YnWebModuleRoleLink> ilistModuleRoleLink = YnDaoHelper.GetInstance().nHibernateHelper.Find<YnWebModuleRoleLink>(sql);
                            if (ilistModuleRoleLink != null && ilistModuleRoleLink.Count > 0)
                            {
                                List<YnWebModuleRoleLink> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<YnWebModuleRoleLink>(ilistModuleRoleLink);
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(list);
                            }
                            //删除角色
                            YnRole ynRole = YnDaoHelper.GetInstance().nHibernateHelper.Get<YnRole>(id.Value);
                            YnDaoHelper.GetInstance().nHibernateHelper.Delete<YnRole>(ynRole);
                            */
                            AscmReadingHeadService.GetInstance().Delete(id.Value);

                            tx.Commit();//正确执行提交

                            jsonObjectResult.result = true;
                            jsonObjectResult.message = "";
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmEmployeeCar)", ex);
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

        #region 读头日志
        public ActionResult ReadingHeadLogIndex()
        {
            return View();
        }
        public ActionResult ReadingHeadLogList(int? page, int? rows, string sort, string order, string queryWord, int? readingHeadId, string queryStartTime, string queryEndTime)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereReadingHead = "";
                string whereStartCreateTime = "", whereEndCreateTime = "";
                if (readingHeadId.HasValue)
                    whereReadingHead = "readingHeadId=" + readingHeadId.Value;
                DateTime dtStartCreateTime, dtEndCreateTime;
                if (!string.IsNullOrEmpty(queryStartTime) && DateTime.TryParse(queryStartTime, out dtStartCreateTime))
                    whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(queryEndTime) && DateTime.TryParse(queryEndTime, out dtEndCreateTime))
                    whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereReadingHead);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);

                List<AscmReadingHeadLog> list = AscmReadingHeadLogService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                if (list != null)
                {
                    foreach (AscmReadingHeadLog ascmReadingHeadLog in list)
                    {
                        jsonDataGridResult.rows.Add(ascmReadingHeadLog);
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

        #region 车门
        public ActionResult DoorIndex()
        {
            //车门、车道管理管理
            return View();
        }
        public ActionResult DoorList()
        {
            List<AscmDoor> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmDoorService.GetInstance().GetList();
                if (list != null)
                {
                    foreach (AscmDoor ascmDoor in list)
                    {
                        jsonDataGridResult.rows.Add(ascmDoor.GetOwner());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DoorEdit(int? id)
        {
            AscmDoor ascmDoor = null;
            try
            {
                if (id.HasValue)
                {
                    ascmDoor = AscmDoorService.GetInstance().Get(id.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ascmDoor.GetOwner(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ContentResult DoorSave(AscmDoor ascmDoor_Model, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                AscmDoor ascmDoor = null;
                if (id.HasValue)
                {
                    ascmDoor = AscmDoorService.GetInstance().Get(id.Value);
                }
                else
                {
                    ascmDoor = new AscmDoor();
                    ascmDoor.enabled = true;
                }
                if (ascmDoor == null)
                    throw new Exception("保存大门信息失败！");
                if (string.IsNullOrEmpty(ascmDoor_Model.name))
                    throw new Exception("必须输入名称！");
                //if (ascmReadingHead_Model.plateNumber == null || ascmReadingHead_Model.plateNumber.Trim() == "")
                //    throw new Exception("员工车辆车牌号不能为空！");

                ascmDoor.name = ascmDoor_Model.name.Trim();
                ascmDoor.direction = ascmDoor_Model.direction;
                ascmDoor.vehicleType = ascmDoor_Model.vehicleType;
                ascmDoor.description = ascmDoor_Model.description;


                if (!id.HasValue)
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmDoor where name='" + ascmDoor_Model.name.Trim() + "'");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已经存在大门【" + ascmDoor_Model.name.Trim() + "】！");
                    int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmDoor");
                    ascmDoor.id = maxId + 1;
                }
                else
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmDoor where name='" + ascmDoor_Model.name.Trim() + "' and id<>" + id.Value + "");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount > 0)
                        throw new Exception("已经存在大门【" + ascmDoor_Model.name.Trim() + "】！");
                    //AscmEmployeeCarService.GetInstance().Update(ascmEmployeeCar);
                }

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        if (!id.HasValue)
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmDoor);
                        }
                        else
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmDoor);
                        }

                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }
                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
                jsonObjectResult.id = ascmDoor.id.ToString();
                jsonObjectResult.entity = ascmDoor;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult DoorDelete(int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue)
                {
                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            AscmDoorService.GetInstance().Delete(id.Value);

                            tx.Commit();//正确执行提交

                            jsonObjectResult.result = true;
                            jsonObjectResult.message = "";
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmDoor)", ex);
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
        public ActionResult DoorReadingHeadList(int? doorId, string sort, string order)
        {

            List<AscmReadingHead> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (doorId.HasValue)
                {
                    string sql = "from AscmReadingHead where bindType='" + AscmReadingHead.BindTypeDefine.employeeCar + "' and bindId='" + doorId.Value+"'";
                    list = AscmReadingHeadService.GetInstance().GetList(sql);
                    foreach (AscmReadingHead ascmReadingHead in list)
                    {
                        jsonDataGridResult.rows.Add(ascmReadingHead.GetOwner());
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
        public ActionResult DoorAddReadingHead(int? id, string ids)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue && !string.IsNullOrEmpty(ids))
                {
                    string[] arrId = ids.Split(',');
                    string whereId = "";
                    foreach (string sid in arrId)
                    {
                        if (sid.Trim() != "")
                        {
                            if (whereId.Trim() != "")
                                whereId += ",";
                            whereId += sid.Trim();
                        }
                    }
                    if (whereId != "")
                    {
                        List<AscmReadingHead> list_Update = new List<AscmReadingHead>();
                        string sql = "from AscmReadingHead where id in (" + whereId + ")";
                        List<AscmReadingHead> list = AscmReadingHeadService.GetInstance().GetList(sql);
                        foreach (AscmReadingHead ascmReadingHead in list)
                        {
                            if (ascmReadingHead.bindId != id.Value.ToString())
                            {
                                ascmReadingHead.bindId = id.Value.ToString();
                                list_Update.Add(ascmReadingHead);
                            }
                        }
                        if (list_Update.Count > 0)
                        {
                            AscmReadingHeadService.GetInstance().Update(list_Update);
                        }
                    }
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
        public ActionResult DoorRemoveReadingHead(int? id, string ids)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (id.HasValue && !string.IsNullOrEmpty(ids))
                {
                    string[] arrId = ids.Split(',');
                    string whereId = "";
                    foreach (string sid in arrId)
                    {
                        if (sid.Trim() != "")
                        {
                            if (whereId.Trim() != "")
                                whereId += ",";
                            whereId += sid.Trim();
                        }
                    }
                    if (whereId != "")
                    {
                        string sql = "from AscmReadingHead where id in (" + whereId + ")";
                        List<AscmReadingHead> list = AscmReadingHeadService.GetInstance().GetList(sql);
                        foreach (AscmReadingHead ascmReadingHead in list)
                        {
                            ascmReadingHead.bindId = "";
                        }
                        if (list.Count > 0)
                        {
                            AscmReadingHeadService.GetInstance().Update(list);
                        }
                    }
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
        #endregion

        #region 物料管理
        public ActionResult MaterialIndex()
        {
            //物料表
            return View();
        }
        public ActionResult MaterialList(int? page, int? rows, string sort, string order, string queryWord, string categoryCode)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "", whereCategoryCode = "";
                //根据物料大类筛选，‘0000’为通用
                if (categoryCode != null && categoryCode != "" && categoryCode != "0000")
                    whereCategoryCode = "substr(docNumber,1,4)='" + categoryCode + "'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereCategoryCode);

                List<AscmMaterialItem> list = AscmMaterialItemService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                foreach (AscmMaterialItem ascmMaterialItem in list)
                {
                    jsonDataGridResult.rows.Add(ascmMaterialItem.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MaterialQueryList(int? page, int? rows, string sort, string order, string queryWord, string startInventoryItemId, string endInventoryItemId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string whereOther = "";

                #region 物料编码
                string whereStartInventoryItemId = "", whereEndInventoryItemId = "";
                if (startInventoryItemId != null && startInventoryItemId != "")
                {
                    whereStartInventoryItemId = " docNumber='" + startInventoryItemId.Trim() + "')";
                }
                if (endInventoryItemId != null && endInventoryItemId != "")
                {
                    if (startInventoryItemId != null && startInventoryItemId != "")
                    {
                        whereStartInventoryItemId = " docNumber>='" + startInventoryItemId.Trim() + "' and docNumber<='" + endInventoryItemId.Trim() + "')";
                    }
                    else
                    {
                        whereEndInventoryItemId = " docNumber<='" + endInventoryItemId.Trim() + "')";
                    }
                }
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartInventoryItemId);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndInventoryItemId);
                #endregion

                List<AscmMaterialItem> list = AscmMaterialItemService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                foreach (AscmMaterialItem ascmMaterialItem in list)
                {
                    jsonDataGridResult.rows.Add(ascmMaterialItem.GetOwner());
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
		public ActionResult MaterialAscxList(int? page, int? rows, string sort, string order, string q, string subInventory)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmMaterialItem> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
				string whereOther = "";
//                if (!string.IsNullOrEmpty(subInventory)) 
//                {
//                    whereOther = string.Format(@" id in 
//						(select a.pk.materialId from AscmLocationMaterialLink a 
//							inner join AscmWarelocation b on b.id = a.pk.warelocationId and b.warehouseId='{0}') ", subInventory);
//                }

				list = AscmMaterialItemService.GetInstance().GetList(ynPage, "", "", q, whereOther);
                foreach (AscmMaterialItem ascmMaterialItem in list)
                {
                    jsonDataGridResult.rows.Add(ascmMaterialItem.GetOwner());
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

        #region 作业管理
        public ActionResult JobIndex()
        {
            return View();
        }
        public ActionResult JobList(int? page, int? rows, string sort, string order, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmJob> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmJobService.GetInstance().GetList(ynPage, sort, order, queryWord);
                if (list != null)
                {
                    foreach (AscmJob ascmJob in list)
                    {
                        jsonDataGridResult.rows.Add(ascmJob);
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
        public ActionResult LoadJob(string jobName)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (jobName == null || jobName.Trim() == "")
                    throw new Exception("请选择作业！");
                AscmJob ascmJob = AscmJobService.GetInstance().Get(jobName.Trim());
                jsonObjectResult.result = true;
                jsonObjectResult.id = jobName.Trim();
                jsonObjectResult.entity = ascmJob;
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadJobLog(int? page, int? rows, string sort, string order, string jobName, string queryWord)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            List<AscmJobLog> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (jobName != null && jobName.Trim() != "")
                {
                    list = AscmJobLogService.GetInstance().GetList(ynPage, sort, order, jobName, queryWord);
                    if (list != null)
                    {
                        foreach (AscmJobLog ascmJobLog in list)
                        {
                            jsonDataGridResult.rows.Add(ascmJobLog);
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
        [HttpPost]
        public ContentResult SaveJob(AscmJob ascmJob_Model, string id, string argumentJson)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (ascmJob_Model == null)
                    throw new Exception("传参错误！");
                if (ascmJob_Model.jobName == null || ascmJob_Model.jobName.Trim() == "")
                    throw new Exception("作业名称不能为空！");
                //bool isAdd = (id == null || id.Trim() == "");
                bool isAdd = false;
                string jobName = ascmJob_Model.jobName.Trim();
                AscmJob ascmJob = AscmJobService.GetInstance().Get(jobName);
                if (ascmJob == null)
                {
                    ascmJob = new AscmJob();
                    ascmJob.jobName = jobName;

                    isAdd = true;
                }
                ascmJob.comments = ascmJob_Model.comments.Trim();
                ascmJob.enabled = ascmJob_Model.enabled;
                ascmJob.autoDrop = ascmJob_Model.autoDrop;
                ascmJob.startDate = ascmJob_Model.startDate;
                JobRepeatInterval jobRepeatInterval = new JobRepeatInterval(
                    ascmJob_Model.freq,
                    ascmJob_Model.interval,
                    ascmJob_Model.byMonthDay,
                    ascmJob_Model.byDay,
                    ascmJob_Model.byTime);
                ascmJob.repeatInterval = jobRepeatInterval.ToString();
                ascmJob.jobAction = ascmJob_Model.jobAction;
                List<AscmProcedureArgument> listProcedureArgument = null;
                if (!string.IsNullOrEmpty(argumentJson))
                    listProcedureArgument = JsonConvert.DeserializeObject<List<AscmProcedureArgument>>(argumentJson);
                //if (!string.IsNullOrEmpty(argumentJson))
                //{
                //    List<AscmProcedureArgument> listProcedureArgument_Model = JsonConvert.DeserializeObject<List<AscmProcedureArgument>>(argumentJson);
                //    if (listProcedureArgument_Model != null)
                //    {
                //        listProcedureArgument = new List<AscmProcedureArgument>();
                //        foreach (AscmProcedureArgument ascmProcedureArgument in listProcedureArgument_Model)
                //        {
                //            if (!string.IsNullOrEmpty(ascmProcedureArgument.argumentName))
                //                listProcedureArgument.Add(ascmProcedureArgument);
                //        }
                //    }
                //}
                ascmJob.listProcedureArgument = listProcedureArgument;
                if (isAdd)
                    AscmJobService.GetInstance().CreateJob(ascmJob);
                else
                    AscmJobService.GetInstance().UpdateJob(ascmJob);
                jsonObjectResult.result = true;
                jsonObjectResult.id = jobName;
                jsonObjectResult.message = isAdd ? "作业创建成功！" : "作业修改成功！";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult RunJob(string jobName)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (jobName == null || jobName.Trim() == "")
                    throw new Exception("请选择要运行的作业！");
                AscmJobService.GetInstance().RunJob(jobName.Trim());
                jsonObjectResult.result = true;
                jsonObjectResult.message = "作业【" + jobName.Trim() + "】运行成功！";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StopJob(string jobName)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (jobName == null || jobName.Trim() == "")
                    throw new Exception("请选择要停止的作业！");
                AscmJobService.GetInstance().StopJob(jobName.Trim());
                jsonObjectResult.result = true;
                jsonObjectResult.message = "作业【" + jobName.Trim() + "】停止成功！";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DropJob(string jobName)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (jobName == null || jobName.Trim() == "")
                    throw new Exception("请选择要删除的作业！");
                AscmJobService.GetInstance().DropJob(jobName.Trim());
                jsonObjectResult.result = true;
                jsonObjectResult.message = "作业【" + jobName.Trim() + "】删除成功！";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            return Json(jsonObjectResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProcedureList(string q)
        {
            List<AscmProcedure> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                list = AscmProcedureService.GetInstance().GetList(q);
                if (list != null)
                {
                    foreach (AscmProcedure ascmProceduresView in list)
                    {
                        jsonDataGridResult.rows.Add(ascmProceduresView);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProcedureArgumentList(string jobName, string jobAction, string procedureJson)
        {
            List<AscmProcedureArgument> list = null;
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                string _procedureName = "";
                if (!string.IsNullOrEmpty(procedureJson))
                {
                    AscmProcedure ascmProcedure = JsonConvert.DeserializeObject<AscmProcedure>(procedureJson);
                    if (ascmProcedure != null)
                    {
                        _procedureName = ascmProcedure._procedureName;
                        list = AscmProcedureArgumentService.GetInstance().GetList(ascmProcedure.objectType, ascmProcedure.objectName, ascmProcedure.procedureName);
                    }
                }
                if (list != null)
                {
                    List<AscmJobArgument> listJobArgument = null;
                    if (!string.IsNullOrEmpty(jobName) && !string.IsNullOrEmpty(_procedureName) &&
                        _procedureName.Equals(jobAction, StringComparison.CurrentCultureIgnoreCase))
                    {
                        listJobArgument = AscmJobArgumentService.GetInstance().GetList(jobName);
                    }
                    foreach (AscmProcedureArgument ascmProcedureArgument in list)
                    {
                        if (listJobArgument != null)
                        {
                            AscmJobArgument ascmJobArgument = listJobArgument.Find(P => P.argumentPosition == ascmProcedureArgument.position);
                            if (ascmJobArgument != null)
                                ascmProcedureArgument.value = ascmJobArgument.value;
                        }
                        jsonDataGridResult.rows.Add(ascmProcedureArgument);
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

        #region 参数管理
        public class ParameterModel
        {
            #region 公共
            /// <summary>大门LED名称</summary>
            public string doorLedTitle { get; set; }
            /// <summary>卸货点预约失效时间（单位分钟）</summary>
            public int reserveInvalid { get; set; }
            /// <summary>供应商到厂放行时长（0.5≤ Tc ≤  24，Tc为0.5的整数倍）</summary>
            public decimal supplierPassDuration { get; set; }
            /// <summary>员工车辆免检级别</summary>
            public string employeeCarExemptionLevel { get; set; }
            #endregion

            public ParameterModel GetOwner()
            {
                return (ParameterModel)this.MemberwiseClone();
            }
        }
        public ActionResult ParameterIndex()
        {
            ParameterModel parameterModel = new ParameterModel();
            #region 公共
            //大门LED名称
            parameterModel.doorLedTitle = YnParameterService.GetInstance().GetValue(MyParameter.doorLedTitle);
            //卸货点预约失效时间
            int reserveInvalid = 5;
            int.TryParse(YnParameterService.GetInstance().GetValue(MyParameter.reserveInvalid), out reserveInvalid);
            parameterModel.reserveInvalid = reserveInvalid;
            //供应商到厂放行时长
            decimal supplierPassDuration = (decimal)0.5;
            decimal.TryParse(YnParameterService.GetInstance().GetValue(MyParameter.supplierPassDuration), out supplierPassDuration);
            parameterModel.supplierPassDuration = supplierPassDuration;
            //员工车辆免检级别
            parameterModel.employeeCarExemptionLevel = YnParameterService.GetInstance().GetValue(MyParameter.employeeCarExemptionLevel);
            #endregion

            return View(parameterModel);
        }
        [HttpPost]
        public ContentResult ParameterSave(ParameterModel parameterModel, int? id)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                //公共
                YnParameterService.GetInstance().SetValue(MyParameter.doorLedTitle, parameterModel.doorLedTitle.Trim());
                YnParameterService.GetInstance().SetValue(MyParameter.reserveInvalid, parameterModel.reserveInvalid.ToString());

                decimal supplierPassDuration = (decimal)0.5;
                supplierPassDuration = (int)(parameterModel.supplierPassDuration / (decimal)0.5) * (decimal)0.5;
                if (supplierPassDuration > 24)
                    supplierPassDuration = 24;
                if (supplierPassDuration < 0)
                    supplierPassDuration = 0;
                YnParameterService.GetInstance().SetValue(MyParameter.supplierPassDuration, supplierPassDuration.ToString("0.0"));

                string employeeCarExemptionLevel_old = YnParameterService.GetInstance().GetValue(MyParameter.employeeCarExemptionLevel);
                YnParameterService.GetInstance().SetValue(MyParameter.employeeCarExemptionLevel, parameterModel.employeeCarExemptionLevel.Trim());
                //设置员工车辆免检

                if (employeeCarExemptionLevel_old.Trim() != parameterModel.employeeCarExemptionLevel.Trim())
                {
                    AscmEmployeeCarService.GetInstance().SetExemption(parameterModel.employeeCarExemptionLevel.Trim());
                }

                jsonObjectResult.result = true;
                jsonObjectResult.message = "";
            }
            catch (Exception ex)
            {
                jsonObjectResult.message = ex.Message;
            }
            string result = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(result);
        }
        #endregion

        #region 供应商物料导入
        public ActionResult SupplierMaterialIndex()
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
        public ActionResult SupplierMaterialList(int? page, int? rows, string sort, string order, int? supplierId, string queryWord, string startInventoryItemId, string endInventoryItemId)
        {
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(rows.HasValue ? rows.Value : YnBaseDal.YnPage.DEFAULT_PAGE_SIZE); //pageRows;
            ynPage.SetCurrentPage(page.HasValue ? page.Value : 1); //pageNumber;

            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (supplierId.HasValue)
                {
                    List<AscmSupplierMaterialLink> listAscmSupplierMaterialLink = AscmSupplierMaterialLinkService.GetInstance().GetList(supplierId.Value);
                    List<AscmMaterialItem> listAscmMaterialItem = null;
                    if (listAscmSupplierMaterialLink != null && listAscmSupplierMaterialLink.Count() > 0)
                    {
                        string materialIds = string.Empty;
                        foreach (AscmSupplierMaterialLink ascmSupplierMaterialLink in listAscmSupplierMaterialLink)
                        {
                            if (!string.IsNullOrEmpty(materialIds))
                                materialIds += ",";
                            materialIds += ascmSupplierMaterialLink.materialId;
                        }
                        if (!string.IsNullOrEmpty(materialIds))
                        {
                            string[] Ids = materialIds.Split(',');
                            int iCount = Ids.Length;
                            string sWhere = string.Empty;
                            if (iCount > 900)
                            {
                                string ids = string.Empty;
                                for (int i = 0; i < iCount; i++)
                                {
                                    if (!string.IsNullOrEmpty(ids))
                                        ids += ",";
                                    ids += Ids.ElementAt(i);
                                    if ((i + 1) % 900 == 0 || (i + 1) == iCount)
                                    {
                                        if (!string.IsNullOrEmpty(sWhere))
                                            sWhere += " or ";
                                        sWhere += "id in (" + ids + ")";
                                        ids = string.Empty;
                                    }
                                }
                            }
                            else
                                sWhere = "id in (" + materialIds + ")";

                            string whereStartInventoryItemId = string.Empty, whereEndInventoryItemId = string.Empty, sWhereId = string.Empty;
                            if (!string.IsNullOrEmpty(startInventoryItemId))
                            {
                                whereStartInventoryItemId = " docNumber='" + startInventoryItemId.Trim() + "'";
                            }
                            if (!string.IsNullOrEmpty(endInventoryItemId))
                            {
                                if (!string.IsNullOrEmpty(startInventoryItemId))
                                {
                                    whereStartInventoryItemId = " docNumber>='" + startInventoryItemId.Trim() + "' and docNumber<='" + endInventoryItemId.Trim() + "'";
                                }
                                else
                                {
                                    whereEndInventoryItemId = " docNumber<='" + endInventoryItemId.Trim() + "'";
                                }
                            }
                            sWhereId = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(sWhereId, whereStartInventoryItemId);
                            sWhereId = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(sWhereId, whereEndInventoryItemId);
                            if (!string.IsNullOrEmpty(sWhereId))
                                sWhere = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(sWhereId, sWhere);
                            listAscmMaterialItem = AscmMaterialItemService.GetInstance().GetList(ynPage, "docNumber", "", "", "", "", sWhere);
                            if (listAscmMaterialItem != null && listAscmMaterialItem.Count() > 0)
                            {
                                foreach (AscmMaterialItem ascmMaterialItem in listAscmMaterialItem)
                                {
                                    jsonDataGridResult.rows.Add(ascmMaterialItem);
                                }
                            }
                        }
                    }
                }
                jsonDataGridResult.total = ynPage.GetRecordCount();
            }
            catch (Exception ex)
            {
                //jsonDataGridResult.message = ex.Message;
                throw ex;
            }
            return Json(jsonDataGridResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SupplierMaterialImport(HttpPostedFileBase fileImport, int? supplierId)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            string sError = "";
            try
            {
                if (!supplierId.HasValue)
                    throw new Exception("供应商ID传值错误！");

                if (fileImport != null)
                {
                    string userName = string.Empty;
                    if (User.Identity.IsAuthenticated)
                        userName = User.Identity.Name;

                    //定义索引
                    int documentIndex = 1;
                    using (Stream stream = fileImport.InputStream)
                    {
                        List<AscmMaterialItem> listAscmMaterialItem = null;
                        List<AscmSupplierMaterialLink> listAscmSupplierMaterialLink = new List<AscmSupplierMaterialLink>();
                        List<AscmSupplierMaterialLink> listLink = AscmSupplierMaterialLinkService.GetInstance().GetList(supplierId.Value);
                        int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmSupplierMaterialLink");

                        IWorkbook wb = WorkbookFactory.Create(stream);
                        ISheet sheet = wb.GetSheet("Sheet1");
                        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                        string docNumbers = string.Empty;//对物料进行匹配，不需单引号
                        string docNumbersIn = string.Empty;//查询物料，需要对每个字符串加单引号
                        while (rows.MoveNext())
                        {
                            sError = "【未成功更新数据】<br>";
                            IRow row = (IRow)rows.Current;
                            if (row.RowNum != 0)
                            {
                                ICell buildingCell = row.GetCell(documentIndex, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                if (buildingCell != null)
                                {
                                    if (!string.IsNullOrEmpty(docNumbers))
                                        docNumbers += ",";
                                    docNumbers += buildingCell.ToString().Trim();
                                    if (!string.IsNullOrEmpty(docNumbersIn))
                                        docNumbersIn += ",";
                                    docNumbersIn += "'" + buildingCell.ToString().Trim() + "'";
                                }
                                else
                                {
                                    sError += "&nbsp;&nbsp;[" + row.RowNum + 1 + "]行" + "无法识别；<br>";
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(docNumbersIn))
                        {
                            string sql = " from AscmMaterialItem where ";
                            string sSql = string.Empty;
                            var sDocNumbersIn = docNumbersIn.Split(',').Distinct();
                            var iCount = sDocNumbersIn.Count();
                            if (iCount > 900)
                            {
                                string ids = string.Empty;
                                for (int i = 0; i < iCount; i++)
                                {
                                    if (!string.IsNullOrEmpty(ids))
                                        ids += ",";
                                    ids += sDocNumbersIn.ElementAt(i);
                                    if ((i + 1) % 900 == 0 || (i + 1) == iCount)
                                    {
                                        if (!string.IsNullOrEmpty(sSql))
                                            sSql += " or ";
                                        sSql += "docNumber in (" + ids + ")";
                                        ids = string.Empty;
                                    }
                                }
                            }
                            else
                                sSql = "docNumber in (" + docNumbersIn + ")";
                            sql = sql + sSql;
                            listAscmMaterialItem = AscmMaterialItemService.GetInstance().GetList(sql, true);
                        }
                        if (listAscmMaterialItem != null && listAscmMaterialItem.Count() > 0)
                        {
                            var sDocNumbers = docNumbers.Split(',').Distinct();
                            AscmMaterialItem ascmMaterialItem = null;
                            int iRow = 0;
                            foreach (string docNumber in sDocNumbers)
                            {
                                iRow++;
                                ascmMaterialItem = listAscmMaterialItem.Find(item => item.docNumber == docNumber);
                                if (ascmMaterialItem != null)
                                {
                                    AscmSupplierMaterialLink ascmSupplierMaterialLink = null;
                                    if (listLink != null && listLink.Count() > 0)
                                        ascmSupplierMaterialLink = listLink.Find(item => item.materialId == ascmMaterialItem.id);
                                    if (ascmSupplierMaterialLink == null)
                                    {
                                        AscmSupplierMaterialLink ascmLink = new AscmSupplierMaterialLink();
                                        ascmLink.id = ++maxId;
                                        ascmLink.materialId = ascmMaterialItem.id;
                                        ascmLink.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        ascmLink.modifyUser = userName;
                                        ascmLink.supplierId = supplierId.Value;
                                        listAscmSupplierMaterialLink.Add(ascmLink);
                                    }
                                }
                                else
                                {
                                    sError += "&nbsp;&nbsp;[" + docNumber + "]" + "未找到匹配物料；<br>";
                                }
                            }
                        }
                        else
                        {
                            sError += "&nbsp;&nbsp;" + "未找到匹配物料；<br>";
                        }
                        if (listAscmSupplierMaterialLink != null && listAscmSupplierMaterialLink.Count() > 0)
                        {
                            AscmSupplierMaterialLinkService.GetInstance().Save(listAscmSupplierMaterialLink);
                            sError += "【成功更新" + listAscmSupplierMaterialLink.Count() + "条】";
                        }
                        jsonObjectResult.message = sError;
                        jsonObjectResult.result = true;
                    }
                    #region
                    //List<AscmSupplierMaterialLink> listAscmSupplierMaterialLink = null;
                    //List<AscmSupplierMaterialLink> listLink=AscmSupplierMaterialLinkService.GetInstance().GetList(supplierId.Value);
                    //List<AscmMaterialItem> listAscmMaterialItem=AscmMaterialItemService.GetInstance().GetList(" from AscmMaterialItem");
                    //int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmSupplierMaterialLink");
                    //using (Stream stream = fileImport.InputStream)
                    //{
                    //    listAscmSupplierMaterialLink = new List<AscmSupplierMaterialLink>();

                    //    IWorkbook wb = WorkbookFactory.Create(stream);
                    //    ISheet sheet = wb.GetSheet("Sheet1");
                    //    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                    //    while (rows.MoveNext())
                    //    {
                    //        sError = "【未成功更新数据】<br>";
                    //        IRow row = (IRow)rows.Current;
                    //        if (row.RowNum != 0)
                    //        {
                    //            ICell buildingCell = row.GetCell(documentIndex, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    //            string docNumber=string.Empty;
                    //            if (buildingCell != null)
                    //            {
                    //                docNumber = buildingCell.ToString().Trim();
                    //                AscmMaterialItem ascmMaterialItem = listAscmMaterialItem.Find(item => item.docNumber == docNumber);
                    //                if (ascmMaterialItem != null)
                    //                {
                    //                    AscmSupplierMaterialLink ascmSupplierMaterialLink = null;
                    //                    if(listLink!=null&&listLink.Count()>0)
                    //                        ascmSupplierMaterialLink = listLink.Find(item => item.materialId == ascmMaterialItem.id);
                    //                    if (ascmSupplierMaterialLink == null)
                    //                    {
                    //                        AscmSupplierMaterialLink ascmLink = new AscmSupplierMaterialLink();
                    //                        ascmLink.id = ++maxId;
                    //                        ascmLink.materialId = ascmMaterialItem.id;
                    //                        ascmLink.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //                        ascmLink.modifyUser = userName;
                    //                        ascmLink.supplierId = supplierId.Value;
                    //                        listAscmSupplierMaterialLink.Add(ascmLink);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    sError += "&nbsp;&nbsp;[" + row.RowNum + 1 + "]行" + "未匹配到相应物料；<br>";
                    //                }
                    //            }
                    //            else
                    //            {
                    //                sError += "&nbsp;&nbsp;[" + row.RowNum + 1 + "]行" + "未识别到物料编码；<br>";
                    //            }
                    //        }
                    //    }
                    //    if (listAscmSupplierMaterialLink != null && listAscmSupplierMaterialLink.Count() > 0)
                    //    {
                    //        AscmSupplierMaterialLinkService.GetInstance().Save(listAscmSupplierMaterialLink);
                    //        sError += "【成功更新" + listAscmSupplierMaterialLink.Count() + "条】";
                    //        jsonObjectResult.message = sError;
                    //        jsonObjectResult.result = true;
                    //    }
                    //}
                    #endregion
                }
            }
            catch (Exception ex)
            {
                jsonObjectResult.message += ex.Message;
            }
            string sReturn = JsonConvert.SerializeObject(jsonObjectResult);
            return Content(sReturn);
        }
        public ActionResult SupplierMaterialDelete(string ids, int? supplierId)
        {
            JsonObjectResult jsonObjectResult = new JsonObjectResult();
            try
            {
                if (!supplierId.HasValue)
                    throw new Exception("供应商ID传值错误！");

                if (!string.IsNullOrEmpty(ids))
                {
                    string whereOther = string.Empty;
                    whereOther = " supplierId=" + supplierId.Value + " and materialId in (" + ids + ")";
                    List<AscmSupplierMaterialLink> list = AscmSupplierMaterialLinkService.GetInstance().GetList("", "", "", whereOther);
                    if (list != null && list.Count() > 0)
                    {
                        AscmSupplierMaterialLinkService.GetInstance().Delete(list);
                        jsonObjectResult.result = true;
                        jsonObjectResult.message = "";
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
        public ActionResult SupplierMaterialIdsList(string ids)
        {
            JsonDataGridResult jsonDataGridResult = new JsonDataGridResult();
            try
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    List<AscmMaterialItem> list = AscmMaterialItemService.GetInstance().GetList("from AscmMaterialItem where id in(" + ids + ")");
                    if (list != null)
                    {
                        foreach (AscmMaterialItem ascmMaterialItem in list)
                        {
                            jsonDataGridResult.rows.Add(ascmMaterialItem);
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
    }
}
