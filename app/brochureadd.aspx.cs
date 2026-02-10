using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class brochureadd : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["eventid"] = this.DecryptQueryString("eid");
                ViewState["bid"] = this.DecryptQueryString("bid");
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


            if (this.ConvertToInteger(ViewState["bid"]) > 0)
            {
                NameValueCollection collection = EventBA.GetBrochureDetail(ViewState["bid"]);
                if (collection == null) Response.Redirect("eventslist.aspx");

                txtName.Text = collection["name"];
                txtDescription.Text = collection["description"];
                txtHeader.Text = collection["headertext"];
                txtFooter.Text = collection["footertext"];
                ViewState["eventid"] = collection["eventid"];
                ViewState["statusid"] = collection["status"];
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("description", this.txtDescription.Text.Trim());
            collection.Add("eventid", ViewState["eventid"].ToString());
            collection.Add("headertext", txtHeader.Text.Trim());
            collection.Add("footertext", txtFooter.Text.Trim());

            int brochureId = this.ConvertToInteger(ViewState["bid"]);
            EventBA objBrochure = new EventBA();
            bool success = true;

            if (brochureId > 0)
            {
                NameValueCollection coll = EventBA.GetBrochureDetail(ViewState["bid"]);
                collection.Add("status", coll["status"]);
                success = objBrochure.UpdateBrochure(collection, brochureId);
            }
            else
            {
                collection.Add("status", "1");
                brochureId = objBrochure.AddBrochure(collection);
            }


            if (brochureId > 0 && !string.IsNullOrEmpty(this.hid_brochure_pic.Value))
                success = objBrochure.UpdateBrochurePic(this.hid_brochure_pic.Value, brochureId);


            if (!success) this.lblError.Text = Resources.Resource.error;
            else Response.Redirect("brochurelist.aspx?eid=" + BASecurity.Encrypt(ViewState["eventid"].ToString(), Breederapp.PageBase.HashKey));
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("brochurelist.aspx?eid=" + BASecurity.Encrypt(ViewState["eventid"].ToString(), PageBase.HashKey));
        }
    }
}