using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;

namespace Breederapp
{
    public partial class reportallanimals : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            this.IsAdminAccess = true;
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            DataTable dataTable = BreederData.GetAllBreedTypes(null); ;
            if (dataTable != null)
            {
                DataRow row = dataTable.NewRow();
                row["id"] = int.MinValue;
                row["namewithbreedname"] = (" - " + Resources.Resource.BreedType + " - ");
                dataTable.Rows.InsertAt(row, 0);

                this.ddlBreedType.DataSource = dataTable;

            }
            this.ddlBreedType.DataBind();
        }

        protected void btnApply1_Click(object sender, EventArgs e)
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("animalname", this.txtName.Text.Trim());
            collection.Add("username", this.txtUserName.Text.Trim());
            if (this.ConvertToInteger(this.ddlCategory.SelectedValue) > 0) collection.Add("animalcategory", this.ddlCategory.SelectedValue);
            if (this.ConvertToInteger(this.ddlBreedType.SelectedValue) > 0) collection.Add("breedtypes", this.ddlBreedType.SelectedValue);
            if (this.ConvertToInteger(this.ddlUserType.SelectedValue) > 0) collection.Add("usertype", this.ddlUserType.SelectedValue);

            this.hdfilter.Value = AnimalBA.AllAnimalReportSearch(collection);
        }
    }
}