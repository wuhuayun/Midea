using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Services.Base;

namespace MideaAscm.Areas.Ascm.ReportViewer
{
    public partial class EmployeeCarPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<AscmEmployeeCar> listAscmDeliveryOrderDetail = AscmEmployeeCarService.GetInstance().GetList(null, "", "", "", "");
                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("EmployeeCarReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = listAscmDeliveryOrderDetail;
                ReportViewer1.LocalReport.DataSources.Clear();//好像不clear也可以
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                string companpyTitle = "美的中央空调";
                string title = companpyTitle + "员工车辆报表";

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
            string title = companpyTitle + "员工车辆报表";
            this.ReportViewer1.LocalReport.DisplayName = title;
        }
    }
}