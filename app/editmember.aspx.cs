using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class editmember : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = this.DecryptQueryString();
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = Member.GetMember(ViewState["id"]);
            if (collection == null) Response.Redirect("memberlist.aspx");

            NameValueCollection ascollection = UserBA.GetAssociation(collection["association_id"]);
            if (ascollection == null) Response.Redirect("manageassociation.aspx");

            ViewState["AssociationId"] = collection["association_id"];

            bool isOwner = (this.ConvertToInteger(ascollection["createdby"]) == this.ConvertToInteger(this.UserId) || Member.CheckISAdminStatus(Session["Email"], ViewState["AssociationId"]) == 1);
            if (!isOwner) Response.Redirect("manageassociation.aspx");

            this.txtMemberNo2.Text = collection["memberno"];
            this.txtFirstName.Text = collection["fname"];
            this.txtLastName.Text = collection["lname"];
            this.txtAddress.Text = collection["address"];
            this.txtCity.Text = collection["city"];
            this.txtCountry.Text = collection["country"];
            this.txtZipCode.Text = collection["zipcode"];
            this.txtEmailAddress.Text = collection["email"];
            this.txtMobile.Text = collection["mobile"];
            this.txtFax.Text = collection["fax"];

            try
            {
                DateTime entrydate = Convert.ToDateTime(collection["entrydate"]);
                this.txtEntryDate.Text = entrydate.ToString(this.DateFormat);
            }
            catch { }


            try
            {
                DateTime exitdate = Convert.ToDateTime(collection["exitdate"]);
                this.txtExitDate.Text = exitdate.ToString(this.DateFormat);
            }
            catch { }


            try
            {
                DateTime animalbirthdate = Convert.ToDateTime(collection["animalbirthdate"]);
                this.txtAnimalBirthdate.Text = animalbirthdate.ToString(this.DateFormat);
            }
            catch { }

            this.txtExitReason.Text = collection["exitreason"];
            this.txtMembershipType.Text = collection["membershiptype"];
            this.txtfamilyfullmember.Text = collection["familyfullmember"];
            this.txtPositioninRegion.Text = collection["positioninregion"];
            this.txtRegion.Text = collection["region"];
            this.ddlIsBREEDERYesorNo.SelectedValue = collection["isbreeder"];
            this.txtfamilymemberof.Text = collection["familymemberof"];
            this.txtRemark.Text = collection["remark"];
            this.txtpaymentmethod.Text = collection["paymentmethod"];
            this.txtAccountnumber.Text = collection["accountnumber"];
            this.txtAccountownerName.Text = collection["accountownername"];
            this.txtBankName.Text = collection["bankname"];
            this.txtBankcode.Text = collection["bankcode"];
            this.txtBreedtype.Text = collection["breedtype"];
            this.txtAnimalName.Text = collection["animalname"];
            this.txtCollarId.Text = collection["collarid"];
            this.txtPhone.Text = collection["phone"];
            this.txtStreet.Text = collection["street"];
        }

        private void BackToPage()
        {
            string refUrl = this.ConvertToString(ViewState["refurl"]);
            if (!string.IsNullOrEmpty(refUrl))
            {
                Response.Redirect(refUrl);
            }
            else
            {
                Response.Redirect("landing.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtExitDate.Text) && string.IsNullOrEmpty(this.txtExitReason.Text))
            {
                lblExitReasonMsg.Text = Resources.Resource.RequriedMsg;
                return;
            }

            if (!string.IsNullOrEmpty(this.txtExitReason.Text) && string.IsNullOrEmpty(this.txtExitDate.Text))
            {
                lblExitDateMsg.Text = Resources.Resource.RequriedMsg;
                return;
            }

            int retVal = Member.CheckIsAssociationMemberEmailExist(this.txtEmailAddress.Text.Trim(), ViewState["AssociationId"], ViewState["id"]);
            if (retVal > 0)
            {
                this.lblError.Text = Resources.Resource.EmailisAlreadyExistPleaseChangeEmail;
                return;
            }

            NameValueCollection collection = new NameValueCollection();
            collection.Add("memberno", this.txtMemberNo2.Text.Trim());
            collection.Add("fname", this.txtFirstName.Text.Trim());
            collection.Add("lname", this.txtLastName.Text.Trim());
            collection.Add("address", this.txtAddress.Text.Trim());
            collection.Add("city", this.txtCity.Text.Trim());
            collection.Add("country", this.txtCountry.Text.Trim());
            collection.Add("zipcode", this.txtZipCode.Text.Trim());
            collection.Add("email", this.txtEmailAddress.Text.Trim());
            collection.Add("mobile", this.txtMobile.Text.Trim());
            collection.Add("fax", this.txtFax.Text.Trim());
            collection.Add("entrydate", this.txtEntryDate.Text.Trim());
            collection.Add("exitdate", this.txtExitDate.Text.Trim());
            collection.Add("exitreason", this.txtExitReason.Text.Trim());
            collection.Add("membershiptype", this.txtMembershipType.Text.Trim());
            collection.Add("familyfullmember", this.txtfamilyfullmember.Text.Trim());
            collection.Add("positioninregion", this.txtPositioninRegion.Text.Trim());
            collection.Add("region", this.txtRegion.Text.Trim());
            collection.Add("isbreeder", this.ddlIsBREEDERYesorNo.SelectedValue);
            collection.Add("familymemberof", this.txtfamilymemberof.Text.Trim());
            collection.Add("remark", this.txtRemark.Text.Trim());
            collection.Add("paymentmethod", this.txtpaymentmethod.Text.Trim());
            collection.Add("accountnumber", this.txtAccountnumber.Text.Trim());
            collection.Add("accountownername", this.txtAccountownerName.Text.Trim());
            collection.Add("bankname", this.txtBankName.Text.Trim());
            collection.Add("bankcode", this.txtBankcode.Text.Trim());
            collection.Add("breedtype", this.txtBreedtype.Text.Trim());
            collection.Add("animalname", this.txtAnimalName.Text.Trim());
            collection.Add("animalbirthdate", this.txtAnimalBirthdate.Text.Trim());
            collection.Add("collarid", this.txtCollarId.Text.Trim());
            collection.Add("phone", this.txtPhone.Text.Trim());
            collection.Add("street", this.txtStreet.Text.Trim());
            // collection.Add("animalid", ViewState["animalid"].ToString());
            Member objMember = new Member();
            bool success = objMember.UpdateMember(collection, ViewState["id"]);
            if (success)
            {
                Response.Redirect("memberlist.aspx?aid=" + BASecurity.Encrypt(ViewState["AssociationId"].ToString(), PageBase.HashKey));
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("memberlist.aspx?aid=" + BASecurity.Encrypt(ViewState["AssociationId"].ToString(), PageBase.HashKey));
        }
    }
}