using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class manageassociation : PageBase
    {

        override protected void Page_Load(object sender, EventArgs e)
        {
            // this.IsAdminAccess = true;
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            { }
            else
            {
                switch (this.Request["__EVENTTARGET"])
                {
                    case "delete":
                        this.ProcessAdmin(this.Request["__EVENTARGUMENT"], "0");
                        break;
                }

            }

        }

        //#todo  ViewState["id"] - need associationid pending
        private void ProcessAdmin(string xiValue, string xiType)
        {
            if (string.IsNullOrEmpty(xiValue)) return;

            int memberId = Member.GetMemberIdByEmailAddress(Session["Email"], xiValue);

            if (memberId > 0)
            {
                Member.UpdateISAdminStatus(xiType, memberId, xiValue);
            }
        }

        protected void btnApply1_Click(object sender, EventArgs e)
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());
            this.hdfilter.Value = UserBA.AssociationSearch(collection);
        }

    }
}