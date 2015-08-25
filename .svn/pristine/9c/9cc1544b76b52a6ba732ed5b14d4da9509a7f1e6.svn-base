using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MideaAscm.Dal.GetMaterialManage.Entities;
using DevComponents.DotNetBar;
using MideaAscm.Dal.Base.Entities;
using Newtonsoft.Json;
using DevComponents.DotNetBar.Controls;

namespace MideaAscm.Pad
{
    public partial class frmAddTaskForm : DevComponents.DotNetBar.RibbonForm
    {
        public AscmGetMaterialTask ascmGetMaterialTask;
        
        public frmAddTaskForm()
        {
            InitializeComponent();
        }

        private void frmAddTaskForm_Load(object sender, EventArgs e)
        {
            BindDataToolbarControls();
        }

        #region Bind Data Controls
        private void BindDataToolbarControls()
        {
            //Job's content
            if (cbTipCn.Items.Count > 0)
                cbTipCn.Items.Clear();

            cbTipCn.Items.Add("");
            List<string> listTipCn = AscmAllocateRule.OtherDefine.GetList();
            foreach (string item in listTipCn)
            {
                cbTipCn.Items.Add(AscmAllocateRule.OtherDefine.DisplayText(item));
            }

            if (cbTaskTime.Items.Count > 0)
                cbTaskTime.Items.Clear();

            cbTaskTime.Items.Add("");
            cbTaskTime.Items.Add("上午");
            cbTaskTime.Items.Add("下午");


            //Material Category Status
            if (cbMtlCategoryStatus.Items.Count > 0)
                cbMtlCategoryStatus.Items.Clear();

            cbMtlCategoryStatus.Items.Add("");
            List<string> listMtlCategoryStatus = MtlCategoryStatusDefine.GetList();
            foreach (string item in listMtlCategoryStatus)
            {
                cbMtlCategoryStatus.Items.Add(MtlCategoryStatusDefine.DisplayText(item));
            }

            //Task Type
            if (cbTaskType.Items.Count > 0)
                cbTaskType.Items.Clear();

            cbTaskType.Items.Add("");
            List<int> listTaskType = AscmDiscreteJobs.IdentificationIdDefine.GetList();
            foreach (int item in listTaskType)
            {
                cbTaskType.Items.Add(AscmDiscreteJobs.IdentificationIdDefine.DisplayText(item));
            }
        }
        #endregion

        #region Struct ComboBox And Bind Data
        private void StructComboBoxMaterial(ComboBoxEx comboBox, List<AscmMaterialItem> list)
        {
            comboBox.DataSource = list;
            comboBox.DropDownColumns = "docNumber,description";
            comboBox.DropDownColumnsHeaders = "物料编码\r\n描述";
            comboBox.DropDownHeight = 200;
            comboBox.DropDownWidth = 230;

            if (comboBox.SelectedItem != null)
            {
                comboBox.SelectedIndex = 0;
                comboBox.Text = ((AscmMaterialItem)comboBox.SelectedItem).docNumber;
            }
        }

        private void StructComboBoxMark(ComboBoxEx comboBox, List<AscmMarkTaskLog> list)
        {
            comboBox.DataSource = list;
            comboBox.DropDownColumns = "id,ascmWipEntitiesName,ascmGetMaterialTaskId,warehouseId";
            comboBox.DropDownColumnsHeaders = "编号\r\n作业号\r\n任务号\r\n子库";
            comboBox.DropDownHeight = 200;
            comboBox.DropDownWidth = 300;

            if (comboBox.SelectedItem != null)
            {
                comboBox.SelectedIndex = 0;
                comboBox.Text = ((AscmMarkTaskLog)comboBox.SelectedItem).id.ToString();
            }
        }

        private void StructComboBoxWarehouse(ComboBoxEx comboBox, List<AscmWarehouse> list)
        {
            comboBox.DataSource = list;
            comboBox.DropDownColumns = "id,description";
            comboBox.DropDownColumnsHeaders = "名称\r\n描述";
            comboBox.DropDownHeight = 200;
            comboBox.DropDownWidth = 230;

            if (comboBox.SelectedItem != null)
            {
                comboBox.SelectedIndex = 0;
                comboBox.Text = ((AscmWarehouse)comboBox.SelectedItem).id;
            }
        }

        private void ComboBoxEx_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEx comboBox = sender as ComboBoxEx;
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                if (comboBox.SelectedItem as AscmMaterialItem != null)
                {
                    comboBox.Text = ((AscmMaterialItem)comboBox.SelectedItem).docNumber;
                }
                else if (comboBox.SelectedItem as AscmMarkTaskLog != null)
                {
                    comboBox.Text = ((AscmMarkTaskLog)comboBox.SelectedItem).id.ToString();
                }
                else if (comboBox.SelectedItem as AscmWarehouse != null)
                {
                    comboBox.Text = ((AscmWarehouse)comboBox.SelectedItem).id;
                }
            }
        }
        #endregion

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cbWarehouse.Text) || string.IsNullOrEmpty(cbTipCn.SelectedItem.ToString()))
                {
                    MessageBoxEx.Show("信息填写不完整：请填写仓库及作业内容！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string queryType = string.Empty;
                if (cbTaskType.SelectedItem != null)
                {
                    switch (cbTaskType.SelectedItem.ToString())
                    {
                        case "总装":
                            queryType = "1";
                            break;
                        case "电装":
                            queryType = "2";
                            break;
                        default:
                            queryType = "0";
                            break;
                    }
                }
                else
                {
                    queryType = "0";
                }

                string queryFormat = string.Empty;
                if (cbMtlCategoryStatus.SelectedItem != null)
                {
                    switch (cbMtlCategoryStatus.SelectedItem.ToString())
                    {
                        case "有库存":
                            queryFormat = "INSTOCK";
                            break;
                        case "须备料":
                            queryFormat = "PRESTOCK";
                            break;
                        case "须配料":
                            queryFormat = "MIXSTOCK";
                            break;
                        default:
                            queryFormat = "";
                            break;
                    }
                }

                string queryTip = string.Empty;
                if (cbTipCn.SelectedItem != null)
                {
                    switch (cbTipCn.SelectedItem.ToString())
                    {
                        case "附件":
                            queryTip = "FUJIAN";
                            break;
                        case "散件":
                            queryTip = "SANJIAN";
                            break;
                        case "两器":
                            queryTip = "LIANGQI";
                            break;
                        case "铜管":
                            queryTip = "TONGGUAN";
                            break;
                        case "铝箔":
                            queryTip = "LVBO";
                            break;
                        case "配管":
                            queryTip = "PEIGUAN";
                            break;
                        case "其他":
                            queryTip = "QITA";
                            break;
                        default:
                            queryTip = "";
                            break;
                    }
                }
                string queryTime = (cbTaskTime.SelectedItem == null) ? null : cbTaskTime.Text;
                string queryMaterial = (cbMaterial.SelectedItem == null) ? null : cbMaterial.Text;
                string queryMark = (cbRelated.SelectedItem == null) ? null : cbRelated.Text;

                AscmGetMaterialTask taskModel = new AscmGetMaterialTask();
                taskModel.warehouserId = cbWarehouse.Text.Trim().ToUpper();
                taskModel.tip = queryTip;
                taskModel.productLine = tbProductLine.Text;
                taskModel.mtlCategoryStatus = queryFormat;
                taskModel.taskTime = queryTime;
                taskModel.IdentificationId = int.Parse(queryType);
                taskModel.materialDocNumber = queryMaterial;
                taskModel.relatedMark = queryMark;

                ascmGetMaterialTask = taskModel;

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnLinkMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";

                AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                string jsonString = Service.GetSomeMaterialList(frmMainView.encryptTicket, cbMaterial.Text, ref message);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    List<AscmMaterialItem> list = (List<AscmMaterialItem>)JsonConvert.DeserializeObject(jsonString, typeof(List<AscmMaterialItem>));
                    if (list != null && list.Count > 0)
                    {
                        StructComboBoxMaterial(this.cbMaterial, list);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw ex;
            }
        }

        private void btnLinkMark_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";

                AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                string jsonString = Service.GetSomeRelatedMarkList(frmMainView.encryptTicket, ref message);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    List<AscmMarkTaskLog> list = (List<AscmMarkTaskLog>)JsonConvert.DeserializeObject(jsonString, typeof(List<AscmMarkTaskLog>));
                    if (list != null && list.Count > 0)
                    {
                        StructComboBoxMark(this.cbRelated, list);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw ex;
            }
        }

        private void btnLinkWarehouse_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "", queryWord = "";
                if (!string.IsNullOrEmpty(cbWarehouse.Text))
                    queryWord = cbWarehouse.Text.Trim().ToUpper();

                AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                string jsonString = Service.GetSomeWareHouse(frmMainView.encryptTicket, queryWord, ref message);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    List<AscmWarehouse> list = (List<AscmWarehouse>)JsonConvert.DeserializeObject(jsonString, typeof(List<AscmWarehouse>));
                    if (list != null && list.Count > 0)
                    {
                        StructComboBoxWarehouse(this.cbWarehouse, list);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw ex;
            }
        }

        private void cbRelated_DataColumnCreated(object sender, DataColumnEventArgs e)
        {
            DevComponents.AdvTree.ColumnHeader header = e.ColumnHeader;
            if (header.DataFieldName == "id")
            {
                header.Width.Relative = 20;
            }
            else if (header.DataFieldName == "ascmGetMaterialTaskId")
            {
                header.Width.Relative = 20;
            }
            else if (header.DataFieldName == "ascmWipEntitiesName")
            {
                header.Width.Relative = 40;
            }
            else
            {
                header.Width.Relative = 20;
            }
        }
    }
}
