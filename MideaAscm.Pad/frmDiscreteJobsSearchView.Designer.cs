namespace MideaAscm.Pad
{
    partial class frmDiscreteJobsSearchView
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
            this.btnSearch = new DevComponents.DotNetBar.ButtonItem();
            this.pagerControlJobs = new MideaAscm.Pad.CustomControl.PagerControl();
            this.dgViewDiscreteJobs = new DevComponents.DotNetBar.Controls.DataGridViewX();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewDiscreteJobs)).BeginInit();
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
            this.btnSearch});
            this.bar1.Location = new System.Drawing.Point(0, 0);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(669, 30);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bar1.TabIndex = 0;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // labelItem1
            // 
            this.labelItem1.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.labelItem2.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            // btnSearch
            // 
            this.btnSearch.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnSearch.Image = global::MideaAscm.Pad.Properties.Resources.search;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // pagerControlJobs
            // 
            this.pagerControlJobs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pagerControlJobs.Location = new System.Drawing.Point(0, 333);
            this.pagerControlJobs.Name = "pagerControlJobs";
            this.pagerControlJobs.NMax = 0;
            this.pagerControlJobs.PageCount = 0;
            this.pagerControlJobs.PageCurrent = 0;
            this.pagerControlJobs.PageSize = 30;
            this.pagerControlJobs.Size = new System.Drawing.Size(669, 25);
            this.pagerControlJobs.TabIndex = 1;
            this.pagerControlJobs.EventPaging += new MideaAscm.Pad.CustomControl.PagerControl.EventPagingHandler(this.pagerControlJobs_EventPaging);
            // 
            // dgViewDiscreteJobs
            // 
            this.dgViewDiscreteJobs.AllowUserToAddRows = false;
            this.dgViewDiscreteJobs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgViewDiscreteJobs.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgViewDiscreteJobs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgViewDiscreteJobs.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.dgViewDiscreteJobs.Location = new System.Drawing.Point(0, 30);
            this.dgViewDiscreteJobs.Name = "dgViewDiscreteJobs";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgViewDiscreteJobs.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgViewDiscreteJobs.RowTemplate.Height = 23;
            this.dgViewDiscreteJobs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgViewDiscreteJobs.Size = new System.Drawing.Size(669, 303);
            this.dgViewDiscreteJobs.TabIndex = 2;
            // 
            // frmDiscreteJobsSearchView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(669, 358);
            this.Controls.Add(this.dgViewDiscreteJobs);
            this.Controls.Add(this.pagerControlJobs);
            this.Controls.Add(this.bar1);
            this.DoubleBuffered = true;
            this.Name = "frmDiscreteJobsSearchView";
            this.Text = "排产单管理视图";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmDiscreteJobsSearchView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewDiscreteJobs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.TextBoxItem tbItemStart;
        private DevComponents.DotNetBar.LabelItem labelItem2;
        private DevComponents.DotNetBar.TextBoxItem tbItemEnd;
        private DevComponents.DotNetBar.ButtonItem btnSearch;
        private CustomControl.PagerControl pagerControlJobs;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgViewDiscreteJobs;

    }
}