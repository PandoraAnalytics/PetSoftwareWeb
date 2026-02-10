using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Threading;
using System.Web.UI;

namespace Breederapp
{
    public partial class budashboard : ERPBase
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
            if (this.ConvertToInteger(dsMaster.Tables[2].Rows[0]["cnt"]) > 0)//currency
            {
                this.currencyYes.Visible = true;
                this.currencyNo.Visible = false;                
            }
            else
            {
                this.currencyYes.Visible = false;
                this.currencyNo.Visible = true;
                checkisAllTrue = false;
            }

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

            if(this.ConvertToInteger(dsMaster.Tables[12].Rows[0]["cnt"]) > 0)//category
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

            if (this.ConvertToInteger(dsMaster.Tables[4].Rows[0]["cnt"]) > 0)//staff department
            {
                this.departmentYes.Visible = true;
                this.departmentNo.Visible = false;
            }
            else
            {
                this.departmentYes.Visible = false;
                this.departmentNo.Visible = true;
                checkisAllTrue = false;
            }

            if (this.ConvertToInteger(dsMaster.Tables[5].Rows[0]["cnt"]) > 0)//staff jobrole
            {
                this.jobroleYes.Visible = true;
                this.jobroleNo.Visible = false;
            }
            else
            {
                this.jobroleYes.Visible = false;
                this.jobroleNo.Visible = true;
                checkisAllTrue = false;
            }

            if (!checkisAllTrue)
                this.panelChecklist.Visible = true;
            else
                this.panelChecklist.Visible = false;
        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("companyid", this.CompanyId);
            collection.Add("currentdate", BusinessBase.Now.ToString(this.DateFormat));
            collection.Add("ispos", "0");

            this.hdpfilter.Value = BUOrderManagement.SearchOrder(collection);

            NameValueCollection pcollection = new NameValueCollection();
            pcollection.Add("companyid", this.CompanyId);
            this.hdpsfilter.Value = BUProduct.Search(pcollection);
        }

        protected void lnkCurrency_Click(object sender, EventArgs e)
        {
            Response.Redirect("bumanagecurrency.aspx");
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

        protected void lnkServiceType_Click(object sender, EventArgs e)
        {
            Response.Redirect("manageservicetype.aspx");
        }

        protected void lnkStaffDepartment_Click(object sender, EventArgs e)
        {
            Response.Redirect("managestaffdepartment.aspx");
        }

        protected void lnkStaffjobrole_Click(object sender, EventArgs e)
        {
            Response.Redirect("managestaffjobrole.aspx");
        }
    }
}