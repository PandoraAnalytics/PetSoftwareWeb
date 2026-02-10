using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class custcompanylist : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.ApplyFilter();
            }
        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("custidnotin", this.UserId);
            collection.Add("companyname", this.txtName.Text.Trim());
            this.hdfilter.Value = UserBA.SearchCompany(collection);
        }

        protected void btnApply1_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;
            NameValueCollection customercollection = new NameValueCollection();
            customercollection.Add("companyid", this.hid.Value);
            customercollection.Add("userid", this.UserId);
            customercollection.Add("gender", this.ddlGender.SelectedValue);
            customercollection.Add("dob", this.txtDOB.Text.Trim());
            customercollection.Add("alternatecontact", this.txtAlterContactNo.Text.Trim());
            customercollection.Add("membershiptype", this.ddlMembershipType.SelectedValue);
            string customercode = GenerateUniqueEmployeecode();
            customercollection.Add("customercode", customercode);
            customercollection.Add("createdby", this.UserId);

            int customerId = BUCustomer.AddCustomer(customercollection);
            if (customerId > 0)
            {
                customercollection.Clear();
                customercollection = new NameValueCollection();
                customercollection["bu_id"] = this.CompanyId;
                customercollection["user_id"] = this.UserId;
                customercollection["message_id"] = (int)UserBA.Status.BUCUSTOMERADDED + "";
                customercollection["old_entry"] = string.Empty;
                customercollection["new_entry"] = customerId.ToString();
                customercollection["comment"] = string.Empty;
                UserBA.AddBULog(customercollection);
                Response.Redirect("profile.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }

        public static string GenerateUniqueEmployeecode()
        {
            string customercode = string.Empty;
            do
            {
                customercode = BABusiness.User.GetRandomPassword();
            }
            while (BUCustomer.IsCustomerCodeExist(customercode) > 0);
            return customercode;
        }
    }
}