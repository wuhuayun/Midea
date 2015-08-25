using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using MideaAscm.Dal.ContainerManage.Entities;
using MideaAscm.Services.ContainerManage;

namespace MideaAscm.Areas.Ascm.ReportViewer
{
    public partial class ContainerHistoryPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //提取数据
                string whereOther = "",whereDocNumber = "", whereSupplier = "", whereDirection = "";
                string whereStartCreateTime = "", whereEndCreateTime = "";

                if (Request.QueryString["docNumber"] != null && Request.QueryString["docNumber"].ToString().Trim() != "" && Request.QueryString["docNumber"].ToString().Trim() != "null")
                    whereDocNumber =  Request.QueryString["docNumber"].ToString();
                if (Request.QueryString["supplierId"] != null && Request.QueryString["supplierId"].ToString().Trim() != "" && Request.QueryString["supplierId"].ToString().Trim() != "null")
                    whereSupplier = "supplierId = '" + Request.QueryString["supplierId"].ToString() + "'";
                if (Request.QueryString["direction"] != null && Request.QueryString["direction"].ToString().Trim() != "" && Request.QueryString["direction"].ToString().Trim() != "null")
                    whereDirection = "direction = '" + Request.QueryString["direction"].ToString() + "'";

                DateTime dtStartCreateTime, dtEndCreateTime;
                if (Request.QueryString["StartTime"] != null && Request.QueryString["StartTime"].ToString().Trim() != "" && Request.QueryString["StartTime"].ToString().Trim() != "null" && DateTime.TryParse(Request.QueryString["StartTime"].ToString().Trim(), out dtStartCreateTime))
                    whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (Request.QueryString["EndTime"] != null && Request.QueryString["EndTime"].ToString().Trim() != "" && Request.QueryString["EndTime"].ToString().Trim() != "null" && DateTime.TryParse(Request.QueryString["EndTime"].ToString().Trim(), out dtEndCreateTime))
                    whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereDocNumber);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereDirection);
                //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereMaterialItem);

                YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                ynPage.SetPageSize(500); //pageRows;
                ynPage.SetCurrentPage(1); //pageNumber;

                List<AscmStoreInOut> listAscmStoreInOut = AscmStoreInOutService.GetInstance().GetReportList(whereDocNumber);
               //var  res=listAscmStoreInOut.GroupBy(x=>new(x.docNumber,x.ascmContainer.specId,x.supplierId));
                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("ContainerHistoryReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = listAscmStoreInOut;
                ReportViewer1.LocalReport.DataSources.Clear();//好像不clear也可以
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                //string companpyTitle = YnParameterService.GetInstance().GetValue(MyParameter.companpyTitle);
                string companpyTitle = "美的中央空调";
                string title = companpyTitle + "容器出入库报表打印";

                ReportParameter[] reportParameters = new ReportParameter[] {
                    new ReportParameter("ReportParameter_Title",title ),
                    new ReportParameter("ReportParameter_ReportTime","打印时间："+DateTime.Now.ToString("yyyy-MM-dd") )
                };
                ReportViewer1.LocalReport.SetParameters(reportParameters);
                ReportViewer1.LocalReport.Refresh();
                //ReportViewer1.LocalReport.Refresh();
            }
        }
        protected void ReportViewer1_PreRender(object sender, EventArgs e)
        {
            string sTitle = "容器出入库报表打印";
            this.ReportViewer1.LocalReport.DisplayName = sTitle;
        }
    }
}