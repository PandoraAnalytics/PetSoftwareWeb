using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class deactivatedanimals : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.ApplyFilters();
            }
        }

        private void ApplyFilters()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("active", "2");
            this.hdfilter.Value = UserBA.Search(collection);
        }
    }
}