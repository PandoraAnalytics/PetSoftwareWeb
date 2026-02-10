using BADBUtils;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;

namespace BABusiness
{
    public class BusinessBase
    {
        public static string ApplicationBasePath = string.Empty;
        public enum WHERECLAUSE
        {
            WHERE,
            AND
        }

        public static string DateFormat
        {
            get
            {
                string temp = string.Empty;
                try
                {
                    temp = ConvertToString(GetDataSlot("dtformat"));
                }
                catch { temp = string.Empty; }
                finally
                {
                    if (string.IsNullOrEmpty(temp)) temp = "dd.MM.yyyy";
                }
                return temp;
            }
            set
            {
                SetDataSlot("dtformat", value);
            }
        }

        public static string DateTimeFormat
        {
            get
            {
                string temp = string.Empty;
                try
                {
                    temp = ConvertToString(GetDataSlot("dtformat")) + " HH:mm";
                }
                catch { temp = string.Empty; }
                finally
                {
                    if (string.IsNullOrEmpty(temp)) temp = "dd.MM.yyyy HH:mm";
                }
                return temp;
            }
        }

        public static string Timezone
        {
            get
            {
                string temp = string.Empty;
                try
                {
                    temp = ConvertToString(GetDataSlot("timezone"));
                }
                catch { temp = string.Empty; }
                finally
                {
                    if (string.IsNullOrEmpty(temp)) temp = "Central European Standard Time";
                }
                return temp;
            }
            set
            {
                SetDataSlot("timezone", value);
                BADBUtils.Utils.Timezone = value;
            }
        }

        public static DateTime Now
        {
            get
            {
                if (string.IsNullOrEmpty(BusinessBase.Timezone))
                {
                    try
                    {
                        BusinessBase.Timezone = "Central European Standard Time";
                    }
                    catch { }
                }
                string timezonestring = string.IsNullOrEmpty(BusinessBase.Timezone) ? "Central European Standard Time" : BusinessBase.Timezone;
                TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById(timezonestring);
                return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, timezone);
            }
        }

        public static CultureInfo USCulture = new CultureInfo("en-US");

        public static string FixedSaltKey = string.Empty;
        public static string FixedDocumentHashKey = string.Empty;

        public static string ConvertToString(object xiObj)
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

        public static int ConvertToInteger(object xiObj)
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

        protected static double ConvertToDouble(object xiObj, IFormatProvider xiProvider = null)
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

        public static CultureInfo GetCulture()
        {
            CultureInfo cultureinfo = null;
            string dateformat = BusinessBase.DateFormat;
            switch (dateformat)
            {
                default:
                    cultureinfo = new CultureInfo("de-DE");
                    break;

                case "MM/dd/yyyy":
                    cultureinfo = new CultureInfo("es-US");
                    break;

                case "dd/MM/yyyy":
                    cultureinfo = new CultureInfo("en-GB");
                    break;

                case "yyyy-M-d":
                    cultureinfo = new CultureInfo("zh-CN");
                    break;

                case "yyyy/MM/dd":
                    cultureinfo = new CultureInfo("en-ZA");
                    break;

                case "yyyy.MM.dd":
                    cultureinfo = new CultureInfo("hu-HU");
                    break;

                case "yyyy-MM-dd":
                    cultureinfo = new CultureInfo("fr-CA");
                    break;

            }
            BADBUtils.Utils.culutreinfo = cultureinfo;
            return cultureinfo;
        }

        public static string GetCultureDateFormat()
        {
            string cultureinfo = string.Empty;

            switch (BusinessBase.DateFormat)
            {
                default:
                    cultureinfo = "de-DE";
                    break;

                case "MM/dd/yyyy":
                    cultureinfo = "es-US";
                    break;

                case "dd/MM/yyyy":
                    cultureinfo = "en-GB";
                    break;

                case "yyyy-M-d":
                    cultureinfo = "zh-CN";
                    break;

                case "yyyy/MM/dd":
                    cultureinfo = "en-ZA";
                    break;

                case "yyyy.MM.dd":
                    cultureinfo = "hu-HU";
                    break;

                case "yyyy-MM-dd":
                    cultureinfo = "fr-CA";
                    break;
            }

            return cultureinfo;
        }

        public static void SecurePrimaryKey(string xiHashKey, ref DataSet xiDataSet, int xiTableIndex = 0, string xiColumnName = "id")
        {
            if (xiDataSet != null && xiDataSet.Tables.Count > xiTableIndex)
            {
                DataTable table = xiDataSet.Tables[xiTableIndex];
                if (!table.Columns.Contains(xiColumnName)) return;

                table.Columns.Add("securedid", typeof(string));
                foreach (DataRow row in table.Rows)
                {
                    if (row[xiColumnName] != DBNull.Value && row[xiColumnName] != null) row["securedid"] = BASecurity.Encrypt(Convert.ToString(row[xiColumnName]), xiHashKey);
                }
            }
        }

        public static string[] SplitSort(string xiSort)
        {
            string[] returnList = new string[2];
            returnList[0] = string.Empty;
            returnList[1] = string.Empty;

            if (string.IsNullOrEmpty(xiSort)) return returnList;

            string[] splitList = xiSort.Split('_');

            if (splitList != null)
            {
                if (splitList.Length > 0) returnList[0] = splitList[0];
                if (splitList.Length > 1) returnList[1] = (splitList[1] == "1") ? "DESC" : "";
            }

            return returnList;
        }

        public static bool IsEmail(string xiInputString)
        {
            if (string.IsNullOrEmpty(xiInputString)) return false;

            //Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            /*string emailPattern = @"^([0-9a-zA-Z]" + @"([\+\-_\.][0-9a-zA-Z]+)*" + @")+" + @"@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$";
            Regex regex = new Regex(emailPattern);
            Match match = regex.Match(xiInputString.Trim());*/

            if (xiInputString.Trim().EndsWith(".")) return false; // suggested by @TK-421

            try
            {
                var addr = new System.Net.Mail.MailAddress(xiInputString);
                return addr.Address == xiInputString;
            }
            catch
            {
                return false;
            }
        }

        private static void SetDataSlot(string xiSlotName, object xiobjValue)
        {
            try
            {
                HttpContext.Current.Session[xiSlotName] = xiobjValue;
            }
            catch { }
        }

        private static object GetDataSlot(string xiSlotName)
        {
            try
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Session[xiSlotName];
                }
                else
                {
                    return System.Threading.Thread.GetData(System.Threading.Thread.GetNamedDataSlot(xiSlotName));
                }
            }
            catch { }
            return null;
        }

        public static DateTime ConvertToUTC(DateTime xiDateTime)
        {
            if (xiDateTime == DateTime.MinValue) return xiDateTime;

            TimeZoneInfo usertimezone = TimeZoneInfo.FindSystemTimeZoneById(BusinessBase.Timezone);
            return TimeZoneInfo.ConvertTimeToUtc(xiDateTime, usertimezone);
        }

        public static DateTime ConvertFromUTCToUserTimeZone(object xiObj)
        {
            DateTime newTime = DateTime.MinValue;
            try
            {
                if (xiObj == null || xiObj == DBNull.Value) return newTime;

                string tempdate = xiObj.ToString();
                tempdate = tempdate.Replace("T", " ").Replace("Z", "");
                // user-specified time zone
                TimeZoneInfo usertimezone = TimeZoneInfo.FindSystemTimeZoneById(BusinessBase.Timezone);

                // an UTC DateTime
                DateTime utcTime = Convert.ToDateTime(tempdate);
                newTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, usertimezone);
            }
            catch (Exception e)
            {
                return newTime;
            }

            return newTime;
        }

        public static string GetFileSize(string xiFileFullName)
        {
            try
            {
                string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };

                System.IO.FileInfo f = new System.IO.FileInfo(xiFileFullName);

                int counter = 0;
                decimal number = (decimal)f.Length;
                while (Math.Round(number / 1024) >= 1)
                {
                    number = number / 1024;
                    counter++;
                }
                return string.Format("{0:n1} {1}", number, suffixes[counter]).Replace(",", ".");
            }
            catch { }

            return string.Empty;

        }
        //newly added 21 june 2024 nilesh
        public static string MaskText(object xiInputText)
        {
            string input = BusinessBase.ConvertToString(xiInputText);
            if (string.IsNullOrEmpty(input)) return string.Empty;
            input = input.Trim();
            if (input.Length == 0) return string.Empty;

            string returnstring = string.Empty;

            string[] names = input.Split(' ');
            foreach (string name in names)
            {
                string temp = string.Empty;

                if (string.IsNullOrEmpty(name)) continue;
                if (name.Trim().Length == 0) continue;

                if (name.Trim().Length <= 3) temp = new String('*', name.Trim().Length);
                else temp = new String('*', name.Trim().Length - 1) + name.Trim().Substring(name.Trim().Length - 1);

                if (returnstring.Length > 0) returnstring += " ";
                returnstring += temp;
            }

            //if (BusinessBase.Department == 2) // only Admin can see actual names on hover
            //{
            //    if (!string.IsNullOrEmpty(returnstring))
            //    {
            //        returnstring = string.Format("<span title='{0}'>{1}</span>", xiInputText, returnstring);
            //    }
            //}

            return returnstring;
        }

        public static void Write_log_file(string sfunc, string sMsg)
        {
            string path = Path.Combine(ApplicationBasePath, "Log");
            StreamWriter SW = null;
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                SW = new StreamWriter(Path.Combine(path, "logs.txt"), true);
                SW.WriteLine(sfunc + "  >> " + sMsg + " >> " + DateTime.Now.ToString());
            }
            catch { }
            finally
            {
                if (SW != null)
                {
                    SW.Close();
                    SW.Dispose();
                }
                SW = null;
            }
        }

        #region Blob

        public static bool StoreSystemFile(string xiFileName, string xiContenttype, byte[] xiBlobValue)
        {
            if (xiBlobValue == null || xiBlobValue.Length == 0) return false;

            string query = "insert into [system_files] ([filenamevalue],[contenttype], [blobfile]) values(@filenamevalue,@contenttype,@blobfile)";

            Parameter param1 = new Parameter("filenamevalue", xiFileName, DbType.String);
            Parameter param2 = new Parameter("contenttype", xiContenttype, DbType.String);
            Parameter param3 = new Parameter("blobfile", xiBlobValue, DbType.Binary);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static byte[] GetSystemFileByteArray(string xiFilename)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            System.Data.SqlClient.SqlDataReader reader1 = null;
            byte[] bytes = null;

            string query = "select top 1 blobfile from system_files where filenamevalue = " + Utils.ConvertToDBString(xiFilename.Trim(), Utils.DataType.String) + " order by fileid desc";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) bytes = (byte[])reader1["blobfile"];
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetBlobFileByID", x.Message);
            }
            finally
            {
                if (reader1 != null)
                {
                    reader1.Close();
                    reader1 = null;
                }
                objdb.Disconnectdb();
            }

            return bytes;
        }

        public static string GetContentType(string xiFileExtension)
        {
            if (string.IsNullOrEmpty(xiFileExtension)) return "text/plain";

            string ext = xiFileExtension.ToLower();
            string retValue = null;
            switch (ext)
            {
                case ".txt":
                    retValue = "text/plain";
                    break;

                case ".docx":
                    retValue = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;

                case ".doc":
                    retValue = "application/msword";
                    break;

                case ".pdf":
                    retValue = "application/pdf";
                    break;

                case ".jpg":
                    retValue = "image/jpeg";
                    break;

                case ".jpeg":
                    retValue = "image/jpeg";
                    break;

                case ".bmp":
                    retValue = "image/bmp";
                    break;

                case ".png":
                    retValue = "image/png";
                    break;

                case ".xls":
                    retValue = "application/vnd.ms-excel";
                    break;

                case ".xlsx":
                    retValue = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;

                case ".rtf":
                    retValue = "application/rtf";
                    break;

                case ".ppt":
                    retValue = "application/vnd.ms-powerpoint";
                    break;

                case ".pptx":
                    retValue = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;

                case ".zip":
                    retValue = "application/zip";
                    break;

                case ".xml":
                    retValue = "application/xml";
                    break;
            }

            return retValue;
        }

        public static string GetBase64ImageString(string xiFileName)
        {
            string query = "select top 1 blobfile from system_files where filenamevalue = " + Utils.ConvertToDBString(xiFileName, Utils.DataType.String) + " order by fileid desc";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            object value = objdb.ExecuteScalar(objdb.con, query);
            objdb.Disconnectdb();

            if (value == null || value == DBNull.Value) return string.Empty;

            try
            {
                return "data:image/png;base64," + Convert.ToBase64String((byte[])value);
            }
            catch { }

            return string.Empty;
        }

        public static bool DeleteSystemFile(string xiFileName)
        {
            string query = "delete from [system_files] where filenamevalue=@filenamevalue";

            Parameter param1 = new Parameter("filenamevalue", xiFileName, DbType.String);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        #endregion

    }
}
