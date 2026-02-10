using BABusiness;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class buregistrationform : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                this.PopulateControls();
            }
        }

        protected void PopulateControls()
        {
            DataTable dtType = UserBA.GetBusinessType();
            if (dtType != null)
            {
                DataRow row = dtType.NewRow();
                row["id"] = int.MinValue;
                row["name"] = Resources.Resource.Select;
                dtType.Rows.InsertAt(row, 0);

                this.ddlBusinessType.DataSource = dtType;
                this.ddlBusinessType.DataBind();
            }
            this.ddlPhoneCountryCode.DataSource = Common.GetAllPhoneCountryCode();
            this.ddlPhoneCountryCode.DataBind();
            this.ddlPhoneCountryCode.Items.Insert(0, new ListItem(Resources.Resource.Select, (int.MinValue).ToString()));

        }

        protected void btnSubmitRequest_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            NameValueCollection collection = new NameValueCollection();
            collection.Add("fname", this.txtFirstName.Text.Trim());
            collection.Add("lname", this.txtLastName.Text.Trim());
            collection.Add("email", this.txtEmailAddress.Text.Trim());
            collection.Add("phone", this.txtPhone.Text.Trim());
            collection.Add("address", this.txtAddress.Text.Trim());
            collection.Add("website", this.txtWebsite.Text.Trim());
            collection.Add("companyname", this.txtCompany.Text.Trim());
            collection.Add("companyshortname", this.txtShortName.Text.Trim());
            collection.Add("businesstype", this.ddlBusinessType.SelectedValue);
            collection.Add("registrationno", this.txtRegistrationNo.Text.Trim());
            collection.Add("contactcountrycode", this.ddlPhoneCountryCode.SelectedValue);

            int enquiryId = UserBA.AddBusinessEnquiry(collection);
            if (enquiryId > 0)
            {
                string[] files = this.filenames.Value.Split(',');
                if (files != null || files.Length > 0)
                {
                    NameValueCollection gcollection = new NameValueCollection();
                    gcollection["enquiryid"] = enquiryId.ToString();
                    gcollection["userid"] = this.UserId;

                    foreach (string file in files)
                    {
                        if (string.IsNullOrEmpty(file)) continue;

                        string extension = file.Substring(file.LastIndexOf('.'));
                        if (string.IsNullOrEmpty(extension)) continue;

                        extension = extension.ToLower();

                        ArrayList extensionArray = new ArrayList(5);
                        extensionArray.Add(".jpg");
                        extensionArray.Add(".gif");
                        extensionArray.Add(".png");
                        extensionArray.Add(".jpeg");
                        extensionArray.Add(".mp4");
                        extensionArray.Add(".pdf");
                        extensionArray.Add(".txt");
                        extensionArray.Add(".doc");
                        extensionArray.Add(".docx");
                        extensionArray.Add(".xls");
                        extensionArray.Add(".xlsx");

                        if (extensionArray.Contains(extension) == false) continue;

                        int fileType = int.MinValue;
                        switch (extension)
                        {
                            case ".jpg":
                            case ".gif":
                            case ".png":
                            case ".jpeg":
                                fileType = 1;
                                break;

                            case ".mp4":
                                fileType = 2;
                                break;
                        }

                        gcollection["file_name"] = file;
                        gcollection["title"] = file.Substring(file.IndexOf('_') + 1);
                        gcollection["file_type"] = fileType.ToString();

                        UserBA.AddBusinessEnquiryGallery(gcollection);
                    }

                    this.filenames.Value = string.Empty;
                }

                NameValueCollection usercollection = UserBA.GetBusinessEnquiryDetail(enquiryId);
                if (usercollection != null)
                {
                    NameValueCollection emailDetailsCollection = new NameValueCollection();
                    emailDetailsCollection.Add("name", usercollection["fname"] + " " + usercollection["lname"]);
                    emailDetailsCollection.Add("email", usercollection["email"]);
                    BreederMail.SendEmail(BreederMail.MessageType.BUSINESSUSERREQUEST, emailDetailsCollection);

                    collection.Clear();
                    collection = new NameValueCollection();
                    collection["bu_id"] = string.Empty;
                    collection["user_id"] = this.UserId;
                    collection["message_id"] = (int)UserBA.Status.NEWBUREQUEST + "";
                    collection["old_entry"] = enquiryId.ToString();
                    collection["new_entry"] = string.Empty;
                    collection["comment"] = "Business Enquiry from: " + this.txtEmailAddress.Text.Trim();
                    UserBA.AddBULog(collection);

                    this.panelParent.Visible = false;
                    this.pnlMessage.Visible = true;
                }
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }

        }
    }
}