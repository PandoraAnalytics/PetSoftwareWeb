using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BABusiness;
using System.Collections;
using System.Collections.Specialized;


namespace Breederapp
{
    public partial class bugallery : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {                
                ViewState["id"] = DecryptQueryString("id");  // Animalid
                (Page.Master as bubreeder).AnimalId = ViewState["id"].ToString();
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = AnimalBA.GetAnimalDetail(ViewState["id"]);
            if (collection == null) Response.Redirect("budashboard.aspx");

            NameValueCollection collection2 = BUCustomer.GetCustomerByAnimalId(ViewState["id"],this.CompanyId);
            if (collection2 != null) ViewState["userid"] = collection2["userid"];           
            else Response.Redirect("budashboard.aspx");
           
            this.repPhotos.DataSource = AnimalBA.GetCompanyAnimalGalleryPhotos(ViewState["id"], this.CompanyId);//gallery for company id
            this.repPhotos.DataBind();

            string script1 = "loadLazyImages();";
            string csname = "loadLazyImages";
            ClientScriptManager cs = Page.ClientScript;
            Type cstype = this.GetType();
            if (!cs.IsStartupScriptRegistered(cstype, csname))
            {
                cs.RegisterStartupScript(cstype, csname, script1, true);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            string[] files = this.filenames.Value.Split(',');
            if (files == null || files.Length == 0)
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
            NameValueCollection collection = new NameValueCollection();
            collection["animalid"] = ViewState["id"].ToString();
            collection["userid"] = this.UserId;           
            collection["companyid"] = this.CompanyId;// BUID

            foreach (string file in files)
            {
                if (string.IsNullOrEmpty(file)) continue;

                string extension = file.Substring(file.LastIndexOf('.'));
                if (string.IsNullOrEmpty(extension)) continue;

                extension = extension.ToLower();

                ArrayList extensionArray = new ArrayList(5);
                extensionArray.Add(".jpg");
                extensionArray.Add(".gif");
                extensionArray.Add(".png");
                extensionArray.Add(".jpeg");
                extensionArray.Add(".mp4");

                if (extensionArray.Contains(extension) == false) continue;

                int fileType = int.MinValue;
                switch (extension)
                {
                    case ".jpg":
                    case ".gif":
                    case ".png":
                    case ".jpeg":
                        fileType = 1;
                        break;

                    case ".mp4":
                        fileType = 2;
                        break;
                }

                collection["file_name"] = file;
                collection["title"] = file.Substring(file.IndexOf('_') + 1);
                collection["file_type"] = fileType.ToString();

                AnimalBA.AddAnimalGallery(collection);
            }

            this.panelView.Visible = true;
            this.lnkEdit.Visible = true;
            this.lnkEdit.Text = "<i class=\"fa-solid fa-pen-to-square\"></i>&nbsp;Edit";
            this.panelEdit.Visible = false;

            this.filenames.Value = string.Empty;

            this.PopulateControls();
        }

        protected void repeaterFiles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            NameValueCollection collection = new NameValueCollection();
            collection["animalid"] = ViewState["id"].ToString();
            collection["userid"] = this.UserId;

            string deletefilename = this.ConvertToString(e.CommandArgument);
            if (string.IsNullOrEmpty(deletefilename)) return;

            AnimalBA.DeleteAnimalGalleryPhoto(deletefilename, collection);
            this.PopulateControls();
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            this.panelView.Visible = false;
            this.lnkEdit.Visible = false;
            this.panelEdit.Visible = true;
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.panelView.Visible = true;
            this.lnkEdit.Visible = true;
            this.lnkEdit.Text = "<i class=\"fa-solid fa-pen-to-square\"></i>&nbsp;Edit";
            this.panelEdit.Visible = false;
        }
    }
}