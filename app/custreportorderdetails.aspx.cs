using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BABusiness;
using System.Collections.Specialized;
using System.Data;


namespace Breederapp
{
    public partial class custreportorderdetails : ERPBase
    {
        
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["cid"] = DecryptQueryString("cid");//customer Id        
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

            if (!string.IsNullOrEmpty(this.ConvertToString(ViewState["cid"]))) collection.Add("cid", this.ConvertToString(ViewState["cid"]));
            else collection.Add("cid", string.Empty);

            this.hdfilter.Value = BUOrderManagement.SearchOrder(collection);

        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }
    }
}