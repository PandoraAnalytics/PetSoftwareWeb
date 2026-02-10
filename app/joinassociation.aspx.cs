using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class joinassociation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ViewState["ivid"] = Request.QueryString["invitationcode"];
            this.PopulateControls();
        }

        private void PopulateControls()
        {
            string invitationcode = this.ConvertToString(ViewState["ivid"]);
            if (string.IsNullOrEmpty(invitationcode)) Response.Redirect("signin.aspx");

            NameValueCollection collection = UserBA.GetAssociationByInvitationCode(invitationcode);
            if (collection == null) Response.Redirect("signin.aspx");

            ViewState["aid"] = collection["id"];
            collection = null;
        }

        private string ConvertToString(object xiObj)
        {
            string returnString = string.Empty;
            if (xiObj == null || xiObj == DBNull.Value) return returnString;

            try
            {
                returnString = Convert.ToString(xiObj);
            }
            catch
            {
                returnString = string.Empty;
            }

            return returnString;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string associationId = this.ConvertToString(ViewState["aid"]);
            if (string.IsNullOrEmpty(associationId)) Response.Redirect("signin.aspx");

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
            collection.Add("submitby", "0");
            collection.Add("active", "2");

            Member objMember = new Member();
            int memberId = objMember.AddMember(collection);

            if (memberId <= 0)
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }

            NameValueCollection memberCollection = new NameValueCollection();
            memberCollection.Add("memberid", memberId.ToString());
            memberCollection.Add("association_id", associationId);
            memberCollection.Add("active", "2");
            int retId = objMember.AddAssociationMembers(memberCollection);

            this.lblError.Text = Resources.Resource.ActionSuccess;
            this.btnSave.Visible = false;
        }
    }
}