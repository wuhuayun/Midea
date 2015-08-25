using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar;
using MideaAscm.Dal.FromErp.Entities;
using Newtonsoft.Json;

namespace MideaAscm.Pad
{
    public partial class frmSumMaterialListDialog : DevComponents.DotNetBar.RibbonForm
    {
        private MonthCalendar monthCalendar;  
        
        public frmSumMaterialListDialog()
        {
            InitializeComponent();
        }

        private void frmSumMaterialListDialog_Load(object sender, EventArgs e)
        {
            tbItemStart.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            tbItemEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
            
            StructureDataGridViewColumns(this.dgViewMaterialList);
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

            DataGridViewTextBoxColumn ascmMaterialItem_DocNumber = new DataGridViewTextBoxColumn();
            ascmMaterialItem_DocNumber.HeaderText = "组件";
            ascmMaterialItem_DocNumber.Name = "ascmMaterialItem_DocNumber";
            ascmMaterialItem_DocNumber.DataPropertyName = "ascmMaterialItem_DocNumber";
            ascmMaterialItem_DocNumber.Width = 110;

            DataGridViewTextBoxColumn ascmMaterialItem_Description = new DataGridViewTextBoxColumn();
            ascmMaterialItem_Description.HeaderText = "组件说明";
            ascmMaterialItem_Description.Name = "ascmMaterialItem_Description";
            ascmMaterialItem_Description.DataPropertyName = "ascmMaterialItem_Description";
            ascmMaterialItem_Description.Width = 200;

            DataGridViewTextBoxColumn requiredQuantity = new DataGridViewTextBoxColumn();
            requiredQuantity.HeaderText = "需求数";
            requiredQuantity.Name = "requiredQuantity";
            requiredQuantity.DataPropertyName = "requiredQuantity";
            requiredQuantity.Width = 70;

            DataGridViewTextBoxColumn quantityIssued = new DataGridViewTextBoxColumn();
            quantityIssued.HeaderText = "发料数";
            quantityIssued.Name = "quantityIssued";
            quantityIssued.DataPropertyName = "quantityIssued";
            quantityIssued.Width = 70;

            DataGridViewTextBoxColumn getMaterialQuantity = new DataGridViewTextBoxColumn();
            getMaterialQuantity.HeaderText = "领料数";
            getMaterialQuantity.Name = "getMaterialQuantity";
            getMaterialQuantity.DataPropertyName = "getMaterialQuantity";
            getMaterialQuantity.Width = 70;

            DataGridViewTextBoxColumn quantityDifference = new DataGridViewTextBoxColumn();
            quantityDifference.HeaderText = "发料差异";
            quantityDifference.Name = "quantityDifference";
            quantityDifference.DataPropertyName = "quantityDifference";
            quantityDifference.Width = 80;

            DataGridViewTextBoxColumn quantityGetMaterialDifference = new DataGridViewTextBoxColumn();
            quantityGetMaterialDifference.HeaderText = "领料差异";
            quantityGetMaterialDifference.Name = "quantityGetMaterialDifference";
            quantityGetMaterialDifference.DataPropertyName = "quantityGetMaterialDifference";
            quantityGetMaterialDifference.Width = 80;

            //DataGridViewTextBoxColumn ascmMaterialItem_Warehouse = new DataGridViewTextBoxColumn();
            //ascmMaterialItem_Warehouse.HeaderText = "子库";
            //ascmMaterialItem_Warehouse.Name = "ascmMaterialItem_Warehouse";
            //ascmMaterialItem_Warehouse.DataPropertyName = "ascmMaterialItem_Warehouse";
            //ascmMaterialItem_Warehouse.Width = 80;

            //DataGridViewTextBoxColumn wipSupplyTypeCn = new DataGridViewTextBoxColumn();
            //wipSupplyTypeCn.HeaderText = "供应类型";
            //wipSupplyTypeCn.Name = "wipSupplyTypeCn";
            //wipSupplyTypeCn.DataPropertyName = "wipSupplyTypeCn";
            //wipSupplyTypeCn.Width = 200;

            dataGridViewX.Columns.Add(ascmMaterialItem_DocNumber);
            dataGridViewX.Columns.Add(ascmMaterialItem_Description);
            dataGridViewX.Columns.Add(requiredQuantity);
            dataGridViewX.Columns.Add(quantityIssued);
            dataGridViewX.Columns.Add(getMaterialQuantity);
            dataGridViewX.Columns.Add(quantityDifference);
            dataGridViewX.Columns.Add(quantityGetMaterialDifference);
            //dataGridViewX.Columns.Add(ascmMaterialItem_Warehouse);
            //dataGridViewX.Columns.Add(wipSupplyTypeCn);
        }

        private List<AscmWipRequirementOperations> DataBindDataGridView(ref int count, string queryStartTime, string queryEndTime)
        {
            List<AscmWipRequirementOperations> list = null;
            string message = string.Empty;
            string _ynPage = string.Empty;

            YnBaseDal.YnPage ynPage = new YnBaseDal.YnPage();
            ynPage.SetPageSize(pagerControlMaterial.PageSize);
            ynPage.SetCurrentPage((pagerControlMaterial.PageCurrent <= 0) ? 1 : pagerControlMaterial.PageCurrent);
            _ynPage = YnBaseClass2.Helper.ObjectHelper.Serialize(ynPage);

            try
            {
                AscmWebService.AscmWebService Service = new AscmWebService.AscmWebService();
                string jsonString = Service.SumMaterial(frmMainView.encryptTicket, frmMainView.userName, queryStartTime, queryEndTime, ref _ynPage, ref message);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    list = (List<AscmWipRequirementOperations>)JsonConvert.DeserializeObject(jsonString, typeof(List<AscmWipRequirementOperations>));
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

        private void btnItemSum_Click(object sender, EventArgs e)
        {
            ButtonItem buttonItem = sender as ButtonItem;
            if (buttonItem != null)
                buttonItem.Focus();

            try
            {

                if (string.IsNullOrEmpty(tbItemStart.Text) || string.IsNullOrEmpty(tbItemEnd.Text))
                    throw new Exception("汇总物料失败：请选择起止日期！");

                pagerControlMaterial.Bind();

            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int pagerControlMaterial_EventPaging(CustomControl.EventPagingArg e)
        {
            int count = 0;
            List<AscmWipRequirementOperations> list = DataBindDataGridView(ref count, tbItemStart.Text, tbItemEnd.Text);
            pagerControlMaterial.DataBind(list);

            if (dgViewMaterialList.DataSource != null)
                this.dgViewMaterialList.DataSource = null;

            this.dgViewMaterialList.AutoGenerateColumns = false;
            this.dgViewMaterialList.DataSource = pagerControlMaterial.bindingSource;
            this.dgViewMaterialList.Refresh();

            return count;
        }


    }
}
