using BABusiness;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class bucertificatetypelist : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {            
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                // ViewState["id"] = DecryptQueryString();               
                //ViewState["customerid"] = DecryptQueryString("cid");
                //(Page.Master as bubreeder).CustomerId = ViewState["customerid"].ToString();
            }
        }
    }
}