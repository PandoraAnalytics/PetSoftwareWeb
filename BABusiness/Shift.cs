using BADBUtils;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BABusiness
{
    public class Shift : BusinessBase
    {
        #region Shift
        public static int AddShift(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "INSERT INTO [bu_shift](shift_name, shift_typeid, startdatetime, enddatetime, break_time_duration,status, created, createdby, updated, updatedby, active,bu_id) values(@shift_name,@shift_typeid,@startdatetime,@enddatetime,@break_time_duration,@status,getutcdate(),@createdby,getutcdate(),@updatedby,@active,@bu_id)";

            Parameter param1 = new Parameter("shift_name", xiCollection["shift_name"]);
            Parameter param2 = new Parameter("shift_typeid", xiCollection["shift_typeid"], DbType.Int32);
            Parameter param3 = new Parameter("startdatetime", xiCollection["startdatetime"], DbType.Time);
            Parameter param4 = new Parameter("enddatetime", xiCollection["enddatetime"], DbType.Time);
            Parameter param5 = new Parameter("break_time_duration", xiCollection["break_time_duration"], DbType.Int32);
            Parameter param6 = new Parameter("status", xiCollection["status"], DbType.Int32);
            Parameter param7 = new Parameter("createdby", xiCollection["userid"], DbType.Int32);
            Parameter param8 = new Parameter("updatedby", xiCollection["userid"], DbType.Int32);
            Parameter param9 = new Parameter("active", "1");
            Parameter param10 = new Parameter("bu_id", xiCollection["companyid"], DbType.Int32);


            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9, param10 });
            objdb.Disconnectdb();

            return value;
        }

        public static bool UpdateShift(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update [bu_shift] set shift_name=@shift_name,shift_typeid=@shift_typeid,startdatetime=@startdatetime,enddatetime=@enddatetime,break_time_duration=@break_time_duration,status=@status,updated=getutcdate() where id=@id";
            Parameter param1 = new Parameter("shift_name", xiCollection["shift_name"]);
            Parameter param2 = new Parameter("shift_typeid", xiCollection["shift_typeid"], DbType.Int32);
            Parameter param3 = new Parameter("startdatetime", xiCollection["startdatetime"], DbType.Time);
            Parameter param4 = new Parameter("enddatetime", xiCollection["enddatetime"], DbType.Time);
            Parameter param5 = new Parameter("break_time_duration", xiCollection["break_time_duration"], DbType.Int32);
            Parameter param6 = new Parameter("status", xiCollection["status"], DbType.Int32);
            Parameter param7 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6, param7 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static string DeleteShift(object xiId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string returnValue = string.Empty;

            String query = string.Format("update bu_shift set active = 0 where id= {0}", Utils.ConvertToDBString(xiId, Utils.DataType.Integer));

            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query);
            objdb.Disconnectdb();

            returnValue = (value > 0) ? "Deleted Successfully." : "false";

            return returnValue;
        }

        public static int GetShiftCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(c.id) as maxcount from [bu_shift] c  where c.active = 1";
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
                objdb.Write_log_file("GetShiftCount", x.Message);
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

        public static DataSet GetShiftDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select c.* from [bu_shift] c where c.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by c.shift_name offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;
        }

        public static NameValueCollection GetShiftDetail(object xiId, object xiBUId)
        {
            NameValueCollection collection = null;

            string query = "select c.* from bu_shift c where c.id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and c.bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " and c.active = 1";
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
                    collection.Add("shift_name", Convert.ToString(objdb.GetValue(reader1, "shift_name")));
                    collection.Add("startdatetime", Convert.ToString(objdb.GetValue(reader1, "startdatetime")));
                    collection.Add("enddatetime", Convert.ToString(objdb.GetValue(reader1, "enddatetime")));
                    collection.Add("break_time_duration", Convert.ToString(objdb.GetValue(reader1, "break_time_duration")));
                    collection.Add("shift_typeid", Convert.ToString(objdb.GetValue(reader1, "shift_typeid")));
                    collection.Add("status", Convert.ToString(objdb.GetValue(reader1, "status")));
                    collection.Add("updatedby", Convert.ToString(objdb.GetValue(reader1, "updatedby")));
                    collection.Add("createdby", Convert.ToString(objdb.GetValue(reader1, "createdby")));
                    collection.Add("bu_id", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetShiftDetail", x.Message);
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
                builder.Append(string.Format("(c.shift_name like {0})", Utils.ConvertToDBString("%" + xiCollection["name"] + "%", Utils.DataType.String)));
            }
            return builder.ToString();
        }

        #endregion

    }
}
