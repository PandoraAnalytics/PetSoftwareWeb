using BABusiness;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class custsignup : Page
    {
        public string DateFormat
        {
            get { return BusinessBase.ConvertToString(Session["dtformat"]); }
        }

        public string DateFormatSmall
        {
            get { return (this.DateFormat != null) ? this.DateFormat.ToLower() : string.Empty; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string queryString = Request.QueryString["token"];
                if (string.IsNullOrEmpty(queryString))
                {
                    this.ResetButton();
                    return;
                }

                queryString = BASecurity.Decrypt(queryString, BusinessBase.FixedSaltKey);
                if (string.IsNullOrEmpty(queryString))
                {
                    this.ResetButton();
                    return;
                }

                string companyid = queryString;
                if (string.IsNullOrEmpty(companyid))
                {
                    this.ResetButton();
                    return;
                }

                ViewState["cid"] = companyid;
                this.PopulateControls();
                if (Session["userlang"] != null) this.ddlLanguage.SelectedValue = Session["userlang"].ToString();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = UserBA.GetBusinessUserDetail(ViewState["cid"]);
            if (collection != null)
            {
                this.lblCompanyName.Text = collection["companyname"];
                if (!string.IsNullOrEmpty(collection["companylogo"]))
                {
                    this.companylogo.Src = PageBase.getbase64url(collection["companylogo"]);
                }
                else
                {
                    this.companylogo.Src = "images/defcomplogo2.png";
                }
            }
            else
            {
                this.ResetButton();
                return;
            }

            this.ddlCountry.DataSource = Common.GetCountries();
            this.ddlCountry.DataBind();
            this.ddlCountry.Items.Insert(0, new ListItem(Resources.Resource.Select, (int.MinValue).ToString()));

            this.ddlPhoneCountryCode.DataSource = Common.GetAllPhoneCountryCode();
            this.ddlPhoneCountryCode.DataBind();
            this.ddlPhoneCountryCode.Items.Insert(0, new ListItem(Resources.Resource.Select, (int.MinValue).ToString()));
        }

        protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["userlang"] = this.ddlLanguage.SelectedValue;
            Response.Redirect("signin.aspx");
        }

        protected override void InitializeCulture()
        {
            string lang = "en-US";
            if (Session["userlang"] != null) lang = Session["userlang"].ToString();

            if (string.IsNullOrEmpty(lang)) lang = "en-US";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";
            User objUser = new User();
            int newuserId = int.MinValue;
            int existingcustomerid = int.MinValue;
            // check if user email is already exist
            int emailID = BABusiness.User.CheckEmailExist(this.txtEmailAddress.Text.Trim());
            if (emailID <= 0)
            {
                // create new user                
                NameValueCollection collection = new NameValueCollection();
                collection.Add("user_pre_name", this.txtFirstName.Text.Trim());
                collection.Add("user_family_name", this.txtLastName.Text.Trim());
                collection.Add("user_email", this.txtEmailAddress.Text.Trim());
                collection.Add("user_phone", this.txtPhone.Text.Trim());
                collection.Add("user_address", this.txtAddress.Text.Trim());
                collection.Add("user_countryid", this.ddlCountry.SelectedValue);
                collection.Add("user_city", this.txtCity.Text.Trim());
                collection.Add("user_pincode", this.txtPincode.Text.Trim());
                collection.Add("user_type", "5");// new for customer
                collection.Add("password_token", "");
                collection.Add("password", "");
                collection.Add("user_token", BusinessBase.Now.Ticks.ToString());
                collection.Add("contactcountrycode", this.ddlPhoneCountryCode.SelectedValue);
                newuserId = objUser.Add(collection);
                if (newuserId > 0)// if new user added then add it to customer table
                {
                    // send email for new user
                    NameValueCollection newusercollection = UserBA.GetUserDetail(newuserId);
                    if (newusercollection != null)
                    {
                        BreederMail.SendEmail(BreederMail.MessageType.NEWUSERWELCOMEEMAIL, newusercollection);
                    }

                    this.AddCustomer(newuserId);
                }
            }
            else
            {
                // existing
                newuserId = BABusiness.User.GetUserId(this.txtEmailAddress.Text.Trim());
                //check for existing customer
                existingcustomerid = BUCustomer.GetCustomerIdByUserID(newuserId, ViewState["cid"]);
                if (existingcustomerid <= 0)
                {
                    this.AddCustomer(newuserId);
                }
                else
                {
                    this.lblError.Text = "Customer already exists for this email address";
                }

            }            
        }

        private void AddCustomer(int xiNewUserId)
        {
            //add new customer into table
            NameValueCollection customercollection = new NameValueCollection();
            customercollection.Add("companyid", ViewState["cid"].ToString());
            customercollection.Add("userid", xiNewUserId.ToString());
            customercollection.Add("gender", this.ddlGender.SelectedValue);
            customercollection.Add("dob", this.txtDOB.Text.Trim());
            customercollection.Add("alternatecontact", this.txtAlterContactNo.Text.Trim());
            customercollection.Add("membershiptype", this.ddlMembershipType.SelectedValue);
            string customercode = GenerateUniqueEmployeecode();
            customercollection.Add("customercode", customercode);
            //customercollection.Add("createdby", this.UserId);
            customercollection.Add("createdby", "1"); //currently userid=1 default

            int customerId = BUCustomer.AddCustomer(customercollection);
            if (customerId > 0)
            {
                customercollection.Clear();
                customercollection = new NameValueCollection();
                customercollection["bu_id"] = ViewState["cid"].ToString();
                //customercollection["user_id"] = this.UserId;
                customercollection["user_id"] = "1";//currently userid=1 default
                customercollection["message_id"] = (int)UserBA.Status.BUCUSTOMERADDED + "";
                customercollection["old_entry"] = string.Empty;
                customercollection["new_entry"] = customerId.ToString();
                customercollection["comment"] = string.Empty;
                UserBA.AddBULog(customercollection);
                Response.Redirect("signin.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }

        public static string GenerateUniqueEmployeecode()
        {
            string customercode = string.Empty;
            do
            {
                customercode = BABusiness.User.GetRandomPassword();
            }
            while (BUCustomer.IsCustomerCodeExist(customercode) > 0);
            return customercode;
        }

        private void ResetButton()
        {
            this.panelInfo.Visible = true;
            this.literalInfo.Text = "There is some error processing this page. Please check with your administrator.";
        }
    }
}