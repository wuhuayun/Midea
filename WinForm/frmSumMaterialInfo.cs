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
    public partial class frmSumMaterialInfo : Office2007Form
    {
        public frmSumMaterialInfo()
        {
            InitializeComponent();
        }

        private void frmSumMaterialInfo_Load(object sender, EventArgs e)
        {
            pcMaterialList.Bind();
        }

        private int pcMaterialList_EventPaging(PageControl.EventPagingArg e)
        {
            int count = 0;
            List<AscmMaterialItem> list = GetMaterialItem(ref count);
            pcMaterialList.DataBind(list);

            if (dgMaterialList.DataSource != null)
            {
                dgMaterialList.DataSource = null;
                dgMaterialList.Refresh();
            }
            dgMaterialList.AutoGenerateColumns = false;
            dgMaterialList.DataSource = pcMaterialList.bindingSource;
            return count;
        }

        private List<AscmMaterialItem> GetMaterialItem(ref int count)
        {
            List<AscmMaterialItem> list = null;
            string message = string.Empty;
            string _ynPage = string.Empty;

            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(pcMaterialList.PageSize);
            ynPage.SetCurrentPage((pcMaterialList.PageCurrent <= 0) ? 1 : pcMaterialList.PageCurrent);
            _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);
            try
            {
                
                WinForm.AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                string jsonStr = Service.SumMaterialTotal(frmMain.encryptTicket, ref _ynPage, ref message);
                if (!string.IsNullOrEmpty(jsonStr))
                {
                    list = (List<AscmMaterialItem>)JsonConvert.DeserializeObject(jsonStr, typeof(List<AscmMaterialItem>));
                    if (list != null && list.Count > 0)
                    {
                        ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);
                        count = ynPage.GetRecordCount();
                    }
                    else
                    {
                        MessageBoxEx.Show("系统没查到当天的作业单的所有物料!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
    }
}
