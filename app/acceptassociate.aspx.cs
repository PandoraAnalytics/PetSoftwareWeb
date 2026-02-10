using System;

namespace Breederapp
{
    public partial class acceptassociate : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                string id = this.ReadQueryString("assoid");
                // string id = DecryptQueryString("assoid"); ;
                if (string.IsNullOrEmpty(id)) Response.Redirect("manageassociation.aspx");

                ViewState["id"] = id;
                this.hdfilter.Value = id;

                if (ViewState["refurl"] == null && Request.UrlReferrer != null)
                {
                    ViewState["refurl"] = Request.UrlReferrer.ToString();
                }
            }
        }

        private void BackToPage()
        {
            string refUrl = this.ConvertToString(ViewState["refurl"]);
            if (!string.IsNullOrEmpty(refUrl))
            {
                Response.Redirect(refUrl);
            }
            else
            {
                Response.Redirect("landing.aspx");
            }
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            this.BackToPage();
        }
    }
}