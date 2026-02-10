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
    public partial class importmembers : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = this.DecryptQueryString("aid");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string associationId = this.ConvertToString(ViewState["id"]);
            if (string.IsNullOrEmpty(associationId)) Response.Redirect("manageassociation.aspx");

            NameValueCollection ascollection = UserBA.GetAssociation(ViewState["id"]);
            if (ascollection == null) Response.Redirect("manageassociation.aspx");

            bool isOwner = this.ConvertToInteger(ascollection["createdby"]) == this.ConvertToInteger(this.UserId);
            if (!isOwner) Response.Redirect("manageassociation.aspx");

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

            string fileName = "import_member_file_" + DateTime.Now.Ticks.ToString() + fileExtension;
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

                    string query = "select Mitgl_NR,Nachname,Vorname,Adresse,Ort,Land,PLZ,Email,Funktelefon,Telefax,Eintritt,Austritt,Grund,Mitgliedsart,Familien_Vollmitglied,LG,Position_LG,ZÜCHTER,Familienmitglied_von,Bemerkung,Zahlungsart,Konto_Nummer,Konto_Inhaber,Bankinstitut,Bankleitzahl,RR,Zwingername,RR_Geburt,ZB_Nr,Telefon,Straße from [Mitgliederliste$]";

                    //string query = "select Mitgl.NR,[Nachname],[Vorname],[Adresse],[Ort],[Land],[PLZ],[Email],[Funktelefon],[Telefax],[Eintritt],[Austritt],[Grund],[Mitgliedsart],[Familien/Vollmitglied],[LG],[Position LG],[ZÜCHTER],[Familienmitglied von],[Bemerkung],[Zahlungsart],[Konto-Nummer],[Konto-Inhaber],[Bankinstitut],[Bankleitzahl],[Rassetyp],[Zwingername],[RR-Geburt],[Halsband-ID],[Telefon],[Straße] from [Mitgliederliste$]";                

                    OleDbCommand cmd = null;
                    OleDbDataAdapter oleDA = new OleDbDataAdapter();
                    DataSet excelDs = new DataSet();

                    objConn = new OleDbConnection(excelconnectionstring);
                    objConn.Open();

                    // establish command object and data adapter
                    cmd = new OleDbCommand(query, objConn);
                    oleDA.SelectCommand = cmd;
                    oleDA.Fill(excelDs);

                    //// selecting distict list of Slno                    
                    //cmd.CommandType = CommandType.Text;
                    //cmd.CommandText = query;
                    //oleDA = new OleDbDataAdapter(cmd);
                    //oleDA.Fill(excelDs);


                    //OleDbDataAdapter oleDA = new OleDbDataAdapter(query, objConn);
                    //oleDA.Fill(excelDs);
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
                Member objMember = new Member();

                string userid = this.UserId;
                int rowindex = 0;

                DataTable resultTable = new DataTable();
                resultTable.Columns.Add("rowindex");
                resultTable.Columns.Add("memberid");
                resultTable.Columns.Add("membername");
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

                        string memberno = this.ConvertToString(excelRow["Mitgl_NR"]).Trim();
                        string[] errorList = validate.Validate(memberno, "required");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "Mitgl_NR is blank, ";
                            }
                        }

                        string firstname = this.ConvertToString(excelRow["Vorname"]).Trim();
                        errorList = validate.Validate(firstname, "required");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "Vorname is blank, ";
                            }
                        }

                        string lastname = this.ConvertToString(excelRow["Nachname"]).Trim();
                        errorList = validate.Validate(lastname, "required");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "Nachname is blank, ";
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

                        string city = this.ConvertToString(excelRow["Ort"]).Trim();
                        errorList = validate.Validate(city, "required");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "Ort is blank, ";
                            }
                        }

                        string country = this.ConvertToString(excelRow["Land"]).Trim();
                        errorList = validate.Validate(country, "required");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "Land is blank, ";
                            }
                        }

                        string zipcode = this.ConvertToString(excelRow["PLZ"]).Trim();
                        errorList = validate.Validate(zipcode, "required");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "PLZ is blank, ";
                            }
                        }

                        string mobile = this.ConvertToString(excelRow["Funktelefon"]).Trim();
                        errorList = validate.Validate(mobile, "required");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "Funktelefon is blank, ";
                            }
                        }

                        string entrydate = this.ConvertToString(excelRow["Eintritt"]).Trim();
                        errorList = validate.Validate(entrydate, "required");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "Eintritt is blank, ";
                            }
                        }

                        string membershiptype = this.ConvertToString(excelRow["Mitgliedsart"]).Trim();
                        errorList = validate.Validate(membershiptype, "required");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "Mitgliedsart is blank, ";
                            }
                        }

                        string breedtype = this.ConvertToString(excelRow["RR"]).Trim();
                        errorList = validate.Validate(breedtype, "required");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "RR is blank, ";
                            }
                        }

                        string animalname = this.ConvertToString(excelRow["Zwingername"]).Trim();
                        errorList = validate.Validate(animalname, "required");
                        if (errorList != null && errorList.Length > 0)
                        {
                            if (string.IsNullOrEmpty(errorList[0]) == false)
                            {
                                errortext += "Zwingername is blank, ";
                            }
                        }

                        resultRow["membername"] = firstname + " " + lastname;

                        if (!string.IsNullOrEmpty(errortext))
                        {
                            resultRow["memberid"] = string.Empty;
                            resultRow["action"] = "error";
                            resultRow["message"] = errortext.Trim().TrimEnd(',');
                            rowindex++;
                            continue;
                        }

                        int existingmemberid = int.MinValue;
                        if (this.ddlDuplicateAction.SelectedValue == "skip" || this.ddlDuplicateAction.SelectedValue == "overwrite")
                        {
                            switch (this.ddlDuplicateColumn.SelectedValue)
                            {
                                case "email":
                                    existingmemberid = Member.GetMemberIdByEmailAddress(emailaddress, ViewState["id"]);
                                    break;
                            }


                            if (existingmemberid > 0 && this.ddlDuplicateAction.SelectedValue == "skip")
                            {
                                resultRow["memberid"] = BASecurity.Encrypt(existingmemberid.ToString(), PageBase.HashKey);
                                resultRow["action"] = "skipped";
                                resultRow["message"] = "Duplicate entry. The row has been skipped";
                                rowindex++;
                                continue;
                            }
                        }

                        #endregion

                        string address = this.ConvertToString(excelRow["Adresse"]).Trim();
                        string fax = this.ConvertToString(excelRow["Telefax"]).Trim();
                        string exitdate = this.ConvertToString(excelRow["Austritt"]).Trim();
                        string exitreason = this.ConvertToString(excelRow["Grund"]).Trim();

                        string family_fullmember = this.ConvertToString(excelRow["Familien_Vollmitglied"]).Trim();
                        string region = this.ConvertToString(excelRow["LG"]).Trim();
                        string positioninregion = this.ConvertToString(excelRow["Position_LG"]).Trim();

                        string isbreeder = this.ConvertToString(excelRow["ZÜCHTER"]).Trim();
                        string familymemberof = this.ConvertToString(excelRow["Familienmitglied_von"]).Trim();
                        string remark = this.ConvertToString(excelRow["Bemerkung"]).Trim();
                        string paymentmethod = this.ConvertToString(excelRow["Zahlungsart"]).Trim();
                        string accountnumber = this.ConvertToString(excelRow["Konto_Nummer"]).Trim();
                        string accountownername = this.ConvertToString(excelRow["Konto_Inhaber"]).Trim();
                        string bankname = this.ConvertToString(excelRow["Bankinstitut"]).Trim();
                        string bankcode = this.ConvertToString(excelRow["Bankleitzahl"]).Trim();
                        string animal_birthdate = this.ConvertToString(excelRow["RR_Geburt"]).Trim();
                        string phone = this.ConvertToString(excelRow["Telefon"]).Trim();
                        string street = this.ConvertToString(excelRow["Straße"]).Trim();
                        string collarid = this.ConvertToString(excelRow["ZB_Nr"]).Trim();


                        NameValueCollection collection = new NameValueCollection();
                        collection.Add("memberno", memberno);
                        collection.Add("fname", firstname);
                        collection.Add("lname", lastname);
                        collection.Add("address", address);
                        collection.Add("city", city);
                        collection.Add("country", country);
                        collection.Add("zipcode", zipcode);
                        collection.Add("email", emailaddress);
                        collection.Add("mobile", mobile);
                        collection.Add("fax", fax);
                        collection.Add("entrydate", entrydate);
                        collection.Add("exitdate", exitdate);
                        collection.Add("exitreason", exitreason);
                        collection.Add("membershiptype", membershiptype);
                        collection.Add("familyfullmember", family_fullmember);
                        collection.Add("region", region);
                        collection.Add("positioninregion", positioninregion);

                        collection.Add("isbreeder", isbreeder);
                        collection.Add("familymemberof", familymemberof);
                        collection.Add("remark", remark);
                        collection.Add("paymentmethod", paymentmethod);
                        collection.Add("accountnumber", accountnumber);
                        collection.Add("accountownername", accountownername);
                        collection.Add("bankname", bankname);
                        collection.Add("bankcode", bankcode);
                        collection.Add("breedtype", breedtype);
                        collection.Add("animalname", animalname);
                        collection.Add("animalbirthdate", animal_birthdate);
                        collection.Add("collarid", collarid);
                        collection.Add("phone", phone);
                        collection.Add("street", street);
                        collection.Add("submitby", this.UserId);

                        if (existingmemberid > 0)
                        {
                            bool success = objMember.UpdateMember(collection, existingmemberid);
                            if (!success)
                            {
                                resultRow["memberid"] = string.Empty;
                                resultRow["action"] = "error";
                                resultRow["message"] = "DB Error - Update";
                            }
                            else
                            {
                                resultRow["memberid"] = BASecurity.Encrypt(existingmemberid.ToString(), PageBase.HashKey);
                                resultRow["action"] = "overwrite";
                                resultRow["message"] = "Record overwritten";
                            }
                        }
                        else
                        {
                            int memberId = objMember.AddMember(collection);
                            if (memberId > 0)
                            {
                                NameValueCollection memberCollection = new NameValueCollection();
                                memberCollection.Add("memberid", this.ConvertToString(memberId));
                                memberCollection.Add("association_id", this.ConvertToString(ViewState["id"]));
                                int retId = objMember.AddAssociationMembers(memberCollection);

                                resultRow["memberid"] = BASecurity.Encrypt(this.ConvertToString(memberId), PageBase.HashKey);
                                resultRow["action"] = "created";
                                resultRow["message"] = "Added new record";

                            }
                            else
                            {
                                resultRow["memberid"] = string.Empty;
                                resultRow["action"] = "error";
                                resultRow["message"] = "DB ERROR - New Record Add";
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
            Response.Redirect("manageassociation.aspx");
        }


    }
}