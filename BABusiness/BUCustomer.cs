using BADBUtils;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml.Linq;

namespace BABusiness
{
    public class BUCustomer : BusinessBase
    {        
        public static int AddCustomer(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;
            string query = "INSERT INTO bu_customer(bu_id,userid,gender,dob,alternatecontact,membershiptype,customercode,createdby,createddate,ispos)values(@bu_id,@userid,@gender,@dob,@alternatecontact,@membershiptype,@customercode,@createdby,getutcdate(),@ispos)";
            Parameter param1 = new Parameter("bu_id", xiCollection["companyid"], DbType.Int32);
            Parameter param2 = new Parameter("userid", xiCollection["userid"], DbType.Int32);
            Parameter param3 = new Parameter("gender", xiCollection["gender"], DbType.Int32);
            Parameter param4 = new Parameter("dob", xiCollection["dob"], DbType.DateTime);
            Parameter param5 = new Parameter("alternatecontact", xiCollection["alternatecontact"], DbType.Int64);
            Parameter param6 = new Parameter("membershiptype", xiCollection["membershiptype"], DbType.Int32);
            Parameter param7 = new Parameter("customercode", xiCollection["customercode"]);
            Parameter param8 = new Parameter("createdby", xiCollection["createdby"], DbType.Int32);
            Parameter param9 = new Parameter("ispos", xiCollection["ispos"], DbType.Int16);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9 });
            objdb.Disconnectdb();

            return value;
        }

        public static bool UpdateCustomer(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update [bu_customer] set gender=@gender,dob=@dob,alternatecontact=@alternatecontact,membershiptype=@membershiptype,updatedby=@updatedby,updateddate=getutcdate() where id=@id";

            Parameter param1 = new Parameter("gender", xiCollection["gender"], DbType.Int32);
            Parameter param2 = new Parameter("dob", xiCollection["dob"], DbType.DateTime);
            Parameter param3 = new Parameter("alternatecontact", xiCollection["alternatecontact"], DbType.Int64);
            Parameter param4 = new Parameter("membershiptype", xiCollection["membershiptype"], DbType.Int32);
            Parameter param5 = new Parameter("updatedby", xiCollection["updatedby"], DbType.Int32);
            Parameter param6 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int GetCustomerCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(c.id) as maxcount from bu_customer c inner join [user] u on u.id = c.userid where c.active = 1";
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
                objdb.Write_log_file("GetCustomerCount", x.Message);
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

        public static DataSet GetCustomerDetails(int xiPage, string xiFilter, string xiSort)
        {
            xiPage = (xiPage == 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            string query = @"select c.*,u.fname as fname,u.lname as lname,u.email as email from bu_customer c inner join [user] u on u.id = c.userid where c.active=1";
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
            string sortcolumn = "c.id desc";

            string[] sortorder = BusinessBase.SplitSort(xiSort);
            switch (sortorder[0])
            {
                case "1":
                    sortcolumn = "u.fname " + sortorder[1] + ", u.lname " + sortorder[1];
                    break;

                case "2":
                    sortcolumn = "u.email " + sortorder[1];
                    break;

                case "3":
                    sortcolumn = "c.membershiptype " + sortorder[1];
                    break;
            }

            return sortcolumn;
        }

        public static NameValueCollection GetCustomerDetail(object xiId, object xiBUId)
        {
            NameValueCollection collection = null;

            string query = "select c.*,u.fname as fname,u.lname as lname,u.email as email,u.phone as phone,u.address as address,u.city as city,u.pincode as pincode,u.countryid as countryid,u.contactcountrycode as ucontactcountrycode,cc.fullname as countryname,ccc1.countrycode as 'userphoneprefix' from bu_customer c inner join [user] u on u.id = c.userid left join [countries] cc on cc.id = u.countryid inner join [contact_countrycode] ccc1 on ccc1.id = u.contactcountrycode where c.id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and  c.bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " and c.active = 1";
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
                    collection.Add("bu_id", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                    collection.Add("userid", Convert.ToString(objdb.GetValue(reader1, "userid")));
                    collection.Add("fname", Convert.ToString(objdb.GetValue(reader1, "fname")));
                    collection.Add("lname", Convert.ToString(objdb.GetValue(reader1, "lname")));
                    collection.Add("email", Convert.ToString(objdb.GetValue(reader1, "email")));
                    collection.Add("phone", Convert.ToString(objdb.GetValue(reader1, "phone")));
                    collection.Add("ucontactcountrycode", Convert.ToString(objdb.GetValue(reader1, "ucontactcountrycode")));
                    collection.Add("userphoneprefix", Convert.ToString(objdb.GetValue(reader1, "userphoneprefix")));
                    collection.Add("address", Convert.ToString(objdb.GetValue(reader1, "address")));
                    collection.Add("countryid", Convert.ToString(objdb.GetValue(reader1, "countryid")));
                    collection.Add("countryname", Convert.ToString(objdb.GetValue(reader1, "countryname")));
                    collection.Add("city", Convert.ToString(objdb.GetValue(reader1, "city")));
                    collection.Add("pincode", Convert.ToString(objdb.GetValue(reader1, "pincode")));
                    collection.Add("gender", Convert.ToString(objdb.GetValue(reader1, "gender")));
                    collection.Add("customercode", Convert.ToString(objdb.GetValue(reader1, "customercode")));
                    collection.Add("dob", Convert.ToString(objdb.GetValue(reader1, "dob")));
                    collection.Add("alternatecontact", Convert.ToString(objdb.GetValue(reader1, "alternatecontact")));
                    collection.Add("membershiptype", Convert.ToString(objdb.GetValue(reader1, "membershiptype")));
                    collection.Add("createdby", Convert.ToString(objdb.GetValue(reader1, "createdby")));
                    collection.Add("createddate", Convert.ToString(objdb.GetValue(reader1, "createddate")));
                    collection.Add("updatedby", Convert.ToString(objdb.GetValue(reader1, "updatedby")));
                    collection.Add("updateddate", Convert.ToString(objdb.GetValue(reader1, "updateddate")));
                    collection.Add("ispos", Convert.ToString(objdb.GetValue(reader1, "ispos")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetCustomerDetail", x.Message);
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

            if (xiCollection["companyid"] != null && xiCollection["companyid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.bu_id={0})", Utils.ConvertToDBString(xiCollection["companyid"], Utils.DataType.Integer)));
            }

            if (xiCollection["name"] != null && xiCollection["name"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(u.fname + ' ' + u.lname like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }

            if (xiCollection["email"] != null && xiCollection["email"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(u.email like {0})", Utils.ConvertToDBString("%" + xiCollection["email"] + "%", Utils.DataType.String)));
            }
            return builder.ToString();
        }

        public static bool DeleteCustomer(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update bu_customer set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int IsCustomerCodeExist(string xiCustomerCode)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "SELECT COUNT(c.id) AS maxcount FROM bu_customer c WHERE c.customercode = " + Utils.ConvertToDBString(xiCustomerCode, Utils.DataType.String) + " AND c.active = 1";

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                    totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception ex)
            {
                objdb.Write_log_file("IsCustomerCodeExist", ex.Message);
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

        public static int GetCustomerIdByUserID(object xiUserID, object xiCompanyID)
        {
            string query = "select c.id from bu_customer c inner join bu_businessuser bu on bu.id = c.bu_id inner join [user] u on u.id = c.userid and u.active = 1 where c.userid=" + Utils.ConvertToDBString(xiUserID, Utils.DataType.Integer) + " and c.bu_id = " + Utils.ConvertToDBString(xiCompanyID, Utils.DataType.Integer) + " and bu.active=1 and c.active= 1";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            object value = objdb.ExecuteScalar(objdb.con, query);
            objdb.Disconnectdb();

            return BusinessBase.ConvertToInteger(value);
        }

        public static NameValueCollection GetCustomerByAnimalId(object xiAnimalId, object xiCompanyId)
        {
            NameValueCollection collection = null;

            string query = @"select u.fname as 'fname', u.lname as 'lname', u.id as 'userid', bc.id as 'customerid' from animal a inner join user_animal ua on a.id  = ua.animalid and ua.active = 1
inner join bu_customer bc on bc.userid = ua.userid and bc.active = 1 and bc.bu_id = {0}
inner join [user] u on u.id = ua.userid and u.active = 1
where a.id = {1}";

            query = string.Format(query, Utils.ConvertToDBString(xiCompanyId, Utils.DataType.Integer), Utils.ConvertToDBString(xiAnimalId, Utils.DataType.Integer));

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
                    collection.Add("userid", Convert.ToString(objdb.GetValue(reader1, "userid")));
                    collection.Add("fname", Convert.ToString(objdb.GetValue(reader1, "fname")));
                    collection.Add("lname", Convert.ToString(objdb.GetValue(reader1, "lname")));
                    collection.Add("customerid", Convert.ToString(objdb.GetValue(reader1, "customerid")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetCustomerDetail", x.Message);
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
    }
}
