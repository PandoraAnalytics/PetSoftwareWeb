using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace Breederapp
{
    public partial class ticketadd : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
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

            DataTable dtPriority = Ticket.GetAllPriority();
            if (dtPriority != null)
            {
                DataRow row = dtPriority.NewRow();
                row["id"] = int.MinValue;
                row["priorityname"] = Resources.Resource.SelectPriority;
                dtPriority.Rows.InsertAt(row, 0);

                this.ddlPriority.DataSource = dtPriority;
                this.ddlPriority.DataBind();
            }

            int[] statusArray = null;
            DataTable statusTable = Ticket.GetUserStatuses(this.UserId);
            if (statusTable != null && statusTable.Rows.Count > 0) statusArray = statusTable.Rows.Cast<DataRow>().Select(row => this.ConvertToInteger(row["status"])).ToArray();
            statusTable = null;
            if (statusArray == null) statusArray = new int[0];

            this.panelOptionalEmail.Visible = (statusArray.Contains((int)Ticket.Status.ADDOPTIONALEMAILS));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            NameValueCollection userCollection = Ticket.GetUser(this.UserId);
            if (userCollection == null)
            {
                this.lblError.Text = Resources.Resource.Youdonthavepermissiontoaddnewticket;
                return;
            }

            NameValueCollection collection = new NameValueCollection();
            collection["description"] = this.txtDescription.Text.Trim();
            collection["header"] = this.txtHeader.Text.Trim();
            collection["userid"] = this.UserId;
            collection["priority"] = this.ddlPriority.SelectedValue;
            collection["application"] = this.ddlApplication.SelectedValue;
            collection["isbug"] = this.ddlBug.SelectedValue;
            collection["voneeded"] = string.Empty;

            if (this.panelOptionalEmail.Visible && this.txtOptionalEmails.Text.Trim().Length > 0)
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
            int ticketId = objTicket.Add(collection);

            if (ticketId > 0)
            {
                NameValueCollection emailDetailsCollection = new NameValueCollection();
                emailDetailsCollection.Add("ticketid", ticketId.ToString());
                //emailDetailsCollection.Add("clientid", this.ClientId);
                emailDetailsCollection.Add("userid", this.UserId);
                BreederMail.SendEmail(BreederMail.MessageType.TICKETADDNEW, emailDetailsCollection);

                collection.Clear();
                collection = new NameValueCollection();
                collection["ticket_id"] = ticketId.ToString();
                collection["userid"] = this.UserId;
                collection["messageid"] = (int)Ticket.Status.NEW + "";
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

    }
}