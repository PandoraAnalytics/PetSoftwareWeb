using BABusiness;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;

namespace Breederapp
{
    public partial class bubasicdetails : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString("id");  //Animal id             
                (Page.Master as bubreeder).AnimalId = ViewState["id"].ToString();
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = AnimalBA.GetAnimalDetail(ViewState["id"]);
            if (collection == null) Response.Redirect("budashboard.aspx");

            NameValueCollection collection2 = BUCustomer.GetCustomerByAnimalId(ViewState["id"], this.CompanyId);
            if (collection2 != null) ViewState["userid"] = collection2["userid"];
            else Response.Redirect("budashboard.aspx");            
            this.ddlType.DataSource = BreederData.GetBreedTypes(collection["animalcategory"]);
            this.ddlType.DataBind();          
            this.lblName.Text = collection["name"];
            this.lblType.Text = collection["typename"];
            this.lblCollarId.Text = collection["collar_id"];
            this.lblAbout.Text = this.nl2br(collection["aboutme"]);
            string selectedGender = collection["gender"];
            switch (selectedGender)
            {
                case "1":
                    lblGender.Text = "Male";
                    break;

                case "2":
                    lblGender.Text = "Female";
                    break;
            }
            this.lblHeight.Text = collection["height"].Replace(",", ".");
            this.lblWeight.Text = collection["weight"].Replace(",", ".");
            this.lblSpanCoat.Text = collection["spancoat"];          
            this.txtName.Text = collection["name"];
            this.ddlType.SelectedValue = collection["breedtype"];
            this.txtCollarId.Text = collection["collar_id"];
            this.txtAbout.Text = collection["aboutme"];
            this.hid_profile_pic.Value = collection["profilepic_file"];
            this.txtHeight.Text = collection["height"].Replace(",", ".");
            this.txtWeight.Text = collection["weight"].Replace(",", ".");           
            this.txtSpanCoat.Text = collection["spancoat"];            
            this.ddlGender.SelectedValue = collection["gender"];

            try
            {
                DateTime birthDay = Convert.ToDateTime(collection["dob"]);
                DateTime today = DateTime.Today;

                int ageYears = today.Year - birthDay.Year;
                int ageMonths = today.Month - birthDay.Month;
                if (today.Month < birthDay.Month || (today.Month == birthDay.Month && today.Day < birthDay.Day))
                {
                    ageYears--;
                    ageMonths = 12 - birthDay.Month + today.Month - 1;
                }

                string ageString = ageYears + " Year";
                if (ageYears != 1) ageString += "s";
                if (ageMonths >= 0)
                {
                    ageString += " " + ageMonths + " Month";
                    if (ageMonths != 1) ageString += "s";
                }

                this.txtDOB.Text = birthDay.ToString(this.DateFormat);
                this.lblDOB.Text = birthDay.ToString(this.DateFormat);
                this.lblAge.Text = ageString;
            }
            catch { }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            this.panelView.Visible = false;
            this.lnkEdit.Visible = false;
            this.panelEdit.Visible = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            string date = this.txtDOB.Text.Trim();
            DateTime dt = DateTime.MinValue;
            DateTime.TryParse(date, out dt);
            if (dt == DateTime.MinValue)
            {
                this.lblError.Text = Resources.Resource.Invalidate;
                return;
            }

            DateTime dobDate = Convert.ToDateTime(this.txtDOB.Text.Trim(), CultureInfo.CurrentCulture);
            DateTime nowDate = Convert.ToDateTime(BusinessBase.Now.ToString(), CultureInfo.CurrentCulture);
            if (DateTime.Compare(dobDate.Date, nowDate.Date) > 0)
            {
                this.lblError.Text = Resources.Resource.dobcannotgreaterthantoday;
                return;
            }

            NameValueCollection collection = new NameValueCollection();
            collection.Add("type", this.ddlType.SelectedValue);
            collection.Add("date", date);
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("collarid", this.txtCollarId.Text.Trim());
            collection.Add("about", this.txtAbout.Text.Trim());
            collection.Add("gender", this.ddlGender.SelectedValue);
            collection.Add("height", this.txtHeight.Text.Trim());
            collection.Add("weight", this.txtWeight.Text.Trim());         
            collection.Add("spancoat", this.txtSpanCoat.Text.Trim());            
            collection.Add("userid", this.UserId);

            AnimalBA objBreed = new AnimalBA();
            bool success = objBreed.UpdateAnimal(collection, ViewState["id"]);
            if (success)
            {
                if (!string.IsNullOrEmpty(this.hid_profile_pic.Value))
                    objBreed.UpdateAnimalProfilePic(this.hid_profile_pic.Value, ViewState["id"]);

                Response.Redirect("bubasicdetails.aspx?id=" + BASecurity.Encrypt(ViewState["id"].ToString(), PageBase.HashKey));

                this.panelView.Visible = true;
                this.lnkEdit.Visible = true;
                this.lnkEdit.Text = "<i class=\"fa-solid fa-pen-to-square\"></i>&nbsp;Edit";
                this.panelEdit.Visible = false;
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.panelView.Visible = true;
            this.lnkEdit.Visible = true;
            this.lnkEdit.Text = "<i class=\"fa-solid fa-pen-to-square\"></i>&nbsp;Edit";
            this.panelEdit.Visible = false;
        }
    }
}