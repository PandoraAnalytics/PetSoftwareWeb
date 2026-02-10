using BABusiness;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace Breederapp
{
    public partial class burequest : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.ApplyFilters();
            }
        }

        private void ApplyFilters()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("companyname", this.txtComapny.Text.Trim());
            collection.Add("status", this.ddlStatus.SelectedValue);

            this.hdfilter.Value = UserBA.SearchRequestDetails(collection);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilters();
        }

        [WebMethod(EnableSession = true)]
        public static string GetBusinessEnquiryDetail(string enquiryid)
        {
            if (string.IsNullOrEmpty(enquiryid)) return string.Empty;
            NameValueCollection collection = UserBA.GetBusinessEnquiryDetail(enquiryid);
            if (collection == null) return string.Empty;

            var dictionary1 = new Dictionary<string, object>();

            dictionary1["fname"] = collection["fname"];
            dictionary1["lname"] = collection["lname"];
            dictionary1["email"] = collection["email"];
            dictionary1["phone"] = collection["countrycode"] +" "+collection["phone"];
            dictionary1["address"] = collection["address"];
            dictionary1["companyname"] = collection["companyname"];
            dictionary1["companyshortname"] = collection["companyshortname"];
            dictionary1["website"] = collection["website"];
            dictionary1["registrationno"] = collection["registrationno"];
            dictionary1["businesstype"] = collection["businesstypename"];
            dictionary1["status"] = collection["status"];
            dictionary1["reason"] = collection["reason"];
            dictionary1["buid"] = collection["buid"];

            DataTable dt = UserBA.GetBusinessEnquiryPhotos(enquiryid);
            string docValue = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {                
                foreach (DataRow dr in dt.Rows)
                {
                    if(!string.IsNullOrEmpty(dr["gallery_file"].ToString()))
                    {
                        docValue += dr["gallery_file"].ToString() +",";
                    }
                }
                 docValue = docValue.Remove(docValue.Length - 1);
            }
            
            dictionary1["docValue"] = docValue;   

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Serialize(dictionary1);
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            this.lblMessage.Visible = false;
            this.lblMessage.Text = string.Empty;

            string remark = this.txtNotAppRemark.Text.Trim();
            if (remark.Length > 255)
            {
                this.lblMessage.Text = Resources.Resource.Remarkcannotmorecharector;
                this.lblMessage.Visible = true;
                return;
            }

            string enquiryId = this.hid.Value;
            ViewState["enquiryid"] = this.hid.Value;

            NameValueCollection collection = UserBA.GetBusinessEnquiryDetail(enquiryId);
            if (this.ConvertToInteger(ViewState["enquiryid"]) > 0)
            {
                NameValueCollection emailDetailsCollection = new NameValueCollection();
                emailDetailsCollection.Add("name", collection["fname"] + " " + collection["lname"]);
                emailDetailsCollection.Add("email", collection["email"]);
                emailDetailsCollection.Add("reason", remark);
                BreederMail.SendEmail(BreederMail.MessageType.BUSINESSUSERREQUESTREJECT, emailDetailsCollection);
            }

            NameValueCollection newCollection = new NameValueCollection();
            newCollection.Add("status", "2");
            newCollection.Add("reason", "Rejected : " + remark);
            newCollection.Add("updatedby", this.UserId);
            bool success = UserBA.UpdateBusinessEnquiry(newCollection, enquiryId);

            if (success)
            {
                collection.Clear();
                collection = new NameValueCollection();
                collection["bu_id"] = string.Empty;
                collection["user_id"] = this.UserId;
                collection["message_id"] = (int)UserBA.Status.BUREQUESTREJECT + "";
                collection["old_entry"] = enquiryId;//here we pass enquiry id
                collection["new_entry"] = string.Empty;
                collection["comment"] = "Rejected : " + remark;
                UserBA.AddBULog(collection);
                this.txtNotAppRemark.Text = string.Empty;
            }

        }

        private void AddBusinessUser(NameValueCollection xiUsercollection, int xiNewUserId)
        {
            //add new business user into table
            NameValueCollection bucollection = new NameValueCollection();
            bucollection.Add("userid", xiNewUserId.ToString());
            bucollection.Add("companyname", xiUsercollection["companyname"]);
            bucollection.Add("companyshortname", xiUsercollection["companyshortname"]);
            bucollection.Add("businesstype", xiUsercollection["businesstype"]);
            bucollection.Add("website", xiUsercollection["website"]);
            bucollection.Add("address", xiUsercollection["address"]);
            bucollection.Add("registrationno", xiUsercollection["registrationno"]);
            bucollection.Add("approvedby", this.UserId);
            bucollection.Add("enquiryid", xiUsercollection["id"]);

            int buId = UserBA.AddBusinessUser(bucollection);
            if (buId > 0)
            {
                //send email to new business user   
                NameValueCollection emailDetailsCollection = UserBA.GetUserDetail(xiNewUserId);
                emailDetailsCollection.Add("name", emailDetailsCollection["fname"] + " " + emailDetailsCollection["lname"]);
                emailDetailsCollection.Add("companyname", xiUsercollection["companyname"]);
                BreederMail.SendEmail(BreederMail.MessageType.BUSINESSUSERREQUESTACCEPT, emailDetailsCollection);

                BreederMail.SendEmail(BreederMail.MessageType.BUSINESSUSERREQUESTACCEPTADMIN, emailDetailsCollection);

                NameValueCollection newCollection = new NameValueCollection();
                newCollection.Add("status", "3");
                newCollection.Add("updatedby", this.UserId);
                bool success = UserBA.UpdateBusinessEnquiry(newCollection, this.hid.Value);

                if (success)
                {
                    //add to log
                    NameValueCollection logCollection = new NameValueCollection();
                    logCollection["bu_id"] = buId.ToString();
                    logCollection["user_id"] = this.UserId;
                    logCollection["message_id"] = (int)UserBA.Status.BUREQUESTACCEPT + "";
                    logCollection["old_entry"] = this.hid.Value;//here we pass enquiry id
                    logCollection["new_entry"] = string.Empty;
                    logCollection["comment"] = "Business user request accepted";
                    UserBA.AddBULog(logCollection);
                }

                // add logic here for request documents transfer to business documents
                bool gsuccess= UserBA.AddGalleryFromBusinessEnquiry(this.ConvertToInteger(xiUsercollection["id"]),buId);

                ViewState["buid"] = buId.ToString();
            }
        }

        protected void btnApproval_Click(object sender, EventArgs e)
        {
            this.lblMessage.Visible = false;
            this.lblMessage.Text = string.Empty;

            string enquiryId = this.hid.Value;
            User objUser = new User();

            int newuserId = int.MinValue;
            NameValueCollection usercollection = UserBA.GetBusinessEnquiryDetail(enquiryId);//here we check for enquiry details
            if (usercollection != null)
            {
                // check if user email is already exist
                int emailID = BABusiness.User.CheckEmailExist(usercollection["email"]);
                if (emailID <= 0)
                {
                    // create new user                
                    NameValueCollection collection = new NameValueCollection();
                    collection.Add("user_pre_name", usercollection["fname"]);
                    collection.Add("user_family_name", usercollection["lname"]);
                    collection.Add("user_email", usercollection["email"]);
                    collection.Add("user_phone", usercollection["phone"]);
                    collection.Add("user_type", "3");// new for business user
                    collection.Add("password_token", "");
                    collection.Add("password", "");
                    collection.Add("user_token", BusinessBase.Now.Ticks.ToString());
                    collection.Add("contactcountrycode", usercollection["contactcountrycode"]);
                    newuserId = objUser.Add(collection);
                    if (newuserId > 0)// if new user added then add it to business user table
                    {
                        // send email for new user
                        NameValueCollection newusercollection = UserBA.GetUserDetail(newuserId);
                        if (newusercollection != null)
                        {
                            BreederMail.SendEmail(BreederMail.MessageType.NEWUSERWELCOMEEMAIL, newusercollection);
                        }

                        this.AddBusinessUser(usercollection, newuserId);
                        //add ispos=1 in customer table as new customer
                        this.AddCustomer(newuserId);
                    }
                }
                else
                {
                   
                    // existing
                    newuserId = BABusiness.User.GetUserId(usercollection["email"]);

                    //update contact country code prefix in user table
                    bool success = UserBA.UpdateUserContactCountryCode(usercollection["contactcountrycode"], newuserId);

                    this.AddBusinessUser(usercollection, newuserId);
                    //add ispos=1 in customer table as new customer
                    this.AddCustomer(newuserId);
                }
            }
            else
            {
                Response.Redirect("burequest.aspx");
            }
        }

        private void AddCustomer(int xiNewUserId)
        {
            //add new customer into table
            NameValueCollection customercollection = new NameValueCollection();
            customercollection.Add("companyid", ViewState["buid"].ToString());
            customercollection.Add("userid", xiNewUserId.ToString());

            customercollection.Add("gender", "1"); //compulsary value currently value set for male=1
           
            string customercode = GenerateUniqueEmployeecode();
            customercollection.Add("customercode", customercode);
            customercollection.Add("createdby", this.UserId);
            customercollection.Add("ispos", "1");

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
                // Response.Redirect("customerlist.aspx");
            }
            else
            {
                //this.lblError.Text = Resources.Resource.error;
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