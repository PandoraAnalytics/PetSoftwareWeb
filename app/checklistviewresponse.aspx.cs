using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class checklistviewresponse : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = this.DecryptQueryString("id");
                ViewState["animalid"] = this.ReadQueryString("aid");
                (Page.Master as breeder).AnimalId = ViewState["animalid"].ToString();
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = Checklist.GetChecklist(ViewState["id"]);
            if (collection == null) Response.Redirect("checklist.aspx");

            this.lblChecklistName.Text = collection["name"];
            collection = null;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("animalchecklist.aspx?id=" + ViewState["animalid"].ToString());
        }
    }
}