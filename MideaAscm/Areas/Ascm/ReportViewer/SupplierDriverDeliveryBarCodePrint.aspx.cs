using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Services.SupplierPreparation;
using Microsoft.Reporting.WebForms;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Services.FromErp;
using MideaAscm.Services.Base;
using System.Drawing;

namespace MideaAscm.Areas.Ascm.ReportViewer
{
    public partial class SupplierDriverDeliveryBarCodePrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                YnFrame.Dal.Entities.YnUser ynUser = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketUserData();
                if (ynUser == null)
                    throw new Exception("用户错误！");
                AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(ynUser.userId);
                if (ascmUserInfo.extExpandType != "erp")
                    throw new Exception("供应商用户错误！");
                AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                if (ascmUserInfo.ascmSupplier == null)
                    throw new Exception("供应商用户错误！");

                //提取数据
                List<AscmDeliBatSumDetail> list = new List<AscmDeliBatSumDetail>();
                string ids = Request.QueryString["ids"];
                if (!string.IsNullOrEmpty(ids))
                {
                    YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                    ynPage.SetPageSize(500); //pageRows;
                    ynPage.SetCurrentPage(1); //pageNumber;

                    list = AscmDeliBatSumDetailService.GetInstance().GetList("from AscmDeliBatSumDetail where id in(" + ids + ")");

                    AscmDeliBatSumDetailService.GetInstance().SetMaterial(list);
                    AscmDeliBatSumDetailService.GetInstance().SetDeliveryOrderBatch(list);
                    List<AscmDeliveryOrderBatch> listDeliveryOrderBatch = new List<AscmDeliveryOrderBatch>();
                    list.ForEach(P => listDeliveryOrderBatch.Add(
                        new AscmDeliveryOrderBatch
                        {
                            id = P.batchId
                        }));
                    //获取货位分配
                    AscmDeliveryOrderBatchService.GetInstance().SetAssignWarelocation(listDeliveryOrderBatch);
                    list.ForEach(P => P.assignWarelocation = listDeliveryOrderBatch.Find(T => T.id == P.batchId).assignWarelocation);
                    //条码
                    foreach (AscmDeliBatSumDetail ascmDeliBatSumDetail in list)
                    {
                        System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                        if (ascmDeliBatSumDetail.ascmMaterialItem!=null&&ascmDeliBatSumDetail.ascmMaterialItem.docNumber != null)
                        {
                            #region Code128
                            MideaAscm.Code.BarCode128 barCode128 = new Code.BarCode128();
                            barCode128.TitleFont = new Font("宋体", 10);
                            barCode128.HeightImage = 50;
                            Bitmap bitmap = barCode128.GetCodeImage(ascmDeliBatSumDetail.ascmMaterialItem.docNumber, ascmDeliBatSumDetail.ascmMaterialItem.docNumber, Code.BarCode128.Encode.Code128B);
                            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Gif);
                            bitmap.Dispose();
                            #endregion

                            #region Code39
                            //YnBaseClass2.Helper.BarCode39 barCode39 = new YnBaseClass2.Helper.BarCode39();
                            //barCode39.WidthCU = 10;
                            //barCode39.WidthXI = 3;
                            //System.Drawing.Bitmap bitmap = barCode39.CreateBarCode(ascmDeliBatSumDetail.ascmMaterialItem.docNumber, ascmDeliBatSumDetail.ascmMaterialItem.docNumber, 0, 0);
                            ////生成图片  
                            ////System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                            //bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Gif);
                            //bitmap.Dispose();
                            #endregion
                        }
                        ascmDeliBatSumDetail.materialBarcode = memoryStream.ToArray();
                        
                    }
                }
                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("SupplierDriverDeliveryBarCodeReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = list;
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                string companpyTitle = "美的中央空调";
                string title = companpyTitle + "供应商送货合单条码";

                ReportParameter[] reportParameters = new ReportParameter[] {
                    new ReportParameter("ReportParameter_SupplierName",ascmUserInfo.ascmSupplier.name )
                };

                ReportViewer1.LocalReport.SetParameters(reportParameters);
                ReportViewer1.LocalReport.Refresh();
            }
        }
        protected void ReportViewer1_PreRender(object sender, EventArgs e)
        {
            string companpyTitle = "美的中央空调";
            string title = companpyTitle + "供应商送货合单条码";
            this.ReportViewer1.LocalReport.DisplayName = title;
        }
    }
}