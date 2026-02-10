using BABusiness;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class serviceedit : ERPBase
    {
        public string EncServiceId
        {
            get { return BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey); }
        }

        public string serviceId
        {
            get { return BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey); }
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
            //this.lblFinalCostCurrency.Text = this.GetCurrntBUCurrency();
            this.lblCostCurrency.Text = this.GetCurrntBUCurrency();

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

            NameValueCollection collection = BuServices.GetServiceDetail(ViewState["id"], this.CompanyId);
            if (collection != null)
            {
                this.txtName.Text = collection["name"];
                this.txtCost.Text = collection["cost"];
                //this.txtFinalCost.Text = collection["final_cost"];
                this.txtAboutDiscription.Text = collection["description"];
                this.ddlType.SelectedValue = collection["type_id"];
                this.ddlStatus.SelectedValue = collection["status"];
                this.ddlTax.SelectedValue = collection["taxid"];

                string services = string.Empty;
                DataTable serviceTable = BuServices.GetServiceTagDetails(ViewState["id"]);
                if (serviceTable != null && serviceTable.Rows.Count > 0)
                {
                    string[] servicelist = serviceTable.Rows.Cast<DataRow>().Select(row => row["name"].ToString()).Distinct().ToArray();
                    services = string.Join(",", servicelist);
                    this.txtServicetags.Text = services;
                }
                if (!string.IsNullOrEmpty(collection["profileimage"]))
                {
                    this.hid_service_pic.Value = collection["profileimage"].ToString();
                    this.lnkServiceProfilePic.HRef = "../app/viewdocument.aspx?file=" + collection["profileimage"].ToString() + "";//"docs/" + collection["profileimage"];
                }
            }
            else
            {
                this.PanelViewPhoto.Visible = false;
            }
        }

        private void PopulateImages()
        {
            NameValueCollection collection2 = BuServices.GetGallaryDetail(ViewState["id"]);
            if (collection2 != null)
            {
                this.repPhotos.DataSource = BuServices.GetServicePhotos(ViewState["id"]);
                this.repPhotos.DataBind();
            }
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
            collection.Add("profileimage", this.hid_service_pic.Value.Trim());
            collection.Add("taxid", this.ddlTax.SelectedValue);

            bool success = false;
            int serviceid = this.ConvertToInteger(ViewState["id"]);
            if (serviceid > 0)
            {
                success = BuServices.UpdateService(collection, ViewState["id"]);
                if (success)
                {
                    bool success1 = BuServices.DeleteTags(ViewState["id"]);
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
                    //add log here
                    collection = new NameValueCollection();
                    collection["service_id"] = serviceid.ToString();
                    collection["user_id"] = this.UserId;
                    collection["message_id"] = (int)BuServices.Status.SERVICEEDIT + "";
                    collection["old_entry"] = string.Empty;
                    collection["new_entry"] = string.Empty;
                    collection["comment"] = this.txtName.Text.Trim();
                    BuServices.AddServiceLog(collection);
                    this.ManageTab("1");
                }
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

            this.PanelViewPhoto.Visible = true;
            this.filenames.Value = string.Empty;
            //Response.Redirect("serviceedit.aspx?id=" + BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey));
            Response.Redirect("serviceview.aspx?id=" + BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey));
        }

        protected void repeaterFiles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            NameValueCollection collection = new NameValueCollection();
            collection["serviceid"] = ViewState["id"].ToString();
            collection["userid"] = this.UserId;

            string deletefilename = this.ConvertToString(e.CommandArgument);
            if (string.IsNullOrEmpty(deletefilename)) return;

            BuServices.DeleteSeviceGalleryPhoto(deletefilename, collection);
            this.PopulateControls();
            this.PopulateImages();
            //this.ManageTab("1");
            Response.Redirect("serviceedit.aspx?id=" + this.EncServiceId + "&tab=1");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("serviceview.aspx?id=" + this.EncServiceId);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            this.ManageTab("1");
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            this.ManageTab("0");
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
    }
}