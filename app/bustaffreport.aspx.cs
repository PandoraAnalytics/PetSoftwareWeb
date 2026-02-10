using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BABusiness;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class bustaffreport : ERPBase
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
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("email", this.txtEmail.Text.Trim());
            this.hdfilter.Value = BUStaff.Search(collection);
        }

        protected void btnApply1_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }

    }
}