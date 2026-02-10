using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI;

namespace Breederapp
{
    public partial class checklistcategory : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            this.IsAdminAccess = true;
            base.Page_Load(sender, e);
            Control divMenu = Master.FindControl("checklistmainmenu");
            Control divUserMenu = Master.FindControl("usermenu");
            divUserMenu.Visible = false;
            divMenu.Visible = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());

            bool success = false;
            Checklist objCheck = new Checklist();

            if (this.hdfilter.Value.Length == 0)
            {
                success = objCheck.AddChecklistCategory(collection);
            }
            else
            {
                success = objCheck.UpdateChecklistCategory(collection, this.hdfilter.Value);
            }

            if (success)
            {
                Response.Redirect("checklistcategory.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }
    }
}