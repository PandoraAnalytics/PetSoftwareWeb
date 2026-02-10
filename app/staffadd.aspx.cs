using BABusiness;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

namespace Breederapp
{
    public partial class staffadd : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            this.tablnkBasic.Attributes.Add("class", "active");
            this.tablnkImages.Attributes.Remove("class");

            this.tab_basic_details.Attributes.Add("class", "tab-pane active");
            this.tab_images.Attributes.Add("class", "tab-pane");

            DataTable dtDepartment = BUStaff.GetAllStaffDepartment(this.CompanyId);
            if (dtDepartment != null)
            {
                DataRow row = dtDepartment.NewRow();
                row["id"] = int.MinValue;
                row["name"] = Resources.Resource.Select;
                dtDepartment.Rows.InsertAt(row, 0);

                this.ddlDepartment.DataSource = dtDepartment;
                this.ddlDepartment.DataBind();
            }

            DataTable dtRole = BUStaff.GetAllStaffJobRoles(this.CompanyId);
            if (dtRole != null)
            {
                DataRow row = dtRole.NewRow();
                row["id"] = int.MinValue;
                row["name"] = Resources.Resource.Select;
                dtRole.Rows.InsertAt(row, 0);

                this.ddlJobRole.DataSource = dtRole;
                this.ddlJobRole.DataBind();
            }

            DataTable dtSupervisor = BUStaff.GetStaff(this.CompanyId);// this.UserId temporary for bu_id=1
            if (dtSupervisor != null)
            {
                DataRow row = dtSupervisor.NewRow();
                row["id"] = int.MinValue;
                row["name"] = Resources.Resource.Select;
                dtSupervisor.Rows.InsertAt(row, 0);

                this.ddlSupervisor.DataSource = dtSupervisor;
                this.ddlSupervisor.DataBind();
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";
            User objUser = new User();
            int newuserId = int.MinValue;
            // check if user email is already exist
            int emailID = BABusiness.User.CheckEmailExist(this.txtEmailAddress.Text.Trim());
            if (emailID <= 0)
            {
                // create new user                
                NameValueCollection collection = new NameValueCollection();
                collection.Add("user_pre_name", this.txtFirstName.Text.Trim());
                collection.Add("user_family_name", this.txtLastName.Text.Trim());
                collection.Add("user_email", this.txtEmailAddress.Text.Trim());
                collection.Add("user_phone", this.txtPhone.Text.Trim());
                collection.Add("user_address", this.txtAddress.Text.Trim());
                collection.Add("user_type", "4");// new for staff
                collection.Add("password_token", "");
                collection.Add("password", "");
                collection.Add("user_token", BusinessBase.Now.Ticks.ToString());
                newuserId = objUser.Add(collection);
                if (newuserId > 0)// if new user added then add it to staff table
                {
                    // send email for new user
                    NameValueCollection newusercollection = UserBA.GetUserDetail(newuserId);
                    if (newusercollection != null)
                    {
                        BreederMail.SendEmail(BreederMail.MessageType.NEWUSERWELCOMEEMAIL, newusercollection);
                    }

                    this.AddStaff(newuserId);
                }
            }
            else
            {
                // existing
                newuserId = BABusiness.User.GetUserId(this.txtEmailAddress.Text.Trim());
                this.AddStaff(newuserId);
            }
        }

        private void AddStaff(int xiNewUserId)
        {
            //add new staff into table
            NameValueCollection staffcollection = new NameValueCollection();
            staffcollection.Add("companyid", this.CompanyId);// this.UserId temporary for bu_id=1
            staffcollection.Add("userid", xiNewUserId.ToString());
            staffcollection.Add("gender", this.ddlGender.SelectedValue);
            staffcollection.Add("dob", this.txtDOB.Text.Trim());
            staffcollection.Add("alternatecontact", this.txtAlterContactNo.Text.Trim());
            staffcollection.Add("jobtitle", this.txtJobTitle.Text.Trim());
            staffcollection.Add("department", this.ddlDepartment.SelectedValue);
            staffcollection.Add("jobrole", this.ddlJobRole.SelectedValue);
            staffcollection.Add("joiningdate", this.txtJoiningDate.Text.Trim());
            staffcollection.Add("employmentstatus", this.ddlEmplymentStatus.SelectedValue);
            staffcollection.Add("supervisorid", (this.ConvertToInteger(this.ddlSupervisor.SelectedValue) > 0 ? this.ddlSupervisor.SelectedValue : string.Empty));
            string emloyeecode = GenerateUniqueEmployeecode();
            staffcollection.Add("employeecode", emloyeecode);
            staffcollection.Add("createdby", this.UserId);

            int staffId = BUStaff.AddStaff(staffcollection);
            if (staffId > 0)
            {
                staffcollection.Clear();
                staffcollection = new NameValueCollection();
                staffcollection["bu_id"] = this.CompanyId;// this.UserId temporary for bu_id=1
                staffcollection["user_id"] = this.UserId;
                staffcollection["message_id"] = (int)UserBA.Status.BUSTAFFADDED + "";
                staffcollection["old_entry"] = string.Empty;
                staffcollection["new_entry"] = staffId.ToString();
                staffcollection["comment"] = string.Empty;
                UserBA.AddBULog(staffcollection);

                NameValueCollection staffemailcollection = new NameValueCollection();
                staffemailcollection["staffid"] = staffId.ToString();
                staffemailcollection["bu_id"] = this.CompanyId;
                BreederMail.SendEmail(BreederMail.MessageType.NEWSTAFFWELCOMEEMAIL, staffemailcollection);

                ViewState["id"] = staffId.ToString();
                //Response.Redirect("stafflist.aspx");
                this.tablnkBasic.Attributes.Remove("class");
                this.tablnkImages.Attributes.Add("class", "active");

                this.tab_basic_details.Attributes.Add("class", "tab-pane");
                this.tab_images.Attributes.Add("class", "tab-pane active");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }

        protected void btnSavePhoto_Click(object sender, EventArgs e)
        {
            this.lblPhotoError.Text = string.Empty;

            string[] files = this.filenames.Value.Split(',');
            if (files == null || files.Length == 0)
            {
                this.lblPhotoError.Text = Resources.Resource.error;
                return;
            }
            NameValueCollection collection = new NameValueCollection();
            collection["staffid"] = ViewState["id"].ToString();
            collection["userid"] = this.UserId;

            foreach (string file in files)
            {
                if (string.IsNullOrEmpty(file)) continue;

                string extension = file.Substring(file.LastIndexOf('.'));
                if (string.IsNullOrEmpty(extension)) continue;

                extension = extension.ToLower();

                ArrayList extensionArray = new ArrayList(5);
                extensionArray.Add(".jpg");
                extensionArray.Add(".gif");
                extensionArray.Add(".png");
                extensionArray.Add(".jpeg");
                extensionArray.Add(".mp4");

                if (extensionArray.Contains(extension) == false) continue;

                int fileType = int.MinValue;
                switch (extension)
                {
                    case ".jpg":
                    case ".gif":
                    case ".png":
                    case ".jpeg":
                        fileType = 1;
                        break;

                    case ".mp4":
                        fileType = 2;
                        break;
                }

                collection["file_name"] = file;
                collection["title"] = file.Substring(file.IndexOf('_') + 1);
                collection["file_type"] = fileType.ToString();

                BUStaff.AddStaffGallery(collection);
            }

            this.filenames.Value = string.Empty;
            Response.Redirect("staffview.aspx?id=" + BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey));
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("stafflist.aspx");
        }

        public static string GenerateUniqueEmployeecode()
        {
            string employeecode = string.Empty;
            do
            {
                employeecode = BABusiness.User.GetRandomPassword();
            }
            while (BUStaff.IsEmployeeCodeExist(employeecode) > 0);

            return employeecode;
        }
    }
}