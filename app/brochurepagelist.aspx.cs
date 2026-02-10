using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class brochurepagelist : PageBase
    {
        public string BrochureId
        {
            get { return BABusiness.BASecurity.Encrypt(ViewState["brochureid"].ToString(), Breederapp.PageBase.HashKey); }
        }

        override protected void Page_Load(object sender, EventArgs e)
        {
            this.IsAssociationAccess = true;
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["brochureid"] = this.DecryptQueryString("bid");
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            if (this.ConvertToInteger(ViewState["brochureid"]) > 0)
            {
                NameValueCollection collection = EventBA.GetBrochureDetail(ViewState["brochureid"]);
                if (collection == null) Response.Redirect("eventslist.aspx");

                this.lblBrochureNM.Text = " (" + collection["name"] + ")";
                ViewState["eventid"] = collection["eventid"];
            }
            else
                Response.Redirect("eventslist.aspx");
        }

        protected void lnk_Click(object sender, EventArgs e)
        {
            Response.Redirect("brochurepageadd.aspx?bid=" + this.BrochureId);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("brochurelist.aspx?eid=" + BASecurity.Encrypt(this.ConvertToString(ViewState["eventid"]), PageBase.HashKey));
        }
    }
}