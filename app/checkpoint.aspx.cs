using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI;

namespace Breederapp
{
    public partial class checkpoint : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            Control divMenu = Master.FindControl("checklistmainmenu");
            Control divUserMenu = Master.FindControl("usermenu");
            divUserMenu.Visible = false;
            divMenu.Visible = true;
        }

        protected void btnApply1_Click(object sender, EventArgs e)
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("title", this.txtName.Text.Trim());
            this.hidfilter.Value = CustomFields.Search(collection);
        }
    }
}