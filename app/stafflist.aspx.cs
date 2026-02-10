using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;

namespace Breederapp
{
    public partial class stafflist : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.PopulateControls();
                this.ApplyFilter();
            }
        }

        private void PopulateControls()
        {
            bool checkisAllTrue = true;
            DataSet dsMaster = UserBA.GetBUMasterDataCount(this.CompanyId);

            if (this.ConvertToInteger(dsMaster.Tables[4].Rows[0]["cnt"]) > 0)//staff department
            {
                this.departmentYes.Visible = true;
                this.departmentNo.Visible = false;
            }
            else
            {
                this.departmentYes.Visible = false;
                this.departmentNo.Visible = true;
                checkisAllTrue = false;
            }

            if (this.ConvertToInteger(dsMaster.Tables[5].Rows[0]["cnt"]) > 0)//staff jobrole
            {
                this.jobroleYes.Visible = true;
                this.jobroleNo.Visible = false;
            }
            else
            {
                this.jobroleYes.Visible = false;
                this.jobroleNo.Visible = true;
                checkisAllTrue = false;
            }
            if (!checkisAllTrue)
                this.panelChecklist.Visible = true;
            else
                this.panelChecklist.Visible = false;
        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("companyid", this.CompanyId);
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("email", this.txtEmail.Text.Trim());
            this.hdfilter.Value = BUStaff.Search(collection);
        }

        protected void btnApply1_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }

        protected void lnk_Click(object sender, EventArgs e)
        {
            Response.Redirect("staffadd.aspx");
        }

        protected void lnkStaffDepartment_Click(object sender, EventArgs e)
        {
            Response.Redirect("managestaffdepartment.aspx");
        }

        protected void lnkStaffjobrole_Click(object sender, EventArgs e)
        {
            Response.Redirect("managestaffjobrole.aspx");
        }
    }
}