using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Threading;

namespace Breederapp
{
    public partial class signin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (Session["userlang"] != null) this.ddlLanguage.SelectedValue = Session["userlang"].ToString();
            }
        }

        protected void btnSignin_Click(object sender, EventArgs e)
        {
            Session.Clear();

            this.lblError.Text = "";

            if (this.txtUsername.Text.Trim().Length == 0 || this.txtPassword.Text.Trim().Length == 0)
            {
                this.lblError.Text = Resources.Resource.UsernameandPasswordcannotbeblank;
                return;
            }
            this.ProcessUserLogin();
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

        private void ProcessUserLogin()
        {
            string userId = string.Empty;
            string userType = string.Empty;

            Common.LoginStatus loginStatus = Common.DoLogin(this.txtUsername.Text.Trim(), this.txtPassword.Text.Trim(), out userId, out userType);

            if (loginStatus == Common.LoginStatus.SUCCESS)
            {
                Session["userid"] = userId;
                Session["usertype"] = userType;

				BreederMail.PageURL = System.Configuration.ConfigurationManager.AppSettings["configurl"];               

                NameValueCollection collection = UserBA.GetUserDetail(userId);
                if (collection != null)
                {
                    Session["username"] = collection["fname"] + " " + collection["lname"];
                    Session["userlang"] = collection["lang"];
                    Session["email"] = collection["email"];
                    Session["dtformat"] = "dd.MM.yyyy";
                    Session["isowner"] = (collection["isowner"] == "1") ? "1" : null;
                    Session["isassociation"] = (collection["isowner"] == "1") ? 1 : int.MinValue;
                    BusinessBase.Timezone = collection["timezone"];
                }
                collection = null;

                DataTable assocationTable = UserBA.GetAssociationBreederTypes(userId);
                if (assocationTable != null && assocationTable.Rows.Count > 0)
                {
                    Session["isassociation"] = 1;

                    string associatedbreeds = string.Empty;
                    foreach (DataRow row in assocationTable.Rows)
                    {
                        if (row["breedtype"] == DBNull.Value) { associatedbreeds = ""; break; }

                        string breedertype = Convert.ToString(row["breedtype"]);
                        if (string.IsNullOrEmpty(breedertype)) { associatedbreeds = ""; break; }

                        associatedbreeds += breedertype + ",";
                    }

                    Session["associationbreedertype"] = associatedbreeds.TrimEnd(',').Trim();
                }

                Response.Redirect("landing.aspx");
            }
            else
            {
                switch (loginStatus)
                {
                    case Common.LoginStatus.NONE:
                        this.lblError.Text = Resources.Resource.error;
                        break;

                    case Common.LoginStatus.USERNOTCONFIRMED:
                        this.lblError.Text = Resources.Resource.Youmustactiveyouraccount;
                        break;

                    case Common.LoginStatus.WRONGUSERORPASS:
                        this.lblError.Text = Resources.Resource.Unabletologin;
                        break;
                }

                return;
            }
        }

    }
}