using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Services.FromErp;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Services.Base;

namespace MideaAscm.Areas.Ascm.ReportViewer
{
    public partial class SupplierDeliveryOrderDetailPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //提取数据
                string queryWord = "";
                string whereOther = "", whereMain = "";
                string whereSupplier = "", whereWarehouse = "", whereMaterialItem = "";
                string whereStartDeliveryTime = "", whereEndDeliveryTime = "", whereStatus = "", whereWipEntity = "";

                YnFrame.Dal.Entities.YnUser ynUser = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketUserData();
                if (ynUser == null)
                    throw new Exception("用户错误！");
                AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(ynUser.userId);
                if (ascmUserInfo.extExpandType != "erp")
                    throw new Exception("供应商用户错误！");
                AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                if (ascmUserInfo.ascmSupplier == null)
                    throw new Exception("供应商用户错误！");

                if (Request.QueryString["queryWord"] != null && Request.QueryString["queryWord"].ToString().Trim() != "" && Request.QueryString["queryWord"].ToString().Trim() != "null")
                    queryWord = Request.QueryString["queryWord"].ToString();

                if (Request.QueryString["materialItem"] != null && Request.QueryString["materialItem"].ToString().Trim() != "" && Request.QueryString["materialItem"].ToString().Trim() != "null")
                    whereMaterialItem = "materialId = " + Request.QueryString["materialItem"].ToString();
                whereSupplier = "supplierId = " + ascmUserInfo.ascmSupplier.id + ""; 
                if (Request.QueryString["warehouse"] != null && Request.QueryString["warehouse"].ToString().Trim() != "" && Request.QueryString["warehouse"].ToString().Trim() != "null")
                    whereWarehouse = "warehouseId = '" + Request.QueryString["warehouse"].ToString() + "'";
                if (Request.QueryString["status"] != null && Request.QueryString["status"].ToString().Trim() != "" && Request.QueryString["status"].ToString().Trim() != "null")
                    whereStatus = "status = '" + Request.QueryString["status"].ToString() + "'";

                DateTime dtStartDeliveryTime, dtEndDeliveryTime;
                if (Request.QueryString["startDeliveryTime"] != null && Request.QueryString["startDeliveryTime"].ToString().Trim() != "" && Request.QueryString["startDeliveryTime"].ToString().Trim() != "null" && DateTime.TryParse(Request.QueryString["startDeliveryTime"].ToString().Trim(), out dtStartDeliveryTime))
                    whereStartDeliveryTime = "deliveryTime>='" + dtStartDeliveryTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (Request.QueryString["endCreateTime"] != null && Request.QueryString["endCreateTime"].ToString().Trim() != "" && Request.QueryString["endCreateTime"].ToString().Trim() != "null" && DateTime.TryParse(Request.QueryString["endCreateTime"].ToString().Trim(), out dtEndDeliveryTime))
                    whereEndDeliveryTime = "deliveryTime<'" + dtEndDeliveryTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                if (Request.QueryString["wipEntityId"] != null && Request.QueryString["wipEntityId"].ToString().Trim() != "" && Request.QueryString["wipEntityId"].ToString().Trim() != "null")
                    whereStatus = "wipEntityId = " + Request.QueryString["wipEntityId"];

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

                YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                ynPage.SetPageSize(500); //pageRows;
                ynPage.SetCurrentPage(1); //pageNumber;

                List<AscmDeliveryOrderDetail> listAscmDeliveryOrderDetail = AscmDeliveryOrderDetailService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("SupplierDeliveryOrderDetailReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = listAscmDeliveryOrderDetail;
                ReportViewer1.LocalReport.DataSources.Clear();//好像不clear也可以
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                //string companpyTitle = YnParameterService.GetInstance().GetValue(MyParameter.companpyTitle);
                string companpyTitle = "美的中央空调";
                string title = companpyTitle + "送货单明细统计报表";

                ReportParameter[] reportParameters = new ReportParameter[] {
                    new ReportParameter("ReportParameter_Title",title ),
                    new ReportParameter("ReportParameter_ReportTime","打印时间："+DateTime.Now.ToString("yyyy-MM-dd") ),
                    new ReportParameter("ReportParameter_SupplierName","供应商："+ascmUserInfo.ascmSupplier.name )
                };

                ReportViewer1.LocalReport.SetParameters(reportParameters);
                ReportViewer1.LocalReport.Refresh();
            }
        }
        protected void ReportViewer1_PreRender(object sender, EventArgs e)
        {
            string companpyTitle = "美的中央空调";
            string title = companpyTitle + "送货单明细统计报表";
            this.ReportViewer1.LocalReport.DisplayName = title;
        }
    }
}