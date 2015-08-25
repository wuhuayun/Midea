namespace MideaAscm.Pad
{
    partial class frmAddTaskForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddTaskForm));
            this.ribbonControl1 = new DevComponents.DotNetBar.RibbonControl();
            this.qatCustomizeItem1 = new DevComponents.DotNetBar.QatCustomizeItem();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.superValidator1 = new DevComponents.DotNetBar.Validator.SuperValidator();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.highlighter1 = new DevComponents.DotNetBar.Validator.Highlighter();
            this.cbTipCn = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.requiredFieldValidator3 = new DevComponents.DotNetBar.Validator.RequiredFieldValidator("Your error message here.");
            this.requiredFieldValidator2 = new DevComponents.DotNetBar.Validator.RequiredFieldValidator("Your error message here.");
            this.tbProductLine = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.cbMaterial = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cbRelated = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnAdd = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.requiredFieldValidator1 = new DevComponents.DotNetBar.Validator.RequiredFieldValidator("Your error message here.");
            this.cbMtlCategoryStatus = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cbTaskType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cbTaskTime = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnLinkMaterial = new DevComponents.DotNetBar.ButtonX();
            this.btnLinkMark = new DevComponents.DotNetBar.ButtonX();
            this.cbWarehouse = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnLinkWarehouse = new DevComponents.DotNetBar.ButtonX();
            this.requiredFieldValidator4 = new DevComponents.DotNetBar.Validator.RequiredFieldValidator("Your error message here.");
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            // 
            // 
            // 
            this.ribbonControl1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonControl1.CaptionVisible = true;
            this.ribbonControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ribbonControl1.KeyTipsFont = new System.Drawing.Font("Tahoma", 7F);
            this.ribbonControl1.Location = new System.Drawing.Point(5, 1);
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.ribbonControl1.QuickToolbarItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.qatCustomizeItem1});
            this.ribbonControl1.Size = new System.Drawing.Size(590, 28);
            this.ribbonControl1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonControl1.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
            this.ribbonControl1.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
            this.ribbonControl1.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
            this.ribbonControl1.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
            this.ribbonControl1.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
            this.ribbonControl1.SystemText.QatDialogAddButton = "&Add >>";
            this.ribbonControl1.SystemText.QatDialogCancelButton = "Cancel";
            this.ribbonControl1.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
            this.ribbonControl1.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
            this.ribbonControl1.SystemText.QatDialogOkButton = "OK";
            this.ribbonControl1.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
            this.ribbonControl1.SystemText.QatDialogRemoveButton = "&Remove";
            this.ribbonControl1.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
            this.ribbonControl1.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
            this.ribbonControl1.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
            this.ribbonControl1.TabGroupHeight = 14;
            this.ribbonControl1.TabIndex = 0;
            this.ribbonControl1.Text = "ribbonControl1";
            // 
            // qatCustomizeItem1
            // 
            this.qatCustomizeItem1.Name = "qatCustomizeItem1";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX1.Location = new System.Drawing.Point(49, 46);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "仓库：";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX2.Location = new System.Drawing.Point(34, 101);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "生产线：";
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX3.Location = new System.Drawing.Point(22, 158);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(83, 23);
            this.labelX3.TabIndex = 3;
            this.labelX3.Text = "上线时间：";
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX4.Location = new System.Drawing.Point(22, 216);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(83, 23);
            this.labelX4.TabIndex = 4;
            this.labelX4.Text = "物料编码：";
            // 
            // labelX5
            // 
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX5.Location = new System.Drawing.Point(308, 45);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(84, 23);
            this.labelX5.TabIndex = 5;
            this.labelX5.Text = "作业内容：";
            // 
            // labelX6
            // 
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX6.Location = new System.Drawing.Point(308, 101);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(84, 23);
            this.labelX6.TabIndex = 6;
            this.labelX6.Text = "备料形式：";
            // 
            // labelX7
            // 
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX7.Location = new System.Drawing.Point(336, 158);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(75, 23);
            this.labelX7.TabIndex = 7;
            this.labelX7.Text = "类型：";
            // 
            // labelX8
            // 
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX8.Location = new System.Drawing.Point(308, 216);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(84, 23);
            this.labelX8.TabIndex = 8;
            this.labelX8.Text = "关联标记：";
            // 
            // superValidator1
            // 
            this.superValidator1.ContainerControl = this;
            this.superValidator1.ErrorProvider = this.errorProvider1;
            this.superValidator1.Highlighter = this.highlighter1;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.Icon = ((System.Drawing.Icon)(resources.GetObject("errorProvider1.Icon")));
            // 
            // highlighter1
            // 
            this.highlighter1.ContainerControl = this;
            // 
            // cbTipCn
            // 
            this.cbTipCn.DisplayMember = "Text";
            this.cbTipCn.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbTipCn.FormattingEnabled = true;
            this.highlighter1.SetHighlightColor(this.cbTipCn, DevComponents.DotNetBar.Validator.eHighlightColor.Red);
            this.highlighter1.SetHighlightOnFocus(this.cbTipCn, true);
            this.cbTipCn.ItemHeight = 15;
            this.cbTipCn.Location = new System.Drawing.Point(394, 47);
            this.cbTipCn.Name = "cbTipCn";
            this.cbTipCn.Size = new System.Drawing.Size(166, 21);
            this.cbTipCn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbTipCn.TabIndex = 21;
            this.superValidator1.SetValidator1(this.cbTipCn, this.requiredFieldValidator3);
            // 
            // requiredFieldValidator3
            // 
            this.requiredFieldValidator3.ErrorMessage = "Your error message here.";
            this.requiredFieldValidator3.HighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            // 
            // requiredFieldValidator2
            // 
            this.requiredFieldValidator2.ErrorMessage = "Your error message here.";
            this.requiredFieldValidator2.HighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            // 
            // tbProductLine
            // 
            // 
            // 
            // 
            this.tbProductLine.Border.Class = "TextBoxBorder";
            this.tbProductLine.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbProductLine.Location = new System.Drawing.Point(101, 101);
            this.tbProductLine.Name = "tbProductLine";
            this.tbProductLine.Size = new System.Drawing.Size(166, 21);
            this.tbProductLine.TabIndex = 11;
            // 
            // cbMaterial
            // 
            this.cbMaterial.DisplayMember = "Text";
            this.cbMaterial.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbMaterial.FormattingEnabled = true;
            this.cbMaterial.ItemHeight = 15;
            this.cbMaterial.Location = new System.Drawing.Point(101, 218);
            this.cbMaterial.Name = "cbMaterial";
            this.cbMaterial.Size = new System.Drawing.Size(145, 21);
            this.cbMaterial.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbMaterial.TabIndex = 16;
            this.cbMaterial.Tag = "AscmMaterialItem";
            this.cbMaterial.SelectedIndexChanged += new System.EventHandler(this.ComboBoxEx_SelectedIndexChanged);
            // 
            // cbRelated
            // 
            this.cbRelated.DisplayMember = "Text";
            this.cbRelated.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbRelated.FormattingEnabled = true;
            this.cbRelated.ItemHeight = 15;
            this.cbRelated.Location = new System.Drawing.Point(394, 214);
            this.cbRelated.Name = "cbRelated";
            this.cbRelated.Size = new System.Drawing.Size(146, 21);
            this.cbRelated.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbRelated.TabIndex = 17;
            this.cbRelated.Tag = "";
            this.cbRelated.DataColumnCreated += new DevComponents.DotNetBar.Controls.DataColumnEventHandler(this.cbRelated_DataColumnCreated);
            this.cbRelated.SelectedIndexChanged += new System.EventHandler(this.ComboBoxEx_SelectedIndexChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAdd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAdd.Image = global::MideaAscm.Pad.Properties.Resources.filesave;
            this.btnAdd.Location = new System.Drawing.Point(336, 275);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(87, 33);
            this.btnAdd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAdd.TabIndex = 18;
            this.btnAdd.Text = "保存";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Image = global::MideaAscm.Pad.Properties.Resources.cancel;
            this.btnCancel.Location = new System.Drawing.Point(185, 275);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(93, 34);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // requiredFieldValidator1
            // 
            this.requiredFieldValidator1.ErrorMessage = "Your error message here.";
            this.requiredFieldValidator1.HighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            // 
            // cbMtlCategoryStatus
            // 
            this.cbMtlCategoryStatus.DisplayMember = "Text";
            this.cbMtlCategoryStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbMtlCategoryStatus.FormattingEnabled = true;
            this.cbMtlCategoryStatus.ItemHeight = 15;
            this.cbMtlCategoryStatus.Location = new System.Drawing.Point(394, 101);
            this.cbMtlCategoryStatus.Name = "cbMtlCategoryStatus";
            this.cbMtlCategoryStatus.Size = new System.Drawing.Size(166, 21);
            this.cbMtlCategoryStatus.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbMtlCategoryStatus.TabIndex = 22;
            // 
            // cbTaskType
            // 
            this.cbTaskType.DisplayMember = "Text";
            this.cbTaskType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbTaskType.FormattingEnabled = true;
            this.cbTaskType.ItemHeight = 15;
            this.cbTaskType.Location = new System.Drawing.Point(394, 159);
            this.cbTaskType.Name = "cbTaskType";
            this.cbTaskType.Size = new System.Drawing.Size(166, 21);
            this.cbTaskType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbTaskType.TabIndex = 23;
            // 
            // cbTaskTime
            // 
            this.cbTaskTime.DisplayMember = "Text";
            this.cbTaskTime.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbTaskTime.FormattingEnabled = true;
            this.cbTaskTime.ItemHeight = 15;
            this.cbTaskTime.Location = new System.Drawing.Point(101, 160);
            this.cbTaskTime.Name = "cbTaskTime";
            this.cbTaskTime.Size = new System.Drawing.Size(166, 21);
            this.cbTaskTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbTaskTime.TabIndex = 25;
            // 
            // btnLinkMaterial
            // 
            this.btnLinkMaterial.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLinkMaterial.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLinkMaterial.Image = global::MideaAscm.Pad.Properties.Resources.link;
            this.btnLinkMaterial.Location = new System.Drawing.Point(246, 217);
            this.btnLinkMaterial.Name = "btnLinkMaterial";
            this.btnLinkMaterial.Size = new System.Drawing.Size(20, 21);
            this.btnLinkMaterial.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLinkMaterial.TabIndex = 26;
            this.btnLinkMaterial.Click += new System.EventHandler(this.btnLinkMaterial_Click);
            // 
            // btnLinkMark
            // 
            this.btnLinkMark.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLinkMark.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLinkMark.Image = global::MideaAscm.Pad.Properties.Resources.link;
            this.btnLinkMark.Location = new System.Drawing.Point(540, 214);
            this.btnLinkMark.Name = "btnLinkMark";
            this.btnLinkMark.Size = new System.Drawing.Size(20, 21);
            this.btnLinkMark.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLinkMark.TabIndex = 27;
            this.btnLinkMark.Click += new System.EventHandler(this.btnLinkMark_Click);
            // 
            // cbWarehouse
            // 
            this.cbWarehouse.DisplayMember = "Text";
            this.cbWarehouse.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbWarehouse.FormattingEnabled = true;
            this.highlighter1.SetHighlightColor(this.cbWarehouse, DevComponents.DotNetBar.Validator.eHighlightColor.Red);
            this.highlighter1.SetHighlightOnFocus(this.cbWarehouse, true);
            this.cbWarehouse.ItemHeight = 15;
            this.cbWarehouse.Location = new System.Drawing.Point(101, 47);
            this.cbWarehouse.Name = "cbWarehouse";
            this.cbWarehouse.Size = new System.Drawing.Size(145, 21);
            this.cbWarehouse.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbWarehouse.TabIndex = 28;
            this.superValidator1.SetValidator1(this.cbWarehouse, this.requiredFieldValidator4);
            this.cbWarehouse.SelectedIndexChanged += new System.EventHandler(this.ComboBoxEx_SelectedIndexChanged);
            // 
            // btnLinkWarehouse
            // 
            this.btnLinkWarehouse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLinkWarehouse.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLinkWarehouse.Image = global::MideaAscm.Pad.Properties.Resources.link;
            this.btnLinkWarehouse.Location = new System.Drawing.Point(247, 47);
            this.btnLinkWarehouse.Name = "btnLinkWarehouse";
            this.btnLinkWarehouse.Size = new System.Drawing.Size(20, 21);
            this.btnLinkWarehouse.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLinkWarehouse.TabIndex = 29;
            this.btnLinkWarehouse.Click += new System.EventHandler(this.btnLinkWarehouse_Click);
            // 
            // requiredFieldValidator4
            // 
            this.requiredFieldValidator4.ErrorMessage = "Your error message here.";
            this.requiredFieldValidator4.HighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            // 
            // frmAddTaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 353);
            this.Controls.Add(this.btnLinkWarehouse);
            this.Controls.Add(this.cbWarehouse);
            this.Controls.Add(this.btnLinkMark);
            this.Controls.Add(this.btnLinkMaterial);
            this.Controls.Add(this.cbTaskTime);
            this.Controls.Add(this.cbTaskType);
            this.Controls.Add(this.cbMtlCategoryStatus);
            this.Controls.Add(this.cbTipCn);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cbRelated);
            this.Controls.Add(this.cbMaterial);
            this.Controls.Add(this.tbProductLine);
            this.Controls.Add(this.labelX8);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "frmAddTaskForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加任务管理";
            this.Load += new System.EventHandler(this.frmAddTaskForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.RibbonControl ribbonControl1;
        private DevComponents.DotNetBar.QatCustomizeItem qatCustomizeItem1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.Validator.SuperValidator superValidator1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DevComponents.DotNetBar.Validator.Highlighter highlighter1;
        private DevComponents.DotNetBar.Controls.TextBoxX tbProductLine;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnAdd;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbRelated;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbMaterial;
        private DevComponents.DotNetBar.Validator.RequiredFieldValidator requiredFieldValidator1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbTipCn;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbMtlCategoryStatus;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbTaskType;
        private DevComponents.DotNetBar.Validator.RequiredFieldValidator requiredFieldValidator2;
        private DevComponents.DotNetBar.Validator.RequiredFieldValidator requiredFieldValidator3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbTaskTime;
        private DevComponents.DotNetBar.ButtonX btnLinkMaterial;
        private DevComponents.DotNetBar.ButtonX btnLinkMark;
        private DevComponents.DotNetBar.ButtonX btnLinkWarehouse;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbWarehouse;
        private DevComponents.DotNetBar.Validator.RequiredFieldValidator requiredFieldValidator4;
    }
}