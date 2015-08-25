using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MideaAscm.Services.Warehouse;
using MideaAscm.Dal.Warehouse;
using MideaAscm.Dal.Warehouse.Entities;
using Microsoft.Reporting.WebForms;

namespace MideaAscm.Areas.Ascm.ReportViewer
{
    public partial class WmsIncManAccMainPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //提取数据
                string queryWord = ""; string supplierDoc = ""; string whereOther = "", whereStartModifyTime = "", whereEndModifyTime = "";
                DateTime dtStartPlanTime, dtEndPlanTime;


                if (Request.QueryString["docNumber"] != null && Request.QueryString["docNumber"].Trim() != "" && Request.QueryString["docNumber"].Trim() != "null")
                    queryWord = Request.QueryString["search"].Trim();
                if (Request.QueryString["supplierDoc"] != null && Request.QueryString["supplierDoc"].Trim() != "" && Request.QueryString["supplierDoc"].Trim() != "null")
                    supplierDoc = Request.QueryString["supplierDoc"].Trim();
                if (!string.IsNullOrEmpty(Request.QueryString["queryStartModifyTime"]) && DateTime.TryParse(Request.QueryString["queryStartModifyTime"].Trim(), out dtStartPlanTime))
                    whereStartModifyTime = "modifyTime>='" + dtStartPlanTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(Request.QueryString["queryEndModifyTime"]) && DateTime.TryParse(Request.QueryString["queryEndModifyTime"].Trim(), out dtEndPlanTime))
                    whereEndModifyTime = "modifyTime<'" + dtEndPlanTime.ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartModifyTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndModifyTime);

                YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                ynPage.SetPageSize(500); //pageRows;
                ynPage.SetCurrentPage(1); //pageNumber;

                List<AscmWmsIncManAccMain> listAscmIncManAccMain = AscmWmsIncManAccMainService.GetInstance().GetList(ynPage, "id", "asc", queryWord,whereOther);
                if (!string.IsNullOrEmpty(supplierDoc))
                    listAscmIncManAccMain=listAscmIncManAccMain.Where(item => item.ascmSupplier.docNumber == supplierDoc).OrderBy(item => item.modifyTime).ToList();

                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("WmsIncManAccMainReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = listAscmIncManAccMain;
                ReportViewer1.LocalReport.DataSources.Clear();//好像不clear也可以
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                string companpyTitle = "美的中央空调";
                string title = companpyTitle + "手工单";

                ReportParameter[] reportParameters = new ReportParameter[] {
                    new ReportParameter("ReportParameter_Title", title),
                    new ReportParameter("ReportParameter_ReportTime", "打印时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
           
                };
                ReportViewer1.LocalReport.SetParameters(reportParameters);
                ReportViewer1.LocalReport.Refresh();
            }
        }
        protected void ReportViewer1_PreRender(object sender, EventArgs e)
        {
            string companpyTitle = "美的中央空调";
            string title = companpyTitle + "手工单";
            this.ReportViewer1.LocalReport.DisplayName = title;
        }

    }

}