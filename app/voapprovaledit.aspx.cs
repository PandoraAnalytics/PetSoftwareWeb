using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class voapprovaledit : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.PopulateControls();
            }
            Control divMenu = Master.FindControl("ticketmainmenu");
            divMenu.Visible = true;
            Control divUserMenu = Master.FindControl("usermenu");
            divUserMenu.Visible = false;
            Control divChecklistMenu = Master.FindControl("checklistmainmenu");
            divChecklistMenu.Visible = false;
        }

        private void PopulateControls()
        {
            DataTable dataTable = Ticket.GetUsers();
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    string user_id = Convert.ToString(row["user_id"]);
                    this.ddlUser.Items.Add(new ListItem(row["user_full_name"].ToString(), user_id));
                    this.ddlUser2.Items.Add(new ListItem(row["user_full_name"].ToString(), user_id));
                    this.ddlUser3.Items.Add(new ListItem(row["user_full_name"].ToString(), user_id));
                }
            }

            NameValueCollection collection = Ticket.GetVOApprovalMatrix();
            if (collection != null)
            {
                string[] slices = collection["matrix"].Split(',');
                if (slices != null && slices.Length > 0)
                {
                    foreach (ListItem listItem in ddlUser.Items)
                    {
                        listItem.Selected = (slices.Contains(listItem.Value));
                    }
                }

                string[] slices2 = collection["matrix2"].Split(',');
                if (slices2 != null && slices2.Length > 0)
                {
                    foreach (ListItem listItem in ddlUser2.Items)
                    {
                        listItem.Selected = (slices2.Contains(listItem.Value));
                    }
                }

                string[] slices3 = collection["matrix3"].Split(',');
                if (slices3 != null && slices3.Length > 0)
                {
                    foreach (ListItem listItem in ddlUser3.Items)
                    {
                        listItem.Selected = (slices3.Contains(listItem.Value));
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            NameValueCollection collection1 = new NameValueCollection();

            string matrix = string.Empty;
            foreach (ListItem item in this.ddlUser.Items)
            {
                if (item.Selected == false) continue;
                if (matrix.Length > 0) matrix += ",";
                matrix += item.Value;
            }
            collection1["matrix"] = matrix;

            string matrix2 = string.Empty;
            foreach (ListItem item in this.ddlUser2.Items)
            {
                if (item.Selected == false) continue;
                if (matrix2.Length > 0) matrix2 += ",";
                matrix2 += item.Value;
            }
            collection1["matrix2"] = matrix2;

            string matrix3 = string.Empty;
            foreach (ListItem item in this.ddlUser3.Items)
            {
                if (item.Selected == false) continue;
                if (matrix3.Length > 0) matrix3 += ",";
                matrix3 += item.Value;
            }
            collection1["matrix3"] = matrix3;

            if (matrix.Length == 0 && matrix2.Length == 0 && matrix3.Length == 0)
            {
                this.lblError.Text = "At least one approval level is required";
                return;
            }

            string[] matrixslices = (matrix.Length > 0) ? matrix.Split(',') : null;
            string[] matrix2slices = (matrix2.Length > 0) ? matrix2.Split(',') : null;
            string[] matrix3slices = (matrix3.Length > 0) ? matrix3.Split(',') : null;

            string[] intersect_1_2 = (matrixslices != null && matrix2slices != null) ? matrixslices.Intersect(matrix2slices).ToArray() : null;
            string[] intersect_1_3 = (matrixslices != null && matrix3slices != null) ? matrixslices.Intersect(matrix3slices).ToArray() : null;
            string[] intersect_2_3 = (matrix2slices != null && matrix3slices != null) ? matrix2slices.Intersect(matrix3slices).ToArray() : null;

            if (
                (intersect_1_2 != null && intersect_1_2.Length > 0) ||
                (intersect_1_3 != null && intersect_1_3.Length > 0) ||
                (intersect_2_3 != null && intersect_2_3.Length > 0)
                )
            {
                this.lblError.Text = "Please select different User in Level 1,2 & 3";
                return;
            }

            Ticket objTickets = new Ticket();
            bool success = objTickets.SaveVOApprovalMatrix(collection1);
            objTickets = null;
            if (success)
            {
                Response.Redirect("voapprovallist.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("voapprovallist.aspx");
        }
    }
}