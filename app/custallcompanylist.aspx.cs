using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class custallcompanylist : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.ApplyFilter();
            }
        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("custidin", this.UserId);
            collection.Add("companyname", this.txtName.Text.Trim());
            this.hdfilter.Value = UserBA.SearchCompany(collection);
        }

        protected void btnApply1_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }

    }
}