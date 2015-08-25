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
using Newtonsoft.Json;

namespace WinForm
{
    public partial class Main_Form : Office2007Form
    {
        public Main_Form()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 分页数据绑定事件 pageControl.bind()可触发
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private int pageControlTask_EventPaging(PageControl.EventPagingArg e)
        {
            int count = 0;
            List<AscmGetMaterialTask> listTasks = taskQuery(ref count);
            pageControlTask.DataBind(listTasks);

            if (dataGridViewTasks.DataSource != null)
            {
                dataGridViewTasks.DataSource = null;
                dataGridViewTasks.Refresh();
            }
            dataGridViewTasks.AutoGenerateColumns = false;
            dataGridViewTasks.DataSource = pageControlTask.bindingSource;
            return count;
        }
        /// <summary>
        /// 分页数据绑定事件 pageControl.bind()可触发
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private int pageControlMaterial_EventPaging(PageControl.EventPagingArg e)
        {
            int count = 0;
            List<AscmMaterialItem> listMaterials = materialQuery(ref count);
            pageControlMaterial.DataBind(listMaterials);

            if (dataGridViewMaterials.DataSource != null)
            {
                dataGridViewMaterials.DataSource = null;
                dataGridViewMaterials.Refresh();
            }
            dataGridViewMaterials.AutoGenerateColumns = false;
            dataGridViewMaterials.DataSource = pageControlMaterial.bindingSource;
            return count;
        }
        #region 全局变量
        /// <summary>
        /// 权限验证票证
        /// </summary>
        public static string encryptTicket = string.Empty;
        /// <summary>
        /// 登陆用户名
        /// </summary>
        public static string userName = string.Empty;
        #endregion

        #region 菜单事件
        /// <summary>
        /// 临时领料面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetMaterialTasks_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != tabGetMaterialTasks)
            {
                tabGetMaterialTasks.Visible = true;
                tabControl.SelectedTab = tabGetMaterialTasks;
            }
        }
        /// <summary>
        /// 添加临时领料任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMaterialTask_Click(object sender, EventArgs e)
        {
            WinForm.Task.AddTask Form = new WinForm.Task.AddTask();
            Form.refreshHandler += btnTasksSearch_Click;
            Form.ShowDialog();
        }
        /// <summary>
        /// 注销登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnlogout_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.Show("确定注销登录?", "注销", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Application.Exit();
                System.Diagnostics.Process.Start("WinForm");
            }
        }
        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnexit_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.Show("确定退出本系统?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        #endregion

        #region 临时领料任务
        /// <summary>
        /// 任务分页查询 返回数量与单页数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<AscmGetMaterialTask> taskQuery(ref int count)
        {
            List<AscmGetMaterialTask> listTasks = null;
            string message = string.Empty;
            string _ynPage = string.Empty;

            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(pageControlTask.PageSize);
            ynPage.SetCurrentPage((pageControlTask.PageCurrent <= 0) ? 1 : pageControlTask.PageCurrent);
            _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
            //try
            //{
                string queryStatus = (cbxTaskStatus.SelectedItem == null) ? string.Empty : cbxTaskStatus.SelectedItem.ToString().Trim();
                if (!String.IsNullOrEmpty(queryStatus))
                {
                    switch (queryStatus)
                    {
                        case "已完成":
                            queryStatus = "FINISH";
                            break;
                        case "执行中":
                            queryStatus = "EXECUTE";
                            break;
                        case "已分配":
                            queryStatus = "NOTEXECUTE";
                            break;
                        case "未分配":
                            queryStatus = "NOTALLOCATE";
                            break;
                        default:
                            queryStatus = "";
                            break;
                    }
                }
                string queryDate = taskCreateTime.Text.Trim();
                if (!String.IsNullOrEmpty(queryDate))
                {
                    if (queryDate.LastIndexOf("星") > 0)
                    {
                        queryDate = queryDate.Substring(0, queryDate.LastIndexOf("星"));
                    }
                    queryDate = DateTime.Parse(queryDate).ToString("yyyy-MM-dd");
                }
                WinForm.AscmWebService.AscmWebService service = new AscmWebService.AscmWebService();
                string jsonstr = service.GetTasksList(encryptTicket, queryStatus, queryDate, ref _ynPage, ref message);
                if (!string.IsNullOrEmpty(jsonstr))
                {
                    listTasks = (List<AscmGetMaterialTask>)JsonConvert.DeserializeObject(jsonstr, typeof(List<AscmGetMaterialTask>));
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
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return listTasks;
        }
        /// <summary>
        /// 任务查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTasksSearch_Click(object sender, EventArgs e)
        {
            pageControlTask.Bind();
        }
        /// <summary>
        /// 任务添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTaskAdd_Click(object sender, EventArgs e)
        {
            WinForm.Task.AddTask Form = new WinForm.Task.AddTask();
            Form.refreshHandler += btnTasksSearch_Click;
            Form.ShowDialog();
        }
        /// <summary>
        /// 任务编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTaskEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewTasks.SelectedRows == null || dataGridViewTasks.SelectedRows.Count == 0)
            {
                MessageBoxEx.Show("请先选中要修改的数据行!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                AscmGetMaterialTask task = new AscmGetMaterialTask();
                task.id = int.Parse((dataGridViewTasks.SelectedRows[0].Cells["ID"].Value == null) ? string.Empty : dataGridViewTasks.SelectedRows[0].Cells["ID"].Value.ToString());
                task.productLine = (dataGridViewTasks.SelectedRows[0].Cells["productLine"].Value == null) ? string.Empty : dataGridViewTasks.SelectedRows[0].Cells["productLine"].Value.ToString();
                task.warehouserId = (dataGridViewTasks.SelectedRows[0].Cells["warehouserId"].Value == null) ? string.Empty : dataGridViewTasks.SelectedRows[0].Cells["warehouserId"].Value.ToString();
                task.mtlCategoryStatus = (dataGridViewTasks.SelectedRows[0].Cells["mtlCategoryStatus"].Value == null) ? string.Empty : dataGridViewTasks.SelectedRows[0].Cells["mtlCategoryStatus"].Value.ToString();
                task.rankerId = (dataGridViewTasks.SelectedRows[0].Cells["rankerId"].Value == null) ? string.Empty : dataGridViewTasks.SelectedRows[0].Cells["rankerId"].Value.ToString();
                task.IdentificationId = int.Parse((dataGridViewTasks.SelectedRows[0].Cells["IdentificationId"].Value == null) ? string.Empty : dataGridViewTasks.SelectedRows[0].Cells["IdentificationId"].Value.ToString());
                task.materialDocNumber = (dataGridViewTasks.SelectedRows[0].Cells["materialDocNumber"].Value == null) ? string.Empty : dataGridViewTasks.SelectedRows[0].Cells["materialDocNumber"].Value.ToString();
                task.taskTime = (dataGridViewTasks.SelectedRows[0].Cells["taskTime"].Value == null) ? string.Empty : dataGridViewTasks.SelectedRows[0].Cells["taskTime"].Value.ToString();
                task.tip = (dataGridViewTasks.SelectedRows[0].Cells["tip"].Value == null) ? string.Empty : dataGridViewTasks.SelectedRows[0].Cells["tip"].Value.ToString();
                task.workerId = (dataGridViewTasks.SelectedRows[0].Cells["workerId"].Value == null) ? string.Empty : dataGridViewTasks.SelectedRows[0].Cells["workerId"].Value.ToString();

                WinForm.Task.EditTask Form = new WinForm.Task.EditTask(task);
                Form.refreshHandler += btnTasksSearch_Click;
                Form.ShowDialog();
            }
        }
        /// <summary>
        /// 任务删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTaskDelete_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.Show("确认删除所选任务?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (dataGridViewTasks.SelectedRows == null)
                {
                    MessageBoxEx.Show("请先选中要删除的数据行!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    string message = string.Empty;
                    try
                    {
                        int id = int.Parse(dataGridViewTasks.SelectedRows[0].Cells["ID"].Value.ToString());
                        WinForm.AscmWebService.AscmWebService service = new AscmWebService.AscmWebService();
                        if (service.TaskDelete(Main_Form.encryptTicket, id, ref message))
                        {
                            btnTasksSearch_Click(this, new EventArgs());
                            MessageBoxEx.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBoxEx.Show(message, "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBoxEx.Show(ex.Message, "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnTaskEdit_Click(this, new EventArgs());
        }
        #endregion

        #region 关于
        private void mAboutUs_Click(object sender, EventArgs e)
        {
            new WinForm.Help.AboutUs().ShowDialog();
        }
        #endregion

        #region 仓库物料查询
        private void btnMaterialSearch_Click(object sender, EventArgs e)
        {
            pageControlMaterial.Bind();
        }
        /// <summary>
        /// 物料查询 返回查询总数量与单页数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<AscmMaterialItem> materialQuery(ref int count)
        {
            List<AscmMaterialItem> listMaterials = null;
            string message = string.Empty;
            string _ynPage = string.Empty;

            string queryMaterialDocNumber = txtMaterialDocNumber.Text;
            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(pageControlMaterial.PageSize);
            ynPage.SetCurrentPage((pageControlMaterial.PageCurrent <= 0) ? 1 : pageControlMaterial.PageCurrent);
            _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);

            WinForm.AscmWebService.AscmWebService service = new AscmWebService.AscmWebService();
            string jsonstr = service.GetMaterialList(encryptTicket, queryMaterialDocNumber, ref _ynPage, ref message);
            if (!string.IsNullOrEmpty(jsonstr))
            {
                listMaterials = (List<AscmMaterialItem>)JsonConvert.DeserializeObject(jsonstr, typeof(List<AscmMaterialItem>));
                if (listMaterials != null && listMaterials.Count > 0)
                {
                    ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);
                    count = ynPage.GetRecordCount();
                }
                else
                {
                    MessageBoxEx.Show("没有查询到符合条件的领料任务!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            return listMaterials;
        }
        /// <summary>
        /// 双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewMaterials_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnQueryJobs_Click(this, new EventArgs());
        }
        /// <summary>
        /// 关联任务查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryJobs_Click(object sender, EventArgs e)
        {
            if (dataGridViewMaterials.SelectedRows == null || dataGridViewMaterials.SelectedRows.Count == 0)
            {
                MessageBoxEx.Show("请先选中数据行!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                int id = int.Parse((dataGridViewMaterials.SelectedRows[0].Cells["ID_M"].Value == null) ? string.Empty : dataGridViewMaterials.SelectedRows[0].Cells["ID_M"].Value.ToString());
                WinForm.Material.wipDiscreteJobs Form = new WinForm.Material.wipDiscreteJobs(id);
                Form.ShowDialog();
            }
        }
        #endregion
    }
}