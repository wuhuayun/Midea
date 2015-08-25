using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using MideaAscm.Dal.Base.Entities;
using Newtonsoft.Json;

namespace MideaAscm.Pad
{
    public partial class frmSelectSignMaterialDialog : DevComponents.DotNetBar.RibbonForm
    {
        private string textboxQueryWord = string.Empty;
        public string queryResult { get; set; }
        public string queryId { get; set; }

        public frmSelectSignMaterialDialog()
        {
            InitializeComponent();
        }

        private void frmSelectSignMaterialDialog_Load(object sender, EventArgs e)
        {
            StructureDataGridViewColumns(this.dgViewMaterialList);
        }

        private void StructureDataGridViewColumns(DataGridViewX dataGridViewX)
        {
            if (dataGridViewX.Columns.Count > 0)
                dataGridViewX.Columns.Clear();

            DataGridViewTextBoxColumn id = new DataGridViewTextBoxColumn();
            id.HeaderText = "ID";
            id.Name = "id";
            id.DataPropertyName = "id";
            id.Width = 0;
            id.Visible = false;

            DataGridViewTextBoxColumn docNumber = new DataGridViewTextBoxColumn();
            docNumber.HeaderText = "组件";
            docNumber.Name = "docNumber";
            docNumber.DataPropertyName = "docNumber";
            docNumber.Width = 110;

            DataGridViewTextBoxColumn description = new DataGridViewTextBoxColumn();
            description.HeaderText = "组件说明";
            description.Name = "description";
            description.DataPropertyName = "description";
            description.Width = 200;

            DataGridViewTextBoxColumn _zMtlCategoryStatus = new DataGridViewTextBoxColumn();
            _zMtlCategoryStatus.HeaderText = "总装备料形式";
            _zMtlCategoryStatus.Name = "_zMtlCategoryStatus";
            _zMtlCategoryStatus.DataPropertyName = "_zMtlCategoryStatus";
            _zMtlCategoryStatus.Width = 110;

            DataGridViewTextBoxColumn _dMtlCategoryStatus = new DataGridViewTextBoxColumn();
            _dMtlCategoryStatus.HeaderText = "电装备料形式";
            _dMtlCategoryStatus.Name = "_dMtlCategoryStatus";
            _dMtlCategoryStatus.DataPropertyName = "_dMtlCategoryStatus";
            _dMtlCategoryStatus.Width = 110;

            DataGridViewTextBoxColumn _wMtlCategoryStatus = new DataGridViewTextBoxColumn();
            _wMtlCategoryStatus.HeaderText = "其他备料形式";
            _wMtlCategoryStatus.Name = "_wMtlCategoryStatus";
            _wMtlCategoryStatus.DataPropertyName = "_wMtlCategoryStatus";
            _wMtlCategoryStatus.Width = 110;

            DataGridViewTextBoxColumn wipSupplyTypeCn = new DataGridViewTextBoxColumn();
            wipSupplyTypeCn.HeaderText = "供应类型";
            wipSupplyTypeCn.Name = "wipSupplyTypeCn";
            wipSupplyTypeCn.DataPropertyName = "wipSupplyTypeCn";
            wipSupplyTypeCn.Width = 80;


            dataGridViewX.Columns.Add(id);
            dataGridViewX.Columns.Add(docNumber);
            dataGridViewX.Columns.Add(description);
            dataGridViewX.Columns.Add(_zMtlCategoryStatus);
            dataGridViewX.Columns.Add(_dMtlCategoryStatus);
            dataGridViewX.Columns.Add(_wMtlCategoryStatus);
            dataGridViewX.Columns.Add(wipSupplyTypeCn);
        }

        private List<AscmMaterialItem> DataBindDataGridView(ref int count, string queryWord)
        {
            List<AscmMaterialItem> list = null;
            string message = string.Empty;
            string _ynPage = string.Empty;

            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(pagerControlMaterial.PageSize);
            ynPage.SetCurrentPage((pagerControlMaterial.PageCurrent <= 0) ? 1 : pagerControlMaterial.PageCurrent);
            _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);

            try
            {
                AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                string jsonString = Service.GetWarehouseMaterialList(frmMainView.encryptTicket, queryWord, ref _ynPage, ref message);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    list = (List<AscmMaterialItem>)JsonConvert.DeserializeObject(jsonString, typeof(List<AscmMaterialItem>));
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

        private int pagerControlMaterial_EventPaging(CustomControl.EventPagingArg e)
        {
            int count = 0;
            List<AscmMaterialItem> list = DataBindDataGridView(ref count, this.textboxQueryWord);
            pagerControlMaterial.DataBind(list);


            if (dgViewMaterialList.DataSource != null)
                this.dgViewMaterialList.DataSource = null;

            this.dgViewMaterialList.AutoGenerateColumns = false;
            this.dgViewMaterialList.DataSource = pagerControlMaterial.bindingSource;
            this.dgViewMaterialList.Refresh();
            return count;
        }

        private void btnItemSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbQueryWord.Text))
            {
                textboxQueryWord = tbQueryWord.Text;

                pagerControlMaterial.Bind();
            }
        }

        private void btnItemOk_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dgViewMaterialList.Rows)
            {
                if (row.Selected)
                {
                    queryResult = row.Cells[1].Value.ToString();
                    queryId = row.Cells[0].Value.ToString();
                    this.DialogResult = DialogResult.OK;
                    break;
                }
            }
        }
    }
}
