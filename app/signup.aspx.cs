using BABusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI;

namespace Breederapp
{
    public partial class signup : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (Session["userlang"] != null) this.ddlLanguage.SelectedValue = Session["userlang"].ToString();
            }
        }
        protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["userlang"] = this.ddlLanguage.SelectedValue;
            Response.Redirect("signup.aspx");
        }

        protected override void InitializeCulture()
        {
            string lang = "en-US";
            if (Session["userlang"] != null) lang = Session["userlang"].ToString();

            if (string.IsNullOrEmpty(lang)) lang = "en-US";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
        }

        private bool ValidateControls()
        {
            bool success = true;

            string newPassword = this.txtNewPassword.Text.Trim();
            if (newPassword.Length == 0)
            {
                this.lblError.Text = Resources.Resource.newpasswordcannotblank;
                success = false;
                return success;
            }

            string newPassword2 = this.txtNewPassword2.Text.Trim();
            if (newPassword != newPassword2)
            {
                this.lblError.Text = Resources.Resource.Newpasswordandconfirmpassword;
                success = false;
                return success;
            }
        
            string[] errors = this.ValidatePassword(newPassword);
            if (errors != null && errors.Length > 0)
            {
                this.lblError.Text = string.Join("<br>", errors);
                success = false;
                return success;
            }

            return success;
        }

        protected string[] ValidatePassword(string xiPassword)
        {
            ArrayList errorList = new ArrayList();

            int minlength = 6;
            if (minlength > 0 && xiPassword.Length < minlength)
            {
                errorList.Add(Resources.Resource.Theminimumpasswordlengthis + minlength + Resources.Resource.characters);
            }

            Regex re1 = new Regex(@".*[0-9].*");
            MatchCollection match1 = re1.Matches(xiPassword);
            if (match1 == null || match1.Count == 0)
            {
                errorList.Add(Resources.Resource.Thepasswordmustcontainatleast1number);
            }

            HashSet<char> specialCharacters = new HashSet<char>() { '%', '$', '#', '@', '!', '&', '*', '%', '_', '-', '+', ':', '.', '?', '~' };

            bool containsSC = false;
            foreach (char x in xiPassword)
            {
                containsSC = specialCharacters.Contains(x);
                if (containsSC) break;
            }
            if (containsSC == false)
            {
                errorList.Add(Resources.Resource.Thepasswordmustcontainatleast1specialcharacter);
            }

            Regex re2 = new Regex(@".*[a-z].*");
            MatchCollection match2 = re2.Matches(xiPassword);
            if (match2 == null || match2.Count == 0)
            {
                errorList.Add(Resources.Resource.Thepasswordmustcontainatleast1lowercaseletter);
            }

            Regex re3 = new Regex(@".*[A-Z].*");
            MatchCollection match3 = re3.Matches(xiPassword);
            if (match3 == null || match3.Count == 0)
            {
                errorList.Add(Resources.Resource.Thepasswordmustcontainatleast1uppercaseletter);
            }

            foreach (char x in xiPassword)
            {
                bool valid = char.IsLetterOrDigit(x) || specialCharacters.Contains(x);
                if (valid == false)
                {
                    errorList.Add(Resources.Resource.Pleaseremovethosecharactersandtryagain);
                    return (string[])errorList.ToArray(typeof(string));
                }
            }

            return (string[])errorList.ToArray(typeof(string));
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            bool success = this.ValidateControls();
            if (success == false) return;

            int matcholdpass = BABusiness.User.CheckEmailExist(this.txtEmail.Text.Trim());
            if (matcholdpass > 0)
            {
                this.lblError.Text = Resources.Resource.EmailisAlreadyExistPleaseChangeEmail;
                return;
            }

            NameValueCollection collection = new NameValueCollection();
            collection.Add("user_pre_name", this.txtFirstName.Text.Trim());
            collection.Add("user_family_name", this.txtLastName.Text.Trim());
            collection.Add("user_email", this.txtEmail.Text.Trim());
            collection.Add("user_phone", this.txtMobile.Text.Trim());
            //collection.Add("user_type", this.rdbType.SelectedItem.Value);
            collection.Add("user_type", "2");//owner
            collection.Add("password_token", "");
            collection.Add("password", this.txtNewPassword.Text.Trim());
            collection.Add("user_token", BusinessBase.Now.Ticks.ToString());
            //collection.Add("is_verified", "1");

            User objUser = new User();
            int userId = objUser.Add(collection);

            if (userId > 0)
            {
                NameValueCollection usercollection = UserBA.GetUserDetail(userId);
                if (usercollection != null)
                {
                    Session["userid"] = userId;
                    Session["username"] = usercollection["fname"] + " " + usercollection["lname"];
                    Session["usertype"] = usercollection["type"];
                    Session["dtformat"] = "dd.MM.yyyy";
                    BreederMail.PageURL = System.Configuration.ConfigurationManager.AppSettings["configurl"];
                    BreederMail.SendEmail(BreederMail.MessageType.NEWUSERWELCOMEEMAIL, usercollection);
                    usercollection = null;
                    Response.Redirect("landing.aspx");
                }
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }
    }
}