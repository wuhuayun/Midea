using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Services.SupplierPreparation;
using MideaAscm.Services.Base;
using Microsoft.Reporting.WebForms;

namespace MideaAscm.Areas.Ascm.ReportViewer
{
    public partial class SupplierMaterialPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                YnFrame.Dal.Entities.YnUser ynUser = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicketUserData();
                if (ynUser == null)
                    throw new Exception("用户错误！");
                AscmUserInfo ascmUserInfo = AscmUserInfoService.GetInstance().Get(ynUser.userId);
                string supplierName = "";
                if (ascmUserInfo.extExpandType == "erp")
                {
                    AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                    if (ascmUserInfo.ascmSupplier != null)
                        supplierName = ascmUserInfo.ascmSupplier.name;

                }

                //提取数据
                List<AscmMaterialItem> sumList = new List<AscmMaterialItem>();
                //最后一个装箱
                AscmMaterialItem lastDetail = new AscmMaterialItem();
                string ids = Request.QueryString["ids"];
                string unitQuantitys = Request.QueryString["unitQuantitys"];
                string totalNumbers = Request.QueryString["totalNumbers"];
                bool isUnitPrint =Boolean.Parse(Request.QueryString["isUnitPrint"]);

                string[] listId = ids.Split(',');
                string[] listUnitQuantity = unitQuantitys.Split(',');
                string[] listTotalNumber = totalNumbers.Split(',');

                List<AscmMaterialItem> list = new List<AscmMaterialItem>();
                for (int i = 0; i < listId.Count(); i++)
                {
                    AscmMaterialItem ascmMaterialItem = AscmMaterialItemService.GetInstance().Get(Int32.Parse(listId[i]));
                    AscmContainerUnitQuantity ascmContainerUnitQuantity = AscmContainerUnitQuantityService.GetInstance().Get(Int32.Parse(listUnitQuantity[i]));
                    ascmMaterialItem.container = ascmContainerUnitQuantity.container;
                    ascmMaterialItem.unitQuantity = ascmContainerUnitQuantity.unitQuantity;
                    ascmMaterialItem.containerUnitQuantityId = ascmContainerUnitQuantity.id;
                    ascmMaterialItem.totalNumber = Decimal.Parse(listTotalNumber[i]);
                    list.Add(ascmMaterialItem);
                }
                if (!string.IsNullOrEmpty(ids))
                {
                    if (list != null && list.Count() > 0)
                    {
                        foreach (AscmMaterialItem ascmMaterialItem in list)
                        {
                            //条码
                            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                            if (ascmMaterialItem.docNumber != null)
                            {
                                #region Code128
                                MideaAscm.Code.BarCode128 barCode128 = new Code.BarCode128();
                                barCode128.TitleFont = new System.Drawing.Font("宋体", 10);
                                barCode128.HeightImage = 50;
                                System.Drawing.Bitmap bitmap = barCode128.GetCodeImage(ascmMaterialItem.docNumber, ascmMaterialItem.docNumber, Code.BarCode128.Encode.Code128B);
                                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Gif);
                                bitmap.Dispose();
                                #endregion
                            }
                            ascmMaterialItem.materialBarcode = memoryStream.ToArray();
                            //如果是总数打印，单元数=总数
                            if (!isUnitPrint)
                                ascmMaterialItem.unitQuantity = ascmMaterialItem.totalNumber;

                            decimal totalNumber = ascmMaterialItem.totalNumber;//总数量
                            decimal unitQuantity = ascmMaterialItem.unitQuantity;//单元数
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
                                    lastDetail.materialBarcode = ascmMaterialItem.materialBarcode;
                                    lastDetail.docNumber = ascmMaterialItem.docNumber;
                                    lastDetail.description = ascmMaterialItem.description;
                                    lastDetail.unit = ascmMaterialItem.unit;
                                    lastDetail.unitQuantity = rest;
                                    sumList.Add(lastDetail);
                                }
                                else
                                {
                                    sumList.Add(ascmMaterialItem);
                                }
                            }
                        }
                    }
                }

                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("SupplierMaterialReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = sumList;
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                string companpyTitle = "美的中央空调";
                string title = companpyTitle + "供应商送货物料条码";

                ReportParameter[] reportParameters = new ReportParameter[] {
                    new ReportParameter("ReportParameter_SupplierName",supplierName )
                };

                ReportViewer1.LocalReport.SetParameters(reportParameters);
                ReportViewer1.LocalReport.Refresh();
            }
        }
        protected void ReportViewer1_PreRender(object sender, EventArgs e)
        {
            string companpyTitle = "美的中央空调";
            string title = companpyTitle + "供应商送货物料条码";
            this.ReportViewer1.LocalReport.DisplayName = title;
        }
    }
}