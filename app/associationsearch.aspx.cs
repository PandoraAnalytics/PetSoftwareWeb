using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;

namespace Breederapp
{
    public partial class associationsearch : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                if (ViewState["refurl"] == null && Request.UrlReferrer != null)
                {
                    ViewState["refurl"] = Request.UrlReferrer.ToString();
                }
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            DataTable dataTable = BreederData.GetAllBreedTypes(null);
            if (dataTable != null)
            {
                DataRow row = dataTable.NewRow();
                row["id"] = int.MinValue;
                row["namewithbreedname"] = Resources.Resource.Select;
                dataTable.Rows.InsertAt(row, 0);

                this.ddlBreedType.DataSource = dataTable;
                this.ddlBreedType.DataBind();
            }
        }

        private void ApplyFilters()
        {

            NameValueCollection collection = new NameValueCollection();
            if (this.ConvertToInteger(this.ddlBreedType.SelectedValue) > 0) collection.Add("breedtype", this.ddlBreedType.SelectedValue);
            collection.Add("name", this.txtName.Text.Trim());
            this.hidfilter.Value = UserBA.AssociationSearch(collection);

        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilters();
        }

        private void BackToPage()
        {
            string refUrl = this.ConvertToString(ViewState["refurl"]);
            if (!string.IsNullOrEmpty(refUrl))
            {
                Response.Redirect(refUrl);
            }
            else
            {
                Response.Redirect("landing.aspx");
            }
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            this.BackToPage();
        }
    }
}