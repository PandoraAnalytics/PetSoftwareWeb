using BABusiness;
using System;
using System.Collections.Specialized;


namespace Breederapp
{
    public partial class eventregistrationlist : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            this.IsAssociationAccess = true;
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString("id");
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = EventBA.GetEventDetail(ViewState["id"]);
            if (collection == null) Response.Redirect("eventslist.aspx");

            this.lblEventTitle.Text = collection["title"];

            this.hidfilter.Value = ViewState["id"].ToString();
        }
    }
}