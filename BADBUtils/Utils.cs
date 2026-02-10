using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BADBUtils
{
    public abstract class Utils
    {
        # region Constants and enums

        public enum DataType
        {
            None,
            Integer,
            String,
            Float,
            Boolean,
            Date,
            DateTime,
            DateTimeNonUTC,
            Time,
            Double,
            Decimal,
            Long,
            Encrypted
        };

        public static string FixedSaltKey = System.Configuration.ConfigurationManager.AppSettings["fixedsaltkey"];

        public static CultureInfo culutreinfo = null;
        public static string Timezone
        {
            get
            {
                string temp = string.Empty;
                try
                {
                    temp = Convert.ToString(GetDataSlot("timezone"));
                }
                catch { }
                finally
                {
                    if (string.IsNullOrEmpty(temp)) temp = "Central European Standard Time";
                }

                return temp;
            }
            set
            {
                SetDataSlot("timezone", value);
            }
        }

        #endregion

        # region Method to convert different types to DB

        public static string ConvertToDBString(object xiValue, DataType xiDataType, bool xiRequired = false)
        {
            CultureInfo USCulture = new CultureInfo("en-US");
            string[] cultures = { "de-DE", "es-US", "en-GB", "zh-CN", "ms-MY", "ru-RU", "th-TH", "tr-TR", "vi-VN", "it-IT", "zh-TW", "en-ZA", "hu-HU", "fr-CA", "ar-AE", "es-ES", "ja-JP", "ko-KR", "bn-BD", "pt-BR", "fr-FR" };
            string returnString = Convert.ToString(xiValue);

            switch (xiDataType)
            {
                case DataType.Integer:
                    if (string.IsNullOrEmpty(Convert.ToString(xiValue)))
                    {
                        returnString = (xiRequired) ? "0" : "NULL";
                        break;
                    }

                    int tempInt = Convert.ToInt32(xiValue);
                    if (tempInt == int.MinValue) returnString = (xiRequired) ? "0" : "NULL";
                    break;

                case DataType.Boolean:
                    bool tempBool = Convert.ToBoolean(xiValue);
                    returnString = tempBool ? "1" : "0";
                    break;


                case DataType.Double:
                    if (xiValue == null) returnString = "NULL";

                    string tempDoubleString = xiValue.ToString().Replace(',', '.');
                    if (tempDoubleString.Length > 0)
                    {
                        double tempDouble = Convert.ToDouble(tempDoubleString, USCulture);
                        if (tempDouble == double.MinValue) returnString = "NULL";
                        else returnString = tempDouble.ToString(USCulture);
                    }
                    else
                    {
                        returnString = "NULL";
                    }
                    break;

                case DataType.Decimal:
                    if (xiValue == null) returnString = "NULL";

                    string tempDecimalString = xiValue.ToString().Replace(',', '.');
                    if (tempDecimalString.Length > 0)
                    {
                        decimal tempDec = Convert.ToDecimal(tempDecimalString, USCulture);
                        if (tempDec == decimal.MinValue) returnString = "NULL";
                        else returnString = tempDec.ToString(USCulture);
                    }
                    else
                    {
                        returnString = "NULL";
                    }
                    break;

                case DataType.Long:
                    if (string.IsNullOrEmpty(Convert.ToString(xiValue)))
                    {
                        returnString = (xiRequired) ? "0" : "NULL";
                        break;
                    }

                    long tempLong = Convert.ToInt64(xiValue);
                    if (tempLong == long.MinValue) returnString = "NULL";
                    break;

                case DataType.Date:
                    if (string.IsNullOrEmpty(Convert.ToString(xiValue)))
                    {
                        returnString = (xiRequired) ? "'0001-01-01'" : "NULL";
                        break;
                    }

                    DateTime tempDate = DateTime.MinValue;
                    if (culutreinfo != null)
                    {
                        tempDate = Convert.ToDateTime(xiValue, culutreinfo);
                    }
                    else
                    {
                        foreach (string c in cultures)
                        {
                            try
                            {
                                CultureInfo info = new CultureInfo(c);
                                tempDate = Convert.ToDateTime(xiValue, info);
                            }
                            catch { }
                            if (tempDate != DateTime.MinValue) break;
                        }
                    }
                    if (tempDate == DateTime.MinValue)
                    {
                        returnString = (xiRequired) ? "'0001-01-01'" : "NULL";
                    }
                    else
                    {
                        //TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById(Utils.Timezone);
                        //tempDate = TimeZoneInfo.ConvertTime(tempDate, timezone, TimeZoneInfo.Utc);
                        returnString = string.Format("'{0}'", tempDate.ToString("yyyy-MM-dd"));
                    }
                    break;

                case DataType.Time:
                    if (string.IsNullOrEmpty(Convert.ToString(xiValue)))
                    {
                        returnString = (xiRequired) ? "'00:00:00'" : "NULL";
                        break;
                    }

                    DateTime tempTime = Convert.ToDateTime(xiValue, CultureInfo.CurrentCulture);
                    if (tempTime == DateTime.MinValue)
                    {
                        returnString = (xiRequired) ? "'00:00:00'" : "NULL";
                    }
                    else
                    {
                        TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById(Utils.Timezone);
                        tempTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(tempTime, DateTimeKind.Unspecified), timezone);
                        returnString = string.Format("'{0}'", tempTime.ToString("HH:mm"));
                    }
                    break;

                case DataType.DateTime:
                case DataType.DateTimeNonUTC:
                    if (string.IsNullOrEmpty(Convert.ToString(xiValue)))
                    {
                        returnString = (xiRequired) ? "'0001-01-01 00:00:00'" : "NULL";
                        break;
                    }

                    DateTime tempDateTime = DateTime.MinValue;
                    if (culutreinfo != null)
                    {
                        tempDateTime = Convert.ToDateTime(xiValue, culutreinfo);
                    }
                    else
                    {
                        foreach (string c in cultures)
                        {
                            try
                            {
                                CultureInfo info = new CultureInfo(c);
                                tempDateTime = Convert.ToDateTime(xiValue, info);
                            }
                            catch { }
                            if (tempDateTime != DateTime.MinValue) break;
                        }
                    }

                    if (tempDateTime == DateTime.MinValue)
                    {
                        returnString = (xiRequired) ? "'0001-01-01 00:00:00'" : "NULL";
                    }
                    else
                    {
                        TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById(Utils.Timezone);
                        //DateTime tempDateTime1 = TimeZoneInfo.ConvertTime(tempDateTime, timezone, TimeZoneInfo.Utc);
                        tempDateTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(tempDateTime, DateTimeKind.Unspecified), timezone);
                        returnString = string.Format("'{0}'", tempDateTime.ToString("yyyy-MM-dd HH:mm"));
                    }
                    break;


                case DataType.String:
                    string tempString = Convert.ToString(xiValue);
                    if (string.IsNullOrEmpty(tempString))
                    {
                        returnString = (xiRequired) ? "''" : "NULL";
                    }
                    else
                    {
                        returnString = returnString.Replace("'", "''");
                        returnString = string.Format("N'{0}'", returnString);
                    }
                    break;

                case DataType.Encrypted:
                    string tempString2 = Convert.ToString(xiValue);
                    if (string.IsNullOrEmpty(tempString2))
                    {
                        returnString = (xiRequired) ? "''" : "NULL";
                    }
                    else
                    {
                        returnString = tempString2.Replace("'", "''");
                        returnString = string.Format("dbo.EncryptData('{0}', '{1}')", returnString, Utils.FixedSaltKey);
                    }
                    break;
            }

            return returnString;
        }

        public static object ConvertToDateTime(object xiObj)
        {
            DateTime newTime = DateTime.MinValue;
            try
            {
                if (xiObj == null || xiObj == DBNull.Value) return xiObj;

                // user-specified time zone
                TimeZoneInfo usertimezone = TimeZoneInfo.FindSystemTimeZoneById(Utils.Timezone);

                // an UTC DateTime
                DateTime utcTime = Convert.ToDateTime(xiObj);
                newTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, usertimezone);
            }
            catch (Exception e)
            {
                return xiObj;
            }

            return newTime;
        }

        public static object ConvertToDateTimeSpan(object xiObj)
        {
            DateTime newTime = DateTime.MinValue;
            try
            {
                if (xiObj == null || xiObj == DBNull.Value) return xiObj;

                // user-specified time zone
                TimeZoneInfo usertimezone = TimeZoneInfo.FindSystemTimeZoneById(Utils.Timezone);

                // an UTC DateTime
                DateTime utcTime = new DateTime(TimeSpan.Parse(xiObj.ToString()).Ticks);
                newTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, usertimezone);
            }
            catch (Exception e)
            {
                return xiObj;
            }

            return new TimeSpan(newTime.Ticks);
        }

        #endregion

        #region Other Methods

        private static void SetDataSlot(string xiSlotName, object xiobjValue)
        {
            try
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Session[xiSlotName] = xiobjValue;
                }
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

        #endregion

        #region Encrypt-Decrypt Method

        public static string ColumnToDecrypt(string xiQuery)
        {
            string[] array = new string[4];
            array[0] = "fname";
            array[1] = "lname";
            array[2] = "email";
            array[3] = "phone";

            try
            {
                foreach (string s in array)
                {
                    string pattern = string.Format(@"\w+[.]?{0}?", s);
                    Regex rg = new Regex(pattern);
                    MatchCollection matches = rg.Matches(xiQuery);
                    if (matches.Count > 0) xiQuery = xiQuery.Replace(matches[0].Value, "dbo.DecryptData(" + matches[0].Value + ", '" + Utils.FixedSaltKey + "')");
                }
            }
            catch { }

            return xiQuery;
        }

        public static string DecryptCombined(string xiFromSqlString, string xiPassword)
        {
            if (string.IsNullOrEmpty(xiFromSqlString)) return xiFromSqlString;

            // pattern for a word that starts with "01000000"  
            string pattern = @"\b[01000000]\w+";
            Regex rg = new Regex(pattern);

            MatchCollection matches = rg.Matches(xiFromSqlString);

            foreach (Match m in matches)
            {
                string temp = ProcessDecryptString(m.Value, xiPassword);
                if (!string.IsNullOrEmpty(temp)) xiFromSqlString = xiFromSqlString.Replace(m.Value, temp);
            }

            return xiFromSqlString;
        }

        private static string ProcessDecryptString(string xiFromSqlString, string xiPassword)
        {
            if (string.IsNullOrEmpty(xiFromSqlString)) xiFromSqlString = string.Empty;

            string inputstring = xiFromSqlString;
            try
            {
                // Encode password as UTF16-LE
                byte[] passwordBytes = Encoding.Unicode.GetBytes(xiPassword);

                // Remove leading "0x"
                //FromSql = FromSql.Substring(2);

                int version = BitConverter.ToInt32(StringToByteArray(inputstring.Substring(0, 8)), 0);
                byte[] encrypted = null;

                HashAlgorithm hashAlgo = null;
                SymmetricAlgorithm cryptoAlgo = null;
                int keySize = (version == 1 ? 16 : 32);

                if (version == 1)
                {
                    hashAlgo = SHA1.Create();
                    cryptoAlgo = TripleDES.Create();
                    cryptoAlgo.IV = StringToByteArray(inputstring.Substring(8, 16));
                    encrypted = StringToByteArray(inputstring.Substring(24));
                }
                else if (version == 2)
                {
                    hashAlgo = SHA256.Create();
                    cryptoAlgo = Aes.Create();
                    cryptoAlgo.IV = StringToByteArray(inputstring.Substring(8, 32));
                    encrypted = StringToByteArray(inputstring.Substring(40));
                }
                else
                {
                    return xiFromSqlString;
                }

                cryptoAlgo.Padding = PaddingMode.PKCS7;
                cryptoAlgo.Mode = CipherMode.CBC;

                hashAlgo.TransformFinalBlock(passwordBytes, 0, passwordBytes.Length);
                cryptoAlgo.Key = hashAlgo.Hash.Take(keySize).ToArray();

                byte[] decrypted = cryptoAlgo.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
                int decryptLength = BitConverter.ToInt16(decrypted, 6);
                UInt32 magic = BitConverter.ToUInt32(decrypted, 0);
                if (magic != 0xbaadf00d)
                {
                    return xiFromSqlString;
                }

                byte[] decryptedData = decrypted.Skip(8).ToArray();
                //bool isUtf16 = (Array.IndexOf(decryptedData, (byte)0) != -1);
                //inputstring = (isUtf16 ? Encoding.Unicode.GetString(decryptedData) : Encoding.UTF8.GetString(decryptedData));
                inputstring = Encoding.Unicode.GetString(decryptedData);
            }
            catch { inputstring = xiFromSqlString; }

            return inputstring;
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        #endregion
    }
}
