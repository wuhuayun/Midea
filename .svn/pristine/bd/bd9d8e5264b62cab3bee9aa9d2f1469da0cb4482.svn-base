using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Services.SupplierPreparation;
using MideaAscm.Services.Base;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.Vehicle.Entities;
using MideaAscm.Services.Vehicle;

namespace MideaAscm.Areas.Ascm.ReportViewer
{
    public partial class UnloadingPointLogPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //提取数据
                string queryWord = "";
                string whereOther = "", whereWarehouse = "";
                string whereStartCreateTime = "", whereEndCreateTime = "";

                if (Request.QueryString["queryWord"] != null && Request.QueryString["queryWord"].Trim() != "" && Request.QueryString["queryWord"].Trim() != "null")
                    queryWord = Request.QueryString["queryWord"].Trim();
                if (Request.QueryString["warehouseId"] != null && Request.QueryString["warehouseId"].Trim() != "" && Request.QueryString["warehouseId"].Trim() != "null")
                    whereWarehouse = "unloadingPointId in(select id from AscmUnloadingPoint where warehouseId='" + Request.QueryString["warehouseId"] + "')";
                DateTime dtStartCreateTime, dtEndCreateTime;
                if (!string.IsNullOrEmpty(Request.QueryString["queryStartTime"]) && DateTime.TryParse(Request.QueryString["queryStartTime"], out dtStartCreateTime))
                    whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(Request.QueryString["queryEndTime"]) && DateTime.TryParse(Request.QueryString["queryEndTime"], out dtEndCreateTime))
                    whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);

                YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                ynPage.SetPageSize(500); //pageRows;
                ynPage.SetCurrentPage(1); //pageNumber;

                List<AscmUnloadingPointLog> listAscmUnloadingPointLog = AscmUnloadingPointLogService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);

                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("UnloadingPointLogReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = listAscmUnloadingPointLog;
                ReportViewer1.LocalReport.DataSources.Clear();//好像不clear也可以
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                string companpyTitle = "美的中央空调";
                string title = companpyTitle + "卸货点出入日志";

                ReportParameter[] reportParameters = new ReportParameter[] {
                    new ReportParameter("ReportParameter_Title", title),
                    new ReportParameter("ReportParameter_ReportTime", "打印时间：" + DateTime.Now.ToString("yyyy-MM-dd"))
                };
                ReportViewer1.LocalReport.SetParameters(reportParameters);
                ReportViewer1.LocalReport.Refresh();
            }
        }
        protected void ReportViewer1_PreRender(object sender, EventArgs e)
        {
            string companpyTitle = "美的中央空调";
            string title = companpyTitle + "卸货点出入日志";
            this.ReportViewer1.LocalReport.DisplayName = title;
        }
    }
}