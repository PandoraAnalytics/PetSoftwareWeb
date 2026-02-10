using BABusiness;
using System;
using System.Threading;
using System.Web.UI;

namespace Breederapp
{
    public partial class ticketdashboard : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();
                DateTime currentDate = BusinessBase.Now;
                this.hid_filter.Value = currentDate.AddMonths(-1).ToString(this.DateFormat) + "," + currentDate.ToString(this.DateFormat);

                this.txtDate1.Text = currentDate.AddMonths(-1).ToString(this.DateFormat);
                this.txtDate2.Text = currentDate.ToString(this.DateFormat);
            }
            Control divMenu = Master.FindControl("ticketmainmenu");
            divMenu.Visible = true;
            Control divUserMenu = Master.FindControl("usermenu");
            divUserMenu.Visible = false;
            Control divChecklistMenu = Master.FindControl("checklistmainmenu");
            divChecklistMenu.Visible = false;
        }
    }
}