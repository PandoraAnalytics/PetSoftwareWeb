using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class buprofileview : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = Request.QueryString["buid"];              
                this.PopulateControls();
            }
        }
        private void PopulateControls()
        {           
            NameValueCollection collection = UserBA.GetBusinessUserDetail(ViewState["id"]);
            if (collection == null) Response.Redirect("budashboard.aspx");

            this.lblName.Text = collection["fname"] + " " + collection["lname"];
            this.lblEmailAddress.Text = collection["email"];
            this.lblPhone.Text = collection["phone"];

            this.lblCompany.Text = collection["companyname"];
            this.lblShortName.Text = collection["companyshortname"];
            this.lblWebsite.Text = collection["website"];
            this.lblBusinessType.Text = collection["businesstypename"];
            this.lblRegistrationNo.Text = collection["registrationno"];
            this.lblAddress.Text = collection["address"];

            if (!String.IsNullOrEmpty(collection["countryname"]))
            {
                this.lblCountry.Text = collection["countryname"];
            }
            this.lblCity.Text = collection["city"];
            this.lblPostcode.Text = collection["pincode"];

            if (!string.IsNullOrEmpty(collection["dateofincorporation"]))
            {
                DateTime incdate = Convert.ToDateTime(collection["dateofincorporation"]);
                this.lblDateOfIncorporation.Text = incdate.ToString(this.DateFormat);
            }

            this.lblTinNo.Text = collection["tinno"];
            this.lblLicenceNo.Text = collection["licenceno"];
            this.lblEmployerId.Text = collection["employeridno"];
            this.lblAboutBusiness.Text = collection["description"];

            if (!string.IsNullOrEmpty(collection["companylogo"]))
            {
                this.lnkCompanyLogo.HRef = "../app/viewdocument.aspx?file=" + collection["companylogo"].ToString() + ""; //"docs/" + collection["companylogo"];
            }
        }
    }
}