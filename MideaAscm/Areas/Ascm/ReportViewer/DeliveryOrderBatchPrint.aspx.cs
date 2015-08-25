using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Services.FromErp;

namespace MideaAscm.Areas.Ascm.ReportViewer
{
    public partial class DeliveryOrderBatchPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //提取数据
                string queryWord = "";
                string whereOther = "", whereSupplier = "", whereWarehouse = "";
                string whereStartCreateTime = "", whereEndCreateTime = "", whereStatus = "";

                if (Request.QueryString["queryWord"] != null && Request.QueryString["queryWord"].ToString().Trim() != "" && Request.QueryString["queryWord"].ToString().Trim() != "null")
                    queryWord =  Request.QueryString["queryWord"].ToString();
                if (Request.QueryString["supplier"] != null && Request.QueryString["supplier"].ToString().Trim() != "" && Request.QueryString["supplier"].ToString().Trim() != "null")
                    whereSupplier = "supplierId = '" + Request.QueryString["supplier"].ToString() + "'";
                if (Request.QueryString["warehouse"] != null && Request.QueryString["warehouse"].ToString().Trim() != "" && Request.QueryString["warehouse"].ToString().Trim() != "null")
                    whereWarehouse = "warehouseId = '" + Request.QueryString["warehouse"].ToString() + "'";
                if (Request.QueryString["status"] != null && Request.QueryString["status"].ToString().Trim() != "" && Request.QueryString["status"].ToString().Trim() != "null")
                    whereStatus = "status = '" + Request.QueryString["status"].ToString()+ "'";

                DateTime dtStartCreateTime, dtEndCreateTime;
                if (Request.QueryString["startCreateTime"] != null && Request.QueryString["startCreateTime"].ToString().Trim() != "" && Request.QueryString["startCreateTime"].ToString().Trim() != "null" && DateTime.TryParse(Request.QueryString["startCreateTime"].ToString().Trim(), out dtStartCreateTime))
                    whereStartCreateTime = "createTime>='" + dtStartCreateTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (Request.QueryString["endCreateTime"] != null && Request.QueryString["endCreateTime"].ToString().Trim() != "" && Request.QueryString["endCreateTime"].ToString().Trim() != "null" && DateTime.TryParse(Request.QueryString["endCreateTime"].ToString().Trim(), out dtEndCreateTime))
                    whereEndCreateTime = "createTime<'" + dtEndCreateTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereSupplier);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWarehouse);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndCreateTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStatus);
                //whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereMaterialItem);

                YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                ynPage.SetPageSize(500); //pageRows;
                ynPage.SetCurrentPage( 1); //pageNumber;

                List<AscmDeliveryOrderBatch> listAscmDeliveryOrderBatch = AscmDeliveryOrderBatchService.GetInstance().GetList(ynPage, "", "", queryWord, whereOther);
                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("DeliveryOrderBatchReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = listAscmDeliveryOrderBatch;
                ReportViewer1.LocalReport.DataSources.Clear();//好像不clear也可以
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                //string companpyTitle = YnParameterService.GetInstance().GetValue(MyParameter.companpyTitle);
                string companpyTitle = "美的中央空调";
                string title = companpyTitle + "批送货单统计报表";

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
            string companpyTitle = "美的中央空调";
            string title = companpyTitle + "批送货单统计报表";
            this.ReportViewer1.LocalReport.DisplayName = title;
        }
    }
}