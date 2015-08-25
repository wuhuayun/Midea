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

namespace WinForm.Task
{
    public partial class EditTask : Office2007Form
    {
        public delegate void RefreshHandler(object sender, EventArgs args);
        public event RefreshHandler refreshHandler = null;//委托事件 触发主页列表刷新
        public AscmGetMaterialTask editTask = null;

        public EditTask(AscmGetMaterialTask task)
        {
            editTask = task;
            InitializeComponent();
            InitializeComponentValue();
            InitializeTaskModel();
        }
        private void InitializeTaskModel()
        {
            if (editTask == null)
                return;
            txtProductLine.Text = editTask.productLine;
            cmbWarehouserId.Text = editTask.warehouserId;
            cmbMtlCategoryStatus.SelectedValue = editTask.mtlCategoryStatus;
            cmbRankerId.Text = editTask.rankerId;
            cmbIdentificationId.SelectedValue = editTask.IdentificationId.ToString();
            cmbMaterialDocNumber.Text = editTask.materialDocNumber;
            cmbTaskTime.SelectedValue = editTask.taskTime;
            cmbTip.SelectedValue = editTask.tip;
            cmbWorkerId.Text = editTask.workerId;
        }

        private void InitializeComponentValue()
        {
            List<KeyValuePair<object, string>> listMtlCategoryStatus = new List<KeyValuePair<object, string>>();
            listMtlCategoryStatus.Add(new KeyValuePair<object, string>("有库存", "INSTOCK"));
            listMtlCategoryStatus.Add(new KeyValuePair<object, string>("须备料", "PRESTOCK"));
            listMtlCategoryStatus.Add(new KeyValuePair<object, string>("须配料", "MIXSTOCK"));

            cmbMtlCategoryStatus.DataSource = listMtlCategoryStatus;
            cmbMtlCategoryStatus.DisplayMember = "Key";
            cmbMtlCategoryStatus.ValueMember = "Value";

            List<KeyValuePair<object, string>> listIdentificationId = new List<KeyValuePair<object, string>>();
            listIdentificationId.Add(new KeyValuePair<object, string>("总装", "1"));
            listIdentificationId.Add(new KeyValuePair<object, string>("电装", "2"));

            cmbIdentificationId.DataSource = listIdentificationId;
            cmbIdentificationId.DisplayMember = "Key";
            cmbIdentificationId.ValueMember = "Value";

            List<KeyValuePair<object, string>> listTaskTime = new List<KeyValuePair<object, string>>();
            listTaskTime.Add(new KeyValuePair<object, string>("上午", "上午"));
            listTaskTime.Add(new KeyValuePair<object, string>("下午", "下午"));

            cmbTaskTime.DataSource = listTaskTime;
            cmbTaskTime.DisplayMember = "Key";
            cmbTaskTime.ValueMember = "Value";

            List<KeyValuePair<object, string>> listTip = new List<KeyValuePair<object, string>>();
            listTip.Add(new KeyValuePair<object, string>("附件", "FUJIAN"));
            listTip.Add(new KeyValuePair<object, string>("散件", "SANJIAN"));
            listTip.Add(new KeyValuePair<object, string>("两器", "LIANGQI"));
            listTip.Add(new KeyValuePair<object, string>("铜管", "TONGGUAN"));
            listTip.Add(new KeyValuePair<object, string>("铝箔", "LVBO"));
            listTip.Add(new KeyValuePair<object, string>("配管", "PEIGUAN"));
            listTip.Add(new KeyValuePair<object, string>("其他", "QITA"));
            cmbTip.DataSource = listTip;
            cmbTip.DisplayMember = "Key";
            cmbTip.ValueMember = "Value";
        }

        private void btnEditTask_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            try
            {
                AscmGetMaterialTask task = (editTask == null) ? (new AscmGetMaterialTask()) : editTask;
                task.productLine = txtProductLine.Text;
                task.warehouserId = (cmbWarehouserId.SelectedItem == null) ? cmbWarehouserId.Text : ((KeyValuePair<object, string>)cmbWarehouserId.SelectedItem).Value;
                task.mtlCategoryStatus = (cmbMtlCategoryStatus.SelectedItem == null) ? cmbMtlCategoryStatus.Text : ((KeyValuePair<object, string>)cmbMtlCategoryStatus.SelectedItem).Value;
                task.rankerId = (cmbRankerId.SelectedItem == null) ? cmbRankerId.Text : ((KeyValuePair<object, string>)cmbRankerId.SelectedItem).Value;
                task.IdentificationId = (cmbIdentificationId.SelectedItem == null) ? 0 : int.Parse(((KeyValuePair<object, string>)cmbIdentificationId.SelectedItem).Value);
                task.materialDocNumber = (cmbMaterialDocNumber.SelectedItem == null) ? cmbMaterialDocNumber.Text : ((KeyValuePair<object, string>)cmbMaterialDocNumber.SelectedItem).Value;
                task.taskTime = (cmbTaskTime.SelectedItem == null) ? cmbTaskTime.Text : ((KeyValuePair<object, string>)cmbTaskTime.SelectedItem).Value;
                task.tip = (cmbTip.SelectedItem == null) ? cmbTip.Text : ((KeyValuePair<object, string>)cmbTip.SelectedItem).Value;
                task.workerId = cmbWorkerId.Text;
                task.modifyUser = frmMain.userName;
                string xmlstr = YnBaseClass2.Helper.ObjectHelper.Serialize(task);

                WinForm.AscmWebService.AscmWebService service = new AscmWebService.AscmWebService();
                if (service.EditTaskSave(frmMain.encryptTicket, xmlstr, ref message))
                {
                    if (refreshHandler != null)
                    {
                        refreshHandler(this, new EventArgs());
                    }
                    MessageBoxEx.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBoxEx.Show(message, "修改失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "修改失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
