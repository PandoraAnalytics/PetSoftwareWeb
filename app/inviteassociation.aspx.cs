using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class inviteassociation : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = this.DecryptQueryString("id");
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            this.lblError.Text = string.Empty;

            NameValueCollection collection = UserBA.GetAssociation(ViewState["id"]);
            if (collection == null) Response.Redirect("manageassociation.aspx");

            string invitationcode = collection["inviation_code"];
            if (string.IsNullOrEmpty(collection["inviation_code"]))
            {
                invitationcode = Guid.NewGuid().ToString();
                bool success = UserBA.UpdateInvitationCode(invitationcode, ViewState["id"]);

                if (!success)
                {
                    this.lblError.Text = Resources.Resource.error;
                    return;
                }
            }

            this.lblHeading.Text = collection["name"];

            this.hid.Value = string.Format("{0}joinassociation.aspx?invitationcode={1}", BreederMail.PageURL, invitationcode);

            this.lblLink.Text = string.Format("<a href='{0}' target='join'>{0}</a>", this.hid.Value);

        }

        protected void lnkReset_Click(object sender, EventArgs e)
        {
            UserBA.UpdateInvitationCode(string.Empty, ViewState["id"]);
            Response.Redirect("manageassociation.aspx");
        }

    }
}