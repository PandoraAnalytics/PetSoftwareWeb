using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class brochurepageadd : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["bid"] = this.DecryptQueryString("bid"); // brochureid
                ViewState["pageid"] = this.DecryptQueryString("bpid"); // brochurepageid
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            if (this.ConvertToInteger(ViewState["bid"]) > 0)
            {
                NameValueCollection collection = EventBA.GetBrochureDetail(ViewState["bid"]);
                if (collection == null) Response.Redirect("brochurepagelist.aspx?bid=" + BASecurity.Encrypt(this.ConvertToString(ViewState["bid"]), PageBase.HashKey));

                this.lblHeaderNM.Text = " (" + collection["name"] + ")";
                ViewState["eventid"] = collection["eventid"];
            }
            else
                Response.Redirect("eventslist.aspx");


            if (this.ConvertToInteger(ViewState["pageid"]) > 0)
            {
                NameValueCollection collection = EventBA.GetBrochurePageDetail(ViewState["pageid"]);
                if (collection == null) Response.Redirect("brochurepagelist.aspx?bid=" + BASecurity.Encrypt(this.ConvertToString(ViewState["bid"]), PageBase.HashKey));

                int type = this.ConvertToInteger(collection["content_type"]);
                this.txtName.Text = collection["pagename"];
                this.ddlContentType.SelectedValue = type.ToString();
                SetControlPanel(type);
                switch (type)
                {
                    case 1:
                        this.txtEditor.Value = HttpUtility.HtmlDecode(collection["content_data"]);
                        break;

                    case 2:
                        break;

                    case 3:
                        break;

                    case 4:
                        this.ddlSponsorType.SelectedValue = collection["content_subtype"];

                        this.ddlSponsors.Items.Clear();

                        this.ddlSponsors.DataSource = EventBA.GetsponsorsByType(collection["content_subtype"], ViewState["eventid"]);
                        this.ddlSponsors.DataBind();

                        string sponsorlist = "," + collection["content_data"] + ",";
                        foreach (ListItem item in this.ddlSponsors.Items)
                        {
                            item.Selected = (sponsorlist.Contains("," + item.Value + ","));
                        }

                        break;
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            NameValueCollection collection = new NameValueCollection();
            collection.Add("pagename", this.txtName.Text.Trim());
            collection.Add("brochureid", ViewState["bid"].ToString());
            collection.Add("content_type", ddlContentType.SelectedValue);

            int cType = this.ConvertToInteger(ddlContentType.SelectedValue);
            if (cType == (int)EventBA.ContentType.SPONSOR)
            {
                collection.Add("content_subtype", ddlSponsorType.SelectedValue);

                string sponsorlist = "";
                foreach (ListItem listItem in ddlSponsors.Items)
                {
                    if (listItem.Selected)
                    {
                        if (sponsorlist.Length > 0) sponsorlist += ",";
                        sponsorlist += listItem.Value;
                    }
                }

                collection.Add("content_data", sponsorlist);
            }

            if (cType == (int)EventBA.ContentType.TEXTEDITOR)
            {
                collection.Add("content_data", txtEditor.Value.Trim());
            }

            collection.Add("createdby", this.UserId);


            int brochurepageId = this.ConvertToInteger(ViewState["pageid"]);
            EventBA objBrochurePage = new EventBA();
            bool success = true;

            if (brochurepageId > 0)
            {
                // update 
                objBrochurePage.UpdateBrochurePage(collection, brochurepageId);
            }
            else
            {
                // Add
                objBrochurePage.AddBrochurePage(collection);
            }

            if (!success) this.lblError.Text = Resources.Resource.error;
            else Response.Redirect("brochurepagelist.aspx?bid=" + BASecurity.Encrypt(this.ConvertToString(ViewState["bid"]), PageBase.HashKey));
        }

        protected void ddlContentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // show panel as per selected type
            int type = this.ConvertToInteger(this.ddlContentType.SelectedValue);
            this.SetControlPanel(type);
        }

        private void SetControlPanel(int xiType)
        {
            switch (xiType)
            {
                case 1:
                    this.pnlEditor.Visible = true;
                    this.pnlAnimal.Visible = false;
                    this.pnlOwner.Visible = false;
                    this.pnlSponsor.Visible = false;
                    break;

                case 2:
                    this.pnlEditor.Visible = false;
                    this.pnlAnimal.Visible = true;
                    this.pnlOwner.Visible = false;
                    this.pnlSponsor.Visible = false;

                    break;

                case 3:
                    this.pnlEditor.Visible = false;
                    this.pnlAnimal.Visible = false;
                    this.pnlOwner.Visible = true;
                    this.pnlSponsor.Visible = false;

                    break;

                case 4:
                    this.pnlEditor.Visible = false;
                    this.pnlAnimal.Visible = false;
                    this.pnlOwner.Visible = false;
                    this.pnlSponsor.Visible = true;

                    break;
            }
        }

        protected void ddlSponsorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlSponsors.Items.Clear();

            this.ddlSponsors.DataSource = EventBA.GetsponsorsByType(this.ddlSponsorType.SelectedValue, ViewState["eventid"]);
            this.ddlSponsors.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("brochurepagelist.aspx?bid=" + BASecurity.Encrypt(this.ConvertToString(ViewState["bid"]), PageBase.HashKey));
        }
    }
}