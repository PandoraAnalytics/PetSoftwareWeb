using System;

namespace Breederapp
{
    public partial class exportbrochure : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString("bid"); // brochureid
                PopulateControls();
            }
        }

        private void PopulateControls()
        {
            try
            {
                string fileName = this.ProcessEventBrochurePdf(ViewState["id"]);
                if (string.IsNullOrEmpty(fileName)) return;

                string url = BABusiness.BreederMail.PageURL + "/app/docs/temp/" + fileName + ".pdf";
                Response.Redirect(url);
            }
            catch { }
        }
    }
}