using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class transferlist : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (this.IsPostBack)
            {
                switch (this.Request["__EVENTTARGET"])
                {
                    case "approve":
                        AnimalBA.UpdateAnimalTransferApprove(this.Request["__EVENTARGUMENT"], this.UserId);
                        break;

                    case "reject":
                        AnimalBA.UpdateAnimalTransferReject(this.Request["__EVENTARGUMENT"], this.UserId);
                        break;
                }
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            NameValueCollection collection = new NameValueCollection();
            //collection.Add("name", this.txtName.Text.Trim());
            //collection.Add("userid", this.UserId);
            this.hdfilter.Value = AnimalBA.TransferSearch(collection);
        }


    }
}