using System;
using System.Web.UI;

namespace Breederapp
{
    public partial class assignedchecklist : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            Control divMenu = Master.FindControl("checklistmainmenu");
            divMenu.Visible = true;

            Control divUserMenu = Master.FindControl("usermenu");
            divUserMenu.Visible = false;
        }
    }
}