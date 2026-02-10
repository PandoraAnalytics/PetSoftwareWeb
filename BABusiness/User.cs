using BADBUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;

namespace BABusiness
{
    public class User
    {
        int userid = int.MinValue;
        string prename = string.Empty;
        string familyname = string.Empty;
        string initials = string.Empty;
        string emailaddress = string.Empty;
        string phone = string.Empty;
        int usercompany = int.MinValue;
        string defaultfolder = string.Empty;
        int userconfirmed = 0;
        int userstatus = 0;
        string usertoken = string.Empty;
        int superadmin = 0;
        string mainUserId = string.Empty;
        string userLang = string.Empty;
        string userdtFormat = string.Empty;
        string usertimeZone = string.Empty;

        public static string GetRandomPassword()
        {
            string possibleChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            Random rnd = new Random();
            StringBuilder ret = new StringBuilder();

            for (int i = 0; i < 6; i++)
                ret.Append(possibleChars.Substring(rnd.Next(possibleChars.Length), 1));

            return ret.ToString();
        }

        public int UserId
        {
            get { return userid; }
            set { userid = value; }
        }

        public string PreName
        {
            get { return prename; }
            set { prename = value; }
        }

        public string FamilyName
        {
            get { return familyname; }
            set { familyname = value; }
        }

        public string Initials
        {
            get { return initials; }
            set { initials = value; }
        }

        public string Emailaddress
        {
            get { return emailaddress; }
            set { emailaddress = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public int UserCompany
        {
            get { return usercompany; }
            set { usercompany = value; }
        }

        public string DefaultFolder
        {
            get { return defaultfolder; }
            set { defaultfolder = value; }
        }

        public int UserConfirmed
        {
            get { return userconfirmed; }
            set { userconfirmed = value; }
        }

        public int UserStatus
        {
            get { return userstatus; }
            set { userstatus = value; }
        }

        public string UserToken
        {
            get { return usertoken; }
            set { usertoken = value; }
        }

        public int SuperAdmin
        {
            get { return superadmin; }
            set { superadmin = value; }
        }

        public string FullName
        {
            get { return prename + " " + familyname; }
        }

        public string MainUserId
        {
            get { return mainUserId; }
            set { mainUserId = value; }
        }

        public string UserLang
        {
            get { return userLang; }
            set { userLang = value; }
        }

        public string UserDateFormat
        {
            get { return userdtFormat; }
            set { userdtFormat = value; }
        }

        public string UserTimeZone
        {
            get { return usertimeZone; }
            set { usertimeZone = value; }
        }

        public int Add(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string password = (string.IsNullOrEmpty(xiCollection["password"])) ? BABusiness.User.GetRandomPassword() : xiCollection["password"];
            string password_enc = BASecurity.HashPassword(password);

            string address = (string.IsNullOrEmpty(xiCollection["user_address"])) ? string.Empty : xiCollection["user_address"];
            string country = (string.IsNullOrEmpty(xiCollection["user_countryid"])) ? string.Empty : xiCollection["user_countryid"];
            string city = (string.IsNullOrEmpty(xiCollection["user_city"])) ? string.Empty : xiCollection["user_city"];
            string pincode = (string.IsNullOrEmpty(xiCollection["user_pincode"])) ? string.Empty : xiCollection["user_pincode"];
            string phonecountrycode = (string.IsNullOrEmpty(xiCollection["contactcountrycode"])) ? string.Empty : xiCollection["contactcountrycode"];

            string query = "INSERT INTO [user] ([fname],[lname],[email],[phone],address,[possword],[type],countryid,city,pincode,[submitdate],[active],[is_verified],[password_token],[user_token],[contactcountrycode],[user_token_date]) values(";
            query += Utils.ConvertToDBString(xiCollection["user_pre_name"], Utils.DataType.Encrypted) + ", ";
            query += Utils.ConvertToDBString(xiCollection["user_family_name"], Utils.DataType.Encrypted) + ", ";
            query += Utils.ConvertToDBString(xiCollection["user_email"], Utils.DataType.Encrypted) + ", ";
            query += Utils.ConvertToDBString(xiCollection["user_phone"], Utils.DataType.Encrypted) + ", ";
            query += Utils.ConvertToDBString(address, Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(password_enc, Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(xiCollection["user_type"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(country, Utils.DataType.Integer) + ", ";
            query += Utils.ConvertToDBString(city, Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(pincode, Utils.DataType.Integer) + ", ";
            query += "getutcdate(), ";
            query += "1, ";
            query += "1, ";
            query += Utils.ConvertToDBString(xiCollection["password_token"], Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(BusinessBase.Now.Ticks.ToString(), Utils.DataType.String) + ", ";
            query += Utils.ConvertToDBString(phonecountrycode, Utils.DataType.Integer) + ", ";
            query += "getutcdate()";
            query += ")";

            string query2 = "Select Scope_Identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int userId = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2);
            objdb.Disconnectdb();
            objdb = null;

            return userId;
        }

        public User ForgotPassword(string xiUsername)
        {
            User objUser = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;
            try
            {
                string selqry = "select id, email from [user] where dbo.DecryptData(email, @basekey) = @email";
                Parameter param1 = new Parameter("email", xiUsername);
                Parameter param2 = new Parameter("basekey", BusinessBase.FixedSaltKey);
                reader1 = objdb.ExecuteReader(objdb.con, selqry, new Parameter[] { param1, param2 });

                while (reader1.Read())
                {
                    objUser = new User();

                    objUser.MainUserId = Convert.ToString(objdb.GetValue(reader1, "id"));
                    objUser.Emailaddress = Convert.ToString(objdb.GetValue(reader1, "email"));
                    break;
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("ForgotPassword", x.Message);
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

            return objUser;
        }

        public bool RequestForgotPassword(string xiToken, object xiUserId)
        {
            string query = "update [user] set ";
            query += "user_token = " + Utils.ConvertToDBString(xiToken, Utils.DataType.String) + ", ";
            query += "user_token_date = getutcdate()";
            query += " where id = " + Utils.ConvertToDBString(xiUserId, Utils.DataType.String);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();
            return (value > 0);
        }

        public static int CheckEmailExist(string xiEmail)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(ua.id) as maxcount from [user] ua where ua.email=" + Utils.ConvertToDBString(xiEmail, Utils.DataType.String) + " and ua.active = 1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("CheckEmailExist", x.Message);
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

            return totalCount;
        }

        public static NameValueCollection GetUserByToken(object xiToken)
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            xiToken = Utils.ConvertToDBString(xiToken, Utils.DataType.String);
            string query = "select s.* from [user] s where s.user_token = " + xiToken + " and active = 1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                while (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("user_id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("user_token", Convert.ToString(objdb.GetValue(reader1, "user_token")));
                    collection.Add("user_email", Convert.ToString(objdb.GetValue(reader1, "user_email")));
                    collection.Add("user_token_date", Convert.ToString(objdb.GetValue(reader1, "user_token_date")));
                    break;
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetUserByToken", x.Message);
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

            return collection;
        }

        public bool ResetPassword(NameValueCollection xiCollection, object xiUserId)
        {
            if (xiCollection == null) return false;

            string query = "update [user] set [possword] = @pwd, user_token = null, user_token_date = null where id = @userid and active = 1";

            Parameter param1 = new Parameter("userid", xiUserId, System.Data.DbType.Int32);
            Parameter param2 = new Parameter("pwd", xiCollection["password"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static bool AddUserSessionLog(string xiUserEmail, string xiAction)
        {
            string query = "insert into user_session_log(user_email, [created_date], [action]) values(dbo.EncryptData(@useremail, @basekey),getutcdate(),@action)";

            Parameter param1 = new Parameter("useremail", xiUserEmail);
            Parameter param2 = new Parameter("action", xiAction);
            Parameter param3 = new Parameter("basekey", BusinessBase.FixedSaltKey);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();
            return (value > 0);
        }

        public static string[] GetUserIds(MailAddressCollection xiToCollection)
        {
            ArrayList userIdList = new ArrayList();
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string emails = string.Empty;
            foreach (MailAddress add in xiToCollection)
            {
                emails += Utils.ConvertToDBString(add.Address, Utils.DataType.String) + ",";
            }
            emails = emails.TrimEnd(',');
            string query = "select [id] from [user] where dbo.DecryptData(user_email, '" + BusinessBase.FixedSaltKey + "') in (" + emails + ") and active = 1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                while (reader1.Read())
                {
                    userIdList.Add(Convert.ToString(objdb.GetValue(reader1, "id")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetUserIds", x.Message);
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

            return (string[])userIdList.ToArray(typeof(string));
        }

        public static int GetUserId(object xiUserEmail)
        {
            int userID = int.MinValue;

            DBClass objdb = new DBClass();
            SqlDataReader reader1 = null;

            string query = "select s.[id] from [user] s where s.active = 1 and dbo.DecryptData(s.[email], '" + BusinessBase.FixedSaltKey + "') = " + Utils.ConvertToDBString(xiUserEmail, Utils.DataType.String);
            try
            {

                objdb.Connectdb();
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read()) userID = Convert.ToInt32(objdb.GetValue(reader1, "id"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetUserId", x.Message);
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

            return userID;
        }
        
    }
}
