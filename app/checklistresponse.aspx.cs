using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class checklistresponse : PageBase
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
            NameValueCollection collection = Checklist.GetCheckListResponse(ViewState["id"]);
            if (collection == null) Response.Redirect("assignedchecklist.aspx");
            if (collection["isdraft"] == "1") Response.Redirect("assignedchecklist.aspx");

            this.lblChecklist.Text = collection["checklistname"];
            this.lblResponseBy.Text = collection["username"] + " - " + Convert.ToDateTime(collection["updateddate"]).ToString(this.DateTimeFormat);

            NameValueCollection acollection = AnimalBA.GetAnimalDetail(collection["animalid"]);
            if (acollection == null) Response.Redirect("assignedchecklist.aspx");

            this.lblResponseBy.Text += "&nbsp;&nbsp;&nbsp;<i class='fa-solid fa-circle-dot'></i>&nbsp;&nbsp;&nbsp;" + acollection["name"] + " - " + acollection["typename"];
            acollection = null;

            collection = null;
        }
    }
}