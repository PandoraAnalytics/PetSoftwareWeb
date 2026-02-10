using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class bueditappointment : ERPBase
    {

        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString("id");//date id
                ViewState["userid"] = DecryptQueryString("uid");//user id
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = AnimalBA.GetAnimalAppointmentDetaillByDate(ViewState["id"], this.ConvertToString(ViewState["userid"]));
            if (collection == null) Response.Redirect("budashboard.aspx");

            ViewState["animalid"] = collection["animalid"];
            (Page.Master as bubreeder).AnimalId = ViewState["animalid"].ToString();

            ViewState["appointmentid"] = collection["id"];

            NameValueCollection collection2 = BUCustomer.GetCustomerByAnimalId(ViewState["animalid"], this.CompanyId);
            if (collection2 != null) ViewState["userid"] = collection2["userid"];
            else Response.Redirect("budashboard.aspx");

            DateTime tempTime = new DateTime(2020, 1, 1, 0, 0, 0);
            for (int i = 0; i < 96; i++)
            {
                DateTime newTime = tempTime.AddMinutes(i * 15);
                this.ddlStartTime.Items.Add(newTime.ToString("HH:mm"));
            }

            try
            {
                DateTime startDate = Convert.ToDateTime(collection["startdatetime"]);
                this.lblDate.Text = startDate.ToString(this.DateFormat);
                ViewState["date"] = this.lblDate.Text;

                this.ddlStartTime.SelectedValue = startDate.ToString("HH:mm");
            }
            catch { }

            this.ddlProfession.SelectedValue = collection["professionid"];


            //DataTable professionTable = BreederData.GetContactProfessions();
            DataTable professionTable = BreederData.GetBUContactProfessions(this.ConvertToInteger(this.CompanyId));

            this.ddlModalProfession.DataSource = professionTable;
            this.ddlModalProfession.DataBind();

            if (professionTable != null)
            {
                DataRow row = professionTable.NewRow();
                row["id"] = 0;
                row["name"] = "(" + Resources.Resource.None + ")";
                professionTable.Rows.InsertAt(row, 0);
            }
            this.ddlProfession.DataSource = professionTable;
            this.ddlProfession.DataBind();
            this.ddlProfession_SelectedIndexChanged(null, null);

            this.ddlContact.SelectedValue = collection["contactid"];

            if (!string.IsNullOrEmpty(collection["remind_before_number"]))
            {
                this.ddlReminder.SelectedValue = "1";
                this.ddlReminder_SelectedIndexChanged(null, null);

                this.ddlReminderText.SelectedValue = collection["remind_before_text"];
                this.txtReminderNumber.Text = collection["remind_before_number"];
            }
        }

        private void Update(bool xiThisOccuranceAndFollowing)
        {
            this.lblError.Text = "";

            string appointdate = this.ConvertToString(ViewState["date"]);

            NameValueCollection collection = new NameValueCollection();
            collection.Add("datetime", appointdate + " " + this.ddlStartTime.SelectedValue);
            collection.Add("appointmentid", ViewState["appointmentid"].ToString());
            collection.Add("contactid", this.ddlContact.SelectedValue);
            if (this.panelReminder.Visible)
            {
                collection.Add("remind_before_number", this.txtReminderNumber.Text.Trim());
                collection.Add("remind_before_text", this.ddlReminderText.SelectedValue);
            }
            else
            {
                collection.Add("remind_before_number", string.Empty);
                collection.Add("remind_before_text", string.Empty);
            }

            AnimalBA objapoint = new AnimalBA();
            bool sucess = objapoint.UpdateAnimalAppointment(collection, ViewState["id"], xiThisOccuranceAndFollowing);
            objapoint = null;
            if (sucess)
            {
                Response.Redirect("buappointmentlist.aspx?id=" + BASecurity.Encrypt(ViewState["animalid"].ToString(), PageBase.HashKey));
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void ddlProfession_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlContact.Items.Clear();

            int professsion = this.ConvertToInteger(this.ddlProfession.SelectedValue);
            if (professsion == 0)
            {
                this.ddlContact.Items.Add(new ListItem("(" + Resources.Resource.None + ")", "0"));
            }
            else if (professsion > 0)
            {
                DataTable dtcontact = AnimalBA.GetContactsByProfession(this.ddlProfession.SelectedValue, this.UserId);
                if (dtcontact != null)
                {
                    DataRow row = dtcontact.NewRow();
                    row["id"] = int.MinValue;
                    row["full_name"] = Resources.Resource.Select;
                    dtcontact.Rows.InsertAt(row, 0);
                }
                this.ddlContact.DataSource = dtcontact;
                this.ddlContact.DataBind();

                this.ddlModalProfession.SelectedValue = professsion.ToString();
            }
        }

        protected void ddlReminder_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.panelReminder.Visible = (this.ddlReminder.SelectedValue == "1");
        }

        protected void btnAddContact_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;
            if (this.ddlModalProfession.SelectedValue.Length == 0 || this.txtModalFirstName.Text.Trim().Length == 0 || this.txtModalLastName.Text.Trim().Length == 0)
            {
                return;
            }

            NameValueCollection collection = new NameValueCollection();
            collection.Add("firstname", this.txtModalFirstName.Text.Trim());
            collection.Add("lastname", this.txtModalLastName.Text.Trim());
            collection.Add("email", "");
            collection.Add("contact", "");
            collection.Add("profession", this.ddlModalProfession.SelectedValue);
            collection.Add("about", "");
            collection.Add("userid", this.UserId);
            collection.Add("companyid", this.CompanyId); // BUID

            AddressBA objBreed = new AddressBA();
            int contactid = objBreed.AddContact(collection);
            objBreed = null;

            if (contactid > 0)
            {
                this.txtModalFirstName.Text = string.Empty;
                this.txtModalLastName.Text = string.Empty;
                this.ddlProfession_SelectedIndexChanged(null, null);

                this.ddlContact.SelectedValue = contactid.ToString();
            }
        }

        protected void btnEditAppointment_Click(object sender, EventArgs e)
        {
            this.Update(false);
        }

        protected void btnEditAppointmentFollowing_Click(object sender, EventArgs e)
        {
            this.Update(true);
        }
    }
}