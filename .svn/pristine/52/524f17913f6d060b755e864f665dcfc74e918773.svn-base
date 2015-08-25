using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Services.SupplierPreparation;
using Microsoft.Reporting.WebForms;
using MideaAscm.Services.Base;
using MideaAscm.Services.FromErp;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Areas.Ascm.ReportViewer
{
    public partial class SupplierDriverDeliveryDetialBarCodePrint : System.Web.UI.Page
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

                //打印的数据
                List<AscmDeliBatSumDetail> sumList = new List<AscmDeliBatSumDetail>();
                //最后一个装箱
                AscmDeliBatSumDetail lastDetail = new AscmDeliBatSumDetail();
                string ids = Request.QueryString["ids"];
                string unitIds = Request.QueryString["unitIds"];
                string[] listId = ids.Split(',');
                string[] listUnitId = unitIds.Split(',');
                if (!string.IsNullOrEmpty(ids))
                {
                    List<AscmDeliBatSumDetail> list = new List<AscmDeliBatSumDetail>();
                    for (int i = 0; i < listId.Count(); i++)
                    {
                        AscmDeliBatSumDetail ascmDeliBatSumDetail = AscmDeliBatSumDetailService.GetInstance().Get(Int32.Parse(listId[i]));
                        AscmContainerUnitQuantity ascmContainerUnitQuantity = AscmContainerUnitQuantityService.GetInstance().Get(Int32.Parse(listUnitId[i]));
                        ascmDeliBatSumDetail.container = ascmContainerUnitQuantity.container;
                        ascmDeliBatSumDetail.materialUnitQuantity = ascmContainerUnitQuantity.unitQuantity;
                        ascmDeliBatSumDetail.containerUnitQuantityId = ascmContainerUnitQuantity.id;
                        list.Add(ascmDeliBatSumDetail);
                    }
                   
                    if (list != null)
                    {
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
                        foreach(AscmDeliBatSumDetail ascmDeliBatSumDetail in list)
                        {
                            //条码
                            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                            if (ascmDeliBatSumDetail.ascmMaterialItem.docNumber != null)
                            {
                                #region Code128
                                MideaAscm.Code.BarCode128 barCode128 = new Code.BarCode128();
                                barCode128.TitleFont = new System.Drawing.Font("宋体", 10);
                                barCode128.HeightImage = 50;
                                System.Drawing.Bitmap bitmap = barCode128.GetCodeImage(ascmDeliBatSumDetail.batchBarCode, ascmDeliBatSumDetail.batchBarCode, Code.BarCode128.Encode.Code128B);
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

                            decimal totalNumber = ascmDeliBatSumDetail.totalNumber;//总数量
                            decimal unitQuantity = ascmDeliBatSumDetail.materialUnitQuantity;//单元数
                            //decimal unitQuantity = 10;
                            decimal count = decimal.Truncate(totalNumber / unitQuantity);//箱数量>=0
                            decimal rest = totalNumber % unitQuantity;//剩余数量>=0

                            //设置装箱数 2015.3.26

                            //总数 < 单元数 ，仅有一个箱子,总数 = 剩余数量
                            if (count == 0)
                            {
                                count = 1;
                            }
                            else
                            {
                                //有剩余数量，多出一箱
                                if (rest != 0) count = count + 1;
                            }
                            for (int i = 0; i < count; i++)
                            { 
                                //最后一个为剩余数量
                                if (i == count - 1 && rest != 0)
                                {
                                    lastDetail.ascmDeliveryOrderBatch = ascmDeliBatSumDetail.ascmDeliveryOrderBatch;
                                    lastDetail.ascmMaterialItem = ascmDeliBatSumDetail.ascmMaterialItem;
                                    lastDetail.assignWarelocation = ascmDeliBatSumDetail.assignWarelocation;
                                    lastDetail.materialUnitQuantity = rest;
                                    sumList.Add(lastDetail);
                                }
                                else
                                {
                                    sumList.Add(ascmDeliBatSumDetail);
                                }
                            }
                        }
                    }
                   
                }

                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("SupplierDriverDeliveryDetialBarCodeReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = sumList;
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                string companpyTitle = "美的中央空调";
                string title = companpyTitle + "供应商送货批单条码";

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
            string title = companpyTitle + "供应商送货批单条码";
            this.ReportViewer1.LocalReport.DisplayName = title;
        }
    }
}