using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI;
namespace Breederapp
{
    public partial class bufoodlist : ERPBase
    {       
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString("id");
                (Page.Master as bubreeder).AnimalId = ViewState["id"].ToString();               
                this.ApplyFilter();
            }
        }

        private void ApplyFilter()
        {
            NameValueCollection collection2 = BUCustomer.GetCustomerByAnimalId(ViewState["id"], this.CompanyId);
            if (collection2 != null) ViewState["userid"] = collection2["userid"];
            else Response.Redirect("budashboard.aspx");

            NameValueCollection collection = new NameValueCollection();
            collection.Add("animalid", this.ConvertToString(ViewState["id"]));
            collection.Add("companyid", this.CompanyId); 
            collection.Add("description", this.txtName.Text.Trim());
            this.hdfilter.Value = AnimalBA.FoodSearch(collection);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }

        protected void lnk_Click(object sender, EventArgs e)
        {
            Response.Redirect("buaddfood.aspx?animalid=" + BASecurity.Encrypt(ViewState["id"].ToString(), PageBase.HashKey));
        }
    }
}