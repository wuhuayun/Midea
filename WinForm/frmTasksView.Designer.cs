namespace WinForm
{
    partial class frmTasksView
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.btnTaskDelete = new DevComponents.DotNetBar.ButtonX();
            this.btnTaskEdit = new DevComponents.DotNetBar.ButtonX();
            this.btnTaskAdd = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.taskCreateTime = new System.Windows.Forms.DateTimePicker();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnTasksSearch = new DevComponents.DotNetBar.ButtonX();
            this.cbxTaskStatus = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.comboItem4 = new DevComponents.Editors.ComboItem();
            this.comboItem5 = new DevComponents.Editors.ComboItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewTasks = new System.Windows.Forms.DataGridView();
            this.createUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdentificationIdCN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mtlCategoryStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tipCN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouserPlace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryStatusCN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.materialDocNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rankerId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.workerId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdentificationId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pageControlTask = new WinForm.PageControl.PageControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTasks)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelEx1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(796, 442);
            this.splitContainer1.SplitterDistance = 29;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.btnTaskDelete);
            this.panelEx1.Controls.Add(this.btnTaskEdit);
            this.panelEx1.Controls.Add(this.btnTaskAdd);
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.taskCreateTime);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.btnTasksSearch);
            this.panelEx1.Controls.Add(this.cbxTaskStatus);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(796, 35);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarCaptionText;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 6;
            // 
            // btnTaskDelete
            // 
            this.btnTaskDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTaskDelete.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnTaskDelete.Location = new System.Drawing.Point(693, 2);
            this.btnTaskDelete.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnTaskDelete.Name = "btnTaskDelete";
            this.btnTaskDelete.Size = new System.Drawing.Size(87, 27);
            this.btnTaskDelete.TabIndex = 10;
            this.btnTaskDelete.Text = "删除";
            this.btnTaskDelete.Click += new System.EventHandler(this.btnTaskDelete_Click);
            // 
            // btnTaskEdit
            // 
            this.btnTaskEdit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTaskEdit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnTaskEdit.Location = new System.Drawing.Point(599, 2);
            this.btnTaskEdit.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnTaskEdit.Name = "btnTaskEdit";
            this.btnTaskEdit.Size = new System.Drawing.Size(87, 27);
            this.btnTaskEdit.TabIndex = 9;
            this.btnTaskEdit.Text = "修改";
            this.btnTaskEdit.Click += new System.EventHandler(this.btnTaskEdit_Click);
            // 
            // btnTaskAdd
            // 
            this.btnTaskAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTaskAdd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnTaskAdd.Location = new System.Drawing.Point(506, 2);
            this.btnTaskAdd.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnTaskAdd.Name = "btnTaskAdd";
            this.btnTaskAdd.Size = new System.Drawing.Size(87, 27);
            this.btnTaskAdd.TabIndex = 8;
            this.btnTaskAdd.Text = "增加";
            this.btnTaskAdd.Click += new System.EventHandler(this.btnTaskAdd_Click);
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(11, 5);
            this.labelX2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(71, 27);
            this.labelX2.TabIndex = 6;
            this.labelX2.Text = "生成日期:";
            // 
            // taskCreateTime
            // 
            this.taskCreateTime.Location = new System.Drawing.Point(83, 6);
            this.taskCreateTime.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.taskCreateTime.Name = "taskCreateTime";
            this.taskCreateTime.Size = new System.Drawing.Size(123, 23);
            this.taskCreateTime.TabIndex = 5;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(212, 6);
            this.labelX1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(71, 27);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "任务状态:";
            // 
            // btnTasksSearch
            // 
            this.btnTasksSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTasksSearch.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnTasksSearch.Location = new System.Drawing.Point(418, 2);
            this.btnTasksSearch.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnTasksSearch.Name = "btnTasksSearch";
            this.btnTasksSearch.Size = new System.Drawing.Size(80, 27);
            this.btnTasksSearch.TabIndex = 3;
            this.btnTasksSearch.Text = "查询";
            this.btnTasksSearch.Click += new System.EventHandler(this.btnTasksSearch_Click);
            // 
            // cbxTaskStatus
            // 
            this.cbxTaskStatus.DisplayMember = "Text";
            this.cbxTaskStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxTaskStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTaskStatus.FormattingEnabled = true;
            this.cbxTaskStatus.ItemHeight = 17;
            this.cbxTaskStatus.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2,
            this.comboItem3,
            this.comboItem4,
            this.comboItem5});
            this.cbxTaskStatus.Location = new System.Drawing.Point(289, 6);
            this.cbxTaskStatus.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cbxTaskStatus.Name = "cbxTaskStatus";
            this.cbxTaskStatus.Size = new System.Drawing.Size(123, 23);
            this.cbxTaskStatus.TabIndex = 1;
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "已完成";
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "执行中";
            // 
            // comboItem4
            // 
            this.comboItem4.Text = "已分配";
            // 
            // comboItem5
            // 
            this.comboItem5.Text = "未分配";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dataGridViewTasks);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pageControlTask);
            this.splitContainer2.Size = new System.Drawing.Size(796, 408);
            this.splitContainer2.SplitterDistance = 373;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // dataGridViewTasks
            // 
            this.dataGridViewTasks.AllowUserToAddRows = false;
            this.dataGridViewTasks.AllowUserToDeleteRows = false;
            this.dataGridViewTasks.AllowUserToOrderColumns = true;
            this.dataGridViewTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.createUser,
            this.IdentificationIdCN,
            this.ID,
            this.mtlCategoryStatus,
            this.createTime,
            this.taskId,
            this.tipCN,
            this.productLine,
            this.warehouserId,
            this.warehouserPlace,
            this.categoryStatusCN,
            this.materialDocNumber,
            this.taskTime,
            this.rankerId,
            this.workerId,
            this._status,
            this.status,
            this.IdentificationId,
            this.tip});
            this.dataGridViewTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTasks.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTasks.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dataGridViewTasks.MultiSelect = false;
            this.dataGridViewTasks.Name = "dataGridViewTasks";
            this.dataGridViewTasks.ReadOnly = true;
            this.dataGridViewTasks.RowTemplate.Height = 23;
            this.dataGridViewTasks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTasks.Size = new System.Drawing.Size(796, 373);
            this.dataGridViewTasks.TabIndex = 7;
            // 
            // createUser
            // 
            this.createUser.DataPropertyName = "createUser";
            this.createUser.HeaderText = "创建人";
            this.createUser.Name = "createUser";
            this.createUser.ReadOnly = true;
            this.createUser.Visible = false;
            // 
            // IdentificationIdCN
            // 
            this.IdentificationIdCN.DataPropertyName = "IdentificationIdCN";
            this.IdentificationIdCN.HeaderText = "类型";
            this.IdentificationIdCN.Name = "IdentificationIdCN";
            this.IdentificationIdCN.ReadOnly = true;
            this.IdentificationIdCN.Width = 60;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // mtlCategoryStatus
            // 
            this.mtlCategoryStatus.DataPropertyName = "mtlCategoryStatus";
            this.mtlCategoryStatus.HeaderText = "物料类别状态";
            this.mtlCategoryStatus.Name = "mtlCategoryStatus";
            this.mtlCategoryStatus.ReadOnly = true;
            this.mtlCategoryStatus.Visible = false;
            // 
            // createTime
            // 
            this.createTime.DataPropertyName = "createTime";
            this.createTime.HeaderText = "创建时间";
            this.createTime.Name = "createTime";
            this.createTime.ReadOnly = true;
            this.createTime.Visible = false;
            // 
            // taskId
            // 
            this.taskId.DataPropertyName = "taskId";
            this.taskId.HeaderText = "任务号";
            this.taskId.Name = "taskId";
            this.taskId.ReadOnly = true;
            this.taskId.Width = 80;
            // 
            // tipCN
            // 
            this.tipCN.DataPropertyName = "tipCN";
            this.tipCN.HeaderText = "任务内容";
            this.tipCN.Name = "tipCN";
            this.tipCN.ReadOnly = true;
            this.tipCN.Width = 90;
            // 
            // productLine
            // 
            this.productLine.DataPropertyName = "productLine";
            this.productLine.HeaderText = "生产线";
            this.productLine.Name = "productLine";
            this.productLine.ReadOnly = true;
            this.productLine.Width = 80;
            // 
            // warehouserId
            // 
            this.warehouserId.DataPropertyName = "warehouserId";
            this.warehouserId.HeaderText = "仓库";
            this.warehouserId.Name = "warehouserId";
            this.warehouserId.ReadOnly = true;
            this.warehouserId.Width = 70;
            // 
            // warehouserPlace
            // 
            this.warehouserPlace.DataPropertyName = "warehouserPlace";
            this.warehouserPlace.HeaderText = "仓库位置";
            this.warehouserPlace.Name = "warehouserPlace";
            this.warehouserPlace.ReadOnly = true;
            this.warehouserPlace.Visible = false;
            // 
            // categoryStatusCN
            // 
            this.categoryStatusCN.DataPropertyName = "categoryStatusCN";
            this.categoryStatusCN.HeaderText = "类别状态";
            this.categoryStatusCN.Name = "categoryStatusCN";
            this.categoryStatusCN.ReadOnly = true;
            this.categoryStatusCN.Width = 90;
            // 
            // materialDocNumber
            // 
            this.materialDocNumber.DataPropertyName = "materialDocNumber";
            this.materialDocNumber.HeaderText = "物料编码";
            this.materialDocNumber.Name = "materialDocNumber";
            this.materialDocNumber.ReadOnly = true;
            this.materialDocNumber.Width = 90;
            // 
            // taskTime
            // 
            this.taskTime.DataPropertyName = "taskTime";
            this.taskTime.FillWeight = 80F;
            this.taskTime.HeaderText = "上线时间";
            this.taskTime.Name = "taskTime";
            this.taskTime.ReadOnly = true;
            this.taskTime.Width = 90;
            // 
            // rankerId
            // 
            this.rankerId.DataPropertyName = "rankerId";
            this.rankerId.HeaderText = "排产员";
            this.rankerId.Name = "rankerId";
            this.rankerId.ReadOnly = true;
            this.rankerId.Visible = false;
            // 
            // workerId
            // 
            this.workerId.DataPropertyName = "workerId";
            this.workerId.HeaderText = "责任人";
            this.workerId.Name = "workerId";
            this.workerId.ReadOnly = true;
            this.workerId.Visible = false;
            // 
            // _status
            // 
            this._status.DataPropertyName = "_status";
            this._status.HeaderText = "状态";
            this._status.Name = "_status";
            this._status.ReadOnly = true;
            this._status.Width = 80;
            // 
            // status
            // 
            this.status.DataPropertyName = "status";
            this.status.HeaderText = "状态";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Visible = false;
            // 
            // IdentificationId
            // 
            this.IdentificationId.DataPropertyName = "IdentificationId";
            this.IdentificationId.HeaderText = "类型";
            this.IdentificationId.Name = "IdentificationId";
            this.IdentificationId.ReadOnly = true;
            this.IdentificationId.Visible = false;
            // 
            // tip
            // 
            this.tip.DataPropertyName = "tip";
            this.tip.HeaderText = "任务内容";
            this.tip.Name = "tip";
            this.tip.ReadOnly = true;
            this.tip.Visible = false;
            // 
            // pageControlTask
            // 
            this.pageControlTask.BackColor = System.Drawing.SystemColors.Control;
            this.pageControlTask.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pageControlTask.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pageControlTask.Location = new System.Drawing.Point(0, 1);
            this.pageControlTask.Name = "pageControlTask";
            this.pageControlTask.NMax = 0;
            this.pageControlTask.PageCount = 0;
            this.pageControlTask.PageCurrent = 0;
            this.pageControlTask.PageSize = 30;
            this.pageControlTask.Size = new System.Drawing.Size(796, 29);
            this.pageControlTask.TabIndex = 8;
            this.pageControlTask.EventPaging += new WinForm.PageControl.PageControl.EventPagingHandler(this.pageControlTask_EventPaging);
            this.pageControlTask.Load += new System.EventHandler(this.pageControlTask_Load);
            // 
            // frmTasksView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 442);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "frmTasksView";
            this.ShowIcon = false;
            this.Text = "临时领料任务";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTasks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX btnTaskDelete;
        private DevComponents.DotNetBar.ButtonX btnTaskEdit;
        private DevComponents.DotNetBar.ButtonX btnTaskAdd;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.DateTimePicker taskCreateTime;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnTasksSearch;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxTaskStatus;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.Editors.ComboItem comboItem4;
        private DevComponents.Editors.ComboItem comboItem5;
        private System.Windows.Forms.DataGridView dataGridViewTasks;
        private PageControl.PageControl pageControlTask;
        private System.Windows.Forms.DataGridViewTextBoxColumn createUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdentificationIdCN;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn mtlCategoryStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn createTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskId;
        private System.Windows.Forms.DataGridViewTextBoxColumn tipCN;
        private System.Windows.Forms.DataGridViewTextBoxColumn productLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouserId;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouserPlace;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryStatusCN;
        private System.Windows.Forms.DataGridViewTextBoxColumn materialDocNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn rankerId;
        private System.Windows.Forms.DataGridViewTextBoxColumn workerId;
        private System.Windows.Forms.DataGridViewTextBoxColumn _status;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdentificationId;
        private System.Windows.Forms.DataGridViewTextBoxColumn tip;
    }
}