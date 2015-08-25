using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MideaAscm.Dal.SupplierPreparation.Entities
{
    ///<summary>容器单元数</summary>
    public class AscmContainerUnitQuantity
    {
        ///<summary>ID</summary>
        public virtual int id { get; set; }
        ///<summary>供应商ID</summary>
        public virtual int supplierId { get; set; }
        ///<summary>物料编码</summary>
        public virtual string materialDocNumber { get; set; }
        ///<summary>容器</summary>
        public virtual string container { get; set; }
        ///<summary>单元数</summary>
        public virtual decimal unitQuantity { get; set; }
        ///<summary>最后更新人</summary>
        public virtual string modifyUser { get; set; }
        ///<summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }

        //辅助信息
        ///<summary>容器规格</summary>
        public string containerCn { get; set; }
        ///<summary>容器单元数</summary>
        public string containerUnitQuantity 
        {
            get { return container + "  " + unitQuantity; }
        }
    }
}
