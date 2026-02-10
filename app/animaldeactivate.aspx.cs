using BABusiness;
using System;
using System.Web.UI;

namespace Breederapp
{
    public partial class animaldeactivate : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = this.ReadQueryString("id");
                (Page.Master as breeder).AnimalId = ViewState["id"].ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            if (this.deactivate.Checked == false && this.delete.Checked == false)
            {
                return;
            }


            if (!UserBA.ValidateAnimalUserRelation(ViewState["id"], this.UserId))
            {
                return;
            }

            AnimalBA obj = new AnimalBA();
            if (this.deactivate.Checked)
            {
                bool success = obj.DeactivateAnimal(ViewState["id"], this.txtReason.Text.Trim());
                if (success) Response.Redirect("landing.aspx");
                else
                {
                    this.lblError.Text = Resources.Resource.error;
                    return;
                }

            }
            else if (this.delete.Checked)
            {
                bool success = obj.DeleteAnimal(ViewState["id"], this.UserId, this.txtReason.Text.Trim());
                if (success) Response.Redirect("landing.aspx");
                else
                {
                    this.lblError.Text = Resources.Resource.error;
                    return;
                }
            }
        }
    }
}