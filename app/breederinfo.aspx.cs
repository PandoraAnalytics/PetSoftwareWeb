using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class breederinfo : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = this.ReadQueryString("id");
                (Page.Master as breeder).AnimalId = ViewState["id"].ToString();
                this.PopulateControls();
            }
            else
            {
                switch (Request["__EVENTTARGET"])
                {
                    case "search":
                        this.SearchBreederInfo(this.Request["__EVENTARGUMENT"]);
                        //this.pnlViewBreederInfo.Visible = true;
                        break;
                }
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = AnimalBA.GetAnimalDetail(ViewState["id"]);
            if (!string.IsNullOrEmpty(collection["breederemail"]))
            {
                this.pnlNoData.Visible = false;
                this.panelView.Visible = true;
                this.pnlSubmitBtn.Visible = true;
                this.lblBreederName.Text = this.ConvertToString(collection["breedername"]);
                this.lblBreederEmail.Text = this.ConvertToString(collection["breederemail"]);

            }
            else
            {
                this.pnlNoData.Visible = true;
                this.panelView.Visible = false;
                this.pnlSubmitBtn.Visible = false;
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            this.panelView.Visible = false;
            this.lnkEdit.Visible = false;
            this.panelEdit.Visible = true;
            this.pnlNoData.Visible = false;
        }

        protected void rdbBreederType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // show panel as per selected type
            int type = this.ConvertToInteger(this.rdbBreederType.SelectedValue);
            switch (type)
            {
                case 1:
                    this.pnlViewBreederInfo.Visible = true;
                    NameValueCollection collection = UserBA.GetUserDetailsByEmailId(this.ConvertToString(Session["email"]));
                    lblViewUserName.Text = collection["fname"] + " " + collection["lname"];
                    lblViewUserEmail.Text = collection["email"];
                    lblViewUserMobile.Text = collection["phone"];
                    this.pnlSubmitBtn.Visible = true;
                    this.pnlSearchBreeder.Visible = false;
                    this.pnlEditBreederInfo.Visible = false;
                    break;

                case 2:
                    this.pnlViewBreederInfo.Visible = false;
                    this.pnlSearchBreeder.Visible = false;
                    this.pnlViewBreederInfo.Visible = false;
                    this.pnlEditBreederInfo.Visible = false;
                    this.pnlSubmitBtn.Visible = true;
                    break;

                case 3:
                    this.txtSearchBreeder.Text = "";
                    this.pnlViewBreederInfo.Visible = false;
                    this.pnlSearchBreeder.Visible = true;
                    this.pnlViewBreederInfo.Visible = false;
                    this.pnlEditBreederInfo.Visible = false;
                    this.pnlSubmitBtn.Visible = false;
                    break;
            }
        }

        private void SearchBreederInfo(string xiEmail)
        {
            this.pnlSubmitBtn.Visible = true;
            NameValueCollection collection = UserBA.GetUserDetailsByEmailId(xiEmail);
            if (collection != null)
            {
                this.pnlViewBreederInfo.Visible = true;
                this.pnlEditBreederInfo.Visible = false;
                lblViewUserName.Text = collection["fname"] + " " + collection["lname"];
                lblViewUserEmail.Text = collection["email"];
                lblViewUserMobile.Text = collection["phone"];
                this.pnlSubmitBtn.Visible = true;
                ViewState["seachuserid"] = collection["id"];
            }
            else
            {
                this.pnlViewBreederInfo.Visible = false;
                this.pnlEditBreederInfo.Visible = true;
            }
        }

        private void ClearControl()
        {
            lblViewUserName.Text = "";
            lblViewUserEmail.Text = "";
            lblViewUserMobile.Text = "";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            int type = this.ConvertToInteger(this.rdbBreederType.SelectedValue);
            NameValueCollection collection = new NameValueCollection();
            switch (type)
            {
                case 1:
                    collection.Add("breederid", this.UserId);
                    break;

                case 3:
                    if (this.pnlViewBreederInfo.Visible == true)
                    {
                        //collection.Add("breedername", this.lblViewUserName.Text.Trim());
                        //collection.Add("breederemail", this.lblViewUserEmail.Text.Trim());

                        collection.Add("breederid", this.ConvertToString(ViewState["seachuserid"]));
                    }
                    else if (this.pnlEditBreederInfo.Visible == true)
                    {
                        collection.Add("breedername", this.txtEditBreederName.Text.Trim());
                        collection.Add("breederemail", this.txtEditBreederEmail.Text.Trim());
                    }
                    break;
            }


            AnimalBA objBreed = new AnimalBA();
            bool success = objBreed.UpdateAnimalBreederInfo(collection, ViewState["id"]);
            if (success)
            {
                Response.Redirect("breederinfo.aspx?id=" + ViewState["id"]);
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }

        }
    }
}