using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class manageassociationapproval : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            this.IsAdminAccess = true;
            //this.IsAssociationAccess = true;
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.ApplyFilters();
            }
        }

        private void ApplyFilters()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());
            this.hdfilter.Value = UserBA.AssociationSearch(collection);
        }

        private void ApproveAssociation()
        {
            this.lblMessage.Visible = false;
            this.lblMessage.Text = string.Empty;
            if (this.cplist.Value.Length == 0)
            {
                this.lblMessage.Text = Resources.Resource.NoRowsSelected;
                this.lblMessage.Visible = true;
                return;
            }

            string[] associationArray = this.cplist.Value.Split(';');

            UserBA objApprove = new UserBA();

            NameValueCollection collection = new NameValueCollection();
            foreach (string assocId in associationArray)
            {
                collection["isapprove"] = "1";
                objApprove.UpdateApprovalStatus(collection, assocId);
            }
            objApprove = null;
            this.cplist.Value = null;

            this.lblMessage.Text = Resources.Resource.ActionSuccess;
            this.lblMessage.Visible = true;
        }

        private void RejectAssociation()
        {
            this.lblMessage.Visible = false;
            this.lblMessage.Text = string.Empty;
            if (this.cplist.Value.Length == 0)
            {
                this.lblMessage.Text = Resources.Resource.NoRowsSelected;
                this.lblMessage.Visible = true;
                return;
            }

            string remark = this.txtNotAppRemark.Text.Trim();
            if (remark.Length > 255)
            {
                this.lblMessage.Text = Resources.Resource.Remarkcannotmorecharector;
                this.lblMessage.Visible = true;
                return;
            }

            string[] associationArray = this.cplist.Value.Split(';');

            UserBA objReject = new UserBA();

            NameValueCollection collection = new NameValueCollection();
            foreach (string assocId in associationArray)
            {
                collection["comments"] = remark;
                collection["isapprove"] = "2";
                objReject.UpdateApprovalStatus(collection, assocId);
            }
            objReject = null;
            this.cplist.Value = null;

            this.lblMessage.Text = Resources.Resource.ActionSuccess;
            this.lblMessage.Visible = true;
        }

        protected void btnApply1_Click(object sender, EventArgs e)
        {
            this.ApplyFilters();
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            this.RejectAssociation();
        }

        protected void btnApproval_Click(object sender, EventArgs e)
        {
            this.ApproveAssociation();
        }

    }
}