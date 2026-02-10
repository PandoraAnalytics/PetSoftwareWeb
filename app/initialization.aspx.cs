using BABusiness;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class initialization : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userid"] == null) Response.Redirect("landing..aspx");
            if (Session["companyid"] == null) Response.Redirect("dashboard.aspx");
           
            if (!this.IsPostBack)
            {
                ViewState["switch"] = Request.Url.Query;
                ViewState["backurl"] = (Request.UrlReferrer != null) ? Request.UrlReferrer.AbsoluteUri : "";
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            this.lnkBack.Visible = false;
            if (Convert.ToString(ViewState["switch"]) == "?switch")
            {
                DataTable companyTable = UserBA.GetBusinessUnitsForUser(Session["userid"], Session["email"]);
                if (companyTable != null && companyTable.Rows.Count > 0)
                {                   
                    this.repeaterCompany.DataSource = companyTable;
                    this.repeaterCompany.DataBind();
                }
                this.lnkBack.Visible = true;
            }
            else
            {
                Response.Redirect("landing..aspx");                
            }
        }

        protected void btnSwitch_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in this.repeaterCompany.Items)
            {
                HtmlInputRadioButton control = item.FindControl("radio1") as HtmlInputRadioButton;
                if (control.Checked)
                {
                    Session["companyid"] = null;
                    Session["companyid"] = control.Value;
                    Response.Redirect("budashboard.aspx");                    
                }
            }
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            string backUrl = "budashboard.aspx";
            if (string.IsNullOrEmpty(backUrl) == false) Response.Redirect(backUrl);
        }
    }
}