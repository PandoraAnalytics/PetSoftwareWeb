using BABusiness;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class certificateadd : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                // ViewState["id"] = DecryptQueryString();
                ViewState["animalid"] = this.ReadQueryString("animalid");
                (Page.Master as breeder).AnimalId = ViewState["animalid"].ToString();
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = AnimalBA.GetAnimalDetail(ViewState["animalid"]);
            if (collection == null) Response.Redirect("landing.aspx");

            this.ddlType.DataSource = Certificate.GetCertificateTypes(collection["breedtype"]);
            this.ddlType.DataBind();
            this.ddlType.Items.Insert(0, new ListItem(Resources.Resource.Select, (int.MinValue).ToString()));
        }

        protected void btnAddCertificate_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            if (this.hid_certificate_pic.Value.Length == 0)
            {
                this.lblError.Text = Resources.Resource.FileRequiredError;
                return;
            }

            if (this.txtEndDate.Text.Trim().Length > 0)
            {
                Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();

                DateTime certificateStartDate = Convert.ToDateTime(this.txtStartDate.Text.Trim(), CultureInfo.CurrentCulture);
                DateTime certificateEndDate = Convert.ToDateTime(this.txtEndDate.Text.Trim(), CultureInfo.CurrentCulture);
                if (DateTime.Compare(certificateEndDate, certificateStartDate) < 0)
                {
                    this.lblError.Text = Resources.Resource.Enddateshouldbegreaterthanstartdate;
                    return;
                }
            }

            NameValueCollection typecollection = Certificate.GetCertificatetype(this.ddlType.SelectedValue);
            if (typecollection == null)
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }


            string approve = null;
            if (typecollection["approval"] == "1")
            {
                approve = "0";
            }
            else
            {
                approve = "1";
            }
            typecollection = null;


            NameValueCollection collection = new NameValueCollection();
            collection.Add("startdate", this.txtStartDate.Text.Trim());
            collection.Add("enddate", this.txtEndDate.Text.Trim());
            //collection.Add("files", this.filenames.Value);
            collection.Add("certificate_name", this.txtCertificateName.Text.Trim());
            collection.Add("animalid", ViewState["animalid"].ToString());
            collection.Add("type", this.ddlType.SelectedValue);
            collection.Add("status", approve); //.ToString());

            collection.Add("userid", this.UserId);

            Certificate objCertificate = new Certificate();
            int cId = objCertificate.AddCertificate(collection);
            if (cId > 0)
            {
                objCertificate.UpdateCertificatePic(this.hid_certificate_pic.Value, cId);

                Response.Redirect("certificateslist.aspx?id=" + ViewState["animalid"].ToString());
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }

        }

        protected void ddlType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            this.panelWarning.Visible = false;
            NameValueCollection typecollection = Certificate.GetCertificatetype(this.ddlType.SelectedValue);
            if (typecollection != null)
            {
                this.panelWarning.Visible = (typecollection["approval"] == "1");
                typecollection = null;
            }
        }
    }
}