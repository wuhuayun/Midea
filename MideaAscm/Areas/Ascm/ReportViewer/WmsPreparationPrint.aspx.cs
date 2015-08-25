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
    public partial class WmsPreparationPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<AscmWmsPreparationMain> listAscmWmsPreparationMain = null;
                List<AscmWmsPreparationDetail> listAscmWmsPreparationDetail = null;
                List<AscmWmsPreparationDetail> listDetail = new List<AscmWmsPreparationDetail>(); ;
                AscmWmsPreparationMain ascmWmsPreparationMain = null;
                List<AscmWipDiscreteJobs> listAscmWipDiscreteJobs = null;
                string wipSupplyType = "";
                string billNoStart = "", billNoEnd = "";
                string materialDocNumberStart = "", materialDocNumberEnd = "";
                string warehouseIdStart = "", warehouseIdEnd = "";
                string scheduledStartDateStart = "", scheduledStartDateEnd = "";
                string jobScheduleGroupsStart = "", jobScheduleGroupsEnd = "";
                string jobProductionLineStart = "", jobProductionLineEnd = "";
                if (!string.IsNullOrEmpty(Request.QueryString["docNumber"]))
                {
                    string docNumber = Request.QueryString["docNumber"];
                    if (!string.IsNullOrEmpty(docNumber))
                    {
                        listAscmWmsPreparationMain = AscmWmsPreparationMainService.GetInstance().GetList(" from AscmWmsPreparationMain where docNumber='" + docNumber + "'");
                        if (listAscmWmsPreparationMain != null && listAscmWmsPreparationMain.Count() > 0)
                        {
                            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                            ynPage.SetPageSize(500); //pageRows;
                            ynPage.SetCurrentPage(1); //pageNumber;

                            ascmWmsPreparationMain = listAscmWmsPreparationMain[0];
                            //listAscmWmsPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetSumList(null,"", "", ascmWmsPreparationMain.id, "", "");
                            //listAscmWmsPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetList(ynPage,"","",ascmWmsPreparationMain.id,"","");
                            listAscmWmsPreparationDetail = AscmWmsPreparationDetailService.GetInstance().GetList(null, "", "", ascmWmsPreparationMain.id, "", "");
                            AscmWmsPreparationDetailService.GetInstance().SetWipDiscreteJobs(listAscmWmsPreparationDetail);
                            List<AscmWarelocation> listAscmWarelocation = AscmWarelocationService.GetInstance().GetList("from AscmWarelocation");
                            if (listAscmWmsPreparationDetail != null && listAscmWmsPreparationDetail.Count() > 0)
                            {
                                foreach (AscmWmsPreparationDetail ascmWmsPreparationDetail in listAscmWmsPreparationDetail)
                                {
                                    var find = listAscmWarelocation.Find(item => item.id == ascmWmsPreparationDetail.warelocationId);
                                    if (find != null)
                                        ascmWmsPreparationDetail.locationDocNumber = find.docNumber;
                                }
                                billNoStart = listAscmWmsPreparationDetail.OrderBy(item => item.wipEntityName).First().wipEntityName;
                                billNoEnd = listAscmWmsPreparationDetail.OrderBy(item => item.wipEntityName).Last().wipEntityName;
                                materialDocNumberStart = listAscmWmsPreparationDetail.OrderBy(item => item.materialDocNumber).First().materialDocNumber;
                                materialDocNumberEnd = listAscmWmsPreparationDetail.OrderBy(item => item.materialDocNumber).Last().materialDocNumber;
                                warehouseIdStart = listAscmWmsPreparationDetail.OrderBy(item => item.warehouseId).First().warehouseId;
                                warehouseIdEnd = listAscmWmsPreparationDetail.OrderBy(item => item.warehouseId).Last().warehouseId;
                                //wipSupplyType = listAscmWmsPreparationDetail[0].ascmMaterialItem.wipSupplyTypeCn;
                                jobScheduleGroupsStart = listAscmWmsPreparationDetail.OrderBy(item => item.jobScheduleGroupsName).First().jobScheduleGroupsName;
                                jobScheduleGroupsEnd = listAscmWmsPreparationDetail.OrderBy(item => item.jobScheduleGroupsName).Last().jobScheduleGroupsName;
                                jobProductionLineStart = listAscmWmsPreparationDetail.OrderBy(item => item.jobProductionLine).First().jobProductionLine;
                                jobProductionLineEnd = listAscmWmsPreparationDetail.OrderBy(item => item.jobProductionLine).Last().jobProductionLine;
                                //string billIds = "";
                                var vbillIds = listAscmWmsPreparationDetail.Select(item => item.wipEntityId).Distinct();
                                if (vbillIds != null)
                                {
                                    string billIds = string.Empty;
                                    foreach (int billId in vbillIds)
                                    {
                                        if (!string.IsNullOrEmpty(billIds))
                                            billIds += ",";
                                        billIds += "'" + billId + "'";
                                    }
                                    if (!string.IsNullOrEmpty(billIds))
                                    {
                                        string whereOther = " id in (" + billIds + ")";
                                        listAscmWipDiscreteJobs = AscmWipDiscreteJobsService.GetInstance().GetList(null,"","","",whereOther);
                                        scheduledStartDateStart = listAscmWipDiscreteJobs.OrderBy(item => item._scheduledStartDate).First()._scheduledStartDate;
                                        scheduledStartDateEnd = listAscmWipDiscreteJobs.OrderBy(item => item._scheduledStartDate).Last()._scheduledStartDate;
                                    }
                                }
                                #region 合计 同种物料在一个货位，进行数量合计，同种物料在不同货位，进行分开显示
                                var groupByMaterial = listAscmWmsPreparationDetail.GroupBy(p => p.materialId);
                                foreach (IGrouping<int, AscmWmsPreparationDetail> ig in groupByMaterial)
                                {
                                    //判断此物料的有多少条数据
                                    List<AscmWmsPreparationDetail> list = listAscmWmsPreparationDetail.Where(item => item.materialId == ig.Key).ToList();
                                    if (list != null && list.Count() > 1)
                                    {
                                        var groupByWarelocation = list.GroupBy(p => p.warelocationId);
                                        foreach (IGrouping<int, AscmWmsPreparationDetail> igrouping in groupByWarelocation)
                                        {
                                            AscmWmsPreparationDetail ascmWmsPreparationDetail_ByWarelocation = igrouping.First();
                                            AscmWmsPreparationDetail ascmWmsPreparationDetail = new AscmWmsPreparationDetail();
                                            ascmWmsPreparationDetail.materialId = igrouping.Key;
                                            //ascmWmsPreparationDetail.ascmMaterialItem = ascmWmsPreparationDetail_ByWarelocation.ascmMaterialItem;
                                            ascmWmsPreparationDetail.warehouseId = ascmWmsPreparationDetail_ByWarelocation.warehouseId;
                                            ascmWmsPreparationDetail.locationDocNumber = ascmWmsPreparationDetail_ByWarelocation.locationDocNumber;
                                            ascmWmsPreparationDetail.wipSupplyType = ascmWmsPreparationDetail_ByWarelocation.wipSupplyType;
                                            ascmWmsPreparationDetail.planQuantity = igrouping.Sum(P => P.planQuantity);
                                            //ascmWmsPreparationDetail.quantity = igrouping.Sum(P => P.quantity);
                                            ascmWmsPreparationDetail.issueQuantity = igrouping.Sum(P => P.issueQuantity);
                                            ascmWmsPreparationDetail.containerBindNumber = igrouping.Sum(P => P.containerBindNumber);
                                            listDetail.Add(ascmWmsPreparationDetail);
                                        }
                                    }
                                    else
                                    {
                                        AscmWmsPreparationDetail _ascmWmsPreparationDetail = ig.First();
                                        AscmWmsPreparationDetail ascmWmsPreparationDetail = new AscmWmsPreparationDetail();
                                        ascmWmsPreparationDetail.materialId = ig.Key;
                                        //ascmWmsPreparationDetail.ascmMaterialItem = _ascmWmsPreparationDetail.ascmMaterialItem;
                                        ascmWmsPreparationDetail.warehouseId = _ascmWmsPreparationDetail.warehouseId;
                                        ascmWmsPreparationDetail.locationDocNumber = _ascmWmsPreparationDetail.locationDocNumber;
                                        ascmWmsPreparationDetail.wipSupplyType = _ascmWmsPreparationDetail.wipSupplyType;
                                        ascmWmsPreparationDetail.planQuantity = ig.Sum(P => P.planQuantity);
                                        //ascmWmsPreparationDetail.quantity = ig.Sum(P => P.quantity);
                                        ascmWmsPreparationDetail.issueQuantity = ig.Sum(P => P.issueQuantity);
                                        ascmWmsPreparationDetail.containerBindNumber = ig.Sum(P => P.containerBindNumber);
                                        listDetail.Add(ascmWmsPreparationDetail);
                                    }
                                }
                                #endregion
                            }
                            #region Code128
                            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                            MideaAscm.Code.BarCode128 barCode128 = new Code.BarCode128();
                            barCode128.TitleFont = new System.Drawing.Font("宋体", 10);
                            barCode128.HeightImage = 50;
                            System.Drawing.Bitmap bitmap = barCode128.GetCodeImage(docNumber, docNumber, Code.BarCode128.Encode.Code128B);
                            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Gif);
                            if (listDetail != null && listDetail.Count() > 0)
                            {
                                listDetail = listDetail.OrderBy(item => item.materialDocNumber).ToList();
                                listDetail[0].barcodeShow = memoryStream.ToArray();
                            }
                            bitmap.Dispose();
                            #endregion

                            #region 条码 Code39
                            //System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                            //YnBaseClass2.Helper.BarCode39 barCode39 = new YnBaseClass2.Helper.BarCode39();
                            //barCode39.WidthCU = 10;
                            //barCode39.WidthXI = 3;
                            //System.Drawing.Bitmap bitmap = barCode39.CreateBarCode(docNumber, docNumber, 0, 0);
                            //bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Gif);
                            //bitmap.Dispose();
                            //if (listDetail != null && listDetail.Count()>0)
                            //    listDetail[0].barcodeShow = memoryStream.ToArray();
                            #endregion
                        }
                    }
                }
                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("WmsPreparationReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = listDetail;
                ReportViewer1.LocalReport.DataSources.Clear();//好像不clear也可以
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                string title = "中央空调顺德工厂需求备料单";
                //string secondTitle = "车间任务物料需求报表";
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;

                ReportParameter[] reportParameters = new ReportParameter[] {
                    new ReportParameter("ReportParameter_Title", title),
                    //new ReportParameter("ReportParameter_secondTitle", secondTitle),
                    new ReportParameter("ReportParameter_ReportTime", "打印时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    new ReportParameter("ReportParameter_wipSupplyType", "供应类型：" + wipSupplyType),
                    new ReportParameter("ReportParameter_scheduledStartDate", "计划时间从：" +scheduledStartDateStart +" 至 "+scheduledStartDateEnd),
                    new ReportParameter("ReportParameter_BillNo", "任务从：" +billNoStart +" 至 "+billNoEnd),
                    new ReportParameter("ReportParameter_materialDocNumber", "组件从：" +materialDocNumberStart +" 至 "+materialDocNumberEnd),
                    new ReportParameter("ReportParameter_warehouseId", "子库从：" +warehouseIdStart +" 至 "+warehouseIdEnd),
                    new ReportParameter("ReportParameter_jobScheduleGroups", "计划组从：" +jobScheduleGroupsStart +" 至 "+jobScheduleGroupsEnd),
                    new ReportParameter("ReportParameter_jobProductionLine", "生产线从：" +jobProductionLineStart +" 至 "+jobProductionLineEnd),
                    new ReportParameter("ReportParameter_Printer", "打印人：" + userName),
                };
                ReportViewer1.LocalReport.SetParameters(reportParameters);
                ReportViewer1.LocalReport.Refresh();
            }
        }
        protected void ReportViewer1_PreRender(object sender, EventArgs e)
        {
            this.ReportViewer1.LocalReport.DisplayName = "中央空调顺德工厂需求备料单" + DateTime.Now.ToString("yyyyMMddHHmmss"); 
        }
    }
}