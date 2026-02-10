using BABusiness;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

namespace Breederapp
{
    public partial class serviceadd : ERPBase
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
           
            this.lblCostCurrency.Text = this.GetCurrntBUCurrency();

            this.tablnkBasic.Attributes.Add("class", "active");
            this.tablnkImages.Attributes.Remove("class");

            this.tab_basic_details.Attributes.Add("class", "tab-pane active");
            this.tab_images.Attributes.Add("class", "tab-pane");

            this.ddlType.DataSource = BuServices.GetServiceType(this.CompanyId);
            this.ddlType.DataBind();

            DataTable dtTax = BUProduct.GetAllBUTax(this.CompanyId);
            if (dtTax != null)
            {
                DataRow row = dtTax.NewRow();
                row["id"] = int.MinValue;
                row["name"] = Resources.Resource.Select;
                dtTax.Rows.InsertAt(row, 0);

                this.ddlTax.DataSource = dtTax;
                this.ddlTax.DataBind();
            }

            this.ddlTax.SelectedValue = this.GetCurrntBUTax();
        }

        protected void btnSaveService_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("services_code", BABusiness.User.GetRandomPassword());
            collection.Add("cost", this.txtCost.Text.Trim());
            collection.Add("final_cost", "0");
            collection.Add("status", this.ddlStatus.SelectedValue);
            collection.Add("type_id", this.ddlType.SelectedValue);
            collection.Add("description", this.txtAboutDiscription.Text.Trim());
            collection.Add("userid", this.UserId);
            collection.Add("companyid", this.CompanyId);
            collection.Add("profileimage", this.hid_service_pic.Value.Trim());
            collection.Add("taxid", this.ddlTax.SelectedValue);

            bool success = false;

            int serviceid = BuServices.AddService(collection);
            success = (serviceid > 0);
            if (success)
            {
                if (!string.IsNullOrEmpty(this.txtServicetags.Text.Trim()))
                {
                    string[] tags = this.txtServicetags.Text.Split(',');
                    if (tags != null || tags.Length > 0)
                    {
                        foreach (string tag in tags)
                        {
                            if (string.IsNullOrEmpty(tag)) continue;

                            NameValueCollection collection1 = new NameValueCollection();
                            collection1.Add("service_id", serviceid.ToString());
                            collection1.Add("name", tag);
                            int tagid = BuServices.AddServiceTag(collection1);
                        }
                    }
                }

                ViewState["id"] = serviceid.ToString();
                collection = new NameValueCollection();
                collection["service_id"] = serviceid.ToString();
                collection["user_id"] = this.UserId;
                collection["message_id"] = (int)BuServices.Status.SERVICEADD + "";
                collection["old_entry"] = string.Empty;
                collection["new_entry"] = string.Empty;
                collection["comment"] = this.txtName.Text.Trim();
                BuServices.AddServiceLog(collection);

                this.tablnkBasic.Attributes.Remove("class");
                this.tablnkImages.Attributes.Add("class", "active");

                this.tab_basic_details.Attributes.Add("class", "tab-pane");
                this.tab_images.Attributes.Add("class", "tab-pane active");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void btnSavePhoto_Click(object sender, EventArgs e)
        {
            this.lblPhotoError.Text = string.Empty;

            string[] files = this.filenames.Value.Split(',');
            if (files == null || files.Length == 0)
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
            NameValueCollection collection = new NameValueCollection();
            collection["serviceid"] = ViewState["id"].ToString();
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

                BuServices.AddServiceGallery(collection);
            }

            this.filenames.Value = string.Empty;
            Response.Redirect("serviceview.aspx?id=" + BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey));
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("serviceslist.aspx");
        }
    }
}