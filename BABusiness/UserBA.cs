using BADBUtils;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Linq;

namespace BABusiness
{
    public class UserBA : BusinessBase
    {
        public enum Status
        {
            NEWBUREQUEST = 1,
            BUREQUESTREJECT = 2,
            BUREQUESTACCEPT = 3,
            BUSTAFFADDED = 4,
            BUCUSTOMERADDED = 5,
        }

        public static string GetStatusLogText(object xiStatus, object xiAdditionMessage)
        {
            string statusText = string.Empty;
            try
            {
                switch (Convert.ToInt32(xiStatus))
                {
                    case (int)Status.NEWBUREQUEST:
                        statusText = "The new business user request has been created";
                        break;

                    case (int)Status.BUREQUESTREJECT:
                        statusText = "The new business user request has been rejected.";
                        break;

                    case (int)Status.BUREQUESTACCEPT:
                        statusText = "The new business user request has been accepted.";
                        break;

                    case (int)Status.BUSTAFFADDED:
                        statusText = "The new staff has been added for business user.";
                        break;

                    case (int)Status.BUCUSTOMERADDED:
                        statusText = "The new customer has been added for business user.";
                        break;

                    default:
                        statusText = "-";
                        break;
                }
            }
            catch { }

            return statusText;
        }

        #region User

        public bool UpdateUser(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;
            string timezone = (string.IsNullOrEmpty(xiCollection["timezone"])) ? string.Empty : xiCollection["timezone"];
            // string profileimage = (string.IsNullOrEmpty(xiCollection["profileimage"])) ? string.Empty : xiCollection["profileimage"];

            string query = "update [user] set ";
            query += "fname = " + Utils.ConvertToDBString(xiCollection["fname"], Utils.DataType.Encrypted) + ", ";
            query += "lname = " + Utils.ConvertToDBString(xiCollection["lname"], Utils.DataType.Encrypted) + ", ";
            query += "phone = " + Utils.ConvertToDBString(xiCollection["phone"], Utils.DataType.Encrypted) + ", ";
            query += "email = " + Utils.ConvertToDBString(xiCollection["email"], Utils.DataType.Encrypted) + ", ";
            query += "lang = " + Utils.ConvertToDBString(xiCollection["lang"], Utils.DataType.String) + ", ";
            query += "user_timezone = " + Utils.ConvertToDBString(timezone, Utils.DataType.String) + ", ";
            query += "countryid = " + Utils.ConvertToDBString(xiCollection["countryid"], Utils.DataType.Integer) + ", ";
            query += "city = " + Utils.ConvertToDBString(xiCollection["city"], Utils.DataType.String) + ", ";
            query += "pincode = " + Utils.ConvertToDBString(xiCollection["pincode"], Utils.DataType.Integer) + ", ";
            query += "address = " + Utils.ConvertToDBString(xiCollection["address"], Utils.DataType.String) + ", ";
            query += "profileimage = " + Utils.ConvertToDBString(xiCollection["profileimage"], Utils.DataType.String) + ", ";
            query += "contactcountrycode = " + Utils.ConvertToDBString(xiCollection["contactcountrycode"], Utils.DataType.Integer);
            query += " where id = " + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();
            return (value > 0);

        }

        public static bool UpdateUserContactCountryCode(object xiCode, object xiUserId)
        {
            if (xiCode == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update [user] set contactcountrycode = @code where id= @id");
            Parameter param1 = new Parameter("code", xiCode, DbType.Int32);
            Parameter param2 = new Parameter("id", xiUserId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetUserDetail(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select u.*,cc.countrycode as 'ucontactcountrycode' from [user] u left join contact_countrycode cc on cc.id =u.contactcountrycode where u.active=1 and u.id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("fname", Convert.ToString(objdb.GetValue(reader1, "fname")));
                    collection.Add("lname", Convert.ToString(objdb.GetValue(reader1, "lname")));
                    collection.Add("email", Convert.ToString(objdb.GetValue(reader1, "email")));
                    collection.Add("phone", Convert.ToString(objdb.GetValue(reader1, "phone")));
                    collection.Add("lang", Convert.ToString(objdb.GetValue(reader1, "lang")));
                    collection.Add("type", Convert.ToString(objdb.GetValue(reader1, "type")));
                    collection.Add("submitdate", Convert.ToString(objdb.GetValue(reader1, "submitdate")));
                    collection.Add("is_verified", Convert.ToString(objdb.GetValue(reader1, "is_verified")));
                    collection.Add("password_token", Convert.ToString(objdb.GetValue(reader1, "password_token")));
                    collection.Add("user_token", Convert.ToString(objdb.GetValue(reader1, "user_token")));
                    collection.Add("user_token_date", Convert.ToString(objdb.GetValue(reader1, "user_token_date")));
                    collection.Add("isowner", Convert.ToString(objdb.GetValue(reader1, "isowner")));
                    collection.Add("timezone", Convert.ToString(objdb.GetValue(reader1, "user_timezone")));
                    collection.Add("password", Convert.ToString(objdb.GetValue(reader1, "password")));

                    collection.Add("countryid", Convert.ToString(objdb.GetValue(reader1, "countryid")));
                    collection.Add("city", Convert.ToString(objdb.GetValue(reader1, "city")));
                    collection.Add("pincode", Convert.ToString(objdb.GetValue(reader1, "pincode")));
                    collection.Add("address", Convert.ToString(objdb.GetValue(reader1, "address")));

                    collection.Add("profileimage", Convert.ToString(objdb.GetValue(reader1, "profileimage")));
                    collection.Add("contactcountrycode", Convert.ToString(objdb.GetValue(reader1, "contactcountrycode")));
                    collection.Add("ucontactcountrycode", Convert.ToString(objdb.GetValue(reader1, "ucontactcountrycode")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetUserDetail", x.Message);
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

        public static bool DeleteUser(object xiId)
        {
            string query = string.Format("delete from [user] where id= {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int GetUserCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(u.id) as maxcount from [user] u where u.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                while (reader1.Read())
                {
                    totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
                    break;
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetUserCount", x.Message);
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
            int divide = (int)(totalCount / Common.RECORDCOUNT);
            int mod = totalCount % Common.RECORDCOUNT;
            return (mod == 0) ? divide : divide + 1;
        }

        public static DataSet GetUserDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select u.id, u.fname as fname, u.lname as lname, u.email as email, u.phone as phone,u.type,u.submitdate from [user] u where u.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by u.fname,u.lname offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("procesed_datetime", typeof(string));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime tempDate1 = Convert.ToDateTime(row["submitdate"]);
                    string logdatetime = string.Empty;
                    if (tempDate1 != DateTime.MinValue)
                    {
                        logdatetime = tempDate1.ToString(BusinessBase.DateTimeFormat);

                    }
                    row["procesed_datetime"] = logdatetime;
                }
            }
            return ds;
        }

        public static int GetUserIdFromEmailAddress(string xiEmailAddress)
        {
            string query = "select id from [user] where dbo.DecryptData(email, '" + BusinessBase.FixedSaltKey + "') = " + Utils.ConvertToDBString(xiEmailAddress, Utils.DataType.String);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            object obj = objdb.ExecuteScalar(objdb.con, query);
            objdb.Disconnectdb();

            return BusinessBase.ConvertToInteger(obj);
        }

        public static string UserSearch(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["name"] != null && xiCollection["name"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("((u.fname + ' ' + u.lname) like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }
            if (xiCollection["email"] != null && xiCollection["email"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("((u.email) like {0})", Utils.ConvertToDBString("%" + xiCollection["email"] + "%", Utils.DataType.String)));
            }
            if (xiCollection["associateid"] != null && xiCollection["associateid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(e.animalcategory={0})", Utils.ConvertToDBString(xiCollection["animalcategory"], Utils.DataType.Integer)));
            }

            if (xiCollection["usertype"] != null && xiCollection["usertype"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(u.type={0})", Utils.ConvertToDBString(xiCollection["usertype"], Utils.DataType.Integer)));
            }

            return builder.ToString();
        }

        public static NameValueCollection GetUserDetailsByEmailId(string xiEmailAddress)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from [user] where active=1 and dbo.DecryptData(email, '" + BusinessBase.FixedSaltKey + "') = " + Utils.ConvertToDBString(xiEmailAddress, Utils.DataType.String);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("fname", Convert.ToString(objdb.GetValue(reader1, "fname")));
                    collection.Add("lname", Convert.ToString(objdb.GetValue(reader1, "lname")));
                    collection.Add("email", Convert.ToString(objdb.GetValue(reader1, "email")));
                    collection.Add("phone", Convert.ToString(objdb.GetValue(reader1, "phone")));
                    collection.Add("lang", Convert.ToString(objdb.GetValue(reader1, "lang")));
                    collection.Add("type", Convert.ToString(objdb.GetValue(reader1, "type")));
                    collection.Add("submitdate", Convert.ToString(objdb.GetValue(reader1, "submitdate")));
                    collection.Add("is_verified", Convert.ToString(objdb.GetValue(reader1, "is_verified")));
                    collection.Add("password_token", Convert.ToString(objdb.GetValue(reader1, "password_token")));
                    collection.Add("user_token", Convert.ToString(objdb.GetValue(reader1, "user_token")));
                    collection.Add("user_token_date", Convert.ToString(objdb.GetValue(reader1, "user_token_date")));
                    collection.Add("isowner", Convert.ToString(objdb.GetValue(reader1, "isowner")));
                    collection.Add("timezone", Convert.ToString(objdb.GetValue(reader1, "user_timezone")));
                    collection.Add("password", Convert.ToString(objdb.GetValue(reader1, "password")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetUserDetail", x.Message);
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

        public bool UpdatePassword(string xiNewPassword, string xiUserId)
        {
            string query = @"update [user] set [possword] = " + Utils.ConvertToDBString(xiNewPassword, Utils.DataType.String) + " where id = " + Utils.ConvertToDBString(xiUserId, Utils.DataType.String) + " and active=1";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();
            return (value > 0);

        }

        public string GetPassword(string xiUserId)
        {
            string query = "select [possword] from [user] where id= " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + " and active=1";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            object obj = objdb.ExecuteScalar(objdb.con, query);
            objdb.Disconnectdb();

            return BusinessBase.ConvertToString(obj);
        }

        #endregion

        #region User_Animal

        public bool AddUser_Animal(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "INSERT INTO [user_animal]([userid],[animalid],[active],[submitdate]) values(";
            query += Utils.ConvertToDBString(xiCollection["userid"], Utils.DataType.Integer) + ", ";
            query += Utils.ConvertToDBString(xiCollection["animalid"], Utils.DataType.Integer) + ", ";
            query += Utils.ConvertToDBString("1", Utils.DataType.Integer) + ", ";
            query += "getutcdate()); ";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);

            objdb.Disconnectdb();
            return (value > 0);
        }

        public static int GetUser_AnimalCount(string xiFilter, object xiUserId)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(ua.id) as maxcount from user_animal ua inner join view_animal a on ua.animalid = a.id and a.active = 1 where ua.userid=" + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + " and ua.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetUser_AnimalCount", x.Message);
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

            int divide = (int)(totalCount / Common.RECORDCOUNT);
            int mod = totalCount % Common.RECORDCOUNT;
            return (mod == 0) ? divide : divide + 1;
        }

        public static DataSet GetUser_AnimalDetails(int xiPage, string xiFilter, object xiUserId)
        {
            if (xiPage != -1)
            {
                xiPage = (xiPage <= 0) ? 1 : xiPage;
                xiPage = xiPage - 1;
            }

            string query = "select a.* from user_animal ua inner join view_animal a on ua.animalid = a.id and a.active = 1 where ua.userid=" + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + " and ua.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by a.[name]";

            if (xiPage != -1) query += " offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("procesed_dob", typeof(string));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime tempDate1 = Convert.ToDateTime(row["dob"]);
                    if (tempDate1 != DateTime.MinValue)
                    {
                        row["procesed_dob"] = tempDate1.ToString(BusinessBase.DateFormat);
                    }
                }
            }
            objdb.Disconnectdb();
            return ds;
        }

        public static bool DeleteUser_Animal(object xiId)
        {
            string query = string.Format("update animal set active=0 where id=@id;delete from user_animal where animalid= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static string Search(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;
            if (BusinessBase.ConvertToInteger(xiCollection["category"]) > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(a.[animalcategory] = {0})", Utils.ConvertToDBString(xiCollection["category"], Utils.DataType.Integer)));
            }

            if (xiCollection["name"] != null && xiCollection["name"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(a.[name] like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }

            if (BusinessBase.ConvertToInteger(xiCollection["active"]) <= 0) xiCollection["active"] = "1";

            if (iswhere) builder.Append(" and ");
            iswhere = true;
            builder.Append(string.Format("(a.[active] = {0})", Utils.ConvertToDBString(xiCollection["active"], Utils.DataType.Integer)));

            return builder.ToString();
        }

        public static bool ValidateAnimalUserRelation(object breedid, object userid)
        {
            return true; // todo last code
        }

        public static DataTable GetAllUserAnimal(object xiUserid, object xiAnimalCat, object xiBreedType)
        {
            string query = @"select a.id, a.[name], a.fathername,a.mothername,t.[name] as 'categoryname' from animal a inner join user_animal ua on a.id = ua.animalid inner join animal_category_type t on a.breedtype = t.id where ua.userid = " + Utils.ConvertToDBString(xiUserid, Utils.DataType.Integer) + " and a.active = 1 and ua.active = 1 and t.active = 1 ";
            query += "and a.animalcategory = " + Utils.ConvertToDBString(xiAnimalCat, Utils.DataType.Integer) + " ";

            string breedtype = BusinessBase.ConvertToString(xiBreedType);
            if (string.IsNullOrEmpty(breedtype))  // True means visible to all
                query += " ";
            else
                query += "and a.breedtype in (" + string.Join(",",
    breedtype.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => Utils.ConvertToDBString(x, Utils.DataType.Integer))) + ") ";

            query += "order by a.[name]";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);

            Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                dataTable.Columns.Add("animalname", typeof(string));

                foreach (DataRow row in dataTable.Rows)
                {
                    row["animalname"] = row["name"] + " (" + BusinessBase.ConvertToString(row["categoryname"]) + ")";
                }
            }
            objdb.Disconnectdb();
            return dataTable;
        }

        #endregion

        #region Association

        public static int GetAllMyAssociationCount(string xiFilter, object xiUserId, object xiEmail)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select count(*) 'maxcount' from 
(select q.id,q.[name] from animal_association q  where q.active = 1 and q.createdby =  {0} 
Union 
select q.id,q.[name] from animal_association q where q.active = 1 and 
q.id in (select am.association_id from association_members am inner join [member] m on am.memberid = m.id and am.active = 1 where m.exitdate Is null and m.email ={1})) qcount";

            query = string.Format(query, Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer), Utils.ConvertToDBString(xiEmail, Utils.DataType.String));

            if (string.IsNullOrEmpty(xiFilter) == false) query += " where" + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAllAssociationCount", x.Message);
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

            int divide = (int)(totalCount / Common.RECORDCOUNT);
            int mod = totalCount % Common.RECORDCOUNT;
            return (mod == 0) ? divide : divide + 1;
        }

        public static DataSet GetAllMyAssociation(int xiPage, string xiFilter, object xiUserId, object xiEmail)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            string query = @"select * from (select  q.*,(select stuff(( 
select ', ' + (act.[name] + ' [' + ac.breedname + ']') from animal_category_type act inner join animal_categary ac on act.categoryid = ac.id where act.id in
(select value from animal_association q1
CROSS APPLY STRING_SPLIT(SUBSTRING (q1.breedtype ,0 ,LEN(q1.breedtype)+100), ',') where q1.id = q.id)
for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as breednames,1 as isowner,
(select count(am.id) from association_members am  where am.active = 2 and am.association_id = q.id) as totalpendingmembers,0 as 'isadmin'
from animal_association q 
where q.active = 1 and q.createdby = {0} 
Union 
select q.*,(select stuff(( 
select ', ' + (act.[name] + ' [' + ac.breedname + ']') from animal_category_type act inner join animal_categary ac on act.categoryid = ac.id where act.id in
(select value from animal_association q1
CROSS APPLY STRING_SPLIT(SUBSTRING (q1.breedtype ,0 ,LEN(q1.breedtype)+100), ',') where q1.id = q.id)
for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as breednames,
(case when q.createdby = {0} then 1 else 0 end) as isowner,
(select count(am.id) from association_members am where am.active = 2 and am.association_id = q.id) as totalpendingmembers,
(select isadmin from [association_members] where memberid = (select m.id from association_members am inner join [member] m on am.memberid = m.id and am.active=1  
where m.exitdate is null and m.email = '{1}' and am.association_id = q.id)) as 'isadmin'
from animal_association q where q.active = 1 and q.createdby <> {0} and  
q.id in (select am.association_id from association_members am inner join [member] m on am.memberid = m.id and am.active=1  where m.exitdate is null and m.email = '{1}')";

            query = string.Format(query, xiUserId, xiEmail);
            query += " order by q.[name] offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only)tbl ";
            if (!string.IsNullOrEmpty(xiFilter)) query += "where" + xiFilter;


            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static int GetAllAssociationCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select  count(*) maxcount from animal_association q where q.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAllAssociationCount", x.Message);
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

            int divide = (int)(totalCount / Common.RECORDCOUNT);
            int mod = totalCount % Common.RECORDCOUNT;
            return (mod == 0) ? divide : divide + 1;
        }

        public static DataSet GetAllAssociation(int xiPage, string xiFilter, object xiUserId, object xiEmail)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            string query = @"select  q.*,(select stuff(( 
select ', ' + (act.[name] + ' [' + ac.breedname + ']') from animal_category_type act inner join animal_categary ac on act.categoryid = ac.id where act.id in
(select value from animal_association q1
CROSS APPLY STRING_SPLIT(SUBSTRING (q1.breedtype ,0 ,LEN(q1.breedtype)+100), ',') where q1.id = q.id)
for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as breednames,
(case when q.createdby = {0} then 1 else 0 end) as isowner,  -- 1 is logged-in userid
(select m.entrydate from association_members am inner join [member] m on am.memberid = m.id and am.active = 1
where am.association_id = q.id and m.email = '{1}') as joiningdate, 
(select m.submitdate from association_members am inner join [member] m on am.memberid = m.id and am.active = 2
where am.association_id = q.id and m.email = '{1}') as requestsentdate,
(select count(am.id) from association_members am inner join [member] m on am.memberid = m.id and am.active=1 where am.association_id = q.id) as totalmembers 
from animal_association q where q.active = 1";

            query = string.Format(query, xiUserId, xiEmail);

            if (!string.IsNullOrEmpty(xiFilter)) query += " and " + xiFilter;
            query += " order by q.[name] offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("procesed_entrydate", typeof(string));
                ds.Tables[0].Columns.Add("procesed_submitdate", typeof(string));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    try
                    {
                        DateTime tempDate1 = Convert.ToDateTime(row["joiningdate"]);
                        if (tempDate1 != DateTime.MinValue)
                        {
                            row["procesed_entrydate"] = tempDate1.ToString(BusinessBase.DateFormat);
                        }
                    }
                    catch
                    { }


                    try
                    {
                        DateTime tempDate1 = Convert.ToDateTime(row["requestsentdate"]);
                        if (tempDate1 != DateTime.MinValue)
                        {
                            row["procesed_submitdate"] = tempDate1.ToString(BusinessBase.DateTimeFormat);
                        }
                    }
                    catch
                    { }
                }
            }

            return ds;
        }

        public static string DeleteAssociation(int xiAssociationId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string query = @"select count(m.id) as maxcount from [association_members] am inner join member m on m.id = am.memberid where am.association_id  = " + Utils.ConvertToDBString(xiAssociationId, Utils.DataType.Integer) + " and am.active=1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("DeleteAssociation", x.Message);
            }
            finally
            {
                if (reader1 != null)
                {
                    reader1.Close();
                    reader1 = null;
                }
            }

            string returnValue = string.Empty;
            if (totalCount == 0)
            {
                query = string.Format("update animal_association set active = 0 where id= {0}", Utils.ConvertToDBString(xiAssociationId, Utils.DataType.Integer));

                objdb.Connectdb();
                int value = objdb.ExecuteNonQuery(objdb.con, query);
                objdb.Disconnectdb();

                returnValue = (value > 0) ? "Deleted Successfully." : "false";
            }
            else
            {
                returnValue = "You can not delete this item because its already used.";
            }
            objdb.Disconnectdb();

            return returnValue;
        }

        public static string AssociationSearch(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["name"] != null && xiCollection["name"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("([name] like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }
            if (xiCollection["breedtype"] != null && xiCollection["breedtype"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(q.breedtype is null or ','+q.breedtype+',' like {0})", Utils.ConvertToDBString("%," + xiCollection["breedtype"] + ",%", Utils.DataType.String)));

            }

            return builder.ToString();
        }

        public int AddAssociation(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = @"insert into animal_association([name],[address],[breedtype],[email],[phone],[website],[active],[createddate],[createdby]) values(@name,@address,@breedtype,@email,@phone,@website,@active,getutcdate(),@createdby)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("address", xiCollection["address"]);
            Parameter param3 = new Parameter("breedtype", xiCollection["breedtype"], DbType.String);
            Parameter param4 = new Parameter("email", xiCollection["email"]);
            Parameter param5 = new Parameter("phone", xiCollection["phone"]);
            Parameter param6 = new Parameter("website", xiCollection["website"]);
            Parameter param7 = new Parameter("active", 1);
            Parameter param8 = new Parameter("createdby", xiCollection["createdby"]);


            string query2 = "select scope_identity()";

            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8 });
            objdb.Disconnectdb();

            if (value > 0)
                return value;
            else
                return int.MinValue;

        }

        public bool UpdateAssociation(NameValueCollection xiCollection, object xiAssociationId)
        {
            if (xiCollection == null) return false;

            string query = @"update animal_association set name=@name,address=@address,breedtype=@breedtype,email=@email ,phone = @phone,website = @website where id=@id";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("address", xiCollection["address"]);
            Parameter param3 = new Parameter("breedtype", xiCollection["breedtype"], DbType.String);
            Parameter param4 = new Parameter("email", xiCollection["email"]);
            Parameter param5 = new Parameter("phone", xiCollection["phone"]);
            Parameter param6 = new Parameter("website", xiCollection["website"]);
            Parameter param7 = new Parameter("id", xiAssociationId, DbType.Int32);
            //Parameter param8 = new Parameter("createdby", xiCollection["createdby"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param2, param3, param4, param1, param5, param6, param7 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetAssociation(object xiAssociationId)
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from animal_association where id = " + Utils.ConvertToDBString(xiAssociationId, Utils.DataType.Integer) + " and active = 1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read())
                {
                    collection = new NameValueCollection();

                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("address", Convert.ToString(objdb.GetValue(reader1, "address")));
                    collection.Add("email", Convert.ToString(objdb.GetValue(reader1, "email")));
                    collection.Add("phone", Convert.ToString(objdb.GetValue(reader1, "phone")));
                    collection.Add("website", Convert.ToString(objdb.GetValue(reader1, "website")));
                    collection.Add("breedtype", Convert.ToString(objdb.GetValue(reader1, "breedtype")));
                    collection.Add("createdby", Convert.ToString(objdb.GetValue(reader1, "createdby")));
                    collection.Add("inviation_code", Convert.ToString(objdb.GetValue(reader1, "inviation_code")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAssociation", x.Message);
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

        public static NameValueCollection GetAssociationByInvitationCode(object xiInvitationCode)
        {
            NameValueCollection collection = null;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from animal_association where inviation_code = " + Utils.ConvertToDBString(xiInvitationCode, Utils.DataType.String) + " and active = 1";

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);

                if (reader1.Read())
                {
                    collection = new NameValueCollection();

                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("address", Convert.ToString(objdb.GetValue(reader1, "address")));
                    collection.Add("email", Convert.ToString(objdb.GetValue(reader1, "email")));
                    collection.Add("phone", Convert.ToString(objdb.GetValue(reader1, "phone")));
                    collection.Add("website", Convert.ToString(objdb.GetValue(reader1, "website")));
                    collection.Add("breedtype", Convert.ToString(objdb.GetValue(reader1, "breedtype")));
                    collection.Add("createdby", Convert.ToString(objdb.GetValue(reader1, "createdby")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAssociationByInvitationCode", x.Message);
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

        public static DataTable GetAssociatedBreeders(object xiAssociationId)
        {
            string query = @"select au.id,(s.fname + ' ' + s.lname) as name,s.email as email,s.phone as phone from associate_user au inner join [user] s on au.userid=s.id where au.asso_id =" + Utils.ConvertToDBString(xiAssociationId, Utils.DataType.Integer) + " order by s.fname,s.lname";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static bool RemoveBreederFromAssocation(object xiAssociationId)
        {
            string query = "delete from associate_user where id = " + Utils.ConvertToDBString(xiAssociationId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public int AddBreederToAssociation(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "if not exists(select 1 from associate_user where asso_id=@asso_id and userid=@userid) insert into associate_user(asso_id,userid,active,submitdate) values(@asso_id, @userid,1,getutcdate())";

            Parameter param1 = new Parameter("asso_id", xiCollection["asso_id"], DbType.Int32);
            Parameter param2 = new Parameter("userid", xiCollection["userid"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return value;
        }

        public static DataTable GetAssociationBreederTypes(object xiUserId)
        {
            string query = @"select a.id, a.breedtype from animal_association a where active = 1 and createdby = " + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static bool AddRequest(object Xiassosiationid, int xiUserId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "insert into [associate_join_user]([userid],[asso_id],[active],[submitdate]) values(";
            query += Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + ", ";
            query += Utils.ConvertToDBString(Xiassosiationid, Utils.DataType.Integer) + ", ";
            query += "1, getutcdate()); ";

            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static bool AcceptRequest(object Xiassosiationid, int xiUserId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "insert into [associate_user]([userid],[asso_id],[active],[submitdate]) values(";
            query += Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + ", ";
            query += Utils.ConvertToDBString(Xiassosiationid, Utils.DataType.Integer) + ", ";
            query += "1, getutcdate()); ";

            int value = objdb.ExecuteNonQuery(objdb.con, query);

            string query1 = "delete from associate_join_user where id = " + Utils.ConvertToDBString(Xiassosiationid, Utils.DataType.Integer);
            int value1 = objdb.ExecuteNonQuery(objdb.con, query1);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static bool DeclineRequest(object Xiassosiationid)
        {
            string query = "delete from associate_join_user where id = " + Utils.ConvertToDBString(Xiassosiationid, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int GetAllRequestCount(object xiUserId)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select count(aju.id) as maxcount  from associate_join_user aju left join [user] u on u.id=aju.[userid] where aju.asso_id=" + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + " and aju.active = 1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAllRequestCount", x.Message);
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

            int divide = (int)(totalCount / Common.RECORDCOUNT);
            int mod = totalCount % Common.RECORDCOUNT;
            return (mod == 0) ? divide : divide + 1;
        }

        public static DataSet GetAllRequest(int xiPage, object xiUserId)
        {
            string query = @"select aju.*,(u.fname + ' '+u.lname) as username from associate_join_user aju left join [user] u on u.id=aju.[userid] where aju.asso_id=" + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + " and aju.active = 1";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            if (ds != null && ds.Tables.Count > 0)
            {
                Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();
                ds.Tables[0].Columns.Add("procesed_datetime", typeof(string));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime tempDate1 = Convert.ToDateTime(row["submitdate"]);
                    if (tempDate1 != DateTime.MinValue)
                    {
                        row["procesed_datetime"] = tempDate1.ToString(BusinessBase.DateTimeFormat);
                    }
                }
            }
            return ds;
        }

        public static bool UpdateInvitationCode(string xiInvitationCode, object xiAssociationId)
        {
            string query = @"update animal_association set inviation_code=@inviation_code where id=@id";

            Parameter param1 = new Parameter("inviation_code", xiInvitationCode);
            Parameter param2 = new Parameter("id", xiAssociationId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        #endregion

        #region Association Approval

        public bool UpdateApprovalStatus(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update [animal_association] set [isapprove]=@isapprove,[comments]=@comments where id=@id";

            Parameter param1 = new Parameter("isapprove", xiCollection["isapprove"], DbType.Int32);
            Parameter param2 = (string.IsNullOrEmpty(xiCollection["comments"])) ? new Parameter("comments", DBNull.Value) : new Parameter("comments", xiCollection["comments"], DbType.String);
            Parameter param3 = new Parameter("id", xiId, DbType.Int32);
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int GetAllAssociationToApproveCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select  count(*) maxcount from animal_association q inner join [user] u on u.id = q.createdby and u.active = 1 where q.isapprove = 0 and q.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAllAssociationToApproveCount", x.Message);
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

            int divide = (int)(totalCount / Common.RECORDCOUNT);
            int mod = totalCount % Common.RECORDCOUNT;
            return (mod == 0) ? divide : divide + 1;
        }

        public static DataSet GetAllAssociationToApprove(int xiPage, string xiFilter, object xiUserId, object xiEmail)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            string query = @"select  q.*,(select stuff(( 
select ', ' + (act.[name] + ' [' + ac.breedname + ']') from animal_category_type act inner join animal_categary ac on act.categoryid = ac.id where act.id in
(select value from animal_association q1
CROSS APPLY STRING_SPLIT(SUBSTRING (q1.breedtype ,0 ,LEN(q1.breedtype)+100), ',') where q1.id = q.id)
for xml path(''), type).value('.','nvarchar(max)'),1,1,'') ) as breednames,(select count(am.id) from association_members am inner join [member] m on am.memberid = m.id and am.active=1 where am.association_id = q.id) as totalmembers, u.lname as lname, u.fname as fname 
from animal_association q inner join [user] u on u.id = q.createdby and u.active = 1 where q.isapprove = 0 and q.active = 1
";

            query = string.Format(query, xiUserId, xiEmail);

            if (!string.IsNullOrEmpty(xiFilter)) query += " and " + xiFilter;
            query += " order by q.[name] offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;
        }

        #endregion

        #region Business User

        public static int AddBusinessEnquiry(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;
            string query = "INSERT INTO [bu_businessenquiry](fname,lname,email,phone,address,website,companyname,companyshortname,businesstype,registrationno,contactcountrycode)values(@fname,@lname,@email,@phone,@address,@website,@companyname,@companyshortname,@businesstype,@registrationno,@contactcountrycode)";

            Parameter param1 = new Parameter("fname", xiCollection["fname"]);
            Parameter param2 = new Parameter("lname", xiCollection["lname"]);
            Parameter param3 = new Parameter("email", xiCollection["email"]);
            Parameter param4 = new Parameter("phone", xiCollection["phone"]);
            Parameter param5 = new Parameter("address", xiCollection["address"]);
            Parameter param6 = new Parameter("website", xiCollection["website"]);
            Parameter param7 = new Parameter("companyname", xiCollection["companyname"]);
            Parameter param8 = new Parameter("companyshortname", xiCollection["companyshortname"]);
            Parameter param9 = new Parameter("businesstype", xiCollection["businesstype"], DbType.Int32);
            Parameter param10 = new Parameter("registrationno", xiCollection["registrationno"]);
            Parameter param11 = new Parameter("contactcountrycode", xiCollection["contactcountrycode"]);

            string query2 = "Select Scope_Identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11 });
            objdb.Disconnectdb();

            return value;
        }

        public static bool UpdateBusinessEnquiry(NameValueCollection xiCollection, object xiEnquiryId)
        {
            if (xiCollection == null) return false;
            string reason = (string.IsNullOrEmpty(xiCollection["reason"])) ? string.Empty : xiCollection["reason"];

            string query = "update [bu_businessenquiry] set status=@status,reason=@reason,updatedby=@updatedby where id=@id";

            Parameter param1 = new Parameter("status", xiCollection["status"], DbType.Int32);
            Parameter param2 = new Parameter("reason", reason);
            Parameter param3 = new Parameter("updatedby", xiCollection["updatedby"], DbType.Int32);
            Parameter param4 = new Parameter("id", xiEnquiryId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        //public static bool DeleteBusinessEnquiry(object xiId)
        //{
        //    string query = string.Format("delete from [bu_businessenquiry] where id= {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));

        //    DBClass objdb = new DBClass();
        //    objdb.Connectdb();
        //    int value = objdb.ExecuteNonQuery(objdb.con, query);
        //    objdb.Disconnectdb();

        //    return (value > 0);
        //}

        public static bool AddBusinessEnquiryGallery(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "INSERT INTO [bu_businessenquiry_images]([enquiryid],[title],[gallery_file],[file_type],[active],[submitdate]) values(@enquiryid,@title,@file_name,@file_type,1,getutcdate())";
            Parameter param1 = new Parameter("enquiryid", xiCollection["enquiryid"], DbType.Int32);
            Parameter param2 = new Parameter("file_name", xiCollection["file_name"]);
            Parameter param3 = new Parameter("title", xiCollection["title"]);
            Parameter param4 = new Parameter("file_type", xiCollection["file_type"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetBusinessEnquiryPhotos(object xiEnquiryId)
        {
            string query = "select * from bu_businessenquiry_images where active=1 and enquiryid=" + Utils.ConvertToDBString(xiEnquiryId, Utils.DataType.Integer) + " order by submitdate desc";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dt = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();
            return dt;
        }

        public static DataSet GetBusinessEnquiryGallery(object xiEnquiryId)
        {
            string query = "select * from bu_businessenquiry_images where active=1 and enquiryid=" + Utils.ConvertToDBString(xiEnquiryId, Utils.DataType.Integer) + " order by submitdate desc";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;
        }

        public static NameValueCollection GetBusinessEnquiryGalleryDetail(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from bu_businessenquiry_images  where active=1 and enquiryid=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("file_name", Convert.ToString(objdb.GetValue(reader1, "file_name")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetBusinessEnquiryGalleryDetail", x.Message);
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

        public static bool DeleteBusinessEnquiryGalleryPhoto(string xiFileName, NameValueCollection xiCollection)
        {
            string query = "delete from [bu_businessenquiry_images] where [gallery_file]=@file";
            Parameter param2 = new Parameter("file", xiFileName);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param2 });
            objdb.Disconnectdb();

            return (value > 0);

        }

        public static NameValueCollection GetBusinessEnquiryDetail(object xiEnquiryId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select be.*,bt.name as businesstypename,pc.countrycode  from bu_businessenquiry be inner join bu_businesstype bt on bt.id = be.businesstype and bt.active=1 inner join contact_countrycode pc on pc.id=be.contactcountrycode and pc.active=1 where be.id =" + Utils.ConvertToDBString(xiEnquiryId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("fname", Convert.ToString(objdb.GetValue(reader1, "fname")));
                    collection.Add("lname", Convert.ToString(objdb.GetValue(reader1, "lname")));
                    collection.Add("email", Convert.ToString(objdb.GetValue(reader1, "email")));
                    collection.Add("phone", Convert.ToString(objdb.GetValue(reader1, "phone")));
                    collection.Add("contactcountrycode", Convert.ToString(objdb.GetValue(reader1, "contactcountrycode")));
                    collection.Add("address", Convert.ToString(objdb.GetValue(reader1, "address")));
                    collection.Add("website", Convert.ToString(objdb.GetValue(reader1, "website")));
                    collection.Add("companyname", Convert.ToString(objdb.GetValue(reader1, "companyname")));
                    collection.Add("companyshortname", Convert.ToString(objdb.GetValue(reader1, "companyshortname")));
                    collection.Add("businesstype", Convert.ToString(objdb.GetValue(reader1, "businesstype")));
                    collection.Add("businesstypename", Convert.ToString(objdb.GetValue(reader1, "businesstypename")));
                    collection.Add("registrationno", Convert.ToString(objdb.GetValue(reader1, "registrationno")));
                    collection.Add("status", Convert.ToString(objdb.GetValue(reader1, "status")));
                    collection.Add("reason", Convert.ToString(objdb.GetValue(reader1, "reason")));
                    collection.Add("countrycode", Convert.ToString(objdb.GetValue(reader1, "countrycode")));
                    //collection.Add("buid", Convert.ToString(objdb.GetValue(reader1, "buid"))); 
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetBusinessEnquiryDetail", x.Message);
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

        public static DataSet GetBusinessUserRequestApprove(int xiPage, string xiFilter, string xiSort)
        {
            xiPage = (xiPage == 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            string query = @"select be.id as id,be.email as email,be.fname as fname,be.lname as lname,be.companyname,be.[status],be.reason,bt.[name] as businesstypename,bu.id as buid from bu_businessenquiry be inner join bu_businesstype bt on bt.id = be.businesstype and bt.active=1 left join bu_businessuser bu on bu.enquiryid=be.id and bu.active=1 where be.active=1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by " + GetSortOrder(xiSort) + " offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        private static string GetSortOrder(string xiSort)
        {
            string sortcolumn = "be.id desc";

            string[] sortorder = BusinessBase.SplitSort(xiSort);
            switch (sortorder[0])
            {
                case "1":
                    sortcolumn = "be.fname " + sortorder[1] + ", be.lname " + sortorder[1];
                    break;

                case "2":
                    sortcolumn = "be.email " + sortorder[1];
                    break;

                case "3":
                    sortcolumn = "be.companyname " + sortorder[1];
                    break;

                case "4":
                    sortcolumn = "businesstypename " + sortorder[1];
                    break;

                case "5":
                    sortcolumn = "be.[status] " + sortorder[1];
                    break;
            }

            return sortcolumn;
        }

        public static int GetBusinessUserRequestApproveCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select count(be.id) as maxcount from bu_businessenquiry be inner join bu_businesstype bt on bt.id = be.businesstype and bt.active=1 left join bu_businessuser bu on bu.enquiryid=be.id and bu.active=1 where be.active=1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetBusinessUserRequestApproveCount", x.Message);
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

            int divide = (int)(totalCount / Common.RECORDCOUNT);
            int mod = totalCount % Common.RECORDCOUNT;
            return (mod == 0) ? divide : divide + 1;
        }

        public static string SearchRequestDetails(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;
            if (xiCollection["companyname"] != null && xiCollection["companyname"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(be.[companyname] like {0})", Utils.ConvertToDBString("%" + xiCollection["companyname"] + "%", Utils.DataType.String)));
            }
            if (xiCollection["name"] != null && xiCollection["name"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(be.fname + ' ' + be.lname like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }

            if (xiCollection["status"] != null && xiCollection["status"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(be.[status]={0})", Utils.ConvertToDBString(xiCollection["status"], Utils.DataType.Integer)));
            }

            return builder.ToString();
        }

        public static bool DeleteBusinessEnquiry(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update bu_businessenquiry set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int AddBusinessUser(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;
            string query = "INSERT INTO [bu_businessuser](userid,address,website,companyname,companyshortname,businesstype,registrationno,submitdate,approvedby,enquiryid)values(@userid,@address,@website,@companyname,@companyshortname,@businesstype,@registrationno,getutcdate(),@approvedby,@enquiryid)";

            Parameter param1 = new Parameter("userid", xiCollection["userid"], DbType.Int32);
            Parameter param2 = new Parameter("address", xiCollection["address"]);
            Parameter param3 = new Parameter("website", xiCollection["website"]);
            Parameter param4 = new Parameter("companyname", xiCollection["companyname"]);
            Parameter param5 = new Parameter("companyshortname", xiCollection["companyshortname"]);
            Parameter param6 = new Parameter("businesstype", xiCollection["businesstype"], DbType.Int32);
            Parameter param7 = new Parameter("registrationno", xiCollection["registrationno"]);
            Parameter param8 = new Parameter("approvedby", xiCollection["approvedby"], DbType.Int32);
            Parameter param9 = new Parameter("enquiryid", xiCollection["enquiryid"], DbType.Int32);

            string query2 = "Select Scope_Identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9 });
            objdb.Disconnectdb();

            return value;
        }

        public static bool UpdateBusinessUser(NameValueCollection xiCollection, object xiBUId)
        {
            if (xiCollection == null) return false;

            string query = "update [bu_businessuser] set companyname=@companyname,companyshortname=@companyshortname,website=@website,businesstype=@businesstype,registrationno=@registrationno,address=@address,dateofincorporation=@dateofincorporation,tinno=@tinno,licenceno=@licenceno,employeridno=@employeridno,description=@description,companylogo=@companylogo,countryid=@countryid,city=@city,pincode=@pincode,currencyid=@currencyid,taxid=@taxid,termscondition=@termscondition,buemail=@buemail,buphone=@buphone,bucontactcountrycode=@bucontactcountrycode where id=@id";

            Parameter param1 = new Parameter("companyname", xiCollection["companyname"]);
            Parameter param2 = new Parameter("companyshortname", xiCollection["companyshortname"]);
            Parameter param3 = new Parameter("website", xiCollection["website"]);
            Parameter param4 = new Parameter("businesstype", xiCollection["businesstype"], DbType.Int32);
            Parameter param5 = new Parameter("registrationno", xiCollection["registrationno"]);
            Parameter param6 = new Parameter("address", xiCollection["address"]);
            Parameter param7 = new Parameter("dateofincorporation", xiCollection["dateofincorporation"], DbType.DateTime);
            Parameter param8 = new Parameter("tinno", xiCollection["tinno"]);
            Parameter param9 = new Parameter("licenceno", xiCollection["licenceno"]);
            Parameter param10 = new Parameter("employeridno", xiCollection["employeridno"]);
            Parameter param11 = new Parameter("description", xiCollection["description"]);
            Parameter param12 = new Parameter("companylogo", xiCollection["companylogo"]);
            Parameter param13 = new Parameter("countryid", xiCollection["countryid"], DbType.Int32);
            Parameter param14 = new Parameter("city", xiCollection["city"]);
            Parameter param15 = new Parameter("pincode", xiCollection["pincode"], DbType.Int32);

            Parameter param16 = new Parameter("currencyid", xiCollection["currencyid"], DbType.Int32);
            Parameter param17 = new Parameter("taxid", xiCollection["taxid"], DbType.Int32);
            Parameter param18 = new Parameter("termscondition", xiCollection["termscondition"]);

            Parameter param19 = new Parameter("buemail", xiCollection["buemail"]);
            Parameter param20 = new Parameter("buphone", xiCollection["buphone"]);
            Parameter param21 = new Parameter("bucontactcountrycode", xiCollection["bucontactcountrycode"], DbType.Int32);

            Parameter param22 = new Parameter("id", xiBUId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14, param15, param16, param17, param18, param19, param20, param21, param22 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        //new 24 july 2025 nilesh
        public static bool UpdateBusinessUserCurrency(object xiCId, object xiBUId)
        {
            string query = "update [bu_businessuser] set currencyid=@currencyid where id=@id ";
            Parameter param1 = new Parameter("currencyid", xiCId, DbType.Int32);
            Parameter param2 = new Parameter("id", xiBUId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();
            return (value > 0);
        }


        public static NameValueCollection GetBusinessUserDetail(object xiBUId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select bu.*,u.fname as fname,u.lname as lname,u.email as email,u.phone as phone,u.contactcountrycode as ucontactcountrycode,bt.name as businesstypename,cc.fullname as countryname,c.[name] as currencyname, t.[name] as 'taxname',ccc1.countrycode as 'userphoneprefix',ccc2.countrycode as 'buphoneprefix' from [bu_businessuser] bu  inner join [user] u on u.id = bu.userid  left join [contact_countrycode] ccc1 on ccc1.id = u.contactcountrycode left join[contact_countrycode] ccc2 on ccc2.id = bu.bucontactcountrycode  inner join bu_businesstype bt on bt.id = bu.businesstype and bt.active = 1 left join [countries] cc on cc.id = bu.countryid left join [bu_currency] c on c.id = bu.currencyid and c.active = 1  left join [bu_tax] t on t.id = bu.taxid and t.active = 1 where bu.active=1 and bu.id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("userid", Convert.ToString(objdb.GetValue(reader1, "userid")));
                    collection.Add("fname", Convert.ToString(objdb.GetValue(reader1, "fname")));
                    collection.Add("lname", Convert.ToString(objdb.GetValue(reader1, "lname")));
                    collection.Add("email", Convert.ToString(objdb.GetValue(reader1, "email")));
                    collection.Add("phone", Convert.ToString(objdb.GetValue(reader1, "phone")));
                    collection.Add("ucontactcountrycode", Convert.ToString(objdb.GetValue(reader1, "ucontactcountrycode")));
                    collection.Add("userphoneprefix", Convert.ToString(objdb.GetValue(reader1, "userphoneprefix")));
                    collection.Add("address", Convert.ToString(objdb.GetValue(reader1, "address")));
                    collection.Add("website", Convert.ToString(objdb.GetValue(reader1, "website")));
                    collection.Add("companyname", Convert.ToString(objdb.GetValue(reader1, "companyname")));
                    collection.Add("companyshortname", Convert.ToString(objdb.GetValue(reader1, "companyshortname")));
                    collection.Add("businesstype", Convert.ToString(objdb.GetValue(reader1, "businesstype")));
                    collection.Add("businesstypename", Convert.ToString(objdb.GetValue(reader1, "businesstypename")));
                    collection.Add("registrationno", Convert.ToString(objdb.GetValue(reader1, "registrationno")));
                    collection.Add("dateofincorporation", Convert.ToString(objdb.GetValue(reader1, "dateofincorporation")));
                    collection.Add("tinno", Convert.ToString(objdb.GetValue(reader1, "tinno")));
                    collection.Add("licenceno", Convert.ToString(objdb.GetValue(reader1, "licenceno")));
                    collection.Add("employeridno", Convert.ToString(objdb.GetValue(reader1, "employeridno")));
                    collection.Add("description", Convert.ToString(objdb.GetValue(reader1, "description")));
                    collection.Add("companylogo", Convert.ToString(objdb.GetValue(reader1, "companylogo")));
                    collection.Add("submitdate", Convert.ToString(objdb.GetValue(reader1, "submitdate")));
                    collection.Add("countryid", Convert.ToString(objdb.GetValue(reader1, "countryid")));
                    collection.Add("countryname", Convert.ToString(objdb.GetValue(reader1, "countryname")));
                    collection.Add("city", Convert.ToString(objdb.GetValue(reader1, "city")));
                    collection.Add("pincode", Convert.ToString(objdb.GetValue(reader1, "pincode")));
                    collection.Add("approvedby", Convert.ToString(objdb.GetValue(reader1, "approvedby")));
                    collection.Add("enquiryid", Convert.ToString(objdb.GetValue(reader1, "enquiryid")));

                    collection.Add("taxid", Convert.ToString(objdb.GetValue(reader1, "taxid")));
                    collection.Add("currencyid", Convert.ToString(objdb.GetValue(reader1, "currencyid")));
                    collection.Add("currencyname", Convert.ToString(objdb.GetValue(reader1, "currencyname")));
                    collection.Add("termscondition", Convert.ToString(objdb.GetValue(reader1, "termscondition")));
                    collection.Add("taxname", Convert.ToString(objdb.GetValue(reader1, "taxname")));

                    collection.Add("buemail", Convert.ToString(objdb.GetValue(reader1, "buemail")));
                    collection.Add("buphone", Convert.ToString(objdb.GetValue(reader1, "buphone")));
                    collection.Add("bucontactcountrycode", Convert.ToString(objdb.GetValue(reader1, "bucontactcountrycode")));
                    collection.Add("buphoneprefix", Convert.ToString(objdb.GetValue(reader1, "buphoneprefix")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetBusinessUserDetail", x.Message);
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

        public static string SearchCompany(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;
            if (xiCollection["companyname"] != null && xiCollection["companyname"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(bu.[companyname] like {0})", Utils.ConvertToDBString("%" + xiCollection["companyname"] + "%", Utils.DataType.String)));
            }
            if (xiCollection["name"] != null && xiCollection["name"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(bu.fname + ' ' + bu.lname like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }

            if (xiCollection["custidnotin"] != null && xiCollection["custidnotin"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(bu.id not in (select bu_id from [bu_customer] where active=1 and userid ={0}))", Utils.ConvertToDBString(xiCollection["custidnotin"], Utils.DataType.Integer)));
            }

            if (xiCollection["custidin"] != null && xiCollection["custidin"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(bu.id in (select bu_id from [bu_customer] where active=1 and userid ={0}))", Utils.ConvertToDBString(xiCollection["custidin"], Utils.DataType.Integer)));
            }

            return builder.ToString();
        }

        public static DataSet GetCompanyForCustomer(int xiPage, string xiFilter)
        {
            xiPage = (xiPage == 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            string query = @"select bu.*,u.fname as fname,u.lname as lname,u.email as email,u.phone as phone,bt.name as businesstypename,cc.fullname as countryname,c.name as currencyname from [bu_businessuser] bu inner join [user] u on u.id = bu.userid inner join bu_businesstype bt on bt.id = bu.businesstype left join [countries] cc on cc.id = bu.countryid left join [bu_currency] c on c.id = bu.currencyid where bu.active=1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by companyname offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static int GetCompanyForCustomerCount(string xiFilter)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select count(bu.id) as maxcount from [bu_businessuser] bu inner join [user] u on u.id = bu.userid inner join bu_businesstype bt on bt.id = bu.businesstype left join [countries] cc on cc.id = bu.countryid left join [bu_currency] c on c.id = bu.currencyid where bu.active=1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetCompanyForCustomerCount", x.Message);
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

            int divide = (int)(totalCount / Common.RECORDCOUNT);
            int mod = totalCount % Common.RECORDCOUNT;
            return (mod == 0) ? divide : divide + 1;
        }



        public static DataTable GetBusinessUnitsForUser(object xiUserId, object xiEmailId)
        {
            string query = @"select id, companyname, companyshortname, 'Owner' as userType,b.id as usertypeid,companylogo,'1'as [status]
from bu_businessuser b 
where userid= {0} and active=1 
Union All
select Distinct s.bu_id , bu.companyname, bu.companyshortname,'Staff' as userType,s.id as usertypeid,bu.companylogo,'2'as [status] 
from bu_staff s 
inner join [user] u on s.userid = u. id and u.active = 1
inner join bu_businessuser bu on bu.id = s.bu_id  and bu.active = 1  
where s.userid  = {0} and s.active=1 and s.bu_id  not in (select id from bu_businessuser b where userid= {0} and active=1 )
union all
select id, companyname, companyshortname, 'Pending' as userType,b.id as usertypeid,null as 'companylogo','0'as [status]
from bu_businessenquiry b 
where email={1} and active=1 and [status]=1";

            query = string.Format(query, Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer), Utils.ConvertToDBString(xiEmailId, Utils.DataType.String));
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static bool DeleteBusinessFilePhotos(string xiFileName)
        {
            string query = "delete from [bu_businessuser_images] where [gallery_file]=@file";
            Parameter param1 = new Parameter("file", xiFileName);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);

        }


        #region Business User Log

        public static bool AddBULog(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into bu_log(bu_id, user_id, [datetime],message_id, old_entry, new_entry, comment) values(@bu_id,@user_id,getutcdate(),@message_id,@old_entry,@new_entry,@comment)";

            Parameter param1 = new Parameter("bu_id", xiCollection["bu_id"], DbType.Int32);
            Parameter param2 = new Parameter("user_id", xiCollection["user_id"], DbType.Int32);
            Parameter param3 = new Parameter("message_id", xiCollection["message_id"], DbType.Int32);
            Parameter param4 = new Parameter("old_entry", xiCollection["old_entry"]);
            Parameter param5 = new Parameter("new_entry", xiCollection["new_entry"]);
            Parameter param6 = new Parameter("comment", xiCollection["comment"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        #endregion

        #region Business Type

        public static bool AddBusinessType(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into bu_businesstype(name)values(@name)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static bool UpdateBusinessType(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update bu_businesstype set name=@name where id=@id";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetBusinessType()
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from bu_businesstype where active=1 order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static NameValueCollection GetBusinessType(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name from bu_businesstype where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetBusinessType", x.Message);
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

        public static string DeleteBusinessType(int xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string returnValue = string.Empty;
            string query = "select count(id) as maxcount from bu_businessuser where businesstype=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                while (reader1.Read())
                {
                    totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
                    break;
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("DeleteBusinessType", x.Message);
            }
            finally
            {
                if (reader1 != null)
                {
                    reader1.Close();
                    reader1 = null;
                }
            }
            if (totalCount == 0)
            {
                query = string.Format("update bu_businesstype set active = 0 where id= {0}", xiId);
                int value = objdb.ExecuteNonQuery(objdb.con, query);
                objdb.Disconnectdb();
                returnValue = (value > 0) ? "Record deleted." : "false";
                return returnValue;
            }
            else
            {
                returnValue = "You can not delete this item because its already used.";
            }
            return returnValue;
        }

        public static DataSet GetBusinessTypes(string xiFilter)
        {
            string query = "select bt.id,bt.name from bu_businesstype bt where bt.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by bt.name";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }
        #endregion

        #region Business Documents

        public static bool AddBusinessUserGallery(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "INSERT INTO [bu_businessuser_images]([bu_id],[title],[gallery_file],[file_type],[active],[submitdate]) values(@bu_id,@title,@file_name,@file_type,1,getutcdate())";
            Parameter param1 = new Parameter("bu_id", xiCollection["bu_id"], DbType.Int32);
            Parameter param2 = new Parameter("file_name", xiCollection["file_name"]);
            Parameter param3 = new Parameter("title", xiCollection["title"]);
            Parameter param4 = new Parameter("file_type", xiCollection["file_type"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static bool AddGalleryFromBusinessEnquiry(int enquiryId, int buId)
        {
            string query = @"
        INSERT INTO bu_businessuser_images ([bu_id], title, gallery_file, file_type, active, submitdate)
        SELECT 
            @bu_id, 
            title, 
            gallery_file, 
            file_type, 
            active, 
            GETUTCDATE()
        FROM 
            bu_businessenquiry_images
        WHERE 
            enquiryid = @enquiryid AND active = 1";

            Parameter param1 = new Parameter("bu_id", buId, DbType.Int32);
            Parameter param2 = new Parameter("enquiryid", enquiryId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }


        public static DataTable GetBusinessUserPhotos(object xiBUId)
        {
            string query = "select * from bu_businessuser_images where active=1 and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " order by submitdate desc";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dt = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();
            return dt;
        }

        public static DataSet GetBusinessDocuments(object xiBUId)
        {
            string query = "select * from bu_businessuser_images where active=1 and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " order by submitdate desc";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;
        }

        public static NameValueCollection GetBusinessUserGalleryDetail(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from bu_businessuser_images  where active=1 and bu_id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("file_name", Convert.ToString(objdb.GetValue(reader1, "file_name")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetBusinessUserGalleryDetail", x.Message);
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

        public static bool DeleteBusinessUserGalleryPhoto(string xiFileName, NameValueCollection xiCollection)
        {
            string query = "delete from [bu_businessuser_images] where [gallery_file]=@file";
            Parameter param2 = new Parameter("file", xiFileName);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param2 });
            objdb.Disconnectdb();

            return (value > 0);

        }
        #endregion

        #endregion


        public static DataSet GetBUMasterData(string xiCompanyId)
        {
            string query = @"select * from bu_businesstype where active=1 order by [name];
select * from [countries] order by fullname;
select * from bu_currency where active=1 and bu_id={0} order by [name];
select * from bu_tax where active=1 and bu_id={0} order by [name];
select * from bu_staff_department where active=1 and bu_id={0} order by [name];
select * from bu_staff_jobrole where active=1 and bu_id={0} order by [name];
select s.id, u.fname + ' ' + u.lname as [name] from bu_staff s inner join [user] u on u.id=s.userid where s.active=1 and s.[bu_id] ={0} order by s.id;
select * from contact_servicetype where active=1 and (bu_id is null or bu_id ={0}) order by [name];
select act.id, act.[name], ac.breedname,(act.[name] + ' [' + ac.breedname + ']') as namewithbreedname from animal_category_type act inner join animal_categary ac on act.categoryid = ac.id where act.active = 1 and ac.active = 1 order by ac.id, act.[name];
select c.id as customerid,u.fname + ' ' + u.lname as 'custname' from bu_customer c inner join [user] u on u.id = c.userid and u.active=1 where c.bu_id={0} and c.active=1 and c.ispos IS NULL order by u.fname,u.lname;
select id2,id,[name], case when itemtype = 'P' then  [name] +' (Product)' when itemtype = 'S' then  [name] +' (Service)' when itemtype = 'C' then  [name] +' (Combo)' End as 'ItemNameType' from view_bu_all_items where active=1 and bu_id={0} ORDER BY [name];
select * from bu_product_brand where active=1 and bu_id={0} order by [name];
select * from bu_product_category where active=1 and bu_id={0} order by [name];
select * from bu_services_type where active=1 and bu_id={0} order by [name];
select * from [contact_countrycode] order by [countrycode];
select c.id as customerid,u.fname + ' ' + u.lname as 'custname' from bu_customer c inner join [user] u on u.id = c.userid and u.active=1 where c.bu_id={0} and c.active=1 and c.ispos=1 order by u.fname,u.lname";

            query = string.Format(query, Utils.ConvertToDBString(xiCompanyId, Utils.DataType.Integer));
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static DataSet GetBUMasterDataCount(string xiCompanyId)
        {
            string query = @"select count(*) as cnt from bu_businesstype where active=1;
        select count(*) as cnt from [countries];
        select count(*) as cnt from bu_currency where active=1 and bu_id={0};
        select count(*) as cnt from bu_tax where active=1 and bu_id={0};
        select count(*) as cnt from bu_staff_department where active=1 and bu_id={0};
        select count(*) as cnt from bu_staff_jobrole where active=1 and bu_id={0};
        select count(*) as cnt from bu_staff s inner join [user] u on u.id=s.userid where s.active=1 and s.[bu_id]={0};
        select count(*) as cnt from contact_servicetype where active=1 and (bu_id is null or bu_id={0});
        select count(*) as cnt from animal_category_type act inner join animal_categary ac on act.categoryid = ac.id where act.active = 1 and ac.active = 1;
        select count(*) as cnt from bu_customer c inner join [user] u on u.id = c.userid and u.active=1 where c.bu_id={0} and c.active=1 and c.ispos IS NULL;
        select count(*) as cnt from view_bu_all_items where active=1 and bu_id={0};
        select count(*) as cnt from bu_product_brand where active=1 and bu_id={0};
        select count(*) as cnt from bu_product_category where active=1 and bu_id={0};
        select count(*) as cnt from bu_services_type where active=1 and bu_id={0};
        select count(*) as cnt from [contact_countrycode];
        select count(*) as cnt from bu_customer c inner join [user] u on u.id = c.userid and u.active=1 where c.bu_id={0} and c.active=1 and c.ispos=1;
		select count(*) as cnt from bu_product where active=1 and bu_id={0};
        select count(*) as cnt from bu_services where active=1 and bu_id={0};
		select count(*) as cnt from bu_combodetails where active=1 and bu_id={0};
        select count(taxid) as cnt from bu_businessuser where active=1 and id={0};
		select count(currencyid) as cnt from bu_businessuser where active=1 and id={0};";

            query = string.Format(query, Utils.ConvertToDBString(xiCompanyId, Utils.DataType.Integer));
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }
    }
}
