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

namespace WinForm
{
    public partial class frmMaterialList : Office2007Form
    {
        /// <summary>领料任务编号</summary>
        public string GetMaterialTaskId { get; set; }
        /// <summary>作业编号</summary>
        public string DiscreteJobsId { get; set; }

        public frmMaterialList()
        {
            InitializeComponent();
        }

        private void frmMaterialList_Load(object sender, EventArgs e)
        {
            pageControlMaterial.Bind();
            //int count = 0;
            //dgMaterialList.DataSource = GetWipRequirementOperations(ref count);
            //dgMaterialList.AutoGenerateColumns = false;
            //dgMaterialList.Refresh();
        }

        private List<AscmWipRequirementOperations> GetWipRequirementOperations(ref int count)
        {
            List<AscmWipRequirementOperations> list = null;
            string message = string.Empty;
            string _ynPage = string.Empty;

            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(pageControlMaterial.PageSize);
            ynPage.SetCurrentPage((pageControlMaterial.PageCurrent <= 0) ? 1 : pageControlMaterial.PageCurrent);
            _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);

            try
            {
                if (!string.IsNullOrEmpty(GetMaterialTaskId) && !string.IsNullOrEmpty(DiscreteJobsId))
                {
                    WinForm.AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                    string jsonStr = Service.GetMtlList(frmMain.encryptTicket, GetMaterialTaskId, DiscreteJobsId, ref _ynPage, ref message);
                    if (!string.IsNullOrEmpty(jsonStr))
                    {
                        list = (List<AscmWipRequirementOperations>)JsonConvert.DeserializeObject(jsonStr, typeof(List<AscmWipRequirementOperations>));
                        if (list != null && list.Count > 0)
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        private int pageControlMaterial_EventPaging(PageControl.EventPagingArg e)
        {
            int count = 0;
            List<AscmWipRequirementOperations> list = GetWipRequirementOperations(ref count);
            pageControlMaterial.DataBind(list);

            if (dgMaterialList.DataSource != null)
            {
                dgMaterialList.DataSource = null;
                dgMaterialList.Refresh();
            }
            dgMaterialList.AutoGenerateColumns = false;
            dgMaterialList.DataSource = pageControlMaterial.bindingSource;
            return count;
        }
    }
}
