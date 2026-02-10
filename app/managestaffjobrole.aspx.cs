using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class managestaffjobrole : ERPBase
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
                success = BUStaff.AddStaffJobRole(collection);
            }
            else
            {
                success = BUStaff.UpdateStaffJobRole(collection, this.hdfilter.Value);
            }

            if (success)
            {
                Response.Redirect("managestaffjobrole.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }
    }
}