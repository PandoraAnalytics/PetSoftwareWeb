using BABusiness;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class breeder : System.Web.UI.MasterPage
    {
        private string animalid = string.Empty;

        public string AnimalId
        {
            get { return animalid; }
            set
            {
                animalid = value;
                this.PopulateLinks();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.panelAssociation.Visible = (Session["isassociation"] != null && Session["isassociation"].ToString() == "1");
                //this.panelConfig.Visible = (Session["isowner"] != null && Session["isowner"].ToString() == "1");
                this.configmenu2.Visible = (Session["isowner"] != null && Session["isowner"].ToString() == "1");
                this.PopulateLinks();
            }
            else
            {
                string searchterm = this.searchbox.Value.Trim();
                if (searchterm.Length == 0) return;

                this.SearchBreed(searchterm);
            }
        }

        private void PopulateLinks()
        {
            if (string.IsNullOrEmpty(animalid))
            {
                Response.Redirect("landing.aspx?notfound=true");
            }

            this.lnkbasicinfo.HRef = "basicdetails.aspx?id=" + animalid;
            this.lnkparentinfo.HRef = "parentinfo.aspx?id=" + animalid;
            this.lnkotherinfo.HRef = "otherdetails.aspx?id=" + animalid;
            this.lnkgallery.HRef = "gallery.aspx?id=" + animalid;
            this.lnkappointment.HRef = "appointmentlist.aspx?id=" + animalid;
            this.lnknotes.HRef = "noteslist.aspx?id=" + animalid;
            this.lnkfood.HRef = "foodlist.aspx?id=" + animalid;
            this.lnkcertificates.HRef = "certificateslist.aspx?id=" + animalid;
            this.lnktransfer.HRef = "animaltransfer.aspx?id=" + animalid;
            this.lnkdelete.HRef = "animaldeactivate.aspx?id=" + animalid;
            this.lnkmychecklist.HRef = "animalchecklist.aspx?id=" + animalid;
            this.lnkAnimalPedigree.HRef = "pedigreereport.aspx?id=" + animalid;
            this.lnkbreederinfo.HRef = "breederinfo.aspx?id=" + animalid;


            NameValueCollection collection = AnimalBA.GetAnimalDetail(animalid);
            if (collection == null) Response.Redirect("landing.aspx?notfound=true");

            this.lblName.Text = collection["name"];
            this.lblType.Text = collection["typename"];
            //if (!string.IsNullOrEmpty(collection["profilepic_file"])) this.profileimg.Src = "docs/" + collection["profilepic_file"];
            //else this.profileimg.Src = "images/" + collection["breedimage"];

            string profile_pic = collection["profilepic_file"];
            if (string.IsNullOrEmpty(profile_pic) == false)
            {
                this.profileimg.Src = PageBase.getbase64url(profile_pic);
            }
            else
            {
                this.profileimg.Src = "images/breed-dog-selected.png";
            }

            collection = null;
        }

        private void SearchBreed(string xiBreedName)
        {
            NameValueCollection collection = AnimalBA.GetAnimalDetailByName(xiBreedName);
            if (collection == null) return;

            Response.Redirect("basicdetails.aspx?id=" + collection["id"]);
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session["userid"] = null;
            Session["username"] = null;
            Session["usertype"] = null;
            Session["dtformat"] = null;
            Session.Abandon();
            Response.Redirect("signin.aspx");
        }
    }
}