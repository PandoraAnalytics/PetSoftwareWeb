using BABusiness;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace Breederapp
{
    public partial class buimportcustomer : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;
            this.panelResult.Visible = false;
            this.lblResultSummary.Text = string.Empty;
            this.gridResult.DataBind();

            if (!this.fileUpload.HasFile || this.fileUpload.PostedFile == null || string.IsNullOrEmpty(this.fileUpload.PostedFile.FileName))
            {
                this.lblError.Text = "Please upload the excel sheet to continue";
                return;
            }

            string fileExtension = Path.GetExtension(this.fileUpload.PostedFile.FileName);
            if (fileExtension == null || (fileExtension.ToLower() != ".xls" && fileExtension.ToLower() != ".xlsx"))
            {
                this.lblError.Text = "Please upload the valid file with xls or xlsx extension to continue";
                return;
            }

            string fileName = "import_customer_file_" + DateTime.Now.Ticks.ToString() + fileExtension;
            string fileLocation = this.FileUploadTempPath + fileName;
            this.fileUpload.PostedFile.SaveAs(fileLocation);

            DataTable excelDataTable = new DataTable();
            OleDbConnection objConn = null;
            try
            {
                try
                {
                    string excelconnectionstring = "";
                    switch (fileExtension)
                    {
                        case ".xls":
                            excelconnectionstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                            break;

                        case ".xlsx":
                            excelconnectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0 xml;HDR=Yes;IMEX=1\"";
                            break;
                    }

                    string query = "select * from [Sheet1$]";

                    OleDbCommand cmd = null;
                    OleDbDataAdapter oleDA = new OleDbDataAdapter();
                    DataSet excelDs = new DataSet();

                    objConn = new OleDbConnection(excelconnectionstring);
                    objConn.Open();

                    cmd = new OleDbCommand(query, objConn);
                    oleDA.SelectCommand = cmd;
                    oleDA.Fill(excelDs);
                    oleDA.Dispose();
                    oleDA = null;
                    objConn.Close();
                    objConn.Dispose();
                    objConn = null;

                    if (excelDs == null || excelDs.Tables.Count == 0 || excelDs.Tables[0].Rows.Count == 0)
                    {
                        this.lblError.Text = "No records found in uploaded excel file";
                        return;
                    }

                    excelDataTable = excelDs.Tables[0];
                }
                catch (Exception ex2)
                {
                    Stream fs = this.fileUpload.PostedFile.InputStream;

                    IWorkbook hssfwb = null;
                    switch (fileExtension)
                    {
                        case ".xls":
                            hssfwb = new HSSFWorkbook(fs);
                            break;

                        case ".xlsx":
                            hssfwb = new XSSFWorkbook(fs);
                            break;
                    }

                    ISheet sheet = hssfwb.GetSheet("Sheet1");

                    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                    IRow headerRow = sheet.GetRow(0);
                    for (int j = 0; j < headerRow.LastCellNum; j++)
                    {
                        ICell cell = headerRow.GetCell(j);
                        excelDataTable.Columns.Add(cell.ToString());
                    }

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        DataRow dataRow = excelDataTable.NewRow();

                        for (int j = row.FirstCellNum; j < headerRow.LastCellNum; j++)
                        {
                            if (row.GetCell(j) != null)
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        excelDataTable.Rows.Add(dataRow);
                    }
                }

                if (excelDataTable == null || excelDataTable.Rows.Count == 0)
                {
                    this.lblError.Text = "No records found in uploaded excel file";
                    return;
                }

                Services.validation validate = new Services.validation();
                User objUser = new User();
                UserBA objUserBA = new UserBA();

                string userid = this.UserId;
                int rowindex = 0;

                DataTable resultTable = new DataTable();
                resultTable.Columns.Add("rowindex");
                resultTable.Columns.Add("customerid");
                resultTable.Columns.Add("customername");
                resultTable.Columns.Add("action");
                resultTable.Columns.Add("message");

                foreach (DataRow excelRow in excelDataTable.Rows)
                {
                    DataRow resultRow = resultTable.NewRow();
                    resultTable.Rows.Add(resultRow);
                    resultRow["rowindex"] = rowindex + 1;

                    try
                    {
                        string errortext = string.Empty;

                        #region validation

                        string firstname = this.ConvertToString(excelRow["First Name"]).Trim();
                        string[] errorList = validate.Validate(firstname, "required");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "First Name is blank, ";
                            }
                        }

                        string lastname = this.ConvertToString(excelRow["Last Name"]).Trim();
                        errorList = validate.Validate(lastname, "required");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "Last Name is blank, ";
                            }
                        }

                        string emailaddress = this.ConvertToString(excelRow["Email"]).Trim();
                        errorList = validate.Validate(emailaddress, "required email");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "Invalid email address, ";
                            }
                        }

                        string gender = this.ConvertToString(excelRow["Gender"]).Trim();
                        errorList = validate.Validate(gender, "required");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "Gender is blank, ";
                            }
                        }

                        string mobile = this.ConvertToString(excelRow["Mobile"]).Trim();
                        errorList = validate.Validate(mobile, "required");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "Phone is blank, ";
                            }
                        }
                        resultRow["customername"] = firstname + " " + lastname;

                        if (!string.IsNullOrEmpty(errortext))
                        {
                            resultRow["customerid"] = string.Empty;
                            resultRow["action"] = "error";
                            resultRow["message"] = errortext.Trim().TrimEnd(',');
                            rowindex++;
                            continue;
                        }

                        int existingcustomerid = int.MinValue;
                        int newuserId = int.MinValue;
                        int emailID = int.MinValue;

                        if (this.ddlDuplicateAction.SelectedValue == "skip" || this.ddlDuplicateAction.SelectedValue == "overwrite")
                        {
                            switch (this.ddlDuplicateColumn.SelectedValue)
                            {
                                case "email":

                                    emailID = BABusiness.User.CheckEmailExist(emailaddress);
                                    if (emailID > 0)
                                    {
                                        newuserId = BABusiness.User.GetUserId(emailaddress);
                                    }

                                    existingcustomerid = BUCustomer.GetCustomerIdByUserID(newuserId, this.CompanyId);

                                    break;
                            }


                            if (existingcustomerid > 0 && this.ddlDuplicateAction.SelectedValue == "skip")
                            {
                                resultRow["customerid"] = BASecurity.Encrypt(existingcustomerid.ToString(), PageBase.HashKey);
                                resultRow["action"] = "skipped";
                                resultRow["message"] = "Duplicate entry. The row has been skipped";
                                rowindex++;
                                continue;
                            }
                        }

                        #endregion
                        string address = this.ConvertToString(excelRow["Address"]).Trim();
                        string city = this.ConvertToString(excelRow["City"]).Trim();
                        string postcode = this.ConvertToString(excelRow["Post Code"]).Trim();
                        string dob = this.ConvertToString(excelRow["DOB"]).Trim();
                        string alternatecontact = this.ConvertToString(excelRow["Alternate Contact"]).Trim();

                        NameValueCollection collection = new NameValueCollection();
                        switch (gender.ToString())
                        {
                            case "Male":
                                collection.Add("gender", "1");
                                break;

                            case "Female":
                                collection.Add("gender", "2");
                                break;
                        }
                        collection.Add("dob", dob);
                        collection.Add("alternatecontact", alternatecontact);
                        collection.Add("createdby", this.UserId);

                        if (existingcustomerid > 0)
                        {
                            if (newuserId > 0)
                            {
                                NameValueCollection langcollection = UserBA.GetUserDetail(newuserId);
                                ViewState["lang"] = langcollection["lang"].ToString();
                                NameValueCollection usercollection = new NameValueCollection();
                                usercollection.Add("fname", firstname);
                                usercollection.Add("lname", lastname);
                                usercollection.Add("phone", mobile);
                                usercollection.Add("email", emailaddress);
                                usercollection.Add("address", address);
                                usercollection.Add("city", city);
                                usercollection.Add("pincode", postcode);
                                usercollection.Add("lang", ViewState["lang"].ToString());
                                bool usersuccess = objUserBA.UpdateUser(usercollection, newuserId);
                                if (!usersuccess)
                                {
                                    resultRow["customerid"] = string.Empty;
                                    resultRow["action"] = "error";
                                    resultRow["message"] = "DB Error - User Update Failed";
                                }
                            }

                            NameValueCollection customercollection = new NameValueCollection();
                            switch (gender.ToString())
                            {
                                case "Male":
                                    customercollection.Add("gender", "1");
                                    break;

                                case "Female":
                                    customercollection.Add("gender", "2");
                                    break;
                            }
                            customercollection.Add("dob", dob);
                            customercollection.Add("alternatecontact", alternatecontact);
                            customercollection.Add("updatedby", this.UserId);

                            bool success = BUCustomer.UpdateCustomer(customercollection, existingcustomerid);
                            if (!success)
                            {
                                resultRow["customerid"] = string.Empty;
                                resultRow["action"] = "error";
                                resultRow["message"] = "DB Error - Customer Update";
                            }
                            else
                            {
                                resultRow["customerid"] = BASecurity.Encrypt(existingcustomerid.ToString(), PageBase.HashKey);
                                resultRow["action"] = "overwrite";
                                resultRow["message"] = "Record overwritten";
                            }
                        }
                        else
                        {
                            if (emailID <= 0)
                            {
                                NameValueCollection usercollection = new NameValueCollection();
                                usercollection.Add("user_pre_name", firstname);
                                usercollection.Add("user_family_name", lastname);
                                usercollection.Add("user_email", emailaddress);
                                usercollection.Add("user_phone", mobile);
                                usercollection.Add("user_address", address);
                                usercollection.Add("user_city", city);
                                usercollection.Add("user_pincode", postcode);
                                usercollection.Add("user_type", "5");// new for customer
                                usercollection.Add("user_token", BusinessBase.Now.Ticks.ToString());
                                newuserId = objUser.Add(usercollection);
                                if (newuserId > 0)
                                {
                                    NameValueCollection newusercollection = UserBA.GetUserDetail(newuserId);
                                    if (newusercollection != null)
                                    {
                                        BreederMail.SendEmail(BreederMail.MessageType.NEWUSERWELCOMEEMAIL, newusercollection);
                                    }

                                    existingcustomerid = AddCustomer(newuserId, collection);
                                    if (existingcustomerid > 0)
                                    {
                                        resultRow["customerid"] = BASecurity.Encrypt(this.ConvertToString(existingcustomerid), PageBase.HashKey);
                                        resultRow["action"] = "created";
                                        resultRow["message"] = "Added new record";

                                    }
                                    else
                                    {
                                        resultRow["customerid"] = string.Empty;
                                        resultRow["action"] = "error";
                                        resultRow["message"] = "DB ERROR - New Record Add";
                                    }
                                }
                            }
                            else
                            {
                                // existing user
                                newuserId = BABusiness.User.GetUserId(emailaddress);
                                existingcustomerid = AddCustomer(newuserId, collection);

                                if (existingcustomerid > 0)
                                {
                                    resultRow["customerid"] = BASecurity.Encrypt(this.ConvertToString(existingcustomerid), PageBase.HashKey);
                                    resultRow["action"] = "created";
                                    resultRow["message"] = "Added new record";

                                }
                                else
                                {
                                    resultRow["customerid"] = string.Empty;
                                    resultRow["action"] = "error";
                                    resultRow["message"] = "DB ERROR - New Record Add";
                                }

                            }
                        }
                    }
                    catch (Exception)
                    {
                        resultRow["action"] = "error";
                        resultRow["message"] = "Exception Generated";
                    }
                    rowindex = rowindex + 1;
                }

                this.panelResult.Visible = true;
                int skippedcount = resultTable.AsEnumerable().Where(myRow => myRow.Field<string>("action") == "skipped").Count();
                int failurecount = resultTable.AsEnumerable().Where(myRow => myRow.Field<string>("action") == "error").Count();
                int createdcount = resultTable.AsEnumerable().Where(myRow => myRow.Field<string>("action") == "created").Count();
                int overwritecount = resultTable.AsEnumerable().Where(myRow => myRow.Field<string>("action") == "overwrite").Count();
                int totalcount = resultTable.Rows.Count;

                this.lblResultSummary.Text = string.Format("Import Completed. {0}/{4} created, {1}/{4} failed, {2}/{4} skipped, {3}/{4} created, ", createdcount, failurecount, skippedcount, overwritecount, totalcount);

                this.gridResult.DataSource = resultTable;
                this.gridResult.DataBind();

            }
            catch (Exception x)
            {
                this.lblError.Text = x.Message + " -  " + "Exception Generated. We recommend to download the template to import the data.";
            }
            finally
            {
                if (objConn != null && objConn.State == ConnectionState.Open)
                {
                    objConn.Close();
                    objConn.Dispose();
                    objConn = null;
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("customerlist.aspx");
        }

        public static string GenerateUniqueEmployeecode()
        {
            string customercode = string.Empty;
            do
            {
                customercode = BABusiness.User.GetRandomPassword();
            }
            while (BUCustomer.IsCustomerCodeExist(customercode) > 0);
            return customercode;
        }

        private int AddCustomer(int xiNewUserId, NameValueCollection xiCollection)
        {
            NameValueCollection customercollection = new NameValueCollection();
            customercollection.Add("companyid", this.CompanyId);
            customercollection.Add("userid", xiNewUserId.ToString());
            if (xiCollection["gender"] != null) customercollection.Add("gender", xiCollection["gender"]);
            customercollection.Add("dob", xiCollection["dob"]);
            customercollection.Add("alternatecontact", xiCollection["alternatecontact"]);
            string customercode = GenerateUniqueEmployeecode();
            customercollection.Add("customercode", customercode);
            customercollection.Add("createdby", this.UserId);

            int customerId = BUCustomer.AddCustomer(customercollection);

            if (customerId > 0)
            {
                customercollection.Clear();
                customercollection = new NameValueCollection();
                customercollection["bu_id"] = this.CompanyId;
                customercollection["user_id"] = this.UserId;
                customercollection["message_id"] = ((int)UserBA.Status.BUCUSTOMERADDED).ToString();
                customercollection["old_entry"] = string.Empty;
                customercollection["new_entry"] = customerId.ToString();
                customercollection["comment"] = string.Empty;
                UserBA.AddBULog(customercollection);
            }
            return customerId;
        }

    }
}