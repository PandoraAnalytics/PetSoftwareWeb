using BADBUtils;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;

namespace BABusiness
{
    public class Common
    {
        public const int RECORDCOUNT = 30;

        public enum LoginStatus
        {
            NONE = -1,
            SUCCESS = 1,
            WRONGUSERORPASS = 2,
            USERDEACTIVATE = 3,
            USERNOTCONFIRMED = 4,
        }

        public enum AnimalLogCategory
        {
            ALL = 0,
            BASICINFO = 1,
            PARENTS = 2,
            OTHERINFO = 3,
            GALLERY = 4,
            APPOINTMENTS = 5,
            NOTES = 6,
            FOOD = 7,
            CERTIFICATES = 8,
            TRANSFER = 9,
            PDFREPORT = 10,
            DELETE = 11
        }

        public enum AnimalLogKey
        {
            AddBASICINFO = 1,
            EDITBASICINFO = 2,

            ADDPARENT = 3,
            EDITPARENT = 4,

            EDITOTHERINFO = 5,

            ADDGALLERY = 6,
            DELETEGALLERY = 7,

            ADDAPPOINTMENT = 8,
            EDITAPPOINTMENT = 9,
            DELETEAPPOINTMENT = 10,
            TODOAPPOINTMENT = 25,

            ADDNOTE = 11,
            EDITNOTE = 12,
            DELETENOTE = 13,


            ADDFOOD = 14,
            EDITFOOD = 15,
            DELETEFOOD = 16,


            ADDCERTIFICATE = 17,
            EDITCERTIFICATE = 18,
            DELETECERTIFICATE = 19,

            TRANSFERREQUESTSEND = 20,
            TRANSFERREQUESTACCEPT = 21,
            TRANSFERREQUESTREJECT = 22,

            PDFREPORT = 23,

            DELETEANIMAL = 24
        }

        public static LoginStatus DoLogin(string xiUsername, string xiPassword, out string xoUserId, out string xoUserType)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            xoUserId = string.Empty;
            xoUserType = string.Empty;

            string dbpassword = string.Empty;
            int userConfirmed = 0;
            try
            {
                string selqry = "select id,is_verified,type,possword from [user] where dbo.DecryptData(email, '" + BusinessBase.FixedSaltKey + "')= @email and active=1";

                Parameter param1 = new Parameter("email", xiUsername);
                reader1 = objdb.ExecuteReader(objdb.con, selqry, new Parameter[] { param1 });

                if (reader1.Read())
                {
                    xoUserId = Convert.ToString(objdb.GetValue(reader1, "id"));
                    userConfirmed = Convert.ToInt32(objdb.GetValue(reader1, "is_verified"));
                    xoUserType = Convert.ToString(objdb.GetValue(reader1, "type"));
                    dbpassword = Convert.ToString(objdb.GetValue(reader1, "possword"));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("Login", x.Message);
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

            bool verifyPassword = BASecurity.VerifyHash(xiPassword, dbpassword);
            if (!verifyPassword)
            {
                xoUserId = string.Empty;
                xoUserType = string.Empty;
                userConfirmed = 0;
            }

            LoginStatus loginStatus = LoginStatus.NONE;
            if (string.IsNullOrEmpty(xoUserId))
            {
                loginStatus = LoginStatus.WRONGUSERORPASS;
                return loginStatus;
            }

            if (userConfirmed != 1)
            {
                loginStatus = LoginStatus.USERNOTCONFIRMED;
                return loginStatus;
            }

            if (string.IsNullOrEmpty(xoUserId) == false)
            {
                loginStatus = LoginStatus.SUCCESS;
                return loginStatus;
            }

            return loginStatus;
        }

        public static User ForgotPassword(string xiUsername)
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

        public static DataTable GetSystemLanguages()
        {
            string query = "select * from system_language where active = 1";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetSystemLanguageKeys(string xiLanguageId, string xiLastSyncDate)
        {
            DateTime syncDate = DateTime.MinValue;
            DateTime.TryParse(xiLastSyncDate, out syncDate);

            string query = @"select slk.lang_id, slk.data_key,
(case when slk.data_value is null or len(slk.data_value) = 0 then (select slk2.data_value from system_language_keys slk2 where slk2.lang_id = 1 and slk2.data_key = slk.data_key)
else slk.data_value end ) as data_value
from system_language_keys slk where slk.lang_id = " + Utils.ConvertToDBString(xiLanguageId, Utils.DataType.Integer) + " and slk.active = 1";
            if (syncDate != DateTime.MinValue) query += " and slk.last_modified >= '" + syncDate.ToString("yyyy-MM-dd HH:mm") + "'";
            query += " order by slk.data_key";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetSystemTimezones()
        {
            string query = "select *, (name + ' ('+current_utc_offset+')') as timezone from sys.time_zone_info stz order by current_utc_offset";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static void SaveAnimalLog(NameValueCollection xiCollection)
        {
            try
            {
                string query2 = "INSERT INTO [dbo].[animal_log]([animal_id],[key],[category],[description],[submitdate],[submitby])VALUES(@animalid, @key, @category,@description,getutcdate(),@userid)";

                Parameter[] param = new Parameter[5];
                param[0] = new Parameter("animalid", xiCollection["animalid"], DbType.Int32);
                param[1] = new Parameter("key", xiCollection["key"], DbType.String);
                param[2] = new Parameter("category", xiCollection["category"], DbType.String);
                param[3] = string.IsNullOrEmpty(xiCollection["description"]) ? new Parameter("description", DBNull.Value) : new Parameter("description", xiCollection["description"], DbType.String);
                param[4] = new Parameter("userid", xiCollection["userid"], DbType.Int32);

                DBClass objdb = new DBClass();
                objdb.Connectdb();
                int retValue = objdb.ExecuteNonQuery(objdb.con, query2, param);
                objdb.Disconnectdb();
            }
            catch { }
        }

        //new 21 dec 2023 nilesh
        public static DataTable GetCountries()
        {
            string query = "select * from [countries] order by fullname";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetAllPhoneCountryCode()
        {
            string query = "select * from [contact_countrycode] order by [countrycode]";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static string GetPhoneCountryCode(int xiId)
        {
            string query = "select countrycode from [contact_countrycode] where id =" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            Object obj = objdb.ExecuteScalar(objdb.con, query);
            objdb.Disconnectdb();

            return obj.ToString();
        }

        //public static string GetPhoneCountryCode(int xiCode)
        //{
        //    string phonecountrycode = string.Empty;
        //    switch (BusinessBase.ConvertToString(xiCode))
        //    {
        //        case "20":
        //            phonecountrycode = "IND (+91)";
        //            break;
        //        case "21":
        //            phonecountrycode = "CHN (+86)";
        //            break;
        //        case "20":
        //            phonecountrycode = "IND (+91)";
        //            break;
        //        case "21":
        //            phonecountrycode = "CHN (+86)";
        //            break;
        //        case "20":
        //            phonecountrycode = "IND (+91)";
        //            break;
        //        case "21":
        //            phonecountrycode = "CHN (+86)";
        //            break;
        //    }
        //    return phonecountrycode;
        //}
    }
}
