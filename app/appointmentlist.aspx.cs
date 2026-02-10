using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class appointmentlist : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = this.ReadQueryString("id");
                (Page.Master as breeder).AnimalId = ViewState["id"].ToString();

                this.PopulateControls();
                this.ApplyFilter();
            }
        }

        private void PopulateControls()
        {
            this.ddlMonth.Items.Clear();
            this.ddlMonth.Items.Add(new ListItem("All Months", "0"));

            for (int mon = 1; mon <= 12; mon++)
            {
                this.ddlMonth.Items.Add(mon.ToString().PadLeft(2, '0'));
            }

            DateTime now = BusinessBase.Now;

            int year = now.Year;
            for (int y = year - 3; y <= year + 1; y++)
            {
                this.ddlYear.Items.Add(y.ToString());
            }

            //this.ddlMonth.SelectedValue = now.Month.ToString().PadLeft(2, '0');
            this.ddlYear.SelectedValue = now.Year.ToString();

            DataTable professionTable = BreederData.GetContactProfessions();
            if (professionTable != null)
            {
                DataRow row = professionTable.NewRow();
                row["id"] = 0;
                row["name"] = "- Select Category -";
                professionTable.Rows.InsertAt(row, 0);
            }
            this.ddlProfession.DataSource = professionTable;
            this.ddlProfession.DataBind();
        }

        private void ApplyFilter()
        {
            string selectedMonth = this.ddlMonth.SelectedValue;
            NameValueCollection collection = new NameValueCollection();
            if (selectedMonth == "0")
            {

                DateTime startDate = new DateTime(this.ConvertToInteger(this.ddlYear.SelectedValue), 1, 1);
                DateTime endDate = new DateTime(this.ConvertToInteger(this.ddlYear.SelectedValue), 12, 31);

                //string startDate = "1" + "." + "01" + "." + this.ddlYear.SelectedValue;
                //string endDate = "31" + "." + "12" + "." + this.ddlYear.SelectedValue;

                //DateTime dtStart = DateTime.MinValue;
                //DateTime dtEnd = DateTime.MinValue;
                //DateTime.TryParse(startDate, out dtStart);
                //DateTime.TryParse(endDate, out dtEnd);

                collection.Add("startdate", startDate.ToString(this.DateFormat));
                collection.Add("enddate", endDate.ToString(this.DateFormat));
            }
            else
            {
                string date = "1" + "." + this.ddlMonth.SelectedValue + "." + this.ddlYear.SelectedValue;
                DateTime dt = DateTime.MinValue;
                DateTime.TryParse(date, out dt);
                if (dt == DateTime.MinValue) return;
                collection.Add("startdate", dt.ToString(this.DateFormat));
                collection.Add("enddate", dt.AddMonths(1).AddDays(-1).ToString(this.DateFormat));

            }

            collection.Add("animalid", ViewState["id"].ToString());
            collection.Add("description", txtDescription.Text.Trim());
            collection.Add("contact", txtContact.Text.Trim());
            if (this.ConvertToInteger(this.ddlProfession.SelectedValue) > 0) collection.Add("category", this.ddlProfession.SelectedValue);

            this.hidfilter.Value = AnimalBA.AppointmentSearch(collection);

        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }

        protected void lnk_Click(object sender, EventArgs e)
        {
            Response.Redirect("createappointment.aspx?id=" + ViewState["id"]);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string dateid = BASecurity.Decrypt(this.hddateid.Value, PageBase.HashKey);

            NameValueCollection appointmentcollection = AnimalBA.GetAnimalAppointmentDetaillByDate(dateid, this.UserId);
            if (appointmentcollection == null) return;

            switch (this.rdbDeleteList.SelectedValue)
            {
                case "0":
                    AnimalBA.DeleteThisAppointment(dateid);
                    break;

                case "1":
                    AnimalBA.DeleteThisAndFollowingAppointment(dateid);
                    break;

                case "2":
                    AnimalBA.DeleteAllAppointments(appointmentcollection["id"]);
                    break;
            }
        }
    }
}