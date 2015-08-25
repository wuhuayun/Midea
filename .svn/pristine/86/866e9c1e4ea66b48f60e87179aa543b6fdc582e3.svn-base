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
    public partial class WmsIncManAccDetailPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //提取数据
                int mainId = 0;

                if (Request.QueryString["mainId"] != null && Request.QueryString["mainId"].Trim() != "" && Request.QueryString["mainId"].Trim() != "null")
                    mainId = Convert.ToInt32(Request.QueryString["mainId"].Trim());

                YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                ynPage.SetPageSize(500); //pageRows;
                ynPage.SetCurrentPage(1); //pageNumber;

                List<AscmWmsIncManAccDetail> listAscmWmsIncManAccDetail = AscmWmsIncManAccDetailService.GetInstance().GetList(mainId);
                AscmWmsIncManAccMain ascmWmsIncManAccMain = AscmWmsIncManAccMainService.GetInstance().Get(mainId);
                if (ascmWmsIncManAccMain != null)
                {
                    ascmWmsIncManAccMain.ascmSupplier = AscmSupplierService.GetInstance().Get(ascmWmsIncManAccMain.supplierId);
                    ascmWmsIncManAccMain.ascmSupplierAddress = AscmSupplierAddressService.GetInstance().Get(ascmWmsIncManAccMain.supplierAddressId);
                }

                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("WmsIncManAccDetailReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = listAscmWmsIncManAccDetail;
                ReportViewer1.LocalReport.DataSources.Clear();//好像不clear也可以
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                string companpyTitle = "美的中央空调";
                string title = companpyTitle + "送货单明细";

                ReportParameter[] reportParameters = new ReportParameter[] {
                    new ReportParameter("ReportParameter_Title", title),
                    new ReportParameter("ReportParameter_ReportTime", "打印时间：" + DateTime.Now.ToString("yyyy-MM-dd")),
                    new ReportParameter("ReportParameter_DocNumber", "送货单号：" + ascmWmsIncManAccMain.docNumber),
                    new ReportParameter("ReportParameter_ResponsiblePerson", "责任人：" + ascmWmsIncManAccMain.responsiblePerson),
                    new ReportParameter("ReportParameter_CreateTimeShow", "生成时间：" + ascmWmsIncManAccMain.createTimeShow),
                    new ReportParameter("ReportParameter_SupplierDocNumber", "供方编码：" + ascmWmsIncManAccMain.supplierDocNumber),
                    new ReportParameter("ReportParameter_SupplierName", "供方名称：" + ascmWmsIncManAccMain.supplierName),
                    new ReportParameter("ReportParameter_WarehouseId", "收货仓库：" + ascmWmsIncManAccMain.warehouseId),
                    new ReportParameter("ReportParameter_SupperWarehouse", "供应子库：" + ascmWmsIncManAccMain.supperWarehouse),
                    new ReportParameter("ReportParameter_SupperPlateNumber", "运输车牌：" + ascmWmsIncManAccMain.supperPlateNumber),
                    new ReportParameter("ReportParameter_SupperTelephone", "联系电话：" + ascmWmsIncManAccMain.supperTelephone),
                    new ReportParameter("ReportParameter_Memo", "备注：" + ascmWmsIncManAccMain.memo),
                };
                ReportViewer1.LocalReport.SetParameters(reportParameters);
                ReportViewer1.LocalReport.Refresh();
            }
        }
        protected void ReportViewer1_PreRender(object sender, EventArgs e)
        {
            string companpyTitle = "美的中央空调";
            string title = companpyTitle + "送货单明细";
            this.ReportViewer1.LocalReport.DisplayName = title;
        }

    }

}