using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class eventlist : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            this.IsAssociationAccess = true;
            base.Page_Load(sender, e);

            if (!this.IsPostBack)
            {
                ViewState["animalid"] = this.ReadQueryString("animalid");
                ViewState["id"] = this.DecryptQueryString("id");
                this.PopulateControls();
            }

            string backgroundimage = this.ConvertToString(this.hid_bannerimage.Value);
            if (!string.IsNullOrEmpty(backgroundimage))
            {
                backgroundimage = "url('" + PageBase.getbase64url(backgroundimage) + "')";
                this.default_banner.Style.Add("background-image", backgroundimage);
            }
        }

        private void PopulateControls()
        {
            int id = this.ConvertToInteger(ViewState["id"]);
            if (id == 0) Response.Redirect("landing.aspx");

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

            string breedlist = "";
            foreach (ListItem listItem in ddlSelectBreed.Items)
            {
                if (listItem.Selected)
                {
                    if (breedlist.Length > 0) breedlist += ",";
                    breedlist += listItem.Value;
                }
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
            NameValueCollection collection1 = null;
            collection1 = EventBA.GetEventDetail(id);

            if (collection1 != null)
            {
                DateTime startdate = Convert.ToDateTime(collection1["startdate"]);
                DateTime enddate = Convert.ToDateTime(collection1["enddate"]);

                this.txtEventTitle.Text = collection1["title"];
                this.txtDate.Text = startdate.ToString(this.DateFormat);
                this.ddlStartTime.SelectedValue = startdate.ToString("HH:mm");
                this.txtEndDate.Text = enddate.ToString(this.DateFormat);
                this.ddlEndTime.SelectedValue = startdate.ToString("HH:mm");
                this.txtDescription.Text = collection1["description"];
                this.txtEventVenue.Text = collection1["venue"];
                this.ddlSelectAnimal.SelectedValue = collection1["animalcategory"];
                this.txtRuleandCondition.Text = collection1["terms_condition"];


                this.ddlSelectBreed.Items.Clear();

                this.ddlSelectBreed.DataSource = BreederData.GetBreedTypes(this.ddlSelectAnimal.SelectedValue);
                this.ddlSelectBreed.DataBind();

                //string type = "";
                //string[] values = collection1["breedtype"].Split(',');
                //foreach (ListItem item in this.ddlSelectBreed.Items)
                //{
                //    item.Selected = values.Contains(item.Value);
                //    if (item.Selected) type += item.Text + ", ";
                //}

                string breedtype = "," + collection1["breedtype"] + ",";
                foreach (ListItem item in this.ddlSelectBreed.Items)
                {
                    item.Selected = (breedtype.Contains("," + item.Value + ","));
                }

                if (!string.IsNullOrEmpty(collection1["visible"]))
                {
                    string visibletype = "," + collection1["visible"] + ",";
                    foreach (ListItem item in this.ddlVisible.Items)
                    {
                        item.Selected = (visibletype.Contains("," + item.Value + ","));
                    }
                }
                else
                {
                    this.ddlVisible.Items[0].Selected = true;
                }
                //this.ddlVisible.SelectedValue = collection1["visible"];

                if (!string.IsNullOrEmpty(collection1["banner_image"]))
                {
                    //string backgroundimage = "url('" + PageBase.getbase64url(collection["banner_image"]) + "')";
                    //this.default_banner.Style.Add("background-image", backgroundimage);

                    string backgroundimage = "url('" + PageBase.getbase64url(collection1["banner_image"]) + "')";
                    this.default_banner.Style.Add("background-image", backgroundimage);
                    this.hid_bannerimage.Value = collection1["banner_image"];
                }
            }

            DateTime now = BusinessBase.Now.Date;

            NameValueCollection collection = new NameValueCollection();
            collection.Add("startdate", now.ToString(this.DateFormat));
            collection.Add("enddate", now.AddMonths(3).ToString(this.DateFormat));

            this.hidfilter.Value = EventBA.EventSearch(collection);
        }

        protected void btnUpdateEvent_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            DateTime startDate = Convert.ToDateTime(this.txtDate.Text.Trim() + " " + this.ddlStartTime.SelectedValue, CultureInfo.CurrentCulture);
            DateTime endDate = Convert.ToDateTime(this.txtEndDate.Text.Trim() + " " + this.ddlEndTime.SelectedValue, CultureInfo.CurrentCulture);
            if (DateTime.Compare(startDate, endDate) >= 0)
            {
                lblError.Text = Resources.Resource.Enddateshouldbegreaterthanstartdate;
                return;
            }

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
            foreach (ListItem listItem in ddlVisible.Items)
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
            collection.Add("startdatetime", this.txtDate.Text.Trim() + " " + this.ddlStartTime.SelectedValue);
            collection.Add("enddatetime", this.txtEndDate.Text.Trim() + " " + this.ddlEndTime.SelectedValue);
            collection.Add("description", this.txtDescription.Text.Trim());
            collection.Add("venue", this.txtEventVenue.Text.Trim());
            collection.Add("breedtype", breedlist);
            collection.Add("animalcategory", this.ddlSelectAnimal.SelectedValue);
            collection.Add("terms_condition", this.txtRuleandCondition.Text.Trim());
            collection.Add("bannerimage", this.hid_bannerimage.Value);
            collection.Add("userid", this.UserId);
            collection.Add("visible", visible);

            EventBA objevent = new EventBA();
            bool success = objevent.UpdateEvent(collection, ViewState["id"]);
            if (success)
            {
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