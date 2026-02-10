using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class staffview : ERPBase
    {
        public string EncStaffId
        {
            get { return BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey); }
        }

        public string StaffId
        {
            get { return this.ConvertToString(ViewState["id"]); }
        }

        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString("id");
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection photocollection = BUStaff.GetStaffGallaryDetail(ViewState["id"]);
            if (photocollection != null)
            {
                this.repPhotos.DataSource = BUStaff.GetStaffPhotos(ViewState["id"]);
                this.repPhotos.DataBind();
            }

            NameValueCollection collection = BUStaff.GetStaffDetail(ViewState["id"], this.CompanyId);
            if (collection != null)
            {
                this.lblFname.Text = collection["fname"];
                this.lblLname.Text = collection["lname"];
                this.lblEmail.Text = collection["email"];
                this.lblPhone.Text = collection["phone"];
                this.lblAddress.Text = collection["address"];

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
                this.lblJobTitle.Text = collection["jobtitle"];
                this.lblDepartment.Text = collection["departmentname"];
                this.lblJobRole.Text = collection["jobrolename"];
                if (!string.IsNullOrEmpty(collection["joiningdate"]))
                {
                    DateTime tempDate2 = Convert.ToDateTime(collection["joiningdate"]);
                    if (tempDate2 != DateTime.MinValue) this.lblJoiningDate.Text = tempDate2.ToString(this.DateFormat);
                }

                switch (collection["employmentstatus"])
                {
                    case "1":
                        this.lblEmploymentStatus.Text = Resources.Resource.Active;
                        break;

                    case "2":
                        this.lblEmploymentStatus.Text = Resources.Resource.Resigned;
                        break;

                    case "3":
                        this.lblEmploymentStatus.Text = Resources.Resource.Terminate;
                        break;

                    case "4":
                        this.lblEmploymentStatus.Text = Resources.Resource.OnLeave;
                        break;

                }
                this.lblSupervisor.Text = collection["sup_fname"] + " " + collection["sup_lname"];

            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("staffedit.aspx?id=" + this.EncStaffId);
        }
    }
}