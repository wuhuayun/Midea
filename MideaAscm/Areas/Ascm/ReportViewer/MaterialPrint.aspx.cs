using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Services.Base;

namespace MideaAscm.Areas.Ascm.ReportViewer
{
    public partial class MaterialPrint : System.Web.UI.Page
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
                //if (ascmUserInfo.extExpandType != "erp")
                //    throw new Exception("供应商用户错误！");
                //AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                //if (ascmUserInfo.ascmSupplier == null)
                //    throw new Exception("供应商用户错误！");

                //提取数据
                int id = 0;

                List<AscmMaterialItem> list = new List<AscmMaterialItem>();
                if (!string.IsNullOrEmpty(Request.QueryString["id"]) && int.TryParse(Request.QueryString["id"], out id))
                {
                    AscmMaterialItem ascmMaterialItem = AscmMaterialItemService.GetInstance().Get(id);
                    if (ascmMaterialItem != null)
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
                        list.Add(ascmMaterialItem);
                    }
                }

                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("MaterialReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = list;
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