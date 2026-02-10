using BABusiness;
using System;
using System.Threading;

namespace Breederapp
{
    public partial class ordermanagement : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();
                DateTime currentDate = BusinessBase.Now;
                this.hid_filter.Value = currentDate.AddMonths(-1).ToString(this.DateFormat) + "," + currentDate.ToString(this.DateFormat);
            }           
        }
    }
}