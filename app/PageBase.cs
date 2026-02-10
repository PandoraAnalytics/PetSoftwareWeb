using BABusiness;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using NReco.PdfGenerator;    

namespace Breederapp
{
    public class PageBase : System.Web.UI.Page
    {
        private bool _isadminaccess = false;
        private bool _isassociateaccess = false;

        public string UserId
        {
            get { return ConvertToString(Session["userid"]); }
        }

        public string UserType
        {
            get { return ConvertToString(Session["usertype"]); }
        }

        public string CompanyId
        {
            get { return ConvertToString(Session["companyid"]); }
        }

        public static string HashKey
        {
            get { return (HttpContext.Current.Session != null) ? HttpContext.Current.Session.SessionID : ""; }
        }

        public string DateFormat
        {
            get { return ConvertToString(Session["dtformat"]); }
        }

        public string DateFormatSmall
        {
            get { return (this.DateFormat != null) ? this.DateFormat.ToLower() : string.Empty; }
        }

        public string DateTimeFormat
        {
            get { return (this.DateFormat != null) ? this.DateFormat + " HH:mm" : this.DateTimeFormat; }
        }

        public bool IsAdminAccess
        {
            get { return _isadminaccess; }
            set { _isadminaccess = value; }
        }

        public bool IsAssociationAccess
        {
            get { return _isassociateaccess; }
            set { _isassociateaccess = value; }
        }

        public string FileUploadTempPath
        {
            get { return System.Web.Hosting.HostingEnvironment.MapPath("~") + @"/app/docs/temp/"; }
        }

        virtual protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.UserId))
            {
                Response.Redirect("signin.aspx");
            }

            if (_isadminaccess && this.ConvertToInteger(Session["isowner"]) != 1)
            {
                Response.Redirect("landing.aspx?access=false");
            }

            if (_isassociateaccess && this.ConvertToInteger(Session["isassociation"]) != 1)
            {
                Response.Redirect("landing.aspx?access=false");
            }
        }

        protected override void InitializeCulture()
        {
            Thread.CurrentThread.CurrentCulture = BABusiness.BusinessBase.GetCulture();

            string lang = this.ConvertToString(Session["userlang"]);
            if (string.IsNullOrEmpty(lang)) lang = "en-US";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
        }

        protected CultureInfo GetLangCulture()
        {
            CultureInfo cultureinfo = null;
            switch (this.ConvertToString(Session["userlang"]))
            {
                default:
                    cultureinfo = new CultureInfo("en-US");
                    break;


                case "de-DE":
                    cultureinfo = new CultureInfo("de-DE");
                    break;
            }
            return cultureinfo;
        }

        protected string ReadQueryString()
        {
            string queryString = Request.Url.Query;
            if (string.IsNullOrEmpty(queryString)) return "0";
            else return HttpUtility.UrlDecode(queryString.Substring(1));
        }

        protected string ReadQueryString(string xiKey)
        {
            try
            {
                string value = string.Empty;
                string[] values = Request.QueryString.GetValues(xiKey);
                if (values != null && values.Length > 0)
                {
                    value = HttpUtility.UrlDecode(values[0]);
                }
                return value;
            }
            catch { }

            return "0";
        }

        protected string DecryptQueryString()
        {
            string code = this.ReadQueryString();
            code = HttpUtility.UrlDecode(code);
            code = code.Replace(' ', '+');
            if (code == "0") return code;
            else return BASecurity.Decrypt(code, PageBase.HashKey);
        }

        protected string DecryptQueryString(string xiKey)
        {
            try
            {
                string value = string.Empty;
                string[] values = Request.QueryString.GetValues(xiKey);
                if (values != null && values.Length > 0)
                {
                    value = HttpUtility.UrlDecode(values[0]);
                    value = BASecurity.Decrypt(value, PageBase.HashKey);
                }
                return value;
            }
            catch { }

            return "0";
        }

        protected string ConvertToString(object xiObj)
        {
            string returnString = string.Empty;
            if (xiObj == null || xiObj == DBNull.Value) return returnString;

            try
            {
                returnString = Convert.ToString(xiObj);
            }
            catch
            {
                returnString = string.Empty;
            }

            return returnString;
        }

        protected int ConvertToInteger(object xiObj)
        {
            int returnInt = int.MinValue;
            if (xiObj == null || xiObj == DBNull.Value) return returnInt;

            try
            {
                returnInt = Convert.ToInt32(xiObj);
            }
            catch
            {
                returnInt = int.MinValue;
            }

            return returnInt;
        }

        protected double ConvertToDouble(object xiObj, IFormatProvider xiProvider = null)
        {
            double returnDouble = double.MinValue;
            if (xiObj == null || xiObj == DBNull.Value) return returnDouble;

            try
            {
                string temp = ConvertToString(xiObj);
                if (string.IsNullOrEmpty(temp)) return returnDouble;

                temp = temp.Replace(',', '.');

                if (xiProvider == null) xiProvider = BusinessBase.USCulture;
                returnDouble = Convert.ToDouble(temp, xiProvider);
            }
            catch
            {
                returnDouble = double.MinValue;
            }

            return returnDouble;
        }

        protected string nl2br(object xiObj)
        {
            string returnString = string.Empty;
            if (xiObj == null || xiObj == DBNull.Value) return returnString;

            try
            {
                returnString = Convert.ToString(xiObj);
                if (string.IsNullOrEmpty(returnString)) return returnString;

                returnString = returnString.Replace(Environment.NewLine, "<br>");
            }
            catch { }

            return returnString;
        }

        protected string br2nl(object xiObj)
        {
            string returnString = string.Empty;
            if (xiObj == null || xiObj == DBNull.Value) return returnString;

            try
            {
                returnString = Convert.ToString(xiObj);
                if (string.IsNullOrEmpty(returnString)) return returnString;

                returnString = returnString.Replace("\n", " ");
                returnString = returnString.Replace(Environment.NewLine, " ");
            }
            catch { }

            return returnString;
        }

        protected string Secure(object xiObj)
        {
            string returnString = string.Empty;
            if (xiObj == null || xiObj == DBNull.Value) return returnString;

            try
            {
                returnString = ConvertToString(xiObj);
                if (string.IsNullOrEmpty(returnString)) return returnString;

                returnString = BASecurity.Encrypt(returnString, PageBase.HashKey);
            }
            catch { }

            return returnString;
        }

        public string ProcessEventBrochurePdf(object xiBrochureId)
        {
            string brochureId = this.ConvertToString(xiBrochureId);
            if (string.IsNullOrEmpty(brochureId)) return null;

            string fileName = string.Empty;

            fileName = "brochure_" + BABusiness.BusinessBase.Now.Ticks.ToString();

            string htmlfilepath = Server.MapPath("~") + @"app\docs\files\eventbrochurepdf.html";
            string pdfFilePath = this.FileUploadTempPath + fileName + ".pdf";

            string headerHTML = @"<header style='padding-bottom: 90px;'><div><div style='width: 50%;float: left;'> {0} </div><div style='width: 50%;float:right;'> <img style='width: 20%;float: right;' src='{1}' /></div></div></header>";
            string footerHTML = @"<footer style='padding-top: 20px;'><div><div style='width: 100%;text-align: center;'>{0}</div></div></footer>";

            string mainHTML = string.Empty;
            StringBuilder mainBuilder = new StringBuilder();
            try
            {
                StreamReader htmlReader = new StreamReader(new FileStream(htmlfilepath, FileMode.Open, FileAccess.Read));
                mainHTML = htmlReader.ReadToEnd();
                htmlReader.Close();
                htmlReader = null;

                if (string.IsNullOrEmpty(mainHTML)) return string.Empty;
                mainHTML = mainHTML.Replace("\r\n", "");
                mainHTML = mainHTML.Replace("\0", "");

                NameValueCollection boCollection = EventBA.GetBrochureDetail(brochureId);
                if (boCollection == null) return null;

                mainHTML = mainHTML.Replace("#brochurename", this.ConvertToString(boCollection["name"]));
                mainHTML = mainHTML.Replace("#brochureimg", this.ConvertToString(boCollection["brochure_file"]));
                mainHTML = mainHTML.Replace("#brochuredescription", this.ConvertToString(boCollection["description"]));

                headerHTML = string.Format(headerHTML, boCollection["headertext"], boCollection["brochure_file"], BreederMail.PageURL);
                footerHTML = string.Format(footerHTML, boCollection["footertext"]);

                DataTable dataTable = EventBA.GetAllBrochurePages(brochureId);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    int index = 0;
                    foreach (DataRow rowPage in dataTable.Rows)
                    {
                        int type = this.ConvertToInteger(rowPage["content_type"]);
                        int subtype = this.ConvertToInteger(rowPage["content_subtype"]);
                        int pagecount = dataTable.Rows.Count;
                        index++;
                        switch (type)
                        {
                            case 1: // text editor
                                NameValueCollection col = EventBA.GetBrochurePageDetail(rowPage["id"]);

                                if (col != null)
                                {
                                    string editorHTML = @"<table class='border' align='center' style='text-align:center; width:100%;'><tr><td style = 'text-align:center; vertical-align: middle;width:100%;'>" + HttpUtility.HtmlDecode(col["content_data"]) + "</td> </tr></table>";
                                    mainBuilder.Append(editorHTML);
                                }
                                break;

                            case 2: // Animal list
                                #region animallist

                                DataTable animalTable = AnimalBA.GetEventsAnimal(this.ConvertToString(boCollection["eventid"]));
                                if (animalTable != null && animalTable.Rows.Count > 0)
                                {
                                    StringBuilder animalHTML = new StringBuilder();
                                    animalHTML.Append("<table style='width:100%;' class='border'><tr><td style='width:100%; padding: 0;'><div class='animalheader'>Pet List </div><div class='animalbox' style='padding:10px; width: 100%; overflow: auto;'>");
                                    foreach (DataRow animalRow in animalTable.Rows)
                                    {
                                        string animalinnerhtml = @"<div style='width:100%; overflow: auto;'><h4> #animalname (#breedtype)222 </h4> <div style='width: 50%;float: left;line-height: 25px;'><lable>Father: <span class='written_font'>#fathername</span>  </lable><br /><lable>Mother: <span class='written_font'>#mothername</span>  </lable><br /><lable>Owner: <span class='written_font'>#ownername</span>  </lable><br /><lable>Breeder: <span class='written_font'>#breedername</span>  </lable><br /><lable>Gender: <span class='written_font'>#gender</span>  </lable><br /><lable>Color: <span class='written_font'>#color</span>  </lable></div>
<div style='width: 50%;float: left;line-height: 25px;'><lable>Birth Date: <span class='written_font'>#birthdate</span></lable><br /><lable>Height: <span class='written_font'>#height</span></lable><br /><lable>Weight: <span class='written_font'>#weight</span>  </lable><br /><lable>Coat Color: <span class='written_font'>#coatcolor</span>  </lable><br /><lable>Life Expectancy: <span class='written_font'>#lifeexpectancy</span></lable></div></div>";

                                        animalinnerhtml = animalinnerhtml.Replace("#animalname", this.ConvertToString(animalRow["name"]));
                                        animalinnerhtml = animalinnerhtml.Replace("#breedtype", this.ConvertToString(animalRow["typename"]));
                                        animalinnerhtml = animalinnerhtml.Replace("#fathername", this.ConvertToString(animalRow["fathername"]));
                                        animalinnerhtml = animalinnerhtml.Replace("#mothername", this.ConvertToString(animalRow["mothername"]));
                                        animalinnerhtml = animalinnerhtml.Replace("#ownername", this.ConvertToString(animalRow["ownername"]));
                                        animalinnerhtml = animalinnerhtml.Replace("#breedername", this.ConvertToString(animalRow["breederfullname"]));

                                        int selectedGender = this.ConvertToInteger(animalRow["gender"]);
                                        string genderVal = string.Empty;
                                        switch (selectedGender)
                                        {
                                            case 1:
                                                genderVal = "Male";
                                                break;

                                            case 2:
                                                genderVal = "Female";
                                                break;

                                            case 3:
                                                genderVal = "Others";
                                                break;
                                        }

                                        animalinnerhtml = animalinnerhtml.Replace("#gender", genderVal);
                                        animalinnerhtml = animalinnerhtml.Replace("#color", this.ConvertToString(animalRow["color"]));

                                        try
                                        {
                                            DateTime birthDay = Convert.ToDateTime(animalRow["dob"]);
                                            DateTime today = DateTime.Today;

                                            int ageYears = today.Year - birthDay.Year;
                                            int ageMonths = today.Month - birthDay.Month;


                                            if (today.Month < birthDay.Month || (today.Month == birthDay.Month && today.Day < birthDay.Day))
                                            {
                                                ageYears--;
                                                ageMonths = 12 - birthDay.Month + today.Month - 1;
                                            }


                                            string ageString = ageYears + " Year";
                                            if (ageYears != 1) ageString += "s";
                                            if (ageMonths >= 0)
                                            {
                                                ageString += " " + ageMonths + " Month";
                                                if (ageMonths != 1) ageString += "s";
                                            }
                                            animalinnerhtml = animalinnerhtml.Replace("#birthdate", birthDay.ToString(this.DateFormat));
                                        }
                                        catch { }

                                        animalinnerhtml = animalinnerhtml.Replace("#height", this.ConvertToString(animalRow["height"]));
                                        animalinnerhtml = animalinnerhtml.Replace("#weight", this.ConvertToString(animalRow["weight"]));
                                        animalinnerhtml = animalinnerhtml.Replace("#coatcolor", this.ConvertToString(animalRow["spancoat"]));
                                        animalinnerhtml = animalinnerhtml.Replace("#lifeexpectancy", this.ConvertToString(animalRow["life"]));
                                        animalHTML.Append(animalinnerhtml);
                                    }

                                    animalHTML.Append("</div></td></tr></table>");
                                    mainBuilder.Append(animalHTML);
                                }
                                #endregion
                                break;

                            case 3: // Owner list
                                #region OwnerList

                                DataTable ownerTable = AnimalBA.GetEventsOwners(this.ConvertToString(boCollection["eventid"]));
                                if (ownerTable != null && ownerTable.Rows.Count > 0)
                                {
                                    string ownerHTML = @"<table style='width:100%;' class='border'>
<tr><td style='width:100%;' style='padding: 0;'><div class='animalheader'>Owner's List </div>";
                                    foreach (DataRow ownerRow in ownerTable.Rows)
                                    {
                                        string innerhtml = @" <div style='padding: 10px;'><div class='list'><span class='txt_bold1'>#ownername-</span>&nbsp;&nbsp;<span class='written_font'>#owneraddress</span></div></div><br />";
                                        innerhtml = innerhtml.Replace("#ownername", this.ConvertToString(ownerRow["ownername"]));
                                        innerhtml = innerhtml.Replace("#owneraddress", this.ConvertToString(ownerRow["fulladdress"]));

                                        ownerHTML += innerhtml;
                                    }

                                    ownerHTML += "</td></tr></table>";
                                    mainBuilder.Append(ownerHTML);
                                }
                                #endregion

                                break;

                            case 4: // Sponsor
                                #region Sponsor
                                DataTable dtSponsor = EventBA.GetPageSponsorDetails(rowPage["id"]);
                                if (dtSponsor != null && dtSponsor.Rows.Count > 0)
                                {
                                    string sponsorHtml = string.Empty;
                                    switch (subtype)
                                    {
                                        case 1:// Type Gold
                                            sponsorHtml = string.Empty;
                                            foreach (DataRow rowSponsor in dtSponsor.Rows)
                                            {
                                                sponsorHtml = @"<table style='width:100%;' class='border'><tr><td style='padding: 0;' align='center'><h2>#sponsorname </h2><br /><img class='goldsponsor' src='{0}' /></td></tr></table>";
                                                sponsorHtml = string.Format(sponsorHtml, PageBase.getbase64url(this.ConvertToString(rowSponsor["sponsor_file"])));
                                                sponsorHtml = sponsorHtml.Replace("#sponsorname", this.ConvertToString(rowSponsor["name"]));

                                            }
                                            mainBuilder.Append(sponsorHtml);
                                            break;

                                        case 2:// Type Silver
                                            sponsorHtml = string.Empty;
                                            sponsorHtml = @"<table style='width:100%;' class='border'><tr><td style='padding: 0;' align='center'>";
                                            foreach (DataRow rowSponsor in dtSponsor.Rows)
                                            {
                                                string innerHtml = @"<h2>#sponsorname </h2><img class='silversponsor' src='{0}' /><br />";
                                                innerHtml = string.Format(innerHtml, PageBase.getbase64url(this.ConvertToString(rowSponsor["sponsor_file"])));
                                                innerHtml = innerHtml.Replace("#sponsorname", this.ConvertToString(rowSponsor["name"]));
                                                sponsorHtml += innerHtml;
                                            }

                                            sponsorHtml += "</td></tr></table>";
                                            mainBuilder.Append(sponsorHtml);
                                            break;

                                        case 3: // Type Bronze
                                            sponsorHtml = string.Empty;
                                            sponsorHtml = @"<table style='width:100%;' class='border'><tr><td style='padding: 0;' align='center'><div class='mycontainer'>";
                                            foreach (DataRow rowSponsor in dtSponsor.Rows)
                                            {
                                                string innerHtml = @"<div><h2>#sponsorname </h2><img class='brsponsor' src='{0}' /></div>";
                                                innerHtml = string.Format(innerHtml, PageBase.getbase64url(this.ConvertToString(rowSponsor["sponsor_file"])));
                                                innerHtml = innerHtml.Replace("#sponsorname", this.ConvertToString(rowSponsor["name"]));
                                                sponsorHtml += innerHtml;
                                            }

                                            sponsorHtml += "</div></td></tr></table>";
                                            mainBuilder.Append(sponsorHtml);

                                            break;
                                    }
                                }

                                #endregion
                                break;
                        }

                        if (index < pagecount)
                            mainBuilder.Append("<div class='page_break'></div>");

                    }
                }
            }
            catch (Exception ex2) { }

            string completeHTML = mainBuilder.ToString();
            mainHTML = mainHTML.Replace("##content", completeHTML);
            mainHTML = mainHTML.Replace("#imgurl", BreederMail.PageURL);
            HtmlToPdfConverter htmlbuilder = new HtmlToPdfConverter();
            htmlbuilder.Size = PageSize.A4;
            htmlbuilder.PageHeaderHtml = headerHTML;
            htmlbuilder.PageFooterHtml = footerHTML;
            htmlbuilder.Orientation = PageOrientation.Portrait;
            byte[] bytefile = htmlbuilder.GeneratePdf(mainHTML);

            if (bytefile != null && bytefile.Length > 0) File.WriteAllBytes(pdfFilePath, bytefile);

            return fileName;
        }

        protected string ProcessPrintTicket(object xiTicketId, object xiUserId)
        {
            string ticketId = this.ConvertToString(xiTicketId);
            if (string.IsNullOrEmpty(ticketId)) return null;

            NameValueCollection ticketCollection = Ticket.GetTicket(ticketId, xiUserId);
            if (ticketCollection == null) return null;

            string fileName = string.Empty;

            string ticketno = ticketCollection["ticketno"].PadLeft(5, '0');

            fileName = ticketno + "-" + BusinessBase.Now.Ticks.ToString(); ;

            string htmlfilepath = Server.MapPath(".") + @"\docs\files\ticketpdf.html";
            string pdfFilePath = Server.MapPath("~") + @"app\docs\temp\" + fileName + ".pdf";

            try
            {
                StreamReader htmlReader = new StreamReader(new FileStream(htmlfilepath, FileMode.Open, FileAccess.Read));
                string mainHTML = htmlReader.ReadToEnd();
                htmlReader.Close();
                htmlReader = null;

                mainHTML = mainHTML.Replace("\r\n", "");
                mainHTML = mainHTML.Replace("\0", "");

                NameValueCollection collection = new NameValueCollection();
                collection["#ticketno"] = ticketno;
                collection["#createddate"] = Convert.ToDateTime(ticketCollection["createddate"]).ToString(this.DateTimeFormat);
                collection["#author"] = ticketCollection["author"];
                collection["#updateddate"] = Convert.ToDateTime(ticketCollection["updateddate"]).ToString(this.DateTimeFormat);
                collection["#updatedbyauthor"] = ticketCollection["updatedbyauthor"];
                collection["#appname"] = ticketCollection["appname"];
                string header = ticketCollection["header"];
                if (this.ConvertToInteger(collection["isbug"]) == 1) header += "&nbsp;[Bug]";
                collection["#header"] = header;
                collection["#description"] = this.nl2br(ticketCollection["description"]);

                string etd = ticketCollection["etd"];
                if (!string.IsNullOrEmpty(ticketCollection["completiondate"]))
                {
                    DateTime tempDate3 = Convert.ToDateTime(ticketCollection["completiondate"]);
                    if (tempDate3 != DateTime.MinValue) etd += "&nbsp;[CD: " + tempDate3.ToString(this.DateFormat) + "]";
                }
                collection["#etd"] = etd;
                //collection["#etd"] = ticketCollection["etd"];
                collection["#statusname"] = ticketCollection["statusname"];
                collection["#priority"] = ticketCollection["priorityname"];
                collection["###comments###"] = "<tr><td colspan='3'>-</td></tr>";
                DataSet commentsDs = Ticket.GetTicketComments(1, ticketId);
                if (commentsDs != null && commentsDs.Tables.Count > 0)
                {
                    StringBuilder builder = new StringBuilder();
                    string temp = @"<tr><td width='50%'>{0}</td><td width='25%' class='secondtext'>{1}</td><td width='25%' class='secondtext'>{2}</td></tr>";
                    foreach (DataRow row in commentsDs.Tables[0].Rows)
                    {
                        builder.Append(string.Format(temp, row["comment"], row["author"], row["createddate"]));
                    }

                    collection["###comments###"] = builder.ToString();
                }

                foreach (string key in collection.Keys)
                {
                    mainHTML = mainHTML.Replace(key, collection[key]);
                }
                //string clientId = this.ClientId;
                //mainHTML = mainHTML.Replace("#myurl", PandoraMail.PageURL);
                mainHTML = mainHTML.Replace("#myurl", BreederMail.PageURL);
                //mainHTML = mainHTML.Replace("#clientid", clientId);
                HtmlToPdfConverter htmlbuilder = new HtmlToPdfConverter();
                byte[] bytefile = htmlbuilder.GeneratePdf(mainHTML);

                if (bytefile != null && bytefile.Length > 0) File.WriteAllBytes(pdfFilePath, bytefile);
            }
            catch { }

            return fileName;
        }

        #region Blob Functions

        public static void SaveFile(HttpPostedFile postedFile, string fileName)
        {
            string contenttype = postedFile.ContentType;

            Stream fs = postedFile.InputStream;
            BinaryReader reader = null;
            byte[] blob = null;
            try
            {
                reader = new BinaryReader(fs);
                blob = reader.ReadBytes((int)fs.Length);

                BusinessBase.StoreSystemFile(fileName, contenttype, blob);
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                }
                blob = null;
            }
        }

        public static void SaveFile(string xiServerPath, string xiFileName)
        {
            string path = Path.Combine(xiServerPath, xiFileName);

            FileStream fs = null;
            BinaryReader reader = null;
            byte[] blob = null;
            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                reader = new BinaryReader(fs);
                blob = reader.ReadBytes((int)fs.Length);

                string contentType = BusinessBase.GetContentType(Path.GetExtension(xiFileName));

                BusinessBase.StoreSystemFile(xiFileName, contentType, blob);
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                }
                blob = null;
            }
        }

        public string GenerateTempFile(string xiFileName)
        {
            return GenerateTempFile(xiFileName, false);
        }

        public string GenerateTempFile(string xiFileName, bool xiForceGenerate)
        {
            if (string.IsNullOrEmpty(xiFileName)) return "blank.png";
            if (xiFileName == "blank.png") return xiFileName;

            string tempfilename = string.Empty;
            try
            {
                if (xiForceGenerate == false) // forcegenerate is false, check file exists
                {
                    // file found, dont generate again
                    if (File.Exists(Path.Combine(this.FileUploadTempPath, xiFileName))) return xiFileName;
                }

                byte[] image64 = BusinessBase.GetSystemFileByteArray(xiFileName);
                if (image64 != null && image64.Length > 0)
                {
                    string extension = Path.GetExtension(xiFileName);
                    xiFileName = xiFileName.ToLower().Replace(extension, "").Replace("-", "_").Replace(" ", "_");
                    xiFileName = System.Text.RegularExpressions.Regex.Replace(xiFileName, "[^a-z0-9_]+", "");

                    tempfilename = Guid.NewGuid().ToString().ToLower() + "_" + xiFileName + extension;
                    string tempfilepath = Path.Combine(this.FileUploadTempPath, tempfilename);
                    File.WriteAllBytes(tempfilepath, image64);
                }
            }
            catch { tempfilename = string.Empty; }

            if (string.IsNullOrEmpty(tempfilename)) tempfilename = "blank.png";
            return tempfilename;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string getbase64url(string xiFileName)
        {
            string returnPath = string.Empty;

            try
            {
                string extension = Path.GetExtension(xiFileName);
                if (!string.IsNullOrEmpty(extension)) extension = extension.ToLower();

                if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif")
                {
                    returnPath = BusinessBase.GetBase64ImageString(xiFileName);
                }
            }
            catch { }
            finally
            {
                if (string.IsNullOrEmpty(returnPath)) returnPath = "../images/image_loading.gif";
            }
            return returnPath;
        }

        #endregion
    }
}