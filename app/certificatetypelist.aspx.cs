using System;

namespace Breederapp
{
    public partial class certificatetypelist : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            this.IsAssociationAccess = true;
            base.Page_Load(sender, e);
        }
    }
}