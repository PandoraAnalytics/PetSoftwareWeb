using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class comboview : ERPBase
    {
        public string EncComboId
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
                this.ApplyFilter();
            }
        }

        private void PopulateControls()
        {
            this.lblCostCurrency.Text = this.GetCurrntBUCurrency();
            //ViewState["costcurrency"] = "$";
            NameValueCollection collection = BUProduct.GetComboDetail(ViewState["id"], this.CompanyId);
            if (collection != null)
            {
                this.lblComboName.Text = collection["title"].ToString();               
                this.lblCost.Text = Convert.ToDecimal(collection["cost"]).ToString("0.00").Replace(",", ".");
                this.cplist.Value = collection["productlist"].ToString();
                this.cplist2.Value = collection["servicelist"].ToString();
                this.lblTax.Text = collection["taxname"] + " ( " + collection["taxpercentage"] + "% )";
                if (!string.IsNullOrEmpty(collection["profileimage"]))
                {
                    this.panelProfiePic.Visible = true;
                    this.lnkComboProfilePic.HRef = "../app/viewdocument.aspx?file=" + collection["profileimage"].ToString() + "";//"docs/" + collection["profileimage"];
                }               
                else
                {
                    this.panelProfiePic.Visible = false;
                }
            }
        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("companyid", this.CompanyId);
            this.hdpfilter.Value = BUProduct.Search(collection);
            this.hdsfilter.Value = BuServices.Search(collection);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("comboedit.aspx?id=" + this.EncComboId);
        }
    }
}