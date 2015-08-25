using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Services.Warehouse;
using Microsoft.Reporting.WebForms;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Services.FromErp;

namespace MideaAscm.Areas.Ascm.ReportViewer
{
    public partial class WmsJobMtlReturnDetialPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //提取数据
                int mainId = 0;
                string whereOther = "";

                if (Request.QueryString["mainId"] != null && Request.QueryString["mainId"].Trim() != "" && Request.QueryString["mainId"].Trim() != "null")
                    mainId = Convert.ToInt32(Request.QueryString["mainId"].Trim());



                YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                ynPage.SetPageSize(500); //pageRows;
                ynPage.SetCurrentPage(1); //pageNumber;

                List<AscmWmsMtlReturnDetail> listAscmWmsMtlReturnDetail = AscmWmsMtlReturnDetailService.GetInstance().GetList(ynPage, "", "", mainId, "", whereOther);
                AscmWmsMtlReturnMain ascmWmsMtlReturnMain = AscmWmsMtlReturnMainService.GetInstance().Get(mainId);
                if(ascmWmsMtlReturnMain!=null)
                {
                    ascmWmsMtlReturnMain.ascmWipEntities = AscmWipEntitiesService.GetInstance().Get(ascmWmsMtlReturnMain.wipEntityId);
                    ascmWmsMtlReturnMain.ascmMtlTransactionReasons = AscmMtlTransactionReasonsService.GetInstance().Get(ascmWmsMtlReturnMain.reasonId);
                }


                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("WmsJobMtlReturnDetialPrintReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = listAscmWmsMtlReturnDetail;
                ReportViewer1.LocalReport.DataSources.Clear();//好像不clear也可以
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                string companpyTitle = "中央空调顺德工厂";
                string title = companpyTitle + "作业退料单";

                ReportParameter[] reportParameters = new ReportParameter[] {
                    new ReportParameter("ReportParameter_Title", title),
                    new ReportParameter("ReportParameter_ReportTime", "打印时间：" + DateTime.Now.ToString("yyyy-MM-dd")),
                    //new ReportParameter("ReportParameter_DocNumber", "单据号：" + ascmWmsMtlReturnMain.docNumber),
                    new ReportParameter("ReportParameter_ManualDocNumber", "作业号：" + ascmWmsMtlReturnMain.wipEntityName),
                    //new ReportParameter("ReportParameter_WarehouseId", "仓库：" + ascmWmsMtlReturnMain.warehouseId),
                    //new ReportParameter("ReportParameter_ReturnAreaCn", "退料区域：" + ascmWmsMtlReturnMain.returnAreaCn),
                    //new ReportParameter("ReportParameter_reasonName", "退料原因：" + ascmWmsMtlReturnMain.reasonName),
                    //new ReportParameter("ReportParameter_Memo", "备注：" + ascmWmsMtlReturnMain.memo),
                    new ReportParameter("ReportParameter_Memo", "打印人：" + ascmWmsMtlReturnMain.createUser)
                };
                ReportViewer1.LocalReport.SetParameters(reportParameters);
                ReportViewer1.LocalReport.Refresh();
            }
        }
        protected void ReportViewer1_PreRender(object sender, EventArgs e)
        {
            string companpyTitle = "美的中央空调";
            string title = companpyTitle + "作业退料明细";
            this.ReportViewer1.LocalReport.DisplayName = title;
        }
    }
}