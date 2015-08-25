namespace WinForm
{
    partial class Main_Form
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_Form));
            this.menuBar = new DevComponents.DotNetBar.Bar();
            this.mTasks = new DevComponents.DotNetBar.ButtonItem();
            this.GetMaterialTasks = new DevComponents.DotNetBar.ButtonItem();
            this.AddMaterialTask = new DevComponents.DotNetBar.ButtonItem();
            this.mHelp = new DevComponents.DotNetBar.ButtonItem();
            this.mAboutUs = new DevComponents.DotNetBar.ButtonItem();
            this.tabControl = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.pageControlTask = new WinForm.PageControl.PageControl();
            this.dataGridViewTasks = new System.Windows.Forms.DataGridView();
            this.createUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdentificationIdCN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mtlCategoryStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryStatusCN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rankerId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.workerId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouserPlace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdentificationId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.materialDocNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tipCN = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.tabGetMaterialTasks = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this.pageControlMaterial = new WinForm.PageControl.PageControl();
            this.dataGridViewMaterials = new System.Windows.Forms.DataGridView();
            this.ID_M = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.docNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warehouseName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.memo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.btnQueryJobs = new DevComponents.DotNetBar.ButtonX();
            this.txtMaterialDocNumber = new System.Windows.Forms.TextBox();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.btnMaterialSearch = new DevComponents.DotNetBar.ButtonX();
            this.tabMaterialQuery = new DevComponents.DotNetBar.TabItem(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.menuBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTasks)).BeginInit();
            this.panelEx1.SuspendLayout();
            this.tabControlPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMaterials)).BeginInit();
            this.panelEx2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuBar
            // 
            this.menuBar.AccessibleDescription = "DotNetBar Bar (menuBar)";
            this.menuBar.AccessibleName = "DotNetBar Bar";
            this.menuBar.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.menuBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuBar.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuBar.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mTasks,
            this.mHelp});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.menuBar.MenuBar = true;
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(764, 24);
            this.menuBar.Stretch = true;
            this.menuBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.menuBar.TabIndex = 1;
            this.menuBar.TabStop = false;
            this.menuBar.Text = "menuBar";
            // 
            // mTasks
            // 
            this.mTasks.ImagePaddingHorizontal = 8;
            this.mTasks.Name = "mTasks";
            this.mTasks.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.GetMaterialTasks,
            this.AddMaterialTask});
            this.mTasks.Text = "领料任务(&T)";
            // 
            // GetMaterialTasks
            // 
            this.GetMaterialTasks.ImagePaddingHorizontal = 8;
            this.GetMaterialTasks.Name = "GetMaterialTasks";
            this.GetMaterialTasks.Text = "临时领料任务(&L)";
            this.GetMaterialTasks.Click += new System.EventHandler(this.GetMaterialTasks_Click);
            // 
            // AddMaterialTask
            // 
            this.AddMaterialTask.ImagePaddingHorizontal = 8;
            this.AddMaterialTask.Name = "AddMaterialTask";
            this.AddMaterialTask.Text = "添加领料任务(&A)";
            this.AddMaterialTask.Click += new System.EventHandler(this.AddMaterialTask_Click);
            // 
            // mHelp
            // 
            this.mHelp.ImagePaddingHorizontal = 8;
            this.mHelp.Name = "mHelp";
            this.mHelp.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mAboutUs});
            this.mHelp.Text = "帮助(&H)";
            // 
            // mAboutUs
            // 
            this.mAboutUs.ImagePaddingHorizontal = 8;
            this.mAboutUs.Name = "mAboutUs";
            this.mAboutUs.Text = "关于本软件(&A)";
            this.mAboutUs.Click += new System.EventHandler(this.mAboutUs_Click);
            // 
            // tabControl
            // 
            this.tabControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.tabControl.CanReorderTabs = true;
            this.tabControl.Controls.Add(this.tabControlPanel2);
            this.tabControl.Controls.Add(this.tabControlPanel1);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabControl.SelectedTabIndex = 1;
            this.tabControl.Size = new System.Drawing.Size(764, 438);
            this.tabControl.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document;
            this.tabControl.TabIndex = 3;
            this.tabControl.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl.Tabs.Add(this.tabGetMaterialTasks);
            this.tabControl.Tabs.Add(this.tabMaterialQuery);
            this.tabControl.Text = "tabControl";
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.pageControlTask);
            this.tabControlPanel1.Controls.Add(this.dataGridViewTasks);
            this.tabControlPanel1.Controls.Add(this.panelEx1);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 25);
            this.tabControlPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(764, 413);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(188)))), ((int)(((byte)(227)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(165)))), ((int)(((byte)(199)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right) 
            | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabGetMaterialTasks;
            // 
            // pageControlTask
            // 
            this.pageControlTask.BackColor = System.Drawing.SystemColors.Control;
            this.pageControlTask.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pageControlTask.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pageControlTask.Location = new System.Drawing.Point(1, 387);
            this.pageControlTask.Name = "pageControlTask";
            this.pageControlTask.NMax = 0;
            this.pageControlTask.PageCount = 0;
            this.pageControlTask.PageCurrent = 0;
            this.pageControlTask.PageSize = 30;
            this.pageControlTask.Size = new System.Drawing.Size(762, 25);
            this.pageControlTask.TabIndex = 7;
            this.pageControlTask.EventPaging += new WinForm.PageControl.PageControl.EventPagingHandler(this.pageControlTask_EventPaging);
            // 
            // dataGridViewTasks
            // 
            this.dataGridViewTasks.AllowUserToAddRows = false;
            this.dataGridViewTasks.AllowUserToDeleteRows = false;
            this.dataGridViewTasks.AllowUserToOrderColumns = true;
            this.dataGridViewTasks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTasks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.createUser,
            this.IdentificationIdCN,
            this.ID,
            this.mtlCategoryStatus,
            this.createTime,
            this.taskId,
            this.productLine,
            this.warehouserId,
            this.categoryStatusCN,
            this.rankerId,
            this.workerId,
            this._status,
            this.status,
            this.taskTime,
            this.warehouserPlace,
            this.IdentificationId,
            this.materialDocNumber,
            this.tip,
            this.tipCN});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTasks.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTasks.Location = new System.Drawing.Point(1, 36);
            this.dataGridViewTasks.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewTasks.MultiSelect = false;
            this.dataGridViewTasks.Name = "dataGridViewTasks";
            this.dataGridViewTasks.ReadOnly = true;
            this.dataGridViewTasks.RowTemplate.Height = 23;
            this.dataGridViewTasks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTasks.Size = new System.Drawing.Size(762, 350);
            this.dataGridViewTasks.TabIndex = 6;
            this.dataGridViewTasks.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTasks_CellDoubleClick);
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
            // 
            // taskId
            // 
            this.taskId.DataPropertyName = "taskId";
            this.taskId.HeaderText = "任务号";
            this.taskId.Name = "taskId";
            this.taskId.ReadOnly = true;
            // 
            // productLine
            // 
            this.productLine.DataPropertyName = "productLine";
            this.productLine.HeaderText = "生产线";
            this.productLine.Name = "productLine";
            this.productLine.ReadOnly = true;
            // 
            // warehouserId
            // 
            this.warehouserId.DataPropertyName = "warehouserId";
            this.warehouserId.HeaderText = "仓库";
            this.warehouserId.Name = "warehouserId";
            this.warehouserId.ReadOnly = true;
            // 
            // categoryStatusCN
            // 
            this.categoryStatusCN.DataPropertyName = "categoryStatusCN";
            this.categoryStatusCN.HeaderText = "物料类别状态";
            this.categoryStatusCN.Name = "categoryStatusCN";
            this.categoryStatusCN.ReadOnly = true;
            // 
            // rankerId
            // 
            this.rankerId.DataPropertyName = "rankerId";
            this.rankerId.HeaderText = "排产员";
            this.rankerId.Name = "rankerId";
            this.rankerId.ReadOnly = true;
            // 
            // workerId
            // 
            this.workerId.DataPropertyName = "workerId";
            this.workerId.HeaderText = "责任人";
            this.workerId.Name = "workerId";
            this.workerId.ReadOnly = true;
            // 
            // _status
            // 
            this._status.DataPropertyName = "_status";
            this._status.HeaderText = "状态";
            this._status.Name = "_status";
            this._status.ReadOnly = true;
            // 
            // status
            // 
            this.status.DataPropertyName = "status";
            this.status.HeaderText = "状态";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Visible = false;
            // 
            // taskTime
            // 
            this.taskTime.DataPropertyName = "taskTime";
            this.taskTime.HeaderText = "上线时间";
            this.taskTime.Name = "taskTime";
            this.taskTime.ReadOnly = true;
            // 
            // warehouserPlace
            // 
            this.warehouserPlace.DataPropertyName = "warehouserPlace";
            this.warehouserPlace.HeaderText = "仓库位置";
            this.warehouserPlace.Name = "warehouserPlace";
            this.warehouserPlace.ReadOnly = true;
            // 
            // IdentificationId
            // 
            this.IdentificationId.DataPropertyName = "IdentificationId";
            this.IdentificationId.HeaderText = "类型";
            this.IdentificationId.Name = "IdentificationId";
            this.IdentificationId.ReadOnly = true;
            this.IdentificationId.Visible = false;
            // 
            // materialDocNumber
            // 
            this.materialDocNumber.DataPropertyName = "materialDocNumber";
            this.materialDocNumber.HeaderText = "物料编码";
            this.materialDocNumber.Name = "materialDocNumber";
            this.materialDocNumber.ReadOnly = true;
            // 
            // tip
            // 
            this.tip.DataPropertyName = "tip";
            this.tip.HeaderText = "任务内容";
            this.tip.Name = "tip";
            this.tip.ReadOnly = true;
            this.tip.Visible = false;
            // 
            // tipCN
            // 
            this.tipCN.DataPropertyName = "tipCN";
            this.tipCN.HeaderText = "任务内容";
            this.tipCN.Name = "tipCN";
            this.tipCN.ReadOnly = true;
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
            this.panelEx1.Location = new System.Drawing.Point(1, 1);
            this.panelEx1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(762, 35);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarCaptionText;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 5;
            // 
            // btnTaskDelete
            // 
            this.btnTaskDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTaskDelete.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnTaskDelete.Location = new System.Drawing.Point(665, 6);
            this.btnTaskDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTaskDelete.Name = "btnTaskDelete";
            this.btnTaskDelete.Size = new System.Drawing.Size(75, 23);
            this.btnTaskDelete.TabIndex = 10;
            this.btnTaskDelete.Text = "删除";
            this.btnTaskDelete.Click += new System.EventHandler(this.btnTaskDelete_Click);
            // 
            // btnTaskEdit
            // 
            this.btnTaskEdit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTaskEdit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnTaskEdit.Location = new System.Drawing.Point(573, 6);
            this.btnTaskEdit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTaskEdit.Name = "btnTaskEdit";
            this.btnTaskEdit.Size = new System.Drawing.Size(75, 23);
            this.btnTaskEdit.TabIndex = 9;
            this.btnTaskEdit.Text = "修改";
            this.btnTaskEdit.Click += new System.EventHandler(this.btnTaskEdit_Click);
            // 
            // btnTaskAdd
            // 
            this.btnTaskAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTaskAdd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnTaskAdd.Location = new System.Drawing.Point(483, 6);
            this.btnTaskAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTaskAdd.Name = "btnTaskAdd";
            this.btnTaskAdd.Size = new System.Drawing.Size(75, 23);
            this.btnTaskAdd.TabIndex = 8;
            this.btnTaskAdd.Text = "增加";
            this.btnTaskAdd.Click += new System.EventHandler(this.btnTaskAdd_Click);
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(13, 8);
            this.labelX2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(61, 23);
            this.labelX2.TabIndex = 6;
            this.labelX2.Text = "生成日期:";
            // 
            // taskCreateTime
            // 
            this.taskCreateTime.Location = new System.Drawing.Point(79, 8);
            this.taskCreateTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.taskCreateTime.Name = "taskCreateTime";
            this.taskCreateTime.Size = new System.Drawing.Size(109, 21);
            this.taskCreateTime.TabIndex = 5;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(204, 8);
            this.labelX1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(61, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "任务状态:";
            // 
            // btnTasksSearch
            // 
            this.btnTasksSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTasksSearch.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnTasksSearch.Location = new System.Drawing.Point(399, 6);
            this.btnTasksSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTasksSearch.Name = "btnTasksSearch";
            this.btnTasksSearch.Size = new System.Drawing.Size(69, 23);
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
            this.cbxTaskStatus.ItemHeight = 15;
            this.cbxTaskStatus.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2,
            this.comboItem3,
            this.comboItem4,
            this.comboItem5});
            this.cbxTaskStatus.Location = new System.Drawing.Point(273, 6);
            this.cbxTaskStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbxTaskStatus.Name = "cbxTaskStatus";
            this.cbxTaskStatus.Size = new System.Drawing.Size(106, 21);
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
            // tabGetMaterialTasks
            // 
            this.tabGetMaterialTasks.AttachedControl = this.tabControlPanel1;
            this.tabGetMaterialTasks.Name = "tabGetMaterialTasks";
            this.tabGetMaterialTasks.Text = "临时领料任务";
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Controls.Add(this.pageControlMaterial);
            this.tabControlPanel2.Controls.Add(this.dataGridViewMaterials);
            this.tabControlPanel2.Controls.Add(this.panelEx2);
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel2.Location = new System.Drawing.Point(0, 25);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel2.Size = new System.Drawing.Size(764, 413);
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(188)))), ((int)(((byte)(227)))));
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(165)))), ((int)(((byte)(199)))));
            this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right) 
            | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel2.Style.GradientAngle = 90;
            this.tabControlPanel2.TabIndex = 2;
            this.tabControlPanel2.TabItem = this.tabMaterialQuery;
            // 
            // pageControlMaterial
            // 
            this.pageControlMaterial.BackColor = System.Drawing.SystemColors.Control;
            this.pageControlMaterial.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pageControlMaterial.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pageControlMaterial.Location = new System.Drawing.Point(1, 387);
            this.pageControlMaterial.Name = "pageControlMaterial";
            this.pageControlMaterial.NMax = 0;
            this.pageControlMaterial.PageCount = 0;
            this.pageControlMaterial.PageCurrent = 0;
            this.pageControlMaterial.PageSize = 30;
            this.pageControlMaterial.Size = new System.Drawing.Size(762, 25);
            this.pageControlMaterial.TabIndex = 8;
            this.pageControlMaterial.EventPaging += new WinForm.PageControl.PageControl.EventPagingHandler(this.pageControlMaterial_EventPaging);
            // 
            // dataGridViewMaterials
            // 
            this.dataGridViewMaterials.AllowUserToAddRows = false;
            this.dataGridViewMaterials.AllowUserToDeleteRows = false;
            this.dataGridViewMaterials.AllowUserToOrderColumns = true;
            this.dataGridViewMaterials.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewMaterials.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewMaterials.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMaterials.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID_M,
            this.docNumber,
            this.description,
            this.warehouseName,
            this.totalNumber,
            this.unit,
            this.memo});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewMaterials.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewMaterials.Location = new System.Drawing.Point(1, 36);
            this.dataGridViewMaterials.MultiSelect = false;
            this.dataGridViewMaterials.Name = "dataGridViewMaterials";
            this.dataGridViewMaterials.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewMaterials.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewMaterials.RowTemplate.Height = 23;
            this.dataGridViewMaterials.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMaterials.Size = new System.Drawing.Size(762, 350);
            this.dataGridViewMaterials.TabIndex = 7;
            this.dataGridViewMaterials.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMaterials_CellDoubleClick);
            // 
            // ID_M
            // 
            this.ID_M.DataPropertyName = "ID";
            this.ID_M.HeaderText = "ID";
            this.ID_M.Name = "ID_M";
            this.ID_M.ReadOnly = true;
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
            this.description.Width = 400;
            // 
            // warehouseName
            // 
            this.warehouseName.DataPropertyName = "warehouseName";
            this.warehouseName.HeaderText = "仓库";
            this.warehouseName.Name = "warehouseName";
            this.warehouseName.ReadOnly = true;
            this.warehouseName.Width = 300;
            // 
            // totalNumber
            // 
            this.totalNumber.DataPropertyName = "totalNumber";
            this.totalNumber.HeaderText = "总量";
            this.totalNumber.Name = "totalNumber";
            this.totalNumber.ReadOnly = true;
            // 
            // unit
            // 
            this.unit.DataPropertyName = "unit";
            this.unit.HeaderText = "单位";
            this.unit.Name = "unit";
            this.unit.ReadOnly = true;
            // 
            // memo
            // 
            this.memo.DataPropertyName = "memo";
            this.memo.HeaderText = "备注";
            this.memo.Name = "memo";
            this.memo.ReadOnly = true;
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx2.Controls.Add(this.btnQueryJobs);
            this.panelEx2.Controls.Add(this.txtMaterialDocNumber);
            this.panelEx2.Controls.Add(this.labelX4);
            this.panelEx2.Controls.Add(this.btnMaterialSearch);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx2.Location = new System.Drawing.Point(1, 1);
            this.panelEx2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(762, 35);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarCaptionText;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 6;
            // 
            // btnQueryJobs
            // 
            this.btnQueryJobs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnQueryJobs.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnQueryJobs.Location = new System.Drawing.Point(309, 6);
            this.btnQueryJobs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnQueryJobs.Name = "btnQueryJobs";
            this.btnQueryJobs.Size = new System.Drawing.Size(99, 23);
            this.btnQueryJobs.TabIndex = 5;
            this.btnQueryJobs.Text = "查询关联作业";
            this.btnQueryJobs.Click += new System.EventHandler(this.btnQueryJobs_Click);
            // 
            // txtMaterialDocNumber
            // 
            this.txtMaterialDocNumber.Location = new System.Drawing.Point(79, 7);
            this.txtMaterialDocNumber.Name = "txtMaterialDocNumber";
            this.txtMaterialDocNumber.Size = new System.Drawing.Size(114, 21);
            this.txtMaterialDocNumber.TabIndex = 4;
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            this.labelX4.Location = new System.Drawing.Point(11, 9);
            this.labelX4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(61, 23);
            this.labelX4.TabIndex = 0;
            this.labelX4.Text = "物料编码:";
            // 
            // btnMaterialSearch
            // 
            this.btnMaterialSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnMaterialSearch.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnMaterialSearch.Location = new System.Drawing.Point(213, 6);
            this.btnMaterialSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMaterialSearch.Name = "btnMaterialSearch";
            this.btnMaterialSearch.Size = new System.Drawing.Size(69, 23);
            this.btnMaterialSearch.TabIndex = 3;
            this.btnMaterialSearch.Text = "查询";
            this.btnMaterialSearch.Click += new System.EventHandler(this.btnMaterialSearch_Click);
            // 
            // tabMaterialQuery
            // 
            this.tabMaterialQuery.AttachedControl = this.tabControlPanel2;
            this.tabMaterialQuery.Name = "tabMaterialQuery";
            this.tabMaterialQuery.Text = "仓库物料查询";
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(764, 462);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuBar);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Main_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PAD.领料计划管理模块";
            ((System.ComponentModel.ISupportInitialize)(this.menuBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTasks)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.tabControlPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMaterials)).EndInit();
            this.panelEx2.ResumeLayout(false);
            this.panelEx2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Bar menuBar;
        private DevComponents.DotNetBar.ButtonItem mHelp;
        private DevComponents.DotNetBar.ButtonItem mAboutUs;
        private DevComponents.DotNetBar.TabControl tabControl;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tabGetMaterialTasks;
        private DevComponents.DotNetBar.ButtonItem mTasks;
        private DevComponents.DotNetBar.ButtonItem GetMaterialTasks;
        private DevComponents.DotNetBar.ButtonItem AddMaterialTask;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnTasksSearch;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxTaskStatus;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.DateTimePicker taskCreateTime;
        private DevComponents.Editors.ComboItem comboItem4;
        private DevComponents.Editors.ComboItem comboItem5;
        private System.Windows.Forms.DataGridView dataGridViewTasks;
        private DevComponents.DotNetBar.ButtonX btnTaskDelete;
        private DevComponents.DotNetBar.ButtonX btnTaskEdit;
        private DevComponents.DotNetBar.ButtonX btnTaskAdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn createUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdentificationIdCN;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn mtlCategoryStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn createTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskId;
        private System.Windows.Forms.DataGridViewTextBoxColumn productLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouserId;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryStatusCN;
        private System.Windows.Forms.DataGridViewTextBoxColumn rankerId;
        private System.Windows.Forms.DataGridViewTextBoxColumn workerId;
        private System.Windows.Forms.DataGridViewTextBoxColumn _status;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouserPlace;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdentificationId;
        private System.Windows.Forms.DataGridViewTextBoxColumn materialDocNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn tip;
        private System.Windows.Forms.DataGridViewTextBoxColumn tipCN;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel2;
        private DevComponents.DotNetBar.TabItem tabMaterialQuery;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.ButtonX btnMaterialSearch;
        private System.Windows.Forms.TextBox txtMaterialDocNumber;
        private System.Windows.Forms.DataGridView dataGridViewMaterials;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID_M;
        private System.Windows.Forms.DataGridViewTextBoxColumn docNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn warehouseName;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn memo;
        private DevComponents.DotNetBar.ButtonX btnQueryJobs;
        private PageControl.PageControl pageControlTask;
        private PageControl.PageControl pageControlMaterial;
    }
}