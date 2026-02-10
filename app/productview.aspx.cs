using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;

namespace Breederapp
{
    public partial class productview : ERPBase
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
                this.PopulateControls();
            }
            else
            {
                switch (this.Request["__EVENTTARGET"])
                {
                    case "editstock":
                        this.EditStock(this.Request["__EVENTARGUMENT"]);
                        break;

                    case "deletestock":
                        this.DeleteStock(this.Request["__EVENTARGUMENT"]);
                        break;
                }
            }
        }

        private void PopulateControls()
        {
            this.txtStockDate.Text = BusinessBase.Now.ToString(this.DateFormat);

            NameValueCollection photocollection = BUProduct.GetGallaryDetail(ViewState["id"]);
            if (photocollection != null)
            {
                this.repPhotos.DataSource = BUProduct.GetProductPhotos(ViewState["id"]);
                this.repPhotos.DataBind();
            }

            //this.lblFinalCostCurrency.Text = this.GetCurrntBUCurrency();
            this.lblCostCurrency.Text = this.GetCurrntBUCurrency();
            this.lblWeightType.Text = "Kg";

            NameValueCollection collection = BUProduct.GetProductDetail(ViewState["id"], this.CompanyId);
            if (collection != null)
            {
                this.lblName.Text = collection["name"];
                this.lblCost.Text = Convert.ToDecimal(collection["cost"]).ToString("0.00").Replace(",", ".");
                //this.lblFinalCost.Text = collection["final_cost"];
                this.lblAboutDiscription.Text = collection["description"];
                this.lblType.Text = collection["material_type"];
                this.lblStatus.Text = collection["status"];
                this.lblBrand.Text = collection["brandname"];
                this.lblCategory.Text = collection["categoryname"];
                this.lblSize.Text = collection["size"];
                this.lblColor.Text = collection["color"];
                this.lblWeight.Text = collection["weight"];
                //this.lblStockQuentity.Text = collection["stock_quantity"];
                //this.lblOriginalQuentity.Text = collection["original_quantity"];
                this.lblThresholdStockValue.Text = collection["thresholdstockvalue"];

                this.lblTax.Text = collection["taxname"] + " ( " + collection["taxpercentage"] + "% )";
                switch (collection["status"])
                {
                    case "0":
                        this.lblStatus.Text = "Deactivate";
                        break;

                    case "1":
                        this.lblStatus.Text = "Activate";
                        break;

                }
                string Product = string.Empty;
                DataTable productTable = BUProduct.GetProductTagDetails(ViewState["id"]);
                if (productTable != null && productTable.Rows.Count > 0)
                {
                    string[] productlist = productTable.Rows.Cast<DataRow>().Select(row => row["name"].ToString()).Distinct().ToArray();
                    Product = string.Join(",", productlist);
                    this.lblproducttag.Text = Product;
                }
                if (!string.IsNullOrEmpty(collection["profileimage"]))
                {
                    this.panelProfiePic.Visible = true;
                    this.lnkProductProfilePic.HRef = "../app/viewdocument.aspx?file=" + collection["profileimage"].ToString() + "";//"docs/" + collection["profileimage"];
                }
                else
                {
                    this.panelProfiePic.Visible = false;
                }

                this.lblAvailableStock.Text = collection["availablestock"] + "/" + collection["totalstock"];
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("productedit.aspx?id=" + this.EncProductId);
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            this.panelAddStock.Visible = true;
            this.btnAddStock.Focus();
            this.ClearStockControl();
        }

        protected void btnAddStock_Click(object sender, EventArgs e)
        {
            this.lblStockError.Text = "";

            int quantity = this.ConvertToInteger(this.txtStockQuantity.Text.Trim());
            if (quantity <= 0)
            {
                this.lblStockError.Text = "Please enter quatity greater than 0.";
                return;
            }

            NameValueCollection collection = new NameValueCollection();
            collection.Add("productid", ViewState["id"].ToString());
            collection.Add("stockquan", this.txtStockQuantity.Text.Trim());
            collection.Add("date", this.txtStockDate.Text.Trim());
            collection.Add("ponumber", this.txtPONumber.Text.Trim());
            collection.Add("staffid", this.UserId);

            bool success = false;

            if (this.hdfilter.Value.Length == 0)
            {
                int stockid = BUProduct.AddProductStock(collection);
                success = (stockid > 0);
            }
            else
            {
                success = BUProduct.UpdateProductStock(collection, this.hdfilter.Value);
            }

            if (success)
            {
                this.panelAddStock.Visible = false;
                this.ClearStockControl();
                this.PopulateControls();
            }
            else
            {
                this.lblStockError.Text = Resources.Resource.error;
                return;
            }
        }

        private void ClearStockControl()
        {
            this.txtStockQuantity.Text = string.Empty;
            this.txtStockDate.Text = BusinessBase.Now.ToString(this.DateFormat);
            this.txtPONumber.Text = string.Empty;
            this.hdfilter.Value = string.Empty;
        }

        private void EditStock(string xiStockid)
        {
            this.panelAddStock.Visible = true;

            NameValueCollection scollection = BUProduct.GetProductStock(xiStockid);
            DateTime date = Convert.ToDateTime(scollection["date"]);
            if (date != DateTime.MinValue) this.txtStockDate.Text = date.ToString("dd.MM.yyyy");
            this.txtStockQuantity.Text = scollection["stockquan"];
            this.txtPONumber.Text = scollection["ponumber"];
            this.hdfilter.Value = xiStockid;
        }

        private void DeleteStock(string xiStockid)
        {
            bool stocksuccess = BUProduct.DeleteProductStock(xiStockid);
            if (stocksuccess)
            {
                this.PopulateControls();
            }
            else
            {
                this.lblStockError.Text = Resources.Resource.error;
                return;
            }

        }
    }
}