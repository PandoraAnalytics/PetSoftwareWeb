using BABusiness;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class bubreeder : System.Web.UI.MasterPage
    {
        private string animalid = string.Empty;
        //private string encryptanimalid = string.Empty;
        private string customerid = string.Empty;

        public string AnimalId
        {
            get { return animalid; }
            set
            {
                animalid = value;
                this.PopulateData();
            }
        }

        public string CustomerId
        {
            get { return customerid; }
            set
            {
                customerid = value;
            }
        }

        //public string EncryptAnimalId
        //{
        //    get { return animalid; }
        //    set
        //    {

        //        encryptanimalid = BASecurity.Encrypt(value, PageBase.HashKey);
        //        this.PopulateLinks();
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            NameValueCollection collection = UserBA.GetBusinessUserDetail(Session["companyid"]);
            if (!string.IsNullOrEmpty(collection["companylogo"])) this.companyLogo.Src = "docs/" + collection["companylogo"];
            else this.companyLogo.Src = "images/logo.png";

            if (!this.IsPostBack)
            {               
                this.PopulateData();
            }
            else
            {
                string searchterm = this.searchbox.Value.Trim();
                if (searchterm.Length == 0) return;

                this.SearchBreed(searchterm);
            }

        }

        private void PopulateData()
        {
            if (string.IsNullOrEmpty(animalid))
            {
                Response.Redirect("bucustomerlanding.aspx?notfound=true");
            }

            this.lnkbasicinfo.HRef = "bubasicdetails.aspx?id=" + BASecurity.Encrypt(animalid, PageBase.HashKey);
            this.lnkotherinfo.HRef = "buotherdetails.aspx?id=" + BASecurity.Encrypt(animalid, PageBase.HashKey);
            this.lnkgallery.HRef = "bugallery.aspx?id=" + BASecurity.Encrypt(animalid, PageBase.HashKey);
            this.lnkappointment.HRef = "buappointmentlist.aspx?id=" + BASecurity.Encrypt(animalid, PageBase.HashKey);
            this.lnknotes.HRef = "bunoteslist.aspx?id=" + BASecurity.Encrypt(animalid, PageBase.HashKey);
            this.lnkfood.HRef = "bufoodlist.aspx?id=" + BASecurity.Encrypt(animalid, PageBase.HashKey);
            this.lnkcertificates.HRef = "bucertificateslist.aspx?id=" + BASecurity.Encrypt(animalid, PageBase.HashKey);


            NameValueCollection collection = AnimalBA.GetAnimalDetail(animalid);
            if (collection == null) Response.Redirect("bucustomerlanding.aspx?notfound=true");

            this.lblName.Text = collection["name"];
            this.lblType.Text = collection["typename"];
            if (!string.IsNullOrEmpty(collection["profilepic_file"])) this.profileimg.Src = "docs/" + collection["profilepic_file"];
            else this.profileimg.Src = "images/" + collection["breedimage"];

            collection = null;
        }

        private void SearchBreed(string xiBreedName)
        {
            NameValueCollection collection = AnimalBA.GetAnimalDetailByName(xiBreedName);
            if (collection == null) return;           

            Response.Redirect("bubasicdetails.aspx?id=" + BASecurity.Encrypt(collection["id"].ToString(), PageBase.HashKey));
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session["userid"] = null;
            Session["username"] = null;
            Session["usertype"] = null;
            Session["dtformat"] = null;
            Session["companyid"] = null;
            Session.Abandon();
            Response.Redirect("signin.aspx");
        }
    }
}