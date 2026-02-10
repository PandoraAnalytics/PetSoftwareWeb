using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }

    public partial class allappointmentlist : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
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

            this.ddlMonth.SelectedValue = now.Month.ToString().PadLeft(2, '0');
            this.ddlYear.SelectedValue = now.Year.ToString();

            this.PopulateAppointments();
        }

        private void PopulateAppointments()
        {
            DateTime monthStartDate = new DateTime(int.Parse(this.ddlYear.SelectedValue), int.Parse(this.ddlMonth.SelectedValue), 1);
            DateTime monthEndDate = monthStartDate.AddMonths(1).AddDays(-1);

            NameValueCollection collection = new NameValueCollection();
            collection.Add("startdate", monthStartDate.ToString(this.DateFormat));
            collection.Add("enddate", monthEndDate.ToString(this.DateFormat));

            string filter1 = AnimalBA.AppointmentSearch(collection);

            DataTable dataTable = null;
            DataSet ds = null;
            int pageno = 1;
            do
            {
                ds = AnimalBA.GetAnimalAppointmentDetails(pageno, filter1, this.UserId);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) break;

                if (dataTable == null) dataTable = ds.Tables[0].Copy();
                else dataTable.Merge(ds.Tables[0]);
                pageno++;
            }
            while (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0);
            ds = null;

            NameValueCollection eventcollection = new NameValueCollection();
            eventcollection.Add("startdate", monthStartDate.ToString(this.DateFormat));
            eventcollection.Add("enddate", monthEndDate.ToString(this.DateFormat));
            eventcollection.Add("userid", this.UserId);
            string filter2 = EventBA.EventSearch(collection);

            DataSet eventDs = EventBA.GetEventDetails(1, filter2, this.UserId);

            DateTime firstDay = monthStartDate.StartOfWeek(DayOfWeek.Sunday);
            DateTime lastDay = monthEndDate.StartOfWeek(DayOfWeek.Sunday).AddDays(7);
            for (DateTime d = firstDay; d < lastDay; d = d.AddDays(7))
            {
                TableRow tblRow = new TableRow();
                for (int i = 0; i < 7; i++)
                {
                    DateTime actualDate = d.AddDays(i);

                    string cellText2 = string.Empty;
                    StringBuilder b = new StringBuilder();
                    TableHeaderCell tblCell = new TableHeaderCell();
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {

                        foreach (DataRow row in dataTable.Rows)
                        {
                            DateTime appointmentDate = Convert.ToDateTime(row["startdatetime"]);
                            if (actualDate.Date == appointmentDate.Date)
                            {
                                string imgSrc = string.Empty;
                            
                                if (row["profilepic_file"] != DBNull.Value && !string.IsNullOrEmpty(row["profilepic_file"].ToString())) imgSrc = PageBase.getbase64url(row["profilepic_file"].ToString());
                                else imgSrc = PageBase.getbase64url(row["breedimage"].ToString());

                                string[] names = (row["contact_name"] == DBNull.Value) ? null : row["contact_name"].ToString().Split(' ');
                                string name = string.Empty;
                                if (names != null && names.Length > 0 && names[0].Length > 1) name += names[0].Substring(0, 1);
                                if (names != null && names.Length > 1 && names[1].Length > 1) name += names[1].Substring(0, 1);

                                b.AppendLine("<li><a class='appoin' href='appointmentview.aspx?id=" + BASecurity.Encrypt(row["dateid"].ToString(), PageBase.HashKey) + "'>" + "<img src='" + imgSrc + "' alt='x' />&nbsp;" + appointmentDate.ToString("HH:mm") + " - " + row["profession_name"] + " - " + name + "</a></li>");
                            }
                        }
                    }

                    if (eventDs != null && eventDs.Tables.Count > 0 && eventDs.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in eventDs.Tables[0].Rows)
                        {
                            if (this.ConvertToInteger(row["myregistrationcount"]) <= 0) continue;

                            DateTime eventDate1 = Convert.ToDateTime(row["startdate"]);
                            DateTime eventDate2 = Convert.ToDateTime(row["enddate"]);

                            for (DateTime ed = eventDate1; ed <= eventDate2; ed = ed.AddDays(1))
                            {
                                if (actualDate.Date == ed.Date)
                                {
                                    string imgSrc = string.Empty;
                                 
                                    if (row["banner_image"] != DBNull.Value && !string.IsNullOrEmpty(row["banner_image"].ToString())) imgSrc = PageBase.getbase64url(row["banner_image"].ToString());
                                    else imgSrc = "app/images/default_banner_image.png";
                                    b.AppendLine("<li><a class='event' href='eventview.aspx?id=" + BASecurity.Encrypt(row["id"].ToString(), PageBase.HashKey) + "'>" + "<img src='" + imgSrc + "' alt='x' />&nbsp;" + ed.ToString("HH:mm") + " - " + row["title"] + "</a></li>");
                                }
                            }
                        }
                    }

                    if (b.ToString().Length > 0) cellText2 = "<ul>" + b.ToString() + "</ul>";

                    tblCell.Text = string.Format("<div class='date_row'>{0}<div>{1}</div></div>", actualDate.ToString("dd", BusinessBase.USCulture) + " " + actualDate.ToString("ddd", this.GetLangCulture()).Substring(0, 2), cellText2);

                    if (actualDate.Date < monthStartDate.Date || actualDate.Date > monthEndDate.Date) tblCell.CssClass = "notcurrentmonth";

                    tblCell.Width = Unit.Percentage(14);
                    tblRow.Cells.Add(tblCell);
                }
                tblDynamic.Rows.Add(tblRow);
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.PopulateAppointments();
        }
    }
}