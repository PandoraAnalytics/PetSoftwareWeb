using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;

namespace Breederapp
{
    public partial class parentinfo : PageBase
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
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = AnimalBA.GetAnimalDetail(ViewState["id"]);
            if (collection == null) Response.Redirect("landing.aspx");

            DataTable table = AnimalBA.GetAllAnimalsByCategory(collection["animalcategory"], this.UserId);
            if (table != null && table.Rows.Count > 0)
            {
                string option = "<option value=\"{0}\"></option>";
                StringBuilder html = new StringBuilder();
                foreach (DataRow row in table.Rows)
                {
                    html.AppendLine(string.Format(option, row["name"].ToString()));
                }
                this.datalist.InnerHtml = html.ToString();
                this.datalist1.InnerHtml = html.ToString();
            }

            if (this.ConvertToInteger(collection["fatherid"]) > 0) this.lblFathersName.Text = "<a href='basicdetails.aspx?id=" + collection["fatherid"] + "'>" + collection["fathername"] + "</a>";
            else this.lblFathersName.Text = collection["fathername"];

            if (this.ConvertToInteger(collection["motherid"]) > 0) this.lblMothersName.Text = "<a href='basicdetails.aspx?id=" + collection["motherid"] + "'>" + collection["mothername"] + "</a>";
            else this.lblMothersName.Text = collection["mothername"];

            this.txtFathersName.Value = collection["fathername"];
            this.txtMothersName.Value = collection["mothername"];
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            if (this.txtFathersName.Value.Trim().Length > 0)
            {
                NameValueCollection collection1 = AnimalBA.GetAnimalDetailByName(this.txtFathersName.Value.Trim());
                if (collection1 != null && this.ConvertToInteger(ViewState["id"]) == this.ConvertToInteger(collection1["id"]))
                {
                    this.lblError.Text = "You can't be your own parent";
                    return;
                }
            }

            if (this.txtMothersName.Value.Trim().Length > 0)
            {
                NameValueCollection collection1 = AnimalBA.GetAnimalDetailByName(this.txtMothersName.Value.Trim());
                if (collection1 != null && this.ConvertToInteger(ViewState["id"]) == this.ConvertToInteger(collection1["id"]))
                {
                    this.lblError.Text = "You can't be your own parent";
                    return;
                }
            }

            NameValueCollection collection = new NameValueCollection();
            collection.Add("fathername", this.txtFathersName.Value.Trim());
            collection.Add("mothername", this.txtMothersName.Value.Trim());
            collection.Add("userid", this.UserId);

            AnimalBA objBreed = new AnimalBA();
            bool success = objBreed.UpdateAnimalParent(collection, ViewState["id"]);
            if (success)
            {
                this.PopulateControls();

                this.panelView.Visible = true;
                this.lnkEdit.Visible = true;
                this.lnkEdit.Text = "<i class=\"fa-solid fa-pen-to-square\"></i>&nbsp;Edit";
                this.panelEdit.Visible = false;
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            this.panelView.Visible = false;
            this.lnkEdit.Visible = false;
            this.panelEdit.Visible = true;
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.panelView.Visible = true;
            this.lnkEdit.Visible = true;
            this.lnkEdit.Text = "<i class=\"fa-solid fa-pen-to-square\"></i>&nbsp;Edit";
            this.panelEdit.Visible = false;
        }
    }
}