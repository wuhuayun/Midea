using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Pad
{
    public partial class frmViewTaskDetailsDialog : DevComponents.DotNetBar.RibbonForm
    {
        public AscmGetMaterialTask taskDetails { get; set; }
        
        public frmViewTaskDetailsDialog()
        {
            InitializeComponent();
        }

        private void frmViewTaskDetailsDialog_Load(object sender, EventArgs e)
        {
            DataBindTaskDetails();
        }

        private void DataBindTaskDetails()
        {
            if (taskDetails != null)
            {
                this.tbTaskId.Text = taskDetails.taskIdCn;
                this.tbAscmRanker_name.Text = taskDetails.ascmRanker_name;
                this.tbEndTime.Text = taskDetails._endTime;
                this.tbIdentificationId.Text = taskDetails.IdentificationIdCN;
                this.tbMaterialDocNumber.Text = taskDetails.materialDocNumber;
                this.tbMtlCategoryStatus.Text = taskDetails._mtlCategoryStatus;
                this.tbProductLine.Text = taskDetails.productLine;
                this.tbStarTime.Text = taskDetails._starTime;
                this.tbStatus.Text = taskDetails._status;
                this.tbTaskTime.Text = taskDetails.taskTime;
                this.tbTipCn.Text = taskDetails.tipCN;
                this.tbUploadDate.Text = taskDetails.uploadDate;
                this.tbWarehouserId.Text = taskDetails.warehouserId;
                this.tbWhich.Text = taskDetails.which.ToString();
                this.tbWarehousePlace.Text = taskDetails.warehouserPlace;

                if (!string.IsNullOrEmpty(taskDetails.relatedMark))
                    cbRelatedMark.Checked = true;
            }
        }
    }
}
