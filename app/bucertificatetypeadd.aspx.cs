using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class bucertificatetypeadd : ERPBase
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
            this.ddlBreedType.DataSource = BreederData.GetAllBreedTypes(string.Empty);
            this.ddlBreedType.DataBind();
            
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
            collection.Add("companyid", this.CompanyId); // BUID
            Certificate objCertificatetype = new Certificate();
            int fieldId = objCertificatetype.AddCertificatetype(collection);
            if (fieldId > 0)
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