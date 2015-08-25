namespace MideaAscm.Pad
{
    partial class frmSumMaterialListView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.tbItemStart = new DevComponents.DotNetBar.TextBoxItem();
            this.labelItem2 = new DevComponents.DotNetBar.LabelItem();
            this.tbItemEnd = new DevComponents.DotNetBar.TextBoxItem();
            this.btnItemSearch = new DevComponents.DotNetBar.ButtonItem();
            this.dgViewMaterialList = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.pagerControlMaterial = new MideaAscm.Pad.CustomControl.PagerControl();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewMaterialList)).BeginInit();
            this.SuspendLayout();
            // 
            // bar1
            // 
            this.bar1.AntiAlias = true;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bar1.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem1,
            this.tbItemStart,
            this.labelItem2,
            this.tbItemEnd,
            this.btnItemSearch});
            this.bar1.Location = new System.Drawing.Point(0, 0);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(797, 30);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bar1.TabIndex = 0;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // labelItem1
            // 
            this.labelItem1.ForeColor = System.Drawing.Color.Black;
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Text = "起止日期：";
            // 
            // tbItemStart
            // 
            this.tbItemStart.Name = "tbItemStart";
            this.tbItemStart.TextBoxWidth = 90;
            this.tbItemStart.WatermarkColor = System.Drawing.SystemColors.GrayText;
            this.tbItemStart.GotFocus += new System.EventHandler(this.textBoxItem_GotFocus);
            // 
            // labelItem2
            // 
            this.labelItem2.ForeColor = System.Drawing.Color.Black;
            this.labelItem2.Name = "labelItem2";
            this.labelItem2.Text = "~";
            // 
            // tbItemEnd
            // 
            this.tbItemEnd.Name = "tbItemEnd";
            this.tbItemEnd.TextBoxWidth = 90;
            this.tbItemEnd.WatermarkColor = System.Drawing.SystemColors.GrayText;
            this.tbItemEnd.GotFocus += new System.EventHandler(this.textBoxItem_GotFocus);
            // 
            // btnItemSearch
            // 
            this.btnItemSearch.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnItemSearch.Image = global::MideaAscm.Pad.Properties.Resources.sum;
            this.btnItemSearch.Name = "btnItemSearch";
            this.btnItemSearch.Text = "汇总";
            this.btnItemSearch.Click += new System.EventHandler(this.btnItemSearch_Click);
            // 
            // dgViewMaterialList
            // 
            this.dgViewMaterialList.AllowUserToAddRows = false;
            this.dgViewMaterialList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgViewMaterialList.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgViewMaterialList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgViewMaterialList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.dgViewMaterialList.Location = new System.Drawing.Point(0, 30);
            this.dgViewMaterialList.Name = "dgViewMaterialList";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgViewMaterialList.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgViewMaterialList.RowTemplate.Height = 23;
            this.dgViewMaterialList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgViewMaterialList.Size = new System.Drawing.Size(797, 341);
            this.dgViewMaterialList.TabIndex = 2;
            // 
            // pagerControlMaterial
            // 
            this.pagerControlMaterial.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pagerControlMaterial.Location = new System.Drawing.Point(0, 371);
            this.pagerControlMaterial.Name = "pagerControlMaterial";
            this.pagerControlMaterial.NMax = 0;
            this.pagerControlMaterial.PageCount = 0;
            this.pagerControlMaterial.PageCurrent = 0;
            this.pagerControlMaterial.PageSize = 30;
            this.pagerControlMaterial.Size = new System.Drawing.Size(797, 25);
            this.pagerControlMaterial.TabIndex = 1;
            this.pagerControlMaterial.EventPaging += new MideaAscm.Pad.CustomControl.PagerControl.EventPagingHandler(this.pagerControlMaterial_EventPaging);
            // 
            // frmSumMaterialListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 396);
            this.Controls.Add(this.dgViewMaterialList);
            this.Controls.Add(this.pagerControlMaterial);
            this.Controls.Add(this.bar1);
            this.DoubleBuffered = true;
            this.Name = "frmSumMaterialListView";
            this.Text = "汇总编码";
            this.Load += new System.EventHandler(this.frmSumMaterialListView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewMaterialList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Bar bar1;
        private CustomControl.PagerControl pagerControlMaterial;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgViewMaterialList;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.TextBoxItem tbItemStart;
        private DevComponents.DotNetBar.LabelItem labelItem2;
        private DevComponents.DotNetBar.TextBoxItem tbItemEnd;
        private DevComponents.DotNetBar.ButtonItem btnItemSearch;
    }
}