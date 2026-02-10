using BABusiness;
using System;
using System.Collections.Specialized;
using System.Web.UI;

namespace Breederapp
{
    public partial class checklist : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.PopulateControls();
                this.ApplySearch();
            }

            Control divMenu = Master.FindControl("checklistmainmenu");
            Control divUserMenu = Master.FindControl("usermenu");
            divUserMenu.Visible = false;
            divMenu.Visible = true;
        }

        private void PopulateControls()
        {
            this.ddlCategory.DataSource = Checklist.GetChecklistCategory();
            this.ddlCategory.DataBind();
            this.ddlCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem(Resources.Resource.Select, (int.MinValue).ToString()));
        }


        protected void btnApply1_Click(object sender, EventArgs e)
        {
            this.ApplySearch();
        }

        private void ApplySearch()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());
            if (this.ConvertToInteger(this.ddlCategory.SelectedValue) > 0) collection.Add("categoryid", this.ddlCategory.SelectedValue);
            this.hidfilter.Value = Checklist.Search(collection);
        }
    }
}