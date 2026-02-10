using BABusiness;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class staffedit : ERPBase
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
                this.ManageTab(Request.QueryString["tab"]);
                this.PopulateControls();
                this.PopulateImages();
            }
        }

        private void PopulateControls()
        {
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

            DataTable dtSupervisor = BUStaff.GetStaff(this.CompanyId);
            if (dtSupervisor != null)
            {
                DataRow row = dtSupervisor.NewRow();
                row["id"] = int.MinValue;
                row["name"] = Resources.Resource.Select;
                dtSupervisor.Rows.InsertAt(row, 0);

                this.ddlSupervisor.DataSource = dtSupervisor;
                this.ddlSupervisor.DataBind();
            }

            NameValueCollection collection = BUStaff.GetStaffDetail(ViewState["id"], this.CompanyId);
            if (collection != null)
            {
                this.lblname.Text = collection["fname"] + " " + collection["lname"];
                this.lblEmail.Text = collection["email"];
                this.ddlGender.SelectedValue = collection["gender"];

                if (!string.IsNullOrEmpty(collection["dob"]))
                {
                    DateTime dob = Convert.ToDateTime(collection["dob"]);
                    this.txtDOB.Text = dob.ToString(this.DateFormat);
                }

                this.txtAlterContactNo.Text = collection["alternatecontact"];
                this.txtJobTitle.Text = collection["jobtitle"];
                this.ddlDepartment.SelectedValue = collection["department"];
                this.ddlJobRole.SelectedValue = collection["jobrole"];

                if (!string.IsNullOrEmpty(collection["joiningdate"]))
                {
                    DateTime joinigdate = Convert.ToDateTime(collection["joiningdate"]);
                    this.txtJoiningDate.Text = joinigdate.ToString(this.DateFormat);
                }

                this.ddlEmplymentStatus.SelectedValue = collection["employmentstatus"];
                if (this.ConvertToInteger(collection["supervisorid"]) > 0)
                {
                    this.ddlSupervisor.SelectedValue = collection["supervisorid"];
                }

            }
        }

        private void PopulateImages()
        {
            NameValueCollection photocollection = BUStaff.GetStaffGallaryDetail(ViewState["id"]);
            if (photocollection != null)
            {
                this.repPhotos.DataSource = BUStaff.GetStaffPhotos(ViewState["id"]);
                this.repPhotos.DataBind();
            }
            else
            {
                this.PanelViewPhoto.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            NameValueCollection staffcollection = new NameValueCollection();

            staffcollection.Add("gender", this.ddlGender.SelectedValue);
            staffcollection.Add("dob", this.txtDOB.Text.Trim());
            staffcollection.Add("alternatecontact", this.txtAlterContactNo.Text.Trim());
            staffcollection.Add("jobtitle", this.txtJobTitle.Text.Trim());
            staffcollection.Add("department", this.ddlDepartment.SelectedValue);
            staffcollection.Add("jobrole", this.ddlJobRole.SelectedValue);
            staffcollection.Add("joiningdate", this.txtJoiningDate.Text.Trim());
            staffcollection.Add("employmentstatus", this.ddlEmplymentStatus.SelectedValue);
            staffcollection.Add("supervisorid", (this.ConvertToInteger(this.ddlSupervisor.SelectedValue) > 0 ? this.ddlSupervisor.SelectedValue : string.Empty));

            bool success = BUStaff.UpdateStaff(staffcollection, ViewState["id"]);
            if (success)
            {
                //Response.Redirect("stafflist.aspx");
                this.ManageTab("1");
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

            this.PanelViewPhoto.Visible = true;
            this.filenames.Value = string.Empty;
            Response.Redirect("staffview.aspx?id=" + this.EncStaffId);
        }

        protected void repeaterFiles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            NameValueCollection collection = new NameValueCollection();
            collection["staffid"] = ViewState["id"].ToString();
            collection["userid"] = this.UserId;

            string deletefilename = this.ConvertToString(e.CommandArgument);
            if (string.IsNullOrEmpty(deletefilename)) return;

            BUStaff.DeleteStaffGalleryPhoto(deletefilename, collection);
            this.PopulateControls();
            this.PopulateImages();
            // this.ManageTab("1");
            Response.Redirect("staffedit.aspx?id=" + this.EncStaffId + "&tab=1");
        }

        public void ManageTab(string strTabId)
        {
            string script1 = string.Empty;
            string csname = string.Empty;
            if (!string.IsNullOrEmpty(strTabId))
            {
                switch (strTabId.Trim())
                {
                    default:
                        this.tablnkBasic.Attributes.Add("class", "active");
                        this.tablnkImages.Attributes.Remove("class");

                        this.tab_basic_details.Attributes.Add("class", "tab-pane active");
                        this.tab_images.Attributes.Add("class", "tab-pane");

                        this.basic_details_tab.Attributes.Add("class", "nav-link active");
                        this.images_tab.Attributes.Add("class", "nav-link ");

                        break;

                    case "1":

                        this.tablnkBasic.Attributes.Remove("class");
                        this.tablnkImages.Attributes.Add("class", "active");

                        this.tab_basic_details.Attributes.Add("class", "tab-pane");
                        this.tab_images.Attributes.Add("class", "tab-pane active");

                        this.basic_details_tab.Attributes.Add("class", "nav-link");
                        this.images_tab.Attributes.Add("class", "nav-link active");

                        break;
                }
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            this.ManageTab("1");
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            this.ManageTab("0");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("staffview.aspx?id=" + this.EncStaffId);
        }
    }
}