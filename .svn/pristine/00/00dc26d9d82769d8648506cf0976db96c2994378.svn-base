using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using MideaAscm.Services.FromErp;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Areas.Ascm.ReportViewer
{
    public partial class DeliveryOrderDetailPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //提取数据
                string queryWord = ""; 
                string whereOther = "", whereMain = "";
                string whereSupplier = "", whereWarehouse = "", whereMaterialItem = "";
                string whereStartDeliveryTime = "", whereEndDeliveryTime = "", whereStatus = "";

                if (Request.QueryString["queryWord"] != null && Request.QueryString["queryWord"].ToString().Trim() != "" && Request.QueryString["queryWord"].ToString().Trim() != "null")
                    queryWord = Request.QueryString["queryWord"].ToString();

                if (Request.QueryString["materialItem"] != null && Request.QueryString["materialItem"].ToString().Trim() != "" && Request.QueryString["materialItem"].ToString().Trim() != "null")
                    whereMaterialItem = "materialId = " + Request.QueryString["materialItem"].ToString();
                if (Request.QueryString["supplier"] != null && Request.QueryString["supplier"].ToString().Trim() != "" && Request.QueryString["supplier"].ToString().Trim() != "null")
                    whereSupplier = "supplierId = " + Request.QueryString["supplier"].ToString() + "";
                if (Request.QueryString["warehouse"] != null && Request.QueryString["warehouse"].ToString().Trim() != "" && Request.QueryString["warehouse"].ToString().Trim() != "null")
                    whereWarehouse = "warehouseId = '" + Request.QueryString["warehouse"].ToString() + "'";
                if (Request.QueryString["status"] != null && Request.QueryString["status"].ToString().Trim() != "" && Request.QueryString["status"].ToString().Trim() != "null")
                    whereStatus = "status = '" + Request.QueryString["status"].ToString() + "'";

                DateTime dtStartDeliveryTime, dtEndDeliveryTime;
                if (Request.QueryString["startDeliveryTime"] != null && Request.QueryString["startDeliveryTime"].ToString().Trim() != "" && Request.QueryString["startDeliveryTime"].ToString().Trim() != "null" && DateTime.TryParse(Request.QueryString["startDeliveryTime"].ToString().Trim(), out dtStartDeliveryTime))
                    whereStartDeliveryTime = "deliveryTime>='" + dtStartDeliveryTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (Request.QueryString["endCreateTime"] != null && Request.QueryString["endCreateTime"].ToString().Trim() != "" && Request.QueryString["endCreateTime"].ToString().Trim() != "null" && DateTime.TryParse(Request.QueryString["endCreateTime"].ToString().Trim(), out dtEndDeliveryTime))
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

                YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                ynPage.SetPageSize(500); //pageRows;
                ynPage.SetCurrentPage(1); //pageNumber;

                List<AscmDeliveryOrderDetail> listAscmDeliveryOrderDetail = AscmDeliveryOrderDetailService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("DeliveryOrderDetailReport.rdlc");
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
                    new ReportParameter("ReportParameter_ReportTime","打印时间："+DateTime.Now.ToString("yyyy-MM-dd") )
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