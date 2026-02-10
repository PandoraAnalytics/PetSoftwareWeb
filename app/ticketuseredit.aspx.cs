using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class ticketuseredit : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = (Request.QueryString.Count > 0) ? Request.QueryString[0] : "0";
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
            DataSet dataSet = Ticket.GetStatuses();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                this.ddlPermission.DataSource = dataSet.Tables[0];
                this.ddlPermission.DataBind();
            }

            NameValueCollection collection = Ticket.GetUser(ViewState["id"]);
            if (collection != null)
            {
                ViewState["userid"] = collection["id"];
                this.lblFullName.Text = collection["fname"] + " " + collection["lname"];

                string status = collection["status"];
                if (string.IsNullOrEmpty(status) == false)
                {
                    string[] spilt = status.Split(',');
                    if (spilt != null && spilt.Length > 0)
                    {
                        foreach (ListItem item in this.ddlPermission.Items)
                        {
                            if (Array.IndexOf(spilt, item.Value) >= 0) item.Selected = true;
                        }
                    }
                }
            }
            else
            {
                Response.Redirect("ticketuserlist.aspx");
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            string pkUserId = ConvertToString(ViewState["userid"]);
            if (string.IsNullOrEmpty(pkUserId))
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }

            int i = 0;
            string[] statusArray = new string[this.ddlPermission.Items.Count];
            foreach (ListItem item in this.ddlPermission.Items)
            {
                if (item.Selected) statusArray[i++] = item.Value;
            }

            Ticket objUser = new Ticket();
            bool success = objUser.Add(statusArray, pkUserId);

            if (success)
            {
                Response.Redirect("ticketuserlist.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ticketuserlist.aspx");
        }
    }
}