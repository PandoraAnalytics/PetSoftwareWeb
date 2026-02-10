using BABusiness;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class addfood : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["animalid"] = this.ReadQueryString("animalid");
                ViewState["id"] = this.DecryptQueryString("id");                
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = null;

            this.ddlDay.Items.Add(new ListItem(Resources.Resource.Monday, "1"));
            this.ddlDay.Items.Add(new ListItem(Resources.Resource.Tuesday, "2"));
            this.ddlDay.Items.Add(new ListItem(Resources.Resource.Wednesday, "3"));
            this.ddlDay.Items.Add(new ListItem(Resources.Resource.Thursday, "4"));
            this.ddlDay.Items.Add(new ListItem(Resources.Resource.Friday, "5"));
            this.ddlDay.Items.Add(new ListItem(Resources.Resource.Saturday, "6"));
            this.ddlDay.Items.Add(new ListItem(Resources.Resource.Sunday, "7"));

            this.ddlTimesperday.Items.Add(new ListItem(Resources.Resource.Morning, "1"));
            this.ddlTimesperday.Items.Add(new ListItem(Resources.Resource.Afternoon, "2"));
            this.ddlTimesperday.Items.Add(new ListItem(Resources.Resource.Evening, "3"));
            this.ddlTimesperday.Items.Add(new ListItem(Resources.Resource.Night, "4"));

            int animalid = this.ConvertToInteger(ViewState["animalid"]);
            if (animalid > 0)
            {
                NameValueCollection animalCollection = AnimalBA.GetAnimalDetail(ViewState["animalid"]);
                if (animalCollection == null) Response.Redirect("landing.aspx");

                this.lnkEdit_Click(null, null);
            }
            else
            {
                int id = this.ConvertToInteger(ViewState["id"]);
                if (id <= 0) Response.Redirect("landing.aspx");

                collection = AnimalBA.GetAnimalFoodDetail(id);
                if (collection == null) Response.Redirect("landing.aspx");

                ViewState["animalid"] = collection["animalid"];
            }

            animalid = this.ConvertToInteger(ViewState["animalid"]);
            if (animalid == 0) Response.Redirect("landing.aspx");

            (Page.Master as breeder).AnimalId = animalid.ToString();

            if (collection != null)
            {
                this.txtFood.Text = collection["food"];
                this.ddlFoodType.SelectedValue = collection["foodtype"];

                string day = "";
                string[] values = collection["day"].Split(',');
                foreach (ListItem item in this.ddlDay.Items)
                {
                    item.Selected = values.Contains(item.Value);
                    if (item.Selected) day += item.Text + ", ";
                }

                string day_interval = "";
                values = collection["day_interval"].Split(',');
                foreach (ListItem item in this.ddlTimesperday.Items)
                {
                    item.Selected = values.Contains(item.Value);
                    if (item.Selected) day_interval += item.Text + ", ";
                }

                this.txtAmount.Text = collection["quantity"];
                this.txtUnit.Text = collection["unit"];

                this.lblFood.Text = collection["food"];

                string foodType = collection["foodtype"];
                switch (foodType)
                {
                    case "1":
                        foodType = "Dry";
                        break;
                    case "2":
                        foodType = "Wet";
                        break;
                }
                this.lblFoodType.Text = foodType;
                this.lblAmount.Text = collection["quantity"] + " " + collection["unit"];

                this.lblTimesperday.Text = day_interval.Trim().TrimEnd(',');
                this.lblDay.Text = day.Trim().TrimEnd(',');
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("food", this.txtFood.Text.Trim());
            collection.Add("foodtype", this.ddlFoodType.SelectedValue);
            collection.Add("userid", this.UserId);

            string value = string.Empty;
            foreach (ListItem item in this.ddlDay.Items)
            {
                if (item.Selected == false) continue;
                if (value.Length > 0) value += ",";
                value += item.Value;
            }
            collection["day"] = value;

            value = string.Empty;
            foreach (ListItem item in this.ddlTimesperday.Items)
            {
                if (item.Selected == false) continue;
                if (value.Length > 0) value += ",";
                value += item.Value;
            }
            collection["day_interval"] = value;

            collection.Add("quantity", this.txtAmount.Text.Trim());
            collection.Add("unit", this.txtUnit.Text.Trim());
            collection.Add("animalid", ViewState["animalid"].ToString());
            AnimalBA objBreed = new AnimalBA();
            bool success = ((ViewState["id"] != null && this.ConvertToInteger(ViewState["id"]) > 0) ? objBreed.UpdateAnimalFood(collection, ViewState["id"]) : objBreed.AddAnimalFood(collection));
            if (success)
            {
                Response.Redirect("foodlist.aspx?id=" + ViewState["animalid"]);
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            this.panelView.Visible = false;
            this.lnkEdit.Visible = false;
            this.panelEdit.Visible = true;
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.panelView.Visible = true;
            this.lnkEdit.Visible = true;
            this.lnkEdit.Text = "<i class=\"fa-solid fa-pen-to-square\"></i>&nbsp;Edit";
            this.panelEdit.Visible = false;
        }
    }
}