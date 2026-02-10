using BADBUtils;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Threading;

namespace BABusiness
{
    public class BUOrderManagement : BusinessBase
    {
        #region Order

        public static int AddBUOrder(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "INSERT INTO [bu_order]([orderno],[customerid],[orderdate],[status],[totalcost],[staffid],[createddate],[termscondition],[bu_id]) values((select isnull(max(orderno), 0)+1 from bu_order),@customerid,@orderdate,@status,@totalcost,@staffid,getutcdate(),@termscondition,@bu_id)";

            Parameter param1 = new Parameter("customerid", xiCollection["customerid"], DbType.Int32);
            Parameter param2 = new Parameter("orderdate", xiCollection["orderdate"], DbType.Date);
            Parameter param3 = new Parameter("status", xiCollection["status"], DbType.Int32);
            Parameter param4 = new Parameter("totalcost", xiCollection["totalcost"], DbType.Decimal);
            Parameter param5 = new Parameter("staffid", xiCollection["staffid"], DbType.Int32);
            Parameter param6 = new Parameter("bu_id", xiCollection["companyid"], DbType.Int32);
            Parameter param7 = new Parameter("termscondition", xiCollection["termscondition"], DbType.String);
            // termsncondition

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6, param7 });
            objdb.Disconnectdb();

            return value;
        }

        public static bool UpdateBUOrder(NameValueCollection xiCollection, object xiId)
        {
            if (xiCollection == null) return false;

            string query = "update [bu_order] set paymentoption=@paymentoption,termscondition=@termscondition,referencefile=@referencefile,status=@status,totalcost=@totalcost where id=@id";

            Parameter param1 = new Parameter("paymentoption", xiCollection["paymentoption"], DbType.Int32);
            Parameter param2 = new Parameter("termscondition", xiCollection["termscondition"]);
            Parameter param3 = new Parameter("referencefile", xiCollection["referencefile"]);
            Parameter param4 = new Parameter("status", xiCollection["status"], DbType.Int32);
            Parameter param5 = new Parameter("totalcost", xiCollection["totalcost"]);
            Parameter param6 = new Parameter("id", xiId, DbType.Int32);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1, param2, param3, param4, param5, param6 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static NameValueCollection GetOrderDetails(object xiId, object xiBUId)
        {
            NameValueCollection collection = null;

            string query = "select o.*,u.fname as cfname,u.lname as clname,u.email as cemail,u.phone as cphone,u.address as caddress from [bu_order] o inner join [bu_customer] c on c.id = o.customerid inner join [user] u on u.id = c.userid where o.id=" + Utils.ConvertToDBString(xiId, Utils.DataType.Integer) + " and o.bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " and o.active = 1";
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
                    collection.Add("orderno", Convert.ToString(objdb.GetValue(reader1, "orderno")));
                    collection.Add("customerid", Convert.ToString(objdb.GetValue(reader1, "customerid")));
                    collection.Add("cfname", Convert.ToString(objdb.GetValue(reader1, "cfname")));
                    collection.Add("clname", Convert.ToString(objdb.GetValue(reader1, "clname")));
                    collection.Add("cemail", Convert.ToString(objdb.GetValue(reader1, "cemail")));
                    collection.Add("cphone", Convert.ToString(objdb.GetValue(reader1, "cphone")));
                    collection.Add("caddress", Convert.ToString(objdb.GetValue(reader1, "caddress")));
                    collection.Add("orderdate", Convert.ToString(objdb.GetValue(reader1, "orderdate")));
                    collection.Add("status", Convert.ToString(objdb.GetValue(reader1, "status")));
                    collection.Add("totalcost", Convert.ToString(objdb.GetValue(reader1, "totalcost")));
                    collection.Add("staffid", Convert.ToString(objdb.GetValue(reader1, "staffid")));
                    collection.Add("companyid", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                    collection.Add("paymentoption", Convert.ToString(objdb.GetValue(reader1, "paymentoption")));
                    collection.Add("termscondition", Convert.ToString(objdb.GetValue(reader1, "termscondition")));
                    collection.Add("referencefile", Convert.ToString(objdb.GetValue(reader1, "referencefile")));

                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetOrderDetails", x.Message);
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

        public static NameValueCollection GetCurrentOrderNo()
        {
            NameValueCollection collection = null;

            string query = "select (isnull(max(o.orderno), 0)+1) as currentorderno from bu_order o where o.active = 1";
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("currentorderno", Convert.ToString(objdb.GetValue(reader1, "currentorderno")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetCurrentOrderNo", x.Message);
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

        public static string SearchOrder(NameValueCollection xiCollection)
        {
            StringBuilder builder = new StringBuilder();
            bool iswhere = false;

            if (xiCollection["orderno"] != null && xiCollection["orderno"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(o.orderno={0})", Utils.ConvertToDBString(xiCollection["orderno"], Utils.DataType.Integer)));
            }

            if (xiCollection["companyid"] != null && xiCollection["companyid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(o.bu_id={0})", Utils.ConvertToDBString(xiCollection["companyid"], Utils.DataType.Integer)));
            }

            if (xiCollection["customerid"] != null && xiCollection["customerid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(c.id={0})", Utils.ConvertToDBString(xiCollection["customerid"], Utils.DataType.Integer)));
            }

            if (xiCollection["ispos"] != null && xiCollection["ispos"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;

                switch (xiCollection["ispos"])
                {
                    case "0":
                        builder.Append("(c.ispos IS NULL OR c.ispos = '' OR c.ispos != 1)");
                        break;

                    case "1":
                        builder.Append("(c.ispos = 1)");
                        break;
                }
                //builder.Append(string.Format("(c.ispos={0})", Utils.ConvertToDBString(xiCollection["ispos"], Utils.DataType.Integer)));
            }           

            if (xiCollection["cid"] != null && xiCollection["cid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(o.customerid={0})", Utils.ConvertToDBString(xiCollection["cid"], Utils.DataType.Integer)));
            }

            if (xiCollection["staffid"] != null && xiCollection["staffid"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(o.staffid={0})", Utils.ConvertToDBString(xiCollection["staffid"], Utils.DataType.Integer)));
            }

            if (xiCollection["status"] != null && xiCollection["status"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                builder.Append(string.Format("(o.status={0})", Utils.ConvertToDBString(xiCollection["status"], Utils.DataType.Integer)));
            }

            if (xiCollection["currentdate"] != null && xiCollection["currentdate"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                try
                {
                    Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();

                    DateTime currentdate = Convert.ToDateTime(xiCollection["currentdate"], CultureInfo.CurrentCulture);
                    string currentDate = Utils.ConvertToDBString(currentdate.ToString(BusinessBase.DateFormat), Utils.DataType.Date);

                    builder.Append(string.Format("o.orderdate={0}", currentDate));
                }
                catch { }
            }

            if (xiCollection["startdate"] != null && xiCollection["startdate"].Length > 0 && xiCollection["enddate"] != null && xiCollection["enddate"].Length > 0)
            {
                if (iswhere) builder.Append(" and ");
                iswhere = true;
                try
                {
                    Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();
                    DateTime startDate = Convert.ToDateTime(xiCollection["startdate"], CultureInfo.CurrentCulture);
                    DateTime endDate = Convert.ToDateTime(xiCollection["enddate"], CultureInfo.CurrentCulture);

                    if (DateTime.Compare(startDate, endDate) > 0)
                    {
                        xiCollection["enddate"] = xiCollection["startdate"];
                    }
                    string startdate = Utils.ConvertToDBString(startDate.ToString(BusinessBase.DateFormat), Utils.DataType.Date);
                    string enddate = Utils.ConvertToDBString(endDate.ToString(BusinessBase.DateFormat), Utils.DataType.Date);
                    builder.Append(string.Format("(o.orderdate between {0} and {1})", startdate, enddate));
                }
                catch { }
            }

            return builder.ToString();
        }

        public static int GetBUProcessingOrderCount(object xiBUId)
        {
            string query = "select count(o.id) as maxcount from [bu_order] o inner join bu_customer c on c.id= o.customerid and c.active=1 where o.active=1 and o.status=1 and c.bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer);

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            object obj = objdb.ExecuteScalar(objdb.con, query);
            objdb.Disconnectdb();

            int count = BusinessBase.ConvertToInteger(obj);
            if (count < 0) count = 0;

            return count;
        }

        public static int GetProcessingOrderCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(o.id) as maxcount from [bu_order] o inner join bu_customer c on c.id= o.customerid where o.active = 1 and o.status = 1";
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
                objdb.Write_log_file("GetProcessingOrderCount", x.Message);
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

        public static DataSet GetProcessingOrderDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select o.*,u.fname as fname,u.lname as lname from [bu_order] o inner join bu_customer c on c.id= o.customerid inner join [user] u on u.id = c.userid where o.active = 1 and o.status=1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by o.orderno desc offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("processed_date", typeof(string));
                ds.Tables[0].Columns.Add("processed_totalcost", typeof(string));                

                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    try
                    {
                        DateTime tempDate = Convert.ToDateTime(dtRow["orderdate"]);
                        dtRow["processed_date"] = (tempDate != DateTime.MinValue) ? tempDate.ToString(BusinessBase.DateFormat) : "";
                    }
                    catch { }
                    
                    string tempCost = Convert.ToDecimal(dtRow["totalcost"]).ToString("0.00").Replace(",", ".");
                    if (string.IsNullOrEmpty(tempCost))
                    {
                        tempCost = "0.00";
                    }
                    dtRow["processed_totalcost"] = tempCost;
                }
            }
            return ds;
        }

        public static bool DeleteOrder(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update bu_order set status = 3 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        public static int GetTodayClosedOrderCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(o.id) as maxcount from [bu_order] o inner join bu_customer c on c.id= o.customerid inner join [user] u on u.id = c.userid where o.active = 1 and o.status = 2";
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
                objdb.Write_log_file("GetTodayClosedOrderCount", x.Message);
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

        public static DataSet GetTodayClosedOrderDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select o.*,u.fname as fname,u.lname as lname from [bu_order] o inner join bu_customer c on c.id= o.customerid inner join [user] u on u.id = c.userid where o.active = 1 and o.status=2";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by o.orderno desc offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("processed_date", typeof(string));
                ds.Tables[0].Columns.Add("processed_totalcost", typeof(string));

                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    try
                    {
                        DateTime tempDate = Convert.ToDateTime(dtRow["orderdate"]);
                        dtRow["processed_date"] = (tempDate != DateTime.MinValue) ? tempDate.ToString(BusinessBase.DateFormat) : "";
                    }
                    catch { }

                    string tempCost = Convert.ToDecimal(dtRow["totalcost"]).ToString("0.00").Replace(",", ".");
                    if (string.IsNullOrEmpty(tempCost))
                    {
                        tempCost = "0.00";
                    }
                    dtRow["processed_totalcost"] = tempCost;
                }
            }

            return ds;
        }

        public static int GetTodayOrderCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(o.id) as maxcount from [bu_order] o inner join bu_customer c on c.id= o.customerid inner join [user] u on u.id = c.userid where o.active = 1";
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
                objdb.Write_log_file("GetTodayOrderCount", x.Message);
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

        public static DataSet GetTodayOrderDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select o.*,u.fname as fname,u.lname as lname from [bu_order] o inner join bu_customer c on c.id= o.customerid inner join [user] u on u.id = c.userid where o.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by o.orderno desc offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("processed_date", typeof(string));
                ds.Tables[0].Columns.Add("processed_totalcost", typeof(string));

                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    try
                    {
                        DateTime tempDate = Convert.ToDateTime(dtRow["orderdate"]);
                        dtRow["processed_date"] = (tempDate != DateTime.MinValue) ? tempDate.ToString(BusinessBase.DateFormat) : "";

                    }
                    catch { }

                    string tempCost = Convert.ToDecimal(dtRow["totalcost"]).ToString("0.00").Replace(",", ".");
                    if (string.IsNullOrEmpty(tempCost))
                    {
                        tempCost = "0.00";
                    }
                    dtRow["processed_totalcost"] = tempCost;
                }
            }

            return ds;
        }

        public static int GetClosedOrderCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(o.id) as maxcount from [bu_order] o inner join bu_customer c on c.id= o.customerid inner join [user] u on u.id = c.userid where o.active = 1 and o.status >= 2";
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
                objdb.Write_log_file("GetClosedOrderCount", x.Message);
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

        public static DataSet GetClosedOrderDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select o.*,u.fname as fname,u.lname as lname from [bu_order] o inner join bu_customer c on c.id= o.customerid inner join [user] u on u.id = c.userid where o.active = 1 and o.status >= 2";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by o.orderno desc offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("processed_date", typeof(string));
                ds.Tables[0].Columns.Add("processed_totalcost", typeof(string));

                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    try
                    {
                        DateTime tempDate = Convert.ToDateTime(dtRow["orderdate"]);
                        dtRow["processed_date"] = (tempDate != DateTime.MinValue) ? tempDate.ToString(BusinessBase.DateFormat) : "";
                    }
                    catch { }

                    string tempCost = Convert.ToDecimal(dtRow["totalcost"]).ToString("0.00").Replace(",", ".");
                    if (string.IsNullOrEmpty(tempCost))
                    {
                        tempCost = "0.00";
                    }
                    dtRow["processed_totalcost"] = tempCost;
                }
            }

            return ds;
        }

        #endregion

        #region Order Item

        public static int AddBUOrderItem(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "INSERT INTO [bu_order_item]([orderid],[productid],[serviceid],[comboid],[quan],[itemcost],[taxid],[taxpercentage],[totalcost],[staffid],[itemid]) values(@orderid,@productid,@serviceid,@comboid,@quan,@itemcost,@taxid,@taxpercentage,@totalcost,@staffid,@itemid)";

            Parameter param1 = new Parameter("orderid", xiCollection["orderid"], DbType.Int32);
            Parameter param2 = new Parameter("productid", xiCollection["productid"], DbType.Int32);
            Parameter param3 = new Parameter("serviceid", xiCollection["serviceid"], DbType.Int32);
            Parameter param4 = new Parameter("comboid", xiCollection["comboid"], DbType.Int32);
            Parameter param5 = new Parameter("quan", xiCollection["quan"], DbType.Int32);
            Parameter param6 = new Parameter("itemcost", xiCollection["itemcost"], DbType.Decimal);
            Parameter param7 = new Parameter("taxid", xiCollection["taxid"], DbType.Int32);
            Parameter param8 = new Parameter("taxpercentage", xiCollection["taxpercentage"], DbType.Decimal);
            Parameter param9 = new Parameter("totalcost", xiCollection["totalcost"], DbType.Decimal);
            Parameter param10 = new Parameter("staffid", xiCollection["staffid"], DbType.Int32);
            Parameter param11 = new Parameter("itemid", xiCollection["itemid"], DbType.Int32);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11 });
            objdb.Disconnectdb();

            return value;
        }

        public static DataSet GetBUOrderItems(string xiOrderId, object xiCompanyId)
        {

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select oi.*,p.[name] as productname,t.[name] as taxname,t.[percentage] as tpercentage from [bu_order_item] oi inner join bu_order o on o.id= oi.orderid inner join bu_product p on p.id= oi.productid inner join bu_tax t on t.id= oi.taxid where oi.active = 1 and oi.[orderid] ={0} and o.[bu_id]={1} order by oi.id";
            query = string.Format(query, Utils.ConvertToDBString(xiOrderId, Utils.DataType.Integer), Utils.ConvertToDBString(xiCompanyId, Utils.DataType.Integer));
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            return ds;

        }

        public static DataTable GetOrderItems(object xiOrderId, object xiCompanyId)
        {
            string query = "select oi.*,p.[name] as productname,t.[name] as taxname,t.[percentage] as tpercentage from [bu_order_item] oi inner join bu_order o on o.id = oi.orderid inner join bu_product p on p.id = oi.productid inner join bu_tax t on t.id = oi.taxid where oi.active = 1 and oi.[orderid] ={0} and o.[bu_id]={1} order by oi.id";
            query = string.Format(query, Utils.ConvertToDBString(xiOrderId, Utils.DataType.Integer), Utils.ConvertToDBString(xiCompanyId, Utils.DataType.Integer));
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();
            return dataTable;

        }

        public static DataTable GetOrderItems(object xiOrderId)
        {
            string query = @"select oi.id,oi.itemid,oi.quan,oi.itemcost,oi.totalcost,oi.staffid,oi.taxid,oi.taxpercentage,p.[name] as productname,p.taxname,p.taxpercentage as tpercentage, (oi.totalcost*cast(oi.quan as float)) as itemtotalcost,
case when itemtype = 'P' then  [name] +' (Product)'
 when itemtype = 'S' then  [name] +' (Service)'
 when itemtype = 'C' then  [name] +' (Combo)' End as 'ItemNameType'
from [bu_order_item] oi 
inner join view_bu_all_items p on p.id2=oi.itemid and p.active=1
inner join bu_order o on o.id = oi.orderid and o.active=1
where oi.[orderid] = " + Utils.ConvertToDBString(xiOrderId, Utils.DataType.Integer) + " and oi.active=1 order by p.[name]";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();
            return dataTable;

        }

        public static bool DeleteBUOrderItem(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update bu_order_item set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        // set quan=0 for delete item
        public static bool UpdateBUOrderItemQuan(object xiId)
        {
            if (xiId == null) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update bu_order_item set quan = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);
        }

        #endregion

        #region Order Head

        public static int AddBUOrderHead(NameValueCollection xiCollection)
        {
            if (xiCollection == null) return int.MinValue;

            string query = "INSERT INTO [bu_order_head]([orderid],[orderhead],[cost],[isnegative],[staffid]) values(@orderid,@orderhead,@cost,@isnegative,@staffid)";

            Parameter param1 = new Parameter("orderid", xiCollection["orderid"], DbType.Int32);
            Parameter param2 = new Parameter("orderhead", xiCollection["orderhead"]);
            Parameter param3 = new Parameter("cost", xiCollection["headcost"]);
            Parameter param4 = new Parameter("isnegative", xiCollection["isnegative"], DbType.Int32);
            Parameter param5 = new Parameter("staffid", xiCollection["staffid"], DbType.Int32);

            string query2 = "select scope_identity()";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            int value = objdb.ExecuteNonQueryScopeIdentity(objdb.con, query, query2, new Parameter[] { param1, param2, param3, param4, param5 });
            objdb.Disconnectdb();

            return value;
        }

        public static DataTable GetHeadDetails(object xiOrderId)
        {
            string query = @"select oh.* 
from [bu_order_head] oh 
inner join [bu_order] o on o.id = oh.orderid
where oh.orderid =" + Utils.ConvertToDBString(xiOrderId, Utils.DataType.Integer) + " and oh.active = 1 order by oh.id";
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static bool DeleteOrderHead(int xiHeadId)
        {
            if (xiHeadId <= 0) return false;

            DBClass objdb = new DBClass();
            string query = string.Format("update bu_order_head set active = 0 where id= @id");
            Parameter param1 = new Parameter("id", xiHeadId, DbType.Int32);
            objdb.Connectdb();
            int value = objdb.ExecuteNonQuery(objdb.con, query, new Parameter[] { param1 });
            objdb.Disconnectdb();

            return (value > 0);

        }

        public static NameValueCollection GetCurrentOrderCost(object xiOrderId)
        {
            NameValueCollection collection = null;

            string query = @"select ISNULL(sum(cast(oi.quan as float) * oi.itemcost),0) as total_item_cost,ISNULL(sum(cast(oi.quan as float) * oi.itemcost * cast(oi.taxpercentage as float) / 100.00),0) as total_tax_amount,
ISNULL(sum((cast(oi.quan as float) * oi.itemcost) + (cast(oi.quan as float) * oi.itemcost * cast(oi.taxpercentage as float) / 100.00)),0) as total_cost_with_tax 
from bu_order_item oi inner join bu_order o on o.id=oi.orderid where oi.active = 1 
and oi.orderid= " + Utils.ConvertToDBString(xiOrderId, Utils.DataType.Integer) + " and oi.active=1";
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("orderid", xiOrderId.ToString());
                    collection.Add("total_item_cost", Convert.ToString(objdb.GetValue(reader1, "total_item_cost")));
                    collection.Add("total_tax_amount", Convert.ToString(objdb.GetValue(reader1, "total_tax_amount")));
                    collection.Add("total_cost_with_tax", Convert.ToString(objdb.GetValue(reader1, "total_cost_with_tax")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetCurrentOrderCost", x.Message);
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

        #endregion       

        public static DataTable GetAllBUCustomer(object xiCompanyId)
        {

            string query = @"select c.id,u.fname + ' ' + u.lname as fullname
from bu_customer c 
inner join [user] u on u.id = c.userid and u.active=1
left join [countries] cc on cc.id = u.countryid
where c.bu_id=" + Utils.ConvertToDBString(xiCompanyId, Utils.DataType.Integer) + " and c.active=1 and c.ispos IS NULL order by u.fname,u.lname";            

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetPOSCustomer(object xiCompanyId)
        {
            string query = @"select c.id,u.fname + ' ' + u.lname as fullname
from bu_customer c 
inner join [user] u on u.id = c.userid and u.active=1
left join [countries] cc on cc.id = u.countryid
where c.bu_id=" + Utils.ConvertToDBString(xiCompanyId, Utils.DataType.Integer) + " and c.active=1 and c.ispos=1 order by u.fname,u.lname";
           
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static NameValueCollection GetPOSCustomerDetail(object xiBUId)
        {
            NameValueCollection collection = null;

            string query = "select c.*,u.fname as fname,u.lname as lname,u.email as email,u.phone as phone,u.address as address,u.city as city,u.pincode as pincode,u.countryid as countryid,u.contactcountrycode as ucontactcountrycode,cc.fullname as countryname,ccc1.countrycode as 'userphoneprefix' from bu_customer c inner join [user] u on u.id = c.userid inner join [contact_countrycode] ccc1 on ccc1.id = u.contactcountrycode left join [countries] cc on cc.id = u.countryid where c.bu_id=" + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + " and c.active = 1 and c.ispos=1";
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
                objdb.Write_log_file("GetPOSCustomerDetail", x.Message);
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

        public static DataTable GetAllBUItems(object xiCompanyId, string xiName)
        {
            string query = @"SELECT id,[name], cost, taxname, taxpercentage,taxid,'Product' AS Type FROM (SELECT p.[id],p.[name],p.[cost],t.[name] AS taxname,t.[percentage] AS taxpercentage,p.taxid, 'Product' AS Type FROM bu_product p inner JOIN bu_tax t ON t.id = p.taxid WHERE  p.[active]= 1 and p.[bu_id] = " + Utils.ConvertToDBString(xiCompanyId, Utils.DataType.Integer) + @"        
        UNION         
        SELECT s.[id],s.[name],s.[cost],t.[name] AS taxname,t.[percentage] AS taxpercentage,s.taxid, 'Service' AS Type FROM bu_services s inner JOIN bu_tax t ON t.id = s.taxid WHERE s.[active]= 1 and s.[bu_id] = " + Utils.ConvertToDBString(xiCompanyId, Utils.DataType.Integer) + @"        
        UNION         
        SELECT cd.[id],cd.[title] AS name,cd.[cost],t.[name] AS taxname,t.[percentage] AS taxpercentage,cd.taxid, 'Combo' AS Type FROM bu_combodetails cd inner JOIN bu_tax t ON t.id = cd.taxid WHERE cd.[active]= 1 and cd.[bu_id] = " + Utils.ConvertToDBString(xiCompanyId, Utils.DataType.Integer) + @"
    ) AS combined WHERE 1=1";
            if (!string.IsNullOrEmpty(xiName)) query += " AND name LIKE " + Utils.ConvertToDBString("%" + xiName + "%", Utils.DataType.String);
            query += " ORDER BY name";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }       

        public static DataTable GetAllBUItem(object xiCompanyId)
        {
            string query = @"select id2,id,[name],
case when itemtype = 'P' then  [name] +' (Product)'
 when itemtype = 'S' then  [name] +' (Service)'
 when itemtype = 'C' then  [name] +' (Combo)' End as 'ItemNameType' from view_bu_all_items where active=1 and bu_id= " + Utils.ConvertToDBString(xiCompanyId, Utils.DataType.Integer) + " ORDER BY [name]";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static NameValueCollection GetBUItem(object xiItemId2)
        {
            NameValueCollection collection = null;

            string query = "select * from view_bu_all_items where active=1 and id2 = " + Utils.ConvertToDBString(xiItemId2, Utils.DataType.Integer);
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id2", Convert.ToString(objdb.GetValue(reader1, "id2")));
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("bu_id", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                    collection.Add("cost", Convert.ToString(objdb.GetValue(reader1, "cost")));
                    collection.Add("taxid", Convert.ToString(objdb.GetValue(reader1, "taxid")));
                    collection.Add("taxname", Convert.ToString(objdb.GetValue(reader1, "taxname")));
                    collection.Add("taxpercentage", Convert.ToString(objdb.GetValue(reader1, "taxpercentage")));
                    collection.Add("taxamount", Convert.ToString(objdb.GetValue(reader1, "taxamount")));
                    collection.Add("itemtype", Convert.ToString(objdb.GetValue(reader1, "itemtype")));
                    collection.Add("availablestock", Convert.ToString(objdb.GetValue(reader1, "availablestock")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetBUItem", x.Message);
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

        public static NameValueCollection GetBUItem(object xiItemId, string xiItemType)
        {
            NameValueCollection collection = null;

            string query = "select * from view_bu_all_items where active=1 and id = " + Utils.ConvertToDBString(xiItemId, Utils.DataType.Integer) + " and itemtype = " + Utils.ConvertToDBString(xiItemType, Utils.DataType.String);
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read())
                {
                    collection = new NameValueCollection();
                    collection.Add("id2", Convert.ToString(objdb.GetValue(reader1, "id2")));
                    collection.Add("id", Convert.ToString(objdb.GetValue(reader1, "id")));
                    collection.Add("name", Convert.ToString(objdb.GetValue(reader1, "name")));
                    collection.Add("bu_id", Convert.ToString(objdb.GetValue(reader1, "bu_id")));
                    collection.Add("cost", Convert.ToString(objdb.GetValue(reader1, "cost")));
                    collection.Add("taxid", Convert.ToString(objdb.GetValue(reader1, "taxid")));
                    collection.Add("taxname", Convert.ToString(objdb.GetValue(reader1, "taxname")));
                    collection.Add("taxpercentage", Convert.ToString(objdb.GetValue(reader1, "taxpercentage")));
                    collection.Add("taxamount", Convert.ToString(objdb.GetValue(reader1, "taxamount")));
                    collection.Add("itemtype", Convert.ToString(objdb.GetValue(reader1, "itemtype")));
                }
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetBUItem-2", x.Message);
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

        public static DataSet GetOrderByStatus(object xiCompanyId)
        {
            string query = @"select count(o.id) as ordercount, CASE WHEN o.[status] = 1 THEN 'Processing' WHEN o.[status] = 2 THEN 'Completed' WHEN o.[status] = 3 THEN 'Deleted' ELSE 'Unknown' END AS status_name from [bu_order] o where o.active = 1 and o.[bu_id]={0} group by o.status";

            query = string.Format(query, Utils.ConvertToDBString(xiCompanyId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static DataSet GetOrderByType(object xiCompanyId)
        {
            string query = @"SELECT sum(oi.quan) AS item_count, CASE WHEN oi.productid IS NOT NULL THEN 'Product' WHEN oi.serviceid IS NOT NULL THEN 'Service' WHEN oi.comboid IS NOT NULL THEN 'Combo' END AS item_type
FROM [bu_order_item] oi
inner join [bu_order] o on o.id=oi.orderid and o.active=1
WHERE oi.active = 1 and o.[bu_id]={0} 
GROUP BY CASE WHEN oi.productid IS NOT NULL THEN 'Product' WHEN oi.serviceid IS NOT NULL THEN 'Service' WHEN oi.comboid IS NOT NULL THEN 'Combo' END";
            query = string.Format(query, Utils.ConvertToDBString(xiCompanyId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static DataSet GetItemCountData(object xiCompanyId)
        {
            string query = @"SELECT sum(oi.quan) AS item_count, (case when itemtype = 'P' then  [name] +' (Product)'
 when itemtype = 'S' then  [name] +' (Service)'
 when itemtype = 'C' then  [name] +' (Combo)' End) as productname
FROM [bu_order_item] oi inner join [bu_order] o on o.id=oi.orderid and o.active=1
inner join view_bu_all_items p on p.id2=oi.itemid
WHERE oi.active = 1 and o.[bu_id]={0}
group by (case when itemtype = 'P' then  [name] +' (Product)'
 when itemtype = 'S' then  [name] +' (Service)'
 when itemtype = 'C' then  [name] +' (Combo)' End) order by sum(oi.quan) desc";

            query = string.Format(query, Utils.ConvertToDBString(xiCompanyId, Utils.DataType.Integer));

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            return ds;
        }

        public static int GetCustomerOrderCount(object xiUserId, string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = @"select count(o.id) as maxcount from bu_order o inner join bu_businessuser bu on bu.id=o.bu_id and bu.active=1 where o.active=1 
and o.customerid in (select id from bu_customer where userid=" + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + " and active=1)";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            try
            {
                reader1 = objdb.ExecuteReader(objdb.con, query);
                if (reader1.Read()) totalCount = Convert.ToInt32(objdb.GetValue(reader1, "maxcount"));
            }
            catch (Exception x)
            {
                objdb.Write_log_file("GetProcessingOrderCount", x.Message);
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

        public static DataSet GetCustomerOrderDetails(int xiPage, object xiUserId, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = @"select o.id,o.bu_id,o.orderno,o.orderdate,o.status,o.totalcost,bu.companyname,(select name from bu_currency where id=bu.id) as currencyname 
from bu_order o inner join bu_businessuser bu on bu.id=o.bu_id and bu.active=1 where o.active=1 
and o.customerid in (select id from bu_customer where userid=" + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + " and active=1)";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by o.orderdate desc offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("processed_date", typeof(string));
                ds.Tables[0].Columns.Add("processed_totalcost", typeof(string));

                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    try
                    {
                        DateTime tempDate = Convert.ToDateTime(dtRow["orderdate"]);
                        dtRow["processed_date"] = (tempDate != DateTime.MinValue) ? tempDate.ToString(BusinessBase.DateFormat) : "";
                    }
                    catch { }

                    string tempCost = Convert.ToDecimal(dtRow["totalcost"]).ToString("0.00").Replace(",", ".");
                    if (string.IsNullOrEmpty(tempCost))
                    {
                        tempCost = "0.00";
                    }
                    dtRow["processed_totalcost"] = tempCost;
                }
            }
            return ds;
        }

        public static DataTable GetAllBUForCustomer(object xiUserId)
        {
            string query = @"select id,companyname from bu_businessuser bu 
where bu.id in (select bu_id from bu_order where customerid in (select id from bu_customer where userid=" + Utils.ConvertToDBString(xiUserId, Utils.DataType.Integer) + @" and active=1) and active=1)
and active=1 order by companyname";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        #region Reports       
        public static int GetCustomerReportCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "SELECT COUNT(DISTINCT o.customerid) AS maxcount FROM [bu_order] o inner JOIN [bu_customer] c ON c.id = o.customerid AND c.active = 1 inner JOIN [user] u ON u.id = c.userid and u.active=1 WHERE o.active = 1";
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
                objdb.Write_log_file("GetCustomerReportCount", x.Message);
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

        public static DataSet GetCustomerReportDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage == 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            string query = @"SELECT c.id AS customer_id,c.userid AS [user_id], u.fname AS first_name, u.lname AS last_name, u.email AS user_email, u.phone AS user_phone,SUM(o.totalcost) AS total_order_cost 
FROM [bu_order] o 
inner JOIN [bu_customer] c ON c.id = o.customerid AND c.active = 1 
inner JOIN [user] u ON u.id = c.userid and u.active=1
WHERE o.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " GROUP BY c.id, c.userid, u.fname, u.lname, u.email, u.phone";
            query += " order by total_order_cost desc offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            if (ds != null && ds.Tables.Count > 0)
            {               
                ds.Tables[0].Columns.Add("processed_totalordercost", typeof(string));

                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {                   
                    string tempCost = Convert.ToDecimal(dtRow["total_order_cost"]).ToString("0.00").Replace(",", ".");
                    if (string.IsNullOrEmpty(tempCost))
                    {
                        tempCost = "0.00";
                    }
                    dtRow["processed_totalordercost"] = tempCost;
                }
            }

            return ds;
        }

        public static int GetStaffReportCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(distinct s.id) as maxcount FROM [bu_staff] s INNER JOIN [user] u ON u.id = s.userid  and u.active=1 WHERE s.active = 1 ";
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
                objdb.Write_log_file("GetStaffReportCount", x.Message);
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

        public static DataSet GetStaffReportDetails(int xiPage, string xiFilter)
        {
            xiPage = (xiPage == 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            string query = @"SELECT s.id AS staff_id,s.jobtitle,s.employmentstatus,s.userid AS [user_id], u.fname AS first_name, u.lname AS last_name, u.email AS user_email,
(select ISNULL(sum(totalcost),0.0) from [bu_order] where [bu_order].staffid=s.userid and [bu_order].bu_id=s.bu_id) as total_order_cost
FROM  [bu_staff] s inner JOIN [user] u ON u.id = s.userid and u.active = 1 WHERE s.active = 1 ";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by u.fname offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("processed_totalordercost", typeof(string));

                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    string tempCost = Convert.ToDecimal(dtRow["total_order_cost"]).ToString("0.00").Replace(",", ".");
                    if (string.IsNullOrEmpty(tempCost))
                    {
                        tempCost = "0.00";
                    }
                    dtRow["processed_totalordercost"] = tempCost;
                }
            }

            return ds;
        }

        //new 06 march 2025
        public static int GetCustReportCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(o.id) as maxcount from [bu_order] o inner join bu_customer c on c.id= o.customerid and c.active=1 inner join [user] u on u.id = c.userid and u.active=1 where o.active = 1";
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
                objdb.Write_log_file("GetCustReportCount", x.Message);
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

        //new 06 march 2025
        public static DataSet GetCustReportList(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select o.id,o.totalcost,o.orderno,o.[status],o.orderdate from [bu_order] o inner join bu_customer c on c.id= o.customerid and c.active=1 inner join [user] u on u.id = c.userid and u.active=1 where o.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by o.orderno offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("processed_date", typeof(string));
                ds.Tables[0].Columns.Add("processed_totalcost", typeof(string));    
               
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    try
                    {
                        DateTime tempDate = Convert.ToDateTime(dtRow["orderdate"]);
                        dtRow["processed_date"] = (tempDate != DateTime.MinValue) ? tempDate.ToString(BusinessBase.DateFormat) : "";

                    }
                    catch { }

                    string tempCost = Convert.ToDecimal(dtRow["totalcost"]).ToString("0.00").Replace(",", ".");
                    if (string.IsNullOrEmpty(tempCost))
                    {
                        tempCost = "0.00";
                    }
                    dtRow["processed_totalcost"] = tempCost;
                }
            }

            return ds;
        }

        //new 06 march 2025
        public static int GetStaffReportsCount(string xiFilter)
        {
            int totalCount = 0;
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            SqlDataReader reader1 = null;

            string query = "select count(distinct o.id) as maxcount from [bu_order] o inner join bu_staff s on s.userid= o.staffid and s.active=1 inner join [user] u on u.id = s.userid and u.active=1 where o.active = 1";
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
                objdb.Write_log_file("GetStaffReportsCount", x.Message);
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

        //new 06 march 2025
        public static DataSet GetStaffReportsList(int xiPage, string xiFilter)
        {
            xiPage = (xiPage <= 0) ? 1 : xiPage;
            xiPage = xiPage - 1;

            DBClass objdb = new DBClass();
            objdb.Connectdb();

            string query = "select distinct o.id,o.totalcost,o.orderno,o.[status],o.orderdate from [bu_order] o inner join bu_staff s on s.userid= o.staffid and s.active=1 inner join [user] u on u.id = s.userid and u.active=1 where o.active = 1";
            if (string.IsNullOrEmpty(xiFilter) == false) query += " and " + xiFilter;
            query += " order by o.orderno offset " + (xiPage * Common.RECORDCOUNT) + " rows fetch next " + Common.RECORDCOUNT + " rows only";
            DataSet ds = objdb.ExecuteDataSet(objdb.con, query);
            objdb.Disconnectdb();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("processed_date", typeof(string));
                ds.Tables[0].Columns.Add("processed_totalcost", typeof(string));

                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    try
                    {
                        DateTime tempDate = Convert.ToDateTime(dtRow["orderdate"]);
                        dtRow["processed_date"] = (tempDate != DateTime.MinValue) ? tempDate.ToString(BusinessBase.DateFormat) : "";

                    }
                    catch { }

                    string tempCost = Convert.ToDecimal(dtRow["totalcost"]).ToString("0.00").Replace(",", ".");
                    if (string.IsNullOrEmpty(tempCost))
                    {
                        tempCost = "0.00";
                    }
                    dtRow["processed_totalcost"] = tempCost;
                }
            }

            return ds;
        }

        #endregion
    }
}
