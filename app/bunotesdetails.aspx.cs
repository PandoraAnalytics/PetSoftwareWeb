using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class bunotesdetails : ERPBase
    {

        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["animalid"] = DecryptQueryString("animalid");
                ViewState["id"] = this.DecryptQueryString("id"); // noteid - edit case
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = null;
            int animalid = this.ConvertToInteger(ViewState["animalid"]);
            if (animalid > 0)
            {
                // add
                NameValueCollection animalCollection = AnimalBA.GetAnimalDetail(animalid);
                if (animalCollection == null) Response.Redirect("budashboard.aspx");
                this.txtDate.Text = BusinessBase.Now.ToString(this.DateFormat);

                this.lnkEdit_Click(null, null);
            }
            else
            {
                // edit
                int id = this.ConvertToInteger(ViewState["id"]);
                if (id <= 0) Response.Redirect("budashboard.aspx");

                collection = AnimalBA.GetAnimalNotesDetail(id);
                if (collection == null) Response.Redirect("budashboard.aspx");

                ViewState["animalid"] = collection["animalid"];
            }

            animalid = this.ConvertToInteger(ViewState["animalid"]);
            if (animalid <= 0) Response.Redirect("budashboard.aspx");

            (Page.Master as bubreeder).AnimalId = animalid.ToString();



            // Edit -- Fill data
            if (collection != null)
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(collection["submitdate"]);
                    if (dt != DateTime.MinValue)
                    {
                        this.txtDate.Text = dt.ToString(this.DateFormat);
                        this.lblDate.Text = dt.ToString(this.DateFormat);
                    }
                }
                catch { }
                this.txtNotes.Text = collection["description"];
                this.lblDescription.Text = this.nl2br(collection["description"]);

                this.PopulateFiles();
            }
        }

        private void PopulateFiles()
        {
            if (ViewState["id"] != null)
            {
                this.repNotesPhotos.DataSource = AnimalBA.GetAnimalNotes_FilesDetails(ViewState["id"]);
                this.repNotesPhotos.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            NameValueCollection collection = new NameValueCollection();
            collection.Add("date", this.txtDate.Text.Trim());
            collection.Add("note", this.txtNotes.Text.Trim());
            collection.Add("animalid", ViewState["animalid"].ToString());
            collection.Add("userid", this.UserId);
            collection.Add("companyid", this.CompanyId); // BUID
            AnimalBA objBreed = new AnimalBA();
            bool success = false;

            if (this.ConvertToInteger(ViewState["id"]) <= 0)
            {
                int noteId = objBreed.AddAnimalNotes(collection);
                if (noteId > 0)
                {
                    ViewState["id"] = noteId;
                    success = true;
                }
            }
            else
            {
                success = objBreed.UpdateAnimalNotes(collection, ViewState["id"]);
            }

            if (success)
            {
                if (!string.IsNullOrEmpty(this.filenames.Value))
                {
                    string[] files = this.filenames.Value.Split(',');
                    collection.Clear();
                    collection["noteid"] = ViewState["id"].ToString();
                    foreach (string file in files)
                    {
                        if (string.IsNullOrEmpty(file)) continue;
                        collection["file"] = file;
                        AnimalBA.AddAnimalNotes_Files(collection);
                    }
                }

                Response.Redirect("bunoteslist.aspx?id=" + BASecurity.Encrypt(ViewState["animalid"].ToString(), PageBase.HashKey));
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }

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

        protected void repeaterFiles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string deletefilename = this.ConvertToString(e.CommandArgument);
            if (string.IsNullOrEmpty(deletefilename)) return;

            AnimalBA.DeleteAnimalNotes_Files(deletefilename);
            this.PopulateFiles();
        }

    }
}