using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class staffreportorderdetails : ERPBase
    {
       
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {               
                ViewState["sid"] = DecryptQueryString("sid");//staff  Id
                this.ApplyFilter();
            }
        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("companyid", this.CompanyId);
            collection.Add("startdate", this.txtStartDate.Text.Trim());
            collection.Add("enddate", this.txtEndDate.Text.Trim());
            collection.Add("status", this.ddlStatus.SelectedValue);
            collection.Add("orderno", this.txtOrderNo.Text.Trim());
            
            if (!string.IsNullOrEmpty(this.ConvertToString(ViewState["sid"]))) collection.Add("staffid", this.ConvertToString(ViewState["sid"]));
            else collection.Add("staffid", string.Empty);

            this.hdfilter.Value = BUOrderManagement.SearchOrder(collection);

        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }
    }
}