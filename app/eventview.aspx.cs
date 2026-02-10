using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class eventview : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
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

            this.lblTitle.Text = collection["title"];
            this.lblDescription.Text = this.nl2br(collection["description"]);
            this.lblVenue.Text = collection["venue"];
            this.litRules.Text = this.nl2br(collection["terms_condition"]);
            this.lblRegisteredCount.Text = collection["registrationcount"];
            ViewState["eventowneremail"] = collection["eventowneremail"];
            ViewState["eventownername"] = collection["eventownername"];

            try
            {
                DateTime tempDate1 = Convert.ToDateTime(collection["startdate"]);
                DateTime tempDate2 = Convert.ToDateTime(collection["endate"]);

                string eventdatetime = string.Empty;
                if (tempDate1 != DateTime.MinValue)
                {
                    eventdatetime = tempDate1.ToString(BusinessBase.DateTimeFormat);
                    if (tempDate2 != DateTime.MinValue)
                    {
                        if (DateTime.Compare(tempDate1.Date, tempDate2.Date) == 0)
                        {
                            eventdatetime += "  - " + tempDate2.ToString("HH:mm");
                        }
                        else
                        {
                            eventdatetime += "  - " + tempDate2.ToString(BusinessBase.DateTimeFormat);
                        }
                    }
                }
                this.lblDate.Text = eventdatetime;
                ViewState["eventdatetime"] = eventdatetime;
            }
            catch { }

            if (!string.IsNullOrEmpty(collection["banner_image"]))
            {
                string backgroundimage = "url('" + PageBase.getbase64url(collection["banner_image"]) + "')";
                this.default_banner.Style.Add("background-image", backgroundimage);
            }

            bool isregistered = EventBA.IsUserRegistered(ViewState["id"], this.UserId);
            //this.btnRegisterEvent2.Visible = isregistered;
            this.btnRegisterEvent.Visible = !isregistered;
            this.btnDeRegisterEvent.Visible = isregistered;

            this.repEventPhotos.DataSource = EventBA.GetFilesDetails(ViewState["id"]);
            this.repEventPhotos.DataBind();

            DataTable dt = EventBA.GetEventPublishBrochure(ViewState["id"]);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("new_id", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    row["new_id"] = BASecurity.Encrypt(row["id"].ToString(), PageBase.HashKey);  // BASecurity.Encrypt(xiPassword, BusinessBase.FixedSaltKey)
                }
            }
            this.rptrBrochure.DataSource = dt;
            this.rptrBrochure.DataBind();
        }

        protected void repeaterFiles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string deletefilename = this.ConvertToString(e.CommandArgument);
            if (string.IsNullOrEmpty(deletefilename)) return;

            EventBA.DeleteEventPhoto(deletefilename);
            this.PopulateControls();
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("eventregistration.aspx?id=" + BASecurity.Encrypt(ViewState["id"].ToString(), PageBase.HashKey));
        }

        protected void btnDeRegisterEvent_Click(object sender, EventArgs e)
        {
            EventBA objEvent = new EventBA();
            bool success = objEvent.DeRegisterUser(ViewState["id"], this.UserId);
            if (success)
            {
                //NameValueCollection collection = EventBA.GetEventDetail(ViewState["id"]);
                NameValueCollection emailcollection = new NameValueCollection();
                emailcollection.Add("userid", this.UserId);
                emailcollection.Add("eventid", this.ConvertToString(ViewState["id"]));
                emailcollection.Add("eventdatetime", this.ConvertToString(ViewState["eventdatetime"]));
                if (emailcollection != null) BreederMail.SendEmail(BreederMail.MessageType.USERUNREGISTEREVENT, emailcollection);

                NameValueCollection owneremailcollection = new NameValueCollection();
                owneremailcollection.Add("userid", this.UserId);
                owneremailcollection.Add("eventowneremail", this.ConvertToString(ViewState["eventowneremail"]));
                owneremailcollection.Add("eventownername", this.ConvertToString(ViewState["eventownername"]));
                owneremailcollection.Add("eventid", this.ConvertToString(ViewState["id"]));
                owneremailcollection.Add("eventdatetime", this.ConvertToString(ViewState["eventdatetime"]));
                if (owneremailcollection != null) BreederMail.SendEmail(BreederMail.MessageType.OWNERUNREGISTEREVENT, owneremailcollection);


                Response.Redirect("eventview.aspx?id=" + BASecurity.Encrypt(ViewState["id"].ToString(), PageBase.HashKey));
            }

        }

    }
}