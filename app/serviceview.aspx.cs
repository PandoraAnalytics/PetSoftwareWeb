using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;

namespace Breederapp
{
    public partial class serviceview : ERPBase
    {
        public string EncServiceId
        {
            get { return BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey); }
        }

        public string serviceId
        {
            get { return BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey); }
        }

        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString("id");
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {            
            this.lblCostCurrency.Text = this.GetCurrntBUCurrency();

            NameValueCollection collection2 = BuServices.GetGallaryDetail(ViewState["id"]);
            if (collection2 != null)
            {
                this.repPhotos.DataSource = BuServices.GetServicePhotos(ViewState["id"]);
                this.repPhotos.DataBind();
            }

            NameValueCollection collection = BuServices.GetServiceDetail(ViewState["id"], this.CompanyId);
            if (collection != null)
            {
                this.lblName.Text = collection["name"];               
                this.lblCost.Text = Convert.ToDecimal(collection["cost"]).ToString("0.00").Replace(",", ".");
                //this.lblFinalCost.Text = collection["final_cost"];
                this.lblAboutDiscription.Text = collection["description"];
                this.lblType.Text = collection["typename"];
                this.lblTax.Text = collection["taxname"]+" ( " + collection["taxpercentage"] + "% )"; 
                switch (collection["status"])
                {
                    case "1":
                        this.lblStatus.Text = "Open";
                        break;

                    case "2":
                        this.lblStatus.Text = "Close";
                        break;

                }
                string services = string.Empty;
                DataTable serviceTable = BuServices.GetServiceTagDetails(ViewState["id"]);
                if (serviceTable != null && serviceTable.Rows.Count > 0)
                {
                    string[] servicelist = serviceTable.Rows.Cast<DataRow>().Select(row => row["name"].ToString()).Distinct().ToArray();
                    services = string.Join(",", servicelist);
                    this.lblServicetag.Text = services;
                }
                if (!string.IsNullOrEmpty(collection["profileimage"]))
                {
                    this.panelProfiePic.Visible = true;
                    this.lnkServiceProfilePic.HRef = "../app/viewdocument.aspx?file=" + collection["profileimage"].ToString() + "";//"docs/" + collection["profileimage"];
                }
                else
                {
                    this.panelProfiePic.Visible = false;
                }
            }

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("serviceedit.aspx?id=" + this.EncServiceId);
        }
    }
}
