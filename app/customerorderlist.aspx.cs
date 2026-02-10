using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;

namespace Breederapp
{
    public partial class customerorderlist : PageBase
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
            DataTable dtcustomer = BUOrderManagement.GetAllBUForCustomer(this.UserId);
            if (dtcustomer != null)
            {
                DataRow row = dtcustomer.NewRow();
                row["id"] = int.MinValue;
                row["companyname"] ="- Company Name -";
                dtcustomer.Rows.InsertAt(row, 0);
            }
            this.ddlCompany.DataSource = dtcustomer;
            this.ddlCompany.DataBind();
        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            //collection.Add("companyid", this.CompanyId);
            collection.Add("startdate", this.txtStartDate.Text.Trim());
            collection.Add("enddate", this.txtEndDate.Text.Trim());
            if (this.ConvertToInteger(this.ddlCompany.SelectedValue) > 0) collection.Add("companyid", this.ddlCompany.SelectedValue);
            collection.Add("status", this.ddlStatus.SelectedValue);
            collection.Add("orderno", this.txtOrderNo.Text.Trim());
            this.hdfilter.Value = BUOrderManagement.SearchOrder(collection);

        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }
    }
}