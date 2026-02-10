using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class bumanagetax : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {            
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {           

            for (int i = 0; i <= 100; i++)
            {
                ddlPercentage.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("percentage", this.ddlPercentage.SelectedValue);
            collection.Add("companyid", this.CompanyId);

            bool success = false;
            if (this.hdfilter.Value.Length == 0)
            {
                int id = BUProduct.AddBUTax(collection);
                success = (id > 0);
            }
            else
            {
                success = BUProduct.UpdateBUTax(collection, this.hdfilter.Value);
            }

            if (success)
            {
                Response.Redirect("bumanagetax.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }
    }
}