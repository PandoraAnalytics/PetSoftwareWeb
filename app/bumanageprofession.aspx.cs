using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class bumanageprofession : ERPBase
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
            AnimalBA objCRM = new AnimalBA();

            if (this.hdfilter.Value.Length == 0)
            {
                success = objCRM.AddProfession(collection);
            }
            else
            {
                success = objCRM.UpdateProfession(collection, this.hdfilter.Value);
            }

            if (success)
            {
                Response.Redirect("bumanageprofession.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }
    }
}