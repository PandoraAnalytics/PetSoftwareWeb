using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class bumanagecurrency : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("companyid", this.CompanyId);

            bool success = false;
            if (this.hdfilter.Value.Length == 0)
            {
                int id = BUProduct.AddBUCurrency(collection);
                success = (id > 0);
            }
            else
            {
                success = BUProduct.UpdateBUCurrency(collection, this.hdfilter.Value);
            }

            if (success)
            {
                Response.Redirect("bumanagecurrency.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }
    }
}