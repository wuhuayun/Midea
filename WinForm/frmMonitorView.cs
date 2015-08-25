using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.IO;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.FromErp.Entities;
using Newtonsoft.Json;
using DevComponents.AdvTree;


namespace WinForm
{
    public partial class frmMonitorView : Office2007Form
    {
        public frmMonitorView()
        {
            InitializeComponent();
        }

        private void frmMonitorView_Load(object sender, EventArgs e)
        {
            StatusDataBind();//状态绑定
            //int count = 0;
            //List<AscmGetMaterialTask> list = GetTaskTree(ref count);
            //if (list != null && list.Count > 0)
            //{
            //    DataBindTaskTree(list);
            //}
            pageControlTask.Bind();
        }

        /// <summary>
        /// 状态绑定
        /// </summary>
        private void StatusDataBind()
        {
            foreach (string str in AscmGetMaterialTask.StatusDefine.GetList())
            {
                if (str != "NOTALLOCATE")
                {
                    tscbTaskStatus.Items.Add(AscmGetMaterialTask.StatusDefine.DisplayText(str));
                }
            }
        }

        private List<AscmGetMaterialTask> GetTaskTree(ref int count)
        {
            List<AscmGetMaterialTask> listTasks = null;
            string message = string.Empty;
            string _ynPage = string.Empty;

            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(pageControlTask.PageSize);
            ynPage.SetCurrentPage((pageControlTask.PageCurrent <= 0) ? 1 : pageControlTask.PageCurrent);
            _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);

            try
            {
                string status = (tscbTaskStatus.SelectedItem == null) ? string.Empty : tscbTaskStatus.SelectedItem.ToString().Trim();
                if (!string.IsNullOrEmpty(status))
                {
                    switch (status)
                    {
                        case "已完成":
                            status = "FINISH";
                            break;
                        case "执行中":
                            status = "EXECUTE";
                            break;
                        case "已分配":
                            status = "NOTEXECUTE";
                            break;
                        case "未分配":
                            status = "NOTALLOCATE";
                            break;
                        default:
                            status = "";
                            break;
                    }
                }
                WinForm.AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                string jsonStr = Service.GetTaskList(frmMain.encryptTicket, frmMain.userName, status, ref _ynPage, ref message);
                if (!string.IsNullOrEmpty(jsonStr))
                {
                    listTasks = (List<AscmGetMaterialTask>)JsonConvert.DeserializeObject(jsonStr, typeof(List<AscmGetMaterialTask>));
                    if (listTasks != null && listTasks.Count > 0)
                    {
                        ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);
                        count = ynPage.GetRecordCount();
                    }
                    else
                    {
                        MessageBoxEx.Show("没有查询到符合条件的领料任务!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listTasks;
        }

        private void DataBindTaskTree(List<AscmGetMaterialTask> list)
        {
            advTree1.Nodes.Clear();
            if (list != null && list.Count > 0)
            {
                foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
                {
                    Node groupNode = new Node();
                    string str = " ";
                    groupNode.Tag = ascmGetMaterialTask;
                    groupNode.Text = ascmGetMaterialTask.id.ToString();
                    groupNode.Cells.Add(new Cell(ascmGetMaterialTask.taskId));
                    groupNode.Cells.Add(new Cell(ascmGetMaterialTask.IdentificationIdCN));
                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.productLine))
                    {
                        groupNode.Cells.Add(new Cell(ascmGetMaterialTask.productLine));
                    }
                    else
                    {
                        groupNode.Cells.Add(new Cell(str));
                    }

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.warehouserId))
                    {
                        groupNode.Cells.Add(new Cell(ascmGetMaterialTask.warehouserId));
                    }
                    else
                    {
                        groupNode.Cells.Add(new Cell(str));
                    }

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.warehouserPlace))
                    {
                        groupNode.Cells.Add(new Cell(ascmGetMaterialTask.warehouserPlace));
                    }
                    else
                    {
                        groupNode.Cells.Add(new Cell(str));
                    }

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.mtlCategoryStatus))
                    {
                        groupNode.Cells.Add(new Cell(ascmGetMaterialTask._mtlCategoryStatus));
                    }
                    else
                    {
                        groupNode.Cells.Add(new Cell(str));
                    }

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.materialDocNumber))
                    {
                        groupNode.Cells.Add(new Cell(ascmGetMaterialTask.materialDocNumber));
                    }
                    else
                    {
                        groupNode.Cells.Add(new Cell(str));
                    }

                    if (!string.IsNullOrEmpty(ascmGetMaterialTask.taskTime))
                    {
                        groupNode.Cells.Add(new Cell(ascmGetMaterialTask.taskTime));
                    }
                    else
                    {
                        groupNode.Cells.Add(new Cell(str));
                    }
                    groupNode.Cells.Add(new Cell(ascmGetMaterialTask._status));
                    groupNode.Cells.Add(new Cell(ascmGetMaterialTask.totalRequiredQuantity.ToString()));
                    groupNode.Cells.Add(new Cell(ascmGetMaterialTask.totalGetMaterialQuantity.ToString()));
                    groupNode.CheckBoxVisible = true;
                    Node node = BuilderJobTree(groupNode);
                    List<AscmWipDiscreteJobs> listDiscreteJobs = GetJobTree(ascmGetMaterialTask.id);
                    if (listDiscreteJobs != null && listDiscreteJobs.Count > 0)
                    {
                        DataBindJobTree(listDiscreteJobs, node);
                    }
                    advTree1.Nodes.Add(node);
                }
            }
        }

        private Node BuilderJobTree(Node groupNode)
        {
            DevComponents.AdvTree.ColumnHeader col1 = new DevComponents.AdvTree.ColumnHeader();
            col1.Name = "ascmWipEntities_Name";
            col1.Text = "作业号";
            col1.Width.Absolute = 140;
            groupNode.NodesColumns.Add(col1);

            DevComponents.AdvTree.ColumnHeader col2 = new DevComponents.AdvTree.ColumnHeader();
            col2.Name = "dateReleased";
            col2.Text = "作业日期";
            col2.Width.Absolute = 80;
            groupNode.NodesColumns.Add(col2);

            DevComponents.AdvTree.ColumnHeader col3 = new DevComponents.AdvTree.ColumnHeader();
            col3.Name = "ascmMaterialItem_DocNumber";
            col3.Text = "装配件";
            col3.Width.Absolute = 120;
            groupNode.NodesColumns.Add(col3);

            DevComponents.AdvTree.ColumnHeader col4 = new DevComponents.AdvTree.ColumnHeader();
            col4.Name = "ascmMaterialItem_Description";
            col4.Text = "装配件描述";
            col4.Width.Absolute = 150;
            groupNode.NodesColumns.Add(col4);

            DevComponents.AdvTree.ColumnHeader col5 = new DevComponents.AdvTree.ColumnHeader();
            col5.Name = "netQuantity";
            col5.Text = "计划数量";
            col5.Width.Absolute = 80;
            groupNode.NodesColumns.Add(col5);

            DevComponents.AdvTree.ColumnHeader col6 = new DevComponents.AdvTree.ColumnHeader();
            col6.Name = "totalRequiredQuantity";
            col6.Text = "总数量";
            col6.Width.Absolute = 70;
            groupNode.NodesColumns.Add(col6);

            DevComponents.AdvTree.ColumnHeader col7 = new DevComponents.AdvTree.ColumnHeader();
            col7.Name = "totalPreparationQuantity";
            col7.Text = "备料数";
            col7.Width.Absolute = 70;
            groupNode.NodesColumns.Add(col7);

            DevComponents.AdvTree.ColumnHeader col8 = new DevComponents.AdvTree.ColumnHeader();
            col8.Name = "totalGetMaterialQuantity";
            col8.Text = "领料数";
            col8.Width.Absolute = 80;
            groupNode.NodesColumns.Add(col8);

            groupNode.NodesColumnsHeaderVisible = true;

            return groupNode;
        }

        private List<AscmWipDiscreteJobs> GetJobTree(int jobId)
        {
            List<AscmWipDiscreteJobs> listJobs = null;
            string message = string.Empty;

            try
            {
                WinForm.AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                string jsonStr = Service.GetJobList(frmMain.encryptTicket, jobId, ref message);
                if (!string.IsNullOrEmpty(jsonStr))
                {
                    listJobs = (List<AscmWipDiscreteJobs>)JsonConvert.DeserializeObject(jsonStr, typeof(List<AscmWipDiscreteJobs>));
                    if (listJobs == null || listJobs.Count == 0)
                    {
                        MessageBoxEx.Show("没有查询到符合条件的领料任务!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listJobs;
        }

        private void DataBindJobTree(List<AscmWipDiscreteJobs> list, Node groupNode)
        {
            foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in list)
            {
                Node subItemNode = new Node();
                subItemNode.Tag = ascmWipDiscreteJobs;
                subItemNode.Text = ascmWipDiscreteJobs.ascmWipEntities_Name;
                if (!string.IsNullOrEmpty(ascmWipDiscreteJobs.dateReleased))
                {
                    subItemNode.Cells.Add(new Cell(ascmWipDiscreteJobs.dateReleased));
                }
                else
                {
                    string str = " ";
                    subItemNode.Cells.Add(new Cell(str));
                }
                subItemNode.Cells.Add(new Cell(ascmWipDiscreteJobs.ascmMaterialItem_DocNumber));
                subItemNode.Cells.Add(new Cell(ascmWipDiscreteJobs.ascmMaterialItem_Description));
                subItemNode.Cells.Add(new Cell(ascmWipDiscreteJobs.netQuantity.ToString()));
                subItemNode.Cells.Add(new Cell(ascmWipDiscreteJobs.totalRequiredQuantity.ToString()));
                subItemNode.Cells.Add(new Cell(ascmWipDiscreteJobs.totalPreparationQuantity.ToString()));
                subItemNode.Cells.Add(new Cell(ascmWipDiscreteJobs.totalGetMaterialQuantity.ToString()));
                groupNode.Nodes.Add(subItemNode);
            }
        }

        private int pageControlTask_EventPaging(PageControl.EventPagingArg e)
        {
            int count = 0;
            List<AscmGetMaterialTask> listTasks = GetTaskTree(ref count);
            if (listTasks != null && listTasks.Count > 0)
            {
                DataBindTaskTree(listTasks);
            }
            pageControlTask.DataBind(listTasks);

            DataBindTaskTree(listTasks);
            return count;
        }

        private void advTree1_NodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            try
            {
                if (e.Node.Parent != null && !string.IsNullOrEmpty(e.Node.Parent.Text))
                {
                    //string str = e.Node.Text + "," + e.Node.Parent.Text;
                    //MessageBox.Show(str);
                    frmMaterialList MaterialList = new frmMaterialList();
                    MaterialList.DiscreteJobsId = e.Node.Text;
                    MaterialList.GetMaterialTaskId = e.Node.Parent.Text;
                    MaterialList.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void tsbStartTask_Click(object sender, EventArgs e)
        {
            try
            {
                string str = string.Empty;
                string message = string.Empty;
                foreach (Node node in advTree1.Nodes)
                {
                    if (node.Checked)
                    {
                        if (!string.IsNullOrEmpty(str))
                            str += ",";
                        str += node.Text;
                    }
                }
                if (!string.IsNullOrEmpty(str))
                {
                    WinForm.AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                    if (Service.StartExcuteTask(frmMain.encryptTicket, frmMain.userName, str, ref message))
                    {
                        MessageBoxEx.Show("任务开始执行!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            MessageBox.Show(message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("请检查任务状态，无法执行开始操作！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        
                    }
                }
                else
                {
                    MessageBox.Show("请选择任务！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                pageControlTask.Bind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void tsbEndTask_Click(object sender, EventArgs e)
        {
            try
            {

                string message = string.Empty;
                WinForm.AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                if (Service.EndExcuteTask(frmMain.encryptTicket, frmMain.userName, ref message))
                {
                    MessageBoxEx.Show("正在执行的任务结束!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pageControlTask.Bind();
                }
                else
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        MessageBox.Show(message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("请检查任务领料数量，无法执行结束任务操作！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    pageControlTask.Bind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void tscbTaskStatus_TextChanged(object sender, EventArgs e)
        {
            pageControlTask.Bind();
        }

        private void tsbCheckTask_Click(object sender, EventArgs e)
        {
            try
            {
                string str = string.Empty;
                foreach (Node node in advTree1.Nodes)
                {
                    if (node.IsSelected)
                    {
                        str = node.Text;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(str))
                {
                    frmCheckTaskInfo CheckTaskInfo = new frmCheckTaskInfo();
                    CheckTaskInfo.GetMaterialTaskId = str;
                    CheckTaskInfo.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void tsbSumMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                frmSumMaterialInfo SumMaterialInfo = new frmSumMaterialInfo();
                SumMaterialInfo.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void tsbGetMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                string str = string.Empty;
                string message = string.Empty;
                foreach (Node node in advTree1.Nodes)
                {
                    if (node.Checked)
                    {
                        if (!string.IsNullOrEmpty(str))
                            str += ",";
                        str += node.Text;
                    }
                }

                if (!string.IsNullOrEmpty(str))
                {
                    WinForm.AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                    Service.ConfrimedGetMaterial(frmMain.encryptTicket, frmMain.userName, str, ref message);
                    pageControlTask.Bind();
                    if (!string.IsNullOrEmpty(message))
                    {
                        
                        MessageBox.Show(message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("请检查该任务备料数量，无法执行领料操作！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("请选择任务！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
