using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;

namespace Breederapp
{
    public partial class productlist : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.ApplyFilter();
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            bool checkisAllTrue = true;
            DataSet dsMaster = UserBA.GetBUMasterDataCount(this.CompanyId);
            if (this.ConvertToInteger(dsMaster.Tables[3].Rows[0]["cnt"]) > 0)//tax
            {
                this.taxYes.Visible = true;
                this.taxNo.Visible = false;
            }
            else
            {
                this.taxYes.Visible = false;
                this.taxNo.Visible = true;
                checkisAllTrue = false;
            }

            if (this.ConvertToInteger(dsMaster.Tables[11].Rows[0]["cnt"]) > 0)//brand
            {
                this.brandYes.Visible = true;
                this.brandNo.Visible = false;
            }
            else
            {
                this.brandYes.Visible = false;
                this.brandNo.Visible = true;
                checkisAllTrue = false;
            }
            if (this.ConvertToInteger(dsMaster.Tables[12].Rows[0]["cnt"]) > 0)//category
            {
                this.categoryYes.Visible = true;
                this.categoryNo.Visible = false;
            }
            else
            {
                this.categoryYes.Visible = false;
                this.categoryNo.Visible = true;
                checkisAllTrue = false;
            }

            if (!checkisAllTrue)
                this.panelChecklist.Visible = true;
            else
                this.panelChecklist.Visible = false;

            this.lblCostCurrency.Text = this.GetCurrntBUCurrency();

            DataTable dtBrand = BUProduct.GetProductBrand(this.CompanyId);
            if (dtBrand != null)
            {
                DataRow row = dtBrand.NewRow();
                row["id"] = int.MinValue;
                row["name"] = "- Select Brand -";
                dtBrand.Rows.InsertAt(row, 0);

                this.ddlBrand.DataSource = dtBrand;
                this.ddlBrand.DataBind();
            }

            DataTable dtCategory = BUProduct.GetProductCategory(this.CompanyId);
            if (dtCategory != null)
            {
                DataRow row = dtCategory.NewRow();
                row["id"] = int.MinValue;
                row["name"] = "- Select Category -";
                dtCategory.Rows.InsertAt(row, 0);

                this.ddlCategory.DataSource = dtCategory;
                this.ddlCategory.DataBind();
            }
        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("companyid", this.CompanyId);
            collection.Add("name", this.txtName.Text.Trim());
            if (this.ConvertToInteger(this.ddlBrand.SelectedValue) > 0) collection.Add("brand_id", this.ddlBrand.SelectedValue);
            if (this.ConvertToInteger(this.ddlCategory.SelectedValue) > 0) collection.Add("category_id", this.ddlCategory.SelectedValue);
            this.hdfilter.Value = BUProduct.Search(collection);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("productadd.aspx");
        }

        protected void lnkTax_Click(object sender, EventArgs e)
        {
            Response.Redirect("bumanagetax.aspx");
        }

        protected void lnkProductCatregory_Click(object sender, EventArgs e)
        {
            Response.Redirect("manageproductcategorytype.aspx");
        }

        protected void lnkProductBrand_Click(object sender, EventArgs e)
        {
            Response.Redirect("manageproductbrandtype.aspx");
        }
    }
}