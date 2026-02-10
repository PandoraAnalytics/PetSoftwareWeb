using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class memberrequest : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!this.IsPostBack)
            {
                ViewState["id"] = this.DecryptQueryString("aid");
                this.ApplyFilter();
                this.PopulateControls();
            }
            else
            {
                bool success = false;
                switch (this.Request["__EVENTTARGET"])
                {
                    case "approve":
                        success = Member.UpdateApproveMembership(this.Request["__EVENTARGUMENT"], this.ConvertToString(ViewState["id"]));
                        break;

                    case "reject":
                        success = Member.UpdateRejectMembership(this.Request["__EVENTARGUMENT"], this.ConvertToString(ViewState["id"]));
                        break;
                }

                if (success)
                    Response.Redirect("manageassociation.aspx");
            }
        }

        private void PopulateControls()
        {
            string aId = this.ConvertToString(ViewState["id"]);
            if (!string.IsNullOrEmpty(aId))
            {
                NameValueCollection collection = UserBA.GetAssociation(aId);
                if (collection != null) this.lblAssociationTitle.Text = "(" + collection["name"] + ")";
            }
            else Response.Redirect("landing.aspx");
        }

        private void ApplyFilter()
        {
            this.hdfilter.Value = this.ConvertToString(ViewState["id"]);
        }


    }
}