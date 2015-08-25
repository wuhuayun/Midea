using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.IO;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.FromErp.Entities;
using Newtonsoft.Json;

namespace WinForm
{
    public partial class frmCheckTaskInfo : Office2007Form
    {
        public string GetMaterialTaskId { get; set; }
        
        public frmCheckTaskInfo()
        {
            InitializeComponent();
        }

        private void frmCheckTaskInfo_Load(object sender, EventArgs e)
        {
            List<AscmGetMaterialTask> list = GetMaterialTaskInfo();
            txtTaskId.Text = "";
            txtTime.Text = "";
            txtType.Text = "";
            txtStatus.Text = "";
            txtPosition.Text = "";
            txtWarehouse.Text = "";
            txtOther.Text = "";
            txtLine.Text = "";
            txtCategory.Text = "";
            txtNumber.Text = "";
            if (list != null && list.Count > 0)
            {
                txtTaskId.Text = list[0].taskId;
                txtTime.Text = list[0].taskTime;
                txtType.Text = list[0].IdentificationIdCN;
                txtStatus.Text = list[0]._status;
                txtPosition.Text = list[0].warehouserPlace;
                txtWarehouse.Text = list[0].warehouserId;
                txtOther.Text = list[0].tipCN;
                txtLine.Text = list[0].productLine;
                txtCategory.Text = list[0]._mtlCategoryStatus;
                txtNumber.Text = list[0].materialDocNumber;
            }
        }

        public List<AscmGetMaterialTask> GetMaterialTaskInfo()
        {
            List<AscmGetMaterialTask> list = null;
            try
            {
                string message = string.Empty;
                if (!string.IsNullOrEmpty(GetMaterialTaskId))
                {
                    WinForm.AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                    string jsonStr = Service.CheckTaskInfo(frmMain.encryptTicket, GetMaterialTaskId, ref message);
                    if (!string.IsNullOrEmpty(jsonStr))
                    {
                        list = (List<AscmGetMaterialTask>)JsonConvert.DeserializeObject(jsonStr, typeof(List<AscmGetMaterialTask>));
                        if (list == null || list.Count == 0)
                        {
                            MessageBoxEx.Show("没有查到符合该条件任务信息!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
    }
}
