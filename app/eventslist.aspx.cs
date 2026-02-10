using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class eventslist : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            this.IsAssociationAccess = true;
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.PopulateControls();
                this.ApplyFilters();
            }
        }

        private void PopulateControls()
        {
            this.ddlPastEvents.Items.Add(new ListItem("- " + Resources.Resource.PastEvents + "-", ""));
            this.ddlPastEvents.Items.Add(new ListItem(Resources.Resource.Yes, "1"));
        }

        private void ApplyFilters()
        {
            DateTime now = BusinessBase.Now.Date;

            NameValueCollection collection = new NameValueCollection();
            collection.Add("title", this.txtTitle.Text.Trim());

            if (ddlPastEvents.SelectedValue == "1") collection.Add("enddate", "");
            else collection.Add("enddate", BusinessBase.Now.ToString());

            collection.Add("userid", this.UserId);

            if (this.ConvertToString(Session["isowner"]) == "1")
                collection.Add("superadmin", "1");
            else
                collection.Add("superadmin", " ");

            this.hidfilter.Value = EventBA.EventListSearch(collection);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilters();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = String.Empty;
            string deleteReason = this.txtReason.Text.Trim();
            if (deleteReason.Length > 255)
            {
                this.lblError.Text = "You cannot enter more than 255 characters";
                return;
            }

            string eventId = this.cplist.Value;

            NameValueCollection eventcollection = EventBA.GetEventDetail(eventId);

            EventBA objDelete = new EventBA();
            NameValueCollection collection = new NameValueCollection();
            collection["delete_reason"] = deleteReason;
            string result = EventBA.DeleteEventReason(eventId, deleteReason);
            this.Hidden1.Value = result;

            //code temporarally commented

            //DataSet dataSet = EventBA.GetRegisteredUsers(1, eventId);
            //if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            //{
            //    NameValueCollection emailcollection = new NameValueCollection();
            //    emailcollection.Add("delete_reason", deleteReason);
            //    emailcollection.Add("title", eventcollection["title"].ToString());
            //    emailcollection.Add("venue", eventcollection["venue"].ToString());
            //    foreach (DataRow row in dataSet.Tables[0].Rows)
            //    {
            //        emailcollection.Add("fullname", row["fname"].ToString() + " " + row["lname"].ToString());
            //        emailcollection.Add("email", row["email"].ToString());

            //        if (emailcollection != null) BreederMail.SendEmail(BreederMail.MessageType.USERCANCELEVENTEMAIL, emailcollection);
            //    }
            //    //while (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0) ;
            //}

            //NameValueCollection owneremailcollection = new NameValueCollection();
            //owneremailcollection.Add("delete_reason", deleteReason);
            //owneremailcollection.Add("eventownername", eventcollection["eventownername"].ToString());
            //owneremailcollection.Add("title", eventcollection["title"].ToString());
            //owneremailcollection.Add("venue", eventcollection["venue"].ToString());
            //owneremailcollection.Add("eventowneremail", eventcollection["eventowneremail"].ToString());
            //if (owneremailcollection != null) BreederMail.SendEmail(BreederMail.MessageType.OWNERCANCELEVENTEMAIL, owneremailcollection);
            objDelete = null;
            this.cplist.Value = null;
        }
    }
}