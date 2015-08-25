namespace WinForm
{
    partial class frmSumMaterialInfo
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
            this.docNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wipSupplyTypeCn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requiredQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityIssued = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.getMaterialQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDifference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pcMaterialList = new WinForm.PageControl.PageControl();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaterialList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgMaterialList
            // 
            this.dgMaterialList.AllowUserToAddRows = false;
            this.dgMaterialList.AllowUserToDeleteRows = false;
            this.dgMaterialList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMaterialList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.docNumber,
            this.description,
            this.wipSupplyTypeCn,
            this.requiredQuantity,
            this.quantityIssued,
            this.getMaterialQuantity,
            this.quantityDifference});
            this.dgMaterialList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMaterialList.Location = new System.Drawing.Point(0, 0);
            this.dgMaterialList.Name = "dgMaterialList";
            this.dgMaterialList.ReadOnly = true;
            this.dgMaterialList.RowTemplate.Height = 23;
            this.dgMaterialList.Size = new System.Drawing.Size(711, 230);
            this.dgMaterialList.TabIndex = 1;
            // 
            // docNumber
            // 
            this.docNumber.DataPropertyName = "docNumber";
            this.docNumber.HeaderText = "物料编码";
            this.docNumber.Name = "docNumber";
            this.docNumber.ReadOnly = true;
            // 
            // description
            // 
            this.description.DataPropertyName = "description";
            this.description.HeaderText = "描述";
            this.description.Name = "description";
            this.description.ReadOnly = true;
            this.description.Width = 180;
            // 
            // wipSupplyTypeCn
            // 
            this.wipSupplyTypeCn.DataPropertyName = "wipSupplyTypeCn";
            this.wipSupplyTypeCn.HeaderText = "类型";
            this.wipSupplyTypeCn.Name = "wipSupplyTypeCn";
            this.wipSupplyTypeCn.ReadOnly = true;
            this.wipSupplyTypeCn.Width = 65;
            // 
            // requiredQuantity
            // 
            this.requiredQuantity.DataPropertyName = "requiredQuantity";
            this.requiredQuantity.HeaderText = "需求";
            this.requiredQuantity.Name = "requiredQuantity";
            this.requiredQuantity.ReadOnly = true;
            this.requiredQuantity.Width = 70;
            // 
            // quantityIssued
            // 
            this.quantityIssued.DataPropertyName = "quantityIssued";
            this.quantityIssued.HeaderText = "发料";
            this.quantityIssued.Name = "quantityIssued";
            this.quantityIssued.ReadOnly = true;
            this.quantityIssued.Width = 70;
            // 
            // getMaterialQuantity
            // 
            this.getMaterialQuantity.DataPropertyName = "getMaterialQuantity";
            this.getMaterialQuantity.HeaderText = "领料";
            this.getMaterialQuantity.Name = "getMaterialQuantity";
            this.getMaterialQuantity.ReadOnly = true;
            this.getMaterialQuantity.Width = 70;
            // 
            // quantityDifference
            // 
            this.quantityDifference.DataPropertyName = "quantityDifference";
            this.quantityDifference.HeaderText = "差异";
            this.quantityDifference.Name = "quantityDifference";
            this.quantityDifference.ReadOnly = true;
            this.quantityDifference.Width = 70;
            // 
            // pcMaterialList
            // 
            this.pcMaterialList.BackColor = System.Drawing.SystemColors.Control;
            this.pcMaterialList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pcMaterialList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pcMaterialList.Location = new System.Drawing.Point(0, 230);
            this.pcMaterialList.Name = "pcMaterialList";
            this.pcMaterialList.NMax = 0;
            this.pcMaterialList.PageCount = 0;
            this.pcMaterialList.PageCurrent = 0;
            this.pcMaterialList.PageSize = 30;
            this.pcMaterialList.Size = new System.Drawing.Size(711, 29);
            this.pcMaterialList.TabIndex = 0;
            this.pcMaterialList.EventPaging += new WinForm.PageControl.PageControl.EventPagingHandler(this.pcMaterialList_EventPaging);
            // 
            // frmSumMaterialInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 259);
            this.Controls.Add(this.dgMaterialList);
            this.Controls.Add(this.pcMaterialList);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "frmSumMaterialInfo";
            this.ShowIcon = false;
            this.Text = "汇总物料";
            this.Load += new System.EventHandler(this.frmSumMaterialInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgMaterialList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PageControl.PageControl pcMaterialList;
        private System.Windows.Forms.DataGridView dgMaterialList;
        private System.Windows.Forms.DataGridViewTextBoxColumn docNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn wipSupplyTypeCn;
        private System.Windows.Forms.DataGridViewTextBoxColumn requiredQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityIssued;
        private System.Windows.Forms.DataGridViewTextBoxColumn getMaterialQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDifference;
    }
}