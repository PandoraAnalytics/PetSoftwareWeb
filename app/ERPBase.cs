using BABusiness;
using NReco.PdfGenerator;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;

namespace Breederapp
{
    public class ERPBase : System.Web.UI.Page
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

        public string FileUploadPath
        {
            get { return System.Web.Hosting.HostingEnvironment.MapPath("~") + @"/app/docs/"; }
        }


        virtual protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.UserId))
            {
                Response.Redirect("signin.aspx");
            }

            if (string.IsNullOrEmpty(this.CompanyId))
            {
                Response.Redirect("landing.aspx?access=true");
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

        protected string GetCurrntBUCurrency()
        {
            string bucurrency = string.Empty;

            NameValueCollection currencycollection = UserBA.GetBusinessUserDetail(this.CompanyId);
            if (currencycollection != null) bucurrency = currencycollection["currencyname"];
            currencycollection = null;
            if (string.IsNullOrEmpty(bucurrency)) bucurrency = "EUR";

            return bucurrency;
        }

        protected string GetCurrntBUTax()
        {
            string butax = string.Empty;

            NameValueCollection taxcollection = UserBA.GetBusinessUserDetail(this.CompanyId);
            if (taxcollection != null) butax = taxcollection["taxid"];
            taxcollection = null;

            return butax;
        }

        public string ProcessPrintOrderDetails(object xiOrderId)
        {
            string orderId = this.ConvertToString(xiOrderId);
            if (string.IsNullOrEmpty(orderId)) return null;

            double dynamicTotal = 0;

            string defaultCurrency = this.GetCurrntBUCurrency();

            NameValueCollection orderCollection = BUOrderManagement.GetOrderDetails(xiOrderId, this.CompanyId);
            if (orderCollection == null) return null;

            NameValueCollection ccollection = UserBA.GetBusinessUserDetail(this.CompanyId);
            //string baseURL = "https://pets.software";//for live
            //string baseURL2 = "https://localhost:44335"; //test for local
            string logoPath = "images/logo.png";

            if (!string.IsNullOrEmpty(ccollection["companylogo"]))
            {
                //logoPath = "/app/docs/temp/" + PageBase.getbase64url(ccollection["companylogo"]);
                logoPath = PageBase.getbase64url(ccollection["companylogo"]);
            }
            else
            {
                logoPath = "/app/images/defcomplogo2.png";
            }
            string orderNo = "#" + orderCollection["orderno"].PadLeft(5, '0');
            string fileName = orderNo.ToLower() + "-" + BusinessBase.Now.Ticks.ToString(); ;
            string htmlfilepath = Server.MapPath("~") + @"\app\docs\files\order_invoice.html";
            string pdfFilePath = this.FileUploadTempPath + fileName + ".pdf";

            try
            {
                StreamReader htmlReader = new StreamReader(new FileStream(htmlfilepath, FileMode.Open, FileAccess.Read));
                string mainHTML = htmlReader.ReadToEnd();
                htmlReader.Close();
                htmlReader = null;

                mainHTML = mainHTML.Replace("\r\n", "");
                mainHTML = mainHTML.Replace("\0", "");

                NameValueCollection collection = new NameValueCollection();
                collection["#companyname"] = ccollection["companyname"];
                collection["#add1"] = ccollection["address"];
                collection["#contactno"] = ccollection["phone"];
                collection["#email"] = ccollection["email"];
                collection["#website"] = ccollection["website"];


                //collection["#orderno"] = "#" + orderCollection["orderno"].PadLeft(5, '0');
                collection["#customername"] = orderCollection["cfname"] + " " + orderCollection["clname"];
                collection["#customeremail"] = orderCollection["cemail"];
                collection["#customercontactno"] = orderCollection["cphone"];
                collection["#billto_address"] = orderCollection["caddress"];

                //invoice details
                collection["#invnumber"] = "#" + orderCollection["orderno"].PadLeft(5, '0');
                collection["#todaydate"] = BusinessBase.Now.ToString(this.DateFormat);
                collection["#duedate"] = BusinessBase.Now.ToString(this.DateFormat);
                //if (string.IsNullOrEmpty(orderCollection["orderdate"]) == false)
                //{
                //    DateTime dtTempDateTime = DateTime.Parse(orderCollection["orderdate"]);
                //    collection["#orderdate"] = dtTempDateTime.ToString(this.DateFormat);
                //}
                //else
                //{
                //    collection["#orderdate"] = "";
                //}

                string status = "";
                switch (orderCollection["status"])
                {
                    case "1":
                        status = "Processing";
                        break;
                    case "2":
                        status = "Completed";
                        break;
                    case "3":
                        status = "Deleted";
                        break;

                }
                collection["#status"] = status;

                //order details
                DataTable dataTable = BUOrderManagement.GetOrderItems(orderId);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    string temp = "<tr><td width='30%'>{0}</td><td width='10%'>{1}</td><td width='15%'>{2}</td><td width='15%'>{3}</td><td width='15%'>{4}</td><td width='15%'>{5}</td></tr>";
                    StringBuilder builder = new StringBuilder();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string itemcost = (Convert.ToDecimal(row["itemcost"]).ToString("0.00")).Replace(",", ".");
                        string totalcost = (Convert.ToDecimal(row["totalcost"]).ToString("0.00")).Replace(",", ".");
                        string itemtotalcost = (Convert.ToDecimal(row["itemtotalcost"]).ToString("0.00")).Replace(",", ".");

                        //builder.Append(string.Format(temp, row["ItemNameType"], row["quan"], defaultCurrency + " " + row["itemcost"].ToString().Replace(",", "."), row["taxname"] + "(" + row["tpercentage"] + "%)", defaultCurrency + " " + row["totalcost"].ToString().Replace(",", "."), defaultCurrency + " " + row["itemtotalcost"].ToString().Replace(",", ".")));
                        builder.Append(string.Format(temp, row["ItemNameType"], row["quan"], defaultCurrency + " " + itemcost, row["taxname"] + "(" + row["tpercentage"] + "%)", defaultCurrency + " " + totalcost, defaultCurrency + " " + itemtotalcost));
                    }
                    collection["####orderdetailsfields###"] = builder.ToString();
                }
                else
                {
                    collection["####orderdetailsfields###"] = "";
                }

                // total amount details 
                NameValueCollection costcollection = BUOrderManagement.GetCurrentOrderCost(ViewState["orderid"]);
                if (costcollection != null)
                {
                    collection["#subtotal"] = defaultCurrency + " " + Convert.ToDecimal(costcollection["total_item_cost"]).ToString("0.00").Replace(",", ".");
                    collection["#totaltax"] = defaultCurrency + " " + Convert.ToDecimal(costcollection["total_tax_amount"]).ToString("0.00").Replace(",", "."); 
                    collection["#total"] = defaultCurrency + " " + Convert.ToDecimal(costcollection["total_cost_with_tax"]).ToString("0.00").Replace(",", "."); 
                    ViewState["ordertotalamount"] = collection["#total"];
                }
                else
                {
                    collection["#subtotal"] = "-";
                    collection["#totaltax"] = "-";
                    collection["#total"] = "-";
                }

                string isNegValue = string.Empty;
                DataTable headTable = BUOrderManagement.GetHeadDetails(ViewState["orderid"]);
                if (costcollection != null && headTable != null && headTable.Rows.Count > 0)
                {
                    string temp2 = "<tr><td width='30%'>{0}</td><td width='20%'>{1}</td></tr>";
                    StringBuilder builder2 = new StringBuilder();

                    foreach (DataRow row in headTable.Rows)
                    {
                        double cost = this.ConvertToDouble(row["cost"].ToString().Replace(",", "."));
                        int isNegative = this.ConvertToInteger(row["isnegative"]);

                        if (isNegative == 1)
                        {
                            isNegValue = "(-)";
                            dynamicTotal -= cost;
                        }
                        else if (isNegative == 0)
                        {
                            isNegValue = "(+)";
                            dynamicTotal += cost;
                        }

                        string headcost = (Convert.ToDecimal(row["cost"]).ToString("0.00")).Replace(",", ".");

                        builder2.Append(string.Format(temp2, row["orderhead"], defaultCurrency + " " + isNegValue + " " + headcost));

                    }
                    collection["####headdetails###"] = builder2.ToString();

                    collection["#total"] = defaultCurrency + " " + (this.ConvertToDouble(costcollection["total_cost_with_tax"].Replace(",", ".")) + this.ConvertToDouble(dynamicTotal)).ToString("0.00").Replace(",", ".");

                }
                else
                {
                    collection["####headdetails###"] = "";
                }

                switch (orderCollection["paymentoption"])
                {
                    case "1":
                        collection["#paymentoption"] = "Cash";
                        break;

                    case "2":
                        collection["#paymentoption"] = "Credit Card";
                        break;

                    case "3":
                        collection["#paymentoption"] = "Debit Card";
                        break;

                    case "4":
                        collection["#paymentoption"] = "Bank Transfer";
                        break;

                    case "5":
                        collection["#paymentoption"] = "E-Wallet";
                        break;

                    case "6":
                        collection["#paymentoption"] = "Mobile";
                        break;

                    case "7":
                        collection["#paymentoption"] = "Prepaid Cards";
                        break;
                }

                collection["#termscondition"] = orderCollection["termscondition"];


                collection["#owner_name"] = ccollection["fname"] + " " + ccollection["lname"];
                collection["#owner_vatinNo"] = ccollection["tinno"];
                collection["#owner_registryno"] = ccollection["registrationno"];

                foreach (string key in collection.Keys)
                {
                    mainHTML = mainHTML.Replace(key, collection[key]);
                }

                //string componyLogoPath = $"{baseURL}/{logoPath}";// for live  baseURL   
                //string componyLogoPath = $"{baseURL2}/{logoPath}"; // for local baseURL2

                string componyLogoPath = $"{logoPath}";
                mainHTML = mainHTML.Replace("#complogo", componyLogoPath);

                HtmlToPdfConverter htmlbuilder = new HtmlToPdfConverter();

                byte[] bytefile = htmlbuilder.GeneratePdf(mainHTML);

                if (bytefile != null && bytefile.Length > 0) File.WriteAllBytes(pdfFilePath, bytefile);
            }
            catch (Exception ex)
            {
                BusinessBase.Write_log_file("PdfFile --", ex.Message);
                fileName = string.Empty;
            }

            return fileName;
        }

    }
}