using BABusiness;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class buappointmentview : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString("id");//date id
                ViewState["userid"] = DecryptQueryString("uid");//user id
                if (ViewState["refurl"] == null && Request.UrlReferrer != null)
                {
                    ViewState["refurl"] = Request.UrlReferrer.ToString();
                }

                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = AnimalBA.GetAnimalAppointmentDetaillByDate(ViewState["id"], this.ConvertToString(ViewState["userid"]));
            if (collection == null) Response.Redirect("budashboard.aspx");

            ViewState["animalid"] = collection["animalid"];
            (Page.Master as bubreeder).AnimalId = ViewState["animalid"].ToString();

            NameValueCollection collection2 = BUCustomer.GetCustomerByAnimalId(ViewState["animalid"], this.CompanyId);
            if (collection2 != null) ViewState["userid"] = collection2["userid"];
            else Response.Redirect("budashboard.aspx");

            try
            {
                DateTime startDate = Convert.ToDateTime(collection["startdatetime"]);
                if (startDate != DateTime.MinValue) this.lblDate.Text = startDate.ToString(this.DateTimeFormat);
            }
            catch { }

            this.lblContact.Text = collection["profession_name"] + " - " + collection["contact_name"] + " [" + collection["contact_email"] + " - " + collection["contact_phone"] + "]";


            this.lblAnimal.Text = collection["animal_name"] + " - " + collection["typename"];
            this.txtToDo.Text = collection["todo_text"];
            this.txtMeditation.Text = collection["resultsandmedication"];

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("filename");
            dataTable.Columns.Add("file_type");

            ViewState["filenames"] = collection["filenames"];

            if (!string.IsNullOrEmpty(collection["filenames"]))
            {
                foreach (string filenameval in collection["filenames"].Split(','))
                {
                    DataRow r = dataTable.NewRow();
                    r["filename"] = filenameval;
                    //
                    string extension = string.Empty;
                    string fileVal = Convert.ToString(filenameval);
                    if (!string.IsNullOrEmpty(fileVal))
                    {
                        string filePath = @"docs/" + fileVal;
                        extension = System.IO.Path.GetExtension(filePath);

                        switch (extension)
                        {
                            case ".pdf":
                            case ".doc":
                            case ".docx":
                            case ".xls":
                            case ".xlsx":
                                extension = "1";
                                break;

                            default:
                                extension = "2";
                                break;

                        }
                    }
                    r["file_type"] = extension;
                    //
                    dataTable.Rows.Add(r);
                }
            }

            DataTable fileTable = AnimalBA.GetdAnimalAppointmentFiles(collection["id"]);
            if (fileTable != null)
            {
                foreach (DataRow row in fileTable.Rows)
                {
                    DataRow r = dataTable.NewRow();
                    r["filename"] = row["file"];

                    //
                    string extension = string.Empty;
                    string fileVal = Convert.ToString(row["file"]);
                    if (!string.IsNullOrEmpty(fileVal))
                    {
                        string filePath = @"docs/" + fileVal;
                        extension = System.IO.Path.GetExtension(filePath);

                        switch (extension)
                        {
                            case ".pdf":
                            case ".doc":
                            case ".docx":
                            case ".xls":
                            case ".xlsx":
                                extension = "1";
                                break;

                            default:
                                extension = "2";
                                break;

                        }
                    }
                    r["file_type"] = extension;
                    //

                    dataTable.Rows.Add(r);
                }
            }

            this.repAppPhotos.DataSource = dataTable;
            this.repAppPhotos.DataBind();
        }

        private void BackToPage()
        {
            string refUrl = this.ConvertToString(ViewState["refurl"]);
            if (!string.IsNullOrEmpty(refUrl))
            {
                Response.Redirect(refUrl);
            }
            else
            {
                Response.Redirect("budashboard.aspx");
            }
        }

        private bool Save()
        {
            ArrayList files = new ArrayList();

            string filenames = this.ConvertToString(ViewState["filenames"]);
            if (!string.IsNullOrEmpty(filenames)) files.AddRange(filenames.Split(','));

            string filenames1 = this.filenames.Value.Trim();
            if (!string.IsNullOrEmpty(filenames1)) files.AddRange(filenames1.Split(','));

            files.ToArray().Distinct();


            NameValueCollection collection = new NameValueCollection();
            collection.Add("todo_text", this.txtToDo.Text.Trim());
            collection.Add("resultsandmedication", this.txtMeditation.Text.Trim());
            collection.Add("filenames", string.Join(",", files.ToArray()));

            collection.Add("animalid", this.ConvertToString(ViewState["animalid"]));
            collection.Add("userid", this.UserId);

            AnimalBA objBreed = new AnimalBA();
            bool success = objBreed.UpdateAnimalAppointmentToDo(collection, ViewState["id"]);

            objBreed = null;

            return success;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            bool success = this.Save();
            if (success)
            {
                this.BackToPage();
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            this.BackToPage();
        }

        protected void repeaterFiles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string deletefilename = this.ConvertToString(e.CommandArgument);
            if (string.IsNullOrEmpty(deletefilename)) return;

            AnimalBA.DeleteAppointmentPhoto(deletefilename);
            this.PopulateControls();
        }
    }
}