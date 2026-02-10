using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;

namespace Breederapp
{
    public partial class serviceslist : ERPBase
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

            if (this.ConvertToInteger(dsMaster.Tables[13].Rows[0]["cnt"]) > 0)//service type
            {
                this.servicetypeYes.Visible = true;
                this.servicetypeNo.Visible = false;
            }
            else
            {
                this.servicetypeYes.Visible = false;
                this.servicetypeNo.Visible = true;
                checkisAllTrue = false;
            }

            if (!checkisAllTrue)
                this.panelChecklist.Visible = true;
            else
                this.panelChecklist.Visible = false;

            this.lblCostCurrency.Text = this.GetCurrntBUCurrency();
            DataTable dtType = BuServices.GetServiceType(this.CompanyId);
            if (dtType != null)
            {
                DataRow row = dtType.NewRow();
                row["id"] = int.MinValue;
                row["name"] = "- Select Type -";
                dtType.Rows.InsertAt(row, 0);

                this.ddlType.DataSource = dtType;
                this.ddlType.DataBind();
            }
            
        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("companyid", this.CompanyId);
            collection.Add("name", this.txtName.Text.Trim());
            this.hdfilter.Value = BuServices.Search(collection);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }

        protected void lnk_Click(object sender, EventArgs e)
        {
            Response.Redirect("serviceadd.aspx");
        }

        protected void lnkTax_Click(object sender, EventArgs e)
        {
            Response.Redirect("bumanagetax.aspx");
        }

        protected void lnkServiceType_Click(object sender, EventArgs e)
        {
            Response.Redirect("manageservicetype.aspx");
        }
    }
}