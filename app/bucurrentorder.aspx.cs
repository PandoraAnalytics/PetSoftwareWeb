using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class bucurrentorder : ERPBase
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
            collection.Add("companyid", this.CompanyId);
            collection.Add("ispos", "0");
            this.hdfilter.Value = BUOrderManagement.SearchOrder(collection);

            NameValueCollection collection2 = new NameValueCollection();
            collection2.Add("companyid", this.CompanyId);
            collection2.Add("currentdate", BusinessBase.Now.ToString(this.DateFormat));
            collection2.Add("ispos", "0");
            this.hdpfilter.Value = BUOrderManagement.SearchOrder(collection2);
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("buaddneworder.aspx");
        }
    }
}