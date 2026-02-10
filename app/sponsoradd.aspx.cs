using BABusiness;
using System;
using System.Collections.Specialized;


namespace Breederapp
{
    public partial class sponsoradd : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["eventid"] = this.DecryptQueryString("eid");
                ViewState["sid"] = this.DecryptQueryString("sid");
                this.PopulateControls();
            }
        }
        private void PopulateControls()
        {
            if (this.ConvertToInteger(ViewState["eventid"]) > 0)
            {
                NameValueCollection collection = EventBA.GetEventDetail(ViewState["eventid"]);
                if (collection == null) Response.Redirect("eventslist.aspx");
                this.lblEventNM.Text = " (" + collection["title"] + ")";
            }

            if (this.ConvertToInteger(ViewState["sid"]) > 0)
            {
                NameValueCollection collection = EventBA.GetSponsorDetail(ViewState["sid"]);
                if (collection == null) Response.Redirect("sponsorlist.aspx");
                txtName.Text = collection["name"];
                txtDescription.Text = collection["description"];
                ViewState["eventid"] = collection["eventid"];
                ddlType.SelectedValue = collection["type"];
                if (!string.IsNullOrEmpty(collection["sponsor_file"]))
                {
                    this.hid_sponsor_pic.Value = collection["sponsor_file"];
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("description", this.txtDescription.Text.Trim());
            collection.Add("eventid", ViewState["eventid"].ToString());
            collection.Add("type", this.ddlType.SelectedValue);
            collection.Add("sponsor_file", this.hid_sponsor_pic.Value);
            int sponsorId = this.ConvertToInteger(ViewState["sid"]);

            EventBA objSponsor = new EventBA();
            if (sponsorId > 0)
            {
                bool success = objSponsor.UpdateSponsor(collection, sponsorId);
                if (!success) this.lblError.Text = Resources.Resource.error;
                else Response.Redirect("sponsorlist.aspx?eid=" + BASecurity.Encrypt(ViewState["eventid"].ToString(), PageBase.HashKey));

            }
            else
            {
                int sponsorId1 = objSponsor.AddSponsor(collection);
                if (sponsorId1 > 0)
                {
                    Response.Redirect("sponsorlist.aspx?eid=" + BASecurity.Encrypt(ViewState["eventid"].ToString(), PageBase.HashKey));
                }
                else
                {
                    this.lblError.Text = Resources.Resource.error;
                }

            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("sponsorlist.aspx?eid=" + BASecurity.Encrypt(ViewState["eventid"].ToString(), PageBase.HashKey));
        }

    }
}