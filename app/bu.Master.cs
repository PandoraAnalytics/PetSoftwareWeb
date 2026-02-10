using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class bu : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {           
            NameValueCollection collection = UserBA.GetBusinessUserDetail(Session["companyid"]);
            if (!string.IsNullOrEmpty(collection["companylogo"])) this.companyLogo.Src = PageBase.getbase64url(collection["companylogo"]); //"docs/" + collection["companylogo"];
            else this.companyLogo.Src = "images/defcomplogo2.png";

            this.lblProcessOrderCount.Text = BUOrderManagement.GetBUProcessingOrderCount(Session["companyid"]).ToString();
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session["userid"] = null;
            Session["username"] = null;
            Session["usertype"] = null;
            Session["dtformat"] = null;
            Session["companyid"] = null;
            Session.Abandon();
            Response.Redirect("signin.aspx");
        }       
    }
}