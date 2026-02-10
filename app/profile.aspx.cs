using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class profile : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            this.ddlPhoneCountryCode.DataSource = Common.GetAllPhoneCountryCode();
            this.ddlPhoneCountryCode.DataBind();
            this.ddlPhoneCountryCode.Items.Insert(0, new ListItem(Resources.Resource.Select, (int.MinValue).ToString()));

            NameValueCollection collection = UserBA.GetUserDetail(this.UserId);
            if (collection == null) Response.Redirect("signin.aspx");

            this.ddlTimezone.DataSource = Common.GetSystemTimezones();
            this.ddlTimezone.DataBind();
            this.ddlTimezone.SelectedValue = BusinessBase.Timezone;

            this.ddlCountry.DataSource = Common.GetCountries();
            this.ddlCountry.DataBind();
            this.ddlCountry.Items.Insert(0, new ListItem(Resources.Resource.Select, (int.MinValue).ToString()));

            this.txtFirstName.Text = collection["fname"];
            this.txtLastName.Text = collection["lname"];
            this.lblEmailAddress.Text = collection["email"];
            this.txtMobile.Text = collection["phone"];
            if (!String.IsNullOrEmpty(collection["contactcountrycode"]))
            {
                this.ddlPhoneCountryCode.SelectedValue = collection["contactcountrycode"];
            }
            this.ddlLanguage.SelectedValue = collection["lang"];
            this.ddlTimezone.SelectedValue = collection["timezone"];
            if (!String.IsNullOrEmpty(collection["countryid"]))
            {
                this.ddlCountry.SelectedValue = collection["countryid"];
            }
            this.txtCity.Text = collection["city"];
            this.txtPincode.Text = collection["pincode"];
            this.txtAddress.Text = collection["address"];

            if (!string.IsNullOrEmpty(collection["profileimage"]))
            {
                this.hid_profile_logo.Value = collection["profileimage"].ToString();
                this.lnkProfileLogo.HRef = "../app/viewdocument.aspx?file=" + collection["profileimage"].ToString() + ""; //PageBase.getbase64url(collection["profileimage"]);// "docs/" + collection["profileimage"];
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            NameValueCollection collection = new NameValueCollection();
            collection.Add("fname", this.txtFirstName.Text.Trim());
            collection.Add("lname", this.txtLastName.Text.Trim());
            collection.Add("phone", this.txtMobile.Text.Trim());
            collection.Add("email", this.lblEmailAddress.Text.Trim());
            collection.Add("lang", this.ddlLanguage.SelectedValue);
            collection.Add("timezone", this.ddlTimezone.SelectedValue);
            collection.Add("countryid", this.ddlCountry.SelectedValue);
            collection.Add("city", this.txtCity.Text.Trim());
            collection.Add("pincode", this.txtPincode.Text.Trim());
            collection.Add("address", this.txtAddress.Text.Trim());
            collection.Add("profileimage", this.hid_profile_logo.Value);
            collection.Add("contactcountrycode", this.ddlPhoneCountryCode.SelectedValue);

            UserBA objUser = new UserBA();
            bool success = objUser.UpdateUser(collection, this.UserId);

            if (success)
            {
                Session["userlang"] = this.ddlLanguage.SelectedValue;
                BusinessBase.Timezone = this.ddlTimezone.SelectedValue;
                this.PopulateControls();
                this.lblError.Text = Resources.Resource.ActionSuccess;
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }

        }
    }
}