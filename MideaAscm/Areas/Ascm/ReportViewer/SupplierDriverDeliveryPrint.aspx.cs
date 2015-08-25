using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Services.SupplierPreparation;
using MideaAscm.Services.Base;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Areas.Ascm.ReportViewer
{
    public partial class SupplierDriverDeliveryPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //提取数据
                int id = 0;
                string containerSpAndQuantity = "";
                //出货子库
                string supplyWarehouse = "";
                //送货地点
                string deliveryPlace = "";
                AscmDeliBatSumMain ascmDeliBatSumMain = new AscmDeliBatSumMain();
                List<AscmDeliBatSumDetail> listAscmDeliBatSumDetail = new List<AscmDeliBatSumDetail>();
                if (!string.IsNullOrEmpty(Request.QueryString["id"]) && int.TryParse(Request.QueryString["id"], out id))
                {
                    //ascmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().Get(id);
                    //ascmDeliBatSumMain.ascmSupplier = AscmSupplierService.GetInstance().Get(ascmDeliBatSumMain.supplierId);
                    //ascmDeliBatSumMain.ascmDriver = AscmDriverService.GetInstance().Get(ascmDeliBatSumMain.driverId);
                    //供方简称
                    string supplierShortName = "select vendorSiteCode from AscmSupplierAddress a where a.vendorSiteCodeAlt=s.docNumber and rownum=1";
                    //司机编号
                    string driverSn = "select sn from AscmDriver d where d.id=h.driverId";
                    string hql = string.Format("select new AscmDeliBatSumMain(h,s.docNumber,({0}),({1})) from AscmDeliBatSumMain h,AscmSupplier s", supplierShortName, driverSn);
                    string where = string.Empty;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "h.supplierId=s.id");
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "h.id=" + id);
                    hql += " where " + where;
                    ascmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().Get(hql);

                    //List<AscmDeliBatSumDetail> list = AscmDeliBatSumDetailService.GetInstance().GetListByMainId(id);
                    //容器绑定数量
                    string containerBindNumber = "select sum(quantity) from AscmContainerDelivery where deliveryOrderBatchId=l.batchId and batSumMainId=l.mainId";
                    //容器数量
                    string containerNumber = "select count(distinct containerSn) from AscmContainerDelivery where deliveryOrderBatchId=l.batchId and batSumMainId=l.mainId";
                    hql = string.Format("select new AscmDeliBatSumDetail(l,({0}),0M,0M,({1})) from AscmDeliBatSumDetail l", containerBindNumber, containerNumber);
                    where = string.Empty;
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, " mainId=" + id);
                    hql += " where " + where;
                    List<AscmDeliBatSumDetail> list = AscmDeliBatSumDetailService.GetInstance().GetList(hql);
                    AscmDeliBatSumDetailService.GetInstance().SetDeliveryOrderBatch(list, true);
                    AscmDeliBatSumDetailService.GetInstance().SetMaterial(list);

                    string warehouseId = Request.QueryString["warehouseId"];
                    string materialCode = Request.QueryString["materialCode"];

                    if (list != null && list.Count > 0)
                    {
                        //设置作业
                        AscmDeliBatSumDetailService.GetInstance().SetWipEntityName(list);
                        supplyWarehouse = list.Where(P => !string.IsNullOrEmpty(P.batchSupperWarehouse)).FirstOrDefault().batchSupperWarehouse;
                        deliveryPlace = list.Where(P=>!string.IsNullOrEmpty(P.batchWipLine)).FirstOrDefault().batchWipLine;

                        string containerSns = "";
                        bool isWarehouseId = true, isMaterialCode = true;
                        List<AscmContainerDelivery> listAscmContainerDelivery = AscmContainerDeliveryService.GetInstance().GetListByDeliverySumMainId(id, false);
                        List<AscmContainerDelivery> _listAscmContainerDelivery = new List<AscmContainerDelivery>();
                        foreach (AscmDeliBatSumDetail detail in list)
                        {
                            isWarehouseId = isMaterialCode = true;
                            if (warehouseId != null && warehouseId.Trim() != "")
                                isWarehouseId = detail.batchWarehouseId == warehouseId.Trim();
                            if (materialCode != null && materialCode.Trim() != "")
                                isMaterialCode = !string.IsNullOrEmpty(detail.materialDocNumber) && detail.materialDocNumber.Substring(0, 4) == materialCode.Trim();
                            if (isWarehouseId && isMaterialCode)
                            {
                                listAscmDeliBatSumDetail.Add(detail);
                                if (listAscmContainerDelivery != null)
                                {
                                    foreach (AscmContainerDelivery delivery in listAscmContainerDelivery)
                                    {
                                        if (delivery.deliveryOrderBatchId == detail.batchId && delivery.materialId == detail.materialId)
                                        {
                                            if (containerSns != "")
                                                containerSns += ",";
                                            containerSns += "'" + delivery.containerSn + "'";
                                            _listAscmContainerDelivery.Add(delivery);
                                        }
                                    }
                                }
                            }
                        }
                        if (containerSns != "")
                        {
                            List<AscmContainer> listAscmContainer = AscmContainerService.GetInstance().GetList("from AscmContainer where sn in(" + containerSns + ")", false, true);
                            var groupByAscmContainerSpec = listAscmContainer.GroupBy(P => P.spec);
                            foreach (IGrouping<string, AscmContainer> igSpec in groupByAscmContainerSpec)
                            {
                                int specCount = 0;
                                foreach (AscmContainer ascmContainer in igSpec)
                                {
                                    specCount += _listAscmContainerDelivery.Where(p => p.containerSn == ascmContainer.sn).Select(item => item.containerSn).Distinct().Count();
                                }
                                if (containerSpAndQuantity != "")
                                    containerSpAndQuantity += " ";
                                containerSpAndQuantity +=
                                    string.Format("{0}:{1}", igSpec.Key, specCount);
                            }
                        }

                        int i = 0;
                        listAscmDeliBatSumDetail = listAscmDeliBatSumDetail.OrderBy(P => P.batchBarCode).ToList();
                        foreach (AscmDeliBatSumDetail detail in listAscmDeliBatSumDetail)
                        {
                            detail.barcode = ascmDeliBatSumMain.docNumber + (Math.Floor(i / 7M) + 1);
                            detail.barcodeShow = Code.BarCode128.GetBarcode(detail.barcode);
                            i++;
                        }
                    }
                }

                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("SupplierDriverDeliveryReport.rdlc");
                ReportDataSource rds1 = new ReportDataSource();
                rds1.Name = "DataSet1";
                rds1.Value = listAscmDeliBatSumDetail;
                ReportViewer1.LocalReport.DataSources.Clear();//好像不clear也可以
                ReportViewer1.LocalReport.DataSources.Add(rds1);

                string companpyTitle = "美的中央空调";
                string title = companpyTitle + "供应商送货合单";

                ReportParameter[] reportParameters = new ReportParameter[] {
                    new ReportParameter("ReportParameter_Title", title),
                    new ReportParameter("ReportParameter_ReportTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm")),
                    new ReportParameter("ReportParameter_SupplierName", ascmDeliBatSumMain.supplierNameCn),
                    new ReportParameter("ReportParameter_SupplierDocNumber", ascmDeliBatSumMain.supplierDocNumberCn),
                    new ReportParameter("ReportParameter_DocNumber", ascmDeliBatSumMain.docNumber),
                    new ReportParameter("ReportParameter_DriverSn", ascmDeliBatSumMain.driverSnCn),
                    new ReportParameter("ReportParameter_DriverPlateNumber", ascmDeliBatSumMain.driverPlateNumber),
                    new ReportParameter("ReportParameter_DeliveryPlace", deliveryPlace),
                    new ReportParameter("ReportParameter_AppointmentStartTime", ascmDeliBatSumMain.appointmentStartTimeShow),
                    new ReportParameter("ReportParameter_AppointmentEndTime", ascmDeliBatSumMain.appointmentEndTimeShow),
                    new ReportParameter("ReportParameter_Status", ascmDeliBatSumMain.statusCn),
                    new ReportParameter("ReportParameter_Warehouse", ascmDeliBatSumMain.warehouseId),
                    new ReportParameter("ReportParameter_SupplyWarehouse", supplyWarehouse),
                    new ReportParameter("ReportParameter_ContainerSpAndQuantity", containerSpAndQuantity)
                };
                ReportViewer1.LocalReport.SetParameters(reportParameters);
                ReportViewer1.LocalReport.Refresh();
            }
        }
        protected void ReportViewer1_PreRender(object sender, EventArgs e)
        {
            string companpyTitle = "美的中央空调";
            string title = companpyTitle + "供应商送货合单";
            this.ReportViewer1.LocalReport.DisplayName = title;
        }
    }
}