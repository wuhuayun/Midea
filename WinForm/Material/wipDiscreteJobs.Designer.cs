namespace WinForm.Material
{
    partial class wipDiscreteJobs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(wipDiscreteJobs));
            this.dataGridViewJobs = new System.Windows.Forms.DataGridView();
            this.wipEntityId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ascmWipEntities_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bomRevisionDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.labelMaterialId = new DevComponents.DotNetBar.LabelX();
            this.btnJobsQuery = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.pageControlJob = new WinForm.PageControl.PageControl();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJobs)).BeginInit();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewJobs
            // 
            this.dataGridViewJobs.AllowUserToAddRows = false;
            this.dataGridViewJobs.AllowUserToDeleteRows = false;
            this.dataGridViewJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewJobs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewJobs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.wipEntityId,
            this.ascmWipEntities_Name,
            this.bomRevisionDate});
            this.dataGridViewJobs.Location = new System.Drawing.Point(0, 36);
            this.dataGridViewJobs.Name = "dataGridViewJobs";
            this.dataGridViewJobs.ReadOnly = true;
            this.dataGridViewJobs.RowTemplate.Height = 23;
            this.dataGridViewJobs.Size = new System.Drawing.Size(654, 310);
            this.dataGridViewJobs.TabIndex = 0;
            // 
            // wipEntityId
            // 
            this.wipEntityId.DataPropertyName = "wipEntityId";
            this.wipEntityId.HeaderText = "作业ID";
            this.wipEntityId.Name = "wipEntityId";
            this.wipEntityId.ReadOnly = true;
            // 
            // ascmWipEntities_Name
            // 
            this.ascmWipEntities_Name.DataPropertyName = "ascmWipEntities_Name";
            this.ascmWipEntities_Name.HeaderText = "作业号";
            this.ascmWipEntities_Name.Name = "ascmWipEntities_Name";
            this.ascmWipEntities_Name.ReadOnly = true;
            this.ascmWipEntities_Name.Width = 290;
            // 
            // bomRevisionDate
            // 
            this.bomRevisionDate.DataPropertyName = "bomRevisionDate";
            this.bomRevisionDate.HeaderText = "需求日期";
            this.bomRevisionDate.Name = "bomRevisionDate";
            this.bomRevisionDate.ReadOnly = true;
            this.bomRevisionDate.Width = 200;
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.labelMaterialId);
            this.panelEx1.Controls.Add(this.btnJobsQuery);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.endDate);
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.startDate);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(654, 35);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarCaptionText;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 6;
            // 
            // labelMaterialId
            // 
            this.labelMaterialId.BackColor = System.Drawing.Color.Transparent;
            this.labelMaterialId.Location = new System.Drawing.Point(8, 7);
            this.labelMaterialId.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelMaterialId.Name = "labelMaterialId";
            this.labelMaterialId.Size = new System.Drawing.Size(145, 21);
            this.labelMaterialId.TabIndex = 10;
            this.labelMaterialId.Text = "物料ID:";
            // 
            // btnJobsQuery
            // 
            this.btnJobsQuery.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnJobsQuery.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnJobsQuery.Location = new System.Drawing.Point(549, 6);
            this.btnJobsQuery.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnJobsQuery.Name = "btnJobsQuery";
            this.btnJobsQuery.Size = new System.Drawing.Size(69, 23);
            this.btnJobsQuery.TabIndex = 9;
            this.btnJobsQuery.Text = "查询";
            this.btnJobsQuery.Click += new System.EventHandler(this.btnJobsQuery_Click);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(344, 10);
            this.labelX1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(61, 21);
            this.labelX1.TabIndex = 8;
            this.labelX1.Text = "截止日期:";
            // 
            // endDate
            // 
            this.endDate.Location = new System.Drawing.Point(410, 7);
            this.endDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(109, 21);
            this.endDate.TabIndex = 7;
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(159, 10);
            this.labelX2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(61, 21);
            this.labelX2.TabIndex = 6;
            this.labelX2.Text = "起始日期:";
            // 
            // startDate
            // 
            this.startDate.Location = new System.Drawing.Point(225, 8);
            this.startDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(109, 21);
            this.startDate.TabIndex = 5;
            // 
            // pageControlJob
            // 
            this.pageControlJob.BackColor = System.Drawing.SystemColors.Control;
            this.pageControlJob.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pageControlJob.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pageControlJob.Location = new System.Drawing.Point(0, 350);
            this.pageControlJob.Name = "pageControlJob";
            this.pageControlJob.NMax = 0;
            this.pageControlJob.PageCount = 0;
            this.pageControlJob.PageCurrent = 0;
            this.pageControlJob.PageSize = 30;
            this.pageControlJob.Size = new System.Drawing.Size(654, 25);
            this.pageControlJob.TabIndex = 7;
            this.pageControlJob.EventPaging += new WinForm.PageControl.PageControl.EventPagingHandler(this.pageControlJob_EventPaging);
            // 
            // wipDiscreteJobs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 375);
            this.Controls.Add(this.pageControlJob);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.dataGridViewJobs);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "wipDiscreteJobs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "关联作业列表";
            this.Load += new System.EventHandler(this.wipDiscreteJobs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJobs)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewJobs;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.DateTimePicker startDate;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.DateTimePicker endDate;
        private DevComponents.DotNetBar.ButtonX btnJobsQuery;
        private DevComponents.DotNetBar.LabelX labelMaterialId;
        private System.Windows.Forms.DataGridViewTextBoxColumn wipEntityId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ascmWipEntities_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn bomRevisionDate;
        private PageControl.PageControl pageControlJob;
    }
}