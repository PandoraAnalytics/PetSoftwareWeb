using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;

namespace Breederapp
{
    public partial class bupospastorder : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                PopulateControls();
                this.ApplyFilter();
            }
        }

        private void PopulateControls()
        {
            DataTable dtcustomer = BUOrderManagement.GetPOSCustomer(this.CompanyId);
            if (dtcustomer != null)
            {
                DataRow row = dtcustomer.NewRow();
                row["id"] = int.MinValue;
                row["fullname"] = "- Customer Name -";
                dtcustomer.Rows.InsertAt(row, 0);
            }
            this.ddlCustomer.DataSource = dtcustomer;
            this.ddlCustomer.DataBind();

        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("companyid", this.CompanyId);
            collection.Add("startdate", this.txtStartDate.Text.Trim());
            collection.Add("enddate", this.txtEndDate.Text.Trim());
            if (this.ConvertToInteger(this.ddlCustomer.SelectedValue) > 0) collection.Add("customerid", this.ddlCustomer.SelectedValue);
            //collection.Add("customerid", this.ddlCustomer.SelectedValue);
            collection.Add("status", this.ddlStatus.SelectedValue);
            collection.Add("orderno", this.txtOrderNo.Text.Trim());
            collection.Add("ispos", "1");
            this.hdfilter.Value = BUOrderManagement.SearchOrder(collection);

        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }
    }
}