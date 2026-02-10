using BADBUtils;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BABusiness
{
    public class AddressBA : BusinessBase
    {
        #region Address

        public bool AddAddress(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;
            string query = "INSERT INTO [address]([userid],[addressline1],[city],[country],[pincode],[active],[submitdate]) values(@[userid],@[addressline1],@[city],@[country],@[pincode],@[active],getutcdate())";

            Parameter param1 = new Parameter("userid", xiCollection["userid"], DbType.Int32);
            Parameter param2 = new Parameter("addressline1", xiCollection["addressline1"]);
            Parameter param3 = new Parameter("city", xiCollection["city"], DbType.Int32);
            Parameter param4 = new Parameter("country", xiCollection["country"], DbType.Int32);
            Parameter param5 = new Parameter("pincode", xiCollection["pincode"], DbType.Int32);
            Parameter param6 = new Parameter("active", xiCollection["active"], DbType.Int32);
            // Parameter param7 = new Parameter("submitdate", xiCollection["submitdate"], DbType.DateTime);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public bool UpdateAddress(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update [address] set userid=@userid,addressline1=@addressline1,city=@city,country=@country,pincode=@pincode,type=@type,submitdate=getutcdate(),active=@active,is_verified=@is_verified,password_token=@password_token,user_token=@user_token,user_token_date=@user_token_date where id=@id";
            Parameter param1 = new Parameter("userid", xiCollection["userid"]);
            Parameter param2 = new Parameter("addressline1", xiCollection["addressline1"]);
            Parameter param3 = new Parameter("city", xiCollection["city"]);
            Parameter param4 = new Parameter("country", xiCollection["country"]);
            Parameter param5 = new Parameter("pincode", xiCollection["pincode"]);
            //  Parameter param6 = new Parameter("submitdate", xiCollection["submitdate"], DbType.DateTime);
            Parameter param7 = new Parameter("id", xiId);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param7 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetAddressDetail(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select [userid],[addressline1],[city],[country],[pincode],[submitdate] from [address] where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("userid", Convert.ToString(objdb.GetValue(reader1, "userid")));
                    collection.Add("addressline1", Convert.ToString(objdb.GetValue(reader1, "addressline1")));
                    collection.Add("city", Convert.ToString(objdb.GetValue(reader1, "city")));
                    collection.Add("country", Convert.ToString(objdb.GetValue(reader1, "country")));
                    collection.Add("pincode", Convert.ToString(objdb.GetValue(reader1, "pincode")));
                    collection.Add("submitdate", Convert.ToString(objdb.GetValue(reader1, "submitdate")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetAddressDetail", x.Message);
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

        public static string DeleteAddress(int xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string returnValue = string.Empty;
            string query = "select count(id) as maxcount from [address] where id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1";
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
                objdb.Write_log_file("DeleteAddress", x.Message);
            }
            finally
            {
                if (reader1 != null)
                {
                    reader1.Close();
                    reader1 = null;
                }
            }
            //if (totalCount == 0)
            //{
            //    query = string.Format("update eam_assestgroup set active = 0 where id= {0}", xiId);
            //    int value = objdb.ExecuteNonQuery(objdb.con, query);
            //    objdb.Disconnectdb();
            //    returnValue = (value > 0) ? "Record deleted." : "false";
            //    return returnValue;
            //}
            //else
            //{
            //    returnValue = "You can not delete this item because its already used.";
            //}
            return returnValue;
        }

        public static int GetAddressCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(id) as maxcount from [address] where active = 1";
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
                objdb.Write_log_file("GetAddressCount", x.Message);
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

        public static DataSet GetAddressDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select * from [address] where active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by id offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("lastmodified", typeof(string));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime tempDate1 = Convert.ToDateTime(row["submitdate"]);
                    if (tempDate1 != DateTime.MinValue)
                    {
                        row["lastmodified"] = tempDate1.ToString(BusinessBase.DateTimeFormat);
                    }
                }
            }
            objdb.Disconnectdb();
            return ds;
        }

        #endregion

        #region Contact

        public int AddContact(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "INSERT INTO [contact]([fname],[lname],[email],[phone],[service_type],[note],[active],[submitdate],[createdby],[bu_id],[addressoptional]) values(@fname,@lname,@email,@phone,@service_type,@note,@active,getutcdate(),@userid,@companyid,@addressoptional)";

            Parameter param1 = new Parameter("fname", xiCollection["firstname"]);
            Parameter param2 = new Parameter("lname", xiCollection["lastname"]);
            Parameter param3 = new Parameter("email", xiCollection["email"]);
            Parameter param4 = new Parameter("phone", xiCollection["contact"]);
            Parameter param5 = new Parameter("service_type", xiCollection["profession"], DbType.Int32);
            Parameter param6 = new Parameter("note", xiCollection["about"]);
            Parameter param7 = new Parameter("userid", xiCollection["userid"], DbType.Int32);
            Parameter param8 = new Parameter("active", "1");
            Parameter param9 = new Parameter("companyid", xiCollection["companyid"], DbType.Int32);
            Parameter param10 = new Parameter("addressoptional", xiCollection["addressoptional"]);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9, param10 });
            objdb.Disconnectdb();

            return value;
        }

        public bool UpdateContact(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update [contact] set fname=@fname,lname=@lname,email=@email,phone=@phone,service_type=@service_type,note=@note,addressoptional=@addressoptional where id=@id";
            Parameter param1 = new Parameter("fname", xiCollection["firstname"]);
            Parameter param2 = new Parameter("lname", xiCollection["lastname"]);
            Parameter param3 = new Parameter("email", xiCollection["email"]);
            Parameter param4 = new Parameter("phone", xiCollection["contact"]);
            Parameter param5 = new Parameter("service_type", xiCollection["profession"], DbType.Int32);
            Parameter param6 = new Parameter("note", xiCollection["about"]);
            Parameter param7 = new Parameter("id", xiId, DbType.Int32);
            Parameter param8 = new Parameter("addressoptional", xiCollection["addressoptional"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public void DeleteOtherEmailPhoneInContact(object xiId)
        {
            string query = "delete from contact_otherdetails where contactid=@contactid";
            Parameter param1 = new Parameter("contactid", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();
        }

        public bool AddOtherEmailPhoneInContact(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "insert into contact_otherdetails([contactid],[email],[phone],[type]) values(@contactid,@email,@phone,@type)";
            Parameter param1 = new Parameter("contactid", xiId, DbType.Int32);
            Parameter param2 = new Parameter("email", xiCollection["email"]);
            Parameter param3 = new Parameter("phone", xiCollection["phone"]);
            Parameter param4 = new Parameter("type", xiCollection["type"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetOtherEmailPhoneInContact(object xiId)
        {
            string query = "select * from contact_otherdetails where contactid=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " order by [type],id";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static string DeleteContact(object xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string query = @"select count(a.id) as maxcount from animal_appointment a 
   inner join contact c on a.contactid = c.id where a.contactid = " + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and a.active=1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("DeleteContact", x.Message);
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
                query = string.Format("update contact set active = 0 where id= {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));

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

        public static int GetContactCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(c.id) as maxcount from [contact] c inner join contact_servicetype st on c.service_type = st.id and st.active=1 where c.active = 1";
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
                objdb.Write_log_file("GetContactCount", x.Message);
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

        public static DataSet GetContactDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select c.*, st.[name] as service_typename from [contact] c inner join contact_servicetype st on c.service_type = st.id and st.active=1 where c.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by c.fname, c.lname offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;
        }

        public static NameValueCollection GetContactDetail(object xiId)
        {
            NameValueCollection collection = null;

            string query = "select c.*, st.[name] as service_typename from contact c inner join contact_servicetype st on c.service_type = st.id and st.active=1 where c.id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and c.active = 1";
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;
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
                    collection.Add("service_type", Convert.ToString(objdb.GetValue(reader1, "service_type")));
                    collection.Add("note", Convert.ToString(objdb.GetValue(reader1, "note")));
                    collection.Add("submitdate", Convert.ToString(objdb.GetValue(reader1, "submitdate")));
                    collection.Add("createdby", Convert.ToString(objdb.GetValue(reader1, "createdby")));
                    collection.Add("service_typename", Convert.ToString(objdb.GetValue(reader1, "service_typename")));
                    collection.Add("companyid", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                    collection.Add("addressoptional", Convert.ToString(objdb.GetValue(reader1, "addressoptional")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetContactDetail", x.Message);
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

        public static string Search(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["userid"] != null && xiCollection["userid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.createdby={0})", Utils.ConvertToDBString(xiCollection["userid"], Utils.DataType.Integer)));
            }


            if (xiCollection["name"] != null && xiCollection["name"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.fname + ' ' + c.lname like {0} or c.lname + ' ' + c.fname like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }

            if (xiCollection["companyid"] != null && xiCollection["companyid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.bu_id is null or c.bu_id={0})", Utils.ConvertToDBString(xiCollection["companyid"], Utils.DataType.Integer)));
            }
            return builder.ToString();
        }

        public static int DeleteContactValue(object xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string query = @"select count(a.id) as maxcount from animal_appointment a 
   inner join contact c on a.contactid = c.id where a.contactid = " + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and a.active=1";
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("DeleteContact", x.Message);
            }
            finally
            {
                if (reader1 != null)
                {
                    reader1.Close();
                    reader1 = null;
                }
            }

            int returnValue = int.MinValue;
            if (totalCount == 0)
            {
                query =  string.Format("update contact set active = 0 where id= {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));

                objdb.Connectdb();
                int value = objdb.ExecuteNonQuery(objdb.con, query);
                objdb.Disconnectdb();

                returnValue =  (value > 0) ? 1 : 0;
            }
            else
            {
                returnValue = 2; //"You can not delete this item because its already used.";
            }
            objdb.Disconnectdb();

            return returnValue;
        }

        #endregion
    }
}

