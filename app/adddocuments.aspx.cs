using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class adddocuments : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString();
                // ViewState["id"] = "1";
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            /*NameValueCollection collection = AnimalBA.GetAnimalCertificateDetail(ViewState["id"]);
            if (collection != null)
            {
                filenames.Value = collection["filename"];
                this.txtnotes.Text = collection["title"];
            }*/
        }

        protected void btnAddNote_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            NameValueCollection collection = new NameValueCollection();
            collection.Add("filename", this.filenames.Value);
            collection.Add("name", this.txtnotes.Text.Trim());

            /*AnimalBA objBreed = new AnimalBA();
            bool success = ((ViewState["id"] != null && this.ConvertToInteger(ViewState["id"]) > 0) ? objBreed.UpdateAnimalCertificate(collection, ViewState["id"]) : objBreed.AddAnimalCertificate(collection));
            if (success)
            {
                Response.Redirect("fooddetails.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }*/

        }
    }
}