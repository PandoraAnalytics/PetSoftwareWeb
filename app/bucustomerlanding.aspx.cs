using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BABusiness;
using System.Collections.Specialized;
using System.Data;


namespace Breederapp
{
    public partial class bucustomerlanding : ERPBase
    {
        public string EncCustomerId
        {
            get { return BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey); }
        }

        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString("cid");
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = BUCustomer.GetCustomerDetail(ViewState["id"], this.CompanyId);
            if (collection != null)
            {
                NameValueCollection usercollection = UserBA.GetUserDetail(collection["userid"]);
                this.lblCustomerName.Text = usercollection["fname"] + " " + usercollection["lname"] + " - Dashboard";
                ViewState["userid"] = collection["userid"];
            }
            else
            {
                Response.Redirect("budashboard.aspx");
            }

            DataTable dataTable = BreederData.GetBreedCategory();
            if (dataTable != null)
            {
                DataRow row = dataTable.NewRow();
                row["id"] = int.MinValue;
                row["breedname"] = Resources.Resource.Select;
                dataTable.Rows.InsertAt(row, 0);

                this.ddlCategory.DataSource = dataTable;
                this.ddlCategory.DataBind();
            }

            //NameValueCollection collection = new NameValueCollection();

            //DateTime now = BusinessBase.Now;

            //string date = "1" + "." + now.Month + "." + now.Year;
            //DateTime dt = DateTime.MinValue;
            //DateTime.TryParse(date, out dt);
            //if (dt == DateTime.MinValue) return;

            //collection.Add("startdate", dt.ToString(this.DateFormat));
            //collection.Add("enddate", dt.AddMonths(1).AddDays(-1).ToString(this.DateFormat));

            //this.hidappfilter.Value = AnimalBA.AppointmentSearch(collection);

            this.ApplyFilters();

        }

        private void ApplyFilters()
        {
            NameValueCollection collection = new NameValueCollection();
            if (this.ConvertToInteger(this.ddlCategory.SelectedValue) > 0) collection.Add("category", this.ddlCategory.SelectedValue);
            collection.Add("name", this.txtName.Text.Trim());
            //collection.Add("custid", this.ConvertToString(ViewState["userid"]));
            this.hdfilter.Value = UserBA.Search(collection);

            
        }

        protected void lnkAddBreed_Click(object sender, EventArgs e)
        {
            Response.Redirect("bubreedadd.aspx?cid=" + this.EncCustomerId);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilters();
        }
    }
}