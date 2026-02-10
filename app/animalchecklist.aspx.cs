using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;

namespace Breederapp
{
    public partial class animalchecklist : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = this.ReadQueryString("id");
                (Page.Master as breeder).AnimalId = ViewState["id"].ToString();
                this.PopulateControls();
                this.ApplyFilter();
            }
        }

        private void PopulateControls()
        {
            DataTable dataTable = AnimalBA.GetAnimalChecklist(ViewState["id"]);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                this.panelQuestions.Visible = true;
                this.panelNoQuestion.Visible = false;
            }
        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            // collection.Add("name", this.txtName.Text.Trim());
            collection.Add("notinchecklist_animalid", ViewState["id"].ToString());
            this.hidfilter.Value = Checklist.Search(collection);
        }

        protected void btnChecklist_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            if (this.cplist.Value.Length == 0)
            {
                this.lblError.Text = Resources.Resource.Pleaseselectatleastonechecklistfromthelist;
                return;
            }

            string ckList = this.cplist.Value;
            if (ckList.StartsWith(";")) ckList = ckList.Substring(1);
            string[] ckListItems = ckList.Split(';');
            if (ckListItems == null || ckListItems.Length == 0)
            {
                this.lblError.Text = Resources.Resource.Pleaseselectatleastonechecklistfromthelist;
                return;
            }

            AnimalBA obj = new AnimalBA();

            NameValueCollection collection = new NameValueCollection();
            collection["animalid"] = ViewState["id"].ToString();
            foreach (string val in ckListItems)
            {
                if (string.IsNullOrEmpty(val)) continue;

                collection["checklistid"] = val;
                obj.AssignChecklist(collection);
            }
            obj = null;
            cplist.Value = string.Empty;
            this.PopulateControls();
        }
    }
}