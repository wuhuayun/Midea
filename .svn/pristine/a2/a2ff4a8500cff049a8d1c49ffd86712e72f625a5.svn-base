using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using MideaAscm.Dal.GetMaterialManage.Entities;
using Newtonsoft.Json;

namespace MideaAscm.Pad
{
    public partial class frmDiscreteJobsSearchView : DevComponents.DotNetBar.OfficeForm
    {
        private MonthCalendar monthCalendar;  
        
        public frmDiscreteJobsSearchView()
        {
            InitializeComponent();
        }

        public void FormActivated()
        {

        }

        private void frmDiscreteJobsSearchView_Load(object sender, EventArgs e)
        {
            tbItemStart.Text = DateTime.Now.ToString("yyyy-MM-dd");

            StructureDataGridViewColumns(this.dgViewDiscreteJobs);
            pagerControlJobs.Bind();
        }

        #region Combination Of Controls(Textbox And MonthCalendar)
        private void textBoxItem_GotFocus(object sender, EventArgs e)
        {
            if (this.Controls.Contains(this.monthCalendar))
                this.Controls.Remove(this.monthCalendar);

            BaseItem baseItem = sender as BaseItem;
            if (baseItem.Tag != null)
                baseItem.Tag = baseItem.Focused;

            if (baseItem != null && baseItem.Focused)
            {
                monthCalendar = new MonthCalendar();
                this.monthCalendar.Location = new System.Drawing.Point(89, 26);
                this.monthCalendar.Name = "monthCalendar";
                this.monthCalendar.TabIndex = 10;


                if (this.monthCalendar.Width > baseItem.DisplayRectangle.Location.X)
                    this.monthCalendar.Location = new System.Drawing.Point(baseItem.DisplayRectangle.Location.X, this.monthCalendar.Location.Y);
                else
                    this.monthCalendar.Location = new System.Drawing.Point(baseItem.DisplayRectangle.Location.X + baseItem.DisplayRectangle.Size.Width - this.monthCalendar.DisplayRectangle.Size.Width, this.monthCalendar.Location.Y);

                this.monthCalendar.Tag = sender;
                this.monthCalendar.DateSelected += new DateRangeEventHandler(monthCalendar_DateSelected);
                this.monthCalendar.MouseLeave += new EventHandler(monthCalendar_MouseLeave);

                this.Controls.Add(this.monthCalendar);
                this.monthCalendar.Show();
                this.monthCalendar.BringToFront();
            }
            this.Focus();
        }

        private void monthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            BaseItem baseItem = monthCalendar.Tag as BaseItem;
            if (baseItem != null)
                baseItem.Text = e.Start.Date.ToString("yyyy-MM-dd");
        }

        private void monthCalendar_MouseLeave(object sender, EventArgs e)
        {
            ((MonthCalendar)sender).Dispose();
        }
        #endregion

        private void StructureDataGridViewColumns(DataGridViewX dataGridViewX)
        {
            if (dataGridViewX.Columns.Count > 0)
                dataGridViewX.Columns.Clear();

            DataGridViewTextBoxColumn id = new DataGridViewTextBoxColumn();
            id.HeaderText = "ID";
            id.Name = "id";
            id.DataPropertyName = "id";
            id.Visible = false;
            id.Width = 40;

            DataGridViewTextBoxColumn sIdentificationId = new DataGridViewTextBoxColumn();
            sIdentificationId.HeaderText = "类型";
            sIdentificationId.Name = "sIdentificationId";
            sIdentificationId.DataPropertyName = "sIdentificationId";
            sIdentificationId.Width = 60;

            DataGridViewTextBoxColumn jobId = new DataGridViewTextBoxColumn();
            jobId.HeaderText = "作业号";
            jobId.Name = "jobId";
            jobId.DataPropertyName = "jobId";
            jobId.Width = 130;

            DataGridViewTextBoxColumn jobDate = new DataGridViewTextBoxColumn();
            jobDate.HeaderText = "作业日期";
            jobDate.Name = "jobDate";
            jobDate.DataPropertyName = "jobDate";
            jobDate.Width = 90;

            DataGridViewTextBoxColumn jobInfoId = new DataGridViewTextBoxColumn();
            jobInfoId.HeaderText = "装配件";
            jobInfoId.Name = "jobInfoId";
            jobInfoId.DataPropertyName = "jobInfoId";
            jobInfoId.Width = 110;

            DataGridViewTextBoxColumn jobDesc = new DataGridViewTextBoxColumn();
            jobDesc.HeaderText = "装配件描述";
            jobDesc.Name = "jobDesc";
            jobDesc.DataPropertyName = "jobDesc";
            jobDesc.Width = 150;

            DataGridViewTextBoxColumn count = new DataGridViewTextBoxColumn();
            count.HeaderText = "数量";
            count.Name = "count";
            count.DataPropertyName = "count";
            count.Width = 70;

            DataGridViewTextBoxColumn lineAndSequence = new DataGridViewTextBoxColumn();
            lineAndSequence.HeaderText = "生产线";
            lineAndSequence.Name = "lineAndSequence";
            lineAndSequence.DataPropertyName = "lineAndSequence";
            lineAndSequence.Width = 70;

            DataGridViewTextBoxColumn tip = new DataGridViewTextBoxColumn();
            tip.HeaderText = "备注";
            tip.Name = "tip";
            tip.DataPropertyName = "tip";
            tip.Width = 70;

            DataGridViewTextBoxColumn onlineTime = new DataGridViewTextBoxColumn();
            onlineTime.HeaderText = "上线时间";
            onlineTime.Name = "onlineTime";
            onlineTime.DataPropertyName = "onlineTime";
            onlineTime.Width = 80;

            DataGridViewTextBoxColumn which = new DataGridViewTextBoxColumn();
            which.HeaderText = "第几次";
            which.Name = "which";
            which.DataPropertyName = "which";
            which.Width = 70;

            DataGridViewTextBoxColumn rankerName = new DataGridViewTextBoxColumn();
            rankerName.HeaderText = "所属排产员";
            rankerName.Name = "rankerName";
            rankerName.DataPropertyName = "rankerName";
            rankerName.Width = 90;

            DataGridViewTextBoxColumn time = new DataGridViewTextBoxColumn();
            time.HeaderText = "时间";
            time.Name = "time";
            time.DataPropertyName = "time";
            time.Width = 80;

            DataGridViewTextBoxColumn _status = new DataGridViewTextBoxColumn();
            _status.HeaderText = "状态";
            _status.Name = "_status";
            _status.DataPropertyName = "_status";
            _status.Width = 70;

            dataGridViewX.Columns.Add(id);
            dataGridViewX.Columns.Add(sIdentificationId);
            dataGridViewX.Columns.Add(jobId);
            dataGridViewX.Columns.Add(jobDate);
            dataGridViewX.Columns.Add(jobInfoId);
            dataGridViewX.Columns.Add(jobDesc);
            dataGridViewX.Columns.Add(count);
            dataGridViewX.Columns.Add(lineAndSequence);
            dataGridViewX.Columns.Add(tip);
            dataGridViewX.Columns.Add(onlineTime);
            dataGridViewX.Columns.Add(which);
            dataGridViewX.Columns.Add(rankerName);
            dataGridViewX.Columns.Add(time);
            dataGridViewX.Columns.Add(_status);
        }

        private List<AscmDiscreteJobs> DataBindDataGridView(ref int count, string queryStartTime, string queryEndTime)
        {
            List<AscmDiscreteJobs> list = null;
            string message = string.Empty;
            string _ynPage = string.Empty;

            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(pagerControlJobs.PageSize);
            ynPage.SetCurrentPage((pagerControlJobs.PageCurrent <= 0) ? 1 : pagerControlJobs.PageCurrent);
            _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);

            try
            {
                AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                string jsonString = Service.GetDiscreteJobsList(frmMainView.encryptTicket, frmMainView.userName, queryStartTime, queryEndTime, ref _ynPage, ref message);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    list = (List<AscmDiscreteJobs>)JsonConvert.DeserializeObject(jsonString, typeof(List<AscmDiscreteJobs>));
                    if (list != null && list.Count > 0)
                    {
                        ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);
                        count = ynPage.GetRecordCount();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(tbItemStart.Text) && string.IsNullOrEmpty(tbItemEnd.Text))
                    throw new Exception("查询排产单失败：请选择起止日期！");

                pagerControlJobs.Bind();

            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int pagerControlJobs_EventPaging(CustomControl.EventPagingArg e)
        {
            int count = 0;
            List<AscmDiscreteJobs> list = DataBindDataGridView(ref count, tbItemStart.Text, tbItemEnd.Text);
            pagerControlJobs.DataBind(list);

            if (dgViewDiscreteJobs.DataSource != null)
                this.dgViewDiscreteJobs.DataSource = null;

            this.dgViewDiscreteJobs.AutoGenerateColumns = false;
            this.dgViewDiscreteJobs.DataSource = pagerControlJobs.bindingSource;
            this.dgViewDiscreteJobs.Refresh();

            return count;
        }
    }
}
