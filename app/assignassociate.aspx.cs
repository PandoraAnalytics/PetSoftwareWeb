using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class assignassociate : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            //this.IsAdminAccess = true;
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                string id = DecryptQueryString("assoid"); ;
                if (string.IsNullOrEmpty(id)) Response.Redirect("manageassociation.aspx");

                ViewState["id"] = id;
            }
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
                this.lblError.Text = "Please select at least one breeder from the list.";
                return;
            }

            string ckList = this.cplist.Value;
            if (ckList.StartsWith(";")) ckList = ckList.Substring(1);
            string[] ckListItems = ckList.Split(';');
            if (ckListItems == null || ckListItems.Length == 0)
            {
                this.lblError.Text = "Please select at least one breeder from the list.";
                return;
            }

            UserBA obj = new UserBA();

            NameValueCollection collection = new NameValueCollection();
            collection["asso_id"] = ViewState["id"].ToString();
            foreach (string val in ckListItems)
            {
                if (string.IsNullOrEmpty(val)) continue;

                collection["userid"] = val;
                obj.AddBreederToAssociation(collection);
            }
            obj = null;
            Response.Redirect("associationedit.aspx?" + BASecurity.Encrypt(ViewState["id"].ToString(), PageBase.HashKey));
        }


        protected void lnkBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("associationedit.aspx?" + BASecurity.Encrypt(ViewState["id"].ToString(), PageBase.HashKey));
        }

    }
}