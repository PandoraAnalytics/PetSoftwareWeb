using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;

namespace Breederapp
{
    public partial class landing : PageBase
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
            //int count = UserBA.GetUser_AnimalCount(string.Empty, this.UserId);
            //if (count <= 0) Response.Redirect("prelanding.aspx");

            DataTable dataTable = BreederData.GetBreedCategory();
            if (dataTable != null)
            {
                DataRow row = dataTable.NewRow();
                row["id"] = int.MinValue;
                row["breedname"] = Resources.Resource.Select;
                dataTable.Rows.InsertAt(row, 0);

                this.ddlCategory.DataSource = dataTable;
                this.ddlCategory.DataBind();
            }

            NameValueCollection collection = new NameValueCollection();
            // collection.Add("animalid", this.ConvertToString(ViewState["id"]));
            // collection.Add("description", this.txtName.Text.Trim());

            DateTime now = BusinessBase.Now;

            string date = "1" + "." + now.Month + "." + now.Year;
            DateTime dt = DateTime.MinValue;
            DateTime.TryParse(date, out dt);
            if (dt == DateTime.MinValue) return;

            collection.Add("startdate", dt.ToString(this.DateFormat));
            collection.Add("enddate", dt.AddMonths(1).AddDays(-1).ToString(this.DateFormat));

            this.hidappfilter.Value = AnimalBA.AppointmentSearch(collection);

            this.ApplyFilters();

        }

        private void ApplyFilters()
        {
            NameValueCollection collection = new NameValueCollection();
            if (this.ConvertToInteger(this.ddlCategory.SelectedValue) > 0) collection.Add("category", this.ddlCategory.SelectedValue);
            collection.Add("name", this.txtName.Text.Trim());
            this.hdfilter.Value = UserBA.Search(collection);
        }

        protected void lnkAddBreed_Click(object sender, EventArgs e)
        {
            Response.Redirect("breedadd.aspx");
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilters();
        }
    }
}