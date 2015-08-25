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
    public partial class WmsStockTransDetailPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //提取数据
                int mainId = 0;
                //string queryWord = "";
                string whereOther = "";

                if (Request.QueryString["mainId"] != null && Request.QueryString["mainId"].Trim() != "" && Request.QueryString["mainId"].Trim() != "null")
                    mainId = Convert.ToInt32(Request.QueryString["mainId"].Trim());
                //queryWord = Request.QueryString["mainId"].Trim(); 



                YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                ynPage.SetPageSize(500); //pageRows;
                ynPage.SetCurrentPage(1); //pageNumber;

                List<AscmWmsStockTransDetail> listAscmWmsStockTransDetail = AscmWmsStockTransDetailService.GetInstance().GetList(ynPage, "", "", mainId, "", whereOther);
                AscmWmsStockTransMain ascmWmsStockTransMain = AscmWmsStockTransMainService.GetInstance().Get(mainId);


                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("WmsStockTransDetailReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = listAscmWmsStockTransDetail;
                ReportViewer1.LocalReport.DataSources.Clear();//好像不clear也可以
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                string companpyTitle = "美的中央空调";
                string title = companpyTitle + "子库存转移明细";

                ReportParameter[] reportParameters = new ReportParameter[] {
                    new ReportParameter("ReportParameter_Title", title),
                    new ReportParameter("ReportParameter_ReportTime", "打印时间：" + DateTime.Now.ToString("yyyy-MM-dd")),
                    new ReportParameter("ReportParameter_DocNumber", "单据号：" + ascmWmsStockTransMain.docNumber),
                    new ReportParameter("ReportParameter_ManualDocNumber", "手工单号：" + ascmWmsStockTransMain.manualDocNumber),
                    new ReportParameter("ReportParameter_ResponsiblePerson", "责任人：" + ascmWmsStockTransMain.responsiblePerson),
                    new ReportParameter("ReportParameter_FromWarehouseId", "来源仓库：" + ascmWmsStockTransMain.fromWarehouseId),
                    new ReportParameter("ReportParameter_ToWarehouseId", "目标仓库：" + ascmWmsStockTransMain.toWarehouseId),
                    new ReportParameter("ReportParameter_Memo", "备注：" + ascmWmsStockTransMain.memo),
                };
                ReportViewer1.LocalReport.SetParameters(reportParameters);
                ReportViewer1.LocalReport.Refresh();
            }
        }
        protected void ReportViewer1_PreRender(object sender, EventArgs e)
        {
            string companpyTitle = "美的中央空调";
            string title = companpyTitle + "子库存转移明细";
            this.ReportViewer1.LocalReport.DisplayName = title;
        }

    }

}