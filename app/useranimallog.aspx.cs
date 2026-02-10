using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI;

namespace Breederapp
{
    public partial class useranimallog : PageBase
    {
        protected int Language
        {
            get { return this.ConvertToInteger(Session["userlang"]); }
        }

        override protected void Page_Load(object sender, EventArgs e)
        {
            //this.IsAssociationAccess = true;
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = this.ReadQueryString("id");
                (Page.Master as breeder).AnimalId = ViewState["id"].ToString();
                this.ApplyFilter();
            }
        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("animalid", this.ConvertToString(ViewState["id"]));
            this.hidfilter.Value = AnimalBA.AnimalLogSearch(collection);
        }
    }
}