using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class associationadd : PageBase
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
            this.ddlBreedType.DataSource = BreederData.GetAllBreedTypes(null);
            this.ddlBreedType.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("address", this.txtAddress.Text.Trim());
            collection.Add("website", this.txtWebsite.Text.Trim());
            collection.Add("phone", this.txtPhone.Text.Trim());
            collection.Add("email", this.txtEmailAddress.Text.Trim());
            collection.Add("createdby", this.UserId);

            string breedtypesIds = string.Empty;
            foreach (ListItem item in this.ddlBreedType.Items)
            {
                if (item.Selected == false) continue;
                if (breedtypesIds.Length > 0) breedtypesIds += ",";
                breedtypesIds += item.Value;
            }
            collection["breedtype"] = breedtypesIds;

            UserBA objass = new UserBA();
            int fieldId = objass.AddAssociation(collection);
            if (fieldId > 0)
            {
                Session["isassociation"] = 1;
                if (!string.IsNullOrEmpty(breedtypesIds)) Session["associationbreedertype"] = breedtypesIds.TrimEnd(',').Trim();

                Response.Redirect("manageassociation.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }

            //Member objMember = new Member();
            //NameValueCollection memberCollection = new NameValueCollection();
            //memberCollection.Add("memberid", this.UserId);
            //memberCollection.Add("association_id", this.ConvertToString(fieldId));
            //int retId = objMember.AddAssociationMembers(memberCollection);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("manageassociation.aspx");
        }

    }
}