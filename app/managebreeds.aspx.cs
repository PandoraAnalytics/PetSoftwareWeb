using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class managebreeds : PageBase
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
            collection.Add("breedname", this.txtName.Text.Trim());
            collection.Add("breedimage", this.hid_profile_pic.Value);

            bool success = false;
            AnimalBA objCRM = new AnimalBA();

            if (this.hdfilter.Value.Length == 0)
            {
                success = objCRM.AddBreedCategory(collection);
            }
            else
            {
                success = objCRM.UpdateBreedCategory(collection, this.hdfilter.Value);
            }

            if (success)
            {
                Response.Redirect("managebreeds.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }
    }
}