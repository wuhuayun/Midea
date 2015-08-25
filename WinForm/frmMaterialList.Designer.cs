namespace WinForm
{
    partial class frmMaterialList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgMaterialList = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ascmMaterialItem_DocNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateRequired = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ascmMaterialItem_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityPerAssembly = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wipSupplyTypeCn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplySubinventory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requiredQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wmsPreparationQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.getMaterialQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityGetMaterialDifference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pageControlMaterial = new WinForm.PageControl.PageControl();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaterialList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgMaterialList
            // 
            this.dgMaterialList.AllowUserToAddRows = false;
            this.dgMaterialList.AllowUserToDeleteRows = false;
            this.dgMaterialList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMaterialList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.ascmMaterialItem_DocNumber,
            this.dateRequired,
            this.ascmMaterialItem_Description,
            this.quantityPerAssembly,
            this.wipSupplyTypeCn,
            this.supplySubinventory,
            this.requiredQuantity,
            this.wmsPreparationQuantity,
            this.getMaterialQuantity,
            this.quantityGetMaterialDifference});
            this.dgMaterialList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMaterialList.Location = new System.Drawing.Point(0, 0);
            this.dgMaterialList.Name = "dgMaterialList";
            this.dgMaterialList.ReadOnly = true;
            this.dgMaterialList.RowTemplate.Height = 23;
            this.dgMaterialList.Size = new System.Drawing.Size(742, 251);
            this.dgMaterialList.TabIndex = 0;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // ascmMaterialItem_DocNumber
            // 
            this.ascmMaterialItem_DocNumber.DataPropertyName = "ascmMaterialItem_DocNumber";
            this.ascmMaterialItem_DocNumber.HeaderText = "组件";
            this.ascmMaterialItem_DocNumber.Name = "ascmMaterialItem_DocNumber";
            this.ascmMaterialItem_DocNumber.ReadOnly = true;
            // 
            // dateRequired
            // 
            this.dateRequired.DataPropertyName = "dateRequired";
            this.dateRequired.HeaderText = "需求日期";
            this.dateRequired.Name = "dateRequired";
            this.dateRequired.ReadOnly = true;
            this.dateRequired.Width = 90;
            // 
            // ascmMaterialItem_Description
            // 
            this.ascmMaterialItem_Description.DataPropertyName = "ascmMaterialItem_Description";
            this.ascmMaterialItem_Description.HeaderText = "组件说明";
            this.ascmMaterialItem_Description.Name = "ascmMaterialItem_Description";
            this.ascmMaterialItem_Description.ReadOnly = true;
            this.ascmMaterialItem_Description.Width = 120;
            // 
            // quantityPerAssembly
            // 
            this.quantityPerAssembly.DataPropertyName = "quantityPerAssembly";
            this.quantityPerAssembly.HeaderText = "每个装";
            this.quantityPerAssembly.Name = "quantityPerAssembly";
            this.quantityPerAssembly.ReadOnly = true;
            this.quantityPerAssembly.Width = 75;
            // 
            // wipSupplyTypeCn
            // 
            this.wipSupplyTypeCn.DataPropertyName = "wipSupplyTypeCn";
            this.wipSupplyTypeCn.HeaderText = "类型";
            this.wipSupplyTypeCn.Name = "wipSupplyTypeCn";
            this.wipSupplyTypeCn.ReadOnly = true;
            this.wipSupplyTypeCn.Width = 60;
            // 
            // supplySubinventory
            // 
            this.supplySubinventory.DataPropertyName = "supplySubinventory";
            this.supplySubinventory.HeaderText = "子库";
            this.supplySubinventory.Name = "supplySubinventory";
            this.supplySubinventory.ReadOnly = true;
            this.supplySubinventory.Width = 70;
            // 
            // requiredQuantity
            // 
            this.requiredQuantity.DataPropertyName = "requiredQuantity";
            this.requiredQuantity.HeaderText = "需求";
            this.requiredQuantity.Name = "requiredQuantity";
            this.requiredQuantity.ReadOnly = true;
            this.requiredQuantity.Width = 60;
            // 
            // wmsPreparationQuantity
            // 
            this.wmsPreparationQuantity.DataPropertyName = "wmsPreparationQuantity";
            this.wmsPreparationQuantity.HeaderText = "备料";
            this.wmsPreparationQuantity.Name = "wmsPreparationQuantity";
            this.wmsPreparationQuantity.ReadOnly = true;
            this.wmsPreparationQuantity.Width = 60;
            // 
            // getMaterialQuantity
            // 
            this.getMaterialQuantity.DataPropertyName = "getMaterialQuantity";
            this.getMaterialQuantity.HeaderText = "领料";
            this.getMaterialQuantity.Name = "getMaterialQuantity";
            this.getMaterialQuantity.ReadOnly = true;
            this.getMaterialQuantity.Width = 60;
            // 
            // quantityGetMaterialDifference
            // 
            this.quantityGetMaterialDifference.DataPropertyName = "quantityGetMaterialDifference";
            this.quantityGetMaterialDifference.HeaderText = "领料差异";
            this.quantityGetMaterialDifference.Name = "quantityGetMaterialDifference";
            this.quantityGetMaterialDifference.ReadOnly = true;
            this.quantityGetMaterialDifference.Width = 90;
            // 
            // pageControlMaterial
            // 
            this.pageControlMaterial.BackColor = System.Drawing.SystemColors.Control;
            this.pageControlMaterial.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pageControlMaterial.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pageControlMaterial.Location = new System.Drawing.Point(0, 251);
            this.pageControlMaterial.Name = "pageControlMaterial";
            this.pageControlMaterial.NMax = 0;
            this.pageControlMaterial.PageCount = 0;
            this.pageControlMaterial.PageCurrent = 0;
            this.pageControlMaterial.PageSize = 30;
            this.pageControlMaterial.Size = new System.Drawing.Size(742, 29);
            this.pageControlMaterial.TabIndex = 1;
            this.pageControlMaterial.EventPaging += new WinForm.PageControl.PageControl.EventPagingHandler(this.pageControlMaterial_EventPaging);
            // 
            // frmMaterialList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 280);
            this.Controls.Add(this.dgMaterialList);
            this.Controls.Add(this.pageControlMaterial);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "frmMaterialList";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "物料清单";
            this.Load += new System.EventHandler(this.frmMaterialList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgMaterialList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgMaterialList;
        private PageControl.PageControl pageControlMaterial;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ascmMaterialItem_DocNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateRequired;
        private System.Windows.Forms.DataGridViewTextBoxColumn ascmMaterialItem_Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityPerAssembly;
        private System.Windows.Forms.DataGridViewTextBoxColumn wipSupplyTypeCn;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplySubinventory;
        private System.Windows.Forms.DataGridViewTextBoxColumn requiredQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn wmsPreparationQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn getMaterialQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityGetMaterialDifference;

    }
}