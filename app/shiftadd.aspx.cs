using BABusiness;
using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Breederapp
{
    public partial class shiftadd : ERPBase
    {
        public string ShiftId
        {
            get { return BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey); }
        }

        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = this.ReadQueryString("id");
                ViewState["id"] = this.DecryptQueryString("id");
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            DateTime tempTime = new DateTime(2020, 1, 1, 0, 0, 0);
            for (int i = 0; i < 96; i++)
            {
                DateTime newTime = tempTime.AddMinutes(i * 15);
                this.ddlStartTime.Items.Add(newTime.ToString("HH:mm"));
            }

            string start = Request.QueryString["start"];
            if (!string.IsNullOrEmpty(start))
            {
                DateTime st = Convert.ToDateTime(start, CultureInfo.CurrentCulture);
                if (st != DateTime.MinValue)
                {
                    this.ddlStartTime.SelectedValue = st.ToString("HH:mm");
                }
            }

            DateTime tempTime1 = new DateTime(2020, 1, 1, 0, 0, 0);
            for (int i = 0; i < 96; i++)
            {
                DateTime newTime = tempTime1.AddMinutes(i * 15);
                this.ddlEndTime.Items.Add(newTime.ToString("HH:mm"));
            }

            string start1 = Request.QueryString["start"];
            if (!string.IsNullOrEmpty(start1))
            {
                DateTime st = Convert.ToDateTime(start, CultureInfo.CurrentCulture);
                if (st != DateTime.MinValue)
                {
                    this.ddlEndTime.SelectedValue = st.ToString("HH:mm");
                }
            }

            int id = this.ConvertToInteger(ViewState["id"]);
            if (id > 0)
            {
                NameValueCollection collection = Shift.GetShiftDetail(id, this.CompanyId);
                if (collection != null)
                {

                    this.txtName.Text = collection["shift_name"];
                    this.txtBreakTimeDuration.Text = collection["break_time_duration"];
                    //this.ddlStartTime.SelectedValue = collection["startdatetime"];
                    string startTime = collection["startdatetime"];
                    if (!string.IsNullOrEmpty(startTime))
                    {
                        DateTime st = Convert.ToDateTime(startTime, CultureInfo.CurrentCulture);
                        if (st != DateTime.MinValue)
                        {
                            this.ddlStartTime.SelectedValue = st.ToString("HH:mm");
                        }
                    }
                    //this.ddlEndTime.SelectedValue = collection["enddatetime"];
                    string endTime = collection["enddatetime"];
                    if (!string.IsNullOrEmpty(endTime))
                    {
                        DateTime et = Convert.ToDateTime(endTime, CultureInfo.CurrentCulture);
                        if (et != DateTime.MinValue)
                        {
                            this.ddlEndTime.SelectedValue = et.ToString("HH:mm");
                        }
                    }
                    this.ddlType.SelectedValue = collection["shift_typeid"];
                    this.ddlStatus.SelectedValue = collection["status"];

                    this.lblName.Text = collection["shift_name"];
                    this.lblbreaktimeduration.Text = collection["break_time_duration"];
                    this.lblStartTime.Text = collection["startdatetime"];
                    this.lblEndTime.Text = collection["enddatetime"];


                    switch (collection["shift_typeid"])
                    {
                        case "1":
                            this.lblType.Text = "Night";
                            break;

                        case "2":
                            this.lblType.Text = "Day";
                            break;

                    }

                    switch (collection["status"])
                    {
                        case "0":
                            this.lblStatus.Text = "Inactive";
                            break;

                        case "1":
                            this.lblStatus.Text = "Active";
                            break;

                    }

                }
            }
            else
            {
                this.panelView.Visible = false;
                this.lnkEdit1.Visible = false;
                this.btnBack.Visible = false;
                this.panelEdit.Visible = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;
            NameValueCollection collection = new NameValueCollection();
            collection.Add("shift_name", this.txtName.Text.Trim());
            collection.Add("break_time_duration", this.txtBreakTimeDuration.Text.Trim());
            collection.Add("startdatetime", this.ddlStartTime.SelectedValue);
            collection.Add("status", this.ddlStatus.SelectedValue);
            collection.Add("shift_typeid", this.ddlType.SelectedValue);
            collection.Add("enddatetime", this.ddlEndTime.SelectedValue);
            collection.Add("userid", this.UserId);
            collection.Add("companyid", this.CompanyId);

            bool success = false;
            int shiftid = this.ConvertToInteger(ViewState["id"]);
            if (shiftid > 0)
            {
                success = Shift.UpdateShift(collection, ViewState["id"]);
            }
            else
            {
                shiftid = Shift.AddShift(collection);
                success = (shiftid > 0);
            }

            if (success)
            {
                Response.Redirect("shiftlist.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            this.panelView.Visible = false;
            this.lnkEdit1.Visible = false;
            this.btnBack.Visible = false;
            this.panelEdit.Visible = true;

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            if (this.ConvertToInteger(ViewState["id"]) > 0)
            {
                this.panelView.Visible = true;
                this.btnBack.Visible = true;
                this.lnkEdit1.Visible = true;
                this.lnkEdit1.Text = "Edit Shift";
                this.panelEdit.Visible = false;
            }
            else
                Response.Redirect("shiftlist.aspx");
        }

        protected void btnBack1_Click(object sender, EventArgs e)
        {
            Response.Redirect("shiftlist.aspx");
        }

    }
}