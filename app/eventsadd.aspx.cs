using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class eventsadd : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            this.IsAssociationAccess = true;
            base.Page_Load(sender, e);

            if (!this.IsPostBack)
            {
                this.PopulateControls();
            }

            string backgroundimage = this.ConvertToString(this.hid_bannerimage.Value);
            if (!string.IsNullOrEmpty(backgroundimage))
            {
                backgroundimage = "url('docs/" + backgroundimage + "')";
                this.default_banner.Style.Add("background-image", backgroundimage);
            }
        }

        private void PopulateControls()
        {
            this.hid_bannerimage.Value = "default_banner_image.png";

            this.txtDate.Text = BusinessBase.Now.ToString(this.DateFormat);
            this.txtEndDate.Text = BusinessBase.Now.ToString(this.DateFormat);

            DateTime tempTime = new DateTime(2020, 1, 1, 0, 0, 0);
            for (int i = 0; i < 96; i++)
            {
                DateTime newTime = tempTime.AddMinutes(i * 15);
                this.ddlStartTime.Items.Add(newTime.ToString("HH:mm"));
                this.ddlEndTime.Items.Add(newTime.ToString("HH:mm"));
            }

            DataTable dataTable = BreederData.GetBreedCategory();
            if (dataTable != null)
            {
                DataRow row = dataTable.NewRow();
                row["id"] = int.MinValue;
                row["breedname"] = Resources.Resource.Select;
                dataTable.Rows.InsertAt(row, 0);

                this.ddlSelectAnimal.DataSource = dataTable;
                this.ddlSelectAnimal.DataBind();
            }

            this.ddlTimezone.DataSource = Common.GetSystemTimezones();
            this.ddlTimezone.DataBind();
            this.ddlTimezone.SelectedValue = BusinessBase.Timezone;

            DateTime now = BusinessBase.Now.Date;

            NameValueCollection collection = new NameValueCollection();
            collection.Add("startdate", now.ToString(this.DateFormat));
            collection.Add("enddate", now.AddMonths(3).ToString(this.DateFormat));
            collection.Add("userid", this.UserId);

            this.hidfilter.Value = EventBA.EventSearch(collection);
        }

        protected void btnCreateEvent_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            DateTime startDate = Convert.ToDateTime(this.txtDate.Text.Trim() + " " + this.ddlStartTime.SelectedValue, CultureInfo.CurrentCulture);
            DateTime endDate = Convert.ToDateTime(this.txtEndDate.Text.Trim() + " " + this.ddlEndTime.SelectedValue, CultureInfo.CurrentCulture);
            if (DateTime.Compare(startDate, endDate) >= 0)
            {
                lblError.Text = Resources.Resource.Enddateshouldbegreaterthanstartdate;
                return;
            }

            TimeZoneInfo sourceTimezone = TimeZoneInfo.FindSystemTimeZoneById(this.ddlTimezone.SelectedValue);
            TimeZoneInfo destinationTimezone = TimeZoneInfo.FindSystemTimeZoneById(BusinessBase.Timezone);
            startDate = TimeZoneInfo.ConvertTime(startDate, sourceTimezone, destinationTimezone);
            endDate = TimeZoneInfo.ConvertTime(endDate, sourceTimezone, destinationTimezone);

            string breedlist = "";
            foreach (ListItem listItem in ddlSelectBreed.Items)
            {
                if (listItem.Selected)
                {
                    if (breedlist.Length > 0) breedlist += ",";
                    breedlist += listItem.Value;
                }
            }

            string visible = "";
            foreach (ListItem listItem in ddlVisible1.Items)
            {
                if (listItem.Selected)
                {
                    if (listItem.Value == "" || string.IsNullOrEmpty(listItem.Value))
                    {
                        visible = "";
                        break;
                    }
                    else
                    {
                        if (visible.Length > 0) visible += ",";
                        visible += listItem.Value;
                    }
                }
            }

            NameValueCollection collection = new NameValueCollection();
            collection.Add("title", this.txtEventTitle.Text.Trim());
            collection.Add("startdatetime", startDate.ToString(this.DateTimeFormat));
            collection.Add("enddatetime", endDate.ToString(this.DateTimeFormat));
            collection.Add("description", this.txtDescription.Text.Trim());
            collection.Add("venue", this.txtEventVenue.Text.Trim());
            collection.Add("breedtype", breedlist);
            collection.Add("animalcategory", this.ddlSelectAnimal.SelectedValue);
            collection.Add("terms_condition", this.txtRuleandCondition.Text.Trim());
            collection.Add("bannerimage", this.hid_bannerimage.Value);
            collection.Add("userid", this.UserId);
            collection.Add("visible", visible);

            EventBA objevent = new EventBA();
            int eventid = objevent.AddEvent(collection);
            if (eventid > 0)
            {
                string[] files = this.filenames.Value.Split(',');

                collection.Clear();
                collection["eventid"] = eventid.ToString();
                foreach (string file in files)
                {
                    if (string.IsNullOrEmpty(file)) continue;

                    collection["file"] = file;
                    objevent.AddFiles(collection);
                }

                Response.Redirect("eventslist.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }

        protected void ddlSelectAnimal_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlSelectBreed.Items.Clear();

            this.ddlSelectBreed.DataSource = BreederData.GetBreedTypes(this.ddlSelectAnimal.SelectedValue);
            this.ddlSelectBreed.DataBind();
        }
    }
}