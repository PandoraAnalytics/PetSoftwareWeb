using BABusiness;
using System;
using System.Collections.Specialized;


namespace Breederapp
{
    public partial class addmember : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = this.DecryptQueryString("aid");
                this.PopulateControls();

            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = UserBA.GetAssociation(ViewState["id"]);
            if (collection == null) Response.Redirect("manageassociation.aspx");

            ViewState["type"] = (this.ConvertToInteger(collection["createdby"]) == this.ConvertToInteger(this.UserId) || Member.CheckISAdminStatus(Session["Email"], ViewState["id"]) == 1) ? 1 : 2;

            int type = this.ConvertToInteger(ViewState["type"]);
            if (type == 2)
            {
                NameValueCollection userCollection = UserBA.GetUserDetail(this.UserId);
                if (userCollection == null) Response.Redirect("manageassociation.aspx");

                this.txtFirstName.Text = userCollection["fname"];
                this.txtLastName.Text = userCollection["lname"];
                this.txtEmailAddress.Text = userCollection["email"];
                this.txtMobile.Text = userCollection["phone"];

                ViewState["owneremail"] = collection["email"];
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string aID = this.ConvertToString(ViewState["id"]);

            if (string.IsNullOrEmpty(aID))
                Response.Redirect("landing.aspx");

            int retVal = Member.CheckIsAssociationMemberEmailExist(this.txtEmailAddress.Text.Trim(), ViewState["id"], null);
            if (retVal > 0)
            {
                this.lblError.Text = Resources.Resource.EmailisAlreadyExistPleaseChangeEmail;
                return;
            }

            NameValueCollection collection = new NameValueCollection();
            collection.Add("memberno", this.txtMemberNo.Text.Trim());
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
            collection.Add("submitby", this.UserId);

            int type = this.ConvertToInteger(ViewState["type"]);
            if (type == 2)
                collection.Add("active", "2");
            else
                collection.Add("active", "1");

            Member objMember = new Member();
            int memberId = objMember.AddMember(collection);
            if (memberId > 0)
            {
                NameValueCollection memberCollection = new NameValueCollection();
                memberCollection.Add("memberid", this.ConvertToString(memberId));
                memberCollection.Add("association_id", this.ConvertToString(ViewState["id"]));
                if (type == 2)
                    memberCollection.Add("active", "2");
                else
                    memberCollection.Add("active", "1");
                int retId = objMember.AddAssociationMembers(memberCollection);

                if (type == 2)
                {
                    //MAIL SEND CODE
                    //string token = BusinessBase.Now.Ticks.ToString();
                    NameValueCollection mailCollection = new NameValueCollection();
                    mailCollection.Add("memberemail", this.ConvertToString(Session["email"]));
                    mailCollection.Add("membername", this.ConvertToString(Session["username"]));
                    mailCollection.Add("toemail", this.ConvertToString(ViewState["owneremail"]));

                    if (mailCollection != null) BreederMail.SendEmail(BreederMail.MessageType.SENDMEMBERSHIPREQUEST, mailCollection);

                    Response.Redirect("associationsearch.aspx");
                }
                else
                    Response.Redirect("memberlist.aspx?aid=" + BASecurity.Encrypt(ViewState["id"].ToString(), Breederapp.PageBase.HashKey));
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("associationsearch.aspx");
        }
    }
}