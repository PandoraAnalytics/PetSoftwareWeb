using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;

namespace Breederapp
{
    public partial class combolist : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.PopulateControls();
                this.ApplyFilter();
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

            if (this.ConvertToInteger(dsMaster.Tables[16].Rows[0]["cnt"]) > 0)//product
            {
                this.productYes.Visible = true;
                this.productNo.Visible = false;
            }
            else
            {
                this.productYes.Visible = false;
                this.productNo.Visible = true;
                checkisAllTrue = false;
            }
           
            if (this.ConvertToInteger(dsMaster.Tables[17].Rows[0]["cnt"]) > 0)//service
            {
                this.serviceYes.Visible = true;
                this.serviceNo.Visible = false;
            }
            else
            {
                this.serviceYes.Visible = false;
                this.serviceNo.Visible = true;
                checkisAllTrue = false;
            }

            if (!checkisAllTrue)
                this.panelChecklist.Visible = true;
            else
                this.panelChecklist.Visible = false;

            this.lblCostCurrency.Text = this.GetCurrntBUCurrency();
        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("companyid", this.CompanyId);
            collection.Add("title", this.txtTitle.Text.Trim());
            this.hdfilter.Value = BUProduct.SearchCombo(collection);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("comboadd.aspx");
        }

        protected void lnkTax_Click(object sender, EventArgs e)
        {
            Response.Redirect("bumanagetax.aspx");
        }

        protected void lnkProduct_Click(object sender, EventArgs e)
        {
            Response.Redirect("productlist.aspx");
        }

        protected void lnkService_Click(object sender, EventArgs e)
        {
            Response.Redirect("serviceslist.aspx");
        }
    }
}