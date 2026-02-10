using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class sponsorlist : PageBase
    {
        public string EventId
        {
            get { return BABusiness.BASecurity.Encrypt(ViewState["eventid"].ToString(), Breederapp.PageBase.HashKey); }
        }

        override protected void Page_Load(object sender, EventArgs e)
        {
            this.IsAssociationAccess = true;
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["eventid"] = this.DecryptQueryString("eid");
                this.PopulateControls();
                this.ApplyFilter();
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
        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("eventid", this.ConvertToString(ViewState["eventid"]));
            collection.Add("name", this.txtSponsorName.Text.Trim());
            this.hidfilter.Value = EventBA.SponsorSearch(collection);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }

        protected void lnk_Click(object sender, EventArgs e)
        {
            Response.Redirect("sponsoradd.aspx?eid=" + this.EventId);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("eventslist.aspx");
        }
    }
}