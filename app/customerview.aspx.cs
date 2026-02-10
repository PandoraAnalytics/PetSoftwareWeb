using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class customerview_aspx : ERPBase
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
                ViewState["id"] = DecryptQueryString("cid");
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = BUCustomer.GetCustomerDetail(ViewState["id"], this.CompanyId);
            if (collection != null)
            {
                this.lblFname.Text = collection["fname"];
                this.lblLname.Text = collection["lname"];
                this.lblEmail.Text = collection["email"];
                this.lblPhone.Text = collection["userphoneprefix"] + " " + collection["phone"];
                this.lblAddress.Text = collection["address"];
                this.lblCountry.Text = collection["countryname"];
                this.lblCity.Text = collection["city"];
                this.lblPostcode.Text = collection["pincode"];

                switch (collection["gender"])
                {
                    case "1":
                        this.lblGender.Text = Resources.Resource.Male;
                        break;

                    case "2":
                        this.lblGender.Text = Resources.Resource.Female;
                        break;
                }

                if (!string.IsNullOrEmpty(collection["dob"]))
                {
                    DateTime tempDate = Convert.ToDateTime(collection["dob"]);
                    if (tempDate != DateTime.MinValue) this.lblDob.Text = tempDate.ToString(this.DateFormat);
                }

                this.lblAlternatecontact.Text = collection["alternatecontact"];


                switch (collection["membershiptype"])
                {
                    case "1":
                        this.lblMembershipType.Text = "Gold";
                        break;

                    case "2":
                        this.lblMembershipType.Text = "Silver";
                        break;

                    case "3":
                        this.lblMembershipType.Text = "Platinum";
                        break;

                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("customeredit.aspx?id=" + this.EncCustomerId);
        }
    }
}