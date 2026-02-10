using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI;

namespace Breederapp
{
    public partial class ticketvodistlist : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
            }
            Control divMenu = Master.FindControl("ticketmainmenu");
            divMenu.Visible = true;
            Control divUserMenu = Master.FindControl("usermenu");
            divUserMenu.Visible = false;
            Control divChecklistMenu = Master.FindControl("checklistmainmenu");
            divChecklistMenu.Visible = false;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            NameValueCollection collection = new NameValueCollection();
            collection.Add("emailaddress", this.txtEmail.Text.Trim());

            bool success = false;
            Ticket obj = new Ticket();

            if (this.hdfilter.Value.Length == 0)
            {
                success = obj.AddVODistListEmail(collection);
            }
            else
            {
                success = obj.UpdateVODistListEmail(collection, this.hdfilter.Value);
            }
            obj = null;

            if (success)
            {
                Response.Redirect("ticketvodistlist.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }
    }
}