using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace Breederapp
{
    public partial class ticketedit : PageBase
    {
        public string TicketId
        {
            get { return BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey); }
        }

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
            NameValueCollection collection = Ticket.GetTicket(ViewState["id"], this.UserId);
            if (collection == null) Response.Redirect("ticketall.aspx");

            bool islogauthor = (ConvertToInteger(Session["userid"]) == ConvertToInteger(collection["createdby"]));
            int isowner = this.ConvertToInteger(collection["isowner"]);
            bool isadmin = (isowner == 1);

            if (!islogauthor && !isadmin) Response.Redirect("ticketlist.aspx");

            int status = this.ConvertToInteger(collection["status"]);

            if (status != (int)Ticket.Status.NEW && status != (int)Ticket.Status.ESTIMATEEDT && status != (int)Ticket.Status.WAITINGFORDEVAPPROVAL) Response.Redirect("ticketlist.aspx");


            if (collection["isbug"] == "1") this.panelBug.Visible = true; // if bug then always visible
            else this.panelBug.Visible = (status == (int)Ticket.Status.NEW || status == (int)Ticket.Status.ESTIMATEEDT); // else condition based

            int[] statusArray = null;
            DataTable statusTable = Ticket.GetUserStatuses(this.UserId);
            if (statusTable != null && statusTable.Rows.Count > 0) statusArray = statusTable.Rows.Cast<DataRow>().Select(row => this.ConvertToInteger(row["status"])).ToArray();
            statusTable = null;
            if (statusArray == null) statusArray = new int[0];

            this.panelOptionalEmail.Visible = (statusArray.Contains((int)Ticket.Status.ADDOPTIONALEMAILS));

            DataTable dataTable1 = Ticket.GetApplicationsForTicket();
            if (dataTable1 != null)
            {
                DataRow row = dataTable1.NewRow();
                row["id"] = int.MinValue;
                row["name"] = Resources.Resource.SelectApplication;
                dataTable1.Rows.InsertAt(row, 0);

                this.ddlApplication.DataSource = dataTable1;
                this.ddlApplication.DataBind();
            }

            this.txtHeader.Text = collection["header"];
            this.txtDescription.Text = collection["description"];
            this.ddlApplication.SelectedValue = collection["application"];
            this.ddlBug.SelectedValue = collection["isbug"];
            this.txtOptionalEmails.Text = collection["optionalemails"];
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            NameValueCollection collection = new NameValueCollection();
            collection["header"] = this.txtHeader.Text.Trim();
            collection["description"] = this.txtDescription.Text.Trim();
            collection["application"] = this.ddlApplication.SelectedValue;
            collection["isbug"] = this.ddlBug.SelectedValue;
            collection["userid"] = this.UserId;
            if (this.txtOptionalEmails.Text.Trim().Length > 0)
            {
                string optionalemails = this.txtOptionalEmails.Text.Trim();
                foreach (string email in optionalemails.Split(','))
                {
                    string em = email.Trim();
                    if (em.Length == 0 || !BusinessBase.IsEmail(em)) continue;

                    collection.Add("optionalemails", em); // this will automatically add to collection using comma
                }
            }

            Ticket objTicket = new Ticket();
            bool success = objTicket.UpdateTicket(collection, ViewState["id"]);
            if (success)
            {
                collection = new NameValueCollection();
                collection["ticket_id"] = this.ConvertToString(ViewState["id"]);
                collection["userid"] = this.UserId;
                collection["messageid"] = (int)Ticket.Status.UPDATE + "";
                collection["oldentry"] = "-";
                collection["newentry"] = "-";
                objTicket.AddLog(collection);
                Response.Redirect("ticketlist.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ticketlist.aspx");
        }
    }
}