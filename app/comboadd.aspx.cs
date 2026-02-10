using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;

namespace Breederapp
{
    public partial class comboadd : ERPBase
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

            this.lblCostCurrency.Text = this.GetCurrntBUCurrency();
            //ViewState["costcurrency"] = "$";
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

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("companyid", this.CompanyId);
            this.hdpfilter.Value = BUProduct.Search(collection);
            this.hdsfilter.Value = BuServices.Search(collection);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            string productList = this.cplist.Value;
            string serviceList = this.cplist2.Value;

            if (productList.Length == 0 && serviceList.Length == 0)
            {
                this.lblError.Text = "Please select at least one item from the list.";
                return;
            }
            else if (productList.Length > 255)
            {
                this.lblError.Text = "The selected products exceed the allowed limit.";
            }
            else if (serviceList.Length > 255)
            {
                this.lblError.Text = "The selected services exceed the allowed limit.";
            }

            NameValueCollection collection = new NameValueCollection();
            collection.Add("companyid", this.CompanyId);
            collection.Add("title", this.txtComboName.Text.Trim());
            collection.Add("productlist", productList);
            collection.Add("servicelist", serviceList);
            collection.Add("cost", this.txtCost.Text.Trim());
            collection.Add("profileimage", this.hid_combo_pic.Value.Trim());
            collection.Add("createdby", this.UserId);
            collection.Add("taxid", this.ddlTax.SelectedValue);

            int Id = BUProduct.AddCombo(collection);
            if (Id > 0)
            {
                cplist.Value = string.Empty;
                cplist2.Value = string.Empty;
                Response.Redirect("combolist.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("combolist.aspx");
        }
    }
}