using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevComponents.DotNetBar;
using MideaAscm.Dal.FromErp.Entities;
using WinForm.AscmWebService;
using Newtonsoft.Json;

namespace WinForm.Material
{
    public partial class wipDiscreteJobs : Office2007Form
    {
        public int materialId = 0;
        public wipDiscreteJobs(int id)
        {
            InitializeComponent();
            materialId = id;
            InitializeDate();
        }

        private void InitializeDate()
        {
            startDate.Value = startDate.Value.AddMonths(-6);
            labelMaterialId.Text = "物料ID: " + materialId;
        }

        private void wipDiscreteJobs_Load(object sender, EventArgs e)
        {
            pageControlJob.Bind();
        }

        private int pageControlJob_EventPaging(PageControl.EventPagingArg e)
        {
            int count = 0;
            List<AscmWipDiscreteJobs> listJobs = getWipDiscreteJobsList(ref count);
            pageControlJob.DataBind(listJobs);

            if (dataGridViewJobs.DataSource != null)
            {
                dataGridViewJobs.DataSource = null;
                dataGridViewJobs.Refresh();
            }
            dataGridViewJobs.AutoGenerateColumns = false;
            dataGridViewJobs.DataSource = pageControlJob.bindingSource;
            return count;
        }

        #region 全局变量
        #endregion

        private List<AscmWipDiscreteJobs> getWipDiscreteJobsList(ref int count)
        {
            List<AscmWipDiscreteJobs> listJobs = null;
            string message = string.Empty;
            string _ynPage = string.Empty;

            try
            {
                YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
                ynPage.SetPageSize(pageControlJob.PageSize);
                ynPage.SetCurrentPage((pageControlJob.PageCurrent <= 0) ? 1 : pageControlJob.PageCurrent);
                _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);

                string startdate = startDate.Value.ToString("yyyy-MM-dd");
                string enddate = endDate.Value.ToString("yyyy-MM-dd");

                AscmWebService.AscmWebService service = new AscmWebService.AscmWebService();
                string jsonstr = service.MaterialOfDiscreteJobList(frmMain.encryptTicket, materialId, startdate, enddate, ref _ynPage, ref message);
                if (!string.IsNullOrEmpty(jsonstr))
                {
                    listJobs = (List<AscmWipDiscreteJobs>)JsonConvert.DeserializeObject(jsonstr, typeof(List<AscmWipDiscreteJobs>));
                    if (listJobs != null && listJobs.Count > 0)
                    {
                        ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);
                        count = ynPage.GetRecordCount();
                    }
                    else
                    {
                        MessageBoxEx.Show("没有查询到符合条件的作业!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message + message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return listJobs;
        }

        private void btnJobsQuery_Click(object sender, EventArgs e)
        {
            pageControlJob.Bind();
        }
    }
}
