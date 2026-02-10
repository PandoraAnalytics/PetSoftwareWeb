using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class managecertificateapproval : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            this.IsAssociationAccess = true;
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.ApplyFilters();
            }
        }

        private void ApplyFilters()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("title", this.txtTitle.Text.Trim());
            if (Session["associationbreedertype"] != null) collection.Add("association_breedtype", this.ConvertToString(Session["associationbreedertype"]));

            this.hdfilter.Value = Certificate.SearchApprove(collection);
        }

        private void ApproveBreed()
        {
            this.lblMessage.Visible = false;
            this.lblMessage.Text = string.Empty;
            if (this.cplist.Value.Length == 0)
            {
                this.lblMessage.Text = Resources.Resource.NoRowsSelected;
                this.lblMessage.Visible = true;
                return;
            }

            string remark1 = this.txtAppRemark.Text.Trim();
            if (remark1.Length > 255)
            {
                this.lblMessage.Text = Resources.Resource.Remarkcannotmorecharector;
                this.lblMessage.Visible = true;
                return;
            }

            string[] certArray = this.cplist.Value.Split(';');

            Certificate objApprove = new Certificate();

            NameValueCollection collection = new NameValueCollection();
            foreach (string certId in certArray)
            {
                collection["comments"] = remark1;
                collection["status"] = "1";
                objApprove.UpdateStatus(collection, certId);
            }
            objApprove = null;
            this.cplist.Value = null;

            this.lblMessage.Text = Resources.Resource.ActionSuccess;
            this.lblMessage.Visible = true;
        }

        private void RejectBreed()
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

            string[] certArray = this.cplist.Value.Split(';');

            Certificate objApprove = new Certificate();

            NameValueCollection collection = new NameValueCollection();
            foreach (string certId in certArray)
            {
                collection["comments"] = remark;
                collection["status"] = "-1";
                objApprove.UpdateStatus(collection, certId);
            }
            objApprove = null;
            this.cplist.Value = null;

            this.lblMessage.Text = Resources.Resource.ActionSuccess;
            this.lblMessage.Visible = true;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilters();
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            this.RejectBreed();
        }

        protected void btnApproval_Click(object sender, EventArgs e)
        {
            this.ApproveBreed();
        }
    }
}