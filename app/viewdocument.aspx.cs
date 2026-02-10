using BABusiness;
using System;
using System.IO;

namespace Breederapp
{
    public partial class viewdocument : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ViewState["filename"] = Request.QueryString["file"];
                ViewState["hashcode"] = Request.QueryString["code"];
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            this.lblMessage.Text = string.Empty;

            if (ViewState["filename"] == null)
            {
                this.lblMessage.Text = "Sorry, the file you have requested does not exist. Make sure that you have the correct URL and the file exists.";
                return;
            }

            string filename = ViewState["filename"].ToString();
            if (string.IsNullOrEmpty(filename))
            {
                this.lblMessage.Text = "Sorry, the file you have requested does not exist. Make sure that you have the correct URL and the file exists.";
                return;
            }


            if (Session["userid"] == null)
            {
                if (ViewState["hashcode"] == null)
                {
                    this.lblMessage.Text = "Sorry, the file you have requested does not exist. Make sure that you have the correct URL and the file exists.";
                    return;
                }

                string hashcode = Convert.ToString(ViewState["hashcode"]);              
                if (hashcode != BusinessBase.FixedDocumentHashKey)
                {
                    this.lblMessage.Text = "Sorry, the file you have requested does not exist. Make sure that you have the correct URL and the file exists.";
                    return;
                }              
            }

            byte[] bytes = BusinessBase.GetSystemFileByteArray(filename);
            if (bytes == null || bytes.Length <= 0)
            {
                this.lblMessage.Text = "Sorry, the file you have requested does not exist. Make sure that you have the correct URL and the file exists.";
                return;
            }

            try
            {
                string tempfilename = Guid.NewGuid().ToString().ToLower() + "_" + filename.ToLower();
                string tempfilepath = Path.Combine(Server.MapPath("~") + @"app/docs/temp/", tempfilename);
                File.WriteAllBytes(tempfilepath, bytes);

                Response.Redirect("docs/temp/" + tempfilename);
            }
            catch
            {
                this.lblMessage.Text = "Sorry, the file you have requested does not exist. Make sure that you have the correct URL and the file exists.";
            }
        }
    }
}