using BABusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class checklistmanage : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            Control divMenu = Master.FindControl("checklistmainmenu");
            Control divUserMenu = Master.FindControl("usermenu");
            divUserMenu.Visible = false;
            divMenu.Visible = true;
            if (!this.IsPostBack)
            {
                ViewState["id"] = this.DecryptQueryString("id");
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            DataTable table = Checklist.GetChecklistCategory();
            if (table != null && table.Rows.Count > 0)
            {
                string option = "<option value=\"{0}\"></option>";
                StringBuilder html = new StringBuilder();
                foreach (DataRow row in table.Rows)
                {
                    html.AppendLine(string.Format(option, row["name"].ToString()));
                }
                this.datalist.InnerHtml = html.ToString();
            }

            this.ddlMonthDays.Items.Add(new ListItem(Resources.Resource.selectDay, ""));
            for (int i = 1; i <= 31; i++)
            {
                this.ddlMonthDays.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            int checklistid = this.ConvertToInteger(ViewState["id"]);
            if (checklistid > 0)
            {


                this.panelMembers.Visible = true;
                this.panelManageSchedule.Visible = true;
                this.btnSave.Text = Resources.Resource.Save;

                NameValueCollection collection = Checklist.GetChecklist(ViewState["id"]);
                if (collection == null) Response.Redirect("checklist.aspx");

                if (this.UserId != collection["createdby"]) Response.Redirect("checklist.aspx");

                this.txtName.Text = collection["name"];
                this.txtCategoryName.Value = collection["categoryname"];
                this.ddlResponseType.SelectedValue = collection["responsetype"];

                this.PopulateSchedule();

                DataTable dataTable = Checklist.GetChecklistUsers(ViewState["id"]);
                if (dataTable != null)
                {
                    DataRow row = dataTable.NewRow();
                    row["id"] = 0;
                    row["name"] = Resources.Resource.AddNewMember;
                    dataTable.Rows.InsertAt(row, 0);
                }
                this.repeaterMembers.DataSource = Checklist.GetChecklistUsers(ViewState["id"]);
                this.repeaterMembers.DataBind();
            }
        }

        private void PopulateSchedule()
        {
            NameValueCollection scheduleCollection = Checklist.GetScheduleFromChecklist(ViewState["id"]);
            if (scheduleCollection != null)
            {
                ViewState["scheduleid"] = scheduleCollection["id"];
                this.ddlRepeat.SelectedValue = scheduleCollection["repeattype"];
                this.ddlRepeat_SelectedIndexChanged(null, null);

                try
                {
                    if (!string.IsNullOrEmpty(scheduleCollection["endson"]))
                        this.txtEndDate.Text = Convert.ToDateTime(scheduleCollection["endson"]).ToString(this.DateFormat);
                }
                catch { }

                switch (this.ConvertToInteger(scheduleCollection["repeattype"]))
                {
                    case 0:
                        this.lblScheduleText.Text = Resources.Resource.NoRepeat;
                        break;

                    case 1:
                        this.lblScheduleText.Text = Resources.Resource.Repeat + " " + Resources.Resource.Daily + " " + Resources.Resource.till + " " + this.txtEndDate.Text.Trim();
                        break;

                    case 2:
                        string[] days = scheduleCollection["repeatval"].Split(',');
                        string temp = "";
                        foreach (ListItem item in this.ddlWeekly.Items)
                        {
                            item.Selected = days.Contains(item.Value);

                            if (item.Selected == false) continue;
                            temp += item.Value + ", ";
                        }
                        temp = temp.Trim().TrimEnd(',');

                        this.lblScheduleText.Text = Resources.Resource.Repeat + " " + Resources.Resource.Weekly + " - " + temp + " " + Resources.Resource.till + " " + this.txtEndDate.Text.Trim();
                        break;

                    case 3:
                        this.ddlMonthDays.SelectedValue = scheduleCollection["repeatval"];

                        this.lblScheduleText.Text = Resources.Resource.Repeat + " " + this.GetOrdinal(this.ConvertToInteger(this.ddlMonthDays.SelectedItem.Text)) + " " + Resources.Resource.dayofeachmonth + " " + Resources.Resource.till + " " + this.txtEndDate.Text.Trim();
                        break;

                    case 4:
                        string[] weeks = scheduleCollection["repeatval"].Split(',');
                        this.ddlDayOfWeek_Day.SelectedValue = weeks[0];
                        this.ddlDayOfWeek_Week.SelectedValue = weeks[0];

                        this.lblScheduleText.Text = Resources.Resource.Repeat + " " + this.ddlDayOfWeek_Day.SelectedItem.Text + " " + this.ddlDayOfWeek_Week.SelectedItem.Text + " " + Resources.Resource.till + " " + this.txtEndDate.Text.Trim();
                        break;

                    case 5:
                        string[] years = scheduleCollection["repeatval"].Split(',');
                        this.ddlAnnualyOn_Date.SelectedValue = years[0];
                        this.ddlAnnualyOn_Month.SelectedValue = years[0];

                        this.lblScheduleText.Text = Resources.Resource.Repeat + " " + Resources.Resource.Annually + " " + this.GetOrdinal(this.ConvertToInteger(this.ddlAnnualyOn_Date.SelectedItem.Text)) + " " + Resources.Resource.Dayof + " " + this.ddlAnnualyOn_Month.SelectedItem.Text + " " + Resources.Resource.till + " " + this.txtEndDate.Text.Trim();
                        break;
                }
            }
        }

        private string GetOrdinal(int number)
        {
            string suffix = String.Empty;

            int ones = number % 10;
            int tens = (int)Math.Floor(number / 10M) % 10;

            if (tens == 1)
            {
                suffix = "th";
            }
            else
            {
                switch (ones)
                {
                    case 1:
                        suffix = "st";
                        break;

                    case 2:
                        suffix = "nd";
                        break;

                    case 3:
                        suffix = "rd";
                        break;

                    default:
                        suffix = "th";
                        break;
                }
            }
            return String.Format("{0}{1}", number, suffix);
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("checklist.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("category", this.txtCategoryName.Value.Trim());
            collection.Add("responsetype", this.ddlResponseType.SelectedValue);
            collection.Add("userid", this.UserId);

            Checklist objChecklist = new Checklist();
            if (this.ConvertToInteger(ViewState["id"]) > 0)
            {
                bool success = objChecklist.Update(collection, ViewState["id"]);
                if (!success)
                {
                    this.lblError.Text = Resources.Resource.error;
                    return;
                }

                this.lblError.Text = Resources.Resource.ActionSuccess;
                this.PopulateControls();
            }
            else
            {
                int checklistid = objChecklist.Add(collection);
                if (checklistid <= 0)
                {
                    this.lblError.Text = Resources.Resource.error;
                    return;
                }

                ViewState["id"] = checklistid;
                this.PopulateControls();
            }
        }

        protected void btnSaveSchedule_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            DateTime startDate = BusinessBase.Now;
            DateTime endDate = (this.panelEndsOn.Visible) ? Convert.ToDateTime(this.txtEndDate.Text.Trim(), CultureInfo.CurrentCulture) : DateTime.MinValue;

            string repeattype = string.Empty;
            switch (this.ConvertToInteger(this.ddlRepeat.SelectedValue))
            {
                case 1:
                    break;

                case 2:
                    foreach (ListItem item in this.ddlWeekly.Items)
                    {
                        if (item.Selected == false) continue;
                        repeattype += item.Value + ",";
                    }
                    repeattype = repeattype.TrimEnd(',');
                    break;

                case 3:
                    repeattype = this.ddlMonthDays.SelectedValue;
                    break;

                case 4:
                    repeattype = this.ddlDayOfWeek_Day.SelectedValue + "," + this.ddlDayOfWeek_Week.SelectedValue;
                    break;

                case 5:
                    repeattype = this.ddlAnnualyOn_Date.SelectedValue + "," + this.ddlAnnualyOn_Month.SelectedValue;
                    break;
            }

            NameValueCollection collection = new NameValueCollection();
            collection.Add("repeattype", this.ddlRepeat.SelectedValue);
            collection.Add("repeatval", repeattype);
            collection.Add("endson", this.txtEndDate.Text.Trim());

            Checklist objChecklist = new Checklist();

            int scheduleid = this.ConvertToInteger(ViewState["scheduleid"]);
            bool success = false;
            if (scheduleid > 0)
            {
                success = objChecklist.UpdateSchedule(collection, ViewState["scheduleid"]);
            }
            else
            {
                collection.Add("checklistid", ViewState["id"].ToString());
                scheduleid = objChecklist.AddSchedule(collection);
                success = (scheduleid > 0);
            }

            if (success)
            {
                objChecklist.DeleteScheduleDates(scheduleid);
                ArrayList datesArray = this.GenerateDatesBetweenSelectedDates(startDate, endDate);
                if (datesArray != null)
                {
                    foreach (Dictionary<string, string> dict in datesArray)
                    {
                        NameValueCollection collection2 = new NameValueCollection();
                        collection2.Add("scheduleid", scheduleid.ToString());
                        collection2.Add("startdate", dict["st"]);
                        collection2.Add("enddate", dict["et"]);
                        objChecklist.AddScheduleDates(collection2);
                    }
                }

                this.panelManageScheduleEdit.Visible = false;
                this.PopulateSchedule();
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void lnkManageSchedule_Click(object sender, EventArgs e)
        {
            this.panelManageScheduleEdit.Visible = true;
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.panelManageScheduleEdit.Visible = false;
            this.PopulateSchedule();
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

    }
}