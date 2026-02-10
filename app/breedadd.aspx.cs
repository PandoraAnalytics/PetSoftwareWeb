using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class breedadd : PageBase
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
            DataTable dataTable = BreederData.GetBreedCategory();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                this.repeaterBreeds.DataSource = dataTable;
                this.repeaterBreeds.DataBind();
            }
        }

        protected void repeaterBreeds_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string categoryid = e.CommandArgument.ToString();

            this.panelStep2.Visible = false;
            this.datalist1.InnerHtml = string.Empty;

            if (!string.IsNullOrEmpty(categoryid))
            {
                this.panelStep2.Visible = true;
                ViewState["animalcategory"] = categoryid;

                DataTable types = BreederData.GetBreedTypes(categoryid);
                if (types != null)
                {
                    string option = "<option value=\"{0}\"></option>";
                    StringBuilder html = new StringBuilder();
                    foreach (DataRow row in types.Rows)
                    {
                        html.AppendLine(string.Format(option, row["name"].ToString()));
                    }
                    this.datalist1.InnerHtml = html.ToString();
                }
            }
        }

        protected void btnAddBreed_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            string date = this.txtDOB.Text.Trim();
            DateTime dt = DateTime.MinValue;
            DateTime.TryParse(date, out dt);
            if (dt == DateTime.MinValue)
            {
                this.lblError.Text = Resources.Resource.Invalidate;
                return;
            }
            AnimalBA objBreed = new AnimalBA();

            NameValueCollection tcollection = AnimalBA.GetBreedTypeName(this.txtType.Value.Trim());
            if (tcollection == null)
            {
                tcollection = new NameValueCollection();
                tcollection.Add("name", this.txtType.Value.Trim());
                tcollection.Add("categoryid", ViewState["animalcategory"].ToString());
                int typeid = objBreed.AddBreedType(tcollection);
                tcollection["id"] = typeid.ToString();
            }

            NameValueCollection collection = new NameValueCollection();
            collection.Add("type", tcollection["id"]);
            collection.Add("date", date);
            collection.Add("animalcategory", this.ConvertToString(ViewState["animalcategory"]));
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("collarid", this.txtCollarId.Text.Trim());
            collection.Add("gender", this.ddlGender.SelectedValue);
            collection.Add("about", this.txtAbout.Text.Trim());
            collection.Add("userid", this.UserId);

            int breedId = objBreed.AddBreed(collection);
            if (breedId > 0)
            {
                Response.Redirect("landing.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }
    }
}