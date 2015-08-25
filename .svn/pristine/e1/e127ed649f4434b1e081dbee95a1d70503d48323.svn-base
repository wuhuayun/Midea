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
    public partial class frmTaskManagementView : DevComponents.DotNetBar.OfficeForm
    {
        private MonthCalendar monthCalendar;
        
        public frmTaskManagementView()
        {
            InitializeComponent();
        }

        public void FormActivated()
        {

        }

        private void frmTaskManagementView_Load(object sender, EventArgs e)
        {
            tbItemStart.Text = DateTime.Now.ToString("yyyy-MM-dd");
            
            BindDataToolbarControls();
            StructureDataGridViewColumns(this.dgViewTaskList);
            pagerControlTask.Bind();
        }

        #region Toolbar Control To Bind Data
        public void BindDataToolbarControls()
        {
            //Task Status
            if (cbItemTaskStatus.Items.Count > 0)
                cbItemTaskStatus.Items.Clear();

            cbItemTaskStatus.Items.Add("");
            List<string> listTaskStatus = AscmGetMaterialTask.StatusDefine.GetList();
            foreach (string item in listTaskStatus)
            {
                if (item != "NOTALLOCATE")
                    cbItemTaskStatus.Items.Add(AscmGetMaterialTask.StatusDefine.DisplayText(item));
            }
        }
        #endregion

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
            id.HeaderText = "id";
            id.Name = "id";
            id.DataPropertyName = "id";
            id.Width = 40;
            id.Visible = false;

            DataGridViewTextBoxColumn taskId = new DataGridViewTextBoxColumn();
            taskId.HeaderText = "任务号";
            taskId.Name = "taskId";
            taskId.DataPropertyName = "taskId";
            taskId.Width = 70;

            DataGridViewTextBoxColumn productLine = new DataGridViewTextBoxColumn();
            productLine.HeaderText = "生产线";
            productLine.Name = "productLine";
            productLine.DataPropertyName = "productLine";
            productLine.Width = 70;

            DataGridViewTextBoxColumn warehouserId = new DataGridViewTextBoxColumn();
            warehouserId.HeaderText = "仓库";
            warehouserId.Name = "warehouserId";
            warehouserId.DataPropertyName = "warehouserId";
            warehouserId.Width = 70;

            DataGridViewTextBoxColumn tipCN = new DataGridViewTextBoxColumn();
            tipCN.HeaderText = "作业内容";
            tipCN.Name = "tipCN";
            tipCN.DataPropertyName = "tipCN";
            tipCN.Width = 80;

            DataGridViewTextBoxColumn relatedMark = new DataGridViewTextBoxColumn();
            relatedMark.HeaderText = "关联标记";
            relatedMark.Name = "relatedMark";
            relatedMark.DataPropertyName = "relatedMark";
            relatedMark.Width = 150;

            DataGridViewTextBoxColumn starTime = new DataGridViewTextBoxColumn();
            starTime.HeaderText = "开始时间";
            starTime.Name = "starTime";
            starTime.DataPropertyName = "starTime";
            starTime.Width = 110;

            DataGridViewTextBoxColumn endTime = new DataGridViewTextBoxColumn();
            endTime.HeaderText = "结束时间";
            endTime.Name = "endTime";
            endTime.DataPropertyName = "endTime";
            endTime.Width = 110;

            dataGridViewX.Columns.Add(id);
            dataGridViewX.Columns.Add(taskId);
            dataGridViewX.Columns.Add(productLine);
            dataGridViewX.Columns.Add(warehouserId);
            dataGridViewX.Columns.Add(tipCN);
            dataGridViewX.Columns.Add(relatedMark);
            dataGridViewX.Columns.Add(starTime);
            dataGridViewX.Columns.Add(endTime);
        }

        private List<AscmGetMaterialTask> DataBindDataGridView(ref int count, string queryStartTime, string queryEndTime, string queryTaskStatus)
        {
            List<AscmGetMaterialTask> list = null;
            string message = string.Empty;
            string _ynPage = string.Empty;

            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(pagerControlTask.PageSize);
            ynPage.SetCurrentPage((pagerControlTask.PageCurrent <= 0) ? 1 : pagerControlTask.PageCurrent);
            _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);

            try
            {
                string queryStatus = string.Empty;
                if (queryTaskStatus != null)
                {
                    switch (queryTaskStatus.ToString())
                    {
                        case "已分配":
                            queryStatus = "NOTEXECUTE";
                            break;
                        case "执行中":
                            queryStatus = "EXECUTE";
                            break;
                        case "已完成":
                            queryStatus = "FINISH";
                            break;
                        case "已关闭":
                            queryStatus = "CLOSE";
                            break;
                        default:
                            queryStatus = "";
                            break;
                    }
                }

                AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                string jsonString = Service.GetUnAutoTaskList(frmMainView.encryptTicket, queryStartTime, queryEndTime, queryStatus, ref _ynPage, ref message);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    list = (List<AscmGetMaterialTask>)JsonConvert.DeserializeObject(jsonString, typeof(List<AscmGetMaterialTask>));
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

        private int pagerControlTask_EventPaging(CustomControl.EventPagingArg e)
        {
            int count = 0;
            List<AscmGetMaterialTask> list = DataBindDataGridView(ref count, tbItemStart.Text, tbItemEnd.Text, cbItemTaskStatus.SelectedText);
            pagerControlTask.DataBind(list);

            if (dgViewTaskList.DataSource != null)
                this.dgViewTaskList.DataSource = null;

            this.dgViewTaskList.AutoGenerateColumns = false;
            this.dgViewTaskList.DataSource = pagerControlTask.bindingSource;
            this.dgViewTaskList.Refresh();

            return count;
        }

        private void btnItemSearch_Click(object sender, EventArgs e)
        {
            ButtonItem buttonItem = sender as ButtonItem;
            if (buttonItem != null)
                buttonItem.Focus();

            try
            {
                if (string.IsNullOrEmpty(tbItemStart.Text) || string.IsNullOrEmpty(tbItemEnd.Text))
                    throw new Exception("任务查询失败：请选择起止日期！");

                pagerControlTask.Bind();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnItemAdd_Click(object sender, EventArgs e)
        {
            frmAddTaskForm frmAddTaskForm = new frmAddTaskForm();
            frmAddTaskForm.ShowDialog();
            if (frmAddTaskForm.DialogResult == DialogResult.OK)
            {
                AscmGetMaterialTask ascmGetMaterialTask = frmAddTaskForm.ascmGetMaterialTask;
                string jsonString = JsonConvert.SerializeObject(ascmGetMaterialTask);
                string message = "";
                AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                if (Service.AddUnAutoTask(frmMainView.encryptTicket, frmMainView.userName, jsonString, ref message))
                {
                    pagerControlTask.Bind();
                }
            }
        }

        private void btnItemDelete_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in this.dgViewTaskList.Rows)
                {
                    if (dr.Selected)
                    {
                        int id = int.Parse(dr.Cells[0].Value.ToString());
                        string message = "";
                        AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                        if (Service.DeleteUnAutoTask(frmMainView.encryptTicket, frmMainView.userName, id, ref message))
                        {
                            pagerControlTask.Bind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
