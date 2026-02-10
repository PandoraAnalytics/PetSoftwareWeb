using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;


namespace Breederapp
{
    public partial class managetypeclass : PageBase
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
            DataTable dataTable = BreederData.GetBreedCategory();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                this.ddlType.DataSource = dataTable;
                this.ddlType.DataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("categoryid", this.ddlType.SelectedValue);

            bool success = false;
            AnimalBA objCRM = new AnimalBA();

            if (this.hdfilter.Value.Length == 0)
            {
                int id = objCRM.AddTypeClass(collection);
                success = (id > 0);
            }
            else
            {
                success = objCRM.UpdateTypeClass(collection, this.hdfilter.Value);
            }

            if (success)
            {
                Response.Redirect("managetypeclass.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }
    }
}