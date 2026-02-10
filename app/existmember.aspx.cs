using BABusiness;
using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Breederapp
{
    public partial class existmember : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {

                ViewState["id"] = this.DecryptQueryString("mid");

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";
            DateTime today = DateTime.Now;

            DateTime dobDate = Convert.ToDateTime(this.txtExitDate.Text.Trim(), CultureInfo.CurrentCulture);
            DateTime nowDate = Convert.ToDateTime(BusinessBase.Now.ToString(), CultureInfo.CurrentCulture);
            if (DateTime.Compare(dobDate.Date, nowDate.Date) > 0)
            {
                this.lblError.Text = Resources.Resource.exitdtcannotgreaterthantoday;
                return;
            }

            NameValueCollection collection = new NameValueCollection();
            collection.Add("exitdate", this.txtExitDate.Text.Trim());
            collection.Add("exitreason", this.txtExitReason.Text.Trim());
            Member objMember = new Member();
            bool success = objMember.ExitMemeber(collection, Session["email"], ViewState["id"]);
            if (success)
            {
                Response.Redirect("manageassociation.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }

        }
    }
}