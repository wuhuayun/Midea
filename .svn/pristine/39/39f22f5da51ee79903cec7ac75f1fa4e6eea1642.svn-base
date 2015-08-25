using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Pad.AscmWebService;
using Newtonsoft.Json;

namespace MideaAscm.Pad
{
    public partial class frmMaterialListDialog : DevComponents.DotNetBar.RibbonForm
    {
        public int taskId { get; set; }
        public int jobId { get; set; }
        
        public frmMaterialListDialog()
        {
            InitializeComponent();
        }

        private void frmMaterialListDialog_Load(object sender, EventArgs e)
        {
            StructureDataGridViewColumns(this.dgViewMaterialList);

            pagerControlMaterial.Bind();
        }

        private void StructureDataGridViewColumns(DataGridViewX dataGridViewX)
        {
            if (dataGridViewX.Columns.Count > 0)
                dataGridViewX.Columns.Clear();

            DataGridViewTextBoxColumn id = new DataGridViewTextBoxColumn();
            id.HeaderText = "ID";
            id.Name = "id";
            id.DataPropertyName = "id";
            id.Width = 0;
            id.Visible = false;

            DataGridViewTextBoxColumn docNumber = new DataGridViewTextBoxColumn();
            docNumber.HeaderText = "组件";
            docNumber.Name = "docNumber";
            docNumber.DataPropertyName = "docNumber";
            docNumber.Width = 110;


            DataGridViewTextBoxColumn mpsDateRequiredStr = new DataGridViewTextBoxColumn();
            mpsDateRequiredStr.HeaderText = "需求日期";
            mpsDateRequiredStr.Name = "mpsDateRequiredStr";
            mpsDateRequiredStr.DataPropertyName = "mpsDateRequiredStr";
            mpsDateRequiredStr.Width = 90;

            DataGridViewTextBoxColumn description = new DataGridViewTextBoxColumn();
            description.HeaderText = "组件说明";
            description.Name = "description";
            description.DataPropertyName = "description";
            description.Width = 200;

            DataGridViewTextBoxColumn requiredQuantity = new DataGridViewTextBoxColumn();
            requiredQuantity.HeaderText = "需求数";
            requiredQuantity.Name = "requiredQuantity";
            requiredQuantity.DataPropertyName = "requiredQuantity";
            requiredQuantity.Width = 70;

            DataGridViewTextBoxColumn transactionQuantity = new DataGridViewTextBoxColumn();
            transactionQuantity.HeaderText = "现有数";
            transactionQuantity.Name = "transactionQuantity";
            transactionQuantity.DataPropertyName = "transactionQuantity";
            transactionQuantity.Width = 70;

            DataGridViewTextBoxColumn wmsPreparationQuantity = new DataGridViewTextBoxColumn();
            wmsPreparationQuantity.HeaderText = "备料数";
            wmsPreparationQuantity.Name = "wmsPreparationQuantity";
            wmsPreparationQuantity.DataPropertyName = "wmsPreparationQuantity";
            wmsPreparationQuantity.Width = 70;

            DataGridViewTextBoxColumn getMaterialQuantity = new DataGridViewTextBoxColumn();
            getMaterialQuantity.HeaderText = "领料数";
            getMaterialQuantity.Name = "getMaterialQuantity";
            getMaterialQuantity.DataPropertyName = "getMaterialQuantity";
            getMaterialQuantity.Width = 70;

            DataGridViewTextBoxColumn quantityGetMaterialDifference = new DataGridViewTextBoxColumn();
            quantityGetMaterialDifference.HeaderText = "领料差异";
            quantityGetMaterialDifference.Name = "quantityGetMaterialDifference";
            quantityGetMaterialDifference.DataPropertyName = "quantityGetMaterialDifference";
            quantityGetMaterialDifference.Width = 80;

            DataGridViewTextBoxColumn quantityPerAssembly = new DataGridViewTextBoxColumn();
            quantityPerAssembly.HeaderText = "每个装";
            quantityPerAssembly.Name = "quantityPerAssembly";
            quantityPerAssembly.DataPropertyName = "quantityPerAssembly";
            quantityPerAssembly.Width = 80;

            DataGridViewTextBoxColumn wipSupplyTypeCn = new DataGridViewTextBoxColumn();
            wipSupplyTypeCn.HeaderText = "供应类型";
            wipSupplyTypeCn.Name = "wipSupplyTypeCn";
            wipSupplyTypeCn.DataPropertyName = "wipSupplyTypeCn";
            wipSupplyTypeCn.Width = 80;

            DataGridViewTextBoxColumn supplySubinventory = new DataGridViewTextBoxColumn();
            supplySubinventory.HeaderText = "子库";
            supplySubinventory.Name = "supplySubinventory";
            supplySubinventory.DataPropertyName = "supplySubinventory";
            supplySubinventory.Width = 80;

            

            dataGridViewX.Columns.Add(id);
            dataGridViewX.Columns.Add(docNumber);
            dataGridViewX.Columns.Add(mpsDateRequiredStr);
            dataGridViewX.Columns.Add(description);
            dataGridViewX.Columns.Add(requiredQuantity);
            dataGridViewX.Columns.Add(wmsPreparationQuantity);
            dataGridViewX.Columns.Add(transactionQuantity);
            dataGridViewX.Columns.Add(getMaterialQuantity);
            dataGridViewX.Columns.Add(quantityGetMaterialDifference);
            dataGridViewX.Columns.Add(quantityPerAssembly);
            dataGridViewX.Columns.Add(wipSupplyTypeCn);
            dataGridViewX.Columns.Add(supplySubinventory);
        }

        private List<AscmWipRequirementOperations> DataBindDataGridView(ref int count)
        {
            List<AscmWipRequirementOperations> list = null;
            string message = string.Empty;
            string _ynPage = string.Empty;

            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(pagerControlMaterial.PageSize);
            ynPage.SetCurrentPage((pagerControlMaterial.PageCurrent <= 0) ? 1 : pagerControlMaterial.PageCurrent);
            _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);

            try
            {
                if (taskId != 0 && jobId != 0)
                {
                    AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                    //string jsonString = Service.GetMonistorMaterialList(frmMainView.encryptTicket, taskId.ToString(), jobId.ToString(), ref _ynPage, ref message);
                    string jsonString = Service.GetMonitorMaterialList(frmMainView.encryptTicket, taskId.ToString(), jobId.ToString(), tbItemWarehouse.Text, tbItemMtlCategory.Text, ref _ynPage, ref message);
                    if (!string.IsNullOrEmpty(jsonString))
                    { 
                        list = (List<AscmWipRequirementOperations>)JsonConvert.DeserializeObject(jsonString,typeof(List<AscmWipRequirementOperations>));
                        if (list != null && list.Count > 0)
                        {
                            ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);
                            count = ynPage.GetRecordCount();
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

        private int pagerControlMaterial_EventPaging(CustomControl.EventPagingArg e)
        {
            int count = 0;
            List<AscmWipRequirementOperations> list = DataBindDataGridView(ref count);
            pagerControlMaterial.DataBind(list);


            if (dgViewMaterialList.DataSource != null)
                this.dgViewMaterialList.DataSource = null;

            this.dgViewMaterialList.AutoGenerateColumns = false;
            this.dgViewMaterialList.DataSource = pagerControlMaterial.bindingSource;
            this.dgViewMaterialList.Refresh();
            return count;
        }

        private void btnItemSearch_Click(object sender, EventArgs e)
        {
            pagerControlMaterial.Bind();
        }
    }
}
