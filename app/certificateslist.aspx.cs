using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace Breederapp
{
    public partial class Certificateslist : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                // ViewState["id"] = DecryptQueryString();
                ViewState["id"] = this.ReadQueryString("id");
                (Page.Master as breeder).AnimalId = ViewState["id"].ToString();
                this.PopulateControls();
                this.ApplyFilter();
            }
        }

        private void PopulateControls()
        {
            DataTable dataTable = Certificate.GetNotProvidedMandatoryCertificates(ViewState["id"]);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                string[] array = dataTable.Rows.Cast<DataRow>().Select(row => row["type"].ToString()).ToArray();

                this.panelWarning.Visible = true;
                this.lblMandatoryCertificateNames.Text = string.Join(", ", array);
            }
        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("animalid", this.ConvertToString(ViewState["id"]));
            collection.Add("name", this.txtName.Text.Trim());

            this.hidfilter.Value = Certificate.Search(collection);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }

        protected void lnk_Click(object sender, EventArgs e)
        {
            Response.Redirect("certificateadd.aspx?animalid=" + ViewState["id"]);
        }
    }
}