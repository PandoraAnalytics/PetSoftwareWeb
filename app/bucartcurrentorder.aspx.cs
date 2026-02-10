using Breederapp;
using System;
using System.Collections.Specialized;
using BABusiness;

namespace PetsSoftware.app
{
    public partial class bucartcurrentorder : ERPBase
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

            //pos
            NameValueCollection collection3 = new NameValueCollection();
            collection3.Add("companyid", this.CompanyId);
            collection3.Add("ispos", "1");
            this.hdfilterpos.Value = BUOrderManagement.SearchOrder(collection3);

            NameValueCollection collection4 = new NameValueCollection();
            collection4.Add("companyid", this.CompanyId);
            collection4.Add("currentdate", BusinessBase.Now.ToString(this.DateFormat));
            collection4.Add("ispos", "1");
            this.hdpfilterpos.Value = BUOrderManagement.SearchOrder(collection4);
        }
    }
}