using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>最新的送货合单</summary>
    public class CurrentDeliBatSum
    {
        /// <summary>供应商名称</summary>
        public string supplierName { get; set; }
        /// <summary>合单号</summary>
        public string deliBatSumNo { get; set; }
        /// <summary>合单明细</summary>
        public List<CurrentDeliBatSumList> list { get; set; }
        /// <summary>合单容器数量</summary>
        public long containerNumber { get; set; }
        /// <summary>接收容器数量</summary>
        public long inWarehouseContainerNumber { get; set; }
        /// <summary>合单入厂时间</summary>
        public string toPlantTime { get; set; }
    }
    /// <summary>送货合单明细</summary>
    public class CurrentDeliBatSumList
    {
        /// <summary>批单号</summary>
        public string batchDocNumber { get; set; }
        /// <summary>物料编码</summary>
        public string materialDocNumber { get; set; }
        /// <summary>物料名称</summary>
        public string materialName { get; set; }
        /// <summary>合单数量</summary>
        public decimal totalNumber { get; set; }
        /// <summary>接收数量</summary>
        public decimal receiveNumber { get; set; }
    }

    /// <summary>供应商送货合单入库校验</summary>
    public class DeliBatSumCheckIn
    {
        /// <summary>供应商简称</summary>
        public string supplierShortName { get; set; }
        /// <summary>合单号</summary>
        public string docNumber { get; set; }
        /// <summary>校验时间</summary>
        public string checkTime { get; set; }
        /// <summary>入库容器校验列表</summary>
        public List<ContainerCheckIn> containerCheckIns { get; set; }
        /// <summary>货位</summary>
        public List<string> warelocations { get; set; }
    }
    /// <summary>入库容器校验</summary>
    public class ContainerCheckIn
    {
        /// <summary>容器规格</summary>
        public string spec { get; set; }
        /// <summary>容器数</summary>
        public decimal quantity { get; set; }
        /// <summary>校验数</summary>
        public decimal checkQuantity { get; set; }
    }

    /// <summary>
    /// 预约到货
    /// 过滤已接收和已入厂的合单
    /// </summary>
    public class AppointmentArrivalOfGoods
    {
        /// <summary>合单Id</summary>
        public int id { get; set; }
        /// <summary>合单号</summary>
        public string docNumber { get; set; }
        /// <summary>供应商简称</summary>
        public string supplierShortName { get; set; }
        /// <summary>物料大类</summary>
        public string category { get; set; }
        ///<summary>预约开始时间</summary>
        public string appointmentStartTime { get; set; }
        ///<summary>预约最后时间</summary>
        public string appointmentEndTime { get; set; }
        /// <summary>合单状态</summary>
        public string status { get; set; }
        /// <summary>核查结果</summary>
        public CheckResultType checkResult { get; set; } 
    }
    public enum CheckResultType
    {
        /// <summary>
        /// 未到
        /// 今日内应到的，但当前时间还未到预约时间的送货合单，显示默认颜色(无色)
        /// </summary>
        unArrive = 1,
        /// <summary>
        /// 到货提醒
        /// 当前时间到了预约时间段内的送货合单，显示黄色
        /// </summary>
        arrived = 2,
        /// <summary>
        /// 超时未到
        /// 超过预约截至时间未到，显示红色
        /// </summary>
        overtime = 3
    }
}
