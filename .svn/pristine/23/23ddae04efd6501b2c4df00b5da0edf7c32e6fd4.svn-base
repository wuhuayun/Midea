using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Warehouse.Entities;

namespace MideaAscm.Dal.Warehouse.Entities
{
    /// <summary>发料校验</summary>
    public class AscmWmsStoreIssueCheck
    {
        /// <summary>领料员</summary>
        public string workerId { get; set; }
        /// <summary>应发容器数</summary>
        public decimal shouldContainerNum { get; set; }
        /// <summary>实发容器数</summary>
        public decimal realContainerNum { get; set; }
        /// <summary>应发物料数</summary>
        public decimal shouldMaterialNum { get; set; }
        /// <summary>实发物料数</summary>
        public decimal realMaterialNum { get; set; }
        /// <summary>校验时间</summary>
        public string checkTime { get; set; }
        /// <summary>领料次数</summary>
        public int times { get; set; }
        /// <summary>目的地</summary>
        public string destination { get; set; }
        /// <summary>目标产线</summary>
        public string productionLine { get; set; }
        /// <summary>状态</summary>
        public IssueStatus status { get; set; }
        /// <summary>待领容器数</summary>
        public decimal waitContainerNum { get { return shouldContainerNum - realContainerNum; } }
    }
    public enum IssueStatus
    {
        /// <summary>正在出仓</summary>
        outingOfWarehouse = 1,
        /// <summary>已出仓</summary>
        outedOfWarehouse = 2,
        /// <summary>已备料</summary>
        prepared = 3
    }

    /// <summary>需求备料监控平台</summary>
    public class AscmWmsWipRequireLedMonitor
    {
        /// <summary>物料编码</summary>
        public string materialDocNumber { get; set; }
        /// <summary>需求数量</summary>
        public decimal netQuantity { get; set; }
        /// <summary>发料数量</summary>
        public decimal issuedQuantity { get; set; }
        /// <summary>领料员</summary>
        public string workerId { get; set; }
        /// <summary>作业产线</summary>
        public string productionLine { get; set; }
    }
}
