using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI;


namespace Breederapp
{
    public partial class pedigreereport : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = this.ReadQueryString("id");
                (Page.Master as breeder).AnimalId = ViewState["id"].ToString();
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = AnimalBA.GetAnimalDetail(ViewState["id"]);
            if (collection == null) Response.Redirect("landing.aspx");

            this.lblName.Text = collection["name"];
        }
    }
}