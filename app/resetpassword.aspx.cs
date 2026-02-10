using BABusiness;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;

namespace Breederapp
{
    public partial class resetpassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string token = (Request.QueryString.Count > 0) ? Request.QueryString[0] : "0";

                NameValueCollection collection = BABusiness.User.GetUserByToken(token);
                if (collection != null && string.IsNullOrEmpty(collection["user_id"]) == false)
                {
                    this.panelExpiry.Visible = false;
                    this.panelReset.Visible = true;

                    try
                    {
                        Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();
                        DateTime dateTime = Convert.ToDateTime(collection["user_token_date"]);
                        if (dateTime != DateTime.MinValue)
                        {
                            if (BusinessBase.Now.Subtract(dateTime).TotalHours > 48) collection = null;
                        }
                    }
                    catch { }

                    if (collection != null)
                    {
                        ViewState["userid"] = collection["user_id"];
                        ViewState["useremail"] = collection["user_email"];
                    }
                }


                if (collection == null)
                {
                    this.panelReset.Visible = false;
                    this.panelExpiry.Visible = true;
                }

                /*string script1 = "closePopup();";
                string csname = "ModalScript";
                Type cstype = this.GetType();
                ClientScriptManager cs = Page.ClientScript;
                if (!cs.IsStartupScriptRegistered(cstype, csname))
                {
                    cs.RegisterStartupScript(cstype, csname, script1, true);
                }*/
            }
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
            if (string.Compare(newPassword, newPassword2, false) != 0)
            {
                this.lblError.Text = Resources.Resource.Newpasswordandconfirmpassword;
                success = false;
                return success;
            }

            if (newPassword.Length < 6)
            {
                this.lblError.Text = Resources.Resource.Pleaseenter6characters;
                success = false;
                return success;
            }

            bool isDigit = false;
            bool isLetter = false;
            HashSet<char> specialCharacters = new HashSet<char>() { '%', '$', '#', '@', '!', '&', '*', '%', '_', '-', '+', ':', '.', '?' };
            foreach (char x in newPassword)
            {
                bool valid = char.IsLetterOrDigit(x) || specialCharacters.Contains(x);
                if (valid == false)
                {
                    this.lblError.Text = Resources.Resource.Invalidcharactersinnewpassword;
                    success = false;
                    return success;
                }

                if (isDigit == false) isDigit = char.IsDigit(x);
                if (isLetter == false) isLetter = char.IsLetter(x);
            }

            if (isDigit == false || isLetter == false)
            {
                this.lblError.Text = Resources.Resource.lengthbetween615;
                success = false;
                return success;
            }
            return success;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            bool success = this.ValidateControls();
            if (success == false) return;

            User objUser = new User();

            string new_password_enc = BASecurity.HashPassword(this.txtNewPassword.Text.Trim());

            NameValueCollection collection = new NameValueCollection();
            collection.Add("password", new_password_enc);
            success = objUser.ResetPassword(collection, ViewState["userid"]);

            if (success)
            {
                BreederMail.PageURL = System.Configuration.ConfigurationManager.AppSettings["configurl"];
                BreederMail.SendEmail(BreederMail.MessageType.CHANGEPASSWORD, ViewState["useremail"]);
                Response.Redirect("signin.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }
    }
}