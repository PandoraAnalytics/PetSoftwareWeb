using BABusiness;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

namespace Breederapp
{
    public partial class productadd : ERPBase
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

            this.ddlTax.SelectedValue = this.GetCurrntBUTax();

            this.tablnkBasic.Attributes.Add("class", "active");
            this.tablnkImages.Attributes.Remove("class");

            this.tab_basic_details.Attributes.Add("class", "tab-pane active");
            this.tab_images.Attributes.Add("class", "tab-pane");
        }

        protected void btnSaveProduct_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("product_code", BABusiness.User.GetRandomPassword());
            collection.Add("cost", this.txtCost.Text.Trim());
            collection.Add("final_cost","0");
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

            int productid = BUProduct.AddProduct(collection);
            success = (productid > 0);
            if (success)
            {
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
                ViewState["id"] = productid.ToString();
                collection = new NameValueCollection();
                collection["product_id"] = productid.ToString();
                collection["user_id"] = this.UserId;
                collection["message_id"] = (int)BUProduct.Status.PRODUCTADD + "";
                collection["old_entry"] = string.Empty;
                collection["new_entry"] = string.Empty;
                collection["comment"] = this.txtName.Text.Trim();
                BUProduct.AddProductLog(collection);

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

            this.filenames.Value = string.Empty;
            Response.Redirect("productview.aspx?id=" + BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey));
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("productlist.aspx");
        }

    }
}