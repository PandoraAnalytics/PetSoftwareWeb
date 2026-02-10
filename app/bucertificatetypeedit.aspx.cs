using BABusiness;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI.WebControls;


namespace Breederapp
{
    public partial class bucertificatetypeedit : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {            
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString();
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = Certificate.GetCertificatetype(ViewState["id"]);
            if (collection == null) Response.Redirect("bucertificatetypelist.aspx");

            //if (Session["associationbreedertype"] != null)
            //{
            //string associationbreedertype = this.ConvertToString(Session["associationbreedertype"]);

            //if (!string.IsNullOrEmpty(associationbreedertype) && !string.IsNullOrEmpty(collection["breedtype"]))
            //{
            //    string[] a_breedTypes1 = associationbreedertype.Split(',');
            //    string[] breedTypes1 = collection["breedtype"].Split(',');

            //    string[] intersect = a_breedTypes1.Intersect(breedTypes1).ToArray();
            //    if (intersect == null || intersect.Length == 0) Response.Redirect("bucertificatetypelist.aspx");
            //}

            this.ddlBreedType.DataSource = BreederData.GetAllBreedTypes(string.Empty);
            this.ddlBreedType.DataBind();

            this.txtCertificateType.Text = collection["type"];
            this.ddlMandatory.SelectedValue = collection["ismandatory"];
            this.ddlApproval.SelectedValue = collection["approval"];
            string[] BreedTypes = collection["breedtype"].Split(',');
            foreach (ListItem item in this.ddlBreedType.Items)
            {
                item.Selected = BreedTypes.Contains(item.Value);
            }
            //}
            //else
            //    Response.Redirect("bucertificatetypelist.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("type", this.txtCertificateType.Text.Trim());
            collection.Add("ismandatory", this.ddlMandatory.SelectedValue);
            collection.Add("approval", this.ddlApproval.SelectedValue);

            string breedtypesIds = string.Empty;
            foreach (ListItem item in this.ddlBreedType.Items)
            {
                if (item.Selected == false) continue;
                if (breedtypesIds.Length > 0) breedtypesIds += ",";
                breedtypesIds += item.Value;
            }
            collection["breedtype"] = breedtypesIds;

            Certificate objbreedtypeslds = new Certificate();
            bool success = objbreedtypeslds.UpdateCertificatetype(collection, ViewState["id"].ToString());

            if (success)
            {
                Response.Redirect("bucertificatetypelist.aspx");
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("bucertificatetypelist.aspx");
        }
    }
}