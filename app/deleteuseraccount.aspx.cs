using System;
using BABusiness;

namespace Breederapp
{
    public partial class deleteuseraccount : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            if (!this.chkconfirmation.Checked)
            {
                this.lblError.Text = "Please check the confirmation box to continue";
                return;
            }

            if (string.Compare(this.txtDelete.Text.Trim(), "DELETE", false) != 0) // #TODO .ToString().ToUpper()
            {
                this.lblError.Text = "Please type DELETE in the box to make sure that you haven't clicked the Delete button accidently";
                return;
            }

            bool success = UserBA.DeleteUser(this.UserId);
            if (!success)
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }

            Session["userid"] = null;
            Session["username"] = null;
            Session["usertype"] = null;
            Session["dtformat"] = null;
            Session.Abandon();
            Response.Redirect("signin.aspx");
        }
    }
}