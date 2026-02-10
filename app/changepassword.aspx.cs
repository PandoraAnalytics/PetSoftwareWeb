using BABusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web.Services;

namespace Breederapp
{
    public partial class changepassword : PageBase
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
        }

        private bool ValidateControls()
        {
            bool success = true;

            string newPassword = this.txtNewPassword.Text.Trim();
            if (newPassword.Length == 0)
            {
                this.lblError.Text = Resources.Resource.Oldpasswordandnewpassword;
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            bool success = this.ValidateControls();
            if (success == false) return;

            string userid = this.UserId;

            UserBA objUser = new UserBA();
            string dbpassword = objUser.GetPassword(userid);
            bool verifyPassword = BASecurity.VerifyHash(this.txtOldPassword.Text.Trim(), dbpassword);
            if (!verifyPassword)
            {
                this.lblError.Text = Resources.Resource.Invalidoldpassword;
                return;
            }

            string new_password_enc = BASecurity.HashPassword(this.txtNewPassword.Text.Trim());
            success = objUser.UpdatePassword(new_password_enc, this.UserId);
            objUser = null;
            if (success)
            {
                NameValueCollection usercollection = UserBA.GetUserDetail(this.UserId);
                BreederMail.SendEmail(BreederMail.MessageType.USERCHANGEPASSWORD, usercollection);
                this.lblError.Text = Resources.Resource.ActionSuccess;
            }
            else
            {
                this.lblError.Text = Resources.Resource.Invalidoldpassword;
                return;
            }
        }

        [WebMethod]
        public static string CheckPasswordKeys(string password)
        {
            if (string.IsNullOrEmpty(password)) return string.Empty;

            NameValueCollection collection = new NameValueCollection();
            collection.Add("min_length", "6");
            collection.Add("isnumberrequired", "1");
            collection.Add("isnumberspecialcharrequired", "1");
            collection.Add("islowercaserequired", "1");
            collection.Add("isuppercaserequried", "1");

            int minlength = int.MinValue;
            int.TryParse(collection["min_length"], out minlength);
            if (minlength > 0 && password.Length < minlength)
            {
                collection.Remove("min_length");
            }

            if (collection["isnumberrequired"] == "1")
            {
                Regex re1 = new Regex(@".*[0-9].*");
                MatchCollection match1 = re1.Matches(password);
                if (match1 == null || match1.Count == 0)
                {
                    collection.Remove("isnumberrequired");
                }
            }

            if (collection["isnumberspecialcharrequired"] == "1")
            {
                HashSet<char> specialCharacters = new HashSet<char>() { '%', '$', '#', '@', '!', '&', '*', '%', '_', '-', '+', ':', '.', '?', '~' };

                bool containsSC = false;
                foreach (char x in password)
                {
                    containsSC = specialCharacters.Contains(x);
                    if (containsSC) break;
                }
                if (containsSC == false)
                {
                    collection.Remove("isnumberspecialcharrequired");
                }
            }

            if (collection["islowercaserequired"] == "1")
            {
                Regex re2 = new Regex(@".*[a-z].*");
                MatchCollection match2 = re2.Matches(password);
                if (match2 == null || match2.Count == 0)
                {
                    collection.Remove("islowercaserequired");
                }
            }

            if (collection["isuppercaserequried"] == "1")
            {
                Regex re3 = new Regex(@".*[A-Z].*");
                MatchCollection match3 = re3.Matches(password);
                if (match3 == null || match3.Count == 0)
                {
                    collection.Remove("isuppercaserequried");
                }
            }

            return string.Join(",", collection.AllKeys);
        }
    }
}