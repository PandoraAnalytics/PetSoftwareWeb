using BADBUtils;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BABusiness
{
    public class BUStaff : BusinessBase
    {
        public static int AddStaff(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;
            string query = "INSERT INTO bu_staff(bu_id,userid,gender,dob,alternatecontact,jobtitle,department,jobrole,joiningdate,employmentstatus,supervisorid,employeecode,createdby,submitdate)values(@bu_id,@userid,@gender,@dob,@alternatecontact,@jobtitle,@department,@jobrole,@joiningdate,@employmentstatus,@supervisorid,@employeecode,@createdby,getutcdate())";
            Parameter param1 = new Parameter("bu_id", xiCollection["companyid"], DbType.Int32);
            Parameter param2 = new Parameter("userid", xiCollection["userid"], DbType.Int32);
            Parameter param3 = new Parameter("gender", xiCollection["gender"], DbType.Int32);
            Parameter param4 = new Parameter("dob", xiCollection["dob"], DbType.DateTime);
            Parameter param5 = new Parameter("alternatecontact", xiCollection["alternatecontact"], DbType.Int64);
            Parameter param6 = new Parameter("jobtitle", xiCollection["jobtitle"]);
            Parameter param7 = new Parameter("department", xiCollection["department"], DbType.Int32);
            Parameter param8 = new Parameter("jobrole", xiCollection["jobrole"], DbType.Int32);
            Parameter param9 = new Parameter("joiningdate", xiCollection["joiningdate"], DbType.DateTime);
            Parameter param10 = new Parameter("employmentstatus", xiCollection["employmentstatus"], DbType.Int32);
            Parameter param11 = new Parameter("supervisorid", xiCollection["supervisorid"], DbType.Int32);
            Parameter param12 = new Parameter("employeecode", xiCollection["employeecode"]);
            Parameter param13 = new Parameter("createdby", xiCollection["createdby"], DbType.Int32);


            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13 });
            objdb.Disconnectdb();

            return value;
        }

        public static bool UpdateStaff(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update [bu_staff] set gender=@gender,dob=@dob,alternatecontact=@alternatecontact,jobtitle=@jobtitle,department=@department,jobrole=@jobrole,joiningdate=@joiningdate,employmentstatus=@employmentstatus,supervisorid=@supervisorid where id=@id";

            Parameter param1 = new Parameter("gender", xiCollection["gender"], DbType.Int32);
            Parameter param2 = new Parameter("dob", xiCollection["dob"], DbType.DateTime);
            Parameter param3 = new Parameter("alternatecontact", xiCollection["alternatecontact"], DbType.Int64);
            Parameter param4 = new Parameter("jobtitle", xiCollection["jobtitle"]);
            Parameter param5 = new Parameter("department", xiCollection["department"], DbType.Int32);
            Parameter param6 = new Parameter("jobrole", xiCollection["jobrole"], DbType.Int32);
            Parameter param7 = new Parameter("joiningdate", xiCollection["joiningdate"], DbType.DateTime);
            Parameter param8 = new Parameter("employmentstatus", xiCollection["employmentstatus"], DbType.Int32);
            Parameter param9 = new Parameter("supervisorid", xiCollection["supervisorid"], DbType.Int32);
            Parameter param10 = new Parameter("id", xiId, DbType.Int32);


            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9, param10 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int GetStaffCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(s.id) as maxcount from bu_staff s inner join [user] u on u.id = s.userid where s.active = 1";
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
                objdb.Write_log_file("GetStaffCount", x.Message);
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

        public static DataSet GetStaffDetails(int xiPage, string xiFilter, string xiSort)
        {
            xiPage = (xiPage == 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            string query = @"select s.*,u.fname as fname,u.lname as lname,u.email as email from bu_staff s inner join [user] u on u.id = s.userid where s.active=1";
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
            string sortcolumn = "s.id desc";

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
                    sortcolumn = "s.jobtitle " + sortorder[1];
                    break;

                case "4":
                    sortcolumn = "s.employmentstatus " + sortorder[1];
                    break;
            }

            return sortcolumn;
        }

        public static NameValueCollection GetStaffDetail(object xiId, object xiBUId)
        {
            NameValueCollection collection = null;

            string query = "select s.*,u.fname as fname,u.lname as lname,u.email as email,u.phone as phone,u.address as address,sd.name as departmentname,sj.name as jobrolename,u1.fname as sup_fname,u1.lname as sup_lname from bu_staff s left join bu_staff s1 on s1.id = s.supervisorid left join [user] u1 on u1.id = s1.userid  inner join [user] u on u.id = s.userid left join bu_staff_department sd on sd.id=s.department left join bu_staff_jobrole sj on sj.id= s.jobrole where s.id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and s.bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " and s.active = 1";
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
                    collection.Add("address", Convert.ToString(objdb.GetValue(reader1, "address")));
                    collection.Add("employeecode", Convert.ToString(objdb.GetValue(reader1, "employeecode")));
                    collection.Add("gender", Convert.ToString(objdb.GetValue(reader1, "gender")));
                    collection.Add("dob", Convert.ToString(objdb.GetValue(reader1, "dob")));
                    collection.Add("alternatecontact", Convert.ToString(objdb.GetValue(reader1, "alternatecontact")));
                    collection.Add("jobtitle", Convert.ToString(objdb.GetValue(reader1, "jobtitle")));
                    collection.Add("department", Convert.ToString(objdb.GetValue(reader1, "department")));
                    collection.Add("departmentname", Convert.ToString(objdb.GetValue(reader1, "departmentname")));
                    collection.Add("jobrole", Convert.ToString(objdb.GetValue(reader1, "jobrole")));
                    collection.Add("joiningdate", Convert.ToString(objdb.GetValue(reader1, "joiningdate")));
                    collection.Add("jobrolename", Convert.ToString(objdb.GetValue(reader1, "jobrolename")));
                    collection.Add("employmentstatus", Convert.ToString(objdb.GetValue(reader1, "employmentstatus")));
                    collection.Add("supervisorid", Convert.ToString(objdb.GetValue(reader1, "supervisorid")));
                    collection.Add("sup_fname", Convert.ToString(objdb.GetValue(reader1, "sup_fname")));
                    collection.Add("sup_lname", Convert.ToString(objdb.GetValue(reader1, "sup_lname")));
                    collection.Add("createdby", Convert.ToString(objdb.GetValue(reader1, "createdby")));
                    collection.Add("submitdate", Convert.ToString(objdb.GetValue(reader1, "submitdate")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetStaffDetail", x.Message);
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
                builder.Append(string.Format("(s.bu_id={0})", Utils.ConvertToDBString(xiCollection["companyid"], Utils.DataType.Integer)));
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

        public static bool DeleteStaff(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update bu_staff set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetStaff(string xiBUId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select s.id, u.fname + ' ' + u.lname as [name] from bu_staff s inner join [user] u on u.id=s.userid where s.active=1 and s.[bu_id] = " + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " order by s.id";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static int IsEmployeeCodeExist(string xiEmployeeCode)
        {
            int totalCount = 0;

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "SELECT COUNT(s.id) AS maxcount FROM bu_staff s WHERE s.employeecode = " + Utils.ConvertToDBString(xiEmployeeCode, Utils.DataType.String) + " AND s.active = 1";

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                    totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception ex)
            {
                objdb.Write_log_file("IsCodeExist", ex.Message);
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

        #region Department

        public static bool AddStaffDepartment(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into bu_staff_department(name,bu_id)values(@name,@bu_id)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("bu_id", xiCollection["companyid"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static bool UpdateStaffDepartment(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update bu_staff_department set name=@name where id=@id";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetAllStaffDepartment(string xiBUId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from bu_staff_department where active=1 and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static NameValueCollection GetStaffDepartment(object xiId, object xiBUId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name,bu_id from bu_staff_department where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("bu_id", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetStaffDepartment", x.Message);
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

        public static string DeleteStaffDepartment(int xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string returnValue = string.Empty;
            string query = "select count(id) as maxcount from bu_staff where department=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1";
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
                objdb.Write_log_file("DeleteStaffDepartment", x.Message);
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
                query = string.Format("update bu_staff_department set active = 0 where id= {0}", xiId);
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

        public static DataSet GetStaffDepartments(string xiFilter, string xiBUId)
        {
            string query = "select sd.id,sd.name from bu_staff_department sd where sd.active = 1 and sd.bu_id = " + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by sd.name";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        #endregion

        #region Job Role

        public static bool AddStaffJobRole(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "insert into bu_staff_jobrole(name,bu_id)values(@name,@bu_id)";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("bu_id", xiCollection["companyid"]);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static bool UpdateStaffJobRole(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update bu_staff_jobrole set name=@name where id=@id";

            Parameter param1 = new Parameter("name", xiCollection["name"]);
            Parameter param2 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static DataTable GetAllStaffJobRoles(string xiBUId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from bu_staff_jobrole where active=1 and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static NameValueCollection GetStaffJobRole(object xiId, object xiBUId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            SqlDataReader reader1 = null;

            string query = "select id,name,bu_id from bu_staff_jobrole where active=1 and id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);

            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("bu_id", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetStaffJobRole", x.Message);
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

        public static string DeleteStaffJobRole(int xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int totalCount = 0;
            SqlDataReader reader1 = null;
            string returnValue = string.Empty;
            string query = "select count(id) as maxcount from bu_staff where jobrole=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and active = 1";
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
                objdb.Write_log_file("DeleteStaffJobRole", x.Message);
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
                query = string.Format("update bu_staff_jobrole set active = 0 where id= {0}", xiId);
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

        public static DataSet GetStaffJobRoles(string xiFilter, string xiBUId)
        {
            string query = "select sr.id,sr.name from bu_staff_jobrole sr where sr.active = 1 and sr.bu_id = " + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by sr.name";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        #endregion

        #region Staff Images

        public static bool AddStaffGallery(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return false;

            string query = "INSERT INTO [bu_staff_images]([staffid],[title],[gallery_file],[file_type],[active],[submitdate]) values(@staffid,@title,@file_name,@file_type,1,getutcdate())";
            Parameter param1 = new Parameter("staffid", xiCollection["staffid"], DbType.Int32);
            Parameter param2 = new Parameter("file_name", xiCollection["file_name"]);
            Parameter param3 = new Parameter("title", xiCollection["title"]);
            Parameter param4 = new Parameter("file_type", xiCollection["file_type"], DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4 });
            objdb.Disconnectdb();

            //NameValueCollection logCollection = new NameValueCollection();
            //logCollection["staff_id"] = xiCollection["staffid"];
            //logCollection["user_id"] = xiCollection["userid"];
            //logCollection["message_id"] = (int)BUProduct.Status.PRODUCTIMAGEADD + "";
            //logCollection["old_entry"] = string.Empty;
            //logCollection["new_entry"] = string.Empty;
            //logCollection["comment"] = xiCollection["file_name"];
            //BUProduct.AddProductLog(logCollection);

            return (value > 0);
        }

        public static DataSet GetStaffPhotos(object xiStaffId)
        {
            string query = "select * from [bu_staff_images] where active=1 and staffid=" + Utils.ConvertToDBString(xiStaffId, Utils.DataType.Integer) + " order by submitdate desc";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;
        }

        public static NameValueCollection GetStaffGallaryDetail(object xiId)
        {
            NameValueCollection collection = null;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select * from bu_staff_images  where active=1 and staffid=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer);

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
                objdb.Write_log_file("GetStaffGallaryDetail", x.Message);
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

        public static bool DeleteStaffGalleryPhoto(string xiFileName, NameValueCollection xiCollection)
        {
            string query = "delete from [bu_staff_images] where [gallery_file]=@file";
            Parameter param2 = new Parameter("file", xiFileName);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param2 });
            objdb.Disconnectdb();
            if (xiCollection != null)
            {
                //NameValueCollection logCollection = new NameValueCollection();
                //logCollection["staff_id"] = xiCollection["staffid"];
                //logCollection["user_id"] = xiCollection["userid"];
                //logCollection["message_id"] = (int)BUProduct.Status.PRODUCTIMAGEDELETE + "";
                //logCollection["old_entry"] = string.Empty;
                //logCollection["new_entry"] = string.Empty;
                //logCollection["comment"] = xiFileName;
                //BUProduct.AddProductLog(logCollection);
            }
            return (value > 0);

        }

        #endregion
    }
}
