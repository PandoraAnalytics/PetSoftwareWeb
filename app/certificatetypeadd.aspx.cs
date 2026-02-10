using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class certificatetypeadd : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            this.IsAssociationAccess = true;
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            if (Session["associationbreedertype"] != null)
            {
                this.ddlBreedType.DataSource = BreederData.GetAllBreedTypes(Session["associationbreedertype"]);
                this.ddlBreedType.DataBind();
            }
            else
                Response.Redirect("certificatetypelist.aspx");
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

            Certificate objCertificatetype = new Certificate();
            int fieldId = objCertificatetype.AddCertificatetype(collection);
            if (fieldId > 0)
            {
                Response.Redirect("certificatetypelist.aspx");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("certificatetypelist.aspx");
        }

    }
}