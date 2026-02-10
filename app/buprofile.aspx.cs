using BABusiness;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class buprofile : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                DataSet dsMaster = UserBA.GetBUMasterDataCount(this.CompanyId);
                if (this.ConvertToInteger(dsMaster.Tables[2].Rows[0]["cnt"]) <= 0) Response.Redirect("budashboard.aspx");//currency
                if (this.ConvertToInteger(dsMaster.Tables[3].Rows[0]["cnt"]) <= 0) Response.Redirect("budashboard.aspx");//tax

                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            this.ddlCountry.DataSource = Common.GetCountries();
            this.ddlCountry.DataBind();
            this.ddlCountry.Items.Insert(0, new ListItem(Resources.Resource.Select, (int.MinValue).ToString()));

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

            DataTable dtCurrency = BUProduct.GetAllBUCurency(this.CompanyId);
            if (dtCurrency != null)
            {
                DataRow row = dtCurrency.NewRow();
                row["id"] = int.MinValue;
                row["name"] = Resources.Resource.Select;
                dtCurrency.Rows.InsertAt(row, 0);

                this.ddlCurrency.DataSource = dtCurrency;
                this.ddlCurrency.DataBind();
            }

            DataTable dtTax = BUProduct.GetAllBUTax(this.CompanyId);
            if (dtTax != null)
            {
                DataRow row = dtTax.NewRow();
                row["id"] = int.MinValue;
                row["name"] = Resources.Resource.Select;
                dtTax.Rows.InsertAt(row, 0);

                this.ddlTax.DataSource = dtTax;
                this.ddlTax.DataBind();
            }
            NameValueCollection photocollection = UserBA.GetBusinessUserGalleryDetail(this.CompanyId);
            if (photocollection != null)
            {
                this.repPhotos.DataSource = UserBA.GetBusinessDocuments(this.CompanyId);
                this.repPhotos.DataBind();
            }

            this.ddlBUPhoneCountryCode.DataSource = Common.GetAllPhoneCountryCode();
            this.ddlBUPhoneCountryCode.DataBind();
            this.ddlBUPhoneCountryCode.Items.Insert(0, new ListItem(Resources.Resource.Select, (int.MinValue).ToString()));

            NameValueCollection collection = UserBA.GetBusinessUserDetail(this.CompanyId);
            if (collection == null) Response.Redirect("budashboard.aspx");

            this.lblName.Text = collection["fname"] + " " + collection["lname"];
            this.lblEmailAddress.Text = collection["email"];
            this.lblPhone.Text = collection["userphoneprefix"] + " " + collection["phone"];

            this.txtCompany.Text = collection["companyname"];
            this.txtShortName.Text = collection["companyshortname"];
            this.txtWebsite.Text = collection["website"];
            this.ddlBusinessType.SelectedValue = collection["businesstype"];
            this.txtRegistrationNo.Text = collection["registrationno"];
            this.txtAddress.Text = collection["address"];

            if (!String.IsNullOrEmpty(collection["countryid"]))
            {
                this.ddlCountry.SelectedValue = collection["countryid"];
            }
            this.txtCity.Text = collection["city"];
            this.txtPincode.Text = collection["pincode"];

            if (!string.IsNullOrEmpty(collection["dateofincorporation"]))
            {
                DateTime incdate = Convert.ToDateTime(collection["dateofincorporation"]);
                this.txtIncorporationDate.Text = incdate.ToString(this.DateFormat);
            }

            this.txtTinNo.Text = collection["tinno"];
            this.txtLicenceNo.Text = collection["licenceno"];
            this.txtEmployerId.Text = collection["employeridno"];
            this.txtAboutBusiness.Text = collection["description"];

            if (!string.IsNullOrEmpty(collection["companylogo"]))
            {
                this.hid_company_logo.Value = collection["companylogo"].ToString();
                this.lnkCompanyLogo.HRef = "../app/viewdocument.aspx?file=" + collection["companylogo"].ToString() + "";//PageBase.getbase64url(collection["companylogo"]);
            }

            this.ddlCurrency.SelectedValue = collection["currencyid"];
            this.ddlTax.SelectedValue = collection["taxid"];
            this.txtTermCondition.Text = collection["termscondition"];

            this.txtBUEmailAddress.Text = collection["buemail"];
            this.txtBUPhone.Text = collection["buphone"];
            this.ddlBUPhoneCountryCode.SelectedValue = collection["bucontactcountrycode"];
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            NameValueCollection collection = new NameValueCollection();
            collection.Add("companyname", this.txtCompany.Text.Trim());
            collection.Add("companyshortname", this.txtShortName.Text.Trim());
            collection.Add("website", this.txtWebsite.Text.Trim());
            collection.Add("businesstype", this.ddlBusinessType.SelectedValue);
            collection.Add("registrationno", this.txtRegistrationNo.Text.Trim());
            collection.Add("address", this.txtAddress.Text.Trim());
            collection.Add("countryid", this.ddlCountry.SelectedValue);
            collection.Add("city", this.txtCity.Text.Trim());
            collection.Add("pincode", this.txtPincode.Text.Trim());
            collection.Add("dateofincorporation", this.txtIncorporationDate.Text.Trim());
            collection.Add("tinno", this.txtTinNo.Text.Trim());
            collection.Add("licenceno", this.txtLicenceNo.Text.Trim());
            collection.Add("employeridno", this.txtEmployerId.Text.Trim());
            collection.Add("description", this.txtAboutBusiness.Text.Trim());
            collection.Add("companylogo", this.hid_company_logo.Value);

            collection.Add("currencyid", this.ddlCurrency.SelectedValue);
            collection.Add("taxid", this.ddlTax.SelectedValue);
            collection.Add("termscondition", this.txtTermCondition.Text.Trim());
            collection.Add("buemail", this.txtBUEmailAddress.Text.Trim());
            collection.Add("buphone", this.txtBUPhone.Text.Trim());
            collection.Add("bucontactcountrycode", this.ddlBUPhoneCountryCode.SelectedValue);

            bool success = UserBA.UpdateBusinessUser(collection, this.CompanyId);
            if (success)
            {

                string[] files = this.filenames.Value.Split(',');
                if (files != null || files.Length > 0)
                {
                    NameValueCollection gcollection = new NameValueCollection();
                    gcollection["bu_id"] = this.CompanyId;
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

                        UserBA.AddBusinessUserGallery(gcollection);
                    }

                    this.filenames.Value = string.Empty;

                    //this.PopulateControls();
                    //this.lblError.Text = Resources.Resource.ActionSuccess;
                    Response.Redirect("buprofile.aspx");
                }
                else
                {
                    this.lblError.Text = Resources.Resource.error;
                }
            }
        }


        protected void repeaterFiles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string deletefilename = this.ConvertToString(e.CommandArgument);
            if (string.IsNullOrEmpty(deletefilename)) return;

            UserBA.DeleteBusinessFilePhotos(deletefilename);
            this.PopulateControls();
        }
    }
}