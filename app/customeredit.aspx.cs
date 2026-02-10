using BABusiness;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class customeredit : ERPBase
    {
        public string EncCustomerId
        {
            get { return BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey); }
        }

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
            this.ddlCountry.DataSource = Common.GetCountries();
            this.ddlCountry.DataBind();
            this.ddlCountry.Items.Insert(0, new ListItem(Resources.Resource.Select, (int.MinValue).ToString()));

            this.ddlPhoneCountryCode.DataSource = Common.GetAllPhoneCountryCode();
            this.ddlPhoneCountryCode.DataBind();
            this.ddlPhoneCountryCode.Items.Insert(0, new ListItem(Resources.Resource.Select, (int.MinValue).ToString()));

            NameValueCollection collection = BUCustomer.GetCustomerDetail(ViewState["id"], this.CompanyId);
            if (collection != null)
            {
                NameValueCollection usercollection = UserBA.GetUserDetail(collection["userid"]);
                ViewState["userid"] = collection["userid"].ToString();
                ViewState["lang"] = usercollection["lang"].ToString();
                this.txtFirstName.Text = usercollection["fname"];
                this.txtLastName.Text = usercollection["lname"];                                
                this.txtPhone.Text = usercollection["phone"];
                if (!String.IsNullOrEmpty(usercollection["contactcountrycode"]))
                {
                    this.ddlPhoneCountryCode.SelectedValue = usercollection["contactcountrycode"];
                }
                if (!String.IsNullOrEmpty(usercollection["countryid"]))
                {
                    this.ddlCountry.SelectedValue = usercollection["countryid"];
                }
                this.txtCity.Text = usercollection["city"];
                this.txtPincode.Text = usercollection["pincode"];
                this.txtAddress.Text = usercollection["address"];
                this.ddlGender.SelectedValue = collection["gender"];

                if (!string.IsNullOrEmpty(collection["dob"]))
                {
                    DateTime dob = Convert.ToDateTime(collection["dob"]);
                    this.txtDOB.Text = dob.ToString(this.DateFormat);
                }

                this.txtAlterContactNo.Text = collection["alternatecontact"];
                this.ddlMembershipType.SelectedValue = collection["membershiptype"];

                this.lblEmailValue.Text = usercollection["email"];
                ViewState["emailValue"] = usercollection["email"];
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            NameValueCollection usercollection = new NameValueCollection();
            usercollection.Add("fname", this.txtFirstName.Text.Trim());
            usercollection.Add("lname", this.txtLastName.Text.Trim());
            usercollection.Add("phone", this.txtPhone.Text.Trim());
            usercollection.Add("email", this.ConvertToString(ViewState["emailValue"]));
            usercollection.Add("lang", ViewState["lang"].ToString());
            usercollection.Add("countryid", this.ddlCountry.SelectedValue);
            usercollection.Add("city", this.txtCity.Text.Trim());
            usercollection.Add("pincode", this.txtPincode.Text.Trim());
            usercollection.Add("address", this.txtAddress.Text.Trim());
            usercollection.Add("contactcountrycode", this.ddlPhoneCountryCode.SelectedValue);

            UserBA objUser = new UserBA();
            bool usersuccess = objUser.UpdateUser(usercollection, ViewState["userid"]);
            if (!usersuccess)
            {
                this.lblError.Text = "Failed to update user details. Please try again.";
                return;
            }

            NameValueCollection customercollection = new NameValueCollection();
            customercollection.Add("gender", this.ddlGender.SelectedValue);
            customercollection.Add("dob", this.txtDOB.Text.Trim());
            customercollection.Add("alternatecontact", this.txtAlterContactNo.Text.Trim());
            customercollection.Add("membershiptype", this.ddlMembershipType.SelectedValue);
            customercollection.Add("updatedby", this.UserId);

            bool success = BUCustomer.UpdateCustomer(customercollection, ViewState["id"]);
            if (success)
            {
                Response.Redirect("customerlist.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("customerview.aspx?cid=" + this.EncCustomerId);
        }
    }
}