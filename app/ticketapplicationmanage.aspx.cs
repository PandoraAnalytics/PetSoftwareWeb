using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI;

namespace Breederapp
{
    public partial class ticketapplicationmanage : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString();
                this.PopulateControls();
            }
            Control divMenu = Master.FindControl("ticketmainmenu");
            divMenu.Visible = true;
            Control divUserMenu = Master.FindControl("usermenu");
            divUserMenu.Visible = false;
            Control divChecklistMenu = Master.FindControl("checklistmainmenu");
            divChecklistMenu.Visible = false;
        }
        private void PopulateControls()
        {
            NameValueCollection collection = Ticket.GetTicketApplication(ViewState["id"]);
            if (collection != null)
            {
                this.txtName.Text = collection["name"];
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            Ticket obj = new Ticket();

            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());

            bool success = ((ViewState["id"] != null && this.ConvertToInteger(ViewState["id"]) > 0) ? obj.UpdateTicketApplication(collection, ViewState["id"]) : obj.AddTicketApplication(collection));
            if (success)
            {
                Response.Redirect("ticketapplicationlist.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ticketapplicationlist.aspx");
        }
    }
}