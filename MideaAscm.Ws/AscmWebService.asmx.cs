﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Xml.Serialization;
using System.Web.Security;
using MideaAscm.Services.Base;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using NHibernate;
using MideaAscm.Services.Vehicle;
using MideaAscm.Dal.Vehicle.Entities;
using MideaAscm.Services.SupplierPreparation;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Services.FromErp;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Services.Warehouse;
using MideaAscm.Dal.Base;
using YnFrame.Services;
using System.Collections;
using MideaAscm.Dal.GetMaterialManage.Entities;
using Newtonsoft.Json;
using MideaAscm.Services.GetMaterialManage;
using YnFrame.Dal.Entities;
using MideaAscm.Services.IEntity;
using MideaAscm.Dal.IEntity;
using System.Text;

namespace MideaAscm.WebService
{
    /// <summary>
    /// AscmWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class AscmWebService : YnWS.YnWebService
    {
        #region YnFrame_Base
        [WebMethod()]
        public string GetDoorList(string ticket, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmDoor> listAscmDoor = AscmDoorService.GetInstance().GetList();
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmDoor);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public bool UserAuthentication(string userId, string userPwd, string hostIP, ref string message, ref string encryptTicket)
        {
            try
            {
                message = "";
                AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().TryGet(userId);
                /*
                YnFrame.Dal.Entities.YnUser ynUser = YnFrame.Services.YnUserService.GetInstance().Get(userId);
                if (ynUser == null)
                {
                    //throw new Exception("用户不存在");
                    //判断是否erp用户
                    ynUser = YnFrame.Services.YnUserService.GetInstance().Get("erp_" + userId);
                    if (ynUser == null)
                    {
                        //判断是否mes用户
                    }
                }

                if (ynUser == null)
                {
                    throw new Exception("用户不存在");
                }*/
                if (!string.IsNullOrEmpty(ascmUserInfo.extExpandType))
                {
                    if (ascmUserInfo.extExpandType.Trim() == "erp")
                    {
                        if (!YnFrame.Services.YnUserService.GetInstance().ValidateUser(ascmUserInfo.userId, userPwd))
                        {
                            //ModelState.AddModelError("", "提供的用户名或密码不正确。");
                            throw new Exception("提供的用户名或密码不正确。");
                        }
                    }
                    else if (ascmUserInfo.extExpandType.Trim() == "mes")
                    {
                        //cn.com.midea.mespda.TransferService service = new cn.com.midea.mespda.TransferService();
                        //cn.com.midea.mespda.OutputWebMessage message1 = service.UserLogin(ascmUserInfo.userId, userPwd);
                        //if (!message1.IsSuccess)
                        //{
                        //    throw new Exception(message1.ErrorMessage);
                        //}
                    }
                }
                else
                {
                    if (!YnFrame.Services.YnUserService.GetInstance().ValidateUser(ascmUserInfo.userId, userPwd))
                    {
                        //ModelState.AddModelError("", "提供的用户名或密码不正确。");
                        throw new Exception("提供的用户名或密码不正确。");
                    }
                }


                //ynUser = YnFrame.Services.YnUserService.GetInstance().Get(userId);

                string sUserData = Newtonsoft.Json.JsonConvert.SerializeObject(ascmUserInfo.GetTicket());
                bool createPersistentCookie = false;
                DateTime dt = createPersistentCookie ? DateTime.Now.AddMinutes(99999) : DateTime.Now.AddDays(365);//用一年看他还过不过期
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                                                                    1, // 票据版本号
                                                                                    userId.ToString(), // 票据持有者
                                                                                    DateTime.Now, //分配票据的时间
                                                                                    dt, // 失效时间
                                                                                    createPersistentCookie, // 需要用户的 cookie 
                                                                                    sUserData, // 用户数据，这里其实就是用户的角色
                                                                                    FormsAuthentication.FormsCookiePath);//cookie有效路径

                ////System.Web.Security.FormsAuthenticationTicket ticket = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicket(userId, 0, ynUser, false);
                //string hash = System.Web.Security.FormsAuthentication.Encrypt(ticket);
                //HttpCookie cookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, hash); //加密之后的cookie
                //if (ticket.IsPersistent)
                //{
                //    cookie.Expires = ticket.Expiration;
                //}
                //encryptTicket = hash;
                ////添加cookie到页面请求响应中
                //HttpContext.Current.Response.Cookies.Add(cookie);
                encryptTicket = System.Web.Security.FormsAuthentication.Encrypt(ticket);

                //写日志
                ascmUserInfo.lastLoginIp = hostIP;
                ascmUserInfo.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                AscmUserInfoService.GetInstance().Update(ascmUserInfo);

                return true;

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public string TryGetAscmUser(string ticket, string userId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().TryGet(userId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmUserInfo);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public long GetCount(string ticket, string sql, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                long iCount = YnDaoHelper.GetInstance().nHibernateHelper.GetCount(sql);
                return iCount;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return -1;
        }
        [WebMethod()]
        public long GetMaxId(string ticket, string sql, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                long maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetCount(sql);
                return maxId;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return -1;
        }
        [WebMethod()]
        public object GetObject(string ticket, string sql, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(sql);
                return object1;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return -1;
        }
        #endregion

        #region 员工
        [WebMethod()]
        public string GetEmployeeList(string ticket, ref string _ynPage, string queryWord, string whereOther, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");


                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);

                List<AscmEmployee> listAscmEmployee = AscmEmployeeService.GetInstance().GetList(ynPage, null, null, queryWord, whereOther);
                _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmEmployee);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public bool SaveEmployee(string ticket, string _ascmEmployee, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                AscmEmployee ascmEmployee = (AscmEmployee)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(AscmEmployee), _ascmEmployee);
                AscmEmployeeService.GetInstance().Save(ascmEmployee);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public bool UpdateEmployee(string ticket, string _ascmEmployee, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                AscmEmployee ascmEmployee = (AscmEmployee)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(AscmEmployee), _ascmEmployee);
                AscmEmployeeService.GetInstance().Update(ascmEmployee);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public void DeleteEmployee(string ticket, int rmployeeId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                AscmEmployeeService.GetInstance().Delete(rmployeeId);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        #endregion

        #region 员工车辆
        [WebMethod()]
        public string GetEmployeeCarByRfid(string ticket, string rfid, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                AscmEmployeeCar ascmEmployeeCar = AscmEmployeeCarService.GetInstance().GetByRfid(rfid);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmEmployeeCar);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetEmployeeCarList(string ticket, ref string _ynPage, string queryWord, string whereOther, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");


                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);

                List<AscmEmployeeCar> listAscmEmployeeCar = AscmEmployeeCarService.GetInstance().GetList(ynPage, null, null, queryWord, whereOther);
                _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmEmployeeCar);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public bool SaveEmployeeCar(string ticket, bool _new, string _ascmEmployeeCar, string _ascmRfid_Old, string _ascmRfid_New_Update, string _ascmRfid_New_Save, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                AscmEmployeeCar ascmEmployeeCar = (AscmEmployeeCar)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(AscmEmployeeCar), _ascmEmployeeCar);
                AscmRfid ascmRfid_Old = (AscmRfid)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(AscmRfid), _ascmRfid_Old);
                AscmRfid ascmRfid_New_Update = (AscmRfid)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(AscmRfid), _ascmRfid_New_Update);
                AscmRfid ascmRfid_New_Save = (AscmRfid)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(AscmRfid), _ascmRfid_New_Save);
                AscmEmployeeCarService.GetInstance().Save(_new, ascmEmployeeCar, ascmRfid_Old, ascmRfid_New_Update, ascmRfid_New_Save);
                /*
                DateTime dtServerTime = MideaAscm.Dal.YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentDate("AscmEmployeeCar");
                ascmEmployeeCar.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                if (ascmRfid_Old!=null)
                    ascmRfid_Old.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                if (ascmRfid_New_Update != null)
                    ascmRfid_New_Update.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                if (ascmRfid_New_Save != null)
                    ascmRfid_New_Save.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        if (_new)
                        {
                            ascmEmployeeCar.createTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmEmployeeCar);
                        }
                        else
                        {
                            YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmEmployeeCar);
                        }
                        if (ascmRfid_Old != null)
                            YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmRfid_Old);
                        if (ascmRfid_New_Update != null)
                            YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmRfid_New_Update);
                        if (ascmRfid_New_Save != null)
                        {
                            ascmRfid_New_Save.createTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmRfid_New_Save);
                        }
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }*/

                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public void DeleteEmployeeCar(string ticket, int rmployeeCarId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                AscmEmployeeCarService.GetInstance().Delete(rmployeeCarId);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public string GetAllEmployeeCarList(string ticket, ref string message)
        {
            string result = string.Empty;
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmEmployeeCar> listAscmEmployeeCar = AscmEmployeeCarService.GetInstance().GetList(null, null, null, "", "");
                if (listAscmEmployeeCar == null)
                    throw new Exception("获取员工车辆信息失败");

                result = YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmEmployeeCar);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }
        #endregion

        #region 员工车辆日志
        [WebMethod()]
        public string GetEmpCarSwipeLogList(string ticket, ref string _ynPage, string sql, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");


                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);

                List<AscmEmpCarSwipeLog> listAscmEmpCarSwipeLog = AscmEmpCarSwipeLogService.GetInstance().GetList(sql);
                _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmEmpCarSwipeLog);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public void AscmEmpCarSwipeLogAdd(string ticket, int doorId, string readerName, string rfid_Id, string employeeName, string plateNumber, bool pass, string description, DateTime startTime, string direction, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                AscmEmpCarSwipeLogService.GetInstance().AddLog(doorId, readerName, rfid_Id, employeeName, plateNumber, pass, description, startTime, direction);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public void SaveAscmEmpCarSwipeLogList(string ticket, string empCarSwipeLogList, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");
                if (string.IsNullOrEmpty(empCarSwipeLogList))
                    throw new Exception("日志列表不能为NULL或空");

                List<AscmEmpCarSwipeLog> listAscmEmpCarSwipeLog =
                    (List<AscmEmpCarSwipeLog>)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(List<AscmEmpCarSwipeLog>), empCarSwipeLogList);

                if (listAscmEmpCarSwipeLog == null)
                    throw new Exception("参数日志列表格式错误，序列化失败");

                AscmEmpCarSwipeLogService.GetInstance().Save(listAscmEmpCarSwipeLog);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        #endregion

        #region RFID
        [WebMethod()]
        public string GetRfid(string ticket, string rfid, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");
                AscmRfid ascmRfid = AscmRfidService.GetInstance().Get(rfid);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmRfid);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        #endregion

        #region 读头
        [WebMethod()]
        public string GetAllReadingHeadList(string ticket, ref string message)
        {
            string result = string.Empty;
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmReadingHead> listAscmReadingHead = AscmReadingHeadService.GetInstance().GetList(null, null, null, "", "");
                if (listAscmReadingHead == null)
                    throw new Exception("获取读头信息失败");

                result = YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmReadingHead);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }
        #endregion

        #region 读头日志
        [WebMethod()]
        public void AscmReadingHeadLogAdd(string ticket, int readingHeadId, string rfid_Id, string sn, DateTime startTime, bool processed, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmReadingHeadLogService.GetInstance().AddLog(readingHeadId, rfid_Id, sn, startTime, processed);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        #endregion

        #region 供应商
        [WebMethod()]
        public string GetSupplierByUserId(string ticket, int erpUserId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                string sql = "from AscmSupplier where id in (select numberValue from AscmAkWebUserSecAttrValues where attributeCode='" + MideaAscm.Dal.FromErp.Entities.AscmAkWebUserSecAttrValues.AttributeCodeDefine.supplierUser + "' and webUserId=" + erpUserId + ")";
                //sql = "from AscmEmployee where id in (" + sql + ")";
                IList<AscmSupplier> ilistAscmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql);
                if (ilistAscmSupplier != null && ilistAscmSupplier.Count > 0)
                {
                    //ascmUserInfo.ascmSupplier = ilistAscmSupplier[0];
                    return YnBaseClass2.Helper.ObjectHelper.Serialize(ilistAscmSupplier[0]);
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetSupplierCurrentDeliveryOrderBatchList(string ticket, int supplierId, int deliveryBatchSumMainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmDeliveryOrderBatch> listAscmDeliveryOrderBatch = AscmDeliveryOrderBatchService.GetInstance().GetSupplierCurrentList(supplierId, deliveryBatchSumMainId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmDeliveryOrderBatch);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetDeliveryOrderBatchDetailList(string ticket, int batchId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmDeliveryOrderDetail> listAscmDeliveryOrderDetail = AscmDeliveryOrderDetailService.GetInstance().GetListByBatch(batchId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmDeliveryOrderDetail);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetDeliveryOrderBatchMaterialDetailList(string ticket, int batchId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmMaterialItem> listAscmMaterialItem = AscmDeliveryOrderDetailService.GetInstance().GetMaterialListByBatch(batchId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmMaterialItem);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetDeliveryOrderBatchSumMainMaterialDetailList(string ticket, int sumMainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmMaterialItem> listAscmMaterialItem = AscmDeliBatSumMainService.GetInstance().GetMaterialListBySumMain(sumMainId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmMaterialItem);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        #endregion

        #region 送货车辆日志
        [WebMethod()]
        public void SupplierDriverToPlant(string ticket, int doorId, string readingHead, int driverId, bool inPlant, string direction, bool onTime, ref string allocateOutDoor, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmDriverService.GetInstance().InOutPlant(doorId, readingHead, driverId, inPlant, direction, onTime, ref allocateOutDoor);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public string GetSwipeTrafficStatistics(string ticket, string rq, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                DateTime dt1 = DateTime.Now;
                DateTime.TryParse(rq, out dt1);
                List<AscmDoor> listAscmDoor = AscmDoorService.GetInstance().GetList();
                foreach (AscmDoor ascmDoor in listAscmDoor)
                {
                    //string sql = "select count(*) from AscmEmpCarSwipeLog where doorId=" + ascmDoor.id + " and direction like '小车入%' and createTime>='" + dt1.ToString("yyyy-MM-dd 00:00:00") + "' and createTime<'" + dt1.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                    //ascmDoor.carEnterCount = YnDaoHelper.GetInstance().nHibernateHelper.GetCount(sql);
                    ascmDoor.carEnterCount = GetSwipeTrafficStatisticsCar(ascmDoor, dt1, "小车入");
                    //string sql = "select count(*) from AscmEmpCarSwipeLog where doorId=" + ascmDoor.id + " and direction like '小车出%' and createTime>='" + dt1.ToString("yyyy-MM-dd 00:00:00") + "' and createTime<'" + dt1.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                    //ascmDoor.carOutCount = YnDaoHelper.GetInstance().nHibernateHelper.GetCount(sql);
                    ascmDoor.carOutCount = GetSwipeTrafficStatisticsCar(ascmDoor, dt1, "小车出");
                    //sql = "select count(*) from AscmTruckSwipeLog where doorId=" + ascmDoor.id + " and direction='货车入' and createTime>='" + dt1.ToString("yyyy-MM-dd 00:00:00") + "' and createTime<'" + dt1.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                    //ascmDoor.truckEnterCount = YnDaoHelper.GetInstance().nHibernateHelper.GetCount(sql);
                    ascmDoor.truckEnterCount = GetSwipeTrafficStatisticsTruck(ascmDoor, dt1, "货车入");
                    //sql = "select count(*) from AscmTruckSwipeLog where doorId=" + ascmDoor.id + " and direction='货车出' and createTime>='" + dt1.ToString("yyyy-MM-dd 00:00:00") + "' and createTime<'" + dt1.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                    //ascmDoor.truckOutCount = YnDaoHelper.GetInstance().nHibernateHelper.GetCount(sql);
                    ascmDoor.truckOutCount = GetSwipeTrafficStatisticsTruck(ascmDoor, dt1, "货车出");
                }

                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmDoor);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        private int GetSwipeTrafficStatisticsCar(AscmDoor ascmDoor, DateTime dt1, string direction)
        {
            try
            {
                string sql = "from AscmEmpCarSwipeLog where doorId=" + ascmDoor.id + " and direction like '" + direction + "%' and createTime>='" + dt1.ToString("yyyy-MM-dd 00:00:00") + "' and createTime<'" + dt1.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                List<AscmEmpCarSwipeLog> listAscmEmpCarSwipeLog = AscmEmpCarSwipeLogService.GetInstance().GetList(sql + " order by id");
                List<AscmEmpCarSwipeLog> listAscmEmpCarSwipeLog_tmp = new List<AscmEmpCarSwipeLog>();
                //for (int irow = listAscmEmpCarSwipeLog.Count - 1; irow >= 0; irow--)
                foreach (AscmEmpCarSwipeLog ascmEmpCarSwipeLog in listAscmEmpCarSwipeLog)
                {
                    AscmEmpCarSwipeLog ascmEmpCarSwipeLog_tmp = listAscmEmpCarSwipeLog_tmp.FindLast(item => item.rfid == ascmEmpCarSwipeLog.rfid);
                    int count = listAscmEmpCarSwipeLog_tmp.Count(item => item.rfid == ascmEmpCarSwipeLog.rfid);
                    if (count > 5)
                        continue;
                    if (ascmEmpCarSwipeLog_tmp == null)
                    {
                        listAscmEmpCarSwipeLog_tmp.Add(ascmEmpCarSwipeLog);
                    }
                    else
                    {
                        DateTime dtCreateTime = DateTime.Now;
                        DateTime dtCreateTime_tmp = DateTime.Now;
                        if (DateTime.TryParse(ascmEmpCarSwipeLog.createTime, out dtCreateTime) && DateTime.TryParse(ascmEmpCarSwipeLog_tmp.createTime, out dtCreateTime_tmp))
                        {
                            TimeSpan ts = dtCreateTime.Subtract(dtCreateTime_tmp);
                            if (ts.TotalMinutes > 5)
                            {
                                //10分钟内重复读过滤
                                listAscmEmpCarSwipeLog_tmp.Add(ascmEmpCarSwipeLog);
                            }
                            else
                            {
                                ascmEmpCarSwipeLog_tmp.createTime = ascmEmpCarSwipeLog.createTime;
                            }
                        }
                    }
                }
                //ascmDoor.carEnterCount = listAscmEmpCarSwipeLog_tmp.Count;
                return listAscmEmpCarSwipeLog_tmp.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int GetSwipeTrafficStatisticsTruck(AscmDoor ascmDoor, DateTime dt1, string direction)
        {
            try
            {
                string sql = "from AscmTruckSwipeLog where doorId=" + ascmDoor.id + " and direction like '" + direction + "%' and createTime>='" + dt1.ToString("yyyy-MM-dd 00:00:00") + "' and createTime<'" + dt1.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                List<AscmTruckSwipeLog> listAscmTruckSwipeLog = AscmTruckSwipeLogService.GetInstance().GetList(sql + " order by id");
                List<AscmTruckSwipeLog> listAscmTruckSwipeLog_tmp = new List<AscmTruckSwipeLog>();
                //for (int irow = listAscmEmpCarSwipeLog.Count - 1; irow >= 0; irow--)
                foreach (AscmTruckSwipeLog ascmTruckSwipeLog in listAscmTruckSwipeLog)
                {
                    AscmTruckSwipeLog ascmTruckSwipeLog_tmp = listAscmTruckSwipeLog_tmp.FindLast(item => item.rfid == ascmTruckSwipeLog.rfid);
                    int count = listAscmTruckSwipeLog_tmp.Count(item => item.rfid == ascmTruckSwipeLog.rfid);
                    if (count > 5)
                        continue;
                    if (ascmTruckSwipeLog_tmp == null)
                    {
                        listAscmTruckSwipeLog_tmp.Add(ascmTruckSwipeLog);
                    }
                    else
                    {
                        DateTime dtCreateTime = DateTime.Now;
                        DateTime dtCreateTime_tmp = DateTime.Now;
                        if (DateTime.TryParse(ascmTruckSwipeLog.createTime, out dtCreateTime) && DateTime.TryParse(ascmTruckSwipeLog_tmp.createTime, out dtCreateTime_tmp))
                        {
                            TimeSpan ts = dtCreateTime.Subtract(dtCreateTime_tmp);
                            if (ts.TotalMinutes > 5)
                            {
                                //10分钟内重复读过滤
                                listAscmTruckSwipeLog_tmp.Add(ascmTruckSwipeLog);
                            }
                            else
                            {
                                ascmTruckSwipeLog_tmp.createTime = ascmTruckSwipeLog.createTime;
                            }
                        }
                    }
                }
                return listAscmTruckSwipeLog_tmp.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 送货合单
        [WebMethod()]
        public string GetTodayDeliveryBatchSumMainList(string ticket, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                string sql = "from AscmDeliBatSumMain where (status='" + AscmDeliBatSumMain.StatusDefine.confirm + "' or status='" + AscmDeliBatSumMain.StatusDefine.inPlant + "') ";
                //sql += " and appointmentStartTime>='" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' and appointmentEndTime<='" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                string whereAppointmentTime = " appointmentStartTime>='" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' and appointmentStartTime<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                whereAppointmentTime += " or appointmentEndTime>='" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' and appointmentEndTime<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                whereAppointmentTime += " or appointmentStartTime<'" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' and appointmentEndTime>='" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                sql += " and (" + whereAppointmentTime + ")";
                List<AscmDeliBatSumMain> listAscmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().GetList(sql);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmDeliBatSumMain);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetSupplierCurrentDeliveryBatchSumMainList(string ticket, int supplierId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmDeliBatSumMain> listAscmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().GetSupplierCurrentList(supplierId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmDeliBatSumMain);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetSupplierCurrentDeliveryBatchSumDetailList(string ticket, int mainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmDeliBatSumDetail> listAscmDeliBatSumDetail = AscmDeliBatSumDetailService.GetInstance().GetListByMainId(mainId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmDeliBatSumDetail);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetSupplierDriverDeliveryBatchSumMain(string ticket, int driverId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");
                AscmDeliBatSumMain ascmDeliBatSumMain = null;
                List<AscmDeliBatSumMain> list = AscmDeliBatSumMainService.GetInstance().GetByDriverId(driverId);
                if (list != null && list.Count > 0)
                {
                    ascmDeliBatSumMain = list[0];
                }
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmDeliBatSumMain);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetSupplierCurrentDeliveryBatchSumAllDetail(string ticket, int mainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmDeliBatSumAllDetail ascmDeliBatSumAllDetail = AscmDeliBatSumMainService.GetInstance().GetAscmDeliBatSumAllDetail(mainId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmDeliBatSumAllDetail);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public void SaveSupplierCurrentDeliveryBatchSumAllDetail(string ticket, string ascmDeliBatSumAllDetail, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");
                if (string.IsNullOrEmpty(ascmDeliBatSumAllDetail))
                    throw new Exception("合单绑定信息不能为NULL或空");

                AscmDeliBatSumAllDetail _ascmDeliBatSumAllDetail =
                    (AscmDeliBatSumAllDetail)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(AscmDeliBatSumAllDetail), ascmDeliBatSumAllDetail);

                if (_ascmDeliBatSumAllDetail == null)
                    throw new Exception("合单绑定信息格式错误，序列化失败");

                AscmDeliBatSumMainService.GetInstance().SaveAscmDeliBatSumAllDetail(_ascmDeliBatSumAllDetail);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        #endregion

        #region 容器备料
        [WebMethod()]
        public string GetContainer(string ticket, string containerSn, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");
                AscmContainer ascmContainer = AscmContainerService.GetInstance().Get(containerSn);
                if (ascmContainer == null)
                    ascmContainer = new AscmContainer();
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmContainer);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetContainerDeliveryList(string ticket, string containerSn, int batSumMainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmContainerDelivery> listAscmContainerDelivery = AscmContainerDeliveryService.GetInstance().GetList(containerSn, batSumMainId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmContainerDelivery);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetContainerDeliveryListByDeliverySumMainId(string ticket, int mainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmContainerDelivery> listAscmContainerDelivery = AscmContainerDeliveryService.GetInstance().GetListByDeliverySumMainId(mainId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmContainerDelivery);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string ContainerDeliveryAdd(string ticket, string containerSn, int batSumMainId, int deliveryOrderBatchId, int materialId, decimal quantity, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmContainerDelivery ascmContainerDelivery = AscmContainerDeliveryService.GetInstance().Add(containerSn, batSumMainId, deliveryOrderBatchId, materialId, quantity);

                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmContainerDelivery);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public void ContainerDeliveryUpdate(string ticket, int id, decimal quantity, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmContainerDeliveryService.GetInstance().Update(id, quantity);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public void ContainerDeliveryDelete(string ticket, int id, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmContainerDeliveryService.GetInstance().Delete(id);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public void ContainerDeliveryClear(string ticket, string containerSn, int batSumMainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmContainerDeliveryService.GetInstance().Clear(containerSn, batSumMainId);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        #endregion

        #region 托盘备料
        [WebMethod()]
        public string GetPallet(string ticket, string palletSn, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmPallet ascmPallet = AscmPalletService.GetInstance().Get(palletSn);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmPallet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetPalletDeliveryList(string ticket, string palletSn, int batSumMainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmPalletDelivery> listAscmPalletDelivery = AscmPalletDeliveryService.GetInstance().GetList(palletSn, batSumMainId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmPalletDelivery);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetPalletDeliveryListByDeliverySumMainId(string ticket, int mainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmPalletDelivery> listAscmPalletDelivery = AscmPalletDeliveryService.GetInstance().GetListByDeliverySumMainId(mainId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmPalletDelivery);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string PalletDeliveryAdd(string ticket, string palletSn, int batSumMainId, int deliveryOrderBatchId, int materialId, decimal quantity, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmPalletDelivery ascmPalletDelivery = AscmPalletDeliveryService.GetInstance().Add(palletSn, batSumMainId, deliveryOrderBatchId, materialId, quantity);

                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmPalletDelivery);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public void PalletDeliveryUpdate(string ticket, int id, decimal quantity, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmPalletDeliveryService.GetInstance().Update(id, quantity);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public void PalletDeliveryDelete(string ticket, int id, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmPalletDeliveryService.GetInstance().Delete(id);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public void PalletDeliveryClear(string ticket, string palletSn, int batSumMainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmPalletDeliveryService.GetInstance().Clear(palletSn, batSumMainId);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        #endregion

        #region 司机备料
        [WebMethod()]
        public string GetDriverByDriverSn(string ticket, string driverSn, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmDriver ascmDriver = AscmDriverService.GetInstance().GetByDriverSn(driverSn);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmDriver);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetDriverDeliveryList(string ticket, string driverSn, int batSumMainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmDriverDelivery> listAscmDriverDelivery = AscmDriverDeliveryService.GetInstance().GetListByDriverSn(driverSn, batSumMainId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmDriverDelivery);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetDriverDeliveryListByDeliverySumMainId(string ticket, int mainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmDriverDelivery> listAscmDriverDelivery = AscmDriverDeliveryService.GetInstance().GetListByDeliverySumMainId(mainId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmDriverDelivery);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string DriverDeliveryAdd(string ticket, string driverSn, int batSumMainId, int deliveryOrderBatchId, int materialId, decimal quantity, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmDriverDelivery ascmDriverDelivery = AscmDriverDeliveryService.GetInstance().Add(driverSn, batSumMainId, deliveryOrderBatchId, materialId, quantity);

                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmDriverDelivery);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public void DriverDeliveryUpdate(string ticket, int id, decimal quantity, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmDriverDeliveryService.GetInstance().Update(id, quantity);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public void DriverDeliveryDelete(string ticket, int id, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmDriverDeliveryService.GetInstance().Delete(id);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public void DriverDeliveryClear(string ticket, string driverSn, int batSumMainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmDriverDeliveryService.GetInstance().Clear(driverSn, batSumMainId);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        #endregion

        #region 卸货点
        [WebMethod()]
        public string GetUnloadingPointList(string ticket, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmUnloadingPoint> listAscmUnloadingPoint = AscmUnloadingPointService.GetInstance().GetList("from AscmUnloadingPoint");
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmUnloadingPoint);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public void UnloadingPointUpdateStatus(string ticket, int id, string status, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmUnloadingPointService.GetInstance().UpdateStatus(id, status);

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        #endregion

        #region 卸货点控制器
        [WebMethod()]
        public string GetUnloadingPointControllerList(string ticket, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmUnloadingPointController> listAscmUnloadingPointController = AscmUnloadingPointControllerService.GetInstance().GetList("from AscmUnloadingPointController");
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmUnloadingPointController);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        #endregion

        #region 仓库备料
        [WebMethod()]
        public string TryGetAscmUser2(string userId, string userPwd, ref string message)
        {
            try
            {
                message = string.Empty;
                if (string.IsNullOrEmpty(userPwd))
                {
                    message = "密码不能为空";
                    return string.Empty;
                }
                AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().TryGet(userId);
                if (ascmUserInfo == null)
                {
                    message = "用户名不正确";
                    return string.Empty;
                }
                if ((string.IsNullOrEmpty(ascmUserInfo.extExpandType) || (ascmUserInfo.extExpandType.Trim() == "erp")) || (ascmUserInfo.extExpandType.Trim() != "mes"))
                {
                    byte[] result = Encoding.Default.GetBytes(userPwd.Trim());
                    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    userPwd = BitConverter.ToString(md5.ComputeHash(result)).Replace("-", "");
                    if (ascmUserInfo.password != userPwd)
                    {
                        message = "密码不正确";
                        return string.Empty;
                    }
                }
                return YnBaseClass2.Helper.ObjectHelper.Serialize<AscmUserInfo>(ascmUserInfo);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return string.Empty;
        }
        
        [WebMethod()]
        public string MobileLogin(string userId, string userPwd, string pdaIdentity, ref string message)
        {
            string result = string.Empty;
            try
            {
                message = string.Empty;
                //if (MideaAscm.Security.Utility.GetInstance().IsPdaAuthorized(pdaIdentity))
                //{
                    //string connString = System.Configuration.ConfigurationManager.ConnectionStrings["OraConnString"].ConnectionString;
                    //string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.16.17.77)(PORT=1601))(CONNECT_DATA=(SERVICE_NAME=md_ascm)));User Id=ascm;Password=Ascm32@;";
                    string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.16.9.191)(PORT=1522))(CONNECT_DATA=(SERVICE_NAME=md_ascm)));User Id=ascm;Password=AScm1240#;";
                    AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().MobileLogin(userId, userPwd, connString, ref message);
                    if (string.IsNullOrEmpty(message))
                    {
                        if (ascmUserInfo != null)
                        {
                            if (ascmUserInfo.extExpandType == "mes")
                            {
                                cn.com.midea.mespda.TransferService service = new cn.com.midea.mespda.TransferService();
                                cn.com.midea.mespda.OutputWebMessage message1 = service.UserLogin(ascmUserInfo.userId, userPwd);
                                if (!message1.IsSuccess)
                                {
                                    message = message1.ErrorMessage;
                                }
                            }
                            if (string.IsNullOrEmpty(message))
                                result = YnBaseClass2.Helper.ObjectHelper.Serialize<AscmUserInfo>(ascmUserInfo);
                        }
                        else
                            message = "用户名不正确";
                    }
                //}
                //else
                //    message = "手持认证失败";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }
        [WebMethod()]
        public string GetWipJobPreparationMainList(string ticket, string userId, ref string message)
        {
            string result = string.Empty;
            try
            {
                message = string.Empty;
                List<AscmWmsPreparationMain> listPreparationMain = AscmWmsPreparationMainService.GetInstance().GetMobileWipJobPreparationList(userId);
                if (listPreparationMain != null)
                    result = YnBaseClass2.Helper.ObjectHelper.Serialize(listPreparationMain);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }
        [WebMethod()]
        public string GetWipRequirePreparationMainList(string ticket, string userId, ref string message)
        {
            string result = string.Empty;
            try
            {
                message = string.Empty;
                List<AscmWmsPreparationMain> listPreparationMain = AscmWmsPreparationMainService.GetInstance().GetMobileWipRequirePreparationList(userId);
                if (listPreparationMain != null)
                    result = YnBaseClass2.Helper.ObjectHelper.Serialize(listPreparationMain);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetWmsPreparationDetailList(string ticket, int mainId, string containerSn, ref string message)
        {
            string result = string.Empty;
            try
            {
                message = string.Empty;
                List<AscmWmsPreparationDetail> listPreparationDetail = AscmWmsPreparationMainService.GetInstance().GetMobileWmsPreparationDetailList(mainId, containerSn);
                if (listPreparationDetail != null)
                    result = YnBaseClass2.Helper.ObjectHelper.Serialize(listPreparationDetail);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }
        [WebMethod()]
        public string GetWmsPreparationMainList(string ticket, string containerSn, ref string message)
        {
            string result = string.Empty;
            try
            {
                message = string.Empty;
                if (!string.IsNullOrEmpty(containerSn))
                {
                    List<AscmWmsPreparationMain> listPreparationMain = AscmWmsPreparationMainService.GetInstance().GetMobileWmsPreparationMainList(containerSn);
                    if (listPreparationMain != null)
                        result = YnBaseClass2.Helper.ObjectHelper.Serialize(listPreparationMain);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }
        [WebMethod()]
        public bool DoMobilePreparation(int preparationMainId, string ascmWmsContainerDelivery, ref string message)
        {
            bool result = false;
            try
            {
                message = string.Empty;
                if (preparationMainId > 0 && !string.IsNullOrEmpty(ascmWmsContainerDelivery))
                {
                    List<AscmWmsContainerDelivery> list = (List<AscmWmsContainerDelivery>)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(List<AscmWmsContainerDelivery>), ascmWmsContainerDelivery);
                    if (list != null)
                        result = AscmWmsPreparationMainService.GetInstance().DoMobilePreparation(preparationMainId, list, ref message);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }

        [WebMethod()]
        public string GetKeeperCurrentPreparationMainList(string ticket, string userId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmWmsPreparationMain> listAscmWmsPreparationMain = AscmWmsPreparationMainService.GetInstance().GetCurrentList(userId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmWmsPreparationMain);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetKeeperCurrentPreparationSumDetailList(string ticket, int mainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmWmsPreparationDetail> listAscmWmsPreparationDetailSum = new List<AscmWmsPreparationDetail>();
                //合计相同的物料
                List<AscmWmsPreparationDetail> listAscmWmsPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetListByMainId(mainId);
                if (listAscmWmsPreparationDetail != null && listAscmWmsPreparationDetail.Count > 0)
                {
                    int i = 0;
                    var groupByMaterial = listAscmWmsPreparationDetail.GroupBy(P => P.materialId);
                    foreach (IGrouping<int, AscmWmsPreparationDetail> ig in groupByMaterial)
                    {
                        AscmWmsPreparationDetail _ascmWmsPreparationDetail = ig.First();
                        AscmWmsPreparationDetail ascmWmsPreparationDetail = new AscmWmsPreparationDetail();
                        ascmWmsPreparationDetail.id = ++i;
                        ascmWmsPreparationDetail.mainId = _ascmWmsPreparationDetail.mainId;
                        ascmWmsPreparationDetail.materialId = ig.Key;
                        //ascmWmsPreparationDetail.ascmMaterialItem = _ascmWmsPreparationDetail.ascmMaterialItem;
                        ascmWmsPreparationDetail.warehouseId = _ascmWmsPreparationDetail.warehouseId;
                        ascmWmsPreparationDetail.wipSupplyType = _ascmWmsPreparationDetail.wipSupplyType;
                        ascmWmsPreparationDetail.planQuantity = ig.Sum(P => P.planQuantity);
                        //ascmWmsPreparationDetail.quantity = ig.Sum(P => P.quantity);
                        listAscmWmsPreparationDetailSum.Add(ascmWmsPreparationDetail);
                    }
                }

                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmWmsPreparationDetailSum);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetWmsContainerDeliveryList(string ticket, string containerSn, int preparationMainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmWmsContainerDelivery> listAscmWmsContainerDelivery = AscmWmsContainerDeliveryService.GetInstance().GetList(containerSn, preparationMainId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmWmsContainerDelivery);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetWmsContainerDeliveryListByPreparationMainId(string ticket, int mainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<AscmWmsContainerDelivery> listAscmWmsContainerDelivery = AscmWmsContainerDeliveryService.GetInstance().GetListByPreparationMainId(mainId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listAscmWmsContainerDelivery);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string WmsContainerDeliveryAdd(string ticket, string containerSn, int preparationMainId, int materialId, int warelocationId, decimal quantity, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmWmsContainerDelivery ascmWmsContainerDelivery = AscmWmsContainerDeliveryService.GetInstance().Add(containerSn, preparationMainId, materialId, warelocationId, quantity);

                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmWmsContainerDelivery);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public void WmsContainerDeliveryUpdate(string ticket, int id, decimal quantity, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmWmsContainerDeliveryService.GetInstance().Update(id, quantity);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public void WmsContainerDeliveryDelete(string ticket, int id, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmWmsContainerDeliveryService.GetInstance().Delete(id);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public void WmsContainerDeliveryClear(string ticket, string containerSn, int preparationMainId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmWmsContainerDeliveryService.GetInstance().Clear(containerSn, preparationMainId);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        #endregion

        #region 货车入厂LED显示
        [WebMethod()]
        public string GetDoorLedTitle(string ticket, ref string message)
        {
            string doorLedTitle = string.Empty;
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                doorLedTitle = YnParameterService.GetInstance().GetValue(MyParameter.doorLedTitle);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return doorLedTitle;
        }
        [WebMethod()]
        public string GetDoorLedSupplier(string ticket, ref string message)
        {
            string result = string.Empty;
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                List<string> list = AscmSupplierService.GetInstance().GetDoorLedSupplier();
                if (list == null)
                    throw new Exception("获取供应商失败");

                result = YnBaseClass2.Helper.ObjectHelper.Serialize(list);
            }
             catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }
        [WebMethod()]
        public string GetEnterSupplierDeliBatSumMain(string ticket, int driverId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmDeliBatSumMain ascmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().GetEnterByDriverId(driverId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmDeliBatSumMain);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetOutSupplierDeliBatSumMain(string ticket, int driverId, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmDeliBatSumMain ascmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().GetOutByDriverId(driverId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmDeliBatSumMain);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        #endregion

        #region 仓库LED显示
        [WebMethod()]
        public string GetWmsLedMonitorList(ref string message)
        {
            string result = string.Empty;
            try
            {
                List<MideaAscm.View.Dal.Entities.AscmWipDiscreteJobsV> list = MideaAscm.View.Services.AscmWipDiscreteJobsVService.GetInstance().GetWmsLedMonitorList();
                result = YnBaseClass2.Helper.ObjectHelper.Serialize(list);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }
        [WebMethod()]
        public string GetWipRequireLedMonitorList(ref string message)
        {
            string result = string.Empty;
            try
            {
                List<AscmWmsWipRequireLedMonitor> list = AscmWmsIncAccCheckoutService.GetInstance().GetWipRequireLedMonitorList();
                result = YnBaseClass2.Helper.ObjectHelper.Serialize(list);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }
        #endregion

        #region 仓库来料接收校验
        [WebMethod()]
        public string GetWmsLedIncAccCheckout(string warehouseId, ref string message)
        {
            string result = string.Empty;
            try
            {
                CurrentDeliBatSum currentDeliBatSum = null;
                if (string.IsNullOrEmpty(warehouseId))
                    currentDeliBatSum = AscmWmsIncAccCheckoutService.GetInstance().GetCurrentDeliBatSum();
                else
                    currentDeliBatSum = AscmWmsIncAccCheckoutService.GetInstance().GetCurrentDeliBatSum(warehouseId);
                result = YnBaseClass2.Helper.ObjectHelper.Serialize(currentDeliBatSum);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }
        #endregion

        #region 仓库发料校验
        [WebMethod()]
        public string GetWmsLedStoreIssueCheck(ref string message)
        {
            string result = string.Empty;
            try
            {
                List<AscmWmsStoreIssueCheck> list = AscmWmsMtlRequisitionMainService.GetInstance().GetWmsLedStoreIssueCheck();
                result = YnBaseClass2.Helper.ObjectHelper.Serialize(list);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }
        #endregion

        #region 仓库货位转移
        [WebMethod()]
        public string GetWarelocationByRfid(string ticket, string rfid, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                AscmWarelocation ascmWarelocation = AscmWarelocationService.GetInstance().GetWarelocationByRfid(rfid);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmWarelocation);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public int AddWmsLocationTransfer(string ticket, string ascmWmsLocationTransfer, ref string message)
        {
            int id = -1;
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");
                if (string.IsNullOrEmpty(ascmWmsLocationTransfer))
                    throw new Exception("货位转移信息不能为NULL或空");

                AscmWmsLocationTransfer _ascmWmsLocationTransfer =
                    (AscmWmsLocationTransfer)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(AscmWmsLocationTransfer), ascmWmsLocationTransfer);

                if (_ascmWmsLocationTransfer == null)
                    throw new Exception("货位转移信息格式错误，序列化失败");

                id = AscmWmsLocationTransferService.GetInstance().AddWmsLocationTransfer(_ascmWmsLocationTransfer);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return id;
        }
        [WebMethod()]
        public void UpdateWmsLocationTransfer(string ticket, string ascmWmsLocationTransfer, ref string message)
        {
            try
            {
                message = "";
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");
                if (string.IsNullOrEmpty(ascmWmsLocationTransfer))
                    throw new Exception("货位转移信息不能为NULL或空");

                AscmWmsLocationTransfer _ascmWmsLocationTransfer =
                    (AscmWmsLocationTransfer)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(AscmWmsLocationTransfer), ascmWmsLocationTransfer);

                if (_ascmWmsLocationTransfer == null)
                    throw new Exception("货位转移信息格式错误，序列化失败");

                AscmWmsLocationTransferService.GetInstance().UpdateWmsLocationTransfer(_ascmWmsLocationTransfer);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        #endregion

        #region 临时领料任务
        [WebMethod()]
        public string GetTasksList(string ticket, string queryStatus, string queryDate, ref string _ynPage, ref string message)
        {
            try
            {
                message = "";
                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                string where = "", whereTime = "", whereStatus = "";
                if (!string.IsNullOrEmpty(queryDate))
                {
                    whereTime = " (CREATETIME like '%" + queryDate + "%')";
                }
                if (!string.IsNullOrEmpty(queryStatus))
                {
                    whereStatus = " (STATUS = '" + queryStatus + "')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereTime);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereStatus);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, " (TASKID like 'L%')");
                string sql = " from AscmGetMaterialTask where " + where;
                string sSql = sql + " order by ID desc ";
                List<AscmGetMaterialTask> list = null;
                IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sSql, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                }
                _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }

        [WebMethod()]
        public bool AddTaskSave(string ticket, string _getMaterialTask, string warehouserPlace, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                AscmGetMaterialTask taskModel = (AscmGetMaterialTask)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(AscmGetMaterialTask), _getMaterialTask);

                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmGetMaterialTask where TaskId like '%L%' AND CREATETIME like '%" + date + "%'");
                taskModel.taskId = (maxId == 0) ? "L1001" : "L" + (int.Parse(AscmGetMaterialTaskService.GetInstance().Get(maxId).taskId.Substring(1, 4)) + 1).ToString();
                taskModel.id = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmGetMaterialTask ") + 1;
                taskModel.createTime = datetime;
                taskModel.modifyTime = datetime;
                taskModel.status = AscmGetMaterialTask.StatusDefine.notExecute;
                taskModel.materialType = 1;
                taskModel.uploadDate = date;
                taskModel.warehouserPlace = warehouserPlace;
                AscmGetMaterialTaskService.GetInstance().Save(taskModel);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }

        [WebMethod()]
        public bool TaskDelete(string ticket, int id, ref string message)
        {
            try
            {
                AscmGetMaterialTask ascmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().Get(id);
                if (ascmGetMaterialTask.status == "NOTALLOCATE" || ascmGetMaterialTask.status == "NOTEXECUTE")
                {
                    AscmGetMaterialTaskService.GetInstance().Delete(id);
                }
                else
                {
                    throw new Exception("只能删除未分配或已分配任务！");
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
            return true;
        }

        [WebMethod()]
        public bool EditTaskSave(string ticket, string _getMaterialTask, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                AscmGetMaterialTask taskModel = (AscmGetMaterialTask)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(AscmGetMaterialTask), _getMaterialTask);
                AscmGetMaterialTask task = AscmGetMaterialTaskService.GetInstance().Get(taskModel.id);
                task.productLine = taskModel.productLine;
                task.warehouserId = taskModel.warehouserId;
                task.mtlCategoryStatus = taskModel.mtlCategoryStatus;
                task.rankerId = taskModel.rankerId;
                task.IdentificationId = taskModel.IdentificationId;
                task.materialDocNumber = taskModel.materialDocNumber;
                task.taskTime = taskModel.taskTime;
                task.tip = taskModel.tip;
                task.workerId = taskModel.workerId;
                task.modifyUser = taskModel.modifyUser;

                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                task.modifyTime = datetime;
                if (!string.IsNullOrEmpty(task.workerId))
                    task.status = AscmGetMaterialTask.StatusDefine.notExecute;
                AscmGetMaterialTaskService.GetInstance().Update(task);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        #endregion

        #region 仓库物料查询
        [WebMethod()]
        public string GetMaterialList(string ticket, string docNumber, ref string _ynPage, ref string message)
        {
            try
            {
                message = "";
                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                string where = "", whereDocNumber = "";
                if (!string.IsNullOrEmpty(docNumber))
                {
                    whereDocNumber = " (DOCNUMBER like '%" + docNumber + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereDocNumber);
                string sql = " from AscmMaterialItem ";
                if (!string.IsNullOrEmpty(where))
                    sql = sql + " where " + where;
                List<AscmMaterialItem> list = null;
                IList<AscmMaterialItem> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilist);
                }
                _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }

        [WebMethod()]
        public string MaterialOfDiscreteJobList(string ticket, int materialId, string startTime, string endTime, ref string _ynPage, ref string message)
        {
            List<AscmWipDiscreteJobs> list = null;
            try
            {
                message = "";
                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);

                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                if (string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
                    throw new Exception("需求起止时间不能为空！");

                startTime = startTime + " 23:59:59";
                endTime = endTime + " 23:59:59";
                string sql = "select wipEntityId from AscmWipRequirementOperations";
                string where = "", whereQueryWord = "";
                if (materialId != 0)
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
                    IList<AscmWipDiscreteJobs> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sSql, sSql, ynPage);
                    if (ilist != null && ilist.Count > 0)
                    {
                        list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobs>(ilist);
                        AscmWipDiscreteJobsService.GetInstance().SetWipEntities(list);
                    }
                }
                _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        #endregion

        #region 监控视图
        [WebMethod()]
        public string GetTaskList(string ticket, string name, string status, string type,string queryDate, string taskString, string queryFormat, ref string _ynPage, ref string message)
        {
            List<AscmGetMaterialTask> list = null;
            try
            {
                message = "";
                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                string userRole = string.Empty;
                List<YnRole> listYnRole = YnRoleService.GetInstance().GetListInUser(name);
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
                    userRole = name;
                }

                string where = "", whereQueryWord = "";
                string sql = "from AscmGetMaterialTask";

                if (!string.IsNullOrEmpty(taskString))
                {
                    string[] taskArray = taskString.Split(',');
                    string ids = string.Empty;
                    foreach (string str in taskArray)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += str.Substring(str.IndexOf('[') + 1, str.IndexOf(']') - 1);
                    }

                    whereQueryWord = "id in (" + ids + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                else
                {
                    if (!string.IsNullOrEmpty(userRole))
                    {
                        whereQueryWord = "workerId = '" + userRole + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        whereQueryWord = "status = '" + status + "'";
                    }
                    else
                    {
                        whereQueryWord = "status in ('EXECUTE','NOTEXECUTE')";
                    }
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    if (!string.IsNullOrEmpty(type))
                    {
                        whereQueryWord = "IdentificationId =" + type;
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryDate))
                    {
                        whereQueryWord = "createTime like '" + queryDate + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                    else
                    {
                        string systemDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                        whereQueryWord = "createTime like '" + systemDate + "%'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(queryFormat))
                    {
                        if (queryFormat == "特殊子库")
                        {
                            whereQueryWord = "mtlCategoryStatus is null and taskId like 'T%'";
                        }
                        else if (queryFormat == "临时任务")
                        {
                            whereQueryWord = "mtlCategoryStatus is null and taskId like 'L%'";
                        }
                        else if (queryFormat == "须备料")
                        {
                            whereQueryWord = "mtlCategoryStatus = 'PRESTOCK'";
                        }
                        else if (queryFormat == "须配料")
                        {
                            whereQueryWord = "mtlCategoryStatus = 'MIXSTOCK'";
                        }
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                }

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where + " order by taskTime,warehouserId,mtlCategoryStatus,productLine";
                IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql, sql, ynPage);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                    AscmGetMaterialTaskService.GetInstance().SumQuantity(list);
                    list = list.OrderByDescending(e => e.statusInt).ToList<AscmGetMaterialTask>();
                }
                _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }

        [WebMethod()]
        public string GetJobList(string ticket, int id, ref string message)
        {
            List<AscmWipDiscreteJobs> list = null;
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                string sql = "select distinct wipentityId,sum(requiredQuantity),sum(getMaterialQuantity),sum(wmsPreparationQuantity) from ascm_wip_require_operat";
                string where = "", whereQueryWord = "";
                if (id > 0)
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
                    whereQueryWord = "wipentityid in (" + ids + ")";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmWipDiscreteJobs> ilistDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sql);
                if (ilistDiscreteJobs != null && ilistDiscreteJobs.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobs>(ilistDiscreteJobs);
                    AscmWipDiscreteJobsService.GetInstance().SetWipEntities(list);
                    AscmWipDiscreteJobsService.GetInstance().SetMaterialItem(list);
                    AscmWipDiscreteJobsService.GetInstance().SetDiscreteJobs(list);
                    SetTotalSum(ilist, list);
                    foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
                    {
                        if (string.IsNullOrEmpty(ascmWipDiscreteJobs.dateReleased))
                        {
                            ascmWipDiscreteJobs.dateReleased = ascmWipDiscreteJobs.ascmDiscreteJobs.jobDate;
                        }
                        if (string.IsNullOrEmpty(ascmWipDiscreteJobs.ascmMaterialItem_Description))
                        {
                            ascmWipDiscreteJobs.ascmMaterialItem_Description = ascmWipDiscreteJobs.ascmDiscreteJobs.jobDesc;
                        }
                        if (string.IsNullOrEmpty(ascmWipDiscreteJobs.ascmMaterialItem_DocNumber))
                        {
                            ascmWipDiscreteJobs.ascmMaterialItem_DocNumber = ascmWipDiscreteJobs.ascmDiscreteJobs.jobInfoId;
                        }
                    }
                }
                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }

        [WebMethod()]
        public string GetMtlList(string ticket, string taskId, string jobName, ref string _ynPage, ref string message)
        {
            List<AscmWipRequirementOperations> list = null;
            try
            {
                message = "";
                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                string where = "", whereQueryWord = "";
                string sql = "from AscmWipEntities";
                if (!string.IsNullOrEmpty(jobName))
                    whereQueryWord = "name = '" + jobName + "'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmWipEntities> listAscmWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                string jobId = string.Empty;
                if (listAscmWipEntities != null && listAscmWipEntities.Count > 0)
                {
                    jobId = listAscmWipEntities[0].wipEntityId.ToString();
                }

                sql = " from AscmWipRequirementOperations ";
                where = "";
                if (!string.IsNullOrEmpty(taskId))
                    whereQueryWord = "taskid = " + taskId;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(jobId))
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
                    list = list.OrderBy(e => e.ascmMaterialItem_DocNumber).ToList<AscmWipRequirementOperations>();
                }
                _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }

        [WebMethod()]
        public string SumMaterialTotal(string ticket, ref string _ynPage, ref string message)
        {
            List<AscmMaterialItem> list = null;
            try
            {
                message = "";
                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                string sql = " from AscmDiscreteJobs ";
                string where = "", whereQueryWord = "";
                string startDate = DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59";
                string endDate = DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59:59";
                whereQueryWord = "time > '" + startDate + "'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                whereQueryWord = "time <= '" + endDate + "'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmDiscreteJobs> ilistDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(sql);
                string ids = string.Empty;
                where = "";
                sql = " from AscmWipEntities ";
                if (ilistDiscreteJobs.Count > 0 && ilistDiscreteJobs != null)
                {
                    foreach (AscmDiscreteJobs ascmDiscreteJobs in ilistDiscreteJobs)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += "'" + ascmDiscreteJobs.jobId + "'";
                    }
                    if (!string.IsNullOrEmpty(ids))
                        whereQueryWord = " name in (" + ids + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    if (!string.IsNullOrEmpty(where))
                        sql += " where " + where;
                    IList<AscmWipEntities> ilistWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                    ids = "";
                    where = "";
                    sql = "select inventoryitemid, sum(requiredQuantity), sum(quantityIssued), sum(getMaterialQuantity), sum(requiredQuantity - quantityIssued) as quantityDifference, sum(requiredQuantity - getMaterialQuantity) as quantityGetMaterialDifference from ascm_wip_require_operat";
                    foreach (AscmWipEntities ascmWipEntities in ilistWipEntities)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += ascmWipEntities.wipEntityId;
                    }
                    if (!string.IsNullOrEmpty(ids))
                        whereQueryWord = "wipentityid in (" + ids + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    whereQueryWord = "wipSupplyType = 1";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    whereQueryWord = " group by inventoryitemid";
                    if (!string.IsNullOrEmpty(where))
                        sql += " where " + where + whereQueryWord;
                    IList ilistRequireOperat = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);
                    ids = "";
                    where = "";
                    sql = " from AscmMaterialItem";
                    foreach (object[] obj in ilistRequireOperat)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += obj[0].ToString();
                    }
                    if (!string.IsNullOrEmpty(ids))
                        whereQueryWord = " id in (" + ids + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    if (!string.IsNullOrEmpty(where))
                        sql += " where " + where;
                    IList<AscmMaterialItem> ilistMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql, sql, ynPage);
                    if (ilistMaterialItem != null && ilistMaterialItem.Count > 0)
                    {
                        list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistMaterialItem);
                        foreach (AscmMaterialItem ascmMaterialItem in ilistMaterialItem)
                        {
                            foreach (object[] obj in ilistRequireOperat)
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
                        }
                    }
                    _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                    return JsonConvert.SerializeObject(list);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }

        [WebMethod()]
        public string CheckTaskInfo(string ticket, string taskId, ref string message)
        {
            List<AscmGetMaterialTask> list = null;
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                string sql = "from AscmGetMaterialTask";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(taskId))
                    whereQueryWord = " id = " + taskId;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                    AscmGetMaterialTaskService.GetInstance().SetRanker(list);
                    AscmGetMaterialTaskService.GetInstance().SetWorker(list);
                }
                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }

        [WebMethod()]
        public string GetMaterialNoticeInfo(string ticket, string name, ref string message)
        {
            try
            {
                message = "";
                StringBuilder sb = new StringBuilder();
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                string userRole = string.Empty;
                List<YnRole> listYnRole = YnRoleService.GetInstance().GetListInUser(name);
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
                    userRole = name;
                }

                string sql = "from AscmWipRequirementOperations";
                string where = "", whereQueryWord = "";

                whereQueryWord = "taskId > 0";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                whereQueryWord = "wmsPreparationQuantity > 0";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmWipRequirementOperations> ilistRequireOperat = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipRequirementOperations>(sql);
                if (ilistRequireOperat != null && ilistRequireOperat.Count > 0)
                {
                    string ids = string.Empty;
                    foreach (AscmWipRequirementOperations ascmWipRequirementOperations in ilistRequireOperat)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += ascmWipRequirementOperations.taskId;
                    }

                    where = IsJudgeListCount(ids, "id");

                    sql = "from AscmGetMaterialTask";
                    whereQueryWord = "status != 'FINISH'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    if (!string.IsNullOrEmpty(userRole))
                    {
                        whereQueryWord = "workerId = '" + userRole + "'";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }

                    if (!string.IsNullOrEmpty(where))
                        sql += " where " + where + " order by uploadDate,taskTime,warehouserId,taskId";

                    IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmGetMaterialTask> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                        int count = 0;
                        foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                        {
                            if (count < 2)
                            {
                                if (!string.IsNullOrEmpty(sb.ToString()))
                                    sb.Append(",");
                                string str = "[" + ascmGetMaterialTask.id + "]" + ascmGetMaterialTask.taskIdCn + "[" + ascmGetMaterialTask.uploadDate + "]";
                                sb.Append(str);
                                count++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }

        [WebMethod()]
        public string GetLineList(string ticket, ref string message)
        {
            List<string> list = new List<string>();
            try
            {
                string sql = "select distinct productline from ascm_discrete_jobs";
                IList ilist = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);

                foreach (object[] obj in ilist)
                {
                    string str = obj[0].ToString();
                    list.Add(str);
                }
                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }

        #endregion

        #region 公用方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids">拼接字符串</param>
        /// <param name="condition">条件范围</param>
        /// <returns></returns>
        public string IsJudgeListCount(string ids, string condition)
        {
            bool flag = false;
            string where = "", whereQueryWord = "";
            if (ids.IndexOf(',') > -1)
            {
                string[] strArray = ids.Split(',');


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
                            whereQueryWord = condition + " in (" + str + ")";
                            where = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(where, whereQueryWord);
                        }
                    }
                }
                else
                {
                    whereQueryWord = condition + " in (" + ids + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
            }
            else
            {

                whereQueryWord = condition + " = " + ids.Trim().ToString();
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
            }
            return where;
        }
        #endregion

        #region 排产单管理
        [WebMethod()]
        public string GetDiscreteJobs(string ticket, string name, string queryWord, string queryType, string queryOtherWord, ref string _ynPage, ref string message)
        {
            List<AscmDiscreteJobs> list = null;
            try
            {
                message = "";
                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                string whereOther = string.Empty;
                if (string.IsNullOrEmpty(queryWord))
                {
                    whereOther = DateTime.Now.Date.ToString("yyyy-MM-dd");
                }
                else
                {
                    whereOther = queryWord;
                    queryWord = string.Empty;
                }

                string userRole = string.Empty;
                string userRule = string.Empty;
                List<YnRole> listYnRole = YnRoleService.GetInstance().GetListInUser(name);
                int nI = 0;
                foreach (YnRole role in listYnRole)
                {
                    if (role.name == "总装排产员" || role.name == "电装排产员")
                    {
                        nI = 1;
                    }
                    else if (role.name == "领料员")
                    {
                        nI = 2;
                    }
                }
                if (nI == 1)
                {
                    queryWord = name;
                }
                else if (nI == 2)
                {
                    userRule = name;
                }

                string sql = " from AscmDiscreteJobs ";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " workerId = '" + queryWord + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                if (!string.IsNullOrEmpty(queryType))
                {
                    switch (queryType)
                    {
                        case "zz":
                            whereQueryWord = "identificationId = 1";
                            break;
                        case "dz":
                            whereQueryWord = "identificationId = 2";
                            break;
                    }
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                if (!string.IsNullOrEmpty(userRule))
                {
                    string sSql = "from AscmAllocateRule where workerName = '" +userRule+ "'";
                    IList<AscmAllocateRule> ilistAscmAllocateRule = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmAllocateRule>(sSql);
                    if (ilistAscmAllocateRule != null && ilistAscmAllocateRule.Count > 0)
                    {
                        foreach (AscmAllocateRule ascmAllocateRule in ilistAscmAllocateRule)
                        {
                            if (!string.IsNullOrEmpty(ascmAllocateRule.zRankerName))
                            {
                                whereQueryWord = "workerId = '" + ascmAllocateRule.zRankerName + "'";
                                where = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(where, whereQueryWord);
                            }
                            if (!string.IsNullOrEmpty(ascmAllocateRule.dRankerName))
                            {
                                whereQueryWord = "workerId = '" + ascmAllocateRule.dRankerName + "'";
                                where = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(where, whereQueryWord);
                            }
                        }
                        where = "(" + where + ")";
                    }
                }
                if (!string.IsNullOrEmpty(queryOtherWord))
                {
                    string sSql = " from YnUser where userName = '" + queryOtherWord + "'";
                    IList<YnUser> ilistYnUser = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.Find<YnUser>(sSql);
                    if (ilistYnUser != null && ilistYnUser.Count > 0)
                    {
                        queryOtherWord = ilistYnUser[0].userId;
                    }
                    whereQueryWord = "workerId = '" + queryOtherWord + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                if (!string.IsNullOrEmpty(whereOther))
                {
                    whereOther = "time like '" + whereOther + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                }
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmDiscreteJobs> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDiscreteJobs>(sql, sql, ynPage);
                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDiscreteJobs>(ilist);
                    AscmDiscreteJobsService.GetInstance().SetRanker(list);

                    _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                    return JsonConvert.SerializeObject(list);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        #endregion

        #region 容器信息

        /// <summary>
        /// 规格
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetContainerSpaceInfo(string ticket, string id, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                int intSpaceId = Convert.ToInt32(id);
                return MideaAscm.Services.SupplierPreparation.AscmContainerSpecService.GetInstance().Get(intSpaceId).spec;

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;


        }
        /// <summary>
        /// 流转信息
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetAscmTagLog(string ticket, string id, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                return MideaAscm.Services.ContainerManage.AscmTagLogService.GetInstance().Get(id);

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;


        }
        /// <summary>
        /// 供应商名称
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetContainerSupplierInfo(string ticket, string id, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                int intSupplierId = Convert.ToInt32(id);
                return MideaAscm.Services.Base.AscmSupplierService.GetInstance().Get(intSupplierId).name;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }

        /// <summary>
        /// 手持端盘点功能
        /// </summary>
        /// <param name="strSn"></param>
        [WebMethod]
        public string ContainerCheck(string ticket, ref string message, string strSn, string status, string userName)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                return MideaAscm.Services.ContainerManage.AscmContaierCheckService.GetInstance().ContainerCheck(strSn, status, userName);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }

        /// <summary>
        /// 出入库
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="message"></param>
        /// <param name="userName"></param>
        /// <param name="containerId"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        /// 
        [WebMethod]
        public string AscmStoreInOutSave(string ticket, ref string message, string userName, string containerId, string direction, string docNumber)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                return MideaAscm.Services.ContainerManage.AscmStoreInOutService.GetInstance().AscmStoreInOutSave(userName, containerId, direction,docNumber);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }

        /// <summary>
        /// 得到单号
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string GetAscmKeyBill()
        {
            return YnFrame.Services.YnBillKeyService.GetInstance().GetBillKey("AscmStoreInOut", "", "yyyyMMdd", 2);
        }

        /// <summary>
        /// 得到容器的全部信息，一次性
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="containerSn"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetContainerAllInfo(string ticket, string containerSn, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                AscmContainer ascmContainer = MideaAscm.Services.ContainerManage.AscmContainerInfoService.GetInstance().GetAllInfo(containerSn);
                if (ascmContainer == null)
                    ascmContainer = new AscmContainer();
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmContainer);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
 
        }
        /// <summary>
        /// LED容器库存信息显示
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string GetContainerDisplayInf()
        {
           
            try
            {
               IList<AscmContainer> ascmContainerlist = MideaAscm.Services.ContainerManage.AscmContainerInfoService.GetInstance().DispayData();
               if (ascmContainerlist.Count()==0)
               {
                   return null;
               }
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmContainerlist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
         
 
 
        }
        /// <summary>
        /// LED显示流转信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string DispayFlowData()
        {
            try
            {
                List<AscmContainer> ascmContainerlist = MideaAscm.Services.ContainerManage.AscmContainerInfoService.GetInstance().DispayFlowData();
                if (ascmContainerlist.Count == 0)
                {
                    return "";
                }
                else
                {
                    return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmContainerlist);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// LED显示超期预警信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string DisplayWarInf()
        {

            try
            {
                List<AscmContainer> ascmContainerlist = MideaAscm.Services.ContainerManage.AscmContainerInfoService.GetInstance().DisplayWarInf();
                if (ascmContainerlist.Count == 0)
                {
                    return "";
                }
                else
                {
                    return YnBaseClass2.Helper.ObjectHelper.Serialize(ascmContainerlist);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [WebMethod]
        public string GetCheckInfBydocNumber(string docNumber)
        {
            try
            {
                List<MideaAscm.Dal.ContainerManage.Entities.AscmStoreInOut> list = MideaAscm.Services.ContainerManage.AscmStoreInOutService.GetInstance().GetCheckInfBydocNumber(docNumber);
                if (list.Count == 0)
                {
                    return "";
                }
                else
                {
                    return YnBaseClass2.Helper.ObjectHelper.Serialize(list);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [WebMethod]
        public string SuplierSiteCode()
        {
            try 
            { 
              return  MideaAscm.Services.ContainerManage.AscmContainerInfoService.GetInstance().SuplierSiteCode();
            }
            catch (Exception ex)
            {

                throw ex;
 
            }
 
        }

        #endregion

        #region 最新领料
        //领料监控
        [WebMethod()]
        public string GetMonitorTaskList(string ticket, string userName, string queryStartDate, string queryEndDate, string queryStartJobDate, string queryEndJobDate, string queryType, string queryFormat, string queryStatus, string queryLine, string queryTime, string queryWarehouse, string queryWipEntity, string taskString, ref string _ynPage, ref string message)
        {
            message = "";
            //List<AscmGetMaterialTask> list = null;
            List<AscmWipDiscreteJobs> list = null;
            YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);

            try
            {
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                
                if (!string.IsNullOrEmpty(queryWipEntity))
                {
                    string sql = "from AscmWipEntities where name = '" + queryWipEntity + "'";
                    IList<AscmWipEntities> ilist_WipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                    if (ilist_WipEntities != null && ilist_WipEntities.Count > 0)
                    {
                        queryWipEntity = ilist_WipEntities[0].wipEntityId.ToString();
                    }
                    else
                    {
                        queryWipEntity = "";
                    }
                }

                string taskid = AscmGetMaterialTaskService.GetInstance().GetMonitorTaskList("", "", "", userName, queryStatus, queryLine, queryType, queryStartDate, queryEndDate, taskString, queryWarehouse, queryFormat, "", queryStartJobDate, queryEndJobDate, queryWipEntity);
                list = AscmWipDiscreteJobsService.GetInstance().GetWipDiscreteJobsSumListI(ynPage, taskid);
                      //GetMonitorTaskList(ynPage, "", "", "", userName, queryStatus, queryLine, queryType, queryStartDate, queryEndDate, taskString, queryWarehouse, queryFormat, "", queryStartJobDate, queryEndJobDate, queryWipEntity);
                if (list != null && list.Count > 0)
                {
                    _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                    return JsonConvert.SerializeObject(list);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return null;
        }

        [WebMethod()]
        public string GetMonitorJobList(string ticket, int id, string queryBomMtlCategory,  ref string message)
        {
            List<AscmWipRequirementOperations> list = null;
            message = "";

            try
            {
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                //string sql = "select distinct wipentityId,sum(requiredQuantity),sum(getMaterialQuantity),sum(wmsPreparationQuantity) from ascm_wip_require_operat";
                //string where = "", whereQueryWord = "";
                //if (id > 0)
                //    whereQueryWord = "taskId = " + id;
                //where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                //string ids = string.Empty;
                //IList ilist = null;
                //if (!string.IsNullOrEmpty(where))
                //{
                //    sql += " where " + where + " group by wipentityId";
                //    ilist = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);
                //    foreach (object[] obj in ilist)
                //    {
                //        if (!string.IsNullOrEmpty(ids))
                //            ids += ",";
                //        ids += obj[0].ToString();
                //    }
                //    where = "";
                //}

                //sql = "from AscmWipDiscreteJobs ";
                //if (!string.IsNullOrEmpty(ids))
                //{
                //    whereQueryWord = "wipentityid in (" + ids + ")";
                //    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                //}

                //if (!string.IsNullOrEmpty(where))
                //{
                //    sql += " where " + where;
                //    IList<AscmWipDiscreteJobs> ilistDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sql);
                //    if (ilistDiscreteJobs != null && ilistDiscreteJobs.Count > 0)
                //    {
                //        list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobs>(ilistDiscreteJobs);
                //        AscmWipDiscreteJobsService.GetInstance().SetWipEntities(list);
                //        AscmWipDiscreteJobsService.GetInstance().SetMaterialItem(list);
                //        AscmWipDiscreteJobsService.GetInstance().SetDiscreteJobs(list);
                //        AscmWipDiscreteJobsService.GetInstance().SetMarkTaskLog(id, list);
                //        SetTotalSum(ilist, list);
                //        list = list.OrderBy(e => e.ascmDiscreteJobs_line).ToList<AscmWipDiscreteJobs>();

                //        return JsonConvert.SerializeObject(list);
                //    }
                //}

                list = AscmWipRequirementOperationsService.GetInstance().GetList("", "", "", "", "", id.ToString(), "", queryBomMtlCategory);
                if (list != null && list.Count > 0)
                {
                    //_ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                    return JsonConvert.SerializeObject(list);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return null;
        }

        //[WebMethod()]
        //public string GetMonitorJobList(string ticket, int id, ref string message)
        //{
        //    List<AscmWipDiscreteJobs> list = null;
        //    message = "";

        //    try
        //    {
        //        //if (!IsTicketValid(ticket))
        //        //    throw new Exception("票证验证失败！");

        //        string sql = "select distinct wipentityId,sum(requiredQuantity),sum(getMaterialQuantity),sum(wmsPreparationQuantity) from ascm_wip_require_operat";
        //        string where = "", whereQueryWord = "";
        //        if (id > 0)
        //            whereQueryWord = "taskId = " + id;
        //        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

        //        string ids = string.Empty;
        //        IList ilist = null;
        //        if (!string.IsNullOrEmpty(where))
        //        {
        //            sql += " where " + where + " group by wipentityId";
        //            ilist = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);
        //            foreach (object[] obj in ilist)
        //            {
        //                if (!string.IsNullOrEmpty(ids))
        //                    ids += ",";
        //                ids += obj[0].ToString();
        //            }
        //            where = "";
        //        }

        //        sql = "from AscmWipDiscreteJobs ";
        //        if (!string.IsNullOrEmpty(ids))
        //        {
        //            whereQueryWord = "wipentityid in (" + ids + ")";
        //            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
        //        }

        //        if (!string.IsNullOrEmpty(where))
        //        {
        //            sql += " where " + where;
        //            IList<AscmWipDiscreteJobs> ilistDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sql);
        //            if (ilistDiscreteJobs != null && ilistDiscreteJobs.Count > 0)
        //            {
        //                list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobs>(ilistDiscreteJobs);
        //                AscmWipDiscreteJobsService.GetInstance().SetWipEntities(list);
        //                AscmWipDiscreteJobsService.GetInstance().SetMaterialItem(list);
        //                AscmWipDiscreteJobsService.GetInstance().SetDiscreteJobs(list);
        //                AscmWipDiscreteJobsService.GetInstance().SetMarkTaskLog(id, list);
        //                SetTotalSum(ilist, list);
        //                list = list.OrderBy(e => e.ascmDiscreteJobs_line).ToList<AscmWipDiscreteJobs>();

        //                return JsonConvert.SerializeObject(list);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message = ex.Message;
        //    }

        //    return null;
        //}

        private void SetTotalSum(IList ilist, List<AscmWipDiscreteJobs> list)
        {
            if (ilist != null && ilist.Count > 0 && list != null && ilist.Count > 0)
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
        }
        [WebMethod()]
        public string GetMonitorMaterialList(string ticket, string taskId, string jobId, string queryBomWarehouse, string queryBomMtlCategory, ref string _ynPage, ref string message)
        {
            YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);
            List<AscmWipRequirementOperations> list = null;
            message = "";

            try
            {
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                list = AscmWipRequirementOperationsService.GetInstance().GetList(ynPage, "", "", "", "", taskId, jobId, queryBomWarehouse, queryBomMtlCategory);
                if (list != null && list.Count > 0)
                {
                    _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                    return JsonConvert.SerializeObject(list);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return null;
        }
        //执行任务
        [WebMethod()]
        public bool StartExcuteTask(string ticket, string userName, string releaseHeaderIds, ref string message)
        {
            List<AscmGetMaterialTask> list = null;
            message = "";
            try
            {
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

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
                    bool flag = false;
                    foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                    {
                        if (ascmGetMaterialTask.status == "NOTEXECUTE")
                        {
                            ascmGetMaterialTask.status = "EXECUTE";
                            ascmGetMaterialTask.starTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.modifyUser = userName;
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        AscmGetMaterialTaskService.GetInstance().Update(list);
                        return true;
                    }
                    else
                    {
                        throw new Exception("请检查任务状态，无法执行开始操作！");
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public bool ConfrimedBatchGetmaterials(string ticket, string userName, string releaseHeaderIds, ref string message)
        {
            bool result = false;
            try
            {
                result = AscmGetMaterialTaskService.GetInstance().BatchGetMaterialTask(userName, releaseHeaderIds);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return result;
        }
        [WebMethod()]
        public bool EndExcuteTask(string ticket, string userName, string releaseHeaderIds, ref string message)
        {
            List<AscmGetMaterialTask> list = null;
            message = "";

            try
            {
                
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                string sql = " from AscmGetMaterialTask ";
                string where = "", whereQueryWord = "";
                whereQueryWord = " status = 'EXECUTE'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(userName))
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
                                    ascmGetMaterialTask.endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
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
                        return true;
                    }
                    else
                    {
                        throw new Exception("请检查任务状态，无法执行结束操作！");
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        //标记任务
        [WebMethod()]
        public bool MarkTask(string ticket, string userName, string taskId, string jobId, ref string message)
        {
            try
            {
                string warehouse = string.Empty;
                if (string.IsNullOrEmpty(taskId) || string.IsNullOrEmpty(jobId))
                    throw new Exception("标记异常:任务号或作业号为空！");

                if (!string.IsNullOrEmpty(taskId))
                {
                    AscmGetMaterialTask ascmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().Get(int.Parse(taskId));
                    if (ascmGetMaterialTask == null)
                        throw new Exception("该任务不存在！");
                    warehouse = string.IsNullOrEmpty(ascmGetMaterialTask.warehouserId) ? null : ascmGetMaterialTask.warehouserId.Substring(0, 4).ToUpper().ToString();
                }

                if (AscmCommonHelperService.GetInstance().IsJudgeSpecWareHouse(warehouse))
                {
                    AscmMarkTaskLog ascmMarkTaskLog = null;
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmMarkTaskLog where wipEntityId = " + jobId + " and taskId = " + taskId + " and isMark = 1");
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
                    ascmMarkTaskLog.wipEntityId = int.Parse(jobId);
                    ascmMarkTaskLog.taskId = int.Parse(taskId);
                    ascmMarkTaskLog.isMark = 1;
                    ascmMarkTaskLog.markType = "NONAUTO";
                    ascmMarkTaskLog.warehouseId = warehouse;

                    AscmGetMaterialTask ascmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().Get(int.Parse(taskId));
                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.relatedMark))
                        ascmGetMaterialTask.relatedMark += ",";
                    ascmGetMaterialTask.relatedMark += ascmMarkTaskLog.id.ToString();
                    ascmGetMaterialTask.modifyUser = userName;
                    ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    AscmGetMaterialTaskService.GetInstance().Update(ascmGetMaterialTask);

                    AscmMarkTaskLogService.GetInstance().Save(ascmMarkTaskLog);
                    return true;
                }
                else
                {
                    throw new Exception("非特殊子库违规标记失败！");
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return false;
        }
        [WebMethod()]
        public bool UnMarkTask(string ticket, string userName, string taskId, string jobId, ref string message)
        {
            List<AscmMarkTaskLog> list = null;

            try
            {
                string warehouse = string.Empty;
                if (string.IsNullOrEmpty(taskId) || string.IsNullOrEmpty(jobId))
                {
                    throw new Exception("取消标记异常:任务号或作业号为空！");
                }
                else
                {
                    object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject("select count(*) from AscmMarkTaskLog where wipEntityId = " + jobId + " and taskId = " + taskId + " and isMark = 1");
                    if (object1 == null)
                        throw new Exception("查询异常！");
                    int iCount = 0;
                    if (int.TryParse(object1.ToString(), out iCount) && iCount == 0)
                        throw new Exception("该标记不存在！");

                    string sql = "from AscmMarkTaskLog";
                    string where = "", whereQueryWord = "";
                    whereQueryWord = "taskId = " + taskId;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    whereQueryWord = "wipEntityId = " + jobId;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    whereQueryWord = "isMark = 1";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    whereQueryWord = "markType = 'NONAUTO'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                    if (!string.IsNullOrEmpty(where))
                    {
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

                                return true;
                            }
                            else
                            {
                                throw new Exception("取消标记失败！");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return false;
        }
        //汇总物料
        [WebMethod()]
        public string SumMaterial(string ticket, string userName, string queryStartTime, string queryEndTime, ref string _ynPage, ref string message)
        {
            List<AscmWipRequirementOperations> list = null;
            message = "";
            
            try
            {
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);

                list = AscmWipRequirementOperationsService.GetInstance().GetSumList(ynPage, userName, queryStartTime, queryEndTime, null);
                if (list != null && list.Count > 0)
                {
                    _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                    return JsonConvert.SerializeObject(list);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        //获取产线
        [WebMethod()]
        public string GetTodayLineList(string ticket, string time, ref string message)
        {
            List<string> list = new List<string>();
            try
            {
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                string startTime = time + " 00:00:00";
                string endTime = time + " 59:59:59";
                string sql = "select distinct productline from ascm_discrete_jobs where createTime >= '" + startTime + "' and createTime <= '" + endTime + "'";
                IList ilist = YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);

                foreach (object[] obj in ilist)
                {
                    string str = obj[0].ToString();
                    list.Add(str);
                }
                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return null;
        }
        //领料通知
        [WebMethod()]
        public string GetTaskbarNotifierMessage(string ticket, string userName, ref string message)
        {
            message = "";
            
            StringBuilder sb = new StringBuilder();
            try
            {
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                string notifierString = AscmGetMaterialTaskService.GetInstance().GetNotifierMessageList(userName);
                if (!string.IsNullOrEmpty(notifierString))
                    return notifierString;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return null;
        }
        //查询排产单
        [WebMethod()]
        public string GetDiscreteJobsList(string ticket, string userName,string queryStartTime, string queryEndTime, ref string _ynPage, ref string message)
        {
            List<AscmDiscreteJobs> list = null;
            message = "";

            try
            {
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);

                list = AscmDiscreteJobsService.GetInstance().GetList(ynPage, "", "", "", queryStartTime, queryEndTime, "", "", userName);

                if (list != null && list.Count > 0)
                {
                    _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                    return JsonConvert.SerializeObject(list);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return null;
        }
        //仓库物料查询
        [WebMethod()]
        public string GetWarehouseMaterialList(string ticket, string queryWord, ref string _ynPage, ref string message)
        {
            List<AscmMaterialItem> list = null;
            message = "";

            try
            {
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);

                list = AscmMaterialItemService.GetInstance().GetList(ynPage, "", "", queryWord);
                if (list != null && list.Count > 0)
                {
                    _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                    return JsonConvert.SerializeObject(list);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return null;
        }
        [WebMethod()]
        public string GetMaterialWarehouseList(string ticket, int materialId, ref string _ynPage, ref string message)
        {
            List<AscmMtlOnhandQuantitiesDetail> list = null;
            message = "";

            try
            {
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);

                list = AscmMtlOnhandQuantitiesDetailService.GetInstance().GetSumList(ynPage, materialId);
                if (list != null && list.Count > 0)
                {
                    _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                    return JsonConvert.SerializeObject(list);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return null;
        }
        [WebMethod()]
        public string GetRelatedDiscreteJobsList(string ticket, string queryStartTime, string queryEndTime, int materialId, ref string _ynPage, ref string message)
        {
            List<AscmDiscreteJobs> list = null;

            try
            {
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);

                queryStartTime = queryStartTime + " 00:00:00";
                queryEndTime = queryEndTime + " 23:59:59";

                list = AscmDiscreteJobsService.GetInstance().GetRelatedDiscreteJobs(ynPage, "", "", "", queryStartTime, queryEndTime, materialId);

                if (list != null && list.Count > 0)
                {
                    _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                    return JsonConvert.SerializeObject(list);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return null;
        }
        //手动任务管理
        [WebMethod()]
        public string GetUnAutoTaskList(string ticket, string queryStartTime, string queryEndTime, string queryStatus, ref string _ynPage, ref string message)
        {
            List<AscmGetMaterialTask> list = null;

            try
            {
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);

                list = AscmGetMaterialTaskService.GetInstance().GetList(ynPage, "", "", "", "", queryStartTime, queryEndTime, queryStatus);

                if (list != null && list.Count > 0)
                {
                    _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
                    return JsonConvert.SerializeObject(list);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return null;
        }
        //添加手动任务
        [WebMethod()]
        public bool AddUnAutoTask(string ticket, string userName, string jsonString, ref string message)
        {
            try
            {
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                AscmGetMaterialTask ascmGetMaterialTask = (AscmGetMaterialTask)JsonConvert.DeserializeObject(jsonString,typeof(AscmGetMaterialTask));
                string userRole = AscmUserInfoService.GetInstance().GetUserRoleName(userName);
                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmGetMaterialTask where TaskId like '%L%' AND CREATETIME like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%'");
                ascmGetMaterialTask.taskId = (maxId == 0) ? "L1001" : "L" + (int.Parse(AscmGetMaterialTaskService.GetInstance().Get(maxId).taskId.Substring(1, 4)) + 1).ToString();
                ascmGetMaterialTask.id = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmGetMaterialTask ") + 1;
                ascmGetMaterialTask.createUser = userName;
                ascmGetMaterialTask.createTime = datetime;
                ascmGetMaterialTask.modifyUser = userName;
                ascmGetMaterialTask.modifyTime = datetime;
                ascmGetMaterialTask.workerId = userName;
                ascmGetMaterialTask.status = AscmGetMaterialTask.StatusDefine.notExecute;
                ascmGetMaterialTask.materialType = 1;
                ascmGetMaterialTask.uploadDate = DateTime.Now.ToString("yyyy-MM-dd");
                if (!string.IsNullOrEmpty(ascmGetMaterialTask.relatedMark))
                {
                    AscmMarkTaskLog ascmMarkTaskLog = AscmMarkTaskLogService.GetInstance().Get(int.Parse(ascmGetMaterialTask.relatedMark));
                    if (ascmMarkTaskLog != null)
                    {
                        ascmMarkTaskLog.isMark = 0;
                        ascmMarkTaskLog.modifyUser = userName;
                        ascmMarkTaskLog.modifyTime = datetime;

                        AscmMarkTaskLogService.GetInstance().Update(ascmMarkTaskLog);
                    }
                }

                AscmGetMaterialTaskService.GetInstance().Save(ascmGetMaterialTask);

                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return false;
        }
        [WebMethod()]
        public bool DeleteUnAutoTask(string ticket, string userName, int id, ref string message)
        {
            try
            {
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                AscmGetMaterialTask ascmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().Get(id);
                if (ascmGetMaterialTask.status == "NOTALLOCATE" || ascmGetMaterialTask.status == "NOTEXECUTE")
                {
                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.relatedMark))
                    {
                        AscmMarkTaskLog ascmMarkTaskLog = AscmMarkTaskLogService.GetInstance().Get(int.Parse(ascmGetMaterialTask.relatedMark));
                        if (ascmMarkTaskLog != null)
                        {
                            ascmMarkTaskLog.isMark = 1;
                            ascmMarkTaskLog.modifyUser = userName;
                            ascmMarkTaskLog.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                            AscmMarkTaskLogService.GetInstance().Update(ascmMarkTaskLog);
                        }
                    }
                    
                    AscmGetMaterialTaskService.GetInstance().Delete(id);
                    return true;
                }
                else
                {
                    throw new Exception("只能删除未分配或已分配任务！");
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return false;
        }
        [WebMethod()]
        public string GetSomeMaterialList(string ticket, string queryWord, ref string message)
        {
            List<AscmMaterialItem> list = null;
            try
            {
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                string sql = "from AscmMaterialItem";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = "docNumber like '" + queryWord + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                whereQueryWord = "wipSupplyType < 4";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                whereQueryWord = "rownum <= 100";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                list = AscmMaterialItemService.GetInstance().GetList(sql);
                if (list != null && list.Count > 0)
                    return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return null;
        }
        [WebMethod()]
        public string GetSomeRelatedMarkList(string ticket, ref string message)
        {
            List<AscmMarkTaskLog> list = null;
            try
            {
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                string sql = "from AscmMarkTaskLog where isMark = 1 and rownum <= 100 order by id";
                list = AscmMarkTaskLogService.GetInstance().GetList(sql, true, true);
                if (list != null && list.Count > 0)
                    return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return null;
        }
        [WebMethod()]
        public string GetSomeWareHouse(string ticket, string queryWord, ref string message)
        {
            List<AscmWarehouse> list = null;
            try
            {
                //if (!IsTicketValid(ticket))
                //    throw new Exception("票证验证失败！");

                string sql = "from AscmWarehouse";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = "id like '" + queryWord + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                whereQueryWord = "rownum <= 100";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                list = AscmWarehouseService.GetInstance().GetList(sql);
                if (list != null && list.Count > 0)
                    return JsonConvert.SerializeObject(list);

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return null;
        }
        #endregion
    }
}