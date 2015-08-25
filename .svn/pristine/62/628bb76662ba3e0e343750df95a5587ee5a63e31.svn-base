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
using MideaAscm.Services.Base;

namespace MideaAscm.Areas.Ascm.ReportViewer
{
    public partial class WmsBackInvoiceDetailPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //提取数据
                int mainId= 0;
                
                string whereOther = "";

                if (Request.QueryString["mainId"] != null && Request.QueryString["mainId"].Trim() != "" && Request.QueryString["mainId"].Trim() != "null")
                    mainId = Convert.ToInt32(Request.QueryString["mainId"].Trim());

               

                YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                ynPage.SetPageSize(500); //pageRows;
                ynPage.SetCurrentPage(1); //pageNumber;

                List<AscmWmsBackInvoiceDetail> listAscmWmsBackInvoiceDetail = AscmWmsBackInvoiceDetailService.GetInstance().GetList(ynPage, "", "", mainId, "", whereOther);
                AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain = AscmWmsBackInvoiceMainService.GetInstance().Get(mainId);
                if (ascmWmsBackInvoiceMain != null)
                {
                    ascmWmsBackInvoiceMain.ascmSupplier = AscmSupplierService.GetInstance().Get(ascmWmsBackInvoiceMain.supplierId);
                }
                

                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("WmsBackInvoiceDetailReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = listAscmWmsBackInvoiceDetail;
                ReportViewer1.LocalReport.DataSources.Clear();//好像不clear也可以
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                string companpyTitle = "美的中央空调";
                string title = companpyTitle + "供应商退货明细";

                ReportParameter[] reportParameters = new ReportParameter[] {
                    new ReportParameter("ReportParameter_Title", title),
                    new ReportParameter("ReportParameter_ReportTime", "打印时间：" + DateTime.Now.ToString("yyyy-MM-dd")),
                    new ReportParameter("ReportParameter_DocNumber", "单据号：" + ascmWmsBackInvoiceMain.docNumber),
                    new ReportParameter("ReportParameter_ManualDocNumber", "手工单号：" + ascmWmsBackInvoiceMain.manualDocNumber),
                    new ReportParameter("ReportParameter_ResponsiblePerson", "责任人：" + ascmWmsBackInvoiceMain.responsiblePerson),
                    new ReportParameter("ReportParameter_WarehouseId", "默认仓库：" + ascmWmsBackInvoiceMain.warehouseId),
                    new ReportParameter("ReportParameter_StatusCn", "状态：" + ascmWmsBackInvoiceMain.statusCn),
                    new ReportParameter("ReportParameter_ReasonName", "退货原因：" + ascmWmsBackInvoiceMain.reasonName),
                    new ReportParameter("ReportParameter_SupplierDocNumber", "供方编码：" + ascmWmsBackInvoiceMain.supplierDocNumber),
                    new ReportParameter("ReportParameter_SupplierName", "供方名称：" + ascmWmsBackInvoiceMain.supplierName),
                    new ReportParameter("ReportParameter_Memo", "备注：" + ascmWmsBackInvoiceMain.memo),
                };
                ReportViewer1.LocalReport.SetParameters(reportParameters);
                ReportViewer1.LocalReport.Refresh();
            }
        }
        protected void ReportViewer1_PreRender(object sender, EventArgs e)
        {
            string companpyTitle = "美的中央空调";
            string title = companpyTitle + "供应商退货明细";
            this.ReportViewer1.LocalReport.DisplayName = title;
        }
    }
}