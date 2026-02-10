using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class associationedit : PageBase
    {
        public string AssociationId
        {
            get { return BABusiness.BASecurity.Encrypt(ViewState["id"].ToString(), Breederapp.PageBase.HashKey); }
        }

        override protected void Page_Load(object sender, EventArgs e)
        {
            // this.IsAdminAccess = true;
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString();
                this.PopulateControls();
                this.lnkManageMemeber.HRef = "memberlist.aspx?aid=" + this.AssociationId;
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = UserBA.GetAssociation(ViewState["id"]);
            if (collection == null) Response.Redirect("manageassociation.aspx");

            this.ddlBreedType.DataSource = BreederData.GetAllBreedTypes(null);
            this.ddlBreedType.DataBind();

            this.txtName.Text = collection["name"];
            this.txtAddress.Text = collection["address"];
            this.txtWebsite.Text = collection["website"];
            this.txtPhone.Text = collection["phone"];
            this.txtEmailAddress.Text = collection["email"];
            string[] breedTypes = collection["breedtype"].Split(',');
            foreach (ListItem item in this.ddlBreedType.Items)
            {
                item.Selected = breedTypes.Contains(item.Value);
            }

            DataTable dataTable = UserBA.GetAssociatedBreeders(ViewState["id"]);
            if (dataTable != null)
            {
                DataRow row = dataTable.NewRow();
                row["id"] = 0;
                row["name"] = Resources.Resource.AddNewMember;
                dataTable.Rows.InsertAt(row, 0);
            }
            //this.repeaterMembers.DataSource = UserBA.GetAssociatedBreeders(ViewState["id"]);
            //this.repeaterMembers.DataBind();
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
            bool success = objass.UpdateAssociation(collection, ViewState["id"]);
            if (success)
            {
                Response.Redirect("manageassociation.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("manageassociation.aspx");
        }

        //protected void btnAccept_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("acceptassociate.aspx?assoid=" + ViewState["id"]);

        //}
    }
}