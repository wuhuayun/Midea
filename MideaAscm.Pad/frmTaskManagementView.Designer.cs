namespace MideaAscm.Pad
{
    partial class frmTaskManagementView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.tbItemStart = new DevComponents.DotNetBar.TextBoxItem();
            this.labelItem2 = new DevComponents.DotNetBar.LabelItem();
            this.tbItemEnd = new DevComponents.DotNetBar.TextBoxItem();
            this.labelItem3 = new DevComponents.DotNetBar.LabelItem();
            this.cbItemTaskStatus = new DevComponents.DotNetBar.ComboBoxItem();
            this.btnItemSearch = new DevComponents.DotNetBar.ButtonItem();
            this.btnItemAdd = new DevComponents.DotNetBar.ButtonItem();
            this.btnItemDelete = new DevComponents.DotNetBar.ButtonItem();
            this.dgViewTaskList = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.pagerControlTask = new MideaAscm.Pad.CustomControl.PagerControl();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewTaskList)).BeginInit();
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
            this.labelItem3,
            this.cbItemTaskStatus,
            this.btnItemSearch,
            this.btnItemAdd,
            this.btnItemDelete});
            this.bar1.Location = new System.Drawing.Point(0, 0);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(859, 31);
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
            this.tbItemStart.TextBoxWidth = 85;
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
            this.tbItemEnd.TextBoxWidth = 85;
            this.tbItemEnd.WatermarkColor = System.Drawing.SystemColors.GrayText;
            this.tbItemEnd.GotFocus += new System.EventHandler(this.textBoxItem_GotFocus);
            // 
            // labelItem3
            // 
            this.labelItem3.ForeColor = System.Drawing.Color.Black;
            this.labelItem3.Name = "labelItem3";
            this.labelItem3.Text = "任务状态：";
            // 
            // cbItemTaskStatus
            // 
            this.cbItemTaskStatus.ComboWidth = 80;
            this.cbItemTaskStatus.DropDownHeight = 106;
            this.cbItemTaskStatus.ItemHeight = 20;
            this.cbItemTaskStatus.Name = "cbItemTaskStatus";
            // 
            // btnItemSearch
            // 
            this.btnItemSearch.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnItemSearch.Image = global::MideaAscm.Pad.Properties.Resources.search;
            this.btnItemSearch.Name = "btnItemSearch";
            this.btnItemSearch.Text = "任务查询";
            this.btnItemSearch.Click += new System.EventHandler(this.btnItemSearch_Click);
            // 
            // btnItemAdd
            // 
            this.btnItemAdd.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnItemAdd.Image = global::MideaAscm.Pad.Properties.Resources.edit_add;
            this.btnItemAdd.Name = "btnItemAdd";
            this.btnItemAdd.Text = "增加";
            this.btnItemAdd.Click += new System.EventHandler(this.btnItemAdd_Click);
            // 
            // btnItemDelete
            // 
            this.btnItemDelete.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnItemDelete.Image = global::MideaAscm.Pad.Properties.Resources.cancel;
            this.btnItemDelete.Name = "btnItemDelete";
            this.btnItemDelete.Text = "删除";
            this.btnItemDelete.Click += new System.EventHandler(this.btnItemDelete_Click);
            // 
            // dgViewTaskList
            // 
            this.dgViewTaskList.AllowUserToAddRows = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgViewTaskList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgViewTaskList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgViewTaskList.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgViewTaskList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgViewTaskList.EnableHeadersVisualStyles = false;
            this.dgViewTaskList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgViewTaskList.Location = new System.Drawing.Point(0, 31);
            this.dgViewTaskList.Name = "dgViewTaskList";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgViewTaskList.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgViewTaskList.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgViewTaskList.RowTemplate.Height = 23;
            this.dgViewTaskList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgViewTaskList.Size = new System.Drawing.Size(859, 356);
            this.dgViewTaskList.TabIndex = 2;
            // 
            // pagerControlTask
            // 
            this.pagerControlTask.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pagerControlTask.Location = new System.Drawing.Point(0, 387);
            this.pagerControlTask.Name = "pagerControlTask";
            this.pagerControlTask.NMax = 0;
            this.pagerControlTask.PageCount = 0;
            this.pagerControlTask.PageCurrent = 0;
            this.pagerControlTask.PageSize = 30;
            this.pagerControlTask.Size = new System.Drawing.Size(859, 25);
            this.pagerControlTask.TabIndex = 1;
            this.pagerControlTask.EventPaging += new MideaAscm.Pad.CustomControl.PagerControl.EventPagingHandler(this.pagerControlTask_EventPaging);
            // 
            // frmTaskManagementView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(859, 412);
            this.Controls.Add(this.dgViewTaskList);
            this.Controls.Add(this.pagerControlTask);
            this.Controls.Add(this.bar1);
            this.DoubleBuffered = true;
            this.Name = "frmTaskManagementView";
            this.Text = "领料任务管理视图";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmTaskManagementView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewTaskList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Bar bar1;
        private CustomControl.PagerControl pagerControlTask;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgViewTaskList;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.TextBoxItem tbItemStart;
        private DevComponents.DotNetBar.LabelItem labelItem2;
        private DevComponents.DotNetBar.TextBoxItem tbItemEnd;
        private DevComponents.DotNetBar.LabelItem labelItem3;
        private DevComponents.DotNetBar.ComboBoxItem cbItemTaskStatus;
        private DevComponents.DotNetBar.ButtonItem btnItemSearch;
        private DevComponents.DotNetBar.ButtonItem btnItemAdd;
        private DevComponents.DotNetBar.ButtonItem btnItemDelete;

    }
}