using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class checklistassingusers : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString("id");
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = Checklist.GetChecklist(ViewState["id"]);
            if (collection == null) Response.Redirect("checklist.aspx");

            this.lblTitle.Text = collection["name"];
        }

        protected void btnApply1_Click(object sender, EventArgs e)
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());

            this.hdfilter.Value = UserBA.UserSearch(collection);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            if (this.cplist.Value.Length == 0)
            {
                this.lblError.Text = "Please select at least one member from the list.";
                return;
            }

            string ckList = this.cplist.Value;
            if (ckList.StartsWith(";")) ckList = ckList.Substring(1);
            string[] ckListItems = ckList.Split(';');
            if (ckListItems == null || ckListItems.Length == 0)
            {
                this.lblError.Text = "Please select at least one member from the list.";
                return;
            }

            Checklist obj = new Checklist();

            NameValueCollection collection = new NameValueCollection();
            collection["checklistid"] = ViewState["id"].ToString();
            foreach (string val in ckListItems)
            {
                if (string.IsNullOrEmpty(val)) continue;

                collection["userid"] = val;
                obj.AddUsersToChecklist(collection);
            }
            obj = null;
            Response.Redirect("checklistmanage.aspx?id=" + BASecurity.Encrypt(ViewState["id"].ToString(), PageBase.HashKey));
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("checklistmanage.aspx?id=" + BASecurity.Encrypt(ViewState["id"].ToString(), PageBase.HashKey));
        }
    }
}