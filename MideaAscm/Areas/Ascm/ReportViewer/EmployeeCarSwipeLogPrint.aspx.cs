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
    public partial class EmployeeCarSwipeLogPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //提取数据
                string queryWord = string.Empty, whereOther = string.Empty;

                if (!string.IsNullOrWhiteSpace(Request.QueryString["queryWord"]))
                    queryWord = Request.QueryString["queryWord"].Trim();
                if (!string.IsNullOrWhiteSpace(Request.QueryString["doorId"]))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "doorId=" + Request.QueryString["doorId"].Trim());
                if (!string.IsNullOrWhiteSpace(Request.QueryString["direction"]))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "direction='" + Request.QueryString["direction"].Trim() + "'");
                if (!string.IsNullOrWhiteSpace(Request.QueryString["plateNumber"]))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "upper(plateNumber) like '%" + Request.QueryString["plateNumber"].Trim().ToUpper() + "%'");
                if (!string.IsNullOrWhiteSpace(Request.QueryString["employeeName"]))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "upper(employeeName) like '%" + Request.QueryString["employeeName"].Trim().ToUpper() + "%'");
                DateTime dtStartCreateTime, dtEndCreateTime;
                if (!string.IsNullOrEmpty(Request.QueryString["queryStartTime"]) && DateTime.TryParse(Request.QueryString["queryStartTime"], out dtStartCreateTime))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'");
                if (!string.IsNullOrEmpty(Request.QueryString["queryEndTime"]) && DateTime.TryParse(Request.QueryString["queryEndTime"], out dtEndCreateTime))
                    whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'");

                List<AscmEmpCarSwipeLog> listAscmEmpCarSwipeLog = AscmEmpCarSwipeLogService.GetInstance().GetList(null, "", "", queryWord, whereOther);

                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("EmployeeCarSwipeLogReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = listAscmEmpCarSwipeLog;
                ReportViewer1.LocalReport.DataSources.Clear();//好像不clear也可以
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                string companpyTitle = "美的中央空调";
                string title = companpyTitle + "员工车辆出入日志";

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
            string title = companpyTitle + "员工车辆出入日志";
            this.ReportViewer1.LocalReport.DisplayName = title;
        }
    }
}