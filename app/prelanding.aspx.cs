using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class prelanding : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            int count = UserBA.GetUser_AnimalCount(string.Empty, this.UserId);
            if (count > 0) Response.Redirect("landing.aspx");

            this.lblUserName.Text = this.ConvertToString(Session["username"]);

            NameValueCollection collection = new NameValueCollection();
            collection.Add("status", "0");
            string filter = AnimalBA.TransferSearch(collection);

            int tcount = AnimalBA.GetAnimalTransferCount(filter, this.UserId);
            this.btnTransferRequest.Visible = (tcount > 0);
        }

        protected void btnCreateNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("breedadd.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session["userid"] = null;
            Session["username"] = null;
            Session["usertype"] = null;
            Session.Abandon();
            Response.Redirect("signin.aspx");
        }
    }
}