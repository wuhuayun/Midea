namespace WinForm
{
    partial class frmMonitorView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMonitorView));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tscbTaskStatus = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSumMaterial = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbStartTask = new System.Windows.Forms.ToolStripButton();
            this.tsbEndTask = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbGetMaterial = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCheckTask = new System.Windows.Forms.ToolStripButton();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.advTree1 = new DevComponents.AdvTree.AdvTree();
            this.id = new DevComponents.AdvTree.ColumnHeader();
            this.taskId = new DevComponents.AdvTree.ColumnHeader();
            this.IdentificationIdCN = new DevComponents.AdvTree.ColumnHeader();
            this.productLine = new DevComponents.AdvTree.ColumnHeader();
            this.warehouserId = new DevComponents.AdvTree.ColumnHeader();
            this.warehouserPlace = new DevComponents.AdvTree.ColumnHeader();
            this.categoryStatusCN = new DevComponents.AdvTree.ColumnHeader();
            this.materialDocNumber = new DevComponents.AdvTree.ColumnHeader();
            this.taskTime = new DevComponents.AdvTree.ColumnHeader();
            this.statusCN = new DevComponents.AdvTree.ColumnHeader();
            this.totalRequiredQuantity = new DevComponents.AdvTree.ColumnHeader();
            this.totalGetMaterialQuantity = new DevComponents.AdvTree.ColumnHeader();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.pageControlTask = new WinForm.PageControl.PageControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(875, 442);
            this.splitContainer1.SplitterDistance = 29;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscbTaskStatus,
            this.toolStripSeparator1,
            this.tsbSumMaterial,
            this.toolStripSeparator2,
            this.tsbStartTask,
            this.tsbEndTask,
            this.toolStripSeparator3,
            this.tsbGetMaterial,
            this.toolStripSeparator4,
            this.tsbCheckTask});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(875, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tscbTaskStatus
            // 
            this.tscbTaskStatus.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tscbTaskStatus.Name = "tscbTaskStatus";
            this.tscbTaskStatus.Size = new System.Drawing.Size(140, 25);
            this.tscbTaskStatus.TextChanged += new System.EventHandler(this.tscbTaskStatus_TextChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSumMaterial
            // 
            this.tsbSumMaterial.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsbSumMaterial.Image = global::WinForm.Properties.Resources.sum;
            this.tsbSumMaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSumMaterial.Name = "tsbSumMaterial";
            this.tsbSumMaterial.Size = new System.Drawing.Size(83, 22);
            this.tsbSumMaterial.Text = "汇总编码";
            this.tsbSumMaterial.Click += new System.EventHandler(this.tsbSumMaterial_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbStartTask
            // 
            this.tsbStartTask.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsbStartTask.Image = ((System.Drawing.Image)(resources.GetObject("tsbStartTask.Image")));
            this.tsbStartTask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStartTask.Name = "tsbStartTask";
            this.tsbStartTask.Size = new System.Drawing.Size(83, 22);
            this.tsbStartTask.Text = "开始任务";
            this.tsbStartTask.Click += new System.EventHandler(this.tsbStartTask_Click);
            // 
            // tsbEndTask
            // 
            this.tsbEndTask.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsbEndTask.Image = ((System.Drawing.Image)(resources.GetObject("tsbEndTask.Image")));
            this.tsbEndTask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEndTask.Name = "tsbEndTask";
            this.tsbEndTask.Size = new System.Drawing.Size(83, 22);
            this.tsbEndTask.Text = "结束任务";
            this.tsbEndTask.Click += new System.EventHandler(this.tsbEndTask_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbGetMaterial
            // 
            this.tsbGetMaterial.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsbGetMaterial.Image = ((System.Drawing.Image)(resources.GetObject("tsbGetMaterial.Image")));
            this.tsbGetMaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbGetMaterial.Name = "tsbGetMaterial";
            this.tsbGetMaterial.Size = new System.Drawing.Size(55, 22);
            this.tsbGetMaterial.Text = "领料";
            this.tsbGetMaterial.Click += new System.EventHandler(this.tsbGetMaterial_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbCheckTask
            // 
            this.tsbCheckTask.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsbCheckTask.Image = ((System.Drawing.Image)(resources.GetObject("tsbCheckTask.Image")));
            this.tsbCheckTask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCheckTask.Name = "tsbCheckTask";
            this.tsbCheckTask.Size = new System.Drawing.Size(83, 22);
            this.tsbCheckTask.Text = "查看任务";
            this.tsbCheckTask.Click += new System.EventHandler(this.tsbCheckTask_Click);
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
            this.splitContainer2.Panel1.Controls.Add(this.advTree1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pageControlTask);
            this.splitContainer2.Size = new System.Drawing.Size(875, 412);
            this.splitContainer2.SplitterDistance = 375;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // advTree1
            // 
            this.advTree1.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTree1.AllowDrop = true;
            this.advTree1.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTree1.BackgroundStyle.Class = "TreeBorderKey";
            this.advTree1.Columns.Add(this.id);
            this.advTree1.Columns.Add(this.taskId);
            this.advTree1.Columns.Add(this.IdentificationIdCN);
            this.advTree1.Columns.Add(this.productLine);
            this.advTree1.Columns.Add(this.warehouserId);
            this.advTree1.Columns.Add(this.warehouserPlace);
            this.advTree1.Columns.Add(this.categoryStatusCN);
            this.advTree1.Columns.Add(this.materialDocNumber);
            this.advTree1.Columns.Add(this.taskTime);
            this.advTree1.Columns.Add(this.statusCN);
            this.advTree1.Columns.Add(this.totalRequiredQuantity);
            this.advTree1.Columns.Add(this.totalGetMaterialQuantity);
            this.advTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTree1.DragDropEnabled = false;
            this.advTree1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.advTree1.GridColumnLines = false;
            this.advTree1.Location = new System.Drawing.Point(0, 0);
            this.advTree1.Name = "advTree1";
            this.advTree1.NodesConnector = this.nodeConnector1;
            this.advTree1.NodeSpacing = 5;
            this.advTree1.NodeStyle = this.elementStyle1;
            this.advTree1.PathSeparator = ";";
            this.advTree1.Size = new System.Drawing.Size(875, 375);
            this.advTree1.Styles.Add(this.elementStyle1);
            this.advTree1.TabIndex = 1;
            this.advTree1.Text = "advTree1";
            this.advTree1.NodeDoubleClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTree1_NodeDoubleClick);
            // 
            // id
            // 
            this.id.ColumnName = "id";
            this.id.DataFieldName = "id";
            this.id.Name = "id";
            this.id.Width.Absolute = 45;
            // 
            // taskId
            // 
            this.taskId.ColumnName = "taskId";
            this.taskId.DataFieldName = "taskId";
            this.taskId.Name = "taskId";
            this.taskId.Text = "任务号";
            this.taskId.Width.Absolute = 60;
            // 
            // IdentificationIdCN
            // 
            this.IdentificationIdCN.ColumnName = "IdentificationIdCN";
            this.IdentificationIdCN.DataFieldName = "IdentificationIdCN";
            this.IdentificationIdCN.Name = "IdentificationIdCN";
            this.IdentificationIdCN.Text = "类型";
            this.IdentificationIdCN.Width.Absolute = 50;
            // 
            // productLine
            // 
            this.productLine.ColumnName = "productLine";
            this.productLine.DataFieldName = "productLine";
            this.productLine.Name = "productLine";
            this.productLine.Text = "生产线";
            this.productLine.Width.Absolute = 60;
            // 
            // warehouserId
            // 
            this.warehouserId.ColumnName = "warehouserId";
            this.warehouserId.DataFieldName = "warehouserId";
            this.warehouserId.Name = "warehouserId";
            this.warehouserId.Text = "仓库";
            this.warehouserId.Width.Absolute = 70;
            // 
            // warehouserPlace
            // 
            this.warehouserPlace.ColumnName = "warehouserPlace";
            this.warehouserPlace.DataFieldName = "warehouserPlace";
            this.warehouserPlace.Name = "warehouserPlace";
            this.warehouserPlace.Text = "仓库位置";
            this.warehouserPlace.Visible = false;
            this.warehouserPlace.Width.Absolute = 150;
            // 
            // categoryStatusCN
            // 
            this.categoryStatusCN.ColumnName = "categoryStatusCN";
            this.categoryStatusCN.DataFieldName = "categoryStatusCN";
            this.categoryStatusCN.Name = "categoryStatusCN";
            this.categoryStatusCN.Text = "类别状态";
            this.categoryStatusCN.Width.Absolute = 80;
            // 
            // materialDocNumber
            // 
            this.materialDocNumber.ColumnName = "materialDocNumber";
            this.materialDocNumber.DataFieldName = "materialDocNumber";
            this.materialDocNumber.Name = "materialDocNumber";
            this.materialDocNumber.Text = "物料编码";
            this.materialDocNumber.Width.Absolute = 110;
            // 
            // taskTime
            // 
            this.taskTime.ColumnName = "taskTime";
            this.taskTime.DataFieldName = "taskTime";
            this.taskTime.Name = "taskTime";
            this.taskTime.Text = "时间";
            this.taskTime.Width.Absolute = 70;
            // 
            // statusCN
            // 
            this.statusCN.ColumnName = "statusCN";
            this.statusCN.DataFieldName = "statusCN";
            this.statusCN.Name = "statusCN";
            this.statusCN.Text = "状态";
            this.statusCN.Width.Absolute = 60;
            // 
            // totalRequiredQuantity
            // 
            this.totalRequiredQuantity.ColumnName = "totalRequiredQuantity";
            this.totalRequiredQuantity.DataFieldName = "totalRequiredQuantity";
            this.totalRequiredQuantity.Name = "totalRequiredQuantity";
            this.totalRequiredQuantity.Text = "需求数";
            this.totalRequiredQuantity.Width.Absolute = 80;
            // 
            // totalGetMaterialQuantity
            // 
            this.totalGetMaterialQuantity.ColumnName = "totalGetMaterialQuantity";
            this.totalGetMaterialQuantity.DataFieldName = "totalGetMaterialQuantity";
            this.totalGetMaterialQuantity.Name = "totalGetMaterialQuantity";
            this.totalGetMaterialQuantity.Text = "领料数";
            this.totalGetMaterialQuantity.Width.Absolute = 80;
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // pageControlTask
            // 
            this.pageControlTask.BackColor = System.Drawing.SystemColors.Control;
            this.pageControlTask.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pageControlTask.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pageControlTask.Location = new System.Drawing.Point(0, 3);
            this.pageControlTask.Name = "pageControlTask";
            this.pageControlTask.NMax = 0;
            this.pageControlTask.PageCount = 0;
            this.pageControlTask.PageCurrent = 0;
            this.pageControlTask.PageSize = 30;
            this.pageControlTask.Size = new System.Drawing.Size(875, 29);
            this.pageControlTask.TabIndex = 0;
            this.pageControlTask.EventPaging += new WinForm.PageControl.PageControl.EventPagingHandler(this.pageControlTask_EventPaging);
            // 
            // frmMonitorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 442);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "frmMonitorView";
            this.ShowIcon = false;
            this.Text = "监控视图";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMonitorView_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox tscbTaskStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbSumMaterial;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbStartTask;
        private System.Windows.Forms.ToolStripButton tsbEndTask;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbGetMaterial;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private DevComponents.AdvTree.AdvTree advTree1;
        private DevComponents.AdvTree.ColumnHeader taskId;
        private DevComponents.AdvTree.ColumnHeader IdentificationIdCN;
        private DevComponents.AdvTree.ColumnHeader productLine;
        private DevComponents.AdvTree.ColumnHeader warehouserId;
        private DevComponents.AdvTree.ColumnHeader warehouserPlace;
        private DevComponents.AdvTree.ColumnHeader categoryStatusCN;
        private DevComponents.AdvTree.ColumnHeader materialDocNumber;
        private DevComponents.AdvTree.ColumnHeader taskTime;
        private DevComponents.AdvTree.ColumnHeader statusCN;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private PageControl.PageControl pageControlTask;
        private DevComponents.AdvTree.ColumnHeader id;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbCheckTask;
        private DevComponents.AdvTree.ColumnHeader totalRequiredQuantity;
        private DevComponents.AdvTree.ColumnHeader totalGetMaterialQuantity;
    }
}