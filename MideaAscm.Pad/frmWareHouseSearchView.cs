﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using MideaAscm.Dal.Base.Entities;
using Newtonsoft.Json;
using MideaAscm.Dal.FromErp.Entities;
using DevComponents.DotNetBar;

namespace MideaAscm.Pad
{
    public partial class frmWareHouseSearchView : DevComponents.DotNetBar.OfficeForm
    {
        private int materialId = 0;

        public frmWareHouseSearchView()
        {
            InitializeComponent();
        }

        public void FormActivated()
        {

        }

        private void frmWareHouseSearchView_Load(object sender, EventArgs e)
        {
            StructureDataGridViewColumns(this.dgViewMaterialList);
        }

        private void StructureDataGridViewColumns(DataGridViewX dataGridViewX)
        {
            if (dataGridViewX.Columns.Count > 0)
                dataGridViewX.Columns.Clear();

            DataGridViewTextBoxColumn inventoryItemId = new DataGridViewTextBoxColumn();
            inventoryItemId.HeaderText = "inventoryItemId";
            inventoryItemId.Name = "inventoryItemId";
            inventoryItemId.DataPropertyName = "inventoryItemId";
            inventoryItemId.Width = 0;
            inventoryItemId.Visible = false;

            DataGridViewTextBoxColumn ascmMateiralItem_Docnumber = new DataGridViewTextBoxColumn();
            ascmMateiralItem_Docnumber.HeaderText = "组件";
            ascmMateiralItem_Docnumber.Name = "ascmMateiralItem_Docnumber";
            ascmMateiralItem_Docnumber.DataPropertyName = "ascmMateiralItem_Docnumber";
            ascmMateiralItem_Docnumber.Width = 110;

            DataGridViewTextBoxColumn ascmMaterialItem_Description = new DataGridViewTextBoxColumn();
            ascmMaterialItem_Description.HeaderText = "组件说明";
            ascmMaterialItem_Description.Name = "ascmMaterialItem_Description";
            ascmMaterialItem_Description.DataPropertyName = "ascmMaterialItem_Description";
            ascmMaterialItem_Description.Width = 200;

            DataGridViewTextBoxColumn subinventoryCode = new DataGridViewTextBoxColumn();
            subinventoryCode.HeaderText = "仓库";
            subinventoryCode.Name = "subinventoryCode";
            subinventoryCode.DataPropertyName = "subinventoryCode";
            subinventoryCode.Width = 80;

            DataGridViewTextBoxColumn transactionQuantity = new DataGridViewTextBoxColumn();
            transactionQuantity.HeaderText = "数量";
            transactionQuantity.Name = "transactionQuantity";
            transactionQuantity.DataPropertyName = "transactionQuantity";
            transactionQuantity.Width = 90;

            DataGridViewTextBoxColumn ascmMaterialItem_Unit = new DataGridViewTextBoxColumn();
            ascmMaterialItem_Unit.HeaderText = "单位";
            ascmMaterialItem_Unit.Name = "ascmMaterialItem_Unit";
            ascmMaterialItem_Unit.DataPropertyName = "ascmMaterialItem_Unit";
            ascmMaterialItem_Unit.Width = 70;


            dataGridViewX.Columns.Add(inventoryItemId);
            dataGridViewX.Columns.Add(subinventoryCode);
            dataGridViewX.Columns.Add(transactionQuantity);
            dataGridViewX.Columns.Add(ascmMaterialItem_Unit);
            dataGridViewX.Columns.Add(ascmMateiralItem_Docnumber);
            dataGridViewX.Columns.Add(ascmMaterialItem_Description);
        }

        private List<AscmMtlOnhandQuantitiesDetail> DataBindDataGridView(ref int count, int queryId)
        {
            List<AscmMtlOnhandQuantitiesDetail> list = null;
            string message = string.Empty;
            string _ynPage = string.Empty;

            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(pagerControlMaterial.PageSize);
            ynPage.SetCurrentPage((pagerControlMaterial.PageCurrent <= 0) ? 1 : pagerControlMaterial.PageCurrent);
            _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);

            try
            {
                AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                string jsonString = Service.GetMaterialWarehouseList(frmMainView.encryptTicket, materialId, ref _ynPage, ref message);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    list = (List<AscmMtlOnhandQuantitiesDetail>)JsonConvert.DeserializeObject(jsonString, typeof(List<AscmMtlOnhandQuantitiesDetail>));
                    if (list != null && list.Count > 0)
                    {
                        ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);
                        count = ynPage.GetRecordCount();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            frmSelectSignMaterialDialog frmSelectSignMaterialDialog = new frmSelectSignMaterialDialog();
            frmSelectSignMaterialDialog.ShowDialog();
            if (frmSelectSignMaterialDialog.DialogResult == DialogResult.OK)
            {
                tbDocnumber.Text = frmSelectSignMaterialDialog.queryResult;
                materialId = int.Parse(frmSelectSignMaterialDialog.queryId);
                frmSelectSignMaterialDialog.Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (this.materialId == 0)
            {
                MessageBoxEx.Show("请选择物料编码！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            pagerControlMaterial.Bind();
        }

        private int pagerControlMaterial_EventPaging(CustomControl.EventPagingArg e)
        {
            int count = 0;
            List<AscmMtlOnhandQuantitiesDetail> list = DataBindDataGridView(ref count, this.materialId);
            pagerControlMaterial.DataBind(list);


            if (dgViewMaterialList.DataSource != null)
                this.dgViewMaterialList.DataSource = null;

            this.dgViewMaterialList.AutoGenerateColumns = false;
            this.dgViewMaterialList.DataSource = pagerControlMaterial.bindingSource;
            this.dgViewMaterialList.Refresh();
            return count;
        }

        private void btnItemJob_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "frmRelateDiscreteJobsView")
                {
                    form.Focus();
                    return;
                }
            }

         //   frmRelateDiscreteJobsView frmRelateDiscreteJobsView = new frmRelateDiscreteJobsView();
            //frmRelateDiscreteJobsView.MdiParent = this.ParentForm;
            //frmRelateDiscreteJobsView.materialId = materialId;
            //frmRelateDiscreteJobsView.BringToFront();
            //frmRelateDiscreteJobsView.Show();
        }
    }
}
