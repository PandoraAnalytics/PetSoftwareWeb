using BABusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class bucreateappointment : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["animalid"] = DecryptQueryString("id");
                (Page.Master as bubreeder).AnimalId = ViewState["animalid"].ToString();
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            int animalid = this.ConvertToInteger(ViewState["animalid"]);
            if (animalid <= 0) Response.Redirect("budashboard.aspx");

            NameValueCollection collection = AnimalBA.GetAnimalDetail(animalid);
            if (collection == null) Response.Redirect("budashboard.aspx");

            NameValueCollection collection2 = BUCustomer.GetCustomerByAnimalId(ViewState["animalid"], this.CompanyId);
            if (collection2 != null) ViewState["userid"] = collection2["userid"];
            else Response.Redirect("budashboard.aspx");

            (Page.Master as bubreeder).AnimalId = animalid.ToString();

            DateTime tempTime = new DateTime(2020, 1, 1, 0, 0, 0);
            for (int i = 0; i < 96; i++)
            {
                DateTime newTime = tempTime.AddMinutes(i * 15);
                this.ddlStartTime.Items.Add(newTime.ToString("HH:mm"));
            }
            string date = Request.QueryString["date"];
            this.txtDate.Text = (!string.IsNullOrEmpty(date)) ? date : BusinessBase.Now.ToString(this.DateFormat);

            string start = Request.QueryString["start"];
            if (!string.IsNullOrEmpty(start))
            {
                DateTime st = Convert.ToDateTime(start, CultureInfo.CurrentCulture);
                if (st != DateTime.MinValue)
                {
                    this.ddlStartTime.SelectedValue = st.ToString("HH:mm");
                }
            }

            this.ddlMonthDays.Items.Add(new ListItem(Resources.Resource.selectDay, ""));
            for (int i = 1; i <= 31; i++)
            {
                this.ddlMonthDays.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

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
        }

        private ArrayList GenerateDatesBetweenSelectedDates(DateTime xiStartDate, DateTime xiEndDate)
        {
            ArrayList datesArray = new ArrayList();
            int days = 1;
            if (this.panelEndsOn.Visible)
            {
                TimeSpan diff = xiEndDate - xiStartDate;
                days = diff.Days + 1;
            }

            string monthday = xiStartDate.ToString("dd", CultureInfo.CurrentCulture);

            switch (this.ConvertToInteger(ddlRepeat.SelectedValue))
            {
                case 0:
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    dictionary["st"] = xiStartDate.ToString();
                    dictionary["et"] = xiStartDate.ToString();
                    datesArray.Add(dictionary);
                    break;

                case 1:
                    for (int i = 0; i <= days; i++)
                    {
                        Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
                        dictionary1["st"] = xiStartDate.AddDays(i).ToString();
                        dictionary1["et"] = xiEndDate.AddDays(i).ToString();
                        datesArray.Add(dictionary1);
                    }
                    break;

                case 2:
                    ArrayList weekArray = new ArrayList();
                    foreach (ListItem item in this.ddlWeekly.Items)
                    {
                        if (item.Selected == false) continue;
                        weekArray.Add(item.Value);
                    }

                    for (int i = 0; i <= days; i++)
                    {
                        DateTime tempDate = xiStartDate.AddDays(i);
                        string w = tempDate.ToString("dddd", BusinessBase.USCulture);
                        if (weekArray.Contains(w))
                        {
                            Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
                            dictionary2["st"] = xiStartDate.AddDays(i).ToString();
                            dictionary2["et"] = xiEndDate.AddDays(i).ToString();
                            datesArray.Add(dictionary2);
                        }
                    }
                    break;

                case 3:
                    int monthoftheday = this.ConvertToInteger(this.ddlMonthDays.SelectedValue);
                    for (int i = 0; i <= days; i++)
                    {
                        DateTime tempDate = xiStartDate.AddDays(i);
                        if (monthoftheday == tempDate.Day)
                        {
                            Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
                            dictionary3["st"] = xiStartDate.AddDays(i).ToString();
                            dictionary3["et"] = xiEndDate.AddDays(i).ToString();
                            datesArray.Add(dictionary3);
                        }
                    }
                    break;

                case 5:
                    int day1 = this.ConvertToInteger(ddlAnnualyOn_Date.SelectedValue);
                    int month1 = this.ConvertToInteger(ddlAnnualyOn_Month.SelectedValue);
                    for (int i = 0; i <= days; i++)
                    {
                        DateTime tempDate = xiStartDate.AddDays(i);
                        if (tempDate.Day == day1 && tempDate.Month == month1)
                        {
                            Dictionary<string, string> dictionary6 = new Dictionary<string, string>();
                            dictionary6["st"] = xiStartDate.AddDays(i).ToString();
                            dictionary6["et"] = xiEndDate.AddDays(i).ToString();
                            datesArray.Add(dictionary6);
                        }
                    }
                    break;

                case 4:
                    int monthsinbetween = ((xiEndDate.Year - xiStartDate.Year) * 12) + xiEndDate.Month - xiStartDate.Month;

                    int dayofweek = this.ConvertToInteger(this.ddlDayOfWeek_Day.SelectedValue);
                    int weekno = this.ConvertToInteger(this.ddlDayOfWeek_Week.SelectedValue);

                    DayOfWeek compareWeek = (DayOfWeek)dayofweek;
                    DateTime tempStartDate = xiStartDate;
                    NameValueCollection monthcollection = new NameValueCollection();
                    for (int mo = 0; mo <= monthsinbetween; mo++)
                    {
                        DateTime _date = new DateTime(tempStartDate.Year, tempStartDate.Month, 1);

                        DayOfWeek day = _date.DayOfWeek;

                        int d = 0;

                        var diff = compareWeek - day;
                        int tempdiff = (diff >= 0) ? weekno - 1 : weekno;

                        DateTime secFriday = _date.AddDays(diff + (tempdiff * 7) + d);
                        if (secFriday.Month != _date.Month) monthcollection[tempStartDate.Year + "_" + tempStartDate.Month] = "";
                        else monthcollection[tempStartDate.Year + "_" + tempStartDate.Month] = secFriday.ToShortDateString();

                        tempStartDate = tempStartDate.AddMonths(1);
                    }

                    for (int i = 0; i <= days; i++)
                    {
                        DateTime tempDate = xiStartDate.AddDays(i);
                        string comparetext = monthcollection[tempDate.Year + "_" + tempDate.Month];
                        DateTime compareDateTime = DateTime.MinValue;
                        DateTime.TryParse(comparetext, out compareDateTime);
                        if (compareDateTime != DateTime.MinValue && compareDateTime.Day == tempDate.Day && compareDateTime.Month == tempDate.Month && compareDateTime.Year == compareDateTime.Year)
                        {
                            Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
                            dictionary5["st"] = xiStartDate.AddDays(i).ToString();
                            dictionary5["et"] = xiEndDate.AddDays(i).ToString();
                            datesArray.Add(dictionary5);
                        }
                    }
                    break;

            }
            return datesArray;
        }

        private bool ValidateControls(DateTime xiStartDate, DateTime xiEndDate)
        {
            bool success = true;
            if (xiStartDate == DateTime.MinValue)
            {
                this.lblError.Text = Resources.Resource.Invalidate;
                success = false;
                return success;
            }

            if (this.panelEndsOn.Visible)
            {
                if (DateTime.Compare(xiStartDate, xiEndDate) >= 0)
                {
                    lblError.Text = Resources.Resource.Enddateshouldbegreaterthanstartdate;
                    success = false;
                    return success;
                }
            }

            return success;
        }

        protected void ddlRepeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.panelEndsOn.Visible = false;
            this.panelDaily.Visible = false;
            this.panelWeekly.Visible = false;
            this.panelMonthly.Visible = false;
            this.panelDayOfWeeks.Visible = false;
            this.panelAnnually.Visible = false;
            this.panelRepeat.Visible = false;

            this.panelRepeat.Visible = (this.ConvertToInteger(ddlRepeat.SelectedValue) > 0);
            switch (this.ConvertToInteger(this.ddlRepeat.SelectedValue))
            {
                case 1:
                    this.panelDaily.Visible = true;
                    this.panelEndsOn.Visible = true;
                    break;

                case 2:
                    this.panelWeekly.Visible = true;
                    this.panelEndsOn.Visible = true;
                    break;

                case 3:
                    this.panelMonthly.Visible = true;
                    this.panelEndsOn.Visible = true;
                    break;

                case 4:
                    this.panelDayOfWeeks.Visible = true;
                    this.panelEndsOn.Visible = true;
                    break;

                case 5:
                    this.panelAnnually.Visible = true;
                    this.panelEndsOn.Visible = true;
                    break;
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

        protected void btnCreateAppointment_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            DateTime startDate = Convert.ToDateTime(this.txtDate.Text.Trim() + " " + this.ddlStartTime.SelectedValue, CultureInfo.CurrentCulture);
            DateTime endDate = (this.panelEndsOn.Visible) ? Convert.ToDateTime(this.txtEndDate.Text.Trim(), CultureInfo.CurrentCulture) : DateTime.MinValue;

            bool success = this.ValidateControls(startDate, endDate);
            if (success == false) return;

            NameValueCollection collection = new NameValueCollection();
            collection.Add("animalid", ViewState["animalid"].ToString());
            collection.Add("datetime", this.txtDate.Text.Trim() + " " + this.ddlStartTime.SelectedValue);
            collection.Add("contact", this.ddlContact.SelectedValue);
            if (this.panelReminder.Visible)
            {
                collection.Add("remindnumber", this.txtReminderNumber.Text.Trim());
                collection.Add("remindtext", this.ddlReminderText.SelectedValue);
            }
            else
            {
                collection.Add("remindnumber", string.Empty);
                collection.Add("remindtext", string.Empty);
            }
            collection.Add("status", "1");

            collection.Add("userid", this.UserId);

            collection.Add("appdates", startDate.ToString() + " - " + endDate.ToString());
            collection.Add("companyid", this.CompanyId); // BUID

            AnimalBA objapoint = new AnimalBA();
            int appointmentId = objapoint.AddAnimalAppointment(collection);
            if (appointmentId > 0)
            {
                if (this.filenames.Value.Length > 0)
                {
                    string[] files = this.filenames.Value.Split(',');
                    foreach (string file in files)
                    {
                        if (string.IsNullOrEmpty(file)) continue;
                        objapoint.AddAnimalAppointmentFiles(file, appointmentId);
                    }
                }

                ArrayList datesArray = this.GenerateDatesBetweenSelectedDates(startDate, endDate);
                if (datesArray != null)
                {
                    foreach (Dictionary<string, string> dict in datesArray)
                    {
                        NameValueCollection collection2 = new NameValueCollection();
                        collection2.Add("appointmentid", appointmentId.ToString());
                        collection2.Add("startdate", dict["st"]);
                        collection2.Add("enddate", dict["et"]);
                        objapoint.AddAnimalAppointmentDates(collection2);
                    }
                }
                objapoint = null;

                Response.Redirect("buappointmentlist.aspx?id=" + BASecurity.Encrypt(ViewState["animalid"].ToString(), PageBase.HashKey));
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
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
    }
}