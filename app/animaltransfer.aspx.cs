using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI;

namespace Breederapp
{
    public partial class animaltransfer : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["animalid"] = this.ReadQueryString("id");
                (Page.Master as breeder).AnimalId = ViewState["animalid"].ToString();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection animalCollection = AnimalBA.GetAnimalDetail(ViewState["animalid"]);
            if (animalCollection == null) Response.Redirect("landing.aspx");

            this.txtDate.Text = BusinessBase.Now.ToString(this.DateFormat);
        }

        protected void btnTransferAnimal_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            bool requestpending = AnimalBA.IsPendingTransferRequest(ViewState["animalid"]);
            if (requestpending)
            {
                this.lblError.Text = Resources.Resource.TransferRequestPendingError;
                return;
            }

            int transfer_userid = UserBA.GetUserIdFromEmailAddress(this.txtEmail.Text.Trim());
            if (transfer_userid <= 0)
            {
                this.lblError.Text = Resources.Resource.EmailNotRegisteredError;
                return;
            }

            NameValueCollection collection = new NameValueCollection();
            collection.Add("transfer_date", txtDate.Text.Trim());
            collection.Add("userid_old", this.UserId);
            collection.Add("userid_new", transfer_userid.ToString());
            collection.Add("files", this.filenames.Value);
            collection.Add("email", this.txtEmail.Text.Trim());
            collection.Add("animalid", ViewState["animalid"].ToString());
            collection.Add("userid", this.UserId);

            AnimalBA objtran = new AnimalBA();
            bool success = objtran.AddAnimalTransfer(collection);

            if (success)
            {
                this.txtDate.Text = string.Empty;
                this.txtEmail.Text = string.Empty;
                this.filenames.Value = string.Empty;

                this.lblError.Text = Resources.Resource.ActionSuccess;
                Response.Redirect("landing.aspx");
                return;
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }

        }
    }
}