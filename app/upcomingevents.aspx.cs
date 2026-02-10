using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class upcomingevents : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.PopulateControls();
                this.ApplyFilters();
            }
        }

        private void PopulateControls()
        {
            this.ddlMonth.Items.Clear();
            this.ddlMonth.Items.Add(new ListItem(Resources.Resource.SelectAll, "0"));

            for (int mon = 1; mon <= 12; mon++)
            {
                this.ddlMonth.Items.Add(mon.ToString().PadLeft(2, '0'));
            }

            DateTime now = BusinessBase.Now;

            int year = now.Year;
            for (int y = year - 3; y <= year + 1; y++)
            {
                this.ddlYear.Items.Add(y.ToString());
            }

            this.ddlYear.SelectedValue = now.Year.ToString();

            DataTable dataTable = BreederData.GetBreedCategory();
            if (dataTable != null)
            {
                DataRow row = dataTable.NewRow();
                row["id"] = int.MinValue;
                row["breedname"] = Resources.Resource.Select;
                dataTable.Rows.InsertAt(row, 0);

                this.ddlSelectAnimal.DataSource = dataTable;
                this.ddlSelectAnimal.DataBind();
            }

            this.panelAddEvent.Visible = (Session["isassociation"] != null && Session["isassociation"].ToString() == "1");
        }

        private void ApplyFilters()
        {
            DateTime now = BusinessBase.Now.Date;

            NameValueCollection collection = new NameValueCollection();
            if (this.ConvertToInteger(this.ddlSelectAnimal.SelectedValue) > 0) collection.Add("animalcategory", this.ddlSelectAnimal.SelectedValue);
            if (this.ConvertToInteger(this.ddlSelectBreed.SelectedValue) > 0) collection.Add("breedtype", this.ddlSelectBreed.SelectedValue);

            string selectedMonth = this.ddlMonth.SelectedValue;
            if (selectedMonth == "0")
            {

                DateTime startDate = new DateTime(this.ConvertToInteger(this.ddlYear.SelectedValue), 1, 1);
                DateTime endDate = new DateTime(this.ConvertToInteger(this.ddlYear.SelectedValue), 12, 31);

                collection.Add("startdate", startDate.ToString(this.DateFormat));
                collection.Add("enddate", endDate.ToString(this.DateFormat));
            }
            else
            {
                string date = "1" + "." + this.ddlMonth.SelectedValue + "." + this.ddlYear.SelectedValue;
                DateTime dt = DateTime.MinValue;
                DateTime.TryParse(date, out dt);
                if (dt == DateTime.MinValue) return;
                collection.Add("startdate", dt.ToString(this.DateFormat));
                collection.Add("enddate", dt.AddMonths(1).AddDays(-1).ToString(this.DateFormat));

            }
            collection.Add("userid", this.UserId);
            this.hidfilter.Value = EventBA.EventSearch(collection);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilters();
        }

        protected void ddlSelectAnimal_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlSelectBreed.Items.Clear();

            if (this.ConvertToInteger(this.ddlSelectAnimal.SelectedValue) > 0)
            {
                this.ddlSelectBreed.DataSource = BreederData.GetBreedTypes(this.ddlSelectAnimal.SelectedValue);
                this.ddlSelectBreed.DataBind();
                this.ddlSelectBreed.Items.Insert(0, new ListItem(Resources.Resource.Select, (int.MinValue).ToString()));
            }
        }
    }
}