using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class animallog : PageBase
    {
        protected int Language
        {
            get { return this.ConvertToInteger(Session["userlang"]); }
        }

        override protected void Page_Load(object sender, EventArgs e)
        {
            this.IsAssociationAccess = true;
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.PopulateControls();
                this.ApplyFilters();
            }
        }

        private void PopulateControls()
        {
            ddlCategory.DataSource = Enum.GetNames(typeof(Common.AnimalLogCategory));
            ddlCategory.DataBind();

        }

        private void ApplyFilters()
        {
            DateTime now = BusinessBase.Now.Date;

            NameValueCollection collection = new NameValueCollection();

            collection.Add("animalname", this.txtAnimalTitle.Text.Trim());
            if (this.ddlCategory.SelectedValue.Trim() == Common.AnimalLogCategory.ALL.ToString())
                collection.Add("category", string.Empty);
            else
                collection.Add("category", this.ddlCategory.Text.Trim());
            collection.Add("startdate", this.txtStartDate.Text.Trim());
            collection.Add("enddate", this.txtEndDate.Text.Trim());

            this.hidfilter.Value = AnimalBA.AnimalLogSearch(collection);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilters();
        }
    }
}