using BABusiness;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class productedit : ERPBase
    {
        public string EncProductId
        {
            get { return BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey); }
        }

        public string productId
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
            this.ddlBrand.DataSource = BUProduct.GetProductBrand(this.CompanyId);
            this.ddlBrand.DataBind();
            this.ddlCategory.DataSource = BUProduct.GetProductCategory(this.CompanyId);
            this.ddlCategory.DataBind();

            //this.lblFinalCostCurrency.Text = this.GetCurrntBUCurrency();
            this.lblCostCurrency.Text = this.GetCurrntBUCurrency();
            this.lblWeightType.Text = "Kg";

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

            NameValueCollection collection = BUProduct.GetProductDetail(ViewState["id"], this.CompanyId);
            if (collection != null)
            {
                this.PanelViewPhoto.Visible = true;
                this.txtName.Text = collection["name"];
                this.txtCost.Text = collection["cost"];
                //this.txtFinalCost.Text = collection["final_cost"];
                this.txtAboutDiscription.Text = collection["description"];
                this.txtMaterialType.Text = collection["material_type"];
                this.ddlStatus.SelectedValue = collection["status"];
                this.ddlBrand.SelectedValue = collection["brand_id"];
                this.ddlCategory.SelectedValue = collection["category_id"];
                this.txtSize.Text = collection["size"];
                this.txtColor.Text = collection["color"];
                this.txtWeight.Text = collection["weight"];
                //this.txtStockQuentity.Text = collection["stock_quantity"];
                //this.txtOriginalQuentity.Text = collection["original_quantity"];
                this.txtThresholdStockValue.Text = collection["thresholdstockvalue"];
                this.ddlTax.SelectedValue = collection["taxid"];

                string Product = string.Empty;
                DataTable productTable = BUProduct.GetProductTagDetails(ViewState["id"]);
                if (productTable != null && productTable.Rows.Count > 0)
                {
                    string[] productlist = productTable.Rows.Cast<DataRow>().Select(row => row["name"].ToString()).Distinct().ToArray();
                    Product = string.Join(",", productlist);
                    this.txtproducttags.Text = Product;
                }
                if (!string.IsNullOrEmpty(collection["profileimage"]))
                {
                    this.hid_product_pic.Value = collection["profileimage"].ToString();
                    this.lnkProductProfilePic.HRef = "../app/viewdocument.aspx?file=" + collection["profileimage"].ToString() + "";//"docs/" + collection["profileimage"];
                }
            }
            else
            {
                this.PanelViewPhoto.Visible = false;
            }
        }

        private void PopulateImages()
        {
            NameValueCollection photocollection = BUProduct.GetGallaryDetail(ViewState["id"]);
            if (photocollection != null)
            {
                this.repPhotos.DataSource = BUProduct.GetProductPhotos(ViewState["id"]);
                this.repPhotos.DataBind();
            }
            else
            {
                this.PanelViewPhoto.Visible = false;
            }
        }

        protected void btnSaveProduct_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("product_code", BABusiness.User.GetRandomPassword());
            collection.Add("cost", this.txtCost.Text.Trim());
            collection.Add("final_cost", "0");
            collection.Add("status", this.ddlStatus.SelectedValue);
            collection.Add("material_type", this.txtMaterialType.Text.Trim());
            collection.Add("brand_id", this.ddlBrand.SelectedValue);
            collection.Add("category_id", this.ddlCategory.SelectedValue);
            collection.Add("description", this.txtAboutDiscription.Text.Trim());
            collection.Add("userid", this.UserId);
            collection.Add("size", this.txtSize.Text.Trim());
            collection.Add("color", this.txtColor.Text.Trim());
            collection.Add("weight", this.txtWeight.Text.Trim());
            //collection.Add("stock_quantity", this.txtStockQuentity.Text.Trim());
            //collection.Add("original_quantity", this.txtOriginalQuentity.Text.Trim());
            collection.Add("thresholdstockvalue", this.txtThresholdStockValue.Text.Trim());
            collection.Add("profileimage", this.hid_product_pic.Value.Trim());
            collection.Add("companyid", this.CompanyId);
            collection.Add("taxid", this.ddlTax.SelectedValue);

            bool success = false;
            int productid = this.ConvertToInteger(ViewState["id"]);
            if (productid > 0)
            {
                success = BUProduct.UpdateProduct(collection, ViewState["id"]);
                if (success)
                {

                    bool success1 = BUProduct.DeleteTags(ViewState["id"]);
                    if (!string.IsNullOrEmpty(this.txtproducttags.Text.Trim()))
                    {
                        string[] tags = this.txtproducttags.Text.Split(',');
                        if (tags != null || tags.Length > 0)
                        {
                            foreach (string tag in tags)
                            {
                                if (string.IsNullOrEmpty(tag)) continue;

                                NameValueCollection collection1 = new NameValueCollection();
                                collection1.Add("product_id", productid.ToString());
                                collection1.Add("name", tag);
                                int tagid = BUProduct.AddProductTag(collection1);
                            }
                        }
                    }
                    //add log here
                    collection = new NameValueCollection();
                    collection["product_id"] = productid.ToString();
                    collection["user_id"] = this.UserId;
                    collection["message_id"] = (int)BUProduct.Status.PRODUCTEDIT + "";
                    collection["old_entry"] = string.Empty;
                    collection["new_entry"] = string.Empty;
                    collection["comment"] = this.txtName.Text.Trim();
                    BUProduct.AddProductLog(collection);

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
                this.lblPhotoError.Text = Resources.Resource.error;
                return;
            }
            NameValueCollection collection = new NameValueCollection();
            collection["productid"] = ViewState["id"].ToString();
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

                BUProduct.AddProductGallery(collection);
            }

            this.PanelViewPhoto.Visible = true;
            this.filenames.Value = string.Empty;
            Response.Redirect("productview.aspx?id=" + BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey));
        }

        protected void repeaterFiles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            NameValueCollection collection = new NameValueCollection();
            collection["productid"] = ViewState["id"].ToString();
            collection["userid"] = this.UserId;

            string deletefilename = this.ConvertToString(e.CommandArgument);
            if (string.IsNullOrEmpty(deletefilename)) return;

            BUProduct.DeleteProductGalleryPhoto(deletefilename, collection);
            this.PopulateControls();
            this.PopulateImages();
            // this.ManageTab("1");
            Response.Redirect("productedit.aspx?id=" + this.EncProductId + "&tab=1");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("productview.aspx?id=" + this.EncProductId);
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