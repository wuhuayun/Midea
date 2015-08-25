using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MideaAscm.Services.Warehouse;
using MideaAscm.Dal.Warehouse.Entities;
using Microsoft.Reporting.WebForms;

namespace MideaAscm.Areas.Ascm.ReportViewer
{
    public partial class WmsJobMtlReturnPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //提取数据
                string queryWord = ""; string wipEntityName = ""; string whereOther = "", whereStartModifyTime = "", whereEndModifyTime = "", whereWipEntityId="",whereBillType="";
                DateTime dtStartPlanTime, dtEndPlanTime;


                if (Request.QueryString["docNumber"] != null && Request.QueryString["docNumber"].Trim() != "" && Request.QueryString["docNumber"].Trim() != "null")
                    queryWord = Request.QueryString["docNumber"].Trim();
                if (Request.QueryString["wipEntityName"] != null && Request.QueryString["wipEntityName"].Trim() != "" && Request.QueryString["wipEntityName"].Trim() != "null")
                {
                    wipEntityName = Request.QueryString["wipEntityName"].Trim();
                    whereWipEntityId = " wipEntityId in(select wipEntityId from AscmWipEntities where name like %'" + wipEntityName + "%')";
                }
                if (!string.IsNullOrEmpty(Request.QueryString["queryStartModifyTime"]) && DateTime.TryParse(Request.QueryString["queryStartModifyTime"].Trim(), out dtStartPlanTime))
                    whereStartModifyTime = "modifyTime>='" + dtStartPlanTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                if (!string.IsNullOrEmpty(Request.QueryString["queryEndModifyTime"]) && DateTime.TryParse(Request.QueryString["queryEndModifyTime"].Trim(), out dtEndPlanTime))
                    whereEndModifyTime = "modifyTime<'" + dtEndPlanTime.ToString("yyyy-MM-dd 00:00:00") + "'";
                
                whereBillType = "billType='WIPENTITY'";

                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereBillType);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereWipEntityId);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereStartModifyTime);
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereEndModifyTime);

                YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                ynPage.SetPageSize(500); //pageRows;
                ynPage.SetCurrentPage(1); //pageNumber;

                List<AscmWmsMtlReturnMain> listAscmWmsMtlReturnMain = AscmWmsMtlReturnMainService.GetInstance().GetList(ynPage, "modifyTime", "asc", queryWord, whereOther);

                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("WmsJobMtlReturnPrintReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = listAscmWmsMtlReturnMain;
                ReportViewer1.LocalReport.DataSources.Clear();//好像不clear也可以
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                string companpyTitle = "美的中央空调";
                string title = companpyTitle + "作业退料";

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
            string title = companpyTitle + "作业退料";
            this.ReportViewer1.LocalReport.DisplayName = title;
        }
    }
}