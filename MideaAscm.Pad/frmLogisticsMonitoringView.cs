using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using MideaAscm.Dal.GetMaterialManage.Entities;
using DevComponents.AdvTree;
using Newtonsoft.Json;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Pad
{
    public partial class frmLogisticsMonitoringView : DevComponents.DotNetBar.OfficeForm
    {

        private MonthCalendar monthCalendar;
        private TaskbarNotifier taskbarNotifier1;
        private ElementStyle fontColor_Gray = null; //Finished Task's Style And Finished Job's Style
        private ElementStyle fontColor_Blue = null; //Executed Task's Style
        private ElementStyle fontColor_Red = null; // Not Statisfied Job's Style
        private ElementStyle fontColor_Black = null; // Statisfied Job's Style
        private string taskId { get; set; } //Task's Id
        private string jobId { get; set; } //DiscreteJob's WipEntityId
        private AscmWipDiscreteJobs taskDetails { get; set; }
        private string releaseHeaderIds = null;

        public frmLogisticsMonitoringView()
        {
            InitializeComponent();
        }

        public void FormActivated()
        {

        }

        private void frmLogisticsMonitoringView_Load(object sender, EventArgs e)
        {
            BindDataToolbarControls();
            CustomAdvTreeNodesStyleInit(this.advTreeGroup);
            StructureAdvTreeFristLevelColumns(this.advTreeGroup);

            pageControlTask.Bind();
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

        #region TaskBarNotifier's Event And Bind Data
        private void CloseClick(object obj, EventArgs ea)
        {
            //MessageBox.Show("Closed was Clicked");
        }

        private void TitleClick(object obj, EventArgs ea)
        {
            int count = 0;
            string taskString = "ALL";
            List<AscmWipDiscreteJobs> list = DataBindAdvTreeFristLevelNode(ref count, taskString);
            if (list != null && list.Count > 0)
            {
                advTreeGroup.BeginInit();
                StructureAdvTreeFristLevelNode(list, advTreeGroup);
                advTreeGroup.EndInit();
            }
            pageControlTask.DataBind(list);
        }

        private void ContentClick(object obj, EventArgs ea)
        {
            int count = 0;
            string taskString = taskbarNotifier1.ContentText.Replace("\r", ",");
            List<AscmWipDiscreteJobs> list = DataBindAdvTreeFristLevelNode(ref count, taskString);
            if (list != null && list.Count > 0)
            {
                advTreeGroup.BeginInit();
                StructureAdvTreeFristLevelNode(list, advTreeGroup);
                advTreeGroup.EndInit();
            }
            pageControlTask.DataBind(list);
        }

        public string GetTaskBarNotifierData()
        {
            string message = "";
            string taskString = string.Empty;

            try
            {
                AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                taskString = Service.GetTaskbarNotifierMessage(frmMainView.encryptTicket, frmMainView.userName, ref message);
                taskString = taskString.Replace(",", "\r");
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return taskString;
        }
        #endregion

        #region Toolbar Control To Bind Data
        private void BindDataToolbarControls()
        {
            //Start time
            tbItemStart.Text = DateTime.Now.ToString("yyyy-MM-dd");

            //Task Type
            if (cbItemTaskType.Items.Count > 0)
                cbItemTaskType.Items.Clear();

            cbItemTaskType.Items.Add("");
            List<int> listTaskType = AscmDiscreteJobs.IdentificationIdDefine.GetList();
            foreach (int item in listTaskType)
            {
                cbItemTaskType.Items.Add(AscmDiscreteJobs.IdentificationIdDefine.DisplayText(item));
            }

            //Material Category Status
            if (cbItemMtlCategoryStatus.Items.Count > 0)
                cbItemMtlCategoryStatus.Items.Clear();

            cbItemMtlCategoryStatus.Items.Add("");
            List<string> listMtlCategoryStatus = MtlCategoryStatusDefine.GetList();
            foreach (string item in listMtlCategoryStatus)
            {
                cbItemMtlCategoryStatus.Items.Add(MtlCategoryStatusDefine.DisplayText(item));
            }
            cbItemMtlCategoryStatus.Items.Add("特殊子库");
            cbItemMtlCategoryStatus.Items.Add("临时任务");

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

            //Task Time
            if (cbItemTaskTime.Items.Count > 0)
                cbItemTaskTime.Items.Clear();

            cbItemTaskTime.Items.Add("");
            cbItemTaskTime.Items.Add("上午");
            cbItemTaskTime.Items.Add("下午");
        }
        #endregion

        #region Gets And Bind AdvTree's Level Data
        private List<AscmWipDiscreteJobs> DataBindAdvTreeFristLevelNode(ref int count, string taskString)
        {
            List<AscmWipDiscreteJobs> list = null;
            string message = string.Empty;
            string _ynPage = string.Empty;

            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(pageControlTask.PageSize);
            ynPage.SetCurrentPage((pageControlTask.PageCurrent <= 0) ? 1 : pageControlTask.PageCurrent);
            _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);

            try
            {
                string queryType = string.Empty;
                if (cbItemTaskType.SelectedItem != null)
                {
                    switch (cbItemTaskType.SelectedItem.ToString())
                    {
                        case "总装":
                            queryType = "1";
                            break;
                        case "电装":
                            queryType = "2";
                            break;
                        default:
                            queryType = "";
                            break;
                    }
                }

                string queryFormat = string.Empty;
                if (cbItemMtlCategoryStatus.SelectedItem != null)
                {
                    switch (cbItemMtlCategoryStatus.SelectedItem.ToString())
                    {
                        case "有库存":
                            queryFormat = "INSTOCK";
                            break;
                        case "须备料":
                            queryFormat = "PRESTOCK";
                            break;
                        case "须配料":
                            queryFormat = "MIXSTOCK";
                            break;
                        case "特殊子库":
                            queryFormat = "SPECWAREHOUSE";
                            break;
                        case "临时任务":
                            queryFormat = "TEMPTASK";
                            break;
                        default:
                            queryFormat = "";
                            break;
                    }
                }

                string queryStatus = string.Empty;
                if (cbItemTaskStatus.SelectedItem != null)
                {
                    switch (cbItemTaskStatus.SelectedItem.ToString())
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

                string queryLine = (tbItemProductLine.Text == "") ? string.Empty : tbItemProductLine.Text.ToUpper().ToString();
                string queryTime = (cbItemTaskTime.SelectedItem == null) ? string.Empty : cbItemTaskTime.SelectedItem.ToString();
                string queryWarehouse = (tbItemWarehouse.Text == "") ? string.Empty : tbItemWarehouse.Text.ToUpper().ToString();

                AscmWebService.AscmWebService service = new AscmWebService.AscmWebService();
                string jsonString = service.GetMonitorTaskList(frmMainView.encryptTicket, frmMainView.userName, tbItemStart.Text, tbItemEnd.Text, tbItemJobStart.Text, tbItemJobEnd.Text, queryType, queryFormat, queryStatus, queryLine, queryTime, queryWarehouse, tbItemWipEntity.Text, taskString, ref _ynPage, ref message);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    list = (List<AscmWipDiscreteJobs>)JsonConvert.DeserializeObject(jsonString, typeof(List<AscmWipDiscreteJobs>));
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

        private List<AscmWipRequirementOperations> DataBindAdvTreeSecondLevelNode(int id, string mtlCategory)
        {
            List<AscmWipRequirementOperations> list = null;
            string message = string.Empty;

            AscmWebService.AscmWebService service = new AscmWebService.AscmWebService();
            string jsonString = service.GetMonitorJobList(frmMainView.encryptTicket, id, mtlCategory, ref message);
            if (!string.IsNullOrEmpty(jsonString))
            {
                list = (List<AscmWipRequirementOperations>)JsonConvert.DeserializeObject(jsonString, typeof(List<AscmWipRequirementOperations>));
            }

            return list;
        }
        #endregion

        #region Sturcture AdvTree's Nodes
        private void StructureAdvTreeFristLevelColumns(AdvTree advTree)
        {
            if (advTree.Columns.Count > 0)
                advTree.Columns.Clear();

            

            DevComponents.AdvTree.ColumnHeader taskId = new DevComponents.AdvTree.ColumnHeader();
            taskId.ColumnName = "ascmWipEntities_Name";
            taskId.DataFieldName = "ascmWipEntities_Name";
            taskId.Name = "ascmWipEntities_Name";
            taskId.Text = "作业号";
            taskId.Width.Absolute = 90;

          

            DevComponents.AdvTree.ColumnHeader productLine = new DevComponents.AdvTree.ColumnHeader();
            productLine.ColumnName = "scheduledStartDateCn";
            productLine.DataFieldName = "scheduledStartDateCn";
            productLine.Name = "scheduledStartDateCn";
            productLine.Text = "作业日期";
            productLine.Width.Absolute = 60;

            DevComponents.AdvTree.ColumnHeader warehouserId = new DevComponents.AdvTree.ColumnHeader();
            warehouserId.ColumnName = "ascmDiscreteJobs_line";
            warehouserId.DataFieldName = "ascmDiscreteJobs_line";
            warehouserId.Name = "ascmDiscreteJobs_line";
            warehouserId.Text = "生产线";
            warehouserId.Width.Absolute = 75;
            
            DevComponents.AdvTree.ColumnHeader categoryStatusCN = new DevComponents.AdvTree.ColumnHeader();
            categoryStatusCN.ColumnName = "_mtlCategoryStatus";
            categoryStatusCN.DataFieldName = "_mtlCategoryStatus";
            categoryStatusCN.Name = "_mtlCategoryStatus";
            categoryStatusCN.Text = "物料类别状态";
            categoryStatusCN.Width.Absolute = 73;

           

            DevComponents.AdvTree.ColumnHeader tipCN = new DevComponents.AdvTree.ColumnHeader();
            tipCN.ColumnName = "onlineTime";
            tipCN.DataFieldName = "onlineTime";
            tipCN.Name = "onlineTime";
            tipCN.Text = "上线时间";
            tipCN.Width.Absolute = 40;

            DevComponents.AdvTree.ColumnHeader taskTime = new DevComponents.AdvTree.ColumnHeader();
            taskTime.ColumnName = "ascmWorker_name";
            taskTime.DataFieldName = "ascmWorker_name";
            taskTime.Name = "ascmWorker_name";
            taskTime.Text = "责任人";
            taskTime.Width.Absolute = 40;

     

            DevComponents.AdvTree.ColumnHeader totalRequiredQuantity = new DevComponents.AdvTree.ColumnHeader();
            totalRequiredQuantity.ColumnName = "taskWipState";
            totalRequiredQuantity.DataFieldName = "taskWipState";
            totalRequiredQuantity.Name = "taskWipState";
            totalRequiredQuantity.Text = "状态";
            totalRequiredQuantity.Width.Absolute = 70;

            DevComponents.AdvTree.ColumnHeader totalWmsPreparationQuantity = new DevComponents.AdvTree.ColumnHeader();
            totalWmsPreparationQuantity.ColumnName = "uploadDate";
            totalWmsPreparationQuantity.DataFieldName = "uploadDate";
            totalWmsPreparationQuantity.Name = "uploadDate";
            totalWmsPreparationQuantity.Text = "上传日期";
            totalWmsPreparationQuantity.Width.Absolute = 70;

            DevComponents.AdvTree.ColumnHeader totalGetMaterialQuantity = new DevComponents.AdvTree.ColumnHeader();
            totalGetMaterialQuantity.ColumnName = "taskStarTime";
            totalGetMaterialQuantity.DataFieldName = "taskStarTime";
            totalGetMaterialQuantity.Name = "taskStarTime";
            totalGetMaterialQuantity.Text = "开始时间";
            totalGetMaterialQuantity.Width.Absolute = 70;

            DevComponents.AdvTree.ColumnHeader taskendTime = new DevComponents.AdvTree.ColumnHeader();
            taskendTime.ColumnName = "taskEndTime";
            taskendTime.DataFieldName = "taskEndTime";
            taskendTime.Name = "taskEndTime";
            taskendTime.Text = "结束时间";
            taskendTime.Width.Absolute = 70;

         

            DevComponents.AdvTree.ColumnHeader ascmMaterialItem_DocNumber = new DevComponents.AdvTree.ColumnHeader();
            ascmMaterialItem_DocNumber.ColumnName = "ascmMaterialItem_DocNumber";
            ascmMaterialItem_DocNumber.DataFieldName = "ascmMaterialItem_DocNumber";
            ascmMaterialItem_DocNumber.Name = "ascmMaterialItem_DocNumber";
            ascmMaterialItem_DocNumber.Text = "装配件";
            ascmMaterialItem_DocNumber.Width.Absolute = 70;

            DevComponents.AdvTree.ColumnHeader ascmMaterialItem_Description = new DevComponents.AdvTree.ColumnHeader();
            ascmMaterialItem_Description.ColumnName = "ascmMaterialItem_Description";
            ascmMaterialItem_Description.DataFieldName = "ascmMaterialItem_Description";
            ascmMaterialItem_Description.Name = "ascmMaterialItem_Description";
            ascmMaterialItem_Description.Text = "装配件描述";
            ascmMaterialItem_Description.Width.Absolute = 70;

            DevComponents.AdvTree.ColumnHeader netQuantity = new DevComponents.AdvTree.ColumnHeader();
            netQuantity.ColumnName = "netQuantity";
            netQuantity.DataFieldName = "netQuantity";
            netQuantity.Name = "netQuantity";
            netQuantity.Text = "计划数量";
            netQuantity.Width.Absolute = 70;

     
            advTree.Columns.Add(taskId);   
            advTree.Columns.Add(productLine);
            advTree.Columns.Add(warehouserId);
            advTree.Columns.Add(categoryStatusCN);  
            advTree.Columns.Add(tipCN);
            advTree.Columns.Add(taskTime);
            advTree.Columns.Add(totalRequiredQuantity);
            advTree.Columns.Add(totalWmsPreparationQuantity);
            advTree.Columns.Add(totalGetMaterialQuantity);
            advTree.Columns.Add(taskendTime);
            advTree.Columns.Add(ascmMaterialItem_DocNumber);
            advTree.Columns.Add(ascmMaterialItem_Description);
            advTree.Columns.Add(netQuantity);
        }

        private void StructureAdvTreeFristLevelNode(List<AscmWipDiscreteJobs> list, AdvTree advTree)
        {
            advTree.Nodes.Clear();

            if (list != null && list.Count > 0)
            {
                foreach (AscmWipDiscreteJobs ascmGetMaterialTask in list)
                {
                    Node fristNode = new Node();
                    fristNode.Tag = ascmGetMaterialTask;
                    fristNode.Text = ascmGetMaterialTask.ascmWipEntities_Name.ToString();
                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.scheduledStartDateCn))
                        fristNode.Cells.Add(new Cell(ascmGetMaterialTask.scheduledStartDateCn));
                    else
                        fristNode.Cells.Add(new Cell(" "));

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.ascmDiscreteJobs_line))
                        fristNode.Cells.Add(new Cell(ascmGetMaterialTask.ascmDiscreteJobs_line));
                    else
                        fristNode.Cells.Add(new Cell(" "));

                    //if (!string.IsNullOrEmpty(ascmGetMaterialTask.ascmDiscreteJobs_line))
                    //    fristNode.Cells.Add(new Cell(ascmGetMaterialTask.ascmDiscreteJobs_line));
                    //else
                    //    fristNode.Cells.Add(new Cell(" "));

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask._mtlCategoryStatus))
                        fristNode.Cells.Add(new Cell(ascmGetMaterialTask._mtlCategoryStatus));
                    else
                        fristNode.Cells.Add(new Cell(" "));

                    //if (!string.IsNullOrEmpty(ascmGetMaterialTask.))
                    //    fristNode.Cells.Add(new Cell(ascmGetMaterialTask.tipCN));
                    //else
                    //    fristNode.Cells.Add(new Cell(" "));

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.onlineTime))
                        fristNode.Cells.Add(new Cell(ascmGetMaterialTask.onlineTime));
                    else
                        fristNode.Cells.Add(new Cell(" "));

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.ascmWorker_name))
                        fristNode.Cells.Add(new Cell(ascmGetMaterialTask.ascmWorker_name));
                    else
                        fristNode.Cells.Add(new Cell(" "));

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.taskWipState))
                        fristNode.Cells.Add(new Cell(ascmGetMaterialTask.taskWipState));
                    else
                        fristNode.Cells.Add(new Cell(" "));

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.uploadDate.ToString()))
                        fristNode.Cells.Add(new Cell(ascmGetMaterialTask.uploadDate.ToString()));
                    else
                        fristNode.Cells.Add(new Cell(" "));

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.taskStarTime))
                        fristNode.Cells.Add(new Cell(ascmGetMaterialTask.taskStarTime));
                    else
                        fristNode.Cells.Add(new Cell(" "));

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.taskEndTime))
                        fristNode.Cells.Add(new Cell(ascmGetMaterialTask.taskEndTime));
                    else
                        fristNode.Cells.Add(new Cell(" "));

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.ascmMaterialItem_DocNumber))
                        fristNode.Cells.Add(new Cell(ascmGetMaterialTask.ascmMaterialItem_DocNumber));
                    else
                        fristNode.Cells.Add(new Cell(" "));

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.ascmMaterialItem_Description))
                        fristNode.Cells.Add(new Cell(ascmGetMaterialTask.ascmMaterialItem_Description));
                    else
                        fristNode.Cells.Add(new Cell(" "));

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.netQuantity.ToString()))
                        fristNode.Cells.Add(new Cell(ascmGetMaterialTask.netQuantity.ToString()));
                    else
                        fristNode.Cells.Add(new Cell(" "));

                   
                    //if (!string.IsNullOrEmpty(ascmGetMaterialTask.ascmMaterialItem_Description ))
                    //    fristNode.Image = global::MideaAscm.Pad.Properties.Resources.flag_blue;
                    //else
                    //    fristNode.Image = global::MideaAscm.Pad.Properties.Resources.flag_white;

                   if (ascmGetMaterialTask.taskWipState == "未完成")
                        fristNode.Style = fontColor_Blue;
                    else
                        fristNode.Style = fontColor_Gray;

                    fristNode.CheckBoxVisible = true;
                    fristNode.Expanded = false;

                    Node node = new Node();
                    node.Text = "";
                    fristNode.Nodes.Add(node);

                    advTree.Nodes.Add(fristNode);
                }
            }
        }

        private void StructureAdvTreeSecondLevelColumns(Node groupNode)
        {
            if (groupNode.NodesColumns.Count > 0)
                groupNode.NodesColumns.Clear();

            DevComponents.AdvTree.ColumnHeader docNumber = new DevComponents.AdvTree.ColumnHeader();
            docNumber.Name = "docNumber";
            docNumber.Text = "组件";
            docNumber.Width.Absolute = 165;

            DevComponents.AdvTree.ColumnHeader mpsDateRequiredStr = new DevComponents.AdvTree.ColumnHeader();
            mpsDateRequiredStr.Name = "mpsDateRequiredStr";
            mpsDateRequiredStr.Text = "需求日期";
            mpsDateRequiredStr.Width.Absolute = 85;

            DevComponents.AdvTree.ColumnHeader description = new DevComponents.AdvTree.ColumnHeader();
            description.Name = "description";
            description.Text = "组件说明";
            description.Width.Absolute = 68;

            DevComponents.AdvTree.ColumnHeader quantityPerAssembly = new DevComponents.AdvTree.ColumnHeader();
            quantityPerAssembly.Name = "quantityPerAssembly";
            quantityPerAssembly.Text = "每个装";
            quantityPerAssembly.Width.Absolute = 105;

            DevComponents.AdvTree.ColumnHeader wipSupplyTypeCn = new DevComponents.AdvTree.ColumnHeader();
            wipSupplyTypeCn.Name = "wipSupplyTypeCn";
            wipSupplyTypeCn.Text = "供应类型";
            wipSupplyTypeCn.Width.Absolute = 140;

            DevComponents.AdvTree.ColumnHeader supplySubinventory = new DevComponents.AdvTree.ColumnHeader();
            supplySubinventory.Name = "netQuantity";
            supplySubinventory.Text = "子库";
            supplySubinventory.Width.Absolute = 65;

            DevComponents.AdvTree.ColumnHeader requiredQuantity = new DevComponents.AdvTree.ColumnHeader();
            requiredQuantity.Name = "requiredQuantity";
            requiredQuantity.Text = "需求数量";
            requiredQuantity.Width.Absolute = 65;

            DevComponents.AdvTree.ColumnHeader transactionQuantity = new DevComponents.AdvTree.ColumnHeader();
            transactionQuantity.Name = "transactionQuantity";
            transactionQuantity.Text = "现有数量";
            transactionQuantity.Width.Absolute = 65;
            

            DevComponents.AdvTree.ColumnHeader totalGetMaterialQuantity = new DevComponents.AdvTree.ColumnHeader();
            totalGetMaterialQuantity.Name = "totalGetMaterialQuantity";
            totalGetMaterialQuantity.Text = "领料数";
            totalGetMaterialQuantity.Width.Absolute = 65;

            DevComponents.AdvTree.ColumnHeader wmsPreparationQuantity = new DevComponents.AdvTree.ColumnHeader();
            wmsPreparationQuantity.Name = "wmsPreparationQuantity";
            wmsPreparationQuantity.Text = "备料数量";
            wmsPreparationQuantity.Width.Absolute = 65;

            DevComponents.AdvTree.ColumnHeader getMaterialQuantity = new DevComponents.AdvTree.ColumnHeader();
            getMaterialQuantity.Name = "getMaterialQuantity";
            getMaterialQuantity.Text = "领料数量";
            getMaterialQuantity.Width.Absolute = 65;

            DevComponents.AdvTree.ColumnHeader quantityGetMaterialDifference = new DevComponents.AdvTree.ColumnHeader();
            quantityGetMaterialDifference.Name = "quantityGetMaterialDifference";
            quantityGetMaterialDifference.Text = "领料差异数量";
            quantityGetMaterialDifference.Width.Absolute = 65;

            groupNode.NodesColumns.Add(docNumber);
            groupNode.NodesColumns.Add(mpsDateRequiredStr);
            groupNode.NodesColumns.Add(description);
            groupNode.NodesColumns.Add(quantityPerAssembly);
            groupNode.NodesColumns.Add(wipSupplyTypeCn);
            groupNode.NodesColumns.Add(supplySubinventory);
            groupNode.NodesColumns.Add(requiredQuantity);
            groupNode.NodesColumns.Add(transactionQuantity);
            groupNode.NodesColumns.Add(totalGetMaterialQuantity);
            groupNode.NodesColumns.Add(wmsPreparationQuantity);
            groupNode.NodesColumns.Add(getMaterialQuantity);
            groupNode.NodesColumns.Add(quantityGetMaterialDifference);

            groupNode.NodesColumnsHeaderVisible = true;
        }

        private void StrucureAdvTreeSecondLevelNode(List<AscmWipRequirementOperations> list, Node groupNode)
        {
            //DotNetBar <a href="textmarkup">text-markup</a> is fully supported
            foreach (AscmWipRequirementOperations ascmWipDiscreteJobs in list)
            {
                Node subNode = new Node();
                subNode.Tag = ascmWipDiscreteJobs;
                if (!string.IsNullOrEmpty(ascmWipDiscreteJobs.docNumber))
                    subNode.Text = ascmWipDiscreteJobs.docNumber;
                else
                    subNode.Text = " ";

                if (!string.IsNullOrEmpty(ascmWipDiscreteJobs.mpsDateRequiredStr))
                    subNode.Cells.Add(new Cell(ascmWipDiscreteJobs.mpsDateRequiredStr));
                else
                    subNode.Cells.Add(new Cell(" "));

                if (!string.IsNullOrEmpty(ascmWipDiscreteJobs.description))
                    subNode.Cells.Add(new Cell(ascmWipDiscreteJobs.description));
                else
                    subNode.Cells.Add(new Cell(" "));

                if (!string.IsNullOrEmpty(ascmWipDiscreteJobs.quantityPerAssembly.ToString()))
                    subNode.Cells.Add(new Cell(ascmWipDiscreteJobs.quantityPerAssembly.ToString()));
                else
                    subNode.Cells.Add(new Cell(" "));

                if (!string.IsNullOrEmpty(ascmWipDiscreteJobs.wipSupplyTypeCn))
                    subNode.Cells.Add(new Cell(ascmWipDiscreteJobs.wipSupplyTypeCn));
                else
                    subNode.Cells.Add(new Cell(" "));

                if (!string.IsNullOrEmpty(ascmWipDiscreteJobs.supplySubinventory))
                    subNode.Cells.Add(new Cell(ascmWipDiscreteJobs.supplySubinventory));
                else
                    subNode.Cells.Add(new Cell(" "));

                if (!string.IsNullOrEmpty(ascmWipDiscreteJobs.requiredQuantity.ToString()))
                    subNode.Cells.Add(new Cell(ascmWipDiscreteJobs.requiredQuantity.ToString()));
                else
                    subNode.Cells.Add(new Cell(" "));

                subNode.Cells.Add(new Cell(ascmWipDiscreteJobs.transactionQuantity.ToString()));
                //subNode.Cells.Add(new Cell(ascmWipDiscreteJobs.totalGetMaterialQuantity.ToString()));
                subNode.Cells.Add(new Cell(ascmWipDiscreteJobs.wmsPreparationQuantity.ToString()));
                subNode.Cells.Add(new Cell(ascmWipDiscreteJobs.getMaterialQuantity.ToString()));
                subNode.Cells.Add(new Cell(ascmWipDiscreteJobs.quantityGetMaterialDifference.ToString()));
                //if (ascmWipDiscreteJobs.ascmMarkTaskLog != null)
                //    subNode.Image = global::MideaAscm.Pad.Properties.Resources.flag_blue;
                //else
                //    subNode.Image = global::MideaAscm.Pad.Properties.Resources.flag_white;

                //if (ascmWipDiscreteJobs.totalRequiredQuantity - ascmWipDiscreteJobs.totalSumQuantity > 0 && ascmWipDiscreteJobs.totalRequiredQuantity - ascmWipDiscreteJobs.totalGetMaterialQuantity > 0)
                //{
                //    subNode.Style = fontColor_Red;
                //}
                //else if (ascmWipDiscreteJobs.totalRequiredQuantity - ascmWipDiscreteJobs.totalSumQuantity == 0 && ascmWipDiscreteJobs.totalRequiredQuantity - ascmWipDiscreteJobs.totalGetMaterialQuantity > 0)
                //{
                //    subNode.Style = fontColor_Black;
                //}
                //else if (ascmWipDiscreteJobs.totalRequiredQuantity - ascmWipDiscreteJobs.totalSumQuantity == 0 && ascmWipDiscreteJobs.totalRequiredQuantity - ascmWipDiscreteJobs.totalGetMaterialQuantity == 0)
                //{
                //    subNode.Style = fontColor_Gray;
                //}

                subNode.CheckBoxVisible = true;

                groupNode.Nodes.Add(subNode);
            }
        }
        #endregion

        #region Custom AdvTree's Nodes Style
        private void CustomAdvTreeNodesStyleInit(AdvTree advTree)
        {
            fontColor_Gray = new ElementStyle();
            fontColor_Gray.TextColor = Color.DarkGray;
            advTree.Styles.Add(fontColor_Gray);

            fontColor_Blue = new ElementStyle();
            fontColor_Blue.TextColor = Color.Blue;
            advTree.Styles.Add(fontColor_Blue);

            fontColor_Black = new ElementStyle();
            fontColor_Black.TextColor = Color.Black;
            advTree.Styles.Add(fontColor_Black);

            fontColor_Red = new ElementStyle();
            fontColor_Red.TextColor = Color.Red;
            advTree.Styles.Add(fontColor_Red);
        }
        #endregion

        private int pageControlTask_EventPaging(CustomControl.EventPagingArg e)
        {
            int count = 0;
            List<AscmWipDiscreteJobs> list = DataBindAdvTreeFristLevelNode(ref count, "");
            if (list != null && list.Count > 0)
            {
                advTreeGroup.BeginInit();
                StructureAdvTreeFristLevelNode(list, advTreeGroup);
                advTreeGroup.EndInit();
            }
            else
            {
                this.advTreeGroup.Nodes.Clear();
            }
            pageControlTask.DataBind(list);

            return count;
        }

        private void btnItemSearch_Click(object sender, EventArgs e)
        {
            pageControlTask.Bind();
        }

        private void advTreeGroup_BeforeExpand(object sender, AdvTreeNodeCancelEventArgs e)
        {
            Node nodeParent = e.Node;
            nodeParent.Nodes.Clear();

            AscmWipDiscreteJobs ascmGetMaterialTask = nodeParent.Tag as AscmWipDiscreteJobs;
            if (ascmGetMaterialTask != null)
            {
                //Structure nodes columns
                StructureAdvTreeSecondLevelColumns(nodeParent);
                //Get discretejobs list
                List<AscmWipRequirementOperations> list = DataBindAdvTreeSecondLevelNode(ascmGetMaterialTask.wipEntityId, ascmGetMaterialTask.mtlCategoryStatus);
                if (list != null && list.Count > 0)
                {
                    advTreeGroup.BeginInit();
                    StrucureAdvTreeSecondLevelNode(list, nodeParent);
                    advTreeGroup.EndInit();
                }
            }
        }

        private void advTreeGroup_NodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                //Double click discretejob, pop-up dialog
                frmMaterialListDialog frmMaterialListDialog = new frmMaterialListDialog();

                if (e.Node.Parent.Tag as AscmGetMaterialTask != null)
                    frmMaterialListDialog.taskId = ((AscmGetMaterialTask)e.Node.Parent.Tag).id;
                else
                    frmMaterialListDialog.taskId = 0;

                if (e.Node.Tag as AscmWipDiscreteJobs != null)
                    frmMaterialListDialog.jobId = ((AscmWipDiscreteJobs)e.Node.Tag).wipEntityId;
                else
                    frmMaterialListDialog.jobId = 0;

                frmMaterialListDialog.ShowDialog();
            }
        }

        private void advTreeGroup_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            e.Node.Checked = !e.Node.Checked;

            if (!string.IsNullOrEmpty(releaseHeaderIds))
                releaseHeaderIds += ",";

            if (e.Node.Parent != null)
            {
                //Select discretejob to get data, and then excuted mark operation
                //if (e.Node.Parent.Tag as AscmWipDiscreteJobs != null)
                //    this.taskId = ((AscmWipDiscreteJobs)e.Node.Parent.Tag).wipEntityId.ToString();

                if (e.Node.Tag as AscmWipRequirementOperations != null)
                    this.jobId = ((AscmWipRequirementOperations)e.Node.Tag).id.ToString();

                //Confirmed tasks Id or discrete jobs Id
                string getMaterialId = "[" + this.jobId + "]";
                if (e.Node.Checked)
                {
                    releaseHeaderIds += getMaterialId;
                }
                else
                {
                    if (!string.IsNullOrEmpty(releaseHeaderIds))
                        releaseHeaderIds = releaseHeaderIds.Replace(getMaterialId, "");
                }
            }
            else
            {
                //Select task's details
                if (e.Node.Tag as AscmWipDiscreteJobs != null)
                    this.taskDetails = (AscmWipDiscreteJobs)e.Node.Tag;

                //Select the job associated with the selected task
                if (e.Node.Expanded)
                {
                    foreach (Node node in e.Node.Nodes)
                    {
                        node.Checked = e.Node.Checked;
                    }
                }
                    
                //Confirmed tasks Id or discrete jobs Id
                string getMaterialId = this.taskDetails.wipEntityId.ToString();
                if (!string.IsNullOrEmpty(this.taskDetails.mtlCategoryStatus))
                {
                    getMaterialId = getMaterialId + ":" + this.taskDetails.mtlCategoryStatus;
                }
                
                if (e.Node.Checked)
                {
                  
                        releaseHeaderIds += getMaterialId;
                   
                }
                else
                {
                    if (!string.IsNullOrEmpty(releaseHeaderIds))
                    {
                        string[] arr = releaseHeaderIds.Split(',');
                        releaseHeaderIds = "";
                        foreach (string s in arr)
                        {
                            if (s != getMaterialId&&!string.IsNullOrEmpty(s))
                            {
                                if (!string.IsNullOrEmpty(releaseHeaderIds))
                                    releaseHeaderIds += ",";
                                releaseHeaderIds += s;
                            }
                        }
                        //releaseHeaderIds = releaseHeaderIds.Replace(getMaterialId, "");
                    }
                }
            }
        }

        private void btnItemRun_Click(object sender, EventArgs e)
        {
            try
            {
                
                string message = string.Empty;
                string[] myArray = releaseHeaderIds.Split(',');
                string newReleaseHeaderIds = string.Empty;
                foreach (string item in myArray)
                {
                    if (!string.IsNullOrEmpty(newReleaseHeaderIds) && !string.IsNullOrEmpty(item))
                        newReleaseHeaderIds += ",";
                    if (!string.IsNullOrEmpty(item))
                        newReleaseHeaderIds += item;
                }
                releaseHeaderIds = newReleaseHeaderIds;
                if (string.IsNullOrEmpty(releaseHeaderIds))
                {
                    MessageBox.Show("请选择任务！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                    if (Service.StartExcuteTask(frmMainView.encryptTicket, frmMainView.userName, releaseHeaderIds, ref message))
                    {
                        MessageBoxEx.Show("任务开始执行!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pageControlTask.Bind();
                        
                    }
                    else
                    {
                        MessageBox.Show(message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                releaseHeaderIds = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnItemSure_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";
                AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                if (Service.ConfrimedBatchGetmaterials(frmMainView.encryptTicket, frmMainView.userName, releaseHeaderIds, ref message))
                {
                    MessageBoxEx.Show("领料成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pageControlTask.Bind();
                    releaseHeaderIds = "";
                }
                else
                {
                    MessageBox.Show(message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnItemStop_Click(object sender, EventArgs e)
        {
            try
            {
                string message = string.Empty;
                if (string.IsNullOrEmpty(releaseHeaderIds))
                {
                    MessageBox.Show("请选择任务！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    string[] myArray = releaseHeaderIds.Split(',');
                    string newReleaseHeaderIds = string.Empty;
                    foreach (string item in myArray)
                    {
                        if (!string.IsNullOrEmpty(newReleaseHeaderIds) && !string.IsNullOrEmpty(item))
                            newReleaseHeaderIds += ",";
                        if (!string.IsNullOrEmpty(item))
                            newReleaseHeaderIds += item;
                    }
                    releaseHeaderIds = newReleaseHeaderIds;
                    AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                    if (Service.EndExcuteTask(frmMainView.encryptTicket, frmMainView.userName, releaseHeaderIds, ref message))
                    {
                        MessageBoxEx.Show("任务已结束!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pageControlTask.Bind();
                    }
                    else
                    {
                        MessageBox.Show(message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                releaseHeaderIds = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnItemMark_Click(object sender, EventArgs e)
        {
            try
            {
                string message = string.Empty;
                AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                if (Service.MarkTask(frmMainView.encryptTicket, frmMainView.userName, this.taskId, this.jobId, ref message))
                    pageControlTask.Bind();
                else
                    MessageBox.Show(message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnItemUnMark_Click(object sender, EventArgs e)
        {
            try
            {
                string message = string.Empty;
                AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                if (Service.UnMarkTask(frmMainView.encryptTicket, frmMainView.userName, this.taskId, this.jobId, ref message))
                    pageControlTask.Bind();
                else
                    MessageBox.Show(message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnItemTotal_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "frmSumMaterialListView")
                {
                    form.Focus();
                    return;
                }
            }
            frmSumMaterialListView frmSumMaterialListView = new frmSumMaterialListView();
            frmSumMaterialListView.MdiParent = this.ParentForm;
            frmSumMaterialListView.BringToFront();
            frmSumMaterialListView.Show();
        
        }

        private void btnItemTaskDetail_Click(object sender, EventArgs e)
        {
            //if (this.advTreeGroup.Nodes.Count > 0 && taskDetails != null)
            //{
            //    frmViewTaskDetailsDialog frmViewTaskDetailsDialog = new frmViewTaskDetailsDialog();
            //    frmViewTaskDetailsDialog.taskDetails = this.taskDetails;
            //    frmViewTaskDetailsDialog.ShowDialog();
            //}
            //else
            //{
            //    taskDetails = null;
            //    MessageBoxEx.Show("未选中任务！","错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void btnItemRefresh_Click(object sender, EventArgs e)
        {
            //pageControlTask.Bind();

            taskbarNotifier1 = new TaskbarNotifier();
            taskbarNotifier1.SetBackgroundBitmap(new Bitmap(GetType(), "bg_message1.gif"), Color.FromArgb(255, 0, 255));
            taskbarNotifier1.SetCloseBitmap(new Bitmap(GetType(), "close.bmp"), Color.FromArgb(255, 0, 255), new Point(263, 8));
            taskbarNotifier1.TitleRectangle = new Rectangle(195, 100, 70, 25);
            taskbarNotifier1.ContentRectangle = new Rectangle(30, 30, 190, 68);
            taskbarNotifier1.TitleClick += new EventHandler(TitleClick);
            taskbarNotifier1.ContentClick += new EventHandler(ContentClick);
            taskbarNotifier1.CloseClick += new EventHandler(CloseClick);

            taskbarNotifier1.CloseClickable = true;
            taskbarNotifier1.TitleClickable = true;
            taskbarNotifier1.ContentClickable = true;
            taskbarNotifier1.EnableSelectionRectangle = false;
            taskbarNotifier1.KeepVisibleOnMousOver = true;	// Added Rev 002
            taskbarNotifier1.ReShowOnMouseOver = false;		// Added Rev 002

            string taskString = GetTaskBarNotifierData();

            if (!string.IsNullOrEmpty(taskString))
            {
                taskbarNotifier1.Show("查看", taskString, 500, 3000, 500);
            }
        }
    }
}
