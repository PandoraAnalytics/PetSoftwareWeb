using BABusiness;
using System;
using System.Data;

namespace Breederapp
{
    public partial class user : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.panelAssociation.Visible = (Session["isassociation"] != null && Session["isassociation"].ToString() == "1");
            //this.panelConfig.Visible = (Session["isowner"] != null && Session["isowner"].ToString() == "1");
            this.configmenu2.Visible = (Session["isowner"] != null && Session["isowner"].ToString() == "1");

            DataTable companyTable = UserBA.GetBusinessUnitsForUser(Session["userid"], Session["email"]);
            if (companyTable != null && companyTable.Rows.Count > 0)
            {
                if (!companyTable.Columns.Contains("displayname"))
                    companyTable.Columns.Add("displayname", typeof(string));

                if (!companyTable.Columns.Contains("isenabled"))
                    companyTable.Columns.Add("isenabled", typeof(bool));

                foreach (DataRow row in companyTable.Rows)
                {
                    int status = Convert.ToInt32(row["status"]);
                    string companyName = row["companyname"].ToString();

                    if (status == 0)
                    {
                        row["displayname"] = companyName + " (Not Approved)";
                        row["isenabled"] = false; // disable click
                    }
                    else
                    {
                        row["displayname"] = companyName;
                        row["isenabled"] = true;
                    }
                }

                this.panelCompany.Visible = true;
                this.companyRepeater.DataSource = companyTable;
                this.companyRepeater.DataBind();               
            }
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

        protected void companyRepeater_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "companyclick":
                    Session["companyid"] = e.CommandArgument;
                    Response.Redirect("budashboard.aspx");
                    break;
            }
        }
    }
}