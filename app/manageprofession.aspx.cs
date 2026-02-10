using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class manageprofession : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            this.IsAdminAccess = true;
            base.Page_Load(sender, e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());

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
                Response.Redirect("manageprofession.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }
    }
}