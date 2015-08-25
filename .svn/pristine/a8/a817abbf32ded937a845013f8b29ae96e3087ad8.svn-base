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
using DevComponents.AdvTree;


namespace WinForm
{
    public partial class frmMaterialView : Office2007Form
    {
        public frmMaterialView()
        {
            InitializeComponent();
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
            string jsonstr = service.GetMaterialList(frmMain.encryptTicket, queryMaterialDocNumber, ref _ynPage, ref message);
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

        private void frmMaterialView_Load(object sender, EventArgs e)
        {
            pageControlMaterial.Bind();
        }
    }
}
